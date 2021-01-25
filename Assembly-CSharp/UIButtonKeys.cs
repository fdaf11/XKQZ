using System;
using UnityEngine;

// Token: 0x02000448 RID: 1096
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Button Keys (Legacy)")]
public class UIButtonKeys : UIKeyNavigation
{
	// Token: 0x06001A57 RID: 6743 RVA: 0x000113C0 File Offset: 0x0000F5C0
	protected override void OnEnable()
	{
		this.Upgrade();
		base.OnEnable();
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x000D2210 File Offset: 0x000D0410
	public void Upgrade()
	{
		if (this.onClick == null && this.selectOnClick != null)
		{
			this.onClick = this.selectOnClick.gameObject;
			this.selectOnClick = null;
			NGUITools.SetDirty(this);
		}
		if (this.onLeft == null && this.selectOnLeft != null)
		{
			this.onLeft = this.selectOnLeft.gameObject;
			this.selectOnLeft = null;
			NGUITools.SetDirty(this);
		}
		if (this.onRight == null && this.selectOnRight != null)
		{
			this.onRight = this.selectOnRight.gameObject;
			this.selectOnRight = null;
			NGUITools.SetDirty(this);
		}
		if (this.onUp == null && this.selectOnUp != null)
		{
			this.onUp = this.selectOnUp.gameObject;
			this.selectOnUp = null;
			NGUITools.SetDirty(this);
		}
		if (this.onDown == null && this.selectOnDown != null)
		{
			this.onDown = this.selectOnDown.gameObject;
			this.selectOnDown = null;
			NGUITools.SetDirty(this);
		}
	}

	// Token: 0x04001F20 RID: 7968
	public UIButtonKeys selectOnClick;

	// Token: 0x04001F21 RID: 7969
	public UIButtonKeys selectOnUp;

	// Token: 0x04001F22 RID: 7970
	public UIButtonKeys selectOnDown;

	// Token: 0x04001F23 RID: 7971
	public UIButtonKeys selectOnLeft;

	// Token: 0x04001F24 RID: 7972
	public UIButtonKeys selectOnRight;
}
