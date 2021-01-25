using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003FC RID: 1020
public class NcDrawFpsRect : MonoBehaviour
{
	// Token: 0x060018A6 RID: 6310 RVA: 0x0000FF95 File Offset: 0x0000E195
	private void Start()
	{
		base.StartCoroutine(this.FPS());
	}

	// Token: 0x060018A7 RID: 6311 RVA: 0x0000FFA4 File Offset: 0x0000E1A4
	private void Update()
	{
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x000C8E84 File Offset: 0x000C7084
	private IEnumerator FPS()
	{
		for (;;)
		{
			float fps = this.accum / (float)this.frames;
			this.sFPS = fps.ToString("f" + Mathf.Clamp(this.nbDecimal, 0, 10));
			this.color = ((fps < 30f) ? ((fps <= 10f) ? Color.red : Color.yellow) : Color.green);
			this.accum = 0f;
			this.frames = 0;
			yield return new WaitForSeconds(this.frequency);
		}
		yield break;
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x000C8EA0 File Offset: 0x000C70A0
	private void OnGUI()
	{
		if (this.style == null)
		{
			this.style = new GUIStyle(GUI.skin.label);
			this.style.normal.textColor = Color.white;
			this.style.alignment = 4;
		}
		GUI.color = ((!this.updateColor) ? Color.white : this.color);
		Rect rect = this.startRect;
		if (this.centerTop)
		{
			rect.x += (float)(Screen.width / 2) - rect.width / 2f;
		}
		this.startRect = GUI.Window(0, rect, new GUI.WindowFunction(this.DoMyWindow), string.Empty);
		if (this.centerTop)
		{
			this.startRect.x = this.startRect.x - ((float)(Screen.width / 2) - rect.width / 2f);
		}
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x000C8F98 File Offset: 0x000C7198
	private void DoMyWindow(int windowID)
	{
		GUI.Label(new Rect(0f, 0f, this.startRect.width, this.startRect.height), this.sFPS + " FPS", this.style);
		if (this.allowDrag)
		{
			GUI.DragWindow(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height));
		}
	}

	// Token: 0x04001D0C RID: 7436
	public bool centerTop = true;

	// Token: 0x04001D0D RID: 7437
	public Rect startRect = new Rect(0f, 0f, 75f, 50f);

	// Token: 0x04001D0E RID: 7438
	public bool updateColor = true;

	// Token: 0x04001D0F RID: 7439
	public bool allowDrag = true;

	// Token: 0x04001D10 RID: 7440
	public float frequency = 0.5f;

	// Token: 0x04001D11 RID: 7441
	public int nbDecimal = 1;

	// Token: 0x04001D12 RID: 7442
	private float accum;

	// Token: 0x04001D13 RID: 7443
	private int frames;

	// Token: 0x04001D14 RID: 7444
	private Color color = Color.white;

	// Token: 0x04001D15 RID: 7445
	private string sFPS = string.Empty;

	// Token: 0x04001D16 RID: 7446
	private GUIStyle style;
}
