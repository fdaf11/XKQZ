using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Heluo.Wulin
{
	// Token: 0x0200025B RID: 603
	public class StoreItemNode
	{
		// Token: 0x06000B16 RID: 2838 RVA: 0x00008C26 File Offset: 0x00006E26
		public StoreItemNode()
		{
			this.m_ConditionList = new List<Condition>();
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0005C49C File Offset: 0x0005A69C
		public StoreItemNode Clone()
		{
			return new StoreItemNode
			{
				m_iItemID = this.m_iItemID,
				m_iBuyAmount = this.m_iBuyAmount,
				m_iProbability = this.m_iProbability,
				bAnd = this.bAnd,
				m_ConditionList = this.m_ConditionList
			};
		}

		// Token: 0x04000C98 RID: 3224
		[JsonIgnore]
		public bool bAnd;

		// Token: 0x04000C99 RID: 3225
		[JsonIgnore]
		public List<Condition> m_ConditionList;

		// Token: 0x04000C9A RID: 3226
		public int m_iItemID;

		// Token: 0x04000C9B RID: 3227
		public int m_iBuyAmount;

		// Token: 0x04000C9C RID: 3228
		public int m_iProbability;
	}
}
