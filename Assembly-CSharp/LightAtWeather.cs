using System;
using UnityEngine;

// Token: 0x02000844 RID: 2116
[RequireComponent(typeof(Light))]
public class LightAtWeather : MonoBehaviour
{
	// Token: 0x06003370 RID: 13168 RVA: 0x000204EB File Offset: 0x0001E6EB
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.lightComponent = base.GetComponent<Light>();
		this.lightIntensity = this.lightComponent.intensity;
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x0018D734 File Offset: 0x0018B934
	protected void Update()
	{
		int num = (this.sky.Components.Weather.Weather != this.type) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.lightComponent.intensity = Mathf.Lerp(0f, this.lightIntensity, this.lerpTime);
		this.lightComponent.enabled = (this.lightComponent.intensity > 0f);
	}

	// Token: 0x04003F98 RID: 16280
	public TOD_Sky sky;

	// Token: 0x04003F99 RID: 16281
	public TOD_Weather.WeatherType type;

	// Token: 0x04003F9A RID: 16282
	public float fadeTime = 1f;

	// Token: 0x04003F9B RID: 16283
	private float lerpTime;

	// Token: 0x04003F9C RID: 16284
	private Light lightComponent;

	// Token: 0x04003F9D RID: 16285
	private float lightIntensity;
}
