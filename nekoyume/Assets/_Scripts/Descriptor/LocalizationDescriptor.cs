using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class LocalizationDescriptor : Descriptor<string>
    {
        public class Loader : DescriptorLoader<LocalizationDescriptor, string, Manager>
        {
            public override string TableName => "Localization";
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
                        if(data is ST_TableLocalization tableData)
                        {
                            manager.Put(tableData.Key, new LocalizationDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<string, LocalizationDescriptor>
        {

        }

        private readonly ST_TableLocalization _data;

        protected LocalizationDescriptor(ST_TableLocalization data) : base(data.Key)
        {
            _data = data;
        }
    }
}
