using System;
using UnityEngine;

// Token: 0x0200039E RID: 926
public class cFPSConsole : MonoBehaviour
{
	// Token: 0x06001579 RID: 5497 RVA: 0x0000DA8D File Offset: 0x0000BC8D
	private void Start()
	{
		if (this.isVisible)
		{
			this.show(false);
		}
		this.m_gsStyleLabel.fontSize = 40;
		Application.targetFrameRate = -1;
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x000B6BEC File Offset: 0x000B4DEC
	private void Update()
	{
		this.iNowCount++;
		this.fNow += Time.deltaTime;
		while (this.fNow > 1f)
		{
			this.fNow -= 1f;
			this.iFPS = this.iNowCount;
			this.iNowCount = 0;
		}
	}

	// Token: 0x0600157B RID: 5499 RVA: 0x000B6C54 File Offset: 0x000B4E54
	private void OnGUI()
	{
		Rect rect;
		rect..ctor(0f, 0f, 80f, 60f);
		rect = GUILayout.Window(3939889, rect, new GUI.WindowFunction(this.ConsoleWindow), "FPS", new GUILayoutOption[0]);
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x0000DAB4 File Offset: 0x0000BCB4
	private void ConsoleWindow(int windowID)
	{
		this.m_gsStyleLabel.normal.textColor = Color.white;
		GUILayout.Label(this.iFPS.ToString(), this.m_gsStyleLabel, new GUILayoutOption[0]);
	}

	// Token: 0x0600157D RID: 5501 RVA: 0x0000DAE7 File Offset: 0x0000BCE7
	public void toggleVisible()
	{
		this.show(!this.isVisible);
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
	public void show(bool bShow)
	{
		base.enabled = bShow;
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x0600157F RID: 5503 RVA: 0x0000DB01 File Offset: 0x0000BD01
	public bool isVisible
	{
		get
		{
			return base.enabled;
		}
	}

	// Token: 0x04001A14 RID: 6676
	private float fNow;

	// Token: 0x04001A15 RID: 6677
	private int iNowCount;

	// Token: 0x04001A16 RID: 6678
	private int iFPS;

	// Token: 0x04001A17 RID: 6679
	private GUIStyle m_gsStyleLabel = new GUIStyle();
}
