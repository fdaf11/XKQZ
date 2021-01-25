using System;
using System.Collections;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020007F6 RID: 2038
public class UITeamMember : MonoBehaviour
{
	// Token: 0x06003209 RID: 12809 RVA: 0x001845B4 File Offset: 0x001827B4
	private void Start()
	{
		this.strLv[0] = Game.StringTable.GetString(263080);
		this.strLv[1] = Game.StringTable.GetString(263081);
		this.strLv[2] = Game.StringTable.GetString(263082);
		this.strLv[3] = Game.StringTable.GetString(263083);
		this.strLv[4] = Game.StringTable.GetString(263084);
		this.strLv[5] = Game.StringTable.GetString(263085);
		this.strLv[6] = Game.StringTable.GetString(263086);
		this.strLv[7] = Game.StringTable.GetString(263087);
		this.strLv[8] = Game.StringTable.GetString(263088);
		this.strLv[9] = Game.StringTable.GetString(263089);
		this.strLv[10] = Game.StringTable.GetString(263090);
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x0600320B RID: 12811 RVA: 0x0001F804 File Offset: 0x0001DA04
	public void SetCharacterData(CharacterData charData, int iValue)
	{
		base.StartCoroutine(this.DoCharPlusExp(charData, iValue));
	}

	// Token: 0x0600320C RID: 12812 RVA: 0x001846C0 File Offset: 0x001828C0
	private IEnumerator DoCharPlusExp(CharacterData charData, int iValue)
	{
		this.texFace.mainTexture = (Game.g_HexHeadBundle.Load("2dtexture/gameui/hexhead/" + charData._NpcDataNode.m_strSmallImage) as Texture);
		this.labName.text = charData._NpcDataNode.m_strNpcName;
		this.labRoution.text = charData.GetNowPracticeName();
		int lv = charData.GetNowPracticeLevel();
		if (lv < this.strLv.Length)
		{
			this.labLv.text = this.strLv[lv];
		}
		else
		{
			this.labLv.text = this.strLv[0];
		}
		float fTalent = 1f;
		iValue = Mathf.FloorToInt((float)iValue * fTalent);
		this.labExp.text = iValue.ToString();
		int Cur = charData.GetNowPracticeCurExp();
		int Next = charData.GetNowPracticeNextExp();
		float fval = (float)Cur;
		fval /= (float)Next;
		fval = Mathf.Clamp01(fval);
		this.pbExpBar.value = fval;
		this.pbExpBar.gameObject.GetComponentInChildren<UILabel>().text = Cur.ToString() + " / " + Next.ToString();
		this.labProperty.text = string.Empty;
		yield return null;
		int iCount = 50;
		int iStepValue = iValue / iCount;
		int iPlusValue = iValue % iCount;
		int iLvUpCount = 0;
		int iret = 0;
		for (int i = 0; i < iCount; i++)
		{
			yield return new WaitForSeconds(0.02f);
			if (i == 0)
			{
				iret = charData.SetNowPracticeExp(iStepValue + iPlusValue);
			}
			else
			{
				iret = charData.SetNowPracticeExp(iStepValue);
			}
			if (iret > 0)
			{
				iLvUpCount += iret;
			}
			lv = charData.GetNowPracticeLevel();
			if (lv < this.strLv.Length)
			{
				this.labLv.text = this.strLv[lv];
			}
			else
			{
				this.labLv.text = this.strLv[0];
			}
			Cur = charData.GetNowPracticeCurExp();
			Next = charData.GetNowPracticeNextExp();
			fval = (float)Cur;
			fval /= (float)Next;
			fval = Mathf.Clamp01(fval);
			this.pbExpBar.value = fval;
			this.pbExpBar.gameObject.GetComponentInChildren<UILabel>().text = Cur.ToString() + " / " + Next.ToString();
			if (iret > 0)
			{
				this.labProperty.text = charData.GetNowPracticeLvupText(iLvUpCount);
			}
		}
		NPC.m_instance.BattleNowPracticeLvUp(charData);
		UINGUI.instance.uiGameOverMenu.CharDone();
		yield break;
	}

	// Token: 0x0600320D RID: 12813 RVA: 0x0001F815 File Offset: 0x0001DA15
	public void SetCharacterDataDLC(CharacterData charData, int iValue)
	{
		base.StartCoroutine(this.DoCharPlusExpDLC(charData, iValue));
	}

	// Token: 0x0600320E RID: 12814 RVA: 0x001846F8 File Offset: 0x001828F8
	private IEnumerator DoCharPlusExpDLC(CharacterData charData, int iValue)
	{
		this.texFace.mainTexture = (Game.g_HexHeadBundle.Load("2dtexture/gameui/hexhead/" + charData._NpcDataNode.m_strSmallImage) as Texture);
		this.labName.text = charData._NpcDataNode.m_strNpcName;
		if (charData.GetNowUseNeigong() != null)
		{
			this.labRoution.text = charData.GetNowUseNeigong().m_Neigong.m_strNeigongName;
		}
		else
		{
			this.labRoution.text = string.Empty;
		}
		int lv = charData.iLevel;
		this.labLv.text = lv.ToString();
		this.labExp.text = iValue.ToString();
		int Cur = charData.DLC_NowLevelExp();
		int Next = charData.DLC_NowLevelUpExp();
		float fval = (float)Cur;
		fval /= (float)Next;
		fval = Mathf.Clamp01(fval);
		this.pbExpBar.value = fval;
		this.pbExpBar.gameObject.GetComponentInChildren<UILabel>().text = Cur.ToString() + " / " + Next.ToString();
		this.labProperty.text = string.Empty;
		yield return null;
		int iCount = 50;
		int iStepValue = iValue / iCount;
		int iPlusValue = iValue % iCount;
		int iLvUpCount = 0;
		int iret = 0;
		charData.ClearLevelupString();
		for (int i = 0; i < iCount; i++)
		{
			yield return new WaitForSeconds(0.02f);
			if (i == 0)
			{
				iret = charData.DLC_AddExp(iStepValue + iPlusValue);
			}
			else
			{
				iret = charData.DLC_AddExp(iStepValue);
			}
			if (iret > 0)
			{
				iLvUpCount += iret;
			}
			lv = charData.iLevel;
			this.labLv.text = lv.ToString();
			Cur = charData.DLC_NowLevelExp();
			Next = charData.DLC_NowLevelUpExp();
			fval = (float)Cur;
			fval /= (float)Next;
			fval = Mathf.Clamp01(fval);
			this.pbExpBar.value = fval;
			this.pbExpBar.gameObject.GetComponentInChildren<UILabel>().text = Cur.ToString() + " / " + Next.ToString();
			if (iret > 0)
			{
				this.labProperty.text = charData.strLevelup;
			}
		}
		UINGUI.instance.uiGameOverMenu.CharDone();
		yield break;
	}

	// Token: 0x04003DB6 RID: 15798
	public UITexture texFace;

	// Token: 0x04003DB7 RID: 15799
	public UILabel labName;

	// Token: 0x04003DB8 RID: 15800
	public UILabel labRoution;

	// Token: 0x04003DB9 RID: 15801
	public UILabel labLv;

	// Token: 0x04003DBA RID: 15802
	public UILabel labExp;

	// Token: 0x04003DBB RID: 15803
	public UIProgressBar pbExpBar;

	// Token: 0x04003DBC RID: 15804
	public UILabel labProperty;

	// Token: 0x04003DBD RID: 15805
	private string[] strLv = new string[]
	{
		"無",
		"一重",
		"二重",
		"三重",
		"四重",
		"五重",
		"六重",
		"七重",
		"八重",
		"九重",
		"十重"
	};
}
