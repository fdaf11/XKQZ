using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200042C RID: 1068
[RequireComponent(typeof(UITexture))]
public class DownloadTexture : MonoBehaviour
{
	// Token: 0x060019E1 RID: 6625 RVA: 0x000CFFCC File Offset: 0x000CE1CC
	private IEnumerator Start()
	{
		WWW www = new WWW(this.url);
		yield return www;
		this.mTex = www.texture;
		if (this.mTex != null)
		{
			UITexture ut = base.GetComponent<UITexture>();
			ut.mainTexture = this.mTex;
			if (this.pixelPerfect)
			{
				ut.MakePixelPerfect();
			}
		}
		www.Dispose();
		yield break;
	}

	// Token: 0x060019E2 RID: 6626 RVA: 0x00010D7F File Offset: 0x0000EF7F
	private void OnDestroy()
	{
		if (this.mTex != null)
		{
			Object.Destroy(this.mTex);
		}
	}

	// Token: 0x04001EA7 RID: 7847
	public string url = "http://www.yourwebsite.com/logo.png";

	// Token: 0x04001EA8 RID: 7848
	public bool pixelPerfect = true;

	// Token: 0x04001EA9 RID: 7849
	private Texture2D mTex;
}
