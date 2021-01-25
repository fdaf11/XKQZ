using System;
using UnityEngine;

namespace Exploder
{
	// Token: 0x020000BE RID: 190
	public class HUDFPS : MonoBehaviour
	{
		// Token: 0x060003F9 RID: 1017 RVA: 0x00004BAC File Offset: 0x00002DAC
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

		// Token: 0x060003FA RID: 1018 RVA: 0x00032674 File Offset: 0x00030874
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
					base.guiText.material.color = Color.black;
				}
				this.timeleft = this.updateInterval;
				this.accum = 0f;
				this.frames = 0;
			}
		}

		// Token: 0x0400031A RID: 794
		public float updateInterval = 0.5f;

		// Token: 0x0400031B RID: 795
		private float accum;

		// Token: 0x0400031C RID: 796
		private int frames;

		// Token: 0x0400031D RID: 797
		private float timeleft;
	}
}
