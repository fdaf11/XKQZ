using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x0200024A RID: 586
	public class QuestionMenuNode
	{
		// Token: 0x06000ADD RID: 2781 RVA: 0x000089B9 File Offset: 0x00006BB9
		public QuestionMenuNode()
		{
			this.m_QuestionRewardNodeList = new List<QuestionRewardNode>();
		}

		// Token: 0x04000C26 RID: 3110
		public int m_iQuestionGroupID;

		// Token: 0x04000C27 RID: 3111
		public List<QuestionRewardNode> m_QuestionRewardNodeList;

		// Token: 0x04000C28 RID: 3112
		public int m_iType;

		// Token: 0x04000C29 RID: 3113
		public string m_strID;

		// Token: 0x04000C2A RID: 3114
		public int m_iQuestionAmount;
	}
}
