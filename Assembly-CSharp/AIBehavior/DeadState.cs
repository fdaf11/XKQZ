using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x0200002C RID: 44
	public class DeadState : BaseState
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x00023858 File Offset: 0x00021A58
		protected override void Init(AIBehaviors fsm)
		{
			fsm.PlayAudio();
			fsm.MoveAgent(fsm.transform, 0f, 0f);
			this.destroyThis = (this.destroyAfterTime > 0f);
			this.destroyTime = Time.time + this.destroyAfterTime;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void StateEnded(AIBehaviors fsm)
		{
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00002B59 File Offset: 0x00000D59
		protected override bool Reason(AIBehaviors fsm)
		{
			return true;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00002CB7 File Offset: 0x00000EB7
		protected override void Action(AIBehaviors fsm)
		{
			if (this.destroyThis && Time.time > this.destroyTime)
			{
				Object.Destroy(fsm.gameObject);
			}
		}

		// Token: 0x040000A5 RID: 165
		public float destroyAfterTime;

		// Token: 0x040000A6 RID: 166
		private bool destroyThis;

		// Token: 0x040000A7 RID: 167
		private float destroyTime;
	}
}
