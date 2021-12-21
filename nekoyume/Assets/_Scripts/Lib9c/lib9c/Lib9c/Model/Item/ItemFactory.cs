using System;
using System.Globalization;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    public static class ItemFactory
    {
        public static ItemBase CreateItem(ItemSheet.Row row)
        {
            switch (row)
            {
                case CostumeItemSheet.Row costumeRow:
                    return CreateCostume(costumeRow);
                case MaterialItemSheet.Row materialRow:
                    return CreateMaterial(materialRow);
                default:
                    return CreateItemUsable(row);
            }
        }

        public static Costume CreateCostume(CostumeItemSheet.Row row)
        {
            return new Costume(row);
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

        public static Consumable CreateConsumableItem(ConsumableItemSheet.Row itemRow)
        {
            return new Consumable(itemRow);
        }

        public static ItemUsable CreateItemUsable(ItemSheet.Row itemRow, int level = 0)
        {
            Equipment equipment = null;

            switch (itemRow.ItemSubType)
            {
                // Consumable
                case ItemSubType.Food:
                    return new Consumable(itemRow as ConsumableItemSheet.Row);
                // Equipment
                case ItemSubType.Weapon:
                    equipment = new Weapon(itemRow as EquipmentItemSheet.Row);
                    break;
                case ItemSubType.Armor:
                    equipment = new Armor(itemRow as EquipmentItemSheet.Row);
                    break;
                case ItemSubType.Belt:
                    equipment = new Belt(itemRow as EquipmentItemSheet.Row);
                    break;
                case ItemSubType.Necklace:
                    equipment = new Necklace(itemRow as EquipmentItemSheet.Row);
                    break;
                case ItemSubType.Ring:
                    equipment = new Ring(itemRow as EquipmentItemSheet.Row);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(itemRow.Id.ToString(CultureInfo.InvariantCulture));
            }

            for (int i = 0; i < level; ++i)
            {
                equipment.LevelUp();
            }

            return equipment;
        }
    }
}
