using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class EquipmentItemRecipeDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<EquipmentItemRecipeDescriptor, int, Manager>
        {
            public override string TableName => "EquipmentItemRecipeSheet";
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
                        if(data is ST_TableEquipmentItemRecipe tableData)
                        {
                            manager.Put(tableData.id, new EquipmentItemRecipeDescriptor(tableData));   
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, EquipmentItemRecipeDescriptor>
        {

        }

        private readonly ST_TableEquipmentItemRecipe _data;

        protected EquipmentItemRecipeDescriptor(ST_TableEquipmentItemRecipe data) : base(data.id)
        {
            _data = data;
        }
    }
}
