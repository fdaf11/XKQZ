using System;
using UnityEngine;

// Token: 0x020005FC RID: 1532
public class FixShaderQueue : MonoBehaviour
{
	// Token: 0x060025E4 RID: 9700 RVA: 0x0012511C File Offset: 0x0012331C
	private void Start()
	{
		if (base.renderer != null)
		{
			base.renderer.sharedMaterial.renderQueue += this.AddQueue;
		}
		else
		{
			base.Invoke("SetProjectorQueue", 0.1f);
		}
	}

	// Token: 0x060025E5 RID: 9701 RVA: 0x000193D4 File Offset: 0x000175D4
	private void SetProjectorQueue()
	{
		base.GetComponent<Projector>().material.renderQueue += this.AddQueue;
	}

	// Token: 0x060025E6 RID: 9702 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04002E78 RID: 11896
	public int AddQueue = 1;
}
