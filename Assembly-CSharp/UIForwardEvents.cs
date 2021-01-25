using System;
using UnityEngine;

// Token: 0x0200045D RID: 1117
[AddComponentMenu("NGUI/Interaction/Forward Events (Legacy)")]
public class UIForwardEvents : MonoBehaviour
{
	// Token: 0x06001AD4 RID: 6868 RVA: 0x00011C02 File Offset: 0x0000FE02
	private void OnHover(bool isOver)
	{
		if (this.onHover && this.target != null)
		{
			this.target.SendMessage("OnHover", isOver, 1);
		}
	}

	// Token: 0x06001AD5 RID: 6869 RVA: 0x00011C37 File Offset: 0x0000FE37
	private void OnPress(bool pressed)
	{
		if (this.onPress && this.target != null)
		{
			this.target.SendMessage("OnPress", pressed, 1);
		}
	}

	// Token: 0x06001AD6 RID: 6870 RVA: 0x00011C6C File Offset: 0x0000FE6C
	private void OnClick()
	{
		if (this.onClick && this.target != null)
		{
			this.target.SendMessage("OnClick", 1);
		}
	}

	// Token: 0x06001AD7 RID: 6871 RVA: 0x00011C9B File Offset: 0x0000FE9B
	private void OnDoubleClick()
	{
		if (this.onDoubleClick && this.target != null)
		{
			this.target.SendMessage("OnDoubleClick", 1);
		}
	}

	// Token: 0x06001AD8 RID: 6872 RVA: 0x00011CCA File Offset: 0x0000FECA
	private void OnSelect(bool selected)
	{
		if (this.onSelect && this.target != null)
		{
			this.target.SendMessage("OnSelect", selected, 1);
		}
	}

	// Token: 0x06001AD9 RID: 6873 RVA: 0x00011CFF File Offset: 0x0000FEFF
	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag && this.target != null)
		{
			this.target.SendMessage("OnDrag", delta, 1);
		}
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x00011D34 File Offset: 0x0000FF34
	private void OnDrop(GameObject go)
	{
		if (this.onDrop && this.target != null)
		{
			this.target.SendMessage("OnDrop", go, 1);
		}
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x00011D64 File Offset: 0x0000FF64
	private void OnSubmit()
	{
		if (this.onSubmit && this.target != null)
		{
			this.target.SendMessage("OnSubmit", 1);
		}
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x00011D93 File Offset: 0x0000FF93
	private void OnScroll(float delta)
	{
		if (this.onScroll && this.target != null)
		{
			this.target.SendMessage("OnScroll", delta, 1);
		}
	}

	// Token: 0x04001FA8 RID: 8104
	public GameObject target;

	// Token: 0x04001FA9 RID: 8105
	public bool onHover;

	// Token: 0x04001FAA RID: 8106
	public bool onPress;

	// Token: 0x04001FAB RID: 8107
	public bool onClick;

	// Token: 0x04001FAC RID: 8108
	public bool onDoubleClick;

	// Token: 0x04001FAD RID: 8109
	public bool onSelect;

	// Token: 0x04001FAE RID: 8110
	public bool onDrag;

	// Token: 0x04001FAF RID: 8111
	public bool onDrop;

	// Token: 0x04001FB0 RID: 8112
	public bool onSubmit;

	// Token: 0x04001FB1 RID: 8113
	public bool onScroll;
}
