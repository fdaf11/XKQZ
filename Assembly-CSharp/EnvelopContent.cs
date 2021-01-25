using System;
using UnityEngine;

// Token: 0x0200042E RID: 1070
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Examples/Envelop Content")]
public class EnvelopContent : MonoBehaviour
{
	// Token: 0x060019EA RID: 6634 RVA: 0x00010DAE File Offset: 0x0000EFAE
	private void Start()
	{
		this.mStarted = true;
		this.Execute();
	}

	// Token: 0x060019EB RID: 6635 RVA: 0x00010DBD File Offset: 0x0000EFBD
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.Execute();
		}
	}

	// Token: 0x060019EC RID: 6636 RVA: 0x000D00CC File Offset: 0x000CE2CC
	[ContextMenu("Execute")]
	public void Execute()
	{
		if (this.targetRoot == base.transform)
		{
			Debug.LogError("Target Root object cannot be the same object that has Envelop Content. Make it a sibling instead.", this);
		}
		else if (NGUITools.IsChild(this.targetRoot, base.transform))
		{
			Debug.LogError("Target Root object should not be a parent of Envelop Content. Make it a sibling instead.", this);
		}
		else
		{
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.transform.parent, this.targetRoot, false);
			float num = bounds.min.x + (float)this.padLeft;
			float num2 = bounds.min.y + (float)this.padBottom;
			float num3 = bounds.max.x + (float)this.padRight;
			float num4 = bounds.max.y + (float)this.padTop;
			UIWidget component = base.GetComponent<UIWidget>();
			component.SetRect(num, num2, num3 - num, num4 - num2);
			base.BroadcastMessage("UpdateAnchors", 1);
		}
	}

	// Token: 0x04001EAF RID: 7855
	public Transform targetRoot;

	// Token: 0x04001EB0 RID: 7856
	public int padLeft;

	// Token: 0x04001EB1 RID: 7857
	public int padRight;

	// Token: 0x04001EB2 RID: 7858
	public int padBottom;

	// Token: 0x04001EB3 RID: 7859
	public int padTop;

	// Token: 0x04001EB4 RID: 7860
	private bool mStarted;
}
