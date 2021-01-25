using System;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000123 RID: 291
	public class BigMapNodeData
	{
		// Token: 0x060005F3 RID: 1523 RVA: 0x000056EC File Offset: 0x000038EC
		public BigMapNodeData ToData(BigMapNode node)
		{
			this.NodeID = node.NodeID;
			this.Poision = node.Pos;
			this.Offset = node.Offset;
			this.Range = node.Range;
			return this;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00043468 File Offset: 0x00041668
		public void ToGame(bool bInit = true)
		{
			GameObject gameObject = new GameObject(this.NodeID);
			BigMapNode bigMapNode = gameObject.AddComponent<BigMapNode>();
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			sphereCollider.radius = this.Range;
			sphereCollider.isTrigger = true;
			bigMapNode.Offset = this.Offset;
			bigMapNode.NodeID = this.NodeID;
			gameObject.transform.position = this.Poision;
			if (bInit)
			{
				bigMapNode.InitData();
			}
		}

		// Token: 0x0400066C RID: 1644
		public string NodeID;

		// Token: 0x0400066D RID: 1645
		public BigMapNodeData.Vector Poision;

		// Token: 0x0400066E RID: 1646
		public BigMapNodeData.Vector Offset;

		// Token: 0x0400066F RID: 1647
		public float Range;

		// Token: 0x02000124 RID: 292
		public class Vector
		{
			// Token: 0x060005F6 RID: 1526 RVA: 0x00005729 File Offset: 0x00003929
			public static implicit operator Vector3(BigMapNodeData.Vector v)
			{
				return new Vector3(v.x, v.y, v.z);
			}

			// Token: 0x060005F7 RID: 1527 RVA: 0x000434E4 File Offset: 0x000416E4
			public static implicit operator BigMapNodeData.Vector(Vector3 v)
			{
				return new BigMapNodeData.Vector
				{
					x = v.x,
					y = v.y,
					z = v.z
				};
			}

			// Token: 0x04000670 RID: 1648
			public float x;

			// Token: 0x04000671 RID: 1649
			public float y;

			// Token: 0x04000672 RID: 1650
			public float z;
		}
	}
}
