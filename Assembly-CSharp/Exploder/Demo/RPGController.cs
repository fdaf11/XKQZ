using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x0200009C RID: 156
	public class RPGController : MonoBehaviour
	{
		// Token: 0x06000357 RID: 855 RVA: 0x00004591 File Offset: 0x00002791
		private void Start()
		{
			this.Rocket.HitCallback = new Rocket.OnHit(this.OnRocketHit);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000045AA File Offset: 0x000027AA
		public void OnActivate()
		{
			this.Rocket.OnActivate();
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0002C8A0 File Offset: 0x0002AAA0
		private void OnRocketHit(Vector3 position)
		{
			this.nextShotTimeout = 0.6f;
			this.exploder.transform.position = position;
			this.exploder.ExplodeSelf = false;
			this.exploder.Force = 20f;
			this.exploder.TargetFragments = 100;
			this.exploder.Radius = 10f;
			this.exploder.UseForceVector = false;
			this.exploder.Explode();
			this.Rocket.Reset();
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0002C924 File Offset: 0x0002AB24
		private void Update()
		{
			TargetType targetType = TargetManager.Instance.TargetType;
			if (Input.GetMouseButtonDown(0) && this.nextShotTimeout < 0f && CursorLocking.IsLocked && targetType != TargetType.UseObject)
			{
				this.MouseLookCamera.Kick();
				Ray ray = this.MouseLookCamera.mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
				Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 0f);
				this.Rocket.Launch(ray);
				this.nextShotTimeout = float.MaxValue;
			}
			this.nextShotTimeout -= Time.deltaTime;
		}

		// Token: 0x0400028C RID: 652
		public ExploderObject exploder;

		// Token: 0x0400028D RID: 653
		public ExploderMouseLook MouseLookCamera;

		// Token: 0x0400028E RID: 654
		public Rocket Rocket;

		// Token: 0x0400028F RID: 655
		private float nextShotTimeout;
	}
}
