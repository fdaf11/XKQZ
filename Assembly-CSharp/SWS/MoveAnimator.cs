using System;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006B1 RID: 1713
	public class MoveAnimator : MonoBehaviour
	{
		// Token: 0x0600295B RID: 10587 RVA: 0x00147AE4 File Offset: 0x00145CE4
		private void Start()
		{
			this.animator = base.GetComponentInChildren<Animator>();
			switch (this.mType)
			{
			case MoveAnimator.MovementType.splineMove:
				this.hMove = base.GetComponent<splineMove>();
				break;
			case MoveAnimator.MovementType.bezierMove:
				this.bMove = base.GetComponent<bezierMove>();
				break;
			case MoveAnimator.MovementType.navMove:
				this.nAgent = base.GetComponent<NavMeshAgent>();
				break;
			}
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x00147B50 File Offset: 0x00145D50
		private void OnAnimatorMove()
		{
			float num = 0f;
			float num2 = 0f;
			switch (this.mType)
			{
			case MoveAnimator.MovementType.splineMove:
				num = ((this.hMove.tween != null && !this.hMove.tween.isPaused) ? this.hMove.speed : 0f);
				num2 = (base.transform.eulerAngles.y - this.lastRotY) * 10f;
				this.lastRotY = base.transform.eulerAngles.y;
				break;
			case MoveAnimator.MovementType.bezierMove:
				num = ((this.bMove.tween != null && !this.bMove.tween.isPaused) ? this.bMove.speed : 0f);
				num2 = (base.transform.eulerAngles.y - this.lastRotY) * 10f;
				this.lastRotY = base.transform.eulerAngles.y;
				break;
			case MoveAnimator.MovementType.navMove:
			{
				num = this.nAgent.velocity.magnitude;
				Vector3 vector = Quaternion.Inverse(base.transform.rotation) * this.nAgent.desiredVelocity;
				num2 = Mathf.Atan2(vector.x, vector.z) * 180f / 3.14159f;
				break;
			}
			}
			this.animator.SetFloat("Speed", num);
			this.animator.SetFloat("Direction", num2, 0.15f, Time.deltaTime);
		}

		// Token: 0x0400345F RID: 13407
		public MoveAnimator.MovementType mType;

		// Token: 0x04003460 RID: 13408
		private splineMove hMove;

		// Token: 0x04003461 RID: 13409
		private bezierMove bMove;

		// Token: 0x04003462 RID: 13410
		private NavMeshAgent nAgent;

		// Token: 0x04003463 RID: 13411
		private Animator animator;

		// Token: 0x04003464 RID: 13412
		private float lastRotY;

		// Token: 0x020006B2 RID: 1714
		public enum MovementType
		{
			// Token: 0x04003466 RID: 13414
			splineMove,
			// Token: 0x04003467 RID: 13415
			bezierMove,
			// Token: 0x04003468 RID: 13416
			navMove
		}
	}
}
