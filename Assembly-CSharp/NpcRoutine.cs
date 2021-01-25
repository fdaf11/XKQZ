using System;
using Heluo.Wulin;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x020001F6 RID: 502
public class NpcRoutine : NpcSkill
{
	// Token: 0x060009FC RID: 2556 RVA: 0x0000802C File Offset: 0x0000622C
	public NpcRoutine()
	{
		this.m_SkillType = NpcSkill.SkillType.Routine;
		this.m_Routine = new RoutineNewDataNode();
		this.iLevel = 1;
		this.iCurExp = 0;
		this.iNextExp = 1;
		this.m_iAccumulationExp = 0;
		this.bUse = false;
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x00053F68 File Offset: 0x00052168
	public NpcRoutine Clone()
	{
		return new NpcRoutine
		{
			iSkillID = this.iSkillID,
			iLevel = this.iLevel,
			iCurExp = this.iCurExp,
			iNextExp = this.iNextExp,
			m_iAccumulationExp = this.m_iAccumulationExp,
			bUse = this.bUse
		};
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x00053FC4 File Offset: 0x000521C4
	public bool SetRoutine()
	{
		RoutineNewDataNode routineNewData = Game.RoutineNewData.GetRoutineNewData(this.iSkillID);
		if (routineNewData == null)
		{
			return false;
		}
		this.m_Routine = routineNewData;
		return true;
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x00053FF4 File Offset: 0x000521F4
	public int SetExp(int iValue)
	{
		if (GameGlobal.m_bDLCMode)
		{
			return 0;
		}
		RoutineExpNode routineExp = RoutineExpManager.Singleton.GetRoutineExp(this.m_Routine.m_iExpType);
		if (routineExp == null)
		{
			Debug.LogError(this.m_Routine.m_iRoutineID + "   " + this.m_Routine.m_iExpType);
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
			num = 10;
			this.iCurExp = 0;
			this.iNextExp = 0;
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

	// Token: 0x06000A00 RID: 2560 RVA: 0x000542F8 File Offset: 0x000524F8
	public int SetLv(int iLv)
	{
		if (GameGlobal.m_bDLCMode)
		{
			this.iLevel = 10;
			return 0;
		}
		RoutineExpNode routineExp = RoutineExpManager.Singleton.GetRoutineExp(this.m_Routine.m_iExpType);
		if (routineExp == null)
		{
			Debug.LogError(this.m_Routine.m_iRoutineID + "   " + this.m_Routine.m_iExpType);
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

	// Token: 0x06000A01 RID: 2561 RVA: 0x00054508 File Offset: 0x00052708
	public string GetLvUpString(int lv)
	{
		string text = string.Empty;
		for (int i = 0; i < this.m_Routine.m_iLevelUP.Count; i++)
		{
			int num = lv * this.m_Routine.m_iLevelUP[i].m_iValue;
			string text2 = Game.StringTable.GetString((int)(110100 + this.m_Routine.m_iLevelUP[i].m_Type)) + " [40FF40]+" + num.ToString() + "[-]";
			text += text2;
			if (i % 2 == 0)
			{
				text += " , ";
			}
			else
			{
				text += "\n";
			}
		}
		return text;
	}

	// Token: 0x04000A4E RID: 2638
	[JsonIgnore]
	public RoutineNewDataNode m_Routine;
}
