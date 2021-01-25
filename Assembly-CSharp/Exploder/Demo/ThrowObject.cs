using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x020000A3 RID: 163
	public class ThrowObject : MonoBehaviour
	{
		// Token: 0x06000375 RID: 885 RVA: 0x000046B5 File Offset: 0x000028B5
		private void Start()
		{
			this.destroyTimer = 10f;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000046C2 File Offset: 0x000028C2
		private void Update()
		{
			this.destroyTimer -= Time.deltaTime;
			if (this.destroyTimer < 0f)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x040002BF RID: 703
		private float destroyTimer;
	}
}
