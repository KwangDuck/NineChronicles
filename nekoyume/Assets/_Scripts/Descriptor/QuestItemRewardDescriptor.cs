using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class QuestItemRewardDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<QuestItemRewardDescriptor, int, Manager>
        {
            public override string TableName => "QuestItemRewardSheet";
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
                        if(data is ST_TableQuestItemReward tableData)
                        {
                            manager.Put(tableData.id, new QuestItemRewardDescriptor(tableData));   
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, QuestItemRewardDescriptor>
        {

        }

        private readonly ST_TableQuestItemReward _data;

        protected QuestItemRewardDescriptor(ST_TableQuestItemReward data) : base(data.id)
        {
            _data = data;
        }
    }
}
