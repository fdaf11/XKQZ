using System;
using System.Collections.Generic;

// Token: 0x02000130 RID: 304
public class Condition
{
	// Token: 0x06000624 RID: 1572 RVA: 0x0000595A File Offset: 0x00003B5A
	public Condition()
	{
		this._iType = ConditionKind.None;
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x00043ED8 File Offset: 0x000420D8
	public Condition(string[] args)
	{
		this._iType = (ConditionKind)int.Parse(args[0]);
		ConditionKind iType = this._iType;
		switch (iType)
		{
		case ConditionKind.Item:
		case ConditionKind.TeamMoney:
		case ConditionKind.PartyMember:
		case ConditionKind.RoundRange:
		case ConditionKind.MoreNpcRoutine:
		case ConditionKind.MoreNpcNeigong:
		case ConditionKind.MoreNpcProperty:
		case ConditionKind.LessNpcProperty:
		case ConditionKind.Friendliness:
		case ConditionKind.TeammateFriendliness:
		case ConditionKind.NpcRoutineAbility:
		case ConditionKind.TimeCheck:
		case ConditionKind.TeamMemberAmount:
		case ConditionKind.LessNpcRoutine:
		case ConditionKind.LessNpcNeigong:
		case ConditionKind.OnlyCheckItem:
		case ConditionKind.CheckSkillMax:
		case ConditionKind.DLC_Level:
		case ConditionKind.DLC_Fame:
		case ConditionKind.DLC_Gold:
		case ConditionKind.DLC_PassiveNode:
		case ConditionKind.DLC_CharLevel:
		case ConditionKind.DLC_UnitLevel:
			break;
		default:
			switch (iType)
			{
			case ConditionKind.NpcMoney:
			case ConditionKind.MoreNpcItem:
			case ConditionKind.LessNpcItem:
				goto IL_B5;
			}
			this.m_Parameter = args[1];
			goto IL_E0;
		}
		IL_B5:
		this.m_Parameter = int.Parse(args[1]);
		IL_E0:
		this.m_iAmount = int.Parse(args[2]);
		if (args.Length > 3)
		{
			this.m_iValue = int.Parse(args[3]);
		}
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x00043FEC File Offset: 0x000421EC
	public Condition Clone()
	{
		return new Condition
		{
			_iType = this._iType,
			m_Parameter = this.m_Parameter,
			m_iValue = this.m_iValue,
			m_iAmount = this.m_iAmount
		};
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x00044030 File Offset: 0x00042230
	public static void GenerateList(List<Condition> list, string data)
	{
		string text = data.Replace(")*(", "*");
		if (text.Length > 2)
		{
			text = text.Substring(1, text.Length - 2);
		}
		string[] array = text.Split(new char[]
		{
			"*".get_Chars(0)
		});
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[]
			{
				",".get_Chars(0)
			});
			if (array2.Length > 2)
			{
				Condition condition = new Condition(array2);
				list.Add(condition);
			}
		}
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x000440D0 File Offset: 0x000422D0
	public void SetParaForMapIcon(string[] args)
	{
		this._iType = (ConditionKind)int.Parse(args[0]);
		switch (this._iType)
		{
		case ConditionKind.Item:
		case ConditionKind.TeamMoney:
		case ConditionKind.PartyMember:
		case ConditionKind.RoundRange:
		case ConditionKind.MoreNpcRoutine:
		case ConditionKind.MoreNpcNeigong:
		case ConditionKind.MoreNpcProperty:
		case ConditionKind.LessNpcProperty:
		case ConditionKind.Friendliness:
		case ConditionKind.TeammateFriendliness:
		case ConditionKind.NpcRoutineAbility:
		case ConditionKind.TimeCheck:
		case ConditionKind.TeamMemberAmount:
		case ConditionKind.LessNpcRoutine:
		case ConditionKind.LessNpcNeigong:
		case ConditionKind.OnlyCheckItem:
			this.m_Parameter = int.Parse(args[1]);
			goto IL_A1;
		}
		this.m_Parameter = args[1];
		IL_A1:
		this.m_iAmount = int.Parse(args[2]);
		if (args.Length > 3)
		{
			this.m_iValue = int.Parse(args[3]);
		}
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x000441A4 File Offset: 0x000423A4
	public void SetParID(int iID)
	{
		ConditionKind iType = this._iType;
		switch (iType)
		{
		case ConditionKind.MoreNpcRoutine:
		case ConditionKind.MoreNpcNeigong:
		case ConditionKind.MoreNpcProperty:
		case ConditionKind.LessNpcProperty:
		case ConditionKind.LessNpcRoutine:
		case ConditionKind.LessNpcNeigong:
			break;
		default:
			if (iType != ConditionKind.DLC_CharLevel && iType != ConditionKind.DLC_UnitLevel && iType != ConditionKind.PartyMember)
			{
				return;
			}
			break;
		}
		this.m_Parameter = iID;
	}

	// Token: 0x040006C9 RID: 1737
	public ConditionKind _iType;

	// Token: 0x040006CA RID: 1738
	public CParaValue m_Parameter;

	// Token: 0x040006CB RID: 1739
	public int m_iAmount;

	// Token: 0x040006CC RID: 1740
	public int m_iValue;
}
