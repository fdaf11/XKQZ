using System;

namespace Heluo.Wulin
{
	// Token: 0x020001D6 RID: 470
	public class BookNode
	{
		// Token: 0x04000998 RID: 2456
		public int m_iID;

		// Token: 0x04000999 RID: 2457
		public int m_iAbilityType;

		// Token: 0x0400099A RID: 2458
		public string m_strAbilityID;

		// Token: 0x0400099B RID: 2459
		public string m_strBookMsg;

		// Token: 0x0400099C RID: 2460
		public string m_strBookImage;

		// Token: 0x0400099D RID: 2461
		public string m_sValueLink;

		// Token: 0x0400099E RID: 2462
		public int m_iSkillful;

		// Token: 0x020001D7 RID: 471
		public enum eMember
		{
			// Token: 0x040009A0 RID: 2464
			ID,
			// Token: 0x040009A1 RID: 2465
			AbilityType,
			// Token: 0x040009A2 RID: 2466
			AbilityID,
			// Token: 0x040009A3 RID: 2467
			BookMsg,
			// Token: 0x040009A4 RID: 2468
			BookImage,
			// Token: 0x040009A5 RID: 2469
			ValueLink,
			// Token: 0x040009A6 RID: 2470
			Skillful
		}
	}
}
