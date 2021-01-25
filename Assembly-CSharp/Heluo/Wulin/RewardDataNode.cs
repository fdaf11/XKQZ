using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x0200024D RID: 589
	public class RewardDataNode
	{
		// Token: 0x06000AE5 RID: 2789 RVA: 0x00008A0F File Offset: 0x00006C0F
		public RewardDataNode()
		{
			this.m_MapRewardNodeList = new List<MapRewardNode>();
		}

		// Token: 0x04000C32 RID: 3122
		public int m_iRewardID;

		// Token: 0x04000C33 RID: 3123
		public List<MapRewardNode> m_MapRewardNodeList;
	}
}
