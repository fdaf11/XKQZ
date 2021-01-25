using System;
using UnityEngine;

// Token: 0x020007B2 RID: 1970
public class VersionLabel : MonoBehaviour
{
	// Token: 0x06003036 RID: 12342 RVA: 0x00176364 File Offset: 0x00174564
	private void Start()
	{
		VersionLabel[] array = Object.FindObjectsOfType(typeof(VersionLabel)) as VersionLabel[];
		if (array.Length > 1)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003037 RID: 12343 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06003038 RID: 12344 RVA: 0x0017639C File Offset: 0x0017459C
	private void OnGUI()
	{
		GUI.depth = 0;
		if (this.menu)
		{
			GUI.Label(new Rect((float)(Screen.width / 2 - 105), (float)(Screen.height - 40), 450f, 25f), "TBTK version1.0 Demo by K.SongTan");
			GUI.Label(new Rect((float)(Screen.width / 2 - 101), (float)(Screen.height - 24), 450f, 25f), "http://song-gamedev.blogspot.co.uk/");
		}
		else
		{
			GUI.Label(new Rect(5f, 5f, 450f, 25f), "TBTK version1.0 Demo by K.SongTan");
			GUI.Label(new Rect(5f, 20f, 450f, 25f), "http://song-gamedev.blogspot.co.uk/");
		}
	}

	// Token: 0x04003BE6 RID: 15334
	public bool menu;
}
