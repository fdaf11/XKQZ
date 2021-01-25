using System;
using UnityEngine;

// Token: 0x02000733 RID: 1843
[Serializable]
public class AudioObject
{
	// Token: 0x06002BA7 RID: 11175 RVA: 0x0001C27A File Offset: 0x0001A47A
	public AudioObject(AudioSource src, Transform t)
	{
		this.source = src;
		this.thisT = t;
	}

	// Token: 0x0400384A RID: 14410
	public AudioSource source;

	// Token: 0x0400384B RID: 14411
	public bool inUse;

	// Token: 0x0400384C RID: 14412
	public Transform thisT;
}
