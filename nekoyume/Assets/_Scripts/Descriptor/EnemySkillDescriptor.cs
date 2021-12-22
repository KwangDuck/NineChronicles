using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class EnemySkillDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<EnemySkillDescriptor, int, Manager>
        {
            public override string TableName => "EnemySkillSheet";
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
                        if(data is ST_TableEnemySkill tableData)
                        {
                            manager.Put(tableData.id, new EnemySkillDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, EnemySkillDescriptor>
        {

        }

        private readonly ST_TableEnemySkill _data;

        protected EnemySkillDescriptor(ST_TableEnemySkill data) : base(data.id)
        {
            _data = data;
        }
    }
}
