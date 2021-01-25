using System;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class TileNode
{
	// Token: 0x040005BA RID: 1466
	public string strName;

	// Token: 0x040005BB RID: 1467
	public TileNode.Vector vPos;

	// Token: 0x040005BC RID: 1468
	public bool bWalkable;

	// Token: 0x040005BD RID: 1469
	public bool bInvisible;

	// Token: 0x040005BE RID: 1470
	public int iPlacementID;

	// Token: 0x0200010D RID: 269
	public class Vector
	{
		// Token: 0x0600059E RID: 1438 RVA: 0x0000551C File Offset: 0x0000371C
		public static implicit operator Vector3(TileNode.Vector v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00041778 File Offset: 0x0003F978
		public static implicit operator TileNode.Vector(Vector3 v)
		{
			return new TileNode.Vector
			{
				x = v.x,
				y = v.y,
				z = v.z
			};
		}

		// Token: 0x040005BF RID: 1471
		public float x;

		// Token: 0x040005C0 RID: 1472
		public float y;

		// Token: 0x040005C1 RID: 1473
		public float z;
	}
}
