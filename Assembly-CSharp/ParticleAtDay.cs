using System;
using UnityEngine;

// Token: 0x02000845 RID: 2117
[RequireComponent(typeof(ParticleSystem))]
public class ParticleAtDay : MonoBehaviour
{
	// Token: 0x06003373 RID: 13171 RVA: 0x00020538 File Offset: 0x0001E738
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.particleComponent = base.GetComponent<ParticleSystem>();
		this.particleEmission = this.particleComponent.emissionRate;
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x0018D7C8 File Offset: 0x0018B9C8
	protected void Update()
	{
		int num = (!this.sky.IsDay) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.particleComponent.emissionRate = Mathf.Lerp(0f, this.particleEmission, this.lerpTime);
	}

	// Token: 0x04003F9E RID: 16286
	public TOD_Sky sky;

	// Token: 0x04003F9F RID: 16287
	public float fadeTime = 1f;

	// Token: 0x04003FA0 RID: 16288
	private float lerpTime;

	// Token: 0x04003FA1 RID: 16289
	private ParticleSystem particleComponent;

	// Token: 0x04003FA2 RID: 16290
	private float particleEmission;
}
