using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000650 RID: 1616
	[RequireComponent(typeof(Rigidbody))]
	public class FX_AddForceForward : MonoBehaviour
	{
		// Token: 0x060027D6 RID: 10198 RVA: 0x0013B870 File Offset: 0x00139A70
		private void Start()
		{
			Rigidbody component = base.GetComponent<Rigidbody>();
			if (component)
			{
				component.AddForce(base.transform.forward * this.Force);
			}
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x040031D6 RID: 12758
		public float Force = 300f;
	}
}
