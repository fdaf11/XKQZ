using System;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000272 RID: 626
public class InsideOutsideMusic : MonoBehaviour
{
	// Token: 0x06000B7A RID: 2938 RVA: 0x0005E000 File Offset: 0x0005C200
	public void PlayInsideMusic()
	{
		string name = base.name;
		string strAreaName = name.Substring(0, 5);
		string text = name.Substring(5, name.Length - 5);
		int num;
		if (!int.TryParse(text, ref num))
		{
			num = 0;
		}
		GameGlobal.m_iMusicIndex = num;
		Game.PlayBGMusicArea(strAreaName, num);
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x0005E04C File Offset: 0x0005C24C
	public void PlayOutsideMusic()
	{
		string name = base.name;
		string strAreaName = name.Substring(0, 5);
		int num = 0;
		GameGlobal.m_iMusicIndex = num;
		Game.PlayBGMusicArea(strAreaName, num);
	}
}
