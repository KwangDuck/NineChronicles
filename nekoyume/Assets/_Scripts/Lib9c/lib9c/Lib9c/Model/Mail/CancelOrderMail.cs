using System;

namespace Nekoyume.Model.Mail
{
    [Serializable]
    public class CancelOrderMail : Mail
    {
        public readonly Guid OrderId;
        public CancelOrderMail(long blockIndex, Guid id, long requiredBlockIndex, Guid orderId)
        {
            OrderId = orderId;
        }

        public override void Read(IMail mail)
        {
            mail.Read(this);
        }

        public override MailType MailType => MailType.Auction;

        protected override string TypeId => nameof(CancelOrderMail);
    }
}
