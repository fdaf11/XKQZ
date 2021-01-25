using System;
using UnityEngine;

// Token: 0x020005BD RID: 1469
public class DestroyAfterTime : MonoBehaviour
{
	// Token: 0x060024A3 RID: 9379 RVA: 0x000184FE File Offset: 0x000166FE
	private void Start()
	{
		base.Invoke("DestroyMe", this.lifetime);
	}

	// Token: 0x060024A4 RID: 9380 RVA: 0x0000DBD4 File Offset: 0x0000BDD4
	private void DestroyMe()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04002C66 RID: 11366
	public float lifetime;
}
