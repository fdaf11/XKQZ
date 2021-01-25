using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x0200009E RID: 158
	public class Rocket : MonoBehaviour
	{
		// Token: 0x06000360 RID: 864 RVA: 0x0002CA9C File Offset: 0x0002AC9C
		private void Start()
		{
			this.parent = base.transform.parent;
			this.launchTimeout = float.MaxValue;
			this.SmokeTrail.emit = false;
			this.ExplosionEffect.emit = false;
			this.ExplosionSmoke.emit = false;
			ExploderUtils.SetActive(this.SmokeTrail.gameObject, true);
			ExploderUtils.SetActive(this.ExplosionEffect.gameObject, true);
			ExploderUtils.SetActive(this.ExplosionSmoke.gameObject, true);
			ExploderUtils.SetActive(this.RocketStatic.gameObject, false);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000460E File Offset: 0x0000280E
		public void OnActivate()
		{
			ExploderUtils.SetActive(this.RocketStatic.gameObject, true);
			if (this.parent)
			{
				ExploderUtils.SetVisible(base.gameObject, false);
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000463D File Offset: 0x0000283D
		public void Reset()
		{
			ExploderUtils.SetActive(this.RocketStatic.gameObject, true);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0002CB30 File Offset: 0x0002AD30
		public void Launch(Ray ray)
		{
			this.direction = ray;
			this.Source.PlayOneShot(this.GunShot);
			this.launchTimeout = 0.3f;
			this.launch = false;
			ExploderUtils.SetActive(this.RocketStatic.gameObject, false);
			ExploderUtils.SetVisible(base.gameObject, true);
			base.gameObject.transform.parent = this.parent;
			base.gameObject.transform.localPosition = this.RocketStatic.gameObject.transform.localPosition;
			base.gameObject.transform.localRotation = this.RocketStatic.gameObject.transform.localRotation;
			base.gameObject.transform.localScale = this.RocketStatic.gameObject.transform.localScale;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0002CC0C File Offset: 0x0002AE0C
		private void Update()
		{
			if (this.launchTimeout < 0f)
			{
				if (!this.launch)
				{
					this.launch = true;
					base.transform.parent = null;
					this.SmokeTrail.emit = true;
					this.RocketLight.intensity = 2f;
					this.direction.origin = this.direction.origin + this.direction.direction * 2f;
					RaycastHit raycastHit;
					if (Physics.Raycast(this.direction, ref raycastHit, float.PositiveInfinity))
					{
						this.shotPos = base.gameObject.transform.position;
						this.targetDistance = (base.gameObject.transform.position - raycastHit.point).sqrMagnitude;
					}
					else
					{
						this.targetDistance = 10000f;
					}
				}
				base.gameObject.transform.position += this.direction.direction * this.RocketVelocity * Time.timeScale;
				this.RocketLight.transform.position = base.gameObject.transform.position;
				if ((this.shotPos - base.gameObject.transform.position).sqrMagnitude > this.targetDistance)
				{
					this.Source.PlayOneShot(this.Explosion);
					this.HitCallback(base.gameObject.transform.position);
					this.launchTimeout = float.MaxValue;
					this.launch = false;
					this.SmokeTrail.emit = false;
					this.ExplosionEffect.gameObject.transform.position = base.gameObject.transform.position;
					this.ExplosionSmoke.gameObject.transform.position = base.gameObject.transform.position;
					this.ExplosionEffect.Emit(1);
					this.ExplosionSmoke.Emit(1);
					ExploderUtils.SetVisible(base.gameObject, false);
					this.RocketLight.intensity = 0f;
				}
			}
			this.launchTimeout -= Time.deltaTime;
			if (Input.GetKeyDown(104))
			{
				this.HitCallback(base.gameObject.transform.position);
			}
		}

		// Token: 0x04000295 RID: 661
		public AudioClip GunShot;

		// Token: 0x04000296 RID: 662
		public AudioClip Explosion;

		// Token: 0x04000297 RID: 663
		public AudioSource Source;

		// Token: 0x04000298 RID: 664
		public ParticleEmitter SmokeTrail;

		// Token: 0x04000299 RID: 665
		public ParticleEmitter ExplosionSmoke;

		// Token: 0x0400029A RID: 666
		public ParticleEmitter ExplosionEffect;

		// Token: 0x0400029B RID: 667
		public GameObject RocketStatic;

		// Token: 0x0400029C RID: 668
		public Light RocketLight;

		// Token: 0x0400029D RID: 669
		public float RocketVelocity = 1f;

		// Token: 0x0400029E RID: 670
		public Rocket.OnHit HitCallback;

		// Token: 0x0400029F RID: 671
		private Ray direction;

		// Token: 0x040002A0 RID: 672
		private bool launch;

		// Token: 0x040002A1 RID: 673
		private float launchTimeout;

		// Token: 0x040002A2 RID: 674
		private Transform parent;

		// Token: 0x040002A3 RID: 675
		private Vector3 shotPos;

		// Token: 0x040002A4 RID: 676
		private float targetDistance;

		// Token: 0x0200009F RID: 159
		// (Invoke) Token: 0x06000366 RID: 870
		public delegate void OnHit(Vector3 pos);
	}
}
