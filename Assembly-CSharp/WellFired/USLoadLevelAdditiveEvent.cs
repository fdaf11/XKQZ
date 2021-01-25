using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200085E RID: 2142
	[USequencerFriendlyName("Load Level Additively")]
	[USequencerEvent("Application/Load Level Additive")]
	public class USLoadLevelAdditiveEvent : USEventBase
	{
		// Token: 0x060033D1 RID: 13265 RVA: 0x0018E75C File Offset: 0x0018C95C
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
				Application.LoadLevelAdditive(this.levelName);
			}
			if (this.levelIndex != -1)
			{
				Application.LoadLevelAdditive(this.levelIndex);
			}
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x04003FEE RID: 16366
		public bool fireInEditor;

		// Token: 0x04003FEF RID: 16367
		public string levelName = string.Empty;

		// Token: 0x04003FF0 RID: 16368
		public int levelIndex = -1;
	}
}
