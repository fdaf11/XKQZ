using System;
using UnityEngine;

// Token: 0x020003CB RID: 971
public class NcChangeAlpha : NcEffectBehaviour
{
	// Token: 0x06001704 RID: 5892 RVA: 0x000BE060 File Offset: 0x000BC260
	public static NcChangeAlpha SetChangeTime(GameObject baseGameObject, float fLifeTime, float fChangeTime, float fFromMeshAlphaValue, float fToMeshAlphaValue)
	{
		NcChangeAlpha ncChangeAlpha = baseGameObject.AddComponent<NcChangeAlpha>();
		ncChangeAlpha.SetChangeTime(fLifeTime, fChangeTime, fFromMeshAlphaValue, fToMeshAlphaValue);
		return ncChangeAlpha;
	}

	// Token: 0x06001705 RID: 5893 RVA: 0x0000EE23 File Offset: 0x0000D023
	public void SetChangeTime(float fDelayTime, float fChangeTime, float fFromAlphaValue, float fToAlphaValue)
	{
		this.m_fDelayTime = fDelayTime;
		this.m_fChangeTime = fChangeTime;
		this.m_fFromAlphaValue = fFromAlphaValue;
		this.m_fToMeshValue = fToAlphaValue;
		if (NcEffectBehaviour.IsActive(base.gameObject))
		{
			this.Start();
			this.Update();
		}
	}

	// Token: 0x06001706 RID: 5894 RVA: 0x0000EE5E File Offset: 0x0000D05E
	public void Restart()
	{
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
		this.m_fStartChangeTime = 0f;
		this.ChangeToAlpha(0f);
	}

	// Token: 0x06001707 RID: 5895 RVA: 0x0000EE81 File Offset: 0x0000D081
	private void Awake()
	{
		this.m_fStartTime = 0f;
		this.m_fStartChangeTime = 0f;
	}

	// Token: 0x06001708 RID: 5896 RVA: 0x0000EE99 File Offset: 0x0000D099
	private void Start()
	{
		this.Restart();
	}

	// Token: 0x06001709 RID: 5897 RVA: 0x000BE080 File Offset: 0x000BC280
	private void Update()
	{
		if (0f < this.m_fStartChangeTime)
		{
			if (0f < this.m_fChangeTime)
			{
				float num = (NcEffectBehaviour.GetEngineTime() - this.m_fStartChangeTime) / this.m_fChangeTime;
				if (1f < num)
				{
					num = 1f;
					if (this.m_bAutoDeactive && this.m_fToMeshValue <= 0f)
					{
						NcEffectBehaviour.SetActiveRecursively(base.gameObject, false);
					}
				}
				this.ChangeToAlpha(num);
			}
			else
			{
				this.ChangeToAlpha(1f);
			}
		}
		else if (0f < this.m_fStartTime && this.m_fStartTime + this.m_fDelayTime <= NcEffectBehaviour.GetEngineTime())
		{
			this.StartChange();
		}
	}

	// Token: 0x0600170A RID: 5898 RVA: 0x0000EEA1 File Offset: 0x0000D0A1
	private void StartChange()
	{
		this.m_fStartChangeTime = NcEffectBehaviour.GetEngineTime();
	}

