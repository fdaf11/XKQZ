using System;
using UnityEngine;

// Token: 0x0200062B RID: 1579
[RequireComponent(typeof(MeshFilter))]
public class ReverseNormals : MonoBehaviour
{
	// Token: 0x06002716 RID: 10006 RVA: 0x0012EBE4 File Offset: 0x0012CDE4
	private void Start()
	{
		MeshFilter meshFilter = base.GetComponent(typeof(MeshFilter)) as MeshFilter;
		if (meshFilter != null)
		{
			Mesh mesh = meshFilter.mesh;
			Vector3[] normals = mesh.normals;
			for (int i = 0; i < normals.Length; i++)
			{
				normals[i] = -normals[i];
			}
			mesh.normals = normals;
			for (int j = 0; j < mesh.subMeshCount; j++)
			{
				int[] triangles = mesh.GetTriangles(j);
				for (int k = 0; k < triangles.Length; k += 3)
				{
					int num = triangles[k];
					triangles[k] = triangles[k + 1];
					triangles[k + 1] = num;
				}
				mesh.SetTriangles(triangles, j);
			}
		}
	}
}
