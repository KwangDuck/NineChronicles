﻿using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class GoldQuestDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<GoldQuestDescriptor, int, Manager>
        {
            public override string TableName => "GoldQuestSheet";
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
                        if(data is ST_TableGoldQuest tableData)
                        {
                            manager.Put(tableData.id, new GoldQuestDescriptor(tableData));
                        }                        
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, GoldQuestDescriptor>
        {

        }

        private readonly ST_TableGoldQuest _data;

        protected GoldQuestDescriptor(ST_TableGoldQuest data) : base(data.id)
        {
            _data = data;
        }
    }
}
