using System;
using System.Collections.Generic;

// Token: 0x020000FA RID: 250
public class BattleAreaNode
{
	// Token: 0x04000540 RID: 1344
	public int m_iID;

	// Token: 0x04000541 RID: 1345
	public string m_sMapName;

	// Token: 0x04000542 RID: 1346
	public List<BattleJoinCharacterNode> m_TeamList = new List<BattleJoinCharacterNode>();

	// Token: 0x04000543 RID: 1347
	public List<BattleJoinCharacterNode> m_EnemyList = new List<BattleJoinCharacterNode>();

	// Token: 0x04000544 RID: 1348
	public List<WinLoseRequirement> m_VicReqList = new List<WinLoseRequirement>();

	// Token: 0x04000545 RID: 1349
	public List<WinLoseRequirement> m_FailReqList = new List<WinLoseRequirement>();

	// Token: 0x04000546 RID: 1350
	public int m_iRewardID;

	// Token: 0x04000547 RID: 1351
	public int m_iFailResultID;

	// Token: 0x04000548 RID: 1352
	public int m_iTime;

	// Token: 0x04000549 RID: 1353
	public string m_sSoundName;

	// Token: 0x0400054A RID: 1354
	public int m_iInside;

	// Token: 0x0400054B RID: 1355
	public int m_iSave;

	// Token: 0x0400054C RID: 1356
	public List<DropItem> m_DropItemList = new List<DropItem>();
}
