using System;
using UnityEngine;

// Token: 0x020006AD RID: 1709
public class ShowcaseGUI : MonoBehaviour
{
	// Token: 0x06002950 RID: 10576 RVA: 0x0001B29B File Offset: 0x0001949B
	private void Start()
	{
		if (ShowcaseGUI.instance)
		{
			Object.Destroy(base.gameObject);
		}
		ShowcaseGUI.instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		this.OnLevelWasLoaded(0);
	}

	// Token: 0x06002951 RID: 10577 RVA: 0x00147568 File Offset: 0x00145768
	private void OnLevelWasLoaded(int level)
	{
		GameObject gameObject = GameObject.Find("Floor_Tile");
		if (gameObject)
		{
			foreach (object obj in gameObject.transform)
			{
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06002952 RID: 10578 RVA: 0x001475E8 File Offset: 0x001457E8
	private void OnGUI()
	{
		int width = Screen.width;
		int num = 30;
		int num2 = 40;
		Rect rect;
		rect..ctor((float)(width - num * 2 - 70), 10f, (float)num, (float)num2);
		if (Application.loadedLevel > 0 && GUI.Button(rect, "<"))
		{
			Application.LoadLevel(Application.loadedLevel - 1);
		}
		else if (GUI.Button(new Rect(rect), "<"))
		{
			Application.LoadLevel(this.levels - 1);
		}
		GUI.Box(new Rect((float)(width - num - 70), 10f, 60f, (float)num2), string.Concat(new object[]
		{
			"Scene:\n",
			Application.loadedLevel + 1,
			" / ",
			this.levels
		}));
		Rect rect2;
		rect2..ctor((float)(width - num - 10), 10f, (float)num, (float)num2);
		if (Application.loadedLevel < this.levels - 1 && GUI.Button(new Rect(rect2), ">"))
		{
			Application.LoadLevel(Application.loadedLevel + 1);
		}
		else if (GUI.Button(new Rect(rect2), ">"))
		{
			Application.LoadLevel(0);
		}
	}

	// Token: 0x0400344D RID: 13389
	private static ShowcaseGUI instance;

	// Token: 0x0400344E RID: 13390
	private int levels = 8;
}
