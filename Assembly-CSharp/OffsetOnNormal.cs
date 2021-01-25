using System;
using UnityEngine;

// Token: 0x020005F7 RID: 1527
public class OffsetOnNormal : MonoBehaviour
{
	// Token: 0x060025CA RID: 9674 RVA: 0x0001929E File Offset: 0x0001749E
	private void Awake()
	{
		this.startPosition = base.transform.position;
	}

	// Token: 0x060025CB RID: 9675 RVA: 0x00124B7C File Offset: 0x00122D7C
	private void OnEnable()
	{
		RaycastHit raycastHit;
		Physics.Raycast(this.startPosition, Vector3.down, ref raycastHit);
		if (this.offsetGameObject != null)
		{
			base.transform.position = this.offsetGameObject.transform.position + raycastHit.normal * this.offset;
		}
		else
		{
			base.transform.position = raycastHit.point + raycastHit.normal * this.offset;
		}
	}

	// Token: 0x060025CC RID: 9676 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04002E59 RID: 11865
	public float offset = 1f;

	// Token: 0x04002E5A RID: 11866
	public GameObject offsetGameObject;

	// Token: 0x04002E5B RID: 11867
	private Vector3 startPosition;
}
