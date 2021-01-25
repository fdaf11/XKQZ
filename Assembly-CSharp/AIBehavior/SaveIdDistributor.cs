using System;

namespace AIBehavior
{
	// Token: 0x02000024 RID: 36
	public static class SaveIdDistributor
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00002A7E File Offset: 0x00000C7E
		public static int GetId(int id)
		{
			if (id == -1)
			{
				id = ++SaveIdDistributor.currentId;
			}
			return id;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00002A97 File Offset: 0x00000C97
		public static void SetId(int id)
		{
			if (SaveIdDistributor.currentId < id)
			{
				SaveIdDistributor.currentId = id;
			}
		}

		// Token: 0x04000074 RID: 116
		private static int currentId;
	}
}
