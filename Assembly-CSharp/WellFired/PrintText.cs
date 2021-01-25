using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000866 RID: 2150
	[USequencerFriendlyName("Print Text")]
	public class PrintText : USEventBase
	{
		// Token: 0x060033F5 RID: 13301 RVA: 0x00020AAC File Offset: 0x0001ECAC
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

		// Token: 0x060033F6 RID: 13302 RVA: 0x0018ED8C File Offset: 0x0018CF8C
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

		// Token: 0x060033F7 RID: 13303 RVA: 0x00020AE8 File Offset: 0x0001ECE8
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x00020AF0 File Offset: 0x0001ECF0
		public override void UndoEvent()
		{
			this.currentText = this.priorText;
			this.display = false;
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x0018EE00 File Offset: 0x0018D000
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

		// Token: 0x04003FFD RID: 16381
		public USEventBase.UILayer uiLayer;

		// Token: 0x04003FFE RID: 16382
		public string textToPrint = string.Empty;

		// Token: 0x04003FFF RID: 16383
		public Rect position = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);

		// Token: 0x04004000 RID: 16384
		private string priorText = string.Empty;

		// Token: 0x04004001 RID: 16385
		private string currentText = string.Empty;

		// Token: 0x04004002 RID: 16386
		private bool display;

		// Token: 0x04004003 RID: 16387
		public float printRatePerCharacter;
	}
}
