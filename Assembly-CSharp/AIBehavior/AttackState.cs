using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000027 RID: 39
	public class AttackState : CooldownableState
	{
		// Token: 0x0600009C RID: 156 RVA: 0x00002B3B File Offset: 0x00000D3B
		protected override void Awake()
		{
			this.skinnedMeshRenderer = base.transform.root.GetComponentInChildren<SkinnedMeshRenderer>();
			base.Awake();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00023354 File Offset: 0x00021554
		protected override void Init(AIBehaviors fsm)
		{
			fsm.MoveAgent(fsm.transform, 0f, this.rotationSpeed);
			this.attackAnimation = fsm.gameObject.GetComponentInChildren<Animation>();
			if (this.attackAnimation != null && this.attackAnimation[this.attackAnimName] != null)
			{
				this.animationLength = this.attackAnimation[this.attackAnimName].length;
			}
			else
			{
				this.animationLength = 1f;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void StateEnded(AIBehaviors fsm)
		{
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00002B59 File Offset: 0x00000D59
		protected override bool Reason(AIBehaviors fsm)
		{
			return true;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000233E4 File Offset: 0x000215E4
		protected override void Action(AIBehaviors fsm)
		{
			Transform closestPlayer = fsm.GetClosestPlayer(this.objectFinder.GetTransforms());
			string name = this.animationStates[0].name;
			bool flag = !(this.skinnedMeshRenderer != null) || this.skinnedMeshRenderer.isVisible;
			if (!flag && this.attackAnimation != null)
			{
				flag = (this.attackAnimation.cullingType == 0);
			}
			if (closestPlayer != null)
			{
				fsm.MoveAgent(closestPlayer, this.movementSpeed, this.rotationSpeed);
			}
			AIAnimationState stateWithName = fsm.animationStates.GetStateWithName(name);
			fsm.PlayAnimation(stateWithName);
			if (this.scriptWithAttackMethod != null && !string.IsNullOrEmpty(this.methodName))
			{
				if (flag && this.attackAnimation != null && this.attackAnimation[name] != null)
				{
					this.curAnimPosition = this.attackAnimation[name].normalizedTime % 1f;
				}
				else
				{
					this.curAnimPosition %= 1f;
					this.curAnimPosition += Time.deltaTime / this.animationLength;
				}
				if (this.previousSamplePosition > this.attackPoint || this.curAnimPosition < this.attackPoint)
				{
					this.previousSamplePosition = this.curAnimPosition;
					return;
				}
				this.previousSamplePosition = this.curAnimPosition;
				base.TriggerCooldown();
				this.Attack(fsm, closestPlayer);
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00023574 File Offset: 0x00021774
		protected virtual void Attack(AIBehaviors fsm, Transform target)
		{
			this.scriptWithAttackMethod.SendMessage(this.methodName, new AttackData(target, this.attackDamage, this));
			fsm.PlayAudio();
			this.attackCount++;
			if (this.attackCount > this.attacksBeforeReload)
			{
				this.attackCount = 0;
				fsm.ChangeActiveState(this.reloadState);
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00002B59 File Offset: 0x00000D59
		public override bool RotatesTowardTarget()
		{
			return true;
		}

		// Token: 0x0400007C RID: 124
		public float attackDamage = 10f;

		// Token: 0x0400007D RID: 125
		public string attackAnimName = string.Empty;

		// Token: 0x0400007E RID: 126
		public float attackPoint = 0.5f;

		// Token: 0x0400007F RID: 127
		public float animAttackTime;

		// Token: 0x04000080 RID: 128
		public Component scriptWithAttackMethod;

		// Token: 0x04000081 RID: 129
		public string methodName = string.Empty;

		// Token: 0x04000082 RID: 130
		private Animation attackAnimation;

		// Token: 0x04000083 RID: 131
		private float animationLength;

		// Token: 0x04000084 RID: 132
		private float curAnimPosition;

		// Token: 0x04000085 RID: 133
		protected float previousSamplePosition;

		// Token: 0x04000086 RID: 134
		public int attacksBeforeReload = 10;

		// Token: 0x04000087 RID: 135
		public int attackCount;

		// Token: 0x04000088 RID: 136
		public BaseState reloadState;

		// Token: 0x04000089 RID: 137
		private SkinnedMeshRenderer skinnedMeshRenderer;
	}
}
