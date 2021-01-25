using System;
using UnityEngine;

// Token: 0x02000449 RID: 1097
[AddComponentMenu("NGUI/Interaction/Button Message (Legacy)")]
public class UIButtonMessage : MonoBehaviour
{
	// Token: 0x06001A5A RID: 6746 RVA: 0x000113CE File Offset: 0x0000F5CE
	private void Start()
	{
		this.mStarted = true;
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x000113D7 File Offset: 0x0000F5D7
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x000113F5 File Offset: 0x0000F5F5
	private void OnHover(bool isOver)
	{
		if (base.enabled && ((isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOver) || (!isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOut)))
		{
			this.Send();
		}
	}

	// Token: 0x06001A5D RID: 6749 RVA: 0x0001142C File Offset: 0x0000F62C
	private void OnPress(bool isPressed)
	{
		if (base.enabled && ((isPressed && this.trigger == UIButtonMessage.Trigger.OnPress) || (!isPressed && this.trigger == UIButtonMessage.Trigger.OnRelease)))
		{
			this.Send();
		}
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x00011463 File Offset: 0x0000F663
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x00011488 File Offset: 0x0000F688
	private void OnClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnClick)
		{
			this.Send();
		}
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x000114A6 File Offset: 0x0000F6A6
	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnDoubleClick)
		{
			this.Send();
		}
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x000D2360 File Offset: 0x000D0560
	private void Send()
	{
		if (string.IsNullOrEmpty(this.functionName))
		{
			return;
		}
		if (this.target == null)
		{
			this.target = base.gameObject;
		}
		if (this.includeChildren)
		{
			Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				Transform transform = componentsInChildren[i];
				transform.gameObject.SendMessage(this.functionName, base.gameObject, 1);
				i++;
			}
		}
		else
		{
			this.target.SendMessage(this.functionName, base.gameObject, 1);
		}
	}

	// Token: 0x04001F25 RID: 7973
	public GameObject target;

	// Token: 0x04001F26 RID: 7974
	public string functionName;

	// Token: 0x04001F27 RID: 7975
	public UIButtonMessage.Trigger trigger;

	// Token: 0x04001F28 RID: 7976
	public bool includeChildren;

	// Token: 0x04001F29 RID: 7977
	private bool mStarted;

	// Token: 0x0200044A RID: 1098
	public enum Trigger
	{
		// Token: 0x04001F2B RID: 7979
		OnClick,
		// Token: 0x04001F2C RID: 7980
		OnMouseOver,
		// Token: 0x04001F2D RID: 7981
		OnMouseOut,
		// Token: 0x04001F2E RID: 7982
		OnPress,
		// Token: 0x04001F2F RID: 7983
		OnRelease,
		// Token: 0x04001F30 RID: 7984
		OnDoubleClick
	}
}
