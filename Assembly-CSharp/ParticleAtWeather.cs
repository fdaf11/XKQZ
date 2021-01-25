using System;
using UnityEngine;

// Token: 0x02000847 RID: 2119
[RequireComponent(typeof(ParticleSystem))]
public class ParticleAtWeather : MonoBehaviour
{
	// Token: 0x06003379 RID: 13177 RVA: 0x000205D2 File Offset: 0x0001E7D2
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.particleComponent = base.GetComponent<ParticleSystem>();
		this.particleEmission = this.particleComponent.emissionRate;
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x0018D898 File Offset: 0x0018BA98
	protected void Update()
	{
		int num = (this.sky.Components.Weather.Weather != this.type) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.particleComponent.emissionRate = Mathf.Lerp(0f, this.particleEmission, this.lerpTime);
	}

	// Token: 0x04003FA8 RID: 16296
	public TOD_Sky sky;

	// Token: 0x04003FA9 RID: 16297
	public TOD_Weather.WeatherType type;

	// Token: 0x04003FAA RID: 16298
	public float fadeTime = 1f;

	// Token: 0x04003FAB RID: 16299
	private float lerpTime;

	// Token: 0x04003FAC RID: 16300
	private ParticleSystem particleComponent;

	// Token: 0x04003FAD RID: 16301
	private float particleEmission;
}
