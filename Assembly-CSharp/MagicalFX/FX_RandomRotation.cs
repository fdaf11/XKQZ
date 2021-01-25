using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x0200065A RID: 1626
	public class FX_RandomRotation : MonoBehaviour
	{
		// Token: 0x060027F5 RID: 10229 RVA: 0x0013BF94 File Offset: 0x0013A194
		private void Start()
		{
			base.transform.Rotate(new Vector3(Random.Range(-this.Rotation.x, this.Rotation.x), Random.Range(-this.Rotation.y, this.Rotation.y), Random.Range(-this.Rotation.z, this.Rotation.z)));
		}

		// Token: 0x040031F2 RID: 12786
		public Vector3 Rotation;
	}
}
