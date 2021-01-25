using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x0200026F RID: 623
public class UIAudioDataManager
{
	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000B66 RID: 2918 RVA: 0x00008F42 File Offset: 0x00007142
	public static UIAudioDataManager Singleton
	{
		get
		{
			return UIAudioDataManager.instance;
		}
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x0005DB58 File Offset: 0x0005BD58
	public AudioClip GetUIAudio(UIAudioData.eUIAudio eType)
	{
		if (eType < UIAudioData.eUIAudio.ClickOK || eType >= (UIAudioData.eUIAudio)this.audioname.Length)
		{
			return null;
		}
		string name = "audio/UI/SFX/" + this.audioname[(int)eType];
		return Game.g_AudioBundle.Load(name) as AudioClip;
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x0005DBA4 File Offset: 0x0005BDA4
	public void PlayUIAudio(UIAudioData.eUIAudio type)
	{
		AudioClip uiaudio = this.GetUIAudio(type);
		if (uiaudio == null)
		{
			Debug.Log("NO Audio Clip");
			return;
		}
		Debug.Log(string.Concat(new object[]
		{
			"Play Sound : ",
			uiaudio.name,
			"GameGlobal.m_fSoundValue : ",
			GameGlobal.m_fSoundValue
		}));
		NGUITools.PlaySound(uiaudio, GameGlobal.m_fSoundValue, 1f);
	}

	// Token: 0x04000D42 RID: 3394
	private static readonly UIAudioDataManager instance = new UIAudioDataManager();

	// Token: 0x04000D43 RID: 3395
	public List<AudioClip> UIAudioList = new List<AudioClip>();

	// Token: 0x04000D44 RID: 3396
	public string[] audioname = new string[]
	{
		"Select09",
		"Slide07",
		"CantUse",
		"Select10",
		"Select21",
		"Select22",
		"Select23",
		"Select16",
		"Select08",
		"Select01"
	};
}
