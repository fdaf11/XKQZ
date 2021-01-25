using System;
using UnityEngine;

// Token: 0x02000696 RID: 1686
public class TriggerForWaterEanble : MonoBehaviour
{
	// Token: 0x060028EA RID: 10474 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060028EB RID: 10475 RVA: 0x0001AF12 File Offset: 0x00019112
	private void OnTriggerEnter(Collider playerOne)
	{
		if (playerOne.CompareTag("Player") && this.Object01 != null)
		{
			this.Object01.GetComponent<MeshRenderer>().enabled = this.enable;
		}
	}

	// Token: 0x040033C3 RID: 13251
	public GameObject Object01;

	// Token: 0x040033C4 RID: 13252
	public bool enable;
}
