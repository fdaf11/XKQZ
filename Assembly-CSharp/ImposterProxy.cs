using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000418 RID: 1048
public class ImposterProxy : MonoBehaviour
{
	// Token: 0x06001987 RID: 6535 RVA: 0x000CE0AC File Offset: 0x000CC2AC
	private void Awake()
	{
		this.imposterTexture = (this.shadowTexture = null);
	}

	// Token: 0x06001988 RID: 6536 RVA: 0x000CE0CC File Offset: 0x000CC2CC
	public void Init(List<Renderer> renderers, bool useShadow)
	{
		if (this.imposterTexture != null)
		{
			ImposterManager.instance.giveBackRenderTexture(this.imposterTexture);
		}
		if (this.shadowTexture != null)
		{
			ImposterManager.instance.giveBackRenderTexture(this.shadowTexture);
		}
		this.castShadow = useShadow;
		this.renderers = renderers;
		this.originalShader = new Shader[this.renderers.Count];
		this.renderingCamera = ImposterManager.instance.imposterRenderingCamera;
		this.imposterTexture = (this.shadowTexture = null);
		if (!useShadow)
		{
			this.shadow.gameObject.SetActive(false);
		}
		for (int i = 0; i < renderers.Count; i++)
		{
			this.originalShader[i] = renderers[i].material.shader;
		}
		this.extractBounds();
		this.adjustSize();
	}

	// Token: 0x06001989 RID: 6537 RVA: 0x0001099F File Offset: 0x0000EB9F
	private void OnDestroy()
	{
		this.InvalidateTexture();
	}

