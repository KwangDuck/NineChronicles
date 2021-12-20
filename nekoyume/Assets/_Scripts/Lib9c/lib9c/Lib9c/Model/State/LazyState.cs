using System;
using System.Collections.Generic;

namespace Nekoyume.Model.State
{
    [Serializable]
    public sealed class LazyState<TState, TEncoding> : IState
        where TState : IState
    {
        private TEncoding _serialized;
        private Func<TEncoding, TState> _loader;
        private TState _loaded;

        public LazyState(TState loadedValue)
        {
            _loaded = loadedValue;
        }

        public LazyState(TEncoding serialized, Func<TEncoding, TState> loader)
        {
            _serialized = serialized;
            _loader = loader;
        }

        public TState State
        {
            get
            {
                if (_loaded == null)
                {
                    _loaded = _loader(_serialized);
                    _serialized = default;
                    _loader = null;
                }

                return _loaded;
            }
        }

        public bool GetStateOrSerializedEncoding(out TState loadedState, out TEncoding serialized)
        {
            if (_loaded == null)
            {
                loadedState = default;
                serialized = _serialized;
                return false;
            }

            loadedState = _loaded;
            serialized = default;
            return true;
        }

        public static TState LoadState(LazyState<TState, TEncoding> lazyState) =>
            lazyState.State;

        public static KeyValuePair<T, TState> LoadStatePair<T>(
            KeyValuePair<T, LazyState<TState, TEncoding>> lazyPair
        ) =>
            new KeyValuePair<T, TState>(lazyPair.Key, lazyPair.Value.State);
    }
}
