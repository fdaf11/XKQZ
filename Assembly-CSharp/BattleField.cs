using System;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class BattleField
{
	// Token: 0x060005A0 RID: 1440 RVA: 0x000417B4 File Offset: 0x0003F9B4
	public BattleField()
	{
		this.vPos = Vector3.zero;
		this.vEulerAngle = Vector3.zero;
		this.vGMPos = Vector3.zero;
		this.vGMEulerAngle = Vector3.zero;
		this.vCameraEulerAngle = Vector3.zero;
	}

	// Token: 0x040005C2 RID: 1474
	public string strMapID;

	// Token: 0x040005C3 RID: 1475
	public BattleField.Vector vPos;

	// Token: 0x040005C4 RID: 1476
	public BattleField.Vector vEulerAngle;

	// Token: 0x040005C5 RID: 1477
	public BattleField.Vector vGMPos;

	// Token: 0x040005C6 RID: 1478
	public BattleField.Vector vGMEulerAngle;

	// Token: 0x040005C7 RID: 1479
	public BattleField.Vector vCameraPos;

	// Token: 0x040005C8 RID: 1480
	public BattleField.Vector vCameraEulerAngle;

	// Token: 0x040005C9 RID: 1481
	public TileNode[] TileArray;

	// Token: 0x0200010F RID: 271
	public class Vector
	{
		// Token: 0x060005A2 RID: 1442 RVA: 0x00005535 File Offset: 0x00003735
		public static implicit operator Vector3(BattleField.Vector v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00041818 File Offset: 0x0003FA18
		public static implicit operator BattleField.Vector(Vector3 v)
		{
			return new BattleField.Vector
			{
				x = v.x,
				y = v.y,
				z = v.z
			};
		}

		// Token: 0x040005CA RID: 1482
		public float x;

		// Token: 0x040005CB RID: 1483
		public float y;

		// Token: 0x040005CC RID: 1484
		public float z;
	}
}
