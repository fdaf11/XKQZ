using System;
using UnityEngine;

// Token: 0x020003D3 RID: 979
public class NcDetachParent : NcEffectBehaviour
{
	// Token: 0x06001746 RID: 5958 RVA: 0x0000F0D9 File Offset: 0x0000D2D9
	public void SetDestroyValue(bool bParentHideToStart, bool bStartDisableEmit, float fSmoothDestroyTime, bool bSmoothHide, bool bMeshFilterOnlySmoothHide)
	{
		this.m_bParentHideToStartDestroy = bParentHideToStart;
		this.m_bDisableEmit = bStartDisableEmit;
		this.m_bSmoothHide = bSmoothHide;
		this.m_fSmoothDestroyTime = fSmoothDestroyTime;
		this.m_bMeshFilterOnlySmoothHide = bMeshFilterOnlySmoothHide;
	}

	// Token: 0x06001747 RID: 5959 RVA: 0x0000F100 File Offset: 0x0000D300
	protected override void OnDestroy()
	{
		if (this.m_ncDetachObject != null)
		{
			Object.Destroy(this.m_ncDetachObject);
		}
		base.OnDestroy();
	}

	// Token: 0x06001748 RID: 5960 RVA: 0x000BFBF0 File Offset: 0x000BDDF0
	private void Update()
	{
		if (!this.m_bStartDetach)
		{
			this.m_bStartDetach = true;
			if (base.transform.parent != null)
			{
				this.m_ParentGameObject = base.transform.parent.gameObject;
				this.m_ncDetachObject = NcDetachObject.Create(this.m_ParentGameObject, base.transform.gameObject);
			}
			GameObject rootInstanceEffect = NcEffectBehaviour.GetRootInstanceEffect();
			if (this.m_bFollowParentTransform)
			{
				this.m_OriginalPos.SetLocalTransform(base.transform);
				base.ChangeParent(rootInstanceEffect.transform, base.transform, false, null);
				this.m_OriginalPos.CopyToLocalTransform(base.transform);
			}
			else
			{
				base.ChangeParent(rootInstanceEffect.transform, base.transform, false, null);
			}
			if (!this.m_bParentHideToStartDestroy)
			{
				this.StartDestroy();
			}
		}
		if (0f < this.m_fStartDestroyTime)
		{
			if (0f < this.m_fSmoothDestroyTime)
			{
				if (this.m_bSmoothHide)
				{
					float num = 1f - (NcEffectBehaviour.GetEngineTime() - this.m_fStartDestroyTime) / this.m_fSmoothDestroyTime;
					if (num < 0f)
					{
						num = 0f;
					}
					if (this.m_bMeshFilterOnlySmoothHide)
					{
						MeshFilter[] componentsInChildren = base.transform.GetComponentsInChildren<MeshFilter>(true);
						for (int i = 0; i < componentsInChildren.Length; i++)
						{
							Color[] array = componentsInChildren[i].mesh.colors;
							if (array.Length == 0)
							{
								array = new Color[componentsInChildren[i].mesh.vertices.Length];
								for (int j = 0; j < array.Length; j++)
								{
									array[j] = Color.white;
								}
							}
							for (int k = 0; k < array.Length; k++)
							{
								Color color = array[k];
								color.a = Mathf.Min(color.a, num);
								array[k] = color;
							}
							componentsInChildren[i].mesh.colors = array;
						}
					}
					else
					{
						foreach (Renderer renderer in base.transform.GetComponentsInChildren<Renderer>(true))
						{
							string materialColorName = NcEffectBehaviour.GetMaterialColorName(renderer.sharedMaterial);
							if (materialColorName != null)
							{
								Color color2 = renderer.material.GetColor(materialColorName);
								color2.a = Mathf.Min(color2.a, num);
								renderer.material.SetColor(materialColorName, color2);
							}
						}
					}
				}
				if (this.m_fStartDestroyTime + this.m_fSmoothDestroyTime < NcEffectBehaviour.GetEngineTime())
				{
					Object.Destroy(base.gameObject);
				}
			}
		}
		else if (this.m_bParentHideToStartDestroy && (this.m_ParentGameObject == null || !NcEffectBehaviour.IsActive(this.m_ParentGameObject)))
		{
			this.StartDestroy();
		}
		if (this.m_bFollowParentTransform && this.m_ParentGameObject != null && this.m_ParentGameObject.transform != null)
		{
			NcTransformTool ncTransformTool = new NcTransformTool();
			ncTransformTool.SetTransform(this.m_OriginalPos);
			ncTransformTool.AddTransform(this.m_ParentGameObject.transform);
			ncTransformTool.CopyToLocalTransform(base.transform);
		}
	}

	// Token: 0x06001749 RID: 5961 RVA: 0x0000F124 File Offset: 0x0000D324
	private void StartDestroy()
	{
		this.m_fStartDestroyTime = NcEffectBehaviour.GetEngineTime();
		if (this.m_bDisableEmit)
		{
			base.DisableEmit();
		}
	}

	// Token: 0x0600174A RID: 5962 RVA: 0x0000F142 File Offset: 0x0000D342
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fSmoothDestroyTime /= fSpeedRate;
	}

	// Token: 0x04001BA3 RID: 7075
	public bool m_bFollowParentTransform = true;

	// Token: 0x04001BA4 RID: 7076
	public bool m_bParentHideToStartDestroy = true;

	// Token: 0x04001BA5 RID: 7077
	public float m_fSmoothDestroyTime = 2f;

	// Token: 0x04001BA6 RID: 7078
	public bool m_bDisableEmit = true;

	// Token: 0x04001BA7 RID: 7079
	public bool m_bSmoothHide = true;

	// Token: 0x04001BA8 RID: 7080
	public bool m_bMeshFilterOnlySmoothHide;

	// Token: 0x04001BA9 RID: 7081
	protected bool m_bStartDetach;

	// Token: 0x04001BAA RID: 7082
	protected float m_fStartDestroyTime;

	// Token: 0x04001BAB RID: 7083
	protected GameObject m_ParentGameObject;

	// Token: 0x04001BAC RID: 7084
	protected NcDetachObject m_ncDetachObject;

	// Token: 0x04001BAD RID: 7085
	protected NcTransformTool m_OriginalPos = new NcTransformTool();
}
