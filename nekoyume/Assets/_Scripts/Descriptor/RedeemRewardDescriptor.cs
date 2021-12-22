﻿using System.Collections.Generic;
using System.Linq;

using Gateway.Protocol.Table;

using NUnit.Framework;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class RedeemRewardDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<RedeemRewardDescriptor, int, Manager>
        {
            public override string TableName => "RedeemRewardSheet";
            private readonly ST_Table _table;

            public Loader(Manager manager, Dictionary<string, ST_Table> tableMap) : base(manager)
            {
                _table = tableMap.Where(entry => entry.Key == TableName).Select(entry => entry.Value).FirstOrDefault();                
            }

            public override void LoadInternal()
            {
                Assert.NotNull(_table);
                {
                    SetTable(new ST_Table
                    {
                        dataList = _table.dataList,
                        version = _table.version,
                    });

                    var manager = Manager as Manager;
                    foreach(var data in _table.dataList)
                    {
                        if(data is ST_TableRedeemReward tableData)
                        {
                            manager.Put(tableData.id, new RedeemRewardDescriptor(tableData));
                        }
                    }
                }
            }
        }

        public class Manager : DescriptorManager<int, RedeemRewardDescriptor>
        {

        }

        private readonly ST_TableRedeemReward _data;

        protected RedeemRewardDescriptor(ST_TableRedeemReward data) : base(data.id)
        {
            _data = data;
        }
    }
}
