using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000585 RID: 1413
	public class GlowEvent : CameraEffectEvent
	{
		// Token: 0x06002363 RID: 9059 RVA: 0x00115920 File Offset: 0x00113B20
		public GlowEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.Glow, owner)
		{
			this.downsampleShader = owner.GlowDownSampleShader;
			this.compositeShader = owner.GlowCompositeShader;
			this.blurShader = owner.GlowBlurShader;
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06002364 RID: 9060 RVA: 0x00017767 File Offset: 0x00015967
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

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06002365 RID: 9061 RVA: 0x0001779E File Offset: 0x0001599E
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

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06002366 RID: 9062 RVA: 0x000177D5 File Offset: 0x000159D5
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

		// Token: 0x06002367 RID: 9063 RVA: 0x00115978 File Offset: 0x00113B78
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

		// Token: 0x06002368 RID: 9064 RVA: 0x001159F4 File Offset: 0x00113BF4
		private void DownSample4x(RenderTexture source, RenderTexture dest)
		{
			this.downsampleMaterial.color = new Color(this.m_glowTint.r, this.m_glowTint.g, this.m_glowTint.b, this.m_glowTint.a / 4f);
			Graphics.Blit(source, dest, this.downsampleMaterial);
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x0001780C File Offset: 0x00015A0C
		private void BlitGlow(RenderTexture source, RenderTexture dest, float intensity)
		{
			this.compositeMaterial.color = new Color(1f, 1f, 1f, Mathf.Clamp01(intensity));
			Graphics.Blit(source, dest, this.compositeMaterial);
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x00115A50 File Offset: 0x00113C50
		public override bool CheckSupport()
		{
			bool result = true;
			if (!SystemInfo.supportsImageEffects)
			{
				result = false;
			}
			if (this.downsampleShader == null)
			{
				Debug.Log("No downsample shader assigned! Disabling glow.");
				result = false;
			}
			else
			{
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
			}
			return result;
		}

		// Token: 0x0600236B RID: 9067 RVA: 0x00115AD4 File Offset: 0x00113CD4
		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int num = this.m_owner.GlowBlurIterations;
			float num2 = this.m_owner.GlowBlurSpread;
			float num3 = this.m_owner.GlowIntensity;
			num3 = Mathf.Clamp(num3, 0f, 10f);
			num = Mathf.Clamp(num, 0, 30);
			num2 = Mathf.Clamp(num2, 0.5f, 1f);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
			this.DownSample4x(source, temporary);
			float num4 = Mathf.Clamp01((num3 - 1f) / 4f);
			this.blurMaterial.color = new Color(1f, 1f, 1f, 0.25f + num4);
			bool flag = true;
			for (int i = 0; i < num; i++)
			{
				if (flag)
				{
					this.FourTapCone(temporary, temporary2, i, num2);
					temporary.DiscardContents();
				}
				else
				{
					this.FourTapCone(temporary2, temporary, i, num2);
					temporary2.DiscardContents();
				}
				flag = !flag;
			}
			Graphics.Blit(source, destination);
			if (flag)
			{
				this.BlitGlow(temporary, destination, num3);
			}
			else
			{
				this.BlitGlow(temporary2, destination, num3);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x00115C28 File Offset: 0x00113E28
		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			float num = this.m_owner.ColorCurve.Evaluate(this.m_elapsedTime);
			this.m_glowTint = Color.Lerp(this.m_owner.GlowColorStart, this.m_owner.GlowColorEnd, num);
		}

		// Token: 0x04002AD6 RID: 10966
		protected Color m_glowTint = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04002AD7 RID: 10967
		public Shader compositeShader;

		// Token: 0x04002AD8 RID: 10968
		private Material m_CompositeMaterial;

		// Token: 0x04002AD9 RID: 10969
		public Shader blurShader;

		// Token: 0x04002ADA RID: 10970
		private Material m_BlurMaterial;

		// Token: 0x04002ADB RID: 10971
		public Shader downsampleShader;

		// Token: 0x04002ADC RID: 10972
		private Material m_DownsampleMaterial;
	}
}
