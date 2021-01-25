using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001D9 RID: 473
	public class AchievementDataNode
	{
		// Token: 0x060009B3 RID: 2483 RVA: 0x00007DDC File Offset: 0x00005FDC
		public AchievementDataNode()
		{
			this.m_checkValueList = new List<int>();
			this.m_checkStringList = new List<string>();
		}

		// Token: 0x040009A9 RID: 2473
		public int m_iID;

		// Token: 0x040009AA RID: 2474
		public string m_strAchName;

		// Token: 0x040009AB RID: 2475
		public string m_strAchExplain;

		// Token: 0x040009AC RID: 2476
		public string m_strUIImage;

		// Token: 0x040009AD RID: 2477
		public string m_strEndMove;

		// Token: 0x040009AE RID: 2478
		public int m_iCheckType;

		// Token: 0x040009AF RID: 2479
		public string m_strCheckValue;

		// Token: 0x040009B0 RID: 2480
		public int m_iCount;

		// Token: 0x040009B1 RID: 2481
		public int m_iNow;

		// Token: 0x040009B2 RID: 2482
		public int m_iOpenType;

		// Token: 0x040009B3 RID: 2483
		public List<int> m_checkValueList;

		// Token: 0x040009B4 RID: 2484
		public List<string> m_checkStringList;
	}
}
