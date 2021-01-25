using System;
using UnityEngine;

// Token: 0x02000846 RID: 2118
[RequireComponent(typeof(ParticleSystem))]
public class ParticleAtNight : MonoBehaviour
{
	// Token: 0x06003376 RID: 13174 RVA: 0x00020585 File Offset: 0x0001E785
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.particleComponent = base.GetComponent<ParticleSystem>();
		this.particleEmission = this.particleComponent.emissionRate;
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x0018D830 File Offset: 0x0018BA30
	protected void Update()
	{
		int num = (!this.sky.IsNight) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.particleComponent.emissionRate = Mathf.Lerp(0f, this.particleEmission, this.lerpTime);
	}

	// Token: 0x04003FA3 RID: 16291
	public TOD_Sky sky;

	// Token: 0x04003FA4 RID: 16292
	public float fadeTime = 1f;

	// Token: 0x04003FA5 RID: 16293
	private float lerpTime;

	// Token: 0x04003FA6 RID: 16294
	private ParticleSystem particleComponent;

	// Token: 0x04003FA7 RID: 16295
	private float particleEmission;
}
