using System;
using System.Collections.Generic;
using System.Linq;
using Gateway.Protocol;
using Nekoyume.Battle;
using Nekoyume.Model.Item;
using Nekoyume.Model.Mail;
using Nekoyume.Model.Quest;
using Nekoyume.TableData;

namespace Nekoyume.Model.State
{
    /// <summary>
    /// Agent가 포함하는 각 Avatar의 상태 모델이다.
    /// </summary>
    [Serializable]
    public class AvatarState : State
    {
        public string NameWithHash => _avatar.Name;
        public string name => _avatar.Name;
        public int characterId => _avatar.CharacterId;
        public int level => _avatar.Level;
        public long exp => _avatar.Exp;
        public int actionPoint => _avatar.ActionPoint;
        public int hair => _avatar.Hair;
        public int lens => _avatar.Lens;
        public int ear => _avatar.Ear;
        public int tail => _avatar.Tail;
        public Inventory inventory { get; private set; }
        public WorldInformation worldInformation { get; private set; }
        public QuestList questList { get; private set; }
        public MailBox mailBox { get; private set; }
        public long dailyRewardReceivedIndex { get; set; }
        public CollectionMap stageMap { get; private set; }
        public CollectionMap monsterMap { get; private set; }
        public CollectionMap itemMap { get; private set; }
        public CollectionMap eventMap { get; private set; }

        private ST_Avatar _avatar;

        public AvatarState(ST_Avatar avatar)
        {
            _avatar = avatar;
            inventory = new Inventory();            
            mailBox = new MailBox();
            stageMap = new CollectionMap();
            monsterMap = new CollectionMap();
            itemMap = new CollectionMap();
            eventMap = new CollectionMap();
        }

        public void SetInventory(
            ST_Inventory inventory,
            ConsumableItemSheet consumableItemSheets,
            CostumeItemSheet costumeItemSheet,
            EquipmentItemSheet equipmentItemSheet,
            MaterialItemSheet materialItemSheet)
        {
            var items = new List<Inventory.Item>();
            items.AddRange(inventory.ConsumableDict.Values.Select(item => ItemFactory.CreateConsumableItem(consumableItemSheets.GetValue(item.ItemId))).OfType<Inventory.Item>());
            items.AddRange(inventory.CostumeDict.Values.Select(item => ItemFactory.CreateCostume(costumeItemSheet.GetValue(item.ItemId))).OfType<Inventory.Item>());
            items.AddRange(inventory.EquipmentDict.Values.Select(item => ItemFactory.CreateItemUsable(equipmentItemSheet.GetValue(item.ItemId))).OfType<Inventory.Item>());
            items.AddRange(inventory.MaterialDict.Values.Select(item => ItemFactory.CreateMaterial(materialItemSheet.GetValue(item.ItemId))).OfType<Inventory.Item>());
            this.inventory = new Inventory(items);
        }

        public void SetWorldAndStage(ST_WorldInfo worldInfo, WorldSheet worldSheet)
        {
            worldInformation = new WorldInformation(worldSheet, GameConfig.IsEditor);
        }

        public void SetQuestList(AvatarSheets avatarSheets)
        {
            questList = new QuestList(
                avatarSheets.QuestSheet,
                avatarSheets.QuestRewardSheet,
                avatarSheets.QuestItemRewardSheet,
                avatarSheets.EquipmentItemRecipeSheet,
                avatarSheets.EquipmentItemSubRecipeSheet
            );
        }

        public void SetMailBox()
        {

        }

        public void Update(StageSimulator stageSimulator, MaterialItemSheet materialItemSheet)
        {
            var player = stageSimulator.Player;
            _avatar.Level = player.Level;
            _avatar.Exp = player.Exp.Current;
            inventory = player.Inventory;
            worldInformation = player.worldInformation;
            foreach (var pair in player.monsterMap)
            {
                monsterMap.Add(pair);
            }
            foreach (var pair in player.eventMap)
            {
                eventMap.Add(pair);
            }
            if (stageSimulator.Log.IsClear)
            {
                stageMap.Add(new KeyValuePair<int, int>(stageSimulator.StageId, 1));
            }
            foreach (var pair in stageSimulator.ItemMap)
            {
                var row = materialItemSheet.OrderedList.First(itemRow => itemRow.Id == pair.Key);
                var item = ItemFactory.CreateMaterial(row);
                var map = inventory.AddItem(item, count: pair.Value);
                itemMap.Add(pair);
            }           

            UpdateStageQuest(stageSimulator.Reward);
        }

        public void Update(Mail.Mail mail)
        {
            mailBox.Add(mail);
            mailBox.CleanUp();
        }

        public int GetArmorId()
        {
            var armor = inventory.Items.Select(i => i.item).OfType<Armor>().FirstOrDefault(e => e.equipped);
            return armor?.Id ?? GameConfig.DefaultAvatarArmorId;
        }

        private void UpdateStageQuest(IEnumerable<ItemBase> items)
        {
            questList.UpdateStageQuest(stageMap);
            questList.UpdateMonsterQuest(monsterMap);
            questList.UpdateCollectQuest(itemMap);
            questList.UpdateItemTypeCollectQuest(items);
            UpdateGeneralQuest(new[] { QuestEventType.Level, QuestEventType.Die });
            UpdateCompletedQuest();
        }

        private void UpdateGeneralQuest(IEnumerable<QuestEventType> types)
        {
            eventMap = questList.UpdateGeneralQuest(types, eventMap);
        }

        private void UpdateCompletedQuest()
        {
            eventMap = questList.UpdateCompletedQuest(eventMap);
        }
    }
}
