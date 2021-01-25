using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x0200002E RID: 46
	public class FleeState : BaseState
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x00002D4E File Offset: 0x00000F4E
		protected override void Init(AIBehaviors fsm)
		{
			this.sqrDistanceToTargetThreshold = this.distanceToTargetThreshold * this.distanceToTargetThreshold;
			fsm.PlayAudio();
			this.fleeToObjects = GameObject.FindGameObjectsWithTag(this.fleeTargetTag);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void StateEnded(AIBehaviors fsm)
		{
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00023948 File Offset: 0x00021B48
		protected override bool Reason(AIBehaviors fsm)
		{
			if (this.currentTarget != null)
			{
				float sqrMagnitude = (fsm.transform.position - this.currentTarget.position).sqrMagnitude;
				if (sqrMagnitude < this.sqrDistanceToTargetThreshold)
				{
					fsm.ChangeActiveState(this.fleeTargetReachedState);
					return false;
				}
			}
			return true;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000239A8 File Offset: 0x00021BA8
		protected override void Action(AIBehaviors fsm)
		{
			FleeState.FleeMode fleeMode = this.fleeMode;
			if (fleeMode != FleeState.FleeMode.NearestTaggedObject)
			{
				if (fleeMode == FleeState.FleeMode.FixedTarget)
				{
					if (this.fleeToTarget != null)
					{
						this.currentTarget = this.fleeToTarget;
						fsm.MoveAgent(this.fleeToTarget, this.movementSpeed, this.rotationSpeed);
					}
					else
					{
						Debug.LogWarning("Flee To Target isn't set for FleeState");
					}
				}
			}
			else
			{
				float num = float.PositiveInfinity;
				int num2 = -1;
				for (int i = 0; i < this.fleeToObjects.Length; i++)
				{
					Vector3 vector = this.fleeToObjects[i].transform.position - base.transform.position;
					if (vector.sqrMagnitude < num)
					{
						num = vector.sqrMagnitude;
						num2 = i;
					}
				}
				if (num2 != -1)
				{
					this.currentTarget = this.fleeToObjects[num2].transform;
					fsm.MoveAgent(this.currentTarget, this.movementSpeed, this.rotationSpeed);
				}
			}
		}

		// Token: 0x040000AC RID: 172
		public float startFleeDistance = 5f;

		// Token: 0x040000AD RID: 173
		public FleeState.FleeMode fleeMode;

		// Token: 0x040000AE RID: 174
		public string fleeTargetTag = "Untagged";

		// Token: 0x040000AF RID: 175
		public Transform fleeToTarget;

		// Token: 0x040000B0 RID: 176
		public Vector3 fleeDirection;

		// Token: 0x040000B1 RID: 177
		private Transform currentTarget;

		// Token: 0x040000B2 RID: 178
		public BaseState fleeTargetReachedState;

		// Token: 0x040000B3 RID: 179
		public float distanceToTargetThreshold = 1f;

		// Token: 0x040000B4 RID: 180
		private float sqrDistanceToTargetThreshold = 1f;

		// Token: 0x040000B5 RID: 181
		private GameObject[] fleeToObjects;

		// Token: 0x0200002F RID: 47
		public enum FleeMode
		{
			// Token: 0x040000B7 RID: 183
			NearestTaggedObject,
			// Token: 0x040000B8 RID: 184
			FixedTarget
		}
	}
}
