using System;
using System.Collections.Generic;

// Token: 0x02000117 RID: 279
public class BattleTacticNode
{
	// Token: 0x060005AE RID: 1454 RVA: 0x00041D10 File Offset: 0x0003FF10
	public bool CheckUnLock()
	{
		for (int i = 0; i < this.lUnlockNodeList.Count; i++)
		{
			if (!this.lUnlockNodeList[i].CheckPass())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0400061B RID: 1563
	public int iNodeID;

	// Token: 0x0400061C RID: 1564
	public string sName;

	// Token: 0x0400061D RID: 1565
	public string sDesc;

	// Token: 0x0400061E RID: 1566
	public List<BattleUnLockNode> lUnlockNodeList = new List<BattleUnLockNode>();

	// Token: 0x0400061F RID: 1567
	public int iTacticPoint;

	// Token: 0x04000620 RID: 1568
	public int iTargetFaction;

	// Token: 0x04000621 RID: 1569
	public int iTargetType;

	// Token: 0x04000622 RID: 1570
	public int iTargetWeapon;

	// Token: 0x04000623 RID: 1571
	public List<int> lConditionIDList = new List<int>();
}
