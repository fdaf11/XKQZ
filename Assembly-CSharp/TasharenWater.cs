using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200060B RID: 1547
[ExecuteInEditMode]
[AddComponentMenu("Tasharen/Water")]
[RequireComponent(typeof(Renderer))]
public class TasharenWater : MonoBehaviour
{
	// Token: 0x17000397 RID: 919
	// (get) Token: 0x0600264C RID: 9804 RVA: 0x00128D24 File Offset: 0x00126F24
	public int reflectionTextureSize
	{
		get
		{
			switch (this.quality)
			{
			case TasharenWater.Quality.Medium:
			case TasharenWater.Quality.High:
				return 512;
			case TasharenWater.Quality.Uber:
				return 1024;
			default:
				return 0;
			}
		}
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x0600264D RID: 9805 RVA: 0x00128D60 File Offset: 0x00126F60
	public LayerMask reflectionMask
	{
		get
		{
			switch (this.quality)
			{
			case TasharenWater.Quality.Medium:
				return this.mediumReflectionMask;
			case TasharenWater.Quality.High:
			case TasharenWater.Quality.Uber:
				return this.highReflectionMask;
			default:
				return 0;
			}
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x0600264E RID: 9806 RVA: 0x0001980A File Offset: 0x00017A0A
	public bool useRefraction
	{
		get
		{
			return this.quality > TasharenWater.Quality.Fastest;
		}
	}

	// Token: 0x0600264F RID: 9807 RVA: 0x00019815 File Offset: 0x00017A15
	private static float SignExt(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	// Token: 0x06002650 RID: 9808 RVA: 0x00128DA4 File Offset: 0x00126FA4
	private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 vector = projection.inverse * new Vector4(TasharenWater.SignExt(clipPlane.x), TasharenWater.SignExt(clipPlane.y), 1f, 1f);
		Vector4 vector2 = clipPlane * (2f / Vector4.Dot(clipPlane, vector));
		projection[2] = vector2.x - projection[3];
		projection[6] = vector2.y - projection[7];
		projection[10] = vector2.z - projection[11];
		projection[14] = vector2.w - projection[15];
	}

	// Token: 0x06002651 RID: 9809 RVA: 0x00128E54 File Offset: 0x00127054
	private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
	}

	// Token: 0x06002652 RID: 9810 RVA: 0x00128FFC File Offset: 0x001271FC
	public static TasharenWater.Quality GetQuality()
	{
		return (TasharenWater.Quality)PlayerPrefs.GetInt("Water", 3);
	}

	// Token: 0x06002653 RID: 9811 RVA: 0x00129018 File Offset: 0x00127218
	public static void SetQuality(TasharenWater.Quality q)
	{
		TasharenWater[] array = Object.FindObjectsOfType(typeof(TasharenWater)) as TasharenWater[];
		if (array.Length > 0)
		{
			foreach (TasharenWater tasharenWater in array)
			{
				tasharenWater.quality = q;
			}
		}
		else
		{
			PlayerPrefs.SetInt("Water", (int)q);
		}
	}

	// Token: 0x06002654 RID: 9812 RVA: 0x0001983E File Offset: 0x00017A3E
	private void Awake()
	{
		this.mTrans = base.transform;
		this.mRen = base.renderer;
		this.quality = TasharenWater.GetQuality();
	}

	// Token: 0x06002655 RID: 9813 RVA: 0x00129074 File Offset: 0x00127274
	private void OnDisable()
	{
		this.Clear();
		foreach (object obj in this.mCameras)
		{
			Object.DestroyImmediate(((Camera)((DictionaryEntry)obj).Value).gameObject);
		}
		this.mCameras.Clear();
	}

	// Token: 0x06002656 RID: 9814 RVA: 0x00019863 File Offset: 0x00017A63
	private void Clear()
	{
		if (this.mTex)
		{
			Object.DestroyImmediate(this.mTex);
			this.mTex = null;
		}
	}

	// Token: 0x06002657 RID: 9815 RVA: 0x001290F8 File Offset: 0x001272F8
	private void CopyCamera(Camera src, Camera dest)
	{
		if (src.clearFlags == 1)
		{
			Skybox component = src.GetComponent<Skybox>();
			Skybox component2 = dest.GetComponent<Skybox>();
			if (!component || !component.material)
			{
				component2.enabled = false;
			}
			else
			{
				component2.enabled = true;
				component2.material = component.material;
			}
		}
		dest.clearFlags = src.clearFlags;
		dest.backgroundColor = src.backgroundColor;
		dest.farClipPlane = src.farClipPlane;
		dest.nearClipPlane = src.nearClipPlane;
		dest.orthographic = src.orthographic;
		dest.fieldOfView = src.fieldOfView;
		dest.aspect = src.aspect;
		dest.orthographicSize = src.orthographicSize;
		dest.depthTextureMode = 0;
		dest.renderingPath = 1;
		dest.useOcclusionCulling = this.reflectionCamUseOC;
	}

	// Token: 0x06002658 RID: 9816 RVA: 0x001291E0 File Offset: 0x001273E0
	private Camera GetReflectionCamera(Camera current, Material mat, int textureSize)
	{
		if (!this.mTex || this.mTexSize != textureSize)
		{
			if (this.mTex)
			{
				Object.DestroyImmediate(this.mTex);
			}
			this.mTex = new RenderTexture(textureSize, textureSize, 16);
			this.mTex.name = "__MirrorReflection" + base.GetInstanceID();
			this.mTex.isPowerOfTwo = true;
			this.mTex.hideFlags = 4;
			this.mTexSize = textureSize;
		}
		Camera camera = this.mCameras[current] as Camera;
		if (!camera)
		{
			camera = new GameObject(string.Concat(new object[]
			{
				"Mirror Refl Camera id",
				base.GetInstanceID(),
				" for ",
				current.GetInstanceID()
			}), new Type[]
			{
				typeof(Camera),
				typeof(Skybox)
			})
			{
				hideFlags = 13
			}.camera;
			camera.enabled = false;
			Transform transform = camera.transform;
			transform.position = this.mTrans.position;
			transform.rotation = this.mTrans.rotation;
			camera.gameObject.AddComponent("FlareLayer");
			this.mCameras[current] = camera;
		}
		if (mat.HasProperty("_ReflectionTex"))
		{
			mat.SetTexture("_ReflectionTex", this.mTex);
		}
		return camera;
	}

	// Token: 0x06002659 RID: 9817 RVA: 0x0012936C File Offset: 0x0012756C
	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 vector = worldToCameraMatrix.MultiplyPoint(pos);
		Vector3 vector2 = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(vector2.x, vector2.y, vector2.z, -Vector3.Dot(vector, vector2));
	}

	// Token: 0x0600265A RID: 9818 RVA: 0x001293C4 File Offset: 0x001275C4
	private void LateUpdate()
	{
		if (this.keepUnderCamera)
		{
			Transform transform = Camera.main.transform;
			Vector3 position = transform.position;
			position.y = this.mTrans.position.y;
			if (this.mTrans.position != position)
			{
				this.mTrans.position = position;
			}
		}
	}

	// Token: 0x0600265B RID: 9819 RVA: 0x0012942C File Offset: 0x0012762C
	private void OnWillRenderObject()
	{
		if (TasharenWater.mIsRendering)
		{
			return;
		}
		if (!base.enabled || !this.mRen || !this.mRen.enabled)
		{
			this.Clear();
			return;
		}
		Material sharedMaterial = this.mRen.sharedMaterial;
		if (!sharedMaterial)
		{
			return;
		}
		Camera current = Camera.current;
		if (!current)
		{
			return;
		}
		bool supportsImageEffects = SystemInfo.supportsImageEffects;
		if (supportsImageEffects)
		{
			current.depthTextureMode |= 1;
		}
		else
		{
			this.quality = TasharenWater.Quality.Fastest;
		}
		if (!this.useRefraction)
		{
			sharedMaterial.shader.maximumLOD = ((!supportsImageEffects) ? 100 : 200);
			this.Clear();
			return;
		}
		LayerMask reflectionMask = this.reflectionMask;
		int reflectionTextureSize = this.reflectionTextureSize;
		if (reflectionMask == 0 || reflectionTextureSize < 512)
		{
			sharedMaterial.shader.maximumLOD = 300;
			this.Clear();
		}
		else
		{
			sharedMaterial.shader.maximumLOD = 400;
			TasharenWater.mIsRendering = true;
			Camera reflectionCamera = this.GetReflectionCamera(current, sharedMaterial, reflectionTextureSize);
			Vector3 position = this.mTrans.position;
			Vector3 up = this.mTrans.up;
			this.CopyCamera(current, reflectionCamera);
			float num = -Vector3.Dot(up, position);
			Vector4 plane;
			plane..ctor(up.x, up.y, up.z, num);
			Matrix4x4 zero = Matrix4x4.zero;
			TasharenWater.CalculateReflectionMatrix(ref zero, plane);
			Vector3 position2 = current.transform.position;
			Vector3 position3 = zero.MultiplyPoint(position2);
			reflectionCamera.worldToCameraMatrix = current.worldToCameraMatrix * zero;
			Vector4 clipPlane = this.CameraSpacePlane(reflectionCamera, position, up, 1f);
			Matrix4x4 projectionMatrix = current.projectionMatrix;
			TasharenWater.CalculateObliqueMatrix(ref projectionMatrix, clipPlane);
			reflectionCamera.projectionMatrix = projectionMatrix;
			reflectionCamera.cullingMask = (-17 & reflectionMask.value);
			reflectionCamera.targetTexture = this.mTex;
			GL.SetRevertBackfacing(true);
			reflectionCamera.transform.position = position3;
			Vector3 eulerAngles = current.transform.eulerAngles;
			reflectionCamera.transform.eulerAngles = new Vector3(0f, eulerAngles.y, eulerAngles.z);
			reflectionCamera.Render();
			reflectionCamera.transform.position = position2;
			GL.SetRevertBackfacing(false);
			TasharenWater.mIsRendering = false;
		}
	}

	// Token: 0x04002F18 RID: 12056
	public TasharenWater.Quality quality = TasharenWater.Quality.High;

	// Token: 0x04002F19 RID: 12057
	public LayerMask highReflectionMask = -1;

	// Token: 0x04002F1A RID: 12058
	public LayerMask mediumReflectionMask = -1;

	// Token: 0x04002F1B RID: 12059
	public bool reflectionCamUseOC = true;

	// Token: 0x04002F1C RID: 12060
	public bool keepUnderCamera = true;

	// Token: 0x04002F1D RID: 12061
	private Transform mTrans;

	// Token: 0x04002F1E RID: 12062
	private Hashtable mCameras = new Hashtable();

	// Token: 0x04002F1F RID: 12063
	private RenderTexture mTex;

	// Token: 0x04002F20 RID: 12064
	private int mTexSize;

	// Token: 0x04002F21 RID: 12065
	private Renderer mRen;

	// Token: 0x04002F22 RID: 12066
	private static bool mIsRendering;

	// Token: 0x0200060C RID: 1548
	public enum Quality
	{
		// Token: 0x04002F24 RID: 12068
		Fastest,
		// Token: 0x04002F25 RID: 12069
		Low,
		// Token: 0x04002F26 RID: 12070
		Medium,
		// Token: 0x04002F27 RID: 12071
		High,
		// Token: 0x04002F28 RID: 12072
		Uber
	}
}
