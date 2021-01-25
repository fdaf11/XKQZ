using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000656 RID: 1622
	public class FX_MoverRandom : MonoBehaviour
	{
		// Token: 0x060027E9 RID: 10217 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x0013BB7C File Offset: 0x00139D7C
		private void FixedUpdate()
		{
			base.transform.position += base.transform.forward * this.Speed * Time.fixedDeltaTime;
			base.transform.position += new Vector3(Random.Range(-this.Noise.x, this.Noise.x), Random.Range(-this.Noise.y, this.Noise.y), Random.Range(-this.Noise.z, this.Noise.z)) * Time.fixedDeltaTime;
		}

		// Token: 0x040031E6 RID: 12774
		public float Speed = 1f;

		// Token: 0x040031E7 RID: 12775
		public Vector3 Noise = Vector3.zero;
	}
}
