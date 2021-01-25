using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000035 RID: 53
	public class IdleState : BaseState
	{
		// Token: 0x060000EC RID: 236 RVA: 0x00023D48 File Offset: 0x00021F48
		protected override void Init(AIBehaviors fsm)
		{
			Transform transform = fsm.transform;
			this.targetRotationObject = new GameObject("RotationTarget");
			Transform transform2 = this.targetRotationObject.transform;
			transform2.position = transform.position + transform.forward;
			fsm.RotateAgent(transform2, this.rotationSpeed);
			transform2.parent = transform;
			fsm.PlayAudio();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00002E3B File Offset: 0x0000103B
		protected override void StateEnded(AIBehaviors fsm)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(this.targetRotationObject);
			}
			else
			{
				Object.DestroyImmediate(this.targetRotationObject);
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00002B59 File Offset: 0x00000D59
		protected override bool Reason(AIBehaviors fsm)
		{
			return true;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00023DAC File Offset: 0x00021FAC
		protected override void Action(AIBehaviors fsm)
		{
			if (!this.rotatesTowardTarget)
			{
				fsm.MoveAgent(fsm.transform, 0f, this.rotationSpeed);
			}
			else
			{
				Transform closestPlayer = fsm.GetClosestPlayer(this.objectFinder.GetTransforms());
				if (closestPlayer != null)
				{
					fsm.MoveAgent(closestPlayer, 0f, this.rotationSpeed);
				}
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00002E62 File Offset: 0x00001062
		public override bool RotatesTowardTarget()
		{
			return this.rotatesTowardTarget;
		}

		// Token: 0x040000CF RID: 207
		public bool rotatesTowardTarget;

		// Token: 0x040000D0 RID: 208
		private GameObject targetRotationObject;
	}
}
