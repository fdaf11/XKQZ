using System;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public class Tutorial5 : MonoBehaviour
{
	// Token: 0x06001A14 RID: 6676 RVA: 0x000D0A98 File Offset: 0x000CEC98
	public void SetDurationToCurrentProgress()
	{
		UITweener[] componentsInChildren = base.GetComponentsInChildren<UITweener>();
		foreach (UITweener uitweener in componentsInChildren)
		{
			uitweener.duration = Mathf.Lerp(2f, 0.5f, UIProgressBar.current.value);
		}
	}
}
