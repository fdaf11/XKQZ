using System;
using UnityEngine;

// Token: 0x020004C7 RID: 1223
[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
public class AnimatedColor : MonoBehaviour
{
	// Token: 0x06001E91 RID: 7825 RVA: 0x000143B7 File Offset: 0x000125B7
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.LateUpdate();
	}

	// Token: 0x06001E92 RID: 7826 RVA: 0x000143CB File Offset: 0x000125CB
	private void LateUpdate()
	{
		this.mWidget.color = this.color;
	}

	// Token: 0x04002266 RID: 8806
	public Color color = Color.white;

	// Token: 0x04002267 RID: 8807
	private UIWidget mWidget;
}
