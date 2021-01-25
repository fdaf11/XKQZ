using System;
using UnityEngine;

// Token: 0x0200052B RID: 1323
public class MouseOrbitCS : MonoBehaviour
{
	// Token: 0x060021D4 RID: 8660 RVA: 0x00100560 File Offset: 0x000FE760
	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		this.x = eulerAngles.y;
		this.y = eulerAngles.x;
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x00100594 File Offset: 0x000FE794
	private void LateUpdate()
	{
		if (this.target && !Input.GetKey(102))
		{
			if (!Input.GetMouseButton(0))
			{
				this.x += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
				this.y -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			}
			this.y = MouseOrbitCS.ClampAngle(this.y, this.yMinLimit + this.normal_angle, this.yMaxLimit + this.normal_angle);
			Quaternion quaternion = Quaternion.Euler(this.y, this.x, 0f);
			Vector3 position = quaternion * new Vector3(0f, 0f, -this.distance) + this.target.position;
			base.transform.rotation = quaternion;
			base.transform.position = position;
		}
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x00010485 File Offset: 0x0000E685
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x00016AD2 File Offset: 0x00014CD2
	public void set_normal_angle(float a)
	{
		this.normal_angle = a;
	}

	// Token: 0x0400253D RID: 9533
	public Transform target;

	// Token: 0x0400253E RID: 9534
	public float distance = 10f;

	// Token: 0x0400253F RID: 9535
	public float xSpeed = 250f;

	// Token: 0x04002540 RID: 9536
	public float ySpeed = 120f;

	// Token: 0x04002541 RID: 9537
	public float yMinLimit = -20f;

	// Token: 0x04002542 RID: 9538
	public float yMaxLimit = 80f;

	// Token: 0x04002543 RID: 9539
	private float x;

	// Token: 0x04002544 RID: 9540
	private float y;

	// Token: 0x04002545 RID: 9541
	private float normal_angle;
}
