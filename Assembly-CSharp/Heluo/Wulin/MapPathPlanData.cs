using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001C5 RID: 453
	public class MapPathPlanData
	{
		// Token: 0x06000965 RID: 2405 RVA: 0x00050F10 File Offset: 0x0004F110
		public void ToJson(MapPathPlan mpp)
		{
			this.m_PathName = mpp.pathName;
			this.m_PathDays = mpp.m_PathDays;
			this.m_PathHours = mpp.m_PathHours;
			this.m_LinkBeginNode = new MapPathNodeData();
			this.m_LinkBeginNode.ToJson(mpp.m_LinkBeginNode);
			this.m_LinkEndNode = new MapPathNodeData();
			this.m_LinkEndNode.ToJson(mpp.m_LinkEndNode);
			this.m_PathColor = mpp.pathColor;
			Vector3[] path = mpp.GetPath(false);
			for (int i = 0; i < path.Length; i++)
			{
				MapPathPlanData.Vector vector = path[i];
				this.m_PathNode.Add(vector);
			}
		}

		// Token: 0x0400094B RID: 2379
		public string m_PathName;

		// Token: 0x0400094C RID: 2380
		public int m_PathDays;

		// Token: 0x0400094D RID: 2381
		public int m_PathHours;

		// Token: 0x0400094E RID: 2382
		public MapPathNodeData m_LinkBeginNode;

		// Token: 0x0400094F RID: 2383
		public MapPathNodeData m_LinkEndNode;

		// Token: 0x04000950 RID: 2384
		public MapPathPlanData.float4 m_PathColor;

		// Token: 0x04000951 RID: 2385
		public List<MapPathPlanData.Vector> m_PathNode = new List<MapPathPlanData.Vector>();

		// Token: 0x020001C6 RID: 454
		public class Vector
		{
			// Token: 0x06000967 RID: 2407 RVA: 0x00007B47 File Offset: 0x00005D47
			public static implicit operator Vector3(MapPathPlanData.Vector v)
			{
				return new Vector3(v.x, v.y, v.z);
			}

			// Token: 0x06000968 RID: 2408 RVA: 0x00050FC4 File Offset: 0x0004F1C4
			public static implicit operator MapPathPlanData.Vector(Vector3 v)
			{
				return new MapPathPlanData.Vector
				{
					x = v.x,
					y = v.y,
					z = v.z
				};
			}

			// Token: 0x04000952 RID: 2386
			public float x;

			// Token: 0x04000953 RID: 2387
			public float y;

			// Token: 0x04000954 RID: 2388
			public float z;
		}

		// Token: 0x020001C7 RID: 455
		public class float4
		{
			// Token: 0x0600096A RID: 2410 RVA: 0x00007B60 File Offset: 0x00005D60
			public static implicit operator Color(MapPathPlanData.float4 v)
			{
				return new Color(v.r, v.g, v.b, v.a);
			}

			// Token: 0x0600096B RID: 2411 RVA: 0x00051000 File Offset: 0x0004F200
			public static implicit operator MapPathPlanData.float4(Color v)
			{
				return new MapPathPlanData.float4
				{
					r = v.r,
					g = v.g,
					b = v.b,
					a = v.a
				};
			}

			// Token: 0x04000955 RID: 2389
			public float r;

			// Token: 0x04000956 RID: 2390
			public float g;

			// Token: 0x04000957 RID: 2391
			public float b;

			// Token: 0x04000958 RID: 2392
			public float a;
		}
	}
}
