using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000255 RID: 597
	public class RoutineNewDataNode
	{
		// Token: 0x06000AFC RID: 2812 RVA: 0x00008B05 File Offset: 0x00006D05
		public RoutineNewDataNode()
		{
			this.m_iLevelUP = new List<LevelUp>();
			this.m_iConditionIDList = new List<int>();
			this.m_iSkillEffectIDList = new List<int>();
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0005BA2C File Offset: 0x00059C2C
		public string GetRoutineAbilityText()
		{
			string text = string.Empty;
			if (this.m_iTargetType == 2)
			{
				text = Game.StringTable.GetString(263030);
			}
			if (this.m_iTargetType == 1)
			{
				text = Game.StringTable.GetString(263031);
			}
			if (this.m_iTargetType == 0)
			{
				text = Game.StringTable.GetString(263032);
			}
			if (this.m_iTargetType == 4)
			{
				text = Game.StringTable.GetString(263033);
			}
			if (this.m_iTargetType == 3)
			{
				text = Game.StringTable.GetString(263034);
			}
			string text2 = string.Empty;
			if (this.m_iTargetArea == 0)
			{
				if (this.m_bNeedToSelectTarget)
				{
					if (this.m_iAOE == 0)
					{
						text2 = string.Format(Game.StringTable.GetString(263035), this.m_iRange, text);
					}
					else
					{
						text2 = string.Format(Game.StringTable.GetString(263036), this.m_iRange, this.m_iAOE, text);
					}
				}
				else
				{
					text2 = string.Format(Game.StringTable.GetString(263037), this.m_iAOE, text);
				}
			}
			else if (this.m_iTargetArea == 1)
			{
				text2 = string.Format(Game.StringTable.GetString(263038), this.m_iRange, text);
			}
			else if (this.m_iTargetArea == 2)
			{
				text2 = string.Format(Game.StringTable.GetString(263039), this.m_iRange, text);
			}
			else if (this.m_iTargetArea == 3)
			{
				text2 = string.Format(Game.StringTable.GetString(263040), this.m_iRange, text);
			}
			return text2.Replace("<br>", " ");
		}

		// Token: 0x04000C6D RID: 3181
		public int m_iRoutineID;

		// Token: 0x04000C6E RID: 3182
		public string m_strRoutineName;

		// Token: 0x04000C6F RID: 3183
		public string m_strRoutineTip;

		// Token: 0x04000C70 RID: 3184
		public string m_strUpgradeNotes;

		// Token: 0x04000C71 RID: 3185
		public string m_strSkillIconName;

		// Token: 0x04000C72 RID: 3186
		public WeaponType m_RoutineType;

		// Token: 0x04000C73 RID: 3187
		public int m_iAdditon;

		// Token: 0x04000C74 RID: 3188
		public CharacterData.PropertyType m_AdditonType;

		// Token: 0x04000C75 RID: 3189
		public int m_iExpType;

		// Token: 0x04000C76 RID: 3190
		public List<LevelUp> m_iLevelUP;

		// Token: 0x04000C77 RID: 3191
		public int _NeigonLinkID;

		// Token: 0x04000C78 RID: 3192
		public bool m_bNeedToSelectTarget;

		// Token: 0x04000C79 RID: 3193
		public int m_iSkillType;

		// Token: 0x04000C7A RID: 3194
		public int m_iTargetType;

		// Token: 0x04000C7B RID: 3195
		public int m_iTargetArea;

		// Token: 0x04000C7C RID: 3196
		public int m_iRange;

		// Token: 0x04000C7D RID: 3197
		public int m_iAOE;

		// Token: 0x04000C7E RID: 3198
		public int m_iDamage;

		// Token: 0x04000C7F RID: 3199
		public int m_iRequestSP;

		// Token: 0x04000C80 RID: 3200
		public int m_iCD;

		// Token: 0x04000C81 RID: 3201
		public List<int> m_iConditionIDList = new List<int>();

		// Token: 0x04000C82 RID: 3202
		public List<int> m_iSkillEffectIDList = new List<int>();
	}
}
