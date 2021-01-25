using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020002CB RID: 715
	public class SaveTitleDataNode
	{
		// Token: 0x0400106D RID: 4205
		public bool m_bHaveData;

		// Token: 0x0400106E RID: 4206
		public string m_strMissionID;

		// Token: 0x0400106F RID: 4207
		public string m_strPlaceName;

		// Token: 0x04001070 RID: 4208
		public string m_strTrueYear;

		// Token: 0x04001071 RID: 4209
		public string m_strPlaceImageName;

		// Token: 0x04001072 RID: 4210
		public PlayGameTime m_PlayGameTime;

		// Token: 0x04001073 RID: 4211
		[JsonIgnore]
		public Texture2D m_Texture;
	}
}
