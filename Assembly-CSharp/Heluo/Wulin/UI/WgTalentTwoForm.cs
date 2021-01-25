using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200035C RID: 860
	public class WgTalentTwoForm : Widget
	{
		// Token: 0x0600139D RID: 5021 RVA: 0x000A9788 File Offset: 0x000A7988
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgTalentTwoForm.<>f__switch$map46 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(14);
					dictionary.Add("Talent", 0);
					dictionary.Add("TalentNode", 1);
					dictionary.Add("TalentPointText", 2);
					dictionary.Add("TalentSelect", 3);
					dictionary.Add("TalentLine", 4);
					dictionary.Add("IconCharacterExit", 5);
					dictionary.Add("UpgradeTalent", 6);
					dictionary.Add("LblTitle", 7);
					dictionary.Add("LblSkill", 8);
					dictionary.Add("OKLearn", 9);
					dictionary.Add("Cancel", 10);
					dictionary.Add("LblNoLearn", 11);
					dictionary.Add("LblNoPoint", 12);
					dictionary.Add("OKNoLearn", 13);
					WgTalentTwoForm.<>f__switch$map46 = dictionary;
				}
				int num;
				if (WgTalentTwoForm.<>f__switch$map46.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Talent = sender;
						break;
					case 1:
						this.m_TalentNode = control;
						break;
					case 2:
						this.m_TalentPointText = control;
						break;
					case 3:
						this.m_TalentSelect = control;
						this.m_TalentSelect.GameObject.SetActive(false);
						break;
					case 4:
						this.m_TalentLine = control;
						break;
					case 5:
						this.m_IconCharacterExit = control;
						this.m_IconCharacterExit.OnClick += this.OnClose;
						break;
					case 6:
						this.m_UpgradeTalent = control;
						break;
					case 7:
						this.m_LblTitle = control;
						break;
					case 8:
						this.m_LblSkill = control;
						break;
					case 9:
						this.m_OKLearn = control;
						this.m_OKLearn.OnClick += this.OnOKLearnClick;
						break;
					case 10:
						this.m_Cancel = control;
						this.m_Cancel.OnClick += this.OnCancelClick;
						break;
					case 11:
						this.m_LblNoLearn = control;
						break;
					case 12:
						this.m_LblNoPoint = control;
						break;
					case 13:
						this.m_OKNoLearn = control;
						this.m_OKNoLearn.OnClick += this.OnOKNoLearnClick;
						break;
					}
				}
			}
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x000A99D8 File Offset: 0x000A7BD8
		public void SetTalent(UpgradeNode upgrade, CharacterData data)
		{
			this.Obj.SetActive(true);
			this.data = data;
			this.m_TalentPointText.Text = data.iSkillPoint.ToString();
			for (int i = 0; i < upgrade.m_PassiveTreeNodeList.Count; i++)
			{
				PassiveTreeNode passiveTreeNode = upgrade.m_PassiveTreeNodeList[i];
				if (passiveTreeNode != null)
				{
					GameObject node = this.GetNode();
					node.name = passiveTreeNode.iPassiveID.ToString();
					node.transform.parent = this.m_Talent;
					int num = passiveTreeNode.iPos - 1;
					node.transform.localPosition = new Vector3((float)(-475 + num % 20 * 50), (float)(250 - num / 20 * 110), 0f);
					node.transform.localScale = Vector3.one;
					Control control = node.transform;
					control.GameObject.SetActive(true);
					if (data.passiveNodeList.Contains(passiveTreeNode.iPassiveID))
					{
						control.UISprite.color = Color.white;
					}
					else
					{
						control.UISprite.color = Color.gray;
					}
					LevelUpPassiveNode levelUpPassiveNode = Game.LevelUpPassiveData.GetLevelUpPassiveNode(passiveTreeNode.iPassiveID);
					control.SpriteName = levelUpPassiveNode.strIcon;
				}
			}
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0000264F File Offset: 0x0000084F
		private void SetTalentLine(LevelUpPassiveNode node)
		{
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x000A9B28 File Offset: 0x000A7D28
		private GameObject GetLine()
		{
			for (int i = 0; i < this.lineList.Count; i++)
			{
				if (!this.lineList[i].activeSelf)
				{
					return this.lineList[i];
				}
			}
			GameObject gameObject = Object.Instantiate(this.m_TalentLine.GameObject) as GameObject;
			this.lineList.Add(gameObject);
			return gameObject;
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x000A9B98 File Offset: 0x000A7D98
		private GameObject GetNode()
		{
			for (int i = 0; i < this.nodeList.Count; i++)
			{
				if (!this.nodeList[i].activeSelf)
				{
					return this.nodeList[i];
				}
			}
			GameObject gameObject = Object.Instantiate(this.m_TalentNode.GameObject) as GameObject;
			Control control = gameObject.transform;
			control.OnHover += this.OnNodeHover;
			control.OnClick += this.OnNodeClick;
			this.nodeList.Add(gameObject);
			return gameObject;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x000A9C38 File Offset: 0x000A7E38
		public void OnClose(GameObject go)
		{
			if (this.OnCloseClick != null)
			{
				this.OnCloseClick.Invoke(go);
			}
			this.Obj.SetActive(false);
			for (int i = 0; i < this.nodeList.Count; i++)
			{
				this.nodeList[i].SetActive(false);
			}
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x000A9C98 File Offset: 0x000A7E98
		public void OnNodeClick(GameObject obj)
		{
			int num = int.Parse(obj.name);
			if (this.data.passiveNodeList.Contains(num))
			{
				return;
			}
			LevelUpPassiveNode levelUpPassiveNode = Game.LevelUpPassiveData.GetLevelUpPassiveNode(num);
			bool flag = ConditionManager.CheckCondition(levelUpPassiveNode.m_RequestConditionList, true, 0, string.Empty);
			this.ShowUpgradeTalent(num, levelUpPassiveNode);
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x000A9CF0 File Offset: 0x000A7EF0
		public void ShowUpgradeTalent(int iD, LevelUpPassiveNode node)
		{
			this.m_UpgradeTalent.GameObject.SetActive(true);
			this.m_LblTitle.GameObject.SetActive(true);
			this.m_LblSkill.GameObject.SetActive(true);
			this.m_OKLearn.GameObject.SetActive(true);
			this.m_OKLearn.Listener.parameter = iD;
			this.m_LblSkill.Text = node.strName;
			this.m_Cancel.GameObject.SetActive(true);
			this.m_LblNoPoint.GameObject.SetActive(false);
			this.m_LblNoLearn.GameObject.SetActive(false);
			this.m_OKNoLearn.GameObject.SetActive(false);
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x000A9DAC File Offset: 0x000A7FAC
		public void OnOKLearnClick(GameObject obj)
		{
			this.m_UpgradeTalent.GameObject.SetActive(false);
			UIEventListener component = obj.GetComponent<UIEventListener>();
			if (component == null)
			{
				return;
			}
			int num = (int)component.parameter;
			LevelUpPassiveNode levelUpPassiveNode = Game.LevelUpPassiveData.GetLevelUpPassiveNode(num);
			if (this.data.iSkillPoint < levelUpPassiveNode.price)
			{
				string @string = Game.StringTable.GetString(990085);
				Game.UI.Get<UIMapMessage>().SetMsg(@string);
				return;
			}
			if (!ConditionManager.CheckCondition(levelUpPassiveNode.m_RequestConditionList, true, this.data.iNpcID, string.Empty))
			{
				string string2 = Game.StringTable.GetString(990084);
				Game.UI.Get<UIMapMessage>().SetMsg(string2);
				return;
			}
			string string3 = Game.StringTable.GetString(990090);
			Game.UI.Get<UIMapMessage>().SetMsg(string3);
			if (this.OnLearnSkill != null)
			{
				this.OnLearnSkill.Invoke(num);
			}
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0000CAF3 File Offset: 0x0000ACF3
		public void OnCancelClick(GameObject obj)
		{
			this.m_UpgradeTalent.GameObject.SetActive(false);
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x0000CAF3 File Offset: 0x0000ACF3
		public void OnOKNoLearnClick(GameObject obj)
		{
			this.m_UpgradeTalent.GameObject.SetActive(false);
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x000A9EB0 File Offset: 0x000A80B0
		public void OnNodeHover(GameObject obj, bool hover)
		{
			if (hover)
			{
				this.m_TalentSelect.GameObject.SetActive(true);
				this.m_TalentSelect.GameObject.transform.localPosition = obj.transform.localPosition;
			}
			else
			{
				this.m_TalentSelect.GameObject.SetActive(false);
			}
			if (this.NodeOnHover != null)
			{
				this.NodeOnHover.Invoke(obj, hover);
			}
		}

		// Token: 0x040017BA RID: 6074
		private Transform m_Talent;

		// Token: 0x040017BB RID: 6075
		private Control m_TalentNode;

		// Token: 0x040017BC RID: 6076
		private Control m_TalentLine;

		// Token: 0x040017BD RID: 6077
		private Control m_TalentSelect;

		// Token: 0x040017BE RID: 6078
		private Control m_IconCharacterExit;

		// Token: 0x040017BF RID: 6079
		private Control m_TalentPointText;

		// Token: 0x040017C0 RID: 6080
		private Control m_UpgradeTalent;

		// Token: 0x040017C1 RID: 6081
		private Control m_LblTitle;

		// Token: 0x040017C2 RID: 6082
		private Control m_LblSkill;

		// Token: 0x040017C3 RID: 6083
		private Control m_OKLearn;

		// Token: 0x040017C4 RID: 6084
		private Control m_Cancel;

		// Token: 0x040017C5 RID: 6085
		private Control m_LblNoLearn;

		// Token: 0x040017C6 RID: 6086
		private Control m_LblNoPoint;

		// Token: 0x040017C7 RID: 6087
		private Control m_OKNoLearn;

		// Token: 0x040017C8 RID: 6088
		private CharacterData data;

		// Token: 0x040017C9 RID: 6089
		private List<GameObject> nodeList = new List<GameObject>();

		// Token: 0x040017CA RID: 6090
		private List<GameObject> lineList = new List<GameObject>();

		// Token: 0x040017CB RID: 6091
		public Action<int> OnLearnSkill;

		// Token: 0x040017CC RID: 6092
		public Action<GameObject, bool> NodeOnHover;

		// Token: 0x040017CD RID: 6093
		public Action<GameObject> OnCloseClick;
	}
}
