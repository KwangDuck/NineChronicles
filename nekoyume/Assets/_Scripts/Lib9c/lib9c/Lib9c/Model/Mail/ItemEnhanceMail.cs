using System;

namespace Nekoyume.Model.Mail
{
    [Serializable]
    public class ItemEnhanceMail : AttachmentMail
    {
        protected override string TypeId => "itemEnhance";
        public override MailType MailType => MailType.Workshop;

        public override void Read(IMail mail)
        {
            mail.Read(this);
        }
    }
}
