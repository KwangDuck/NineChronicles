using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class MonsterCollectionRewardDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<MonsterCollectionRewardDescriptor, int, Manager>
        {
            public override string TableName => "MonsterCollectionRewardSheet";
            private readonly ST_Table _table;

            public Loader(Manager manager, Dictionary<string, ST_Table> tableMap) : base(manager)
            {
                _table = tableMap.Where(entry => entry.Key == TableName).Select(entry => entry.Value).FirstOrDefault();                
            }

            public override void LoadInternal()
            {                
                Assert.NotNull(_table);
                {
                    // keep table
                    SetTable(new ST_Table
                    {
                        dataList = _table.dataList,
                        version = _table.version
                    });

                    // init descriptors
                    var manager = Manager as Manager;

                    if(_table.dataList is List<ST_TableMonsterCollectionReward> dataList)
                    {
                        var groupByCollectionLevel = dataList.GroupBy(data => data.collection_level, data => data);
                        foreach (var entry in groupByCollectionLevel)
                        {
                            var id = entry.Key;
                            var data = entry.ToList();
                            manager.Put(id, new MonsterCollectionRewardDescriptor(id, data));
                        }
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, MonsterCollectionRewardDescriptor>
        {

        }

        private readonly List<ST_TableMonsterCollectionReward> _dataList;

        protected MonsterCollectionRewardDescriptor(int id, List<ST_TableMonsterCollectionReward> dataList) : base(id)
        {
            _dataList = dataList;
        }
    }
}
