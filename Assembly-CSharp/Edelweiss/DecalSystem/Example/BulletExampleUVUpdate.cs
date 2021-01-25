using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem.Example
{
	// Token: 0x02000612 RID: 1554
	public class BulletExampleUVUpdate : MonoBehaviour
	{
		// Token: 0x06002671 RID: 9841 RVA: 0x00019976 File Offset: 0x00017B76
		private void NextUVRectangleIndex()
		{
			this.m_UVRectangleIndex++;
			if (this.m_UVRectangleIndex >= this.m_DecalsInstance.CurrentUvRectangles.Length)
			{
				this.m_UVRectangleIndex = 0;
			}
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x0012A60C File Offset: 0x0012880C
		private void Start()
		{
			if (Edition.IsDecalSystemFree)
			{
				Debug.Log("This demo only works with Decal System Pro.");
				base.enabled = false;
			}
			else
			{
				this.m_DecalsInstance = (Object.Instantiate(this.m_DecalsPrefab) as DS_Decals);
				if (this.m_DecalsInstance == null)
				{
					Debug.LogError("The decals prefab does not contain a DS_Decals instance!");
				}
				else
				{
					this.m_DecalsMesh = new DecalsMesh(this.m_DecalsInstance);
					this.m_DecalsMesh.PreserveProjectedUVArrays = true;
					this.m_DecalsMeshCutter = new DecalsMeshCutter();
					this.m_WorldToDecalsMatrix = this.m_DecalsInstance.CachedTransform.worldToLocalMatrix;
					base.StartCoroutine(this.UpdateUVRectangleIndices());
				}
			}
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x0012A6BC File Offset: 0x001288BC
		private void Update()
		{
			if (Input.GetKeyDown(99))
			{
				while (this.m_DecalProjectors.Count > 0)
				{
					this.m_DecalsMesh.ClearAll();
					this.m_DecalProjectors.Clear();
					this.m_DecalsMesh.Initialize(this.m_DecalsInstance);
				}
				this.m_DecalsInstance.UpdateDecalsMeshes(this.m_DecalsMesh);
			}
			if (Input.GetButtonDown("Fire1"))
			{
				Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity))
				{
					if (this.m_DecalProjectors.Count >= this.m_MaximumNumberOfProjectors)
					{
						DecalProjector decalProjector = this.m_DecalProjectors[0];
						this.m_DecalProjectors.RemoveAt(0);
						this.m_DecalsMesh.RemoveProjector(decalProjector);
					}
					Vector3 vector = raycastHit.point - this.m_DecalProjectorOffset * ray.direction.normalized;
					Quaternion quaternion = ProjectorRotationUtility.ProjectorRotation(Camera.main.transform.forward, Vector3.up);
					Quaternion quaternion2 = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
					quaternion *= quaternion2;
					TerrainCollider terrainCollider = raycastHit.collider as TerrainCollider;
					if (terrainCollider != null)
					{
						Terrain component = terrainCollider.GetComponent<Terrain>();
						if (component != null)
						{
							DecalProjector decalProjector2 = new DecalProjector(vector, quaternion, this.m_DecalProjectorScale, this.m_CullingAngle, this.m_MeshOffset, this.m_UVRectangleIndex, this.m_UVRectangleIndex);
							this.m_DecalProjectors.Add(decalProjector2);
							this.m_DecalsMesh.AddProjector(decalProjector2);
							Matrix4x4 matrix4x = this.m_WorldToDecalsMatrix * Matrix4x4.TRS(component.transform.position, Quaternion.identity, Vector3.one);
							this.m_DecalsMesh.Add(component, matrix4x);
							this.m_DecalsMeshCutter.CutDecalsPlanes(this.m_DecalsMesh);
							this.m_DecalsMesh.OffsetActiveProjectorVertices();
							this.m_DecalsInstance.UpdateDecalsMeshes(this.m_DecalsMesh);
							this.NextUVRectangleIndex();
						}
						else
						{
							Debug.LogError("Terrain is null!");
						}
					}
					else
					{
						MeshCollider component2 = raycastHit.collider.GetComponent<MeshCollider>();
						MeshFilter component3 = raycastHit.collider.GetComponent<MeshFilter>();
						if (component2 != null || component3 != null)
						{
							Mesh mesh = null;
							if (component2 != null)
							{
								mesh = component2.sharedMesh;
							}
							else if (component3 != null)
							{
								mesh = component3.sharedMesh;
							}
							if (mesh != null)
							{
								DecalProjector decalProjector3 = new DecalProjector(vector, quaternion, this.m_DecalProjectorScale, this.m_CullingAngle, this.m_MeshOffset, this.m_UVRectangleIndex, this.m_UVRectangleIndex);
								this.m_DecalProjectors.Add(decalProjector3);
								this.m_DecalsMesh.AddProjector(decalProjector3);
								Matrix4x4 worldToLocalMatrix = raycastHit.collider.renderer.transform.worldToLocalMatrix;
								Matrix4x4 localToWorldMatrix = raycastHit.collider.renderer.transform.localToWorldMatrix;
								this.m_DecalsMesh.Add(mesh, worldToLocalMatrix, localToWorldMatrix);
								this.m_DecalsMeshCutter.CutDecalsPlanes(this.m_DecalsMesh);
								this.m_DecalsMesh.OffsetActiveProjectorVertices();
								this.m_DecalsInstance.UpdateDecalsMeshes(this.m_DecalsMesh);
								this.NextUVRectangleIndex();
							}
						}
					}
				}
			}
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0012AA2C File Offset: 0x00128C2C
		private IEnumerator UpdateUVRectangleIndices()
		{
			for (;;)
			{
				if (this.m_DecalProjectors.Count > 0)
				{
					foreach (DecalProjector l_DecalProjector in this.m_DecalProjectors)
					{
						int l_UVRectangleIndex = l_DecalProjector.uv1RectangleIndex;
						l_UVRectangleIndex++;
						if (l_UVRectangleIndex >= this.m_DecalsInstance.CurrentUvRectangles.Length)
						{
							l_UVRectangleIndex = 0;
						}
						l_DecalProjector.uv1RectangleIndex = l_UVRectangleIndex;
						this.m_DecalsMesh.UpdateProjectedUV(l_DecalProjector);
					}
					this.m_DecalsInstance.UpdateProjectedUVs(this.m_DecalsMesh);
				}
				yield return new WaitForSeconds(1f);
			}
			yield break;
		}

		// Token: 0x04002F57 RID: 12119
		[SerializeField]
		private DS_Decals m_DecalsPrefab;

		// Token: 0x04002F58 RID: 12120
		private DS_Decals m_DecalsInstance;

		// Token: 0x04002F59 RID: 12121
		private Matrix4x4 m_WorldToDecalsMatrix;

		// Token: 0x04002F5A RID: 12122
		private List<DecalProjector> m_DecalProjectors = new List<DecalProjector>();

		// Token: 0x04002F5B RID: 12123
		[SerializeField]
		private int m_MaximumNumberOfProjectors = 50;

		// Token: 0x04002F5C RID: 12124
		private DecalsMesh m_DecalsMesh;

		// Token: 0x04002F5D RID: 12125
		private DecalsMeshCutter m_DecalsMeshCutter;

		// Token: 0x04002F5E RID: 12126
		[SerializeField]
		private float m_DecalProjectorOffset = 0.5f;

		// Token: 0x04002F5F RID: 12127
		[SerializeField]
		private Vector3 m_DecalProjectorScale = new Vector3(0.2f, 2f, 0.2f);

		// Token: 0x04002F60 RID: 12128
		[SerializeField]
		private float m_CullingAngle = 80f;

		// Token: 0x04002F61 RID: 12129
		[SerializeField]
		private float m_MeshOffset;

		// Token: 0x04002F62 RID: 12130
		private int m_UVRectangleIndex;
	}
}
