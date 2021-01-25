using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000489 RID: 1161
[AddComponentMenu("NGUI/Interaction/Toggled Objects")]
public class UIToggledObjects : MonoBehaviour
{
	// Token: 0x06001C04 RID: 7172 RVA: 0x000DB208 File Offset: 0x000D9408
	private void Awake()
	{
		if (this.target != null)
		{
			if (this.activate.Count == 0 && this.deactivate.Count == 0)
			{
				if (this.inverse)
				{
					this.deactivate.Add(this.target);
				}
				else
				{
					this.activate.Add(this.target);
				}
			}
			else
			{
				this.target = null;
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.Toggle));
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x000DB2A4 File Offset: 0x000D94A4
	public void Toggle()
	{
		bool value = UIToggle.current.value;
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				this.Set(this.activate[i], value);
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				this.Set(this.deactivate[j], !value);
			}
		}
	}

	// Token: 0x06001C06 RID: 7174 RVA: 0x00012B00 File Offset: 0x00010D00
	private void Set(GameObject go, bool state)
	{
		if (go != null)
		{
			NGUITools.SetActive(go, state);
		}
	}

	// Token: 0x040020E6 RID: 8422
	public List<GameObject> activate;

	// Token: 0x040020E7 RID: 8423
	public List<GameObject> deactivate;

	// Token: 0x040020E8 RID: 8424
	[HideInInspector]
	[SerializeField]
	private GameObject target;

	// Token: 0x040020E9 RID: 8425
	[HideInInspector]
	[SerializeField]
	private bool inverse;
}
