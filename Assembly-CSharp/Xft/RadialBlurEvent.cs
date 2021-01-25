using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000587 RID: 1415
	public class RadialBlurEvent : CameraEffectEvent
	{
		// Token: 0x0600237E RID: 9086 RVA: 0x000179B1 File Offset: 0x00015BB1
		public RadialBlurEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.RadialBlur, owner)
		{
			this.RadialBlurShader = owner.RadialBlurShader;
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x0600237F RID: 9087 RVA: 0x000179E1 File Offset: 0x00015BE1
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

		// Token: 0x06002380 RID: 9088 RVA: 0x0011613C File Offset: 0x0011433C
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

		// Token: 0x06002381 RID: 9089 RVA: 0x00116170 File Offset: 0x00114370
		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			Vector3 center = base.MyCamera.WorldToScreenPoint(this.m_owner.RadialBlurObj.position);
			this.Center = center;
			if (this.m_owner.RBStrengthType == MAGTYPE.Fixed)
			{
				this.m_strength = this.m_owner.RBSampleStrength;
			}
			else if (this.m_owner.RBStrengthType == MAGTYPE.Curve_OBSOLETE)
			{
				this.m_strength = this.m_owner.RBSampleStrengthCurve.Evaluate(this.m_elapsedTime);
			}
			else
			{
				this.m_strength = this.m_owner.RBSampleStrengthCurveX.Evaluate(this.m_elapsedTime);
			}
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x00116224 File Offset: 0x00114424
		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.MyMaterial.SetFloat("_SampleDist", this.m_owner.RBSampleDist);
			this.MyMaterial.SetFloat("_SampleStrength", this.m_strength);
			Vector4 zero = Vector4.zero;
			zero.x = this.Center.x / (float)Screen.width;
			zero.y = this.Center.y / (float)Screen.height;
			this.MyMaterial.SetVector("_Center", zero);
			Graphics.Blit(source, destination, this.m_material);
		}

		// Token: 0x04002AEA RID: 10986
		protected Material m_material;

		// Token: 0x04002AEB RID: 10987
		public Shader RadialBlurShader;

		// Token: 0x04002AEC RID: 10988
		public Vector3 Center = new Vector3(0.5f, 0.5f, 0f);

		// Token: 0x04002AED RID: 10989
		protected float m_strength;
	}
}
