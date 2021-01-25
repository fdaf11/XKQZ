using System;
using UnityEngine;

// Token: 0x020005BE RID: 1470
public class FPSController : MonoBehaviour
{
	// Token: 0x060024A6 RID: 9382 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060024A7 RID: 9383 RVA: 0x0011E128 File Offset: 0x0011C328
	private void Update()
	{
		this.CamAnimator.SetBool("Running", Input.GetKey(119));
		this.WeaponAnimator.SetBool("Fire", Input.GetKey(32));
		if (Input.GetKey(119))
		{
			base.transform.position = base.transform.position + base.transform.forward * this.moveSpeed * Time.deltaTime;
		}
	}

	// Token: 0x04002C67 RID: 11367
	public Animator CamAnimator;

	// Token: 0x04002C68 RID: 11368
	public Animator WeaponAnimator;

	// Token: 0x04002C69 RID: 11369
	public float moveSpeed;
}
