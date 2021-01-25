using System;
using System.Collections.Generic;
using UnityEngine;

namespace Edelweiss.DecalSystem.Example
{
	// Token: 0x02000616 RID: 1558
	public class ColoredBulletExample : MonoBehaviour
	{
		// Token: 0x06002689 RID: 9865 RVA: 0x00019A19 File Offset: 0x00017C19
		private void NextUVRectangleIndex()
		{
			this.m_UVRectangleIndex++;
			if (this.m_UVRectangleIndex >= this.m_DecalsInstance.CurrentUvRectangles.Length)
			{
				this.m_UVRectangleIndex = 0;
			}
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x00019A48 File Offset: 0x00017C48
		private void NextColorIndex()
		{
			this.m_ColorIndex++;
			if (this.m_ColorIndex > 2)
			{
				this.m_ColorIndex = 0;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x0600268B RID: 9867 RVA: 0x0012B220 File Offset: 0x00129420
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

		// Token: 0x0600268C RID: 9868 RVA: 0x0012B264 File Offset: 0x00129464
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
					this.m_DecalsMeshCutter = new DecalsMeshCutter();
					this.m_WorldToDecalsMatrix = this.m_DecalsInstance.CachedTransform.worldToLocalMatrix;
				}
			}
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x0012B308 File Offset: 0x00129508
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
							this.NextColorIndex();
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
								this.NextColorIndex();
							}
						}
					}
				}
			}
		}

		// Token: 0x04002F7C RID: 12156
		[SerializeField]
		private DS_Decals m_DecalsPrefab;

		// Token: 0x04002F7D RID: 12157
		private DS_Decals m_DecalsInstance;

		// Token: 0x04002F7E RID: 12158
		private Matrix4x4 m_WorldToDecalsMatrix;

		// Token: 0x04002F7F RID: 12159
		private List<DecalProjector> m_DecalProjectors = new List<DecalProjector>();

		// Token: 0x04002F80 RID: 12160
		[SerializeField]
		private int m_MaximumNumberOfProjectors = 50;

		// Token: 0x04002F81 RID: 12161
		private DecalsMesh m_DecalsMesh;

		// Token: 0x04002F82 RID: 12162
		private DecalsMeshCutter m_DecalsMeshCutter;

		// Token: 0x04002F83 RID: 12163
		[SerializeField]
		private float m_DecalProjectorOffset = 0.5f;

		// Token: 0x04002F84 RID: 12164
		[SerializeField]
		private Vector3 m_DecalProjectorScale = new Vector3(0.2f, 2f, 0.2f);

		// Token: 0x04002F85 RID: 12165
		[SerializeField]
		private float m_CullingAngle = 80f;

		// Token: 0x04002F86 RID: 12166
		[SerializeField]
		private float m_MeshOffset;

		// Token: 0x04002F87 RID: 12167
		private int m_UVRectangleIndex;

		// Token: 0x04002F88 RID: 12168
		private int m_ColorIndex;
	}
}
