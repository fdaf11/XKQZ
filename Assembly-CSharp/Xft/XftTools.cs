using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x020005A7 RID: 1447
	public class XftTools
	{
		// Token: 0x06002436 RID: 9270 RVA: 0x000180E0 File Offset: 0x000162E0
		public static void TopLeftUVToLowerLeft(ref Vector2 tl, ref Vector2 dimension)
		{
			tl.y = 1f - tl.y;
			dimension.y = -dimension.y;
		}
	}
}
