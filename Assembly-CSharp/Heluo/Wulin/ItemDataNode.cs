using System;
using System.Collections.Generic;
using Heluo.Wulin.UI;

namespace Heluo.Wulin
{
	// Token: 0x02000209 RID: 521
	public class ItemDataNode
	{
		// Token: 0x06000A2D RID: 2605 RVA: 0x00008246 File Offset: 0x00006446
		public ItemDataNode()
		{
			this.m_ItmeEffectNodeList = new List<ItmeEffectNode>();
			this.m_strNpcLikeList = new List<NpcLikeNode>();
			this.m_UseLimitNodeList = new List<UseLimitNode>();
			this.m_CanUseiNpcIDList = new List<int>();
			this.m_NoUseNpcList = new List<int>();
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00056954 File Offset: 0x00054B54
		public int GetSkillID()
		{
			int result = 0;
			foreach (ItmeEffectNode itmeEffectNode in this.m_ItmeEffectNodeList)
			{
				if (itmeEffectNode.m_iItemType == 15)
				{
					result = itmeEffectNode.m_iRecoverType;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x000569C4 File Offset: 0x00054BC4
		public bool CheckUse(CharacterData _CharacterData)
		{
			bool flag = false;
			if (this.m_UseLimitNodeList.Count > 0)
			{
				foreach (UseLimitNode useLimitNode in this.m_UseLimitNodeList)
				{
					UseLimitType type = useLimitNode.m_Type;
					if (type != UseLimitType.MoreNpcProperty)
					{
						if (type == UseLimitType.LessNpcProperty)
						{
							flag = this.GreaterProperty(_CharacterData, useLimitNode, false);
						}
					}
					else
					{
						flag = this.GreaterProperty(_CharacterData, useLimitNode, true);
					}
					if (!flag)
					{
						return false;
					}
				}
				return flag;
			}
			return true;
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00056A7C File Offset: 0x00054C7C
		private bool GreaterProperty(CharacterData _CharacterData, UseLimitNode ULN, bool bMore)
		{
			int characterProperty = NPC.m_instance.getCharacterProperty(0, ULN.m_iInde, _CharacterData);
			int iValue = ULN.m_iValue;
			if (bMore)
			{
				return characterProperty >= iValue;
			}
			return characterProperty < iValue;
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00056AC8 File Offset: 0x00054CC8
		private void Log(int iMsgID)
		{
			string @string = Game.StringTable.GetString(iMsgID);
			Game.UI.Get<UIMapMessage>().SetMsg(@string);
		}

		// Token: 0x04000AB6 RID: 2742
		public int m_iItemID;

		// Token: 0x04000AB7 RID: 2743
		public string m_strItemName;

		// Token: 0x04000AB8 RID: 2744
		public int m_iItemKind;

		// Token: 0x04000AB9 RID: 2745
		public int m_iItemType;

		// Token: 0x04000ABA RID: 2746
		public string m_strIcon;

		// Token: 0x04000ABB RID: 2747
		public string m_strTip;

		// Token: 0x04000ABC RID: 2748
		public int m_iItemBuy;

		// Token: 0x04000ABD RID: 2749
		public int m_iItemSell;

		// Token: 0x04000ABE RID: 2750
		public int m_iItemStack;

		// Token: 0x04000ABF RID: 2751
		public int m_iMixStack;

		// Token: 0x04000AC0 RID: 2752
		public int m_iItemUse;

		// Token: 0x04000AC1 RID: 2753
		public int m_iUseTime;

		// Token: 0x04000AC2 RID: 2754
		public int m_iAddType;

		// Token: 0x04000AC3 RID: 2755
		public int m_iUseMsgID;

		// Token: 0x04000AC4 RID: 2756
		public List<int> m_NoUseNpcList;

		// Token: 0x04000AC5 RID: 2757
		public WeaponType m_AmsType;

		// Token: 0x04000AC6 RID: 2758
		public List<NpcLikeNode> m_strNpcLikeList;

		// Token: 0x04000AC7 RID: 2759
		public List<int> m_CanUseiNpcIDList;

		// Token: 0x04000AC8 RID: 2760
		public List<UseLimitNode> m_UseLimitNodeList;

		// Token: 0x04000AC9 RID: 2761
		public List<ItmeEffectNode> m_ItmeEffectNodeList;

		// Token: 0x04000ACA RID: 2762
		public string m_strStatusTip;

		// Token: 0x04000ACB RID: 2763
		public int m_iLock;

		// Token: 0x04000ACC RID: 2764
		public int mod_Guid;

		// Token: 0x0200020A RID: 522
		public enum ItemType
		{
			// Token: 0x04000ACE RID: 2766
			None,
			// Token: 0x04000ACF RID: 2767
			Weapon,
			// Token: 0x04000AD0 RID: 2768
			Arror,
			// Token: 0x04000AD1 RID: 2769
			Necklace,
			// Token: 0x04000AD2 RID: 2770
			Solution,
			// Token: 0x04000AD3 RID: 2771
			Mission,
			// Token: 0x04000AD4 RID: 2772
			TipsBook
		}

		// Token: 0x0200020B RID: 523
		public enum ItemKind
		{
			// Token: 0x04000AD6 RID: 2774
			None,
			// Token: 0x04000AD7 RID: 2775
			Herbs,
			// Token: 0x04000AD8 RID: 2776
			Mine,
			// Token: 0x04000AD9 RID: 2777
			Poison,
			// Token: 0x04000ADA RID: 2778
			Other
		}

		// Token: 0x0200020C RID: 524
		public enum ItemUseTime
		{
			// Token: 0x04000ADC RID: 2780
			AnyTime,
			// Token: 0x04000ADD RID: 2781
			Map,
			// Token: 0x04000ADE RID: 2782
			Battle,
			// Token: 0x04000ADF RID: 2783
			CantUse
		}
	}
}
