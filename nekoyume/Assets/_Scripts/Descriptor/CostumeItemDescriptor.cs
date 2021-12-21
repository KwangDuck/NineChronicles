using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class CostumeItemDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<CostumeItemDescriptor, int, Manager>
        {
            public override string TableName => "CostumeItemSheet";
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
                        if(data is ST_TableCostumeItem tableData)
                        {
                            manager.Put(tableData.id, new CostumeItemDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, CostumeItemDescriptor>
        {

        }

        private readonly ST_TableCostumeItem _data;

        protected CostumeItemDescriptor(ST_TableCostumeItem data) : base(data.id)
        {
            _data = data;
        }
    }
}
