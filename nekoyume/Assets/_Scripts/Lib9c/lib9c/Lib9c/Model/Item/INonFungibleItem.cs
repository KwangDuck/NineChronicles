using System;

namespace Nekoyume.Model.Item
{
    public interface INonFungibleItem : IItem
    {
        public Guid NonFungibleId { get; }
    }
}
