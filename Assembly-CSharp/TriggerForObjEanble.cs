using System;
using UnityEngine;

// Token: 0x02000695 RID: 1685
public class TriggerForObjEanble : MonoBehaviour
{
	// Token: 0x060028E7 RID: 10471 RVA: 0x0001AEFF File Offset: 0x000190FF
	private void Start()
	{
		this.AllObject = this.Object01.GetComponentsInChildren<Transform>();
	}

	// Token: 0x060028E8 RID: 10472 RVA: 0x00144D34 File Offset: 0x00142F34
	private void OnTriggerEnter(Collider playerOne)
	{
		if (playerOne.CompareTag("Player"))
		{
			if (this.AllObject == null)
			{
				return;
			}
			if (this.enable)
			{
				foreach (Transform transform in this.AllObject)
				{
					if (transform != null && transform.gameObject != null)
					{
						transform.gameObject.SetActive(this.enable);
					}
				}
			}
			else
			{
				foreach (Transform transform2 in this.AllObject)
				{
					if (transform2 != null && transform2.gameObject != null)
					{
						transform2.gameObject.SetActive(this.enable);
					}
				}
			}
		}
	}

	// Token: 0x040033C0 RID: 13248
	public GameObject Object01;

	// Token: 0x040033C1 RID: 13249
	public bool enable;

	// Token: 0x040033C2 RID: 13250
	private Transform[] AllObject;
}
