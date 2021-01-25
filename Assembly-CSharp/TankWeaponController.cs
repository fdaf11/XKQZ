using System;
using UnityEngine;

// Token: 0x020005C5 RID: 1477
public class TankWeaponController : MonoBehaviour
{
	// Token: 0x060024BD RID: 9405 RVA: 0x0011E80C File Offset: 0x0011CA0C
	private void Update()
	{
		if (!base.animation.isPlaying && Input.GetKeyDown(32))
		{
			base.animation.Play();
			Object.Instantiate(this.ProjectilePrefab, this.Nozzle.position, this.Nozzle.rotation);
		}
	}

	// Token: 0x04002C8F RID: 11407
	public TankProjectile ProjectilePrefab;

	// Token: 0x04002C90 RID: 11408
	public Transform Nozzle;
}
