using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x02000099 RID: 153
	public class PanelChairBomb : UseObject
	{
		// Token: 0x0600034C RID: 844 RVA: 0x0002C5B0 File Offset: 0x0002A7B0
		public override void Use()
		{
			base.Use();
			this.Exploder.transform.position = this.ChairBomb.transform.position;
			this.Exploder.ExplodeSelf = false;
			this.Exploder.UseForceVector = false;
			this.Exploder.Radius = 10f;
			this.Exploder.TargetFragments = 300;
			this.Exploder.Force = 30f;
			this.Exploder.Explode(new ExploderObject.OnExplosion(this.OnExplode));
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0002C644 File Offset: 0x0002A844
		private void OnExplode(float timeMS, ExploderObject.ExplosionState state)
		{
			if (state == ExploderObject.ExplosionState.ExplosionStarted)
			{
				this.SourceExplosion.PlayOneShot(this.ExplosionSound);
				this.Flash.gameObject.transform.position = this.ChairBomb.transform.position;
				this.Flash.gameObject.transform.position += Vector3.up;
				this.flashing = 10;
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0002C6BC File Offset: 0x0002A8BC
		private void Update()
		{
			if (this.flashing > 0)
			{
				this.Flash.intensity = 5f;
				this.flashing--;
			}
			else
			{
				this.Flash.intensity = 0f;
			}
		}

		// Token: 0x04000283 RID: 643
		public ExploderObject Exploder;

		// Token: 0x04000284 RID: 644
		public GameObject ChairBomb;

		// Token: 0x04000285 RID: 645
		public AudioSource SourceExplosion;

		// Token: 0x04000286 RID: 646
		public AudioClip ExplosionSound;

		// Token: 0x04000287 RID: 647
		public Light Flash;

		// Token: 0x04000288 RID: 648
		private int flashing;
	}
}
