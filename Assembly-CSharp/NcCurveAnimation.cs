using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003CE RID: 974
public class NcCurveAnimation : NcEffectAniBehaviour
{
	// Token: 0x06001710 RID: 5904 RVA: 0x000BE3D8 File Offset: 0x000BC5D8
	public override int GetAnimationState()
	{
		if (!base.enabled || !NcEffectBehaviour.IsActive(base.gameObject))
		{
			return -1;
		}
		if (0f < this.m_fDurationTime && (this.m_fStartTime == 0f || !base.IsEndAnimation()))
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06001711 RID: 5905 RVA: 0x0000EEE6 File Offset: 0x0000D0E6
	public void ResetPosition()
	{
		this.m_NcTansform = new NcTransformTool(base.transform);
	}

	// Token: 0x06001712 RID: 5906 RVA: 0x000BE430 File Offset: 0x000BC630
	public override void ResetAnimation()
	{
		this.m_NcTansform.CopyToLocalTransform(base.transform);
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime() - this.m_fAddElapsedTime;
		this.m_Transform = null;
		this.m_ChildRenderers = null;
		this.m_ChildColorNames = null;
		base.ResetAnimation();
		if (0f < this.m_fDelayTime)
		{
			this.m_Timer = null;
		}
		this.InitAnimation();
		this.UpdateAnimation(0f);
	}

	// Token: 0x06001713 RID: 5907 RVA: 0x0000EEF9 File Offset: 0x0000D0F9
	public void AdjustfElapsedTime(float fAddStartTime)
	{
		this.m_fAddElapsedTime = fAddStartTime;
	}

	// Token: 0x06001714 RID: 5908 RVA: 0x0000EF02 File Offset: 0x0000D102
	public float GetRepeatedRate()
	{
		return this.m_fElapsedRate;
	}

	// Token: 0x06001715 RID: 5909 RVA: 0x0000EF0A File Offset: 0x0000D10A
	private void Awake()
	{
		this.ResetPosition();
	}

	// Token: 0x06001716 RID: 5910 RVA: 0x000BE4A4 File Offset: 0x000BC6A4
	private void Start()
	{
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime() - this.m_fAddElapsedTime;
		this.InitAnimation();
		if (0f < this.m_fDelayTime)
		{
			if (base.renderer)
			{
				base.renderer.enabled = false;
			}
		}
		else
		{
			base.InitAnimationTimer();
			this.UpdateAnimation(0f);
		}
	}

	// Token: 0x06001717 RID: 5911 RVA: 0x000BE50C File Offset: 0x000BC70C
	private void LateUpdate()
	{
		if (this.m_fStartTime == 0f)
		{
			return;
		}
		if (!base.IsStartAnimation() && this.m_fDelayTime != 0f)
		{
			if (NcEffectBehaviour.GetEngineTime() < this.m_fStartTime + this.m_fDelayTime)
			{
				return;
			}
			base.InitAnimationTimer();
			if (base.renderer)
			{
				base.renderer.enabled = true;
			}
		}
		float num = this.m_Timer.GetTime() + this.m_fAddElapsedTime;
		float fElapsedRate = num;
		if (this.m_fDurationTime != 0f)
		{
			fElapsedRate = num / this.m_fDurationTime;
		}
		this.UpdateAnimation(fElapsedRate);
	}

	// Token: 0x06001718 RID: 5912 RVA: 0x000BE5B4 File Offset: 0x000BC7B4
	private void InitAnimation()
	{
		if (this.m_Transform != null)
		{
			return;
		}
		this.m_fElapsedRate = 0f;
		this.m_Transform = base.transform;
		foreach (NcCurveAnimation.NcInfoCurve ncInfoCurve in this.m_CurveInfoList)
		{
			if (ncInfoCurve.m_bEnabled)
			{
				switch (ncInfoCurve.m_ApplyType)
				{
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.POSITION:
					ncInfoCurve.m_OriginalValue = Vector4.zero;
					ncInfoCurve.m_BeforeValue = ncInfoCurve.m_OriginalValue;
					break;
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.ROTATION:
					ncInfoCurve.m_OriginalValue = Vector4.zero;
					ncInfoCurve.m_BeforeValue = ncInfoCurve.m_OriginalValue;
					break;
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.SCALE:
					ncInfoCurve.m_OriginalValue = this.m_Transform.localScale;
					ncInfoCurve.m_BeforeValue = ncInfoCurve.m_OriginalValue;
					break;
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MATERIAL_COLOR:
					if (ncInfoCurve.m_bRecursively)
					{
						if (this.m_ChildRenderers == null)
						{
							this.m_ChildRenderers = base.transform.GetComponentsInChildren<Renderer>(true);
							this.m_ChildColorNames = new string[this.m_ChildRenderers.Length];
						}
						ncInfoCurve.m_ChildOriginalColorValues = new Vector4[this.m_ChildRenderers.Length];
						ncInfoCurve.m_ChildBeforeColorValues = new Vector4[this.m_ChildRenderers.Length];
						for (int i = 0; i < this.m_ChildRenderers.Length; i++)
						{
							Renderer renderer = this.m_ChildRenderers[i];
							this.m_ChildColorNames[i] = NcCurveAnimation.Ng_GetMaterialColorName(renderer.sharedMaterial);
							if (this.m_ChildColorNames[i] != null)
							{
								if (!this.m_bSavedOriginalValue)
								{
									ncInfoCurve.m_ChildOriginalColorValues[i] = renderer.material.GetColor(this.m_ChildColorNames[i]);
								}
								else
								{
									renderer.material.SetColor(this.m_ChildColorNames[i], ncInfoCurve.m_ChildOriginalColorValues[i]);
								}
							}
							ncInfoCurve.m_ChildBeforeColorValues[i] = Vector4.zero;
						}
					}
					else if (base.renderer != null)
					{
						this.m_ColorName = NcCurveAnimation.Ng_GetMaterialColorName(base.renderer.sharedMaterial);
						if (this.m_ColorName != null)
						{
							if (!this.m_bSavedOriginalValue)
							{
								ncInfoCurve.m_OriginalValue = base.renderer.material.GetColor(this.m_ColorName);
							}
							else
							{
								base.renderer.material.SetColor(this.m_ColorName, ncInfoCurve.m_OriginalValue);
							}
						}
						ncInfoCurve.m_BeforeValue = Vector4.zero;
					}
					break;
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.TEXTUREUV:
					if (this.m_NcUvAnimation == null)
					{
						this.m_NcUvAnimation = base.GetComponent<NcUvAnimation>();
					}
					if (this.m_NcUvAnimation != null)
					{
						if (!this.m_bSavedOriginalValue)
						{
							ncInfoCurve.m_OriginalValue = new Vector4(this.m_NcUvAnimation.m_fScrollSpeedX, this.m_NcUvAnimation.m_fScrollSpeedY, 0f, 0f);
						}
						else
						{
							this.m_NcUvAnimation.m_fScrollSpeedX = ncInfoCurve.m_OriginalValue.x;
							this.m_NcUvAnimation.m_fScrollSpeedY = ncInfoCurve.m_OriginalValue.y;
						}
					}
					ncInfoCurve.m_BeforeValue = ncInfoCurve.m_OriginalValue;
					break;
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MESH_COLOR:
				{
					float num = ncInfoCurve.m_AniCurve.Evaluate(0f);
					Color tarColor = Color.Lerp(ncInfoCurve.m_FromColor, ncInfoCurve.m_ToColor, num);
					if (ncInfoCurve.m_bRecursively)
					{
						this.m_ChildMeshFilters = base.transform.GetComponentsInChildren<MeshFilter>(true);
						if (this.m_ChildMeshFilters != null && this.m_ChildMeshFilters.Length >= 0)
						{
							for (int j = 0; j < this.m_ChildMeshFilters.Length; j++)
							{
								this.ChangeMeshColor(this.m_ChildMeshFilters[j], tarColor);
							}
						}
					}
					else
					{
						this.m_MainMeshFilter = base.GetComponent<MeshFilter>();
						this.ChangeMeshColor(this.m_MainMeshFilter, tarColor);
					}
					break;
				}
				}
			}
		}
		this.m_bSavedOriginalValue = true;
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x000BEA00 File Offset: 0x000BCC00
	private void UpdateAnimation(float fElapsedRate)
	{
		this.m_fElapsedRate = fElapsedRate;
		foreach (NcCurveAnimation.NcInfoCurve ncInfoCurve in this.m_CurveInfoList)
		{
			if (ncInfoCurve.m_bEnabled)
			{
				float num = ncInfoCurve.m_AniCurve.Evaluate(this.m_fElapsedRate);
				if (ncInfoCurve.m_ApplyType != NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MATERIAL_COLOR && ncInfoCurve.m_ApplyType != NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MESH_COLOR)
				{
					num *= ncInfoCurve.m_fValueScale;
				}
				switch (ncInfoCurve.m_ApplyType)
				{
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.POSITION:
					if (ncInfoCurve.m_bApplyOption[3])
					{
						this.m_Transform.position += new Vector3(this.GetNextValue(ncInfoCurve, 0, num), this.GetNextValue(ncInfoCurve, 1, num), this.GetNextValue(ncInfoCurve, 2, num));
					}
					else
					{
						this.m_Transform.localPosition += new Vector3(this.GetNextValue(ncInfoCurve, 0, num), this.GetNextValue(ncInfoCurve, 1, num), this.GetNextValue(ncInfoCurve, 2, num));
					}
					break;
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.ROTATION:
					if (ncInfoCurve.m_bApplyOption[3])
					{
						this.m_Transform.rotation *= Quaternion.Euler(this.GetNextValue(ncInfoCurve, 0, num), this.GetNextValue(ncInfoCurve, 1, num), this.GetNextValue(ncInfoCurve, 2, num));
					}
					else
					{
						this.m_Transform.localRotation *= Quaternion.Euler(this.GetNextValue(ncInfoCurve, 0, num), this.GetNextValue(ncInfoCurve, 1, num), this.GetNextValue(ncInfoCurve, 2, num));
					}
					break;
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.SCALE:
					this.m_Transform.localScale += new Vector3(this.GetNextScale(ncInfoCurve, 0, num), this.GetNextScale(ncInfoCurve, 1, num), this.GetNextScale(ncInfoCurve, 2, num));
					break;
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MATERIAL_COLOR:
					if (ncInfoCurve.m_bRecursively)
					{
						if (this.m_ChildColorNames != null && this.m_ChildColorNames.Length >= 0)
						{
							for (int i = 0; i < this.m_ChildColorNames.Length; i++)
							{
								if (this.m_ChildColorNames[i] != null && this.m_ChildRenderers[i] != null)
								{
									this.SetChildMaterialColor(ncInfoCurve, num, i);
								}
							}
						}
					}
					else if (base.renderer != null && this.m_ColorName != null)
					{
						if (this.m_MainMaterial == null)
						{
							this.m_MainMaterial = base.renderer.material;
							base.AddRuntimeMaterial(this.m_MainMaterial);
						}
						Color color = ncInfoCurve.m_ToColor - ncInfoCurve.m_OriginalValue;
						Color color2 = this.m_MainMaterial.GetColor(this.m_ColorName);
						for (int j = 0; j < 4; j++)
						{
							ref Color ptr = ref color2;
							int num3;
							int num2 = num3 = j;
							float num4 = ptr[num3];
							color2[num2] = num4 + this.GetNextValue(ncInfoCurve, j, color[j] * num);
						}
						this.m_MainMaterial.SetColor(this.m_ColorName, color2);
					}
					break;
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.TEXTUREUV:
					if (this.m_NcUvAnimation)
					{
						this.m_NcUvAnimation.m_fScrollSpeedX += this.GetNextValue(ncInfoCurve, 0, num);
						this.m_NcUvAnimation.m_fScrollSpeedY += this.GetNextValue(ncInfoCurve, 1, num);
					}
					break;
				case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MESH_COLOR:
				{
					Color tarColor = Color.Lerp(ncInfoCurve.m_FromColor, ncInfoCurve.m_ToColor, num);
					if (ncInfoCurve.m_bRecursively)
					{
						if (this.m_ChildMeshFilters != null && this.m_ChildMeshFilters.Length >= 0)
						{
							for (int k = 0; k < this.m_ChildMeshFilters.Length; k++)
							{
								this.ChangeMeshColor(this.m_ChildMeshFilters[k], tarColor);
							}
						}
					}
					else
					{
						this.ChangeMeshColor(this.m_MainMeshFilter, tarColor);
					}
					break;
				}
				}
			}
		}
		if (this.m_fDurationTime != 0f && 1f < this.m_fElapsedRate)
		{
			if (!base.IsEndAnimation())
			{
				base.OnEndAnimation();
			}
			if (this.m_bAutoDestruct)
			{
				if (this.m_bReplayState)
				{
					NcEffectBehaviour.SetActiveRecursively(base.gameObject, false);
				}
				else
				{
					Object.DestroyObject(base.gameObject);
				}
			}
		}
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x000BEEA0 File Offset: 0x000BD0A0
	private void ChangeMeshColor(MeshFilter mFilter, Color tarColor)
	{
		if (mFilter == null || mFilter.mesh == null)
		{
			Debug.LogWarning("ChangeMeshColor mFilter : " + mFilter);
			Debug.LogWarning("ChangeMeshColor mFilter.mesh : " + mFilter.mesh);
			return;
		}
		Color[] array = mFilter.mesh.colors;
		if (array.Length == 0)
		{
			if (mFilter.mesh.vertices.Length == 0)
			{
				NcSpriteFactory.CreateEmptyMesh(mFilter);
			}
			array = new Color[mFilter.mesh.vertices.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Color.white;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = tarColor;
		}
		mFilter.mesh.colors = array;
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x000BEF84 File Offset: 0x000BD184
	private void SetChildMaterialColor(NcCurveAnimation.NcInfoCurve curveInfo, float fValue, int arrayIndex)
	{
		Color color = curveInfo.m_ToColor - curveInfo.m_ChildOriginalColorValues[arrayIndex];
		Color color2 = this.m_ChildRenderers[arrayIndex].material.GetColor(this.m_ChildColorNames[arrayIndex]);
		for (int i = 0; i < 4; i++)
		{
			ref Color ptr = ref color2;
			int num2;
			int num = num2 = i;
			float num3 = ptr[num2];
			color2[num] = num3 + this.GetChildNextColorValue(curveInfo, i, color[i] * fValue, arrayIndex);
		}
		this.m_ChildRenderers[arrayIndex].material.SetColor(this.m_ChildColorNames[arrayIndex], color2);
	}

	// Token: 0x0600171C RID: 5916 RVA: 0x000BF028 File Offset: 0x000BD228
	private float GetChildNextColorValue(NcCurveAnimation.NcInfoCurve curveInfo, int nIndex, float fValue, int arrayIndex)
	{
		if (curveInfo.m_bApplyOption[nIndex])
		{
			float result = fValue - curveInfo.m_ChildBeforeColorValues[arrayIndex][nIndex];
			curveInfo.m_ChildBeforeColorValues[arrayIndex][nIndex] = fValue;
			return result;
		}
		return 0f;
	}

	// Token: 0x0600171D RID: 5917 RVA: 0x000BF074 File Offset: 0x000BD274
	private float GetNextValue(NcCurveAnimation.NcInfoCurve curveInfo, int nIndex, float fValue)
	{
		if (curveInfo.m_bApplyOption[nIndex])
		{
			float result = fValue - curveInfo.m_BeforeValue[nIndex];
			curveInfo.m_BeforeValue[nIndex] = fValue;
			return result;
		}
		return 0f;
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x000BF0B4 File Offset: 0x000BD2B4
	private float GetNextScale(NcCurveAnimation.NcInfoCurve curveInfo, int nIndex, float fValue)
	{
		if (curveInfo.m_bApplyOption[nIndex])
		{
			float num = curveInfo.m_OriginalValue[nIndex] * (1f + fValue);
			float result = num - curveInfo.m_BeforeValue[nIndex];
			curveInfo.m_BeforeValue[nIndex] = num;
			return result;
		}
		return 0f;
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x0000EF02 File Offset: 0x0000D102
	public float GetElapsedRate()
	{
		return this.m_fElapsedRate;
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x000BF108 File Offset: 0x000BD308
	public void CopyTo(NcCurveAnimation target, bool bCurveOnly)
	{
		target.m_CurveInfoList = new List<NcCurveAnimation.NcInfoCurve>();
		foreach (NcCurveAnimation.NcInfoCurve ncInfoCurve in this.m_CurveInfoList)
		{
			target.m_CurveInfoList.Add(ncInfoCurve.GetClone());
		}
		if (!bCurveOnly)
		{
			target.m_fDelayTime = this.m_fDelayTime;
			target.m_fDurationTime = this.m_fDurationTime;
		}
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x000BF198 File Offset: 0x000BD398
	public void AppendTo(NcCurveAnimation target, bool bCurveOnly)
	{
		if (target.m_CurveInfoList == null)
		{
			target.m_CurveInfoList = new List<NcCurveAnimation.NcInfoCurve>();
		}
		foreach (NcCurveAnimation.NcInfoCurve ncInfoCurve in this.m_CurveInfoList)
		{
			target.m_CurveInfoList.Add(ncInfoCurve.GetClone());
		}
		if (!bCurveOnly)
		{
			target.m_fDelayTime = this.m_fDelayTime;
			target.m_fDurationTime = this.m_fDurationTime;
		}
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x0000EF12 File Offset: 0x0000D112
	public NcCurveAnimation.NcInfoCurve GetCurveInfo(int nIndex)
	{
		if (this.m_CurveInfoList == null || nIndex < 0 || this.m_CurveInfoList.Count <= nIndex)
		{
			return null;
		}
		return this.m_CurveInfoList[nIndex];
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x000BF230 File Offset: 0x000BD430
	public NcCurveAnimation.NcInfoCurve GetCurveInfo(string curveName)
	{
		if (this.m_CurveInfoList == null)
		{
			return null;
		}
		foreach (NcCurveAnimation.NcInfoCurve ncInfoCurve in this.m_CurveInfoList)
		{
			if (ncInfoCurve.m_CurveName == curveName)
			{
				return ncInfoCurve;
			}
		}
		return null;
	}

	// Token: 0x06001724 RID: 5924 RVA: 0x000BF2AC File Offset: 0x000BD4AC
	public NcCurveAnimation.NcInfoCurve SetCurveInfo(int nIndex, NcCurveAnimation.NcInfoCurve newInfo)
	{
		if (this.m_CurveInfoList == null || nIndex < 0 || this.m_CurveInfoList.Count <= nIndex)
		{
			return null;
		}
		NcCurveAnimation.NcInfoCurve result = this.m_CurveInfoList[nIndex];
		this.m_CurveInfoList[nIndex] = newInfo;
		return result;
	}

	// Token: 0x06001725 RID: 5925 RVA: 0x000BF2FC File Offset: 0x000BD4FC
	public int AddCurveInfo()
	{
		NcCurveAnimation.NcInfoCurve ncInfoCurve = new NcCurveAnimation.NcInfoCurve();
		ncInfoCurve.m_AniCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		ncInfoCurve.m_ToColor = Color.white;
		ncInfoCurve.m_ToColor.w = 0f;
		if (this.m_CurveInfoList == null)
		{
			this.m_CurveInfoList = new List<NcCurveAnimation.NcInfoCurve>();
		}
		this.m_CurveInfoList.Add(ncInfoCurve);
		return this.m_CurveInfoList.Count - 1;
	}

	// Token: 0x06001726 RID: 5926 RVA: 0x0000EF45 File Offset: 0x0000D145
	public int AddCurveInfo(NcCurveAnimation.NcInfoCurve addCurveInfo)
	{
		if (this.m_CurveInfoList == null)
		{
			this.m_CurveInfoList = new List<NcCurveAnimation.NcInfoCurve>();
		}
		this.m_CurveInfoList.Add(addCurveInfo.GetClone());
		return this.m_CurveInfoList.Count - 1;
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x0000EF7B File Offset: 0x0000D17B
	public void DeleteCurveInfo(int nIndex)
	{
		if (this.m_CurveInfoList == null || nIndex < 0 || this.m_CurveInfoList.Count <= nIndex)
		{
			return;
		}
		this.m_CurveInfoList.Remove(this.m_CurveInfoList[nIndex]);
	}

	// Token: 0x06001728 RID: 5928 RVA: 0x0000EFB9 File Offset: 0x0000D1B9
	public void ClearAllCurveInfo()
	{
		if (this.m_CurveInfoList == null)
		{
			return;
		}
		this.m_CurveInfoList.Clear();
	}

	// Token: 0x06001729 RID: 5929 RVA: 0x0000EFD2 File Offset: 0x0000D1D2
	public int GetCurveInfoCount()
	{
		if (this.m_CurveInfoList == null)
		{
			return 0;
		}
		return this.m_CurveInfoList.Count;
	}

	// Token: 0x0600172A RID: 5930 RVA: 0x000BF380 File Offset: 0x000BD580
	public void SortCurveInfo()
	{
		if (this.m_CurveInfoList == null)
		{
			return;
		}
		this.m_CurveInfoList.Sort(new NcCurveAnimation.NcComparerCurve());
		foreach (NcCurveAnimation.NcInfoCurve ncInfoCurve in this.m_CurveInfoList)
		{
			ncInfoCurve.m_nSortGroup = NcCurveAnimation.NcComparerCurve.GetSortGroup(ncInfoCurve);
		}
	}

	// Token: 0x0600172B RID: 5931 RVA: 0x000BF3FC File Offset: 0x000BD5FC
	public bool CheckInvalidOption()
	{
		bool result = false;
		for (int i = 0; i < this.m_CurveInfoList.Count; i++)
		{
			if (this.CheckInvalidOption(i))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x0600172C RID: 5932 RVA: 0x000BF438 File Offset: 0x000BD638
	public bool CheckInvalidOption(int nSrcIndex)
	{
		NcCurveAnimation.NcInfoCurve curveInfo = this.GetCurveInfo(nSrcIndex);
		return curveInfo != null && (curveInfo.m_ApplyType != NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MATERIAL_COLOR && curveInfo.m_ApplyType != NcCurveAnimation.NcInfoCurve.APPLY_TYPE.SCALE && curveInfo.m_ApplyType != NcCurveAnimation.NcInfoCurve.APPLY_TYPE.TEXTUREUV) && false;
	}

	// Token: 0x0600172D RID: 5933 RVA: 0x0000EFEC File Offset: 0x0000D1EC
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fDelayTime /= fSpeedRate;
		this.m_fDurationTime /= fSpeedRate;
	}

	// Token: 0x0600172E RID: 5934 RVA: 0x0000EC13 File Offset: 0x0000CE13
	public override void OnSetReplayState()
	{
		base.OnSetReplayState();
	}

	// Token: 0x0600172F RID: 5935 RVA: 0x0000F00A File Offset: 0x0000D20A
	public override void OnResetReplayStage(bool bClearOldParticle)
	{
		base.OnResetReplayStage(bClearOldParticle);
		this.ResetAnimation();
	}

	// Token: 0x06001730 RID: 5936 RVA: 0x000B91A0 File Offset: 0x000B73A0
	public static string Ng_GetMaterialColorName(Material mat)
	{
		string[] array = new string[]
		{
			"_Color",
			"_TintColor",
			"_EmisColor"
		};
		if (mat != null)
		{
			foreach (string text in array)
			{
				if (mat.HasProperty(text))
				{
					return text;
				}
			}
		}
		return null;
	}

	// Token: 0x04001B70 RID: 7024
	[SerializeField]
	public List<NcCurveAnimation.NcInfoCurve> m_CurveInfoList;

	// Token: 0x04001B71 RID: 7025
	public float m_fDelayTime;

	// Token: 0x04001B72 RID: 7026
	public float m_fDurationTime = 0.6f;

	// Token: 0x04001B73 RID: 7027
	public bool m_bAutoDestruct = true;

	// Token: 0x04001B74 RID: 7028
	protected float m_fStartTime;

	// Token: 0x04001B75 RID: 7029
	public float m_fAddElapsedTime;

	// Token: 0x04001B76 RID: 7030
	protected float m_fElapsedRate;

	// Token: 0x04001B77 RID: 7031
	protected Transform m_Transform;

	// Token: 0x04001B78 RID: 7032
	protected string m_ColorName;

	// Token: 0x04001B79 RID: 7033
	protected Material m_MainMaterial;

	// Token: 0x04001B7A RID: 7034
	protected string[] m_ChildColorNames;

	// Token: 0x04001B7B RID: 7035
	protected Renderer[] m_ChildRenderers;

	// Token: 0x04001B7C RID: 7036
	protected MeshFilter m_MainMeshFilter;

	// Token: 0x04001B7D RID: 7037
	protected MeshFilter[] m_ChildMeshFilters;

	// Token: 0x04001B7E RID: 7038
	protected NcUvAnimation m_NcUvAnimation;

	// Token: 0x04001B7F RID: 7039
	protected NcTransformTool m_NcTansform;

	// Token: 0x04001B80 RID: 7040
	protected bool m_bSavedOriginalValue;

	// Token: 0x020003CF RID: 975
	private class NcComparerCurve : IComparer<NcCurveAnimation.NcInfoCurve>
	{
		// Token: 0x06001733 RID: 5939 RVA: 0x000BF480 File Offset: 0x000BD680
		public int Compare(NcCurveAnimation.NcInfoCurve a, NcCurveAnimation.NcInfoCurve b)
		{
			float num = a.m_AniCurve.Evaluate(NcCurveAnimation.NcComparerCurve.m_fEqualRange / NcCurveAnimation.NcComparerCurve.m_fHDiv) - b.m_AniCurve.Evaluate(NcCurveAnimation.NcComparerCurve.m_fEqualRange / NcCurveAnimation.NcComparerCurve.m_fHDiv);
			if (Mathf.Abs(num) < NcCurveAnimation.NcComparerCurve.m_fEqualRange)
			{
				num = b.m_AniCurve.Evaluate(1f - NcCurveAnimation.NcComparerCurve.m_fEqualRange / NcCurveAnimation.NcComparerCurve.m_fHDiv) - a.m_AniCurve.Evaluate(1f - NcCurveAnimation.NcComparerCurve.m_fEqualRange / NcCurveAnimation.NcComparerCurve.m_fHDiv);
				if (Mathf.Abs(num) < NcCurveAnimation.NcComparerCurve.m_fEqualRange)
				{
					return 0;
				}
			}
			return (int)(num * 1000f);
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x000BF520 File Offset: 0x000BD720
		public static int GetSortGroup(NcCurveAnimation.NcInfoCurve info)
		{
			float num = info.m_AniCurve.Evaluate(NcCurveAnimation.NcComparerCurve.m_fEqualRange / NcCurveAnimation.NcComparerCurve.m_fHDiv);
			if (num < -NcCurveAnimation.NcComparerCurve.m_fEqualRange)
			{
				return 1;
			}
			if (NcCurveAnimation.NcComparerCurve.m_fEqualRange < num)
			{
				return 3;
			}
			return 2;
		}

		// Token: 0x04001B81 RID: 7041
		protected static float m_fEqualRange = 0.03f;

		// Token: 0x04001B82 RID: 7042
		protected static float m_fHDiv = 5f;
	}

	// Token: 0x020003D0 RID: 976
	[Serializable]
	public class NcInfoCurve
	{
		// Token: 0x06001737 RID: 5943 RVA: 0x0000F02F File Offset: 0x0000D22F
		public bool IsEnabled()
		{
			return this.m_bEnabled;
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x0000F037 File Offset: 0x0000D237
		public void SetEnabled(bool bEnable)
		{
			this.m_bEnabled = bEnable;
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x0000F040 File Offset: 0x0000D240
		public string GetCurveName()
		{
			return this.m_CurveName;
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x000BF624 File Offset: 0x000BD824
		public NcCurveAnimation.NcInfoCurve GetClone()
		{
			NcCurveAnimation.NcInfoCurve ncInfoCurve = new NcCurveAnimation.NcInfoCurve();
			ncInfoCurve.m_AniCurve = new AnimationCurve(this.m_AniCurve.keys);
			ncInfoCurve.m_AniCurve.postWrapMode = this.m_AniCurve.postWrapMode;
			ncInfoCurve.m_AniCurve.preWrapMode = this.m_AniCurve.preWrapMode;
			ncInfoCurve.m_bEnabled = this.m_bEnabled;
			ncInfoCurve.m_CurveName = this.m_CurveName;
			ncInfoCurve.m_ApplyType = this.m_ApplyType;
			Array.Copy(this.m_bApplyOption, ncInfoCurve.m_bApplyOption, this.m_bApplyOption.Length);
			ncInfoCurve.m_fValueScale = this.m_fValueScale;
			ncInfoCurve.m_bRecursively = this.m_bRecursively;
			ncInfoCurve.m_FromColor = this.m_FromColor;
			ncInfoCurve.m_ToColor = this.m_ToColor;
			ncInfoCurve.m_nTag = this.m_nTag;
			ncInfoCurve.m_nSortGroup = this.m_nSortGroup;
			return ncInfoCurve;
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x000BF700 File Offset: 0x000BD900
		public void CopyTo(NcCurveAnimation.NcInfoCurve target)
		{
			target.m_AniCurve = new AnimationCurve(this.m_AniCurve.keys);
			target.m_AniCurve.postWrapMode = this.m_AniCurve.postWrapMode;
			target.m_AniCurve.preWrapMode = this.m_AniCurve.preWrapMode;
			target.m_bEnabled = this.m_bEnabled;
			target.m_ApplyType = this.m_ApplyType;
			Array.Copy(this.m_bApplyOption, target.m_bApplyOption, this.m_bApplyOption.Length);
			target.m_fValueScale = this.m_fValueScale;
			target.m_bRecursively = this.m_bRecursively;
			target.m_FromColor = this.m_FromColor;
			target.m_ToColor = this.m_ToColor;
			target.m_nTag = this.m_nTag;
			target.m_nSortGroup = this.m_nSortGroup;
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x000BF7C8 File Offset: 0x000BD9C8
		public int GetValueCount()
		{
			switch (this.m_ApplyType)
			{
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.POSITION:
				return 4;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.ROTATION:
				return 4;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.SCALE:
				return 3;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MATERIAL_COLOR:
				return 4;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.TEXTUREUV:
				return 2;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MESH_COLOR:
				return 4;
			}
			return 0;
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x000BF810 File Offset: 0x000BDA10
		public string GetValueName(int nIndex)
		{
			string[] array;
			switch (this.m_ApplyType)
			{
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.POSITION:
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.ROTATION:
				array = new string[]
				{
					"X",
					"Y",
					"Z",
					"World"
				};
				goto IL_136;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.SCALE:
				array = new string[]
				{
					"X",
					"Y",
					"Z",
					string.Empty
				};
				goto IL_136;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MATERIAL_COLOR:
				array = new string[]
				{
					"R",
					"G",
					"B",
					"A"
				};
				goto IL_136;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.TEXTUREUV:
				array = new string[]
				{
					"X",
					"Y",
					string.Empty,
					string.Empty
				};
				goto IL_136;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.MESH_COLOR:
				array = new string[]
				{
					"R",
					"G",
					"B",
					"A"
				};
				goto IL_136;
			}
			array = new string[]
			{
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty
			};
			IL_136:
			return array[nIndex];
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x000BF958 File Offset: 0x000BDB58
		public void SetDefaultValueScale()
		{
			switch (this.m_ApplyType)
			{
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.POSITION:
				this.m_fValueScale = 1f;
				break;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.ROTATION:
				this.m_fValueScale = 360f;
				break;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.SCALE:
				this.m_fValueScale = 1f;
				break;
			case NcCurveAnimation.NcInfoCurve.APPLY_TYPE.TEXTUREUV:
				this.m_fValueScale = 10f;
				break;
			}
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x0000F048 File Offset: 0x0000D248
		public Rect GetFixedDrawRange()
		{
			return new Rect(-0.2f, -1.2f, 1.4f, 2.4f);
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x000BF9E4 File Offset: 0x000BDBE4
		public Rect GetVariableDrawRange()
		{
			Rect result = default(Rect);
			for (int i = 0; i < this.m_AniCurve.keys.Length; i++)
			{
				result.yMin = Mathf.Min(result.yMin, this.m_AniCurve[i].value);
				result.yMax = Mathf.Max(result.yMax, this.m_AniCurve[i].value);
			}
			int num = 20;
			for (int j = 0; j < num; j++)
			{
				float num2 = this.m_AniCurve.Evaluate((float)j / (float)num);
				result.yMin = Mathf.Min(result.yMin, num2);
				result.yMax = Mathf.Max(result.yMax, num2);
			}
			result.xMin = 0f;
			result.xMax = 1f;
			result.xMin -= result.width * 0.2f;
			result.xMax += result.width * 0.2f;
			result.yMin -= result.height * 0.2f;
			result.yMax += result.height * 0.2f;
			return result;
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x0000F063 File Offset: 0x0000D263
		public Rect GetEditRange()
		{
			return new Rect(0f, -1f, 1f, 2f);
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x000BFB40 File Offset: 0x000BDD40
		public void NormalizeCurveTime()
		{
			int i = 0;
			while (i < this.m_AniCurve.keys.Length)
			{
				Keyframe keyframe = this.m_AniCurve[i];
				float num = Mathf.Max(0f, keyframe.time);
				float num2 = Mathf.Min(1f, Mathf.Max(num, keyframe.time));
				if (num2 != keyframe.time)
				{
					Keyframe keyframe2;
					keyframe2..ctor(num2, keyframe.value, keyframe.inTangent, keyframe.outTangent);
					this.m_AniCurve.RemoveKey(i);
					i = 0;
					this.m_AniCurve.AddKey(keyframe2);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x04001B83 RID: 7043
		protected const float m_fOverDraw = 0.2f;

		// Token: 0x04001B84 RID: 7044
		public bool m_bEnabled = true;

		// Token: 0x04001B85 RID: 7045
		public string m_CurveName = string.Empty;

		// Token: 0x04001B86 RID: 7046
		public AnimationCurve m_AniCurve = new AnimationCurve();

		// Token: 0x04001B87 RID: 7047
		public static string[] m_TypeName = new string[]
		{
			"None",
			"Position",
			"Rotation",
			"Scale",
			"MaterialColor",
			"TextureUV",
			"MeshColor"
		};

		// Token: 0x04001B88 RID: 7048
		public NcCurveAnimation.NcInfoCurve.APPLY_TYPE m_ApplyType = NcCurveAnimation.NcInfoCurve.APPLY_TYPE.POSITION;

		// Token: 0x04001B89 RID: 7049
		public bool[] m_bApplyOption = new bool[]
		{
			default(bool),
			default(bool),
			default(bool),
			true
		};

		// Token: 0x04001B8A RID: 7050
		public bool m_bRecursively;

		// Token: 0x04001B8B RID: 7051
		public float m_fValueScale = 1f;

		// Token: 0x04001B8C RID: 7052
		public Vector4 m_FromColor = Color.white;

		// Token: 0x04001B8D RID: 7053
		public Vector4 m_ToColor = Color.white;

		// Token: 0x04001B8E RID: 7054
		public int m_nTag;

		// Token: 0x04001B8F RID: 7055
		public int m_nSortGroup;

		// Token: 0x04001B90 RID: 7056
		public Vector4 m_OriginalValue;

		// Token: 0x04001B91 RID: 7057
		public Vector4 m_BeforeValue;

		// Token: 0x04001B92 RID: 7058
		public Vector4[] m_ChildOriginalColorValues;

		// Token: 0x04001B93 RID: 7059
		public Vector4[] m_ChildBeforeColorValues;

		// Token: 0x020003D1 RID: 977
		public enum APPLY_TYPE
		{
			// Token: 0x04001B95 RID: 7061
			NONE,
			// Token: 0x04001B96 RID: 7062
			POSITION,
			// Token: 0x04001B97 RID: 7063
			ROTATION,
			// Token: 0x04001B98 RID: 7064
			SCALE,
			// Token: 0x04001B99 RID: 7065
			MATERIAL_COLOR,
			// Token: 0x04001B9A RID: 7066
			TEXTUREUV,
			// Token: 0x04001B9B RID: 7067
			MESH_COLOR
		}
	}
}
