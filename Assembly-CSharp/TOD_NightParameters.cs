using System;
using UnityEngine;

// Token: 0x02000827 RID: 2087
[Serializable]
public class TOD_NightParameters
{
	// Token: 0x060032ED RID: 13037 RVA: 0x0001FECF File Offset: 0x0001E0CF
	public void CheckRange()
	{
		this.SkyMultiplier = Mathf.Clamp01(this.SkyMultiplier);
		this.CloudMultiplier = Mathf.Clamp01(this.CloudMultiplier);
	}

	// Token: 0x04003EB1 RID: 16049
	public Color AdditiveColor = new Color32(0, 0, 0, byte.MaxValue);

	// Token: 0x04003EB2 RID: 16050
	public Color AmbientColor = new Color32(50, 62, 81, byte.MaxValue);

	// Token: 0x04003EB3 RID: 16051
	public Color CloudColor = new Color32(47, 73, 137, byte.MaxValue);

	// Token: 0x04003EB4 RID: 16052
	public float SkyMultiplier = 0.1f;

	// Token: 0x04003EB5 RID: 16053
	public float CloudMultiplier = 0.1f;
}
