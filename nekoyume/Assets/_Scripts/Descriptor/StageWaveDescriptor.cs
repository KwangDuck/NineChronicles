using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class StageWaveDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<StageWaveDescriptor, int, Manager>
        {
            public override string TableName => "StageWaveSheet";
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
                        if(data is ST_TableStageWave tableData)
                        {
                            manager.Put(tableData.id, new StageWaveDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, StageWaveDescriptor>
        {

        }

        private readonly ST_TableStageWave _data;

        protected StageWaveDescriptor(ST_TableStageWave data) : base(data.id)
        {
            _data = data;
        }
    }
}
