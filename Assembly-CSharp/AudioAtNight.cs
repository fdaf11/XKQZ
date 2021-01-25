using System;
using UnityEngine;

// Token: 0x0200083F RID: 2111
[RequireComponent(typeof(AudioSource))]
public class AudioAtNight : MonoBehaviour
{
	// Token: 0x06003362 RID: 13154 RVA: 0x0018D46C File Offset: 0x0018B66C
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.audioComponent = base.GetComponent<AudioSource>();
		this.audioVolume = this.audioComponent.volume;
		if (!this.sky.IsNight)
		{
			this.audioComponent.volume = 0f;
		}
	}

	// Token: 0x06003363 RID: 13155 RVA: 0x0018D4D4 File Offset: 0x0018B6D4
	protected void Update()
	{
		int num = (!this.sky.IsNight) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.audioComponent.volume = Mathf.Lerp(0f, this.audioVolume, this.lerpTime);
	}

	// Token: 0x04003F82 RID: 16258
	public TOD_Sky sky;

	// Token: 0x04003F83 RID: 16259
	public float fadeTime = 1f;

	// Token: 0x04003F84 RID: 16260
	private float lerpTime;

	// Token: 0x04003F85 RID: 16261
	private AudioSource audioComponent;

	// Token: 0x04003F86 RID: 16262
	private float audioVolume;
}
