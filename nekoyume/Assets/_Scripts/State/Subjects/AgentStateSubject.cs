using System;
using UniRx;

namespace Nekoyume.State.Subjects
{
    /// <summary>
    /// The change of the value included in `AgentState` is notified to the outside through each Subject<T> field.
    /// </summary>
    public static class AgentStateSubject
    {
        private static readonly Subject<long> _gold;
            
        public static readonly IObservable<long> Gold;

        static AgentStateSubject()
        {
            _gold = new Subject<long>();
            Gold = _gold.ObserveOnMainThread();
        }

        public static void OnNextGold(long gold)
        {
            _gold.OnNext(gold);
        }
    }
}
