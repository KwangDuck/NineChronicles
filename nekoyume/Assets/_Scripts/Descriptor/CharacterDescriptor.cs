using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class CharacterDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<CharacterDescriptor, int, Manager>
        {
            public override string TableName => "CharacterSheet";
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

                    // init descriptor
                    var manager = Manager as Manager;
                    foreach (var data in _table.dataList)
                    {
                        if(data is ST_TableCharacter tableData)
                        {
                            manager.Put(tableData.id, new CharacterDescriptor(tableData));
                        }
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, CharacterDescriptor>
        {

        }

        private readonly ST_TableCharacter _data;

        protected CharacterDescriptor(ST_TableCharacter data) : base(data.id)
        {
            _data = data;
        }

        
    }
}
