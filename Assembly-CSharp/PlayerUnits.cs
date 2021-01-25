using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000772 RID: 1906
[Serializable]
public class PlayerUnits
{
	// Token: 0x06002D68 RID: 11624 RVA: 0x0001D3B9 File Offset: 0x0001B5B9
	public PlayerUnits(int ID)
	{
		this.factionID = ID;
	}

	// Token: 0x040039D4 RID: 14804
	[HideInInspector]
	public int factionID = -1;

	// Token: 0x040039D5 RID: 14805
	public List<UnitTB> starting = new List<UnitTB>();

	// Token: 0x040039D6 RID: 14806
	[HideInInspector]
	public bool showInInspector;
}
