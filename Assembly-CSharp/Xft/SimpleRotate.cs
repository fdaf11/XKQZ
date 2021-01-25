using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000575 RID: 1397
	public class SimpleRotate : MonoBehaviour
	{
		// Token: 0x060022B5 RID: 8885 RVA: 0x00110880 File Offset: 0x0010EA80
		private void Start()
		{
			this.OriAngleX = base.transform.rotation.x;
			this.OriAngleY = base.transform.rotation.y;
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x001108C0 File Offset: 0x0010EAC0
		private void Update()
		{
			if (this.RotateX)
			{
				this.OriAngleX += Time.deltaTime * this.RotateSpeed;
			}
			if (this.RotateY)
			{
				this.OriAngleY -= Time.deltaTime * this.RotateSpeed;
			}
			base.transform.rotation = Quaternion.Euler(this.OriAngleX, this.OriAngleY, base.transform.rotation.z);
		}

		// Token: 0x04002A3F RID: 10815
		protected float OriAngleX;

		// Token: 0x04002A40 RID: 10816
		protected float OriAngleY;

		// Token: 0x04002A41 RID: 10817
		public float RotateSpeed = 20f;

		// Token: 0x04002A42 RID: 10818
		public bool RotateX = true;

		// Token: 0x04002A43 RID: 10819
		public bool RotateY;
	}
}
