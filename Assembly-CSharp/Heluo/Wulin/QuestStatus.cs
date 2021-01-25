using System;
using Newtonsoft.Json;

namespace Heluo.Wulin
{
	// Token: 0x02000279 RID: 633
	public class QuestStatus
	{
		// Token: 0x06000BC3 RID: 3011 RVA: 0x00060F04 File Offset: 0x0005F104
		public QuestStatus Clone()
		{
			return new QuestStatus
			{
				m_strQuestID = this.m_strQuestID,
				iType = this.iType
			};
		}

		// Token: 0x04000D77 RID: 3447
		public string m_strQuestID;

		// Token: 0x04000D78 RID: 3448
		public int iType;

		// Token: 0x04000D79 RID: 3449
		[JsonIgnore]
		public bool bfinish;
	}
}
