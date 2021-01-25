using System;

namespace AIBehavior
{
	// Token: 0x02000044 RID: 68
	public class IsAliveTrigger : BaseTrigger
	{
		// Token: 0x06000136 RID: 310 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void Init()
		{
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000030F4 File Offset: 0x000012F4
		protected override bool Evaluate(AIBehaviors fsm)
		{
			return fsm.health > 0f;
		}
	}
}
