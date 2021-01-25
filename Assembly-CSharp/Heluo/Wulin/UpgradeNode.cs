using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x0200026B RID: 619
	public class UpgradeNode
	{
		// Token: 0x06000B5C RID: 2908 RVA: 0x00008EB3 File Offset: 0x000070B3
		public UpgradeNode()
		{
			this.m_HidePassiveNodeList = new List<int>();
			this.m_PassiveTreeNodeList = new List<PassiveTreeNode>();
		}

		// Token: 0x04000D2E RID: 3374
		public int iCharID;

		// Token: 0x04000D2F RID: 3375
		public List<int> m_HidePassiveNodeList;

		// Token: 0x04000D30 RID: 3376
		public List<PassiveTreeNode> m_PassiveTreeNodeList;
	}
}
