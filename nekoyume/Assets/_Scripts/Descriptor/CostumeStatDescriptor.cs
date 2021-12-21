using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class CostumeStatDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<CostumeStatDescriptor, int, Manager>
        {
            public override string TableName => "CostumeStatSheet";
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
                        if(data is ST_TableCostumeStat tableData)
                        {
                            manager.Put(tableData.id, new CostumeStatDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, CostumeStatDescriptor>
        {

        }

        private readonly ST_TableCostumeStat _data; 

        protected CostumeStatDescriptor(ST_TableCostumeStat data) : base(data.id)
        {
            _data = data;
        }
    }
}
