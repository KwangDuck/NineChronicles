using System;

namespace Nekoyume.Model.Mail
{
    [Serializable]
    public class SellCancelMail : AttachmentMail
    {
        protected override string TypeId => "sellCancel";
        public override MailType MailType => MailType.Auction;

        public override void Read(IMail mail)
        {
            mail.Read(this);
        }
    }
}
