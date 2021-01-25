using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
public class CameraPathSpeed : CameraPathPoint
{
	// Token: 0x17000049 RID: 73
	// (get) Token: 0x060002C7 RID: 711 RVA: 0x00004158 File Offset: 0x00002358
	// (set) Token: 0x060002C8 RID: 712 RVA: 0x00004160 File Offset: 0x00002360
	public float speed
	{
		get
		{
			return this._speed;
		}
		set
		{
			this._speed = Mathf.Max(value, 1E-07f);
		}
	}

	// Token: 0x0400021A RID: 538
	public float _speed = 1f;
}
