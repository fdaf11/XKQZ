using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x0200003A RID: 58
	public class SeekState : BaseState
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00002FA1 File Offset: 0x000011A1
		protected override void Init(AIBehaviors fsm)
		{
			this.sqrDistanceToTargetThreshold = this.distanceToTargetThreshold * this.distanceToTargetThreshold;
			fsm.PlayAudio();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void StateEnded(AIBehaviors fsm)
		{
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00024380 File Offset: 0x00022580
		protected override bool Reason(AIBehaviors fsm)
		{
			if (this.targetItem == null)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag(this.seekItemsWithTag);
				Vector3 position = fsm.transform.position;
				float num = float.PositiveInfinity;
				foreach (GameObject gameObject in array)
				{
					Transform transform = gameObject.transform;
					float sqrMagnitude = (transform.position - position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						this.targetItem = transform;
						num = sqrMagnitude;
					}
				}
			}
			else
			{
				float sqrMagnitude2 = (fsm.transform.position - this.targetItem.position).sqrMagnitude;
				if (sqrMagnitude2 < this.sqrDistanceToTargetThreshold)
				{
					if (this.destroyTargetWhenReached)
					{
						Object.Destroy(this.targetItem.gameObject);
					}
					fsm.ChangeActiveState(this.seekTargetReachedState);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00002FBC File Offset: 0x000011BC
		protected override void Action(AIBehaviors fsm)
		{
			if (this.targetItem != null)
			{
				fsm.MoveAgent(this.targetItem, this.movementSpeed, this.rotationSpeed);
			}
		}

		// Token: 0x040000E8 RID: 232
		public string seekItemsWithTag = "Untagged";

		// Token: 0x040000E9 RID: 233
		private Transform targetItem;

		// Token: 0x040000EA RID: 234
		public BaseState seekTargetReachedState;

		// Token: 0x040000EB RID: 235
		public float distanceToTargetThreshold = 0.25f;

		// Token: 0x040000EC RID: 236
		private float sqrDistanceToTargetThreshold = 1f;

		// Token: 0x040000ED RID: 237
		public bool destroyTargetWhenReached;
	}
}
