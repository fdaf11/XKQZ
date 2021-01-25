using System;
using UnityEngine;

// Token: 0x02000548 RID: 1352
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Edge Detection (Color)")]
public class EdgeDetectEffect : ImageEffectBase
{
	// Token: 0x06002248 RID: 8776 RVA: 0x00016E39 File Offset: 0x00015039
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetFloat("_Treshold", this.threshold * this.threshold);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04002876 RID: 10358
	public float threshold = 0.2f;
}
