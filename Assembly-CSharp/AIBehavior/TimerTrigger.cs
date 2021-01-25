using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x0200004C RID: 76
	public class TimerTrigger : BaseTrigger
	{
		// Token: 0x06000152 RID: 338 RVA: 0x000031C0 File Offset: 0x000013C0
		protected override void Init()
		{
			this.ResetTimer(Time.time, this.duration + (Random.value * this.plusOrMinusDuration - Random.value * this.plusOrMinusDuration));
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000031ED File Offset: 0x000013ED
		protected override bool Evaluate(AIBehaviors fsm)
		{
			return this.DidTimeExpire(Time.time);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000031FA File Offset: 0x000013FA
		public void ResetTimer(float currentTime, float duration)
		{
			this.timerExpiration = currentTime + duration;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00003205 File Offset: 0x00001405
		public bool DidTimeExpire(float currentTime)
		{
			return currentTime > this.timerExpiration;
		}

		// Token: 0x0400010E RID: 270
		public float duration = 1f;

		// Token: 0x0400010F RID: 271
		public float plusOrMinusDuration;

		// Token: 0x04000110 RID: 272
		private float timerExpiration;
	}
}
