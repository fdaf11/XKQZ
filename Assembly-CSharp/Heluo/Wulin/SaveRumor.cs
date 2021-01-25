using System;

namespace Heluo.Wulin
{
	// Token: 0x020002C8 RID: 712
	public class SaveRumor
	{
		// Token: 0x06000E20 RID: 3616 RVA: 0x00009A09 File Offset: 0x00007C09
		public SaveRumor(int NPCID, string RumorQuestID, SaveRumor.RumorType _type)
		{
			this.m_id = NPCID;
			this.m_RumorQuest = RumorQuestID;
			this.m_type = _type;
		}

		// Token: 0x04001064 RID: 4196
		public int m_id;

		// Token: 0x04001065 RID: 4197
		public string m_RumorQuest;

		// Token: 0x04001066 RID: 4198
		public SaveRumor.RumorType m_type;

		// Token: 0x020002C9 RID: 713
		public enum RumorType
		{
			// Token: 0x04001068 RID: 4200
			NpcQuset,
			// Token: 0x04001069 RID: 4201
			Quest
		}
	}
}
