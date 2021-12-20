using System;
using System.Collections.Generic;
using System.Linq;
using Nekoyume.Model.State;

namespace Nekoyume.Model.Mail
{
    [Serializable]
    public class OrderBuyerMail : Mail
    {
        public readonly Guid OrderId;
        public OrderBuyerMail(long blockIndex, Guid id, long requiredBlockIndex, Guid orderId)
        {
            OrderId = orderId;
        }

        public override void Read(IMail mail)
        {
            mail.Read(this);
        }

        public override MailType MailType => MailType.Auction;

        protected override string TypeId => nameof(OrderBuyerMail);
    }
}
