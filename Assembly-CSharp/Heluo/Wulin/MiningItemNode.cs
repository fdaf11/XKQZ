using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001C9 RID: 457
	public class MiningItemNode
	{
		// Token: 0x06000975 RID: 2421 RVA: 0x00007BD9 File Offset: 0x00005DD9
		public MiningItemNode()
		{
			this.m_ProbabilityList = new List<int>();
		}

		// Token: 0x0400095D RID: 2397
		public List<int> m_ProbabilityList;

		// Token: 0x0400095E RID: 2398
		public int m_iItemID;

		// Token: 0x0400095F RID: 2399
		public int m_iGetAmount;

		// Token: 0x04000960 RID: 2400
		public string m_strImageID;

		// Token: 0x04000961 RID: 2401
		public int m_iType;

		// Token: 0x04000962 RID: 2402
		public string m_strMapID;

		// Token: 0x04000963 RID: 2403
		public int m_iBatter;
	}
}
