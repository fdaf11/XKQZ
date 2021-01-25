using System;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x02000006 RID: 6
	public class Crosshairs : MonoBehaviour
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00021D58 File Offset: 0x0001FF58
		private void Start()
		{
			this.halfDrawSize = (float)this.drawSize / 2f;
			float num = (float)this.drawSize;
			this.drawRect.height = num;
			this.drawRect.width = num;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000253B File Offset: 0x0000073B
		private void Update()
		{
			this.drawRect.x = (float)Screen.width / 2f - this.halfDrawSize;
			this.drawRect.y = (float)Screen.height / 2f - this.halfDrawSize;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002579 File Offset: 0x00000779
		private void OnGUI()
		{
			GUI.DrawTexture(this.drawRect, this.crosshairs);
		}

		// Token: 0x04000006 RID: 6
		public Texture2D crosshairs;

		// Token: 0x04000007 RID: 7
		public int drawSize = 32;

		// Token: 0x04000008 RID: 8
		private float halfDrawSize;

		// Token: 0x04000009 RID: 9
		private Rect drawRect = new Rect(0f, 0f, 0f, 0f);
	}
}
