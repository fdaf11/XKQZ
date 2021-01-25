using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001DE RID: 478
	public class AlchemyProduceNode
	{
		// Token: 0x060009C9 RID: 2505 RVA: 0x00007E63 File Offset: 0x00006063
		public AlchemyProduceNode()
		{
			this.m_MaterialNodeList = new List<MaterialNode>();
		}

		// Token: 0x040009C6 RID: 2502
		public int m_iAbilityBookID;

		// Token: 0x040009C7 RID: 2503
		public int m_iType;

		// Token: 0x040009C8 RID: 2504
		public List<MaterialNode> m_MaterialNodeList;

		// Token: 0x040009C9 RID: 2505
		public int m_iRequestSkill;

		// Token: 0x040009CA RID: 2506
		public string m_strIcon;
	}
}
