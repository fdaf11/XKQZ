using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000308 RID: 776
	// 草药UI
	public class UIHerbs : UILayer
	{
		// Token: 0x06001083 RID: 4227 RVA: 0x0008E754 File Offset: 0x0008C954
		protected override void Awake()
		{
			this.m_BtnHerbsList = new List<Control>();					// 使用？
			this.m_ItemIconNodeList = new List<HerbsIconNode>();		// 图像点
			this.m_HoleIconList = new List<Control>();					// 这个啥图像？
			this.m_HoeIconList = new List<Control>();					// 锄头
			this.m_GetItemIconList = new List<Control>();				// ItemIcon?
			this.m_LabelItemAmountList = new List<Control>();			// 标签
			this.m_GetItemIconBGList = new List<Control>();				// ItemBG
			this.m_LabelNewNumericalList = new List<Control>();			// 标签数值
			this.m_LabelOldNumericalList = new List<Control>();			// 旧标签数值
			this.m_HerbsItemNodeList = new List<HerbsItemNode>();		// 草药Node
			this.m_HerbsRewardNodeList = new List<HerbsRewardNode>();	// 奖励节点
			this.m_HerbsScaleList = new List<Control>();				// 刻度？缩放？
			base.Awake();
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0000AC91 File Offset: 0x00008E91
		private void Start()
		{
			this.m_bStart = false;
			this.m_bGetItem = false;
			this.m_iOneSelect = -1;
			this.m_iRightCount = 0;
			this.m_iIndex = -1;
			this.m_LabelExplanation.Text = Game.StringTable.GetString(302003);
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0000ACD0 File Offset: 0x00008ED0
		public void GameOpen()
		{
			this.m_Group.GameObject.SetActive(true);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0008E7EC File Offset: 0x0008C9EC
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIHerbs.<>f__switch$mapB == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(24);
					dictionary.Add("Group", 0);
					dictionary.Add("GameExplanation", 1);
					dictionary.Add("LabelExplanation", 2);
					dictionary.Add("BtnStart", 3);
					dictionary.Add("BarTime", 4);
					dictionary.Add("BtnLeave", 5);
					dictionary.Add("BtnHerbs", 6);
					dictionary.Add("ItemIcon", 7);
					dictionary.Add("HoleIcon", 8);
					dictionary.Add("HoeIcon", 9);
					dictionary.Add("HerbsScale", 10);
					dictionary.Add("EndBackground", 11);
					dictionary.Add("BtnClose", 12);
					dictionary.Add("GetItemIcon", 13);
					dictionary.Add("LabelItemAmount", 14);
					dictionary.Add("GetItemIconBG", 15);
					dictionary.Add("LabelNewNumerical", 16);
					dictionary.Add("LabelOldNumerical", 17);
					dictionary.Add("UpIcon1", 18);
					dictionary.Add("UpIcon2", 19);
					dictionary.Add("UpIcon3", 20);
					dictionary.Add("UpIcon4", 21);
					dictionary.Add("ItemNameBase", 22);
					dictionary.Add("LabelItemName", 23);
					UIHerbs.<>f__switch$mapB = dictionary;
				}
				int num;
				if (UIHerbs.<>f__switch$mapB.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_GameExplanation = sender;
						break;
					case 2:
						this.m_LabelExplanation = sender;
						break;
					case 3:
						this.m_BtnStart = sender;
						this.m_BtnStart.OnClick += this.BtnStartOnClick;
						break;
					case 4:
						this.m_BarTime = sender;
						break;
					case 5:
						this.m_BtnLeave = sender;
						this.m_BtnLeave.OnClick += this.BtnLeaveOnClick;
						break;
					case 6:
					{
						Control control = sender;
						control.OnClick += this.BtnHerbsOnClick;
						this.m_BtnHerbsList.Add(control);
						break;
					}
					case 7:
					{
						HerbsIconNode herbsIconNode = new HerbsIconNode();
						herbsIconNode.m_ItemIcon = sender;
						herbsIconNode.m_iIndex = herbsIconNode.m_ItemIcon.GetComponent<ImageData>().m_iIndex;
						this.m_ItemIconNodeList.Add(herbsIconNode);
						break;
					}
					case 8:
					{
						Control control = sender;
						this.m_HoleIconList.Add(control);
						break;
					}
					case 9:
					{
						Control control = sender;
						this.m_HoeIconList.Add(control);
						break;
					}
					case 10:
					{
						Control control = sender;
						this.m_HerbsScaleList.Add(control);
						break;
					}
					case 11:
						this.m_EndBackground = sender;
						break;
					case 12:
						this.m_BtnClose = sender;
						this.m_BtnClose.OnClick += this.BtnCloseOnClick;
						break;
					case 13:
					{
						Control control = sender;
						this.m_GetItemIconList.Add(control);
						break;
					}
					case 14:
					{
						Control control = sender;
						this.m_LabelItemAmountList.Add(control);
						break;
					}
					case 15:
					{
						Control control = sender;
						control.OnHover += this.GetItemIconBGOnHover;
						this.m_GetItemIconBGList.Add(control);
						break;
					}
					case 16:
					{
						Control control = sender;
						this.m_LabelNewNumericalList.Add(control);
						break;
					}
					case 17:
					{
						Control control = sender;
						this.m_LabelOldNumericalList.Add(control);
						break;
					}
					case 18:
						this.m_UpIcon1 = sender;
						break;
					case 19:
						this.m_UpIcon2 = sender;
						break;
					case 20:
						this.m_UpIcon3 = sender;
						break;
					case 21:
						this.m_UpIcon4 = sender;
						break;
					case 22:
						this.m_ItemNameBase = sender;
						break;
					case 23:
						this.m_LabelItemName = sender;
						break;
					}
				}
			}
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0000ACE3 File Offset: 0x00008EE3
		private void BtnStartOnClick(GameObject go)
		{
			this.m_GameExplanation.GameObject.SetActive(false);
			this.SetHerbsGameStart();
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0008EC34 File Offset: 0x0008CE34
		public void SetHerbsGameStart()
		{
			int iValue = 0;
			List<HerbsItemNode> list = new List<HerbsItemNode>();
			List<HerbsItemNode> list2 = new List<HerbsItemNode>();
			for (int i = 0; i < this.m_iProbability.Length; i++)
			{
				list.Clear();
				if (i == 5)
				{
					list = Game.HerbsData.GetHerbsItemGroupList(2, iValue);
				}
				else
				{
					list = Game.HerbsData.GetHerbsItemGroupList(i, iValue);
				}
				for (int j = 0; j < this.m_iProbability[i]; j++)
				{
					int num = Random.Range(0, list.Count);
					list2.Add(list[num]);
					list.RemoveAt(num);
				}
				if (this.m_iProbability2[i] != 0)
				{
					int num2 = this.m_iProbability2[i] * 2 / list2.Count;
					for (int k = 0; k < list2.Count; k++)
					{
						for (int l = 0; l < num2; l++)
						{
							this.m_HerbsItemNodeList.Add(list2[k]);
						}
					}
					list2.Clear();
				}
			}
			for (int i = 0; i < this.m_ItemIconNodeList.Count; i++)
			{
				int num = Random.Range(0, this.m_HerbsItemNodeList.Count);
				Texture2D texture2D = Game.g_Item.Load("2dtexture/gameui/item/" + this.m_HerbsItemNodeList[num].m_strImageID) as Texture2D;
				if (texture2D != null)
				{
					this.m_ItemIconNodeList[i].m_ItemIcon.GetComponent<UITexture>().mainTexture = texture2D;
				}
				else
				{
					Debug.LogError("抓不到道具圖");
				}
				this.m_ItemIconNodeList[i].m_iItemID = this.m_HerbsItemNodeList[num].m_iItemID;
				this.m_ItemIconNodeList[i].m_iAmount = this.m_HerbsItemNodeList[num].m_iAmount;
				this.m_HerbsItemNodeList.RemoveAt(num);
			}
			this.m_HerbsItemNodeList.Clear();
			this.m_fOverTime = Time.time + 60f;
			this.m_fOverTime = this.m_fOverTime;
			this.m_bStart = true;
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0008EE60 File Offset: 0x0008D060
		private void BtnHerbsOnClick(GameObject go)
		{
			if (!this.m_bStart || this.m_bCheck)
			{
				return;
			}
			this.m_bCheck = true;
			this.m_iIndex = go.GetComponent<BtnData>().m_iBtnID;
			for (int i = 0; i < this.m_HoeIconList.Count; i++)
			{
				if (this.m_HoeIconList[i].GetComponent<ImageData>().m_iIndex == this.m_iIndex)
				{
					this.m_HoeIconList[i].GameObject.SetActive(true);
					this.m_HoeIconList[i].GetComponent<TweenRotation>().ResetToBeginning();
					this.m_HoeIconList[i].GetComponent<TweenRotation>().PlayForward();
					break;
				}
			}
			go.GetComponent<TweenScale>().ResetToBeginning();
			go.GetComponent<TweenScale>().PlayForward();
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0008EF38 File Offset: 0x0008D138
		public void EndHoeAni()
		{
			for (int i = 0; i < this.m_HoeIconList.Count; i++)
			{
				if (this.m_HoeIconList[i].GetComponent<ImageData>().m_iIndex == this.m_iIndex)
				{
					this.m_HoeIconList[i].GameObject.SetActive(false);
					break;
				}
			}
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0008EFA0 File Offset: 0x0008D1A0
		public void PlayResult()
		{
			for (int i = 0; i < this.m_BtnHerbsList.Count; i++)
			{
				if (this.m_BtnHerbsList[i].GetComponent<BtnData>().m_iBtnID == this.m_iIndex)
				{
					this.m_BtnHerbsList[i].GameObject.SetActive(false);
				}
			}
			for (int j = 0; j < this.m_ItemIconNodeList.Count; j++)
			{
				if (this.m_ItemIconNodeList[j].m_ItemIcon.GetComponent<ImageData>().m_iIndex == this.m_iIndex)
				{
					if (this.m_iOneSelect == -1)
					{
						this.m_iOneSelect = this.m_iIndex;
					}
					else
					{
						this.m_bCheckItem = true;
					}
					this.m_ItemIconNodeList[j].m_ItemIcon.GameObject.SetActive(true);
					this.m_ItemIconNodeList[j].m_ItemIcon.GetComponent<TweenPosition>().ResetToBeginning();
					this.m_ItemIconNodeList[j].m_ItemIcon.GetComponent<TweenPosition>().PlayForward();
					this.m_ItemIconNodeList[j].m_ItemIcon.GetComponent<TweenScale>().ResetToBeginning();
					this.m_ItemIconNodeList[j].m_ItemIcon.GetComponent<TweenScale>().PlayForward();
					break;
				}
			}
			for (int k = 0; k < this.m_HoleIconList.Count; k++)
			{
				if (this.m_HoleIconList[k].GetComponent<ImageData>().m_iIndex == this.m_iIndex)
				{
					this.m_HoleIconList[k].GameObject.SetActive(true);
					break;
				}
			}
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0008F150 File Offset: 0x0008D350
		public void CheckItem()
		{
			if (this.m_bCheckItem)
			{
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < this.m_ItemIconNodeList.Count; i++)
				{
					if (this.m_ItemIconNodeList[i].m_ItemIcon.GetComponent<ImageData>().m_iIndex == this.m_iIndex)
					{
						num = this.m_ItemIconNodeList[i].m_iItemID;
					}
					else if (this.m_ItemIconNodeList[i].m_ItemIcon.GetComponent<ImageData>().m_iIndex == this.m_iOneSelect)
					{
						num2 = this.m_ItemIconNodeList[i].m_iItemID;
					}
				}
				if (num == num2)
				{
					this.m_iRightCount++;
					this.m_bGetItem = true;
				}
				this.PlayAlphaOut();
			}
			else
			{
				this.m_bCheck = false;
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0008F238 File Offset: 0x0008D438
		private void RewardNodeCheck(int iIndex)
		{
			this.m_bGetItem = false;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this.m_ItemIconNodeList.Count; i++)
			{
				if (this.m_ItemIconNodeList[i].m_ItemIcon.GetComponent<ImageData>().m_iIndex == this.m_iIndex)
				{
					num = this.m_ItemIconNodeList[i].m_iItemID;
					num2 = this.m_ItemIconNodeList[i].m_iAmount;
				}
			}
			if (num == 0)
			{
				return;
			}
			for (int j = 0; j < this.m_HerbsRewardNodeList.Count; j++)
			{
				if (this.m_HerbsRewardNodeList[j].m_iItemID == num)
				{
					this.m_HerbsRewardNodeList[j].m_iAmount += num2 * 2;
					return;
				}
			}
			HerbsRewardNode herbsRewardNode = new HerbsRewardNode();
			herbsRewardNode.m_iAmount = num2 * 2;
			herbsRewardNode.m_iItemID = num;
			herbsRewardNode.m_strItemName = Game.ItemData.GetItemName(num);
			this.m_HerbsRewardNodeList.Add(herbsRewardNode);
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0008F344 File Offset: 0x0008D544
		public void PlayAlphaOut()
		{
			for (int i = 0; i < this.m_ItemIconNodeList.Count; i++)
			{
				if (this.m_ItemIconNodeList[i].m_ItemIcon.GetComponent<ImageData>().m_iIndex == this.m_iIndex)
				{
					int iItemID = this.m_ItemIconNodeList[i].m_iItemID;
					this.m_ItemIconNodeList[i].m_ItemIcon.GameObject.SetActive(false);
					this.m_ItemIconNodeList[i].m_ItemIcon.GetComponent<UITexture>().alpha = 1f;
				}
				else if (this.m_ItemIconNodeList[i].m_ItemIcon.GetComponent<ImageData>().m_iIndex == this.m_iOneSelect)
				{
					this.m_ItemIconNodeList[i].m_ItemIcon.GameObject.SetActive(false);
					this.m_ItemIconNodeList[i].m_ItemIcon.GetComponent<UITexture>().alpha = 1f;
				}
			}
			if (this.m_bGetItem)
			{
				this.RewardNodeCheck(this.m_iIndex);
			}
			else
			{
				for (int j = 0; j < this.m_BtnHerbsList.Count; j++)
				{
					if (this.m_HoleIconList[j].GetComponent<ImageData>().m_iIndex == this.m_iIndex)
					{
						this.m_HoleIconList[j].GameObject.SetActive(false);
					}
					if (this.m_HoleIconList[j].GetComponent<ImageData>().m_iIndex == this.m_iOneSelect)
					{
						this.m_HoleIconList[j].GameObject.SetActive(false);
					}
					if (this.m_BtnHerbsList[j].GetComponent<BtnData>().m_iBtnID == this.m_iIndex)
					{
						this.m_BtnHerbsList[j].GetComponent<TweenAlpha>().ResetToBeginning();
						this.m_BtnHerbsList[j].GetComponent<TweenAlpha>().Play();
						this.m_BtnHerbsList[j].GameObject.SetActive(true);
					}
					if (this.m_BtnHerbsList[j].GetComponent<BtnData>().m_iBtnID == this.m_iOneSelect)
					{
						this.m_BtnHerbsList[j].GetComponent<TweenAlpha>().ResetToBeginning();
						this.m_BtnHerbsList[j].GetComponent<TweenAlpha>().Play();
						this.m_BtnHerbsList[j].GameObject.SetActive(true);
					}
					if (this.m_HerbsScaleList[j].GetComponent<ImageData>().m_iIndex == this.m_iIndex)
					{
						this.m_HerbsScaleList[j].GetComponent<TweenScale>().ResetToBeginning();
						this.m_HerbsScaleList[j].GetComponent<TweenScale>().Play();
					}
					if (this.m_HerbsScaleList[j].GetComponent<ImageData>().m_iIndex == this.m_iOneSelect)
					{
						this.m_HerbsScaleList[j].GetComponent<TweenScale>().ResetToBeginning();
						this.m_HerbsScaleList[j].GetComponent<TweenScale>().Play();
					}
				}
			}
			this.m_iOneSelect = -1;
			this.m_bCheckItem = false;
			this.m_bCheck = false;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0000ACFC File Offset: 0x00008EFC
		private void BtnLeaveOnClick(GameObject go)
		{
			this.EndGame();
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0000AD04 File Offset: 0x00008F04
		private void EndGame()
		{
			if (this.m_bEnd)
			{
				return;
			}
			this.m_bEnd = true;
			this.m_EndBackground.GameObject.SetActive(true);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0000AD2A File Offset: 0x00008F2A
		private void BtnCloseOnClick(GameObject go)
		{
			this.ReSetData();
			this.m_Group.GameObject.SetActive(false);
			Game.PrefabSmallGameEnd();
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0008F66C File Offset: 0x0008D86C
		private void ReSetData()
		{
			this.m_bEnd = false;
			this.m_bStart = false;
			this.m_bGetItem = false;
			this.m_iOneSelect = -1;
			this.m_iRightCount = 0;
			this.m_iIndex = -1;
			this.m_GameExplanation.GameObject.SetActive(true);
			this.m_EndBackground.GameObject.SetActive(false);
			this.m_HerbsRewardNodeList.Clear();
			for (int i = 0; i < this.m_BtnHerbsList.Count; i++)
			{
				this.m_BtnHerbsList[i].GameObject.SetActive(true);
			}
			for (int i = 0; i < this.m_HoleIconList.Count; i++)
			{
				this.m_HoleIconList[i].GameObject.SetActive(false);
			}
			for (int i = 0; i < this.m_GetItemIconList.Count; i++)
			{
				this.m_GetItemIconList[i].GameObject.SetActive(false);
			}
			for (int i = 0; i < this.m_GetItemIconBGList.Count; i++)
			{
				this.m_GetItemIconBGList[i].GameObject.SetActive(false);
			}
			this.m_UpIcon1.GameObject.SetActive(false);
			this.m_UpIcon2.GameObject.SetActive(false);
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0008F7C0 File Offset: 0x0008D9C0
		private void GetItemIconBGOnHover(GameObject go, bool bHover)
		{
			if (bHover)
			{
				this.m_ItemNameBase.GameObject.SetActive(true);
				this.m_ItemNameBase.GameObject.transform.localPosition = new Vector3(go.transform.localPosition.x - 28f, this.m_ItemNameBase.GameObject.transform.localPosition.y, this.m_ItemNameBase.GameObject.transform.localPosition.z);
				int iIndex = go.GetComponent<ImageData>().m_iIndex;
				this.m_LabelItemName.Text = this.m_HerbsRewardNodeList[iIndex].m_strItemName;
				int num = (int)this.m_LabelItemName.GetComponent<UILabel>().printedSize.x;
				this.m_ItemNameBase.GameObject.GetComponent<UITexture>().width = num + 60;
			}
			else
			{
				this.m_ItemNameBase.GameObject.SetActive(false);
			}
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0008F8C4 File Offset: 0x0008DAC4
		private void Update()
		{
			if (this.m_bStart)
			{
				if (this.m_fOverTime - Time.time > 0f)
				{
					float fillAmount = (this.m_fOverTime - Time.time) / 60f;
					this.m_BarTime.GetComponent<UISprite>().fillAmount = fillAmount;
				}
				else if (this.m_fOverTime - Time.time <= 0f)
				{
					this.m_bStart = false;
					this.EndGame();
				}
			}
		}

		// Token: 0x040013E1 RID: 5089
		private Control m_Group;

		// Token: 0x040013E2 RID: 5090
		private Control m_GameExplanation;

		// Token: 0x040013E3 RID: 5091
		private Control m_LabelExplanation;

		// Token: 0x040013E4 RID: 5092
		private Control m_BtnStart;

		// Token: 0x040013E5 RID: 5093
		private Control m_BarTime;

		// Token: 0x040013E6 RID: 5094
		private Control m_BtnLeave;

		// Token: 0x040013E7 RID: 5095
		private List<Control> m_HerbsScaleList;

		// Token: 0x040013E8 RID: 5096
		private List<Control> m_BtnHerbsList;

		// Token: 0x040013E9 RID: 5097
		private List<Control> m_HoleIconList;

		// Token: 0x040013EA RID: 5098
		private List<Control> m_HoeIconList;

		// Token: 0x040013EB RID: 5099
		private List<HerbsIconNode> m_ItemIconNodeList;

		// Token: 0x040013EC RID: 5100
		private Control m_EndBackground;

		// Token: 0x040013ED RID: 5101
		private Control m_BtnClose;

		// Token: 0x040013EE RID: 5102
		private Control m_UpIcon1;

		// Token: 0x040013EF RID: 5103
		private Control m_UpIcon2;

		// Token: 0x040013F0 RID: 5104
		private Control m_UpIcon3;

		// Token: 0x040013F1 RID: 5105
		private Control m_UpIcon4;

		// Token: 0x040013F2 RID: 5106
		private Control m_ItemNameBase;

		// Token: 0x040013F3 RID: 5107
		private Control m_LabelItemName;

		// Token: 0x040013F4 RID: 5108
		private List<Control> m_GetItemIconList;

		// Token: 0x040013F5 RID: 5109
		private List<Control> m_LabelItemAmountList;

		// Token: 0x040013F6 RID: 5110
		private List<Control> m_GetItemIconBGList;

		// Token: 0x040013F7 RID: 5111
		private List<Control> m_LabelNewNumericalList;

		// Token: 0x040013F8 RID: 5112
		private List<Control> m_LabelOldNumericalList;

		// Token: 0x040013F9 RID: 5113
		private List<HerbsItemNode> m_HerbsItemNodeList;

		// Token: 0x040013FA RID: 5114
		private List<HerbsRewardNode> m_HerbsRewardNodeList;

		// Token: 0x040013FB RID: 5115
		private int m_iIndex;

		// Token: 0x040013FC RID: 5116
		private int m_iOneSelect;

		// Token: 0x040013FD RID: 5117
		private int m_iRightCount;

		// Token: 0x040013FE RID: 5118
		private float m_fOverTime;

		// Token: 0x040013FF RID: 5119
		private bool m_bStart;

		// Token: 0x04001400 RID: 5120
		private bool m_bCheckItem;

		// Token: 0x04001401 RID: 5121
		private bool m_bCheck;

		// Token: 0x04001402 RID: 5122
		private bool m_bGetItem;

		// Token: 0x04001403 RID: 5123
		private int[] m_iProbability = new int[]
		{
			4,
			2,
			1,
			0,
			0,
			1
		};

		// Token: 0x04001404 RID: 5124
		private int[] m_iProbability2 = new int[]
		{
			16,
			6,
			2,
			0,
			0,
			1
		};

		// Token: 0x04001405 RID: 5125
		private bool m_bEnd;
	}
}
