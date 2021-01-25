using System;
using UnityEngine;

// Token: 0x020005AB RID: 1451
public class mouseFollowScript : MonoBehaviour
{
	// Token: 0x06002442 RID: 9282 RVA: 0x0011BD5C File Offset: 0x00119F5C
	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 12f;
			Vector3 position = Camera.main.ScreenToWorldPoint(mousePosition);
			base.transform.position = position;
		}
	}
}
