using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000524 RID: 1316
public class Triangulate
{
	// Token: 0x060021B5 RID: 8629 RVA: 0x000FE9A4 File Offset: 0x000FCBA4
	public static float Area(ref List<Vector2> contour)
	{
		int count = contour.Count;
		float num = 0f;
		int num2 = count - 1;
		int i = 0;
		while (i < count)
		{
			num += contour[num2].x * contour[i].y - contour[i].x * contour[num2].y;
			num2 = i++;
		}
		return num * 0.5f;
	}

	// Token: 0x060021B6 RID: 8630 RVA: 0x000FEA28 File Offset: 0x000FCC28
	public static bool InsideTriangle(float Ax, float Ay, float Bx, float By, float Cx, float Cy, float Px, float Py)
	{
		float num = Cx - Bx;
		float num2 = Cy - By;
		float num3 = Ax - Cx;
		float num4 = Ay - Cy;
		float num5 = Bx - Ax;
		float num6 = By - Ay;
		float num7 = Px - Ax;
		float num8 = Py - Ay;
		float num9 = Px - Bx;
		float num10 = Py - By;
		float num11 = Px - Cx;
		float num12 = Py - Cy;
		float num13 = num * num10 - num2 * num9;
		float num14 = num5 * num8 - num6 * num7;
		float num15 = num3 * num12 - num4 * num11;
		return num13 >= 0f && num15 >= 0f && num14 >= 0f;
	}

	// Token: 0x060021B7 RID: 8631 RVA: 0x000FEAC4 File Offset: 0x000FCCC4
	public static bool Snip(ref List<Vector2> contour, int u, int v, int w, int n, ref List<int> V)
	{
		float x = contour[V[u]].x;
		float y = contour[V[u]].y;
		float x2 = contour[V[v]].x;
		float y2 = contour[V[v]].y;
		float x3 = contour[V[w]].x;
		float y3 = contour[V[w]].y;
		if (Triangulate.EPSILON > (x2 - x) * (y3 - y) - (y2 - y) * (x3 - x))
		{
			return false;
		}
		for (int i = 0; i < n; i++)
		{
			if (i != u && i != v && i != w)
			{
				float x4 = contour[V[i]].x;
				float y4 = contour[V[i]].y;
				if (Triangulate.InsideTriangle(x, y, x2, y2, x3, y3, x4, y4))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060021B8 RID: 8632 RVA: 0x000FEC0C File Offset: 0x000FCE0C
	public static bool Process(ref List<Vector2> contour, ref List<int> result, out bool counterClockwise)
	{
		counterClockwise = false;
		int count = contour.Count;
		if (count < 3)
		{
			return false;
		}
		List<int> list = new List<int>();
		if (0f < Triangulate.Area(ref contour))
		{
			counterClockwise = true;
			for (int i = 0; i < count; i++)
			{
				list.Add(i);
			}
		}
		else
		{
			for (int j = 0; j < count; j++)
			{
				list.Add(count - 1 - j);
			}
		}
		int k = count;
		int num = 2 * k;
		int num2 = 0;
		int num3 = k - 1;
		while (k > 2)
		{
			if (0 >= num--)
			{
				return false;
			}
			int num4 = num3;
			if (k <= num4)
			{
				num4 = 0;
			}
			num3 = num4 + 1;
			if (k <= num3)
			{
				num3 = 0;
			}
			int num5 = num3 + 1;
			if (k <= num5)
			{
				num5 = 0;
			}
			if (Triangulate.Snip(ref contour, num4, num3, num5, k, ref list))
			{
				int num6 = list[num4];
				int num7 = list[num3];
				int num8 = list[num5];
				result.Add(num8);
				result.Add(num7);
				result.Add(num6);
				num2++;
				int num9 = num3;
				for (int l = num3 + 1; l < k; l++)
				{
					list[num9] = list[l];
					num9++;
				}
				k--;
				num = 2 * k;
			}
		}
		return true;
	}

	// Token: 0x04002515 RID: 9493
	private static float EPSILON = 1E-10f;
}
