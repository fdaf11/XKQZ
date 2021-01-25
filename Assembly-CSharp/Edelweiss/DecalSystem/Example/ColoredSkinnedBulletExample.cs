using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem.Example
{
	// Token: 0x02000617 RID: 1559
	public class ColoredSkinnedBulletExample : MonoBehaviour
	{
		// Token: 0x0600268F RID: 9871 RVA: 0x00019A6B File Offset: 0x00017C6B
		private void NextUVRectangleIndex()
		{
			this.m_UVRectangleIndex++;
			if (this.m_UVRectangleIndex >= this.m_DecalsInstance.CurrentUvRectangles.Length)
			{
				this.m_UVRectangleIndex = 0;
			}
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x00019A9A File Offset: 0x00017C9A
		private void NextColorIndex()
		{
			this.m_ColorIndex++;
			if (this.m_ColorIndex > 2)
			{
				this.m_ColorIndex = 0;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06002691 RID: 9873 RVA: 0x0012B6F0 File Offset: 0x001298F0
		private Color CurrentColor
		{
			get
			{
				Color result;
				if (this.m_ColorIndex == 0)
				{
					result = Color.red;
				}
				else if (this.m_ColorIndex == 1)
				{
					result = Color.green;
				}
				else
				{
					result = Color.blue;
				}
				return result;
			}
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x0012B734 File Offset: 0x00129934
		private void Start()
		{
			if (Edition.IsDecalSystemFree)
			{
				Debug.Log("This demo only works with Decal System Pro.");
				base.enabled = false;
			}
			else
			{
				this.m_DecalsInstance = (Object.Instantiate(this.m_DecalsPrefab) as DS_SkinnedDecals);
				this.m_DecalsInstance.UseVertexColors = true;
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
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x0012B7C0 File Offset: 0x001299C0
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
							SkinnedDecalProjector skinnedDecalProjector2 = new SkinnedDecalProjector(vector, quaternion, this.m_DecalProjectorScale, this.m_CullingAngle, this.m_MeshOffset, this.m_UVRectangleIndex, this.m_UVRectangleIndex, this.CurrentColor, 0f);
							this.m_DecalProjectors.Add(skinnedDecalProjector2);
							this.m_DecalsMesh.AddProjector(skinnedDecalProjector2);
							Matrix4x4 worldToLocalMatrix = raycastHit.collider.renderer.transform.worldToLocalMatrix;
							Matrix4x4 localToWorldMatrix = raycastHit.collider.renderer.transform.localToWorldMatrix;
							this.m_DecalsMesh.Add(sharedMesh, component.bones, component.quality, worldToLocalMatrix, localToWorldMatrix);
							this.m_DecalsMeshCutter.CutDecalsPlanes(this.m_DecalsMesh);
							this.m_DecalsMesh.OffsetActiveProjectorVertices();
							this.m_DecalsInstance.UpdateDecalsMeshes(this.m_DecalsMesh);
							this.NextUVRectangleIndex();
							this.NextColorIndex();
						}
					}
				}
			}
		}

		// Token: 0x04002F89 RID: 12169
		[SerializeField]
		private DS_SkinnedDecals m_DecalsPrefab;

		// Token: 0x04002F8A RID: 12170
		private DS_SkinnedDecals m_DecalsInstance;

		// Token: 0x04002F8B RID: 12171
		private List<SkinnedDecalProjector> m_DecalProjectors = new List<SkinnedDecalProjector>();

		// Token: 0x04002F8C RID: 12172
		[SerializeField]
		private int m_MaximumNumberOfProjectors = 10;

		// Token: 0x04002F8D RID: 12173
		private SkinnedDecalsMesh m_DecalsMesh;

		// Token: 0x04002F8E RID: 12174
		private SkinnedDecalsMeshCutter m_DecalsMeshCutter;

		// Token: 0x04002F8F RID: 12175
		[SerializeField]
		private float m_DecalProjectorOffset = 0.5f;

		// Token: 0x04002F90 RID: 12176
		[SerializeField]
		private Vector3 m_DecalProjectorScale = new Vector3(0.2f, 2f, 0.2f);

		// Token: 0x04002F91 RID: 12177
		[SerializeField]
		private float m_CullingAngle = 80f;

		// Token: 0x04002F92 RID: 12178
		[SerializeField]
		private float m_MeshOffset;

		// Token: 0x04002F93 RID: 12179
		private int m_UVRectangleIndex;

		// Token: 0x04002F94 RID: 12180
		private int m_ColorIndex;
	}
}
