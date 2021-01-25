using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x0200023D RID: 573
	public class NpcRandomNode
	{
		// Token: 0x06000AC1 RID: 2753 RVA: 0x00008882 File Offset: 0x00006A82
		public NpcRandomNode()
		{
			this.m_ReandomEventList = new List<NpcReandomQuest>();
		}

		// Token: 0x04000BF3 RID: 3059
		public int m_iOrder;

		// Token: 0x04000BF4 RID: 3060
		public string m_strStartQuest;

		// Token: 0x04000BF5 RID: 3061
		public string m_strOverQuest;

		// Token: 0x04000BF6 RID: 3062
		public List<NpcReandomQuest> m_ReandomEventList;
	}
}
