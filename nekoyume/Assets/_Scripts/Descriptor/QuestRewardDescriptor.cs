using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class QuestRewardDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<QuestRewardDescriptor, int, Manager>
        {
            public override string TableName => "QuestRewardSheet";
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
                    if(_table.dataList is List<ST_TableQuestReward> dataList)
                    {
                        var groupById = dataList
                            .GroupBy(data => data.id, data => data);                    

                        foreach (var entry in groupById)
                        {
                            var id = entry.Key;
                            var data = entry.ToList();
                            manager.Put(id, new QuestRewardDescriptor(id, data));
                        }
                    }                                       
                }
            }
        }


        public class Manager : DescriptorManager<int, QuestRewardDescriptor>
        {

        }

        private readonly List<ST_TableQuestReward> _dataList;

        protected QuestRewardDescriptor(int id, List<ST_TableQuestReward> dataList) : base(id)
        {
            _dataList = dataList;
        }
    }
}
