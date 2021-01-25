using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000522 RID: 1314
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
[AddComponentMenu("Mesh/Polygon")]
public class Polygon : MonoBehaviour
{
	// Token: 0x060021AF RID: 8623 RVA: 0x00016974 File Offset: 0x00014B74
	private void Start()
	{
		this.UpdateComponents();
	}

	// Token: 0x060021B0 RID: 8624 RVA: 0x000FE028 File Offset: 0x000FC228
	public void UpdateComponents()
	{
		if (Application.isPlaying)
		{
			base.GetComponent<MeshCollider>().sharedMesh = this.GenerateMesh(this.PolygonCollider.GenerateFront, this.PolygonCollider.GenerateBack, this.PolygonCollider.GenerateSides, this.PolygonCollider.Extrude, this.PolygonCollider.Elevation, true, true, Vector2.one, Vector2.one, Vector2.one);
		}
		Mesh mesh = this.GenerateMesh(this.PolygonMesh.GenerateFront, this.PolygonMesh.GenerateBack, this.PolygonMesh.GenerateSides, this.PolygonMesh.Extrude, this.PolygonMesh.Elevation, true, true, this.FrontUVScale, this.BackUVScale, this.SideUVScale);
		base.GetComponent<MeshFilter>().mesh = mesh;
	}

