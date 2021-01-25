using System;
using UnityEngine;

// Token: 0x02000824 RID: 2084
[Serializable]
public class TOD_AtmosphereParameters
{
	// Token: 0x060032E7 RID: 13031 RVA: 0x00188984 File Offset: 0x00186B84
	public void CheckRange()
	{
		this.MieMultiplier = Mathf.Max(0f, this.MieMultiplier);
		this.RayleighMultiplier = Mathf.Max(0f, this.RayleighMultiplier);
		this.Brightness = Mathf.Max(0f, this.Brightness);
		this.Contrast = Mathf.Max(0f, this.Contrast);
		this.Directionality = Mathf.Clamp01(this.Directionality);
		this.Haziness = Mathf.Clamp01(this.Haziness);
		this.Fogginess = Mathf.Clamp01(this.Fogginess);
		this.FakeHDR = Mathf.Clamp01(this.FakeHDR);
	}

	// Token: 0x04003EA0 RID: 16032
	public Color ScatteringColor = Color.white;

	// Token: 0x04003EA1 RID: 16033
	public float RayleighMultiplier = 1f;

	// Token: 0x04003EA2 RID: 16034
	public float MieMultiplier = 1f;

	// Token: 0x04003EA3 RID: 16035
	public float Brightness = 1f;

	// Token: 0x04003EA4 RID: 16036
	public float Contrast = 1f;

	// Token: 0x04003EA5 RID: 16037
	public float Directionality = 0.5f;

	// Token: 0x04003EA6 RID: 16038
	public float Haziness = 0.5f;

	// Token: 0x04003EA7 RID: 16039
	public float Fogginess;

	// Token: 0x04003EA8 RID: 16040
	public float FakeHDR = 1f;
}
