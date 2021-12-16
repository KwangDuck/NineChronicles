using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Nekoyume.Model.State;
using Nekoyume.State.Subjects;
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
        public WeeklyArenaState WeeklyArenaState { get; private set; }

        public AgentState AgentState { get; private set; }

        public GoldBalanceState GoldBalanceState { get; private set; }

        private readonly Dictionary<int, AvatarState> _avatarStates = new Dictionary<int, AvatarState>();

        public IReadOnlyDictionary<int, AvatarState> AvatarStates => _avatarStates;

        public int CurrentAvatarKey { get; private set; }

        public AvatarState CurrentAvatarState { get; private set; }

        public GameConfigState GameConfigState { get; private set; }

        private readonly Dictionary<int, CombinationSlotState> _combinationSlotStates =
            new Dictionary<int, CombinationSlotState>();

        public States()
        {
            DeselectAvatar();
        }

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

        public AvatarState AddOrReplaceAvatarState(string avatarAddress, int index, bool initializeReactiveState = true)
        {
            var (exist, avatarState) = TryGetAvatarState(avatarAddress, true);
            if (exist)
            {
                AddOrReplaceAvatarState(avatarState, index, initializeReactiveState);
            }

            return null;
        }

        public static (bool exist, AvatarState avatarState) TryGetAvatarState(string address, bool allowBrokenState = true)
        {
            return (false, null);
        }

        public AvatarState AddOrReplaceAvatarState(AvatarState state, int index, bool initializeReactiveState = true)
        {
            if (state is null)
            {
                Debug.LogWarning($"[{nameof(States)}.{nameof(AddOrReplaceAvatarState)}] {nameof(state)} is null.");
                return null;
            }

            if (_avatarStates.ContainsKey(index))
            {
                _avatarStates[index] = state;
            }
            else
            {
                _avatarStates.Add(index, state);
            }
            return state;
        }

        public void RemoveAvatarState(int index)
        {
            if (_avatarStates.ContainsKey(index))
            {
                _avatarStates.Remove(index);
            }

            if (index == CurrentAvatarKey)
            {
                DeselectAvatar();
            }
        }

        public AvatarState SelectAvatar(int index, bool initializeReactiveState = true)
        {
            if (!_avatarStates.ContainsKey(index))
            {
                throw new KeyNotFoundException($"{nameof(index)}({index})");
            }

            var isNew = CurrentAvatarKey != index;

            CurrentAvatarKey = index;
            var avatarState = _avatarStates[CurrentAvatarKey];
            return CurrentAvatarState;
        }

        public void DeselectAvatar()
        {
            CurrentAvatarKey = -1;            
            UpdateCurrentAvatarState(null);
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

        public void SetGameConfigState(GameConfigState state)
        {
            GameConfigState = state;
            GameConfigStateSubject.OnNext(state);
        }

        #endregion

        /// <summary>
        /// `CurrentAvatarKey`에 따라서 `CurrentAvatarState`를 업데이트 한다.
        /// </summary>
        public void UpdateCurrentAvatarState(AvatarState state, bool initializeReactiveState = true)
        {
            CurrentAvatarState = state;

            if (!initializeReactiveState)
                return;

            ReactiveAvatarState.Initialize(CurrentAvatarState);
        }
    }
}
