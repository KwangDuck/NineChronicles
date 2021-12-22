using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class CharacterLevelDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<CharacterLevelDescriptor, int, Manager>
        {
            public override string TableName => "CharacterLevelSheet";
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
                        if(data is ST_TableCharacterLevel tableData)
                        {
                            manager.Put(tableData.level, new CharacterLevelDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, CharacterLevelDescriptor>
        {

        }

        private readonly ST_TableCharacterLevel _data;

        protected CharacterLevelDescriptor(ST_TableCharacterLevel data) : base(data.level)
        {
            _data = data;
        }
    }
}
