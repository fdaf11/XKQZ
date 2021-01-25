using System;
using UnityEngine;

// Token: 0x02000445 RID: 1093
[AddComponentMenu("NGUI/Interaction/Button Activate")]
public class UIButtonActivate : MonoBehaviour
{
	// Token: 0x06001A41 RID: 6721 RVA: 0x00011286 File Offset: 0x0000F486
	private void OnClick()
	{
		if (this.target != null)
		{
			NGUITools.SetActive(this.target, this.state);
		}
	}

	// Token: 0x04001F0F RID: 7951
	public GameObject target;

	// Token: 0x04001F10 RID: 7952
	public bool state = true;
}
