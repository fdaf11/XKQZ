using System;
using UnityEngine;

// Token: 0x020002C3 RID: 707
public class NpcOnChangePosition : MonoBehaviour
{
	// Token: 0x06000E06 RID: 3590 RVA: 0x00009947 File Offset: 0x00007B47
	public void UpdatePositionDirect()
	{
		if (this.onPositionDirectChange != null)
		{
			this.onPositionDirectChange(base.transform);
		}
	}

	// Token: 0x04001056 RID: 4182
	public NpcOnChangePosition.OnPositionDirectChange onPositionDirectChange;

	// Token: 0x020002C4 RID: 708
	// (Invoke) Token: 0x06000E08 RID: 3592
	public delegate void OnPositionDirectChange(Transform trans);
}
