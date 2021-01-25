using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001FE RID: 510
	public class FameNode
	{
		// Token: 0x06000A1B RID: 2587 RVA: 0x00008171 File Offset: 0x00006371
		public FameNode()
		{
			this.LevelUpConditionList = new List<Condition>();
		}

		// Token: 0x04000A80 RID: 2688
		public int iFame;

		// Token: 0x04000A81 RID: 2689
		public string strDesc;

		// Token: 0x04000A82 RID: 2690
		public List<Condition> LevelUpConditionList;

		// Token: 0x04000A83 RID: 2691
		public int iReward;
	}
}
