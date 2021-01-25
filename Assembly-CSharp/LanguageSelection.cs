using System;
using UnityEngine;

// Token: 0x02000442 RID: 1090
[RequireComponent(typeof(UIPopupList))]
[AddComponentMenu("NGUI/Interaction/Language Selection")]
public class LanguageSelection : MonoBehaviour
{
	// Token: 0x06001A26 RID: 6694 RVA: 0x000D1320 File Offset: 0x000CF520
	private void Start()
	{
		this.mList = base.GetComponent<UIPopupList>();
		if (Localization.knownLanguages != null)
		{
			this.mList.items.Clear();
			int i = 0;
			int num = Localization.knownLanguages.Length;
			while (i < num)
			{
				this.mList.items.Add(Localization.knownLanguages[i]);
				i++;
			}
			this.mList.value = Localization.language;
		}
		EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.OnChange));
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x00011153 File Offset: 0x0000F353
	private void OnChange()
	{
		Localization.language = UIPopupList.current.value;
	}

	// Token: 0x04001EF9 RID: 7929
	private UIPopupList mList;
}
