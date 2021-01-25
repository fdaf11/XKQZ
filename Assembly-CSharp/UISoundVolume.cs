using System;
using UnityEngine;

// Token: 0x02000482 RID: 1154
[AddComponentMenu("NGUI/Interaction/Sound Volume")]
[RequireComponent(typeof(UISlider))]
public class UISoundVolume : MonoBehaviour
{
	// Token: 0x06001BE4 RID: 7140 RVA: 0x00012973 File Offset: 0x00010B73
	private void Awake()
	{
		this.mSlider = base.GetComponent<UISlider>();
		this.mSlider.value = NGUITools.soundVolume;
		EventDelegate.Add(this.mSlider.onChange, new EventDelegate.Callback(this.OnChange));
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x000129AE File Offset: 0x00010BAE
	private void OnChange()
	{
		NGUITools.soundVolume = UIProgressBar.current.value;
	}

	// Token: 0x040020BB RID: 8379
	private UISlider mSlider;
}
