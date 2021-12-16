using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Nekoyume.Model.Item;
using UnityEngine;

namespace Nekoyume.Helper
{
    public static class Util
    {
        public const int VisibleEnhancementEffectLevel = 10;
        private const int BlockPerSecond = 12;
        private const string StoredSlotIndex = "AutoSelectedSlotIndex_";

        public static string GetBlockToTime(int block)
        {
            var remainSecond = block * BlockPerSecond;
            var timeSpan = TimeSpan.FromSeconds(remainSecond);

            var sb = new StringBuilder();

            if (timeSpan.Days > 0)
            {
                sb.Append($"{timeSpan.Days}d");
            }

            if (timeSpan.Hours > 0)
            {
                if (timeSpan.Days > 0)
                {
                    sb.Append(" ");
                }

                sb.Append($"{timeSpan.Hours}h");
            }

            if (timeSpan.Minutes > 0)
            {
                if (timeSpan.Hours > 0)
                {
                    sb.Append(" ");
                }

                sb.Append($"{timeSpan.Minutes}m");
            }

            if (sb.Length == 0)
            {
                sb.Append("1m");
            }

            return sb.ToString();
        }

        public static async Task<ItemBase> GetItemBaseByTradableId(Guid tradableId, long requiredBlockExpiredIndex)
        {
            return null;
        }

        public static ItemBase CreateItemBaseByItemId(int itemId)
        {
            return null;
        }

        public static int GetHourglassCount(Inventory inventory, long currentBlockIndex)
        {
            if (inventory is null)
            {
                return 0;
            }

            var count = 0;
            var materials =
                inventory.Items.OrderByDescending(x => x.item.ItemType == ItemType.Material);
            var hourglass = materials.Where(x => x.item.ItemSubType == ItemSubType.Hourglass);
            foreach (var item in hourglass)
            {
                if (item.item is TradableMaterial tradableItem)
                {
                    if (tradableItem.RequiredBlockIndex > currentBlockIndex)
                    {
                        continue;
                    }
                }

                count += item.count;
            }

            return count;
        }

        public static bool TryGetStoredAvatarSlotIndex(out int slotIndex)
        {            
            var agentAddress = string.Empty;
            var key = $"{StoredSlotIndex}{agentAddress}";
            var hasKey = PlayerPrefs.HasKey(key);
            slotIndex = hasKey ? PlayerPrefs.GetInt(key) : 0;
            return hasKey;
        }

        public static void SaveAvatarSlotIndex(int slotIndex)
        {            
            var agentAddress = string.Empty;
            var key = $"{StoredSlotIndex}{agentAddress}";
            PlayerPrefs.SetInt(key, slotIndex);
        }
    }
}
