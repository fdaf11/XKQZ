using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000036 RID: 54
	public class MecanimAttackState : AttackState
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00023E10 File Offset: 0x00022010
		protected override void Awake()
		{
			base.Awake();
			this.animator = base.transform.parent.GetComponentInChildren<Animator>();
			if (this.animator == null)
			{
				Debug.LogWarning("An Animator component must be attached when using the " + base.GetType());
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00002E72 File Offset: 0x00001072
		protected override void Init(AIBehaviors fsm)
		{
			this.prevNormalizedTime = 0f;
			base.Init(fsm);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00023E60 File Offset: 0x00022060
		protected override void Action(AIBehaviors fsm)
		{
			AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(this.mecanimLayerIndex);
			int num = Animator.StringToHash(this.animationStates[0].name);
			if (num == currentAnimatorStateInfo.nameHash)
			{
				float num2 = currentAnimatorStateInfo.normalizedTime % 1f;
				if (num2 > this.attackPoint && this.prevNormalizedTime < this.attackPoint && this.scriptWithAttackMethod != null)
				{
					base.TriggerCooldown();
					this.Attack(fsm, fsm.GetClosestPlayerWithinSight(this.objectFinder.GetTransforms()));
				}
				this.prevNormalizedTime = num2;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00002E86 File Offset: 0x00001086
		protected override void StateEnded(AIBehaviors fsm)
		{
			base.StateEnded(fsm);
		}

		// Token: 0x040000D1 RID: 209
		private Animator animator;

		// Token: 0x040000D2 RID: 210
		private float prevNormalizedTime;

		// Token: 0x040000D3 RID: 211
		public int mecanimLayerIndex;
	}
}
