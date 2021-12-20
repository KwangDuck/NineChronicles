using System;

namespace Nekoyume.Model.Mail
{
    [Serializable]
    public class BuyerMail : AttachmentMail
    {
        protected override string TypeId => "buyerMail";
        public override MailType MailType => MailType.Auction;

        public override void Read(IMail mail)
        {
            mail.Read(this);
        }
    }
}
