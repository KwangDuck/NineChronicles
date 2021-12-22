using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class SkillBuffDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<SkillBuffDescriptor, int, Manager>
        {
            public override string TableName => "SkillBuffSheet";
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
                        if(data is ST_TableSkillBuff tableData)
                        {
                            manager.Put(tableData.skill_id, new SkillBuffDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, SkillBuffDescriptor>
        {

        }

        private readonly ST_TableSkillBuff _data;

        protected SkillBuffDescriptor(ST_TableSkillBuff data) : base(data.skill_id)
        {
            _data = data;
        }
    }
}
