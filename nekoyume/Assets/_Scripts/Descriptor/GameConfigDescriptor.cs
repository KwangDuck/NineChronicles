using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class GameConfigDescriptor : Descriptor<string>
    {
        public class Loader : DescriptorLoader<GameConfigDescriptor, string, Manager>
        {
            public override string TableName => "GameConfigSheet";
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
                        if(data is ST_TableGameConfig tableData)
                        {
                            manager.Put(tableData.key, new GameConfigDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<string, GameConfigDescriptor>
        {

        }

        private readonly ST_TableGameConfig _data;

        protected GameConfigDescriptor(ST_TableGameConfig data) : base(data.key)
        {
            _data = data;
        }
    }
}
