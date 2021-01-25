using System;
using UnityEngine;

namespace Exploder.Utils
{
	// Token: 0x020000BF RID: 191
	internal class Hull2D
	{
		// Token: 0x060003FC RID: 1020 RVA: 0x00004BDC File Offset: 0x00002DDC
		public static void Sort(Vector2[] array)
		{
			Array.Sort<Vector2>(array, delegate(Vector2 value0, Vector2 value1)
			{
				int num = value0.x.CompareTo(value1.x);
				if (num != 0)
				{
					return num;
				}
				return value0.y.CompareTo(value1.y);
			});
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00032778 File Offset: 0x00030978
		public static void DumpArray(Vector2[] array)
		{
			foreach (Vector2 vector in array)
			{
				Debug.Log("V: " + vector);
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x000327C0 File Offset: 0x000309C0
		public static Vector2[] ChainHull2D(Vector2[] Pnts)
		{
			int num = Pnts.Length;
			int num2 = 0;
			Hull2D.Sort(Pnts);
			Vector2[] array = new Vector2[2 * num];
			for (int i = 0; i < num; i++)
			{
				while (num2 >= 2 && Hull2D.Hull2DCross(ref array[num2 - 2], ref array[num2 - 1], ref Pnts[i]) <= 0f)
				{
					num2--;
				}
				array[num2++] = Pnts[i];
			}
			int j = num - 2;
			int num3 = num2 + 1;
			while (j >= 0)
			{
				while (num2 >= num3 && Hull2D.Hull2DCross(ref array[num2 - 2], ref array[num2 - 1], ref Pnts[j]) <= 0f)
				{
					num2--;
				}
				array[num2++] = Pnts[j];
				j--;
			}
			Vector2[] array2 = new Vector2[num2];
			for (int k = 0; k < num2; k++)
			{
				array2[k] = array[k];
			}
			return array2;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00004C01 File Offset: 0x00002E01
		private static float Hull2DCross(ref Vector2 O, ref Vector2 A, ref Vector2 B)
		{
			return (A.x - O.x) * (B.y - O.y) - (A.y - O.y) * (B.x - O.x);
		}
	}
}
