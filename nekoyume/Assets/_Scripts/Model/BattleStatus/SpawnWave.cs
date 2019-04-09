using System;
using System.Collections;
using System.Collections.Generic;

namespace Nekoyume.Model
{
    [Serializable]
    public class SpawnWave : EventBase
    {
        public List<Monster> monsters;
        public bool isBoss;
        public override IEnumerator CoExecute(IStage stage)
        {
            yield return stage.CoSpawnWave(monsters, isBoss);
        }
    }
}
