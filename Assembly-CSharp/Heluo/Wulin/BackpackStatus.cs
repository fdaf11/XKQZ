using System;
using System.Collections.Generic;
using System.Linq;
using Heluo.Wulin.UI;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020007FF RID: 2047
	public class BackpackStatus : MonoBehaviour
	{
		// Token: 0x06003236 RID: 12854 RVA: 0x0001F906 File Offset: 0x0001DB06
		private void Awake()
		{
			if (BackpackStatus.m_Instance == null)
			{
				BackpackStatus.m_Instance = this;
				this.m_BackpackList = new List<BackpackNewDataNode>();
				this.m_SortItemList = new List<BackpackNewDataNode>();
				return;
			}
		}

		// Token: 0x06003237 RID: 12855 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x0001F93A File Offset: 0x0001DB3A
		public void StartNewGame()
		{
			this.m_iMoney = 0;
			this.m_BackpackList.Clear();
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x00185804 File Offset: 0x00183A04
		public bool AddPackItem(int itemID, int amount = 1, bool bNotPack = true)
		{
			if (amount == 0)
			{
				return false;
			}
			ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(itemID);
			if (itemDataNode == null)
			{
				return false;
			}
			if (itemDataNode.m_iItemStack == 0)
			{
				BackpackNewDataNode backpackNewDataNode = this.m_BackpackList.Find((BackpackNewDataNode item) => item.ItemID == itemID);
				if (backpackNewDataNode != null)
				{
					backpackNewDataNode.m_iAmount += amount;
					backpackNewDataNode.m_iAmount = Mathf.Clamp(backpackNewDataNode.m_iAmount, 0, 999999);
					if (bNotPack)
					{
						backpackNewDataNode.m_bNew = true;
						BackpackNewDataNode backpackNewDataNode2 = backpackNewDataNode;
						this.m_BackpackList.Remove(backpackNewDataNode);
						this.m_BackpackList.Insert(0, backpackNewDataNode2);
					}
					else
					{
						this.ChangeNew(backpackNewDataNode);
					}
				}
				else
				{
					this.AddNewPackItem(itemDataNode, amount, bNotPack);
				}
			}
			else
			{
				for (int i = 0; i < amount; i++)
				{
					BackpackNewDataNode backpackNewDataNode3 = new BackpackNewDataNode();
					foreach (BackpackNewDataNode backpackNewDataNode4 in this.m_BackpackList)
					{
						if (backpackNewDataNode4.ItemID == itemID && backpackNewDataNode4.m_bNew)
						{
							backpackNewDataNode3 = backpackNewDataNode4;
							break;
						}
					}
					if (backpackNewDataNode3 != null)
					{
						backpackNewDataNode3.m_bNew = false;
					}
					this.AddNewPackItem(itemDataNode, 1, bNotPack);
				}
			}
			return true;
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x00185954 File Offset: 0x00183B54
		private void AddNewPackItem(ItemDataNode itemDataNode, int amount, bool bNew)
		{
			BackpackNewDataNode backpackNewDataNode = new BackpackNewDataNode();
			backpackNewDataNode._ItemDataNode = itemDataNode;
			backpackNewDataNode.m_iAmount = amount;
			backpackNewDataNode.m_bNew = bNew;
			if (itemDataNode.m_iItemType >= 1 && itemDataNode.m_iItemType <= 3 && bNew)
			{
				int num = Game.Variable.mod_GetGuid();
				List<ItmeEffectNode> list = new List<ItmeEffectNode>();
				List<int> list2 = Enumerable.ToList<int>(Enumerable.Select<ConditionNode, int>(Game.g_BattleControl.m_battleAbility.m_ConditionList, (ConditionNode x) => x.m_iConditionID));
				for (int i = 0; i < (GameGlobal.mod_Difficulty + 1) / 2; i++)
				{
					int num2 = Random.Range(0, list2.Count);
					list.Add(new ItmeEffectNode
					{
						m_iItemType = 7,
						m_iRecoverType = list2[num2],
						m_iValue = 0,
						m_iMsgID = 0
					});
					list2.RemoveAt(num2);
				}
				Game.Variable.mod_EquipDic.Add(num, list);
				backpackNewDataNode.mod_Guid = num;
			}
			if (Game.mod_CurrentCharacterData != null && !bNew && itemDataNode.m_iItemType == 1)
			{
				backpackNewDataNode.mod_Guid = Game.mod_CurrentCharacterData.mod_WeaponGuid;
			}
			if (Game.mod_CurrentCharacterData != null && !bNew && itemDataNode.m_iItemType == 2)
			{
				backpackNewDataNode.mod_Guid = Game.mod_CurrentCharacterData.mod_ArmorGuid;
			}
			if (Game.mod_CurrentCharacterData != null && !bNew && itemDataNode.m_iItemType == 3)
			{
				backpackNewDataNode.mod_Guid = Game.mod_CurrentCharacterData.mod_NecklaceGuid;
			}
			this.m_BackpackList.Insert(0, backpackNewDataNode);
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x00185AD0 File Offset: 0x00183CD0
		public bool LessPackItem(int itemID, int amount = 1, BackpackNewDataNode node = null)
		{
			if (amount == 0)
			{
				return false;
			}
			if (node == null)
			{
				node = this.m_BackpackList.Find((BackpackNewDataNode item) => item.ItemID == itemID);
			}
			if (node == null)
			{
				Debug.Log("背包沒有這個東西 :  " + itemID);
				return false;
			}
			node.m_iAmount -= amount;
			node.m_iAmount = Mathf.Clamp(node.m_iAmount, 0, 999999);
			if (node.m_iAmount <= 0)
			{
				this.m_BackpackList.Remove(node);
			}
			return true;
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x00185B68 File Offset: 0x00183D68
		public int CheckPackNewItemAmount()
		{
			this.m_iNewItemAmount = 0;
			for (int i = 0; i < this.m_BackpackList.Count; i++)
			{
				if (this.m_BackpackList[i].m_bNew)
				{
					this.m_iNewItemAmount++;
				}
			}
			return this.m_iNewItemAmount;
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x0001F94E File Offset: 0x0001DB4E
		public void ChangeNew(BackpackNewDataNode backpackNode)
		{
			backpackNode.m_bNew = false;
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x00185BC4 File Offset: 0x00183DC4
		public void UseItem(CharacterData characterData, BackpackNewDataNode backpackNewDataNode)
		{
			if (this.CheclItemAmount(backpackNewDataNode.ItemID) <= 0)
			{
				return;
			}
			if (!this.CheckItemUse(characterData, backpackNewDataNode._ItemDataNode))
			{
				return;
			}
			List<ItmeEffectNode> itmeEffectNodeList = backpackNewDataNode._ItemDataNode.m_ItmeEffectNodeList;
			List<string> list = new List<string>();
			ItemDataNode.ItemType iItemType = (ItemDataNode.ItemType)backpackNewDataNode._ItemDataNode.m_iItemType;
			bool flag = true;
			switch (iItemType)
			{
			case ItemDataNode.ItemType.Weapon:
			{
				int iItemID = backpackNewDataNode._ItemDataNode.m_iItemID;
				this.AddPackItem(characterData.iEquipWeaponID, 1, false);
				characterData.mod_WeaponGuid = backpackNewDataNode.mod_Guid;
				characterData.SetEquip(iItemType, iItemID);
				break;
			}
			case ItemDataNode.ItemType.Arror:
			{
				int iItemID2 = backpackNewDataNode._ItemDataNode.m_iItemID;
				this.AddPackItem(characterData.iEquipArrorID, 1, false);
				characterData.mod_ArmorGuid = backpackNewDataNode.mod_Guid;
				characterData.SetEquip(iItemType, iItemID2);
				break;
			}
			case ItemDataNode.ItemType.Necklace:
			{
				int iItemID3 = backpackNewDataNode._ItemDataNode.m_iItemID;
				this.AddPackItem(characterData.iEquipNecklaceID, 1, false);
				characterData.mod_NecklaceGuid = backpackNewDataNode.mod_Guid;
				characterData.SetEquip(iItemType, iItemID3);
				break;
			}
			case ItemDataNode.ItemType.Solution:
			case ItemDataNode.ItemType.TipsBook:
				for (int i = 0; i < itmeEffectNodeList.Count; i++)
				{
					int iItemType2 = itmeEffectNodeList[i].m_iItemType;
					int iRecoverType = itmeEffectNodeList[i].m_iRecoverType;
					int iValue = itmeEffectNodeList[i].m_iValue;
					switch (iItemType2)
					{
					case 1:
					{
						NPC.m_instance.SetCharacterProperty(characterData.iNpcID, iRecoverType, iValue);
						string @string = Game.StringTable.GetString(110100 + iRecoverType);
						string text = string.Format(Game.StringTable.GetString(210037), characterData._NpcDataNode.m_strNpcName, @string, iValue);
						list.Add(text);
						break;
					}
					case 3:
						flag = NPC.m_instance.AddRoutine(characterData.iNpcID, iRecoverType, 1);
						break;
					case 4:
						flag = NPC.m_instance.AddNeigong(characterData.iNpcID, iRecoverType, 1);
						break;
					case 5:
					{
						NPC.m_instance.SetNPCNowPracticeExp(characterData, iValue, false, 0);
						string string2 = Game.StringTable.GetString(210046);
						if (string2 != string.Empty)
						{
							string[] array = new string[]
							{
								characterData.GetNowPracticeName(),
								iValue.ToString()
							};
							string text2 = string2;
							object[] array2 = array;
							string text3 = string.Format(text2, array2);
							list.Add(text3);
						}
						string string3 = Game.StringTable.GetString(210047);
						if (string3 != string.Empty)
						{
							string[] array3 = new string[]
							{
								characterData.GetNowPracticeName(),
								characterData.GetNowPracticeLevel().ToString()
							};
							string text4 = string3;
							object[] array2 = array3;
							string text5 = string.Format(text4, array2);
							list.Add(text5);
						}
						break;
					}
					}
					if (!flag)
					{
						return;
					}
				}
				break;
			}
			int num;
			if (iItemType == ItemDataNode.ItemType.Weapon || iItemType == ItemDataNode.ItemType.Arror)
			{
				num = 179996;
			}
			else
			{
				num = 179994 + backpackNewDataNode._ItemDataNode.m_iItemType;
			}
			string text6 = Game.StringTable.GetString(num);
			if (text6.Equals(string.Empty))
			{
				Debug.LogError("抓不到文字" + num);
			}
			text6 = string.Format(text6, characterData._NpcDataNode.m_strNpcName, backpackNewDataNode._ItemDataNode.m_strItemName);
			Game.UI.Get<UIMapMessage>().SetMsg(text6);
			for (int j = 0; j < list.Count; j++)
			{
				Game.UI.Get<UIMapMessage>().SetMsg(list[j]);
			}
			list.Clear();
			this.LessPackItem(0, 1, backpackNewDataNode);
			switch (iItemType)
			{
			case ItemDataNode.ItemType.Weapon:
				UIAudioDataManager.Singleton.PlayUIAudio(UIAudioData.eUIAudio.Weapon);
				return;
			case ItemDataNode.ItemType.Arror:
				UIAudioDataManager.Singleton.PlayUIAudio(UIAudioData.eUIAudio.Armor);
				return;
			case ItemDataNode.ItemType.Necklace:
				UIAudioDataManager.Singleton.PlayUIAudio(UIAudioData.eUIAudio.Necklace);
				return;
			case ItemDataNode.ItemType.Solution:
				UIAudioDataManager.Singleton.PlayUIAudio(UIAudioData.eUIAudio.UseItem);
				return;
			case ItemDataNode.ItemType.Mission:
				break;
			case ItemDataNode.ItemType.TipsBook:
				UIAudioDataManager.Singleton.PlayUIAudio(UIAudioData.eUIAudio.TipsBook);
				break;
			default:
				return;
			}
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x00185FB0 File Offset: 0x001841B0
		public bool CheckItemUse(CharacterData _CharacterData, ItemDataNode _ItemDataNode)
		{
			if (_ItemDataNode.m_iUseTime == 2)
			{
				return false;
			}
			if (_ItemDataNode.m_iItemUse == 0)
			{
				return false;
			}
			if (_ItemDataNode.m_AmsType != WeaponType.None && !_CharacterData.CheckWeaponType(_ItemDataNode.m_AmsType))
			{
				return false;
			}
			if (_ItemDataNode.m_NoUseNpcList.Count > 0)
			{
				bool flag = _ItemDataNode.m_NoUseNpcList.Contains(_CharacterData.iNpcID);
				if (flag)
				{
					return false;
				}
			}
			if (_ItemDataNode.m_CanUseiNpcIDList.Count != 0)
			{
				bool flag2 = false;
				for (int i = 0; i < _ItemDataNode.m_CanUseiNpcIDList.Count; i++)
				{
					if (_CharacterData.iNpcID == _ItemDataNode.m_CanUseiNpcIDList[i])
					{
						flag2 = true;
					}
					if (_ItemDataNode.m_CanUseiNpcIDList[i] == 99 && _CharacterData._NpcDataNode.m_Gender == GenderType.Female)
					{
						flag2 = true;
					}
					else if (_ItemDataNode.m_CanUseiNpcIDList[i] == 98 && _CharacterData._NpcDataNode.m_Gender == GenderType.Male)
					{
						flag2 = true;
					}
				}
				if (!flag2)
				{
					return false;
				}
			}
			if (_ItemDataNode.m_UseLimitNodeList.Count != 0 && !_ItemDataNode.CheckUse(_CharacterData))
			{
				return false;
			}
			if (_ItemDataNode.m_iItemType == 6)
			{
				for (int j = 0; j < _ItemDataNode.m_ItmeEffectNodeList.Count; j++)
				{
					List<ItmeEffectNode> itmeEffectNodeList = _ItemDataNode.m_ItmeEffectNodeList;
					int iItemType = itmeEffectNodeList[j].m_iItemType;
					int iRecoverType = itmeEffectNodeList[j].m_iRecoverType;
					if (iItemType == 3)
					{
						if (!_CharacterData.CheckRoutine(iRecoverType))
						{
							return false;
						}
					}
					else if (iItemType == 4 && !_CharacterData.CheckNeigong(iRecoverType))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x00186174 File Offset: 0x00184374
		public void SortItem(int iType)
		{
			this.m_SortItemList.Clear();
			for (int i = 0; i < this.m_BackpackList.Count; i++)
			{
				if (iType == this.m_BackpackList[i]._ItemDataNode.m_iItemType)
				{
					this.m_SortItemList.Add(this.m_BackpackList[i]);
				}
			}
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x001861DC File Offset: 0x001843DC
		public void SortBattleItem()
		{
			this.m_SortItemList.Clear();
			for (int i = 0; i < this.m_BackpackList.Count; i++)
			{
				if (this.m_BackpackList[i]._ItemDataNode.m_iUseTime == 2 || this.m_BackpackList[i]._ItemDataNode.m_iUseTime == 0)
				{
					this.m_SortItemList.Add(this.m_BackpackList[i]);
				}
			}
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x0001F957 File Offset: 0x0001DB57
		public void ChangeMoney(int iValue)
		{
			this.m_iMoney += iValue;
			this.m_iMoney = Mathf.Clamp(this.m_iMoney, 0, 999999999);
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x00186260 File Offset: 0x00184460
		public BackpackNewDataNode GetBackpackItem(int iID)
		{
			for (int i = 0; i < this.m_BackpackList.Count; i++)
			{
				if (this.m_BackpackList[i]._ItemDataNode.m_iItemID == iID)
				{
					return this.m_BackpackList[i];
				}
			}
			return null;
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x0001F97E File Offset: 0x0001DB7E
		public List<BackpackNewDataNode> GetTeamBackpack()
		{
			return this.m_BackpackList;
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x0001F986 File Offset: 0x0001DB86
		public List<BackpackNewDataNode> GetSortTeamBackpack()
		{
			return this.m_SortItemList;
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x001862B4 File Offset: 0x001844B4
		public int CheclItemAmount(int iID)
		{
			int num = 0;
			for (int i = 0; i < this.m_BackpackList.Count; i++)
			{
				if (this.m_BackpackList[i]._ItemDataNode.m_iItemID == iID)
				{
					if (this.m_BackpackList[i]._ItemDataNode.m_iItemStack != 1)
					{
						num = this.m_BackpackList[i].m_iAmount;
						break;
					}
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x0001F98E File Offset: 0x0001DB8E
		public int GetMoney()
		{
			return this.m_iMoney;
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x0001F996 File Offset: 0x0001DB96
		public void setMoney(int iMoney)
		{
			this.m_iMoney = iMoney;
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x00186338 File Offset: 0x00184538
		public List<BackpackNewDataNode> GetBackpackSaveDataList()
		{
			List<BackpackNewDataNode> list = new List<BackpackNewDataNode>();
			for (int i = 0; i < this.m_BackpackList.Count; i++)
			{
				list.Add(new BackpackNewDataNode
				{
					m_ItemID = this.m_BackpackList[i]._ItemDataNode.m_iItemID,
					m_bNew = this.m_BackpackList[i].m_bNew,
					m_iAmount = this.m_BackpackList[i].m_iAmount,
					mod_Guid = this.m_BackpackList[i].mod_Guid
				});
			}
			return list;
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x001863D0 File Offset: 0x001845D0
		public void LoadBackpack(List<BackpackNewDataNode> LoadBackpackList)
		{
			this.m_BackpackList.Clear();
			List<BackpackNewDataNode> list = new List<BackpackNewDataNode>();
			if (LoadBackpackList == null)
			{
				return;
			}
			for (int i = 0; i < LoadBackpackList.Count; i++)
			{
				ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(LoadBackpackList[i].m_ItemID);
				if (itemDataNode != null)
				{
					LoadBackpackList[i]._ItemDataNode = itemDataNode;
					list.Add(LoadBackpackList[i]);
				}
				else
				{
					Debug.LogError("ItemData    don't have this id " + LoadBackpackList[i].m_ItemID);
					LoadBackpackList[i]._ItemDataNode = new ItemDataNode();
				}
			}
			this.m_BackpackList.AddRange(list);
		}

		// Token: 0x04003DF8 RID: 15864
		public static BackpackStatus m_Instance;

		// Token: 0x04003DF9 RID: 15865
		private List<BackpackNewDataNode> m_BackpackList;

		// Token: 0x04003DFA RID: 15866
		private List<BackpackNewDataNode> m_SortItemList;

		// Token: 0x04003DFB RID: 15867
		private int m_iMoney;

		// Token: 0x04003DFC RID: 15868
		private int m_iNewItemAmount;

		// Token: 0x04003DFD RID: 15869
		public CharacterData mod_CharacterData;
	}
}
