using System;
using UnityEngine;

// Token: 0x02000829 RID: 2089
[Serializable]
public class TOD_MoonParameters
{
	// Token: 0x060032F1 RID: 13041 RVA: 0x00188D08 File Offset: 0x00186F08
	public void CheckRange()
	{
		this.MeshSize = Mathf.Max(0f, this.MeshSize);
		this.MeshBrightness = Mathf.Max(0f, this.MeshBrightness);
		this.MeshContrast = Mathf.Max(0f, this.MeshContrast);
		this.LightIntensity = Mathf.Max(0f, this.LightIntensity);
		this.ShadowStrength = Mathf.Clamp01(this.ShadowStrength);
		if (this.Phase > 1f)
		{
			this.Phase -= (float)((int)this.Phase + 1);
		}
		else if (this.Phase < -1f)
		{
			this.Phase -= (float)((int)this.Phase - 1);
		}
	}

	// Token: 0x04003EBE RID: 16062
	public Color LightColor = new Color32(181, 204, byte.MaxValue, byte.MaxValue);

	// Token: 0x04003EBF RID: 16063
	public Color MeshColor = new Color32(byte.MaxValue, 233, 200, byte.MaxValue);

	// Token: 0x04003EC0 RID: 16064
	public Color HaloColor = new Color32(81, 104, 155, 50);

	// Token: 0x04003EC1 RID: 16065
	public float MeshSize = 1f;

	// Token: 0x04003EC2 RID: 16066
	public float MeshBrightness = 1f;

	// Token: 0x04003EC3 RID: 16067
	public float MeshContrast = 1f;

	// Token: 0x04003EC4 RID: 16068
	public float LightIntensity = 0.1f;

	// Token: 0x04003EC5 RID: 16069
	public float ShadowStrength = 1f;

	// Token: 0x04003EC6 RID: 16070
	public float Phase;

	// Token: 0x04003EC7 RID: 16071
	public TOD_MoonPositionType Position;
}
