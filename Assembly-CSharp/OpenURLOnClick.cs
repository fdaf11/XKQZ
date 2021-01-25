using System;
using UnityEngine;

// Token: 0x02000435 RID: 1077
public class OpenURLOnClick : MonoBehaviour
{
	// Token: 0x06001A00 RID: 6656 RVA: 0x000D0450 File Offset: 0x000CE650
	private void OnClick()
	{
		UILabel component = base.GetComponent<UILabel>();
		if (component != null)
		{
			string urlAtPosition = component.GetUrlAtPosition(UICamera.lastWorldPosition);
			if (!string.IsNullOrEmpty(urlAtPosition))
			{
				Application.OpenURL(urlAtPosition);
			}
		}
	}
}
