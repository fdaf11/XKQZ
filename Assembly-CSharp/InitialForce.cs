using System;
using UnityEngine;

// Token: 0x02000271 RID: 625
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class InitialForce : MonoBehaviour
{
	// Token: 0x06000B78 RID: 2936 RVA: 0x00008FAA File Offset: 0x000071AA
	private void Start()
	{
		base.rigidbody.AddForce(this.direction * this.value, 1);
	}

	// Token: 0x04000D48 RID: 3400
	public Vector3 direction;

	// Token: 0x04000D49 RID: 3401
	public float value = 1f;
}
