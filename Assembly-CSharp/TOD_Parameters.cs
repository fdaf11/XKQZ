using System;

// Token: 0x02000822 RID: 2082
[Serializable]
public class TOD_Parameters
{
	// Token: 0x060032DD RID: 13021 RVA: 0x00002672 File Offset: 0x00000872
	public TOD_Parameters()
	{
	}

	// Token: 0x060032DE RID: 13022 RVA: 0x0018862C File Offset: 0x0018682C
	public TOD_Parameters(TOD_Sky sky)
	{
		this.Cycle = sky.Cycle;
		this.Atmosphere = sky.Atmosphere;
		this.Day = sky.Day;
		this.Night = sky.Night;
		this.Sun = sky.Sun;
		this.Moon = sky.Moon;
		this.Light = sky.Light;
		this.Stars = sky.Stars;
		this.Clouds = sky.Clouds;
		this.World = sky.World;
		this.Fog = sky.Fog;
		this.Ambient = sky.Ambient;
		this.Reflection = sky.Reflection;
	}

	// Token: 0x060032DF RID: 13023 RVA: 0x001886DC File Offset: 0x001868DC
	public void ToSky(TOD_Sky sky)
	{
		sky.Cycle = this.Cycle;
		sky.Atmosphere = this.Atmosphere;
		sky.Day = this.Day;
		sky.Night = this.Night;
		sky.Sun = this.Sun;
		sky.Moon = this.Moon;
		sky.Light = this.Light;
		sky.Stars = this.Stars;
		sky.Clouds = this.Clouds;
		sky.World = this.World;
		sky.Fog = this.Fog;
		sky.Ambient = this.Ambient;
		sky.Reflection = this.Reflection;
	}

	// Token: 0x04003E8C RID: 16012
	public TOD_CycleParameters Cycle;

	// Token: 0x04003E8D RID: 16013
	public TOD_AtmosphereParameters Atmosphere;

	// Token: 0x04003E8E RID: 16014
	public TOD_DayParameters Day;

	// Token: 0x04003E8F RID: 16015
	public TOD_NightParameters Night;

	// Token: 0x04003E90 RID: 16016
	public TOD_SunParameters Sun;

	// Token: 0x04003E91 RID: 16017
	public TOD_MoonParameters Moon;

	// Token: 0x04003E92 RID: 16018
	public TOD_LightParameters Light;

	// Token: 0x04003E93 RID: 16019
	public TOD_StarParameters Stars;

	// Token: 0x04003E94 RID: 16020
	public TOD_CloudParameters Clouds;

	// Token: 0x04003E95 RID: 16021
	public TOD_WorldParameters World;

	// Token: 0x04003E96 RID: 16022
	public TOD_FogParameters Fog;

	// Token: 0x04003E97 RID: 16023
	public TOD_AmbientParameters Ambient;

	// Token: 0x04003E98 RID: 16024
	public TOD_ReflectionParameters Reflection;
}
