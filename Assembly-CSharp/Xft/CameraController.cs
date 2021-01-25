using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x020005A8 RID: 1448
	public class CameraController : MonoBehaviour
	{
		// Token: 0x06002438 RID: 9272 RVA: 0x0011B840 File Offset: 0x00119A40
		private void Start()
		{
			this.mDistance = Vector3.Distance(base.transform.position, this.Target.position);
			this.mCurDistance = this.mDistance;
			this.mDesiredDistance = this.mDistance;
			this.mCurrentRotation = base.transform.rotation;
			this.mDesiredRotation = base.transform.rotation;
			this.mDegX = base.transform.rotation.eulerAngles.y;
			this.mDegY = base.transform.rotation.eulerAngles.x;
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000058AA File Offset: 0x00003AAA
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

		// Token: 0x0600243A RID: 9274 RVA: 0x0011B8EC File Offset: 0x00119AEC
		private void LateUpdate()
		{
			base.transform.LookAt(this.Target);
			if (Input.GetMouseButton(0))
			{
				this.mDegX += Input.GetAxis("Mouse X") * this.RotateSpeed * Time.deltaTime;
				this.mDegY -= Input.GetAxis("Mouse Y") * this.RotateSpeed * Time.deltaTime;
				this.mDegY = this.ClampAngle(this.mDegY, (float)this.RotateYMin, (float)this.RotateYMax);
				this.mDesiredRotation = Quaternion.Euler(this.mDegY, this.mDegX, 0f);
				this.mCurrentRotation = base.transform.rotation;
				Quaternion rotation = Quaternion.Lerp(this.mCurrentRotation, this.mDesiredRotation, Time.deltaTime * this.ZoomDampening);
				base.transform.rotation = rotation;
			}
			else if (Input.GetMouseButton(1))
			{
				this.Target.rotation = base.transform.rotation;
				this.Target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * this.panSpeed);
				this.Target.Translate(base.transform.up * -Input.GetAxis("Mouse Y") * this.panSpeed, 0);
			}
			this.mDesiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * (float)this.ZoomRate * Mathf.Abs(this.mDesiredDistance);
			this.mCurDistance = Mathf.Lerp(this.mCurDistance, this.mDesiredDistance, Time.deltaTime * this.ZoomDampening);
			Vector3 position = this.Target.position - base.transform.rotation * Vector3.forward * this.mCurDistance;
			base.transform.position = position;
		}

		// Token: 0x04002BFE RID: 11262
		public Transform Target;

		// Token: 0x04002BFF RID: 11263
		public int ZoomRate = 40;

		// Token: 0x04002C00 RID: 11264
		public float ZoomDampening = 5f;

		// Token: 0x04002C01 RID: 11265
		public float RotateSpeed = 200f;

		// Token: 0x04002C02 RID: 11266
		public int RotateYMin = -80;

		// Token: 0x04002C03 RID: 11267
		public int RotateYMax = 80;

		// Token: 0x04002C04 RID: 11268
		public float panSpeed = 0.3f;

		// Token: 0x04002C05 RID: 11269
		protected float mDistance;

		// Token: 0x04002C06 RID: 11270
		protected float mCurDistance;

		// Token: 0x04002C07 RID: 11271
		protected float mDesiredDistance;

		// Token: 0x04002C08 RID: 11272
		protected Quaternion mCurrentRotation;

		// Token: 0x04002C09 RID: 11273
		protected Quaternion mDesiredRotation;

		// Token: 0x04002C0A RID: 11274
		protected float mDegX;

		// Token: 0x04002C0B RID: 11275
		protected float mDegY;
	}
}
