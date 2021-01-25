using System;
using UnityEngine;

// Token: 0x020005CF RID: 1487
public class SetRandomStartPoint : MonoBehaviour
{
	// Token: 0x060024F4 RID: 9460 RVA: 0x00120990 File Offset: 0x0011EB90
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

	// Token: 0x060024F5 RID: 9461 RVA: 0x001209DC File Offset: 0x0011EBDC
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings == null)
		{
			Debug.Log("Prefab root or children have not script \"PrefabSettings\"");
		}
		this.tRoot = this.effectSettings.transform;
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	// Token: 0x060024F6 RID: 9462 RVA: 0x000187E7 File Offset: 0x000169E7
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	// Token: 0x060024F7 RID: 9463 RVA: 0x00120A30 File Offset: 0x0011EC30
	private void InitDefaultVariables()
	{
		if (base.particleSystem != null)
		{
			base.particleSystem.Stop();
		}
		Vector3 position = this.effectSettings.Target.transform.position;
		Vector3 vector;
		vector..ctor(position.x, this.Height, position.z);
		float num = Random.Range(0f, this.RandomRange.x * 200f) / 100f - this.RandomRange.x;
		float num2 = Random.Range(0f, this.RandomRange.y * 200f) / 100f - this.RandomRange.y;
		float num3 = Random.Range(0f, this.RandomRange.z * 200f) / 100f - this.RandomRange.z;
		Vector3 position2;
		position2..ctor(vector.x + num, vector.y + num2, vector.z + num3);
		if (this.StartPointGo == null)
		{
			this.tRoot.position = position2;
		}
		else
		{
			this.StartPointGo.transform.position = position2;
		}
		if (base.particleSystem != null)
		{
			base.particleSystem.Play();
		}
	}

	// Token: 0x060024F8 RID: 9464 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04002D1C RID: 11548
	public Vector3 RandomRange;

	// Token: 0x04002D1D RID: 11549
	public GameObject StartPointGo;

	// Token: 0x04002D1E RID: 11550
	public float Height = 10f;

	// Token: 0x04002D1F RID: 11551
	private EffectSettings effectSettings;

	// Token: 0x04002D20 RID: 11552
	private bool isInitialized;

	// Token: 0x04002D21 RID: 11553
	private Transform tRoot;
}