	// Token: 0x0600170B RID: 5899 RVA: 0x000BE144 File Offset: 0x000BC344
	private void ChangeToAlpha(float fElapsedRate)
	{
		float num = Mathf.Lerp(this.m_fFromAlphaValue, this.m_fToMeshValue, fElapsedRate);
		if (this.m_TargetType == NcChangeAlpha.TARGET_TYPE.MeshColor)
		{
			MeshFilter[] array;
			if (this.m_bRecursively)
			{
				array = base.transform.GetComponentsInChildren<MeshFilter>(true);
			}
			else
			{
				array = base.transform.GetComponents<MeshFilter>();
			}
			for (int i = 0; i < array.Length; i++)
			{
				Color[] array2 = array[i].mesh.colors;
				if (array2.Length == 0)
				{
					if (array[i].mesh.vertices.Length == 0)
					{
						NcSpriteFactory.CreateEmptyMesh(array[i]);
					}
					array2 = new Color[array[i].mesh.vertices.Length];
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j] = Color.white;
					}
				}
				for (int k = 0; k < array2.Length; k++)
				{
					Color color = array2[k];
					color.a = num;
					array2[k] = color;
				}
				array[i].mesh.colors = array2;
			}
		}
		else
		{
			Renderer[] array3;
			if (this.m_bRecursively)
			{
				array3 = base.transform.GetComponentsInChildren<Renderer>(true);
			}
			else
			{
				array3 = base.transform.GetComponents<Renderer>();
			}
			foreach (Renderer renderer in array3)
			{
				string materialColorName = NcEffectBehaviour.GetMaterialColorName(renderer.sharedMaterial);
				if (materialColorName != null)
				{
					Color color2 = renderer.material.GetColor(materialColorName);
					color2.a = num;
					renderer.material.SetColor(materialColorName, color2);
				}
			}
		}
		if (fElapsedRate == 1f && num == 0f)
		{
			NcEffectBehaviour.SetActiveRecursively(base.gameObject, false);
		}
	}

	// Token: 0x0600170C RID: 5900 RVA: 0x0000EEAE File Offset: 0x0000D0AE
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fDelayTime /= fSpeedRate;
		this.m_fChangeTime /= fSpeedRate;
	}

	// Token: 0x0600170D RID: 5901 RVA: 0x000BE320 File Offset: 0x000BC520
	public override void OnSetReplayState()
	{
		base.OnSetReplayState();
		this.m_NcEffectInitBackup = new NcEffectInitBackup();
		if (this.m_TargetType == NcChangeAlpha.TARGET_TYPE.MeshColor)
		{
			this.m_NcEffectInitBackup.BackupMeshColor(base.gameObject, this.m_bRecursively);
		}
		else
		{
			this.m_NcEffectInitBackup.BackupMaterialColor(base.gameObject, this.m_bRecursively);
		}
	}

	// Token: 0x0600170E RID: 5902 RVA: 0x000BE37C File Offset: 0x000BC57C
	public override void OnResetReplayStage(bool bClearOldParticle)
	{
		base.OnResetReplayStage(bClearOldParticle);
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
		this.m_fStartChangeTime = 0f;
		if (this.m_NcEffectInitBackup != null)
		{
			if (this.m_TargetType == NcChangeAlpha.TARGET_TYPE.MeshColor)
			{
				this.m_NcEffectInitBackup.RestoreMeshColor();
			}
			else
			{
				this.m_NcEffectInitBackup.RestoreMaterialColor();
			}
		}
	}

	// Token: 0x04001B61 RID: 7009
	public NcChangeAlpha.TARGET_TYPE m_TargetType;

	// Token: 0x04001B62 RID: 7010
	public float m_fDelayTime = 2f;

	// Token: 0x04001B63 RID: 7011
	public float m_fChangeTime = 1f;

	// Token: 0x04001B64 RID: 7012
	public bool m_bRecursively = true;

	// Token: 0x04001B65 RID: 7013
	public NcChangeAlpha.CHANGE_MODE m_ChangeMode;

	// Token: 0x04001B66 RID: 7014
	public float m_fFromAlphaValue = 1f;

	// Token: 0x04001B67 RID: 7015
	public float m_fToMeshValue;

	// Token: 0x04001B68 RID: 7016
	public bool m_bAutoDeactive = true;

	// Token: 0x04001B69 RID: 7017
	protected float m_fStartTime;

	// Token: 0x04001B6A RID: 7018
	protected float m_fStartChangeTime;

	// Token: 0x020003CC RID: 972
	public enum TARGET_TYPE
	{
		// Token: 0x04001B6C RID: 7020
		MeshColor,
		// Token: 0x04001B6D RID: 7021
		MaterialColor
	}

	// Token: 0x020003CD RID: 973
	public enum CHANGE_MODE
	{
		// Token: 0x04001B6F RID: 7023
		FromTo
	}
}
