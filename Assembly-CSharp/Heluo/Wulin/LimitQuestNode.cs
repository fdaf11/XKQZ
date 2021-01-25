using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000240 RID: 576
	public class LimitQuestNode
	{
		// Token: 0x06000AC9 RID: 2761 RVA: 0x000088EB File Offset: 0x00006AEB
		public LimitQuestNode()
		{
			this.m_iNpcidEndList = new List<int>();
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00059E70 File Offset: 0x00058070
		public static void GetData(int idx, LimitQuestNode Node, string data)
		{
			if (idx >= 4)
			{
				idx = 4;
			}
			switch (idx)
			{
			case 0:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				Node.m_iIsLimit = num;
				break;
			}
			case 1:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				Node.m_iRound = num;
				break;
			}
			case 2:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				Node.m_iNpcCall = num;
				break;
			}
			case 3:
				Node.m_strNpcQuest = data;
				break;
			case 4:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				Node.m_iNpcidEndList.Add(num);
				break;
			}
			}
		}

		// Token: 0x04000BFB RID: 3067
		public int m_iIsLimit;

		// Token: 0x04000BFC RID: 3068
		public int m_iRound;

		// Token: 0x04000BFD RID: 3069
		public int m_iNpcCall;

		// Token: 0x04000BFE RID: 3070
		public string m_strNpcQuest;

		// Token: 0x04000BFF RID: 3071
		public List<int> m_iNpcidEndList;

		// Token: 0x02000241 RID: 577
		private enum eMember
		{
			// Token: 0x04000C01 RID: 3073
			isLimit,
			// Token: 0x04000C02 RID: 3074
			Round,
			// Token: 0x04000C03 RID: 3075
			NpcCall,
			// Token: 0x04000C04 RID: 3076
			NpcQuest,
			// Token: 0x04000C05 RID: 3077
			NpcEnd
		}
	}
}
