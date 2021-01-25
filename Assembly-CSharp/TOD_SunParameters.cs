using System;
using UnityEngine;

// Token: 0x02000828 RID: 2088
[Serializable]
public class TOD_SunParameters
{
	// Token: 0x060032EF RID: 13039 RVA: 0x00188BE0 File Offset: 0x00186DE0
	public void CheckRange()
	{
		this.MeshSize = Mathf.Max(0f, this.MeshSize);
		this.MeshBrightness = Mathf.Max(0f, this.MeshBrightness);
		this.MeshContrast = Mathf.Max(0f, this.MeshContrast);
		this.LightIntensity = Mathf.Max(0f, this.LightIntensity);
		this.ShadowStrength = Mathf.Clamp01(this.ShadowStrength);
	}

	// Token: 0x04003EB6 RID: 16054
	public Color LightColor = new Color32(byte.MaxValue, 243, 234, byte.MaxValue);

	// Token: 0x04003EB7 RID: 16055
	public Color MeshColor = new Color32(byte.MaxValue, 160, 25, byte.MaxValue);

	// Token: 0x04003EB8 RID: 16056
	public Color ShaftColor = new Color32(byte.MaxValue, 243, 234, byte.MaxValue);

	// Token: 0x04003EB9 RID: 16057
	public float MeshSize = 1f;

	// Token: 0x04003EBA RID: 16058
	public float MeshBrightness = 1f;

	// Token: 0x04003EBB RID: 16059
	public float MeshContrast = 1f;

	// Token: 0x04003EBC RID: 16060
	public float LightIntensity = 1f;

	// Token: 0x04003EBD RID: 16061
	public float ShadowStrength = 1f;
}
