using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x02000097 RID: 151
	public class GrenadeController : MonoBehaviour
	{
		// Token: 0x06000343 RID: 835 RVA: 0x000044F1 File Offset: 0x000026F1
		private void Start()
		{
			this.throwTimer = float.MaxValue;
			this.explodeTimer = float.MaxValue;
			this.exploding = false;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0002C228 File Offset: 0x0002A428
		private void Update()
		{
			if (Input.GetKeyDown(103) && !this.exploding)
			{
				this.throwTimer = 0.4f;
				this.Source.PlayOneShot(this.Throw);
				this.explodeTimer = 2f;
				this.exploding = true;
				this.Grenade.Throw();
				ExploderUtils.SetVisible(base.gameObject, false);
			}
			this.throwTimer -= Time.deltaTime;
			if (this.throwTimer < 0f)
			{
				this.throwTimer = float.MaxValue;
				ExploderUtils.SetVisible(this.Grenade.gameObject, true);
				ExploderUtils.SetActive(this.Grenade.gameObject, true);
				this.Grenade.transform.position = base.gameObject.transform.position;
				this.Grenade.rigidbody.velocity = this.MainCamera.transform.forward * 20f;
			}
			this.explodeTimer -= Time.deltaTime;
			if (this.explodeTimer < 0f)
			{
				this.Grenade.Explode();
				this.explodeTimer = float.MaxValue;
			}
			if (this.Grenade.ExplodeFinished)
			{
				this.exploding = false;
				ExploderUtils.SetVisible(base.gameObject, true);
			}
		}

		// Token: 0x04000271 RID: 625
		public AudioClip Throw;

		// Token: 0x04000272 RID: 626
		public AudioSource Source;

		// Token: 0x04000273 RID: 627
		public GrenadeObject Grenade;

		// Token: 0x04000274 RID: 628
		public Camera MainCamera;

		// Token: 0x04000275 RID: 629
		private float explodeTimer;

		// Token: 0x04000276 RID: 630
		private float throwTimer;

		// Token: 0x04000277 RID: 631
		private bool exploding;
	}
}
