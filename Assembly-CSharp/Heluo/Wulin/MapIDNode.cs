using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000218 RID: 536
	public class MapIDNode
	{
		// Token: 0x04000B10 RID: 2832
		public string m_strMapID;

		// Token: 0x04000B11 RID: 2833
		public string m_strMapName;

		// Token: 0x04000B12 RID: 2834
		public List<Condition> m_OpenCdn = new List<Condition>();

		// Token: 0x04000B13 RID: 2835
		public List<Condition> m_CloseCdn = new List<Condition>();

		// Token: 0x04000B14 RID: 2836
		public bool IsAllOpenCdn;

		// Token: 0x04000B15 RID: 2837
		public bool IsAllCloseCdn;

		// Token: 0x04000B16 RID: 2838
		public Vector3 Pos;

		// Token: 0x04000B17 RID: 2839
		public float AngleY;

		// Token: 0x04000B18 RID: 2840
		public string MapIcon = "10001";

		// Token: 0x04000B19 RID: 2841
		public float Range;

		// Token: 0x02000219 RID: 537
		public enum eMember
		{
			// Token: 0x04000B1B RID: 2843
			MapID,
			// Token: 0x04000B1C RID: 2844
			MapName,
			// Token: 0x04000B1D RID: 2845
			OpenCdn,
			// Token: 0x04000B1E RID: 2846
			CloseCdn,
			// Token: 0x04000B1F RID: 2847
			IsAllOpenCdn,
			// Token: 0x04000B20 RID: 2848
			IsAllCloseCdn,
			// Token: 0x04000B21 RID: 2849
			Pos,
			// Token: 0x04000B22 RID: 2850
			AngleY,
			// Token: 0x04000B23 RID: 2851
			MapIcon,
			// Token: 0x04000B24 RID: 2852
			Range
		}
	}
}
