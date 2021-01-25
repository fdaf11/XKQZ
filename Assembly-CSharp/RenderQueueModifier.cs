using System;
using UnityEngine;

// Token: 0x02000390 RID: 912
public class RenderQueueModifier : MonoBehaviour
{
	// Token: 0x06001530 RID: 5424 RVA: 0x0000D798 File Offset: 0x0000B998
	private void Start()
	{
		this._renderers = base.GetComponentsInChildren<Renderer>();
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x000B5EE0 File Offset: 0x000B40E0
	private void FixedUpdate()
	{
		int num;
		if (this.m_target == null || this.m_target.drawCall == null)
		{
			num = this.renderQueue;
		}
		else
		{
			this.renderQueue = this.m_target.drawCall.renderQueue;
			num = this.renderQueue;
		}
		num += ((this.m_type != RenderQueueModifier.RenderType.FRONT) ? -1 : 1);
		if (this._lastQueue != num)
		{
			this._lastQueue = num;
			foreach (Renderer renderer in this._renderers)
			{
				renderer.material.renderQueue = this._lastQueue;
			}
		}
	}

	// Token: 0x040019E1 RID: 6625
	public UIWidget m_target;

	// Token: 0x040019E2 RID: 6626
	public RenderQueueModifier.RenderType m_type;

	// Token: 0x040019E3 RID: 6627
	public int renderQueue;

	// Token: 0x040019E4 RID: 6628
	private Renderer[] _renderers;

	// Token: 0x040019E5 RID: 6629
	private int _lastQueue;

	// Token: 0x02000391 RID: 913
	public enum RenderType
	{
		// Token: 0x040019E7 RID: 6631
		FRONT,
		// Token: 0x040019E8 RID: 6632
		BACK
	}
}
