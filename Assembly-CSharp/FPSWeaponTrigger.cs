using System;
using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;

// Token: 0x020005BF RID: 1471
public class FPSWeaponTrigger : MonoBehaviour
{
	// Token: 0x060024A9 RID: 9385 RVA: 0x0011E1AC File Offset: 0x0011C3AC
	private void Update()
	{
		this.MuzzlePlume.Emit = (this._smoke > this.SmokeAfter);
		this._smoke -= Time.deltaTime;
		if (this._smoke > this.SmokeMax)
		{
			this._smoke = this.SmokeMax;
		}
		if (this._smoke < 0f)
		{
			this._smoke = 0f;
		}
	}

	// Token: 0x060024AA RID: 9386 RVA: 0x0011E21C File Offset: 0x0011C41C
	public void Fire()
	{
		this.MuzzleFlashObject.SetActive(true);
		base.Invoke("LightsOff", 0.05f);
		this._smoke += this.SmokeIncrement;
		Rigidbody rigidbody = (Object.Instantiate(this.Shell.gameObject, this.ShellEjectionTransform.position, this.ShellEjectionTransform.rotation) as GameObject).rigidbody;
		rigidbody.velocity = this.ShellEjectionTransform.right * this.EjectionForce + Random.onUnitSphere * 0.25f;
		rigidbody.angularVelocity = Random.onUnitSphere * this.EjectionForce;
		Object.Instantiate(this.Bullet, this.Muzzle.transform.position, this.Muzzle.rotation);
	}

	// Token: 0x060024AB RID: 9387 RVA: 0x00018511 File Offset: 0x00016711
	private void LightsOff()
	{
		this.MuzzleFlashObject.SetActive(false);
	}

	// Token: 0x04002C6A RID: 11370
	public Transform ShellEjectionTransform;

	// Token: 0x04002C6B RID: 11371
	public float EjectionForce;

	// Token: 0x04002C6C RID: 11372
	public Rigidbody Shell;

	// Token: 0x04002C6D RID: 11373
	public Transform Muzzle;

	// Token: 0x04002C6E RID: 11374
	public GameObject Bullet;

	// Token: 0x04002C6F RID: 11375
	public float SmokeAfter;

	// Token: 0x04002C70 RID: 11376
	public float SmokeMax;

	// Token: 0x04002C71 RID: 11377
	public float SmokeIncrement;

	// Token: 0x04002C72 RID: 11378
	public SmokePlume MuzzlePlume;

	// Token: 0x04002C73 RID: 11379
	public GameObject MuzzleFlashObject;

	// Token: 0x04002C74 RID: 11380
	private float _smoke;
}
