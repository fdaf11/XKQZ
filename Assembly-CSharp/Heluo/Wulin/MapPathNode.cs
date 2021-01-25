using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001C4 RID: 452
	public class MapPathNode : MonoBehaviour
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x00007B23 File Offset: 0x00005D23
		// (set) Token: 0x06000960 RID: 2400 RVA: 0x00007B2B File Offset: 0x00005D2B
		public bool IsOpen { get; set; }

		// Token: 0x06000961 RID: 2401 RVA: 0x00050E4C File Offset: 0x0004F04C
		public void LoadFromData(MapPathNodeData data)
		{
			this.m_NodeName = data.m_NodeName;
			this.m_NodeId = data.m_NodeId;
			base.gameObject.transform.localPosition = data.m_LocalPos;
			base.gameObject.transform.localEulerAngles = data.m_LocalEuler;
			base.gameObject.transform.localScale = data.m_LocalScale;
			BoxCollider boxCollider = base.gameObject.GetComponent<BoxCollider>();
			if (boxCollider == null)
			{
				boxCollider = base.gameObject.AddComponent<BoxCollider>();
				boxCollider.size = data.m_ColliderSize;
			}
			MapPathNode.nodes.Add(data.m_NodeId, this);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnDrawGizmos()
		{
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x04000946 RID: 2374
		public string m_NodeName;

		// Token: 0x04000947 RID: 2375
		public string m_NodeId;

		// Token: 0x04000948 RID: 2376
		public List<MapPathNode> m_NebrList = new List<MapPathNode>();

		// Token: 0x04000949 RID: 2377
		public static Dictionary<string, MapPathNode> nodes = new Dictionary<string, MapPathNode>();
	}
}
