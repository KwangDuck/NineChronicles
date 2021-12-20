using System;
using System.Collections.Generic;
using Nekoyume.Model.State;
using static Nekoyume.TableData.TableExtensions;

namespace Nekoyume.TableData
{
    [Serializable]
    public class QuestSheet : Sheet<int, QuestSheet.Row>
    {
        [Serializable]
        public class Row : SheetRow<int>, IState
        {
            public override int Key => Id;
            public int Id { get; private set; }
            public int Goal { get; private set; }
            public int QuestRewardId { get; private set; }
            
            public override void Set(IReadOnlyList<string> fields)
            {
                Id = ParseInt(fields[0]);
                Goal = ParseInt(fields[1]);
                QuestRewardId = ParseInt(fields[2]);
            }
        }
        
        public QuestSheet() : base(nameof(QuestSheet))
        {
        }
    }
}
