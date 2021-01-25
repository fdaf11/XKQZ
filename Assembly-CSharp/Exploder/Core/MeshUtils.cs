using System;
using System.Collections.Generic;
using Exploder.Utils;
using UnityEngine;

namespace Exploder.Core
{
	// Token: 0x020000B6 RID: 182
	public static class MeshUtils
	{
		// Token: 0x060003CA RID: 970 RVA: 0x00030DF8 File Offset: 0x0002EFF8
		public static Vector3 ComputeBarycentricCoordinates(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
		{
			float num = b.x - a.x;
			float num2 = b.y - a.y;
			float num3 = b.z - a.z;
			float num4 = c.x - a.x;
			float num5 = c.y - a.y;
			float num6 = c.z - a.z;
			float num7 = p.x - a.x;
			float num8 = p.y - a.y;
			float num9 = p.z - a.z;
			float num10 = num * num + num2 * num2 + num3 * num3;
			float num11 = num * num4 + num2 * num5 + num3 * num6;
			float num12 = num4 * num4 + num5 * num5 + num6 * num6;
			float num13 = num7 * num + num8 * num2 + num9 * num3;
			float num14 = num7 * num4 + num8 * num5 + num9 * num6;
			float num15 = num10 * num12 - num11 * num11;
			float num16 = (num12 * num13 - num11 * num14) / num15;
			float num17 = (num10 * num14 - num11 * num13) / num15;
			float num18 = 1f - num16 - num17;
			return new Vector3(num18, num16, num17);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00030F30 File Offset: 0x0002F130
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = a;
			a = b;
			b = t;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00030F58 File Offset: 0x0002F158
		public static void CenterPivot(Vector3[] vertices, Vector3 centroid)
		{
			int num = vertices.Length;
			for (int i = 0; i < num; i++)
			{
				Vector3 vector = vertices[i];
				vector.x -= centroid.x;
				vector.y -= centroid.y;
				vector.z -= centroid.z;
				vertices[i] = vector;
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00030FD4 File Offset: 0x0002F1D4
		public static List<CutterMesh> IsolateMeshIslands(Mesh mesh)
		{
			int[] triangles = mesh.triangles;
			int vertexCount = mesh.vertexCount;
			int num = mesh.triangles.Length;
			Vector4[] tangents = mesh.tangents;
			Color32[] colors = mesh.colors32;
			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;
			Vector2[] uv = mesh.uv;
			bool flag = tangents != null && tangents.Length > 0;
			bool flag2 = colors != null && colors.Length > 0;
			bool flag3 = normals != null && normals.Length > 0;
			if (num <= 3)
			{
				return null;
			}
			LSHash lshash = new LSHash(0.1f, vertexCount);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = lshash.Hash(vertices[triangles[i]]);
			}
			List<HashSet<int>> list = new List<HashSet<int>>();
			List<HashSet<int>> list2 = list;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(array[0]);
			hashSet.Add(array[1]);
			hashSet.Add(array[2]);
			list2.Add(hashSet);
			List<HashSet<int>> list3 = list;
			List<List<int>> list4 = new List<List<int>>();
			List<List<int>> list5 = list4;
			List<int> list6 = new List<int>(num);
			list6.Add(0);
			list6.Add(1);
			list6.Add(2);
			list5.Add(list6);
			List<List<int>> list7 = list4;
			bool[] array2 = new bool[num];
			array2[0] = true;
			array2[1] = true;
			array2[2] = true;
			HashSet<int> hashSet2 = list3[0];
			List<int> list8 = list7[0];
			int num2 = 3;
			int num3 = -1;
			int num4 = 0;
			do
			{
				bool flag4 = false;
				for (int j = 3; j < num; j += 3)
				{
					if (!array2[j])
					{
						if (hashSet2.Contains(array[j]) || hashSet2.Contains(array[j + 1]) || hashSet2.Contains(array[j + 2]))
						{
							hashSet2.Add(array[j]);
							hashSet2.Add(array[j + 1]);
							hashSet2.Add(array[j + 2]);
							list8.Add(j);
							list8.Add(j + 1);
							list8.Add(j + 2);
							array2[j] = true;
							array2[j + 1] = true;
							array2[j + 2] = true;
							num2 += 3;
							flag4 = true;
						}
						else
						{
							num3 = j;
						}
					}
				}
				if (num2 == num)
				{
					break;
				}
				if (!flag4)
				{
					hashSet = new HashSet<int>();
					hashSet.Add(array[num3]);
					hashSet.Add(array[num3 + 1]);
					hashSet.Add(array[num3 + 2]);
					hashSet2 = hashSet;
					list6 = new List<int>(num / 2);
					list6.Add(num3);
					list6.Add(num3 + 1);
					list6.Add(num3 + 2);
					list8 = list6;
					list3.Add(hashSet2);
					list7.Add(list8);
				}
				num4++;
			}
			while (num4 <= 100);
			int count = list3.Count;
			if (count == 1)
			{
				return null;
			}
			List<CutterMesh> list9 = new List<CutterMesh>(list3.Count);
			foreach (List<int> list10 in list7)
			{
				CutterMesh cutterMesh = default(CutterMesh);
				CutterMesh cutterMesh2 = cutterMesh;
				cutterMesh2.mesh = new Mesh();
				cutterMesh = cutterMesh2;
				int count2 = list10.Count;
				Mesh mesh2 = cutterMesh.mesh;
				List<int> list11 = new List<int>(count2);
				List<Vector3> list12 = new List<Vector3>(count2);
				List<Vector3> list13 = new List<Vector3>(count2);
				List<Vector2> list14 = new List<Vector2>(count2);
				List<Color32> list15 = new List<Color32>(count2);
				List<Vector4> list16 = new List<Vector4>(count2);
				Dictionary<int, int> dictionary = new Dictionary<int, int>(num);
				Vector3 vector = Vector3.zero;
				int num5 = 0;
				int num6 = 0;
				foreach (int num7 in list10)
				{
					int num8 = triangles[num7];
					int num9 = 0;
					if (dictionary.TryGetValue(num8, ref num9))
					{
						list11.Add(num9);
					}
					else
					{
						list11.Add(num6);
						dictionary.Add(num8, num6);
						num6++;
						vector += vertices[num8];
						num5++;
						list12.Add(vertices[num8]);
						list14.Add(uv[num8]);
						if (flag3)
						{
							list13.Add(normals[num8]);
						}
						if (flag2)
						{
							list15.Add(colors[num8]);
						}
						if (flag)
						{
							list16.Add(tangents[num8]);
						}
					}
				}
				mesh2.vertices = list12.ToArray();
				mesh2.uv = list14.ToArray();
				if (flag3)
				{
					mesh2.normals = list13.ToArray();
				}
				if (flag2)
				{
					mesh2.colors32 = list15.ToArray();
				}
				if (flag)
				{
					mesh2.tangents = list16.ToArray();
				}
				mesh2.triangles = list11.ToArray();
				cutterMesh.centroid = vector / (float)num5;
				list9.Add(cutterMesh);
			}
			return list9;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00031558 File Offset: 0x0002F758
		public static void GeneratePolygonCollider(PolygonCollider2D collider, Mesh mesh)
		{
			if (mesh && collider)
			{
				Vector3[] vertices = mesh.vertices;
				Vector2[] array = new Vector2[vertices.Length];
				for (int i = 0; i < vertices.Length; i++)
				{
					array[i] = vertices[i];
				}
				Vector2[] array2 = Hull2D.ChainHull2D(array);
				collider.SetPath(0, array2);
			}
		}
	}
}
