using System;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class MouseOrbit : MonoBehaviour
{
	// Token: 0x0600060C RID: 1548 RVA: 0x00043904 File Offset: 0x00041B04
	private void Start()
	{
		if (this.target == null)
		{
			this.target = GameObject.FindGameObjectWithTag("Player");
			if (this.target == null)
			{
				Debug.LogWarning("Don't found player tag please change player tag to Player");
			}
		}
		Vector3 eulerAngles = base.transform.eulerAngles;
		this.x = eulerAngles.y;
		this.y = eulerAngles.x;
		this.CalDistance();
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00005861 File Offset: 0x00003A61
	private void LateUpdate()
	{
		this.ScrollMouse();
		this.RotateCamera();
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0004397C File Offset: 0x00041B7C
	private void RotateCamera()
	{
		if (Input.GetMouseButtonDown(1))
		{
			this.isActivated = true;
		}
		if (Input.GetMouseButtonUp(1))
		{
			this.isActivated = false;
		}
		if (this.target && this.isActivated)
		{
			this.y -= Input.GetAxis("Mouse Y") * this.ySpeed;
			this.x += Input.GetAxis("Mouse X") * this.xSpeed;
			this.y = this.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
			Quaternion quaternion = Quaternion.Euler(this.y, this.x, 0f);
			Vector3 vector;
			vector..ctor(0f, 0f, -this.distanceLerp);
			this.position = quaternion * vector + this.target.transform.position;
			base.transform.rotation = quaternion;
			base.transform.position = this.position;
		}
		else
		{
			Quaternion quaternion2 = Quaternion.Euler(this.y, this.x, 0f);
			Vector3 vector2;
			vector2..ctor(0f, 0f, -this.distanceLerp);
			this.position = quaternion2 * vector2 + this.target.transform.position;
			base.transform.rotation = quaternion2;
			base.transform.position = this.position;
		}
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x00043B04 File Offset: 0x00041D04
	private void CalDistance()
	{
		this.distance = this.zoomMax;
		this.distanceLerp = this.distance;
		Quaternion quaternion = Quaternion.Euler(this.y, this.x, 0f);
		Vector3 vector;
		vector..ctor(0f, 0f, -this.distanceLerp);
		this.position = quaternion * vector + this.target.transform.position;
		base.transform.rotation = quaternion;
		base.transform.position = this.position;
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0000586F File Offset: 0x00003A6F
	private void ScrollMouse()
	{
		this.distanceLerp = Mathf.Lerp(this.distanceLerp, this.distance, Time.deltaTime * 5f);
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x00005893 File Offset: 0x00003A93
	private float ScrollLimit(float dist, float min, float max)
	{
		if (dist < min)
		{
			dist = min;
		}
		if (dist > max)
		{
			dist = max;
		}
		return dist;
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x000058AA File Offset: 0x00003AAA
	private float ClampAngle(float angle, float min, float max)
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

	// Token: 0x0400067F RID: 1663
	[HideInInspector]
	public GameObject target;

	// Token: 0x04000680 RID: 1664
	public float xSpeed;

	// Token: 0x04000681 RID: 1665
	public float ySpeed;

	// Token: 0x04000682 RID: 1666
	public float yMinLimit;

	// Token: 0x04000683 RID: 1667
	public float yMaxLimit;

	// Token: 0x04000684 RID: 1668
	public float scrollSpeed;

	// Token: 0x04000685 RID: 1669
	public float zoomMin;

	// Token: 0x04000686 RID: 1670
	public float zoomMax;

	// Token: 0x04000687 RID: 1671
	private float distance;

	// Token: 0x04000688 RID: 1672
	private float distanceLerp;

	// Token: 0x04000689 RID: 1673
	private Vector3 position;

	// Token: 0x0400068A RID: 1674
	private bool isActivated;

	// Token: 0x0400068B RID: 1675
	private float x;

	// Token: 0x0400068C RID: 1676
	private float y;

	// Token: 0x0400068D RID: 1677
	private bool setupCamera;
}
