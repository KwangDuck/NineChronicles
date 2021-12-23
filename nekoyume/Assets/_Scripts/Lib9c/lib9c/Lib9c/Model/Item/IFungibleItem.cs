using System;

namespace Nekoyume.Model.Item
{
    public interface IFungibleItem : IItem
    {
        public Guid FungibleId { get; }
    }
}
