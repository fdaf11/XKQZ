using System;
using UnityEngine;

// Token: 0x020005F3 RID: 1523
public class ScaleParticlesFromBound : MonoBehaviour
{
	// Token: 0x060025BB RID: 9659 RVA: 0x00124860 File Offset: 0x00122A60
	private void GetMeshFilterParent(Transform t)
	{
		Collider component = t.parent.GetComponent<Collider>();
		if (component == null)
		{
			this.GetMeshFilterParent(t.parent);
		}
		else
		{
			this.targetCollider = component;
		}
	}

	// Token: 0x060025BC RID: 9660 RVA: 0x001248A0 File Offset: 0x00122AA0
	private void Start()
	{
		this.GetMeshFilterParent(base.transform);
		if (this.targetCollider == null)
		{
			return;
		}
		Vector3 size = this.targetCollider.bounds.size;
		base.transform.localScale = size;
	}

	// Token: 0x060025BD RID: 9661 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04002E47 RID: 11847
	private Collider targetCollider;
}
