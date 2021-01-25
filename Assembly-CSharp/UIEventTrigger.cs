using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200045C RID: 1116
[AddComponentMenu("NGUI/Interaction/Event Trigger")]
public class UIEventTrigger : MonoBehaviour
{
	// Token: 0x06001AC9 RID: 6857 RVA: 0x00011A46 File Offset: 0x0000FC46
	private void OnHover(bool isOver)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (isOver)
		{
			EventDelegate.Execute(this.onHoverOver);
		}
		else
		{
			EventDelegate.Execute(this.onHoverOut);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x00011A86 File Offset: 0x0000FC86
	private void OnPress(bool pressed)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (pressed)
		{
			EventDelegate.Execute(this.onPress);
		}
		else
		{
			EventDelegate.Execute(this.onRelease);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x00011AC6 File Offset: 0x0000FCC6
	private void OnSelect(bool selected)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		if (selected)
		{
			EventDelegate.Execute(this.onSelect);
		}
		else
		{
			EventDelegate.Execute(this.onDeselect);
		}
		UIEventTrigger.current = null;
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x00011B06 File Offset: 0x0000FD06
	private void OnClick()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onClick);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x00011B30 File Offset: 0x0000FD30
	private void OnDoubleClick()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDoubleClick);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x00011B5A File Offset: 0x0000FD5A
	private void OnDragStart()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragStart);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x00011B84 File Offset: 0x0000FD84
	private void OnDragEnd()
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragEnd);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x00011BAE File Offset: 0x0000FDAE
	private void OnDragOver(GameObject go)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOver);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x00011BD8 File Offset: 0x0000FDD8
	private void OnDragOut(GameObject go)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOut);
		UIEventTrigger.current = null;
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x00011BD8 File Offset: 0x0000FDD8
	private void OnDrag(Vector2 delta)
	{
		if (UIEventTrigger.current != null)
		{
			return;
		}
		UIEventTrigger.current = this;
		EventDelegate.Execute(this.onDragOut);
		UIEventTrigger.current = null;
	}

	// Token: 0x04001F9A RID: 8090
	public static UIEventTrigger current;

	// Token: 0x04001F9B RID: 8091
	public List<EventDelegate> onHoverOver = new List<EventDelegate>();

	// Token: 0x04001F9C RID: 8092
	public List<EventDelegate> onHoverOut = new List<EventDelegate>();

	// Token: 0x04001F9D RID: 8093
	public List<EventDelegate> onPress = new List<EventDelegate>();

	// Token: 0x04001F9E RID: 8094
	public List<EventDelegate> onRelease = new List<EventDelegate>();

	// Token: 0x04001F9F RID: 8095
	public List<EventDelegate> onSelect = new List<EventDelegate>();

	// Token: 0x04001FA0 RID: 8096
	public List<EventDelegate> onDeselect = new List<EventDelegate>();

	// Token: 0x04001FA1 RID: 8097
	public List<EventDelegate> onClick = new List<EventDelegate>();

	// Token: 0x04001FA2 RID: 8098
	public List<EventDelegate> onDoubleClick = new List<EventDelegate>();

	// Token: 0x04001FA3 RID: 8099
	public List<EventDelegate> onDragStart = new List<EventDelegate>();

	// Token: 0x04001FA4 RID: 8100
	public List<EventDelegate> onDragEnd = new List<EventDelegate>();

	// Token: 0x04001FA5 RID: 8101
	public List<EventDelegate> onDragOver = new List<EventDelegate>();

	// Token: 0x04001FA6 RID: 8102
	public List<EventDelegate> onDragOut = new List<EventDelegate>();

	// Token: 0x04001FA7 RID: 8103
	public List<EventDelegate> onDrag = new List<EventDelegate>();
}
