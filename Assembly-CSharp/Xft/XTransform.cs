using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x020005A5 RID: 1445
	public struct XTransform
	{
		// Token: 0x0600241F RID: 9247 RVA: 0x00017FB0 File Offset: 0x000161B0
		public void Reset()
		{
			this.position = Vector3.zero;
			this.rotation = Quaternion.identity;
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x00017FC8 File Offset: 0x000161C8
		public void LookAt(Vector3 dir, Vector3 up)
		{
			this.rotation = Quaternion.LookRotation(dir, up);
		}

		// Token: 0x04002BE2 RID: 11234
		public Vector3 position;

		// Token: 0x04002BE3 RID: 11235
		public Quaternion rotation;
	}
}
