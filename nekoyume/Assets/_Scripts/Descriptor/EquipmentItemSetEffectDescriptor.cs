using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class EquipmentItemSetEffectDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<EquipmentItemSetEffectDescriptor, int, Manager>
        {
            public override string TableName => "EquipmentItemSetEffectSheet";
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

                    if(_table.dataList is List<ST_TableEquipmentItemSetEffect> tableDataList)
                    {
                        var groupById = tableDataList.GroupBy(data => data.id, data => data);
                        foreach(var entry in groupById)
                        {
                            var id = entry.Key;
                            var data = entry.ToList();
                            manager.Put(id, new EquipmentItemSetEffectDescriptor(id, data));
                        }
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, EquipmentItemSetEffectDescriptor>
        {

        }

        private readonly List<ST_TableEquipmentItemSetEffect> _dataList;

        protected EquipmentItemSetEffectDescriptor(int id, List<ST_TableEquipmentItemSetEffect> dataList) : base(id)
        {
            _dataList = dataList;
        }
    }
}
