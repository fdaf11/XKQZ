using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x02000098 RID: 152
	public class GrenadeObject : MonoBehaviour
	{
		// Token: 0x06000346 RID: 838 RVA: 0x00004510 File Offset: 0x00002710
		public void Throw()
		{
			this.Impact = false;
			this.throwing = true;
			this.explodeTimeoutMax = 5f;
			this.ExplodeFinished = false;
			this.flashing = -1;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0002C388 File Offset: 0x0002A588
		public void Explode()
		{
			if (this.explosionInProgress)
			{
				return;
			}
			this.explosionInProgress = true;
			this.throwing = false;
			if (!this.Impact)
			{
				this.explodeTimeoutMax = 5f;
			}
			else
			{
				this.exploder.transform.position = base.transform.position;
				this.exploder.ExplodeSelf = false;
				this.exploder.UseForceVector = false;
				this.exploder.Radius = 5f;
				this.exploder.TargetFragments = 200;
				this.exploder.Force = 20f;
				this.exploder.Explode(new ExploderObject.OnExplosion(this.OnExplode));
				this.ExplodeFinished = false;
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0002C44C File Offset: 0x0002A64C
		private void OnExplode(float timeMS, ExploderObject.ExplosionState state)
		{
			if (state == ExploderObject.ExplosionState.ExplosionStarted)
			{
				ExploderUtils.SetVisible(base.gameObject, false);
				this.SourceExplosion.PlayOneShot(this.ExplosionSound);
				this.Flash.gameObject.transform.position = base.gameObject.transform.position;
				this.Flash.gameObject.transform.position += Vector3.up;
				this.flashing = 10;
				this.ExplosionEffect.gameObject.transform.position = base.gameObject.transform.position;
				this.ExplosionEffect.Emit(1);
			}
			if (state == ExploderObject.ExplosionState.ExplosionFinished)
			{
				this.ExplodeFinished = true;
				this.explosionInProgress = false;
			}
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00004539 File Offset: 0x00002739
		private void OnCollisionEnter(Collision other)
		{
			this.Impact = true;
			if (!this.throwing && !this.ExplodeFinished)
			{
				this.Explode();
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0002C514 File Offset: 0x0002A714
		private void Update()
		{
			if (this.flashing >= 0)
			{
				if (this.flashing > 0)
				{
					this.Flash.intensity = 5f;
					this.flashing--;
				}
				else
				{
					this.Flash.intensity = 0f;
					this.flashing = -1;
				}
			}
			this.explodeTimeoutMax -= Time.deltaTime;
			if (!this.ExplodeFinished && this.explodeTimeoutMax < 0f)
			{
				this.Impact = true;
				this.Explode();
			}
		}

		// Token: 0x04000278 RID: 632
		public AudioClip ExplosionSound;

		// Token: 0x04000279 RID: 633
		public AudioSource SourceExplosion;

		// Token: 0x0400027A RID: 634
		public ParticleEmitter ExplosionEffect;

		// Token: 0x0400027B RID: 635
		public Light Flash;

		// Token: 0x0400027C RID: 636
		public bool ExplodeFinished;

		// Token: 0x0400027D RID: 637
		public bool Impact;

		// Token: 0x0400027E RID: 638
		private bool throwing;

		// Token: 0x0400027F RID: 639
		private float explodeTimeoutMax;

		// Token: 0x04000280 RID: 640
		private bool explosionInProgress;

		// Token: 0x04000281 RID: 641
		public ExploderObject exploder;

		// Token: 0x04000282 RID: 642
		private int flashing;
	}
}
