using System;
using UnityEngine;

// Token: 0x02000882 RID: 2178
public class Vector3OrTransformArray
{
	// Token: 0x04004054 RID: 16468
	public static readonly string[] choices = new string[]
	{
		"Vector3",
		"Transform",
		"Path"
	};

	// Token: 0x04004055 RID: 16469
	public static readonly int vector3Selected = 0;

	// Token: 0x04004056 RID: 16470
	public static readonly int transformSelected = 1;

	// Token: 0x04004057 RID: 16471
	public static readonly int iTweenPathSelected = 2;

	// Token: 0x04004058 RID: 16472
	public int selected;

	// Token: 0x04004059 RID: 16473
	public Vector3[] vectorArray;

	// Token: 0x0400405A RID: 16474
	public Transform[] transformArray;

	// Token: 0x0400405B RID: 16475
	public string pathName;
}
