using System;
using UnityEngine;

// Token: 0x02000825 RID: 2085
[Serializable]
public class TOD_StarParameters
{
	// Token: 0x060032E9 RID: 13033 RVA: 0x0001FE7D File Offset: 0x0001E07D
	public void CheckRange()
	{
		this.Tiling = Mathf.Max(0f, this.Tiling);
		this.Brightness = Mathf.Max(0f, this.Brightness);
	}

	// Token: 0x04003EA9 RID: 16041
	public float Tiling = 2f;

	// Token: 0x04003EAA RID: 16042
	public float Brightness = 2f;

	// Token: 0x04003EAB RID: 16043
	public TOD_StarsPositionType Position = TOD_StarsPositionType.Rotating;
}
