using System;
using UnityEngine;

// Token: 0x02000401 RID: 1025
[ExecuteInEditMode]
public class FXMakerGrayscaleEffect : FXMakerImageEffectBase
{
	// Token: 0x060018C3 RID: 6339 RVA: 0x000100E9 File Offset: 0x0000E2E9
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		base.material.SetTexture("_RampTex", this.textureRamp);
		base.material.SetFloat("_RampOffset", this.rampOffset);
		Graphics.Blit(source, destination, base.material);
	}

	// Token: 0x04001D24 RID: 7460
	public Texture textureRamp;

	// Token: 0x04001D25 RID: 7461
	public float rampOffset;
}
