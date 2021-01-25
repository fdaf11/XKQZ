using System;
using UnityEngine;

// Token: 0x020003D7 RID: 983
public class NcParticleSpiral : NcEffectBehaviour
{
	// Token: 0x06001763 RID: 5987 RVA: 0x0000F2DC File Offset: 0x0000D4DC
	public override int GetAnimationState()
	{
		if (!base.enabled || !NcEffectBehaviour.IsActive(base.gameObject))
		{
			return -1;
		}
		if (NcEffectBehaviour.GetEngineTime() < this.m_fStartTime + this.m_fDelayTime + 0.1f)
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x000C0734 File Offset: 0x000BE934
	public void RandomizeEditor()
	{
		this.m_nNumberOfArms = Random.Range(1, 10);
		this.m_nParticlesPerArm = Random.Range(20, 60);
		this.m_fParticleSeparation = Random.Range(-0.3f, 0.3f);
		this.m_fTurnDistance = Random.Range(-1.5f, 1.5f);
		this.m_fVerticalTurnDistance = Random.Range(0f, 0.5f);
		this.m_fOriginOffset = Random.Range(-3f, 3f);
		this.m_fTurnSpeed = Random.Range(-180f, 180f);
		this.m_fFadeValue = Random.Range(-1f, 1f);
		this.m_fSizeValue = Random.Range(-2f, 2f);
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x000C07F4 File Offset: 0x000BE9F4
	private void Start()
	{
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
		if (this.m_ParticlePrefab == null)
		{
			ParticleEmitter component = base.GetComponent<ParticleEmitter>();
			if (component == null)
			{
				return;
			}
			component.emit = false;
		}
		this.defaultSettings = this.getSettings();
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x000C0844 File Offset: 0x000BEA44
	private void SpawnEffect()
	{
		GameObject gameObject;
		if (this.m_ParticlePrefab != null)
		{
			gameObject = base.CreateGameObject(this.m_ParticlePrefab);
			if (gameObject == null)
			{
				return;
			}
			base.ChangeParent(base.transform, gameObject.transform, true, null);
		}
		else
		{
			gameObject = base.gameObject;
		}
		ParticleEmitter component = gameObject.GetComponent<ParticleEmitter>();
		if (component == null)
		{
			return;
		}
		component.emit = false;
		component.useWorldSpace = false;
		ParticleAnimator component2 = component.transform.GetComponent<ParticleAnimator>();
		if (component2 != null)
		{
			component2.autodestruct = true;
		}
		component.Emit(this.m_nNumberOfArms * this.m_nParticlesPerArm);
		Particle[] particles = component.particles;
		float num = 6.2831855f / (float)this.m_nNumberOfArms;
		for (int i = 0; i < this.m_nNumberOfArms; i++)
		{
			float num2 = 0f;
			float num3 = (float)i * num;
			for (int j = 0; j < this.m_nParticlesPerArm; j++)
			{
				int num4 = i * this.m_nParticlesPerArm + j;
				float num5 = this.m_fOriginOffset + this.m_fTurnDistance * num2;
				Vector3 vector = gameObject.transform.localPosition;
				vector.x += num5 * Mathf.Cos(num2);
				vector.z += num5 * Mathf.Sin(num2);
				float x = vector.x * Mathf.Cos(num3) + vector.z * Mathf.Sin(num3);
				float z = -vector.x * Mathf.Sin(num3) + vector.z * Mathf.Cos(num3);
				vector.x = x;
				vector.z = z;
				vector.y += (float)j * this.m_fVerticalTurnDistance;
				if (component.useWorldSpace)
				{
					vector = base.transform.TransformPoint(vector);
				}
				particles[num4].position = vector;
				num2 += this.m_fParticleSeparation;
				if (this.m_fFadeValue != 0f)
				{
					particles[num4].energy = particles[num4].energy * (1f - Mathf.Abs(this.m_fFadeValue)) + particles[num4].energy * Mathf.Abs(this.m_fFadeValue) * (float)((this.m_fFadeValue >= 0f) ? (j + 1) : (this.m_nParticlesPerArm - j)) / (float)this.m_nParticlesPerArm;
				}
				if (this.m_fSizeValue != 0f)
				{
					Particle[] array = particles;
					int num6 = num4;
					array[num6].size = array[num6].size + Mathf.Abs(this.m_fSizeValue) * (float)((this.m_fSizeValue >= 0f) ? (j + 1) : (this.m_nParticlesPerArm - j)) / (float)this.m_nParticlesPerArm;
				}
			}
		}
		component.particles = particles;
	}

	// Token: 0x06001767 RID: 5991 RVA: 0x000C0B38 File Offset: 0x000BED38
	private void Update()
	{
		if (NcEffectBehaviour.GetEngineTime() < this.m_fStartTime + this.m_fDelayTime)
		{
			return;
		}
		if (this.m_fTurnSpeed != 0f)
		{
			base.transform.Rotate(base.transform.up * NcEffectBehaviour.GetEngineDeltaTime() * this.m_fTurnSpeed, 0);
		}
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x000C0B9C File Offset: 0x000BED9C
	private void LateUpdate()
	{
		if (NcEffectBehaviour.GetEngineTime() < this.m_fStartTime + this.m_fDelayTime)
		{
			return;
		}
		float num = NcEffectBehaviour.GetEngineTime() - this.timeOfLastSpawn;
		if (this.m_fSpawnRate <= num && this.spawnCount < this.m_nNumberOfSpawns)
		{
			this.SpawnEffect();
			this.timeOfLastSpawn = NcEffectBehaviour.GetEngineTime();
			this.spawnCount++;
		}
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x000C0C0C File Offset: 0x000BEE0C
	public NcParticleSpiral.SpiralSettings getSettings()
	{
		NcParticleSpiral.SpiralSettings result;
		result.numArms = this.m_nNumberOfArms;
		result.numPPA = this.m_nParticlesPerArm;
		result.partSep = this.m_fParticleSeparation;
		result.turnDist = this.m_fTurnDistance;
		result.vertDist = this.m_fVerticalTurnDistance;
		result.originOffset = this.m_fOriginOffset;
		result.turnSpeed = this.m_fTurnSpeed;
		result.fade = this.m_fFadeValue;
		result.size = this.m_fSizeValue;
		return result;
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x000C0C90 File Offset: 0x000BEE90
	public NcParticleSpiral.SpiralSettings resetEffect(bool killCurrent, NcParticleSpiral.SpiralSettings settings)
	{
		if (killCurrent)
		{
			this.killCurrentEffects();
		}
		this.m_nNumberOfArms = settings.numArms;
		this.m_nParticlesPerArm = settings.numPPA;
		this.m_fParticleSeparation = settings.partSep;
		this.m_fTurnDistance = settings.turnDist;
		this.m_fVerticalTurnDistance = settings.vertDist;
		this.m_fOriginOffset = settings.originOffset;
		this.m_fTurnSpeed = settings.turnSpeed;
		this.m_fFadeValue = settings.fade;
		this.m_fSizeValue = settings.size;
		this.SpawnEffect();
		this.timeOfLastSpawn = NcEffectBehaviour.GetEngineTime();
		this.spawnCount++;
		return this.getSettings();
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x0000F31B File Offset: 0x0000D51B
	public NcParticleSpiral.SpiralSettings resetEffectToDefaults(bool killCurrent)
	{
		return this.resetEffect(killCurrent, this.defaultSettings);
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x0000F32A File Offset: 0x0000D52A
	public NcParticleSpiral.SpiralSettings randomizeEffect(bool killCurrent)
	{
		if (killCurrent)
		{
			this.killCurrentEffects();
		}
		this.RandomizeEditor();
		this.SpawnEffect();
		this.timeOfLastSpawn = NcEffectBehaviour.GetEngineTime();
		this.spawnCount++;
		return this.getSettings();
	}

	// Token: 0x0600176D RID: 5997 RVA: 0x000C0D44 File Offset: 0x000BEF44
	private void killCurrentEffects()
	{
		ParticleEmitter[] componentsInChildren = base.transform.GetComponentsInChildren<ParticleEmitter>();
		foreach (ParticleEmitter particleEmitter in componentsInChildren)
		{
			Debug.Log("resetEffect killing: " + particleEmitter.name);
			ParticleAnimator component = particleEmitter.transform.GetComponent<ParticleAnimator>();
			if (component != null)
			{
				component.autodestruct = true;
			}
			Particle[] particles = particleEmitter.particles;
			for (int j = 0; j < particles.Length; j++)
			{
				particles[j].energy = 0.1f;
			}
			particleEmitter.particles = particles;
		}
	}

	// Token: 0x0600176E RID: 5998 RVA: 0x0000F363 File Offset: 0x0000D563
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fDelayTime /= fSpeedRate;
		this.m_fTurnSpeed *= fSpeedRate;
	}

	// Token: 0x04001BC9 RID: 7113
	protected const int Min_numArms = 1;

	// Token: 0x04001BCA RID: 7114
	protected const int Max_numArms = 10;

	// Token: 0x04001BCB RID: 7115
	protected const int Min_numPPA = 20;

	// Token: 0x04001BCC RID: 7116
	protected const int Max_numPPA = 60;

	// Token: 0x04001BCD RID: 7117
	protected const float Min_partSep = -0.3f;

	// Token: 0x04001BCE RID: 7118
	protected const float Max_partSep = 0.3f;

	// Token: 0x04001BCF RID: 7119
	protected const float Min_turnDist = -1.5f;

	// Token: 0x04001BD0 RID: 7120
	protected const float Max_turnDist = 1.5f;

	// Token: 0x04001BD1 RID: 7121
	protected const float Min_vertDist = 0f;

	// Token: 0x04001BD2 RID: 7122
	protected const float Max_vertDist = 0.5f;

	// Token: 0x04001BD3 RID: 7123
	protected const float Min_originOffset = -3f;

	// Token: 0x04001BD4 RID: 7124
	protected const float Max_originOffset = 3f;

	// Token: 0x04001BD5 RID: 7125
	protected const float Min_turnSpeed = -180f;

	// Token: 0x04001BD6 RID: 7126
	protected const float Max_turnSpeed = 180f;

	// Token: 0x04001BD7 RID: 7127
	protected const float Min_fade = -1f;

	// Token: 0x04001BD8 RID: 7128
	protected const float Max_fade = 1f;

	// Token: 0x04001BD9 RID: 7129
	protected const float Min_size = -2f;

	// Token: 0x04001BDA RID: 7130
	protected const float Max_size = 2f;

	// Token: 0x04001BDB RID: 7131
	public float m_fDelayTime;

	// Token: 0x04001BDC RID: 7132
	protected float m_fStartTime;

	// Token: 0x04001BDD RID: 7133
	public GameObject m_ParticlePrefab;

	// Token: 0x04001BDE RID: 7134
	public int m_nNumberOfArms = 2;

	// Token: 0x04001BDF RID: 7135
	public int m_nParticlesPerArm = 100;

	// Token: 0x04001BE0 RID: 7136
	public float m_fParticleSeparation = 0.05f;

	// Token: 0x04001BE1 RID: 7137
	public float m_fTurnDistance = 0.5f;

	// Token: 0x04001BE2 RID: 7138
	public float m_fVerticalTurnDistance;

	// Token: 0x04001BE3 RID: 7139
	public float m_fOriginOffset;

	// Token: 0x04001BE4 RID: 7140
	public float m_fTurnSpeed;

	// Token: 0x04001BE5 RID: 7141
	public float m_fFadeValue;

	// Token: 0x04001BE6 RID: 7142
	public float m_fSizeValue;

	// Token: 0x04001BE7 RID: 7143
	public int m_nNumberOfSpawns = 9999999;

	// Token: 0x04001BE8 RID: 7144
	public float m_fSpawnRate = 5f;

	// Token: 0x04001BE9 RID: 7145
	private float timeOfLastSpawn = -1000f;

	// Token: 0x04001BEA RID: 7146
	private int spawnCount;

	// Token: 0x04001BEB RID: 7147
	private int totParticles;

	// Token: 0x04001BEC RID: 7148
	private NcParticleSpiral.SpiralSettings defaultSettings;

	// Token: 0x020003D8 RID: 984
	public struct SpiralSettings
	{
		// Token: 0x04001BED RID: 7149
		public int numArms;

		// Token: 0x04001BEE RID: 7150
		public int numPPA;

		// Token: 0x04001BEF RID: 7151
		public float partSep;

		// Token: 0x04001BF0 RID: 7152
		public float turnDist;

		// Token: 0x04001BF1 RID: 7153
		public float vertDist;

		// Token: 0x04001BF2 RID: 7154
		public float originOffset;

		// Token: 0x04001BF3 RID: 7155
		public float turnSpeed;

		// Token: 0x04001BF4 RID: 7156
		public float fade;

		// Token: 0x04001BF5 RID: 7157
		public float size;
	}
}
