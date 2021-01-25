using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000655 RID: 1621
	[RequireComponent(typeof(Rigidbody))]
	public class FX_Mover : MonoBehaviour
	{
		// Token: 0x060027E6 RID: 10214 RVA: 0x0013BA7C File Offset: 0x00139C7C
		private void Start()
		{
			this.direction = Quaternion.LookRotation(base.transform.forward * 1000f);
			base.transform.Rotate(new Vector3(Random.Range(-this.Noise.x, this.Noise.x), Random.Range(-this.Noise.y, this.Noise.y), Random.Range(-this.Noise.z, this.Noise.z)));
		}

		// Token: 0x060027E7 RID: 10215 RVA: 0x0013BB10 File Offset: 0x00139D10
		private void LateUpdate()
		{
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.direction, this.Damping);
			base.transform.position += base.transform.forward * this.Speed * Time.deltaTime;
		}

		// Token: 0x040031E2 RID: 12770
		public float Speed = 1f;

		// Token: 0x040031E3 RID: 12771
		public Vector3 Noise = Vector3.zero;

		// Token: 0x040031E4 RID: 12772
		public float Damping = 0.3f;

		// Token: 0x040031E5 RID: 12773
		private Quaternion direction;
	}
}
