using System.Linq;
using System.Collections.Generic;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class EquipmentItemDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<EquipmentItemDescriptor, int, Manager>
        {
            public override string TableName => "EquipmentItemSheet";
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
                        if(data is ST_TableEquipmentItem tableData)
                        {
                            manager.Put(tableData.id, new EquipmentItemDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, EquipmentItemDescriptor>
        {

        }

        private readonly ST_TableEquipmentItem _data;

        protected EquipmentItemDescriptor(ST_TableEquipmentItem data) : base(data.id)
        {
            _data = data;
        }
    }
}
