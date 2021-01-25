using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000654 RID: 1620
	public class FX_LifeTime : MonoBehaviour
	{
		// Token: 0x060027E3 RID: 10211 RVA: 0x0001A4A6 File Offset: 0x000186A6
		private void Start()
		{
			if (this.SpawnAfterDead == null)
			{
				Object.Destroy(base.gameObject, this.LifeTime);
			}
			else
			{
				this.timeTemp = Time.time;
			}
		}

		// Token: 0x060027E4 RID: 10212 RVA: 0x0013BA14 File Offset: 0x00139C14
		private void Update()
		{
			if (this.SpawnAfterDead != null && Time.time > this.timeTemp + this.LifeTime)
			{
				Object.Destroy(base.gameObject);
				Object.Instantiate(this.SpawnAfterDead, base.transform.position, this.SpawnAfterDead.transform.rotation);
			}
		}

		// Token: 0x040031DF RID: 12767
		public float LifeTime = 3f;

		// Token: 0x040031E0 RID: 12768
		public GameObject SpawnAfterDead;

		// Token: 0x040031E1 RID: 12769
		private float timeTemp;
	}
}
