using System;
using UnityEngine;

// Token: 0x02000823 RID: 2083
[Serializable]
public class TOD_CycleParameters
{
	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x060032E1 RID: 13025 RVA: 0x00188788 File Offset: 0x00186988
	// (set) Token: 0x060032E2 RID: 13026 RVA: 0x001887F4 File Offset: 0x001869F4
	public DateTime DateTime
	{
		get
		{
			this.CheckRange();
			float hour = this.Hour;
			int num = (int)hour;
			float num2 = (hour - (float)num) * 60f;
			int num3 = (int)num2;
			float num4 = (num2 - (float)num3) * 60f;
			int num5 = (int)num4;
			float num6 = (num4 - (float)num5) * 1000f;
			int num7 = (int)num6;
			return new DateTime(this.Year, this.Month, this.Day, num, num3, num5, num7);
		}
		set
		{
			this.Year = value.Year;
			this.Month = value.Month;
			this.Day = value.Day;
			this.Hour = (float)value.Hour + (float)value.Minute / 60f + (float)value.Second / 3600f;
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x060032E3 RID: 13027 RVA: 0x00188854 File Offset: 0x00186A54
	// (set) Token: 0x060032E4 RID: 13028 RVA: 0x0001FE4A File Offset: 0x0001E04A
	public long Ticks
	{
		get
		{
			return this.DateTime.Ticks;
		}
		set
		{
			this.DateTime = new DateTime(value);
		}
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x00188870 File Offset: 0x00186A70
	public void CheckRange()
	{
		this.Year = Mathf.Clamp(this.Year, 1, 9999);
		this.Month = Mathf.Clamp(this.Month, 1, 12);
		this.Day = Mathf.Clamp(this.Day, 1, DateTime.DaysInMonth(this.Year, this.Month));
		this.Hour = Mathf.Repeat(this.Hour, 24f);
		this.Longitude = Mathf.Clamp(this.Longitude, -180f, 180f);
		this.Latitude = Mathf.Clamp(this.Latitude, -90f, 90f);
	}

	// Token: 0x04003E99 RID: 16025
	public float Hour = 12f;

	// Token: 0x04003E9A RID: 16026
	public int Day = 1;

	// Token: 0x04003E9B RID: 16027
	public int Month = 3;

	// Token: 0x04003E9C RID: 16028
	public int Year = 2000;

	// Token: 0x04003E9D RID: 16029
	public float Latitude;

	// Token: 0x04003E9E RID: 16030
	public float Longitude;

	// Token: 0x04003E9F RID: 16031
	public float UTC;
}
