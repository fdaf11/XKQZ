using System;
using UnityEngine;

// Token: 0x02000478 RID: 1144
[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06001BA4 RID: 7076 RVA: 0x00012758 File Offset: 0x00010958
	private string key
	{
		get
		{
			return (!string.IsNullOrEmpty(this.keyName)) ? this.keyName : ("NGUI State: " + base.name);
		}
	}

	// Token: 0x06001BA5 RID: 7077 RVA: 0x00012785 File Offset: 0x00010985
	private void Awake()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.mCheck = base.GetComponent<UIToggle>();
	}

	// Token: 0x06001BA6 RID: 7078 RVA: 0x000D8210 File Offset: 0x000D6410
	private void OnEnable()
	{
		if (this.mList != null)
		{
			EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
		}
		if (this.mCheck != null)
		{
			EventDelegate.Add(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
		}
		if (this.mList != null)
		{
			string @string = PlayerPrefs.GetString(this.key);
			if (!string.IsNullOrEmpty(@string))
			{
				this.mList.value = @string;
			}
			return;
		}
		if (this.mCheck != null)
		{
			this.mCheck.value = (PlayerPrefs.GetInt(this.key, (!this.mCheck.startsActive) ? 0 : 1) != 0);
		}
		else
		{
			string string2 = PlayerPrefs.GetString(this.key);
			UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				UIToggle uitoggle = componentsInChildren[i];
				uitoggle.value = (uitoggle.name == string2);
				i++;
			}
		}
	}

	// Token: 0x06001BA7 RID: 7079 RVA: 0x000D833C File Offset: 0x000D653C
	private void OnDisable()
	{
		if (this.mCheck != null)
		{
			EventDelegate.Remove(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
		}
		if (this.mList != null)
		{
			EventDelegate.Remove(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
		}
		if (this.mCheck == null && this.mList == null)
		{
			UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				UIToggle uitoggle = componentsInChildren[i];
				if (uitoggle.value)
				{
					PlayerPrefs.SetString(this.key, uitoggle.name);
					break;
				}
				i++;
			}
		}
	}

	// Token: 0x06001BA8 RID: 7080 RVA: 0x0001279F File Offset: 0x0001099F
	public void SaveSelection()
	{
		PlayerPrefs.SetString(this.key, UIPopupList.current.value);
	}

	// Token: 0x06001BA9 RID: 7081 RVA: 0x000127B6 File Offset: 0x000109B6
	public void SaveState()
	{
		PlayerPrefs.SetInt(this.key, (!UIToggle.current.value) ? 0 : 1);
	}

	// Token: 0x04002079 RID: 8313
	public string keyName;

	// Token: 0x0400207A RID: 8314
	private UIPopupList mList;

	// Token: 0x0400207B RID: 8315
	private UIToggle mCheck;
}
