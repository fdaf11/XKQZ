using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000242 RID: 578
	public class QuestNode
	{
		// Token: 0x06000ACB RID: 2763 RVA: 0x000088FE File Offset: 0x00006AFE
		public QuestNode()
		{
			this.m_QuestOpenNodeList = new List<Condition>();
			this.m_FinshQuestNodeList = new List<Condition>();
			this.m_LimitQuest = new LimitQuestNode();
		}

		// Token: 0x04000C06 RID: 3078
		public string m_strQuestID;

		// Token: 0x04000C07 RID: 3079
		public string m_strQuestName;

		// Token: 0x04000C08 RID: 3080
		public QuestNode.eQuestType m_eType;

		// Token: 0x04000C09 RID: 3081
		public string m_strQuestTip;

		// Token: 0x04000C0A RID: 3082
		public LimitQuestNode m_LimitQuest;

		// Token: 0x04000C0B RID: 3083
		public List<Condition> m_QuestOpenNodeList;

		// Token: 0x04000C0C RID: 3084
		public List<Condition> m_FinshQuestNodeList;

		// Token: 0x04000C0D RID: 3085
		public string m_strGetManager;

		// Token: 0x04000C0E RID: 3086
		public string m_strQuestIngManager;

		// Token: 0x04000C0F RID: 3087
		public string m_strFinshQuestManager;

		// Token: 0x04000C10 RID: 3088
		public int m_iGiftID;

		// Token: 0x04000C11 RID: 3089
		public bool m_bRepeat;

		// Token: 0x02000243 RID: 579
		public enum eQuestType
		{
			// Token: 0x04000C13 RID: 3091
			HideUI,
			// Token: 0x04000C14 RID: 3092
			ShowUI,
			// Token: 0x04000C15 RID: 3093
			BigMap,
			// Token: 0x04000C16 RID: 3094
			MouseEvent
		}
	}
}
