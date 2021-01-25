using System;
using UnityEngine;

// Token: 0x020004C8 RID: 1224
[ExecuteInEditMode]
public class AnimatedWidget : MonoBehaviour
{
	// Token: 0x06001E94 RID: 7828 RVA: 0x000143FC File Offset: 0x000125FC
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.LateUpdate();
	}

	// Token: 0x06001E95 RID: 7829 RVA: 0x00014410 File Offset: 0x00012610
	private void LateUpdate()
	{
		if (this.mWidget != null)
		{
			this.mWidget.width = Mathf.RoundToInt(this.width);
			this.mWidget.height = Mathf.RoundToInt(this.height);
		}
	}

	// Token: 0x04002268 RID: 8808
	public float width = 1f;

	// Token: 0x04002269 RID: 8809
	public float height = 1f;

	// Token: 0x0400226A RID: 8810
	private UIWidget mWidget;
}
