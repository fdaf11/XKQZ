using System;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class BattleUITactic : MonoBehaviour
{
	// Token: 0x060005B7 RID: 1463 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x0000264F File Offset: 0x0000084F
	public void SetTacticItem(int iID, int iPoint)
	{
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x00042018 File Offset: 0x00040218
	public void SetTacticItemEnable(bool bEnable)
	{
		if (bEnable)
		{
			this.bTacticEnable = true;
			this.tacticName.color = new Color(1f, 1f, 1f);
		}
		else
		{
			this.bTacticEnable = false;
			this.tacticName.color = new Color(0.5f, 0.5f, 0.5f);
		}
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x0000559B File Offset: 0x0000379B
	public void SetTacticFace(UnitTB unit)
	{
		this.tacticName.text = unit.unitName;
		this.iTacticID = unit.iUnitID;
		this.tacticTex.mainTexture = unit.iconTalk;
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x0004207C File Offset: 0x0004027C
	public void SetTacticNeigong(NpcNeigong node)
	{
		this.tacticName.text = node.m_Neigong.m_strNeigongName;
		this.iTacticID = node.m_Neigong.m_iNeigongID;
		Texture2D texture2D = Game.g_NeigongBundle.Load("2dtexture/gameui/neigong/" + node.m_Neigong.sIconImage) as Texture2D;
		if (texture2D == null)
		{
			texture2D = (Game.g_NeigongBundle.Load("2dtexture/gameui/neigong/NeigongImage21") as Texture2D);
		}
		if (texture2D != null)
		{
			this.tacticTex.mainTexture = texture2D;
		}
		else
		{
			this.tacticTex.mainTexture = null;
		}
	}

	// Token: 0x04000628 RID: 1576
	public UILabel tacticName;

	// Token: 0x04000629 RID: 1577
	public UITexture tacticTex;

	// Token: 0x0400062A RID: 1578
	public UISprite tacticSpr;

	// Token: 0x0400062B RID: 1579
	public UISprite tacticBox;

	// Token: 0x0400062C RID: 1580
	public int iTacticID;

	// Token: 0x0400062D RID: 1581
	public bool bTacticEnable;
}
