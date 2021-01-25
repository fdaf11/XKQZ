using System;
using UnityEngine;

// Token: 0x02000881 RID: 2177
public class Vector3OrTransform
{
	// Token: 0x0400404E RID: 16462
	public static readonly string[] choices = new string[]
	{
		"Vector3",
		"Transform"
	};

	// Token: 0x0400404F RID: 16463
	public static readonly int vector3Selected = 0;

	// Token: 0x04004050 RID: 16464
	public static readonly int transformSelected = 1;

	// Token: 0x04004051 RID: 16465
	public int selected;

	// Token: 0x04004052 RID: 16466
	public Vector3 vector;

	// Token: 0x04004053 RID: 16467
	public Transform transform;
}
