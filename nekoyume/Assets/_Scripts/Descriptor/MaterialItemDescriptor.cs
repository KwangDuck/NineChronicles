using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class MaterialItemDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<MaterialItemDescriptor, int, Manager>
        {
            public override string TableName => "MaterialItemSheet";
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
                        if(data is ST_TableMaterialItem tableData)
                        {
                            manager.Put(tableData.id, new MaterialItemDescriptor(tableData));   
                        }                        
                    }
                }
            }
        }


        public class Manager : DescriptorManager<int, MaterialItemDescriptor>
        {

        }

        private readonly ST_TableMaterialItem _data;

        protected MaterialItemDescriptor(ST_TableMaterialItem data) : base(data.id)
        {
            _data = data;
        }
    }
}
