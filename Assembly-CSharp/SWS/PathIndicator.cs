using System;
using System.Collections;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006B3 RID: 1715
	public class PathIndicator : MonoBehaviour
	{
		// Token: 0x0600295E RID: 10590 RVA: 0x0001B2E2 File Offset: 0x000194E2
		private void Start()
		{
			this.pSys = base.GetComponentInChildren<ParticleSystem>();
			base.StartCoroutine("EmitParticles");
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x00147D08 File Offset: 0x00145F08
		private IEnumerator EmitParticles()
		{
			yield return new WaitForEndOfFrame();
			for (;;)
			{
				float rot = (base.transform.eulerAngles.y + this.modRotation) * 0.017453292f;
				this.pSys.startRotation = rot;
				this.pSys.Emit(1);
				yield return new WaitForSeconds(0.2f);
			}
			yield break;
		}

		// Token: 0x04003469 RID: 13417
		public float modRotation;

		// Token: 0x0400346A RID: 13418
		private ParticleSystem pSys;
	}
}
