using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000030 RID: 48
	public class FollowState : BaseState
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00002D7A File Offset: 0x00000F7A
		protected override void Init(AIBehaviors fsm)
		{
			fsm.PlayAudio();
			this.lastSightedPlayerLocation = fsm.transform.position;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void StateEnded(AIBehaviors fsm)
		{
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00023B04 File Offset: 0x00021D04
		protected override bool Reason(AIBehaviors fsm)
		{
			float positiveInfinity = float.PositiveInfinity;
			Transform closestPlayerWithinSight = fsm.GetClosestPlayerWithinSight(this.objectFinder.GetTransforms(), out positiveInfinity, false);
			if (closestPlayerWithinSight != null)
			{
				float num = this.startChaseDistance * this.startChaseDistance;
				float num2 = this.breakChaseDistance * this.breakChaseDistance;
				if (positiveInfinity > num2)
				{
					this.curFollowMode = FollowState.FollowMode.Normal;
				}
				else if (positiveInfinity < num)
				{
					this.curFollowMode = FollowState.FollowMode.Chase;
				}
				this.lastSightedPlayer = closestPlayerWithinSight;
				this.lastSightedPlayerLocation = this.lastSightedPlayer.position;
			}
			else
			{
				this.lastSightedPlayer = null;
			}
			return true;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00023B9C File Offset: 0x00021D9C
		protected override void Action(AIBehaviors fsm)
		{
			float movementSpeed;
			if (this.curFollowMode == FollowState.FollowMode.Chase)
			{
				movementSpeed = this.chaseSpeed;
			}
			else
			{
				movementSpeed = this.movementSpeed;
			}
			if (this.lastSightedPlayer != null)
			{
				fsm.MoveAgent(this.lastSightedPlayer, movementSpeed, this.rotationSpeed);
			}
			else
			{
				fsm.MoveAgent(this.lastSightedPlayerLocation, movementSpeed, this.rotationSpeed);
			}
		}

		// Token: 0x040000B9 RID: 185
		public Transform targetToFollow;

		// Token: 0x040000BA RID: 186
		public float chaseSpeed = 1f;

		// Token: 0x040000BB RID: 187
		public float startChaseDistance = 10f;

		// Token: 0x040000BC RID: 188
		public float breakChaseDistance = 20f;

		// Token: 0x040000BD RID: 189
		public float chaseDuration = 10f;

		// Token: 0x040000BE RID: 190
		public bool horizontalMove = true;

		// Token: 0x040000BF RID: 191
		public bool verticalMove;

		// Token: 0x040000C0 RID: 192
		public bool stopWhenTargetReached = true;

		// Token: 0x040000C1 RID: 193
		public float passingDistance = 10f;

		// Token: 0x040000C2 RID: 194
		private Transform lastSightedPlayer;

		// Token: 0x040000C3 RID: 195
		private Vector3 lastSightedPlayerLocation;

		// Token: 0x040000C4 RID: 196
		private FollowState.FollowMode curFollowMode;

		// Token: 0x02000031 RID: 49
		private enum FollowMode
		{
			// Token: 0x040000C6 RID: 198
			Normal,
			// Token: 0x040000C7 RID: 199
			Chase
		}
	}
}
