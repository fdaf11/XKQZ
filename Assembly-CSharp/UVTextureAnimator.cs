using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005EE RID: 1518
internal class UVTextureAnimator : MonoBehaviour
{
	// Token: 0x0600259B RID: 9627 RVA: 0x00019110 File Offset: 0x00017310
	private void Start()
	{
		this.InitMaterial();
		this.InitDefaultVariables();
		this.isInizialised = true;
		this.isVisible = true;
		base.StartCoroutine(this.UpdateCorutine());
	}

	// Token: 0x0600259C RID: 9628 RVA: 0x00019139 File Offset: 0x00017339
	public void SetInstanceMaterial(Material mat, Vector2 offsetMat)
	{
		this.instanceMaterial = mat;
		this.InitDefaultVariables();
	}

	// Token: 0x0600259D RID: 9629 RVA: 0x00123A2C File Offset: 0x00121C2C
	private void InitDefaultVariables()
	{
		this.allCount = 0;
		this.deltaFps = 1f / this.Fps;
		this.count = this.Rows * this.Columns;
		this.index = this.Columns - 1;
		Vector2 vector;
		vector..ctor((float)this.index / (float)this.Columns - (float)(this.index / this.Columns), 1f - (float)(this.index / this.Columns) / (float)this.Rows);
		this.OffsetMat = (this.IsRandomOffsetForInctance ? Random.Range(0, this.count) : (this.OffsetMat - this.OffsetMat / this.count * this.count));
		Vector2 vector2 = (!(this.SelfTiling == Vector2.zero)) ? this.SelfTiling : new Vector2(1f / (float)this.Columns, 1f / (float)this.Rows);
		if (this.AnimatedMaterialsNotInstance.Length > 0)
		{
			foreach (Material material in this.AnimatedMaterialsNotInstance)
			{
				material.SetTextureScale("_MainTex", vector2);
				material.SetTextureOffset("_MainTex", Vector2.zero);
				if (this.IsBump)
				{
					material.SetTextureScale("_BumpMap", vector2);
					material.SetTextureOffset("_BumpMap", Vector2.zero);
				}
				if (this.IsHeight)
				{
					material.SetTextureScale("_HeightMap", vector2);
					material.SetTextureOffset("_HeightMap", Vector2.zero);
				}
				if (this.IsCutOut)
				{
					material.SetTextureScale("_CutOut", vector2);
					material.SetTextureOffset("_CutOut", Vector2.zero);
				}
			}
		}
		else if (this.instanceMaterial != null)
		{
			this.instanceMaterial.SetTextureScale("_MainTex", vector2);
			this.instanceMaterial.SetTextureOffset("_MainTex", vector);
			if (this.IsBump)
			{
				this.instanceMaterial.SetTextureScale("_BumpMap", vector2);
				this.instanceMaterial.SetTextureOffset("_BumpMap", vector);
			}
			if (this.IsBump)
			{
				this.instanceMaterial.SetTextureScale("_HeightMap", vector2);
				this.instanceMaterial.SetTextureOffset("_HeightMap", vector);
			}
			if (this.IsCutOut)
			{
				this.instanceMaterial.SetTextureScale("_CutOut", vector2);
				this.instanceMaterial.SetTextureOffset("_CutOut", vector);
			}
		}
		else if (this.currentRenderer != null)
		{
			this.currentRenderer.material.SetTextureScale("_MainTex", vector2);
			this.currentRenderer.material.SetTextureOffset("_MainTex", vector);
			if (this.IsBump)
			{
				this.currentRenderer.material.SetTextureScale("_BumpMap", vector2);
				this.currentRenderer.material.SetTextureOffset("_BumpMap", vector);
			}
			if (this.IsHeight)
			{
				this.currentRenderer.material.SetTextureScale("_HeightMap", vector2);
				this.currentRenderer.material.SetTextureOffset("_HeightMap", vector);
			}
			if (this.IsCutOut)
			{
				this.currentRenderer.material.SetTextureScale("_CutOut", vector2);
				this.currentRenderer.material.SetTextureOffset("_CutOut", vector);
			}
		}
	}

	// Token: 0x0600259E RID: 9630 RVA: 0x00123D9C File Offset: 0x00121F9C
	private void InitMaterial()
	{
		if (base.renderer != null)
		{
			this.currentRenderer = base.renderer;
		}
		else
		{
			Projector component = base.GetComponent<Projector>();
			if (component != null)
			{
				if (!component.material.name.EndsWith("(Instance)"))
				{
					component.material = new Material(component.material)
					{
						name = component.material.name + " (Instance)"
					};
				}
				this.instanceMaterial = component.material;
			}
		}
	}

	// Token: 0x0600259F RID: 9631 RVA: 0x00019148 File Offset: 0x00017348
	private void OnEnable()
	{
		if (!this.isInizialised)
		{
			return;
		}
		this.InitDefaultVariables();
		this.isVisible = true;
		if (!this.isCorutineStarted)
		{
			base.StartCoroutine(this.UpdateCorutine());
		}
	}