	// Token: 0x0600198A RID: 6538 RVA: 0x000CE1A8 File Offset: 0x000CC3A8
	public void AdjustTextureSize(int size)
	{
		if (this.imposterTexture != null && this.imposterTexture.size == size)
		{
			return;
		}
		if (this.imposterTexture != null)
		{
			ImposterManager.instance.giveBackRenderTexture(this.imposterTexture);
			if (this.castShadow)
			{
				ImposterManager.instance.giveBackRenderTexture(this.shadowTexture);
			}
		}
		this.imposterTexture = ImposterManager.instance.getRenderTexture(this, size);
		this.quad.material.mainTexture = this.imposterTexture.texture;
		this.quad.material.SetFloat("_ZOffset", this.zOffset);
		if (this.castShadow)
		{
			this.shadow.gameObject.SetActive(true);
			this.updateShadow(size);
		}
		else
		{
			this.shadow.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600198B RID: 6539 RVA: 0x000CE28C File Offset: 0x000CC48C
	private void updateShadow(int size)
	{
		this.shadowTexture = ImposterManager.instance.getRenderTexture(this, size / this.shadowDivider);
		this.shadow.material.mainTexture = this.shadowTexture.texture;
		this.shadow.transform.rotation = Quaternion.LookRotation(ImposterManager.instance.mainLight.transform.forward, Vector3.up);
		this.shadow.transform.position = base.transform.position - this.shadow.transform.forward * (this.shadowZOffset * this.maxSize / 2f);
		this.renderShadow();
	}

	// Token: 0x0600198C RID: 6540 RVA: 0x000CE34C File Offset: 0x000CC54C
	private void extractBounds()
	{
		if (base.transform.parent.GetComponent<Renderer>() != null)
		{
			this.bound = base.transform.parent.GetComponent<Renderer>().bounds;
		}
		foreach (Renderer renderer in base.transform.parent.gameObject.GetComponentsInChildren<Renderer>())
		{
			if (renderer != base.GetComponentInChildren<Renderer>())
			{
				if (this.bound.extents == Vector3.zero)
				{
					this.bound = renderer.bounds;
				}
				else
				{
					this.bound.Encapsulate(renderer.bounds);
				}
			}
		}
		this.shadowBound = this.bound;
		this.shadowBound.extents = this.shadowBound.extents * (1f + this.maxShadowDistance);
		this.maxSize = this.bound.extents.magnitude * 2f;
	}

	// Token: 0x0600198D RID: 6541 RVA: 0x000CE458 File Offset: 0x000CC658
	public void Render()
	{
		this.quad.transform.rotation = Quaternion.LookRotation(base.transform.position - ImposterManager.instance.mainCamera.transform.position, Vector3.up);
		bool fog = RenderSettings.fog;
		RenderSettings.fog = false;
		this.renderImposter();
		RenderSettings.fog = fog;
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x000CE4BC File Offset: 0x000CC6BC
	public void InvalidateTexture()
	{
		if (this.IsTextureInvalid())
		{
			return;
		}
		ImposterManager.instance.giveBackRenderTexture(this.imposterTexture);
		this.imposterTexture = null;
		if (this.castShadow)
		{
			ImposterManager.instance.giveBackRenderTexture(this.shadowTexture);
			this.shadowTexture = null;
		}
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x000109A7 File Offset: 0x0000EBA7
	public bool IsTextureInvalid()
	{
		return this.imposterTexture == null || this.imposterTexture.texture == null;
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x000CE510 File Offset: 0x000CC710
	private void adjustSize()
	{
		base.transform.position = this.bound.center;
		Transform parent = base.transform.parent;
		base.transform.SetParent(null);
		base.transform.localScale = new Vector3(this.maxSize, this.maxSize, this.maxSize);
		base.transform.SetParent(parent);
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x000CE57C File Offset: 0x000CC77C
	private void renderImposter()
	{
		if (this.imposterTexture == null)
		{
			return;
		}
		this.renderingCamera.transform.position = ImposterManager.instance.mainCamera.transform.position;
		this.renderingCamera.transform.LookAt(this.bound.center);
		this.renderingCamera.cullingMask = 1 << ImposterManager.instance.imposterLayer;
		Vector3 vector = ImposterManager.instance.mainCamera.transform.position - this.bound.center;
		this.renderingCamera.orthographic = false;
		float num = Vector3.Distance(ImposterManager.instance.mainCamera.transform.position, base.transform.position);
		float fieldOfView = 2f * Mathf.Atan(this.maxSize / (2f * num)) * 57.295776f;
		this.renderingCamera.fieldOfView = fieldOfView;
		this.renderingCamera.nearClipPlane = vector.magnitude - this.maxSize;
		this.renderingCamera.farClipPlane = vector.magnitude + this.maxSize;
		this.renderingCamera.targetTexture = this.imposterTexture.texture;
		foreach (Renderer renderer in this.renderers)
		{
			renderer.enabled = true;
		}
		this.renderingCamera.Render();
		foreach (Renderer renderer2 in this.renderers)
		{
			renderer2.enabled = false;
		}
		this.renderingCamera.targetTexture = null;
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x000CE76C File Offset: 0x000CC96C
	private void renderShadow()
	{
		if (this.shadowTexture == null)
		{
			return;
		}
		Vector3 forward = ImposterManager.instance.mainLight.transform.forward;
		this.renderingCamera.transform.position = base.transform.position - forward * (2f * this.maxSize + 1f);
		this.renderingCamera.transform.LookAt(this.bound.center);
		this.renderingCamera.cullingMask = 1 << ImposterManager.instance.imposterLayer;
		this.renderingCamera.orthographic = true;
		this.renderingCamera.orthographicSize = this.maxSize / 2f;
		this.renderingCamera.nearClipPlane = 1f;
		this.renderingCamera.farClipPlane = 4f * this.maxSize + 1f;
		this.renderingCamera.targetTexture = this.shadowTexture.texture;
		foreach (Renderer renderer in this.renderers)
		{
			renderer.enabled = true;
		}
		this.renderingCamera.Render();
		foreach (Renderer renderer2 in this.renderers)
		{
			renderer2.enabled = false;
		}
		this.renderingCamera.targetTexture = null;
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x000109CD File Offset: 0x0000EBCD
	public bool isVisible()
	{
		return (!this.castShadow) ? ImposterManager.instance.isVisible(this.bound) : ImposterManager.instance.isVisible(this.shadowBound);
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x000109FF File Offset: 0x0000EBFF
	public bool isVisibleInCache()
	{
		return (!this.castShadow) ? ImposterManager.instance.isVisibleInCache(this.bound) : ImposterManager.instance.isVisibleInCache(this.shadowBound);
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x000CE91C File Offset: 0x000CCB1C
	public void setVisibility(bool bVisible)
	{
		if (bVisible && !this.quad.enabled)
		{
			this.quad.enabled = true;
			if (this.castShadow)
			{
				this.shadow.enabled = true;
			}
		}
		else if (!bVisible && this.quad.enabled)
		{
			this.quad.enabled = false;
			this.shadow.enabled = false;
		}
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x000CE998 File Offset: 0x000CCB98
	private void OnEnable()
	{
		if (this.renderers == null)
		{
			return;
		}
		for (int i = 0; i < this.renderers.Count; i++)
		{
			if (!(this.renderers[i].material == null))
			{
				Dictionary<string, ImposterManager.ShadeReplacement> shaderReplacementMapping = ImposterManager.instance.shaderReplacementMapping;
				string name = this.renderers[i].material.shader.name;
				if (shaderReplacementMapping.ContainsKey(name))
				{
					this.renderers[i].material.shader = shaderReplacementMapping[name].replacement;
				}
			}
		}
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x000CEA48 File Offset: 0x000CCC48
	private void OnDisable()
	{
		for (int i = 0; i < this.renderers.Count; i++)
		{
			if (!(this.renderers[i].material == null))
			{
				if (this.originalShader[i] != this.renderers[i].material.shader)
				{
					this.renderers[i].material.shader = this.originalShader[i];
				}
			}
		}
	}

	// Token: 0x04001E2D RID: 7725
	[HideInInspector]
	public float maxSize;

	// Token: 0x04001E2E RID: 7726
	[HideInInspector]
	public Bounds bound;

	// Token: 0x04001E2F RID: 7727
	[HideInInspector]
	public Bounds shadowBound;

	// Token: 0x04001E30 RID: 7728
	[HideInInspector]
	public float shadowZOffset = 0.35f;

	// Token: 0x04001E31 RID: 7729
	[HideInInspector]
	public float zOffset = 0.35f;

	// Token: 0x04001E32 RID: 7730
	[HideInInspector]
	public float maxShadowDistance = 1f;

	// Token: 0x04001E33 RID: 7731
	[HideInInspector]
	public int shadowDivider = 1;

	// Token: 0x04001E34 RID: 7732
	public MeshRenderer quad;

	// Token: 0x04001E35 RID: 7733
	public MeshRenderer shadow;

	// Token: 0x04001E36 RID: 7734
	private ImposterTexture imposterTexture;

	// Token: 0x04001E37 RID: 7735
	private ImposterTexture shadowTexture;

	// Token: 0x04001E38 RID: 7736
	private Camera renderingCamera;

	// Token: 0x04001E39 RID: 7737
	private List<Renderer> renderers;

	// Token: 0x04001E3A RID: 7738
	private bool castShadow = true;

	// Token: 0x04001E3B RID: 7739
	private Shader[] originalShader;
}
