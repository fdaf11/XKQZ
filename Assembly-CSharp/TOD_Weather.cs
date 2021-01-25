using System;
using UnityEngine;

// Token: 0x0200083B RID: 2107
public class TOD_Weather : MonoBehaviour
{
	// Token: 0x0600335C RID: 13148 RVA: 0x0018D080 File Offset: 0x0018B280
	protected void Start()
	{
		this.sky = base.GetComponent<TOD_Sky>();
		this.cloudBrightness = (this.cloudBrightnessDefault = this.sky.Clouds.Brightness);
		this.cloudDensity = (this.cloudDensityDefault = this.sky.Clouds.Density);
		this.atmosphereFog = (this.atmosphereFogDefault = this.sky.Atmosphere.Fogginess);
		this.cloudSharpness = this.sky.Clouds.Sharpness;
	}

	// Token: 0x0600335D RID: 13149 RVA: 0x0018D10C File Offset: 0x0018B30C
	protected void Update()
	{
		if (this.Clouds == TOD_Weather.CloudType.Custom && this.Weather == TOD_Weather.WeatherType.Custom)
		{
			return;
		}
		switch (this.Clouds)
		{
		case TOD_Weather.CloudType.Custom:
			this.cloudDensity = this.sky.Clouds.Density;
			this.cloudSharpness = this.sky.Clouds.Sharpness;
			break;
		case TOD_Weather.CloudType.None:
			this.cloudDensity = 0f;
			this.cloudSharpness = 1f;
			break;
		case TOD_Weather.CloudType.Few:
			this.cloudDensity = this.cloudDensityDefault;
			this.cloudSharpness = 5f;
			break;
		case TOD_Weather.CloudType.Scattered:
			this.cloudDensity = this.cloudDensityDefault;
			this.cloudSharpness = 1f;
			break;
		case TOD_Weather.CloudType.Broken:
			this.cloudDensity = this.cloudDensityDefault;
			this.cloudSharpness = 0.5f;
			break;
		case TOD_Weather.CloudType.Overcast:
			this.cloudDensity = this.cloudDensityDefault;
			this.cloudSharpness = 0.1f;
			break;
		}
		switch (this.Weather)
		{
		case TOD_Weather.WeatherType.Custom:
			this.cloudBrightness = this.sky.Clouds.Brightness;
			this.atmosphereFog = this.sky.Atmosphere.Fogginess;
			break;
		case TOD_Weather.WeatherType.Clear:
			this.cloudBrightness = this.cloudBrightnessDefault;
			this.atmosphereFog = this.atmosphereFogDefault;
			break;
		case TOD_Weather.WeatherType.Storm:
			this.cloudBrightness = 0.3f;
			this.atmosphereFog = 1f;
			break;
		case TOD_Weather.WeatherType.Dust:
			this.cloudBrightness = this.cloudBrightnessDefault;
			this.atmosphereFog = 0.5f;
			break;
		case TOD_Weather.WeatherType.Fog:
			this.cloudBrightness = this.cloudBrightnessDefault;
			this.atmosphereFog = 1f;
			break;
		}
		float num = Time.deltaTime / this.FadeTime;
		this.sky.Clouds.Brightness = Mathf.Lerp(this.sky.Clouds.Brightness, this.cloudBrightness, num);
		this.sky.Clouds.Density = Mathf.Lerp(this.sky.Clouds.Density, this.cloudDensity, num);
		this.sky.Clouds.Sharpness = Mathf.Lerp(this.sky.Clouds.Sharpness, this.cloudSharpness, num);
		this.sky.Atmosphere.Fogginess = Mathf.Lerp(this.sky.Atmosphere.Fogginess, this.atmosphereFog, num);
	}

	// Token: 0x04003F65 RID: 16229
	public float FadeTime = 10f;

	// Token: 0x04003F66 RID: 16230
	public TOD_Weather.CloudType Clouds;

	// Token: 0x04003F67 RID: 16231
	public TOD_Weather.WeatherType Weather;

	// Token: 0x04003F68 RID: 16232
	private float cloudBrightnessDefault;

	// Token: 0x04003F69 RID: 16233
	private float cloudDensityDefault;

	// Token: 0x04003F6A RID: 16234
	private float atmosphereFogDefault;

	// Token: 0x04003F6B RID: 16235
	private float cloudBrightness;

	// Token: 0x04003F6C RID: 16236
	private float cloudDensity;

	// Token: 0x04003F6D RID: 16237
	private float atmosphereFog;

	// Token: 0x04003F6E RID: 16238
	private float cloudSharpness;

	// Token: 0x04003F6F RID: 16239
	private TOD_Sky sky;

	// Token: 0x0200083C RID: 2108
	public enum CloudType
	{
		// Token: 0x04003F71 RID: 16241
		Custom,
		// Token: 0x04003F72 RID: 16242
		None,
		// Token: 0x04003F73 RID: 16243
		Few,
		// Token: 0x04003F74 RID: 16244
		Scattered,
		// Token: 0x04003F75 RID: 16245
		Broken,
		// Token: 0x04003F76 RID: 16246
		Overcast
	}

	// Token: 0x0200083D RID: 2109
	public enum WeatherType
	{
		// Token: 0x04003F78 RID: 16248
		Custom,
		// Token: 0x04003F79 RID: 16249
		Clear,
		// Token: 0x04003F7A RID: 16250
		Storm,
		// Token: 0x04003F7B RID: 16251
		Dust,
		// Token: 0x04003F7C RID: 16252
		Fog
	}
}
