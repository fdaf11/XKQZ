using System;
using UnityEngine;

// Token: 0x0200076D RID: 1901
[Serializable]
public class Wall
{
	// Token: 0x06002D20 RID: 11552 RVA: 0x0015C4A4 File Offset: 0x0015A6A4
	public Wall(Transform obj, Tile t1, Tile t2, float ang)
	{
		this.wallObj = obj;
		this.tile1 = t1;
		this.tile2 = t2;
		if (ang > 360f)
		{
			ang -= 360f;
		}
		else if (ang < 0f)
		{
			ang += 360f;
		}
		this.angle = ang;
	}

	// Token: 0x06002D21 RID: 11553 RVA: 0x0015C508 File Offset: 0x0015A708
	public bool Contains(Tile t1, Tile t2)
	{
		return (t1 == this.tile1 || t1 == this.tile2) && (t2 == this.tile1 || t2 == this.tile2);
	}

	// Token: 0x06002D22 RID: 11554 RVA: 0x0001D166 File Offset: 0x0001B366
	public bool Contains(int ang)
	{
		return Mathf.Abs((float)ang - this.angle) < 2f;
	}

	// Token: 0x04003992 RID: 14738
	public Transform wallObj;

	// Token: 0x04003993 RID: 14739
	public Tile tile1;

	// Token: 0x04003994 RID: 14740
	public Tile tile2;

	// Token: 0x04003995 RID: 14741
	public float angle;
}
