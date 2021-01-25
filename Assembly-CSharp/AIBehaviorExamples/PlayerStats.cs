using System;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x0200000F RID: 15
	public class PlayerStats : MonoBehaviour
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00002788 File Offset: 0x00000988
		public void SubtractHealth(float amount)
		{
			this.health -= amount;
			if (this.health <= 0f)
			{
				this.health = 0f;
				Debug.LogWarning("You're Dead!");
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000027BD File Offset: 0x000009BD
		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<ProjectileCollider>() != null)
			{
				this.SubtractHealth(10f);
			}
		}

		// Token: 0x0400001F RID: 31
		public float health = 100f;
	}
}
