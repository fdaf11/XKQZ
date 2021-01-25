using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000226 RID: 550
	public class MoodTalkGroup
	{
		// Token: 0x06000A7B RID: 2683 RVA: 0x0000859A File Offset: 0x0000679A
		public MoodTalkGroup()
		{
			this.m_MoodTalkNodeList = new List<MoodTalkNode>();
		}

		// Token: 0x04000B72 RID: 2930
		public string m_strMoodID;

		// Token: 0x04000B73 RID: 2931
		public List<MoodTalkNode> m_MoodTalkNodeList;
	}
}
