using System;
using UnityEngine;

// Token: 0x0200082E RID: 2094
[Serializable]
public class TOD_AmbientParameters
{
	// Token: 0x060032FB RID: 13051 RVA: 0x0018901C File Offset: 0x0018721C
	public void CheckRange()
	{
		this.UpdateInterval = Mathf.Max(0f, this.UpdateInterval);
		this.Exposure = Mathf.Max(0f, this.Exposure);
		this.Resolution = Mathf.Clamp(Mathf.ClosestPowerOfTwo(this.Resolution), 2, 16);
	}

	// Token: 0x04003EDD RID: 16093
	public TOD_AmbientType Mode = TOD_AmbientType.Flat;

	// Token: 0x04003EDE RID: 16094
	public bool Directional = true;

	// Token: 0x04003EDF RID: 16095
	public float UpdateInterval = 1f;

	// Token: 0x04003EE0 RID: 16096
	public float Exposure = 1f;

	// Token: 0x04003EE1 RID: 16097
	public int Resolution = 8;

	// Token: 0x04003EE2 RID: 16098
	public bool bSkipCheckIntervalOnce;
}
