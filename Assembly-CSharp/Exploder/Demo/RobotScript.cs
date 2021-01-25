using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x0200009D RID: 157
	internal class RobotScript : MonoBehaviour
	{
		// Token: 0x0600035C RID: 860 RVA: 0x000045D5 File Offset: 0x000027D5
		private void Start()
		{
			this.center = base.gameObject.transform.position;
		}

		// Token: 0x0600035D RID: 861 RVA: 0x000045ED File Offset: 0x000027ED
		private void Update()
		{
			base.animation.Play();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0002C9E4 File Offset: 0x0002ABE4
		private void FixedUpdate()
		{
			Vector3 position = base.gameObject.transform.position;
			position.x = this.center.x + Mathf.Sin(this.angle) * this.radius;
			position.z = this.center.z + Mathf.Cos(this.angle) * this.radius;
			base.gameObject.transform.position = position;
			base.gameObject.transform.forward = position - this.lastPos;
			this.lastPos = position;
			this.angle += Time.deltaTime * this.velocity;
		}

		// Token: 0x04000290 RID: 656
		public float radius = 4f;

		// Token: 0x04000291 RID: 657
		public float velocity = 1f;

		// Token: 0x04000292 RID: 658
		private float angle;

		// Token: 0x04000293 RID: 659
		private Vector3 center;

		// Token: 0x04000294 RID: 660
		private Vector3 lastPos;
	}
}
