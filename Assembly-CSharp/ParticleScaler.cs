using System;
using UnityEngine;

// Token: 0x02000629 RID: 1577
[ExecuteInEditMode]
public class ParticleScaler : MonoBehaviour
{
	// Token: 0x0600270A RID: 9994 RVA: 0x00019C97 File Offset: 0x00017E97
	private void Start()
	{
		this.prevScale = this.particleScale;
	}

	// Token: 0x0600270B RID: 9995 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x0600270C RID: 9996 RVA: 0x0000264F File Offset: 0x0000084F
	private void ScaleShurikenSystems(float scaleFactor)
	{
	}

	// Token: 0x0600270D RID: 9997 RVA: 0x0000264F File Offset: 0x0000084F
	private void ScaleLegacySystems(float scaleFactor)
	{
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x0012E11C File Offset: 0x0012C31C
	private void ScaleTrailRenderers(float scaleFactor)
	{
		TrailRenderer[] componentsInChildren = base.GetComponentsInChildren<TrailRenderer>();
		foreach (TrailRenderer trailRenderer in componentsInChildren)
		{
			trailRenderer.startWidth *= scaleFactor;
			trailRenderer.endWidth *= scaleFactor;
		}
	}

	// Token: 0x04003035 RID: 12341
	public float particleScale = 1f;

	// Token: 0x04003036 RID: 12342
	public bool alsoScaleGameobject = true;

	// Token: 0x04003037 RID: 12343
	private float prevScale;
}
