using System;
using UnityEngine;

// Token: 0x0200004F RID: 79
[AddComponentMenu("")]
public class AmplifyColorBase : MonoBehaviour
{
	// Token: 0x0600015E RID: 350 RVA: 0x0000323B File Offset: 0x0000143B
	public void BlendTo(Texture2D blendTargetLUT, float blendTimeInSec, Action onFinishBlend)
	{
		this.LutBlendTexture = blendTargetLUT;
		this.BlendAmount = 0f;
		this.onFinishBlend = onFinishBlend;
		this.blendingTime = blendTimeInSec;
		this.blendingTimeCountdown = blendTimeInSec;
		this.blending = true;
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00024B38 File Offset: 0x00022D38
	private bool CheckMaterialAndShader(Material material, string name)
	{
		if (material == null || material.shader == null)
		{
			Debug.LogError("[AmplifyColor] Error creating " + name + " material. Effect disabled.");
			base.enabled = false;
		}
		else if (!material.shader.isSupported)
		{
			Debug.LogError("[AmplifyColor] " + name + " shader not supported on this platform. Effect disabled.");
			base.enabled = false;
		}
		else
		{
			material.hideFlags = 13;
		}
		return base.enabled;
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000326B File Offset: 0x0000146B
	private bool CheckShader(Shader s)
	{
		if (s == null)
		{
			this.ReportMissingShaders();
			return false;
		}
		if (!s.isSupported)
		{
			this.ReportNotSupported();
			return false;
		}
		return true;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00024BC4 File Offset: 0x00022DC4
	private bool CheckShaders()
	{
		return this.CheckShader(this.shaderBase) && this.CheckShader(this.shaderBlend) && this.CheckShader(this.shaderBlendCache) && this.CheckShader(this.shaderMask) && this.CheckShader(this.shaderBlendMask);
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00003295 File Offset: 0x00001495
	private bool CheckSupport()
	{
		if (SystemInfo.supportsImageEffects && SystemInfo.supportsRenderTextures)
		{
			return true;
		}
		this.ReportNotSupported();
		return false;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00024C24 File Offset: 0x00022E24
	private void CreateHelperTextures()
	{
		int num = 1024;
		int num2 = 32;
		this.ReleaseTextures();
		RenderTexture renderTexture = new RenderTexture(num, num2, 0, 0, 1)
		{
			hideFlags = 13
		};
		this.blendCacheLut = renderTexture;
		this.blendCacheLut.name = "BlendCacheLut";
		this.blendCacheLut.wrapMode = 1;
		this.blendCacheLut.useMipMap = false;
		this.blendCacheLut.anisoLevel = 0;
		this.blendCacheLut.Create();
		Texture2D texture2D = new Texture2D(num, num2, 3, false, true)
		{
			hideFlags = 13
		};
		this.normalLut = texture2D;
		this.normalLut.name = "NormalLut";
		this.normalLut.hideFlags = 4;
		Color32[] array = new Color32[num * num2];
		for (int i = 0; i < 32; i++)
		{
			int num3 = i * 32;
			for (int j = 0; j < 32; j++)
			{
				int num4 = num3 + j * num;
				for (int k = 0; k < 32; k++)
				{
					float num5 = (float)k / 31f;
					float num6 = (float)j / 31f;
					float num7 = (float)i / 31f;
					byte b = (byte)(num5 * 255f);
					byte b2 = (byte)(num6 * 255f);
					byte b3 = (byte)(num7 * 255f);
					array[num4 + k] = new Color32(b, b2, b3, byte.MaxValue);
				}
			}
		}
		this.normalLut.SetPixels32(array);
		this.normalLut.Apply();
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00024DB4 File Offset: 0x00022FB4
	private void CreateMaterials()
	{
		this.SetupShader();
		this.ReleaseMaterials();
		this.materialBase = new Material(this.shaderBase);
		this.materialBlend = new Material(this.shaderBlend);
		this.materialBlendCache = new Material(this.shaderBlendCache);
		this.materialMask = new Material(this.shaderMask);
		this.materialBlendMask = new Material(this.shaderBlendMask);
		this.CheckMaterialAndShader(this.materialBase, "BaseMaterial");
		this.CheckMaterialAndShader(this.materialBlend, "BlendMaterial");
		this.CheckMaterialAndShader(this.materialBlendCache, "BlendCacheMaterial");
		this.CheckMaterialAndShader(this.materialMask, "MaskMaterial");
		this.CheckMaterialAndShader(this.materialBlendMask, "BlendMaskMaterial");
		if (base.enabled)
		{
			this.CreateHelperTextures();
		}
	}

	// Token: 0x06000165 RID: 357 RVA: 0x000032B4 File Offset: 0x000014B4
	private void OnDisable()
	{
		this.ReleaseMaterials();
		this.ReleaseTextures();
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00024E90 File Offset: 0x00023090
	private void OnEnable()
	{
		if (this.CheckSupport())
		{
			this.CreateMaterials();
			if ((this.LutTexture != null && this.LutTexture.mipmapCount > 1) || (this.LutBlendTexture != null && this.LutBlendTexture.mipmapCount > 1))
			{
				Debug.LogError("[AmplifyColor] Please disable \"Generate Mip Maps\" import settings on all LUT textures to avoid visual glitches. Change Texture Type to \"Advanced\" to access Mip settings.");
			}
		}
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00024EFC File Offset: 0x000230FC
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.BlendAmount = Mathf.Clamp01(this.BlendAmount);
		if (this.colorSpace != QualitySettings.activeColorSpace)
		{
			this.CreateMaterials();
		}
		bool flag = AmplifyColorBase.ValidateLutDimensions(this.LutTexture);
		bool flag2 = AmplifyColorBase.ValidateLutDimensions(this.LutBlendTexture);
		if (this.JustCopy || !flag || !flag2)
		{
			Graphics.Blit(source, destination);
		}
		else if ((this.LutTexture == null && this.lutTexture3d == null) || (this.BlendAmount == 1f && this.LutBlendTexture == null))
		{
			Graphics.Blit(source, destination);
		}
		else
		{
			int num = base.camera.hdr ? 1 : 0;
			bool flag3 = this.BlendAmount != 0f;
			bool flag4 = flag3 || (flag3 && (this.LutBlendTexture != null || this.lutBlendTexture3d != null));
			bool flag5 = flag4 && !this.use3d;
			Material material;
			if (flag4)
			{
				if (this.MaskTexture != null)
				{
					material = this.materialBlendMask;
				}
				else
				{
					material = this.materialBlend;
				}
			}
			else if (this.MaskTexture != null)
			{
				material = this.materialMask;
			}
			else
			{
				material = this.materialBase;
			}
			material.SetFloat("_lerpAmount", this.BlendAmount);
			if (this.MaskTexture != null)
			{
				material.SetTexture("_MaskTex", this.MaskTexture);
			}
			if (flag5)
			{
				this.materialBlendCache.SetFloat("_lerpAmount", this.BlendAmount);
				this.materialBlendCache.SetTexture("_RgbTex", this.LutTexture);
				this.materialBlendCache.SetTexture("_LerpRgbTex", (!(this.LutBlendTexture != null)) ? this.normalLut : this.LutBlendTexture);
				Graphics.Blit(this.LutTexture, this.blendCacheLut, this.materialBlendCache);
				material.SetTexture("_RgbBlendCacheTex", this.blendCacheLut);
			}
			else if (!this.use3d)
			{
				if (this.LutTexture != null)
				{
					material.SetTexture("_RgbTex", this.LutTexture);
				}
				if (this.LutBlendTexture != null)
				{
					material.SetTexture("_LerpRgbTex", this.LutBlendTexture);
				}
			}
			Graphics.Blit(source, destination, material, num);
			if (flag5)
			{
				this.blendCacheLut.DiscardContents();
			}
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x000251B4 File Offset: 0x000233B4
	private void ReleaseMaterials()
	{
		if (this.materialBase != null)
		{
			Object.DestroyImmediate(this.materialBase);
			this.materialBase = null;
		}
		if (this.materialBlend != null)
		{
			Object.DestroyImmediate(this.materialBlend);
			this.materialBlend = null;
		}
		if (this.materialBlendCache != null)
		{
			Object.DestroyImmediate(this.materialBlendCache);
			this.materialBlendCache = null;
		}
		if (this.materialMask != null)
		{
			Object.DestroyImmediate(this.materialMask);
			this.materialMask = null;
		}
		if (this.materialBlendMask != null)
		{
			Object.DestroyImmediate(this.materialBlendMask);
			this.materialBlendMask = null;
		}
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00025270 File Offset: 0x00023470
	private void ReleaseTextures()
	{
		if (this.blendCacheLut != null)
		{
			Object.DestroyImmediate(this.blendCacheLut);
			this.blendCacheLut = null;
		}
		if (this.normalLut != null)
		{
			Object.DestroyImmediate(this.normalLut);
			this.normalLut = null;
		}
	}

	// Token: 0x0600016A RID: 362 RVA: 0x000032C2 File Offset: 0x000014C2
	private void ReportMissingShaders()
	{
		Debug.LogError("[AmplifyColor] Error initializing shaders. Please reinstall Amplify Color.");
		base.enabled = false;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x000032D5 File Offset: 0x000014D5
	private void ReportNotSupported()
	{
		Debug.LogError("[AmplifyColor] This image effect is not supported on this platform. Please make sure your Unity license supports Full-Screen Post-Processing Effects which is usually reserved forn Pro licenses.");
		base.enabled = false;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x000252C4 File Offset: 0x000234C4
	private void SetupShader()
	{
		this.colorSpace = QualitySettings.activeColorSpace;
		string text = (this.colorSpace != 1) ? string.Empty : "Linear";
		string empty = string.Empty;
		this.shaderBase = Shader.Find("Hidden/Amplify Color/Base" + text + empty);
		if (this.shaderBase == null)
		{
			Debug.Log("Shader Lost Hidden/Amplify Color/Base" + text + empty);
		}
		this.shaderBlend = Shader.Find("Hidden/Amplify Color/Blend" + text + empty);
		if (this.shaderBlend == null)
		{
			Debug.Log("Shader Lost Hidden/Amplify Color/Blend" + text + empty);
		}
		this.shaderBlendCache = Shader.Find("Hidden/Amplify Color/BlendCache");
		if (this.shaderBlendCache == null)
		{
			Debug.Log("Shader Lost Hidden/Amplify Color/BlendCache" + text + empty);
		}
		this.shaderMask = Shader.Find("Hidden/Amplify Color/Mask" + text + empty);
		if (this.shaderMask == null)
		{
			Debug.Log("Shader Lost Hidden/Amplify Color/Mask" + text + empty);
		}
		this.shaderBlendMask = Shader.Find("Hidden/Amplify Color/BlendMask" + text + empty);
		if (this.shaderBlendMask == null)
		{
			Debug.Log("Shader Lost Hidden/Amplify Color/BlendMask" + text + empty);
		}
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00025414 File Offset: 0x00023614
	private void Update()
	{
		if (this.blending)
		{
			this.BlendAmount = (this.blendingTime - this.blendingTimeCountdown) / this.blendingTime;
			this.blendingTimeCountdown -= Time.smoothDeltaTime;
			if (this.BlendAmount >= 1f)
			{
				this.LutTexture = this.LutBlendTexture;
				this.BlendAmount = 0f;
				this.blending = false;
				this.LutBlendTexture = null;
				if (this.onFinishBlend != null)
				{
					this.onFinishBlend.Invoke();
				}
			}
		}
		else
		{
			this.BlendAmount = Mathf.Clamp01(this.BlendAmount);
		}
	}

	// Token: 0x0600016E RID: 366 RVA: 0x000254BC File Offset: 0x000236BC
	public static bool ValidateLutDimensions(Texture2D lut)
	{
		if (lut != null)
		{
			if (lut.width / lut.height != lut.height)
			{
				Debug.LogWarning("[AmplifyColor] Lut " + lut.name + " has invalid dimensions.");
				return false;
			}
			if (lut.anisoLevel != 0)
			{
				lut.anisoLevel = 0;
			}
		}
		return true;
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600016F RID: 367 RVA: 0x000032E8 File Offset: 0x000014E8
	public bool IsBlending
	{
		get
		{
			return this.blending;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000170 RID: 368 RVA: 0x000032F0 File Offset: 0x000014F0
	public bool WillItBlend
	{
		get
		{
			return this.LutTexture != null && this.LutBlendTexture != null && !this.blending;
		}
	}

	// Token: 0x04000111 RID: 273
	public float BlendAmount;

	// Token: 0x04000112 RID: 274
	private RenderTexture blendCacheLut;

	// Token: 0x04000113 RID: 275
	private bool blending;

	// Token: 0x04000114 RID: 276
	private float blendingTime;

	// Token: 0x04000115 RID: 277
	private float blendingTimeCountdown;

	// Token: 0x04000116 RID: 278
	private ColorSpace colorSpace = -1;

	// Token: 0x04000117 RID: 279
	internal bool JustCopy;

	// Token: 0x04000118 RID: 280
	public Texture2D LutTexture;

	// Token: 0x04000119 RID: 281
	private Texture lutTexture3d = new Texture();

	// Token: 0x0400011A RID: 282
	public Texture2D LutBlendTexture;

	// Token: 0x0400011B RID: 283
	private Texture lutBlendTexture3d = new Texture();

	// Token: 0x0400011C RID: 284
	public Texture MaskTexture;

	// Token: 0x0400011D RID: 285
	private Material materialBase;

	// Token: 0x0400011E RID: 286
	private Material materialBlend;

	// Token: 0x0400011F RID: 287
	private Material materialBlendCache;

	// Token: 0x04000120 RID: 288
	private Material materialBlendMask;

	// Token: 0x04000121 RID: 289
	private Material materialMask;

	// Token: 0x04000122 RID: 290
	private Texture2D normalLut;

	// Token: 0x04000123 RID: 291
	private Action onFinishBlend;

	// Token: 0x04000124 RID: 292
	private Shader shaderBase;

	// Token: 0x04000125 RID: 293
	private Shader shaderBlend;

	// Token: 0x04000126 RID: 294
	private Shader shaderBlendCache;

	// Token: 0x04000127 RID: 295
	private Shader shaderBlendMask;

	// Token: 0x04000128 RID: 296
	private Shader shaderMask;

	// Token: 0x04000129 RID: 297
	private bool use3d;
}
