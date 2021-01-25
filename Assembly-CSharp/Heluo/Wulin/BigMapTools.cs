using System;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000128 RID: 296
	public static class BigMapTools
	{
		// Token: 0x0600060A RID: 1546 RVA: 0x000438B8 File Offset: 0x00041AB8
		public static float GetMapHeight(float x, float z)
		{
			Vector3 vector;
			vector..ctor(x, 1000f, z);
			float result = 40f;
			if (Terrain.activeTerrain != null)
			{
				result = Terrain.activeTerrain.SampleHeight(vector) + 0.1f;
			}
			return result;
		}
	}
}
