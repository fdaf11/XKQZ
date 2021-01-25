using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000247 RID: 583
	public class QuestionGroupNode
	{
		// Token: 0x06000AD5 RID: 2773 RVA: 0x00008963 File Offset: 0x00006B63
		public QuestionGroupNode()
		{
			this.m_QuestionNodeList = new List<QuestionNode>();
		}

		// Token: 0x04000C20 RID: 3104
		public int m_iQuestionGroupID;

		// Token: 0x04000C21 RID: 3105
		public List<QuestionNode> m_QuestionNodeList;
	}
}
