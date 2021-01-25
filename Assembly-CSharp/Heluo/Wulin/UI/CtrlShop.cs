using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002F6 RID: 758
	public class CtrlShop
	{
		// Token: 0x06001013 RID: 4115 RVA: 0x0008837C File Offset: 0x0008657C
		public void UpdateShopData(bool bReset)
		{
			this.initView.Invoke();
			this.packList.Clear();
			List<BackpackNewDataNode> teamBackpack = BackpackStatus.m_Instance.GetTeamBackpack();
			for (int i = 0; i < teamBackpack.Count; i++)
			{
				ItemDataNode itemDataNode = teamBackpack[i]._ItemDataNode;
				if (itemDataNode.m_iLock != 1)
				{
					this.packList.Add(teamBackpack[i]);
				}
			}
			this.GetShopList();
			this.SetShopData();
			this.setMouseActive.Invoke(this.showAmount, bReset);
			this.packMoney = BackpackStatus.m_Instance.GetMoney();
			this.setMoneyView.Invoke(this.packMoney);
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00088430 File Offset: 0x00086630
		private void CreateDLCShopList()
		{
			List<StoreItemNode> storeItemNodeList = this.Storedata.m_StoreItemNodeList;
			this.shopList.Clear();
			for (int i = 0; i < storeItemNodeList.Count; i++)
			{
				ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(storeItemNodeList[i].m_iItemID);
				if (itemDataNode == null)
				{
					Debug.LogWarning("DLCShop Item no found " + storeItemNodeList[i].m_iItemID.ToString());
				}
				else if (ConditionManager.CheckCondition(storeItemNodeList[i].m_ConditionList, storeItemNodeList[i].bAnd, 0, string.Empty))
				{
					if (itemDataNode.m_iLock != 1)
					{
						if (Game.DLCShopInfo.m_ItemList.Contains(storeItemNodeList[i].m_iItemID))
						{
							BackpackNewDataNode backpackNewDataNode = new BackpackNewDataNode();
							backpackNewDataNode._ItemDataNode = itemDataNode;
							this.shopList.Add(backpackNewDataNode);
						}
					}
				}
			}
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0008852C File Offset: 0x0008672C
		private void GetShopList()
		{
			this.go_player = GameObject.FindWithTag("Player");
			int iStoreID = 0;
			if (GameGlobal.m_bDLCMode)
			{
				iStoreID = 1000087;
			}
			else if (this.go_player != null)
			{
				this.target = this.go_player.GetComponent<PlayerController>().Target;
				if (this.target != null)
				{
					iStoreID = this.target.GetComponent<NpcCollider>().m_StoreID;
				}
				else
				{
					iStoreID = Game.UI.Get<UITalk>().iStoreID;
				}
			}
			this.Storedata = Game.StoreData.GetStoreDataNode(iStoreID);
			if (this.Storedata == null)
			{
				return;
			}
			if (GameGlobal.m_bDLCMode)
			{
				this.CreateDLCShopList();
			}
			else
			{
				List<StoreItemNode> storeItemNodeList = this.Storedata.m_StoreItemNodeList;
				this.shopList.Clear();
				for (int i = 0; i < storeItemNodeList.Count; i++)
				{
					ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(storeItemNodeList[i].m_iItemID);
					if (ConditionManager.CheckCondition(storeItemNodeList[i].m_ConditionList, storeItemNodeList[i].bAnd, 0, string.Empty))
					{
						if (itemDataNode.m_iLock != 1)
						{
							BackpackNewDataNode backpackNewDataNode = new BackpackNewDataNode();
							backpackNewDataNode._ItemDataNode = itemDataNode;
							this.shopList.Add(backpackNewDataNode);
						}
					}
				}
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00088694 File Offset: 0x00086894
		private void SetShopData()
		{
			this.m_SellMagnification = 1f + TeamStatus.m_Instance.GetTeamTalentPercentValue(TalentEffect.AuctionMaster, true);
			this.m_BuyMagnification = 1f + TeamStatus.m_Instance.GetTeamTalentPercentValue(TalentEffect.StroeItemDiscounts, false);
			this.complementAmount.Invoke(this.packList.Count, this.shopList.Count);
			this.SetShopDataBase(this.shopList, false);
			this.SetShopDataBase(this.packList, true);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00088714 File Offset: 0x00086914
		private void SetShopDataBase(List<BackpackNewDataNode> dataList, bool pack = false)
		{
			int num = 0;
			for (int i = 0; i < dataList.Count; i++)
			{
				ItemData itemData = new ItemData();
				if (dataList[i]._ItemDataNode.m_iLock != 1)
				{
					itemData.name = dataList[i]._ItemDataNode.m_strItemName;
					if (pack)
					{
						itemData.price = (int)((float)dataList[i]._ItemDataNode.m_iItemSell * this.m_SellMagnification);
						itemData.bNew = dataList[i].m_bNew;
						itemData.amount = dataList[i].m_iAmount;
					}
					else
					{
						itemData.price = (int)((float)dataList[i]._ItemDataNode.m_iItemBuy * this.m_BuyMagnification);
						itemData.bNew = false;
						itemData.amount = -1;
					}
					this.setView.Invoke(itemData, num);
					num++;
				}
			}
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00088804 File Offset: 0x00086A04
		public void SetTipData(int index, bool bPack)
		{
			ItemDataNode itemDataNode;
			if (bPack)
			{
				itemDataNode = this.packList[index]._ItemDataNode;
			}
			else
			{
				itemDataNode = this.shopList[index]._ItemDataNode;
			}
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
						string text = Game.StringTable.GetString(110100 + iRecoverType);
						string text2;
						if (iValue > 0)
						{
							text2 = "  +";
						}
						else
						{
							text2 = "  ";
						}
						tipData.appendList.Add(text + text2 + iValue.ToString());
						break;
					}
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
						string text = Game.StringTable.GetString(160004);
						ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(iRecoverType);
						string text3 = string.Concat(new string[]
						{
							"[c][12C0D3]",
							text,
							"  ",
							conditionNode.m_strName,
							"[-][/c]\n",
							conditionNode.m_strDesp
						});
						tipData.appendList.Add(text3);
						break;
					}
					case 8:
					{
						string text = Game.StringTable.GetString(110300);
						tipData.appendList.Add(text + " " + iValue.ToString());
						break;
					}
					case 9:
					{
						string text = Game.StringTable.GetString(110301);
						tipData.appendList.Add(text + " " + iValue.ToString());
						break;
					}
					case 16:
					{
						string text = Game.StringTable.GetString(110100);
						tipData.appendList.Add(text + " " + iValue.ToString() + "%");
						break;
					}
					case 17:
					{
						string text = Game.StringTable.GetString(110102);
						tipData.appendList.Add(text + " " + iValue.ToString() + "%");
						break;
					}
					case 18:
					{
						string text = Game.StringTable.GetString(110302);
						text = string.Format(text, iValue.ToString());
						tipData.appendList.Add(text);
						break;
					}
					}
				}
			}
			for (int j = 0; j < itemDataNode.m_UseLimitNodeList.Count; j++)
			{
				string text4 = string.Empty;
				if (j == 0)
				{
					switch (itemDataNode.m_iItemType)
					{
					case 1:
					case 2:
					case 3:
						text4 = Game.StringTable.GetString(110200) + "：";
						break;
					case 6:
						text4 = Game.StringTable.GetString(110201) + "：";
						break;
					}
				}
				UseLimitNode useLimitNode = itemDataNode.m_UseLimitNodeList[j];
				UseLimitType type = useLimitNode.m_Type;
				int iInde = useLimitNode.m_iInde;
				int iValue2 = useLimitNode.m_iValue;
				if (type == UseLimitType.MoreNpcProperty)
				{
					text4 += Game.StringTable.GetString(110100 + iInde);
					text4 += iValue2.ToString();
					tipData.limitList.Add(text4);
				}
			}
			for (int k = 0; k < itemDataNode.m_CanUseiNpcIDList.Count; k++)
			{
				string text5 = string.Empty;
				if (itemDataNode.m_iItemType == 6)
				{
					if (k == 0)
					{
						text5 = Game.StringTable.GetString(110202) + "：";
					}
					if (itemDataNode.m_CanUseiNpcIDList[k] == 98)
					{
						text5 += Game.StringTable.GetString(110206);
					}
					else if (itemDataNode.m_CanUseiNpcIDList[k] == 99)
					{
						text5 += Game.StringTable.GetString(110207);
					}
					else
					{
						text5 += Game.NpcData.GetNpcName(itemDataNode.m_CanUseiNpcIDList[k]);
					}
					tipData.limitNPCList.Add(text5);
				}
			}
			this.setItemTipView.Invoke(tipData);
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0000A9E6 File Offset: 0x00008BE6
		public void GetItemIndex(int index, int state)
		{
			if (state == 4)
			{
				this.currentNode = this.shopList[index];
			}
			else
			{
				this.currentNode = this.packList[index];
			}
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00088D70 File Offset: 0x00086F70
		public void InitAmountPrice(int state)
		{
			this.amount = 0;
			this.price = 0;
			this.max = 99;
			if (this.currentNode != null)
			{
				if (this.currentNode._ItemDataNode.m_iItemStack == 1)
				{
					this.max = 1;
				}
				for (int i = 0; i < this.packList.Count; i++)
				{
					if (this.packList[i]._ItemDataNode.m_iItemID == this.currentNode._ItemDataNode.m_iItemID)
					{
						if (state == 4)
						{
							this.max -= this.packList[i].m_iAmount;
						}
						else
						{
							this.max = this.packList[i].m_iAmount;
						}
					}
				}
				if (GameGlobal.m_bDLCMode && this.currentNode._ItemDataNode.m_iItemStack == 2)
				{
					this.max = 1;
				}
				this.max = Mathf.Clamp(this.max, 0, 99);
			}
			this.setInputView.Invoke(this.amount, this.price, this.max);
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00088E9C File Offset: 0x0008709C
		public void ComplementAdd(int state)
		{
			if (this.amount != this.max)
			{
				if (state == 4)
				{
					this.price += (int)((float)this.currentNode._ItemDataNode.m_iItemBuy * this.m_BuyMagnification);
				}
				else
				{
					this.price += (int)((float)this.currentNode._ItemDataNode.m_iItemSell * this.m_SellMagnification);
				}
			}
			this.amount++;
			if (this.amount >= this.max)
			{
				this.amount = this.max;
			}
			this.setInputView.Invoke(this.amount, this.price, this.max);
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00088F5C File Offset: 0x0008715C
		public void ComplementLess(int state)
		{
			if (this.amount != 0)
			{
				if (state == 4)
				{
					this.price -= (int)((float)this.currentNode._ItemDataNode.m_iItemBuy * this.m_BuyMagnification);
				}
				else
				{
					this.price -= (int)((float)this.currentNode._ItemDataNode.m_iItemSell * this.m_SellMagnification);
				}
			}
			this.amount--;
			if (this.amount <= 0)
			{
				this.amount = 0;
			}
			this.setInputView.Invoke(this.amount, this.price, this.max);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0008900C File Offset: 0x0008720C
		public void CheckOut(int state)
		{
			if (this.amount <= 0)
			{
				return;
			}
			if (state == 4)
			{
				if (this.packMoney < this.price)
				{
					return;
				}
				this.price = -this.price;
				if (this.currentNode._ItemDataNode.m_iItemStack == 1)
				{
					Game.StoreData.AddChangesStoreData(this.Storedata, this.currentNode.ItemID);
				}
				if (GameGlobal.m_bDLCMode && this.currentNode._ItemDataNode.m_iItemStack == 2)
				{
					for (int i = 0; i < Game.DLCShopInfo.m_ItemList.Count; i++)
					{
						if (Game.DLCShopInfo.m_ItemList[i] == this.currentNode.ItemID)
						{
							Game.DLCShopInfo.m_ItemList.RemoveAt(i);
							break;
						}
					}
				}
				BackpackStatus.m_Instance.AddPackItem(this.currentNode.ItemID, this.amount, true);
			}
			else
			{
				BackpackStatus.m_Instance.LessPackItem(this.currentNode.ItemID, this.amount, null);
			}
			BackpackStatus.m_Instance.ChangeMoney(this.price);
			this.UpdateShopData(false);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0000AA18 File Offset: 0x00008C18
		public int GetCount(int state)
		{
			if (state == 3)
			{
				return this.packList.Count;
			}
			return this.shopList.Count;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0000AA38 File Offset: 0x00008C38
		public int GetShowAmount()
		{
			return this.showAmount;
		}

		// Token: 0x0400130B RID: 4875
		public Action<ItemData, int> setView;

		// Token: 0x0400130C RID: 4876
		public Action<int, int> complementAmount;

		// Token: 0x0400130D RID: 4877
		public Action<int> setMoneyView;

		// Token: 0x0400130E RID: 4878
		public Action<TipData> setItemTipView;

		// Token: 0x0400130F RID: 4879
		public Action<int, int, int> setInputView;

		// Token: 0x04001310 RID: 4880
		public Action initView;

		// Token: 0x04001311 RID: 4881
		public Action<int, bool> setMouseActive;

		// Token: 0x04001312 RID: 4882
		private GameObject go_player;

		// Token: 0x04001313 RID: 4883
		private GameObject target;

		// Token: 0x04001314 RID: 4884
		private List<BackpackNewDataNode> packList = new List<BackpackNewDataNode>();

		// Token: 0x04001315 RID: 4885
		private List<StoreItemNode> shopItemNodeList = new List<StoreItemNode>();

		// Token: 0x04001316 RID: 4886
		private StoreDataNode Storedata;

		// Token: 0x04001317 RID: 4887
		private List<BackpackNewDataNode> shopList = new List<BackpackNewDataNode>();

		// Token: 0x04001318 RID: 4888
		private int showAmount = 16;

		// Token: 0x04001319 RID: 4889
		private int packMoney;

		// Token: 0x0400131A RID: 4890
		private int amount;

		// Token: 0x0400131B RID: 4891
		private int price;

		// Token: 0x0400131C RID: 4892
		private BackpackNewDataNode currentNode;

		// Token: 0x0400131D RID: 4893
		private int currentState;

		// Token: 0x0400131E RID: 4894
		private int max;

		// Token: 0x0400131F RID: 4895
		private float m_SellMagnification = 1f;

		// Token: 0x04001320 RID: 4896
		private float m_BuyMagnification = 1f;

		// Token: 0x020002F7 RID: 759
		private enum State
		{
			// Token: 0x04001322 RID: 4898
			Sell = 3,
			// Token: 0x04001323 RID: 4899
			Buy
		}
	}
}
