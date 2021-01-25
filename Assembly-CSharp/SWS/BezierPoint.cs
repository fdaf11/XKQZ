using System;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006B8 RID: 1720
	[Serializable]
	public class BezierPoint
	{
		// Token: 0x0400347E RID: 13438
		public Transform wp;

		// Token: 0x0400347F RID: 13439
		public Transform[] cp = new Transform[2];
	}
}
