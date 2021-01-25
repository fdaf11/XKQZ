using System;
using UnityEngine;

namespace Exploder
{
	// Token: 0x020000BC RID: 188
	public class ExploderSlowMotion : MonoBehaviour
	{
		// Token: 0x060003EB RID: 1003 RVA: 0x000323FC File Offset: 0x000305FC
		public void EnableSlowMotion(bool status)
		{
			this.slowmo = status;
			if (this.slowmo)
			{
				this.slowMotionSpeed = 0.05f;
				if (this.Exploder)
				{
					this.Exploder.MeshColliders = true;
				}
			}
			else
			{
				this.slowMotionSpeed = 1f;
				if (this.Exploder)
				{
					this.Exploder.MeshColliders = false;
				}
			}
			this.slowMotionTime = this.slowMotionSpeed;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0003247C File Offset: 0x0003067C
		public void Update()
		{
			this.slowMotionSpeed = this.slowMotionTime;
			Time.timeScale = this.slowMotionSpeed;
			Time.fixedDeltaTime = this.slowMotionSpeed * 0.02f;
			if (Input.GetKeyDown(116))
			{
				this.slowmo = !this.slowmo;
				this.EnableSlowMotion(this.slowmo);
			}
		}

		// Token: 0x04000316 RID: 790
		public float slowMotionTime = 1f;

		// Token: 0x04000317 RID: 791
		public ExploderObject Exploder;

		// Token: 0x04000318 RID: 792
		private float slowMotionSpeed = 1f;

		// Token: 0x04000319 RID: 793
		private bool slowmo;
	}
}
