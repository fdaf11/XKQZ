using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000658 RID: 1624
	public class FX_Position : MonoBehaviour
	{
		// Token: 0x060027F1 RID: 10225 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x0001A556 File Offset: 0x00018756
		private void Awake()
		{
			if (this.Normal)
			{
				this.PlaceNormal(base.transform.position);
			}
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x0013BF18 File Offset: 0x0013A118
		public void PlaceNormal(Vector3 position)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(position, -Vector3.up * 100f, ref raycastHit))
			{
				base.transform.position = raycastHit.point + this.Offset;
				base.transform.forward = raycastHit.normal;
			}
			else
			{
				base.transform.position = position + this.Offset;
			}
		}

		// Token: 0x040031EC RID: 12780
		public Vector3 Offset = new Vector3(0f, 0.001f, 0f);

		// Token: 0x040031ED RID: 12781
		public bool Normal;

		// Token: 0x040031EE RID: 12782
		public SpawnMode Mode;
	}
}
