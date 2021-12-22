using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class MonsterCollectionDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<MonsterCollectionDescriptor, int, Manager>
        {
            public override string TableName => "MonsterCollectionSheet";
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
                    foreach (var data in _table.dataList)
                    {
                        if(data is ST_TableMonsterCollection tableData)
                        {
                            manager.Put(tableData.level, new MonsterCollectionDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, MonsterCollectionDescriptor>
        {

        }

        private readonly ST_TableMonsterCollection _data;

        protected MonsterCollectionDescriptor(ST_TableMonsterCollection data) : base(data.level)
        {
            _data = data;
        }
    }
}
