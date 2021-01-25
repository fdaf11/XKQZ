using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem.Example
{
	// Token: 0x02000618 RID: 1560
	public class SkinnedBulletExample : MonoBehaviour
	{
		// Token: 0x06002695 RID: 9877 RVA: 0x00019ABD File Offset: 0x00017CBD
		private void NextUVRectangleIndex()
		{
			this.m_UVRectangleIndex++;
			if (this.m_UVRectangleIndex >= this.m_DecalsInstance.CurrentUvRectangles.Length)
			{
				this.m_UVRectangleIndex = 0;
			}
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x0012BA74 File Offset: 0x00129C74
		private void Start()
		{
			this.m_DecalsInstance = (Object.Instantiate(this.m_DecalsPrefab) as DS_SkinnedDecals);
			if (this.m_DecalsInstance == null)
			{
				Debug.LogError("The decals prefab does not contain a DS_SkinnedDecals instance!");
			}
			else
			{
				this.m_DecalsMesh = new SkinnedDecalsMesh(this.m_DecalsInstance);
				this.m_DecalsMeshCutter = new SkinnedDecalsMeshCutter();
			}
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x0012BAD4 File Offset: 0x00129CD4
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
						SkinnedDecalProjector skinnedDecalProjector = this.m_DecalProjectors[0];
						this.m_DecalProjectors.RemoveAt(0);
						this.m_DecalsMesh.RemoveProjector(skinnedDecalProjector);
					}
					Vector3 vector = raycastHit.point - this.m_DecalProjectorOffset * ray.direction.normalized;
					Quaternion quaternion = ProjectorRotationUtility.ProjectorRotation(Camera.main.transform.forward, Vector3.up);
					Quaternion quaternion2 = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
					quaternion *= quaternion2;
					SkinnedMeshRenderer component = raycastHit.collider.GetComponent<SkinnedMeshRenderer>();
					if (component != null)
					{
						Mesh sharedMesh = component.sharedMesh;
						if (sharedMesh != null)
						{
							SkinnedDecalProjector skinnedDecalProjector2 = new SkinnedDecalProjector(vector, quaternion, this.m_DecalProjectorScale, this.m_CullingAngle, this.m_MeshOffset, this.m_UVRectangleIndex, this.m_UVRectangleIndex);
							this.m_DecalProjectors.Add(skinnedDecalProjector2);
							this.m_DecalsMesh.AddProjector(skinnedDecalProjector2);
							Matrix4x4 worldToLocalMatrix = raycastHit.collider.renderer.transform.worldToLocalMatrix;
							Matrix4x4 localToWorldMatrix = raycastHit.collider.renderer.transform.localToWorldMatrix;
							this.m_DecalsMesh.Add(sharedMesh, component.bones, component.quality, worldToLocalMatrix, localToWorldMatrix);
							this.m_DecalsMeshCutter.CutDecalsPlanes(this.m_DecalsMesh);
							this.m_DecalsMesh.OffsetActiveProjectorVertices();
							this.m_DecalsInstance.UpdateDecalsMeshes(this.m_DecalsMesh);
							this.NextUVRectangleIndex();
						}
					}
				}
			}
		}

		// Token: 0x04002F95 RID: 12181
		[SerializeField]
		private DS_SkinnedDecals m_DecalsPrefab;

		// Token: 0x04002F96 RID: 12182
		private DS_SkinnedDecals m_DecalsInstance;

		// Token: 0x04002F97 RID: 12183
		private List<SkinnedDecalProjector> m_DecalProjectors = new List<SkinnedDecalProjector>();

		// Token: 0x04002F98 RID: 12184
		[SerializeField]
		private int m_MaximumNumberOfProjectors = 10;

		// Token: 0x04002F99 RID: 12185
		private SkinnedDecalsMesh m_DecalsMesh;

		// Token: 0x04002F9A RID: 12186
		private SkinnedDecalsMeshCutter m_DecalsMeshCutter;

		// Token: 0x04002F9B RID: 12187
		[SerializeField]
		private float m_DecalProjectorOffset = 0.5f;

		// Token: 0x04002F9C RID: 12188
		[SerializeField]
		private Vector3 m_DecalProjectorScale = new Vector3(0.2f, 2f, 0.2f);

		// Token: 0x04002F9D RID: 12189
		[SerializeField]
		private float m_CullingAngle = 80f;

		// Token: 0x04002F9E RID: 12190
		[SerializeField]
		private float m_MeshOffset;

		// Token: 0x04002F9F RID: 12191
		private int m_UVRectangleIndex;
	}
}
