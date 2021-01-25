using System;
using UnityEngine;

// Token: 0x02000841 RID: 2113
public class DeviceTime : MonoBehaviour
{
	// Token: 0x06003368 RID: 13160 RVA: 0x0002040C File Offset: 0x0001E60C
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.sky.Cycle.DateTime = DateTime.Now;
	}

	// Token: 0x04003F8D RID: 16269
	public TOD_Sky sky;
}
