using System;
using System.Collections.Generic;
using System.Linq;
using Gateway.Protocol;
using Nekoyume.Model.State;
using Nekoyume.TableData;

namespace Nekoyume.Model
{
    [Serializable]
    public class WorldInformation : IState
    {
        [Serializable]
        public struct World : IState
        {
            public readonly int Id;
            public readonly string Name;
            public readonly int StageBegin;
            public readonly int StageEnd;
            public readonly int StageClearedId;

            public bool IsUnlocked => true;
            public bool IsStageCleared => true;

            public World(WorldSheet.Row worldRow, int stageClearedId = 0)
            {
                Id = worldRow.Id;
                Name = worldRow.Name;
                StageBegin = worldRow.StageBegin;
                StageEnd = worldRow.StageEnd;
                StageClearedId = stageClearedId;
            }

            public bool ContainsStageId(int stageId)
            {
                return stageId >= StageBegin &&
                       stageId <= StageEnd;
            }

            public bool IsPlayable(int stageId)
            {
                return stageId <= GetNextStageIdForPlay();
            }

            public int GetNextStageIdForPlay()
            {
                if (!IsUnlocked)
                    return -1;

                return GetNextStageId();
            }

            public int GetNextStageId()
            {
                return IsStageCleared ? Math.Min(StageEnd, StageClearedId + 1) : StageBegin;
            }
        }

        private Dictionary<int, World> _worlds;

        public WorldInformation(WorldSheet worldSheet, bool openAllOfWorldsAndStages = false)
        {
            if (worldSheet is null)
            {
                return;
            }

            var orderedSheet = worldSheet.OrderedList;
            _worlds = new Dictionary<int, World>();

            if (openAllOfWorldsAndStages)
            {
                foreach (var row in orderedSheet)
                {
                    _worlds.Add(row.Id, new World(row, row.StageEnd));
                }
            }
            else
            {
                var isFirst = true;
                foreach (var row in orderedSheet)
                {
                    var worldId = row.Id;
                    if (isFirst)
                    {
                        isFirst = false;
                        _worlds.Add(worldId, new World(row));
                    }
                    else
                    {
                        _worlds.Add(worldId, new World(row));
                    }
                }
            }
        }

        public WorldInformation(ST_WorldInfo worldInfo, bool openAllOfWorldsAndStages = false)
        {

        }

        public bool TryGetWorld(int worldId, out World world)
        {
            if (!_worlds.ContainsKey(worldId))
            {
                world = default;
                return false;
            }

            world = _worlds[worldId];
            return true;
        }

        public bool TryGetFirstWorld(out World world)
        {
            if (_worlds.Count == 0)
            {
                world = default;
                return false;
            }

            world = _worlds.OrderBy(w => w.Key).First().Value;
            return true;
        }

        public bool TryGetWorldByStageId(int stageId, out World world)
        {
            foreach (var w in _worlds.Values)
            {
                if (w.ContainsStageId(stageId))
                {
                    world = w;
                    return true;
                }
            }

            world = default;
            return false;
        }

        public bool IsStageCleared(int stageId)
        {
            return true;
        }
    }
}
