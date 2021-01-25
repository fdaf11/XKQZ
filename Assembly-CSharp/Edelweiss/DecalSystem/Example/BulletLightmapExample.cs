using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem.Example
{
	// Token: 0x02000611 RID: 1553
	public class BulletLightmapExample : MonoBehaviour
	{
		// Token: 0x0600266D RID: 9837 RVA: 0x00019934 File Offset: 0x00017B34
		private void NextUVRectangleIndex(DS_Decals a_DecalsInstance)
		{
			this.m_UVRectangleIndex++;
			if (this.m_UVRectangleIndex >= a_DecalsInstance.CurrentUvRectangles.Length)
			{
				this.m_UVRectangleIndex = 7;
			}
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x0001995E File Offset: 0x00017B5E
		private void Start()
		{
			this.m_DecalsMesh = new DecalsMesh();
			this.m_DecalsMeshCutter = new DecalsMeshCutter();
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x0012A2B4 File Offset: 0x001284B4
		private void Update()
		{
			if (Input.GetKeyDown(99))
			{
				foreach (DS_Decals ds_Decals in this.m_DecalsInstances)
				{
					Object.Destroy(ds_Decals.gameObject);
				}
				this.m_DecalsInstances.Clear();
			}
			if (Input.GetButtonDown("Fire1"))
			{
				Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity) && raycastHit.collider as TerrainCollider == null)
				{
					if (this.m_DecalsInstances.Count >= this.m_MaximumNumberOfDecals)
					{
						DS_Decals ds_Decals2 = this.m_DecalsInstances[0];
						Object.Destroy(ds_Decals2);
						this.m_DecalsInstances.RemoveAt(0);
					}
					DS_Decals ds_Decals3 = Object.Instantiate(this.m_DecalsPrefab) as DS_Decals;
					this.m_DecalsMesh.Initialize(ds_Decals3);
					Vector3 vector = raycastHit.point - this.m_DecalProjectorOffset * ray.direction.normalized;
					Quaternion quaternion = ProjectorRotationUtility.ProjectorRotation(Camera.main.transform.forward, Vector3.up);
					Quaternion quaternion2 = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
					quaternion *= quaternion2;
					MeshCollider component = raycastHit.collider.GetComponent<MeshCollider>();
					MeshFilter component2 = raycastHit.collider.GetComponent<MeshFilter>();
					MeshRenderer component3 = raycastHit.collider.GetComponent<MeshRenderer>();
					if (component != null || component2 != null)
					{
						Mesh mesh = null;
						if (component != null)
						{
							mesh = component.sharedMesh;
						}
						else if (component2 != null)
						{
							mesh = component2.sharedMesh;
						}
						if (mesh != null)
						{
							DecalProjector decalProjector = new DecalProjector(vector, quaternion, this.m_DecalProjectorScale, this.m_CullingAngle, this.m_MeshOffset, this.m_UVRectangleIndex, this.m_UVRectangleIndex);
							this.m_DecalsInstances.Add(ds_Decals3);
							this.m_DecalsMesh.AddProjector(decalProjector);
							Matrix4x4 worldToLocalMatrix = raycastHit.collider.renderer.transform.worldToLocalMatrix;
							Matrix4x4 localToWorldMatrix = raycastHit.collider.renderer.transform.localToWorldMatrix;
							this.m_DecalsMesh.Add(mesh, worldToLocalMatrix, localToWorldMatrix);
							this.m_DecalsMeshCutter.CutDecalsPlanes(this.m_DecalsMesh);
							this.m_DecalsMesh.OffsetActiveProjectorVertices();
							ds_Decals3.UpdateDecalsMeshes(this.m_DecalsMesh);
							ds_Decals3.DecalsMeshRenderers[0].MeshRenderer.lightmapIndex = component3.lightmapIndex;
							ds_Decals3.DecalsMeshRenderers[0].MeshRenderer.lightmapTilingOffset = component3.lightmapTilingOffset;
							this.NextUVRectangleIndex(ds_Decals3);
						}
					}
				}
			}
		}

		// Token: 0x04002F4D RID: 12109
		[SerializeField]
		private DS_Decals m_DecalsPrefab;

		// Token: 0x04002F4E RID: 12110
		private List<DS_Decals> m_DecalsInstances = new List<DS_Decals>();

		// Token: 0x04002F4F RID: 12111
		[SerializeField]
		private int m_MaximumNumberOfDecals = 50;

		// Token: 0x04002F50 RID: 12112
		private DecalsMesh m_DecalsMesh;

		// Token: 0x04002F51 RID: 12113
		private DecalsMeshCutter m_DecalsMeshCutter;

		// Token: 0x04002F52 RID: 12114
		[SerializeField]
		private float m_DecalProjectorOffset = 0.5f;

		// Token: 0x04002F53 RID: 12115
		[SerializeField]
		private Vector3 m_DecalProjectorScale = new Vector3(0.4f, 2f, 0.4f);

		// Token: 0x04002F54 RID: 12116
		[SerializeField]
		private float m_CullingAngle = 80f;

		// Token: 0x04002F55 RID: 12117
		[SerializeField]
		private float m_MeshOffset;

		// Token: 0x04002F56 RID: 12118
		private int m_UVRectangleIndex = 7;
	}
}
