using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x020000A4 RID: 164
	public abstract class UseObject : MonoBehaviour
	{
		// Token: 0x06000378 RID: 888 RVA: 0x0002D44C File Offset: 0x0002B64C
		public virtual void Use()
		{
			AudioSource component = base.GetComponent<AudioSource>();
			if (component && this.UseClip)
			{
				component.PlayOneShot(this.UseClip);
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000470F File Offset: 0x0000290F
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, this.UseRadius);
		}

		// Token: 0x040002C0 RID: 704
		public float UseRadius = 5f;

		// Token: 0x040002C1 RID: 705
		public string HelperText = string.Empty;

		// Token: 0x040002C2 RID: 706
		public AudioClip UseClip;
	}
}
