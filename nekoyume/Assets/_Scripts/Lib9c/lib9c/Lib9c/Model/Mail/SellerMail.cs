using System;

namespace Nekoyume.Model.Mail
{
    [Serializable]
    public class SellerMail : AttachmentMail
    {
        protected override string TypeId => "seller";
        public override MailType MailType => MailType.Auction;

        public override void Read(IMail mail)
        {
            mail.Read(this);
        }
    }
}
