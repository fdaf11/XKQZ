using System;
using UnityEngine;

// Token: 0x020005F2 RID: 1522
public class LineProjectileCollisionBehaviour : MonoBehaviour
{
	// Token: 0x060025B2 RID: 9650 RVA: 0x0012437C File Offset: 0x0012257C
	private void GetEffectSettingsComponent(Transform tr)
	{
		Transform parent = tr.parent;
		if (parent != null)
		{
			this.effectSettings = parent.GetComponentInChildren<EffectSettings>();
			if (this.effectSettings == null)
			{
				this.GetEffectSettingsComponent(parent.transform);
			}
		}
	}

	// Token: 0x060025B3 RID: 9651 RVA: 0x001243C8 File Offset: 0x001225C8
	private void Start()
	{
		this.t = base.transform;
		if (this.EffectOnHit != null)
		{
			this.tEffectOnHit = this.EffectOnHit.transform;
			this.effectOnHitParticles = this.EffectOnHit.GetComponentsInChildren<ParticleSystem>();
		}
		if (this.ParticlesScale != null)
		{
			this.tParticleScale = this.ParticlesScale.transform;
		}
		this.GetEffectSettingsComponent(this.t);
		if (this.effectSettings == null)
		{
			Debug.Log("Prefab root or children have not script \"PrefabSettings\"");
		}
		if (this.GoLight != null)
		{
			this.tLight = this.GoLight.transform;
		}
		this.InitializeDefault();
		this.isInitializedOnStart = true;
	}

	// Token: 0x060025B4 RID: 9652 RVA: 0x000191F8 File Offset: 0x000173F8
	private void OnEnable()
	{
		if (this.isInitializedOnStart)
		{
			this.InitializeDefault();
		}
	}

	// Token: 0x060025B5 RID: 9653 RVA: 0x0001920B File Offset: 0x0001740B
	private void OnDisable()
	{
		this.CollisionLeave();
	}

	// Token: 0x060025B6 RID: 9654 RVA: 0x0012448C File Offset: 0x0012268C
	private void InitializeDefault()
	{
		this.hit = default(RaycastHit);
		this.frameDroped = false;
	}

	// Token: 0x060025B7 RID: 9655 RVA: 0x001244B0 File Offset: 0x001226B0
	private void Update()
	{
		if (!this.frameDroped)
		{
			this.frameDroped = true;
			return;
		}
		Vector3 vector = this.t.position + this.t.forward * this.effectSettings.MoveDistance;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.t.position, this.t.forward, ref raycastHit, this.effectSettings.MoveDistance + 1f, this.effectSettings.LayerMask))
		{
			this.hit = raycastHit;
			vector = raycastHit.point;
			if (this.oldRaycastHit.collider != this.hit.collider)
			{
				this.CollisionLeave();
				this.oldRaycastHit = this.hit;
				this.CollisionEnter();
				if (this.EffectOnHit != null)
				{
					foreach (ParticleSystem particleSystem in this.effectOnHitParticles)
					{
						particleSystem.Play();
					}
				}
			}
			if (this.EffectOnHit != null)
			{
				this.tEffectOnHit.position = this.hit.point - this.t.forward * this.effectSettings.ColliderRadius;
			}
		}
		else if (this.EffectOnHit != null)
		{
			foreach (ParticleSystem particleSystem2 in this.effectOnHitParticles)
			{
				particleSystem2.Stop();
			}
		}
		if (this.EffectOnHit != null)
		{
			this.tEffectOnHit.LookAt(this.hit.point + this.hit.normal);
		}
		if (this.IsCenterLightPosition && this.GoLight != null)
		{
			this.tLight.position = (this.t.position + vector) / 2f;
		}
		foreach (LineRenderer lineRenderer in this.LineRenderers)
		{
			lineRenderer.SetPosition(0, vector);
			lineRenderer.SetPosition(1, this.t.position);
		}
		if (this.ParticlesScale != null)
		{
			float num = Vector3.Distance(this.t.position, vector) / 2f;
			this.tParticleScale.localScale = new Vector3(num, 1f, 1f);
		}
	}

	// Token: 0x060025B8 RID: 9656 RVA: 0x00124750 File Offset: 0x00122950
	private void CollisionEnter()
	{
		if (this.EffectOnHitObject != null && this.hit.transform != null)
		{
			AddMaterialOnHit componentInChildren = this.hit.transform.GetComponentInChildren<AddMaterialOnHit>();
			this.effectSettingsInstance = null;
			if (componentInChildren != null)
			{
				this.effectSettingsInstance = componentInChildren.gameObject.GetComponent<EffectSettings>();
			}
			if (this.effectSettingsInstance != null)
			{
				this.effectSettingsInstance.IsVisible = true;
			}
			else
			{
				Transform transform = this.hit.transform;
				Renderer componentInChildren2 = transform.GetComponentInChildren<Renderer>();
				GameObject gameObject = Object.Instantiate(this.EffectOnHitObject) as GameObject;
				gameObject.transform.parent = componentInChildren2.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.GetComponent<AddMaterialOnHit>().UpdateMaterial(this.hit);
				this.effectSettingsInstance = gameObject.GetComponent<EffectSettings>();
			}
		}
		this.effectSettings.OnCollisionHandler(new CollisionInfo
		{
			Hit = this.hit
		});
	}

	// Token: 0x060025B9 RID: 9657 RVA: 0x00019213 File Offset: 0x00017413
	private void CollisionLeave()
	{
		if (this.effectSettingsInstance != null)
		{
			this.effectSettingsInstance.IsVisible = false;
		}
	}

	// Token: 0x04002E36 RID: 11830
	public GameObject EffectOnHit;

	// Token: 0x04002E37 RID: 11831
	public GameObject EffectOnHitObject;

	// Token: 0x04002E38 RID: 11832
	public GameObject ParticlesScale;

	// Token: 0x04002E39 RID: 11833
	public GameObject GoLight;

	// Token: 0x04002E3A RID: 11834
	public bool IsCenterLightPosition;

	// Token: 0x04002E3B RID: 11835
	public LineRenderer[] LineRenderers;

	// Token: 0x04002E3C RID: 11836
	private EffectSettings effectSettings;

	// Token: 0x04002E3D RID: 11837
	private Transform t;

	// Token: 0x04002E3E RID: 11838
	private Transform tLight;

	// Token: 0x04002E3F RID: 11839
	private Transform tEffectOnHit;

	// Token: 0x04002E40 RID: 11840
	private Transform tParticleScale;

	// Token: 0x04002E41 RID: 11841
	private RaycastHit hit;

	// Token: 0x04002E42 RID: 11842
	private RaycastHit oldRaycastHit;

	// Token: 0x04002E43 RID: 11843
	private bool isInitializedOnStart;

	// Token: 0x04002E44 RID: 11844
	private bool frameDroped;

	// Token: 0x04002E45 RID: 11845
	private ParticleSystem[] effectOnHitParticles;

	// Token: 0x04002E46 RID: 11846
	private EffectSettings effectSettingsInstance;
}
