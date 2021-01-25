using System;
using UnityEngine;

// Token: 0x020003C5 RID: 965
public class NcAutoDestruct : NcEffectBehaviour
{
	// Token: 0x060016F3 RID: 5875 RVA: 0x000BD980 File Offset: 0x000BBB80
	public static NcAutoDestruct CreateAutoDestruct(GameObject baseGameObject, float fLifeTime, float fDestroyTime, bool bSmoothHide, bool bMeshFilterOnlySmoothHide)
	{
		NcAutoDestruct ncAutoDestruct = baseGameObject.AddComponent<NcAutoDestruct>();
		ncAutoDestruct.m_fLifeTime = fLifeTime;
		ncAutoDestruct.m_fSmoothDestroyTime = fDestroyTime;
		ncAutoDestruct.m_bSmoothHide = bSmoothHide;
		ncAutoDestruct.m_bMeshFilterOnlySmoothHide = bMeshFilterOnlySmoothHide;
		if (NcEffectBehaviour.IsActive(baseGameObject))
		{
			ncAutoDestruct.Start();
			ncAutoDestruct.Update();
		}
		return ncAutoDestruct;
	}

	// Token: 0x060016F4 RID: 5876 RVA: 0x0000ECD0 File Offset: 0x0000CED0
	private void Awake()
	{
		this.m_bEndNcCurveAnimation = false;
		this.m_fStartTime = 0f;
		this.m_NcCurveAnimation = null;
	}

	// Token: 0x060016F5 RID: 5877 RVA: 0x0000ECEB File Offset: 0x0000CEEB
	private void Start()
	{
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
		if (this.m_bEndNcCurveAnimation)
		{
			this.m_NcCurveAnimation = base.GetComponent<NcCurveAnimation>();
		}
	}

	// Token: 0x060016F6 RID: 5878 RVA: 0x000BD9CC File Offset: 0x000BBBCC
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
					this.AutoDestruct();
				}
			}
		}
		else
		{
			if (0f < this.m_fStartTime && this.m_fStartTime + this.m_fLifeTime <= NcEffectBehaviour.GetEngineTime())
			{
				this.StartDestroy();
			}
			if (this.m_bEndNcCurveAnimation && this.m_NcCurveAnimation != null && 1f <= this.m_NcCurveAnimation.GetElapsedRate())
			{
				this.StartDestroy();
			}
		}
	}

	// Token: 0x060016F7 RID: 5879 RVA: 0x000BDC14 File Offset: 0x000BBE14
	private void FixedUpdate()
	{
		if (0f < this.m_fStartDestroyTime)
		{
			return;
		}
		bool flag = false;
		if (this.m_CollisionType == NcAutoDestruct.CollisionType.NONE)
		{
			return;
		}
		if (this.m_CollisionType == NcAutoDestruct.CollisionType.COLLISION)
		{
			if (Physics.CheckSphere(base.transform.position, this.m_fCollisionRadius, this.m_CollisionLayer))
			{
				flag = true;
			}
		}
		else if (this.m_CollisionType == NcAutoDestruct.CollisionType.WORLD_Y && base.transform.position.y <= this.m_fDestructPosY)
		{
			flag = true;
		}
		if (flag)
		{
			this.StartDestroy();
		}
	}

	// Token: 0x060016F8 RID: 5880 RVA: 0x0000ED0F File Offset: 0x0000CF0F
	private void StartDestroy()
	{
		if (this.m_fSmoothDestroyTime <= 0f)
		{
			this.AutoDestruct();
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

	// Token: 0x060016F9 RID: 5881 RVA: 0x0000ED48 File Offset: 0x0000CF48
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fLifeTime /= fSpeedRate;
		this.m_fSmoothDestroyTime /= fSpeedRate;
	}

	// Token: 0x060016FA RID: 5882 RVA: 0x000BDCB4 File Offset: 0x000BBEB4
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

	// Token: 0x060016FB RID: 5883 RVA: 0x000BDD24 File Offset: 0x000BBF24
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

	// Token: 0x060016FC RID: 5884 RVA: 0x0000ED66 File Offset: 0x0000CF66
	private void AutoDestruct()
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

	// Token: 0x04001B37 RID: 6967
	public float m_fLifeTime = 2f;

	// Token: 0x04001B38 RID: 6968
	public float m_fSmoothDestroyTime;

	// Token: 0x04001B39 RID: 6969
	public bool m_bDisableEmit = true;

	// Token: 0x04001B3A RID: 6970
	public bool m_bSmoothHide = true;

	// Token: 0x04001B3B RID: 6971
	public bool m_bMeshFilterOnlySmoothHide;

	// Token: 0x04001B3C RID: 6972
	protected bool m_bEndNcCurveAnimation;

	// Token: 0x04001B3D RID: 6973
	public NcAutoDestruct.CollisionType m_CollisionType;

	// Token: 0x04001B3E RID: 6974
	public LayerMask m_CollisionLayer = -1;

	// Token: 0x04001B3F RID: 6975
	public float m_fCollisionRadius = 0.3f;

	// Token: 0x04001B40 RID: 6976
	public float m_fDestructPosY = 0.2f;

	// Token: 0x04001B41 RID: 6977
	protected float m_fStartTime;

	// Token: 0x04001B42 RID: 6978
	protected float m_fStartDestroyTime;

	// Token: 0x04001B43 RID: 6979
	protected NcCurveAnimation m_NcCurveAnimation;

	// Token: 0x020003C6 RID: 966
	public enum CollisionType
	{
		// Token: 0x04001B45 RID: 6981
		NONE,
		// Token: 0x04001B46 RID: 6982
		COLLISION,
		// Token: 0x04001B47 RID: 6983
		WORLD_Y
	}
}
