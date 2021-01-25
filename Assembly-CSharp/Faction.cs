using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200075D RID: 1885
[Serializable]
public class Faction
{
	// Token: 0x0400394D RID: 14669
	public int factionID = -1;

	// Token: 0x0400394E RID: 14670
	public string factionName = "faction";

	// Token: 0x0400394F RID: 14671
	public Texture icon;

	// Token: 0x04003950 RID: 14672
	public Color color;

	// Token: 0x04003951 RID: 14673
	public List<UnitTB> allUnitList = new List<UnitTB>();

	// Token: 0x04003952 RID: 14674
	public bool isPlayerControl;

	// Token: 0x04003953 RID: 14675
	public bool allUnitMoved;

	// Token: 0x04003954 RID: 14676
	public bool bFirstAction = true;

	// Token: 0x04003955 RID: 14677
	public int currentTurnID;

	// Token: 0x04003956 RID: 14678
	public List<int> unitYetToMove = new List<int>();

	// Token: 0x04003957 RID: 14679
	public List<FactionSpawnInfo> spawnInfo = new List<FactionSpawnInfo>();
}
