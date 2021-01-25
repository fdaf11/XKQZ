using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000352 RID: 850
	public class WgPracticeNode : Widget
	{
		// Token: 0x06001372 RID: 4978 RVA: 0x000A8224 File Offset: 0x000A6424
		public override void InitWidget(UILayer layer)
		{
			base.InitWidget(layer);
			UICharacter uicharacter = layer as UICharacter;
			this.m_CtrlCharacter = uicharacter.m_CtrlCharacter;
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x000A824C File Offset: 0x000A644C
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgPracticeNode.<>f__switch$map41 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("OnHover", 0);
					dictionary.Add("PracticeNode", 1);
					dictionary.Add("IconPractice", 2);
					dictionary.Add("LblPractice", 3);
					dictionary.Add("LblPracticeEffect", 4);
					dictionary.Add("LblPracticeLevelAmount", 5);
					WgPracticeNode.<>f__switch$map41 = dictionary;
				}
				int num;
				if (WgPracticeNode.<>f__switch$map41.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_OnHover = control;
						break;
					case 1:
						control.OnClick += this.PracticeNodeOnClick;
						control.OnHover += this.PracticeNodeOnHover;
						control.OnKeySelect += this.PracticeNodeOnKeySelect;
						this.m_PracticeNode = control;
						this.ParentLayer.SetInputButton(3, control.Listener);
						break;
					case 2:
						this.m_IconPractice = control;
						break;
					case 3:
						this.m_LblPractice = control;
						break;
					case 4:
						this.m_LblPracticeEffect = control;
						break;
					case 5:
						this.m_LblPracticeLevelAmount = control;
						break;
					}
				}
			}
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x000A8394 File Offset: 0x000A6594
		public void SetWgPracticeNode(NpcRoutine data)
		{
			this.m_LblPractice.Text = data.m_Routine.m_strRoutineName;
			string strSkillIconName = data.m_Routine.m_strSkillIconName;
			this.m_LblPracticeEffect.Text = data.m_Routine.m_strUpgradeNotes.Replace("<br>", "\n");
			this.m_LblPracticeLevelAmount.Text = data.iLevel.ToString();
			this.Obj.GetComponent<PracticeDataNode>().SkillData = data;
			this.SetTexture(strSkillIconName, this.m_IconPractice);
			this.m_OnHover.GameObject.SetActive(false);
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x000A8430 File Offset: 0x000A6630
		public void SetWgPracticeNode(NpcNeigong data)
		{
			this.m_LblPractice.Text = data.m_Neigong.m_strNeigongName;
			string sIconImage = data.m_Neigong.sIconImage;
			this.m_LblPracticeEffect.Text = data.m_Neigong.m_strUpgradeNotes.Replace("<br>", "\n");
			this.m_LblPracticeLevelAmount.Text = data.iLevel.ToString();
			this.Obj.GetComponent<PracticeDataNode>().SkillData = data;
			this.SetTexture(sIconImage, this.m_IconPractice);
			this.m_OnHover.GameObject.SetActive(false);
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x000A5CAC File Offset: 0x000A3EAC
		private void SetTexture(string name, Control Icon)
		{
			Texture texture = Game.g_Item.Load("2dtexture/gameui/item/" + name) as Texture;
			if (texture != null)
			{
				Icon.Texture = texture;
			}
			else
			{
				Icon.Texture = null;
				GameDebugTool.Log("無此裝備貼圖 : " + name);
			}
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x000A84CC File Offset: 0x000A66CC
		private void PracticeNodeOnKeySelect(GameObject go, bool isSelect)
		{
			this.m_OnHover.GameObject.SetActive(isSelect);
			if (isSelect)
			{
				this.ParentLayer.ShowKeySelect(go, new Vector3(-350f, 0f, 0f), KeySelect.eSelectDir.Left, 32, 32);
				if (this.SetScrollBar != null)
				{
					this.SetScrollBar.Invoke(go);
				}
			}
			else
			{
				this.ParentLayer.HideKeySelect();
			}
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x000A853C File Offset: 0x000A673C
		private void PracticeNodeOnHover(GameObject go, bool bHover)
		{
			this.m_OnHover.GameObject.SetActive(bHover);
			if (bHover && GameCursor.IsShow)
			{
				Debug.Log("IsShow : " + GameCursor.IsShow);
				this.m_CtrlCharacter.SetCurrent(go);
			}
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x000A8590 File Offset: 0x000A6790
		private void PracticeNodeOnClick(GameObject go)
		{
			PracticeDataNode component = go.GetComponent<PracticeDataNode>();
			if (component.SkillData == null)
			{
				return;
			}
			this.m_CtrlCharacter.SetPractice(component.SkillData);
		}

		// Token: 0x04001781 RID: 6017
		private CtrlCharacter m_CtrlCharacter;

		// Token: 0x04001782 RID: 6018
		public new Action<GameObject> SetScrollBar;

		// Token: 0x04001783 RID: 6019
		private Control m_OnHover;

		// Token: 0x04001784 RID: 6020
		private Control m_PracticeNode;

		// Token: 0x04001785 RID: 6021
		private Control m_IconPractice;

		// Token: 0x04001786 RID: 6022
		private Control m_LblPractice;

		// Token: 0x04001787 RID: 6023
		private Control m_LblPracticeEffect;

		// Token: 0x04001788 RID: 6024
		private Control m_LblPracticeLevelAmount;
	}
}
