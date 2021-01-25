using System;
using System.Collections.Generic;

// Token: 0x020000F4 RID: 244
public class AbilityNode
{
	// Token: 0x0400051E RID: 1310
	public int m_iAbilityID;

	// Token: 0x0400051F RID: 1311
	public string m_strName;

	// Token: 0x04000520 RID: 1312
	public bool m_bNeedToSelectTarget;

	// Token: 0x04000521 RID: 1313
	public int m_iSkillType;

	// Token: 0x04000522 RID: 1314
	public int m_iTargetType;

	// Token: 0x04000523 RID: 1315
	public int m_iTargetArea;

	// Token: 0x04000524 RID: 1316
	public int m_iRange;

	// Token: 0x04000525 RID: 1317
	public int m_iAOE;

	// Token: 0x04000526 RID: 1318
	public int m_iValue1;

	// Token: 0x04000527 RID: 1319
	public int m_iValue2;

	// Token: 0x04000528 RID: 1320
	public int m_iRequestSP;

	// Token: 0x04000529 RID: 1321
	public int m_iCD;

	// Token: 0x0400052A RID: 1322
	public bool m_bUseSkillAfterMove;

	// Token: 0x0400052B RID: 1323
	public int m_iConditionID;

	// Token: 0x0400052C RID: 1324
	public List<int> m_iConditionIDList = new List<int>();

	// Token: 0x0400052D RID: 1325
	public List<int> m_iSkillEffectIDList = new List<int>();

	// Token: 0x0400052E RID: 1326
	public string m_strSkillIconName;
}
