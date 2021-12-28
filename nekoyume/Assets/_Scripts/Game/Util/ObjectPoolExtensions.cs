using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nekoyume.Game.Util
{
    public static class ObjectPoolExtensions
    {
        public static void Despawn(this GameObject go) 
        {
            if(go.TryGetComponent<PooledObject>(out var pooledObject))
            {
                pooledObject.Dispose();
            }
        }

        public static void Despawn(this PooledObject po) 
        {
            po.Dispose();
        }
    }
}
