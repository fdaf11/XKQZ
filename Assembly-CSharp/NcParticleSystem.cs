using System;
using System.Reflection;
using UnityEngine;

// Token: 0x020003D9 RID: 985
public class NcParticleSystem : NcEffectBehaviour
{
	// Token: 0x06001770 RID: 6000 RVA: 0x0000F381 File Offset: 0x0000D581
	public void SetDisableEmit()
	{
		this.m_bDisabledEmit = true;
	}

	// Token: 0x06001771 RID: 6001 RVA: 0x0000F38A File Offset: 0x0000D58A
	public bool IsShuriken()
	{
		return base.particleSystem != null;
	}

	// Token: 0x06001772 RID: 6002 RVA: 0x0000F398 File Offset: 0x0000D598
	public bool IsLegacy()
	{
		return base.particleEmitter != null && base.particleEmitter.enabled;
	}

	// Token: 0x06001773 RID: 6003 RVA: 0x000C0EC8 File Offset: 0x000BF0C8
	public override int GetAnimationState()
	{
		if (!base.enabled || !NcEffectBehaviour.IsActive(base.gameObject))
		{
			return -1;
		}
		if (this.m_bBurst)
		{
			if (0 >= this.m_nBurstRepeatCount)
			{
				return 1;
			}
			if (this.m_nCreateCount < this.m_nBurstRepeatCount)
			{
				return 1;
			}
			return 0;
		}
		else
		{
			if (0f < this.m_fStartDelayTime)
			{
				return 1;
			}
			if (0f >= this.m_fEmitTime || this.m_fSleepTime > 0f)
			{
				return -1;
			}
			if (this.m_nCreateCount < 1)
			{
				return 1;
			}
			return 0;
		}
	}

	// Token: 0x06001774 RID: 6004 RVA: 0x0000F3B9 File Offset: 0x0000D5B9
	public bool IsMeshParticleEmitter()
	{
		return this.m_bMeshParticleEmitter;
	}

	// Token: 0x06001775 RID: 6005 RVA: 0x000C0F64 File Offset: 0x000BF164
	public void ResetParticleEmit(bool bClearOldParticle)
	{
		this.m_bDisabledEmit = false;
		this.m_OldPos = base.transform.position;
		this.Init();
		if (bClearOldParticle && this.m_pe != null)
		{
			this.m_pe.ClearParticles();
		}
		if (this.m_bBurst || 0f < this.m_fStartDelayTime)
		{
			this.SetEnableParticle(false);
		}
		else
		{
			this.SetEnableParticle(true);
		}
	}

	// Token: 0x06001776 RID: 6006 RVA: 0x000C0FE0 File Offset: 0x000BF1E0
	private void Awake()
	{
		if (this.IsShuriken())
		{
			this.m_ps = base.particleSystem;
		}
		else
		{
			this.m_pe = base.GetComponent<ParticleEmitter>();
			this.m_pa = base.GetComponent<ParticleAnimator>();
			this.m_pr = base.GetComponent<ParticleRenderer>();
			if (this.m_pe != null)
			{
				this.m_bMeshParticleEmitter = this.m_pe.ToString().Contains("MeshParticleEmitter");
			}
		}
	}

	// Token: 0x06001777 RID: 6007 RVA: 0x0000F3C1 File Offset: 0x0000D5C1
	private void OnEnable()
	{
		if (this.m_bScaleWithTransform)
		{
			this.AddRenderEventCall();
		}
		this.m_OldPos = base.transform.position;
	}

	// Token: 0x06001778 RID: 6008 RVA: 0x0000F3E5 File Offset: 0x0000D5E5
	private void OnDisable()
	{
		if (this.m_bScaleWithTransform)
		{
			this.RemoveRenderEventCall();
		}
	}

	// Token: 0x06001779 RID: 6009 RVA: 0x0000F3F8 File Offset: 0x0000D5F8
	private void Init()
	{
		this.m_bSleep = false;
		this.m_nCreateCount = 0;
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
		this.m_fEmitStartTime = 0f;
	}

	// Token: 0x0600177A RID: 6010 RVA: 0x000C105C File Offset: 0x000BF25C
	private void Start()
	{
		if (this.m_bDisabledEmit)
		{
			return;
		}
		this.m_bStart = true;
		this.Init();
		if (this.IsShuriken())
		{
			this.ShurikenInitParticle();
		}
		else
		{
			this.LegacyInitParticle();
		}
		if (this.m_bBurst || 0f < this.m_fStartDelayTime)
		{
			this.SetEnableParticle(false);
		}
	}

