using System;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class BiographiesDebug : MonoBehaviour
{
	// Token: 0x0600102F RID: 4143 RVA: 0x0008A798 File Offset: 0x00088998
	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 70f, 100f, 30f), "列傳ID : ");
		this.id = GUI.TextField(new Rect(100f, 70f, 150f, 20f), this.id);
		if (GUI.Button(new Rect(250f, 40f, 100f, 60f), "開始列傳"))
		{
			Game.UI.Get<UIBiographies>().StartBiographies(this.id);
		}
	}

	// Token: 0x04001338 RID: 4920
	public string id;
}
