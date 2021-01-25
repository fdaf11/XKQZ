using System;
using UnityEngine;

// Token: 0x02000843 RID: 2115
[RequireComponent(typeof(Light))]
public class LightAtNight : MonoBehaviour
{
	// Token: 0x0600336D RID: 13165 RVA: 0x0002049E File Offset: 0x0001E69E
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.lightComponent = base.GetComponent<Light>();
		this.lightIntensity = this.lightComponent.intensity;
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x0018D6B0 File Offset: 0x0018B8B0
	protected void Update()
	{
		int num = (!this.sky.IsNight) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.lightComponent.intensity = Mathf.Lerp(0f, this.lightIntensity, this.lerpTime);
		this.lightComponent.enabled = (this.lightComponent.intensity > 0f);
	}

	// Token: 0x04003F93 RID: 16275
	public TOD_Sky sky;

	// Token: 0x04003F94 RID: 16276
	public float fadeTime = 1f;

	// Token: 0x04003F95 RID: 16277
	private float lerpTime;

	// Token: 0x04003F96 RID: 16278
	private Light lightComponent;

	// Token: 0x04003F97 RID: 16279
	private float lightIntensity;
}
