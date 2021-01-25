using System;

namespace AIBehavior
{
	// Token: 0x02000049 RID: 73
	public class NoPlayerInSightTrigger : BaseTrigger
	{
		// Token: 0x06000144 RID: 324 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void Init()
		{
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000316A File Offset: 0x0000136A
		protected override bool Evaluate(AIBehaviors fsm)
		{
			return fsm.GetClosestPlayerWithinSight(this.objectFinder.GetTransforms(), false) == null;
		}
	}
}
