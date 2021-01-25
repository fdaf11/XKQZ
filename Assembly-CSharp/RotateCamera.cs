using System;
using UnityEngine;

// Token: 0x020005AE RID: 1454
public class RotateCamera : MonoBehaviour
{
	// Token: 0x06002448 RID: 9288 RVA: 0x00018127 File Offset: 0x00016327
	private void Update()
	{
		base.transform.RotateAround(this.RotationCenter.position, Vector3.up, this.speed * Time.deltaTime);
	}

	// Token: 0x04002C20 RID: 11296
	public Transform RotationCenter;

	// Token: 0x04002C21 RID: 11297
	public float speed = 5f;
}
