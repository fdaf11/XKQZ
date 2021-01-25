using System;
using UnityEngine;

// Token: 0x0200082D RID: 2093
[Serializable]
public class TOD_FogParameters
{
	// Token: 0x060032F9 RID: 13049 RVA: 0x0001FF3C File Offset: 0x0001E13C
	public void CheckRange()
	{
		this.UpdateInterval = Mathf.Max(0f, this.UpdateInterval);
		this.HeightBias = Mathf.Clamp01(this.HeightBias);
	}

	// Token: 0x04003ED9 RID: 16089
	public TOD_FogType Mode = TOD_FogType.Color;

	// Token: 0x04003EDA RID: 16090
	public bool Directional;

	// Token: 0x04003EDB RID: 16091
	public float UpdateInterval = 1f;

	// Token: 0x04003EDC RID: 16092
	public float HeightBias = 0.1f;
}
