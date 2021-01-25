using System;
using UnityEngine;

// Token: 0x02000836 RID: 2102
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Time of Day/Camera Sun Shafts")]
internal class TOD_SunShafts : TOD_PostEffectsBase
{
	// Token: 0x06003347 RID: 13127 RVA: 0x00020239 File Offset: 0x0001E439
	protected void OnDisable()
	{
		if (this.sunShaftsMaterial)
		{
			Object.DestroyImmediate(this.sunShaftsMaterial);
		}
		if (this.screenClearMaterial)
		{
			Object.DestroyImmediate(this.screenClearMaterial);
		}
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x00020271 File Offset: 0x0001E471
	protected override bool CheckResources()
	{
		this.sunShaftsMaterial = base.CheckShaderAndCreateMaterial(this.SunShaftsShader, this.sunShaftsMaterial);
		this.screenClearMaterial = base.CheckShaderAndCreateMaterial(this.ScreenClearShader, this.screenClearMaterial);
		return base.CheckSupport(this.UseDepthTexture, false);
	}

	// Token: 0x06003349 RID: 13129 RVA: 0x0018C69C File Offset: 0x0018A89C
	protected void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.sky.Components.SunShafts = this;
		int num;
		int num2;
		int num3;
		if (this.Resolution == TOD_SunShafts.SunShaftsResolution.High)
		{
			num = source.width;
			num2 = source.height;
			num3 = 0;
		}
		else if (this.Resolution == TOD_SunShafts.SunShaftsResolution.Normal)
		{
			num = source.width / 2;
			num2 = source.height / 2;
			num3 = 0;
		}
		else
		{
			num = source.width / 4;
			num2 = source.height / 4;
			num3 = 0;
		}
		Vector3 vector = this.cam.WorldToViewportPoint(this.sky.Components.SunTransform.position);
		this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * this.SunShaftBlurRadius);
		this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.MaxRadius));
		RenderTexture temporary = RenderTexture.GetTemporary(num, num2, num3);
		if (this.UseDepthTexture)
		{
			Graphics.Blit(source, temporary, this.sunShaftsMaterial, 2);
		}
		else
		{
			Graphics.Blit(source, temporary, this.sunShaftsMaterial, 3);
		}
		base.DrawBorder(temporary, this.screenClearMaterial);
		float num4 = this.SunShaftBlurRadius * 0.0013020834f;
		this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num4, num4, 0f, 0f));
		this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.MaxRadius));
		for (int i = 0; i < this.RadialBlurIterations; i++)
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(num, num2, num3);
			Graphics.Blit(temporary, temporary2, this.sunShaftsMaterial, 1);
			RenderTexture.ReleaseTemporary(temporary);
			num4 = this.SunShaftBlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num4, num4, 0f, 0f));
			temporary = RenderTexture.GetTemporary(num, num2, num3);
			Graphics.Blit(temporary2, temporary, this.sunShaftsMaterial, 1);
			RenderTexture.ReleaseTemporary(temporary2);
			num4 = this.SunShaftBlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num4, num4, 0f, 0f));
		}
		Vector4 vector2 = ((double)vector.z < 0.0) ? Vector4.zero : ((1f - this.sky.Atmosphere.Fogginess) * this.SunShaftIntensity * this.sky.SunShaftColor);
		this.sunShaftsMaterial.SetVector("_SunColor", vector2);
		this.sunShaftsMaterial.SetTexture("_ColorBuffer", temporary);
		if (this.BlendMode == TOD_SunShafts.SunShaftsBlendMode.Screen)
		{
			Graphics.Blit(source, destination, this.sunShaftsMaterial, 0);
		}
		else
		{
			Graphics.Blit(source, destination, this.sunShaftsMaterial, 4);
		}
		RenderTexture.ReleaseTemporary(temporary);
	}

	// Token: 0x04003F40 RID: 16192
	private const int PASS_DEPTH = 2;

	// Token: 0x04003F41 RID: 16193
	private const int PASS_NODEPTH = 3;

	// Token: 0x04003F42 RID: 16194
	private const int PASS_RADIAL = 1;

	// Token: 0x04003F43 RID: 16195
	private const int PASS_SCREEN = 0;

	// Token: 0x04003F44 RID: 16196
	private const int PASS_ADD = 4;

	// Token: 0x04003F45 RID: 16197
	public TOD_SunShafts.SunShaftsResolution Resolution = TOD_SunShafts.SunShaftsResolution.Normal;

	// Token: 0x04003F46 RID: 16198
	public TOD_SunShafts.SunShaftsBlendMode BlendMode;

	// Token: 0x04003F47 RID: 16199
	public int RadialBlurIterations = 2;

	// Token: 0x04003F48 RID: 16200
	public float SunShaftBlurRadius = 2f;

	// Token: 0x04003F49 RID: 16201
	public float SunShaftIntensity = 1f;

	// Token: 0x04003F4A RID: 16202
	public float MaxRadius = 0.5f;

	// Token: 0x04003F4B RID: 16203
	public bool UseDepthTexture = true;

	// Token: 0x04003F4C RID: 16204
	public Shader SunShaftsShader;

	// Token: 0x04003F4D RID: 16205
	public Shader ScreenClearShader;

	// Token: 0x04003F4E RID: 16206
	private Material sunShaftsMaterial;

	// Token: 0x04003F4F RID: 16207
	private Material screenClearMaterial;

	// Token: 0x02000837 RID: 2103
	public enum SunShaftsResolution
	{
		// Token: 0x04003F51 RID: 16209
		Low,
		// Token: 0x04003F52 RID: 16210
		Normal,
		// Token: 0x04003F53 RID: 16211
		High
	}

	// Token: 0x02000838 RID: 2104
	public enum SunShaftsBlendMode
	{
		// Token: 0x04003F55 RID: 16213
		Screen,
		// Token: 0x04003F56 RID: 16214
		Add
	}
}
