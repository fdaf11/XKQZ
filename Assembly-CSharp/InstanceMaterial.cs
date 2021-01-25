using System;
using UnityEngine;

// Token: 0x020005D7 RID: 1495
public class InstanceMaterial : MonoBehaviour
{
	// Token: 0x06002520 RID: 9504 RVA: 0x000189A9 File Offset: 0x00016BA9
	private void Start()
	{
		base.renderer.material = this.Material;
	}

	// Token: 0x06002521 RID: 9505 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04002D5A RID: 11610
	public Material Material;
}
