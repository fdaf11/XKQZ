using System;
using AIBehavior;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x0200000A RID: 10
	public class ExampleGotHitComponent : MonoBehaviour
	{
		// Token: 0x06000019 RID: 25 RVA: 0x0000261D File Offset: 0x0000081D
		private void Awake()
		{
			this.fsm = base.GetComponent<AIBehaviors>();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000262B File Offset: 0x0000082B
		private void OnTriggerEnter(Collider col)
		{
			if (col.GetComponent<ProjectileCollider>() != null)
			{
				this.fsm.GotHit(this.projectileDamage);
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000264F File Offset: 0x0000084F
		public void OnStartDefending(DefendState defendState)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000264F File Offset: 0x0000084F
		public void OnStopDefending(DefendState defendState)
		{
		}

		// Token: 0x0400000C RID: 12
		public float projectileDamage = 10f;

		// Token: 0x0400000D RID: 13
		private AIBehaviors fsm;
	}
}
