using System;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001C8 RID: 456
	public class MapPathPlan : iTweenPath
	{
		// Token: 0x0600096D RID: 2413 RVA: 0x00051048 File Offset: 0x0004F248
		public void LoadFromData(MapPathPlanData data)
		{
			this.pathName = data.m_PathName;
			this.m_PathDays = data.m_PathDays;
			this.m_PathHours = data.m_PathHours;
			this.pathColor = data.m_PathColor;
			this.nodes.Clear();
			for (int i = 0; i < data.m_PathNode.Count; i++)
			{
				Vector3 vector = data.m_PathNode[i];
				this.nodes.Add(vector);
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00007B87 File Offset: 0x00005D87
		public Vector3 GetEndNode()
		{
			return this.nodes[this.nodes.Count - 1];
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00007BA1 File Offset: 0x00005DA1
		public Vector3 GetBeingNode()
		{
			return this.nodes[0];
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00007BAF File Offset: 0x00005DAF
		public void SetEndNode(Vector3 pos)
		{
			this.nodes[this.nodes.Count - 1] = pos;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00007BCA File Offset: 0x00005DCA
		public void SetBeingNode(Vector3 pos)
		{
			this.nodes[0] = pos;
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x000510D0 File Offset: 0x0004F2D0
		public Vector3[] GetPath(bool bReverse = false)
		{
			if (bReverse)
			{
				Vector3[] array = this.nodes.ToArray();
				Array.Reverse(array);
				return array;
			}
			return this.nodes.ToArray();
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnDrawGizmos()
		{
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnDrawGizmosSelected()
		{
		}

		// Token: 0x04000959 RID: 2393
		public int m_PathDays;

		// Token: 0x0400095A RID: 2394
		public int m_PathHours;

		// Token: 0x0400095B RID: 2395
		public MapPathNode m_LinkBeginNode;

		// Token: 0x0400095C RID: 2396
		public MapPathNode m_LinkEndNode;
	}
}
