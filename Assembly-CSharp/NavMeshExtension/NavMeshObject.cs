using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NavMeshExtension
{
	// Token: 0x02000516 RID: 1302
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class NavMeshObject : MonoBehaviour
	{
		// Token: 0x06002175 RID: 8565 RVA: 0x000FBE70 File Offset: 0x000FA070
		public void Combine()
		{
			NavMeshObject.<Combine>c__AnonStoreyDE <Combine>c__AnonStoreyDE = new NavMeshObject.<Combine>c__AnonStoreyDE();
			MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
			if (componentsInChildren.Length == 1)
			{
				Debug.LogWarning("No submeshes to combine. Cancelling.");
				return;
			}
			MeshFilter meshFilter = componentsInChildren[0];
			List<CombineInstance> list = new List<CombineInstance>();
			for (int i2 = 0; i2 < componentsInChildren.Length; i2++)
			{
				if (!(componentsInChildren[i2].sharedMesh == null))
				{
					CombineInstance combineInstance = default(CombineInstance);
					combineInstance.mesh = componentsInChildren[i2].sharedMesh;
					combineInstance.transform = componentsInChildren[i2].transform.localToWorldMatrix;
					list.Add(combineInstance);
				}
			}
			string name = "NavMesh";
			if (meshFilter.sharedMesh != null)
			{
				name = meshFilter.sharedMesh.name;
			}
			meshFilter.sharedMesh = new Mesh();
			meshFilter.sharedMesh.name = name;
			meshFilter.sharedMesh.CombineMeshes(list.ToArray());
			this.current.Clear();
			List<Vector3> list2 = new List<Vector3>(meshFilter.sharedMesh.vertices);
			<Combine>c__AnonStoreyDE.triangles = new List<int>(meshFilter.sharedMesh.triangles);
			for (int j = 0; j < list2.Count; j++)
			{
				list2[j] = base.transform.InverseTransformPoint(list2[j]);
			}
			List<int> list3 = new List<int>();
			<Combine>c__AnonStoreyDE.duplicates = Enumerable.ToList<Vector3>(Enumerable.Select<IGrouping<Vector3, Vector3>, Vector3>(Enumerable.Where<IGrouping<Vector3, Vector3>>(Enumerable.GroupBy<Vector3, Vector3>(list2, (Vector3 x) => x), (IGrouping<Vector3, Vector3> x) => Enumerable.Count<Vector3>(x) > 1), (IGrouping<Vector3, Vector3> x) => x.Key));
			int i;
			for (i = 0; i < <Combine>c__AnonStoreyDE.duplicates.Count; i++)
			{
				List<int> list4 = Enumerable.ToList<int>(Enumerable.Select(Enumerable.Where(Enumerable.Select(list2, (Vector3 value, int index) => new
				{
					value,
					index
				}), a => object.Equals(a.value, <Combine>c__AnonStoreyDE.duplicates[i])), a => a.index));
				int num = list4[0];
				list4.RemoveAt(0);
				for (int k = 0; k < list4.Count; k++)
				{
					int dupIndex = list4[k];
					List<int> list5 = Enumerable.ToList<int>(Enumerable.Where<int>(Enumerable.Range(0, <Combine>c__AnonStoreyDE.triangles.Count), (int v) => <Combine>c__AnonStoreyDE.triangles[v] == dupIndex));
					for (int l = 0; l < list5.Count; l++)
					{
						<Combine>c__AnonStoreyDE.triangles[list5[l]] = num;
					}
					list3.Add(dupIndex);
				}
			}
			list3 = Enumerable.ToList<int>(Enumerable.OrderByDescending<int, int>(list3, (int x) => x));
			for (int m = 0; m < list3.Count; m++)
			{
				int num2 = list3[m];
				list2.RemoveAt(num2);
				for (int n = num2; n < <Combine>c__AnonStoreyDE.triangles.Count; n++)
				{
					if (<Combine>c__AnonStoreyDE.triangles[n] >= num2)
					{
						<Combine>c__AnonStoreyDE.triangles[n] = <Combine>c__AnonStoreyDE.triangles[n] - 1;
					}
				}
			}
			meshFilter.sharedMesh.triangles = <Combine>c__AnonStoreyDE.triangles.ToArray();
			meshFilter.sharedMesh.vertices = list2.ToArray();
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x000FC278 File Offset: 0x000FA478
		public GameObject CreateSubMesh()
		{
			this.subPoints.Add(new NavMeshObject.SubPoints());
			GameObject gameObject = new GameObject("New SubMesh");
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = Vector3.zero;
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.sharedMaterial = base.renderer.sharedMaterial;
			meshRenderer.enabled = base.renderer.enabled;
			meshFilter.mesh = (this.subMesh = new Mesh());
			this.subMesh.name = "SubMesh";
			this.current.Clear();
			return gameObject;
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000FC324 File Offset: 0x000FA524
		public void UpdateMesh(Vector3[] verts)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			for (int i = 0; i < verts.Length; i++)
			{
				verts[i] = base.transform.InverseTransformPoint(verts[i]);
			}
			component.sharedMesh.vertices = verts;
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x000FC37C File Offset: 0x000FA57C
		public void AddPoint(Vector3 point)
		{
			point += new Vector3(0f, this.yOffset, 0f);
			if (this.list.Count == 0)
			{
				base.transform.position = point;
			}
			this.list.Add(base.transform.InverseTransformPoint(point));
			int num = this.list.Count - 1;
			this.current.Add(num);
			this.subPoints[this.subPoints.Count - 1].list.Add(num);
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000167AD File Offset: 0x000149AD
		public void AddPoint(int point)
		{
			this.current.Add(point);
			this.subPoints[this.subPoints.Count - 1].list.Add(point);
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000FC418 File Offset: 0x000FA618
		public void CreateMesh()
		{
			if (this.subMesh)
			{
				this.subMesh.Clear();
			}
			MeshFilter meshFilter = null;
			MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].sharedMesh == this.subMesh)
				{
					meshFilter = componentsInChildren[i];
					break;
				}
			}
			Vector3[] array = new Vector3[this.current.Count];
			for (int j = 0; j < this.current.Count; j++)
			{
				array[j] = this.list[this.current[j]];
			}
			if (!meshFilter || array.Length < 3)
			{
				return;
			}
			Vector2[] array2 = new Vector2[array.Length];
			for (int k = 0; k < array.Length; k++)
			{
				if (k % 2 == 0)
				{
					array2[k] = new Vector2(0f, 0f);
				}
				else
				{
					array2[k] = new Vector2(1f, 1f);
				}
			}
			int[] array3 = new int[3 * (array.Length - 2) * 2];
			int num = 0;
			int num2 = 1;
			int num3 = 2;
			for (int l = 0; l < array3.Length / 2; l += 3)
			{
				array3[l] = num;
				array3[l + 1] = num2;
				array3[l + 2] = num3;
				num2++;
				num3++;
			}
			num = 0;
			num2 = array.Length - 1;
			num3 = array.Length - 2;
			for (int m = array3.Length / 2; m < array3.Length; m += 3)
			{
				array3[m] = num;
				array3[m + 1] = num2;
				array3[m + 2] = num3;
				num2--;
				num3--;
			}
			this.subMesh.vertices = array;
			this.subMesh.uv = array2;
			this.subMesh.triangles = array3;
			this.subMesh.RecalculateNormals();
			this.subMesh.RecalculateBounds();
			this.subMesh.Optimize();
			meshFilter.mesh = this.subMesh;
		}

		// Token: 0x040024A1 RID: 9377
		public bool autoSplit;

		// Token: 0x040024A2 RID: 9378
		public int splitAt = 4;

		// Token: 0x040024A3 RID: 9379
		public float yOffset = 0.015f;

		// Token: 0x040024A4 RID: 9380
		[HideInInspector]
		public List<Vector3> list = new List<Vector3>();

		// Token: 0x040024A5 RID: 9381
		[HideInInspector]
		public List<int> current = new List<int>();

		// Token: 0x040024A6 RID: 9382
		[HideInInspector]
		public List<NavMeshObject.SubPoints> subPoints = new List<NavMeshObject.SubPoints>();

		// Token: 0x040024A7 RID: 9383
		[HideInInspector]
		public Mesh subMesh;

		// Token: 0x02000517 RID: 1303
		[Serializable]
		public class SubPoints
		{
			// Token: 0x040024AE RID: 9390
			public List<int> list = new List<int>();
		}
	}
}
