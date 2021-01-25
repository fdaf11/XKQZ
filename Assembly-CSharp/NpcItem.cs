using System;

// Token: 0x020001EC RID: 492
public class NpcItem
{
	// Token: 0x060009E7 RID: 2535 RVA: 0x000539C4 File Offset: 0x00051BC4
	public NpcItem Clone()
	{
		return new NpcItem
		{
			m_iItemID = this.m_iItemID,
			m_iCount = this.m_iCount
		};
	}

	// Token: 0x04000A08 RID: 2568
	public int m_iItemID;

	// Token: 0x04000A09 RID: 2569
	public int m_iCount;
}
