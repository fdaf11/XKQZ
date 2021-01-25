using System;

namespace AIBehavior
{
	// Token: 0x02000048 RID: 72
	public class LowHealthTrigger : HealthTrigger
	{
		// Token: 0x06000142 RID: 322 RVA: 0x00003157 File Offset: 0x00001357
		public override bool IsThresholdCrossed(AIBehaviors fsm)
		{
			return fsm.GetHealthValue() <= this.healthThreshold;
		}
	}
}
