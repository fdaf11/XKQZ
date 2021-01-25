using System;
using UnityEngine;

// Token: 0x020005ED RID: 1517
public class SetPositionOnHit : MonoBehaviour
{
	// Token: 0x06002595 RID: 9621 RVA: 0x00123918 File Offset: 0x00121B18
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

	// Token: 0x06002596 RID: 9622 RVA: 0x00019099 File Offset: 0x00017299
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings == null)
		{
			Debug.Log("Prefab root or children have not script \"PrefabSettings\"");
		}
		this.tRoot = this.effectSettings.transform;
	}

	// Token: 0x06002597 RID: 9623 RVA: 0x00123964 File Offset: 0x00121B64
	private void effectSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		Vector3 normalized = (this.tRoot.position + Vector3.Normalize(e.Hit.point - this.tRoot.position) * (this.effectSettings.MoveDistance + 1f)).normalized;
		base.transform.position = e.Hit.point - normalized * this.OffsetPosition;
	}

	// Token: 0x06002598 RID: 9624 RVA: 0x000190D3 File Offset: 0x000172D3
	private void Update()
	{
		if (!this.isInitialized)
		{
			this.isInitialized = true;
			this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.effectSettings_CollisionEnter);
		}
	}

	// Token: 0x06002599 RID: 9625 RVA: 0x000190FE File Offset: 0x000172FE
	private void OnDisable()
	{
		base.transform.position = Vector3.zero;
	}

	// Token: 0x04002E16 RID: 11798
	public float OffsetPosition;

	// Token: 0x04002E17 RID: 11799
	private EffectSettings effectSettings;

	// Token: 0x04002E18 RID: 11800
	private Transform tRoot;

	// Token: 0x04002E19 RID: 11801
	private bool isInitialized;
}
