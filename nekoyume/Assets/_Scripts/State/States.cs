using System.Collections.Generic;
using System.Linq;
using Gateway.Protocol;
using Nekoyume.Model.State;
using Debug = UnityEngine.Debug;

namespace Nekoyume.State
{
    /// <summary>
    /// 클라이언트가 참조할 상태를 포함한다.
    /// 체인의 상태를 Setter를 통해서 받은 후, 로컬의 상태로 필터링해서 사용한다.
    /// </summary>
    public class States
    {
        public static States Instance => Game.Game.instance.States;
        public int CurrentAvatarKey { get; private set; }
        public AvatarState CurrentAvatarState => _avatarStateDict[CurrentAvatarKey];

        private Dictionary<int, AvatarState> _avatarStateDict;

        public WeeklyArenaState WeeklyArenaState { get; private set; }
        public GameConfigState GameConfigState { get; private set; } = new GameConfigState();

        private readonly Dictionary<int, CombinationSlotState> _combinationSlotStates =
            new Dictionary<int, CombinationSlotState>();

        #region Setter
        public void SetRankingMapStates(RankingMapState state)
        {
            if (state is null)
            {
                Debug.LogWarning($"[{nameof(States)}.{nameof(SetRankingMapStates)}] {nameof(state)} is null.");
                return;
            }
        }

        public void SetWeeklyArenaState(WeeklyArenaState state)
        {
            if (state is null)
            {
                Debug.LogWarning($"[{nameof(States)}.{nameof(SetWeeklyArenaState)}] {nameof(state)} is null.");
                return;
            }
        }

        public void SetAvatarInfo(ST_AvatarInfo avatarInfo)
        {
            CurrentAvatarKey = avatarInfo.SelectedAvatarIndex;
            _avatarStateDict = avatarInfo.AvatarDict
                .ToDictionary(entry => entry.Key, entry => new AvatarState(entry.Value));
            SelectAvatar(CurrentAvatarKey);
        }

        public bool HasAvatarState(int index)
        {
            return _avatarStateDict.ContainsKey(index);
        }

        public bool TryGetAvatarState(int index, out AvatarState state)
        {
            return _avatarStateDict.TryGetValue(index, out state);
        }

        public AvatarState SelectAvatar(int index, bool initializeReactiveState = true)
        {
            if (!HasAvatarState(index))
            {
                throw new KeyNotFoundException($"{nameof(index)}({index})");
            }

            CurrentAvatarKey = index;
            var tableSheets = Game.Game.instance.TableSheets;
            CurrentAvatarState.SetInventory(new ST_Inventory
                {
                    ConsumableDict = new Dictionary<int, ST_Consumable>(),
                    CostumeDict = new Dictionary<int, ST_Costume>(),
                    EquipmentDict = new Dictionary<int, ST_Equipment>(),
                    MaterialDict = new Dictionary<int, ST_Material>()
                },
                tableSheets.ConsumableItemSheet,
                tableSheets.CostumeItemSheet,
                tableSheets.EquipmentItemSheet,
                tableSheets.MaterialItemSheet
            );
            CurrentAvatarState.SetWorldAndStage(new ST_WorldInfo
                {
                    LastWorldId = 1,
                    LastStageId = 1,
                    WorldDict = new Dictionary<int, ST_World>
                    {

                    }
                },
                tableSheets.WorldSheet
            );
            CurrentAvatarState.SetQuestList(tableSheets.GetAvatarSheets());
            CurrentAvatarState.SetMailBox();

            if (initializeReactiveState)
            {
                ReactiveAvatarState.Initialize(CurrentAvatarState);
            }
            return CurrentAvatarState;
        }

        private void SetCombinationSlotStates(AvatarState avatarState)
        {
            
        }

        public void UpdateCombinationSlotState(int index, CombinationSlotState state)
        {
            if (_combinationSlotStates.ContainsKey(index))
            {
                _combinationSlotStates[index] = state;
            }
            else
            {
                _combinationSlotStates.Add(index, state);
            }
        }

        public Dictionary<int, CombinationSlotState> GetCombinationSlotState(long currentBlockIndex)
        {
            if (_combinationSlotStates == null)
            {
                return new Dictionary<int, CombinationSlotState>();
            }

            return _combinationSlotStates
                .Where(pair => !pair.Value.Validate(CurrentAvatarState, currentBlockIndex))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        #endregion
    }
}
