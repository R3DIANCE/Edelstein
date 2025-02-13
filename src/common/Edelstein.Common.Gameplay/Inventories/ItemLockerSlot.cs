﻿using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Inventories.Items;

namespace Edelstein.Common.Gameplay.Inventories;

public record ItemLockerSlot : IItemLockerSlot
{
    public int ID => Item.ID;

    public IItemSlot Item { get; set; }

    public int AccountID { get; set; }
    public int CharacterID { get; set; }
    public int CommodityID { get; set; }
    public string BuyCharacterName { get; set; }
    public int PaybackRate { get; set; }
    public int DiscountRate { get; set; }
}
