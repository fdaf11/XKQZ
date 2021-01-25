using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x0200023E RID: 574
	public class NpcRandomGroup
	{
		// Token: 0x06000AC2 RID: 2754 RVA: 0x00008895 File Offset: 0x00006A95
		public NpcRandomGroup()
		{
			this.m_NpcRandomEvent = new List<NpcRandomNode>();
		}

		// Token: 0x04000BF7 RID: 3063
		public int NpcID;

		// Token: 0x04000BF8 RID: 3064
		public List<NpcRandomNode> m_NpcRandomEvent;
	}
}
