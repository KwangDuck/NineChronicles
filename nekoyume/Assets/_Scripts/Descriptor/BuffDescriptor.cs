using System.Linq;
using System.Collections.Generic;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class BuffDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<BuffDescriptor, int, Manager>
        {
            public override string TableName => "BuffSheet";
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
                        if(data is ST_TableBuff tableData)
                        {
                            manager.Put(tableData.id, new BuffDescriptor(tableData));
                        }                        
                    }
                }                
            }
        }

        public class Manager : DescriptorManager<int, BuffDescriptor>
        {
            //Link
        }

        private readonly ST_TableBuff _data;

        protected BuffDescriptor(ST_TableBuff data) : base(data.id)
        {
            _data = data;
        }
        
        public int Group => _data.group;
    }
}
