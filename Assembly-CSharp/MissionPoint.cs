using System;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public class MissionPoint : MonoBehaviour
{
	// Token: 0x06000F19 RID: 3865 RVA: 0x0007E2BC File Offset: 0x0007C4BC
	public void SetMissionNode(MissionPositionNode mpn, int setType)
	{
		Vector3 localPosition;
		localPosition..ctor((float)mpn.iOffsetX, (float)mpn.iOffsetY, 0f);
		base.gameObject.transform.localPosition = localPosition;
		if (this.iType != setType)
		{
			this.iType = setType;
			this.sprMission.spriteName = this.strMissionSpriteName[this.iType];
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x0000A2FA File Offset: 0x000084FA
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x0007E32C File Offset: 0x0007C52C
	public void SetHover(bool bHover)
	{
		if (bHover)
		{
			this.sprMission.spriteName = this.strMissionSpriteName[this.iType] + "Hover";
		}
		else
		{
			this.sprMission.spriteName = this.strMissionSpriteName[this.iType];
		}
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x0000A308 File Offset: 0x00008508
	public void Select(bool bSelect)
	{
		this.sprDirt.gameObject.SetActive(bSelect);
	}

	// Token: 0x040011D3 RID: 4563
	public int iIndex;

	// Token: 0x040011D4 RID: 4564
	public UISprite sprMission;

	// Token: 0x040011D5 RID: 4565
	public UISprite sprDirt;

	// Token: 0x040011D6 RID: 4566
	private int iType;

	// Token: 0x040011D7 RID: 4567
	private string[] strMissionSpriteName = new string[]
	{
		"none",
		"Mission",
		"Quest",
		"Quest",
		"Quest",
		"Smuggle"
	};
}
