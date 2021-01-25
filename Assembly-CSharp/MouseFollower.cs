using System;
using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;

// Token: 0x020005C0 RID: 1472
public class MouseFollower : MonoBehaviour
{
	// Token: 0x060024AD RID: 9389 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060024AE RID: 9390 RVA: 0x0011E2F8 File Offset: 0x0011C4F8
	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			this.Trail.Emit = true;
			base.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 0.01f));
		}
		else
		{
			this.Trail.Emit = false;
		}
	}

	// Token: 0x04002C75 RID: 11381
	public Trail Trail;
}
