using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001FC RID: 508
	public class ChildEndDataNode
	{
		// Token: 0x06000A15 RID: 2581 RVA: 0x0000811B File Offset: 0x0000631B
		public ChildEndDataNode()
		{
			this.m_OpenConditionlist = new List<Condition>();
		}

		// Token: 0x04000A7B RID: 2683
		public int m_iEndID;

		// Token: 0x04000A7C RID: 2684
		public List<Condition> m_OpenConditionlist;

		// Token: 0x04000A7D RID: 2685
		public string m_strDesc;
	}
}
