using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x0200065D RID: 1629
	public class FX_ShakeCamera : MonoBehaviour
	{
		// Token: 0x060027FD RID: 10237 RVA: 0x0001A5FE File Offset: 0x000187FE
		private void Start()
		{
			CameraEffect.Shake(this.Power);
		}

		// Token: 0x040031F9 RID: 12793
		public Vector3 Power = Vector3.up;
	}
}
