using System;

namespace Heluo.Wulin
{
	// Token: 0x020001E2 RID: 482
	public class BiographiesActionNode
	{
		// Token: 0x060009D9 RID: 2521 RVA: 0x00053534 File Offset: 0x00051734
		public BiographiesActionNode(string[] data)
		{
			int eFadeType = 0;
			int.TryParse(data[0], ref eFadeType);
			this.m_eFadeType = (BiographiesActionNode.eFade)eFadeType;
			float.TryParse(data[1], ref this.m_fFadeTime);
			int eDisplacement = 0;
			int.TryParse(data[2], ref eDisplacement);
			this.m_eDisplacement = (BiographiesActionNode.eDisplacement)eDisplacement;
			float.TryParse(data[3], ref this.m_fDisTime);
		}

		// Token: 0x040009D2 RID: 2514
		public BiographiesActionNode.eFade m_eFadeType;

		// Token: 0x040009D3 RID: 2515
		public float m_fFadeTime;

		// Token: 0x040009D4 RID: 2516
		public BiographiesActionNode.eDisplacement m_eDisplacement;

		// Token: 0x040009D5 RID: 2517
		public float m_fDisTime;

		// Token: 0x020001E3 RID: 483
		public enum eFade
		{
			// Token: 0x040009D7 RID: 2519
			None,
			// Token: 0x040009D8 RID: 2520
			FadeIn,
			// Token: 0x040009D9 RID: 2521
			FadeOut
		}

		// Token: 0x020001E4 RID: 484
		public enum eDisplacement
		{
			// Token: 0x040009DB RID: 2523
			None,
			// Token: 0x040009DC RID: 2524
			LeftIn,
			// Token: 0x040009DD RID: 2525
			BottomIn,
			// Token: 0x040009DE RID: 2526
			RightIn,
			// Token: 0x040009DF RID: 2527
			TopIn,
			// Token: 0x040009E0 RID: 2528
			LeftOut,
			// Token: 0x040009E1 RID: 2529
			BottomOut,
			// Token: 0x040009E2 RID: 2530
			RightOut,
			// Token: 0x040009E3 RID: 2531
			TopOut
		}
	}
}
