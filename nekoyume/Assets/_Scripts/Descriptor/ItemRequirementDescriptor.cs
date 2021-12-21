using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class ItemRequirementDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<ItemRequirementDescriptor, int, Manager>
        {
            public override string TableName => "ItemRequirementSheet";
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
                        if(data is ST_TableItemRequirement tableData)
                        {
                            manager.Put(tableData.item_id, new ItemRequirementDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, ItemRequirementDescriptor>
        {

        }

        private readonly ST_TableItemRequirement _data;

        protected ItemRequirementDescriptor(ST_TableItemRequirement data) : base(data.item_id)
        {
            _data = data;
        }
    }
}
