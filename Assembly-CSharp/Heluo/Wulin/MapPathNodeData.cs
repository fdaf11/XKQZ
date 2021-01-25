using System;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001C2 RID: 450
	public class MapPathNodeData
	{
		// Token: 0x06000959 RID: 2393 RVA: 0x00050D78 File Offset: 0x0004EF78
		public void ToJson(MapPathNode mpn)
		{
			this.m_NodeName = mpn.m_NodeName;
			this.m_NodeId = mpn.m_NodeId;
			this.m_LocalPos = mpn.gameObject.transform.localPosition;
			this.m_LocalEuler = mpn.gameObject.transform.localEulerAngles;
			this.m_LocalScale = mpn.gameObject.transform.localScale;
			this.m_ColliderSize = (mpn.gameObject.collider as BoxCollider).size;
		}

		// Token: 0x0400093D RID: 2365
		public string m_NodeName;

		// Token: 0x0400093E RID: 2366
		public string m_NodeId;

		// Token: 0x0400093F RID: 2367
		public MapPathNodeData.Vector m_LocalPos;

		// Token: 0x04000940 RID: 2368
		public MapPathNodeData.Vector m_LocalEuler;

		// Token: 0x04000941 RID: 2369
		public MapPathNodeData.Vector m_LocalScale;

		// Token: 0x04000942 RID: 2370
		public MapPathNodeData.Vector m_ColliderSize;

		// Token: 0x020001C3 RID: 451
		public class Vector
		{
			// Token: 0x0600095B RID: 2395 RVA: 0x00007AEB File Offset: 0x00005CEB
			public static implicit operator Vector3(MapPathNodeData.Vector v)
			{
				return new Vector3(v.x, v.y, v.z);
			}

			// Token: 0x0600095C RID: 2396 RVA: 0x00050E10 File Offset: 0x0004F010
			public static implicit operator MapPathNodeData.Vector(Vector3 v)
			{
				return new MapPathNodeData.Vector
				{
					x = v.x,
					y = v.y,
					z = v.z
				};
			}

			// Token: 0x04000943 RID: 2371
			public float x;

			// Token: 0x04000944 RID: 2372
			public float y;

			// Token: 0x04000945 RID: 2373
			public float z;
		}
	}
}
