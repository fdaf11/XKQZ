using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000316 RID: 790
	public class UIMining : UILayer
	{
		// Token: 0x0600110B RID: 4363 RVA: 0x00092BF8 File Offset: 0x00090DF8
		protected override void Awake()
		{
			this.m_GophersIconList = new List<Control>();
			this.m_OreIconList = new List<Control>();
			this.m_SelectBoxList = new List<Control>();
			this.m_HitIconList = new List<Control>();
			this.m_MiningItemList = new List<MiningItemNode>();
			this.m_GetItemIDList = new List<GetMiningItem>();
			this.m_LabelAmountList = new List<Control>();
			this.m_RightWordList = new List<Control>();
			this.m_HammerIconList = new List<Control>();
			this.m_GetItemIconBGList = new List<Control>();
			this.m_GetItemIconList = new List<Control>();
			this.RightWord_Tween_ScaleList = new List<TweenScale>();
			this.RightWord_Tween_PositionList = new List<TweenPosition>();
			this.m_LabelNewNumericalList = new List<Control>();
			this.m_LabelOldNumericalList = new List<Control>();
			this.m_LabelItemAmountList = new List<Control>();
			base.Awake();
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00092CBC File Offset: 0x00090EBC
		private void Start()
		{
			this.ReSetData();
			Game.UI.Root.GetComponentInChildren<UICamera>().cancelKey0 = 0;
			Game.UI.Root.GetComponentInChildren<UICamera>().cancelKey1 = 0;
			Game.UI.Root.GetComponentInChildren<UICamera>().submitKey0 = 0;
			Game.UI.Root.GetComponentInChildren<UICamera>().submitKey1 = 0;
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00092D24 File Offset: 0x00090F24
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIMining.<>f__switch$map12 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(25);
					dictionary.Add("Group", 0);
					dictionary.Add("GophersIcon", 1);
					dictionary.Add("BtnLeave", 2);
					dictionary.Add("BarTime", 3);
					dictionary.Add("SelectBox", 4);
					dictionary.Add("HitIcon", 5);
					dictionary.Add("LabelAmount", 6);
					dictionary.Add("GameExplanation", 7);
					dictionary.Add("LabelExplanation", 8);
					dictionary.Add("RightWord", 9);
					dictionary.Add("BtnStart", 10);
					dictionary.Add("EndBackground", 11);
					dictionary.Add("BtnClose", 12);
					dictionary.Add("HammerIcon", 13);
					dictionary.Add("UpIcon1", 14);
					dictionary.Add("UpIcon2", 15);
					dictionary.Add("UpIcon3", 16);
					dictionary.Add("GetItemIcon", 17);
					dictionary.Add("GetItemIconBG", 18);
					dictionary.Add("LabelNewNumerical", 19);
					dictionary.Add("LabelOldNumerical", 20);
					dictionary.Add("LabelItemAmount", 21);
					dictionary.Add("ItemNameBase", 22);
					dictionary.Add("LabelItemName", 23);
					dictionary.Add("LabelAddEye", 24);
					UIMining.<>f__switch$map12 = dictionary;
				}
				int num;
				if (UIMining.<>f__switch$map12.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
					{
						Control control = sender;
						control.OnPress += this.GophersIconOnPress;
						control.OnHover += this.GophersIconOnHover;
						this.m_GophersIconList.Add(control);
						break;
					}
					case 2:
						this.m_BtnLeave = sender;
						this.m_BtnLeave.OnClick += this.BtnLeaveOnClick;
						break;
					case 3:
						this.m_BarTime = sender;
						break;
					case 4:
					{
						Control control = sender;
						this.m_SelectBoxList.Add(control);
						break;
					}
					case 5:
					{
						Control control = sender;
						this.m_HitIconList.Add(control);
						break;
					}
					case 6:
					{
						Control control = sender;
						this.m_LabelAmountList.Add(control);
						break;
					}
					case 7:
						this.m_GameExplanation = sender;
						break;
					case 8:
						this.m_LabelExplanation = sender;
						break;
					case 9:
					{
						Control control = sender;
						this.m_RightWordList.Add(control);
						this.RightWord_Tween_ScaleList.Add(sender.GetComponent<TweenScale>());
						this.RightWord_Tween_PositionList.Add(sender.GetComponent<TweenPosition>());
						break;
					}
					case 10:
						this.m_BtnStart = sender;
						this.m_BtnStart.OnClick += this.BtnStartOnClick;
						break;
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
						this.m_HammerIconList.Add(control);
						break;
					}
					case 14:
						this.m_UpIcon1 = sender;
						break;
					case 15:
						this.m_UpIcon2 = sender;
						break;
					case 16:
						this.m_UpIcon3 = sender;
						break;
					case 17:
					{
						Control control = sender;
						this.m_GetItemIconList.Add(control);
						break;
					}
					case 18:
					{
						Control control = sender;
						control.OnHover += this.GetItemIconBGOnHover;
						this.m_GetItemIconBGList.Add(control);
						break;
					}
					case 19:
					{
						Control control = sender;
						this.m_LabelNewNumericalList.Add(control);
						break;
					}
					case 20:
					{
						Control control = sender;
						this.m_LabelOldNumericalList.Add(control);
						break;
					}
					case 21:
					{
						Control control = sender;
						this.m_LabelItemAmountList.Add(control);
						break;
					}
					case 22:
						this.m_ItemNameBase = sender;
						break;
					case 23:
						this.m_LabelItemName = sender;
						break;
					case 24:
						this.m_LabelAddEye = sender;
						break;
					}
				}
			}
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0000B2BA File Offset: 0x000094BA
		public void GameOpen()
		{
			this.m_Group.GameObject.SetActive(true);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x000931A8 File Offset: 0x000913A8
		public void BtnStartOnClick(GameObject go)
		{
			this.m_GameExplanation.GameObject.SetActive(false);
			this.m_BtnStart.GameObject.SetActive(false);
			this.CheckOverTiem();
			this.m_bFirst = true;
			this.m_fFirstTime = Time.time;
			this.m_bHit = true;
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x000931F8 File Offset: 0x000913F8
		private void GophersIconOnPress(GameObject go, bool bDown)
		{
			if (this.m_bCheck)
			{
				return;
			}
			if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonDown(1))
			{
				return;
			}
			if (bDown)
			{
				this.m_bHit = false;
				this.m_iIndex = go.GetComponent<BtnData>().m_iBtnID;
				for (int i = 0; i < this.m_HammerIconList.Count; i++)
				{
					if (this.m_HammerIconList[i].GetComponent<ImageData>().m_iIndex == this.m_iIndex)
					{
						this.m_HammerIconList[i].GameObject.SetActive(true);
						this.m_HammerIconList[i].GetComponent<TweenRotation>().ResetToBeginning();
						this.m_HammerIconList[i].GetComponent<TweenRotation>().Play();
					}
					this.m_GophersIconList[i].GameObject.GetComponent<TweenPosition>().enabled = false;
					if (this.m_GophersIconList[i].GetComponent<BtnData>().m_iBtnID == this.m_iIndex)
					{
						this.m_GophersIconList[i].GetComponent<TweenScale>().ResetToBeginning();
						this.m_GophersIconList[i].GetComponent<TweenScale>().Play(true);
						this.m_GophersIconList[i].GetComponent<UIPlaySound>().Play();
					}
				}
				this.m_bCheck = true;
				if (this.m_iIndex == this.m_iAnswer)
				{
					this.m_iAllGet++;
					this.m_iGet++;
					GetMiningItem getMiningItem = this.CheckGetItemID(this.m_MiningItemList[this.m_iAnswerIndex].m_iItemID, this.m_MiningItemList[this.m_iAnswerIndex].m_iType);
					getMiningItem.m_iAmount += this.m_MiningItemList[this.m_iAnswerIndex].m_iGetAmount;
					for (int j = 0; j < this.m_GetItemIDList.Count; j++)
					{
						for (int k = 0; k < this.m_LabelAmountList.Count; k++)
						{
							int num = getMiningItem.m_iType;
							if (num == 10)
							{
								num = 9;
							}
							int num3;
							if (num == 9)
							{
								int num2 = 0;
								for (int l = 0; l < this.m_GetItemIDList.Count; l++)
								{
									if (this.m_GetItemIDList[l].m_iType == 9 || this.m_GetItemIDList[l].m_iType == 10)
									{
										num2 += this.m_GetItemIDList[l].m_iAmount;
									}
								}
								num3 = num2;
							}
							else
							{
								num3 = getMiningItem.m_iAmount;
							}
							if (this.m_LabelAmountList[k].GetComponent<LabelData>().m_iIndex == num)
							{
								int num4 = num3 % 10;
								int num5 = num3 / 10;
								string text = string.Empty;
								if (num5 != 0)
								{
									if (num3 >= 10 && num3 < 20)
									{
										num5 = 10;
									}
									if (num3 == 10)
									{
										text = this.m_astrNumList[num5];
									}
									else
									{
										text = this.m_astrNumList[num5] + this.m_astrNumList[num4];
									}
								}
								else
								{
									text = this.m_astrNumList[num4];
								}
								this.m_LabelAmountList[k].Text = text;
							}
						}
					}
				}
				else
				{
					this.m_iGet = 0;
					this.m_fOverTime -= 2f;
				}
				this.SetBatterWord();
			}
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00093570 File Offset: 0x00091770
		private void SetBatterWord()
		{
			if (this.m_iOldGet == this.m_iGet)
			{
				return;
			}
			this.m_iOldGet = this.m_iGet;
			int num = this.m_iGet % 10;
			int num2 = this.m_iGet / 10;
			for (int i = 0; i < this.m_RightWordList.Count; i++)
			{
				int num3 = -1;
				if (this.m_RightWordList[i].GetComponent<LabelData>().m_iIndex == 0)
				{
					num3 = num;
				}
				else if (this.m_RightWordList[i].GetComponent<LabelData>().m_iIndex == 1)
				{
					num3 = num2;
				}
				this.m_RightWordList[i].GetComponent<UISprite>().spriteName = this.m_aWordIconName[num3];
			}
			for (int j = 0; j < this.RightWord_Tween_ScaleList.Count; j++)
			{
				this.RightWord_Tween_ScaleList[j].ResetToBeginning();
				this.RightWord_Tween_ScaleList[j].Play(true);
				this.RightWord_Tween_PositionList[j].ResetToBeginning();
				this.RightWord_Tween_PositionList[j].Play(true);
			}
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00093698 File Offset: 0x00091898
		public void OnHitAniFinish()
		{
			for (int i = 0; i < this.m_HammerIconList.Count; i++)
			{
				this.m_GophersIconList[i].GameObject.GetComponent<TweenPosition>().enabled = true;
			}
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x000936E0 File Offset: 0x000918E0
		public void ShowHitIcon()
		{
			for (int i = 0; i < this.m_HitIconList.Count; i++)
			{
				if (this.m_HitIconList[i].GetComponent<ImageData>().m_iIndex == this.m_iIndex)
				{
					this.m_HitIconList[i].GameObject.SetActive(true);
				}
			}
			for (int i = 0; i < this.m_HammerIconList.Count; i++)
			{
				if (this.m_HammerIconList[i].GetComponent<ImageData>().m_iIndex == this.m_iIndex)
				{
					this.m_HammerIconList[i].GameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00093798 File Offset: 0x00091998
		private GetMiningItem CheckGetItemID(int iID, int iType)
		{
			for (int i = 0; i < this.m_GetItemIDList.Count; i++)
			{
				if (this.m_GetItemIDList[i].m_iItemID == iID)
				{
					return this.m_GetItemIDList[i];
				}
			}
			GetMiningItem getMiningItem = new GetMiningItem();
			getMiningItem.m_iItemID = iID;
			getMiningItem.m_iType = iType;
			getMiningItem.m_strItemName = Game.ItemData.GetItemName(iID);
			this.m_GetItemIDList.Add(getMiningItem);
			return getMiningItem;
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0009381C File Offset: 0x00091A1C
		private void GophersIconOnHover(GameObject go, bool bHover)
		{
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			for (int i = 0; i < this.m_SelectBoxList.Count; i++)
			{
				if (bHover)
				{
					if (iBtnID == this.m_SelectBoxList[i].GetComponent<ImageData>().m_iIndex)
					{
						this.m_SelectBoxList[i].GameObject.SetActive(true);
					}
					else
					{
						this.m_SelectBoxList[i].GameObject.SetActive(false);
					}
				}
				else
				{
					this.m_SelectBoxList[i].GameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x000938C4 File Offset: 0x00091AC4
		private void PlayGophersIcon()
		{
			this.m_bWaitNext = true;
			this.m_iAnswerIndex = Random.Range(0, this.m_MiningItemList.Count);
			if (this.m_MiningItemList[this.m_iAnswerIndex].m_iBatter > this.m_iGet)
			{
				this.m_iAnswerIndex = this.CheckAnswerIndex();
			}
			this.m_iAnswer = Random.Range(0, this.m_GophersIconList.Count);
			for (int i = 0; i < this.m_GophersIconList.Count; i++)
			{
				this.m_HitIconList[i].GameObject.SetActive(false);
				if (this.m_GophersIconList[i].GetComponent<BtnData>().m_iBtnID == this.m_iAnswer)
				{
					this.m_GophersIconList[i].GetComponent<UISprite>().spriteName = this.m_MiningItemList[this.m_iAnswerIndex].m_strImageID;
				}
				else
				{
					this.m_GophersIconList[i].GetComponent<UISprite>().spriteName = "molehitting_mole00";
				}
				this.m_GophersIconList[i].GetComponent<TweenPosition>().ResetToBeginning();
				this.m_GophersIconList[i].GetComponent<TweenPosition>().Play();
			}
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00093A00 File Offset: 0x00091C00
		private int CheckAnswerIndex()
		{
			int num = Random.Range(0, this.m_MiningItemList.Count);
			if (this.m_MiningItemList[num].m_iBatter == 0)
			{
				return num;
			}
			if (this.m_iGet >= this.m_MiningItemList[num].m_iBatter)
			{
				return num;
			}
			return this.CheckAnswerIndex();
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00093A60 File Offset: 0x00091C60
		private void CheckOverTiem()
		{
			int num = 0;
			if (num <= 10)
			{
				this.m_iExp = 5;
			}
			else if (num > 10 && num <= 20)
			{
				this.m_iExp = 5;
			}
			else if (num > 20 && num <= 30)
			{
				this.m_iExp = 5;
			}
			else if (num > 30 && num <= 40)
			{
				this.m_iExp = 5;
			}
			else if (num > 40 && num <= 50)
			{
				this.m_iExp = 5;
			}
			else if (num > 50 && num <= 60)
			{
				this.m_iExp = 5;
			}
			else if (num > 60 && num <= 70)
			{
				this.m_iExp = 5;
			}
			else if (num > 70 && num <= 80)
			{
				this.m_iExp = 5;
			}
			else if (num > 80 && num <= 90)
			{
				this.m_iExp = 5;
			}
			else if (num > 90 && num <= 100)
			{
				this.m_iExp = 5;
			}
			else if (num > 100 && num <= 110)
			{
				this.m_iExp = 5;
			}
			else if (num > 110 && num <= 120)
			{
				this.m_iExp = 5;
			}
			List<MiningItemNode> miningItemList = Game.MiningData.GetMiningItemList();
			this.m_MiningItemList.Clear();
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00093BE8 File Offset: 0x00091DE8
		private void BtnLeaveOnClick(GameObject go)
		{
			for (int i = 0; i < this.m_GophersIconList.Count; i++)
			{
				this.m_GophersIconList[i].GetComponent<TweenPosition>().ResetToBeginning();
			}
			this.m_bStart = false;
			this.m_bLeave = true;
			this.m_bNext = false;
			this.SetEnd();
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00093C44 File Offset: 0x00091E44
		public void SetNext()
		{
			this.m_bWaitNext = false;
			if (this.m_bLeave)
			{
				return;
			}
			if (this.m_bHit)
			{
				this.m_iGet = 0;
				this.SetBatterWord();
			}
			if (!this.m_bStart)
			{
				this.SetEnd();
				return;
			}
			for (int i = 0; i < this.m_SelectBoxList.Count; i++)
			{
				this.m_SelectBoxList[i].GameObject.SetActive(false);
			}
			this.m_bHit = true;
			this.m_bNext = true;
			this.m_bCheck = false;
			this.m_fWaitTime = Time.time;
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00093CE4 File Offset: 0x00091EE4
		private void SetEnd()
		{
			if (this.m_bEnd)
			{
				return;
			}
			this.m_bEnd = true;
			this.m_EndBackground.GameObject.SetActive(true);
			List<GetMiningItem> list = new List<GetMiningItem>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < this.m_GetItemIDList.Count; i++)
			{
				if (this.m_GetItemIDList[i].m_iItemID >= 100001 && this.m_GetItemIDList[i].m_iItemID <= 100009)
				{
					if (!list2.Contains(this.m_GetItemIDList[i].m_iItemID))
					{
						list2.Add(this.m_GetItemIDList[i].m_iItemID);
					}
				}
			}
			if (list2.Count >= 9)
			{
				Game.Achievement.SetMinningAllGet();
			}
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00093DCC File Offset: 0x00091FCC
		private void GetItemIconBGOnHover(GameObject go, bool bHover)
		{
			if (bHover)
			{
				this.m_ItemNameBase.GameObject.SetActive(true);
				this.m_ItemNameBase.GameObject.transform.localPosition = new Vector3(go.transform.localPosition.x - 28f, this.m_ItemNameBase.GameObject.transform.localPosition.y, this.m_ItemNameBase.GameObject.transform.localPosition.z);
				int iIndex = go.GetComponent<ImageData>().m_iIndex;
				this.m_LabelItemName.Text = this.m_GetItemIDList[iIndex].m_strItemName;
				int num = (int)this.m_LabelItemName.GetComponent<UILabel>().printedSize.x;
				this.m_ItemNameBase.GameObject.GetComponent<UITexture>().width = num + 60;
			}
			else
			{
				this.m_ItemNameBase.GameObject.SetActive(false);
			}
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00093ED0 File Offset: 0x000920D0
		private void BtnCloseOnClick(GameObject go)
		{
			if (this.m_bWaitNext)
			{
				for (int i = 0; i < this.m_GophersIconList.Count; i++)
				{
					this.m_GophersIconList[i].GetComponent<TweenPosition>().ResetToBeginning();
					this.m_GophersIconList[i].GetComponent<TweenPosition>().enabled = false;
				}
				this.m_bWaitNext = false;
			}
			this.ReSetData();
			this.m_Group.GameObject.SetActive(false);
			Game.PrefabSmallGameEnd();
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00093F54 File Offset: 0x00092154
		private void ReSetData()
		{
			this.m_bEnd = false;
			this.m_EndBackground.GameObject.SetActive(false);
			this.m_GameExplanation.GameObject.SetActive(true);
			this.m_BtnStart.GameObject.SetActive(true);
			this.m_fWaitTime = 0f;
			this.m_fOverTime = 0f;
			this.m_iAnswer = -1;
			this.m_iIndex = -1;
			this.m_iGet = 0;
			this.m_iAllGet = 0;
			this.m_iOldGet = 0;
			this.m_iExp = 0;
			this.m_bNext = false;
			this.m_bFirst = false;
			this.m_fFirstTime = 0f;
			this.m_iAnswerIndex = -1;
			this.m_LabelExplanation.Text = Game.StringTable.GetString(210001);
			this.m_bStart = false;
			this.m_bLeave = false;
			this.m_bCheck = false;
			this.m_GetItemIDList.Clear();
			for (int i = 0; i < this.m_LabelAmountList.Count; i++)
			{
				this.m_LabelAmountList[i].Text = "O";
			}
			for (int j = 0; j < this.m_GetItemIconList.Count; j++)
			{
				this.m_GetItemIconList[j].GameObject.SetActive(false);
			}
			for (int k = 0; k < this.m_GetItemIconBGList.Count; k++)
			{
				this.m_GetItemIconBGList[k].GameObject.SetActive(false);
			}
			for (int l = 0; l < this.m_LabelItemAmountList.Count; l++)
			{
				this.m_LabelItemAmountList[l].Text = string.Empty;
			}
			this.m_UpIcon1.GameObject.SetActive(false);
			for (int m = 0; m < this.m_RightWordList.Count; m++)
			{
				this.m_RightWordList[m].GetComponent<UISprite>().spriteName = "UI_fight_026_00";
			}
			for (int m = 0; m < this.RightWord_Tween_ScaleList.Count; m++)
			{
				this.RightWord_Tween_ScaleList[m].ResetToBeginning();
			}
			for (int m = 0; m < this.RightWord_Tween_PositionList.Count; m++)
			{
				this.RightWord_Tween_PositionList[m].ResetToBeginning();
			}
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x000941B0 File Offset: 0x000923B0
		private void Update()
		{
			if (this.m_bFirst && Time.time - this.m_fFirstTime >= 0.5f)
			{
				this.m_bFirst = false;
				this.PlayGophersIcon();
			}
			if (this.m_bNext && Time.time - this.m_fWaitTime >= 1f)
			{
				this.m_bNext = false;
				this.PlayGophersIcon();
			}
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
				}
			}
		}

		// Token: 0x04001497 RID: 5271
		private List<Control> m_GophersIconList;

		// Token: 0x04001498 RID: 5272
		private List<Control> m_OreIconList;

		// Token: 0x04001499 RID: 5273
		private List<Control> m_SelectBoxList;

		// Token: 0x0400149A RID: 5274
		private List<Control> m_HitIconList;

		// Token: 0x0400149B RID: 5275
		private List<Control> m_LabelAmountList;

		// Token: 0x0400149C RID: 5276
		private List<Control> m_RightWordList;

		// Token: 0x0400149D RID: 5277
		private List<Control> m_HammerIconList;

		// Token: 0x0400149E RID: 5278
		private List<TweenScale> RightWord_Tween_ScaleList;

		// Token: 0x0400149F RID: 5279
		private List<TweenPosition> RightWord_Tween_PositionList;

		// Token: 0x040014A0 RID: 5280
		private Control m_Group;

		// Token: 0x040014A1 RID: 5281
		private Control m_BarTime;

		// Token: 0x040014A2 RID: 5282
		private Control m_GameExplanation;

		// Token: 0x040014A3 RID: 5283
		private Control m_LabelExplanation;

		// Token: 0x040014A4 RID: 5284
		private Control m_BtnStart;

		// Token: 0x040014A5 RID: 5285
		private Control m_EndBackground;

		// Token: 0x040014A6 RID: 5286
		private Control m_BtnClose;

		// Token: 0x040014A7 RID: 5287
		private Control m_UpIcon1;

		// Token: 0x040014A8 RID: 5288
		private Control m_UpIcon2;

		// Token: 0x040014A9 RID: 5289
		private Control m_UpIcon3;

		// Token: 0x040014AA RID: 5290
		private Control m_BtnLeave;

		// Token: 0x040014AB RID: 5291
		private Control m_ItemNameBase;

		// Token: 0x040014AC RID: 5292
		private Control m_LabelItemName;

		// Token: 0x040014AD RID: 5293
		private Control m_LabelAddEye;

		// Token: 0x040014AE RID: 5294
		private List<Control> m_GetItemIconBGList;

		// Token: 0x040014AF RID: 5295
		private List<Control> m_GetItemIconList;

		// Token: 0x040014B0 RID: 5296
		private List<Control> m_LabelNewNumericalList;

		// Token: 0x040014B1 RID: 5297
		private List<Control> m_LabelOldNumericalList;

		// Token: 0x040014B2 RID: 5298
		private List<Control> m_LabelItemAmountList;

		// Token: 0x040014B3 RID: 5299
		private float m_fWaitTime;

		// Token: 0x040014B4 RID: 5300
		private float m_fOverTime;

		// Token: 0x040014B5 RID: 5301
		private bool m_bWaitNext;

		// Token: 0x040014B6 RID: 5302
		private bool m_bNext;

		// Token: 0x040014B7 RID: 5303
		private bool m_bStart;

		// Token: 0x040014B8 RID: 5304
		private bool m_bCheck;

		// Token: 0x040014B9 RID: 5305
		private bool m_bHit;

		// Token: 0x040014BA RID: 5306
		private int m_iOldGet;

		// Token: 0x040014BB RID: 5307
		private int m_iAnswer;

		// Token: 0x040014BC RID: 5308
		private int m_iGet;

		// Token: 0x040014BD RID: 5309
		private int m_iInToType;

		// Token: 0x040014BE RID: 5310
		private int m_iIndex;

		// Token: 0x040014BF RID: 5311
		private int m_iAnswerIndex;

		// Token: 0x040014C0 RID: 5312
		private int m_iExp;

		// Token: 0x040014C1 RID: 5313
		private int m_iAllGet;

		// Token: 0x040014C2 RID: 5314
		private bool m_bFirst;

		// Token: 0x040014C3 RID: 5315
		private bool m_bEnd;

		// Token: 0x040014C4 RID: 5316
		private float m_fFirstTime;

		// Token: 0x040014C5 RID: 5317
		private bool m_bLeave;

		// Token: 0x040014C6 RID: 5318
		private string[] m_aWordIconName = new string[]
		{
			"UI_fight_026_00",
			"UI_fight_026_01",
			"UI_fight_026_02",
			"UI_fight_026_03",
			"UI_fight_026_04",
			"UI_fight_026_05",
			"UI_fight_026_06",
			"UI_fight_026_07",
			"UI_fight_026_08",
			"UI_fight_026_09"
		};

		// Token: 0x040014C7 RID: 5319
		private string[] m_astrNumList = new string[]
		{
			"Ｏ",
			"一",
			"二",
			"三",
			"四",
			"五",
			"六",
			"七",
			"八",
			"九",
			"十"
		};

		// Token: 0x040014C8 RID: 5320
		private List<MiningItemNode> m_MiningItemList;

		// Token: 0x040014C9 RID: 5321
		private List<GetMiningItem> m_GetItemIDList;
	}
}
