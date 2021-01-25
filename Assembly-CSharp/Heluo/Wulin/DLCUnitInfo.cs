using System;
using Newtonsoft.Json;

namespace Heluo.Wulin
{
	// Token: 0x0200080D RID: 2061
	public class DLCUnitInfo
	{
		// Token: 0x0600326F RID: 12911 RVA: 0x0001FA78 File Offset: 0x0001DC78
		public void LevelUP()
		{
			this.LV++;
			if (this.LV >= 4)
			{
				this.LV = 4;
			}
			this.Refresh();
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x0001FAA1 File Offset: 0x0001DCA1
		public void Refresh()
		{
			this.Data.iLevel = this.LV;
			this.Data.DLC_LevelupHidePassive(this.GID);
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x0018656C File Offset: 0x0018476C
		public DLCUnitInfo Clone()
		{
			return new DLCUnitInfo
			{
				GID = this.GID,
				ID = this.ID,
				LV = this.LV
			};
		}

		// Token: 0x04003E15 RID: 15893
		public string GID;

		// Token: 0x04003E16 RID: 15894
		public int ID;

		// Token: 0x04003E17 RID: 15895
		public int LV;

		// Token: 0x04003E18 RID: 15896
		[JsonIgnore]
		public CharacterData Data;
	}
}
