using System;
using UnityEngine;

// Token: 0x020005A9 RID: 1449
public class ScenePicker : MonoBehaviour
{
	// Token: 0x0600243C RID: 9276 RVA: 0x0000501F File Offset: 0x0000321F
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0600243D RID: 9277 RVA: 0x0011BAE8 File Offset: 0x00119CE8
	private void OnGUI()
	{
		if (GUI.Button(new Rect((float)(Screen.width / 2 - 50), 50f, 100f, 20f), "Next Scene"))
		{
			if (this.currentScene == 8)
			{
				this.currentScene = 0;
				Application.LoadLevel(this.currentScene);
				Object.Destroy(base.gameObject);
			}
			else
			{
				this.currentScene++;
			}
			Application.LoadLevel(this.currentScene);
		}
	}

	// Token: 0x04002C0C RID: 11276
	private int currentScene;
}
