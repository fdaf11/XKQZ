using System;

namespace AIBehavior
{
	// Token: 0x02000041 RID: 65
	public abstract class HealthTrigger : BaseTrigger
	{
		// Token: 0x0600012B RID: 299 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void Init()
		{
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00003090 File Offset: 0x00001290
		protected override bool Evaluate(AIBehaviors fsm)
		{
			return this.IsThresholdCrossed(fsm);
		}

		// Token: 0x0600012D RID: 301
		public abstract bool IsThresholdCrossed(AIBehaviors fsm);

		// Token: 0x040000FA RID: 250
		public float healthThreshold = 50f;
	}
}
