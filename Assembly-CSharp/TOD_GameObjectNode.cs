using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000820 RID: 2080
[Serializable]
public class TOD_GameObjectNode
{
	// Token: 0x04003E86 RID: 16006
	public float fStartActiveTime;

	// Token: 0x04003E87 RID: 16007
	public float fEndActiveTime;

	// Token: 0x04003E88 RID: 16008
	public bool bActive;

	// Token: 0x04003E89 RID: 16009
	public List<GameObject> goList = new List<GameObject>();
}
