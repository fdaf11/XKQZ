using System;

namespace Heluo.Wulin
{
	// Token: 0x0200027A RID: 634
	public class TimeQuestStatus : QuestStatus
	{
		// Token: 0x06000BC5 RID: 3013 RVA: 0x00060F30 File Offset: 0x0005F130
		public new TimeQuestStatus Clone()
		{
			return new TimeQuestStatus
			{
				m_strQuestID = this.m_strQuestID,
				iType = this.iType,
				m_iOpenTime = this.m_iOpenTime,
				m_failTime = this.m_failTime
			};
		}

		// Token: 0x04000D7A RID: 3450
		public int m_iOpenTime;

		// Token: 0x04000D7B RID: 3451
		public int m_failTime;
	}
}
