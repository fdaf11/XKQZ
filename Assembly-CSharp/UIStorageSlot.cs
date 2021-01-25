using System;
using UnityEngine;

// Token: 0x02000420 RID: 1056
[AddComponentMenu("NGUI/Examples/UI Storage Slot")]
public class UIStorageSlot : UIItemSlot
{
	// Token: 0x170001EF RID: 495
	// (get) Token: 0x060019B8 RID: 6584 RVA: 0x00010BDC File Offset: 0x0000EDDC
	protected override InvGameItem observedItem
	{
		get
		{
			return (!(this.storage != null)) ? null : this.storage.GetItem(this.slot);
		}
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x00010C06 File Offset: 0x0000EE06
	protected override InvGameItem Replace(InvGameItem item)
	{
		return (!(this.storage != null)) ? item : this.storage.Replace(this.slot, item);
	}

	// Token: 0x04001E60 RID: 7776
	public UIItemStorage storage;

	// Token: 0x04001E61 RID: 7777
	public int slot;
}
