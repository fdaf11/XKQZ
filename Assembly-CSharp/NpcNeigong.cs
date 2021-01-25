using System;
using Heluo.Wulin;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public class NpcNeigong : NpcSkill
{
	// Token: 0x06000A02 RID: 2562 RVA: 0x00008069 File Offset: 0x00006269
	public NpcNeigong()
	{
		this.m_SkillType = NpcSkill.SkillType.Neigong;
		this.m_Neigong = new NeigongNewDataNode();
		this.iLevel = 1;
		this.iCurExp = 0;
		this.iNextExp = 1;
		this.m_iAccumulationExp = 0;
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x000545C0 File Offset: 0x000527C0
	public NpcNeigong Clone()
	{
		return new NpcNeigong
		{
			iSkillID = this.iSkillID,
			iLevel = this.iLevel,
			iCurExp = this.iCurExp,
			iNextExp = this.iNextExp,
			bUse = this.bUse,
			m_iAccumulationExp = this.m_iAccumulationExp
		};
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x0005461C File Offset: 0x0005281C
	public bool SetNeigongData(int iID)
	{
		NeigongNewDataNode neigongData = Game.NeigongData.GetNeigongData(this.iSkillID);
		if (neigongData == null)
		{
			return false;
		}
		this.m_Neigong = neigongData;
		for (int i = 0; i < this.m_Neigong.m_ConditionEffectList.Count; i++)
		{
			ConditionEffect conditionEffect = this.m_Neigong.m_ConditionEffectList[i];
			if (conditionEffect.m_ConditionList.Count > 0)
			{
				for (int j = 0; j < conditionEffect.m_ConditionList.Count; j++)
				{
					Condition condition = conditionEffect.m_ConditionList[j];
					condition.SetParID(iID);
				}
			}
		}
		return true;
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x000546C0 File Offset: 0x000528C0
	public int SetExp(int iValue)
	{
		RoutineExpNode routineExp = RoutineExpManager.Singleton.GetRoutineExp(this.m_Neigong.m_iExpType);
		if (routineExp == null)
		{
			if (!GameGlobal.m_bDLCMode)
			{
				Debug.Log(string.Concat(new object[]
				{
					this.m_Neigong.m_iNeigongID,
					" ExpType ",
					this.m_Neigong.m_iExpType,
					"  找不到"
				}));
			}
			return 0;
		}
		this.m_iAccumulationExp += iValue;
		this.m_iAccumulationExp = Mathf.Clamp(this.m_iAccumulationExp, 0, routineExp.m_iLV10Exp);
		int iAccumulationExp = this.m_iAccumulationExp;
		int num;
		if (iAccumulationExp < routineExp.m_iLV2Exp)
		{
			num = 1;
			this.iCurExp = iAccumulationExp;
			this.iNextExp = routineExp.m_iLV2Exp;
		}
		else if (iAccumulationExp >= routineExp.m_iLV2Exp && iAccumulationExp < routineExp.m_iLV3Exp)
		{
			num = 2;
			this.iCurExp = iAccumulationExp - routineExp.m_iLV2Exp;
			this.iNextExp = routineExp.m_iLV3Exp - routineExp.m_iLV2Exp;
		}
		else if (iAccumulationExp >= routineExp.m_iLV3Exp && iAccumulationExp < routineExp.m_iLV4Exp)
		{
			num = 3;
			this.iCurExp = iAccumulationExp - routineExp.m_iLV3Exp;
			this.iNextExp = routineExp.m_iLV4Exp - routineExp.m_iLV3Exp;
		}
		else if (iAccumulationExp >= routineExp.m_iLV4Exp && iAccumulationExp < routineExp.m_iLV5Exp)
		{
			num = 4;
			this.iCurExp = iAccumulationExp - routineExp.m_iLV4Exp;
			this.iNextExp = routineExp.m_iLV5Exp - routineExp.m_iLV4Exp;
		}
		else if (iAccumulationExp >= routineExp.m_iLV5Exp && iAccumulationExp < routineExp.m_iLV6Exp)
		{
			num = 5;
			this.iCurExp = iAccumulationExp - routineExp.m_iLV5Exp;
			this.iNextExp = routineExp.m_iLV6Exp - routineExp.m_iLV5Exp;
		}
		else if (iAccumulationExp >= routineExp.m_iLV6Exp && iAccumulationExp < routineExp.m_iLV7Exp)
		{
			num = 6;
			this.iCurExp = iAccumulationExp - routineExp.m_iLV6Exp;
			this.iNextExp = routineExp.m_iLV7Exp - routineExp.m_iLV6Exp;
		}
		else if (iAccumulationExp >= routineExp.m_iLV7Exp && iAccumulationExp < routineExp.m_iLV8Exp)
		{
			num = 7;
			this.iCurExp = iAccumulationExp - routineExp.m_iLV7Exp;
			this.iNextExp = routineExp.m_iLV8Exp - routineExp.m_iLV7Exp;
		}
		else if (iAccumulationExp >= routineExp.m_iLV8Exp && iAccumulationExp < routineExp.m_iLV9Exp)
		{
			num = 8;
			this.iCurExp = iAccumulationExp - routineExp.m_iLV8Exp;
			this.iNextExp = routineExp.m_iLV9Exp - routineExp.m_iLV8Exp;
		}
		else if (iAccumulationExp >= routineExp.m_iLV9Exp && iAccumulationExp < routineExp.m_iLV10Exp)
		{
			num = 9;
			this.iCurExp = iAccumulationExp - routineExp.m_iLV9Exp;
			this.iNextExp = routineExp.m_iLV10Exp - routineExp.m_iLV9Exp;
		}
		else if (iAccumulationExp >= routineExp.m_iLV10Exp)
		{
			this.iCurExp = 0;
			this.iNextExp = 0;
			num = 10;
		}
		else
		{
			num = 1;
			this.iCurExp = iAccumulationExp;
			this.iNextExp = routineExp.m_iLV2Exp;
		}
		int result = num - this.iLevel;
		this.iLevel = num;
		return result;
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x000549DC File Offset: 0x00052BDC
	public int SetLv(int iLv)
	{
		RoutineExpNode routineExp = RoutineExpManager.Singleton.GetRoutineExp(this.m_Neigong.m_iExpType);
		if (routineExp == null)
		{
			Debug.Log(string.Concat(new object[]
			{
				this.m_Neigong.m_iNeigongID,
				" ExpType ",
				this.m_Neigong.m_iExpType,
				" 錯誤  找不到 "
			}));
			return 0;
		}
		this.iCurExp = 0;
		switch (iLv)
		{
		default:
			this.m_iAccumulationExp = 0;
			this.iNextExp = routineExp.m_iLV2Exp;
			break;
		case 2:
			this.m_iAccumulationExp = routineExp.m_iLV2Exp;
			this.iNextExp = routineExp.m_iLV3Exp - routineExp.m_iLV2Exp;
			break;
		case 3:
			this.m_iAccumulationExp = routineExp.m_iLV3Exp;
			this.iNextExp = routineExp.m_iLV4Exp - routineExp.m_iLV3Exp;
			break;
		case 4:
			this.m_iAccumulationExp = routineExp.m_iLV4Exp;
			this.iNextExp = routineExp.m_iLV5Exp - routineExp.m_iLV4Exp;
			break;
		case 5:
			this.m_iAccumulationExp = routineExp.m_iLV5Exp;
			this.iNextExp = routineExp.m_iLV6Exp - routineExp.m_iLV5Exp;
			break;
		case 6:
			this.m_iAccumulationExp = routineExp.m_iLV6Exp;
			this.iNextExp = routineExp.m_iLV7Exp - routineExp.m_iLV6Exp;
			break;
		case 7:
			this.m_iAccumulationExp = routineExp.m_iLV7Exp;
			this.iNextExp = routineExp.m_iLV8Exp - routineExp.m_iLV7Exp;
			break;
		case 8:
			this.m_iAccumulationExp = routineExp.m_iLV8Exp;
			this.iNextExp = routineExp.m_iLV9Exp - routineExp.m_iLV8Exp;
			break;
		case 9:
			this.m_iAccumulationExp = routineExp.m_iLV9Exp;
			this.iNextExp = routineExp.m_iLV10Exp - routineExp.m_iLV9Exp;
			break;
		case 10:
			this.m_iAccumulationExp = routineExp.m_iLV10Exp;
			this.iNextExp = 0;
			break;
		}
		int result = iLv - this.iLevel;
		this.iLevel = iLv;
		return result;
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x00054BF0 File Offset: 0x00052DF0
	public string GetLvUpString(int lv)
	{
		string text = string.Empty;
		if (lv <= 0)
		{
			return text;
		}
		int num = lv;
		if (this.iLevel == 10)
		{
			num--;
		}
		int num2 = 0;
		for (int i = 0; i < this.m_Neigong.m_LevelUP.Count; i++)
		{
			int num3 = num * this.m_Neigong.m_LevelUP[i].m_iValue;
			if (this.iLevel == 10)
			{
				for (int j = 0; j < this.m_Neigong.m_MaxLevelUP.Count; j++)
				{
					if (this.m_Neigong.m_LevelUP[i].m_Type == this.m_Neigong.m_MaxLevelUP[j].m_Type)
					{
						num3 += this.m_Neigong.m_MaxLevelUP[j].m_iValue;
					}
				}
			}
			if (num3 > 0)
			{
				string text2 = Game.StringTable.GetString((int)(110100 + this.m_Neigong.m_LevelUP[i].m_Type)) + " [40FF40]+" + num3.ToString() + "[-]";
				text += text2;
				if (num2 % 2 == 0)
				{
					text += " , ";
				}
				else
				{
					text += "\n";
				}
				num2++;
			}
		}
		if (this.iLevel == 10)
		{
			for (int k = 0; k < this.m_Neigong.m_MaxLevelUP.Count; k++)
			{
				bool flag = false;
				for (int l = 0; l < this.m_Neigong.m_LevelUP.Count; l++)
				{
					if (this.m_Neigong.m_LevelUP[l].m_Type == this.m_Neigong.m_MaxLevelUP[k].m_Type)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					int num4 = lv * this.m_Neigong.m_MaxLevelUP[k].m_iValue;
					string text3 = Game.StringTable.GetString((int)(110100 + this.m_Neigong.m_MaxLevelUP[k].m_Type)) + " [40FF40]+" + num4.ToString() + "[-]";
					text += text3;
					if (num2 % 2 == 0)
					{
						text += " , ";
					}
					else
					{
						text += "\n";
					}
					num2++;
				}
			}
		}
		return text;
	}

	// Token: 0x04000A4F RID: 2639
	[JsonIgnore]
	public NeigongNewDataNode m_Neigong;
}