	// Token: 0x0600177B RID: 6011 RVA: 0x000C10C0 File Offset: 0x000BF2C0
	private void Update()
	{
		if (this.m_bDisabledEmit)
		{
			return;
		}
		if (this.m_fEmitStartTime == 0f)
		{
			if (0f < this.m_fStartDelayTime)
			{
				if (this.m_fStartTime + this.m_fStartDelayTime <= NcEffectBehaviour.GetEngineTime())
				{
					this.m_fEmitStartTime = NcEffectBehaviour.GetEngineTime();
					this.m_fDurationStartTime = NcEffectBehaviour.GetEngineTime();
					this.SetEnableParticle(true);
				}
				return;
			}
			this.m_fEmitStartTime = NcEffectBehaviour.GetEngineTime();
			this.m_fDurationStartTime = NcEffectBehaviour.GetEngineTime();
		}
		if (this.m_bBurst)
		{
			if (this.m_fDurationStartTime <= NcEffectBehaviour.GetEngineTime())
			{
				if (this.m_nBurstRepeatCount == 0 || this.m_nCreateCount < this.m_nBurstRepeatCount)
				{
					this.m_fDurationStartTime = this.m_fBurstRepeatTime + NcEffectBehaviour.GetEngineTime();
					this.m_nCreateCount++;
					if (this.IsShuriken())
					{
						this.m_ps.Emit(this.m_fBurstEmissionCount);
					}
					else if (this.m_pe != null)
					{
						this.m_pe.Emit(this.m_fBurstEmissionCount);
					}
				}
			}
		}
		else if (this.m_bSleep)
		{
			if (this.m_fEmitStartTime + this.m_fEmitTime + this.m_fSleepTime < NcEffectBehaviour.GetEngineTime())
			{
				this.SetEnableParticle(true);
				this.m_fEmitStartTime = NcEffectBehaviour.GetEngineTime();
				this.m_bSleep = false;
			}
		}
		else if (0f < this.m_fEmitTime && this.m_fEmitStartTime + this.m_fEmitTime < NcEffectBehaviour.GetEngineTime())
		{
			this.m_nCreateCount++;
			this.SetEnableParticle(false);
			if (0f < this.m_fSleepTime)
			{
				this.m_bSleep = true;
			}
			else
			{
				this.SetDisableEmit();
			}
		}
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x000C1290 File Offset: 0x000BF490
	private void FixedUpdate()
	{
		if (this.m_ParticleDestruct != NcParticleSystem.ParticleDestruct.NONE)
		{
			bool flag = false;
			if (this.IsShuriken())
			{
				if (this.m_ps != null)
				{
					this.AllocateParticleSystem(ref this.m_BufColliderOriParts);
					this.AllocateParticleSystem(ref this.m_BufColliderConParts);
					this.m_ps.GetParticles(this.m_BufColliderOriParts);
					this.m_ps.GetParticles(this.m_BufColliderConParts);
					this.ShurikenScaleParticle(this.m_BufColliderConParts, this.m_ps.particleCount, this.m_bScaleWithTransform, true);
					for (int i = 0; i < this.m_ps.particleCount; i++)
					{
						bool flag2 = false;
						Vector3 vector;
						if (this.m_bWorldSpace)
						{
							vector = this.m_BufColliderConParts[i].position;
						}
						else
						{
							vector = base.transform.TransformPoint(this.m_BufColliderConParts[i].position);
						}
						if (this.m_ParticleDestruct == NcParticleSystem.ParticleDestruct.COLLISION)
						{
							if (Physics.CheckSphere(vector, this.m_fCollisionRadius, this.m_CollisionLayer))
							{
								flag2 = true;
							}
						}
						else if (this.m_ParticleDestruct == NcParticleSystem.ParticleDestruct.WORLD_Y && vector.y <= this.m_fDestructPosY)
						{
							flag2 = true;
						}
						if (flag2 && 0f < this.m_BufColliderOriParts[i].lifetime)
						{
							this.m_BufColliderOriParts[i].lifetime = 0f;
							flag = true;
							this.CreateAttachPrefab(vector, this.m_BufColliderConParts[i].size * this.m_fPrefabScale);
						}
					}
					if (flag)
					{
						this.m_ps.SetParticles(this.m_BufColliderOriParts, this.m_ps.particleCount);
					}
				}
			}
			else if (this.m_pe != null)
			{
				Particle[] particles = this.m_pe.particles;
				Particle[] particles2 = this.m_pe.particles;
				this.LegacyScaleParticle(particles2, this.m_bScaleWithTransform, true);
				for (int j = 0; j < particles2.Length; j++)
				{
					bool flag3 = false;
					Vector3 vector;
					if (this.m_bWorldSpace)
					{
						vector = particles2[j].position;
					}
					else
					{
						vector = base.transform.TransformPoint(particles2[j].position);
					}
					if (this.m_ParticleDestruct == NcParticleSystem.ParticleDestruct.COLLISION)
					{
						if (Physics.CheckSphere(vector, this.m_fCollisionRadius, this.m_CollisionLayer))
						{
							flag3 = true;
						}
					}
					else if (this.m_ParticleDestruct == NcParticleSystem.ParticleDestruct.WORLD_Y && vector.y <= this.m_fDestructPosY)
					{
						flag3 = true;
					}
					if (flag3 && 0f < particles[j].energy)
					{
						particles[j].energy = 0f;
						flag = true;
						this.CreateAttachPrefab(vector, particles2[j].size * this.m_fPrefabScale);
					}
				}
				if (flag)
				{
					this.m_pe.particles = particles;
				}
			}
		}
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x0000F41E File Offset: 0x0000D61E
	private void OnPreRender()
	{
		if (!this.m_bStart)
		{
			return;
		}
		if (this.m_bScaleWithTransform)
		{
			this.m_bScalePreRender = true;
			if (this.IsShuriken())
			{
				this.ShurikenSetRuntimeParticleScale(true);
			}
			else
			{
				this.LegacySetRuntimeParticleScale(true);
			}
		}
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x000C1590 File Offset: 0x000BF790
	private void OnPostRender()
	{
		if (!this.m_bStart)
		{
			return;
		}
		if (this.m_bScalePreRender)
		{
			if (this.IsShuriken())
			{
				this.ShurikenSetRuntimeParticleScale(false);
			}
			else
			{
				this.LegacySetRuntimeParticleScale(false);
			}
		}
		this.m_OldPos = base.transform.position;
		this.m_bScalePreRender = false;
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x000C15EC File Offset: 0x000BF7EC
	private void CreateAttachPrefab(Vector3 position, float size)
	{
		if (this.m_AttachPrefab == null)
		{
			return;
		}
		GameObject gameObject = base.CreateGameObject(this.m_AttachPrefab, this.m_AttachPrefab.transform.position + position, this.m_AttachPrefab.transform.rotation);
		if (gameObject == null)
		{
			return;
		}
		base.ChangeParent(NcEffectBehaviour.GetRootInstanceEffect().transform, gameObject.transform, false, null);
		NcTransformTool.CopyLossyToLocalScale(gameObject.transform.lossyScale * size, gameObject.transform);
		NsEffectManager.AdjustSpeedRuntime(gameObject, this.m_fPrefabSpeed);
		if (0f < this.m_fPrefabLifeTime)
		{
			NcAutoDestruct ncAutoDestruct = gameObject.GetComponent<NcAutoDestruct>();
			if (ncAutoDestruct == null)
			{
				ncAutoDestruct = gameObject.AddComponent<NcAutoDestruct>();
			}
			ncAutoDestruct.m_fLifeTime = this.m_fPrefabLifeTime;
		}
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x000C16C4 File Offset: 0x000BF8C4
	private void AddRenderEventCall()
	{
		foreach (Camera camera in Camera.allCameras)
		{
			NsRenderManager nsRenderManager = camera.GetComponent<NsRenderManager>();
			if (nsRenderManager == null)
			{
				nsRenderManager = camera.gameObject.AddComponent<NsRenderManager>();
			}
			nsRenderManager.AddRenderEventCall(this);
		}
	}

	// Token: 0x06001781 RID: 6017 RVA: 0x000C1718 File Offset: 0x000BF918
	private void RemoveRenderEventCall()
	{
		foreach (Camera camera in Camera.allCameras)
		{
			NsRenderManager component = camera.GetComponent<NsRenderManager>();
			if (component != null)
			{
				component.RemoveRenderEventCall(this);
			}
		}
	}

	// Token: 0x06001782 RID: 6018 RVA: 0x0000F45C File Offset: 0x0000D65C
	private void SetEnableParticle(bool bEnable)
	{
		if (this.m_ps != null)
		{
			this.m_ps.enableEmission = bEnable;
		}
		if (this.m_pe != null)
		{
			this.m_pe.emit = bEnable;
		}
	}

	// Token: 0x06001783 RID: 6019 RVA: 0x0000F498 File Offset: 0x0000D698
	private void ClearParticle()
	{
		if (this.m_ps != null)
		{
			this.m_ps.Clear(false);
		}
		if (this.m_pe != null)
		{
			this.m_pe.ClearParticles();
		}
	}

	// Token: 0x06001784 RID: 6020 RVA: 0x0000F4D3 File Offset: 0x0000D6D3
	public float GetScaleMinMeshNormalVelocity()
	{
		return this.m_fLegacyMinMeshNormalVelocity * ((!this.m_bScaleWithTransform) ? 1f : NcTransformTool.GetTransformScaleMeanValue(base.transform));
	}

	// Token: 0x06001785 RID: 6021 RVA: 0x0000F4FC File Offset: 0x0000D6FC
	public float GetScaleMaxMeshNormalVelocity()
	{
		return this.m_fLegacyMaxMeshNormalVelocity * ((!this.m_bScaleWithTransform) ? 1f : NcTransformTool.GetTransformScaleMeanValue(base.transform));
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x0000F525 File Offset: 0x0000D725
	private void LegacyInitParticle()
	{
		if (this.m_pe != null)
		{
			this.LegacySetParticle();
		}
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x000C1760 File Offset: 0x000BF960
	private void LegacySetParticle()
	{
		ParticleEmitter pe = this.m_pe;
		ParticleAnimator pa = this.m_pa;
		ParticleRenderer pr = this.m_pr;
		if (pe == null || pr == null)
		{
			return;
		}
		if (this.m_bLegacyRuntimeScale)
		{
			Vector3 vector = Vector3.one * this.m_fStartSpeedRate;
			float fStartSpeedRate = this.m_fStartSpeedRate;
			pe.minSize *= this.m_fStartSizeRate;
			pe.maxSize *= this.m_fStartSizeRate;
			pe.minEnergy *= this.m_fStartLifeTimeRate;
			pe.maxEnergy *= this.m_fStartLifeTimeRate;
			pe.minEmission *= this.m_fStartEmissionRate;
			pe.maxEmission *= this.m_fStartEmissionRate;
			pe.worldVelocity = Vector3.Scale(pe.worldVelocity, vector);
			pe.localVelocity = Vector3.Scale(pe.localVelocity, vector);
			pe.rndVelocity = Vector3.Scale(pe.rndVelocity, vector);
			pe.angularVelocity *= fStartSpeedRate;
			pe.rndAngularVelocity *= fStartSpeedRate;
			pe.emitterVelocityScale *= fStartSpeedRate;
			if (pa != null)
			{
				pa.rndForce = Vector3.Scale(pa.rndForce, vector);
				pa.force = Vector3.Scale(pa.force, vector);
			}
			pr.lengthScale *= this.m_fRenderLengthRate;
		}
		else
		{
			Vector3 vector2 = ((!this.m_bScaleWithTransform) ? Vector3.one : pe.transform.lossyScale) * this.m_fStartSpeedRate;
			float num = ((!this.m_bScaleWithTransform) ? 1f : NcTransformTool.GetTransformScaleMeanValue(pe.transform)) * this.m_fStartSpeedRate;
			float num2 = ((!this.m_bScaleWithTransform) ? 1f : NcTransformTool.GetTransformScaleMeanValue(pe.transform)) * this.m_fStartSizeRate;
			pe.minSize *= num2;
			pe.maxSize *= num2;
			pe.minEnergy *= this.m_fStartLifeTimeRate;
			pe.maxEnergy *= this.m_fStartLifeTimeRate;
			pe.minEmission *= this.m_fStartEmissionRate;
			pe.maxEmission *= this.m_fStartEmissionRate;
			pe.worldVelocity = Vector3.Scale(pe.worldVelocity, vector2);
			pe.localVelocity = Vector3.Scale(pe.localVelocity, vector2);
			pe.rndVelocity = Vector3.Scale(pe.rndVelocity, vector2);
			pe.angularVelocity *= num;
			pe.rndAngularVelocity *= num;
			pe.emitterVelocityScale *= num;
			if (pa != null)
			{
				pa.rndForce = Vector3.Scale(pa.rndForce, vector2);
				pa.force = Vector3.Scale(pa.force, vector2);
			}
			pr.lengthScale *= this.m_fRenderLengthRate;
		}
	}

	// Token: 0x06001788 RID: 6024 RVA: 0x000C1A70 File Offset: 0x000BFC70
	private void LegacyParticleSpeed(float fSpeed)
	{
		ParticleEmitter pe = this.m_pe;
		ParticleAnimator pa = this.m_pa;
		ParticleRenderer pr = this.m_pr;
		if (pe == null || pr == null)
		{
			return;
		}
		Vector3 vector = Vector3.one * fSpeed;
		pe.minEnergy /= fSpeed;
		pe.maxEnergy /= fSpeed;
		pe.worldVelocity = Vector3.Scale(pe.worldVelocity, vector);
		pe.localVelocity = Vector3.Scale(pe.localVelocity, vector);
		pe.rndVelocity = Vector3.Scale(pe.rndVelocity, vector);
		pe.angularVelocity *= fSpeed;
		pe.rndAngularVelocity *= fSpeed;
		pe.emitterVelocityScale *= fSpeed;
		if (pa != null)
		{
			pa.rndForce = Vector3.Scale(pa.rndForce, vector);
			pa.force = Vector3.Scale(pa.force, vector);
		}
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x000C1B64 File Offset: 0x000BFD64
	private void LegacySetRuntimeParticleScale(bool bScale)
	{
		if (!this.m_bLegacyRuntimeScale)
		{
			return;
		}
		if (this.m_pe != null)
		{
			Particle[] particles = this.m_pe.particles;
			this.m_pe.particles = this.LegacyScaleParticle(particles, bScale, true);
		}
	}

	// Token: 0x0600178A RID: 6026 RVA: 0x000C1BB0 File Offset: 0x000BFDB0
	public Particle[] LegacyScaleParticle(Particle[] parts, bool bScale, bool bPosUpdate)
	{
		float num;
		if (bScale)
		{
			num = NcTransformTool.GetTransformScaleMeanValue(base.transform);
		}
		else
		{
			num = 1f / NcTransformTool.GetTransformScaleMeanValue(base.transform);
		}
		for (int i = 0; i < parts.Length; i++)
		{
			if (!this.IsMeshParticleEmitter())
			{
				if (this.m_bWorldSpace)
				{
					if (bPosUpdate)
					{
						Vector3 vector = this.m_OldPos - base.transform.position;
						if (bScale)
						{
							int num2 = i;
							parts[num2].position = parts[num2].position - vector * (1f - 1f / num);
						}
					}
					int num3 = i;
					parts[num3].position = parts[num3].position - base.transform.position;
					int num4 = i;
					parts[num4].position = parts[num4].position * num;
					int num5 = i;
					parts[num5].position = parts[num5].position + base.transform.position;
				}
				else
				{
					int num6 = i;
					parts[num6].position = parts[num6].position * num;
				}
			}
			int num7 = i;
			parts[num7].angularVelocity = parts[num7].angularVelocity * num;
			int num8 = i;
			parts[num8].velocity = parts[num8].velocity * num;
			int num9 = i;
			parts[num9].size = parts[num9].size * num;
		}
		return parts;
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x000C1D18 File Offset: 0x000BFF18
	private void ShurikenInitParticle()
	{
		if (this.m_ps != null)
		{
			this.m_ps.startSize *= this.m_fStartSizeRate;
			this.m_ps.startLifetime *= this.m_fStartLifeTimeRate;
			this.m_ps.emissionRate *= this.m_fStartEmissionRate;
			this.m_ps.startSpeed *= this.m_fStartSpeedRate;
		}
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x0000F53E File Offset: 0x0000D73E
	private void AllocateParticleSystem(ref ParticleSystem.Particle[] tmpPsParts)
	{
		if (tmpPsParts == null || tmpPsParts.Length < this.m_ps.particleCount)
		{
			tmpPsParts = new ParticleSystem.Particle[this.m_ps.particleCount + 50];
		}
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x000C1D98 File Offset: 0x000BFF98
	private void ShurikenSetRuntimeParticleScale(bool bScale)
	{
		if (this.m_ps != null)
		{
			this.AllocateParticleSystem(ref this.m_BufPsParts);
			this.m_ps.GetParticles(this.m_BufPsParts);
			this.m_BufPsParts = this.ShurikenScaleParticle(this.m_BufPsParts, this.m_ps.particleCount, bScale, true);
			this.m_ps.SetParticles(this.m_BufPsParts, this.m_ps.particleCount);
		}
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x000C1E10 File Offset: 0x000C0010
	public ParticleSystem.Particle[] ShurikenScaleParticle(ParticleSystem.Particle[] parts, int nCount, bool bScale, bool bPosUpdate)
	{
		float num;
		if (bScale)
		{
			num = NcTransformTool.GetTransformScaleMeanValue(base.transform);
		}
		else
		{
			num = 1f / NcTransformTool.GetTransformScaleMeanValue(base.transform);
		}
		for (int i = 0; i < nCount; i++)
		{
			if (this.m_bWorldSpace)
			{
				if (bPosUpdate)
				{
					Vector3 vector = this.m_OldPos - base.transform.position;
					if (bScale)
					{
						int num2 = i;
						parts[num2].position = parts[num2].position - vector * (1f - 1f / num);
					}
				}
				int num3 = i;
				parts[num3].position = parts[num3].position - base.transform.position;
				int num4 = i;
				parts[num4].position = parts[num4].position * num;
				int num5 = i;
				parts[num5].position = parts[num5].position + base.transform.position;
			}
			else
			{
				int num6 = i;
				parts[num6].position = parts[num6].position * num;
			}
			int num7 = i;
			parts[num7].size = parts[num7].size * num;
		}
		return parts;
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x000C1F40 File Offset: 0x000C0140
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fStartDelayTime /= fSpeedRate;
		this.m_fBurstRepeatTime /= fSpeedRate;
		this.m_fEmitTime /= fSpeedRate;
		this.m_fSleepTime /= fSpeedRate;
		this.m_fShurikenSpeedRate *= fSpeedRate;
		this.LegacyParticleSpeed(fSpeedRate);
		this.m_fPrefabLifeTime /= fSpeedRate;
		this.m_fPrefabSpeed *= fSpeedRate;
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x0000F570 File Offset: 0x0000D770
	public override void OnSetReplayState()
	{
		base.OnSetReplayState();
		this.m_pe = base.GetComponent<ParticleEmitter>();
		this.m_pa = base.GetComponent<ParticleAnimator>();
		if (this.m_pa != null)
		{
			this.m_pa.autodestruct = false;
		}
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x0000F5AD File Offset: 0x0000D7AD
	public override void OnResetReplayStage(bool bClearOldParticle)
	{
		base.OnResetReplayStage(bClearOldParticle);
		this.ResetParticleEmit(bClearOldParticle);
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x000C1FB8 File Offset: 0x000C01B8
	public static void Ng_SetProperty(object srcObj, string fieldName, object newValue)
	{
		PropertyInfo property = srcObj.GetType().GetProperty(fieldName, 52);
		if (property != null && property.CanWrite)
		{
			property.SetValue(srcObj, newValue, null);
		}
		else
		{
			Debug.LogWarning(fieldName + " could not be write.");
		}
	}

	// Token: 0x06001793 RID: 6035 RVA: 0x000C2004 File Offset: 0x000C0204
	public static object Ng_GetProperty(object srcObj, string fieldName)
	{
		object result = null;
		PropertyInfo property = srcObj.GetType().GetProperty(fieldName, 52);
		if (property != null && property.CanRead && property.GetIndexParameters().Length == 0)
		{
			result = property.GetValue(srcObj, null);
		}
		else
		{
			Debug.LogWarning(fieldName + " could not be read.");
		}
		return result;
	}

	// Token: 0x04001BF6 RID: 7158
	protected const int m_nAllocBufCount = 50;

	// Token: 0x04001BF7 RID: 7159
	protected bool m_bDisabledEmit;

	// Token: 0x04001BF8 RID: 7160
	public float m_fStartDelayTime;

	// Token: 0x04001BF9 RID: 7161
	public bool m_bBurst;

	// Token: 0x04001BFA RID: 7162
	public float m_fBurstRepeatTime = 0.5f;

	// Token: 0x04001BFB RID: 7163
	public int m_nBurstRepeatCount;

	// Token: 0x04001BFC RID: 7164
	public int m_fBurstEmissionCount = 10;

	// Token: 0x04001BFD RID: 7165
	public float m_fEmitTime;

	// Token: 0x04001BFE RID: 7166
	public float m_fSleepTime;

	// Token: 0x04001BFF RID: 7167
	public bool m_bScaleWithTransform;

	// Token: 0x04001C00 RID: 7168
	public bool m_bWorldSpace = true;

	// Token: 0x04001C01 RID: 7169
	public float m_fStartSizeRate = 1f;

	// Token: 0x04001C02 RID: 7170
	public float m_fStartLifeTimeRate = 1f;

	// Token: 0x04001C03 RID: 7171
	public float m_fStartEmissionRate = 1f;

	// Token: 0x04001C04 RID: 7172
	public float m_fStartSpeedRate = 1f;

	// Token: 0x04001C05 RID: 7173
	public float m_fRenderLengthRate = 1f;

	// Token: 0x04001C06 RID: 7174
	public float m_fLegacyMinMeshNormalVelocity = 10f;

	// Token: 0x04001C07 RID: 7175
	public float m_fLegacyMaxMeshNormalVelocity = 10f;

	// Token: 0x04001C08 RID: 7176
	public float m_fShurikenSpeedRate = 1f;

	// Token: 0x04001C09 RID: 7177
	protected bool m_bStart;

	// Token: 0x04001C0A RID: 7178
	protected Vector3 m_OldPos = Vector3.zero;

	// Token: 0x04001C0B RID: 7179
	protected bool m_bLegacyRuntimeScale = true;

	// Token: 0x04001C0C RID: 7180
	public NcParticleSystem.ParticleDestruct m_ParticleDestruct;

	// Token: 0x04001C0D RID: 7181
	public LayerMask m_CollisionLayer = -1;

	// Token: 0x04001C0E RID: 7182
	public float m_fCollisionRadius = 0.3f;

	// Token: 0x04001C0F RID: 7183
	public float m_fDestructPosY = 0.2f;

	// Token: 0x04001C10 RID: 7184
	public GameObject m_AttachPrefab;

	// Token: 0x04001C11 RID: 7185
	public float m_fPrefabScale = 1f;

	// Token: 0x04001C12 RID: 7186
	public float m_fPrefabSpeed = 1f;

	// Token: 0x04001C13 RID: 7187
	public float m_fPrefabLifeTime = 2f;

	// Token: 0x04001C14 RID: 7188
	protected bool m_bSleep;

	// Token: 0x04001C15 RID: 7189
	protected float m_fStartTime;

	// Token: 0x04001C16 RID: 7190
	protected float m_fDurationStartTime;

	// Token: 0x04001C17 RID: 7191
	protected float m_fEmitStartTime;

	// Token: 0x04001C18 RID: 7192
	protected int m_nCreateCount;

	// Token: 0x04001C19 RID: 7193
	protected bool m_bScalePreRender;

	// Token: 0x04001C1A RID: 7194
	protected bool m_bMeshParticleEmitter;

	// Token: 0x04001C1B RID: 7195
	protected ParticleSystem m_ps;

	// Token: 0x04001C1C RID: 7196
	protected ParticleEmitter m_pe;

	// Token: 0x04001C1D RID: 7197
	protected ParticleAnimator m_pa;

	// Token: 0x04001C1E RID: 7198
	protected ParticleRenderer m_pr;

	// Token: 0x04001C1F RID: 7199
	protected ParticleSystem.Particle[] m_BufPsParts;

	// Token: 0x04001C20 RID: 7200
	protected ParticleSystem.Particle[] m_BufColliderOriParts;

	// Token: 0x04001C21 RID: 7201
	protected ParticleSystem.Particle[] m_BufColliderConParts;

	// Token: 0x020003DA RID: 986
	public enum ParticleDestruct
	{
		// Token: 0x04001C23 RID: 7203
		NONE,
		// Token: 0x04001C24 RID: 7204
		COLLISION,
		// Token: 0x04001C25 RID: 7205
		WORLD_Y
	}
}
