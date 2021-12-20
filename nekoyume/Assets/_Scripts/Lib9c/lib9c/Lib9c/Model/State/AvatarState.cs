using System;
using System.Linq;
using Gateway.Protocol;
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
        public int hair;
        public int lens;
        public int ear;
        public int tail;
        public Inventory inventory { get; private set; }
        public WorldInformation worldInformation { get; private set; }
        public QuestList questList;
        public MailBox mailBox;
        public long dailyRewardReceivedIndex;
        
        public CollectionMap stageMap;
        public CollectionMap monsterMap;
        public CollectionMap itemMap;
        public CollectionMap eventMap;
        
        private readonly ST_Avatar _avatar;

        public AvatarState(ST_Avatar avatar, AvatarSheets avatarSheets, WorldSheet worldSheet)
        {
            _avatar = avatar;
            inventory = new Inventory();
            worldInformation = new WorldInformation(worldSheet);
        }

        public int GetArmorId()
        {
            var armor = inventory.Items.Select(i => i.item).OfType<Armor>().FirstOrDefault(e => e.equipped);
            return armor?.Id ?? GameConfig.DefaultAvatarArmorId;
        }
    }
}
