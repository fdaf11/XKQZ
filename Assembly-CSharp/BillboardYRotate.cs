using System;
using UnityEngine;

// Token: 0x02000665 RID: 1637
public class BillboardYRotate : MonoBehaviour
{
	// Token: 0x0600281F RID: 10271 RVA: 0x0001A727 File Offset: 0x00018927
	private void Awake()
	{
		this.cameraToLookAt = Camera.main;
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x0013D334 File Offset: 0x0013B534
	private void LateUpdate()
	{
		Vector3 vector = this.cameraToLookAt.transform.position - base.transform.position;
		vector.x = (vector.z = 0f);
		base.transform.LookAt(this.cameraToLookAt.transform.position - vector);
	}

	// Token: 0x04003239 RID: 12857
	private Camera cameraToLookAt;
}
