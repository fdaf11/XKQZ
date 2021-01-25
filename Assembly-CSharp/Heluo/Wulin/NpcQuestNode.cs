using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000239 RID: 569
	public class NpcQuestNode
	{
		// Token: 0x06000AB8 RID: 2744 RVA: 0x00008821 File Offset: 0x00006A21
		public NpcQuestNode()
		{
			this.m_NpcRewardList = new List<NpcReward>();
			this.m_NpcConditionList = new List<Condition>();
		}

		// Token: 0x04000BDD RID: 3037
		public string m_strQuestID;

		// Token: 0x04000BDE RID: 3038
		public bool m_bShow;

		// Token: 0x04000BDF RID: 3039
		public List<Condition> m_NpcConditionList;

		// Token: 0x04000BE0 RID: 3040
		public int m_iRound;

		// Token: 0x04000BE1 RID: 3041
		public List<NpcReward> m_NpcRewardList;

		// Token: 0x04000BE2 RID: 3042
		public string m_strNote;

		// Token: 0x04000BE3 RID: 3043
		public string m_NpcLines;

		// Token: 0x04000BE4 RID: 3044
		public bool m_bOnly;

		// Token: 0x0200023A RID: 570
		public enum eMember
		{
			// Token: 0x04000BE6 RID: 3046
			ID,
			// Token: 0x04000BE7 RID: 3047
			Show,
			// Token: 0x04000BE8 RID: 3048
			Condition,
			// Token: 0x04000BE9 RID: 3049
			Round,
			// Token: 0x04000BEA RID: 3050
			Reward,
			// Token: 0x04000BEB RID: 3051
			Note,
			// Token: 0x04000BEC RID: 3052
			Lines,
			// Token: 0x04000BED RID: 3053
			Only
		}
	}
}
