using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000653 RID: 1619
	public class FX_HitSpawner : MonoBehaviour
	{
		// Token: 0x060027DE RID: 10206 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x0013B94C File Offset: 0x00139B4C
		private void Spawn()
		{
			if (this.FXSpawn != null)
			{
				Quaternion rotation = base.transform.rotation;
				if (!this.FixRotation)
				{
					rotation = this.FXSpawn.transform.rotation;
				}
				GameObject gameObject = (GameObject)Object.Instantiate(this.FXSpawn, base.transform.position, rotation);
				if (this.LifeTime > 0f)
				{
					Object.Destroy(gameObject.gameObject, this.LifeTime);
				}
			}
			if (this.DestoyOnHit)
			{
				Object.Destroy(base.gameObject, this.LifeTimeAfterHit);
				if (base.gameObject.collider)
				{
					base.gameObject.collider.enabled = false;
				}
			}
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x0001A48B File Offset: 0x0001868B
		private void OnTriggerEnter(Collider other)
		{
			this.Spawn();
		}

		// Token: 0x060027E1 RID: 10209 RVA: 0x0001A48B File Offset: 0x0001868B
		private void OnCollisionEnter(Collision collision)
		{
			this.Spawn();
		}

		// Token: 0x040031DA RID: 12762
		public GameObject FXSpawn;

		// Token: 0x040031DB RID: 12763
		public bool DestoyOnHit;

		// Token: 0x040031DC RID: 12764
		public bool FixRotation;

		// Token: 0x040031DD RID: 12765
		public float LifeTimeAfterHit = 1f;

		// Token: 0x040031DE RID: 12766
		public float LifeTime;
	}
}
