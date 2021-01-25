using System;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x0200000D RID: 13
	public class ExampleProjectile : MonoBehaviour
	{
		// Token: 0x06000028 RID: 40 RVA: 0x000026A5 File Offset: 0x000008A5
		private void Awake()
		{
			base.rigidbody.AddRelativeForce(Vector3.forward * 1000f);
			Object.Destroy(base.gameObject, 10f);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000220F8 File Offset: 0x000202F8
		private void OnTriggerEnter(Collider col)
		{
			if (col.gameObject.tag == this.hitTag)
			{
				PlayerStats component = col.GetComponent<PlayerStats>();
				if (component != null)
				{
					component.SubtractHealth(this.damage);
					Debug.LogWarning(string.Concat(new object[]
					{
						"Attack: ",
						this.damage,
						" : health=",
						component.health
					}));
				}
				this.SpawnFlames();
				col.SendMessage("GotHit", this.damage);
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000026D1 File Offset: 0x000008D1
		private void OnCollisionEnter(Collision col)
		{
			this.SpawnFlames();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000026D9 File Offset: 0x000008D9
		private void SpawnFlames()
		{
			Object.Destroy(base.gameObject);
			Object.Instantiate(this.explosionPrefab, base.transform.position, Quaternion.identity);
		}

		// Token: 0x04000016 RID: 22
		public float damage;

		// Token: 0x04000017 RID: 23
		public GameObject explosionPrefab;

		// Token: 0x04000018 RID: 24
		public string hitTag = "Player";
	}
}
