using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200086A RID: 2154
	[USequencerEvent("Fullscreen/Print Text")]
	[USequencerFriendlyName("Print Text")]
	public class USPrintTextEvent : USEventBase
	{
		// Token: 0x0600340B RID: 13323 RVA: 0x00020B48 File Offset: 0x0001ED48
		public override void FireEvent()
		{
			this.priorText = this.currentText;
			this.currentText = this.textToPrint;
			if (base.Duration > 0f)
			{
				this.currentText = string.Empty;
			}
			this.display = true;
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x0018F5B4 File Offset: 0x0018D7B4
		public override void ProcessEvent(float deltaTime)
		{
			if (this.printRatePerCharacter <= 0f)
			{
				this.currentText = this.textToPrint;
			}
			else
			{
				int num = (int)(deltaTime / this.printRatePerCharacter);
				if (num < this.textToPrint.Length)
				{
					this.currentText = this.textToPrint.Substring(0, num);
				}
				else
				{
					this.currentText = this.textToPrint;
				}
			}
			this.display = true;
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x00020B84 File Offset: 0x0001ED84
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x00020B8C File Offset: 0x0001ED8C
		public override void UndoEvent()
		{
			this.currentText = this.priorText;
			this.display = false;
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x0018F628 File Offset: 0x0018D828
		private void OnGUI()
		{
			if (!base.Sequence.IsPlaying)
			{
				return;
			}
			if (!this.display)
			{
				return;
			}
			int depth = GUI.depth;
			GUI.depth = this.uiLayer;
			GUI.Label(this.position, this.currentText);
			GUI.depth = depth;
		}

		// Token: 0x04004015 RID: 16405
		public USEventBase.UILayer uiLayer;

		// Token: 0x04004016 RID: 16406
		public string textToPrint = string.Empty;

		// Token: 0x04004017 RID: 16407
		public Rect position = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);

		// Token: 0x04004018 RID: 16408
		private string priorText = string.Empty;

		// Token: 0x04004019 RID: 16409
		private string currentText = string.Empty;

		// Token: 0x0400401A RID: 16410
		private bool display;

		// Token: 0x0400401B RID: 16411
		public float printRatePerCharacter;
	}
}
