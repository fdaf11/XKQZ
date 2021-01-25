using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005E5 RID: 1509
public class InvokeInterval : MonoBehaviour
{
	// Token: 0x0600256B RID: 9579 RVA: 0x00123088 File Offset: 0x00121288
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

	// Token: 0x0600256C RID: 9580 RVA: 0x001230D4 File Offset: 0x001212D4
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		this.goInstances = new List<GameObject>();
		this.count = (int)(this.Duration / this.Interval);
		for (int i = 0; i < this.count; i++)
		{
			GameObject gameObject = Object.Instantiate(this.GO, base.transform.position, default(Quaternion)) as GameObject;
			gameObject.transform.parent = base.transform;
			EffectSettings component = gameObject.GetComponent<EffectSettings>();
			component.Target = this.effectSettings.Target;
			component.IsHomingMove = this.effectSettings.IsHomingMove;
			component.MoveDistance = this.effectSettings.MoveDistance;
			component.MoveSpeed = this.effectSettings.MoveSpeed;
			component.CollisionEnter += delegate(object n, CollisionInfo e)
			{
				this.effectSettings.OnCollisionHandler(e);
			};
			component.ColliderRadius = this.effectSettings.ColliderRadius;
			component.EffectRadius = this.effectSettings.EffectRadius;
			component.EffectDeactivated += new EventHandler(this.effectSettings_EffectDeactivated);
			this.goInstances.Add(gameObject);
			gameObject.SetActive(false);
		}
		this.InvokeAll();
		this.isInitialized = true;
	}

	// Token: 0x0600256D RID: 9581 RVA: 0x0012320C File Offset: 0x0012140C
	private void InvokeAll()
	{
		for (int i = 0; i < this.count; i++)
		{
			base.Invoke("InvokeInstance", (float)i * this.Interval);
		}
	}

	// Token: 0x0600256E RID: 9582 RVA: 0x00123244 File Offset: 0x00121444
	private void InvokeInstance()
	{
		this.goInstances[this.goIndexActivate].SetActive(true);
		if (this.goIndexActivate >= this.goInstances.Count - 1)
		{
			this.goIndexActivate = 0;
		}
		else
		{
			this.goIndexActivate++;
		}
	}

	// Token: 0x0600256F RID: 9583 RVA: 0x0012329C File Offset: 0x0012149C
	private void effectSettings_EffectDeactivated(object sender, EventArgs e)
	{
		EffectSettings effectSettings = sender as EffectSettings;
		effectSettings.transform.position = base.transform.position;
		if (this.goIndexDeactivate >= this.count - 1)
		{
			this.effectSettings.Deactivate();
			this.goIndexDeactivate = 0;
		}
		else
		{
			this.goIndexDeactivate++;
		}
	}

	// Token: 0x06002570 RID: 9584 RVA: 0x00018E93 File Offset: 0x00017093
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InvokeAll();
		}
	}

	// Token: 0x06002571 RID: 9585 RVA: 0x0000264F File Offset: 0x0000084F
	private void OnDisable()
	{
	}

	// Token: 0x04002DE3 RID: 11747
	public GameObject GO;

	// Token: 0x04002DE4 RID: 11748
	public float Interval = 0.3f;

	// Token: 0x04002DE5 RID: 11749
	public float Duration = 3f;

	// Token: 0x04002DE6 RID: 11750
	private List<GameObject> goInstances;

	// Token: 0x04002DE7 RID: 11751
	private EffectSettings effectSettings;

	// Token: 0x04002DE8 RID: 11752
	private int goIndexActivate;

	// Token: 0x04002DE9 RID: 11753
	private int goIndexDeactivate;

	// Token: 0x04002DEA RID: 11754
	private bool isInitialized;

	// Token: 0x04002DEB RID: 11755
	private int count;
}
