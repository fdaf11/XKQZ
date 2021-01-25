using System;
using UnityEngine;

// Token: 0x0200060D RID: 1549
public class BigMapHighlighting : MonoBehaviour
{
	// Token: 0x0600265D RID: 9821 RVA: 0x00019887 File Offset: 0x00017A87
	private void Start()
	{
		this.ho = base.transform.GetComponentInChildren<HighlightableObject>();
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x00129694 File Offset: 0x00127894
	private void OnMouseEnter()
	{
		Color color = new Color32(186, 206, 245, byte.MaxValue);
		this.ho.ConstantOn(color);
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x0001989A File Offset: 0x00017A9A
	private void OnMouseExit()
	{
		this.ho.ConstantOff();
	}

	// Token: 0x04002F29 RID: 12073
	private HighlightableObject ho;
}
