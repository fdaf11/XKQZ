using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000034 RID: 52
	public class HelpState : BaseState
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x00002C40 File Offset: 0x00000E40
		protected override void Init(AIBehaviors fsm)
		{
			fsm.PlayAudio();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void StateEnded(AIBehaviors fsm)
		{
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00023CF8 File Offset: 0x00021EF8
		protected override bool Reason(AIBehaviors fsm)
		{
			if (Vector3.Distance(fsm.transform.position, this.helpPoint) < this.withinHelpPointDistance && this.helpPointReachedState != null)
			{
				fsm.ChangeActiveState(this.helpPointReachedState);
				return false;
			}
			return true;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00002E21 File Offset: 0x00001021
		protected override void Action(AIBehaviors fsm)
		{
			fsm.MoveAgent(this.helpPoint, this.movementSpeed, this.rotationSpeed);
		}

		// Token: 0x040000CC RID: 204
		public float withinHelpPointDistance = 1f;

		// Token: 0x040000CD RID: 205
		public Vector3 helpPoint;

		// Token: 0x040000CE RID: 206
		public BaseState helpPointReachedState;
	}
}
