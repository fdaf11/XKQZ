using System;
using UnityEngine;

// Token: 0x02000433 RID: 1075
[AddComponentMenu("NGUI/Examples/Load Level On Click")]
public class LoadLevelOnClick : MonoBehaviour
{
	// Token: 0x060019FB RID: 6651 RVA: 0x00010ECB File Offset: 0x0000F0CB
	private void OnClick()
	{
		if (!string.IsNullOrEmpty(this.levelName))
		{
			Application.LoadLevel(this.levelName);
		}
	}

	// Token: 0x04001EC1 RID: 7873
	public string levelName;
}
