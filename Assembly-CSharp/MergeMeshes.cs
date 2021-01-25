using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000607 RID: 1543
[AddComponentMenu("Tasharen/Merge Meshes")]
public class MergeMeshes : MonoBehaviour
{
	// Token: 0x06002632 RID: 9778 RVA: 0x000196AB File Offset: 0x000178AB
	private void Start()
	{
		if (this.mMerge)
		{
			this.Merge(true);
		}
		base.enabled = false;
	}

	// Token: 0x06002633 RID: 9779 RVA: 0x000196AB File Offset: 0x000178AB
	private void Update()
	{
		if (this.mMerge)
		{
			this.Merge(true);
		}
		base.enabled = false;
	}

	// Token: 0x06002634 RID: 9780 RVA: 0x001283A8 File Offset: 0x001265A8
	public void Merge(bool immediate)
	{
		if (!immediate)
		{
			this.mMerge = true;
			base.enabled = true;
			return;
		}
		this.mMerge = false;
		this.mName = base.name;
		this.mFilter = base.GetComponent<MeshFilter>();
		this.mTrans = base.transform;
		this.Clear();
		MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
		if (componentsInChildren.Length == 0 || (this.mFilter != null && componentsInChildren.Length == 1))
		{
			return;
		}
		GameObject gameObject = base.gameObject;
		Matrix4x4 worldToLocalMatrix = gameObject.transform.worldToLocalMatrix;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		foreach (MeshFilter meshFilter in componentsInChildren)
		{
			if (!(meshFilter == this.mFilter))
			{
				if (meshFilter.gameObject.isStatic)
				{
					Debug.LogError("MergeMeshes can't merge objects marked as static", meshFilter.gameObject);
				}
				else
				{
					Mesh sharedMesh = meshFilter.sharedMesh;
					if (this.material == null)
					{
						this.material = meshFilter.renderer.sharedMaterial;
					}
					num += sharedMesh.vertexCount;
					num2 += sharedMesh.triangles.Length;
					if (sharedMesh.normals != null)
					{
						num3 += sharedMesh.normals.Length;
					}
					if (sharedMesh.tangents != null)
					{
						num4 += sharedMesh.tangents.Length;
					}
					if (sharedMesh.colors != null)
					{
						num5 += sharedMesh.colors.Length;
					}
					if (sharedMesh.uv != null)
					{
						num6 += sharedMesh.uv.Length;
					}
					if (sharedMesh.uv2 != null)
					{
						num7 += sharedMesh.uv2.Length;
					}
				}
			}
		}
		if (num == 0)
		{
			Debug.LogWarning("Unable to find any non-static objects to merge", this);
			return;
		}
		Vector3[] array2 = new Vector3[num];
		int[] array3 = new int[num2];
		Vector2[] array4 = (num6 != num) ? null : new Vector2[num];
		Vector2[] array5 = (num7 != num) ? null : new Vector2[num];
		Vector3[] array6 = (num3 != num) ? null : new Vector3[num];
		Vector4[] array7 = (num4 != num) ? null : new Vector4[num];
		Color[] array8 = (num5 != num) ? null : new Color[num];
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		foreach (MeshFilter meshFilter2 in componentsInChildren)
		{
			if (!(meshFilter2 == this.mFilter) && !meshFilter2.gameObject.isStatic)
			{
				Mesh sharedMesh2 = meshFilter2.sharedMesh;
				if (sharedMesh2.vertexCount != 0)
				{
					Matrix4x4 localToWorldMatrix = meshFilter2.transform.localToWorldMatrix;
					Renderer renderer = meshFilter2.renderer;
					if (this.afterMerging != MergeMeshes.PostMerge.DestroyRenderers)
					{
						renderer.enabled = false;
						this.mDisabledRen.Add(renderer);
					}
					if (this.afterMerging == MergeMeshes.PostMerge.DisableGameObjects)
					{
						GameObject gameObject2 = meshFilter2.gameObject;
						Transform transform = gameObject2.transform;
						while (transform != this.mTrans)
						{
							if (transform.rigidbody != null)
							{
								gameObject2 = transform.gameObject;
								break;
							}
							transform = transform.parent;
						}
						this.mDisabledGO.Add(gameObject2);
						TWTools.SetActive(gameObject2, false);
					}
					Vector3[] vertices = sharedMesh2.vertices;
					Vector3[] array10 = (array6 == null) ? null : sharedMesh2.normals;
					Vector4[] array11 = (array7 == null) ? null : sharedMesh2.tangents;
					Vector2[] array12 = (array4 == null) ? null : sharedMesh2.uv;
					Vector2[] array13 = (array5 == null) ? null : sharedMesh2.uv2;
					Color[] array14 = (array8 == null) ? null : sharedMesh2.colors;
					int[] triangles = sharedMesh2.triangles;
					int k = 0;
					int num11 = vertices.Length;
					while (k < num11)
					{
						array2[num10] = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(vertices[k]));
						if (array6 != null)
						{
							array6[num10] = worldToLocalMatrix.MultiplyVector(localToWorldMatrix.MultiplyVector(array10[k]));
						}
						if (array8 != null)
						{
							array8[num10] = array14[k];
						}
						if (array4 != null)
						{
							array4[num10] = array12[k];
						}
						if (array5 != null)
						{
							array5[num10] = array13[k];
						}
						if (array7 != null)
						{
							Vector4 vector = array11[k];
							Vector3 vector2;
							vector2..ctor(vector.x, vector.y, vector.z);
							vector2 = worldToLocalMatrix.MultiplyVector(localToWorldMatrix.MultiplyVector(vector2));
							vector.x = vector2.x;
							vector.y = vector2.y;
							vector.z = vector2.z;
							array7[num10] = vector;
						}
						num10++;
						k++;
					}
					int l = 0;
					int num12 = triangles.Length;
					while (l < num12)
					{
						array3[num9++] = num8 + triangles[l];
						l++;
					}
					num8 = num10;
					if (this.afterMerging == MergeMeshes.PostMerge.DestroyGameObjects)
					{
						Object.Destroy(meshFilter2.gameObject);
					}
					else if (this.afterMerging == MergeMeshes.PostMerge.DestroyRenderers)
					{
						Object.Destroy(renderer);
					}
				}
			}
		}
		if (this.afterMerging == MergeMeshes.PostMerge.DestroyGameObjects)
		{
			this.mDisabledGO.Clear();
		}
		if (array2.Length > 0)
		{
			if (this.mMesh == null)
			{
				this.mMesh = new Mesh();
				this.mMesh.hideFlags = 4;
			}
			else
			{
				this.mMesh.Clear();
			}
			this.mMesh.name = this.mName;
			this.mMesh.vertices = array2;
			this.mMesh.normals = array6;
			this.mMesh.tangents = array7;
			this.mMesh.colors = array8;
			this.mMesh.uv = array4;
			this.mMesh.uv2 = array5;
			this.mMesh.triangles = array3;
			this.mMesh.RecalculateBounds();
			if (this.mFilter == null)
			{
				this.mFilter = gameObject.AddComponent<MeshFilter>();
				this.mFilter.mesh = this.mMesh;
			}
			if (this.mRen == null)
			{
				this.mRen = gameObject.AddComponent<MeshRenderer>();
			}
			this.mRen.sharedMaterial = this.material;
			this.mRen.enabled = true;
			gameObject.name = string.Concat(new object[]
			{
				this.mName,
				" (",
				array3.Length / 3,
				" tri)"
			});
		}
		else
		{
			this.Release();
		}
		base.enabled = false;
	}

	// Token: 0x06002635 RID: 9781 RVA: 0x00128AEC File Offset: 0x00126CEC
	public void Clear()
	{
		int i = 0;
		int count = this.mDisabledGO.Count;
		while (i < count)
		{
			GameObject gameObject = this.mDisabledGO[i];
			if (gameObject)
			{
				TWTools.SetActive(gameObject, true);
			}
			i++;
		}
		int j = 0;
		int count2 = this.mDisabledRen.Count;
		while (j < count2)
		{
			Renderer renderer = this.mDisabledRen[j];
			if (renderer)
			{
				renderer.enabled = true;
			}
			j++;
		}
		this.mDisabledGO.Clear();
		this.mDisabledRen.Clear();
		if (this.mRen != null)
		{
			this.mRen.enabled = false;
		}
	}

	// Token: 0x06002636 RID: 9782 RVA: 0x000196C6 File Offset: 0x000178C6
	public void Release()
	{
		this.Clear();
		TWTools.Destroy(this.mRen);
		TWTools.Destroy(this.mFilter);
		TWTools.Destroy(this.mMesh);
		this.mFilter = null;
		this.mMesh = null;
		this.mRen = null;
	}

	// Token: 0x04002F01 RID: 12033
	public Material material;

	// Token: 0x04002F02 RID: 12034
	public MergeMeshes.PostMerge afterMerging;

	// Token: 0x04002F03 RID: 12035
	private string mName;

	// Token: 0x04002F04 RID: 12036
	private Transform mTrans;

	// Token: 0x04002F05 RID: 12037
	private Mesh mMesh;

	// Token: 0x04002F06 RID: 12038
	private MeshFilter mFilter;

	// Token: 0x04002F07 RID: 12039
	private MeshRenderer mRen;

	// Token: 0x04002F08 RID: 12040
	private List<GameObject> mDisabledGO = new List<GameObject>();

	// Token: 0x04002F09 RID: 12041
	private List<Renderer> mDisabledRen = new List<Renderer>();

	// Token: 0x04002F0A RID: 12042
	private bool mMerge = true;

	// Token: 0x02000608 RID: 1544
	public enum PostMerge
	{
		// Token: 0x04002F0C RID: 12044
		DisableRenderers,
		// Token: 0x04002F0D RID: 12045
		DestroyRenderers,
		// Token: 0x04002F0E RID: 12046
		DisableGameObjects,
		// Token: 0x04002F0F RID: 12047
		DestroyGameObjects
	}
}
