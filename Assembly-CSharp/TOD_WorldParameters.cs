using System;
using UnityEngine;

// Token: 0x0200082C RID: 2092
[Serializable]
public class TOD_WorldParameters
{
	// Token: 0x060032F7 RID: 13047 RVA: 0x0001FEF3 File Offset: 0x0001E0F3
	public void CheckRange()
	{
		this.ViewerHeight = Mathf.Clamp01(this.ViewerHeight);
		this.HorizonOffset = Mathf.Clamp01(this.HorizonOffset);
	}

	// Token: 0x04003ED7 RID: 16087
	public float ViewerHeight;

	// Token: 0x04003ED8 RID: 16088
	public float HorizonOffset;
}
