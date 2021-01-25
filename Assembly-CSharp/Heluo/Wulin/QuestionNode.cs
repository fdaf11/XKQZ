using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000246 RID: 582
	public class QuestionNode
	{
		// Token: 0x06000AD4 RID: 2772 RVA: 0x00008950 File Offset: 0x00006B50
		public QuestionNode()
		{
			this.m_QuestionBtnList = new List<QuestionBtnNode>();
		}

		// Token: 0x04000C1E RID: 3102
		public string m_strQuestionMsg;

		// Token: 0x04000C1F RID: 3103
		public List<QuestionBtnNode> m_QuestionBtnList;
	}
}
