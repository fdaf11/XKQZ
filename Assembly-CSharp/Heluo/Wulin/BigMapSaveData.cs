using System;

namespace Heluo.Wulin
{
	// Token: 0x02000127 RID: 295
	public class BigMapSaveData
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0000585A File Offset: 0x00003A5A
		public static BigMapSaveData Singleton
		{
			get
			{
				return BigMapSaveData.instance;
			}
		}

		// Token: 0x0400067D RID: 1661
		private static readonly BigMapSaveData instance = new BigMapSaveData();

		// Token: 0x0400067E RID: 1662
		public int UseDate;
	}
}
