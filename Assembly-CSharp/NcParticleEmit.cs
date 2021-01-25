using System;
using UnityEngine;

// Token: 0x020003D5 RID: 981
public class NcParticleEmit : NcEffectBehaviour
{
	// Token: 0x06001756 RID: 5974 RVA: 0x000C03A0 File Offset: 0x000BE5A0
	public override int GetAnimationState()
	{
		if (this.m_bEnabled && base.enabled && NcEffectBehaviour.IsActive(base.gameObject) && this.m_ParticlePrefab != null)
		{
			if (this.m_AttachType == NcParticleEmit.AttachType.Active && ((this.m_nRepeatCount == 0 && this.m_nCreateCount < 1) || (0f < this.m_fRepeatTime && this.m_nRepeatCount == 0) || (0 < this.m_nRepeatCount && this.m_nCreateCount < this.m_nRepeatCount)))
			{
				return 1;
			}
			if (this.m_AttachType == NcParticleEmit.AttachType.Destroy)
			{
				return 1;
			}
		}
		return 0;
	}

	// Token: 0x06001757 RID: 5975 RVA: 0x0000F214 File Offset: 0x0000D414
	public void UpdateImmediately()
	{
		this.Update();
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x0000F21C File Offset: 0x0000D41C
	public GameObject EmitSharedParticle()
	{
		return this.CreateAttachSharedParticle();
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x0000F224 File Offset: 0x0000D424
	public GameObject GetInstanceObject()
	{
		if (this.m_CreateGameObject == null)
		{
			this.UpdateImmediately();
		}
		return this.m_CreateGameObject;
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x0000F243 File Offset: 0x0000D443
	public void SetEnable(bool bEnable)
	{
		this.m_bEnabled = bEnable;
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x0000F24C File Offset: 0x0000D44C
	private void Awake()
	{
		this.m_bEnabled = (base.enabled && NcEffectBehaviour.IsActive(base.gameObject) && base.GetComponent<NcDontActive>() == null);
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x000C0450 File Offset: 0x000BE650
	private void Update()
	{
		if (this.m_ParticlePrefab == null)
		{
			return;
		}
		if (this.m_AttachType == NcParticleEmit.AttachType.Active)
		{
			if (!this.m_bStartAttach)
			{
				this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
				this.m_bStartAttach = true;
			}
			if (this.m_fStartTime + this.m_fDelayTime <= NcEffectBehaviour.GetEngineTime())
			{
				this.CreateAttachPrefab();
				if ((0f < this.m_fRepeatTime && this.m_nRepeatCount == 0) || this.m_nCreateCount < this.m_nRepeatCount)
				{
					this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
					this.m_fDelayTime = this.m_fRepeatTime;
				}
				else
				{
					base.enabled = false;
				}
			}
		}
	}

	// Token: 0x0600175E RID: 5982 RVA: 0x0000F27E File Offset: 0x0000D47E
	protected override void OnDestroy()
	{
		if (this.m_bEnabled && NcEffectBehaviour.IsSafe() && this.m_AttachType == NcParticleEmit.AttachType.Destroy && this.m_ParticlePrefab != null)
		{
			this.CreateAttachPrefab();
		}
		base.OnDestroy();
	}

	// Token: 0x0600175F RID: 5983 RVA: 0x000C0504 File Offset: 0x000BE704
	private void CreateAttachPrefab()
	{
		this.m_nCreateCount++;
		this.CreateAttachSharedParticle();
		if ((this.m_fRepeatTime == 0f || this.m_AttachType == NcParticleEmit.AttachType.Destroy) && 0 < this.m_nRepeatCount && this.m_nCreateCount < this.m_nRepeatCount)
		{
			this.CreateAttachPrefab();
		}
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x000C0568 File Offset: 0x000BE768
	private GameObject CreateAttachSharedParticle()
	{
		if (this.m_CreateGameObject == null)
		{
			this.m_CreateGameObject = NsSharedManager.inst.GetSharedParticleGameObject(this.m_ParticlePrefab);
		}
		if (this.m_CreateGameObject == null)
		{
			return null;
		}
		Vector3 vector = base.transform.position + this.m_AddStartPos + this.m_ParticlePrefab.transform.position;
		this.m_CreateGameObject.transform.position = new Vector3(Random.Range(-this.m_RandomRange.x, this.m_RandomRange.x) + vector.x, Random.Range(-this.m_RandomRange.y, this.m_RandomRange.y) + vector.y, Random.Range(-this.m_RandomRange.z, this.m_RandomRange.z) + vector.z);
		if (this.m_CreateGameObject.particleEmitter != null)
		{
			this.m_CreateGameObject.particleEmitter.Emit(this.m_EmitCount);
		}
		else
		{
			if (this.m_ps == null)
			{
				this.m_ps = this.m_CreateGameObject.GetComponent<ParticleSystem>();
			}
			if (this.m_ps != null)
			{
				this.m_ps.Emit(this.m_EmitCount);
			}
		}
		return this.m_CreateGameObject;
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x0000F2BE File Offset: 0x0000D4BE
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fDelayTime /= fSpeedRate;
		this.m_fRepeatTime /= fSpeedRate;
	}

	// Token: 0x04001BB8 RID: 7096
	public NcParticleEmit.AttachType m_AttachType;

	// Token: 0x04001BB9 RID: 7097
	public float m_fDelayTime;

	// Token: 0x04001BBA RID: 7098
	public float m_fRepeatTime;

	// Token: 0x04001BBB RID: 7099
	public int m_nRepeatCount;

	// Token: 0x04001BBC RID: 7100
	public GameObject m_ParticlePrefab;

	// Token: 0x04001BBD RID: 7101
	public int m_EmitCount = 10;

	// Token: 0x04001BBE RID: 7102
	public Vector3 m_AddStartPos = Vector3.zero;

	// Token: 0x04001BBF RID: 7103
	public Vector3 m_RandomRange = Vector3.zero;

	// Token: 0x04001BC0 RID: 7104
	protected float m_fStartTime;

	// Token: 0x04001BC1 RID: 7105
	protected int m_nCreateCount;

	// Token: 0x04001BC2 RID: 7106
	protected bool m_bStartAttach;

	// Token: 0x04001BC3 RID: 7107
	protected GameObject m_CreateGameObject;

	// Token: 0x04001BC4 RID: 7108
	protected bool m_bEnabled;

	// Token: 0x04001BC5 RID: 7109
	protected ParticleSystem m_ps;

	// Token: 0x020003D6 RID: 982
	public enum AttachType
	{
		// Token: 0x04001BC7 RID: 7111
		Active,
		// Token: 0x04001BC8 RID: 7112
		Destroy
	}
}
