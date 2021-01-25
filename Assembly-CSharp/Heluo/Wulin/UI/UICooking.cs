using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002FC RID: 764
	public class UICooking : UILayer
	{
		// Token: 0x06001031 RID: 4145 RVA: 0x0008A830 File Offset: 0x00088A30
		protected override void Awake()
		{
			this.m_AnswerBackgroundList = new List<Control>();
			this.m_LabelHoldList = new List<Control>();
			this.m_AnswerIconList = new List<Control>();
			this.m_LabelAnswer1List = new List<Control>();
			this.m_LabelAnswer2List = new List<Control>();
			this.m_LabelAnswer3List = new List<Control>();
			this.m_LabelAnswer4List = new List<Control>();
			this.m_LabelQuestion1List = new List<Control>();
			this.m_LabelQuestion2List = new List<Control>();
			this.m_LabelQuestion3List = new List<Control>();
			this.m_LabelQuestion4List = new List<Control>();
			this.m_SeasoningsBackgroundList = new List<Control>();
			this.m_SelectBackgroundList = new List<Control>();
			this.m_SeasoningsIconList = new List<Control>();
			this.m_SpoonBackgroundList = new List<Control>();
			this.m_SpoonSelectBackgroundList = new List<Control>();
			this.m_SpoonNodeList = new List<UICooking.SpoonNode>();
			this.m_BtnSelectChangeList = new List<Control>();
			this.m_CookingNodeList = new List<UICooking.CookingNode>();
			this.m_FailIconList = new List<Control>();
			this.m_OkIconList = new List<Control>();
			this.m_LabelNewNumericalList = new List<Control>();
			this.m_LabelOldNumericalList = new List<Control>();
			this.m_GetItemIconList = new List<Control>();
			this.m_GetItemIconBGList = new List<Control>();
			this.m_ItemNameList = new List<string>();
			this.m_BookNodeList = new List<BookNode>();
			base.Awake();
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0000AAAF File Offset: 0x00008CAF
		private void Start()
		{
			this.m_bEnd = false;
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x0008A96C File Offset: 0x00088B6C
		public void GameOpen()
		{
			this.m_Group.GameObject.SetActive(true);
			this.m_iSpoonIndex = -1;
			this.m_iGet = 0;
			for (int i = 0; i < 4; i++)
			{
				UICooking.CookingNode cookingNode = new UICooking.CookingNode();
				cookingNode.m_iIndex = i;
				cookingNode.m_iNowChili = 0;
				cookingNode.m_iNowMSG = 0;
				cookingNode.m_iNowSalt = 0;
				cookingNode.m_bComplete = false;
				this.m_CookingNodeList.Add(cookingNode);
			}
			this.ReSetSeasonings();
			for (int i = 0; i < this.m_LabelHoldList.Count; i++)
			{
				this.m_LabelHoldList[i].Text = Game.StringTable.GetString(300457);
			}
			for (int i = 0; i < this.m_SeasoningsIconList.Count; i++)
			{
				if (this.m_SeasoningsIconList[i].GetComponent<ImageData>().m_iIndex == 0)
				{
					this.m_SeasoningsIconList[i].GetComponent<UITexture>().mainTexture = (Game.g_Item.Load("2dtexture/gameui/item/i00428") as Texture2D);
				}
				else if (this.m_SeasoningsIconList[i].GetComponent<ImageData>().m_iIndex == 1)
				{
					this.m_SeasoningsIconList[i].GetComponent<UITexture>().mainTexture = (Game.g_Item.Load("2dtexture/gameui/item/i00487") as Texture2D);
				}
				else if (this.m_SeasoningsIconList[i].GetComponent<ImageData>().m_iIndex == 2)
				{
					this.m_SeasoningsIconList[i].GetComponent<UITexture>().mainTexture = (Game.g_Item.Load("2dtexture/gameui/item/i00449") as Texture2D);
				}
			}
			for (int i = 0; i < 3; i++)
			{
				UICooking.SpoonNode spoonNode = new UICooking.SpoonNode();
				spoonNode.m_iIndex = i;
				spoonNode.m_iContainerNow = 0;
				spoonNode.m_iContainerMax = 0;
				spoonNode.m_iType = -1;
				this.m_SpoonNodeList.Add(spoonNode);
			}
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x0008AB58 File Offset: 0x00088D58
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UICooking.<>f__switch$map8 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(52);
					dictionary.Add("GameExplanation", 0);
					dictionary.Add("AnswerBackground", 1);
					dictionary.Add("AnswerIcon", 2);
					dictionary.Add("LabelAnswer1", 3);
					dictionary.Add("LabelAnswer2", 4);
					dictionary.Add("LabelAnswer3", 5);
					dictionary.Add("LabelAnswer4", 6);
					dictionary.Add("LabelQuestion1", 7);
					dictionary.Add("LabelQuestion2", 8);
					dictionary.Add("LabelQuestion3", 9);
					dictionary.Add("LabelQuestion4", 10);
					dictionary.Add("SeasoningsBackground", 11);
					dictionary.Add("SelectBackground", 12);
					dictionary.Add("LabelContainer1Max", 13);
					dictionary.Add("LabelContainer1Now", 14);
					dictionary.Add("LabelContainer2Max", 15);
					dictionary.Add("LabelContainer2Now", 16);
					dictionary.Add("LabelContainer3Max", 17);
					dictionary.Add("LabelContainer3Now", 18);
					dictionary.Add("SeasoningsIcon", 19);
					dictionary.Add("LabelHold", 20);
					dictionary.Add("BtnStart", 21);
					dictionary.Add("BtnLeave", 22);
					dictionary.Add("SpoonBackground", 23);
					dictionary.Add("SpoonSelectBackground", 24);
					dictionary.Add("BtnSelectChange", 25);
					dictionary.Add("FailIcon", 26);
					dictionary.Add("OkIcon", 27);
					dictionary.Add("TalkGroup", 28);
					dictionary.Add("TalkIcon", 29);
					dictionary.Add("BarTime", 30);
					dictionary.Add("TalkLabel", 31);
					dictionary.Add("SpoonGroup", 32);
					dictionary.Add("SeasoningsGroup", 33);
					dictionary.Add("LabelExplanation", 34);
					dictionary.Add("BtnRecover", 35);
					dictionary.Add("GetItemIcon", 36);
					dictionary.Add("GetItemIconBG", 37);
					dictionary.Add("EndBackground", 38);
					dictionary.Add("BtnClose", 39);
					dictionary.Add("LabelNewNumerical", 40);
					dictionary.Add("LabelOldNumerical", 41);
					dictionary.Add("UpIcon1", 42);
					dictionary.Add("UpIcon2", 43);
					dictionary.Add("Group", 44);
					dictionary.Add("CookMask", 45);
					dictionary.Add("SeasoningsMask", 46);
					dictionary.Add("SpoonIcon1", 47);
					dictionary.Add("SpoonIcon2", 48);
					dictionary.Add("SpoonIcon3", 49);
					dictionary.Add("ItemNameBase", 50);
					dictionary.Add("LabelItemName", 51);
					UICooking.<>f__switch$map8 = dictionary;
				}
				int num;
				if (UICooking.<>f__switch$map8.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_GameExplanation = sender;
						break;
					case 1:
					{
						Control control = sender;
						control.OnClick += this.OnClickAnswerBackground;
						this.m_AnswerBackgroundList.Add(control);
						break;
					}
					case 2:
					{
						Control control = sender;
						this.m_AnswerIconList.Add(control);
						break;
					}
					case 3:
					{
						Control control = sender;
						this.m_LabelAnswer1List.Add(control);
						break;
					}
					case 4:
					{
						Control control = sender;
						this.m_LabelAnswer2List.Add(control);
						break;
					}
					case 5:
					{
						Control control = sender;
						this.m_LabelAnswer3List.Add(control);
						break;
					}
					case 6:
					{
						Control control = sender;
						this.m_LabelAnswer4List.Add(control);
						break;
					}
					case 7:
					{
						Control control = sender;
						this.m_LabelQuestion1List.Add(control);
						break;
					}
					case 8:
					{
						Control control = sender;
						this.m_LabelQuestion2List.Add(control);
						break;
					}
					case 9:
					{
						Control control = sender;
						this.m_LabelQuestion3List.Add(control);
						break;
					}
					case 10:
					{
						Control control = sender;
						this.m_LabelQuestion4List.Add(control);
						break;
					}
					case 11:
					{
						Control control = sender;
						control.OnClick += this.SeasoningsBackgroundOnClick;
						control.OnHover += this.SeasoningsBackgroundOnHover;
						this.m_SeasoningsBackgroundList.Add(control);
						break;
					}
					case 12:
					{
						Control control = sender;
						this.m_SelectBackgroundList.Add(control);
						break;
					}
					case 13:
						this.m_LabelContainer1Max = sender;
						break;
					case 14:
						this.m_LabelContainer1Now = sender;
						break;
					case 15:
						this.m_LabelContainer2Max = sender;
						break;
					case 16:
						this.m_LabelContainer2Now = sender;
						break;
					case 17:
						this.m_LabelContainer3Max = sender;
						break;
					case 18:
						this.m_LabelContainer3Now = sender;
						break;
					case 19:
					{
						Control control = sender;
						this.m_SeasoningsIconList.Add(control);
						break;
					}
					case 20:
					{
						Control control = sender;
						this.m_LabelHoldList.Add(control);
						break;
					}
					case 21:
						this.m_BtnStart = sender;
						this.m_BtnStart.OnClick += this.BtnStartOnClick;
						break;
					case 22:
						this.m_BtnLeave = sender;
						this.m_BtnLeave.OnClick += this.BtnLeaveOnClick;
						break;
					case 23:
					{
						Control control = sender;
						control.OnClick += this.SpoonBackgroundOnClick;
						control.OnHover += this.SpoonBackgroundOnHover;
						this.m_SpoonBackgroundList.Add(control);
						break;
					}
					case 24:
					{
						Control control = sender;
						this.m_SpoonSelectBackgroundList.Add(control);
						break;
					}
					case 25:
					{
						Control control = sender;
						control.OnClick += this.BtnSelectChangeOnClick;
						this.m_BtnSelectChangeList.Add(control);
						break;
					}
					case 26:
					{
						Control control = sender;
						this.m_FailIconList.Add(control);
						break;
					}
					case 27:
					{
						Control control = sender;
						this.m_OkIconList.Add(control);
						break;
					}
					case 28:
						this.m_TalkGroup = sender;
						break;
					case 29:
						this.m_TalkIcon = sender;
						this.m_TalkIcon.OnClick += this.TalkIconOnClick;
						break;
					case 30:
						this.m_BarTime = sender;
						break;
					case 31:
						this.m_TalkLabel = sender;
						break;
					case 32:
						this.m_SpoonGroup = sender;
						this.m_SpoonGroup.OnClick += this.SpoonGroupOnClick;
						break;
					case 33:
						this.m_SeasoningsGroup = sender;
						break;
					case 34:
						this.m_LabelExplanation = sender;
						break;
					case 35:
						this.m_BtnRecover = sender;
						this.m_BtnRecover.OnClick += this.BtnRecoverOnClick;
						break;
					case 36:
					{
						Control control = sender;
						this.m_GetItemIconList.Add(control);
						break;
					}
					case 37:
					{
						Control control = sender;
						control.OnHover += this.GetItemIconBGOnHover;
						this.m_GetItemIconBGList.Add(control);
						break;
					}
					case 38:
						this.m_EndBackground = sender;
						break;
					case 39:
						this.m_BtnClose = sender;
						this.m_BtnClose.OnClick += this.BtnCloseOnClick;
						break;
					case 40:
					{
						Control control = sender;
						this.m_LabelNewNumericalList.Add(control);
						break;
					}
					case 41:
					{
						Control control = sender;
						this.m_LabelOldNumericalList.Add(control);
						break;
					}
					case 42:
						this.m_UpIcon1 = sender;
						break;
					case 43:
						this.m_UpIcon2 = sender;
						break;
					case 44:
						this.m_Group = sender;
						break;
					case 45:
						this.m_CookMask = sender;
						break;
					case 46:
						this.m_SeasoningsMask = sender;
						break;
					case 47:
						this.m_SpoonIcon1 = sender;
						break;
					case 48:
						this.m_SpoonIcon2 = sender;
						break;
					case 49:
						this.m_SpoonIcon3 = sender;
						break;
					case 50:
						this.m_ItemNameBase = sender;
						break;
					case 51:
						this.m_LabelItemName = sender;
						break;
					}
				}
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0008B430 File Offset: 0x00089630
		private void BtnStartOnClick(GameObject go)
		{
			this.m_GameExplanation.GameObject.SetActive(false);
			this.SetQuestData();
			this.m_CookMask.GameObject.SetActive(true);
			this.m_SeasoningsMask.GameObject.SetActive(true);
			int num = 0;
			this.m_fGameTime = 90f;
			this.m_bSpoonCheck = true;
			this.m_bSeasonings = true;
			this.m_bStart = true;
			this.m_BarTime.GetComponent<UISprite>().fillAmount = 1f;
			this.m_fOverTime = Time.time;
			this.m_fOverTime += this.m_fGameTime + (float)(num / 4);
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0008B4D0 File Offset: 0x000896D0
		private void ReSetSeasonings()
		{
			for (int i = 0; i < this.m_LabelAnswer1List.Count; i++)
			{
				this.m_LabelAnswer1List[i].Text = "0";
				this.m_LabelAnswer2List[i].Text = "0";
				this.m_LabelAnswer3List[i].Text = "0";
				this.m_LabelAnswer4List[i].Text = "0";
			}
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0008B554 File Offset: 0x00089754
		private void SetQuestData()
		{
			Texture2D mainTexture = null;
			for (int i = 0; i < this.m_AnswerIconList.Count; i++)
			{
				int num = Random.Range(0, this.m_BookNodeList.Count);
				string strBookImage = this.m_BookNodeList[num].m_strBookImage;
				string text = strBookImage.Substring(1, strBookImage.Length - 1);
				int num2 = int.Parse(text) % 10000;
				string strIcon = "i" + string.Format("{0:00000}", num2);
				this.m_BookNodeList.RemoveAt(num);
				Texture2D texture2D = Game.g_AbilityBook.Load("2dtexture/gameui/abilitybook/" + strBookImage) as Texture2D;
				if (texture2D != null)
				{
					this.m_AnswerIconList[i].GetComponent<UITexture>().mainTexture = texture2D;
				}
				if (i == 2)
				{
					mainTexture = texture2D;
				}
				else if (i == 3)
				{
					this.m_AnswerIconList[2].GetComponent<UITexture>().mainTexture = texture2D;
					this.m_AnswerIconList[i].GetComponent<UITexture>().mainTexture = mainTexture;
				}
				for (int j = 0; j < this.m_CookingNodeList.Count; j++)
				{
					if (this.m_CookingNodeList[j].m_iIndex == i)
					{
						this.m_CookingNodeList[j].m_iItemID = Game.ItemData.GetItemID(strIcon);
					}
				}
			}
			int num3 = 0;
			int num4 = 0;
			for (int k = 0; k < this.m_SpoonNodeList.Count; k++)
			{
				if (k == 0)
				{
					this.m_LabelContainer1Now.Text = this.m_SpoonNodeList[k].m_iContainerNow.ToString();
					num3 = Random.Range(7, 9);
					this.m_SpoonNodeList[k].m_iContainerMax = num3;
					this.m_LabelContainer1Max.Text = this.m_SpoonNodeList[k].m_iContainerMax.ToString();
				}
				else if (k == 1)
				{
					this.m_LabelContainer2Now.Text = this.m_SpoonNodeList[k].m_iContainerNow.ToString();
					num4 = Random.Range(4, 6);
					this.m_SpoonNodeList[k].m_iContainerMax = num4;
					this.m_LabelContainer2Max.Text = this.m_SpoonNodeList[k].m_iContainerMax.ToString();
				}
				else if (k == 2)
				{
					int iContainerMax;
					if (num3 == 8)
					{
						if (num4 == 4 || num4 == 6)
						{
							iContainerMax = 3;
						}
						else
						{
							iContainerMax = Random.Range(1, 3);
						}
					}
					else
					{
						iContainerMax = Random.Range(1, 3);
					}
					this.m_LabelContainer3Now.Text = this.m_SpoonNodeList[k].m_iContainerNow.ToString();
					this.m_SpoonNodeList[k].m_iContainerMax = iContainerMax;
					this.m_LabelContainer3Max.Text = this.m_SpoonNodeList[k].m_iContainerMax.ToString();
				}
			}
			for (int l = 0; l < this.m_CookingNodeList.Count; l++)
			{
				List<int> list = new List<int>();
				int num = Random.Range(1, 10);
				list.Add(num);
				for (int m = 0; m < 2; m++)
				{
					num = Random.Range(1, 10);
					num = this.QuestionRandom(num, list);
					list.Add(num);
				}
				this.m_CookingNodeList[l].m_iMaxSalt = list[0];
				this.m_CookingNodeList[l].m_iMaxMSG = list[1];
				this.m_CookingNodeList[l].m_iMaxChili = list[2];
				List<Control> list2 = new List<Control>();
				if (l == 0)
				{
					list2 = this.m_LabelQuestion1List;
				}
				else if (l == 1)
				{
					list2 = this.m_LabelQuestion2List;
				}
				else if (l == 2)
				{
					list2 = this.m_LabelQuestion3List;
				}
				else if (l == 3)
				{
					list2 = this.m_LabelQuestion4List;
				}
				for (int n = 0; n < list2.Count; n++)
				{
					if (list2[n].GetComponent<LabelData>().m_iIndex == 0)
					{
						list2[n].Text = this.m_CookingNodeList[l].m_iMaxSalt.ToString();
					}
					else if (list2[n].GetComponent<LabelData>().m_iIndex == 1)
					{
						list2[n].Text = this.m_CookingNodeList[l].m_iMaxMSG.ToString();
					}
					else if (list2[n].GetComponent<LabelData>().m_iIndex == 2)
					{
						list2[n].Text = this.m_CookingNodeList[l].m_iMaxChili.ToString();
					}
				}
			}
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0008BA50 File Offset: 0x00089C50
		private int QuestionRandom(int iRange, List<int> ValueList)
		{
			int num = iRange;
			for (int i = 0; i < ValueList.Count; i++)
			{
				if (ValueList[i] == num)
				{
					num = Random.Range(1, 10);
					num = this.QuestionRandom(num, ValueList);
				}
			}
			return num;
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0008BA98 File Offset: 0x00089C98
		private void SpoonBackgroundOnHover(GameObject go, bool bHover)
		{
			for (int i = 0; i < this.m_SpoonBackgroundList.Count; i++)
			{
				this.m_SpoonBackgroundList[i].GetComponent<UISprite>().spriteName = "cook_btn";
				if (bHover && this.m_SpoonBackgroundList[i].GetComponent<ImageData>().m_iIndex == go.GetComponent<ImageData>().m_iIndex && this.m_iSpoonIndex != go.GetComponent<ImageData>().m_iIndex)
				{
					this.m_SpoonBackgroundList[i].GetComponent<UISprite>().spriteName = "cook_btnChoice";
				}
			}
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0008BB3C File Offset: 0x00089D3C
		private void SeasoningsBackgroundOnHover(GameObject go, bool bHover)
		{
			for (int i = 0; i < this.m_SeasoningsBackgroundList.Count; i++)
			{
				this.m_SeasoningsBackgroundList[i].GetComponent<UISprite>().spriteName = "cook_btn";
				if (bHover && this.m_SeasoningsBackgroundList[i].GetComponent<ImageData>().m_iIndex == go.GetComponent<ImageData>().m_iIndex)
				{
					this.m_SeasoningsBackgroundList[i].GetComponent<UISprite>().spriteName = "cook_btnChoice";
				}
			}
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x0008BBC8 File Offset: 0x00089DC8
		private void BtnSelectChangeOnClick(GameObject go)
		{
			for (int i = 0; i < this.m_SpoonSelectBackgroundList.Count; i++)
			{
				if (this.m_SpoonSelectBackgroundList[i].GetComponent<ImageData>().m_iIndex == this.m_iSpoonIndex)
				{
					this.m_SpoonSelectBackgroundList[i].GameObject.SetActive(false);
					break;
				}
			}
			this.m_iSpoonIndex = -1;
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0008BC38 File Offset: 0x00089E38
		private void SpoonBackgroundOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				if (this.m_iSpoonIndex == -1)
				{
					return;
				}
				for (int i = 0; i < this.m_SpoonSelectBackgroundList.Count; i++)
				{
					if (this.m_SpoonSelectBackgroundList[i].GetComponent<ImageData>().m_iIndex == this.m_iSpoonIndex)
					{
						this.m_SpoonSelectBackgroundList[i].GameObject.SetActive(false);
						break;
					}
				}
				this.m_iSpoonIndex = -1;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				if (this.m_iSpoonIndex == go.GetComponent<ImageData>().m_iIndex)
				{
					return;
				}
				if (this.m_iSpoonIndex == -1)
				{
					this.m_bSpoonCheck = true;
					if (this.m_SeasoningsMask.GameObject.activeSelf && this.m_bSpoonCheck)
					{
						this.m_SeasoningsMask.GameObject.SetActive(false);
					}
					this.m_iSpoonIndex = go.GetComponent<ImageData>().m_iIndex;
					for (int i = 0; i < this.m_SpoonSelectBackgroundList.Count; i++)
					{
						if (this.m_SpoonSelectBackgroundList[i].GetComponent<ImageData>().m_iIndex == this.m_iSpoonIndex)
						{
							this.m_SpoonSelectBackgroundList[i].GameObject.SetActive(true);
							break;
						}
					}
				}
				else
				{
					UICooking.SpoonNode spoonNode = null;
					int num = -1;
					for (int i = 0; i < this.m_SpoonNodeList.Count; i++)
					{
						if (this.m_SpoonNodeList[i].m_iIndex == go.GetComponent<ImageData>().m_iIndex)
						{
							spoonNode = this.m_SpoonNodeList[i];
						}
						if (this.m_SpoonNodeList[i].m_iIndex == this.m_iSpoonIndex)
						{
							num = this.m_SpoonNodeList[i].m_iType;
						}
					}
					if (num == -1)
					{
						this.m_iSpoonIndex = go.GetComponent<ImageData>().m_iIndex;
						for (int i = 0; i < this.m_SpoonSelectBackgroundList.Count; i++)
						{
							this.m_SpoonSelectBackgroundList[i].GameObject.SetActive(false);
							if (this.m_SpoonSelectBackgroundList[i].GetComponent<ImageData>().m_iIndex == this.m_iSpoonIndex)
							{
								this.m_SpoonSelectBackgroundList[i].GameObject.SetActive(true);
							}
						}
					}
					else if (spoonNode.m_iContainerNow == spoonNode.m_iContainerMax)
					{
						this.m_iSpoonIndex = go.GetComponent<ImageData>().m_iIndex;
						for (int i = 0; i < this.m_SpoonSelectBackgroundList.Count; i++)
						{
							this.m_SpoonSelectBackgroundList[i].GameObject.SetActive(false);
							if (this.m_SpoonSelectBackgroundList[i].GetComponent<ImageData>().m_iIndex == this.m_iSpoonIndex)
							{
								this.m_SpoonSelectBackgroundList[i].GameObject.SetActive(true);
							}
						}
					}
					else
					{
						for (int i = 0; i < this.m_SpoonNodeList.Count; i++)
						{
							if (this.m_iSpoonIndex == this.m_SpoonNodeList[i].m_iIndex)
							{
								if (spoonNode.m_iType != this.m_SpoonNodeList[i].m_iType && spoonNode.m_iType != -1)
								{
									return;
								}
								int num2 = spoonNode.m_iContainerMax - spoonNode.m_iContainerNow;
								int iContainerNow = this.m_SpoonNodeList[i].m_iContainerNow;
								spoonNode.m_iType = this.m_SpoonNodeList[i].m_iType;
								if (iContainerNow <= num2)
								{
									spoonNode.m_iContainerNow += iContainerNow;
									this.m_SpoonNodeList[i].m_iContainerNow = 0;
									this.m_SpoonNodeList[i].m_iType = -1;
									for (int j = 0; j < this.m_LabelHoldList.Count; j++)
									{
										if (this.m_LabelHoldList[j].GetComponent<LabelData>().m_iIndex == this.m_SpoonNodeList[i].m_iIndex)
										{
											this.m_LabelHoldList[j].Text = Game.StringTable.GetString(300457);
										}
									}
								}
								else
								{
									this.m_SpoonNodeList[i].m_iContainerNow -= num2;
									spoonNode.m_iContainerNow += num2;
								}
								Control control = null;
								if (this.m_SpoonNodeList[i].m_iIndex == 0)
								{
									if (this.m_SpoonNodeList[i].m_iContainerNow == 0)
									{
										this.m_SpoonIcon1.GetComponent<UISprite>().spriteName = "cook_spoon_L";
									}
									control = this.m_LabelContainer1Now;
								}
								else if (this.m_SpoonNodeList[i].m_iIndex == 1)
								{
									if (this.m_SpoonNodeList[i].m_iContainerNow == 0)
									{
										this.m_SpoonIcon2.GetComponent<UISprite>().spriteName = "cook_spoon_M";
									}
									control = this.m_LabelContainer2Now;
								}
								else if (this.m_SpoonNodeList[i].m_iIndex == 2)
								{
									if (this.m_SpoonNodeList[i].m_iContainerNow == 0)
									{
										this.m_SpoonIcon3.GetComponent<UISprite>().spriteName = "cook_spoon_S";
									}
									control = this.m_LabelContainer3Now;
								}
								control.Text = this.m_SpoonNodeList[i].m_iContainerNow.ToString();
								if (spoonNode.m_iIndex == 0)
								{
									this.m_SpoonIcon1.GetComponent<UISprite>().spriteName = "cook_spoon_Lfull";
									control = this.m_LabelContainer1Now;
								}
								else if (spoonNode.m_iIndex == 1)
								{
									this.m_SpoonIcon2.GetComponent<UISprite>().spriteName = "cook_spoon_Mfull";
									control = this.m_LabelContainer2Now;
								}
								else if (spoonNode.m_iIndex == 2)
								{
									this.m_SpoonIcon3.GetComponent<UISprite>().spriteName = "cook_spoon_Sfull";
									control = this.m_LabelContainer3Now;
								}
								control.Text = spoonNode.m_iContainerNow.ToString();
							}
						}
						for (int i = 0; i < this.m_LabelHoldList.Count; i++)
						{
							if (this.m_LabelHoldList[i].GetComponent<LabelData>().m_iIndex == spoonNode.m_iIndex)
							{
								if (spoonNode.m_iType == 0)
								{
									this.m_LabelHoldList[i].Text = Game.StringTable.GetString(300454);
								}
								else if (spoonNode.m_iType == 1)
								{
									this.m_LabelHoldList[i].Text = Game.StringTable.GetString(300455);
								}
								else if (spoonNode.m_iType == 2)
								{
									this.m_LabelHoldList[i].Text = Game.StringTable.GetString(300456);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x0008C304 File Offset: 0x0008A504
		private void BtnRecoverOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(0))
			{
				this.m_SpoonNodeList[this.m_iSpoonIndex].m_iContainerNow = 0;
				this.m_SpoonNodeList[this.m_iSpoonIndex].m_iType = -1;
				if (this.m_iSpoonIndex == 0)
				{
					this.m_SpoonIcon1.GetComponent<UISprite>().spriteName = "cook_spoon_L";
					this.m_LabelContainer1Now.Text = "0";
				}
				else if (this.m_iSpoonIndex == 1)
				{
					this.m_SpoonIcon2.GetComponent<UISprite>().spriteName = "cook_spoon_M";
					this.m_LabelContainer2Now.Text = "0";
				}
				else if (this.m_iSpoonIndex == 2)
				{
					this.m_SpoonIcon3.GetComponent<UISprite>().spriteName = "cook_spoon_S";
					this.m_LabelContainer3Now.Text = "0";
				}
				for (int i = 0; i < this.m_LabelHoldList.Count; i++)
				{
					if (this.m_LabelHoldList[i].GetComponent<LabelData>().m_iIndex == this.m_iSpoonIndex)
					{
						this.m_LabelHoldList[i].Text = Game.StringTable.GetString(300457);
						break;
					}
				}
			}
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0008C448 File Offset: 0x0008A648
		private void SeasoningsBackgroundOnClick(GameObject go)
		{
			for (int i = 0; i < this.m_SpoonNodeList.Count; i++)
			{
				if (this.m_SpoonNodeList[i].m_iIndex == this.m_iSpoonIndex)
				{
					if (Input.GetMouseButtonUp(0) && this.m_SpoonNodeList[i].m_iContainerNow == 0)
					{
						if (!this.m_bSeasonings)
						{
							this.m_bSeasonings = true;
						}
						if (this.m_CookMask.GameObject.activeSelf && this.m_bSeasonings)
						{
							this.m_CookMask.GameObject.SetActive(false);
						}
						this.m_SpoonNodeList[i].m_iType = go.GetComponent<ImageData>().m_iIndex;
						this.m_SpoonNodeList[i].m_iContainerNow = this.m_SpoonNodeList[i].m_iContainerMax;
						if (i == 0)
						{
							this.m_SpoonIcon1.GetComponent<UISprite>().spriteName = "cook_spoon_Lfull";
							this.m_LabelContainer1Now.Text = this.m_SpoonNodeList[i].m_iContainerNow.ToString();
						}
						else if (i == 1)
						{
							this.m_SpoonIcon2.GetComponent<UISprite>().spriteName = "cook_spoon_Mfull";
							this.m_LabelContainer2Now.Text = this.m_SpoonNodeList[i].m_iContainerNow.ToString();
						}
						else if (i == 2)
						{
							this.m_SpoonIcon3.GetComponent<UISprite>().spriteName = "cook_spoon_Sfull";
							this.m_LabelContainer3Now.Text = this.m_SpoonNodeList[i].m_iContainerNow.ToString();
						}
						for (int j = 0; j < this.m_LabelHoldList.Count; j++)
						{
							if (this.m_LabelHoldList[j].GetComponent<LabelData>().m_iIndex == this.m_iSpoonIndex)
							{
								if (this.m_SpoonNodeList[i].m_iType == 0)
								{
									this.m_LabelHoldList[j].Text = Game.StringTable.GetString(300454);
								}
								else if (this.m_SpoonNodeList[i].m_iType == 1)
								{
									this.m_LabelHoldList[j].Text = Game.StringTable.GetString(300455);
								}
								else if (this.m_SpoonNodeList[i].m_iType == 2)
								{
									this.m_LabelHoldList[j].Text = Game.StringTable.GetString(300456);
								}
							}
						}
					}
					break;
				}
			}
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0008C6E0 File Offset: 0x0008A8E0
		private void OnClickAnswerBackground(GameObject go)
		{
			if (Input.GetMouseButtonUp(0))
			{
				int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
				if (this.m_CookingNodeList[iBtnID].m_bComplete)
				{
					return;
				}
				if (this.m_iSpoonIndex == -1)
				{
					return;
				}
				List<Control> list = new List<Control>();
				if (iBtnID == 0)
				{
					list = this.m_LabelAnswer1List;
				}
				else if (iBtnID == 1)
				{
					list = this.m_LabelAnswer2List;
				}
				else if (iBtnID == 2)
				{
					list = this.m_LabelAnswer3List;
				}
				else if (iBtnID == 3)
				{
					list = this.m_LabelAnswer4List;
				}
				int num = 0;
				bool flag = false;
				if (this.m_SpoonNodeList[this.m_iSpoonIndex].m_iType == 0)
				{
					if (this.m_CookingNodeList[iBtnID].m_iNowSalt + this.m_SpoonNodeList[this.m_iSpoonIndex].m_iContainerNow > this.m_CookingNodeList[iBtnID].m_iMaxSalt)
					{
						flag = true;
						this.m_TalkLabel.Text = Game.StringTable.GetString(300451);
						Texture2D texture2D = Game.g_DevelopQHead.Load("2dtexture/gameui/develop/developqhead/100014_09") as Texture2D;
						if (texture2D != null)
						{
							this.m_TalkIcon.GetComponent<UITexture>().mainTexture = texture2D;
						}
					}
					else
					{
						this.m_CookingNodeList[iBtnID].m_iNowSalt = this.m_SpoonNodeList[this.m_iSpoonIndex].m_iContainerNow + this.m_CookingNodeList[iBtnID].m_iNowSalt;
					}
					num = this.m_CookingNodeList[iBtnID].m_iNowSalt;
				}
				else if (this.m_SpoonNodeList[this.m_iSpoonIndex].m_iType == 1)
				{
					if (this.m_CookingNodeList[iBtnID].m_iNowMSG + this.m_SpoonNodeList[this.m_iSpoonIndex].m_iContainerNow > this.m_CookingNodeList[iBtnID].m_iMaxMSG)
					{
						flag = true;
						this.m_TalkLabel.Text = Game.StringTable.GetString(300452);
						Texture2D texture2D = Game.g_DevelopQHead.Load("2dtexture/gameui/develop/developqhead/100014_02") as Texture2D;
						if (texture2D != null)
						{
							this.m_TalkIcon.GetComponent<UITexture>().mainTexture = texture2D;
						}
					}
					else
					{
						this.m_CookingNodeList[iBtnID].m_iNowMSG = this.m_SpoonNodeList[this.m_iSpoonIndex].m_iContainerNow + this.m_CookingNodeList[iBtnID].m_iNowMSG;
					}
					num = this.m_CookingNodeList[iBtnID].m_iNowMSG;
				}
				else if (this.m_SpoonNodeList[this.m_iSpoonIndex].m_iType == 2)
				{
					if (this.m_CookingNodeList[iBtnID].m_iNowChili + this.m_SpoonNodeList[this.m_iSpoonIndex].m_iContainerNow > this.m_CookingNodeList[iBtnID].m_iMaxChili)
					{
						flag = true;
						this.m_TalkLabel.Text = Game.StringTable.GetString(300453);
						Texture2D texture2D = Game.g_DevelopQHead.Load("2dtexture/gameui/develop/developqhead/100014_19") as Texture2D;
						if (texture2D != null)
						{
							this.m_TalkIcon.GetComponent<UITexture>().mainTexture = texture2D;
						}
					}
					else
					{
						this.m_CookingNodeList[iBtnID].m_iNowChili = this.m_SpoonNodeList[this.m_iSpoonIndex].m_iContainerNow + this.m_CookingNodeList[iBtnID].m_iNowChili;
					}
					num = this.m_CookingNodeList[iBtnID].m_iNowChili;
				}
				if (flag)
				{
					this.m_CookingNodeList[iBtnID].m_iNowSalt = 0;
					this.m_CookingNodeList[iBtnID].m_iNowMSG = 0;
					this.m_CookingNodeList[iBtnID].m_iNowChili = 0;
					for (int i = 0; i < list.Count; i++)
					{
						list[i].Text = "0";
					}
					for (int i = 0; i < this.m_FailIconList.Count; i++)
					{
						if (this.m_FailIconList[i].GetComponent<ImageData>().m_iIndex == iBtnID)
						{
							this.m_FailIconList[i].GameObject.SetActive(true);
							this.m_TalkGroup.GameObject.SetActive(true);
							this.m_TalkGroup.GetComponent<TweenPosition>().Play(true);
							break;
						}
					}
				}
				else
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].GetComponent<LabelData>().m_iIndex == this.m_SpoonNodeList[this.m_iSpoonIndex].m_iType)
						{
							list[i].Text = num.ToString();
						}
					}
					this.CookingCompleteCheck(iBtnID);
				}
				this.m_SpoonNodeList[this.m_iSpoonIndex].m_iContainerNow = 0;
				this.m_SpoonNodeList[this.m_iSpoonIndex].m_iType = -1;
				if (this.m_SpoonNodeList[this.m_iSpoonIndex].m_iIndex == 0)
				{
					this.m_SpoonIcon1.GetComponent<UISprite>().spriteName = "cook_spoon_L";
					this.m_LabelContainer1Now.Text = "0";
				}
				else if (this.m_SpoonNodeList[this.m_iSpoonIndex].m_iIndex == 1)
				{
					this.m_SpoonIcon2.GetComponent<UISprite>().spriteName = "cook_spoon_M";
					this.m_LabelContainer2Now.Text = "0";
				}
				else if (this.m_SpoonNodeList[this.m_iSpoonIndex].m_iIndex == 2)
				{
					this.m_SpoonIcon3.GetComponent<UISprite>().spriteName = "cook_spoon_S";
					this.m_LabelContainer3Now.Text = "0";
				}
				for (int j = 0; j < this.m_LabelHoldList.Count; j++)
				{
					if (this.m_LabelHoldList[j].GetComponent<LabelData>().m_iIndex == this.m_iSpoonIndex)
					{
						this.m_LabelHoldList[j].Text = Game.StringTable.GetString(300457);
						break;
					}
				}
			}
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0008CD24 File Offset: 0x0008AF24
		private void SpoonGroupOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				for (int i = 0; i < this.m_SpoonSelectBackgroundList.Count; i++)
				{
					if (this.m_SpoonSelectBackgroundList[i].GetComponent<ImageData>().m_iIndex == this.m_iSpoonIndex)
					{
						this.m_SpoonSelectBackgroundList[i].GameObject.SetActive(false);
						break;
					}
				}
				this.m_iSpoonIndex = -1;
			}
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0008CD9C File Offset: 0x0008AF9C
		private void CookingCompleteCheck(int iIndex)
		{
			bool flag = true;
			if (this.m_CookingNodeList[iIndex].m_iNowSalt != this.m_CookingNodeList[iIndex].m_iMaxSalt)
			{
				flag = false;
			}
			if (this.m_CookingNodeList[iIndex].m_iNowMSG != this.m_CookingNodeList[iIndex].m_iMaxMSG)
			{
				flag = false;
			}
			if (this.m_CookingNodeList[iIndex].m_iNowChili != this.m_CookingNodeList[iIndex].m_iMaxChili)
			{
				flag = false;
			}
			if (flag)
			{
				this.m_CookingNodeList[iIndex].m_bComplete = true;
				this.m_iGet++;
				for (int i = 0; i < this.m_OkIconList.Count; i++)
				{
					if (this.m_OkIconList[i].GetComponent<ImageData>().m_iIndex == iIndex)
					{
						this.m_OkIconList[i].GameObject.SetActive(true);
						this.m_OkIconList[i].GetComponent<TweenScale>().ResetToBeginning();
						this.m_OkIconList[i].GetComponent<TweenScale>().Play(true);
					}
				}
			}
			bool flag2 = true;
			for (int j = 0; j < this.m_CookingNodeList.Count; j++)
			{
				if (!this.m_CookingNodeList[j].m_bComplete)
				{
					flag2 = false;
				}
			}
			if (flag2)
			{
				this.m_bStart = false;
				this.SetEnd();
			}
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0000AAB8 File Offset: 0x00008CB8
		private void BtnLeaveOnClick(GameObject go)
		{
			this.m_bStart = false;
			this.SetEnd();
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0000AAC7 File Offset: 0x00008CC7
		private void BtnCloseOnClick(GameObject go)
		{
			this.ReSetData();
			this.m_Group.GameObject.SetActive(false);
			Game.PrefabSmallGameEnd();
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0008CF14 File Offset: 0x0008B114
		private void ReSetData()
		{
			this.m_EndBackground.GameObject.SetActive(false);
			this.m_GameExplanation.GameObject.SetActive(true);
			this.m_CookingNodeList.Clear();
			this.m_SpoonNodeList.Clear();
			this.m_BookNodeList.Clear();
			this.m_bEnd = false;
			for (int i = 0; i < this.m_OkIconList.Count; i++)
			{
				this.m_OkIconList[i].GameObject.SetActive(false);
			}
			for (int i = 0; i < this.m_SpoonSelectBackgroundList.Count; i++)
			{
				this.m_SpoonSelectBackgroundList[i].GameObject.SetActive(false);
			}
			for (int i = 0; i < this.m_AnswerIconList.Count; i++)
			{
				this.m_AnswerIconList[i].GetComponent<UITexture>().mainTexture = null;
			}
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0008D008 File Offset: 0x0008B208
		public void TalkGroupFinish()
		{
			if (this.m_bClose)
			{
				this.m_TalkGroup.GameObject.SetActive(false);
				this.m_bClose = false;
			}
			else
			{
				for (int i = 0; i < this.m_FailIconList.Count; i++)
				{
					this.m_FailIconList[i].GameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0000AAE5 File Offset: 0x00008CE5
		public void TalkIconOnClick(GameObject go)
		{
			this.m_TalkGroup.GetComponent<TweenPosition>().Play(false);
			this.m_bClose = true;
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0008D070 File Offset: 0x0008B270
		private void SetEnd()
		{
			if (this.m_bEnd)
			{
				return;
			}
			this.m_bEnd = true;
			this.m_EndBackground.GameObject.SetActive(true);
			for (int i = 0; i < this.m_GetItemIconBGList.Count; i++)
			{
				this.m_GetItemIconBGList[i].GameObject.SetActive(false);
			}
			for (int j = 0; j < this.m_GetItemIconList.Count; j++)
			{
				this.m_GetItemIconList[j].GameObject.SetActive(false);
			}
			this.m_ItemNameList.Clear();
			for (int k = 0; k < this.m_CookingNodeList.Count; k++)
			{
				if (this.m_CookingNodeList[k].m_bComplete)
				{
					this.m_ItemNameList.Add(Game.ItemData.GetItemName(this.m_CookingNodeList[k].m_iItemID));
				}
			}
			for (int k = 0; k < this.m_ItemNameList.Count; k++)
			{
				for (int l = 0; l < this.m_GetItemIconBGList.Count; l++)
				{
					if (k == this.m_GetItemIconBGList[l].GetComponent<ImageData>().m_iIndex)
					{
						this.m_GetItemIconBGList[l].GameObject.SetActive(true);
					}
				}
				for (int l = 0; l < this.m_GetItemIconList.Count; l++)
				{
					if (k == this.m_GetItemIconList[l].GetComponent<ImageData>().m_iIndex)
					{
						string itemIconID = Game.ItemData.GetItemIconID(this.m_ItemNameList[k]);
						Texture2D texture2D = Game.g_Item.Load("2dtexture/gameui/item/" + itemIconID) as Texture2D;
						if (texture2D != null)
						{
							this.m_GetItemIconList[l].GetComponent<UITexture>().mainTexture = texture2D;
						}
						this.m_GetItemIconList[l].GameObject.SetActive(true);
					}
				}
			}
			int num = 0;
			int num2 = 0;
			for (int k = 0; k < this.m_LabelOldNumericalList.Count; k++)
			{
				if (this.m_LabelOldNumericalList[k].GetComponent<LabelData>().m_iIndex == 0)
				{
					num = 0;
					this.m_LabelOldNumericalList[k].Text = num.ToString();
				}
				else if (this.m_LabelOldNumericalList[k].GetComponent<LabelData>().m_iIndex == 1)
				{
					num2 = 0;
					this.m_LabelOldNumericalList[k].Text = num2.ToString();
				}
			}
			int num3 = (int)((float)this.m_iGet * this.m_fGameTime) / 54;
			if (num3 == 0)
			{
				num3 = 1;
			}
			num3 += this.m_iGet;
			float num4 = 1f;
			num3 = Mathf.RoundToInt(num4 * (float)num3);
			if (this.m_iGet == 4)
			{
				Game.NpcData.SetNpcFriendly(100015, num / 10);
				Game.NpcData.SetNpcFriendly(100016, num / 10);
				Game.NpcData.SetNpcFriendly(100017, num / 10);
				Game.NpcData.SetNpcFriendly(100019, num / 10);
			}
			for (int k = 0; k < this.m_LabelNewNumericalList.Count; k++)
			{
				this.m_LabelNewNumericalList[k].GameObject.SetActive(false);
			}
			for (int k = 0; k < this.m_LabelNewNumericalList.Count; k++)
			{
				if (this.m_LabelNewNumericalList[k].GetComponent<LabelData>().m_iIndex == 0)
				{
					int num5 = 0;
					if (num5 != num)
					{
						this.m_LabelNewNumericalList[k].GameObject.SetActive(true);
						this.m_LabelNewNumericalList[k].Text = num5.ToString();
						this.m_UpIcon1.GameObject.SetActive(true);
					}
					else
					{
						this.m_LabelNewNumericalList[k].GameObject.SetActive(false);
						this.m_UpIcon1.GameObject.SetActive(false);
					}
				}
				else if (this.m_LabelNewNumericalList[k].GetComponent<LabelData>().m_iIndex == 1)
				{
					if (this.m_iGet == 4)
					{
						int num6 = 0;
						if (num6 != num2)
						{
							this.m_LabelNewNumericalList[k].GameObject.SetActive(true);
							this.m_LabelNewNumericalList[k].Text = num6.ToString();
							this.m_UpIcon2.GameObject.SetActive(true);
						}
						else
						{
							this.m_LabelNewNumericalList[k].GameObject.SetActive(false);
							this.m_UpIcon2.GameObject.SetActive(false);
						}
					}
				}
			}
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0008D544 File Offset: 0x0008B744
		private void GetItemIconBGOnHover(GameObject go, bool bHover)
		{
			if (bHover)
			{
				this.m_ItemNameBase.GameObject.SetActive(true);
				this.m_ItemNameBase.GameObject.transform.localPosition = new Vector3(go.transform.localPosition.x - 28f, this.m_ItemNameBase.GameObject.transform.localPosition.y, this.m_ItemNameBase.GameObject.transform.localPosition.z);
				int iIndex = go.GetComponent<ImageData>().m_iIndex;
				this.m_LabelItemName.Text = this.m_ItemNameList[iIndex];
				int num = (int)this.m_LabelItemName.GetComponent<UILabel>().printedSize.x;
				this.m_ItemNameBase.GameObject.GetComponent<UITexture>().width = num + 60;
			}
			else
			{
				this.m_ItemNameBase.GameObject.SetActive(false);
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0008D644 File Offset: 0x0008B844
		private void Update()
		{
			if (this.m_bStart)
			{
				if (this.m_fOverTime - Time.time > 0f)
				{
					float fillAmount = (this.m_fOverTime - Time.time) / 90f;
					this.m_BarTime.GetComponent<UISprite>().fillAmount = fillAmount;
				}
				else if (this.m_fOverTime - Time.time <= 0f)
				{
					this.m_bStart = false;
					this.SetEnd();
				}
			}
		}

		// Token: 0x04001339 RID: 4921
		private List<Control> m_AnswerBackgroundList;

		// Token: 0x0400133A RID: 4922
		private List<Control> m_AnswerIconList;

		// Token: 0x0400133B RID: 4923
		private List<Control> m_LabelAnswer1List;

		// Token: 0x0400133C RID: 4924
		private List<Control> m_LabelAnswer2List;

		// Token: 0x0400133D RID: 4925
		private List<Control> m_LabelAnswer3List;

		// Token: 0x0400133E RID: 4926
		private List<Control> m_LabelAnswer4List;

		// Token: 0x0400133F RID: 4927
		private List<Control> m_SeasoningsIconList;

		// Token: 0x04001340 RID: 4928
		private List<Control> m_LabelHoldList;

		// Token: 0x04001341 RID: 4929
		private List<UICooking.SpoonNode> m_SpoonNodeList;

		// Token: 0x04001342 RID: 4930
		private Control m_LabelContainer1Max;

		// Token: 0x04001343 RID: 4931
		private Control m_LabelContainer1Now;

		// Token: 0x04001344 RID: 4932
		private Control m_LabelContainer2Max;

		// Token: 0x04001345 RID: 4933
		private Control m_LabelContainer2Now;

		// Token: 0x04001346 RID: 4934
		private Control m_LabelContainer3Max;

		// Token: 0x04001347 RID: 4935
		private Control m_LabelContainer3Now;

		// Token: 0x04001348 RID: 4936
		private Control m_GameExplanation;

		// Token: 0x04001349 RID: 4937
		private Control m_BtnRecover;

		// Token: 0x0400134A RID: 4938
		private Control m_SpoonGroup;

		// Token: 0x0400134B RID: 4939
		private Control m_TalkLabel;

		// Token: 0x0400134C RID: 4940
		private Control m_TalkGroup;

		// Token: 0x0400134D RID: 4941
		private Control m_TalkIcon;

		// Token: 0x0400134E RID: 4942
		private Control m_BarTime;

		// Token: 0x0400134F RID: 4943
		private Control m_EndBackground;

		// Token: 0x04001350 RID: 4944
		private Control m_BtnClose;

		// Token: 0x04001351 RID: 4945
		private Control m_UpIcon1;

		// Token: 0x04001352 RID: 4946
		private Control m_UpIcon2;

		// Token: 0x04001353 RID: 4947
		private Control m_SeasoningsGroup;

		// Token: 0x04001354 RID: 4948
		private Control m_LabelExplanation;

		// Token: 0x04001355 RID: 4949
		private Control m_BtnLeave;

		// Token: 0x04001356 RID: 4950
		private Control m_Group;

		// Token: 0x04001357 RID: 4951
		private Control m_CookMask;

		// Token: 0x04001358 RID: 4952
		private Control m_SeasoningsMask;

		// Token: 0x04001359 RID: 4953
		private Control m_SpoonIcon1;

		// Token: 0x0400135A RID: 4954
		private Control m_SpoonIcon2;

		// Token: 0x0400135B RID: 4955
		private Control m_SpoonIcon3;

		// Token: 0x0400135C RID: 4956
		private Control m_ItemNameBase;

		// Token: 0x0400135D RID: 4957
		private Control m_LabelItemName;

		// Token: 0x0400135E RID: 4958
		private List<Control> m_LabelQuestion1List;

		// Token: 0x0400135F RID: 4959
		private List<Control> m_LabelQuestion2List;

		// Token: 0x04001360 RID: 4960
		private List<Control> m_LabelQuestion3List;

		// Token: 0x04001361 RID: 4961
		private List<Control> m_LabelQuestion4List;

		// Token: 0x04001362 RID: 4962
		private List<Control> m_SeasoningsBackgroundList;

		// Token: 0x04001363 RID: 4963
		private List<Control> m_SelectBackgroundList;

		// Token: 0x04001364 RID: 4964
		private List<Control> m_SpoonBackgroundList;

		// Token: 0x04001365 RID: 4965
		private List<Control> m_SpoonSelectBackgroundList;

		// Token: 0x04001366 RID: 4966
		private List<Control> m_BtnSelectChangeList;

		// Token: 0x04001367 RID: 4967
		private List<UICooking.CookingNode> m_CookingNodeList;

		// Token: 0x04001368 RID: 4968
		private List<Control> m_FailIconList;

		// Token: 0x04001369 RID: 4969
		private List<Control> m_OkIconList;

		// Token: 0x0400136A RID: 4970
		private List<string> m_ItemNameList;

		// Token: 0x0400136B RID: 4971
		private List<Control> m_LabelNewNumericalList;

		// Token: 0x0400136C RID: 4972
		private List<Control> m_LabelOldNumericalList;

		// Token: 0x0400136D RID: 4973
		private List<Control> m_GetItemIconList;

		// Token: 0x0400136E RID: 4974
		private List<Control> m_GetItemIconBGList;

		// Token: 0x0400136F RID: 4975
		private List<BookNode> m_BookNodeList;

		// Token: 0x04001370 RID: 4976
		private Control m_BtnStart;

		// Token: 0x04001371 RID: 4977
		private int m_iSpoonIndex;

		// Token: 0x04001372 RID: 4978
		private bool m_bClose;

		// Token: 0x04001373 RID: 4979
		private bool m_bStart;

		// Token: 0x04001374 RID: 4980
		private bool m_bSpoonCheck;

		// Token: 0x04001375 RID: 4981
		private bool m_bSeasonings;

		// Token: 0x04001376 RID: 4982
		private float m_fOverTime;

		// Token: 0x04001377 RID: 4983
		private int m_iGet;

		// Token: 0x04001378 RID: 4984
		private float m_fGameTime;

		// Token: 0x04001379 RID: 4985
		private bool m_bEnd;

		// Token: 0x020002FD RID: 765
		public class SpoonNode
		{
			// Token: 0x0400137B RID: 4987
			public int m_iIndex;

			// Token: 0x0400137C RID: 4988
			public int m_iContainerNow;

			// Token: 0x0400137D RID: 4989
			public int m_iContainerMax;

			// Token: 0x0400137E RID: 4990
			public int m_iType;
		}

		// Token: 0x020002FE RID: 766
		public class CookingNode
		{
			// Token: 0x0400137F RID: 4991
			public bool m_bComplete;

			// Token: 0x04001380 RID: 4992
			public int m_iIndex;

			// Token: 0x04001381 RID: 4993
			public int m_iNowSalt;

			// Token: 0x04001382 RID: 4994
			public int m_iNowMSG;

			// Token: 0x04001383 RID: 4995
			public int m_iNowChili;

			// Token: 0x04001384 RID: 4996
			public int m_iMaxSalt;

			// Token: 0x04001385 RID: 4997
			public int m_iMaxMSG;

			// Token: 0x04001386 RID: 4998
			public int m_iMaxChili;

			// Token: 0x04001387 RID: 4999
			public int m_iItemID;
		}
	}
}
