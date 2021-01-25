using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem.Example
{
	// Token: 0x02000610 RID: 1552
	public class BulletExampleDynamicObject : MonoBehaviour
	{
		// Token: 0x06002669 RID: 9833 RVA: 0x00019905 File Offset: 0x00017B05
		private void NextUVRectangleIndex()
		{
			this.m_UVRectangleIndex++;
			if (this.m_UVRectangleIndex >= this.m_DecalsInstance.CurrentUvRectangles.Length)
			{
				this.m_UVRectangleIndex = 0;
			}
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x00129FD4 File Offset: 0x001281D4
		private void Start()
		{
			this.m_DecalsInstance = (Object.Instantiate(this.m_DecalsPrefab) as DS_Decals);
			this.m_DecalsInstance.transform.parent = base.transform;
			this.m_DecalsInstance.transform.localPosition = Vector3.zero;
			this.m_DecalsInstance.transform.localScale = Vector3.one;
			if (this.m_DecalsInstance == null)
			{
				Debug.LogError("The decals prefab does not contain a DS_Decals instance!");
			}
			else
			{
				this.m_DecalsMesh = new DecalsMesh(this.m_DecalsInstance);
				this.m_DecalsMeshCutter = new DecalsMeshCutter();
			}
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x0012A074 File Offset: 0x00128274
		public void AddDecalProjector(Ray a_Ray, RaycastHit a_RaycastHit)
		{
			if (this.m_DecalProjectors.Count >= this.m_MaximumNumberOfProjectors)
			{
				DecalProjector decalProjector = this.m_DecalProjectors[0];
				this.m_DecalProjectors.RemoveAt(0);
				this.m_DecalsMesh.RemoveProjector(decalProjector);
			}
			Vector3 vector = a_RaycastHit.point - this.m_DecalProjectorOffset * a_Ray.direction.normalized;
			Quaternion quaternion = ProjectorRotationUtility.ProjectorRotation(Camera.main.transform.forward, Vector3.up);
			Quaternion quaternion2 = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
			quaternion *= quaternion2;
			MeshCollider component = a_RaycastHit.collider.GetComponent<MeshCollider>();
			MeshFilter component2 = a_RaycastHit.collider.GetComponent<MeshFilter>();
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
					DecalProjector decalProjector2 = new DecalProjector(vector, quaternion, this.m_DecalProjectorScale, this.m_CullingAngle, this.m_MeshOffset, this.m_UVRectangleIndex, this.m_UVRectangleIndex);
					this.m_DecalProjectors.Add(decalProjector2);
					this.m_DecalsMesh.AddProjector(decalProjector2);
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

		// Token: 0x04002F42 RID: 12098
		[SerializeField]
		private DS_Decals m_DecalsPrefab;

		// Token: 0x04002F43 RID: 12099
		private DS_Decals m_DecalsInstance;

		// Token: 0x04002F44 RID: 12100
		private List<DecalProjector> m_DecalProjectors = new List<DecalProjector>();

		// Token: 0x04002F45 RID: 12101
		[SerializeField]
		private int m_MaximumNumberOfProjectors = 50;

		// Token: 0x04002F46 RID: 12102
		private DecalsMesh m_DecalsMesh;

		// Token: 0x04002F47 RID: 12103
		private DecalsMeshCutter m_DecalsMeshCutter;

		// Token: 0x04002F48 RID: 12104
		[SerializeField]
		private float m_DecalProjectorOffset = 0.5f;

		// Token: 0x04002F49 RID: 12105
		[SerializeField]
		private Vector3 m_DecalProjectorScale = new Vector3(0.2f, 2f, 0.2f);

		// Token: 0x04002F4A RID: 12106
		[SerializeField]
		private float m_CullingAngle = 80f;

		// Token: 0x04002F4B RID: 12107
		[SerializeField]
		private float m_MeshOffset;

		// Token: 0x04002F4C RID: 12108
		private int m_UVRectangleIndex;
	}
}
