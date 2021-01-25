using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x0200001B RID: 27
	public class MecanimNavMeshPathScript : MonoBehaviour
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00022FC8 File Offset: 0x000211C8
		private void Start()
		{
			AIBehaviors component = base.GetComponent<AIBehaviors>();
			component.externalMove = new AIBehaviors.ExternalMoveDelegate(this.OnNewDestination);
			this.navMeshPath = new NavMeshPath();
			if (this.mecanimAnimator == null)
			{
				this.mecanimAnimator = base.GetComponentInChildren<Animator>();
			}
			this.mecanimTransform = this.mecanimAnimator.transform;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000029FA File Offset: 0x00000BFA
		private void OnNewDestination(Vector3 targetPoint, float movementSpeed, float rotationSpeed)
		{
			this.movementSpeed = movementSpeed;
			this.rotationSpeed = rotationSpeed;
			NavMesh.CalculatePath(this.mecanimTransform.position, targetPoint, 255, this.navMeshPath);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00023028 File Offset: 0x00021228
		private void Update()
		{
			this.UpdateVerticalAndHorizontalMovement();
			this.mecanimAnimator.SetFloat(this.speedVariable, this.v * this.movementSpeed);
			this.mecanimAnimator.SetFloat(this.directionVariable, this.h * this.rotationSpeed);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00023078 File Offset: 0x00021278
		private void UpdateVerticalAndHorizontalMovement()
		{
			Vector3 characterOffsetVector = this.GetCharacterOffsetVector();
			bool flag = this.ShouldTurn(characterOffsetVector);
			float num = 0f;
			if (flag)
			{
				num = this.GetTurnValue(characterOffsetVector);
			}
			this.h = Mathf.SmoothStep(this.h, num, Time.deltaTime * this.rotationLerpRate);
			this.v = Mathf.Lerp(0.2f, 1f, 1f - Mathf.Abs(this.h));
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000230EC File Offset: 0x000212EC
		private Vector3 GetCharacterOffsetVector()
		{
			if (this.navMeshPath.corners.Length > 1)
			{
				this.curPoint = 1;
				return this.mecanimTransform.InverseTransformPoint(this.navMeshPath.corners[1]);
			}
			if (this.navMeshPath.corners.Length > 0)
			{
				this.curPoint = 0;
				return this.mecanimTransform.InverseTransformPoint(this.navMeshPath.corners[0]);
			}
			return this.mecanimTransform.position;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002A27 File Offset: 0x00000C27
		private bool ShouldMoveForward(Vector3 offsetVector)
		{
			return offsetVector.sqrMagnitude > this.minDistanceMoveThreshold * this.minDistanceMoveThreshold;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0002317C File Offset: 0x0002137C
		private bool ShouldTurn(Vector3 offsetVector)
		{
			return offsetVector.normalized.z < 0.99f;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000231A0 File Offset: 0x000213A0
		private float GetTurnValue(Vector3 offsetVector)
		{
			return offsetVector.normalized.x;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000231BC File Offset: 0x000213BC
		private void OnDrawGizmos()
		{
			if (this.navMeshPath != null)
			{
				for (int i = 0; i < this.navMeshPath.corners.Length; i++)
				{
					Gizmos.color = ((this.curPoint != i) ? Color.white : Color.red);
					Gizmos.DrawCube(this.navMeshPath.corners[i], Vector3.one);
				}
			}
		}

		// Token: 0x04000057 RID: 87
		public Animator mecanimAnimator;

		// Token: 0x04000058 RID: 88
		public string speedVariable = "Speed";

		// Token: 0x04000059 RID: 89
		public string directionVariable = "Direction";

		// Token: 0x0400005A RID: 90
		public float rotationLerpRate = 5f;

		// Token: 0x0400005B RID: 91
		public float minDistanceMoveThreshold = 3f;

		// Token: 0x0400005C RID: 92
		private Transform mecanimTransform;

		// Token: 0x0400005D RID: 93
		private NavMeshPath navMeshPath;

		// Token: 0x0400005E RID: 94
		private float movementSpeed;

		// Token: 0x0400005F RID: 95
		private float rotationSpeed;

		// Token: 0x04000060 RID: 96
		private float h;

		// Token: 0x04000061 RID: 97
		private float v;

		// Token: 0x04000062 RID: 98
		private int curPoint;
	}
}
