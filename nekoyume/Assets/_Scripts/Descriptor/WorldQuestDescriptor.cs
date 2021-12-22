﻿using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class WorldQuestDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<WorldQuestDescriptor, int, Manager>
        {
            public override string TableName => "WorldQuestSheet";
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
                        if(data is ST_TableWorldQuest tableData)
                        {
                            manager.Put(tableData.id, new WorldQuestDescriptor(tableData));
                        }                        
                    }
                }
            }
        }


        public class Manager : DescriptorManager<int, WorldQuestDescriptor>
        {

        }

        private readonly ST_TableWorldQuest _data;

        protected WorldQuestDescriptor(ST_TableWorldQuest data) : base(data.id)
        {
            _data = data;
        }
    }
}
