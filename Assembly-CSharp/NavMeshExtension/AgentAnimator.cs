using System;
using UnityEngine;

namespace NavMeshExtension
{
	// Token: 0x02000512 RID: 1298
	public class AgentAnimator : MonoBehaviour
	{
		// Token: 0x06002167 RID: 8551 RVA: 0x00016738 File Offset: 0x00014938
		private void Start()
		{
			this.animator = base.GetComponentInChildren<Animator>();
			this.nAgent = base.GetComponent<NavMeshAgent>();
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000FBB44 File Offset: 0x000F9D44
		private void OnAnimatorMove()
		{
			float magnitude = this.nAgent.velocity.magnitude;
			Vector3 vector = Quaternion.Inverse(base.transform.rotation) * this.nAgent.desiredVelocity;
			float num = Mathf.Atan2(vector.x, vector.z) * 180f / 3.14159f;
			this.animator.SetFloat("Speed", magnitude);
			this.animator.SetFloat("Direction", num, 0.15f, Time.deltaTime);
		}

		// Token: 0x04002494 RID: 9364
		private NavMeshAgent nAgent;

		// Token: 0x04002495 RID: 9365
		private Animator animator;
	}
}
