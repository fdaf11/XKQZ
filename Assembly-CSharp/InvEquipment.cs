using System;
using UnityEngine;

// Token: 0x02000425 RID: 1061
[AddComponentMenu("NGUI/Examples/Equipment")]
public class InvEquipment : MonoBehaviour
{
	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x060019C8 RID: 6600 RVA: 0x00010CA5 File Offset: 0x0000EEA5
	public InvGameItem[] equippedItems
	{
		get
		{
			return this.mItems;
		}
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x000CF6B8 File Offset: 0x000CD8B8
	public InvGameItem Replace(InvBaseItem.Slot slot, InvGameItem item)
	{
		InvBaseItem invBaseItem = (item == null) ? null : item.baseItem;
		if (slot == InvBaseItem.Slot.None)
		{
			if (item != null)
			{
				Debug.LogWarning("Can't equip \"" + item.name + "\" because it doesn't specify an item slot");
			}
			return item;
		}
		if (invBaseItem != null && invBaseItem.slot != slot)
		{
			return item;
		}
		if (this.mItems == null)
		{
			int num = 8;
			this.mItems = new InvGameItem[num];
		}
		InvGameItem result = this.mItems[slot - InvBaseItem.Slot.Weapon];
		this.mItems[slot - InvBaseItem.Slot.Weapon] = item;
		if (this.mAttachments == null)
		{
			this.mAttachments = base.GetComponentsInChildren<InvAttachmentPoint>();
		}
		int i = 0;
		int num2 = this.mAttachments.Length;
		while (i < num2)
		{
			InvAttachmentPoint invAttachmentPoint = this.mAttachments[i];
			if (invAttachmentPoint.slot == slot)
			{
				GameObject gameObject = invAttachmentPoint.Attach((invBaseItem == null) ? null : invBaseItem.attachment);
				if (invBaseItem != null && gameObject != null)
				{
					Renderer renderer = gameObject.renderer;
					if (renderer != null)
					{
						renderer.material.color = invBaseItem.color;
					}
				}
			}
			i++;
		}
		return result;
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x000CF7E8 File Offset: 0x000CD9E8
	public InvGameItem Equip(InvGameItem item)
	{
		if (item != null)
		{
			InvBaseItem baseItem = item.baseItem;
			if (baseItem != null)
			{
				return this.Replace(baseItem.slot, item);
			}
			Debug.LogWarning("Can't resolve the item ID of " + item.baseItemID);
		}
		return item;
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x000CF834 File Offset: 0x000CDA34
	public InvGameItem Unequip(InvGameItem item)
	{
		if (item != null)
		{
			InvBaseItem baseItem = item.baseItem;
			if (baseItem != null)
			{
				return this.Replace(baseItem.slot, null);
			}
		}
		return item;
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x00010CAD File Offset: 0x0000EEAD
	public InvGameItem Unequip(InvBaseItem.Slot slot)
	{
		return this.Replace(slot, null);
	}

	// Token: 0x060019CD RID: 6605 RVA: 0x000CF864 File Offset: 0x000CDA64
	public bool HasEquipped(InvGameItem item)
	{
		if (this.mItems != null)
		{
			int i = 0;
			int num = this.mItems.Length;
			while (i < num)
			{
				if (this.mItems[i] == item)
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x000CF8A8 File Offset: 0x000CDAA8
	public bool HasEquipped(InvBaseItem.Slot slot)
	{
		if (this.mItems != null)
		{
			int i = 0;
			int num = this.mItems.Length;
			while (i < num)
			{
				InvBaseItem baseItem = this.mItems[i].baseItem;
				if (baseItem != null && baseItem.slot == slot)
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x000CF900 File Offset: 0x000CDB00
	public InvGameItem GetItem(InvBaseItem.Slot slot)
	{
		if (slot != InvBaseItem.Slot.None)
		{
			int num = slot - InvBaseItem.Slot.Weapon;
			if (this.mItems != null && num < this.mItems.Length)
			{
				return this.mItems[num];
			}
		}
		return null;
	}

	// Token: 0x04001E7F RID: 7807
	private InvGameItem[] mItems;

	// Token: 0x04001E80 RID: 7808
	private InvAttachmentPoint[] mAttachments;
}
