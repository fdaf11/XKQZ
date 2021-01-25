using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001DA RID: 474
	public class AchievementKindNode
	{
		// Token: 0x060009B4 RID: 2484 RVA: 0x00007DFA File Offset: 0x00005FFA
		public AchievementKindNode()
		{
			this.m_AchiDataNodeList = new List<AchievementDataNode>();
		}

		// Token: 0x040009B5 RID: 2485
		public int m_iAchiType;

		// Token: 0x040009B6 RID: 2486
		public int m_iFinishCount;

		// Token: 0x040009B7 RID: 2487
		public List<AchievementDataNode> m_AchiDataNodeList;
	}
}