	// Token: 0x060025A0 RID: 9632 RVA: 0x0001917B File Offset: 0x0001737B
	private void OnDisable()
	{
		this.isCorutineStarted = false;
		this.isVisible = false;
		base.StopAllCoroutines();
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x00019191 File Offset: 0x00017391
	private void OnBecameVisible()
	{
		this.isVisible = true;
		if (!this.isCorutineStarted)
		{
			base.StartCoroutine(this.UpdateCorutine());
		}
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x000191B2 File Offset: 0x000173B2
	private void OnBecameInvisible()
	{
		this.isVisible = false;
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x00123E34 File Offset: 0x00122034
	private IEnumerator UpdateCorutine()
	{
		this.isCorutineStarted = true;
		while (this.isVisible && (this.IsLoop || this.allCount != this.count))
		{
			this.UpdateCorutineFrame();
			if (!this.IsLoop && this.allCount == this.count)
			{
				break;
			}
			yield return new WaitForSeconds(this.deltaFps);
		}
		this.isCorutineStarted = false;
		yield break;
	}

	// Token: 0x060025A4 RID: 9636 RVA: 0x00123E50 File Offset: 0x00122050
	private void UpdateCorutineFrame()
	{
		if (this.currentRenderer == null && this.instanceMaterial == null && this.AnimatedMaterialsNotInstance.Length == 0)
		{
			return;
		}
		this.allCount++;
		if (this.IsReverse)
		{
			this.index--;
		}
		else
		{
			this.index++;
		}
		if (this.index >= this.count)
		{
			this.index = 0;
		}
		if (this.AnimatedMaterialsNotInstance.Length > 0)
		{
			for (int i = 0; i < this.AnimatedMaterialsNotInstance.Length; i++)
			{
				int num = i * this.OffsetMat + this.index + this.OffsetMat;
				num -= num / this.count * this.count;
				Vector2 vector;
				vector..ctor((float)num / (float)this.Columns - (float)(num / this.Columns), 1f - (float)(num / this.Columns) / (float)this.Rows);
				this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_MainTex", vector);
				if (this.IsBump)
				{
					this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_BumpMap", vector);
				}
				if (this.IsHeight)
				{
					this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_HeightMap", vector);
				}
				if (this.IsCutOut)
				{
					this.AnimatedMaterialsNotInstance[i].SetTextureOffset("_CutOut", vector);
				}
			}
		}
		else
		{
			Vector2 vector2;
			if (this.IsRandomOffsetForInctance)
			{
				int num2 = this.index + this.OffsetMat;
				vector2..ctor((float)num2 / (float)this.Columns - (float)(num2 / this.Columns), 1f - (float)(num2 / this.Columns) / (float)this.Rows);
			}
			else
			{
				vector2..ctor((float)this.index / (float)this.Columns - (float)(this.index / this.Columns), 1f - (float)(this.index / this.Columns) / (float)this.Rows);
			}
			if (this.instanceMaterial != null)
			{
				this.instanceMaterial.SetTextureOffset("_MainTex", vector2);
				if (this.IsBump)
				{
					this.instanceMaterial.SetTextureOffset("_BumpMap", vector2);
				}
				if (this.IsHeight)
				{
					this.instanceMaterial.SetTextureOffset("_HeightMap", vector2);
				}
				if (this.IsCutOut)
				{
					this.instanceMaterial.SetTextureOffset("_CutOut", vector2);
				}
			}
			else if (this.currentRenderer != null)
			{
				this.currentRenderer.material.SetTextureOffset("_MainTex", vector2);
				if (this.IsBump)
				{
					this.currentRenderer.material.SetTextureOffset("_BumpMap", vector2);
				}
				if (this.IsHeight)
				{
					this.currentRenderer.material.SetTextureOffset("_HeightMap", vector2);
				}
				if (this.IsCutOut)
				{
					this.currentRenderer.material.SetTextureOffset("_CutOut", vector2);
				}
			}
		}
	}

	// Token: 0x04002E1A RID: 11802
	public Material[] AnimatedMaterialsNotInstance;

	// Token: 0x04002E1B RID: 11803
	public int Rows = 4;

	// Token: 0x04002E1C RID: 11804
	public int Columns = 4;

	// Token: 0x04002E1D RID: 11805
	public float Fps = 20f;

	// Token: 0x04002E1E RID: 11806
	public int OffsetMat;

	// Token: 0x04002E1F RID: 11807
	public Vector2 SelfTiling = default(Vector2);

	// Token: 0x04002E20 RID: 11808
	public bool IsLoop = true;

	// Token: 0x04002E21 RID: 11809
	public bool IsReverse;

	// Token: 0x04002E22 RID: 11810
	public bool IsRandomOffsetForInctance;

	// Token: 0x04002E23 RID: 11811
	public bool IsBump;

	// Token: 0x04002E24 RID: 11812
	public bool IsHeight;

	// Token: 0x04002E25 RID: 11813
	public bool IsCutOut;

	// Token: 0x04002E26 RID: 11814
	private bool isInizialised;

	// Token: 0x04002E27 RID: 11815
	private int index;

	// Token: 0x04002E28 RID: 11816
	private int count;

	// Token: 0x04002E29 RID: 11817
	private int allCount;

	// Token: 0x04002E2A RID: 11818
	private float deltaFps;

	// Token: 0x04002E2B RID: 11819
	private bool isVisible;

	// Token: 0x04002E2C RID: 11820
	private bool isCorutineStarted;

	// Token: 0x04002E2D RID: 11821
	private Renderer currentRenderer;

	// Token: 0x04002E2E RID: 11822
	private Material instanceMaterial;
}
