using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class EnhancementCostDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<EnhancementCostDescriptor, int, Manager>
        {
            public override string TableName => "EnhancementCostSheet";
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
                        if(data is ST_TableEnhancementCost tableData)
                        {
                            manager.Put(tableData.id, new EnhancementCostDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, EnhancementCostDescriptor>
        {

        }

        private readonly ST_TableEnhancementCost _data;

        protected EnhancementCostDescriptor(ST_TableEnhancementCost data) : base(data.id)
        {
            _data = data;
        }
    }
}
