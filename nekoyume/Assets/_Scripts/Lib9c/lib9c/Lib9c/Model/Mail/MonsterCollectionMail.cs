using System;

namespace Nekoyume.Model.Mail
{
    [Serializable]
    public class MonsterCollectionMail : AttachmentMail
    {
        public override void Read(IMail mail)
        {
            mail.Read(this);
        }

        protected override string TypeId => "monsterCollectionMail";
        public override MailType MailType => MailType.System;
    }
}
