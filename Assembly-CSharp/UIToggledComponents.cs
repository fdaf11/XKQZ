using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000488 RID: 1160
[RequireComponent(typeof(UIToggle))]
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Toggled Components")]
public class UIToggledComponents : MonoBehaviour
{
	// Token: 0x06001C01 RID: 7169 RVA: 0x000DB0DC File Offset: 0x000D92DC
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

	// Token: 0x06001C02 RID: 7170 RVA: 0x000DB178 File Offset: 0x000D9378
	public void Toggle()
	{
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				MonoBehaviour monoBehaviour = this.activate[i];
				monoBehaviour.enabled = UIToggle.current.value;
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				MonoBehaviour monoBehaviour2 = this.deactivate[j];
				monoBehaviour2.enabled = !UIToggle.current.value;
			}
		}
	}

	// Token: 0x040020E2 RID: 8418
	public List<MonoBehaviour> activate;

	// Token: 0x040020E3 RID: 8419
	public List<MonoBehaviour> deactivate;

	// Token: 0x040020E4 RID: 8420
	[HideInInspector]
	[SerializeField]
	private MonoBehaviour target;

	// Token: 0x040020E5 RID: 8421
	[SerializeField]
	[HideInInspector]
	private bool inverse;
}
