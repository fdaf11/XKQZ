using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000652 RID: 1618
	public static class CameraEffect
	{
		// Token: 0x060027DC RID: 10204 RVA: 0x0001A45B File Offset: 0x0001865B
		public static void Shake(Vector3 power)
		{
			if (CameraEffect.CameraFX != null)
			{
				CameraEffect.CameraFX.Shake(power);
			}
		}

		// Token: 0x040031D9 RID: 12761
		public static FX_Camera CameraFX;
	}
}
