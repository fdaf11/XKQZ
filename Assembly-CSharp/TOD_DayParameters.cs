using System;
using UnityEngine;

// Token: 0x02000826 RID: 2086
[Serializable]
public class TOD_DayParameters
{
	// Token: 0x060032EB RID: 13035 RVA: 0x0001FEAB File Offset: 0x0001E0AB
	public void CheckRange()
	{
		this.SkyMultiplier = Mathf.Clamp01(this.SkyMultiplier);
		this.CloudMultiplier = Mathf.Clamp01(this.CloudMultiplier);
	}

	// Token: 0x04003EAC RID: 16044
	public Color AdditiveColor = new Color32(0, 0, 0, byte.MaxValue);

	// Token: 0x04003EAD RID: 16045
	public Color AmbientColor = new Color32(78, 97, 127, byte.MaxValue);

	// Token: 0x04003EAE RID: 16046
	public Color CloudColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04003EAF RID: 16047
	public float SkyMultiplier = 1f;

	// Token: 0x04003EB0 RID: 16048
	public float CloudMultiplier = 1f;
}
