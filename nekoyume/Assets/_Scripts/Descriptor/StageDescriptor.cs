using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class StageDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<StageDescriptor, int, Manager>
        {
            public override string TableName => "StageSheet";
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
                        if(data is ST_TableStage tableData)
                        {
                            manager.Put(tableData.id, new StageDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, StageDescriptor>
        {

        }

        private readonly ST_TableStage _data;

        protected StageDescriptor(ST_TableStage data) : base(data.id)
        {
            _data = data;
        }
    }
}
