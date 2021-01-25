using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000040 RID: 64
	[ExcludeFromSubTriggersList]
	public class GoToRandomStateTrigger : BaseTrigger
	{
		// Token: 0x06000126 RID: 294 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void Init()
		{
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000305B File Offset: 0x0000125B
		protected override bool Evaluate(AIBehaviors fsm)
		{
			return this.GetRandomResult(fsm, out this.result, Random.value);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000246F8 File Offset: 0x000228F8
		public bool GetRandomResult(AIBehaviors fsm, out BaseState resultState, float randomValue)
		{
			resultState = null;
			if (this.states.Length > 0)
			{
				float num = 1f;
				for (int i = 0; i < this.states.Length; i++)
				{
					if (randomValue > num - this.weights[i])
					{
						resultState = this.states[i];
						return true;
					}
					num -= this.weights[i];
				}
			}
			else
			{
				Debug.LogWarning("The 'Go To Random State Trigger' requires at least 1 possible state!");
			}
			resultState = this.transitionState;
			return true;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000306F File Offset: 0x0000126F
		protected override void ChangeToTransitionState(AIBehaviors fsm)
		{
			fsm.ChangeActiveState(this.result);
		}

		// Token: 0x040000F7 RID: 247
		public BaseState[] states;

		// Token: 0x040000F8 RID: 248
		private BaseState result;

		// Token: 0x040000F9 RID: 249
		public float[] weights;
	}
}
