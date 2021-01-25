using System;
using UnityEngine;

namespace PigeonCoopToolkit.Utillities
{
	// Token: 0x020005BB RID: 1467
	public static class GizmosExtra
	{
		// Token: 0x0600249E RID: 9374 RVA: 0x0011E044 File Offset: 0x0011C244
		public static void GizmosDrawCircle(Vector3 position, Vector3 up, float size, int divisions)
		{
			Vector3 vector = Quaternion.Euler(90f, 0f, 0f) * (up * size);
			for (int i = 0; i < divisions; i++)
			{
				Vector3 vector2 = Quaternion.AngleAxis(360f / (float)divisions, up) * vector;
				Gizmos.DrawLine(position + vector, position + vector2);
				vector = vector2;
			}
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x0011E0B0 File Offset: 0x0011C2B0
		public static void GizmosDrawArrow(Vector3 from, Vector3 to, float arrowSize)
		{
			Gizmos.DrawLine(from, to);
			Vector3 vector = (to - from).normalized * arrowSize;
			Gizmos.DrawLine(to, to - Quaternion.Euler(0f, 0f, 45f) * vector);
			Gizmos.DrawLine(to, to - Quaternion.Euler(0f, 0f, -45f) * vector);
		}
	}
}
