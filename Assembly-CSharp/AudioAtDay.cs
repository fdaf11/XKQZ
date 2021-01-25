using System;
using UnityEngine;

// Token: 0x0200083E RID: 2110
[RequireComponent(typeof(AudioSource))]
public class AudioAtDay : MonoBehaviour
{
	// Token: 0x0600335F RID: 13151 RVA: 0x0018D39C File Offset: 0x0018B59C
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.audioComponent = base.GetComponent<AudioSource>();
		this.audioVolume = this.audioComponent.volume;
		if (!this.sky.IsDay)
		{
			this.audioComponent.volume = 0f;
		}
	}

	// Token: 0x06003360 RID: 13152 RVA: 0x0018D404 File Offset: 0x0018B604
	protected void Update()
	{
		int num = (!this.sky.IsDay) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.audioComponent.volume = Mathf.Lerp(0f, this.audioVolume, this.lerpTime);
	}

	// Token: 0x04003F7D RID: 16253
	public TOD_Sky sky;

	// Token: 0x04003F7E RID: 16254
	public float fadeTime = 1f;

	// Token: 0x04003F7F RID: 16255
	private float lerpTime;

	// Token: 0x04003F80 RID: 16256
	private AudioSource audioComponent;

	// Token: 0x04003F81 RID: 16257
	private float audioVolume;
}
