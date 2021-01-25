using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001B0 RID: 432
	public class HerbsItemGroupNode
	{
		// Token: 0x06000913 RID: 2323 RVA: 0x0000777A File Offset: 0x0000597A
		public HerbsItemGroupNode()
		{
			this.m_HerbsItemGroupList = new List<HerbsItemNode>();
		}

		// Token: 0x040008AA RID: 2218
		public int m_iGroupID;

		// Token: 0x040008AB RID: 2219
		public List<HerbsItemNode> m_HerbsItemGroupList;
	}
}
