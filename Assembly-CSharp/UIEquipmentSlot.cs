using System;
using UnityEngine;

// Token: 0x0200041D RID: 1053
[AddComponentMenu("NGUI/Examples/UI Equipment Slot")]
public class UIEquipmentSlot : UIItemSlot
{
	// Token: 0x170001EC RID: 492
	// (get) Token: 0x060019A7 RID: 6567 RVA: 0x00010AA5 File Offset: 0x0000ECA5
	protected override InvGameItem observedItem
	{
		get
		{
			return (!(this.equipment != null)) ? null : this.equipment.GetItem(this.slot);
		}
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x00010ACF File Offset: 0x0000ECCF
	protected override InvGameItem Replace(InvGameItem item)
	{
		return (!(this.equipment != null)) ? item : this.equipment.Replace(this.slot, item);
	}

	// Token: 0x04001E4D RID: 7757
	public InvEquipment equipment;

	// Token: 0x04001E4E RID: 7758
	public InvBaseItem.Slot slot;
}
