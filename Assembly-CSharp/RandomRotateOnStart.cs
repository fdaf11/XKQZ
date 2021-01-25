using System;
using UnityEngine;

// Token: 0x020005EC RID: 1516
public class RandomRotateOnStart : MonoBehaviour
{
	// Token: 0x06002592 RID: 9618 RVA: 0x00019033 File Offset: 0x00017233
	private void Start()
	{
		this.t = base.transform;
		this.t.Rotate(this.NormalizedRotateVector * (float)Random.Range(0, 360));
		this.isInitialized = true;
	}

	// Token: 0x06002593 RID: 9619 RVA: 0x0001906A File Offset: 0x0001726A
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.t.Rotate(this.NormalizedRotateVector * (float)Random.Range(0, 360));
		}
	}

	// Token: 0x04002E13 RID: 11795
	public Vector3 NormalizedRotateVector = new Vector3(0f, 1f, 0f);

	// Token: 0x04002E14 RID: 11796
	private Transform t;

	// Token: 0x04002E15 RID: 11797
	private bool isInitialized;
}
