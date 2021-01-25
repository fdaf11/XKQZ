using System;
using UnityEngine;

namespace Exploder
{
	// Token: 0x020000CF RID: 207
	public class Fragment : MonoBehaviour
	{
		// Token: 0x06000432 RID: 1074 RVA: 0x00004D98 File Offset: 0x00002F98
		public bool IsSleeping()
		{
			if (this.rigid2D)
			{
				return this.rigid2D.IsSleeping();
			}
			return this.rigidBody.IsSleeping();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00004DC1 File Offset: 0x00002FC1
		public void Sleep()
		{
			if (this.rigid2D)
			{
				this.rigid2D.Sleep();
			}
			else
			{
				this.rigidBody.Sleep();
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00004DEE File Offset: 0x00002FEE
		public void WakeUp()
		{
			if (this.rigid2D)
			{
				this.rigid2D.WakeUp();
			}
			else
			{
				this.rigidBody.WakeUp();
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00004E1B File Offset: 0x0000301B
		public void SetConstraints(RigidbodyConstraints constraints)
		{
			if (base.rigidbody)
			{
				this.rigidBody.constraints = constraints;
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00034914 File Offset: 0x00032B14
		public void SetSFX(ExploderObject.SFXOption sfx, bool allowParticle)
		{
			this.audioClip = sfx.FragmentSoundClip;
			if (this.audioClip && !this.audioSource)
			{
				this.audioSource = base.gameObject.AddComponent<AudioSource>();
			}
			if (sfx.FragmentEmitter && allowParticle)
			{
				if (!this.particleChild)
				{
					GameObject gameObject = Object.Instantiate(sfx.FragmentEmitter) as GameObject;
					if (gameObject)
					{
						this.particleChild = new GameObject("Particles");
						this.particleChild.transform.parent = base.gameObject.transform;
						gameObject.transform.parent = this.particleChild.transform;
						this.emmiters = gameObject.GetComponentsInChildren<ParticleEmitter>();
					}
				}
			}
			else if (this.particleChild)
			{
				Object.Destroy(this.particleChild);
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00034A10 File Offset: 0x00032C10
		private void OnCollisionEnter()
		{
			FragmentPool instance = FragmentPool.Instance;
			if (instance.CanPlayHitSound())
			{
				if (this.audioClip && this.audioSource)
				{
					this.audioSource.PlayOneShot(this.audioClip);
				}
				instance.OnFragmentHit();
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00034A68 File Offset: 0x00032C68
		public void DisableColliders(bool disable, bool meshColliders, bool physics2d)
		{
			if (disable)
			{
				if (physics2d)
				{
					Object.Destroy(this.polygonCollider2D);
				}
				else
				{
					if (this.meshCollider)
					{
						Object.Destroy(this.meshCollider);
					}
					if (this.boxCollider)
					{
						Object.Destroy(this.boxCollider);
					}
				}
			}
			else if (physics2d)
			{
				if (!this.polygonCollider2D)
				{
					this.polygonCollider2D = base.gameObject.AddComponent<PolygonCollider2D>();
				}
			}
			else if (meshColliders)
			{
				if (!this.meshCollider)
				{
					this.meshCollider = base.gameObject.AddComponent<MeshCollider>();
				}
			}
			else if (!this.boxCollider)
			{
				this.boxCollider = base.gameObject.AddComponent<BoxCollider>();
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00034B48 File Offset: 0x00032D48
		public void ApplyExplosion(Transform meshTransform, Vector3 centroid, Vector3 mainCentroid, ExploderObject.FragmentOption fragmentOption, bool useForceVector, Vector3 ForceVector, float force, GameObject original, int targetFragments)
		{
			if (this.rigid2D)
			{
				this.ApplyExplosion2D(meshTransform, centroid, mainCentroid, fragmentOption, useForceVector, ForceVector, force, original, targetFragments);
				return;
			}
			Rigidbody rigidbody = this.rigidBody;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			float mass = fragmentOption.Mass;
			bool useGravity = fragmentOption.UseGravity;
			rigidbody.maxAngularVelocity = fragmentOption.MaxAngularVelocity;
			if (fragmentOption.InheritParentPhysicsProperty && original && original.rigidbody)
			{
				Rigidbody rigidbody2 = original.rigidbody;
				vector = rigidbody2.velocity;
				vector2 = rigidbody2.angularVelocity;
				mass = rigidbody2.mass / (float)targetFragments;
				useGravity = rigidbody2.useGravity;
			}
			Vector3 vector3 = (meshTransform.TransformPoint(centroid) - mainCentroid).normalized;
			Vector3 vector4 = fragmentOption.AngularVelocity * ((!fragmentOption.RandomAngularVelocityVector) ? fragmentOption.AngularVelocityVector : Random.onUnitSphere);
			if (useForceVector)
			{
				vector3 = ForceVector;
			}
			rigidbody.velocity = vector3 * force + vector;
			rigidbody.angularVelocity = vector4 + vector2;
			rigidbody.mass = mass;
			this.maxVelocity = fragmentOption.MaxVelocity;
			rigidbody.useGravity = useGravity;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00034C98 File Offset: 0x00032E98
		private void ApplyExplosion2D(Transform meshTransform, Vector3 centroid, Vector3 mainCentroid, ExploderObject.FragmentOption fragmentOption, bool useForceVector, Vector2 ForceVector, float force, GameObject original, int targetFragments)
		{
			Rigidbody2D rigidbody2D = this.rigid2D;
			Vector2 vector = Vector2.zero;
			float num = 0f;
			float mass = fragmentOption.Mass;
			if (fragmentOption.InheritParentPhysicsProperty && original && original.rigidbody2D)
			{
				Rigidbody2D rigidbody2D2 = original.rigidbody2D;
				vector = rigidbody2D2.velocity;
				num = rigidbody2D2.angularVelocity;
				mass = rigidbody2D2.mass / (float)targetFragments;
			}
			Vector2 vector2 = (meshTransform.TransformPoint(centroid) - mainCentroid).normalized;
			float num2 = fragmentOption.AngularVelocity * ((!fragmentOption.RandomAngularVelocityVector) ? fragmentOption.AngularVelocityVector.y : Random.insideUnitCircle.x);
			if (useForceVector)
			{
				vector2 = ForceVector;
			}
			rigidbody2D.velocity = vector2 * force + vector;
			rigidbody2D.angularVelocity = num2 + num;
			rigidbody2D.mass = mass;
			this.maxVelocity = fragmentOption.MaxVelocity;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00034DA0 File Offset: 0x00032FA0
		public void RefreshComponentsCache()
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
			this.meshRenderer = base.GetComponent<MeshRenderer>();
			this.meshCollider = base.GetComponent<MeshCollider>();
			this.boxCollider = base.GetComponent<BoxCollider>();
			this.options = base.GetComponent<ExploderOption>();
			this.rigidBody = base.GetComponent<Rigidbody>();
			this.rigid2D = base.GetComponent<Rigidbody2D>();
			this.polygonCollider2D = base.GetComponent<PolygonCollider2D>();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00034E10 File Offset: 0x00033010
		public void Explode()
		{
			this.activeObj = true;
			ExploderUtils.SetActiveRecursively(base.gameObject, true);
			this.visibilityCheckTimer = 0.1f;
			this.visible = true;
			this.deactivateTimer = this.deactivateTimeout;
			this.originalScale = base.transform.localScale;
			if (this.explodable)
			{
				base.tag = ExploderObject.Tag;
			}
			this.Emit(true);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00034E7C File Offset: 0x0003307C
		public void Emit(bool centerToBound)
		{
			if (this.emmiters != null)
			{
				if (centerToBound && this.particleChild && this.meshRenderer)
				{
					this.particleChild.transform.position = this.meshRenderer.bounds.center;
				}
				foreach (ParticleEmitter particleEmitter in this.emmiters)
				{
					particleEmitter.Emit();
				}
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00034F04 File Offset: 0x00033104
		public void Deactivate()
		{
			ExploderUtils.SetActive(base.gameObject, false);
			this.visible = false;
			this.activeObj = false;
			if (this.emmiters != null)
			{
				foreach (ParticleEmitter particleEmitter in this.emmiters)
				{
					particleEmitter.ClearParticles();
				}
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00004E39 File Offset: 0x00003039
		private void Start()
		{
			this.visibilityCheckTimer = 1f;
			this.RefreshComponentsCache();
			this.visible = false;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00034F5C File Offset: 0x0003315C
		private void Update()
		{
			if (this.activeObj)
			{
				if (this.rigidBody)
				{
					if (this.rigidBody.velocity.sqrMagnitude > this.maxVelocity * this.maxVelocity)
					{
						Vector3 normalized = this.rigidBody.velocity.normalized;
						this.rigidBody.velocity = normalized * this.maxVelocity;
					}
				}
				else if (this.rigid2D && this.rigid2D.velocity.sqrMagnitude > this.maxVelocity * this.maxVelocity)
				{
					Vector2 normalized2 = this.rigid2D.velocity.normalized;
					this.rigid2D.velocity = normalized2 * this.maxVelocity;
				}
				if (this.deactivateOptions == DeactivateOptions.Timeout)
				{
					this.deactivateTimer -= Time.deltaTime;
					if (this.deactivateTimer < 0f)
					{
						this.Sleep();
						this.activeObj = false;
						ExploderUtils.SetActiveRecursively(base.gameObject, false);
						FadeoutOptions fadeoutOptions = this.fadeoutOptions;
						if (fadeoutOptions != FadeoutOptions.FadeoutAlpha)
						{
						}
					}
					else
					{
						float num = this.deactivateTimer / this.deactivateTimeout;
						if (this.emmiters != null)
						{
							foreach (ParticleEmitter particleEmitter in this.emmiters)
							{
								for (int j = 0; j < particleEmitter.particles.Length; j++)
								{
									Color color = particleEmitter.particles[j].color;
									color.a = 1f - num;
									particleEmitter.particles[j].color = color;
								}
							}
						}
						FadeoutOptions fadeoutOptions = this.fadeoutOptions;
						if (fadeoutOptions != FadeoutOptions.FadeoutAlpha)
						{
							if (fadeoutOptions == FadeoutOptions.ScaleDown)
							{
								base.gameObject.transform.localScale = this.originalScale * num;
							}
						}
						else if (this.meshRenderer.material && this.meshRenderer.material.HasProperty("_Color"))
						{
							Color color2 = this.meshRenderer.material.color;
							color2.a = num;
							this.meshRenderer.material.color = color2;
						}
					}
				}
				this.visibilityCheckTimer -= Time.deltaTime;
				if (this.visibilityCheckTimer < 0f && Camera.main)
				{
					Vector3 vector = Camera.main.WorldToViewportPoint(base.transform.position);
					if (vector.z < 0f || vector.x < 0f || vector.y < 0f || vector.x > 1f || vector.y > 1f)
					{
						if (this.deactivateOptions == DeactivateOptions.OutsideOfCamera)
						{
							this.Sleep();
							this.activeObj = false;
							ExploderUtils.SetActiveRecursively(base.gameObject, false);
						}
						this.visible = false;
					}
					else
					{
						this.visible = true;
					}
					this.visibilityCheckTimer = Random.Range(0.1f, 0.3f);
					if (this.explodable)
					{
						Vector3 size = base.collider.bounds.size;
						if (Mathf.Max(new float[]
						{
							size.x,
							size.y,
							size.z
						}) < this.minSizeToExplode)
						{
							base.tag = string.Empty;
						}
					}
				}
			}
		}

		// Token: 0x040003A7 RID: 935
		public bool explodable;

		// Token: 0x040003A8 RID: 936
		public DeactivateOptions deactivateOptions;

		// Token: 0x040003A9 RID: 937
		public float deactivateTimeout = 10f;

		// Token: 0x040003AA RID: 938
		public FadeoutOptions fadeoutOptions;

		// Token: 0x040003AB RID: 939
		public float maxVelocity = 1000f;

		// Token: 0x040003AC RID: 940
		public bool disableColliders;

		// Token: 0x040003AD RID: 941
		public float disableCollidersTimeout;

		// Token: 0x040003AE RID: 942
		public bool visible;

		// Token: 0x040003AF RID: 943
		public bool activeObj;

		// Token: 0x040003B0 RID: 944
		public float minSizeToExplode = 0.5f;

		// Token: 0x040003B1 RID: 945
		public MeshFilter meshFilter;

		// Token: 0x040003B2 RID: 946
		public MeshRenderer meshRenderer;

		// Token: 0x040003B3 RID: 947
		public MeshCollider meshCollider;

		// Token: 0x040003B4 RID: 948
		public BoxCollider boxCollider;

		// Token: 0x040003B5 RID: 949
		public AudioSource audioSource;

		// Token: 0x040003B6 RID: 950
		public AudioClip audioClip;

		// Token: 0x040003B7 RID: 951
		private ParticleEmitter[] emmiters;

		// Token: 0x040003B8 RID: 952
		private GameObject particleChild;

		// Token: 0x040003B9 RID: 953
		public PolygonCollider2D polygonCollider2D;

		// Token: 0x040003BA RID: 954
		public Rigidbody2D rigid2D;

		// Token: 0x040003BB RID: 955
		public ExploderOption options;

		// Token: 0x040003BC RID: 956
		public Rigidbody rigidBody;

		// Token: 0x040003BD RID: 957
		private Vector3 originalScale;

		// Token: 0x040003BE RID: 958
		private float visibilityCheckTimer;

		// Token: 0x040003BF RID: 959
		private float deactivateTimer;
	}
}
