using System;
using UnityEngine;

// Token: 0x0200026D RID: 621
public class UIAudioData
{
	// Token: 0x04000D34 RID: 3380
	public UIAudioData.eUIAudio audioType = UIAudioData.eUIAudio.None;

	// Token: 0x04000D35 RID: 3381
	public AudioClip audioClip;

	// Token: 0x0200026E RID: 622
	public enum eUIAudio
	{
		// Token: 0x04000D37 RID: 3383
		None = -1,
		// Token: 0x04000D38 RID: 3384
		ClickOK,
		// Token: 0x04000D39 RID: 3385
		Hover,
		// Token: 0x04000D3A RID: 3386
		Cancel,
		// Token: 0x04000D3B RID: 3387
		AddSub,
		// Token: 0x04000D3C RID: 3388
		Weapon,
		// Token: 0x04000D3D RID: 3389
		Armor,
		// Token: 0x04000D3E RID: 3390
		Necklace,
		// Token: 0x04000D3F RID: 3391
		UseItem,
		// Token: 0x04000D40 RID: 3392
		TipsBook,
		// Token: 0x04000D41 RID: 3393
		UpAndDown
	}
}
