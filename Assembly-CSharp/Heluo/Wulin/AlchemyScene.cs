using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001DD RID: 477
	public class AlchemyScene
	{
		// Token: 0x060009C8 RID: 2504 RVA: 0x00007E50 File Offset: 0x00006050
		public AlchemyScene()
		{
			this.m_iTileList = new List<int>();
		}

		// Token: 0x040009BC RID: 2492
		public int m_iAbilityBookID;

		// Token: 0x040009BD RID: 2493
		public int m_iSkilllevel;

		// Token: 0x040009BE RID: 2494
		public int m_iMoveCount;

		// Token: 0x040009BF RID: 2495
		public int m_iMarkTarget;

		// Token: 0x040009C0 RID: 2496
		public int m_iMarkTargetCount;

		// Token: 0x040009C1 RID: 2497
		public int m_iSuccessItemID;

		// Token: 0x040009C2 RID: 2498
		public int m_iMaxItemCount;

		// Token: 0x040009C3 RID: 2499
		public int m_iWidth;

		// Token: 0x040009C4 RID: 2500
		public int m_iHeight;

		// Token: 0x040009C5 RID: 2501
		public List<int> m_iTileList;
	}
}
