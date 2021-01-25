using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000025 RID: 37
	public struct AttackData
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00002AAA File Offset: 0x00000CAA
		public AttackData(Transform target, float damage, AttackState attackState)
		{
			this.target = target;
			this.damage = damage;
			this.attackState = attackState;
		}

		// Token: 0x04000075 RID: 117
		public Transform target;

		// Token: 0x04000076 RID: 118
		public float damage;

		// Token: 0x04000077 RID: 119
		public AttackState attackState;
	}
}
