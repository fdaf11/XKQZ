using System;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x02000010 RID: 16
	public class ProjectileCollider : MonoBehaviour
	{
		// Token: 0x06000035 RID: 53 RVA: 0x000027DB File Offset: 0x000009DB
		private void Update()
		{
			if (this.shouldDestroy)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000027F3 File Offset: 0x000009F3
		private void OnTriggerEnter(Collider col)
		{
			if (col.tag != "Player")
			{
				this.shouldDestroy = true;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002811 File Offset: 0x00000A11
		private void OnCollisionEnter()
		{
			this.shouldDestroy = true;
		}

		// Token: 0x04000020 RID: 32
		private bool shouldDestroy;
	}
}
