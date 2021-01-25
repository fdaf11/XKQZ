using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
[ExecuteInEditMode]
public class CameraPathOrientation : CameraPathPoint
{
	// Token: 0x06000297 RID: 663 RVA: 0x00003BF2 File Offset: 0x00001DF2
	private void OnEnable()
	{
		base.hideFlags = 2;
	}

	// Token: 0x040001FB RID: 507
	public Quaternion rotation = Quaternion.identity;

	// Token: 0x040001FC RID: 508
	public Transform lookAt;
}
