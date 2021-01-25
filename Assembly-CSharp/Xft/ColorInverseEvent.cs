using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000582 RID: 1410
	public class ColorInverseEvent : CameraEffectEvent
	{
		// Token: 0x06002354 RID: 9044 RVA: 0x00017614 File Offset: 0x00015814
		public ColorInverseEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.ColorInverse, owner)
		{
			this.ColorInverseShader = owner.ColorInverseShader;
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06002355 RID: 9045 RVA: 0x0001762A File Offset: 0x0001582A
		public Material MyMaterial
		{
			get
			{
				if (this.m_material == null)
				{
					this.m_material = new Material(this.ColorInverseShader);
					this.m_material.hideFlags = 13;
				}
				return this.m_material;
			}
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x00115660 File Offset: 0x00113860
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

		// Token: 0x06002357 RID: 9047 RVA: 0x00017661 File Offset: 0x00015861
		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			this.m_strength = this.m_owner.CIStrengthCurve.Evaluate(this.m_elapsedTime);
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0001768D File Offset: 0x0001588D
		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.MyMaterial.SetFloat("_Strength", this.m_strength);
			Graphics.Blit(source, destination, this.MyMaterial);
		}

		// Token: 0x04002ACA RID: 10954
		protected float m_strength;

		// Token: 0x04002ACB RID: 10955
		public Shader ColorInverseShader;

		// Token: 0x04002ACC RID: 10956
		protected Material m_material;
	}
}
