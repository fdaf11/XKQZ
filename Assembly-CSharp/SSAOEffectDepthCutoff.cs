using System;
using UnityEngine;

// Token: 0x0200062C RID: 1580
[AddComponentMenu("Image Effects/Screen Space Ambient Occlusion")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class SSAOEffectDepthCutoff : MonoBehaviour
{
	// Token: 0x06002718 RID: 10008 RVA: 0x0012ED20 File Offset: 0x0012CF20
	private static Material CreateMaterial(Shader shader)
	{
		if (!shader)
		{
			return null;
		}
		return new Material(shader)
		{
			hideFlags = 13
		};
	}

	// Token: 0x06002719 RID: 10009 RVA: 0x00019D00 File Offset: 0x00017F00
	private static void DestroyMaterial(Material mat)
	{
		if (mat)
		{
			Object.DestroyImmediate(mat);
			mat = null;
		}
	}

	// Token: 0x0600271A RID: 10010 RVA: 0x00019D16 File Offset: 0x00017F16
	private void OnDisable()
	{
		SSAOEffectDepthCutoff.DestroyMaterial(this.m_SSAOMaterial);
	}

	// Token: 0x0600271B RID: 10011 RVA: 0x0012ED4C File Offset: 0x0012CF4C
	private void Start()
	{
		if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(1))
		{
			this.m_Supported = false;
			base.enabled = false;
			return;
		}
		this.CreateMaterials();
		if (!this.m_SSAOMaterial || this.m_SSAOMaterial.passCount != 5)
		{
			this.m_Supported = false;
			base.enabled = false;
			return;
		}
		this.m_Supported = true;
	}

	// Token: 0x0600271C RID: 10012 RVA: 0x00019D23 File Offset: 0x00017F23
	private void OnEnable()
	{
		base.camera.depthTextureMode |= 2;
	}

	// Token: 0x0600271D RID: 10013 RVA: 0x0012EDBC File Offset: 0x0012CFBC
	private void CreateMaterials()
	{
		if (!this.m_SSAOMaterial && this.m_SSAOShader.isSupported)
		{
			this.m_SSAOMaterial = SSAOEffectDepthCutoff.CreateMaterial(this.m_SSAOShader);
			this.m_SSAOMaterial.SetTexture("_RandomTexture", this.m_RandomTexture);
		}
	}

	// Token: 0x0600271E RID: 10014 RVA: 0x0012EE10 File Offset: 0x0012D010
	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!this.m_Supported || !this.m_SSAOShader.isSupported)
		{
			base.enabled = false;
			return;
		}
		this.CreateMaterials();
		this.m_Downsampling = Mathf.Clamp(this.m_Downsampling, 1, 6);
		this.m_Radius = Mathf.Clamp(this.m_Radius, 0.05f, 1f);
		this.m_MinZ = Mathf.Clamp(this.m_MinZ, 1E-05f, 0.5f);
		this.m_OcclusionIntensity = Mathf.Clamp(this.m_OcclusionIntensity, 0.5f, 4f);
		this.m_OcclusionAttenuation = Mathf.Clamp(this.m_OcclusionAttenuation, 0.2f, 2f);
		this.m_Blur = Mathf.Clamp(this.m_Blur, 0, 4);
		RenderTexture renderTexture = RenderTexture.GetTemporary(source.width / this.m_Downsampling, source.height / this.m_Downsampling, 0);
		float fieldOfView = base.camera.fieldOfView;
		float farClipPlane = base.camera.farClipPlane;
		float num = Mathf.Tan(fieldOfView * 0.017453292f * 0.5f) * farClipPlane;
		float num2 = num * base.camera.aspect;
		this.m_SSAOMaterial.SetVector("_FarCorner", new Vector3(num2, num, farClipPlane));
		int num3;
		int num4;
		if (this.m_RandomTexture)
		{
			num3 = this.m_RandomTexture.width;
			num4 = this.m_RandomTexture.height;
		}
		else
		{
			num3 = 1;
			num4 = 1;
		}
		this.m_SSAOMaterial.SetVector("_NoiseScale", new Vector3((float)renderTexture.width / (float)num3, (float)renderTexture.height / (float)num4, 0f));
		this.m_SSAOMaterial.SetVector("_Params", new Vector4(this.m_Radius, this.m_MinZ, 1f / this.m_OcclusionAttenuation, this.m_OcclusionIntensity));
		this.m_SSAOMaterial.SetFloat("_DepthCutoff", this.m_DepthCutoff);
		bool flag = this.m_Blur > 0;
		Graphics.Blit((!flag) ? source : null, renderTexture, this.m_SSAOMaterial, (int)this.m_SampleCount);
		if (flag)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
			this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4((float)this.m_Blur / (float)source.width, 0f, 0f, 0f));
			this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
			Graphics.Blit(null, temporary, this.m_SSAOMaterial, 3);
			RenderTexture.ReleaseTemporary(renderTexture);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
			this.m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0f, (float)this.m_Blur / (float)source.height, 0f, 0f));
			this.m_SSAOMaterial.SetTexture("_SSAO", temporary);
			Graphics.Blit(source, temporary2, this.m_SSAOMaterial, 3);
			RenderTexture.ReleaseTemporary(temporary);
			renderTexture = temporary2;
		}
		this.m_SSAOMaterial.SetTexture("_SSAO", renderTexture);
		Graphics.Blit(source, destination, this.m_SSAOMaterial, 4);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	// Token: 0x04003047 RID: 12359
	public float m_Radius = 0.4f;

	// Token: 0x04003048 RID: 12360
	public SSAOEffectDepthCutoff.SSAOSamples m_SampleCount = SSAOEffectDepthCutoff.SSAOSamples.Medium;

	// Token: 0x04003049 RID: 12361
	public float m_OcclusionIntensity = 1.5f;

	// Token: 0x0400304A RID: 12362
	public int m_Blur = 2;

	// Token: 0x0400304B RID: 12363
	public int m_Downsampling = 2;

	// Token: 0x0400304C RID: 12364
	public float m_OcclusionAttenuation = 1f;

	// Token: 0x0400304D RID: 12365
	public float m_MinZ = 0.01f;

	// Token: 0x0400304E RID: 12366
	public float m_DepthCutoff = 50f;

	// Token: 0x0400304F RID: 12367
	public Shader m_SSAOShader;

	// Token: 0x04003050 RID: 12368
	private Material m_SSAOMaterial;

	// Token: 0x04003051 RID: 12369
	public Texture2D m_RandomTexture;

	// Token: 0x04003052 RID: 12370
	private bool m_Supported;

	// Token: 0x0200062D RID: 1581
	public enum SSAOSamples
	{
		// Token: 0x04003054 RID: 12372
		Low,
		// Token: 0x04003055 RID: 12373
		Medium,
		// Token: 0x04003056 RID: 12374
		High
	}
}
