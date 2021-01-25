using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x02000095 RID: 149
	public class ExploderMouseLook : MonoBehaviour
	{
		// Token: 0x0600033F RID: 831 RVA: 0x0002BFCC File Offset: 0x0002A1CC
		private void Update()
		{
			if (!Screen.lockCursor)
			{
				return;
			}
			float num2;
			if (this.axes == ExploderMouseLook.RotationAxes.MouseXAndY)
			{
				float num = base.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * this.sensitivityX;
				this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
				this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
				num2 = num;
				this.mainCamera.transform.localEulerAngles = new Vector3(-this.rotationY, 0f, 0f);
				if (this.kickTimeout > 0f)
				{
					this.rotationTarget += Input.GetAxis("Mouse Y") * this.sensitivityY;
				}
			}
			else if (this.axes == ExploderMouseLook.RotationAxes.MouseX)
			{
				num2 = Input.GetAxis("Mouse X") * this.sensitivityX;
				this.mainCamera.transform.Rotate(0f, 0f, 0f);
			}
			else
			{
				this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
				this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
				if (this.kickTimeout > 0f)
				{
					this.rotationTarget += Input.GetAxis("Mouse Y") * this.sensitivityY;
				}
				num2 = base.transform.localEulerAngles.y;
				this.mainCamera.transform.localEulerAngles = new Vector3(-this.rotationY, 0f, 0f);
			}
			this.kickTimeout -= Time.deltaTime;
			if (this.kickTimeout > 0f)
			{
				this.rotationY = Mathf.Lerp(this.rotationY, this.rotationTarget, Time.deltaTime * 10f);
			}
			else if (this.kickBack)
			{
				this.kickBack = false;
				this.kickTimeout = 0.5f;
				this.rotationTarget = this.kickBackRot;
			}
			base.gameObject.transform.rotation = Quaternion.Euler(0f, num2, 0f);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000449C File Offset: 0x0000269C
		private void Start()
		{
			if (base.rigidbody)
			{
				base.rigidbody.freezeRotation = true;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x000044BA File Offset: 0x000026BA
		public void Kick()
		{
			this.kickTimeout = 0.1f;
			this.rotationTarget = this.rotationY + (float)Random.Range(15, 20);
			this.kickBackRot = this.rotationY;
			this.kickBack = true;
		}

		// Token: 0x04000260 RID: 608
		public ExploderMouseLook.RotationAxes axes;

		// Token: 0x04000261 RID: 609
		public float sensitivityX = 15f;

		// Token: 0x04000262 RID: 610
		public float sensitivityY = 15f;

		// Token: 0x04000263 RID: 611
		public float minimumX = -360f;

		// Token: 0x04000264 RID: 612
		public float maximumX = 360f;

		// Token: 0x04000265 RID: 613
		public float minimumY = -60f;

		// Token: 0x04000266 RID: 614
		public float maximumY = 60f;

		// Token: 0x04000267 RID: 615
		private float rotationY;

		// Token: 0x04000268 RID: 616
		private float kickTimeout;

		// Token: 0x04000269 RID: 617
		private float kickBackRot;

		// Token: 0x0400026A RID: 618
		private bool kickBack;

		// Token: 0x0400026B RID: 619
		private float rotationTarget;

		// Token: 0x0400026C RID: 620
		public Camera mainCamera;

		// Token: 0x02000096 RID: 150
		public enum RotationAxes
		{
			// Token: 0x0400026E RID: 622
			MouseXAndY,
			// Token: 0x0400026F RID: 623
			MouseX,
			// Token: 0x04000270 RID: 624
			MouseY
		}
	}
}
