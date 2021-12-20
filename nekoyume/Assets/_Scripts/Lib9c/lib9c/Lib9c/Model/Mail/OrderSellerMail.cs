using System;

namespace Nekoyume.Model.Mail
{
    [Serializable]
    public class OrderSellerMail : Mail
    {
        public readonly Guid OrderId;
        public OrderSellerMail(long blockIndex, Guid id, long requiredBlockIndex, Guid orderId)
        {
            OrderId = orderId;
        }

        public override void Read(IMail mail)
        {
            mail.Read(this);
        }

        public override MailType MailType => MailType.Auction;

        protected override string TypeId => nameof(OrderSellerMail);
    }
}
