using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x020001B2 RID: 434
	public class AnimalDataNode
	{
		// Token: 0x0600091A RID: 2330 RVA: 0x000077BE File Offset: 0x000059BE
		public AnimalDataNode()
		{
			this.m_iProbabilityList = new List<int>();
		}

		// Token: 0x040008AE RID: 2222
		public int m_iAnimalID;

		// Token: 0x040008AF RID: 2223
		public List<int> m_iProbabilityList;

		// Token: 0x040008B0 RID: 2224
		public int m_iBodyType;

		// Token: 0x040008B1 RID: 2225
		public int m_iAnimalType;

		// Token: 0x040008B2 RID: 2226
		public int m_iRound;

		// Token: 0x040008B3 RID: 2227
		public float m_fAtkDedSec;

		// Token: 0x040008B4 RID: 2228
		public int m_iAnimalHp;

		// Token: 0x040008B5 RID: 2229
		public float m_fSpeed;

		// Token: 0x040008B6 RID: 2230
		public int m_ipoint;

		// Token: 0x040008B7 RID: 2231
		public int m_iGift1Item;

		// Token: 0x040008B8 RID: 2232
		public int m_iGift2Item;

		// Token: 0x040008B9 RID: 2233
		public int m_iGift3Item;

		// Token: 0x040008BA RID: 2234
		public int m_iProbability1;

		// Token: 0x040008BB RID: 2235
		public int m_iProbability2;

		// Token: 0x040008BC RID: 2236
		public int m_iProbability3;

		// Token: 0x040008BD RID: 2237
		public string m_strSoundID;

		// Token: 0x040008BE RID: 2238
		public string m_strMapID;

		// Token: 0x040008BF RID: 2239
		public string m_strIcon;
	}
}
