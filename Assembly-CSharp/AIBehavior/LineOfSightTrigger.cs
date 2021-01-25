using System;

namespace AIBehavior
{
	// Token: 0x02000047 RID: 71
	public class LineOfSightTrigger : BaseTrigger
	{
		// Token: 0x0600013F RID: 319 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void Init()
		{
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000313D File Offset: 0x0000133D
		protected override bool Evaluate(AIBehaviors fsm)
		{
			return fsm.GetClosestPlayerWithinSight(this.objectFinder.GetTransforms(), true) != null;
		}
	}
}
