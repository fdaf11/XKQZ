using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200035B RID: 859
	public class WgSkillInfoForm : Widget
	{
		// Token: 0x06001393 RID: 5011 RVA: 0x000A8CF0 File Offset: 0x000A6EF0
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgSkillInfoForm.<>f__switch$map45 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(21);
					dictionary.Add("SkillInfoTitle", 0);
					dictionary.Add("SkillInfoExplain", 1);
					dictionary.Add("MartialEffect", 2);
					dictionary.Add("LblMartialEffectName", 3);
					dictionary.Add("LblMartialEffect", 4);
					dictionary.Add("IconMartialEffectList", 5);
					dictionary.Add("LblDamage", 6);
					dictionary.Add("LblRange", 7);
					dictionary.Add("LblRequestSP", 8);
					dictionary.Add("LblCondition", 9);
					dictionary.Add("LblNeigonLink", 10);
					dictionary.Add("LblCD", 11);
					dictionary.Add("LblAdditon", 12);
					dictionary.Add("SkillInfoForm", 13);
					dictionary.Add("GridMartiaData", 14);
					dictionary.Add("GridMartialEffect", 15);
					dictionary.Add("GridNeigongEffect", 16);
					dictionary.Add("LblNeigongEffectName", 17);
					dictionary.Add("LblNeigongEffect", 18);
					dictionary.Add("TalentImage", 19);
					dictionary.Add("TalentInfo", 20);
					WgSkillInfoForm.<>f__switch$map45 = dictionary;
				}
				int num;
				if (WgSkillInfoForm.<>f__switch$map45.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_SkillInfoTitle = sender;
						break;
					case 1:
						this.m_SkillInfoExplain = sender;
						break;
					case 2:
					{
						Control control = sender;
						this.m_MartialEffectList.Add(control);
						break;
					}
					case 3:
					{
						Control control = sender;
						this.m_LblMartialEffectNameList.Add(control);
						break;
					}
					case 4:
					{
						Control control = sender;
						this.m_LblMartailEffectList.Add(control);
						break;
					}
					case 5:
					{
						Control control = sender;
						this.m_IconMartialEffectList.Add(control);
						break;
					}
					case 6:
						this.m_LblDamage = sender;
						break;
					case 7:
						this.m_LblRange = sender;
						break;
					case 8:
						this.m_LblRequestSP = sender;
						break;
					case 9:
						this.m_LblCondition = sender;
						break;
					case 10:
						this.m_LblNeigonLink = sender;
						break;
					case 11:
						this.m_LblCD = sender;
						break;
					case 12:
						this.m_LblAdditon = sender;
						break;
					case 13:
						this.m_SkillInfoForm = sender;
						break;
					case 14:
						this.m_GridMartiaData = sender;
						break;
					case 15:
						this.m_GridMartialEffect = sender;
						break;
					case 16:
						this.m_GridNeigongEffect = sender;
						break;
					case 17:
					{
						Control control = sender;
						this.m_LblNeigongEffectNameList.Add(control);
						break;
					}
					case 18:
					{
						Control control = sender;
						this.m_LblNeigongEffectList.Add(control);
						break;
					}
					case 19:
						this.m_TalentImage = sender;
						break;
					case 20:
						this.m_TalentInfo = sender;
						break;
					}
				}
			}
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x000A902C File Offset: 0x000A722C
		private void Reset()
		{
			this.m_GridMartiaData.GameObject.SetActive(false);
			this.m_GridMartialEffect.GameObject.SetActive(false);
			this.m_GridNeigongEffect.GameObject.SetActive(false);
			this.m_TalentImage.GameObject.SetActive(false);
			this.m_TalentInfo.GameObject.SetActive(false);
			this.m_SkillInfoExplain.GameObject.SetActive(false);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x000A90A0 File Offset: 0x000A72A0
		public void UpdateSkillTowFormDLC(int skillTreeID)
		{
			this.Reset();
			this.Obj.SetActive(true);
			LevelUpPassiveNode levelUpPassiveNode = Game.LevelUpPassiveData.GetLevelUpPassiveNode(skillTreeID);
			this.m_SkillInfoExplain.GameObject.SetActive(false);
			for (int i = 0; i < levelUpPassiveNode.m_PassiveNodeList.Count; i++)
			{
				PassiveNode passiveNode = levelUpPassiveNode.m_PassiveNodeList[i];
				switch (passiveNode.pNodeType)
				{
				case PassiveNodeType.Property:
					this.m_SkillInfoTitle.Text = levelUpPassiveNode.strName;
					this.m_SkillInfoExplain.Text = levelUpPassiveNode.strDesc;
					this.m_SkillInfoExplain.GameObject.SetActive(true);
					break;
				case PassiveNodeType.Routine:
					this.UpdateMartialTwoFormData(new NpcRoutine
					{
						m_Routine = Game.RoutineNewData.GetRoutineNewData(passiveNode.iValue1)
					}, 1000);
					break;
				case PassiveNodeType.NeigongCondition:
				case PassiveNodeType.Talnet:
				case PassiveNodeType.ResistPoint:
					this.m_SkillInfoTitle.Text = levelUpPassiveNode.strName;
					this.m_SkillInfoExplain.Text = levelUpPassiveNode.strDesc;
					this.m_SkillInfoExplain.GameObject.SetActive(true);
					break;
				}
			}
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x000A91E0 File Offset: 0x000A73E0
		public void UpdateSkillTowForm(int talentID)
		{
			this.Reset();
			this.m_TalentImage.GameObject.SetActive(true);
			this.m_TalentInfo.GameObject.SetActive(true);
			this.Obj.SetActive(true);
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(talentID);
			if (talentData == null)
			{
				return;
			}
			this.m_SkillInfoTitle.Text = talentData.m_strTalentName;
			this.m_TalentInfo.Text = talentData.m_strTalentTip;
			this.m_TalentImage.Texture = (Game.g_TalentImageBundle.Load("2dtexture/gameui/talentimage/" + talentData.m_strTalentImage) as Texture);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x000A9280 File Offset: 0x000A7480
		public void UpdateSkillTowForm(object param, int maxSp)
		{
			NpcNeigong npcNeigong = param as NpcNeigong;
			NpcRoutine npcRoutine = param as NpcRoutine;
			this.Obj.SetActive(true);
			if (npcNeigong != null)
			{
				this.UpdateNeigongTwoFormData(npcNeigong);
			}
			else if (npcRoutine != null)
			{
				this.UpdateMartialTwoFormData(npcRoutine, maxSp);
			}
			else
			{
				this.Obj.SetActive(false);
			}
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x000A92D8 File Offset: 0x000A74D8
		public void UpdateNeigongTwoFormData(NpcNeigong data)
		{
			this.Reset();
			this.m_GridNeigongEffect.GameObject.SetActive(true);
			this.m_SkillInfoExplain.GameObject.SetActive(true);
			for (int i = 0; i < this.m_LblNeigongEffectNameList.Count; i++)
			{
				this.m_LblNeigongEffectNameList[i].GameObject.SetActive(false);
				this.m_LblNeigongEffectList[i].GameObject.SetActive(false);
			}
			this.SetNeigongData(data);
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x000A9360 File Offset: 0x000A7560
		public void SetNeigongData(NpcNeigong data)
		{
			this.m_SkillInfoTitle.Text = data.m_Neigong.m_strNeigongName;
			this.m_SkillInfoExplain.Text = data.m_Neigong.m_strNeigongTip;
			for (int i = 0; i < this.m_LblNeigongEffectNameList.Count; i++)
			{
				if (i < data.m_Neigong.m_iConditionList.Count)
				{
					int id = data.m_Neigong.m_iConditionList[i];
					ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(id);
					if (conditionNode == null)
					{
						this.m_LblNeigongEffectNameList[i].GameObject.SetActive(false);
						this.m_LblNeigongEffectList[i].GameObject.SetActive(false);
						Debug.LogError("沒有這個狀態ID" + id.ToString());
					}
					else
					{
						this.m_LblNeigongEffectNameList[i].GameObject.SetActive(true);
						this.m_LblNeigongEffectList[i].GameObject.SetActive(true);
						this.m_LblNeigongEffectNameList[i].Text = conditionNode.m_strName;
						this.m_LblNeigongEffectList[i].Text = conditionNode.m_strDesp;
					}
				}
			}
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x000A949C File Offset: 0x000A769C
		public void UpdateMartialTwoFormData(NpcRoutine data, int maxSp)
		{
			this.Obj.SetActive(true);
			this.Reset();
			this.m_GridMartiaData.GameObject.SetActive(true);
			this.m_GridMartialEffect.GameObject.SetActive(true);
			this.m_SkillInfoExplain.GameObject.SetActive(true);
			for (int i = 0; i < this.m_LblMartialEffectNameList.Count; i++)
			{
				this.m_MartialEffectList[i].GameObject.SetActive(false);
			}
			this.SetMartialData(data, maxSp);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x000A952C File Offset: 0x000A772C
		public void SetMartialData(NpcRoutine data, int maxSp)
		{
			this.m_SkillInfoTitle.Text = data.m_Routine.m_strRoutineName;
			this.m_SkillInfoExplain.Text = data.m_Routine.m_strRoutineTip;
			for (int i = 0; i < this.m_LblMartialEffectNameList.Count; i++)
			{
				if (i < data.m_Routine.m_iConditionIDList.Count)
				{
					this.m_MartialEffectList[i].GameObject.SetActive(true);
					if (data.m_Routine.m_iConditionIDList[i] != 0)
					{
						int id = data.m_Routine.m_iConditionIDList[i];
						ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(id);
						if (conditionNode == null)
						{
							Debug.LogError("沒有這個狀態ID" + id.ToString());
							this.m_LblMartialEffectNameList[i].Text = string.Empty;
							this.m_LblMartailEffectList[i].Text = string.Empty;
						}
						else
						{
							this.m_LblMartialEffectNameList[i].Text = conditionNode.m_strName;
							this.m_LblMartailEffectList[i].Text = conditionNode.m_strDesp;
						}
					}
				}
			}
			float num = (float)data.m_Routine.m_iDamage;
			num *= 0.1f;
			float num2 = (float)data.m_Routine.m_iRequestSP;
			num2 *= 0.01f;
			float num3 = (float)maxSp;
			float num4 = (float)maxSp * num2;
			int iLevel = data.iLevel;
			int num5 = (int)num4;
			int num6 = (int)(num4 * num * (1f + (float)iLevel * 0.1f));
			this.m_LblDamage.Text = num6.ToString();
			this.m_LblRequestSP.Text = num5.ToString();
			this.m_LblRange.Text = data.m_Routine.GetRoutineAbilityText();
			if (data.m_Routine._NeigonLinkID != 0)
			{
				this.m_LblNeigonLink.GameObject.SetActive(true);
				Control lblNeigonLink = this.m_LblNeigonLink;
				lblNeigonLink.Text += Game.NeigongData.GetNeigongName(data.m_Routine._NeigonLinkID);
			}
			else
			{
				this.m_LblNeigonLink.GameObject.SetActive(false);
			}
			this.m_LblCD.Text = data.m_Routine.m_iCD.ToString();
			this.m_LblAdditon.GameObject.SetActive(false);
		}

		// Token: 0x040017A4 RID: 6052
		private Control m_SkillInfoForm;

		// Token: 0x040017A5 RID: 6053
		private Control m_SkillInfoTitle;

		// Token: 0x040017A6 RID: 6054
		private Control m_SkillInfoExplain;

		// Token: 0x040017A7 RID: 6055
		private Control m_GridMartiaData;

		// Token: 0x040017A8 RID: 6056
		private Control m_GridMartialEffect;

		// Token: 0x040017A9 RID: 6057
		private List<Control> m_MartialEffectList = new List<Control>();

		// Token: 0x040017AA RID: 6058
		private List<Control> m_LblMartialEffectNameList = new List<Control>();

		// Token: 0x040017AB RID: 6059
		private List<Control> m_LblMartailEffectList = new List<Control>();

		// Token: 0x040017AC RID: 6060
		private List<Control> m_IconMartialEffectList = new List<Control>();

		// Token: 0x040017AD RID: 6061
		private Control m_LblDamage;

		// Token: 0x040017AE RID: 6062
		private Control m_LblRange;

		// Token: 0x040017AF RID: 6063
		private Control m_LblRequestSP;

		// Token: 0x040017B0 RID: 6064
		private Control m_LblCondition;

		// Token: 0x040017B1 RID: 6065
		private Control m_LblNeigonLink;

		// Token: 0x040017B2 RID: 6066
		private Control m_LblCD;

		// Token: 0x040017B3 RID: 6067
		private Control m_LblAdditon;

		// Token: 0x040017B4 RID: 6068
		private Control m_GridNeigongEffect;

		// Token: 0x040017B5 RID: 6069
		private List<Control> m_LblNeigongEffectNameList = new List<Control>();

		// Token: 0x040017B6 RID: 6070
		private List<Control> m_LblNeigongEffectList = new List<Control>();

		// Token: 0x040017B7 RID: 6071
		private Control m_TalentImage;

		// Token: 0x040017B8 RID: 6072
		private Control m_TalentInfo;
	}
}
