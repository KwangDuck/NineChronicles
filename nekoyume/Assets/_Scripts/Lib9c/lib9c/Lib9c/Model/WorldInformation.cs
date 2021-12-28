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

            public bool IsUnlocked { get; private set; }
            public bool IsStageCleared { get; private set; }

            public World(WorldSheet.Row worldRow, int stageClearedId = 0)
            {
                Id = worldRow.Id;
                Name = worldRow.Name;
                StageBegin = worldRow.StageBegin;
                StageEnd = worldRow.StageEnd;
                StageClearedId = stageClearedId;
                
                IsUnlocked = true;
                IsStageCleared = stageClearedId >= StageBegin;
            }

            public World(WorldSheet.Row worldRow, int lastWorldId, int stageClearedId = 0)
            {
                Id = worldRow.Id;
                Name = worldRow.Name;
                StageBegin = worldRow.StageBegin;
                StageEnd = worldRow.StageEnd;
                StageClearedId = stageClearedId;

                IsUnlocked = Id <= lastWorldId;
                IsStageCleared = stageClearedId > StageBegin;
            }

            public World(World world, int stageClearedId)
            {
                Id = world.Id;
                Name = world.Name;
                StageBegin = world.StageBegin;
                StageEnd = world.StageEnd;
                StageClearedId = stageClearedId;

                IsUnlocked = true;
                IsStageCleared = stageClearedId > StageBegin;
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

        public WorldInformation(WorldSheet worldSheet, int lastWorldId, int lastStageId, bool openAllOfWorldsAndStages = false)
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
                //var isFirst = true;
                foreach (var row in orderedSheet)
                {
                    _worlds.Add(row.Id, new World(row, lastWorldId, lastStageId));

                    // var worldId = row.Id;
                    // if (isFirst)
                    // {
                    //     isFirst = false;
                    //     _worlds.Add(worldId, new World(row, lastWorldId, lastStageId));
                    // }
                    // else
                    // {
                    //     //_worlds.Add(worldId, new World(row));
                    //     _worlds.Add(worldId, new World(row, lastWorldId, lastStageId));
                    // }
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

        public bool TryGetLastClearedStageId(out int stageId)
        {
            var clearedStages = _worlds.Values
                .Where(world => world.Id < GameConfig.MimisbrunnrWorldId &&
                                world.IsStageCleared)
                .ToList();

            if (clearedStages.Any())
            {
                stageId = clearedStages.Max(world => world.StageClearedId);
                return true;
            }

            stageId = default;
            return false;
        }

        public bool TryGetLastClearedMimisbrunnrStageId(out int stageId)
        {
            var clearedStages = _worlds.Values
                .Where(world => world.Id == GameConfig.MimisbrunnrWorldId &&
                                world.IsStageCleared)
                .ToList();

            if (clearedStages.Any())
            {
                stageId = clearedStages.Max(world => world.StageClearedId);
                return true;
            }

            stageId = default;
            return false;
        }

        public bool IsStageCleared(int stageId)
        {
            int clearedStageId;
            if (stageId >= GameConfig.MimisbrunnrStartStageId
                ? TryGetLastClearedMimisbrunnrStageId(out clearedStageId)
                : TryGetLastClearedStageId(out clearedStageId))
            {
                return stageId <= clearedStageId;
            }

            return false;
        }

        public void ClearStage(int worldId, int stageId, WorldSheet worldSheet, WorldUnlockSheet worldUnlockSheet)
        {
            if(!_worlds.ContainsKey(worldId))            
                return;    

            var world = _worlds[worldId];
            if(stageId < world.StageBegin || stageId > world.StageEnd)
                return;

            if(worldUnlockSheet.TryGetUnlockedInformation(worldId, stageId, out var worldIdsToUnlock))
            {
                foreach(var worldIdToUnlock in worldIdsToUnlock)
                {
                    UnlockWorld(worldIdToUnlock, worldId, stageId, worldSheet);
                }
            }

            if (stageId <= world.StageClearedId)
            {
                return;
            }

            _worlds[worldId] = new World(world, stageId);
        }

        public void UnlockWorld(int worldId, int lastWorldId, int lastStageId, WorldSheet worldSheet)
        {
            World world;
            if (_worlds.ContainsKey(worldId))  
            {
                world = _worlds[worldId];
            }                                      
            else if (!worldSheet.TryGetValue(worldId, out var worldRow) || !TryAddWorld(worldRow, out world))
            {
                throw new Exception($"{nameof(worldId)}: {worldId}");
            }

            if (world.IsUnlocked)
            {
                return;
            }

            _worlds[worldId] = new World(world, lastStageId);
        }

        public bool TryAddWorld(WorldSheet.Row worldRow, out World world)
        {
            if(worldRow is null || _worlds.ContainsKey(worldRow.Id))
            {
                world = default;
                return false;
            }

            world = new World(worldRow);
            _worlds.Add(worldRow.Id, world);
            return true;
        }
    }
}
