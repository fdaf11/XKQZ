using System;
using AIBehavior;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x0200000E RID: 14
	[RequireComponent(typeof(Rigidbody))]
	public class ExampleSpaceshipNavigation : MonoBehaviour
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00022198 File Offset: 0x00020398
		private void Start()
		{
			this.ai = base.GetComponent<AIBehaviors>();
			AIBehaviors aibehaviors = this.ai;
			aibehaviors.externalMove = (AIBehaviors.ExternalMoveDelegate)Delegate.Combine(aibehaviors.externalMove, new AIBehaviors.ExternalMoveDelegate(this.OnGotNewDestination));
			this.targetPoint = base.transform.position;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000272B File Offset: 0x0000092B
		private void OnGotNewDestination(Vector3 targetPoint, float targetSpeed, float rotationSpeed)
		{
			this.targetPoint = targetPoint;
			this.targetSpeed = targetSpeed;
			this.rotationSpeed = rotationSpeed;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000221EC File Offset: 0x000203EC
		private void Update()
		{
			Vector3 vector = this.targetPoint - base.transform.position;
			Quaternion rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(vector), this.rotationSpeed / 360f * this.rotationMultiplier * Time.deltaTime);
			base.transform.rotation = rotation;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002742 File Offset: 0x00000942
		private void FixedUpdate()
		{
			base.rigidbody.AddRelativeForce(Vector3.forward * this.forceMultiplier * this.targetSpeed * Time.fixedDeltaTime, 1);
		}

		// Token: 0x04000019 RID: 25
		public float forceMultiplier = 10f;

		// Token: 0x0400001A RID: 26
		private float rotationMultiplier = 2f;

		// Token: 0x0400001B RID: 27
		private AIBehaviors ai;

		// Token: 0x0400001C RID: 28
		private Vector3 targetPoint = Vector3.forward;

		// Token: 0x0400001D RID: 29
		private float targetSpeed;

		// Token: 0x0400001E RID: 30
		private float rotationSpeed;
	}
}
