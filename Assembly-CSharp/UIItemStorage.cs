using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200041F RID: 1055
[AddComponentMenu("NGUI/Examples/UI Item Storage")]
public class UIItemStorage : MonoBehaviour
{
	// Token: 0x170001EE RID: 494
	// (get) Token: 0x060019B3 RID: 6579 RVA: 0x00010B88 File Offset: 0x0000ED88
	public List<InvGameItem> items
	{
		get
		{
			while (this.mItems.Count < this.maxItemCount)
			{
				this.mItems.Add(null);
			}
			return this.mItems;
		}
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x00010BB7 File Offset: 0x0000EDB7
	public InvGameItem GetItem(int slot)
	{
		return (slot >= this.items.Count) ? null : this.mItems[slot];
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x000CF2C0 File Offset: 0x000CD4C0
	public InvGameItem Replace(int slot, InvGameItem item)
	{
		if (slot < this.maxItemCount)
		{
			InvGameItem result = this.items[slot];
			this.mItems[slot] = item;
			return result;
		}
		return item;
	}

	// Token: 0x060019B6 RID: 6582 RVA: 0x000CF2F8 File Offset: 0x000CD4F8
	private void Start()
	{
		if (this.template != null)
		{
			int num = 0;
			Bounds bounds = default(Bounds);
			for (int i = 0; i < this.maxRows; i++)
			{
				for (int j = 0; j < this.maxColumns; j++)
				{
					GameObject gameObject = NGUITools.AddChild(base.gameObject, this.template);
					Transform transform = gameObject.transform;
					transform.localPosition = new Vector3((float)this.padding + ((float)j + 0.5f) * (float)this.spacing, (float)(-(float)this.padding) - ((float)i + 0.5f) * (float)this.spacing, 0f);
					UIStorageSlot component = gameObject.GetComponent<UIStorageSlot>();
					if (component != null)
					{
						component.storage = this;
						component.slot = num;
					}
					bounds.Encapsulate(new Vector3((float)this.padding * 2f + (float)((j + 1) * this.spacing), (float)(-(float)this.padding) * 2f - (float)((i + 1) * this.spacing), 0f));
					if (++num >= this.maxItemCount)
					{
						if (this.background != null)
						{
							this.background.transform.localScale = bounds.size;
						}
						return;
					}
				}
			}
			if (this.background != null)
			{
				this.background.transform.localScale = bounds.size;
			}
		}
	}

	// Token: 0x04001E58 RID: 7768
	public int maxItemCount = 8;

	// Token: 0x04001E59 RID: 7769
	public int maxRows = 4;

	// Token: 0x04001E5A RID: 7770
	public int maxColumns = 4;

	// Token: 0x04001E5B RID: 7771
	public GameObject template;

	// Token: 0x04001E5C RID: 7772
	public UIWidget background;

	// Token: 0x04001E5D RID: 7773
	public int spacing = 128;

	// Token: 0x04001E5E RID: 7774
	public int padding = 10;

	// Token: 0x04001E5F RID: 7775
	private List<InvGameItem> mItems = new List<InvGameItem>();
}
