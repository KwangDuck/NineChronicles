using System;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public struct OrderLock : ILock
    {
        public LockType Type => LockType.Order;
        public readonly Guid OrderId;

        public OrderLock(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
