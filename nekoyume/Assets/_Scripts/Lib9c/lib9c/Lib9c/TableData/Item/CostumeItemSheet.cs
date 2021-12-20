using System;
using System.Collections.Generic;
using Nekoyume.Model.Item;

namespace Nekoyume.TableData
{
    [Serializable]
    public class CostumeItemSheet : Sheet<int, CostumeItemSheet.Row>
    {
        [Serializable]
        public class Row : ItemSheet.Row
        {
            public override ItemType ItemType => ItemType.Costume;
            public string SpineResourcePath { get; private set; }

            public override void Set(IReadOnlyList<string> fields)
            {
                base.Set(fields);
                if (string.IsNullOrEmpty(fields[4]))
                {
                    switch (ItemSubType)
                    {
                        default:
                            SpineResourcePath = $"Character/PlayerSpineTexture/{ItemSubType}/{Id}";
                            break;
                        case ItemSubType.FullCostume:
                            SpineResourcePath = $"Character/{ItemSubType}/{Id}";
                            break;
                        case ItemSubType.Title:
                            SpineResourcePath = "";
                            break;
                    }
                }
                else
                {
                    SpineResourcePath = fields[4];
                }
            }
        }

        public CostumeItemSheet() :
            base(nameof(CostumeItemSheet))
        {
        }
    }
}
