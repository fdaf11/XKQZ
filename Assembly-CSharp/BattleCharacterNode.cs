using System;
using System.Collections.Generic;

// Token: 0x020000FC RID: 252
public class BattleCharacterNode
{
	// Token: 0x0400054E RID: 1358
	public int m_iCharID;

	// Token: 0x0400054F RID: 1359
	public string m_strName;

	// Token: 0x04000550 RID: 1360
	public string m_Rank;

	// Token: 0x04000551 RID: 1361
	public int m_iNeigong;

	// Token: 0x04000552 RID: 1362
	public int m_iHp;

	// Token: 0x04000553 RID: 1363
	public int m_iSp;

	// Token: 0x04000554 RID: 1364
	public float m_fCritical;

	// Token: 0x04000555 RID: 1365
	public float m_fDodge;

	// Token: 0x04000556 RID: 1366
	public float m_fCounter;

	// Token: 0x04000557 RID: 1367
	public int m_iMobility;

	// Token: 0x04000558 RID: 1368
	public int m_iDefance;

	// Token: 0x04000559 RID: 1369
	public List<int> m_iRoutineIDList = new List<int>();

	// Token: 0x0400055A RID: 1370
	public string m_strVoice;
}
