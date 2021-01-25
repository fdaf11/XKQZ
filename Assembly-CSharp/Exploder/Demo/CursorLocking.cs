using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x02000094 RID: 148
	public class CursorLocking : MonoBehaviour
	{
		// Token: 0x0600033D RID: 829 RVA: 0x0002BF1C File Offset: 0x0002A11C
		private void Update()
		{
			CursorLocking.IsLocked = Screen.lockCursor;
			if (Input.GetMouseButtonDown(0))
			{
				Screen.lockCursor = true;
				Screen.showCursor = false;
			}
			if (Input.GetKeyDown(27))
			{
				Screen.lockCursor = false;
				Screen.showCursor = true;
			}
			if (!Screen.lockCursor)
			{
				Screen.showCursor = true;
			}
		}

		// Token: 0x0400025F RID: 607
		public static bool IsLocked;
	}
}
