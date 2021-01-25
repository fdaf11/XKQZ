using System;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x02000005 RID: 5
	public class CharacterMover : MonoBehaviour
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000024BD File Offset: 0x000006BD
		private void Start()
		{
			this.controller = base.gameObject.GetComponent<CharacterController>();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000024D0 File Offset: 0x000006D0
		private void Update()
		{
			this.moveDirection.y = this.moveDirection.y - this.gravity * Time.deltaTime;
			this.controller.Move(this.moveDirection * Time.deltaTime);
		}

		// Token: 0x04000003 RID: 3
		public float gravity = 20f;

		// Token: 0x04000004 RID: 4
		private Vector3 moveDirection = Vector3.zero;

		// Token: 0x04000005 RID: 5
		private CharacterController controller;
	}
}
