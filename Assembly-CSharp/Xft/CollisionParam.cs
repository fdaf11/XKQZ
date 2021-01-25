using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200057A RID: 1402
	public class CollisionParam
	{
		// Token: 0x06002311 RID: 8977 RVA: 0x000173C8 File Offset: 0x000155C8
		public CollisionParam(GameObject obj, Vector3 pos, Vector3 dir)
		{
			this.m_collideObject = obj;
			this.m_collidePos = pos;
			this.m_direction = dir;
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x000173E5 File Offset: 0x000155E5
		// (set) Token: 0x06002313 RID: 8979 RVA: 0x000173ED File Offset: 0x000155ED
		public GameObject CollideObject
		{
			get
			{
				return this.m_collideObject;
			}
			set
			{
				this.m_collideObject = value;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x000173F6 File Offset: 0x000155F6
		// (set) Token: 0x06002315 RID: 8981 RVA: 0x000173FE File Offset: 0x000155FE
		public Vector3 CollidePos
		{
			get
			{
				return this.m_collidePos;
			}
			set
			{
				this.m_collidePos = value;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06002316 RID: 8982 RVA: 0x00017407 File Offset: 0x00015607
		public Vector3 CollideDir
		{
			get
			{
				return this.m_direction;
			}
		}

		// Token: 0x04002A87 RID: 10887
		protected GameObject m_collideObject;

		// Token: 0x04002A88 RID: 10888
		protected Vector3 m_collidePos;

		// Token: 0x04002A89 RID: 10889
		protected Vector3 m_direction;
	}
}
