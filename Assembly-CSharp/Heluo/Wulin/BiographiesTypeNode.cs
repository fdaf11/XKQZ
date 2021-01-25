using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001EA RID: 490
	public class BiographiesTypeNode
	{
		// Token: 0x060009DD RID: 2525 RVA: 0x00007F31 File Offset: 0x00006131
		public BiographiesTypeNode()
		{
			this.m_BiographiesNodeList = new List<BiographiesNode>();
		}

		// Token: 0x04000A03 RID: 2563
		public string m_BiographiesGroupID;

		// Token: 0x04000A04 RID: 2564
		public List<BiographiesNode> m_BiographiesNodeList;
	}
}