	// Token: 0x060021B1 RID: 8625 RVA: 0x000FE0F8 File Offset: 0x000FC2F8
	private Mesh GenerateMesh(bool front, bool back, bool sides, float extrude, float elevate, bool useNormals, bool useUVS, Vector2 frontUVScale, Vector2 backUVScale, Vector2 sideUVScale)
	{
		Mesh mesh = new Mesh();
		if (this.Points.Count == 0)
		{
			return mesh;
		}
		int num = 0;
		int num2 = 0;
		Vector3 vector = Vector3.back * elevate;
		List<Vector3> list = new List<Vector3>();
		if (front)
		{
			foreach (Vector2 vector2 in this.Points)
			{
				Vector3 vector3 = vector2;
				list.Add(vector3 + vector);
			}
		}
		num = list.Count;
		if (back)
		{
			foreach (Vector2 vector4 in this.Points)
			{
				Vector3 vector5 = vector4;
				list.Add(vector5 + Vector3.forward * extrude + vector);
			}
		}
		num2 = list.Count;
		if (sides)
		{
			foreach (Vector2 vector6 in this.Points)
			{
				Vector3 vector7 = vector6;
				list.Add(vector7 + vector);
				list.Add(vector7 + vector);
			}
			foreach (Vector2 vector8 in this.Points)
			{
				Vector3 vector9 = vector8;
				list.Add(vector9 + Vector3.forward * extrude + vector);
				list.Add(vector9 + Vector3.forward * extrude + vector);
			}
		}
		mesh.vertices = list.ToArray();
		if (useUVS)
		{
			List<Vector2> list2 = new List<Vector2>();
			float num3 = 0f;
			Vector2 vector10 = this.Points[this.Points.Count - 1];
			foreach (Vector2 vector11 in this.Points)
			{
				num3 += (vector11 - vector10).magnitude;
				vector10 = vector11;
			}
			if (front)
			{
				for (int i = 0; i < this.Points.Count; i++)
				{
					Vector2 vector12 = this.Points[i];
					list2.Add(new Vector2(vector12.x * frontUVScale.x, vector12.y * frontUVScale.y));
				}
			}
			if (back)
			{
				for (int j = 0; j < this.Points.Count; j++)
				{
					Vector2 vector13 = this.Points[j];
					list2.Add(new Vector2(vector13.x * backUVScale.x, vector13.y * backUVScale.y));
				}
			}
			if (sides)
			{
				for (int k = 0; k < 2; k++)
				{
					Vector2 vector14 = this.Points[0];
					float num4 = 0f;
					for (int l = 0; l < this.Points.Count; l++)
					{
						Vector2 vector15 = this.Points[l];
						num4 += (vector15 - vector14).magnitude;
						list2.Add(new Vector2((float)k * extrude * sideUVScale.x, ((l != 0) ? num4 : num3) * sideUVScale.y));
						list2.Add(new Vector2((float)k * extrude * sideUVScale.x, num4 * sideUVScale.y));
						vector14 = vector15;
					}
					num4 += (this.Points[0] - vector14).magnitude;
				}
			}
			mesh.uv = list2.ToArray();
		}
		List<Vector2> list3 = new List<Vector2>();
		foreach (Vector2 vector16 in this.Points)
		{
			Vector3 vector17 = vector16;
			list3.Add(vector17);
		}
		List<int> list4 = new List<int>();
		bool flag;
		Triangulate.Process(ref list3, ref list4, out flag);
		List<int> list5 = new List<int>();
		if (front)
		{
			list5.AddRange(list4);
		}
		if (back)
		{
			for (int m = 0; m < list4.Count; m += 3)
			{
				list5.Add(list4[m + 2] + num);
				list5.Add(list4[m + 1] + num);
				list5.Add(list4[m] + num);
			}
		}
		if (sides)
		{
			int num5 = num2 + this.Points.Count * 0 + this.Points.Count * 2 - 1;
			int num6 = num2 + this.Points.Count * 2 + this.Points.Count * 2 - 1;
			for (int n = 0; n < this.Points.Count; n++)
			{
				int num7 = num2 + this.Points.Count * 0 + n * 2;
				int num8 = num2 + this.Points.Count * 2 + n * 2;
				if (flag)
				{
					list5.Add(num5);
					list5.Add(num7);
					list5.Add(num6);
					list5.Add(num7);
					list5.Add(num8);
					list5.Add(num6);
				}
				else
				{
					list5.Add(num5);
					list5.Add(num6);
					list5.Add(num7);
					list5.Add(num7);
					list5.Add(num6);
					list5.Add(num8);
				}
				num5 = num7 + 1;
				num6 = num8 + 1;
			}
		}
		mesh.triangles = list5.ToArray();
		if (useNormals)
		{
			List<Vector3> list6 = new List<Vector3>();
			if (front)
			{
				for (int num9 = 0; num9 < this.Points.Count; num9++)
				{
					list6.Add(Vector3.back);
				}
			}
			if (back)
			{
				for (int num10 = 0; num10 < this.Points.Count; num10++)
				{
					list6.Add(Vector3.forward);
				}
			}
			if (sides)
			{
				for (int num11 = 0; num11 < 2; num11++)
				{
					for (int num12 = 0; num12 < this.Points.Count; num12++)
					{
						Vector2 vector18 = this.Points[(num12 + this.Points.Count - 1) % this.Points.Count];
						Vector2 vector19 = this.Points[num12];
						Vector2 vector20 = this.Points[(num12 + 1) % this.Points.Count];
						Vector2 normalized = (vector18 - vector19).normalized;
						Vector2 normalized2 = (vector19 - vector20).normalized;
						Vector3 vector21;
						vector21..ctor(normalized.y, -normalized.x, 0f);
						Vector2 vector22 = vector21.normalized;
						Vector3 vector23;
						vector23..ctor(normalized2.y, -normalized2.x, 0f);
						Vector2 vector24 = vector23.normalized;
						Vector2 vector25 = (vector22 + vector24).normalized;
						if (flag)
						{
							vector25 *= -1f;
							vector22 *= -1f;
							vector24 *= -1f;
						}
						if (Vector2.Dot(vector22, vector24) > Mathf.Cos(0.017453292f * this.SmoothAngle))
						{
							list6.Add(vector25);
							list6.Add(vector25);
						}
						else
						{
							list6.Add(vector22);
							list6.Add(vector24);
						}
					}
				}
			}
			mesh.normals = list6.ToArray();
		}
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x04002506 RID: 9478
	public List<Vector2> Points = new List<Vector2>();

	// Token: 0x04002507 RID: 9479
	public HashSet<int> Selected = new HashSet<int>();

	// Token: 0x04002508 RID: 9480
	public float SmoothAngle = 35f;

	// Token: 0x04002509 RID: 9481
	public Vector2 FrontUVScale = new Vector2(0.25f, 0.25f);

	// Token: 0x0400250A RID: 9482
	public Vector2 BackUVScale = new Vector2(0.25f, 0.25f);

	// Token: 0x0400250B RID: 9483
	public Vector2 SideUVScale = new Vector2(0.25f, 0.25f);

	// Token: 0x0400250C RID: 9484
	public Polygon.ShapeData PolygonMesh = new Polygon.ShapeData();

	// Token: 0x0400250D RID: 9485
	public Polygon.ShapeData PolygonCollider = new Polygon.ShapeData();

	// Token: 0x0400250E RID: 9486
	public int InsertBefore;

	// Token: 0x02000523 RID: 1315
	[Serializable]
	public class ShapeData
	{
		// Token: 0x0400250F RID: 9487
		public bool GenerateFront = true;

		// Token: 0x04002510 RID: 9488
		public bool GenerateBack = true;

		// Token: 0x04002511 RID: 9489
		public bool GenerateSides = true;

		// Token: 0x04002512 RID: 9490
		public float Extrude = 1f;

		// Token: 0x04002513 RID: 9491
		public float Elevation = 0.5f;

		// Token: 0x04002514 RID: 9492
		public bool Enabled = true;
	}
}
