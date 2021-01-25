using System;
using UnityEngine;

// Token: 0x0200082A RID: 2090
[Serializable]
public class TOD_LightParameters
{
	// Token: 0x060032F3 RID: 13043 RVA: 0x00188E2C File Offset: 0x0018702C
	public void CheckRange()
	{
		this.UpdateInterval = Mathf.Max(0f, this.UpdateInterval);
		this.MinimumHeight = Mathf.Clamp(this.MinimumHeight, -1f, 1f);
		this.Falloff = Mathf.Clamp01(this.Falloff);
		this.Coloring = Mathf.Clamp01(this.Coloring);
		this.SkyColoring = Mathf.Clamp01(this.SkyColoring);
		this.CloudColoring = Mathf.Clamp01(this.CloudColoring);
		this.ShaftColoring = Mathf.Clamp01(this.ShaftColoring);
		this.AmbientColoring = Mathf.Clamp01(this.AmbientColoring);
	}

	// Token: 0x04003EC8 RID: 16072
	public float UpdateInterval;

	// Token: 0x04003EC9 RID: 16073
	public float MinimumHeight;

	// Token: 0x04003ECA RID: 16074
	public float Falloff = 0.75f;

	// Token: 0x04003ECB RID: 16075
	public float Coloring = 0.75f;

	// Token: 0x04003ECC RID: 16076
	public float SkyColoring = 0.5f;

	// Token: 0x04003ECD RID: 16077
	public float CloudColoring = 0.75f;

	// Token: 0x04003ECE RID: 16078
	public float ShaftColoring = 0.75f;

	// Token: 0x04003ECF RID: 16079
	public float AmbientColoring = 0.5f;
}
