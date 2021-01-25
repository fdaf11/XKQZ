using System;
using UnityEngine;

// Token: 0x020005C4 RID: 1476
public class TankProjectile : MonoBehaviour
{
	// Token: 0x060024B9 RID: 9401 RVA: 0x00018584 File Offset: 0x00016784
	private void Start()
	{
		base.Invoke("DestroySelf", this.Lifetime);
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x0000DBD4 File Offset: 0x0000BDD4
	private void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x00018597 File Offset: 0x00016797
	private void Update()
	{
		base.transform.position = base.transform.position + base.transform.forward * this.Speed * Time.deltaTime;
	}

	// Token: 0x04002C8D RID: 11405
	public float Speed;

	// Token: 0x04002C8E RID: 11406
	public float Lifetime;
}
