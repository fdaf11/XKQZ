using System;

namespace AIBehavior
{
	// Token: 0x02000033 RID: 51
	public class GotHitState : CooldownableState
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x00002DCB File Offset: 0x00000FCB
		protected override void Init(AIBehaviors fsm)
		{
			fsm.PlayAudio();
			base.TriggerCooldown();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void StateEnded(AIBehaviors fsm)
		{
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00002B59 File Offset: 0x00000D59
		protected override bool Reason(AIBehaviors fsm)
		{
			return true;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00002DD9 File Offset: 0x00000FD9
		protected override void Action(AIBehaviors fsm)
		{
			fsm.MoveAgent(fsm.currentDestination, this.movementSpeed, this.rotationSpeed);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00002DF3 File Offset: 0x00000FF3
		public new bool CoolDownFinished()
		{
			return base.CoolDownFinished();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00002DFB File Offset: 0x00000FFB
		public virtual bool CanGetHit(AIBehaviors fsm)
		{
			return !(fsm.currentState is DeadState);
		}

		// Token: 0x040000CA RID: 202
		public bool hitMovesPosition = true;

		// Token: 0x040000CB RID: 203
		public float movePositionAmount = 1f;
	}
}
