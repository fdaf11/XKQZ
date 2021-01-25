using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200085F RID: 2143
	[USequencerEvent("Application/Load Level")]
	[USequencerFriendlyName("Load Level")]
	public class USLoadLevelEvent : USEventBase
	{
		// Token: 0x060033D4 RID: 13268 RVA: 0x0018E800 File Offset: 0x0018CA00
		public override void FireEvent()
		{
			if (this.levelName.Length == 0 && this.levelIndex < 0)
			{
				Debug.LogError("You have a Load Level event in your sequence, however, you didn't give it a level to load.");
				return;
			}
			if (this.levelIndex >= Application.levelCount)
			{
				Debug.LogError("You tried to load a level that is invalid, the level index is out of range.");
				return;
			}
			if (!Application.isPlaying && !this.fireInEditor)
			{
				Debug.Log("Load Level Fired, but it wasn't processed, since we are in the editor. Please set the fire In Editor flag in the inspector if you require this behaviour.");
				return;
			}
			if (this.levelName.Length != 0)
			{
				Application.LoadLevel(this.levelName);
			}
			if (this.levelIndex != -1)
			{
				Application.LoadLevel(this.levelIndex);
			}
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x04003FF1 RID: 16369
		public bool fireInEditor;

		// Token: 0x04003FF2 RID: 16370
		public string levelName = string.Empty;

		// Token: 0x04003FF3 RID: 16371
		public int levelIndex = -1;
	}
}
