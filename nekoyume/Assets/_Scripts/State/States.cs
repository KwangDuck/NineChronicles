using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
        
        public AgentState AgentState { get; private set; }
        public GoldBalanceState GoldBalanceState { get; private set; }
        private ST_AvatarInfo _avatarInfo { get; set; }
        public int CurrentAvatarKey => _avatarInfo.SelectedAvatarIndex;
        public AvatarState CurrentAvatarState { get; private set; }
        public WeeklyArenaState WeeklyArenaState { get; private set; }
        public GameConfigState GameConfigState { get; private set; } = new GameConfigState();

        private readonly Dictionary<int, CombinationSlotState> _combinationSlotStates =
            new Dictionary<int, CombinationSlotState>();

        #region Setter

        /// <summary>
        /// 랭킹 상태를 할당한다.
        /// </summary>
        /// <param name="state"></param>
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

        /// <summary>
        /// 에이전트 상태를 할당한다.
        /// 로컬 세팅을 거친 상태가 최종적으로 할당된다.
        /// 최초로 할당하거나 기존과 다른 주소의 에이전트를 할당하면, 모든 아바타 상태를 새롭게 할당된다.
        /// </summary>
        /// <param name="state"></param>
        public async UniTask SetAgentStateAsync(AgentState state)
        {
            if (state is null)
            {
                Debug.LogWarning($"[{nameof(States)}.{nameof(SetAgentStateAsync)}] {nameof(state)} is null.");
                return;
            }

        }

        public void SetGoldBalanceState(GoldBalanceState goldBalanceState)
        {
            if (goldBalanceState is null)
            {
                Debug.LogWarning($"[{nameof(States)}.{nameof(SetGoldBalanceState)}] {nameof(goldBalanceState)} is null.");
                return;
            }
        }

        public void InitAvatarInfo(ST_AvatarInfo avatarInfo)
        {
            _avatarInfo = avatarInfo;
            var selectedAvatar = _avatarInfo.GetSelectedAvatar();
            if (selectedAvatar != null)
            {
                CurrentAvatarState = new AvatarState(selectedAvatar, Game.TableSheets.Instance.GetAvatarSheets());
            }
        }

        public bool HasAvatarState(int index)
        {
            return _avatarInfo.HasAvatar(index);
        }

        public bool TryGetAvatarState(int index, out AvatarState state)
        {
            var exists = _avatarInfo.HasAvatar(index);
            state = null;
            if (exists)
            {
                state = new AvatarState(_avatarInfo.GetAvatar(index), Game.TableSheets.Instance.GetAvatarSheets());
            }
            return exists;
        }

        public AvatarState SelectAvatar(int index, bool initializeReactiveState = true)
        {
            if (!_avatarInfo.HasAvatar(index))
            {
                throw new KeyNotFoundException($"{nameof(index)}({index})");
            }

            _avatarInfo.SelectAvatar(index);
            CurrentAvatarState = new AvatarState(_avatarInfo.GetAvatar(index), Game.TableSheets.Instance.GetAvatarSheets());
            UpdateCurrentAvatarState(CurrentAvatarState, initializeReactiveState);
            return CurrentAvatarState;
        }

        public void UpdateCurrentAvatarState(AvatarState state, bool initializeReactiveState = true)
        {
            CurrentAvatarState = state;

            if (!initializeReactiveState)
                return;

            ReactiveAvatarState.Initialize(CurrentAvatarState);
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
