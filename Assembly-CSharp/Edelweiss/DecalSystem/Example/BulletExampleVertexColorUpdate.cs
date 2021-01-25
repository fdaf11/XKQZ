using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem.Example
{
	// Token: 0x02000614 RID: 1556
	public class BulletExampleVertexColorUpdate : MonoBehaviour
	{
		// Token: 0x0600267C RID: 9852 RVA: 0x000199B6 File Offset: 0x00017BB6
		private void NextUVRectangleIndex()
		{
			this.m_UVRectangleIndex++;
			if (this.m_UVRectangleIndex >= this.m_DecalsInstance.CurrentUvRectangles.Length)
			{
				this.m_UVRectangleIndex = 0;
			}
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x000199E5 File Offset: 0x00017BE5
		private void NextColorIndex()
		{
			this.m_ColorIndex++;
			if (this.m_ColorIndex > 2)
			{
				this.m_ColorIndex = 0;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x0600267E RID: 9854 RVA: 0x0012ABF8 File Offset: 0x00128DF8
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

		// Token: 0x0600267F RID: 9855 RVA: 0x0012AC3C File Offset: 0x00128E3C
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
				this.m_DecalsInstance.UseVertexColors = true;
				if (this.m_DecalsInstance == null)
				{
					Debug.LogError("The decals prefab does not contain a DS_Decals instance!");
				}
				else
				{
					this.m_DecalsMesh = new DecalsMesh(this.m_DecalsInstance);
					this.m_DecalsMesh.PreserveVertexColorArrays = true;
					this.m_DecalsMeshCutter = new DecalsMeshCutter();
					this.m_WorldToDecalsMatrix = this.m_DecalsInstance.CachedTransform.worldToLocalMatrix;
					base.StartCoroutine(this.UpdateUVRectangleIndices());
				}
			}
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x0012ACF8 File Offset: 0x00128EF8
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
							DecalProjector decalProjector2 = new DecalProjector(vector, quaternion, this.m_DecalProjectorScale, this.m_CullingAngle, this.m_MeshOffset, this.m_UVRectangleIndex, this.m_UVRectangleIndex, this.CurrentColor, 0f);
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
								DecalProjector decalProjector3 = new DecalProjector(vector, quaternion, this.m_DecalProjectorScale, this.m_CullingAngle, this.m_MeshOffset, this.m_UVRectangleIndex, this.m_UVRectangleIndex, this.CurrentColor, 0f);
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

		// Token: 0x06002681 RID: 9857 RVA: 0x0012B07C File Offset: 0x0012927C
		private IEnumerator UpdateUVRectangleIndices()
		{
			for (;;)
			{
				if (this.m_DecalProjectors.Count > 0)
				{
					this.NextColorIndex();
					Color l_Color = this.CurrentColor;
					foreach (DecalProjector l_DecalProjector in this.m_DecalProjectors)
					{
						l_DecalProjector.vertexColor = l_Color;
						this.m_DecalsMesh.UpdateVertexColors(l_DecalProjector);
					}
					this.m_DecalsInstance.UpdateVertexColors(this.m_DecalsMesh);
				}
				yield return new WaitForSeconds(1f);
			}
			yield break;
		}

		// Token: 0x04002F69 RID: 12137
		[SerializeField]
		private DS_Decals m_DecalsPrefab;

		// Token: 0x04002F6A RID: 12138
		private DS_Decals m_DecalsInstance;

		// Token: 0x04002F6B RID: 12139
		private Matrix4x4 m_WorldToDecalsMatrix;

		// Token: 0x04002F6C RID: 12140
		private List<DecalProjector> m_DecalProjectors = new List<DecalProjector>();

		// Token: 0x04002F6D RID: 12141
		[SerializeField]
		private int m_MaximumNumberOfProjectors = 50;

		// Token: 0x04002F6E RID: 12142
		private DecalsMesh m_DecalsMesh;

		// Token: 0x04002F6F RID: 12143
		private DecalsMeshCutter m_DecalsMeshCutter;

		// Token: 0x04002F70 RID: 12144
		[SerializeField]
		private float m_DecalProjectorOffset = 0.5f;

		// Token: 0x04002F71 RID: 12145
		[SerializeField]
		private Vector3 m_DecalProjectorScale = new Vector3(0.2f, 2f, 0.2f);

		// Token: 0x04002F72 RID: 12146
		[SerializeField]
		private float m_CullingAngle = 80f;

		// Token: 0x04002F73 RID: 12147
		[SerializeField]
		private float m_MeshOffset;

		// Token: 0x04002F74 RID: 12148
		private int m_UVRectangleIndex;

		// Token: 0x04002F75 RID: 12149
		private int m_ColorIndex;
	}
}
