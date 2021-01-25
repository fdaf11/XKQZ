using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000414 RID: 1044
public class ImposterManager : MonoBehaviour
{
	// Token: 0x06001972 RID: 6514 RVA: 0x000CD9A4 File Offset: 0x000CBBA4
	private void Awake()
	{
		ImposterManager.instance = this;
		this.lastCastShadow = this.castShadow;
		if (this.useMainCamera)
		{
			this.mainCamera = Camera.main;
		}
		this.lastMainCamera = this.mainCamera;
		for (int i = 0; i < this.shaderReplacement.Length; i++)
		{
			this.shaderReplacementMapping.Add(this.shaderReplacement[i].original.name, this.shaderReplacement[i]);
		}
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x000108EC File Offset: 0x0000EAEC
	private void Start()
	{
		this.SetupCachingCamera();
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x000108F4 File Offset: 0x0000EAF4
	public void ForceImposterUpdate()
	{
		this.ForceImposterUpdate(int.MaxValue);
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x00010901 File Offset: 0x0000EB01
	public void ForceImposterUpdate(int maxUpdatesPerFrame)
	{
		base.StartCoroutine(this.forceUpdate(maxUpdatesPerFrame));
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x000CDA30 File Offset: 0x000CBC30
	private IEnumerator forceUpdate(int maxUpdates)
	{
		int i = 0;
		while (i < this.imposters.Count)
		{
			int updatesThisFrame = 0;
			while (updatesThisFrame < maxUpdates && i < this.imposters.Count)
			{
				this.imposters[i].ForceUpdate();
				updatesThisFrame++;
				i++;
			}
			yield return new WaitForEndOfFrame();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x000CDA5C File Offset: 0x000CBC5C
	private void SetupCachingCamera()
	{
		if (this.mainCamera == null)
		{
			return;
		}
		if (this.cachingCamera != null)
		{
			Object.Destroy(this.cachingCamera.gameObject);
		}
		GameObject gameObject = new GameObject("Imposter Caching Camera");
		this.cachingCamera = gameObject.AddComponent<Camera>();
		this.cachingCamera.orthographic = ImposterManager.instance.mainCamera.orthographic;
		this.cachingCamera.orthographicSize = ImposterManager.instance.mainCamera.orthographicSize;
		this.cachingCamera.fieldOfView = ImposterManager.instance.mainCamera.fieldOfView * this.preLoadFovFactor;
		this.cachingCamera.nearClipPlane = ImposterManager.instance.mainCamera.nearClipPlane;
		this.cachingCamera.farClipPlane = ImposterManager.instance.mainCamera.farClipPlane;
		this.cachingCamera.transform.position = ImposterManager.instance.mainCamera.transform.position;
		this.cachingCamera.transform.rotation = ImposterManager.instance.mainCamera.transform.rotation;
		this.cachingCamera.enabled = false;
		gameObject.transform.SetParent(ImposterManager.instance.mainCamera.transform);
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x000CDBAC File Offset: 0x000CBDAC
	private void LateUpdate()
	{
		if (this.useMainCamera)
		{
			this.mainCamera = Camera.main;
		}
		if (this.mainCamera == null)
		{
			return;
		}
		if (this.lastMainCamera != this.mainCamera)
		{
			this.lastMainCamera = this.mainCamera;
			this.SetupCachingCamera();
		}
		this.preloadCounter = 0;
		this.cameraPlanes = GeometryUtility.CalculateFrustumPlanes(ImposterManager.instance.mainCamera);
		this.cachingCameraPlanes = GeometryUtility.CalculateFrustumPlanes(this.cachingCamera);
		this.castShadow = (this.castShadow && this.mainLight != null && this.mainLight.enabled && this.mainLight.type == 1 && this.mainLight.shadows != 0);
		foreach (Imposter imposter in this.imposters)
		{
			if (imposter != null)
			{
				imposter.UpdateImposter();
			}
			else
			{
				this.invalidImposters.Add(imposter);
			}
		}
		if (this.invalidImposters.Count > 0)
		{
			for (int i = 0; i < this.invalidImposters.Count; i++)
			{
				this.imposters.Remove(this.invalidImposters[i]);
			}
			this.invalidImposters.Clear();
		}
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x00010911 File Offset: 0x0000EB11
	public bool isVisible(Bounds bound)
	{
		return GeometryUtility.TestPlanesAABB(this.cameraPlanes, bound);
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x0001091F File Offset: 0x0000EB1F
	public bool isVisibleInCache(Bounds bound)
	{
		return GeometryUtility.TestPlanesAABB(this.cachingCameraPlanes, bound);
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x000CDD48 File Offset: 0x000CBF48
	public ImposterTexture getRenderTexture(ImposterProxy newOwner, int size)
	{
		for (int i = 0; i < this.freeImposterTextures.Count; i++)
		{
			ImposterTexture imposterTexture = this.freeImposterTextures[i];
			if (imposterTexture.size == size)
			{
				this.freeImposterTextures.Remove(imposterTexture);
				imposterTexture.owner = newOwner;
				return imposterTexture;
			}
		}
		RenderTexture renderTexture = new RenderTexture(size, size, 16);
		renderTexture.antiAliasing = this.antialiasing;
		ImposterTexture imposterTexture2 = new ImposterTexture();
		imposterTexture2.owner = newOwner;
		imposterTexture2.size = size;
		imposterTexture2.createdTime = (imposterTexture2.lastUsedTime = Time.frameCount);
		imposterTexture2.texture = renderTexture;
		this.imposterTextures.Add(imposterTexture2);
		this.textureMemory += imposterTexture2.getMemoryAmount();
		return imposterTexture2;
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x0001092D File Offset: 0x0000EB2D
	public void giveBackRenderTexture(ImposterTexture imposterTexture)
	{
		if (imposterTexture.texture == null)
		{
			Debug.LogError("Texture given back is empty!");
			return;
		}
		this.freeImposterTextures.Add(imposterTexture);
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x000CDE08 File Offset: 0x000CC008
	public void garbageCollect()
	{
		foreach (ImposterTexture imposterTexture in this.freeImposterTextures)
		{
			this.imposterTextures.Remove(imposterTexture);
			this.textureMemory -= imposterTexture.getMemoryAmount();
			Object.Destroy(imposterTexture.texture);
		}
		this.freeImposterTextures.Clear();
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x000CDE94 File Offset: 0x000CC094
	public bool getPreloadLock(Imposter imposter)
	{
		return this.preloadCounter++ < this.preloadRate;
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x000CDEC0 File Offset: 0x000CC0C0
	private void Clear()
	{
		foreach (Imposter imposter in this.imposters)
		{
			if (imposter != null)
			{
				Object.Destroy(imposter.gameObject);
			}
		}
		foreach (ImposterTexture imposterTexture in this.imposterTextures)
		{
			this.textureMemory -= imposterTexture.getMemoryAmount();
			Object.Destroy(imposterTexture.texture);
		}
		this.imposters.Clear();
		this.imposterTextures.Clear();
		this.freeImposterTextures.Clear();
	}

	// Token: 0x04001E06 RID: 7686
	public static ImposterManager instance;

	// Token: 0x04001E07 RID: 7687
	public bool active = true;

	// Token: 0x04001E08 RID: 7688
	public int imposterLayer = 30;

	// Token: 0x04001E09 RID: 7689
	public bool useMainCamera = true;

	// Token: 0x04001E0A RID: 7690
	public Camera mainCamera;

	// Token: 0x04001E0B RID: 7691
	public int antialiasing = 2;

	// Token: 0x04001E0C RID: 7692
	public bool castShadow;

	// Token: 0x04001E0D RID: 7693
	public Light mainLight;

	// Token: 0x04001E0E RID: 7694
	public ImposterManager.CachingBehaviour cachingBehaviour;

	// Token: 0x04001E0F RID: 7695
	public float preLoadFovFactor = 2f;

	// Token: 0x04001E10 RID: 7696
	public int preloadRate = 5;

	// Token: 0x04001E11 RID: 7697
	public List<Imposter> imposters;

	// Token: 0x04001E12 RID: 7698
	public List<ImposterTexture> imposterTextures = new List<ImposterTexture>();

	// Token: 0x04001E13 RID: 7699
	public List<ImposterTexture> freeImposterTextures = new List<ImposterTexture>();

	// Token: 0x04001E14 RID: 7700
	public float textureMemory;

	// Token: 0x04001E15 RID: 7701
	public ImposterProxy proxyPrefab;

	// Token: 0x04001E16 RID: 7702
	public Camera imposterRenderingCamera;

	// Token: 0x04001E17 RID: 7703
	public ImposterManager.ShadeReplacement[] shaderReplacement;

	// Token: 0x04001E18 RID: 7704
	public Dictionary<string, ImposterManager.ShadeReplacement> shaderReplacementMapping = new Dictionary<string, ImposterManager.ShadeReplacement>();

	// Token: 0x04001E19 RID: 7705
	private Plane[] cameraPlanes;

	// Token: 0x04001E1A RID: 7706
	private Plane[] cachingCameraPlanes;

	// Token: 0x04001E1B RID: 7707
	private bool lastCastShadow;

	// Token: 0x04001E1C RID: 7708
	private List<Imposter> invalidImposters = new List<Imposter>();

	// Token: 0x04001E1D RID: 7709
	private Camera cachingCamera;

	// Token: 0x04001E1E RID: 7710
	private int preloadCounter;

	// Token: 0x04001E1F RID: 7711
	private Camera lastMainCamera;

	// Token: 0x02000415 RID: 1045
	[Serializable]
	public struct ShadeReplacement
	{
		// Token: 0x04001E20 RID: 7712
		public Shader original;

		// Token: 0x04001E21 RID: 7713
		public Shader replacement;
	}

	// Token: 0x02000416 RID: 1046
	public enum CachingBehaviour
	{
		// Token: 0x04001E23 RID: 7715
		discardInvisibleImpostors,
		// Token: 0x04001E24 RID: 7716
		cacheInvisibleIposters,
		// Token: 0x04001E25 RID: 7717
		preloadAndCacheInvisibleImposters
	}
}
