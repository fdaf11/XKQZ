using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000412 RID: 1042
public class Imposter : MonoBehaviour
{
	// Token: 0x06001965 RID: 6501 RVA: 0x00010892 File Offset: 0x0000EA92
	private void Start()
	{
		if (ImposterManager.instance == null)
		{
			Debug.LogError("Can´t find ImposterManager!");
			return;
		}
		this.Init();
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x000108B5 File Offset: 0x0000EAB5
	public void ForceUpdate()
	{
		this.forceUpdate = true;
	}

	// Token: 0x06001967 RID: 6503 RVA: 0x000CD1D0 File Offset: 0x000CB3D0
	public void Init()
	{
		this.lastCameraVector = Vector3.zero;
		this.timeOffset = Random.Range(0f, this.updateInterval);
		if (!ImposterManager.instance.imposters.Contains(this))
		{
			ImposterManager.instance.imposters.Add(this);
		}
		if (this.proxy != null)
		{
			Object.DestroyImmediate(this.proxy.gameObject);
		}
		this.extractRenderer();
		this.createProxy();
		this.deactivate();
	}

	// Token: 0x06001968 RID: 6504 RVA: 0x000CD258 File Offset: 0x000CB458
	private void extractRenderer()
	{
		this.renderers.Clear();
		if (base.GetComponent<Renderer>() != null)
		{
			this.renderers.Add(base.GetComponent<Renderer>());
		}
		foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
		{
			this.renderers.Add(renderer);
		}
		foreach (Renderer renderer2 in this.renderers)
		{
			if (!this.originalRenderLayers.ContainsKey(renderer2))
			{
				this.originalRenderLayers.Add(renderer2, renderer2.gameObject.layer);
			}
		}
	}

	// Token: 0x06001969 RID: 6505 RVA: 0x000CD330 File Offset: 0x000CB530
	private void createProxy()
	{
		GameObject gameObject = (GameObject)Object.Instantiate(ImposterManager.instance.proxyPrefab.gameObject, base.transform.position, base.transform.rotation);
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localScale = new Vector3(1f / base.transform.lossyScale.x, 1f / base.transform.lossyScale.y, 1f / base.transform.lossyScale.z);
		gameObject.SetActive(true);
		this.proxy = gameObject.GetComponent<ImposterProxy>();
		this.proxy.shadowZOffset = this.shadowZOffset;
		this.proxy.zOffset = this.zOffset;
		this.proxy.shadowDivider = 1 << this.shadowDownSampling;
		this.proxy.maxShadowDistance = this.maxShadowDistance;
		this.proxy.Init(this.renderers, this.castShadow && ImposterManager.instance.castShadow);
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x000CD460 File Offset: 0x000CB660
	private void activate()
	{
		foreach (Renderer renderer in this.renderers)
		{
			renderer.enabled = false;
			renderer.gameObject.layer = ImposterManager.instance.imposterLayer;
		}
		this.proxy.gameObject.SetActive(true);
	}

	// Token: 0x0600196B RID: 6507 RVA: 0x000CD4E0 File Offset: 0x000CB6E0
	private void deactivate()
	{
		foreach (Renderer renderer in this.renderers)
		{
			renderer.enabled = true;
			int layer = 0;
			this.originalRenderLayers.TryGetValue(renderer, ref layer);
			renderer.gameObject.layer = layer;
		}
		this.proxy.gameObject.SetActive(false);
	}

	// Token: 0x0600196C RID: 6508 RVA: 0x000CD568 File Offset: 0x000CB768
	public void UpdateImposter()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (!ImposterManager.instance.active)
		{
			if (this.proxy.isActiveAndEnabled)
			{
				this.proxy.InvalidateTexture();
				this.deactivate();
			}
			return;
		}
		Vector3 vector = this.proxy.bound.center - ImposterManager.instance.mainCamera.transform.position;
		bool flag = this.IsInRange();
		bool flag2 = this.proxy.isVisible();
		bool flag3 = this.NeedsUpdate() || this.forceUpdate;
		if (flag || this.forceUpdate)
		{
			if (!this.proxy.isActiveAndEnabled && ImposterManager.instance.active)
			{
				this.activate();
			}
			if (flag2)
			{
				this.proxy.setVisibility(true);
				if (flag3 || this.proxy.IsTextureInvalid() || (Time.time + this.timeOffset - this.lastUpdateTime > this.updateInterval && this.dynamic))
				{
					this.forceUpdate = false;
					this.fitTexture();
					this.proxy.Render();
					this.lastCameraVector = vector;
					this.lastUpdateTime = Time.time + this.timeOffset;
				}
			}
			else
			{
				bool flag4 = this.proxy.isVisibleInCache();
				this.proxy.setVisibility(false);
				if (flag4 && ImposterManager.instance.cachingBehaviour == ImposterManager.CachingBehaviour.preloadAndCacheInvisibleImposters)
				{
					if ((flag3 || this.proxy.IsTextureInvalid()) && ImposterManager.instance.getPreloadLock(this))
					{
						this.forceUpdate = false;
						this.fitTexture();
						this.proxy.Render();
						this.lastCameraVector = vector;
						this.lastUpdateTime = Time.time;
					}
				}
				else if (ImposterManager.instance.cachingBehaviour == ImposterManager.CachingBehaviour.discardInvisibleImpostors || flag3 || !flag4)
				{
					this.proxy.InvalidateTexture();
				}
			}
		}
		else if (this.proxy.isActiveAndEnabled || (!ImposterManager.instance.active && this.proxy.isActiveAndEnabled))
		{
			this.proxy.InvalidateTexture();
			this.deactivate();
			return;
		}
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x000CD7B8 File Offset: 0x000CB9B8
	private bool NeedsUpdate()
	{
		Vector3 vector = this.proxy.bound.center - ImposterManager.instance.mainCamera.transform.position;
		float num = Vector3.Angle(vector, this.lastCameraVector);
		float num2 = Mathf.Abs(vector.magnitude - this.lastCameraVector.magnitude);
		return num > this.angleTolerance || num2 > this.lastCameraVector.magnitude * (this.distanceTolerance / 100f);
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x000CD844 File Offset: 0x000CBA44
	private bool IsInRange()
	{
		float num = Vector3.Distance(ImposterManager.instance.mainCamera.transform.position, this.proxy.bound.center);
		float num2 = this.proxy.maxSize / num * 57.29578f;
		this.pixelSize = num2 * (float)Screen.height / ImposterManager.instance.mainCamera.fieldOfView;
		bool result;
		if (this.lodMethod == Imposter.ImposterLodMethod.Distance)
		{
			result = (num > this.maxDistance);
		}
		else
		{
			result = (this.pixelSize < (float)this.maxTextureSize);
		}
		return result;
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x000CD8DC File Offset: 0x000CBADC
	private void fitTexture()
	{
		int num = Mathf.Min(this.maxTextureSize, this.nlpo2((int)this.pixelSize));
		if (this.proxy.IsTextureInvalid() || this.lastTextureSize != num)
		{
			this.proxy.AdjustTextureSize(num);
		}
		this.lastTextureSize = num;
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x000108BE File Offset: 0x0000EABE
	private int nlpo2(int x)
	{
		x--;
		x |= x >> 1;
		x |= x >> 2;
		x |= x >> 4;
		x |= x >> 8;
		x |= x >> 16;
		return x + 1;
	}

	// Token: 0x04001DEE RID: 7662
	public int maxTextureSize = 256;

	// Token: 0x04001DEF RID: 7663
	[HideInInspector]
	public Imposter.ImposterLodMethod lodMethod = Imposter.ImposterLodMethod.ScreenSize;

	// Token: 0x04001DF0 RID: 7664
	public float maxDistance = 30f;

	// Token: 0x04001DF1 RID: 7665
	public float angleTolerance = 2.5f;

	// Token: 0x04001DF2 RID: 7666
	public float distanceTolerance = 15f;

	// Token: 0x04001DF3 RID: 7667
	public float zOffset = 0.25f;

	// Token: 0x04001DF4 RID: 7668
	public bool castShadow;

	// Token: 0x04001DF5 RID: 7669
	public float shadowZOffset = 0.25f;

	// Token: 0x04001DF6 RID: 7670
	public float maxShadowDistance = 1f;

	// Token: 0x04001DF7 RID: 7671
	public int shadowDownSampling = 2;

	// Token: 0x04001DF8 RID: 7672
	public bool dynamic;

	// Token: 0x04001DF9 RID: 7673
	public float updateInterval = 1f;

	// Token: 0x04001DFA RID: 7674
	private float pixelSize;

	// Token: 0x04001DFB RID: 7675
	private int lastTextureSize;

	// Token: 0x04001DFC RID: 7676
	private ImposterProxy proxy;

	// Token: 0x04001DFD RID: 7677
	private Vector3 lastCameraVector = Vector3.zero;

	// Token: 0x04001DFE RID: 7678
	private List<Renderer> renderers = new List<Renderer>();

	// Token: 0x04001DFF RID: 7679
	private Dictionary<Renderer, int> originalRenderLayers = new Dictionary<Renderer, int>();

	// Token: 0x04001E00 RID: 7680
	private bool forceUpdate;

	// Token: 0x04001E01 RID: 7681
	private float lastUpdateTime;

	// Token: 0x04001E02 RID: 7682
	private float timeOffset;

	// Token: 0x02000413 RID: 1043
	public enum ImposterLodMethod
	{
		// Token: 0x04001E04 RID: 7684
		Distance,
		// Token: 0x04001E05 RID: 7685
		ScreenSize
	}
}
