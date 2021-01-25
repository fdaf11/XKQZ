using System;
using SWS;
using UnityEngine;

// Token: 0x0200069E RID: 1694
public class CameraInputDemo : MonoBehaviour
{
	// Token: 0x0600291C RID: 10524 RVA: 0x0001B174 File Offset: 0x00019374
	private void Start()
	{
		this.myMove = base.gameObject.GetComponent<splineMove>();
		this.myMove.StartMove();
		this.myMove.Pause();
	}

	// Token: 0x0600291D RID: 10525 RVA: 0x00146690 File Offset: 0x00144890
	private void Update()
	{
		if (this.myMove.tween == null || !this.myMove.tween.isPaused)
		{
			return;
		}
		if (Input.GetKeyDown(273))
		{
			this.myMove.Resume();
		}
	}

	// Token: 0x0600291E RID: 10526 RVA: 0x001466E0 File Offset: 0x001448E0
	private void OnGUI()
	{
		if (this.myMove.tween != null && !this.myMove.tween.isPaused)
		{
			return;
		}
		GUI.Box(new Rect((float)(Screen.width - 150), (float)(Screen.height / 2), 150f, 100f), string.Empty);
		Rect rect;
		rect..ctor((float)(Screen.width - 130), (float)(Screen.height / 2 + 10), 110f, 90f);
		GUI.Label(rect, this.infoText);
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x0001B19D File Offset: 0x0001939D
	public void ShowInformation(string text)
	{
		this.infoText = text;
	}

	// Token: 0x04003412 RID: 13330
	public string infoText = "Welcome to this customized input example";

	// Token: 0x04003413 RID: 13331
	private splineMove myMove;
}
