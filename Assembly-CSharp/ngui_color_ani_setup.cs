using System;
using UnityEngine;

// Token: 0x020007FC RID: 2044
public class ngui_color_ani_setup : MonoBehaviour
{
	// Token: 0x0600322C RID: 12844 RVA: 0x0001F8B0 File Offset: 0x0001DAB0
	private void LateUpdate()
	{
		this.setupcolor();
	}

	// Token: 0x0600322D RID: 12845 RVA: 0x0001F8B8 File Offset: 0x0001DAB8
	private void OnDrawGizmos()
	{
		this.SetDisableAutoRecordMode();
		this.setupcolor();
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x001857AC File Offset: 0x001839AC
	private void setupcolor()
	{
		if (this.uis == null)
		{
			this.uis = base.gameObject.GetComponent<UISprite>();
		}
		this.uis.color = new Color(this.Red, this.Green, this.Blue, this.Alpha);
	}

	// Token: 0x0600322F RID: 12847 RVA: 0x0000264F File Offset: 0x0000084F
	private void SetDisableAutoRecordMode()
	{
	}

	// Token: 0x04003DEC RID: 15852
	public float Alpha = 1f;

	// Token: 0x04003DED RID: 15853
	public float Red = 1f;

	// Token: 0x04003DEE RID: 15854
	public float Green = 1f;

	// Token: 0x04003DEF RID: 15855
	public float Blue = 1f;

	// Token: 0x04003DF0 RID: 15856
	protected UISprite uis;
}
