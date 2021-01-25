using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000651 RID: 1617
	public class FX_Camera : MonoBehaviour
	{
		// Token: 0x060027D9 RID: 10201 RVA: 0x0001A434 File Offset: 0x00018634
		private void Start()
		{
			CameraEffect.CameraFX = this;
			this.positionTemp = base.transform.position;
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x0001A44D File Offset: 0x0001864D
		public void Shake(Vector3 power)
		{
			this.forcePower = -power;
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x0013B8AC File Offset: 0x00139AAC
		private void Update()
		{
			this.forcePower = Vector3.Lerp(this.forcePower, Vector3.zero, Time.deltaTime * 5f);
			base.transform.position = this.positionTemp + new Vector3(Mathf.Cos(Time.time * 80f) * this.forcePower.x, Mathf.Cos(Time.time * 80f) * this.forcePower.y, Mathf.Cos(Time.time * 80f) * this.forcePower.z);
		}

		// Token: 0x040031D7 RID: 12759
		private Vector3 positionTemp;

		// Token: 0x040031D8 RID: 12760
		private Vector3 forcePower;
	}
}
