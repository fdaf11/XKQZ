using System;
using UnityEngine;

// Token: 0x0200061A RID: 1562
public class ScreenLock : MonoBehaviour
{
	// Token: 0x0600269D RID: 9885 RVA: 0x00019B1B File Offset: 0x00017D1B
	private void Start()
	{
		Screen.lockCursor = true;
	}

	// Token: 0x0600269E RID: 9886 RVA: 0x00019B23 File Offset: 0x00017D23
	private void Update()
	{
		if (!Screen.lockCursor && Input.GetMouseButtonDown(0))
		{
			Screen.lockCursor = true;
		}
	}
}
