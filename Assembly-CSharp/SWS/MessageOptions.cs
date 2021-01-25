using System;
using System.Collections.Generic;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006AF RID: 1711
	[Serializable]
	public class MessageOptions
	{
		// Token: 0x04003450 RID: 13392
		public List<string> message = new List<string>();

		// Token: 0x04003451 RID: 13393
		public List<Object> obj = new List<Object>();

		// Token: 0x04003452 RID: 13394
		public List<string> text = new List<string>();

		// Token: 0x04003453 RID: 13395
		public List<float> num = new List<float>();

		// Token: 0x04003454 RID: 13396
		public List<Vector2> vect2 = new List<Vector2>();

		// Token: 0x04003455 RID: 13397
		public List<Vector3> vect3 = new List<Vector3>();

		// Token: 0x04003456 RID: 13398
		public List<MessageOptions.ValueType> type = new List<MessageOptions.ValueType>();

		// Token: 0x04003457 RID: 13399
		public float pos;

		// Token: 0x020006B0 RID: 1712
		public enum ValueType
		{
			// Token: 0x04003459 RID: 13401
			None,
			// Token: 0x0400345A RID: 13402
			Object,
			// Token: 0x0400345B RID: 13403
			Text,
			// Token: 0x0400345C RID: 13404
			Numeric,
			// Token: 0x0400345D RID: 13405
			Vector2,
			// Token: 0x0400345E RID: 13406
			Vector3
		}
	}
}
