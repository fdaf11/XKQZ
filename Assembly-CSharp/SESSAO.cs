using System;
using UnityEngine;

// Token: 0x0200069D RID: 1693
[AddComponentMenu("Image Effects/Sonic Ether/SESSAO")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class SESSAO : MonoBehaviour
{
	// Token: 0x06002911 RID: 10513 RVA: 0x0001B12A File Offset: 0x0001932A
	private void CheckInit()
	{
		if (this.initChecker == null)
		{
			this.Init();
		}
	}

	// Token: 0x06002912 RID: 10514 RVA: 0x00145EFC File Offset: 0x001440FC
	private void Init()
	{
		this.skipThisFrame = false;
		Shader shader = Shader.Find("Hidden/SESSAO");
		if (!shader)
		{
			this.skipThisFrame = true;
			return;
		}
		this.material = new Material(shader);
		this.attachedCamera = base.GetComponent<Camera>();
		this.attachedCamera.depthTextureMode |= 1;
		this.attachedCamera.depthTextureMode |= 2;
		this.SetupDitherTexture();
		this.SetupDitherTextureSmall();
		this.initChecker = new object();
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x0001B13D File Offset: 0x0001933D
	private void Cleanup()
	{
		Object.DestroyImmediate(this.material);
		this.initChecker = null;
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x00145F84 File Offset: 0x00144184
	private void SetupDitherTextureSmall()
	{
		this.ditherTextureSmall = new Texture2D(3, 3, 1, false);
		this.ditherTextureSmall.filterMode = 0;
		float[] array = new float[]
		{
			8f,
			1f,
			6f,
			3f,
			0f,
			4f,
			7f,
			2f,
			5f
		};
		for (int i = 0; i < 9; i++)
		{
			Color color;
			color..ctor(0f, 0f, 0f, array[i] / 9f);
			int num = i % 3;
			int num2 = Mathf.FloorToInt((float)i / 3f);
			this.ditherTextureSmall.SetPixel(num, num2, color);
		}
		this.ditherTextureSmall.Apply();
		this.ditherTextureSmall.hideFlags = 13;
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x0014602C File Offset: 0x0014422C
	private void SetupDitherTexture()
	{
		this.ditherTexture = new Texture2D(5, 5, 1, false);
		this.ditherTexture.filterMode = 0;
		float[] array = new float[]
		{
			12f,
			1f,
			10f,
			3f,
			20f,
			5f,
			18f,
			7f,
			16f,
			9f,
			24f,
			2f,
			11f,
			6f,
			22f,
			15f,
			8f,
			0f,
			13f,
			19f,
			4f,
			21f,
			14f,
			23f,
			17f
		};
		for (int i = 0; i < 25; i++)
		{
			Color color;
			color..ctor(0f, 0f, 0f, array[i] / 25f);
			int num = i % 5;
			int num2 = Mathf.FloorToInt((float)i / 5f);
			this.ditherTexture.SetPixel(num, num2, color);
		}
		this.ditherTexture.Apply();
		this.ditherTexture.hideFlags = 13;
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x0001B151 File Offset: 0x00019351
	private void Start()
	{
		this.CheckInit();
	}

	// Token: 0x06002917 RID: 10519 RVA: 0x0001B151 File Offset: 0x00019351
	private void OnEnable()
	{
		this.CheckInit();
	}

	// Token: 0x06002918 RID: 10520 RVA: 0x0001B159 File Offset: 0x00019359
	private void OnDisable()
	{
		this.Cleanup();
	}

	// Token: 0x06002919 RID: 10521 RVA: 0x001460D4 File Offset: 0x001442D4
	private void Update()
	{
		this.drawDistance = Mathf.Max(0f, this.drawDistance);
		this.drawDistanceFadeSize = Mathf.Max(0.001f, this.drawDistanceFadeSize);
		this.bilateralDepthTolerance = Mathf.Max(1E-06f, this.bilateralDepthTolerance);
	}

	// Token: 0x0600291A RID: 10522 RVA: 0x00146124 File Offset: 0x00144324
	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.CheckInit();
		if (this.skipThisFrame)
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.material.hideFlags = 13;
		this.material.SetTexture("_DitherTexture", (!this.preserveDetails) ? this.ditherTexture : this.ditherTextureSmall);
		this.material.SetInt("PreserveDetails", (!this.preserveDetails) ? 0 : 1);
		this.material.SetMatrix("ProjectionMatrixInverse", base.GetComponent<Camera>().projectionMatrix.inverse);
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, 2);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, 2);
		RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
		temporary3.wrapMode = 1;
		temporary3.filterMode = 1;
		Graphics.Blit(source, temporary3);
		this.material.SetTexture("_ColorDownsampled", temporary3);
		RenderTexture renderTexture = null;
		this.material.SetFloat("Radius", this.radius);
		this.material.SetFloat("Bias", this.bias);
		this.material.SetFloat("DepthTolerance", this.bilateralDepthTolerance);
		this.material.SetFloat("ZThickness", this.zThickness);
		this.material.SetFloat("Intensity", this.occlusionIntensity);
		this.material.SetFloat("SampleDistributionCurve", this.sampleDistributionCurve);
		this.material.SetFloat("ColorBleedAmount", this.colorBleedAmount);
		this.material.SetFloat("DrawDistance", this.drawDistance);
		this.material.SetFloat("DrawDistanceFadeSize", this.drawDistanceFadeSize);
		this.material.SetFloat("SelfBleedReduction", (!this.reduceSelfBleeding) ? 0f : 1f);
		this.material.SetFloat("BrightnessThreshold", this.brightnessThreshold);
		this.material.SetInt("HalfSampling", (!this.halfSampling) ? 0 : 1);
		this.material.SetInt("Orthographic", (!this.attachedCamera.orthographic) ? 0 : 1);
		if (this.useDownsampling)
		{
			renderTexture = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, 2);
			renderTexture.filterMode = 1;
			this.material.SetInt("Downsamp", 1);
			Graphics.Blit(source, renderTexture, this.material, (this.colorBleedAmount > 0.0001f) ? 0 : 1);
		}
		else
		{
			this.material.SetInt("Downsamp", 0);
			Graphics.Blit(source, temporary, this.material, (this.colorBleedAmount > 0.0001f) ? 0 : 1);
		}
		RenderTexture.ReleaseTemporary(temporary3);
		this.material.SetFloat("BlurDepthTolerance", 0.1f);
		int num = (!this.attachedCamera.orthographic) ? 2 : 6;
		if (this.attachedCamera.orthographic)
		{
			this.material.SetFloat("Near", this.attachedCamera.nearClipPlane);
			this.material.SetFloat("Far", this.attachedCamera.farClipPlane);
		}
		if (this.useDownsampling)
		{
			this.material.SetVector("Kernel", new Vector2(2f, 0f));
			Graphics.Blit(renderTexture, temporary2, this.material, num);
			RenderTexture.ReleaseTemporary(renderTexture);
			this.material.SetVector("Kernel", new Vector2(0f, 2f));
			Graphics.Blit(temporary2, temporary, this.material, num);
			this.material.SetVector("Kernel", new Vector2(2f, 0f));
			Graphics.Blit(temporary, temporary2, this.material, num);
			this.material.SetVector("Kernel", new Vector2(0f, 2f));
			Graphics.Blit(temporary2, temporary, this.material, num);
		}
		else
		{
			this.material.SetVector("Kernel", new Vector2(1f, 0f));
			Graphics.Blit(temporary, temporary2, this.material, num);
			this.material.SetVector("Kernel", new Vector2(0f, 1f));
			Graphics.Blit(temporary2, temporary, this.material, num);
			this.material.SetVector("Kernel", new Vector2(1f, 0f));
			Graphics.Blit(temporary, temporary2, this.material, num);
			this.material.SetVector("Kernel", new Vector2(0f, 1f));
			Graphics.Blit(temporary2, temporary, this.material, num);
		}
		RenderTexture.ReleaseTemporary(temporary2);
		this.material.SetTexture("_SSAO", temporary);
		if (!this.visualizeSSAO)
		{
			Graphics.Blit(source, destination, this.material, 3);
		}
		else
		{
			Graphics.Blit(source, destination, this.material, 5);
		}
		RenderTexture.ReleaseTemporary(temporary);
	}

	// Token: 0x040033FD RID: 13309
	private Material material;

	// Token: 0x040033FE RID: 13310
	public bool visualizeSSAO;

	// Token: 0x040033FF RID: 13311
	private Texture2D ditherTexture;

	// Token: 0x04003400 RID: 13312
	private Texture2D ditherTextureSmall;

	// Token: 0x04003401 RID: 13313
	private bool skipThisFrame;

	// Token: 0x04003402 RID: 13314
	[Range(0.02f, 5f)]
	public float radius = 1f;

	// Token: 0x04003403 RID: 13315
	[Range(-0.2f, 0.5f)]
	public float bias = 0.1f;

	// Token: 0x04003404 RID: 13316
	[Range(0.1f, 3f)]
	public float bilateralDepthTolerance = 0.2f;

	// Token: 0x04003405 RID: 13317
	[Range(1f, 5f)]
	public float zThickness = 2.35f;

	// Token: 0x04003406 RID: 13318
	[Range(0.5f, 5f)]
	public float occlusionIntensity = 1.3f;

	// Token: 0x04003407 RID: 13319
	[Range(1f, 6f)]
	public float sampleDistributionCurve = 1.15f;

	// Token: 0x04003408 RID: 13320
	[Range(0f, 1f)]
	public float colorBleedAmount = 1f;

	// Token: 0x04003409 RID: 13321
	[Range(0.1f, 3f)]
	public float brightnessThreshold;

	// Token: 0x0400340A RID: 13322
	public float drawDistance = 500f;

	// Token: 0x0400340B RID: 13323
	public float drawDistanceFadeSize = 1f;

	// Token: 0x0400340C RID: 13324
	public bool reduceSelfBleeding = true;

	// Token: 0x0400340D RID: 13325
	public bool useDownsampling;

	// Token: 0x0400340E RID: 13326
	public bool halfSampling;

	// Token: 0x0400340F RID: 13327
	public bool preserveDetails;

	// Token: 0x04003410 RID: 13328
	[HideInInspector]
	public Camera attachedCamera;

	// Token: 0x04003411 RID: 13329
	private object initChecker;
}
