using System;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class DestroyForTime : MonoBehaviour
{
	// Token: 0x060006E3 RID: 1763 RVA: 0x00006221 File Offset: 0x00004421
	private void Start()
	{
		Object.Destroy(base.gameObject, this.time);
	}

	// Token: 0x0400074E RID: 1870
	public float time;
}
