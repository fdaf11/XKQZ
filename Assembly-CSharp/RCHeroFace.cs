using System;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x020002E4 RID: 740
public class RCHeroFace : MonoBehaviour
{
	// Token: 0x06000F24 RID: 3876 RVA: 0x0007E800 File Offset: 0x0007CA00
	public void UpdateHeroFace(CharacterData chardata)
	{
		base.gameObject.SetActive(true);
		this.sprBox.gameObject.SetActive(true);
		this.texFace.gameObject.SetActive(true);
		string strSmallImage = chardata._NpcDataNode.m_strSmallImage;
		string name = "2dtexture/gameui/hexhead/" + strSmallImage;
		if (Game.g_HexHeadBundle.Contains(name))
		{
			this.texFace.mainTexture = (Game.g_HexHeadBundle.Load(name) as Texture2D);
		}
		else
		{
			name = "2dtexture/gameui/bighead/B000001";
			if (Game.g_BigHeadBundle.Contains(name))
			{
				this.texFace.mainTexture = (Game.g_BigHeadBundle.Load(name) as Texture2D);
			}
			else
			{
				this.texFace.mainTexture = null;
			}
		}
		this.sprBox.spriteName = this.strHeroSpriteName;
		if (chardata.iHurtTurn > 0)
		{
			this.sprHurt.gameObject.SetActive(true);
		}
		else
		{
			this.sprHurt.gameObject.SetActive(false);
		}
		this.characterData = chardata;
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x0000A2FA File Offset: 0x000084FA
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x0000A388 File Offset: 0x00008588
	public void SetHover(bool bHover)
	{
		this.sprHover.gameObject.SetActive(bHover);
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x0007E910 File Offset: 0x0007CB10
	public void OpenDLCCharacterStatus()
	{
		UIDLCCharacter uidlccharacter = Game.UI.Get<UIDLCCharacter>();
		uidlccharacter.Show(UIDLCCharacter.UIType.Kinght, this.characterData.iNpcID.ToString());
	}

	// Token: 0x06000F28 RID: 3880 RVA: 0x0007E940 File Offset: 0x0007CB40
	public void ShowBackOnly()
	{
		this.sprBack.gameObject.SetActive(true);
		this.sprBox.gameObject.SetActive(false);
		this.texFace.gameObject.SetActive(false);
		this.sprHurt.gameObject.SetActive(false);
	}

	// Token: 0x04001201 RID: 4609
	public int iIndex;

	// Token: 0x04001202 RID: 4610
	public UISprite sprBack;

	// Token: 0x04001203 RID: 4611
	public UISprite sprBox;

	// Token: 0x04001204 RID: 4612
	public UITexture texFace;

	// Token: 0x04001205 RID: 4613
	public UISprite sprHurt;

	// Token: 0x04001206 RID: 4614
	public UISprite sprHover;

	// Token: 0x04001207 RID: 4615
	public CharacterData characterData;

	// Token: 0x04001208 RID: 4616
	private string strHeroSpriteName = "FaceLevel3";

	// Token: 0x04001209 RID: 4617
	private string[] strMinorSpriteName = new string[]
	{
		string.Empty,
		"FaceLevel1",
		"FaceLevel2",
		"FaceLevel3"
	};
}
