using System;
using UnityEngine;

// Token: 0x02000842 RID: 2114
[RequireComponent(typeof(Light))]
public class LightAtDay : MonoBehaviour
{
	// Token: 0x0600336A RID: 13162 RVA: 0x00020451 File Offset: 0x0001E651
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.lightComponent = base.GetComponent<Light>();
		this.lightIntensity = this.lightComponent.intensity;
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x0018D62C File Offset: 0x0018B82C
	protected void Update()
	{
		int num = (!this.sky.IsDay) ? -1 : 1;
		this.lerpTime = Mathf.Clamp01(this.lerpTime + (float)num * Time.deltaTime / this.fadeTime);
		this.lightComponent.intensity = Mathf.Lerp(0f, this.lightIntensity, this.lerpTime);
		this.lightComponent.enabled = (this.lightComponent.intensity > 0f);
	}

	// Token: 0x04003F8E RID: 16270
	public TOD_Sky sky;

	// Token: 0x04003F8F RID: 16271
	public float fadeTime = 1f;

	// Token: 0x04003F90 RID: 16272
	private float lerpTime;

	// Token: 0x04003F91 RID: 16273
	private Light lightComponent;

	// Token: 0x04003F92 RID: 16274
	private float lightIntensity;
}
