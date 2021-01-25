using System;

namespace AIBehavior
{
	// Token: 0x0200003E RID: 62
	public class CurrentStateTrigger : BaseTrigger
	{
		// Token: 0x0600011C RID: 284 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void Init()
		{
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00024618 File Offset: 0x00022818
		protected override bool Evaluate(AIBehaviors fsm)
		{
			bool flag = fsm.currentState == this.checkState;
			if (this.isOrNot == IsAndIsNot.IsNot)
			{
				return !flag;
			}
			return flag;
		}

		// Token: 0x040000F3 RID: 243
		public BaseState checkState;

		// Token: 0x040000F4 RID: 244
		public IsAndIsNot isOrNot;
	}
}
