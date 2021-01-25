using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x0200022D RID: 557
	public class NeigongUpValueNode
	{
		// Token: 0x06000A96 RID: 2710 RVA: 0x000086A9 File Offset: 0x000068A9
		public NeigongUpValueNode()
		{
			this.m_UpValueList = new List<UpValueNode>();
		}

		// Token: 0x04000B97 RID: 2967
		public int m_iID;

		// Token: 0x04000B98 RID: 2968
		public List<UpValueNode> m_UpValueList;
	}
}
