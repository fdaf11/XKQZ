using System;
using UnityEngine;

// Token: 0x020005DA RID: 1498
public class DeadTime : MonoBehaviour
{
	// Token: 0x0600252A RID: 9514 RVA: 0x00018A12 File Offset: 0x00016C12
	private void Awake()
	{
		Object.Destroy(this.destroyRoot ? base.transform.root.gameObject : base.gameObject, this.deadTime);
	}

	// Token: 0x04002D66 RID: 11622
	public float deadTime = 1.5f;

	// Token: 0x04002D67 RID: 11623
	public bool destroyRoot;
}
