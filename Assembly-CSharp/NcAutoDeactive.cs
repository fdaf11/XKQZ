using System;
using UnityEngine;

// Token: 0x020003C3 RID: 963
public class NcAutoDeactive : NcEffectBehaviour
{
	// Token: 0x060016E7 RID: 5863 RVA: 0x000BD514 File Offset: 0x000BB714
	public static NcAutoDeactive CreateAutoDestruct(GameObject baseGameObject, float fLifeTime, float fDestroyTime, bool bSmoothHide, bool bMeshFilterOnlySmoothHide)
	{
		NcAutoDeactive ncAutoDeactive = baseGameObject.AddComponent<NcAutoDeactive>();
		ncAutoDeactive.m_fLifeTime = fLifeTime;
		ncAutoDeactive.m_fSmoothDestroyTime = fDestroyTime;
		ncAutoDeactive.m_bSmoothHide = bSmoothHide;
		ncAutoDeactive.m_bMeshFilterOnlySmoothHide = bMeshFilterOnlySmoothHide;
		if (NcEffectBehaviour.IsActive(baseGameObject))
		{
			ncAutoDeactive.Start();
			ncAutoDeactive.Update();
		}
		return ncAutoDeactive;
	}

	// Token: 0x060016E8 RID: 5864 RVA: 0x0000EC2A File Offset: 0x0000CE2A
	private void Awake()
	{
		this.m_bEndNcCurveAnimation = false;
		this.m_fStartTime = 0f;
		this.m_NcCurveAnimation = null;
	}

	// Token: 0x060016E9 RID: 5865 RVA: 0x0000EC45 File Offset: 0x0000CE45
	private void OnEnable()
	{
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
	}

	// Token: 0x060016EA RID: 5866 RVA: 0x0000EC52 File Offset: 0x0000CE52
	private void Start()
	{
		if (this.m_bEndNcCurveAnimation)
		{
			this.m_NcCurveAnimation = base.GetComponent<NcCurveAnimation>();
		}
	}

	// Token: 0x060016EB RID: 5867 RVA: 0x000BD560 File Offset: 0x000BB760
	private void Update()
	{
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
					this.AutoDeactive();
				}
			}
		}
		else
		{
			if (0f < this.m_fStartTime && this.m_fStartTime + this.m_fLifeTime <= NcEffectBehaviour.GetEngineTime())
			{
				this.StartDeactive();
			}
			if (this.m_bEndNcCurveAnimation && this.m_NcCurveAnimation != null && 1f <= this.m_NcCurveAnimation.GetElapsedRate())
			{
				this.StartDeactive();
			}
		}
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x000BD7A8 File Offset: 0x000BB9A8
	private void FixedUpdate()
	{
		if (0f < this.m_fStartDestroyTime)
		{
			return;
		}
		bool flag = false;
		if (this.m_CollisionType == NcAutoDeactive.CollisionType.NONE)
		{
			return;
		}
		if (this.m_CollisionType == NcAutoDeactive.CollisionType.COLLISION)
		{
			if (Physics.CheckSphere(base.transform.position, this.m_fCollisionRadius, this.m_CollisionLayer))
			{
				flag = true;
			}
		}
		else if (this.m_CollisionType == NcAutoDeactive.CollisionType.WORLD_Y && base.transform.position.y <= this.m_fDestructPosY)
		{
			flag = true;
		}
		if (flag)
		{
			this.StartDeactive();
		}
	}

	// Token: 0x060016ED RID: 5869 RVA: 0x0000EC6B File Offset: 0x0000CE6B
	private void StartDeactive()
	{
		if (this.m_fSmoothDestroyTime <= 0f)
		{
			this.AutoDeactive();
		}
		else
		{
			this.m_fStartDestroyTime = NcEffectBehaviour.GetEngineTime();
			if (this.m_bDisableEmit)
			{
				base.DisableEmit();
			}
		}
	}

	// Token: 0x060016EE RID: 5870 RVA: 0x0000ECA4 File Offset: 0x0000CEA4
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fLifeTime /= fSpeedRate;
		this.m_fSmoothDestroyTime /= fSpeedRate;
	}

	// Token: 0x060016EF RID: 5871 RVA: 0x000BD848 File Offset: 0x000BBA48
	public override void OnSetReplayState()
	{
		base.OnSetReplayState();
		if (0f < this.m_fSmoothDestroyTime && this.m_bSmoothHide)
		{
			this.m_NcEffectInitBackup = new NcEffectInitBackup();
			if (this.m_bMeshFilterOnlySmoothHide)
			{
				this.m_NcEffectInitBackup.BackupMeshColor(base.gameObject, true);
			}
			else
			{
				this.m_NcEffectInitBackup.BackupMaterialColor(base.gameObject, true);
			}
		}
	}

	// Token: 0x060016F0 RID: 5872 RVA: 0x000BD8B8 File Offset: 0x000BBAB8
	public override void OnResetReplayStage(bool bClearOldParticle)
	{
		base.OnResetReplayStage(bClearOldParticle);
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
		this.m_fStartDestroyTime = 0f;
		if (0f < this.m_fSmoothDestroyTime && this.m_bSmoothHide && this.m_NcEffectInitBackup != null)
		{
			if (this.m_bMeshFilterOnlySmoothHide)
			{
				this.m_NcEffectInitBackup.RestoreMeshColor();
			}
			else
			{
				this.m_NcEffectInitBackup.RestoreMaterialColor();
			}
		}
	}

	// Token: 0x060016F1 RID: 5873 RVA: 0x0000ECC2 File Offset: 0x0000CEC2
	private void AutoDeactive()
	{
		NcEffectBehaviour.SetActiveRecursively(base.gameObject, false);
	}

	// Token: 0x04001B26 RID: 6950
	public float m_fLifeTime = 2f;

	// Token: 0x04001B27 RID: 6951
	public float m_fSmoothDestroyTime;

	// Token: 0x04001B28 RID: 6952
	public bool m_bDisableEmit = true;

	// Token: 0x04001B29 RID: 6953
	public bool m_bSmoothHide = true;

	// Token: 0x04001B2A RID: 6954
	public bool m_bMeshFilterOnlySmoothHide;

	// Token: 0x04001B2B RID: 6955
	protected bool m_bEndNcCurveAnimation;

	// Token: 0x04001B2C RID: 6956
	public NcAutoDeactive.CollisionType m_CollisionType;

	// Token: 0x04001B2D RID: 6957
	public LayerMask m_CollisionLayer = -1;

	// Token: 0x04001B2E RID: 6958
	public float m_fCollisionRadius = 0.3f;

	// Token: 0x04001B2F RID: 6959
	public float m_fDestructPosY = 0.2f;

	// Token: 0x04001B30 RID: 6960
	protected float m_fStartTime;

	// Token: 0x04001B31 RID: 6961
	protected float m_fStartDestroyTime;

	// Token: 0x04001B32 RID: 6962
	protected NcCurveAnimation m_NcCurveAnimation;

	// Token: 0x020003C4 RID: 964
	public enum CollisionType
	{
		// Token: 0x04001B34 RID: 6964
		NONE,
		// Token: 0x04001B35 RID: 6965
		COLLISION,
		// Token: 0x04001B36 RID: 6966
		WORLD_Y
	}
}
