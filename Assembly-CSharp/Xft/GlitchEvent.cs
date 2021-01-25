using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000584 RID: 1412
	public class GlitchEvent : CameraEffectEvent
	{
		// Token: 0x0600235C RID: 9052 RVA: 0x000176DD File Offset: 0x000158DD
		public GlitchEvent(XftEventComponent owner) : base(CameraEffectEvent.EType.Glitch, owner)
		{
			this.m_random = new WaveRandom();
			this.Mask = owner.GlitchMask;
			this.GlitchShader = owner.GlitchShader;
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x0600235D RID: 9053 RVA: 0x0001770A File Offset: 0x0001590A
		public Material GlitchMaterial
		{
			get
			{
				if (this.m_material == null)
				{
					this.m_material = new Material(this.GlitchShader);
					this.m_material.hideFlags = 13;
				}
				return this.m_material;
			}
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x00115828 File Offset: 0x00113A28
		public override bool CheckSupport()
		{
			bool result = true;
			if (!SystemInfo.supportsImageEffects)
			{
				result = false;
			}
			if (!this.GlitchMaterial.shader.isSupported)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x00017741 File Offset: 0x00015941
		public override void Initialize()
		{
			base.Initialize();
			this.m_random.Reset();
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x00017754 File Offset: 0x00015954
		public override void Reset()
		{
			base.Reset();
			this.m_random.Reset();
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0011585C File Offset: 0x00113A5C
		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			this.m_offset = this.m_random.GetRandom(this.m_owner.MinAmp, this.m_owner.MaxAmp, this.m_owner.MinRand, this.m_owner.MaxRand, this.m_owner.WaveLen);
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x001158C0 File Offset: 0x00113AC0
		public override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Mask == null)
			{
				return;
			}
			this.GlitchMaterial.SetVector("_displace", this.m_offset);
			this.GlitchMaterial.SetTexture("_Mask", this.Mask);
			Graphics.Blit(source, destination, this.GlitchMaterial);
		}

		// Token: 0x04002AD1 RID: 10961
		protected Vector3 m_offset;

		// Token: 0x04002AD2 RID: 10962
		protected WaveRandom m_random;

		// Token: 0x04002AD3 RID: 10963
		protected Material m_material;

		// Token: 0x04002AD4 RID: 10964
		public Shader GlitchShader;

		// Token: 0x04002AD5 RID: 10965
		public Texture2D Mask;
	}
}
