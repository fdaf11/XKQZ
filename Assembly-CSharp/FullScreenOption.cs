using System;
using UnityEngine;

// Token: 0x02000606 RID: 1542
[AddComponentMenu("Common/Full Screen Option")]
public class FullScreenOption : MonoBehaviour
{
	// Token: 0x06002630 RID: 9776 RVA: 0x0012834C File Offset: 0x0012654C
	private void Update()
	{
		if (Input.GetKeyDown(286))
		{
			if (Screen.fullScreen)
			{
				Screen.SetResolution(1280, 720, false);
			}
			else
			{
				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
			}
		}
	}
}
