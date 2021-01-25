using System;

namespace Heluo.Wulin
{
	// Token: 0x020001CC RID: 460
	public class NpcIsFought
	{
		// Token: 0x0600097D RID: 2429 RVA: 0x00051260 File Offset: 0x0004F460
		public NpcIsFought Clone()
		{
			return new NpcIsFought
			{
				m_Npc = this.m_Npc,
				m_MapName = this.m_MapName,
				ReSetRound = this.ReSetRound,
				iStay = this.iStay
			};
		}

		// Token: 0x0400096C RID: 2412
		public string m_Npc;

		// Token: 0x0400096D RID: 2413
		public string m_MapName;

		// Token: 0x0400096E RID: 2414
		public int ReSetRound;

		// Token: 0x0400096F RID: 2415
		public int iStay;
	}
}
