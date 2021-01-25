using System;
using UnityEngine;

// Token: 0x020005AC RID: 1452
public class AddForce : MonoBehaviour
{
	// Token: 0x06002444 RID: 9284 RVA: 0x0011BDA0 File Offset: 0x00119FA0
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			base.rigidbody.AddForce(new Vector3(Random.Range(-this.strength, this.strength), Random.Range(-this.strength, this.strength), Random.Range(-this.strength, this.strength)));
		}
	}

	// Token: 0x04002C19 RID: 11289
	public float strength;
}
