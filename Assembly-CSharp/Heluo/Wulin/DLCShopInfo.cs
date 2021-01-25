using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x0200080B RID: 2059
	public class DLCShopInfo
	{
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600326A RID: 12906 RVA: 0x0001FA71 File Offset: 0x0001DC71
		public static DLCShopInfo Singleton
		{
			get
			{
				return DLCShopInfo.instance;
			}
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x00186488 File Offset: 0x00184688
		public DLCShopInfo Clone()
		{
			DLCShopInfo dlcshopInfo = new DLCShopInfo();
			dlcshopInfo.m_iRound = this.m_iRound;
			dlcshopInfo.m_Update = this.m_Update;
			dlcshopInfo.m_UseInfo = this.m_UseInfo;
			for (int i = 0; i < this.m_ItemList.Count; i++)
			{
				dlcshopInfo.m_ItemList.Add(this.m_ItemList[i]);
			}
			return dlcshopInfo;
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x001864F4 File Offset: 0x001846F4
		public void Copy(DLCShopInfo info)
		{
			this.m_ItemList.Clear();
			if (info == null)
			{
				return;
			}
			this.m_iRound = info.m_iRound;
			this.m_Update = info.m_Update;
			this.m_UseInfo = info.m_UseInfo;
			for (int i = 0; i < info.m_ItemList.Count; i++)
			{
				this.m_ItemList.Add(info.m_ItemList[i]);
			}
		}

		// Token: 0x04003E0E RID: 15886
		private static readonly DLCShopInfo instance = new DLCShopInfo();

		// Token: 0x04003E0F RID: 15887
		public int m_iRound;

		// Token: 0x04003E10 RID: 15888
		public bool m_Update;

		// Token: 0x04003E11 RID: 15889
		public bool m_UseInfo;

		// Token: 0x04003E12 RID: 15890
		public List<int> m_ItemList = new List<int>();
	}
}
