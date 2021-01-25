using System;
using System.Collections.Generic;
using Poly2Tri;
using UnityEngine;

namespace Exploder.Core
{
	// Token: 0x020000B9 RID: 185
	public class Polygon
	{
		// Token: 0x060003DC RID: 988 RVA: 0x00004A7F File Offset: 0x00002C7F
		public Polygon(Vector2[] pnts)
		{
			this.Points = pnts;
			this.Area = this.GetArea();
			this.holes = new List<Polygon>();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00031904 File Offset: 0x0002FB04
		public float GetArea()
		{
			this.Min.x = float.MaxValue;
			this.Min.y = float.MaxValue;
			this.Max.x = float.MinValue;
			this.Max.y = float.MinValue;
			int num = this.Points.Length;
			float num2 = 0f;
			int num3 = num - 1;
			int i = 0;
			while (i < num)
			{
				Vector2 vector = this.Points[num3];
				Vector2 vector2 = this.Points[i];
				num2 += vector.x * vector2.y - vector2.x * vector.y;
				if (vector.x < this.Min.x)
				{
					this.Min.x = vector.x;
				}
				if (vector.y < this.Min.y)
				{
					this.Min.y = vector.y;
				}
				if (vector.x > this.Max.x)
				{
					this.Max.x = vector.x;
				}
				if (vector.y > this.Max.y)
				{
					this.Max.y = vector.y;
				}
				num3 = i++;
			}
			return num2 * 0.5f;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00031A6C File Offset: 0x0002FC6C
		public bool IsPointInside(Vector3 p)
		{
			int num = this.Points.Length;
			Vector2 vector = this.Points[num - 1];
			bool flag = vector.y >= p.y;
			Vector2 vector2 = Vector2.zero;
			bool flag2 = false;
			for (int i = 0; i < num; i++)
			{
				vector2 = this.Points[i];
				bool flag3 = vector2.y >= p.y;
				if (flag != flag3 && (vector2.y - p.y) * (vector.x - vector2.x) >= (vector2.x - p.x) * (vector.y - vector2.y) == flag3)
				{
					flag2 = !flag2;
				}
				flag = flag3;
				vector = vector2;
			}
			return flag2;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00004AA5 File Offset: 0x00002CA5
		public bool IsPolygonInside(Polygon polygon)
		{
			return this.Area > polygon.Area && this.IsPointInside(polygon.Points[0]);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00004AD6 File Offset: 0x00002CD6
		public void AddHole(Polygon polygon)
		{
			this.holes.Add(polygon);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00031B54 File Offset: 0x0002FD54
		public List<int> Triangulate()
		{
			if (this.holes.Count != 0)
			{
				List<PolygonPoint> list = new List<PolygonPoint>(this.Points.Length);
				foreach (Vector2 vector in this.Points)
				{
					list.Add(new PolygonPoint((double)vector.x, (double)vector.y));
				}
				Polygon polygon = new Polygon(list);
				foreach (Polygon polygon2 in this.holes)
				{
					List<PolygonPoint> list2 = new List<PolygonPoint>(polygon2.Points.Length);
					foreach (Vector2 vector2 in polygon2.Points)
					{
						list2.Add(new PolygonPoint((double)vector2.x, (double)vector2.y));
					}
					polygon.AddHole(new Polygon(list2));
				}
				try
				{
					P2T.Triangulate(polygon);
				}
				catch (Exception ex)
				{
					return null;
				}
				int count = polygon.Triangles.Count;
				List<int> list3 = new List<int>(count * 3);
				this.Points = new Vector2[count * 3];
				int num = 0;
				this.Min.x = float.MaxValue;
				this.Min.y = float.MaxValue;
				this.Max.x = float.MinValue;
				this.Max.y = float.MinValue;
				for (int k = 0; k < count; k++)
				{
					list3.Add(num);
					list3.Add(num + 1);
					list3.Add(num + 2);
					this.Points[num + 2].x = (float)polygon.Triangles[k].Points._0.X;
					this.Points[num + 2].y = (float)polygon.Triangles[k].Points._0.Y;
					this.Points[num + 1].x = (float)polygon.Triangles[k].Points._1.X;
					this.Points[num + 1].y = (float)polygon.Triangles[k].Points._1.Y;
					this.Points[num].x = (float)polygon.Triangles[k].Points._2.X;
					this.Points[num].y = (float)polygon.Triangles[k].Points._2.Y;
					for (int l = 0; l < 3; l++)
					{
						if (this.Points[num + l].x < this.Min.x)
						{
							this.Min.x = this.Points[num + l].x;
						}
						if (this.Points[num + l].y < this.Min.y)
						{
							this.Min.y = this.Points[num + l].y;
						}
						if (this.Points[num + l].x > this.Max.x)
						{
							this.Max.x = this.Points[num + l].x;
						}
						if (this.Points[num + l].y > this.Max.y)
						{
							this.Max.y = this.Points[num + l].y;
						}
					}
					num += 3;
				}
				return list3;
			}
			List<int> list4 = new List<int>(this.Points.Length);
			int num2 = this.Points.Length;
			if (num2 < 3)
			{
				return list4;
			}
			int[] array = new int[num2];
			if (this.Area > 0f)
			{
				for (int m = 0; m < num2; m++)
				{
					array[m] = m;
				}
			}
			else
			{
				for (int n = 0; n < num2; n++)
				{
					array[n] = num2 - 1 - n;
				}
			}
			int num3 = num2;
			int num4 = 2 * num3;
			int num5 = 0;
			int num6 = num3 - 1;
			while (num3 > 2)
			{
				if (num4-- <= 0)
				{
					return list4;
				}
				int num7 = num6;
				if (num3 <= num7)
				{
					num7 = 0;
				}
				num6 = num7 + 1;
				if (num3 <= num6)
				{
					num6 = 0;
				}
				int num8 = num6 + 1;
				if (num3 <= num8)
				{
					num8 = 0;
				}
				if (this.Snip(num7, num6, num8, num3, array))
				{
					int num9 = array[num7];
					int num10 = array[num6];
					int num11 = array[num8];
					list4.Add(num9);
					list4.Add(num10);
					list4.Add(num11);
					num5++;
					int num12 = num6;
					for (int num13 = num6 + 1; num13 < num3; num13++)
					{
						array[num12] = array[num13];
						num12++;
					}
					num3--;
					num4 = 2 * num3;
				}
			}
			list4.Reverse();
			return list4;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00032108 File Offset: 0x00030308
		private bool Snip(int u, int v, int w, int n, int[] V)
		{
			Vector2 a = this.Points[V[u]];
			Vector2 b = this.Points[V[v]];
			Vector2 c = this.Points[V[w]];
			if (1E-45f > (b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x))
			{
				return false;
			}
			for (int i = 0; i < n; i++)
			{
				if (i != u && i != v && i != w)
				{
					Vector2 p = this.Points[V[i]];
					if (this.InsideTriangle(a, b, c, p))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000321F8 File Offset: 0x000303F8
		private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
		{
			float num = C.x - B.x;
			float num2 = C.y - B.y;
			float num3 = A.x - C.x;
			float num4 = A.y - C.y;
			float num5 = B.x - A.x;
			float num6 = B.y - A.y;
			float num7 = P.x - A.x;
			float num8 = P.y - A.y;
			float num9 = P.x - B.x;
			float num10 = P.y - B.y;
			float num11 = P.x - C.x;
			float num12 = P.y - C.y;
			float num13 = num * num10 - num2 * num9;
			float num14 = num5 * num8 - num6 * num7;
			float num15 = num3 * num12 - num4 * num11;
			return num13 >= 0f && num15 >= 0f && num14 >= 0f;
		}

		// Token: 0x0400030F RID: 783
		public Vector2[] Points;

		// Token: 0x04000310 RID: 784
		public readonly float Area;

		// Token: 0x04000311 RID: 785
		public Vector2 Min;

		// Token: 0x04000312 RID: 786
		public Vector2 Max;

		// Token: 0x04000313 RID: 787
		private readonly List<Polygon> holes;
	}
}
