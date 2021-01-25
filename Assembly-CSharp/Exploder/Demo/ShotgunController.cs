using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x020000A0 RID: 160
	public class ShotgunController : MonoBehaviour
	{
		// Token: 0x0600036A RID: 874 RVA: 0x00004663 File Offset: 0x00002863
		public void OnActivate()
		{
			ExploderUtils.SetActive(this.MuzzleFlash, false);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0002CE84 File Offset: 0x0002B084
		private void Update()
		{
			GameObject gameObject = null;
			TargetType targetType = TargetManager.Instance.TargetType;
			if (targetType == TargetType.UseObject)
			{
				if (this.lastTarget != TargetType.UseObject)
				{
				}
				this.lastTarget = TargetType.UseObject;
			}
			if (this.lastTarget == TargetType.UseObject)
			{
			}
			this.lastTarget = targetType;
			Ray ray = this.MouseLookCamera.mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
			Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 0f);
			if (targetType == TargetType.DestroyableObject)
			{
				gameObject = TargetManager.Instance.TargetObject;
			}
			if (Input.GetMouseButtonDown(0) && this.nextShotTimeout < 0f && CursorLocking.IsLocked)
			{
				if (targetType != TargetType.UseObject)
				{
					this.Source.PlayOneShot(this.GunShot);
					this.MouseLookCamera.Kick();
					this.reloadTimeout = 0.3f;
					this.flashing = 5;
				}
				if (gameObject)
				{
					Vector3 centroid = ExploderUtils.GetCentroid(gameObject);
					this.exploder.transform.position = centroid;
					this.exploder.ExplodeSelf = false;
					this.exploder.ForceVector = ray.direction.normalized;
					this.exploder.Force = 10f;
					this.exploder.UseForceVector = true;
					this.exploder.TargetFragments = 30;
					this.exploder.Radius = 1f;
					this.exploder.Explode();
				}
				this.nextShotTimeout = 0.6f;
			}
			this.nextShotTimeout -= Time.deltaTime;
			if (this.flashing > 0)
			{
				this.Flash.intensity = 1f;
				ExploderUtils.SetActive(this.MuzzleFlash, true);
				this.flashing--;
			}
			else
			{
				this.Flash.intensity = 0f;
				ExploderUtils.SetActive(this.MuzzleFlash, false);
			}
			this.reloadTimeout -= Time.deltaTime;
			if (this.reloadTimeout < 0f)
			{
				this.reloadTimeout = float.MaxValue;
				this.Source.PlayOneShot(this.Reload);
				this.ReloadAnim.Play();
			}
		}

		// Token: 0x040002A5 RID: 677
		public AudioClip GunShot;

		// Token: 0x040002A6 RID: 678
		public AudioClip Reload;

		// Token: 0x040002A7 RID: 679
		public AudioSource Source;

		// Token: 0x040002A8 RID: 680
		public ExploderMouseLook MouseLookCamera;

		// Token: 0x040002A9 RID: 681
		public Light Flash;

		// Token: 0x040002AA RID: 682
		public Animation ReloadAnim;

		// Token: 0x040002AB RID: 683
		public AnimationClip HideAnim;

		// Token: 0x040002AC RID: 684
		public GameObject MuzzleFlash;

		// Token: 0x040002AD RID: 685
		private int flashing;

		// Token: 0x040002AE RID: 686
		private float reloadTimeout = float.MaxValue;

		// Token: 0x040002AF RID: 687
		private float nextShotTimeout;

		// Token: 0x040002B0 RID: 688
		private TargetType lastTarget;

		// Token: 0x040002B1 RID: 689
		public ExploderObject exploder;
	}
}
