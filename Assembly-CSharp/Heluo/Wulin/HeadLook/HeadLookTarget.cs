using System;
using UnityEngine;

namespace Heluo.Wulin.HeadLook
{
	// Token: 0x020001AD RID: 429
	public class HeadLookTarget : MonoBehaviour
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x000076FF File Offset: 0x000058FF
		public Vector3 Origin
		{
			get
			{
				if (this.m_Transform == null)
				{
					this.m_Transform = base.transform;
				}
				return this.m_Transform.position + this.Offset;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00007734 File Offset: 0x00005934
		public Vector3 Target
		{
			get
			{
				if (this.m_Transform == null)
				{
					this.m_Transform = base.transform;
				}
				return this.m_Transform.position;
			}
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0000775E File Offset: 0x0000595E
		private void Awake()
		{
			this.m_Transform = base.transform;
			HeadLookController.AddLookTarget(this);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00007772 File Offset: 0x00005972
		private void OnDestroy()
		{
			HeadLookController.RemoveLookTarget(this);
		}

		// Token: 0x040008A0 RID: 2208
		public Vector3 Offset;

		// Token: 0x040008A1 RID: 2209
		public float Dis = 2f;

		// Token: 0x040008A2 RID: 2210
		private Transform m_Transform;
	}
}
