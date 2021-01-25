using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002E7 RID: 743
	public class UIAlchemy : UILayer
	{
		// Token: 0x06000F3D RID: 3901 RVA: 0x0000A465 File Offset: 0x00008665
		protected override void Awake()
		{
			this.m_BtnCastingList = new List<Control>();
			this.m_LabelCastingNameList = new List<Control>();
			this.m_SelectBoxList = new List<Control>();
			this.m_Colloder = base.collider;
			base.Awake();
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0000A49A File Offset: 0x0000869A
		private void Start()
		{
			Game.UI.Remove(this);
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0007F614 File Offset: 0x0007D814
		public void GameOpen()
		{
			this.m_AlchemyBowl.GameObject.SetActive(false);
			this.m_EndBackground.GameObject.SetActive(false);
			this.m_StartGroup.GameObject.SetActive(true);
			this.m_Group.GameObject.SetActive(true);
			this.m_Quit.GameObject.transform.localPosition = new Vector3(888f, 434f);
			this.m_Colloder.enabled = true;
			this.m_iPage = 0;
			this.m_fPromptTime = 0f;
			this.GameStart();
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0007F6B0 File Offset: 0x0007D8B0
		[ContextMenu("GameStart")]
		private void GameStart()
		{
			this.m_BookNodeList = Game.AbilityBookData.GetBookNodeList(this.iType);
			this.iNowSkill = 0;
			if (this.m_BookNodeList == null)
			{
				return;
			}
			this.m_iMaxPage = this.m_BookNodeList.Count / 6;
			if (this.m_BookNodeList.Count % 6 != 0)
			{
				this.m_iMaxPage++;
			}
			if (this.m_iMaxPage == 0)
			{
				this.m_iMaxPage = 1;
			}
			if (this.m_BookNodeList == null)
			{
				Debug.LogError("m_BookNodeList = Null");
				return;
			}
			this.CheckBtn();
			this.SetCastingData();
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0007F750 File Offset: 0x0007D950
		private void SetCastingData()
		{
			for (int i = 0; i < 6; i++)
			{
				int num = i + this.m_iPage * 6;
				if (num >= this.m_BookNodeList.Count)
				{
					for (int j = 0; j < this.m_BtnCastingList.Count; j++)
					{
						if (this.m_BtnCastingList[j].GetComponent<BtnData>().m_iBtnID == i)
						{
							this.m_BtnCastingList[j].GameObject.SetActive(false);
						}
					}
				}
				else
				{
					for (int k = 0; k < this.m_BtnCastingList.Count; k++)
					{
						if (this.m_BtnCastingList[k].GetComponent<BtnData>().m_iBtnID == i)
						{
							this.m_BtnCastingList[k].GameObject.SetActive(true);
							this.m_BtnCastingList[k].GetComponent<BtnData>().m_iBtnType = this.m_BookNodeList[num].m_iID;
							AlchemyProduceNode alchemyProduceNode = Game.AlchemyData.GetAlchemyProduceNode(this.m_BookNodeList[num].m_iID);
							if (alchemyProduceNode != null)
							{
								Texture2D texture2D = Game.g_Item.Load("2dtexture/gameui/item/" + alchemyProduceNode.m_strIcon) as Texture2D;
								if (texture2D == null)
								{
									texture2D = (Game.g_Item.Load("2dtexture/gameui/item/i00409") as Texture2D);
								}
								this.m_BtnCastingList[k].GetComponent<UITexture>().mainTexture = texture2D;
								bool flag = true;
								if (this.iNowSkill < alchemyProduceNode.m_iRequestSkill)
								{
									flag = false;
								}
								for (int l = 0; l < alchemyProduceNode.m_MaterialNodeList.Count; l++)
								{
									if (!flag)
									{
										break;
									}
									int num2 = 0;
									if (alchemyProduceNode.m_MaterialNodeList[l].m_iAmount > num2)
									{
										flag = false;
									}
								}
								if (!flag)
								{
									this.m_BtnCastingList[k].GetComponent<UITexture>().color = new Color(0.25f, 0.25f, 0.25f, 1f);
								}
								else
								{
									this.m_BtnCastingList[k].GetComponent<UITexture>().color = new Color(1f, 1f, 1f, 1f);
								}
							}
						}
					}
					for (int k = 0; k < this.m_LabelCastingNameList.Count; k++)
					{
						if (this.m_LabelCastingNameList[k].GetComponent<LabelData>().m_iIndex == i)
						{
							this.m_LabelCastingNameList[k].Text = this.m_BookNodeList[num].m_strAbilityID;
						}
					}
				}
			}
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0007FA04 File Offset: 0x0007DC04
		private void CheckBtn()
		{
			if (this.m_iMaxPage > 1)
			{
				if (this.m_iPage + 1 == this.m_iMaxPage)
				{
					this.m_BtnNext.GameObject.SetActive(false);
				}
				else
				{
					this.m_BtnNext.GameObject.SetActive(true);
				}
				if (this.m_iPage != 0)
				{
					this.m_BtnUp.GameObject.SetActive(true);
				}
				else
				{
					this.m_BtnUp.GameObject.SetActive(false);
				}
			}
			else
			{
				this.m_BtnNext.GameObject.SetActive(false);
				this.m_BtnUp.GameObject.SetActive(false);
			}
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0007FAB0 File Offset: 0x0007DCB0
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIAlchemy.<>f__switch$map5 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(24);
					dictionary.Add("Group", 0);
					dictionary.Add("AlchemyBowl", 1);
					dictionary.Add("StartGroup", 2);
					dictionary.Add("BtnCasting", 3);
					dictionary.Add("LabelCastingName", 4);
					dictionary.Add("SelectBox", 5);
					dictionary.Add("EndBackground", 6);
					dictionary.Add("BtnClose", 7);
					dictionary.Add("GetItemIcon", 8);
					dictionary.Add("GetItemIconBG", 9);
					dictionary.Add("LabelSkill", 10);
					dictionary.Add("LabelNewNumerical", 11);
					dictionary.Add("LabelOldNumerical", 12);
					dictionary.Add("LabelItemAmount", 13);
					dictionary.Add("UpIcon1", 14);
					dictionary.Add("UpIcon2", 15);
					dictionary.Add("LabelPrompt", 16);
					dictionary.Add("LabelSelectProduct", 17);
					dictionary.Add("BtnNext", 18);
					dictionary.Add("BtnUp", 19);
					dictionary.Add("BtnReturn", 20);
					dictionary.Add("ItemNameBase", 21);
					dictionary.Add("LabelItemName", 22);
					dictionary.Add("Quit", 23);
					UIAlchemy.<>f__switch$map5 = dictionary;
				}
				int num;
				if (UIAlchemy.<>f__switch$map5.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_AlchemyBowl = sender;
						break;
					case 2:
						this.m_StartGroup = sender;
						break;
					case 3:
					{
						Control control = sender;
						control.OnHover += this.BtnCastingOnHover;
						control.OnClick += this.BtnCastingOnClick;
						this.m_BtnCastingList.Add(control);
						break;
					}
					case 4:
					{
						Control control = sender;
						this.m_LabelCastingNameList.Add(control);
						break;
					}
					case 5:
					{
						Control control = sender;
						this.m_SelectBoxList.Add(control);
						break;
					}
					case 6:
						this.m_EndBackground = sender;
						break;
					case 7:
						this.m_BtnClose = sender;
						this.m_BtnClose.OnClick += this.BtnCloseOnClick;
						break;
					case 8:
						if (sender.gameObject.GetComponent<ImageData>().m_iIndex == 0)
						{
							this.m_GetItemIcon = sender;
						}
						break;
					case 9:
						if (sender.gameObject.GetComponent<ImageData>().m_iIndex == 0)
						{
							this.m_GetItemIconBG = sender;
							this.m_GetItemIconBG.OnHover += this.GetItemIconBGOnHover;
						}
						break;
					case 10:
						if (sender.gameObject.GetComponent<LabelData>().m_iIndex == 0)
						{
							this.m_LabelSkill1 = sender;
						}
						if (sender.gameObject.GetComponent<LabelData>().m_iIndex == 1)
						{
							this.m_LabelSkill2 = sender;
						}
						break;
					case 11:
						if (sender.gameObject.GetComponent<LabelData>().m_iIndex == 0)
						{
							this.m_LabelNewNumerical1 = sender;
						}
						if (sender.gameObject.GetComponent<LabelData>().m_iIndex == 1)
						{
							this.m_LabelNewNumerical2 = sender;
						}
						break;
					case 12:
						if (sender.gameObject.GetComponent<LabelData>().m_iIndex == 0)
						{
							this.m_LabelOldNumerical1 = sender;
						}
						if (sender.gameObject.GetComponent<LabelData>().m_iIndex == 1)
						{
							this.m_LabelOldNumerical2 = sender;
						}
						break;
					case 13:
						if (sender.gameObject.GetComponent<LabelData>().m_iIndex == 0)
						{
							this.m_LabelItemAmount = sender;
						}
						break;
					case 14:
						this.m_UpIcon1 = sender;
						break;
					case 15:
						this.m_UpIcon2 = sender;
						break;
					case 16:
						this.m_LabelPrompt = sender;
						break;
					case 17:
						this.m_LabelSelectProduct = sender;
						break;
					case 18:
						this.m_BtnNext = sender;
						this.m_BtnNext.OnClick += this.BtnNextOnClick;
						break;
					case 19:
						this.m_BtnUp = sender;
						this.m_BtnUp.OnClick += this.BtnUpOnClick;
						break;
					case 20:
						this.m_BtnReturn = sender;
						this.m_BtnReturn.OnClick += this.BtnCloseOnClick;
						break;
					case 21:
						this.m_ItemNameBase = sender;
						break;
					case 22:
						this.m_LabelItemName = sender;
						break;
					case 23:
						this.m_Quit = sender;
						break;
					}
				}
			}
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0007FFB8 File Offset: 0x0007E1B8
		private void GetItemIconBGOnHover(GameObject go, bool bHover)
		{
			if (bHover)
			{
				this.m_ItemNameBase.GameObject.SetActive(true);
				this.m_ItemNameBase.GameObject.transform.localPosition = new Vector3(go.transform.localPosition.x, this.m_ItemNameBase.GameObject.transform.localPosition.y, this.m_ItemNameBase.GameObject.transform.localPosition.z);
				int num = (int)this.m_LabelItemName.GetComponent<UILabel>().printedSize.x;
				this.m_ItemNameBase.GameObject.GetComponent<UITexture>().width = num + 30;
			}
			else
			{
				this.m_ItemNameBase.GameObject.SetActive(false);
			}
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x00080090 File Offset: 0x0007E290
		private void BtnCastingOnHover(GameObject go, bool bHover)
		{
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			for (int i = 0; i < this.m_SelectBoxList.Count; i++)
			{
				this.m_SelectBoxList[i].GameObject.SetActive(false);
				if (bHover && this.m_SelectBoxList[i].GetComponent<ImageData>().m_iIndex == iBtnID)
				{
					this.m_SelectBoxList[i].GameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x00080118 File Offset: 0x0007E318
		private void BtnCastingOnClick(GameObject go)
		{
			this.m_iIndex = go.GetComponent<BtnData>().m_iBtnID + this.m_iPage * 6;
			this.m_AlchemyNode = Game.AlchemyData.GetAlchemyProduceNode(this.m_BookNodeList[this.m_iIndex].m_iID);
			if (this.m_AlchemyNode == null)
			{
				return;
			}
			int num = 0;
			if (this.iType == 0)
			{
				num = 0;
			}
			else if (this.iType == 2)
			{
				num = 0;
			}
			this.m_AlchemyScene = Game.AlchemyData.GetAlchemyScene(this.m_BookNodeList[this.m_iIndex].m_iID, num);
			this.m_AlchemyBowl.GameObject.SetActive(true);
			this.m_StartGroup.GameObject.SetActive(false);
			base.gameObject.GetComponent<AlchemyGameControl>().SetData(this.m_AlchemyScene);
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0000A4A7 File Offset: 0x000086A7
		private void BtnCloseOnClick(GameObject go)
		{
			this.m_Colloder.enabled = false;
			this.m_Group.GameObject.SetActive(false);
			Game.PrefabSmallGameEnd();
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0000A4CB File Offset: 0x000086CB
		private void BtnNextOnClick(GameObject go)
		{
			this.m_iPage++;
			this.CheckBtn();
			this.SetCastingData();
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0000A4E7 File Offset: 0x000086E7
		private void BtnUpOnClick(GameObject go)
		{
			this.m_iPage--;
			this.CheckBtn();
			this.SetCastingData();
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x000801F4 File Offset: 0x0007E3F4
		public void SetEnd(bool bWin)
		{
			this.m_AlchemyBowl.GameObject.SetActive(false);
			this.m_EndBackground.GameObject.SetActive(true);
			int num = 0;
			int num2 = 0;
			if (this.iType == 0)
			{
				num = 0;
				num2 = 0;
				this.m_LabelSkill1.Text = string.Format(Game.StringTable.GetString(300125), Game.StringTable.GetString(100036));
				this.m_LabelOldNumerical1.Text = num.ToString();
				this.m_LabelSkill2.Text = string.Format(Game.StringTable.GetString(300125), Game.StringTable.GetString(100043));
				this.m_LabelOldNumerical2.Text = num2.ToString();
			}
			else if (this.iType == 2)
			{
				num = 0;
				this.m_LabelSkill1.Text = string.Format(Game.StringTable.GetString(300125), Game.StringTable.GetString(100037));
				this.m_LabelOldNumerical1.Text = num.ToString();
			}
			if (bWin)
			{
				int num3 = Random.Range(1, this.m_AlchemyScene.m_iMaxItemCount + 1);
				this.m_LabelItemAmount.Text = num3.ToString();
				this.m_LabelItemName.Text = Game.ItemData.GetItemName(this.m_AlchemyScene.m_iSuccessItemID);
				string itemIconID = Game.ItemData.GetItemIconID(this.m_AlchemyScene.m_iSuccessItemID);
				if (Game.g_Item != null)
				{
					Texture2D texture2D = Game.g_Item.Load("2dtexture/gameui/item/" + itemIconID) as Texture2D;
					if (texture2D != null)
					{
						this.m_GetItemIcon.GetComponent<UITexture>().mainTexture = texture2D;
					}
					else
					{
						Debug.LogError(this.m_AlchemyScene.m_iSuccessItemID);
					}
				}
				this.m_GetItemIcon.GameObject.SetActive(true);
				this.m_GetItemIconBG.GameObject.SetActive(true);
				int num4 = 6;
				float num5 = 1f;
				num4 = Mathf.RoundToInt(num5 * (float)num4);
			}
			else
			{
				this.m_GetItemIcon.GameObject.SetActive(false);
				this.m_GetItemIconBG.GameObject.SetActive(false);
			}
			int num6 = 0;
			int num7 = 0;
			if (this.iType == 0)
			{
				num6 = 0;
				num7 = 0;
				this.m_LabelNewNumerical1.Text = num6.ToString();
				this.m_LabelNewNumerical2.Text = num7.ToString();
			}
			else if (this.iType == 2)
			{
				num6 = 0;
				this.m_LabelNewNumerical1.Text = num6.ToString();
			}
			if (num6 != num)
			{
				this.m_LabelNewNumerical1.GameObject.SetActive(true);
				this.m_UpIcon1.GameObject.SetActive(true);
			}
			else
			{
				this.m_LabelNewNumerical1.GameObject.SetActive(false);
				this.m_UpIcon1.GameObject.SetActive(false);
			}
			if (num7 != num2)
			{
				this.m_LabelNewNumerical2.GameObject.SetActive(true);
				this.m_UpIcon2.GameObject.SetActive(true);
			}
			else
			{
				this.m_LabelNewNumerical2.GameObject.SetActive(false);
				this.m_UpIcon2.GameObject.SetActive(false);
			}
		}

		// Token: 0x04001225 RID: 4645
		private Control m_AlchemyBowl;

		// Token: 0x04001226 RID: 4646
		private Control m_StartGroup;

		// Token: 0x04001227 RID: 4647
		private Control m_EndBackground;

		// Token: 0x04001228 RID: 4648
		private Control m_BtnClose;

		// Token: 0x04001229 RID: 4649
		private Control m_GetItemIcon;

		// Token: 0x0400122A RID: 4650
		private Control m_GetItemIconBG;

		// Token: 0x0400122B RID: 4651
		private Control m_LabelSkill1;

		// Token: 0x0400122C RID: 4652
		private Control m_LabelNewNumerical1;

		// Token: 0x0400122D RID: 4653
		private Control m_LabelOldNumerical1;

		// Token: 0x0400122E RID: 4654
		private Control m_LabelSkill2;

		// Token: 0x0400122F RID: 4655
		private Control m_LabelNewNumerical2;

		// Token: 0x04001230 RID: 4656
		private Control m_LabelOldNumerical2;

		// Token: 0x04001231 RID: 4657
		private Control m_UpIcon1;

		// Token: 0x04001232 RID: 4658
		private Control m_UpIcon2;

		// Token: 0x04001233 RID: 4659
		private Control m_LabelPrompt;

		// Token: 0x04001234 RID: 4660
		private Control m_LabelSelectProduct;

		// Token: 0x04001235 RID: 4661
		private Control m_BtnNext;

		// Token: 0x04001236 RID: 4662
		private Control m_BtnUp;

		// Token: 0x04001237 RID: 4663
		private Control m_BtnReturn;

		// Token: 0x04001238 RID: 4664
		private Control m_LabelItemAmount;

		// Token: 0x04001239 RID: 4665
		private Control m_ItemNameBase;

		// Token: 0x0400123A RID: 4666
		private Control m_LabelItemName;

		// Token: 0x0400123B RID: 4667
		private Control m_Group;

		// Token: 0x0400123C RID: 4668
		private Control m_Quit;

		// Token: 0x0400123D RID: 4669
		private int m_iPage;

		// Token: 0x0400123E RID: 4670
		private float m_fPromptTime;

		// Token: 0x0400123F RID: 4671
		private List<BookNode> m_BookNodeList;

		// Token: 0x04001240 RID: 4672
		private int m_iIndex;

		// Token: 0x04001241 RID: 4673
		private List<Control> m_BtnCastingList;

		// Token: 0x04001242 RID: 4674
		private List<Control> m_LabelCastingNameList;

		// Token: 0x04001243 RID: 4675
		private List<Control> m_SelectBoxList;

		// Token: 0x04001244 RID: 4676
		private Collider m_Colloder;

		// Token: 0x04001245 RID: 4677
		private int m_iMaxPage;

		// Token: 0x04001246 RID: 4678
		private AlchemyProduceNode m_AlchemyNode;

		// Token: 0x04001247 RID: 4679
		private AlchemyScene m_AlchemyScene;

		// Token: 0x04001248 RID: 4680
		public int iType;

		// Token: 0x04001249 RID: 4681
		public int iNowSkill;
	}
}
