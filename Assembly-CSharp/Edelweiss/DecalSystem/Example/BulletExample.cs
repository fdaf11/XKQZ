using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem.Example
{
	// Token: 0x0200060E RID: 1550
	public class BulletExample : MonoBehaviour
	{
		// Token: 0x06002661 RID: 9825 RVA: 0x000198A7 File Offset: 0x00017AA7
		private void NextUVRectangleIndex()
		{
			this.m_UVRectangleIndex++;
			if (this.m_UVRectangleIndex >= this.m_DecalsInstance.CurrentUvRectangles.Length)
			{
				this.m_UVRectangleIndex = 0;
			}
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x00129724 File Offset: 0x00127924
		private void Start()
		{
			this.m_DecalsInstance = (Object.Instantiate(this.m_DecalsPrefab) as DS_Decals);
			if (this.m_DecalsInstance == null)
			{
				Debug.LogError("The decals prefab does not contain a DS_Decals instance!");
			}
			else
			{
				this.m_DecalsMesh = new DecalsMesh(this.m_DecalsInstance);
				this.m_DecalsMeshCutter = new DecalsMeshCutter();
				this.m_WorldToDecalsMatrix = this.m_DecalsInstance.CachedTransform.worldToLocalMatrix;
			}
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x0012979C File Offset: 0x0012799C
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

		// Token: 0x04002F2A RID: 12074
		[SerializeField]
		private DS_Decals m_DecalsPrefab;

		// Token: 0x04002F2B RID: 12075
		private DS_Decals m_DecalsInstance;

		// Token: 0x04002F2C RID: 12076
		private Matrix4x4 m_WorldToDecalsMatrix;

		// Token: 0x04002F2D RID: 12077
		private List<DecalProjector> m_DecalProjectors = new List<DecalProjector>();

		// Token: 0x04002F2E RID: 12078
		[SerializeField]
		private int m_MaximumNumberOfProjectors = 50;

		// Token: 0x04002F2F RID: 12079
		private DecalsMesh m_DecalsMesh;

		// Token: 0x04002F30 RID: 12080
		private DecalsMeshCutter m_DecalsMeshCutter;

		// Token: 0x04002F31 RID: 12081
		[SerializeField]
		private float m_DecalProjectorOffset = 0.5f;

		// Token: 0x04002F32 RID: 12082
		[SerializeField]
		private Vector3 m_DecalProjectorScale = new Vector3(0.2f, 2f, 0.2f);

		// Token: 0x04002F33 RID: 12083
		[SerializeField]
		private float m_CullingAngle = 80f;

		// Token: 0x04002F34 RID: 12084
		[SerializeField]
		private float m_MeshOffset;

		// Token: 0x04002F35 RID: 12085
		private int m_UVRectangleIndex;
	}
}
