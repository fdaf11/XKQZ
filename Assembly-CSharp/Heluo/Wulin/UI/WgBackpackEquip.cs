using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000343 RID: 835
	public class WgBackpackEquip : Widget
	{
		// Token: 0x0600130B RID: 4875 RVA: 0x0000C5F4 File Offset: 0x0000A7F4
		public override void InitWidget(UILayer layer)
		{
			base.InitWidget(layer);
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x000A5764 File Offset: 0x000A3964
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgBackpackEquip.<>f__switch$map32 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("BackpackEquip", 0);
					dictionary.Add("IconBackpackEquip", 1);
					dictionary.Add("LblEquip", 2);
					dictionary.Add("LblEquipEffect", 3);
					dictionary.Add("LblEquipAddEffect", 4);
					dictionary.Add("OnHover", 5);
					WgBackpackEquip.<>f__switch$map32 = dictionary;
				}
				int num;
				if (WgBackpackEquip.<>f__switch$map32.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						control.OnClick += this.BackpackEqupipOnClick;
						control.OnHover += this.BackpackEqupipOnHover;
						control.OnKeySelect += this.BackpackEqupipOnKeySelect;
						this.m_BackpackEquip = control;
						this.ParentLayer.SetInputButton(2, control.Listener);
						break;
					case 1:
						this.m_IconBackpackEquip = control;
						break;
					case 2:
						this.m_LblEquip = control;
						break;
					case 3:
						this.m_LblEquipEffect = control;
						break;
					case 4:
						this.m_LblEquipAddEffect = control;
						break;
					case 5:
						this.m_OnHover = control;
						break;
					}
				}
			}
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x000A58AC File Offset: 0x000A3AAC
		private void BackpackEqupipOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (this.RemoveType == -1)
			{
				if (this.UseEquip != null)
				{
					this.UseEquip.Invoke(go);
				}
			}
			else if (this.RemoveEqupip != null)
			{
				this.RemoveEqupip.Invoke(go, this.RemoveType);
			}
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x000A590C File Offset: 0x000A3B0C
		private void BackpackEqupipOnKeySelect(GameObject go, bool bSelect)
		{
			this.m_OnHover.GameObject.SetActive(bSelect);
			if (bSelect)
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

		// Token: 0x0600130F RID: 4879 RVA: 0x0000C5FD File Offset: 0x0000A7FD
		private void BackpackEqupipOnHover(GameObject go, bool bHover)
		{
			this.m_OnHover.GameObject.SetActive(bHover);
			if (bHover && GameCursor.IsShow && this.SetCurrent != null)
			{
				this.SetCurrent.Invoke(go);
			}
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0000C637 File Offset: 0x0000A837
		public void SetRemoveEquip(int iType)
		{
			this.m_LblEquip.Text = Game.StringTable.GetString(100159);
			this.Obj.SetActive(true);
			this.RemoveType = iType;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x000A597C File Offset: 0x000A3B7C
		public void SetWgBackpackEquip(BackpackNewDataNode node)
		{
			if (node == null)
			{
				return;
			}
			this.RemoveType = -1;
			this.Obj.SetActive(true);
			this.Obj.GetComponent<BackpackDataNode>().m_BackpackNode = node;
			string strIcon = node._ItemDataNode.m_strIcon;
			this.SetTexture(strIcon, this.m_IconBackpackEquip);
			this.m_LblEquip.Text = node._ItemDataNode.m_strItemName;
			this.m_LblEquipAddEffect.GameObject.SetActive(false);
			this.m_LblEquipEffect.GameObject.SetActive(false);
			string text = string.Empty;
			string text2 = string.Empty;
			for (int i = 0; i < node._ItemDataNode.m_ItmeEffectNodeList.Count; i++)
			{
				int iItemType = node._ItemDataNode.m_ItmeEffectNodeList[i].m_iItemType;
				int iRecoverType = node._ItemDataNode.m_ItmeEffectNodeList[i].m_iRecoverType;
				int iValue = node._ItemDataNode.m_ItmeEffectNodeList[i].m_iValue;
				if (iItemType == 1)
				{
					this.m_LblEquipEffect.GameObject.SetActive(true);
					this.m_LblEquipEffect.Text = string.Empty;
					string @string = Game.StringTable.GetString(110100 + iRecoverType);
					if (iValue >= 0)
					{
						string text3 = text;
						text = string.Concat(new object[]
						{
							text3,
							@string,
							" +",
							iValue,
							"  "
						});
					}
					else
					{
						string text4 = text;
						text = string.Concat(new object[]
						{
							text4,
							@string,
							" ",
							iValue,
							"  "
						});
					}
				}
				else if (iItemType == 7)
				{
					this.m_LblEquipAddEffect.GameObject.SetActive(true);
					string string2 = Game.StringTable.GetString(160004);
					ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(iRecoverType);
					text2 = string2 + ":\n" + conditionNode.m_strName;
				}
				this.m_LblEquipEffect.Text = text;
				this.m_LblEquipAddEffect.Text = text2;
			}
			string text5 = string.Empty;
			if (node.mod_Guid >= 1000 && Game.Variable.mod_EquipDic[node.mod_Guid] != null)
			{
				if (!this.m_LblEquipAddEffect.GameObject.active)
				{
					this.m_LblEquipAddEffect.GameObject.SetActive(true);
					text5 = Game.StringTable.GetString(160004) + ":";
				}
				this.m_LblEquipAddEffect.UILabel.height = 300;
				this.m_LblEquipAddEffect.UILabel.maxLineCount = 0;
				this.m_LblEquipAddEffect.UILabel.fontSize = 25;
				List<ItmeEffectNode> list = Game.Variable.mod_EquipDic[node.mod_Guid];
				for (int j = 0; j < list.Count; j++)
				{
					ConditionNode conditionNode2 = Game.g_BattleControl.m_battleAbility.GetConditionNode(list[j].m_iRecoverType);
					text5 = text5 + "\n" + conditionNode2.m_strName;
				}
				this.m_LblEquipAddEffect.Text = this.m_LblEquipAddEffect.Text + text5;
			}
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x000A5CAC File Offset: 0x000A3EAC
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

		// Token: 0x0400170A RID: 5898
		public new Action<GameObject> SetScrollBar;

		// Token: 0x0400170B RID: 5899
		private Control m_BackpackEquip;

		// Token: 0x0400170C RID: 5900
		private Control m_IconBackpackEquip;

		// Token: 0x0400170D RID: 5901
		private Control m_LblEquip;

		// Token: 0x0400170E RID: 5902
		private Control m_LblEquipEffect;

		// Token: 0x0400170F RID: 5903
		private Control m_LblEquipAddEffect;

		// Token: 0x04001710 RID: 5904
		private Control m_OnHover;

		// Token: 0x04001711 RID: 5905
		private int RemoveType = -1;

		// Token: 0x04001712 RID: 5906
		public Action<GameObject> SetCurrent;

		// Token: 0x04001713 RID: 5907
		public Action<GameObject> UseEquip;

		// Token: 0x04001714 RID: 5908
		public Action<GameObject, int> RemoveEqupip;
	}
}
