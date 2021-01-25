using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000537 RID: 1335
public class WireFrameLineRenderer : MonoBehaviour
{
	// Token: 0x060021F9 RID: 8697 RVA: 0x00102534 File Offset: 0x00100734
	public void Start()
	{
		this.LineMaterial = new Material("Shader \"Lines/Colored Blended\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Front Fog { Mode Off } } } }");
		this.LineMaterial.hideFlags = 13;
		this.LineMaterial.shader.hideFlags = 13;
		MeshFilter component = base.GetComponent<MeshFilter>();
		Mesh sharedMesh = component.sharedMesh;
		Vector3[] vertices = sharedMesh.vertices;
		int[] triangles = sharedMesh.triangles;
		for (int i = 0; i < triangles.Length / 3; i++)
		{
			int num = i * 3;
			WireFrameLineRenderer.Line l = new WireFrameLineRenderer.Line(vertices[triangles[num]], vertices[triangles[num + 1]]);
			WireFrameLineRenderer.Line l2 = new WireFrameLineRenderer.Line(vertices[triangles[num + 1]], vertices[triangles[num + 2]]);
			WireFrameLineRenderer.Line l3 = new WireFrameLineRenderer.Line(vertices[triangles[num + 2]], vertices[triangles[num]]);
			if (this.Fidelity == 3)
			{
				this.AddLine(l);
				this.AddLine(l2);
				this.AddLine(l3);
			}
			else if (this.Fidelity == 2)
			{
				this.AddLine(l);
				this.AddLine(l2);
			}
			else if (this.Fidelity == 1)
			{
				this.AddLine(l);
			}
		}
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x00102684 File Offset: 0x00100884
	public void AddLine(WireFrameLineRenderer.Line l)
	{
		bool flag = false;
		foreach (WireFrameLineRenderer.Line lB in this.LinesArray)
		{
			if (l == lB)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.LinesArray.Add(l);
		}
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x00102700 File Offset: 0x00100900
	public void OnRenderObject()
	{
		this.LineMaterial.SetPass(0);
		GL.PushMatrix();
		GL.MultMatrix(base.transform.localToWorldMatrix);
		GL.Begin(1);
		GL.Color(this.LineColor);
		foreach (WireFrameLineRenderer.Line line in this.LinesArray)
		{
			GL.Vertex(line.PointA);
			GL.Vertex(line.PointB);
		}
		GL.End();
		GL.PopMatrix();
	}

	// Token: 0x040025FA RID: 9722
	public Color LineColor;

	// Token: 0x040025FB RID: 9723
	public bool ZWrite = true;

	// Token: 0x040025FC RID: 9724
	public bool AWrite = true;

	// Token: 0x040025FD RID: 9725
	public bool Blend = true;

	// Token: 0x040025FE RID: 9726
	public int Fidelity = 3;

	// Token: 0x040025FF RID: 9727
	private Vector3[] Lines;

	// Token: 0x04002600 RID: 9728
	private List<WireFrameLineRenderer.Line> LinesArray = new List<WireFrameLineRenderer.Line>();

	// Token: 0x04002601 RID: 9729
	private Material LineMaterial;

	// Token: 0x02000538 RID: 1336
	public class Line
	{
		// Token: 0x060021FC RID: 8700 RVA: 0x00016BDE File Offset: 0x00014DDE
		public Line(Vector3 a, Vector3 b)
		{
			this.PointA = a;
			this.PointB = b;
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x001027A8 File Offset: 0x001009A8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			WireFrameLineRenderer.Line line = obj as WireFrameLineRenderer.Line;
			return line != null && ((this.PointA == line.PointA && this.PointB == line.PointB) || (this.PointA == line.PointB && this.PointB == line.PointA));
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x0010282C File Offset: 0x00100A2C
		public bool Equals(WireFrameLineRenderer.Line lB)
		{
			return (this.PointA == lB.PointA && this.PointB == lB.PointB) || (this.PointA == lB.PointB && this.PointB == lB.PointA);
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x00002C2D File Offset: 0x00000E2D
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x0010282C File Offset: 0x00100A2C
		public static bool operator ==(WireFrameLineRenderer.Line lA, WireFrameLineRenderer.Line lB)
		{
			return (lA.PointA == lB.PointA && lA.PointB == lB.PointB) || (lA.PointA == lB.PointB && lA.PointB == lB.PointA);
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x00016BF4 File Offset: 0x00014DF4
		public static bool operator !=(WireFrameLineRenderer.Line lA, WireFrameLineRenderer.Line lB)
		{
			return !(lA == lB);
		}

		// Token: 0x04002602 RID: 9730
		public Vector3 PointA;

		// Token: 0x04002603 RID: 9731
		public Vector3 PointB;
	}
}
