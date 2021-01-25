using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200041E RID: 1054
public abstract class UIItemSlot : MonoBehaviour
{
	// Token: 0x170001ED RID: 493
	// (get) Token: 0x060019AA RID: 6570
	protected abstract InvGameItem observedItem { get; }

	// Token: 0x060019AB RID: 6571
	protected abstract InvGameItem Replace(InvGameItem item);

	// Token: 0x060019AC RID: 6572 RVA: 0x000CEED4 File Offset: 0x000CD0D4
	private void OnTooltip(bool show)
	{
		InvGameItem invGameItem = (!show) ? null : this.mItem;
		if (invGameItem != null)
		{
			InvBaseItem baseItem = invGameItem.baseItem;
			if (baseItem != null)
			{
				string text = string.Concat(new string[]
				{
					"[",
					NGUIText.EncodeColor(invGameItem.color),
					"]",
					invGameItem.name,
					"[-]\n"
				});
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"[AFAFAF]Level ",
					invGameItem.itemLevel,
					" ",
					baseItem.slot
				});
				List<InvStat> list = invGameItem.CalculateStats();
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					InvStat invStat = list[i];
					if (invStat.amount != 0)
					{
						if (invStat.amount < 0)
						{
							text = text + "\n[FF0000]" + invStat.amount;
						}
						else
						{
							text = text + "\n[00FF00]+" + invStat.amount;
						}
						if (invStat.modifier == InvStat.Modifier.Percent)
						{
							text += "%";
						}
						text = text + " " + invStat.id;
						text += "[-]";
					}
					i++;
				}
				if (!string.IsNullOrEmpty(baseItem.description))
				{
					text = text + "\n[FF9900]" + baseItem.description;
				}
				UITooltip.Show(text);
				return;
			}
		}
		UITooltip.Hide();
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x000CF070 File Offset: 0x000CD270
	private void OnClick()
	{
		if (UIItemSlot.mDraggedItem != null)
		{
			this.OnDrop(null);
		}
		else if (this.mItem != null)
		{
			UIItemSlot.mDraggedItem = this.Replace(null);
			if (UIItemSlot.mDraggedItem != null)
			{
				NGUITools.PlaySound(this.grabSound);
			}
			this.UpdateCursor();
		}
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x00010B0D File Offset: 0x0000ED0D
	private void OnDrag(Vector2 delta)
	{
		if (UIItemSlot.mDraggedItem == null && this.mItem != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UIItemSlot.mDraggedItem = this.Replace(null);
			NGUITools.PlaySound(this.grabSound);
			this.UpdateCursor();
		}
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x000CF0C8 File Offset: 0x000CD2C8
	private void OnDrop(GameObject go)
	{
		InvGameItem invGameItem = this.Replace(UIItemSlot.mDraggedItem);
		if (UIItemSlot.mDraggedItem == invGameItem)
		{
			NGUITools.PlaySound(this.errorSound);
		}
		else if (invGameItem != null)
		{
			NGUITools.PlaySound(this.grabSound);
		}
		else
		{
			NGUITools.PlaySound(this.placeSound);
		}
		UIItemSlot.mDraggedItem = invGameItem;
		this.UpdateCursor();
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x000CF12C File Offset: 0x000CD32C
	private void UpdateCursor()
	{
		if (UIItemSlot.mDraggedItem != null && UIItemSlot.mDraggedItem.baseItem != null)
		{
			UICursor.Set(UIItemSlot.mDraggedItem.baseItem.iconAtlas, UIItemSlot.mDraggedItem.baseItem.iconName);
		}
		else
		{
			UICursor.Clear();
		}
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x000CF180 File Offset: 0x000CD380
	private void Update()
	{
		InvGameItem observedItem = this.observedItem;
		if (this.mItem != observedItem)
		{
			this.mItem = observedItem;
			InvBaseItem invBaseItem = (observedItem == null) ? null : observedItem.baseItem;
			if (this.label != null)
			{
				string text = (observedItem == null) ? null : observedItem.name;
				if (string.IsNullOrEmpty(this.mText))
				{
					this.mText = this.label.text;
				}
				this.label.text = ((text == null) ? this.mText : text);
			}
			if (this.icon != null)
			{
				if (invBaseItem == null || invBaseItem.iconAtlas == null)
				{
					this.icon.enabled = false;
				}
				else
				{
					this.icon.atlas = invBaseItem.iconAtlas;
					this.icon.spriteName = invBaseItem.iconName;
					this.icon.enabled = true;
					this.icon.MakePixelPerfect();
				}
			}
			if (this.background != null)
			{
				this.background.color = ((observedItem == null) ? Color.white : observedItem.color);
			}
		}
	}

	// Token: 0x04001E4F RID: 7759
	public UISprite icon;

	// Token: 0x04001E50 RID: 7760
	public UIWidget background;

	// Token: 0x04001E51 RID: 7761
	public UILabel label;

	// Token: 0x04001E52 RID: 7762
	public AudioClip grabSound;

	// Token: 0x04001E53 RID: 7763
	public AudioClip placeSound;

	// Token: 0x04001E54 RID: 7764
	public AudioClip errorSound;

	// Token: 0x04001E55 RID: 7765
	private InvGameItem mItem;

	// Token: 0x04001E56 RID: 7766
	private string mText = string.Empty;

	// Token: 0x04001E57 RID: 7767
	private static InvGameItem mDraggedItem;
}
