using System;
using UnityEngine;

// Token: 0x02000605 RID: 1541
[AddComponentMenu("Camera-Control/Smooth Mouse Orbit - Unluck Software")]
public class SmoothCameraOrbit : MonoBehaviour
{
	// Token: 0x0600262A RID: 9770 RVA: 0x0001967E File Offset: 0x0001787E
	private void Start()
	{
		this.Init();
	}

	// Token: 0x0600262B RID: 9771 RVA: 0x0001967E File Offset: 0x0001787E
	private void OnEnable()
	{
		this.Init();
	}

	// Token: 0x0600262C RID: 9772 RVA: 0x00128014 File Offset: 0x00126214
	public void Init()
	{
		if (!this.target)
		{
			this.target = new GameObject("Cam Target")
			{
				transform = 
				{
					position = base.transform.position + base.transform.forward * this.distance
				}
			}.transform;
		}
		this.currentDistance = this.distance;
		this.desiredDistance = this.distance;
		this.position = base.transform.position;
		this.rotation = base.transform.rotation;
		this.currentRotation = base.transform.rotation;
		this.desiredRotation = base.transform.rotation;
		this.xDeg = Vector3.Angle(Vector3.right, base.transform.right);
		this.yDeg = Vector3.Angle(Vector3.up, base.transform.up);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
	}

	// Token: 0x0600262D RID: 9773 RVA: 0x00128148 File Offset: 0x00126348
	private void LateUpdate()
	{
		if (Input.GetMouseButton(2) && Input.GetKey(308) && Input.GetKey(306))
		{
			this.desiredDistance -= Input.GetAxis("Mouse Y") * Time.deltaTime * (float)this.zoomRate * 0.125f * Mathf.Abs(this.desiredDistance);
		}
		else if (Input.GetMouseButton(0))
		{
			this.xDeg += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
			this.yDeg -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			this.yDeg = SmoothCameraOrbit.ClampAngle(this.yDeg, (float)this.yMinLimit, (float)this.yMaxLimit);
			this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.currentRotation = base.transform.rotation;
			this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, Time.deltaTime * this.zoomDampening);
			base.transform.rotation = this.rotation;
		}
		this.desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * (float)this.zoomRate * Mathf.Abs(this.desiredDistance);
		this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
		this.currentDistance = Mathf.Lerp(this.currentDistance, this.desiredDistance, Time.deltaTime * this.zoomDampening);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
		base.transform.position = this.position;
	}

	// Token: 0x0600262E RID: 9774 RVA: 0x00010485 File Offset: 0x0000E685
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

	// Token: 0x04002EED RID: 12013
	public Transform target;

	// Token: 0x04002EEE RID: 12014
	public Vector3 targetOffset;

	// Token: 0x04002EEF RID: 12015
	public float distance = 5f;

	// Token: 0x04002EF0 RID: 12016
	public float maxDistance = 20f;

	// Token: 0x04002EF1 RID: 12017
	public float minDistance = 0.6f;

	// Token: 0x04002EF2 RID: 12018
	public float xSpeed = 200f;

	// Token: 0x04002EF3 RID: 12019
	public float ySpeed = 200f;

	// Token: 0x04002EF4 RID: 12020
	public int yMinLimit = -80;

	// Token: 0x04002EF5 RID: 12021
	public int yMaxLimit = 80;

	// Token: 0x04002EF6 RID: 12022
	public int zoomRate = 40;

	// Token: 0x04002EF7 RID: 12023
	public float panSpeed = 0.3f;

	// Token: 0x04002EF8 RID: 12024
	public float zoomDampening = 5f;

	// Token: 0x04002EF9 RID: 12025
	private float xDeg;

	// Token: 0x04002EFA RID: 12026
	private float yDeg;

	// Token: 0x04002EFB RID: 12027
	private float currentDistance;

	// Token: 0x04002EFC RID: 12028
	private float desiredDistance;

	// Token: 0x04002EFD RID: 12029
	private Quaternion currentRotation;

	// Token: 0x04002EFE RID: 12030
	private Quaternion desiredRotation;

	// Token: 0x04002EFF RID: 12031
	private Quaternion rotation;

	// Token: 0x04002F00 RID: 12032
	private Vector3 position;
}
