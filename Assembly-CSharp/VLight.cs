using System;
using UnityEngine;

// Token: 0x02000601 RID: 1537
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(MeshRenderer))]
public class VLight : MonoBehaviour
{
	// Token: 0x17000391 RID: 913
	// (get) Token: 0x060025FF RID: 9727 RVA: 0x00019526 File Offset: 0x00017726
	// (set) Token: 0x06002600 RID: 9728 RVA: 0x0001952E File Offset: 0x0001772E
	public int MaxSlices
	{
		get
		{
			return this._maxSlices;
		}
		set
		{
			this._maxSlices = value;
		}
	}

	// Token: 0x06002601 RID: 9729 RVA: 0x0012610C File Offset: 0x0012430C
	public void OnEnable()
	{
		this._maxSlices = this.slices;
		int num = LayerMask.NameToLayer("vlight");
		if (num != -1)
		{
			base.gameObject.layer = num;
		}
		base.camera.enabled = false;
		base.camera.cullingMask &= ~(1 << base.gameObject.layer);
		this.CreateMaterials();
	}

	// Token: 0x06002602 RID: 9730 RVA: 0x0000264F File Offset: 0x0000084F
	private void OnApplicationQuit()
	{
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x00019537 File Offset: 0x00017737
	private void OnDestroy()
	{
		this.CleanMaterials();
		this.SafeDestroy(this._depthTexture);
		this.SafeDestroy(this.meshContainer);
	}

	// Token: 0x06002604 RID: 9732 RVA: 0x00019557 File Offset: 0x00017757
	private void Start()
	{
		this.CreateMaterials();
		this.spotNear = base.camera.near;
		this.spotRange = base.camera.far;
		this.spotAngle = base.camera.fov;
	}

	// Token: 0x06002605 RID: 9733 RVA: 0x00019537 File Offset: 0x00017737
	public void Reset()
	{
		this.CleanMaterials();
		this.SafeDestroy(this._depthTexture);
		this.SafeDestroy(this.meshContainer);
	}

	// Token: 0x06002606 RID: 9734 RVA: 0x0012617C File Offset: 0x0012437C
	public bool GenerateNewMaterial(Material originalMaterial, ref Material instancedMaterial)
	{
		string text = base.GetInstanceID().ToString();
		if (originalMaterial != null && (instancedMaterial == null || instancedMaterial.name.IndexOf(text, 5) < 0 || instancedMaterial.name.IndexOf(originalMaterial.name, 5) < 0))
		{
			if (!originalMaterial.shader.isSupported)
			{
				Debug.LogError("Volumetric light shader not supported");
				base.enabled = false;
				return false;
			}
			Material material = originalMaterial;
			if (instancedMaterial != null && instancedMaterial.name.IndexOf(originalMaterial.name, 5) > 0)
			{
				material = instancedMaterial;
			}
			instancedMaterial = new Material(material);
			instancedMaterial.name = text + " " + originalMaterial.name;
		}
		return true;
	}

	// Token: 0x06002607 RID: 9735 RVA: 0x00126254 File Offset: 0x00124454
	public void CreateMaterials()
	{
		this._propertyBlock = new MaterialPropertyBlock();
		this._idColorTint = Shader.PropertyToID("_Color");
		this._idLightMultiplier = Shader.PropertyToID("_Strength");
		this._idSpotExponent = Shader.PropertyToID("_SpotExp");
		this._idConstantAttenuation = Shader.PropertyToID("_ConstantAttn");
		this._idLinearAttenuation = Shader.PropertyToID("_LinearAttn");
		this._idQuadraticAttenuation = Shader.PropertyToID("_QuadAttn");
		this._idLightParams = Shader.PropertyToID("_LightParams");
		this._idMinBounds = Shader.PropertyToID("_minBounds");
		this._idMaxBounds = Shader.PropertyToID("_maxBounds");
		this._idViewWorldLight = Shader.PropertyToID("_ViewWorldLight");
		this._idLocalRotation = Shader.PropertyToID("_LocalRotation");
		this._idRotation = Shader.PropertyToID("_Rotation");
		this._idProjection = Shader.PropertyToID("_Projection");
		Material material = (this.lightType != VLight.LightTypes.Spot) ? this._instancedPointMaterial : this._instancedSpotMaterial;
		if (material == null)
		{
			Material material2 = (this.lightType != VLight.LightTypes.Spot) ? this.pointMaterial : this.spotMaterial;
		}
		if (this._instancedSpotMaterial != null)
		{
			this.spotEmission = this._instancedSpotMaterial.GetTexture("_LightColorEmission");
			this.spotNoise = this._instancedSpotMaterial.GetTexture("_NoiseTex");
			this.spotShadow = this._instancedSpotMaterial.GetTexture("_ShadowTexture");
		}
		if (this._instancedPointMaterial != null)
		{
			this.pointEmission = (this._instancedPointMaterial.GetTexture("_LightColorEmission") as Cubemap);
			this.pointNoise = (this._instancedPointMaterial.GetTexture("_NoiseTex") as Cubemap);
			this.pointShadow = (this._instancedPointMaterial.GetTexture("_ShadowTexture") as Cubemap);
		}
		bool flag = false;
		flag |= this.GenerateNewMaterial(this.pointMaterial, ref this._instancedPointMaterial);
		flag |= this.GenerateNewMaterial(this.spotMaterial, ref this._instancedSpotMaterial);
		if (flag)
		{
			VLight.LightTypes lightTypes = this.lightType;
			if (lightTypes != VLight.LightTypes.Spot)
			{
				if (lightTypes == VLight.LightTypes.Point)
				{
					base.renderer.sharedMaterial = this._instancedPointMaterial;
					if (this.pointEmission != null)
					{
						base.renderer.sharedMaterial.SetTexture("_LightColorEmission", this.pointEmission);
					}
					if (this.pointNoise != null)
					{
						base.renderer.sharedMaterial.SetTexture("_NoiseTex", this.pointNoise);
					}
					if (this.pointShadow != null)
					{
						base.renderer.sharedMaterial.SetTexture("_ShadowTexture", this.pointShadow);
					}
				}
			}
			else
			{
				base.renderer.sharedMaterial = this._instancedSpotMaterial;
				if (this.spotEmission != null)
				{
					base.renderer.sharedMaterial.SetTexture("_LightColorEmission", this.spotEmission);
				}
				if (this.spotNoise != null)
				{
					base.renderer.sharedMaterial.SetTexture("_NoiseTex", this.spotNoise);
				}
				if (this.spotShadow != null)
				{
					base.renderer.sharedMaterial.SetTexture("_ShadowTexture", this.spotShadow);
				}
			}
		}
	}

	// Token: 0x06002608 RID: 9736 RVA: 0x001265B8 File Offset: 0x001247B8
	private void CleanMaterials()
	{
		this.SafeDestroy(this._instancedSpotMaterial);
		this.SafeDestroy(this._instancedPointMaterial);
		this.SafeDestroy(base.renderer.sharedMaterial);
		this.SafeDestroy(this.meshContainer);
		this._prevMaterialPoint = null;
		this._prevMaterialSpot = null;
		this._instancedSpotMaterial = null;
		this._instancedPointMaterial = null;
		this.meshContainer = null;
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x00126620 File Offset: 0x00124820
	private void OnDrawGizmosSelected()
	{
		if (this._frustrumPoints == null)
		{
			return;
		}
		Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[0]), base.transform.TransformPoint(this._frustrumPoints[1]));
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[2]), base.transform.TransformPoint(this._frustrumPoints[3]));
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[4]), base.transform.TransformPoint(this._frustrumPoints[5]));
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[6]), base.transform.TransformPoint(this._frustrumPoints[7]));
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[1]), base.transform.TransformPoint(this._frustrumPoints[3]));
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[3]), base.transform.TransformPoint(this._frustrumPoints[7]));
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[7]), base.transform.TransformPoint(this._frustrumPoints[5]));
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[5]), base.transform.TransformPoint(this._frustrumPoints[1]));
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[0]), base.transform.TransformPoint(this._frustrumPoints[2]));
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[2]), base.transform.TransformPoint(this._frustrumPoints[6]));
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[6]), base.transform.TransformPoint(this._frustrumPoints[4]));
		Gizmos.DrawLine(base.transform.TransformPoint(this._frustrumPoints[4]), base.transform.TransformPoint(this._frustrumPoints[0]));
	}

	// Token: 0x0600260A RID: 9738 RVA: 0x00126934 File Offset: 0x00124B34
	private void CalculateMinMax(out Vector3 min, out Vector3 max, bool forceFrustrumUpdate)
	{
		if (this._frustrumPoints == null || forceFrustrumUpdate)
		{
			VLightGeometryUtil.RecalculateFrustrumPoints(base.camera, 1f, out this._frustrumPoints);
		}
		Vector3[] array = new Vector3[8];
		Vector3 vector;
		vector..ctor(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
		Vector3 vector2;
		vector2..ctor(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		Matrix4x4 matrix4x = this._viewWorldToCameraMatrixCached * this._localToWorldMatrix;
		for (int i = 0; i < this._frustrumPoints.Length; i++)
		{
			array[i] = matrix4x.MultiplyPoint(this._frustrumPoints[i]);
			vector.x = ((vector.x <= array[i].x) ? array[i].x : vector.x);
			vector.y = ((vector.y <= array[i].y) ? array[i].y : vector.y);
			vector.z = ((vector.z <= array[i].z) ? array[i].z : vector.z);
			vector2.x = ((vector2.x > array[i].x) ? array[i].x : vector2.x);
			vector2.y = ((vector2.y > array[i].y) ? array[i].y : vector2.y);
			vector2.z = ((vector2.z > array[i].z) ? array[i].z : vector2.z);
		}
		min = vector;
		max = vector2;
	}

	// Token: 0x0600260B RID: 9739 RVA: 0x00126B54 File Offset: 0x00124D54
	private Matrix4x4 CalculateProjectionMatrix()
	{
		float fov = base.camera.fov;
		float near = base.camera.near;
		float far = base.camera.far;
		Matrix4x4 result;
		if (!base.camera.isOrthoGraphic)
		{
			result = Matrix4x4.Perspective(fov, 1f, near, far);
		}
		else
		{
			float num = base.camera.orthographicSize * 0.5f;
			result = Matrix4x4.Ortho(-num, num, -num, num, far, near);
		}
		return result;
	}

	// Token: 0x0600260C RID: 9740 RVA: 0x00126BD0 File Offset: 0x00124DD0
	private void BuildMesh(bool manualPositioning, int planeCount, Vector3 minBounds, Vector3 maxBounds)
	{
		if (this.meshContainer == null || this.meshContainer.name.IndexOf(base.GetInstanceID().ToString(), 5) != 0)
		{
			this.meshContainer = new Mesh();
			this.meshContainer.hideFlags = 13;
			this.meshContainer.name = base.GetInstanceID().ToString();
		}
		if (this._meshFilter == null)
		{
			this._meshFilter = base.GetComponent<MeshFilter>();
		}
		Vector3[] array = new Vector3[65000];
		int[] array2 = new int[195000];
		int num = 0;
		int num2 = 0;
		float num3 = 1f / (float)(planeCount - 1);
		float num4 = (!manualPositioning) ? 0f : 1f;
		float num5 = 0f;
		float num6 = 1f;
		float num7 = 0f;
		float num8 = 1f;
		int num9 = 0;
		for (int i = 0; i < planeCount; i++)
		{
			Vector3[] array3 = new Vector3[4];
			Vector3[] array5;
			if (manualPositioning)
			{
				Plane[] array4 = GeometryUtility.CalculateFrustumPlanes(this._projectionMatrixCached * base.camera.worldToCameraMatrix);
				for (int j = 0; j < array4.Length; j++)
				{
					Vector3 vector = array4[j].normal * -array4[j].distance;
					array4[j] = new Plane(this._viewWorldToCameraMatrixCached.MultiplyVector(array4[j].normal), this._viewWorldToCameraMatrixCached.MultiplyPoint3x4(vector));
				}
				array3[0] = this.CalculateTriLerp(new Vector3(num5, num7, num4), minBounds, maxBounds);
				array3[1] = this.CalculateTriLerp(new Vector3(num5, num8, num4), minBounds, maxBounds);
				array3[2] = this.CalculateTriLerp(new Vector3(num6, num8, num4), minBounds, maxBounds);
				array3[3] = this.CalculateTriLerp(new Vector3(num6, num7, num4), minBounds, maxBounds);
				array5 = VLightGeometryUtil.ClipPolygonAgainstPlane(array3, array4);
			}
			else
			{
				array3[0] = new Vector3(num5, num7, num4);
				array3[1] = new Vector3(num5, num8, num4);
				array3[2] = new Vector3(num6, num8, num4);
				array3[3] = new Vector3(num6, num7, num4);
				array5 = array3;
			}
			num4 += ((!manualPositioning) ? num3 : (-num3));
			if (array5.Length > 2)
			{
				Array.Copy(array5, 0, array, num, array5.Length);
				num += array5.Length;
				int[] array6 = new int[(array5.Length - 2) * 3];
				int num10 = 0;
				for (int k = 0; k < array6.Length; k += 3)
				{
					array6[k] = num9;
					array6[k + 1] = num9 + (num10 + 1);
					array6[k + 2] = num9 + (num10 + 2);
					num10++;
				}
				num9 += array5.Length;
				Array.Copy(array6, 0, array2, num2, array6.Length);
				num2 += array6.Length;
			}
		}
		this.meshContainer.Clear();
		Vector3[] array7 = new Vector3[num];
		Array.Copy(array, array7, num);
		this.meshContainer.vertices = array7;
		int[] array8 = new int[num2];
		Array.Copy(array2, array8, num2);
		this.meshContainer.triangles = array8;
		this.meshContainer.normals = new Vector3[num];
		this.meshContainer.uv = new Vector2[num];
		Vector3 vector2 = Vector3.zero;
		foreach (Vector3 vector3 in this._frustrumPoints)
		{
			vector2 += vector3;
		}
		vector2 /= (float)this._frustrumPoints.Length;
		Bounds bounds;
		bounds..ctor(vector2, Vector3.zero);
		foreach (Vector3 vector4 in this._frustrumPoints)
		{
			bounds.Encapsulate(vector4);
		}
		this._meshFilter.sharedMesh = this.meshContainer;
		this._meshFilter.sharedMesh.bounds = bounds;
	}

	// Token: 0x0600260D RID: 9741 RVA: 0x0012703C File Offset: 0x0012523C
	private Vector3 CalculateTriLerp(Vector3 vertex, Vector3 minBounds, Vector3 maxBounds)
	{
		Vector3 vector = new Vector3(1f, 1f, 1f) - vertex;
		return new Vector3(minBounds.x * vertex.x, minBounds.y * vertex.y, maxBounds.z * vertex.z) + new Vector3(maxBounds.x * vector.x, maxBounds.y * vector.y, minBounds.z * vector.z);
	}

	// Token: 0x0600260E RID: 9742 RVA: 0x001270D0 File Offset: 0x001252D0
	public void RenderShadowMap()
	{
		float far = base.camera.far;
		switch (this.shadowMode)
		{
		case VLight.ShadowMode.Realtime:
			if (SystemInfo.supportsImageEffects)
			{
				int num = LayerMask.NameToLayer("vlight");
				if (num != -1)
				{
					base.gameObject.layer = num;
					base.camera.backgroundColor = Color.red;
					if (Camera.main == base.camera)
					{
						Debug.LogError("Damn Camera.main == VLightCamera ");
					}
					base.camera.clearFlags = 2;
					base.camera.depthTextureMode = 0;
					base.camera.renderingPath = 0;
					this.CreateDepthTexture(this.lightType);
					if (this.renderDepthShader == null)
					{
						this.renderDepthShader = Shader.Find("V-Light/Volumetric Light Depth");
					}
					VLight.LightTypes lightTypes = this.lightType;
					if (lightTypes != VLight.LightTypes.Spot)
					{
						if (lightTypes == VLight.LightTypes.Point)
						{
							base.camera.projectionMatrix = Matrix4x4.Perspective(90f, 1f, 0.1f, far);
							base.camera.SetReplacementShader(this.renderDepthShader, "RenderType");
							base.camera.RenderToCubemap(this._depthTexture, 63);
							base.camera.ResetReplacementShader();
						}
					}
					else
					{
						base.camera.targetTexture = this._depthTexture;
						base.camera.projectionMatrix = this.CalculateProjectionMatrix();
						base.camera.RenderWithShader(this.renderDepthShader, "RenderType");
					}
				}
			}
			break;
		}
	}

	// Token: 0x0600260F RID: 9743 RVA: 0x00019592 File Offset: 0x00017792
	private RenderTexture GenerateShadowMap(int resX, int resY)
	{
		return new RenderTexture(256, 256, 1, 0, 1);
	}

	// Token: 0x06002610 RID: 9744 RVA: 0x00127270 File Offset: 0x00125470
	private void CreateDepthTexture(VLight.LightTypes type)
	{
		if (this._depthTexture == null)
		{
			this._depthTexture = this.GenerateShadowMap(256, 256);
			this._depthTexture.hideFlags = 13;
			this._depthTexture.isPowerOfTwo = true;
			if (type == VLight.LightTypes.Point)
			{
				this._depthTexture.isCubemap = true;
			}
		}
		else if (type == VLight.LightTypes.Point && !this._depthTexture.isCubemap && this._depthTexture.IsCreated())
		{
			this.SafeDestroy(this._depthTexture);
			this._depthTexture = this.GenerateShadowMap(256, 256);
			this._depthTexture.hideFlags = 13;
			this._depthTexture.isPowerOfTwo = true;
			this._depthTexture.isCubemap = true;
		}
		else if (type == VLight.LightTypes.Spot && this._depthTexture.isCubemap && this._depthTexture.IsCreated())
		{
			this.SafeDestroy(this._depthTexture);
			this._depthTexture = this.GenerateShadowMap(512, 512);
			this._depthTexture.hideFlags = 13;
			this._depthTexture.isPowerOfTwo = true;
			this._depthTexture.isCubemap = false;
		}
	}

	// Token: 0x06002611 RID: 9745 RVA: 0x001273C4 File Offset: 0x001255C4
	private void OnWillRenderObject()
	{
		if (!this.lockTransforms)
		{
			this.UpdateSettings();
			this.UpdateLightMatrices();
		}
		this.UpdateViewMatrices(Camera.current);
		this.CalculateMinMax(out this._minBounds, out this._maxBounds, this._cameraHasBeenUpdated);
		this.SetShaderPropertiesBlock(this._propertyBlock);
		base.renderer.SetPropertyBlock(this._propertyBlock);
	}

	// Token: 0x06002612 RID: 9746 RVA: 0x000195A6 File Offset: 0x000177A6
	private void OnBecameVisible()
	{
		this._isVisible = true;
	}

	// Token: 0x06002613 RID: 9747 RVA: 0x000195AF File Offset: 0x000177AF
	private void OnBecameInvisible()
	{
		this._isVisible = false;
	}

	// Token: 0x06002614 RID: 9748 RVA: 0x000195B8 File Offset: 0x000177B8
	private void Update()
	{
		this.UpdateSettings();
		this.UpdateLightMatrices();
		if (this._isVisible)
		{
			this.RenderShadowMap();
		}
	}

	// Token: 0x06002615 RID: 9749 RVA: 0x00127428 File Offset: 0x00125628
	private bool CameraHasBeenUpdated()
	{
		bool flag = false;
		flag |= (this._meshFilter == null || this._meshFilter.sharedMesh == null);
		flag |= (this.spotRange != this._prevFar);
		flag |= (this.spotNear != this._prevNear);
		flag |= (this.spotAngle != this._prevFov);
		flag |= (base.camera.orthographicSize != this._prevOrthoSize);
		flag |= (base.camera.isOrthoGraphic != this._prevIsOrtho);
		flag |= (this.pointLightRadius != this._prevPointLightRadius);
		flag |= (this.spotMaterial != this._prevMaterialSpot);
		flag |= (this.pointMaterial != this._prevMaterialPoint);
		flag |= (this._prevSlices != this.slices);
		flag |= (this._prevShadowMode != this.shadowMode);
		return flag | this._prevLightType != this.lightType;
	}

	// Token: 0x06002616 RID: 9750 RVA: 0x00127548 File Offset: 0x00125748
	public void UpdateSettings()
	{
		this._cameraHasBeenUpdated = this.CameraHasBeenUpdated();
		if (this._cameraHasBeenUpdated)
		{
			VLight.LightTypes lightTypes = this.lightType;
			if (lightTypes != VLight.LightTypes.Spot)
			{
				if (lightTypes == VLight.LightTypes.Point)
				{
					base.renderer.sharedMaterial = this._instancedPointMaterial;
					base.camera.isOrthoGraphic = true;
					base.camera.near = -this.pointLightRadius;
					base.camera.far = this.pointLightRadius;
					base.camera.orthographicSize = this.pointLightRadius * 2f;
				}
			}
			else
			{
				base.renderer.sharedMaterial = this._instancedSpotMaterial;
				base.camera.far = this.spotRange;
				base.camera.near = this.spotNear;
				base.camera.fov = this.spotAngle;
				base.camera.isOrthoGraphic = false;
			}
			if ((this.shadowMode == VLight.ShadowMode.None || this.shadowMode == VLight.ShadowMode.Baked) && this._depthTexture != null)
			{
				this.SafeDestroy(this._depthTexture);
			}
		}
		this._prevSlices = this.slices;
		this._prevFov = base.camera.fov;
		this._prevNear = base.camera.near;
		this._prevFar = base.camera.far;
		this._prevIsOrtho = base.camera.isOrthoGraphic;
		this._prevOrthoSize = base.camera.orthographicSize;
		this._prevMaterialSpot = this.spotMaterial;
		this._prevMaterialPoint = this.pointMaterial;
		this._prevShadowMode = this.shadowMode;
		this._prevLightType = this.lightType;
		this._prevPointLightRadius = this.pointLightRadius;
	}

	// Token: 0x06002617 RID: 9751 RVA: 0x00127708 File Offset: 0x00125908
	public void UpdateLightMatrices()
	{
		this._localToWorldMatrix = base.transform.localToWorldMatrix;
		this._worldToCamera = base.camera.worldToCameraMatrix;
		this._rotation = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(this._angle.x, this._angle.y, this._angle.z), Vector3.one);
		this._angle += this.noiseSpeed * Time.deltaTime;
		this.RebuildMesh();
	}

	// Token: 0x06002618 RID: 9752 RVA: 0x0012779C File Offset: 0x0012599C
	public void UpdateViewMatrices(Camera targetCamera)
	{
		this._viewWorldToCameraMatrixCached = targetCamera.worldToCameraMatrix;
		this._viewCameraToWorldMatrixCached = targetCamera.cameraToWorldMatrix;
		VLight.LightTypes lightTypes = this.lightType;
		if (lightTypes != VLight.LightTypes.Spot)
		{
			if (lightTypes == VLight.LightTypes.Point)
			{
				Matrix4x4 matrix4x = Matrix4x4.TRS(-base.transform.position, Quaternion.identity, Vector3.one);
				this._localRotation = Matrix4x4.TRS(Vector3.zero, base.transform.rotation, Vector3.one);
				this._viewWorldLight = matrix4x * this._viewCameraToWorldMatrixCached;
			}
		}
		else
		{
			this._viewWorldLight = this._worldToCamera * this._viewCameraToWorldMatrixCached;
		}
	}

	// Token: 0x06002619 RID: 9753 RVA: 0x00127850 File Offset: 0x00125A50
	public void RebuildMesh()
	{
		this.CalculateMinMax(out this._minBounds, out this._maxBounds, this._cameraHasBeenUpdated);
		if (this._cameraHasBeenUpdated)
		{
			this._projectionMatrixCached = this.CalculateProjectionMatrix();
			this.CreateMaterials();
			if (Application.isPlaying)
			{
				if (!this._builtMesh)
				{
					this._builtMesh = true;
					this.BuildMesh(false, this.slices, this._minBounds, this._maxBounds);
				}
			}
			else
			{
				this.BuildMesh(false, this.slices, this._minBounds, this._maxBounds);
			}
		}
	}

	// Token: 0x0600261A RID: 9754 RVA: 0x001278E8 File Offset: 0x00125AE8
	public MaterialPropertyBlock CreatePropertiesBlock()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		materialPropertyBlock.AddVector(this._idMinBounds, this._minBounds);
		materialPropertyBlock.AddVector(this._idMaxBounds, this._maxBounds);
		materialPropertyBlock.AddMatrix(this._idProjection, this._projectionMatrixCached);
		materialPropertyBlock.AddMatrix(this._idViewWorldLight, this._viewWorldLight);
		materialPropertyBlock.AddMatrix(this._idLocalRotation, this._localRotation);
		materialPropertyBlock.AddMatrix(this._idRotation, this._rotation);
		materialPropertyBlock.AddColor(this._idColorTint, this.colorTint);
		materialPropertyBlock.AddFloat(this._idSpotExponent, this.spotExponent);
		materialPropertyBlock.AddFloat(this._idConstantAttenuation, this.constantAttenuation);
		materialPropertyBlock.AddFloat(this._idLinearAttenuation, this.linearAttenuation);
		materialPropertyBlock.AddFloat(this._idQuadraticAttenuation, this.quadraticAttenuation);
		materialPropertyBlock.AddFloat(this._idLightMultiplier, this.lightMultiplier);
		switch (this.shadowMode)
		{
		case VLight.ShadowMode.None:
			base.renderer.sharedMaterial.SetTexture("_ShadowTexture", null);
			break;
		case VLight.ShadowMode.Realtime:
			base.renderer.sharedMaterial.SetTexture("_ShadowTexture", this._depthTexture);
			break;
		}
		float far = base.camera.far;
		float near = base.camera.near;
		float fov = base.camera.fov;
		materialPropertyBlock.AddVector(this._idLightParams, new Vector4(near, far, far - near, (!base.camera.isOrthoGraphic) ? (fov * 0.5f * 0.017453292f) : 3.1415927f));
		return materialPropertyBlock;
	}

	// Token: 0x0600261B RID: 9755 RVA: 0x00127AA0 File Offset: 0x00125CA0
	public void SetShaderPropertiesBlock(MaterialPropertyBlock propertyBlock)
	{
		propertyBlock.Clear();
		propertyBlock.AddVector(this._idMinBounds, this._minBounds);
		propertyBlock.AddVector(this._idMaxBounds, this._maxBounds);
		propertyBlock.AddMatrix(this._idProjection, this._projectionMatrixCached);
		propertyBlock.AddMatrix(this._idViewWorldLight, this._viewWorldLight);
		propertyBlock.AddMatrix(this._idLocalRotation, this._localRotation);
		propertyBlock.AddMatrix(this._idRotation, this._rotation);
		propertyBlock.AddColor(this._idColorTint, this.colorTint);
		propertyBlock.AddFloat(this._idSpotExponent, this.spotExponent);
		propertyBlock.AddFloat(this._idConstantAttenuation, this.constantAttenuation);
		propertyBlock.AddFloat(this._idLinearAttenuation, this.linearAttenuation);
		propertyBlock.AddFloat(this._idQuadraticAttenuation, this.quadraticAttenuation);
		propertyBlock.AddFloat(this._idLightMultiplier, this.lightMultiplier);
		switch (this.shadowMode)
		{
		case VLight.ShadowMode.None:
			base.renderer.sharedMaterial.SetTexture("_ShadowTexture", null);
			break;
		case VLight.ShadowMode.Realtime:
			base.renderer.sharedMaterial.SetTexture("_ShadowTexture", this._depthTexture);
			break;
		}
		float far = base.camera.far;
		float near = base.camera.near;
		float fov = base.camera.fov;
		propertyBlock.AddVector(this._idLightParams, new Vector4(near, far, far - near, (!base.camera.isOrthoGraphic) ? (fov * 0.5f * 0.017453292f) : 3.1415927f));
	}

	// Token: 0x0600261C RID: 9756 RVA: 0x00127C54 File Offset: 0x00125E54
	public void SetShaderPropertiesMaterials()
	{
		Material sharedMaterial = base.renderer.sharedMaterial;
		sharedMaterial.SetVector("_minBounds", this._minBounds);
		sharedMaterial.SetVector("_maxBounds", this._maxBounds);
		sharedMaterial.SetMatrix("_Projection", this._projectionMatrixCached);
		sharedMaterial.SetMatrix("_ViewWorldLight", this._viewWorldLight);
		sharedMaterial.SetMatrix("_LocalRotation", this._localRotation);
		sharedMaterial.SetMatrix("_Rotation", this._rotation);
		Plane[] array = GeometryUtility.CalculateFrustumPlanes(this._projectionMatrixCached);
		VLight.LightTypes lightTypes = this.lightType;
		if (lightTypes != VLight.LightTypes.Spot)
		{
			if (lightTypes == VLight.LightTypes.Point)
			{
				for (int i = 0; i < array.Length; i++)
				{
					Vector3 vector = base.transform.TransformDirection(array[i].normal);
					float distance = array[i].distance;
					sharedMaterial.SetVector("_FrustrumPlane" + i, new Vector4(vector.x, vector.y, vector.z, distance));
				}
			}
		}
		else
		{
			for (int j = 0; j < array.Length; j++)
			{
				Vector3 normal = array[j].normal;
				float distance2 = array[j].distance;
				sharedMaterial.SetVector("_FrustrumPlane" + j, new Vector4(normal.x, normal.y, normal.z, distance2));
			}
		}
		switch (this.shadowMode)
		{
		case VLight.ShadowMode.None:
			sharedMaterial.SetTexture("_ShadowTexture", null);
			break;
		case VLight.ShadowMode.Realtime:
			sharedMaterial.SetTexture("_ShadowTexture", this._depthTexture);
			break;
		}
		float far = base.camera.far;
		float near = base.camera.near;
		float fov = base.camera.fov;
		sharedMaterial.SetVector("_LightParams", new Vector4(near, far, far - near, (!base.camera.isOrthoGraphic) ? (fov * 0.5f * 0.017453292f) : 3.1415927f));
	}

	// Token: 0x0600261D RID: 9757 RVA: 0x000195D7 File Offset: 0x000177D7
	private void SafeDestroy(Object obj)
	{
		if (obj != null)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(obj);
			}
			else
			{
				Object.DestroyImmediate(obj, true);
			}
		}
		obj = null;
	}

	// Token: 0x04002E93 RID: 11923
	private const int VERT_COUNT = 65000;

	// Token: 0x04002E94 RID: 11924
	private const int TRI_COUNT = 195000;

	// Token: 0x04002E95 RID: 11925
	private const StringComparison STR_CMP_TYPE = 5;

	// Token: 0x04002E96 RID: 11926
	[HideInInspector]
	[SerializeField]
	private Material spotMaterial;

	// Token: 0x04002E97 RID: 11927
	[HideInInspector]
	[SerializeField]
	private Material pointMaterial;

	// Token: 0x04002E98 RID: 11928
	[SerializeField]
	[HideInInspector]
	private Shader renderDepthShader;

	// Token: 0x04002E99 RID: 11929
	[HideInInspector]
	public bool lockTransforms;

	// Token: 0x04002E9A RID: 11930
	[HideInInspector]
	public bool renderWireFrame = true;

	// Token: 0x04002E9B RID: 11931
	public VLight.LightTypes lightType;

	// Token: 0x04002E9C RID: 11932
	public float pointLightRadius = 1f;

	// Token: 0x04002E9D RID: 11933
	public float spotRange = 1f;

	// Token: 0x04002E9E RID: 11934
	public float spotNear = 1f;

	// Token: 0x04002E9F RID: 11935
	public float spotAngle = 45f;

	// Token: 0x04002EA0 RID: 11936
	public VLight.ShadowMode shadowMode;

	// Token: 0x04002EA1 RID: 11937
	public int slices = 30;

	// Token: 0x04002EA2 RID: 11938
	public Color colorTint = Color.white;

	// Token: 0x04002EA3 RID: 11939
	public float lightMultiplier = 1f;

	// Token: 0x04002EA4 RID: 11940
	public float spotExponent = 1f;

	// Token: 0x04002EA5 RID: 11941
	public float constantAttenuation = 1f;

	// Token: 0x04002EA6 RID: 11942
	public float linearAttenuation = 10f;

	// Token: 0x04002EA7 RID: 11943
	public float quadraticAttenuation = 100f;

	// Token: 0x04002EA8 RID: 11944
	public Vector3 noiseSpeed;

	// Token: 0x04002EA9 RID: 11945
	[SerializeField]
	[HideInInspector]
	private Texture spotEmission;

	// Token: 0x04002EAA RID: 11946
	[HideInInspector]
	[SerializeField]
	private Texture spotNoise;

	// Token: 0x04002EAB RID: 11947
	[SerializeField]
	[HideInInspector]
	private Texture spotShadow;

	// Token: 0x04002EAC RID: 11948
	[HideInInspector]
	[SerializeField]
	private Cubemap pointEmission;

	// Token: 0x04002EAD RID: 11949
	[SerializeField]
	[HideInInspector]
	private Cubemap pointNoise;

	// Token: 0x04002EAE RID: 11950
	[SerializeField]
	[HideInInspector]
	private Cubemap pointShadow;

	// Token: 0x04002EAF RID: 11951
	[SerializeField]
	[HideInInspector]
	private Mesh meshContainer;

	// Token: 0x04002EB0 RID: 11952
	[SerializeField]
	[HideInInspector]
	private Material _prevMaterialSpot;

	// Token: 0x04002EB1 RID: 11953
	[HideInInspector]
	[SerializeField]
	private Material _prevMaterialPoint;

	// Token: 0x04002EB2 RID: 11954
	[HideInInspector]
	[SerializeField]
	public Material _instancedSpotMaterial;

	// Token: 0x04002EB3 RID: 11955
	[SerializeField]
	[HideInInspector]
	public Material _instancedPointMaterial;

	// Token: 0x04002EB4 RID: 11956
	private MaterialPropertyBlock _propertyBlock;

	// Token: 0x04002EB5 RID: 11957
	private int _idColorTint;

	// Token: 0x04002EB6 RID: 11958
	private int _idLightMultiplier;

	// Token: 0x04002EB7 RID: 11959
	private int _idSpotExponent;

	// Token: 0x04002EB8 RID: 11960
	private int _idConstantAttenuation;

	// Token: 0x04002EB9 RID: 11961
	private int _idLinearAttenuation;

	// Token: 0x04002EBA RID: 11962
	private int _idQuadraticAttenuation;

	// Token: 0x04002EBB RID: 11963
	private int _idLightParams;

	// Token: 0x04002EBC RID: 11964
	private int _idMinBounds;

	// Token: 0x04002EBD RID: 11965
	private int _idMaxBounds;

	// Token: 0x04002EBE RID: 11966
	private int _idViewWorldLight;

	// Token: 0x04002EBF RID: 11967
	private int _idRotation;

	// Token: 0x04002EC0 RID: 11968
	private int _idLocalRotation;

	// Token: 0x04002EC1 RID: 11969
	private int _idProjection;

	// Token: 0x04002EC2 RID: 11970
	private VLight.LightTypes _prevLightType;

	// Token: 0x04002EC3 RID: 11971
	private VLight.ShadowMode _prevShadowMode;

	// Token: 0x04002EC4 RID: 11972
	private int _prevSlices;

	// Token: 0x04002EC5 RID: 11973
	private bool _frustrumSwitch;

	// Token: 0x04002EC6 RID: 11974
	private bool _prevIsOrtho;

	// Token: 0x04002EC7 RID: 11975
	private float _prevNear;

	// Token: 0x04002EC8 RID: 11976
	private float _prevFar;

	// Token: 0x04002EC9 RID: 11977
	private float _prevFov;

	// Token: 0x04002ECA RID: 11978
	private float _prevOrthoSize;

	// Token: 0x04002ECB RID: 11979
	private float _prevPointLightRadius;

	// Token: 0x04002ECC RID: 11980
	private Matrix4x4 _worldToCamera;

	// Token: 0x04002ECD RID: 11981
	private Matrix4x4 _projectionMatrixCached;

	// Token: 0x04002ECE RID: 11982
	private Matrix4x4 _viewWorldToCameraMatrixCached;

	// Token: 0x04002ECF RID: 11983
	private Matrix4x4 _viewCameraToWorldMatrixCached;

	// Token: 0x04002ED0 RID: 11984
	private Matrix4x4 _localToWorldMatrix;

	// Token: 0x04002ED1 RID: 11985
	private Matrix4x4 _rotation;

	// Token: 0x04002ED2 RID: 11986
	private Matrix4x4 _localRotation;

	// Token: 0x04002ED3 RID: 11987
	private Matrix4x4 _viewWorldLight;

	// Token: 0x04002ED4 RID: 11988
	private Vector3[] _frustrumPoints;

	// Token: 0x04002ED5 RID: 11989
	private Vector3 _angle = Vector3.zero;

	// Token: 0x04002ED6 RID: 11990
	private Vector3 _minBounds;

	// Token: 0x04002ED7 RID: 11991
	private Vector3 _maxBounds;

	// Token: 0x04002ED8 RID: 11992
	private bool _cameraHasBeenUpdated;

	// Token: 0x04002ED9 RID: 11993
	private MeshFilter _meshFilter;

	// Token: 0x04002EDA RID: 11994
	private RenderTexture _depthTexture;

	// Token: 0x04002EDB RID: 11995
	private bool _builtMesh;

	// Token: 0x04002EDC RID: 11996
	private int _maxSlices;

	// Token: 0x04002EDD RID: 11997
	private bool _isVisible;

	// Token: 0x02000602 RID: 1538
	public enum ShadowMode
	{
		// Token: 0x04002EDF RID: 11999
		None,
		// Token: 0x04002EE0 RID: 12000
		Realtime,
		// Token: 0x04002EE1 RID: 12001
		Baked
	}

	// Token: 0x02000603 RID: 1539
	public enum LightTypes
	{
		// Token: 0x04002EE3 RID: 12003
		Spot,
		// Token: 0x04002EE4 RID: 12004
		Point
	}
}
