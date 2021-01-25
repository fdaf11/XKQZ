using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200075E RID: 1886
[Serializable]
public class FactionSpawnInfo
{
	// Token: 0x06002D05 RID: 11525 RVA: 0x0015C060 File Offset: 0x0015A260
	public FactionSpawnInfo()
	{
		this.area.width = 1f;
		this.area.height = 1f;
	}

	// Token: 0x04003958 RID: 14680
	public Rect area;

	// Token: 0x04003959 RID: 14681
	public _SpawnQuota spawnQuota;

	// Token: 0x0400395A RID: 14682
	public int budget;

	// Token: 0x0400395B RID: 14683
	public int unitCount;

	// Token: 0x0400395C RID: 14684
	public UnitTB[] unitPrefabs = new UnitTB[0];

	// Token: 0x0400395D RID: 14685
	public int[] unitPrefabsMax = new int[0];

	// Token: 0x0400395E RID: 14686
	public List<Tile> spawnTileList = new List<Tile>();

	// Token: 0x0400395F RID: 14687
	public bool showUnitPrefabList;

	// Token: 0x04003960 RID: 14688
	public int unitRotation;
}
