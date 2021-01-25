using System;
using UnityEngine;

namespace Heluo.Wulin.HeadLook
{
	// Token: 0x020001A9 RID: 425
	[Serializable]
	public class BendingSegment
	{
		// Token: 0x04000870 RID: 2160
		public Transform firstTransform;

		// Token: 0x04000871 RID: 2161
		public Transform lastTransform;

		// Token: 0x04000872 RID: 2162
		public float thresholdAngleDifference;

		// Token: 0x04000873 RID: 2163
		public float bendingMultiplier = 0.6f;

		// Token: 0x04000874 RID: 2164
		public float maxAngleDifference = 30f;

		// Token: 0x04000875 RID: 2165
		public float maxBendingAngle = 80f;

		// Token: 0x04000876 RID: 2166
		public float responsiveness = 5f;

		// Token: 0x04000877 RID: 2167
		internal float angleH;

		// Token: 0x04000878 RID: 2168
		internal float angleV;

		// Token: 0x04000879 RID: 2169
		internal Vector3 dirUp;

		// Token: 0x0400087A RID: 2170
		internal Vector3 referenceLookDir;

		// Token: 0x0400087B RID: 2171
		internal Vector3 referenceUpDir;

		// Token: 0x0400087C RID: 2172
		internal int chainLength;

		// Token: 0x0400087D RID: 2173
		internal Quaternion[] origRotations;
	}
}
