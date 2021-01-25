using System;

namespace AIBehavior
{
	// Token: 0x02000042 RID: 66
	public class HighHealthTrigger : HealthTrigger
	{
		// Token: 0x0600012F RID: 303 RVA: 0x000030A1 File Offset: 0x000012A1
		public override bool IsThresholdCrossed(AIBehaviors fsm)
		{
			return fsm.GetHealthValue() > this.healthThreshold;
		}
	}
}
