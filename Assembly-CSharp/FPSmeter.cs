using System;
using UnityEngine;

// Token: 0x02000528 RID: 1320
public class FPSmeter : MonoBehaviour
{
	// Token: 0x060021C8 RID: 8648 RVA: 0x00016A4D File Offset: 0x00014C4D
	private void Start()
	{
		this.lastInterval = Time.realtimeSinceStartup;
		this.frames = 0;
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x000FFEA4 File Offset: 0x000FE0A4
	private void OnGUI()
	{
		if (this.showFPS)
		{
			GUI.Label(new Rect(10f, 10f, 100f, 20f), string.Empty + Mathf.Round(FPSmeter.fps * 100f) / 100f);
		}
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x000FFF00 File Offset: 0x000FE100
	private void Update()
	{
		this.frames++;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (realtimeSinceStartup > this.lastInterval + this.updateInterval)
		{
			FPSmeter.fps = (float)this.frames / (realtimeSinceStartup - this.lastInterval);
			this.frames = 0;
			this.lastInterval = realtimeSinceStartup;
		}
	}

	// Token: 0x0400252B RID: 9515
	public float updateInterval = 0.5f;

	// Token: 0x0400252C RID: 9516
	private float lastInterval;

	// Token: 0x0400252D RID: 9517
	private int frames;

	// Token: 0x0400252E RID: 9518
	public static float fps;

	// Token: 0x0400252F RID: 9519
	public bool showFPS;
}
