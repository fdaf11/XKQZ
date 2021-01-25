using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001E7 RID: 487
	public class BiographiesNode
	{
		// Token: 0x060009DC RID: 2524 RVA: 0x00007F1E File Offset: 0x0000611E
		public BiographiesNode()
		{
			this.m_BiographiesNpcQHeadNodeList = new List<BiographiesNpcQHeadNode>();
		}

		// Token: 0x040009ED RID: 2541
		public int m_iOrder;

		// Token: 0x040009EE RID: 2542
		public int m_iNpcID;

		// Token: 0x040009EF RID: 2543
		public string m_Message;

		// Token: 0x040009F0 RID: 2544
		public string m_Voice;

		// Token: 0x040009F1 RID: 2545
		public string m_Image;

		// Token: 0x040009F2 RID: 2546
		public string m_BackgroundImage;

		// Token: 0x040009F3 RID: 2547
		public BiographiesNode.eEndMovie m_EndMovie;

		// Token: 0x040009F4 RID: 2548
		public BiographiesNode.eMsgPos m_eMsgPlace;

		// Token: 0x040009F5 RID: 2549
		public List<BiographiesNpcQHeadNode> m_BiographiesNpcQHeadNodeList;

		// Token: 0x040009F6 RID: 2550
		public int m_BiographiesReward;

		// Token: 0x020001E8 RID: 488
		public enum eEndMovie
		{
			// Token: 0x040009F8 RID: 2552
			None,
			// Token: 0x040009F9 RID: 2553
			Over,
			// Token: 0x040009FA RID: 2554
			OverAndNight
		}

		// Token: 0x020001E9 RID: 489
		public enum eMsgPos
		{
			// Token: 0x040009FC RID: 2556
			None,
			// Token: 0x040009FD RID: 2557
			Left,
			// Token: 0x040009FE RID: 2558
			MediumLeft,
			// Token: 0x040009FF RID: 2559
			MediumRight,
			// Token: 0x04000A00 RID: 2560
			Right,
			// Token: 0x04000A01 RID: 2561
			BackLeft,
			// Token: 0x04000A02 RID: 2562
			BackRight
		}
	}
}
