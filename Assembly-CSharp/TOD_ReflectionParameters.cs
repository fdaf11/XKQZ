using System;
using UnityEngine;

// Token: 0x0200082F RID: 2095
[Serializable]
public class TOD_ReflectionParameters
{
	// Token: 0x060032FD RID: 13053 RVA: 0x00189070 File Offset: 0x00187270
	public void CheckRange()
	{
		this.UpdateInterval = Mathf.Max(0f, this.UpdateInterval);
		this.Exposure = Mathf.Max(0f, this.Exposure);
		this.Resolution = Mathf.Clamp(Mathf.ClosestPowerOfTwo(this.Resolution), 2, 16);
	}

	// Token: 0x04003EE3 RID: 16099
	public TOD_ReflectionType Mode;

	// Token: 0x04003EE4 RID: 16100
	public bool Directional = true;

	// Token: 0x04003EE5 RID: 16101
	public float UpdateInterval = 1f;

	// Token: 0x04003EE6 RID: 16102
	public float Exposure = 1f;

	// Token: 0x04003EE7 RID: 16103
	public int Resolution = 8;
}
