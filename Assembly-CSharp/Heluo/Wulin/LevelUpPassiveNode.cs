using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000212 RID: 530
	public class LevelUpPassiveNode
	{
		// Token: 0x06000A4A RID: 2634 RVA: 0x0000833B File Offset: 0x0000653B
		public LevelUpPassiveNode()
		{
			this.m_RequestConditionList = new List<Condition>();
			this.m_PassiveNodeList = new List<PassiveNode>();
		}

		// Token: 0x04000AF5 RID: 2805
		public int iID;

		// Token: 0x04000AF6 RID: 2806
		public string strName;

		// Token: 0x04000AF7 RID: 2807
		public string strIcon;

		// Token: 0x04000AF8 RID: 2808
		public bool bAuto;

		// Token: 0x04000AF9 RID: 2809
		public List<Condition> m_RequestConditionList;

		// Token: 0x04000AFA RID: 2810
		public List<PassiveNode> m_PassiveNodeList;

		// Token: 0x04000AFB RID: 2811
		public string strDesc;

		// Token: 0x04000AFC RID: 2812
		public int price;

		// Token: 0x02000213 RID: 531
		public enum eMember
		{
			// Token: 0x04000AFE RID: 2814
			ID,
			// Token: 0x04000AFF RID: 2815
			Name,
			// Token: 0x04000B00 RID: 2816
			IconName,
			// Token: 0x04000B01 RID: 2817
			Auto,
			// Token: 0x04000B02 RID: 2818
			RequestCondition,
			// Token: 0x04000B03 RID: 2819
			Passive,
			// Token: 0x04000B04 RID: 2820
			Desc,
			// Token: 0x04000B05 RID: 2821
			Price
		}
	}
}
