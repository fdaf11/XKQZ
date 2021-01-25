using System;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
[ExecuteInEditMode]
public class AnimatedAlpha : MonoBehaviour
{
	// Token: 0x06001E8E RID: 7822 RVA: 0x00014384 File Offset: 0x00012584
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.mPanel = base.GetComponent<UIPanel>();
		this.LateUpdate();
	}

	// Token: 0x06001E8F RID: 7823 RVA: 0x000EC26C File Offset: 0x000EA46C
	private void LateUpdate()
	{
		if (this.mWidget != null)
		{
			this.mWidget.alpha = this.alpha;
		}
		if (this.mPanel != null)
		{
			this.mPanel.alpha = this.alpha;
		}
	}

	// Token: 0x04002263 RID: 8803
	[Range(0f, 1f)]
	public float alpha = 1f;

	// Token: 0x04002264 RID: 8804
	private UIWidget mWidget;

	// Token: 0x04002265 RID: 8805
	private UIPanel mPanel;
}
