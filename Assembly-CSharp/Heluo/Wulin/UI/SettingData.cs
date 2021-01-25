using System;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002F8 RID: 760
	public class SettingData
	{
		// Token: 0x06001020 RID: 4128 RVA: 0x0008914C File Offset: 0x0008734C
		public SettingData()
		{
			this.settingDataNode = new SettingDataNode();
			this.keyboard = new KeyCode[Enum.GetValues(typeof(KeyControl.Key)).Length];
			this.joystick = new KeyCode[Enum.GetValues(typeof(KeyControl.Key)).Length];
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x000891A8 File Offset: 0x000873A8
		public SettingData Clone()
		{
			return new SettingData
			{
				iScreenWidth = this.iScreenWidth,
				iScreenHeight = this.iScreenHeight,
				bFullScreen = this.bFullScreen,
				bOwnSetting = this.bOwnSetting,
				settingDataNode = this.settingDataNode.Clone(),
				bSSAA = this.bSSAA,
				fSSAA = this.fSSAA,
				fMusicValue = this.fMusicValue,
				fSoundValue = this.fSoundValue,
				keyboard = this.keyboard,
				joystick = this.joystick
			};
		}

		// Token: 0x04001324 RID: 4900
		public int iScreenWidth;

		// Token: 0x04001325 RID: 4901
		public int iScreenHeight;

		// Token: 0x04001326 RID: 4902
		public bool bFullScreen;

		// Token: 0x04001327 RID: 4903
		public bool bOwnSetting;

		// Token: 0x04001328 RID: 4904
		public SettingDataNode settingDataNode;

		// Token: 0x04001329 RID: 4905
		public bool bSSAA;

		// Token: 0x0400132A RID: 4906
		public float fSSAA;

		// Token: 0x0400132B RID: 4907
		public float fMusicValue;

		// Token: 0x0400132C RID: 4908
		public float fSoundValue;

		// Token: 0x0400132D RID: 4909
		public KeyCode[] keyboard;

		// Token: 0x0400132E RID: 4910
		public KeyCode[] joystick;
	}
}
