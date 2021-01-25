using System;

namespace Heluo.Wulin
{
	// Token: 0x0200080E RID: 2062
	public class LevelTurnNode
	{
		// Token: 0x06003273 RID: 12915 RVA: 0x001865A4 File Offset: 0x001847A4
		public LevelTurnNode Clone()
		{
			return new LevelTurnNode
			{
				iLevelID = this.iLevelID,
				iTurn = this.iTurn
			};
		}

		// Token: 0x04003E19 RID: 15897
		public int iLevelID;

		// Token: 0x04003E1A RID: 15898
		public int iTurn;
	}
}
