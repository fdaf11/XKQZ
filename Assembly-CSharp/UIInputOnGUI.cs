using System;
using UnityEngine;

// Token: 0x020004F9 RID: 1273
[RequireComponent(typeof(UIInput))]
public class UIInputOnGUI : MonoBehaviour
{
	// Token: 0x06002031 RID: 8241 RVA: 0x000157C2 File Offset: 0x000139C2
	private void Awake()
	{
		this.mInput = base.GetComponent<UIInput>();
	}

	// Token: 0x06002032 RID: 8242 RVA: 0x000157D0 File Offset: 0x000139D0
	private void OnGUI()
	{
		if (Event.current.rawType == 4)
		{
			this.mInput.ProcessEvent(Event.current);
		}
	}

	// Token: 0x040023BE RID: 9150
	[NonSerialized]
	private UIInput mInput;
}
