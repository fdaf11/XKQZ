using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000588 RID: 1416
	public class RadialBlurTexAddEvent : CameraEffectEvent
	{
		// Token: 0x06002383 RID: 9091 RVA: 0x00017A18 File Offset: 0x00015C18
		public RadialBlurTexAddEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.RadialBlurMask, owner)
		{
			this.RadialBlurShader = owner.RadialBlurTexAddShader;
			this.Mask = owner.RadialBlurMask;
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x00017A3A File Offset: 0x00015C3A
		public Material MyMaterial
		{
			get
			{
				if (this.m_material == null)
				{
					this.m_material = new Material(this.RadialBlurShader);
					this.m_material.hideFlags = 13;
				}
				return this.m_material;
			}
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x001162B8 File Offset: 0x001144B8
		public override bool CheckSupport()
		{
			bool result = true;
			if (!SystemInfo.supportsImageEffects)
			{
				result = false;
			}
			if (!this.MyMaterial.shader.isSupported)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x001162EC File Offset: 0x001144EC
		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.m_material == null)
			{
				return;
			}
			this.MyMaterial.SetTexture("_Mask", this.Mask);
			this.MyMaterial.SetFloat("_SampleDist", this.m_owner.RBMaskSampleDist);
			this.MyMaterial.SetFloat("_SampleStrength", this.m_strength);
			Graphics.Blit(source, destination, this.m_material);
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x00116360 File Offset: 0x00114560
		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			if (this.m_owner.RBMaskStrengthType == MAGTYPE.Fixed)
			{
				this.m_strength = this.m_owner.RBMaskSampleStrength;
			}
			else if (this.m_owner.RBMaskStrengthType == MAGTYPE.Curve_OBSOLETE)
			{
				this.m_strength = this.m_owner.RBMaskSampleStrengthCurve.Evaluate(this.m_elapsedTime);
			}
			else
			{
				this.m_strength = this.m_owner.RBMaskSampleStrengthCurveX.Evaluate(this.m_elapsedTime);
			}
		}

		// Token: 0x04002AEE RID: 10990
		protected float m_strength;

		// Token: 0x04002AEF RID: 10991
		protected Material m_material;

		// Token: 0x04002AF0 RID: 10992
		public Shader RadialBlurShader;

		// Token: 0x04002AF1 RID: 10993
		public Texture2D Mask;
	}
}
