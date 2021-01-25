using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000268 RID: 616
	public class TreasureBoxGroup
	{
		// Token: 0x06000B51 RID: 2897 RVA: 0x00008E36 File Offset: 0x00007036
		public TreasureBoxGroup()
		{
			this.m_SceneTBList = new List<TreasureBoxNode>();
		}

		// Token: 0x04000D28 RID: 3368
		public string m_strSceneID;

		// Token: 0x04000D29 RID: 3369
		public List<TreasureBoxNode> m_SceneTBList;
	}
}
