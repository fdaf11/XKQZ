using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000586 RID: 1414
	public class GlowPerObjEvent : CameraEffectEvent
	{
		// Token: 0x0600236D RID: 9069 RVA: 0x00115C7C File Offset: 0x00113E7C
		public GlowPerObjEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.GlowPerObj, owner)
		{
			this.ReplacementShader = owner.GlowPerObjReplacementShader;
			this.blendShader = owner.GlowPerObjBlendShader;
			this.downsampleShader = owner.GlowDownSampleShader;
			this.compositeShader = owner.GlowCompositeShader;
			this.blurShader = owner.GlowBlurShader;
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x0600236E RID: 9070 RVA: 0x00115CD0 File Offset: 0x00113ED0
		protected Camera ShaderCamera
		{
			get
			{
				if (this.m_shaderCamera == null)
				{
					this.m_shaderCamera = new GameObject("ShaderCamera", new Type[]
					{
						typeof(Camera)
					});
					this.m_shaderCamera.camera.enabled = false;
					this.m_shaderCamera.hideFlags = 13;
				}
				return this.m_shaderCamera.camera;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x0600236F RID: 9071 RVA: 0x00017840 File Offset: 0x00015A40
		protected Material compositeMaterial
		{
			get
			{
				if (this.m_CompositeMaterial == null)
				{
					this.m_CompositeMaterial = new Material(this.compositeShader);
					this.m_CompositeMaterial.hideFlags = 13;
				}
				return this.m_CompositeMaterial;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06002370 RID: 9072 RVA: 0x00017877 File Offset: 0x00015A77
		protected Material blurMaterial
		{
			get
			{
				if (this.m_BlurMaterial == null)
				{
					this.m_BlurMaterial = new Material(this.blurShader);
					this.m_BlurMaterial.hideFlags = 13;
				}
				return this.m_BlurMaterial;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06002371 RID: 9073 RVA: 0x000178AE File Offset: 0x00015AAE
		protected Material downsampleMaterial
		{
			get
			{
				if (this.m_DownsampleMaterial == null)
				{
					this.m_DownsampleMaterial = new Material(this.downsampleShader);
					this.m_DownsampleMaterial.hideFlags = 13;
				}
				return this.m_DownsampleMaterial;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x000178E5 File Offset: 0x00015AE5
		protected Material blendMaterial
		{
			get
			{
				if (this.m_blendMaterial == null)
				{
					this.m_blendMaterial = new Material(this.blendShader);
					this.m_blendMaterial.hideFlags = 13;
				}
				return this.m_blendMaterial;
			}
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x0001791C File Offset: 0x00015B1C
		public override void Initialize()
		{
			base.Initialize();
			if (base.MyCamera.depthTextureMode == null)
			{
				base.MyCamera.depthTextureMode = 1;
			}
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x00115D3C File Offset: 0x00113F3C
		public override bool CheckSupport()
		{
			bool result = true;
			if (!SystemInfo.supportsImageEffects)
			{
				result = false;
			}
			if (!this.blurMaterial.shader.isSupported)
			{
				result = false;
			}
			if (!this.compositeMaterial.shader.isSupported)
			{
				result = false;
			}
			if (!this.downsampleMaterial.shader.isSupported)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x00115DA0 File Offset: 0x00113FA0
		public override void OnPreRender()
		{
			this.PrepareRenderTex();
			Camera shaderCamera = this.ShaderCamera;
			shaderCamera.CopyFrom(base.MyCamera);
			shaderCamera.backgroundColor = Color.black;
			shaderCamera.clearFlags = 2;
			shaderCamera.targetTexture = this.TempRenderTex;
			shaderCamera.RenderWithShader(this.ReplacementShader, "XftEffect");
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x00115DF8 File Offset: 0x00113FF8
		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			float num = this.m_owner.ColorCurve.Evaluate(this.m_elapsedTime);
			this.m_tint = Color.Lerp(this.m_owner.GlowColorStart, this.m_owner.GlowColorEnd, num);
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x00017940 File Offset: 0x00015B40
		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.RenderGlow(this.TempRenderTex, this.TempRenderGlow);
			this.blendMaterial.SetTexture("_GlowTex", this.TempRenderGlow);
			Graphics.Blit(source, destination, this.blendMaterial);
			this.ReleaseRenderTex();
		}

		// Token: 0x06002378 RID: 9080 RVA: 0x00115E4C File Offset: 0x0011404C
		private void ReleaseRenderTex()
		{
			if (this.TempRenderGlow != null)
			{
				RenderTexture.ReleaseTemporary(this.TempRenderGlow);
				this.TempRenderGlow = null;
			}
			if (this.TempRenderTex != null)
			{
				RenderTexture.ReleaseTemporary(this.TempRenderTex);
				this.TempRenderTex = null;
			}
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x00115EA0 File Offset: 0x001140A0
		private void PrepareRenderTex()
		{
			if (this.TempRenderTex == null)
			{
				this.TempRenderTex = RenderTexture.GetTemporary((int)base.MyCamera.pixelWidth, (int)base.MyCamera.pixelHeight, 0);
			}
			if (this.TempRenderGlow == null)
			{
				this.TempRenderGlow = RenderTexture.GetTemporary((int)base.MyCamera.pixelWidth, (int)base.MyCamera.pixelHeight, 0);
			}
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x00115F18 File Offset: 0x00114118
		private void FourTapCone(RenderTexture source, RenderTexture dest, int iteration, float spread)
		{
			float num = 0.5f + (float)iteration * spread;
			Graphics.BlitMultiTap(source, dest, this.blurMaterial, new Vector2[]
			{
				new Vector2(num, num),
				new Vector2(-num, num),
				new Vector2(num, -num),
				new Vector2(-num, -num)
			});
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x00115F94 File Offset: 0x00114194
		private void DownSample4x(RenderTexture source, RenderTexture dest, Color tint)
		{
			this.downsampleMaterial.color = new Color(tint.r, tint.g, tint.b, tint.a / 4f);
			Graphics.Blit(source, dest, this.downsampleMaterial);
		}

		// Token: 0x0600237C RID: 9084 RVA: 0x00115FE0 File Offset: 0x001141E0
		private void RenderGlow(RenderTexture source, RenderTexture destination)
		{
			float num = this.m_owner.GlowIntensity;
			int num2 = this.m_owner.GlowBlurIterations;
			float num3 = this.m_owner.GlowBlurSpread;
			num = Mathf.Clamp(num, 0f, 10f);
			num2 = Mathf.Clamp(num2, 0, 30);
			num3 = Mathf.Clamp(num3, 0.5f, 1f);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
			this.DownSample4x(source, temporary, this.m_tint);
			float num4 = Mathf.Clamp01((num - 1f) / 4f);
			this.blurMaterial.color = new Color(1f, 1f, 1f, 0.25f + num4);
			bool flag = true;
			for (int i = 0; i < num2; i++)
			{
				if (flag)
				{
					this.FourTapCone(temporary, temporary2, i, num3);
					temporary.DiscardContents();
				}
				else
				{
					this.FourTapCone(temporary2, temporary, i, num3);
					temporary2.DiscardContents();
				}
				flag = !flag;
			}
			Graphics.Blit(source, destination);
			if (flag)
			{
				this.BlitGlow(temporary, destination, num);
			}
			else
			{
				this.BlitGlow(temporary2, destination, num);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}

		// Token: 0x0600237D RID: 9085 RVA: 0x0001797D File Offset: 0x00015B7D
		private void BlitGlow(RenderTexture source, RenderTexture dest, float intensity)
		{
			this.compositeMaterial.color = new Color(1f, 1f, 1f, Mathf.Clamp01(intensity));
			Graphics.Blit(source, dest, this.compositeMaterial);
		}

		// Token: 0x04002ADD RID: 10973
		public Shader ReplacementShader;

		// Token: 0x04002ADE RID: 10974
		protected GameObject m_shaderCamera;

		// Token: 0x04002ADF RID: 10975
		protected RenderTexture TempRenderTex;

		// Token: 0x04002AE0 RID: 10976
		protected RenderTexture TempRenderGlow;

		// Token: 0x04002AE1 RID: 10977
		protected Color m_tint;

		// Token: 0x04002AE2 RID: 10978
		public Shader compositeShader;

		// Token: 0x04002AE3 RID: 10979
		private Material m_CompositeMaterial;

		// Token: 0x04002AE4 RID: 10980
		public Shader blurShader;

		// Token: 0x04002AE5 RID: 10981
		private Material m_BlurMaterial;

		// Token: 0x04002AE6 RID: 10982
		public Shader downsampleShader;

		// Token: 0x04002AE7 RID: 10983
		private Material m_DownsampleMaterial;

		// Token: 0x04002AE8 RID: 10984
		public Shader blendShader;

		// Token: 0x04002AE9 RID: 10985
		private Material m_blendMaterial;
	}
}
