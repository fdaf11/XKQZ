using System;
using UnityEngine;

// Token: 0x02000421 RID: 1057
[AddComponentMenu("NGUI/Examples/Item Attachment Point")]
public class InvAttachmentPoint : MonoBehaviour
{
	// Token: 0x060019BB RID: 6587 RVA: 0x000CF478 File Offset: 0x000CD678
	public GameObject Attach(GameObject prefab)
	{
		if (this.mPrefab != prefab)
		{
			this.mPrefab = prefab;
			if (this.mChild != null)
			{
				Object.Destroy(this.mChild);
			}
			if (this.mPrefab != null)
			{
				Transform transform = base.transform;
				this.mChild = (Object.Instantiate(this.mPrefab, transform.position, transform.rotation) as GameObject);
				Transform transform2 = this.mChild.transform;
				transform2.parent = transform;
				transform2.localPosition = Vector3.zero;
				transform2.localRotation = Quaternion.identity;
				transform2.localScale = Vector3.one;
			}
		}
		return this.mChild;
	}

	// Token: 0x04001E62 RID: 7778
	public InvBaseItem.Slot slot;

	// Token: 0x04001E63 RID: 7779
	private GameObject mPrefab;

	// Token: 0x04001E64 RID: 7780
	private GameObject mChild;
}
