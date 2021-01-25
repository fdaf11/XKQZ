using System;
using UnityEngine;

// Token: 0x0200084B RID: 2123
public class MouseLook : MonoBehaviour
{
	// Token: 0x06003385 RID: 13189 RVA: 0x0018D910 File Offset: 0x0018BB10
	private void Update()
	{
		this.lookX += Input.GetAxis("Mouse X") * this.lookSpeed * Time.deltaTime;
		this.lookY += Input.GetAxis("Mouse Y") * this.lookSpeed * Time.deltaTime;
		this.lookX = Mathf.Clamp(this.lookX, -this.lookXMax, this.lookXMax);
		this.lookY = Mathf.Clamp(this.lookY, -this.lookYMax, this.lookYMax);
		Quaternion quaternion = Quaternion.Euler(-this.lookY, this.lookX, 0f);
		base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, quaternion, this.lookSmooth);
	}

	// Token: 0x04003FB5 RID: 16309
	private float lookX;

	// Token: 0x04003FB6 RID: 16310
	private float lookY;

	// Token: 0x04003FB7 RID: 16311
	public float lookSpeed = 40f;

	// Token: 0x04003FB8 RID: 16312
	public float lookSmooth = 0.1f;

	// Token: 0x04003FB9 RID: 16313
	public float lookXMax = 20f;

	// Token: 0x04003FBA RID: 16314
	public float lookYMax = 20f;
}
