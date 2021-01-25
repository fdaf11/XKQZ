using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000691 RID: 1681
[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class ProFlareBatch : MonoBehaviour
{
	// Token: 0x060028CF RID: 10447 RVA: 0x00142408 File Offset: 0x00140608
	private void Reset()
	{
		if (this.helperTransform == null)
		{
			this.CreateHelperTransform();
		}
		this.mat = new Material(Shader.Find("ProFlares/Textured Flare Shader"));
		if (this.meshFilter == null)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
		}
		if (this.meshFilter == null)
		{
			this.meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		this.meshRender = base.gameObject.GetComponent<MeshRenderer>();
		if (this.meshRender == null)
		{
			this.meshRender = base.gameObject.AddComponent<MeshRenderer>();
		}
		if (this.FlareCamera == null)
		{
			this.FlareCamera = base.transform.root.GetComponentInChildren<Camera>();
		}
		this.meshRender.material = this.mat;
		this.SetupMeshes();
		this.dirty = true;
	}

	// Token: 0x060028D0 RID: 10448 RVA: 0x001424F8 File Offset: 0x001406F8
	private void Awake()
	{
		this.PI_Div180 = 0.017453292f;
		this.Div180_PI = 57.295776f;
		ProFlare[] array = Object.FindObjectsOfType(typeof(ProFlare)) as ProFlare[];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i]._Atlas == this._atlas)
			{
				this.AddFlare(array[i]);
			}
		}
	}

	// Token: 0x060028D1 RID: 10449 RVA: 0x00142568 File Offset: 0x00140768
	private void Start()
	{
		if (Application.isPlaying)
		{
			this.overdrawDebug = false;
		}
		if (this.GameCamera == null)
		{
			GameObject gameObject = GameObject.FindWithTag("MainCamera");
			if (gameObject && gameObject.camera)
			{
				this.GameCamera = gameObject.camera;
			}
		}
		if (this.GameCamera)
		{
			this.GameCameraTrans = this.GameCamera.transform;
		}
		this.thisTransform = base.transform;
		this.SetupMeshes();
	}

	// Token: 0x060028D2 RID: 10450 RVA: 0x001425FC File Offset: 0x001407FC
	private void CreateMat()
	{
		this.mat = new Material(Shader.Find("ProFlares/Textured Flare Shader"));
		this.meshRender.material = this.mat;
		if (this._atlas && this._atlas.texture)
		{
			this.mat.mainTexture = this._atlas.texture;
		}
	}

	// Token: 0x060028D3 RID: 10451 RVA: 0x0014266C File Offset: 0x0014086C
	public void SwitchCamera(Camera newCamera)
	{
		this.GameCamera = newCamera;
		this.GameCameraTrans = newCamera.transform;
		this.FixedUpdate();
		for (int i = 0; i < this.FlaresList.Count; i++)
		{
			if (this.FlaresList[i].occlusion != null && this.FlaresList[i].occlusion.occluded)
			{
				this.FlaresList[i].occlusion.occlusionScale = 0f;
			}
		}
	}

	// Token: 0x060028D4 RID: 10452 RVA: 0x0000264F File Offset: 0x0000084F
	private void OnDestroy()
	{
	}

	// Token: 0x060028D5 RID: 10453 RVA: 0x001426FC File Offset: 0x001408FC
	public void RemoveFlare(ProFlare _flare)
	{
		bool flag = false;
		FlareData flareData = null;
		for (int i = 0; i < this.FlaresList.Count; i++)
		{
			if (_flare == this.FlaresList[i].flare)
			{
				flareData = this.FlaresList[i];
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.FlaresList.Remove(flareData);
		}
	}

	// Token: 0x060028D6 RID: 10454 RVA: 0x0014276C File Offset: 0x0014096C
	public void AddFlare(ProFlare _flare)
	{
		bool flag = false;
		for (int i = 0; i < this.FlaresList.Count; i++)
		{
			if (_flare == this.FlaresList[i].flare)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			FlareData flareData = new FlareData();
			flareData.flare = _flare;
			FlareOcclusion flareOcclusion = new FlareOcclusion();
			if (_flare.neverCull)
			{
				flareOcclusion._CullingState = FlareOcclusion.CullingState.NeverCull;
			}
			flareData.occlusion = flareOcclusion;
			this.FlaresList.Add(flareData);
			this.dirty = true;
		}
	}

	// Token: 0x060028D7 RID: 10455 RVA: 0x00142800 File Offset: 0x00140A00
	private void CreateHelperTransform()
	{
		GameObject gameObject = new GameObject("_HelperTransform");
		this.helperTransform = gameObject.transform;
		this.helperTransform.parent = base.transform;
		this.helperTransform.localScale = Vector3.one;
		this.helperTransform.localPosition = Vector3.zero;
	}

	// Token: 0x060028D8 RID: 10456 RVA: 0x00142858 File Offset: 0x00140A58
	private void Update()
	{
		if (this.thisTransform)
		{
			this.thisTransform.localPosition = Vector3.forward * this.zPos;
		}
		if (this.helperTransform == null)
		{
			this.CreateHelperTransform();
		}
		if (this.meshRender)
		{
			if (this.meshRender.sharedMaterial == null)
			{
				this.CreateMat();
			}
		}
		else
		{
			this.meshRender = base.gameObject.GetComponent<MeshRenderer>();
		}
		bool flag = false;
		if (this.meshA == null)
		{
			flag = true;
		}
		if (this.meshB == null)
		{
			flag = true;
		}
		if (flag && this._atlas != null)
		{
			this.SetupMeshes();
		}
		if (this.dirty)
		{
			this.ReBuildGeometry();
		}
	}

	// Token: 0x060028D9 RID: 10457 RVA: 0x0001AE3F File Offset: 0x0001903F
	private void LateUpdate()
	{
		if (this._atlas == null)
		{
			return;
		}
		this.UpdateFlares();
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x00142940 File Offset: 0x00140B40
	public void UpdateFlares()
	{
		this.bufferMesh = ((!this.PingPong) ? this.meshB : this.meshA);
		this.PingPong = !this.PingPong;
		this.UpdateGeometry();
		this.bufferMesh.vertices = this.vertices;
		this.bufferMesh.colors32 = this.colors;
		this.meshFilter.sharedMesh = this.bufferMesh;
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x001429B8 File Offset: 0x00140BB8
	public void ForceRefresh()
	{
		this.FlaresList.Clear();
		ProFlare[] array = Object.FindObjectsOfType(typeof(ProFlare)) as ProFlare[];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i]._Atlas == this._atlas)
			{
				this.AddFlare(array[i]);
			}
		}
		this.dirty = true;
	}

	// Token: 0x060028DC RID: 10460 RVA: 0x00142A24 File Offset: 0x00140C24
	private void ReBuildGeometry()
	{
		this.FlareElements.Clear();
		int num = 0;
		bool flag = false;
		for (int i = 0; i < this.FlaresList.Count; i++)
		{
			if (this.FlaresList[i].flare == null)
			{
				flag = true;
				break;
			}
			for (int j = 0; j < this.FlaresList[i].flare.Elements.Count; j++)
			{
				if (this.FlaresList[i].occlusion._CullingState == FlareOcclusion.CullingState.CanCull)
				{
					this.FlaresList[i].occlusion._CullingState = FlareOcclusion.CullingState.Culled;
					this.FlaresList[i].occlusion.cullFader = 0f;
				}
				if (this.FlaresList[i].occlusion._CullingState != FlareOcclusion.CullingState.Culled)
				{
					num++;
				}
			}
		}
		this.FlareElementsArray = new ProFlareElement[num];
		num = 0;
		if (!flag)
		{
			for (int k = 0; k < this.FlaresList.Count; k++)
			{
				for (int l = 0; l < this.FlaresList[k].flare.Elements.Count; l++)
				{
					if (this.FlaresList[k].occlusion._CullingState != FlareOcclusion.CullingState.Culled)
					{
						this.FlareElementsArray[num] = this.FlaresList[k].flare.Elements[l];
						num++;
					}
				}
			}
		}
		if (flag)
		{
			this.ForceRefresh();
			this.ReBuildGeometry();
		}
		this.meshA = null;
		this.meshB = null;
		this.bufferMesh = null;
		this.SetupMeshes();
		this.dirty = false;
	}

	// Token: 0x060028DD RID: 10461 RVA: 0x00142BFC File Offset: 0x00140DFC
	private void SetupMeshes()
	{
		if (this._atlas == null)
		{
			return;
		}
		if (this.FlareElementsArray == null)
		{
			return;
		}
		this.meshA = new Mesh();
		this.meshB = new Mesh();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < this.FlareElementsArray.Length; i++)
		{
			ProFlareElement.Type type = this.FlareElementsArray[i].type;
			if (type != ProFlareElement.Type.Single)
			{
				if (type == ProFlareElement.Type.Multi)
				{
					int count = this.FlareElementsArray[i].subElements.Count;
					num += 4 * count;
					num2 += 4 * count;
					num3 += 4 * count;
					num4 += 6 * count;
				}
			}
			else
			{
				num += 4;
				num2 += 4;
				num3 += 4;
				num4 += 6;
			}
		}
		this.vertices = new Vector3[num];
		this.uv = new Vector2[num2];
		this.colors = new Color32[num3];
		this.triangles = new int[num4];
		for (int j = 0; j < this.vertices.Length / 4; j++)
		{
			int num5 = j * 4;
			this.vertices[0 + num5] = new Vector3(1f, 1f, 0f);
			this.vertices[1 + num5] = new Vector3(1f, -1f, 0f);
			this.vertices[2 + num5] = new Vector3(-1f, 1f, 0f);
			this.vertices[3 + num5] = new Vector3(-1f, -1f, 0f);
		}
		int num6 = 0;
		for (int k = 0; k < this.FlareElementsArray.Length; k++)
		{
			ProFlareElement.Type type = this.FlareElementsArray[k].type;
			if (type != ProFlareElement.Type.Single)
			{
				if (type == ProFlareElement.Type.Multi)
				{
					for (int l = 0; l < this.FlareElementsArray[k].subElements.Count; l++)
					{
						int num7 = (num6 + l) * 4;
						Rect rect = this._atlas.elementsList[this.FlareElementsArray[k].elementTextureID].UV;
						this.uv[0 + num7] = new Vector2(rect.xMax, rect.yMax);
						this.uv[1 + num7] = new Vector2(rect.xMax, rect.yMin);
						this.uv[2 + num7] = new Vector2(rect.xMin, rect.yMax);
						this.uv[3 + num7] = new Vector2(rect.xMin, rect.yMin);
					}
					num6 += this.FlareElementsArray[k].subElements.Count;
				}
			}
			else
			{
				int num8 = num6 * 4;
				Rect rect2 = this._atlas.elementsList[this.FlareElementsArray[k].elementTextureID].UV;
				this.uv[0 + num8] = new Vector2(rect2.xMax, rect2.yMax);
				this.uv[1 + num8] = new Vector2(rect2.xMax, rect2.yMin);
				this.uv[2 + num8] = new Vector2(rect2.xMin, rect2.yMax);
				this.uv[3 + num8] = new Vector2(rect2.xMin, rect2.yMin);
				num6++;
			}
		}
		Color32 color;
		color..ctor(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		for (int m = 0; m < this.colors.Length / 4; m++)
		{
			int num9 = m * 4;
			this.colors[0 + num9] = color;
			this.colors[1 + num9] = color;
			this.colors[2 + num9] = color;
			this.colors[3 + num9] = color;
		}
		for (int n = 0; n < this.triangles.Length / 6; n++)
		{
			int num10 = n * 4;
			int num11 = n * 6;
			this.triangles[0 + num11] = 0 + num10;
			this.triangles[1 + num11] = 1 + num10;
			this.triangles[2 + num11] = 2 + num10;
			this.triangles[3 + num11] = 2 + num10;
			this.triangles[4 + num11] = 1 + num10;
			this.triangles[5 + num11] = 3 + num10;
		}
		this.meshA.vertices = this.vertices;
		this.meshA.uv = this.uv;
		this.meshA.triangles = this.triangles;
		this.meshA.colors32 = this.colors;
		this.meshA.bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
		this.meshB.vertices = this.vertices;
		this.meshB.uv = this.uv;
		this.meshB.triangles = this.triangles;
		this.meshB.colors32 = this.colors;
		this.meshB.bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
	}

	// Token: 0x060028DE RID: 10462 RVA: 0x001431D8 File Offset: 0x001413D8
	private void FixedUpdate()
	{
		if (!this.dirty)
		{
			for (int i = 0; i < this.FlaresList.Count; i++)
			{
				if (!(this.FlaresList[i].flare == null))
				{
					ProFlare flare = this.FlaresList[i].flare;
					FlareOcclusion occlusion = this.FlaresList[i].occlusion;
					if (flare.RaycastPhysics)
					{
						Vector3 vector = this.GameCameraTrans.position - flare.thisTransform.position;
						float num = Vector3.Distance(this.GameCameraTrans.position, flare.thisTransform.position);
						occlusion.occluded = true;
						if (flare.isVisible)
						{
							Ray ray;
							ray..ctor(flare.thisTransform.position, vector);
							RaycastHit raycastHit;
							if (Physics.Raycast(ray, ref raycastHit, num, flare.mask))
							{
								occlusion.occluded = true;
							}
							else
							{
								occlusion.occluded = false;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060028DF RID: 10463 RVA: 0x001432EC File Offset: 0x001414EC
	private void UpdateGeometry()
	{
		if (this.GameCamera == null)
		{
			this.meshRender.enabled = false;
			return;
		}
		this.meshRender.enabled = true;
		this.visibleFlares = 0;
		int num = 0;
		for (int i = 0; i < this.FlaresList.Count; i++)
		{
			ProFlare flare = this.FlaresList[i].flare;
			FlareOcclusion occlusion = this.FlaresList[i].occlusion;
			if (!(flare == null))
			{
				flare.LensPosition = this.GameCamera.WorldToViewportPoint(flare.thisTransform.position);
				Vector3 vector = flare.LensPosition;
				bool flag = vector.z > 0f && vector.x + flare.OffScreenFadeDist > 0f && vector.x - flare.OffScreenFadeDist < 1f && vector.y + flare.OffScreenFadeDist > 0f && vector.y - flare.OffScreenFadeDist < 1f;
				flare.isVisible = flag;
				float num2 = 1f;
				if (vector.x <= 0f || vector.x >= 1f || vector.y <= 0f || vector.y >= 1f)
				{
					float num3 = 1f / flare.OffScreenFadeDist;
					float num4 = 0f;
					float num5 = 0f;
					if (vector.x <= 0f || vector.x >= 1f)
					{
						num4 = ((vector.x <= 0.5f) ? Mathf.Abs(vector.x) : (vector.x - 1f));
					}
					if (vector.y <= 0f || vector.y >= 1f)
					{
						num5 = ((vector.y <= 0.5f) ? Mathf.Abs(vector.y) : (vector.y - 1f));
					}
					num2 = Mathf.Clamp01(num2 - Mathf.Max(num4, num5) * num3);
				}
				float num6 = 0f;
				if (vector.x > 0.5f - flare.DynamicCenterRange && vector.x < 0.5f + flare.DynamicCenterRange && vector.y > 0.5f - flare.DynamicCenterRange && vector.y < 0.5f + flare.DynamicCenterRange && flare.DynamicCenterRange > 0f)
				{
					float num7 = 1f / flare.DynamicCenterRange;
					num6 = 1f - Mathf.Max(Mathf.Abs(vector.x - 0.5f), Mathf.Abs(vector.y - 0.5f)) * num7;
				}
				float num8 = 0f;
				bool flag2 = vector.x > 0f + flare.DynamicEdgeBias + flare.DynamicEdgeRange && vector.x < 1f - flare.DynamicEdgeBias - flare.DynamicEdgeRange && vector.y > 0f + flare.DynamicEdgeBias + flare.DynamicEdgeRange && vector.y < 1f - flare.DynamicEdgeBias - flare.DynamicEdgeRange;
				bool flag3 = vector.x + flare.DynamicEdgeRange > 0f + flare.DynamicEdgeBias && vector.x - flare.DynamicEdgeRange < 1f - flare.DynamicEdgeBias && vector.y + flare.DynamicEdgeRange > 0f + flare.DynamicEdgeBias && vector.y - flare.DynamicEdgeRange < 1f - flare.DynamicEdgeBias;
				if (!flag2 && flag3)
				{
					float num9 = 1f / flare.DynamicEdgeRange;
					float num10 = 0f;
					float num11 = 0f;
					bool flag4 = vector.x > 0f + flare.DynamicEdgeBias + flare.DynamicEdgeRange && vector.x < 1f - flare.DynamicEdgeBias - flare.DynamicEdgeRange;
					bool flag5 = vector.x + flare.DynamicEdgeRange > 0f + flare.DynamicEdgeBias && vector.x - flare.DynamicEdgeRange < 1f - flare.DynamicEdgeBias;
					bool flag6 = vector.y > 0f + flare.DynamicEdgeBias + flare.DynamicEdgeRange && vector.y < 1f - flare.DynamicEdgeBias - flare.DynamicEdgeRange;
					bool flag7 = vector.y + flare.DynamicEdgeRange > 0f + flare.DynamicEdgeBias && vector.y - flare.DynamicEdgeRange < 1f - flare.DynamicEdgeBias;
					if (!flag4 && flag5)
					{
						num10 = ((vector.x <= 0.5f) ? Mathf.Abs(vector.x - flare.DynamicEdgeBias - flare.DynamicEdgeRange) : (vector.x - 1f + flare.DynamicEdgeBias + flare.DynamicEdgeRange));
						num10 = num10 * num9 * 0.5f;
					}
					if (!flag6 && flag7)
					{
						num11 = ((vector.y <= 0.5f) ? Mathf.Abs(vector.y - flare.DynamicEdgeBias - flare.DynamicEdgeRange) : (vector.y - 1f + flare.DynamicEdgeBias + flare.DynamicEdgeRange));
						num11 = num11 * num9 * 0.5f;
					}
					num8 = Mathf.Max(num10, num11);
				}
				num8 = flare.DynamicEdgeCurve.Evaluate(num8);
				float num12 = 1f;
				if (flare.UseAngleLimit)
				{
					Vector3 forward = flare.thisTransform.forward;
					Vector3 vector2 = this.GameCameraTrans.position - flare.thisTransform.position;
					float num13 = Vector3.Angle(forward, vector2);
					num13 = Mathf.Abs(Mathf.Clamp(num13, -flare.maxAngle, flare.maxAngle));
					if (num13 > flare.maxAngle)
					{
						num12 = 0f;
					}
					else
					{
						num12 = 1f - num13 * (1f / (flare.maxAngle * 0.5f));
						if (flare.UseAngleCurve)
						{
							num12 = flare.AngleCurve.Evaluate(num12);
						}
					}
				}
				float num14 = 1f;
				if (flare.useMaxDistance)
				{
					Vector3 vector3 = flare.thisTransform.position - this.GameCameraTrans.position;
					float num15 = Vector3.Dot(vector3, this.GameCameraTrans.forward);
					float num16 = 1f - num15 / flare.GlobalMaxDistance;
					num14 = 1f * num16;
					if (num14 < 0.001f)
					{
						flare.isVisible = false;
					}
				}
				if (!this.dirty && occlusion != null)
				{
					if (occlusion.occluded)
					{
						occlusion.occlusionScale = Mathf.Lerp(occlusion.occlusionScale, 0f, Time.deltaTime * 16f);
					}
					else
					{
						occlusion.occlusionScale = Mathf.Lerp(occlusion.occlusionScale, 1f, Time.deltaTime * 16f);
					}
				}
				if (!flare.isVisible)
				{
					num2 = 0f;
				}
				float num17 = 1f;
				if (this.FlareCamera)
				{
					this.helperTransform.position = this.FlareCamera.ViewportToWorldPoint(vector);
				}
				vector = this.helperTransform.localPosition;
				if (!this.VR_Mode && !this.SingleCamera_Mode)
				{
					vector.z = 0f;
				}
				for (int j = 0; j < flare.Elements.Count; j++)
				{
					ProFlareElement proFlareElement = flare.Elements[j];
					Color globalTintColor = flare.GlobalTintColor;
					if (flag)
					{
						ProFlareElement.Type type = proFlareElement.type;
						if (type != ProFlareElement.Type.Single)
						{
							if (type == ProFlareElement.Type.Multi)
							{
								for (int k = 0; k < proFlareElement.subElements.Count; k++)
								{
									SubElement subElement = proFlareElement.subElements[k];
									subElement.colorFinal.r = subElement.color.r * globalTintColor.r;
									subElement.colorFinal.g = subElement.color.g * globalTintColor.g;
									subElement.colorFinal.b = subElement.color.b * globalTintColor.b;
									float num18 = subElement.color.a * globalTintColor.a;
									if (flare.useDynamicEdgeBoost)
									{
										if (proFlareElement.OverrideDynamicEdgeBrightness)
										{
											num18 += proFlareElement.DynamicEdgeBrightnessOverride * num8;
										}
										else
										{
											num18 += flare.DynamicEdgeBrightness * num8;
										}
									}
									if (flare.useDynamicCenterBoost)
									{
										if (proFlareElement.OverrideDynamicCenterBrightness)
										{
											num18 += proFlareElement.DynamicCenterBrightnessOverride * num6;
										}
										else
										{
											num18 += flare.DynamicCenterBrightness * num6;
										}
									}
									if (flare.UseAngleBrightness)
									{
										num18 *= num12;
									}
									if (flare.useDistanceFade)
									{
										num18 *= num14;
									}
									num18 *= occlusion.occlusionScale;
									num18 *= occlusion.cullFader;
									num18 *= num2;
									subElement.colorFinal.a = num18;
								}
							}
						}
						else
						{
							proFlareElement.ElementFinalColor.r = proFlareElement.ElementTint.r * globalTintColor.r;
							proFlareElement.ElementFinalColor.g = proFlareElement.ElementTint.g * globalTintColor.g;
							proFlareElement.ElementFinalColor.b = proFlareElement.ElementTint.b * globalTintColor.b;
							float num18 = proFlareElement.ElementTint.a * globalTintColor.a;
							if (flare.useDynamicEdgeBoost)
							{
								if (proFlareElement.OverrideDynamicEdgeBrightness)
								{
									num18 += proFlareElement.DynamicEdgeBrightnessOverride * num8;
								}
								else
								{
									num18 += flare.DynamicEdgeBrightness * num8;
								}
							}
							if (flare.useDynamicCenterBoost)
							{
								if (proFlareElement.OverrideDynamicCenterBrightness)
								{
									num18 += proFlareElement.DynamicCenterBrightnessOverride * num6;
								}
								else
								{
									num18 += flare.DynamicCenterBrightness * num6;
								}
							}
							if (flare.UseAngleBrightness)
							{
								num18 *= num12;
							}
							if (flare.useDistanceFade)
							{
								num18 *= num14;
							}
							num18 *= occlusion.occlusionScale;
							num18 *= occlusion.cullFader;
							num18 *= num2;
							proFlareElement.ElementFinalColor.a = num18;
						}
					}
					else
					{
						ProFlareElement.Type type = proFlareElement.type;
						if (type != ProFlareElement.Type.Single)
						{
							if (type == ProFlareElement.Type.Multi)
							{
								num17 = 0f;
							}
						}
						else
						{
							num17 = 0f;
						}
					}
					float num19 = num17;
					if (flare.useDynamicEdgeBoost)
					{
						if (proFlareElement.OverrideDynamicEdgeBoost)
						{
							num19 += num8 * proFlareElement.DynamicEdgeBoostOverride;
						}
						else
						{
							num19 += num8 * flare.DynamicEdgeBoost;
						}
					}
					if (flare.useDynamicCenterBoost)
					{
						if (proFlareElement.OverrideDynamicCenterBoost)
						{
							num19 += proFlareElement.DynamicCenterBoostOverride * num6;
						}
						else
						{
							num19 += flare.DynamicCenterBoost * num6;
						}
					}
					if (num19 < 0f)
					{
						num19 = 0f;
					}
					if (flare.UseAngleScale)
					{
						num19 *= num12;
					}
					if (flare.useDistanceScale)
					{
						num19 *= num14;
					}
					num19 *= occlusion.occlusionScale;
					if (!proFlareElement.Visible)
					{
						num19 = 0f;
					}
					if (!flag)
					{
						num19 = 0f;
					}
					proFlareElement.ScaleFinal = num19;
					if (flag)
					{
						ProFlareElement.Type type = proFlareElement.type;
						if (type != ProFlareElement.Type.Single)
						{
							if (type == ProFlareElement.Type.Multi)
							{
								for (int l = 0; l < proFlareElement.subElements.Count; l++)
								{
									SubElement subElement2 = proFlareElement.subElements[l];
									if (proFlareElement.useRangeOffset)
									{
										Vector3 vector4 = vector * -subElement2.position;
										float num20 = vector.z;
										if (this.VR_Mode)
										{
											float num21 = subElement2.position * -1f - 1f;
											num20 = vector.z * (num21 * this.VR_Depth + 1f);
										}
										Vector3 vector5;
										vector5..ctor(Mathf.Lerp(vector4.x, vector.x, proFlareElement.Anamorphic.x), Mathf.Lerp(vector4.y, vector.y, proFlareElement.Anamorphic.y), num20);
										vector5 += proFlareElement.OffsetPostion;
										subElement2.offset = vector5;
									}
									else
									{
										subElement2.offset = vector * -proFlareElement.position;
									}
								}
							}
						}
						else
						{
							Vector3 vector6 = vector * -proFlareElement.position;
							float num22 = vector.z;
							if (this.VR_Mode)
							{
								float num23 = proFlareElement.position * -1f - 1f;
								num22 = vector.z * (num23 * this.VR_Depth + 1f);
							}
							Vector3 vector7;
							vector7..ctor(Mathf.Lerp(vector6.x, vector.x, proFlareElement.Anamorphic.x), Mathf.Lerp(vector6.y, vector.y, proFlareElement.Anamorphic.y), num22);
							vector7 += proFlareElement.OffsetPostion;
							proFlareElement.OffsetPosition = vector7;
						}
					}
					float num24 = 0f;
					if (proFlareElement.rotateToFlare)
					{
						num24 = this.Div180_PI * Mathf.Atan2(vector.y, vector.x);
					}
					num24 += vector.x * proFlareElement.rotationSpeed;
					num24 += vector.y * proFlareElement.rotationSpeed;
					num24 += Time.time * proFlareElement.rotationOverTime;
					proFlareElement.FinalAngle = proFlareElement.angle + num24;
				}
				if (!flare.neverCull && this.useCulling)
				{
					FlareOcclusion.CullingState cullingState = occlusion._CullingState;
					if (flare.isVisible)
					{
						this.visibleFlares++;
						if (occlusion.occluded)
						{
							if (cullingState == FlareOcclusion.CullingState.Visible)
							{
								occlusion.CullTimer = (float)this.cullFlaresAfterTime;
								cullingState = FlareOcclusion.CullingState.CullCountDown;
							}
						}
						else
						{
							if (cullingState == FlareOcclusion.CullingState.Culled)
							{
								this.culledFlaresNowVisiable = true;
							}
							cullingState = FlareOcclusion.CullingState.Visible;
						}
					}
					else if (cullingState == FlareOcclusion.CullingState.Visible)
					{
						occlusion.CullTimer = (float)this.cullFlaresAfterTime;
						cullingState = FlareOcclusion.CullingState.CullCountDown;
					}
					FlareOcclusion.CullingState cullingState2 = cullingState;
					if (cullingState2 != FlareOcclusion.CullingState.Visible)
					{
						if (cullingState2 == FlareOcclusion.CullingState.CullCountDown)
						{
							occlusion.CullTimer -= Time.deltaTime;
							if (occlusion.CullTimer < 0f)
							{
								cullingState = FlareOcclusion.CullingState.CanCull;
							}
						}
					}
					if (cullingState != FlareOcclusion.CullingState.Culled)
					{
						occlusion.cullFader = Mathf.Clamp01(occlusion.cullFader + Time.deltaTime);
					}
					if (cullingState == FlareOcclusion.CullingState.CanCull)
					{
						num++;
					}
					occlusion._CullingState = cullingState;
				}
				this.reshowCulledFlaresTimer += Time.deltaTime;
				if (this.reshowCulledFlaresTimer > this.reshowCulledFlaresAfter)
				{
					this.reshowCulledFlaresTimer = 0f;
					if (this.culledFlaresNowVisiable)
					{
						this.dirty = true;
						this.culledFlaresNowVisiable = false;
					}
				}
				if (!this.dirty && num >= this.cullFlaresAfterCount)
				{
					Debug.Log("Culling Flares");
					this.dirty = true;
				}
			}
		}
		int num25 = 0;
		if (this.FlareElementsArray != null)
		{
			for (int m = 0; m < this.FlareElementsArray.Length; m++)
			{
				float num26 = 1f;
				ProFlare flare2 = this.FlareElementsArray[m].flare;
				if (flare2.MultiplyScaleByTransformScale)
				{
					num26 = flare2.thisTransform.localScale.x;
				}
				ProFlareElement.Type type = this.FlareElementsArray[m].type;
				if (type != ProFlareElement.Type.Single)
				{
					if (type == ProFlareElement.Type.Multi)
					{
						for (int n = 0; n < this.FlareElementsArray[m].subElements.Count; n++)
						{
							int num27 = (num25 + n) * 4;
							if (this.FlareElementsArray[m].flare.DisabledPlayMode)
							{
								this.vertices[0 + num27] = Vector3.zero;
								this.vertices[1 + num27] = Vector3.zero;
								this.vertices[2 + num27] = Vector3.zero;
								this.vertices[3 + num27] = Vector3.zero;
							}
							else
							{
								this._scale = this.FlareElementsArray[m].size * this.FlareElementsArray[m].Scale * 0.01f * this.FlareElementsArray[m].flare.GlobalScale * this.FlareElementsArray[m].subElements[n].scale * this.FlareElementsArray[m].ScaleFinal;
								this._scale *= num26;
								if (this._scale.x < 0f || this._scale.y < 0f)
								{
									this._scale = Vector3.zero;
								}
								Vector3 offset = this.FlareElementsArray[m].subElements[n].offset;
								float num28 = this.FlareElementsArray[m].FinalAngle;
								num28 += this.FlareElementsArray[m].subElements[n].angle;
								this._color = this.FlareElementsArray[m].subElements[n].colorFinal;
								if (this.useBrightnessThreshold)
								{
									if ((int)this._color.a < this.BrightnessThreshold)
									{
										this._scale = Vector2.zero;
									}
									else if ((int)(this._color.r + this._color.g + this._color.b) < this.BrightnessThreshold)
									{
										this._scale = Vector2.zero;
									}
								}
								if (this.overdrawDebug)
								{
									this._color = new Color32(20, 20, 20, 100);
								}
								if (!this.FlareElementsArray[m].flare.DisabledPlayMode)
								{
									float num29 = num28 * this.PI_Div180;
									float num30 = Mathf.Cos(num29);
									float num31 = Mathf.Sin(num29);
									this.vertices[0 + num27] = new Vector3(num30 * (1f * this._scale.x) - num31 * (1f * this._scale.y), num31 * (1f * this._scale.x) + num30 * (1f * this._scale.y), 0f) + offset;
									this.vertices[1 + num27] = new Vector3(num30 * (1f * this._scale.x) - num31 * (-1f * this._scale.y), num31 * (1f * this._scale.x) + num30 * (-1f * this._scale.y), 0f) + offset;
									this.vertices[2 + num27] = new Vector3(num30 * (-1f * this._scale.x) - num31 * (1f * this._scale.y), num31 * (-1f * this._scale.x) + num30 * (1f * this._scale.y), 0f) + offset;
									this.vertices[3 + num27] = new Vector3(num30 * (-1f * this._scale.x) - num31 * (-1f * this._scale.y), num31 * (-1f * this._scale.x) + num30 * (-1f * this._scale.y), 0f) + offset;
								}
								Color32 color = this._color;
								this.colors[0 + num27] = color;
								this.colors[1 + num27] = color;
								this.colors[2 + num27] = color;
								this.colors[3 + num27] = color;
							}
						}
						num25 += this.FlareElementsArray[m].subElements.Count;
					}
				}
				else
				{
					int num32 = num25 * 4;
					if (this.FlareElementsArray[m].flare.DisabledPlayMode)
					{
						this.vertices[0 + num32] = Vector3.zero;
						this.vertices[1 + num32] = Vector3.zero;
						this.vertices[2 + num32] = Vector3.zero;
						this.vertices[3 + num32] = Vector3.zero;
					}
					this._scale = this.FlareElementsArray[m].size * this.FlareElementsArray[m].Scale * 0.01f * flare2.GlobalScale * this.FlareElementsArray[m].ScaleFinal * num26;
					if (this._scale.x < 0f || this._scale.y < 0f)
					{
						this._scale = Vector3.zero;
					}
					Vector3 offsetPosition = this.FlareElementsArray[m].OffsetPosition;
					float finalAngle = this.FlareElementsArray[m].FinalAngle;
					this._color = this.FlareElementsArray[m].ElementFinalColor;
					if (this.useBrightnessThreshold)
					{
						if ((int)this._color.a < this.BrightnessThreshold)
						{
							this._scale = Vector2.zero;
						}
						else if ((int)(this._color.r + this._color.g + this._color.b) < this.BrightnessThreshold)
						{
							this._scale = Vector2.zero;
						}
					}
					if (this.overdrawDebug)
					{
						this._color = new Color32(20, 20, 20, 100);
					}
					if (!this.FlareElementsArray[m].flare.DisabledPlayMode)
					{
						float num33 = finalAngle * this.PI_Div180;
						float num34 = Mathf.Cos(num33);
						float num35 = Mathf.Sin(num33);
						this.vertices[0 + num32] = new Vector3(num34 * (1f * this._scale.x) - num35 * (1f * this._scale.y), num35 * (1f * this._scale.x) + num34 * (1f * this._scale.y), 0f) + offsetPosition;
						this.vertices[1 + num32] = new Vector3(num34 * (1f * this._scale.x) - num35 * (-1f * this._scale.y), num35 * (1f * this._scale.x) + num34 * (-1f * this._scale.y), 0f) + offsetPosition;
						this.vertices[2 + num32] = new Vector3(num34 * (-1f * this._scale.x) - num35 * (1f * this._scale.y), num35 * (-1f * this._scale.x) + num34 * (1f * this._scale.y), 0f) + offsetPosition;
						this.vertices[3 + num32] = new Vector3(num34 * (-1f * this._scale.x) - num35 * (-1f * this._scale.y), num35 * (-1f * this._scale.x) + num34 * (-1f * this._scale.y), 0f) + offsetPosition;
					}
					Color32 color2 = this._color;
					this.colors[0 + num32] = color2;
					this.colors[1 + num32] = color2;
					this.colors[2 + num32] = color2;
					this.colors[3 + num32] = color2;
					num25++;
				}
			}
		}
	}

	// Token: 0x0400338B RID: 13195
	public bool debugMessages = true;

	// Token: 0x0400338C RID: 13196
	public ProFlareBatch.Mode mode;

	// Token: 0x0400338D RID: 13197
	public ProFlareAtlas _atlas;

	// Token: 0x0400338E RID: 13198
	public List<FlareData> FlaresList = new List<FlareData>();

	// Token: 0x0400338F RID: 13199
	public List<ProFlareElement> FlareElements = new List<ProFlareElement>();

	// Token: 0x04003390 RID: 13200
	public ProFlareElement[] FlareElementsArray;

	// Token: 0x04003391 RID: 13201
	public Camera GameCamera;

	// Token: 0x04003392 RID: 13202
	public Transform GameCameraTrans;

	// Token: 0x04003393 RID: 13203
	public Camera FlareCamera;

	// Token: 0x04003394 RID: 13204
	public Transform FlareCameraTrans;

	// Token: 0x04003395 RID: 13205
	public MeshFilter meshFilter;

	// Token: 0x04003396 RID: 13206
	public Transform thisTransform;

	// Token: 0x04003397 RID: 13207
	public MeshRenderer meshRender;

	// Token: 0x04003398 RID: 13208
	public float zPos;

	// Token: 0x04003399 RID: 13209
	private Mesh bufferMesh;

	// Token: 0x0400339A RID: 13210
	private Mesh meshA;

	// Token: 0x0400339B RID: 13211
	private Mesh meshB;

	// Token: 0x0400339C RID: 13212
	private bool PingPong;

	// Token: 0x0400339D RID: 13213
	public Material mat;

	// Token: 0x0400339E RID: 13214
	private Vector3[] vertices;

	// Token: 0x0400339F RID: 13215
	private Vector2[] uv;

	// Token: 0x040033A0 RID: 13216
	private Color32[] colors;

	// Token: 0x040033A1 RID: 13217
	private int[] triangles;

	// Token: 0x040033A2 RID: 13218
	public FlareOcclusion[] FlareOcclusionData;

	// Token: 0x040033A3 RID: 13219
	public bool useBrightnessThreshold = true;

	// Token: 0x040033A4 RID: 13220
	public int BrightnessThreshold = 1;

	// Token: 0x040033A5 RID: 13221
	public bool overdrawDebug;

	// Token: 0x040033A6 RID: 13222
	public bool dirty;

	// Token: 0x040033A7 RID: 13223
	public bool useCulling = true;

	// Token: 0x040033A8 RID: 13224
	public int cullFlaresAfterTime = 5;

	// Token: 0x040033A9 RID: 13225
	public int cullFlaresAfterCount = 5;

	// Token: 0x040033AA RID: 13226
	public bool culledFlaresNowVisiable;

	// Token: 0x040033AB RID: 13227
	private float reshowCulledFlaresTimer;

	// Token: 0x040033AC RID: 13228
	public float reshowCulledFlaresAfter = 0.3f;

	// Token: 0x040033AD RID: 13229
	public Transform helperTransform;

	// Token: 0x040033AE RID: 13230
	public bool showAllConnectedFlares;

	// Token: 0x040033AF RID: 13231
	public bool VR_Mode;

	// Token: 0x040033B0 RID: 13232
	public float VR_Depth = 0.2f;

	// Token: 0x040033B1 RID: 13233
	public bool SingleCamera_Mode;

	// Token: 0x040033B2 RID: 13234
	private Vector3[] verts;

	// Token: 0x040033B3 RID: 13235
	private Vector2 _scale;

	// Token: 0x040033B4 RID: 13236
	private Color32 _color;

	// Token: 0x040033B5 RID: 13237
	private float PI_Div180;

	// Token: 0x040033B6 RID: 13238
	private float Div180_PI;

	// Token: 0x040033B7 RID: 13239
	private int visibleFlares;

	// Token: 0x02000692 RID: 1682
	public enum Mode
	{
		// Token: 0x040033B9 RID: 13241
		Standard,
		// Token: 0x040033BA RID: 13242
		SingleCamera,
		// Token: 0x040033BB RID: 13243
		VR
	}
}
