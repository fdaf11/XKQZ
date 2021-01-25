using System;
using UnityEngine;

// Token: 0x020003FE RID: 1022
public class NcDrawFpsText : MonoBehaviour
{
	// Token: 0x060018B2 RID: 6322 RVA: 0x0000FFF0 File Offset: 0x0000E1F0
	private void Start()
	{
		if (!base.guiText)
		{
			Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
			base.enabled = false;
			return;
		}
		this.timeleft = this.updateInterval;
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x000C9128 File Offset: 0x000C7328
	private void Update()
	{
		this.timeleft -= Time.deltaTime;
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			float num = this.accum / (float)this.frames;
			string text = string.Format("{0:F2} FPS", num);
			base.guiText.text = text;
			if (num < 30f)
			{
				base.guiText.material.color = Color.yellow;
			}
			else if (num < 10f)
			{
				base.guiText.material.color = Color.red;
			}
			else
			{
				base.guiText.material.color = Color.green;
			}
			this.timeleft = this.updateInterval;
			this.accum = 0f;
			this.frames = 0;
		}
	}

	// Token: 0x04001D1B RID: 7451
	public float updateInterval = 0.5f;

	// Token: 0x04001D1C RID: 7452
	private float accum;

	// Token: 0x04001D1D RID: 7453
	private int frames;

	// Token: 0x04001D1E RID: 7454
	private float timeleft;
}
