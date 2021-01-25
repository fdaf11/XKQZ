using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000037 RID: 55
	public class PatrolState : BaseState
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x00023F00 File Offset: 0x00022100
		public PatrolState()
		{
			this.currentPatrolPoint = 0;
			this.patrolDirection = 1;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00002E8F File Offset: 0x0000108F
		protected override void Awake()
		{
			base.Awake();
			if (this.isEnabled)
			{
				this.SortPatrolPoints();
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00002EA8 File Offset: 0x000010A8
		public void SetPatrolPoints(Transform patrolPointsGroup)
		{
			if (patrolPointsGroup != null)
			{
				this.patrolPointsGroup = patrolPointsGroup;
				this.SortPatrolPoints();
				this.ResetPatrol();
			}
			else
			{
				this.patrolPoints = null;
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00002ED5 File Offset: 0x000010D5
		public void SetPatrolPoints(Transform[] newPatrolPoints)
		{
			if (newPatrolPoints.Length >= 2)
			{
				this.patrolPoints = newPatrolPoints;
				this.ResetPatrol();
			}
			else
			{
				Debug.LogError("In order to set patrol points, the array must contain at least two points");
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00002EFC File Offset: 0x000010FC
		public Transform GetCurrentPatrolTarget()
		{
			if (this.currentPatrolPoint < 0 || this.currentPatrolPoint >= this.patrolPoints.Length)
			{
				return null;
			}
			return this.patrolPoints[this.currentPatrolPoint];
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00023F50 File Offset: 0x00022150
		private void SortPatrolPoints()
		{
			if (this.patrolPointsGroup != null)
			{
				Transform[] componentsInChildren = this.patrolPointsGroup.GetComponentsInChildren<Transform>();
				int num = 0;
				this.patrolPoints = new Transform[componentsInChildren.Length - 1];
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i] != this.patrolPointsGroup)
					{
						this.patrolPoints[num] = componentsInChildren[i];
						num++;
					}
				}
				for (int j = 0; j < this.patrolPoints.Length; j++)
				{
					for (int k = j + 1; k < this.patrolPoints.Length; k++)
					{
						if (this.patrolPoints[j].name.CompareTo(this.patrolPoints[k].name) > 0)
						{
							Transform transform = this.patrolPoints[j];
							this.patrolPoints[j] = this.patrolPoints[k];
							this.patrolPoints[k] = transform;
						}
					}
				}
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00024048 File Offset: 0x00022248
		protected override void Init(AIBehaviors fsm)
		{
			if (this.patrolPointsGroup == null && base.transform.parent != null)
			{
				Debug.LogWarning("The variable 'patrolPointsGroup' is unassigned for the 'Patrol' state on " + base.transform.parent.name);
			}
			if (this.patrolPoints == null)
			{
				this.SortPatrolPoints();
			}
			if (this.rotationHelper == null)
			{
				this.rotationHelper = new GameObject("RotationHelper").transform;
				this.rotationHelper.parent = fsm.transform;
				this.rotationHelper.localPosition = Vector3.forward;
			}
			switch (this.continuePatrolMode)
			{
			case PatrolState.ContinuePatrolMode.Reset:
				this.patrolDirection = 1;
				this.currentPatrolPoint = 0;
				break;
			case PatrolState.ContinuePatrolMode.NearestNode:
			case PatrolState.ContinuePatrolMode.NearestNextNode:
			{
				Vector3 position = fsm.transform.position;
				int num = 0;
				float sqrDistanceThreshold = float.PositiveInfinity;
				for (int i = 0; i < this.patrolPoints.Length; i++)
				{
					float sqrMagnitude = (position - this.patrolPoints[i].position).sqrMagnitude;
					if (this.CheckIfWithinThreshold(position, this.patrolPoints[i].position, sqrDistanceThreshold))
					{
						sqrDistanceThreshold = sqrMagnitude;
						num = i;
					}
				}
				if (this.continuePatrolMode == PatrolState.ContinuePatrolMode.NearestNode)
				{
					this.currentPatrolPoint = num;
				}
				else if (this.patrolPoints.Length != 0)
				{
					this.currentPatrolPoint = (num + 1) % this.patrolPoints.Length;
				}
				break;
			}
			case PatrolState.ContinuePatrolMode.Random:
				this.currentPatrolPoint = Random.Range(0, this.patrolPoints.Length);
				break;
			}
			fsm.RotateAgent(this.rotationHelper, this.rotationSpeed);
			fsm.PlayAudio();
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void StateEnded(AIBehaviors fsm)
		{
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00002F2C File Offset: 0x0000112C
		protected override bool Reason(AIBehaviors fsm)
		{
			if (this.patrolMode == PatrolState.PatrolMode.Once && this.currentPatrolPoint >= this.patrolPoints.Length)
			{
				this.currentPatrolPoint = 0;
				fsm.ChangeActiveState(this.patrolEndedState);
				return false;
			}
			return true;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0002420C File Offset: 0x0002240C
		protected override void Action(AIBehaviors fsm)
		{
			Vector3 position = this.patrolPoints[this.currentPatrolPoint].position;
			Vector3 position2 = fsm.transform.position;
			fsm.MoveAgent(position, this.movementSpeed, this.rotationSpeed);
			if (this.CheckIfWithinThreshold(position2, position, this.pointDistanceThreshold * this.pointDistanceThreshold))
			{
				this.GetNextPatrolPoint();
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0002426C File Offset: 0x0002246C
		protected virtual bool CheckIfWithinThreshold(Vector3 currentPosition, Vector3 destination, float sqrDistanceThreshold)
		{
			return (currentPosition - destination).sqrMagnitude < sqrDistanceThreshold;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0002428C File Offset: 0x0002248C
		private void GetNextPatrolPoint()
		{
			this.currentPatrolPoint += this.patrolDirection;
			if (this.patrolMode == PatrolState.PatrolMode.Loop)
			{
				this.currentPatrolPoint %= this.patrolPoints.Length;
			}
			else if (this.patrolMode == PatrolState.PatrolMode.PingPong)
			{
				if (this.patrolDirection == 1 && this.currentPatrolPoint == this.patrolPoints.Length)
				{
					this.currentPatrolPoint = this.patrolPoints.Length - 1;
					this.patrolDirection = -1;
				}
				else if (this.currentPatrolPoint == 0)
				{
					this.currentPatrolPoint = 0;
					this.patrolDirection = 1;
				}
			}
			else if (this.patrolMode == PatrolState.PatrolMode.Random && this.patrolPoints.Length > 1)
			{
				int num;
				for (num = this.currentPatrolPoint; num == this.currentPatrolPoint; num = Random.Range(0, this.patrolPoints.Length))
				{
				}
				this.currentPatrolPoint = num;
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00002F62 File Offset: 0x00001162
		public void ResetPatrol()
		{
			this.currentPatrolPoint = 0;
			this.patrolDirection = 1;
			this.GetNextPatrolPoint();
		}

		// Token: 0x040000D4 RID: 212
		private Transform rotationHelper;

		// Token: 0x040000D5 RID: 213
		public Transform patrolPointsGroup;

		// Token: 0x040000D6 RID: 214
		private Transform[] patrolPoints = new Transform[0];

		// Token: 0x040000D7 RID: 215
		private int currentPatrolPoint;

		// Token: 0x040000D8 RID: 216
		private int patrolDirection = 1;

		// Token: 0x040000D9 RID: 217
		public float pointDistanceThreshold = 1f;

		// Token: 0x040000DA RID: 218
		public PatrolState.PatrolMode patrolMode = PatrolState.PatrolMode.Loop;

		// Token: 0x040000DB RID: 219
		public PatrolState.ContinuePatrolMode continuePatrolMode = PatrolState.ContinuePatrolMode.NearestNextNode;

		// Token: 0x040000DC RID: 220
		public BaseState patrolEndedState;

		// Token: 0x02000038 RID: 56
		public enum PatrolMode
		{
			// Token: 0x040000DE RID: 222
			Once,
			// Token: 0x040000DF RID: 223
			Loop,
			// Token: 0x040000E0 RID: 224
			PingPong,
			// Token: 0x040000E1 RID: 225
			Random
		}

		// Token: 0x02000039 RID: 57
		public enum ContinuePatrolMode
		{
			// Token: 0x040000E3 RID: 227
			Reset,
			// Token: 0x040000E4 RID: 228
			ContinuePrevious,
			// Token: 0x040000E5 RID: 229
			NearestNode,
			// Token: 0x040000E6 RID: 230
			NearestNextNode,
			// Token: 0x040000E7 RID: 231
			Random
		}
	}
}
