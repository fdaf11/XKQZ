using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x0200065C RID: 1628
	public class FX_Rotation : MonoBehaviour
	{
		// Token: 0x060027FA RID: 10234 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x0001A5D8 File Offset: 0x000187D8
		private void FixedUpdate()
		{
			base.transform.Rotate(this.Speed);
		}

		// Token: 0x040031F8 RID: 12792
		public Vector3 Speed = Vector3.up;
	}
}
