using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem.Example
{
	// Token: 0x0200060F RID: 1551
	public class BulletExampleDynamic : MonoBehaviour
	{
		// Token: 0x06002665 RID: 9829 RVA: 0x000198D6 File Offset: 0x00017AD6
		private void NextUVRectangleIndex()
		{
			this.m_UVRectangleIndex++;
			if (this.m_UVRectangleIndex >= this.m_DecalsInstance.CurrentUvRectangles.Length)
			{
				this.m_UVRectangleIndex = 0;
			}
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x00129B64 File Offset: 0x00127D64
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

		// Token: 0x06002667 RID: 9831 RVA: 0x00129BDC File Offset: 0x00127DDC
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
				RaycastHit a_RaycastHit;
				if (Physics.Raycast(ray, ref a_RaycastHit, float.PositiveInfinity))
				{
					if (a_RaycastHit.rigidbody != null)
					{
						BulletExampleDynamicObject component = a_RaycastHit.collider.GetComponent<BulletExampleDynamicObject>();
						component.AddDecalProjector(ray, a_RaycastHit);
					}
					else
					{
						if (this.m_DecalProjectors.Count >= this.m_MaximumNumberOfProjectors)
						{
							DecalProjector decalProjector = this.m_DecalProjectors[0];
							this.m_DecalProjectors.RemoveAt(0);
							this.m_DecalsMesh.RemoveProjector(decalProjector);
						}
						Vector3 vector = a_RaycastHit.point - this.m_DecalProjectorOffset * ray.direction.normalized;
						Quaternion quaternion = ProjectorRotationUtility.ProjectorRotation(Camera.main.transform.forward, Vector3.up);
						Quaternion quaternion2 = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
						quaternion *= quaternion2;
						TerrainCollider terrainCollider = a_RaycastHit.collider as TerrainCollider;
						if (terrainCollider != null)
						{
							Terrain component2 = terrainCollider.GetComponent<Terrain>();
							if (component2 != null)
							{
								DecalProjector decalProjector2 = new DecalProjector(vector, quaternion, this.m_DecalProjectorScale, this.m_CullingAngle, this.m_MeshOffset, this.m_UVRectangleIndex, this.m_UVRectangleIndex);
								this.m_DecalProjectors.Add(decalProjector2);
								this.m_DecalsMesh.AddProjector(decalProjector2);
								Matrix4x4 matrix4x = this.m_WorldToDecalsMatrix * Matrix4x4.TRS(component2.transform.position, Quaternion.identity, Vector3.one);
								this.m_DecalsMesh.Add(component2, matrix4x);
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
							MeshCollider component3 = a_RaycastHit.collider.GetComponent<MeshCollider>();
							MeshFilter component4 = a_RaycastHit.collider.GetComponent<MeshFilter>();
							if (component3 != null || component4 != null)
							{
								Mesh mesh = null;
								if (component3 != null)
								{
									mesh = component3.sharedMesh;
								}
								else if (component4 != null)
								{
									mesh = component4.sharedMesh;
								}
								if (mesh != null)
								{
									DecalProjector decalProjector3 = new DecalProjector(vector, quaternion, this.m_DecalProjectorScale, this.m_CullingAngle, this.m_MeshOffset, this.m_UVRectangleIndex, this.m_UVRectangleIndex);
									this.m_DecalProjectors.Add(decalProjector3);
									this.m_DecalsMesh.AddProjector(decalProjector3);
									Matrix4x4 worldToLocalMatrix = a_RaycastHit.collider.renderer.transform.worldToLocalMatrix;
									Matrix4x4 localToWorldMatrix = a_RaycastHit.collider.renderer.transform.localToWorldMatrix;
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
		}

		// Token: 0x04002F36 RID: 12086
		[SerializeField]
		private DS_Decals m_DecalsPrefab;

		// Token: 0x04002F37 RID: 12087
		private DS_Decals m_DecalsInstance;

		// Token: 0x04002F38 RID: 12088
		private Matrix4x4 m_WorldToDecalsMatrix;

		// Token: 0x04002F39 RID: 12089
		private List<DecalProjector> m_DecalProjectors = new List<DecalProjector>();

		// Token: 0x04002F3A RID: 12090
		[SerializeField]
		private int m_MaximumNumberOfProjectors = 50;

		// Token: 0x04002F3B RID: 12091
		private DecalsMesh m_DecalsMesh;

		// Token: 0x04002F3C RID: 12092
		private DecalsMeshCutter m_DecalsMeshCutter;

		// Token: 0x04002F3D RID: 12093
		[SerializeField]
		private float m_DecalProjectorOffset = 0.5f;

		// Token: 0x04002F3E RID: 12094
		[SerializeField]
		private Vector3 m_DecalProjectorScale = new Vector3(0.2f, 2f, 0.2f);

		// Token: 0x04002F3F RID: 12095
		[SerializeField]
		private float m_CullingAngle = 80f;

		// Token: 0x04002F40 RID: 12096
		[SerializeField]
		private float m_MeshOffset;

		// Token: 0x04002F41 RID: 12097
		private int m_UVRectangleIndex;
	}
}
