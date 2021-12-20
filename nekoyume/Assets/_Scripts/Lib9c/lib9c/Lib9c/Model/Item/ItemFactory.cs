using System;
using System.Globalization;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    public static class ItemFactory
    {
        public static ItemBase CreateItem(ItemSheet.Row row, Random random)
        {
            var guid = Guid.NewGuid();
            switch (row)
            {
                case CostumeItemSheet.Row costumeRow:
                    return CreateCostume(costumeRow, guid);
                case MaterialItemSheet.Row materialRow:
                    return CreateMaterial(materialRow);
                default:
                    return CreateItemUsable(row, guid, 0);
            }
        }

        public static Costume CreateCostume(CostumeItemSheet.Row row, Guid itemId)
        {
            return new Costume(row, itemId);
        }

        public static Material CreateMaterial(MaterialItemSheet sheet, int itemId)
        {
            return sheet.TryGetValue(itemId, out var itemData)
                ? CreateMaterial(itemData)
                : null;
        }

        public static Material CreateMaterial(MaterialItemSheet.Row row) => new Material(row);

        public static TradableMaterial CreateTradableMaterial(MaterialItemSheet.Row row)
            => new TradableMaterial(row);

        public static ItemUsable CreateItemUsable(ItemSheet.Row itemRow, Guid id,
            long requiredBlockIndex, int level = 0)
        {
            Equipment equipment = null;

            switch (itemRow.ItemSubType)
            {
                // Consumable
                case ItemSubType.Food:
                    return new Consumable((ConsumableItemSheet.Row) itemRow, id, requiredBlockIndex);
                // Equipment
                case ItemSubType.Weapon:
                    equipment = new Weapon((EquipmentItemSheet.Row) itemRow, id, requiredBlockIndex);
                    break;
                case ItemSubType.Armor:
                    equipment = new Armor((EquipmentItemSheet.Row) itemRow, id, requiredBlockIndex);
                    break;
                case ItemSubType.Belt:
                    equipment = new Belt((EquipmentItemSheet.Row) itemRow, id, requiredBlockIndex);
                    break;
                case ItemSubType.Necklace:
                    equipment = new Necklace((EquipmentItemSheet.Row) itemRow, id, requiredBlockIndex);
                    break;
                case ItemSubType.Ring:
                    equipment = new Ring((EquipmentItemSheet.Row) itemRow, id, requiredBlockIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        itemRow.Id.ToString(CultureInfo.InvariantCulture));
            }

            for (int i = 0; i < level; ++i)
            {
                equipment.LevelUp();
            }

            return equipment;
        }

        public static ItemUsable CreateItemUsableV2(ItemSheet.Row itemRow, Guid id,
            long requiredBlockIndex, int level,
            Random random, EnhancementCostSheetV2.Row row, bool isGreatSuccess)
        {
            Equipment equipment = null;

            switch (itemRow.ItemSubType)
            {
                // Consumable
                case ItemSubType.Food:
                    return new Consumable((ConsumableItemSheet.Row) itemRow, id, requiredBlockIndex);
                // Equipment
                case ItemSubType.Weapon:
                    equipment = new Weapon((EquipmentItemSheet.Row) itemRow, id, requiredBlockIndex);
                    break;
                case ItemSubType.Armor:
                    equipment = new Armor((EquipmentItemSheet.Row) itemRow, id, requiredBlockIndex);
                    break;
                case ItemSubType.Belt:
                    equipment = new Belt((EquipmentItemSheet.Row) itemRow, id, requiredBlockIndex);
                    break;
                case ItemSubType.Necklace:
                    equipment = new Necklace((EquipmentItemSheet.Row) itemRow, id, requiredBlockIndex);
                    break;
                case ItemSubType.Ring:
                    equipment = new Ring((EquipmentItemSheet.Row) itemRow, id, requiredBlockIndex);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        itemRow.Id.ToString(CultureInfo.InvariantCulture));
            }

            for (int i = 0; i < level; ++i)
            {
                equipment.LevelUpV2(random, row, isGreatSuccess);
            }

            return equipment;
        }
    }
}
