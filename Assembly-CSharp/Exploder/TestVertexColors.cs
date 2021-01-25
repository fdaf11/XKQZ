using System;
using UnityEngine;

namespace Exploder
{
	// Token: 0x020000C0 RID: 192
	public class TestVertexColors : MonoBehaviour
	{
		// Token: 0x06000402 RID: 1026 RVA: 0x00032934 File Offset: 0x00030B34
		private void Start()
		{
			Mesh mesh = base.GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			Color32[] array = new Color32[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				array[i] = Color.blue;
			}
			mesh.colors32 = array;
		}
	}
}
