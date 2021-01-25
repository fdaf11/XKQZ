using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class ConditionNode
{
	// Token: 0x0600050A RID: 1290 RVA: 0x0003AF8C File Offset: 0x0003918C
	public ConditionNode Clone()
	{
		ConditionNode conditionNode = new ConditionNode();
		conditionNode.m_iConditionID = this.m_iConditionID;
		conditionNode.m_strName = this.m_strName;
		conditionNode.m_strDesp = this.m_strDesp;
		conditionNode.m_strIconName = this.m_strIconName;
		conditionNode.m_iCondType = this.m_iCondType;
		conditionNode.m_iCondTarget = this.m_iCondTarget;
		conditionNode.m_iMinTurn = this.m_iMinTurn;
		conditionNode.m_iMaxTurn = this.m_iMaxTurn;
		conditionNode.m_iRemoveByAttack = this.m_iRemoveByAttack;
		conditionNode.m_iRemoveOnHit = this.m_iRemoveOnHit;
		conditionNode.m_iTargetUnitID = this.m_iTargetUnitID;
		for (int i = 0; i < this.m_effectPartList.Count; i++)
		{
			EffectPart effectPart = this.m_effectPartList[i];
			if (effectPart != null)
			{
				conditionNode.m_effectPartList.Add(effectPart.Clone());
			}
		}
		return conditionNode;
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x0003B06C File Offset: 0x0003926C
	public void InitEffectPart()
	{
		for (int i = 0; i < this.m_effectPartList.Count; i++)
		{
			EffectPart effectPart = this.m_effectPartList[i];
			if (effectPart != null)
			{
				if (effectPart.m_effectAccumulateType == _EffectAccumulateType.Minus)
				{
					effectPart.m_iValueSum = effectPart.m_iValueLimit;
				}
				else if (effectPart.m_effectAccumulateType == _EffectAccumulateType.Multiply)
				{
					effectPart.m_iValueSum = effectPart.m_iValue1;
				}
				else if (effectPart.m_effectAccumulateType == _EffectAccumulateType.None)
				{
					effectPart.m_iValueSum = Random.Range(effectPart.m_iValue1, effectPart.m_iValue2);
				}
				else
				{
					effectPart.m_iValueSum = 0;
				}
			}
		}
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0003B118 File Offset: 0x00039318
	public bool GetEffectPartAbsoluteBuff(_EffectPartType typePart)
	{
		for (int i = 0; i < this.m_effectPartList.Count; i++)
		{
			EffectPart effectPart = this.m_effectPartList[i];
			if (effectPart != null)
			{
				if (typePart == effectPart.m_effectPartType && effectPart.m_effectAccumulateType == _EffectAccumulateType.Absolute && (this.m_iCondType == _ConditionType.Buff || this.m_iCondType == _ConditionType.InstantBuff))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x0003B18C File Offset: 0x0003938C
	public bool GetEffectPartAbsoluteDebuff(_EffectPartType typePart)
	{
		for (int i = 0; i < this.m_effectPartList.Count; i++)
		{
			EffectPart effectPart = this.m_effectPartList[i];
			if (effectPart != null)
			{
				if (typePart == effectPart.m_effectPartType && effectPart.m_effectAccumulateType == _EffectAccumulateType.Absolute && (this.m_iCondType == _ConditionType.Debuff || this.m_iCondType == _ConditionType.InstantDebuff))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04000511 RID: 1297
	public int m_iConditionID;

	// Token: 0x04000512 RID: 1298
	public string m_strName;

	// Token: 0x04000513 RID: 1299
	public string m_strDesp;

	// Token: 0x04000514 RID: 1300
	public string m_strIconName;

	// Token: 0x04000515 RID: 1301
	public _ConditionType m_iCondType;

	// Token: 0x04000516 RID: 1302
	public int m_iCondTarget;

	// Token: 0x04000517 RID: 1303
	public int m_iMinTurn;

	// Token: 0x04000518 RID: 1304
	public int m_iMaxTurn;

	// Token: 0x04000519 RID: 1305
	public int m_iRemoveByAttack;

	// Token: 0x0400051A RID: 1306
	public int m_iRemoveOnHit;

	// Token: 0x0400051B RID: 1307
	public int m_iCountTillNextTurn;

	// Token: 0x0400051C RID: 1308
	public int m_iTargetUnitID;

	// Token: 0x0400051D RID: 1309
	public List<EffectPart> m_effectPartList = new List<EffectPart>();
}
