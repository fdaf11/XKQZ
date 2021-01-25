using System;
using UnityEngine;

// Token: 0x020004A9 RID: 1193
public class RealTime : MonoBehaviour
{
	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06001D82 RID: 7554 RVA: 0x00013876 File Offset: 0x00011A76
	public static float time
	{
		get
		{
			return Time.unscaledTime;
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06001D83 RID: 7555 RVA: 0x0001387D File Offset: 0x00011A7D
	public static float deltaTime
	{
		get
		{
			return Time.unscaledDeltaTime;
		}
	}
}
