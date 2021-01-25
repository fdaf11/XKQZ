using System;
using UnityEngine;

// Token: 0x02000840 RID: 2112
[RequireComponent(typeof(AudioSource))]
public class AudioAtWeather : MonoBehaviour
{
	// Token: 0x06003365 RID: 13157 RVA: 0x0018D53C File Offset: 0x0018B73C
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.audioComponent = base.GetComponent<AudioSource>();
		this.audioVolume = this.audioComponent.volume;
		if (this.sky.Components.Weather.Weather != this.type)
		{
			this.audioComponent.volume = 0f;
		}
	}

	// Token: 0x06003366 RID: 13158 RVA: 0x0018D5B4 File Offset: 0x0018B7B4
	protected void Update()
	{
		int num = (this.sky.Components.Weather.Weather != this.type) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.audioComponent.volume = Mathf.Lerp(0f, this.audioVolume, this.lerpTime);
	}

	// Token: 0x04003F87 RID: 16263
	public TOD_Sky sky;

	// Token: 0x04003F88 RID: 16264
	public TOD_Weather.WeatherType type;

	// Token: 0x04003F89 RID: 16265
	public float fadeTime = 1f;

	// Token: 0x04003F8A RID: 16266
	private float lerpTime;

	// Token: 0x04003F8B RID: 16267
	private AudioSource audioComponent;

	// Token: 0x04003F8C RID: 16268
	private float audioVolume;
}
