using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002ED RID: 749
	public class CtrlBackpack
	{
		// Token: 0x06000FA8 RID: 4008 RVA: 0x0000A673 File Offset: 0x00008873
		public void GetBackpackData()
		{
			this.packList = BackpackStatus.m_Instance.GetTeamBackpack();
			this.sortList = BackpackStatus.m_Instance.GetSortTeamBackpack();
			this.memberList = TeamStatus.m_Instance.GetTeamMemberList();
			this.maxAmount = 16;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0000A6AD File Offset: 0x000088AD
		public void UpdateBackpackView()
		{
			this.money = BackpackStatus.m_Instance.GetMoney();
			this.setMoney.Invoke(this.money);
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00085298 File Offset: 0x00083498
		public void UpdateItemView(bool action, int type)
		{
			if (!action)
			{
				this.ChangeItemNewStatus(this.sortList);
			}
			this.SetTypeNewAmount();
			BackpackStatus.m_Instance.SortItem(type);
			this.SetItemData();
			this.setMouseActive.Invoke(this.sortList.Count, this.maxAmount, true);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x000852EC File Offset: 0x000834EC
		private void SetItemData()
		{
			this.resetItem.Invoke();
			this.complementAmount.Invoke(this.sortList.Count);
			for (int i = 0; i < this.sortList.Count; i++)
			{
				this.setItemView.Invoke(i, this.sortList[i]);
			}
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00085350 File Offset: 0x00083550
		private void SetTypeNewAmount()
		{
			this.resetTypeNewAmount.Invoke();
			for (int i = 1; i < Enum.GetNames(typeof(ItemDataNode.ItemType)).Length; i++)
			{
				for (int j = 0; j < this.packList.Count; j++)
				{
					if (i == this.packList[j]._ItemDataNode.m_iItemType && this.packList[j].m_bNew)
					{
						this.newAmount++;
					}
				}
				if (this.newAmount > 0)
				{
					this.setTypeNewAmount.Invoke(i - 1, this.newAmount);
				}
				this.newAmount = 0;
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00085410 File Offset: 0x00083610
		public void ChangeItemNewStatus(List<BackpackNewDataNode> backpackList)
		{
			for (int i = 0; i < backpackList.Count; i++)
			{
				BackpackStatus.m_Instance.ChangeNew(backpackList[i]);
			}
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x00085448 File Offset: 0x00083648
		public void SetMemberData(int type)
		{
			this.showEquipID = 0;
			this.initMemberView.Invoke();
			for (int i = 0; i < this.memberList.Count; i++)
			{
				MemberData memberData = new MemberData();
				memberData.texture = (Game.g_HexHeadBundle.Load("2dtexture/gameui/hexhead/" + this.memberList[i]._NpcDataNode.m_strSmallImage) as Texture);
				memberData.name = this.memberList[i]._NpcDataNode.m_strNpcName;
				memberData.maxHP = this.memberList[i]._TotalProperty.Get(CharacterData.PropertyType.MaxHP).ToString();
				memberData.maxSP = this.memberList[i]._TotalProperty.Get(CharacterData.PropertyType.MaxSP).ToString();
				switch (type)
				{
				case 1:
					this.showEquipID = this.memberList[i].iEquipWeaponID;
					break;
				case 2:
					this.showEquipID = this.memberList[i].iEquipArrorID;
					break;
				case 3:
					this.showEquipID = this.memberList[i].iEquipNecklaceID;
					break;
				}
				if (this.showEquipID == 0)
				{
					memberData.equip = string.Empty;
				}
				else
				{
					memberData.equip = Game.ItemData.GetItemName(this.showEquipID);
				}
				this.setMemberView.Invoke(i, memberData);
			}
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x000855D4 File Offset: 0x000837D4
		public void SetTipData(int index)
		{
			ItemDataNode itemDataNode = this.sortList[index]._ItemDataNode;
			TipData tipData = new TipData();
			tipData.name = itemDataNode.m_strItemName;
			tipData.texture = (Game.g_Item.Load("2dtexture/gameui/item/" + itemDataNode.m_strIcon) as Texture);
			tipData.explain = itemDataNode.m_strTip;
			for (int i = 0; i < itemDataNode.m_ItmeEffectNodeList.Count; i++)
			{
				if (i < itemDataNode.m_ItmeEffectNodeList.Count)
				{
					ItmeEffectNode itmeEffectNode = itemDataNode.m_ItmeEffectNodeList[i];
					int iItemType = itmeEffectNode.m_iItemType;
					int iRecoverType = itmeEffectNode.m_iRecoverType;
					int iValue = itmeEffectNode.m_iValue;
					switch (iItemType)
					{
					case 1:
					{
						string @string = Game.StringTable.GetString(110100 + iRecoverType);
						string text;
						if (iValue > 0)
						{
							text = "  +";
						}
						else
						{
							text = "  ";
						}
						tipData.appendList.Add(@string + text + iValue.ToString());
						break;
					}
					case 2:
					case 5:
					case 6:
						break;
					case 3:
					{
						RoutineNewDataNode routineNewData = Game.RoutineNewData.GetRoutineNewData(iRecoverType);
						if (routineNewData != null)
						{
							tipData.appendList.Add(routineNewData.m_strUpgradeNotes.Replace("<br>", "\n"));
						}
						break;
					}
					case 4:
					{
						NeigongNewDataNode neigongData = Game.NeigongData.GetNeigongData(iRecoverType);
						if (neigongData != null)
						{
							tipData.appendList.Add(neigongData.m_strUpgradeNotes.Replace("<br>", "\n"));
						}
						break;
					}
					case 7:
					{
						string string2 = Game.StringTable.GetString(160004);
						ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(iRecoverType);
						string text2 = string.Concat(new string[]
						{
							"[c][12C0D3]",
							string2,
							"  ",
							conditionNode.m_strName,
							"[-][/c]\n",
							conditionNode.m_strDesp
						});
						tipData.appendList.Add(text2);
						break;
					}
					case 8:
					{
						string string3 = Game.StringTable.GetString(110300);
						tipData.appendList.Add(string3 + " " + iValue.ToString());
						break;
					}
					case 9:
					{
						string string4 = Game.StringTable.GetString(110301);
						tipData.appendList.Add(string4 + " " + iValue.ToString());
						break;
					}
					default:
						switch (iItemType)
						{
						case 16:
						{
							string string5 = Game.StringTable.GetString(110100);
							tipData.appendList.Add(string5 + " " + iValue.ToString() + "%");
							break;
						}
						case 17:
						{
							string string6 = Game.StringTable.GetString(110102);
							tipData.appendList.Add(string6 + " " + iValue.ToString() + "%");
							break;
						}
						case 18:
						{
							string text3 = Game.StringTable.GetString(110302);
							text3 = string.Format(text3, iValue.ToString());
							tipData.appendList.Add(text3);
							break;
						}
						}
						break;
					}
				}
			}
			if (this.sortList[index].mod_Guid >= 1000 && Game.Variable.mod_EquipDic[this.sortList[index].mod_Guid] != null)
			{
				List<ItmeEffectNode> list = Game.Variable.mod_EquipDic[this.sortList[index].mod_Guid];
				for (int j = 0; j < list.Count; j++)
				{
					string string7 = Game.StringTable.GetString(160004);
					ConditionNode conditionNode2 = Game.g_BattleControl.m_battleAbility.GetConditionNode(list[j].m_iRecoverType);
					string text4 = string.Concat(new string[]
					{
						"[c][12C0D3]",
						string7,
						"  ",
						conditionNode2.m_strName,
						"[-][/c]"
					});
					tipData.appendList.Add(text4);
				}
			}
			for (int k = 0; k < itemDataNode.m_UseLimitNodeList.Count; k++)
			{
				string text5 = string.Empty;
				if (k == 0)
				{
					int iItemType2 = itemDataNode.m_iItemType;
					if (iItemType2 - 1 > 2)
					{
						if (iItemType2 == 6)
						{
							text5 = Game.StringTable.GetString(110201) + "：";
						}
					}
					else
					{
						text5 = Game.StringTable.GetString(110200) + "：";
					}
				}
				UseLimitNode useLimitNode = itemDataNode.m_UseLimitNodeList[k];
				UseLimitType type = useLimitNode.m_Type;
				int iInde = useLimitNode.m_iInde;
				int iValue2 = useLimitNode.m_iValue;
				if (type == UseLimitType.MoreNpcProperty)
				{
					text5 += Game.StringTable.GetString(110100 + iInde);
					text5 += iValue2.ToString();
					tipData.limitList.Add(text5);
				}
			}
			for (int l = 0; l < itemDataNode.m_CanUseiNpcIDList.Count; l++)
			{
				string text6 = string.Empty;
				if (l == 0)
				{
					text6 = Game.StringTable.GetString(110202) + "：";
				}
				if (itemDataNode.m_CanUseiNpcIDList[l] == 98)
				{
					text6 += Game.StringTable.GetString(110206);
				}
				else if (itemDataNode.m_CanUseiNpcIDList[l] == 99)
				{
					text6 += Game.StringTable.GetString(110207);
				}
				else
				{
					text6 += Game.NpcData.GetNpcName(itemDataNode.m_CanUseiNpcIDList[l]);
				}
				tipData.limitNPCList.Add(text6);
			}
			this.setItemTipView.Invoke(tipData);
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0000A6D0 File Offset: 0x000088D0
		public bool CheckUseTime(int index)
		{
			this.currentItem = this.sortList[index];
			return this.currentItem._ItemDataNode.m_iUseTime != 2;
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00085BA0 File Offset: 0x00083DA0
		public void SetMsg()
		{
			string text = Game.StringTable.GetString(210039);
			if (text.Equals(string.Empty))
			{
				Debug.LogError("抓不到文字" + 210039);
			}
			text = string.Format(text, this.currentItem._ItemDataNode.m_strItemName);
			Game.UI.Get<UIMapMessage>().SetMsg(text);
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00085C10 File Offset: 0x00083E10
		public void CheckMemberCanUse()
		{
			for (int i = 0; i < this.memberList.Count; i++)
			{
				CharacterData characterData = this.memberList[i];
				bool flag = BackpackStatus.m_Instance.CheckItemUse(characterData, this.currentItem._ItemDataNode);
				this.setNotUseMemberView.Invoke(i, flag);
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00085C6C File Offset: 0x00083E6C
		public void MemberUseItem(int index, int type)
		{
			if (index > this.memberList.Count - 1)
			{
				return;
			}
			CharacterData characterData = this.memberList[index];
			if (this.currentItem.m_iAmount <= 0)
			{
				return;
			}
			BackpackStatus.m_Instance.ChangeNew(this.currentItem);
			BackpackStatus.m_Instance.UseItem(characterData, this.currentItem);
			BackpackStatus.m_Instance.SortItem(type);
			this.SetItemData();
			this.resetClickItem.Invoke();
			this.SetTypeNewAmount();
			this.setMouseActive.Invoke(this.sortList.Count, this.maxAmount, false);
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0000A6FD File Offset: 0x000088FD
		public void CloseView()
		{
			this.ChangeItemNewStatus(this.packList);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0000A70B File Offset: 0x0000890B
		public int GetSortCount()
		{
			return this.sortList.Count;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0000A718 File Offset: 0x00008918
		public int GetMemberCount()
		{
			return this.memberList.Count;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0000A725 File Offset: 0x00008925
		public int GetMaxAmount()
		{
			return this.maxAmount;
		}

		// Token: 0x040012BC RID: 4796
		private List<BackpackNewDataNode> packList;

		// Token: 0x040012BD RID: 4797
		private List<BackpackNewDataNode> sortList;

		// Token: 0x040012BE RID: 4798
		private List<CharacterData> memberList;

		// Token: 0x040012BF RID: 4799
		private int money;

		// Token: 0x040012C0 RID: 4800
		private int newAmount;

		// Token: 0x040012C1 RID: 4801
		private int maxAmount;

		// Token: 0x040012C2 RID: 4802
		private int showEquipID;

		// Token: 0x040012C3 RID: 4803
		private BackpackNewDataNode currentItem;

		// Token: 0x040012C4 RID: 4804
		public Action<int> complementAmount;

		// Token: 0x040012C5 RID: 4805
		public Action<int, BackpackNewDataNode> setItemView;

		// Token: 0x040012C6 RID: 4806
		public Action<int> setMoney;

		// Token: 0x040012C7 RID: 4807
		public Action<int, int> setTypeNewAmount;

		// Token: 0x040012C8 RID: 4808
		public Action initMemberView;

		// Token: 0x040012C9 RID: 4809
		public Action<int, MemberData> setMemberView;

		// Token: 0x040012CA RID: 4810
		public Action<TipData> setItemTipView;

		// Token: 0x040012CB RID: 4811
		public Action<int, int, bool> setMouseActive;

		// Token: 0x040012CC RID: 4812
		public Action resetItem;

		// Token: 0x040012CD RID: 4813
		public Action resetClickItem;

		// Token: 0x040012CE RID: 4814
		public Action resetTypeNewAmount;

		// Token: 0x040012CF RID: 4815
		public Action<int, bool> setNotUseMemberView;
	}
}
