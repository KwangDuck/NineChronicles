using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Nekoyume.Game.Util
{
    public class PooledObject : MonoBehaviour, IDisposable
    {
        public Action<GameObject> onDispose = null;

        public void Dispose()
        {
            OnDispose();
            gameObject.SetActive(false);
        }

        private void OnDispose() {
            onDispose?.Invoke(gameObject);
        }     
    }
}
