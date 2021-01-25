using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002E6 RID: 742
	public class UIAchievement : UILayer
	{
		// Token: 0x06000F2D RID: 3885 RVA: 0x0000A3E0 File Offset: 0x000085E0
		protected override void Awake()
		{
			this.m_AchiBackgroundList = new List<Control>();
			this.m_AchiImageList = new List<Control>();
			this.m_AchiImageMaskList = new List<Control>();
			this.m_LabelAchiNameList = new List<Control>();
			this.m_BtnTypeList = new List<Control>();
			base.Awake();
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0007EA34 File Offset: 0x0007CC34
		private void Start()
		{
			this.m_AchievementKindNode = null;
			this.m_LabelBigAchiName.Text = string.Empty;
			this.m_LabelDesc.Text = string.Empty;
			this.m_LabelTitle.Text = string.Empty;
			this.m_iCountOnePage = 7;
			this.m_iType = 0;
			this.m_iNowPage = 0;
			this.m_iIndex = 0;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0007EA94 File Offset: 0x0007CC94
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIAchievement.<>f__switch$map4 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(18);
					dictionary.Add("BackGround", 0);
					dictionary.Add("AchievementBackground", 1);
					dictionary.Add("AchiImage", 2);
					dictionary.Add("AchiImageMask", 3);
					dictionary.Add("LabelAchiName", 4);
					dictionary.Add("BtnExit", 5);
					dictionary.Add("BtnMove", 6);
					dictionary.Add("BtnNext", 7);
					dictionary.Add("BtnPrev", 8);
					dictionary.Add("BtnType", 9);
					dictionary.Add("LabelBigAchiName", 10);
					dictionary.Add("LabelDesc", 11);
					dictionary.Add("LabelFinishCount", 12);
					dictionary.Add("LabelMaxCount", 13);
					dictionary.Add("LabelMaxPage", 14);
					dictionary.Add("LabelPage", 15);
					dictionary.Add("LabelTitle", 16);
					dictionary.Add("ImageTexture", 17);
					UIAchievement.<>f__switch$map4 = dictionary;
				}
				int num;
				if (UIAchievement.<>f__switch$map4.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_BackGround = sender;
						break;
					case 1:
					{
						Control control = sender;
						control.OnClick += this.AchievementBackgroundOnClick;
						this.m_AchiBackgroundList.Add(control);
						break;
					}
					case 2:
					{
						Control control = sender;
						this.m_AchiImageList.Add(control);
						break;
					}
					case 3:
					{
						Control control = sender;
						this.m_AchiImageMaskList.Add(control);
						break;
					}
					case 4:
					{
						Control control = sender;
						this.m_LabelAchiNameList.Add(control);
						break;
					}
					case 5:
						this.m_BtnExit = sender;
						this.m_BtnExit.OnClick += this.BtnExitOnClick;
						break;
					case 6:
						this.m_BtnMove = sender;
						this.m_BtnMove.OnClick += this.BtnMoveOnClick;
						break;
					case 7:
						this.m_BtnNext = sender;
						this.m_BtnNext.OnClick += this.BtnNextOnClick;
						break;
					case 8:
						this.m_BtnPrev = sender;
						this.m_BtnPrev.OnClick += this.BtnPrevOnClick;
						break;
					case 9:
					{
						Control control = sender;
						control.OnClick += this.BtnTypeOnClick;
						this.m_BtnTypeList.Add(control);
						break;
					}
					case 10:
						this.m_LabelBigAchiName = sender;
						break;
					case 11:
						this.m_LabelDesc = sender;
						break;
					case 12:
						this.m_LabelFinishCount = sender;
						break;
					case 13:
						this.m_LabelMaxCount = sender;
						break;
					case 14:
						this.m_LabelMaxPage = sender;
						break;
					case 15:
						this.m_LabelPage = sender;
						break;
					case 16:
						this.m_LabelTitle = sender;
						break;
					case 17:
						this.m_ImageTexture = sender;
						this.m_ImageTexture.OnClick += this.ImageTextureOnClick;
						break;
					}
				}
			}
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0000A41F File Offset: 0x0000861F
		public override void Show()
		{
			this.m_BackGround.GameObject.SetActive(true);
			this.CalculateAchi();
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0007EDF8 File Offset: 0x0007CFF8
		private void CalculateAchi()
		{
			this.m_LabelTitle.Text = Game.StringTable.GetString(911143);
			this.m_AchievementKindNode = Game.Achievement.GetAchiKindNode(this.m_iType);
			this.m_LabelMaxCount.Text = this.m_AchievementKindNode.m_AchiDataNodeList.Count.ToString();
			this.m_LabelFinishCount.Text = this.m_AchievementKindNode.m_iFinishCount.ToString();
			this.m_iMaxPage = this.m_AchievementKindNode.m_AchiDataNodeList.Count / this.m_iCountOnePage;
			if (this.m_AchievementKindNode.m_AchiDataNodeList.Count % this.m_iCountOnePage != 0)
			{
				this.m_iMaxPage++;
			}
			this.m_LabelMaxPage.Text = this.m_iMaxPage.ToString();
			this.m_LabelPage.Text = (this.m_iNowPage + 1).ToString();
			this.ReSetVisible();
			this.SetAchiNodeData();
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0007EEF8 File Offset: 0x0007D0F8
		private void ReSetVisible()
		{
			this.m_BtnNext.GameObject.SetActive(false);
			this.m_BtnPrev.GameObject.SetActive(false);
			if (this.m_iNowPage + 1 != this.m_iMaxPage)
			{
				this.m_BtnNext.GameObject.SetActive(true);
			}
			if (this.m_iNowPage != 0)
			{
				this.m_BtnPrev.GameObject.SetActive(true);
			}
			for (int i = 0; i < this.m_AchiBackgroundList.Count; i++)
			{
				this.m_AchiBackgroundList[i].GameObject.SetActive(false);
			}
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0007EF9C File Offset: 0x0007D19C
		private void SetAchiNodeData()
		{
			for (int i = 0; i < this.m_iCountOnePage; i++)
			{
				int num = this.m_iNowPage * this.m_iCountOnePage + i;
				if (num > this.m_AchievementKindNode.m_AchiDataNodeList.Count - 1)
				{
					break;
				}
				Texture2D texture2D = null;
				if (!this.m_AchievementKindNode.m_AchiDataNodeList[num].m_strUIImage.Equals("0"))
				{
					texture2D = (Game.g_Achievement.Load("2dtexture/gameui/achievement/" + this.m_AchievementKindNode.m_AchiDataNodeList[num].m_strUIImage) as Texture2D);
				}
				for (int j = 0; j < this.m_AchiBackgroundList.Count; j++)
				{
					if (this.m_AchiBackgroundList[j].GetComponent<ImageData>().m_iIndex == i)
					{
						this.m_AchiBackgroundList[j].GameObject.SetActive(true);
					}
				}
				for (int j = 0; j < this.m_AchiImageList.Count; j++)
				{
					if (this.m_AchiImageList[j].GetComponent<ImageData>().m_iIndex == i)
					{
						if (texture2D != null)
						{
							this.m_AchiImageList[j].GetComponent<UITexture>().mainTexture = texture2D;
						}
						else
						{
							Debug.LogError("沒有成就圖");
						}
						break;
					}
				}
				for (int j = 0; j < this.m_AchiImageMaskList.Count; j++)
				{
					if (this.m_AchiImageMaskList[j].GetComponent<ImageData>().m_iIndex == i)
					{
						if (texture2D != null)
						{
							this.m_AchiImageMaskList[j].GetComponent<UITexture>().mainTexture = texture2D;
						}
						else
						{
							Debug.LogError("沒有成就圖");
						}
						if (this.m_AchievementKindNode.m_AchiDataNodeList[num].m_iOpenType == 1)
						{
							this.m_AchiImageMaskList[j].GameObject.SetActive(false);
						}
						else if (this.m_AchievementKindNode.m_AchiDataNodeList[num].m_iOpenType == 0)
						{
							this.m_AchiImageMaskList[j].GameObject.SetActive(true);
						}
					}
				}
				for (int j = 0; j < this.m_LabelAchiNameList.Count; j++)
				{
					if (this.m_LabelAchiNameList[j].GetComponent<LabelData>().m_iIndex == i)
					{
						this.m_LabelAchiNameList[j].Text = this.m_AchievementKindNode.m_AchiDataNodeList[num].m_strAchName;
					}
				}
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x0007F238 File Offset: 0x0007D438
		private void BtnTypeOnClick(GameObject go)
		{
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.m_iType = iBtnID;
			this.m_iNowPage = 0;
			this.m_LabelBigAchiName.Text = string.Empty;
			this.m_LabelDesc.Text = string.Empty;
			this.m_BtnMove.GameObject.SetActive(false);
			this.m_AchievementKindNode = Game.Achievement.GetAchiKindNode(iBtnID);
			if (iBtnID == 0)
			{
				this.m_LabelTitle.Text = Game.StringTable.GetString(911143);
			}
			else if (iBtnID == 1)
			{
				this.m_LabelTitle.Text = Game.StringTable.GetString(911144);
			}
			else if (iBtnID == 2)
			{
				this.m_LabelTitle.Text = Game.StringTable.GetString(911145);
			}
			this.CalculateAchi();
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0007F314 File Offset: 0x0007D514
		private void BtnNextOnClick(GameObject go)
		{
			this.m_iNowPage++;
			this.m_LabelPage.Text = (this.m_iNowPage + 1).ToString();
			this.ReSetVisible();
			this.SetAchiNodeData();
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x0007F358 File Offset: 0x0007D558
		private void BtnPrevOnClick(GameObject go)
		{
			this.m_iNowPage--;
			this.m_LabelPage.Text = (this.m_iNowPage + 1).ToString();
			this.ReSetVisible();
			this.SetAchiNodeData();
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0000A438 File Offset: 0x00008638
		private void BtnExitOnClick(GameObject go)
		{
			this.m_BackGround.GameObject.SetActive(false);
			this.m_iType = 0;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x0000A452 File Offset: 0x00008652
		private void ImageTextureOnClick(GameObject go)
		{
			this.m_ImageTexture.GameObject.SetActive(false);
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0007F39C File Offset: 0x0007D59C
		private void AchievementBackgroundOnClick(GameObject go)
		{
			this.m_iIndex = go.GetComponent<ImageData>().m_iIndex;
			int num = this.m_iNowPage * this.m_iCountOnePage + this.m_iIndex;
			if (this.m_AchievementKindNode.m_AchiDataNodeList[num].m_iOpenType == 0)
			{
				this.m_LabelBigAchiName.Text = string.Empty;
				this.m_LabelDesc.Text = string.Empty;
				this.m_BtnMove.GameObject.SetActive(false);
				return;
			}
			this.m_LabelBigAchiName.Text = this.m_AchievementKindNode.m_AchiDataNodeList[num].m_strAchName;
			this.m_LabelDesc.Text = this.m_AchievementKindNode.m_AchiDataNodeList[num].m_strAchExplain;
			if (this.m_AchievementKindNode.m_AchiDataNodeList[num].m_strEndMove.CompareTo("0") != 0)
			{
				this.m_BtnMove.GameObject.SetActive(true);
			}
			else
			{
				this.m_BtnMove.GameObject.SetActive(false);
			}
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0007F4AC File Offset: 0x0007D6AC
		private void BtnMoveOnClick(GameObject go)
		{
			int num = this.m_iNowPage * this.m_iCountOnePage + this.m_iIndex;
			string strEndMove = this.m_AchievementKindNode.m_AchiDataNodeList[num].m_strEndMove;
			int iCheckType = this.m_AchievementKindNode.m_AchiDataNodeList[num].m_iCheckType;
			if (iCheckType == 0 || iCheckType == 1)
			{
				int endingId = int.Parse(strEndMove);
				if (Game.UI.Get<UIEnd>() == null)
				{
					Game.UI.CreateUI("cFormEnd");
				}
				if (Game.UI.Get<UIEnd>() != null)
				{
					Game.UI.Get<UIEnd>().PlayEnd(endingId, 1);
				}
			}
			else if (iCheckType == 10)
			{
				if (this.m_ImageTexture != null)
				{
					Texture2D mainTexture = Game.g_DevelopTalkEvent.Load("2dtexture/gameui/develop/developtalkevent/" + strEndMove) as Texture2D;
					this.m_ImageTexture.GetComponent<UITexture>().mainTexture = mainTexture;
					this.m_ImageTexture.GameObject.SetActive(true);
				}
			}
			else if (this.m_ImageTexture != null)
			{
				Texture2D mainTexture2 = Game.g_Achievement.Load("2dtexture/gameui/achievement/" + strEndMove) as Texture2D;
				this.m_ImageTexture.GetComponent<UITexture>().mainTexture = mainTexture2;
				this.m_ImageTexture.GetComponent<UIWidget>().MakePixelPerfect();
				this.m_ImageTexture.GameObject.SetActive(true);
			}
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x0400120C RID: 4620
		private Control m_BackGround;

		// Token: 0x0400120D RID: 4621
		private List<Control> m_AchiBackgroundList;

		// Token: 0x0400120E RID: 4622
		private List<Control> m_AchiImageList;

		// Token: 0x0400120F RID: 4623
		private List<Control> m_AchiImageMaskList;

		// Token: 0x04001210 RID: 4624
		private List<Control> m_LabelAchiNameList;

		// Token: 0x04001211 RID: 4625
		private Control m_BtnExit;

		// Token: 0x04001212 RID: 4626
		private Control m_BtnMove;

		// Token: 0x04001213 RID: 4627
		private Control m_BtnNext;

		// Token: 0x04001214 RID: 4628
		private Control m_BtnPrev;

		// Token: 0x04001215 RID: 4629
		private List<Control> m_BtnTypeList;

		// Token: 0x04001216 RID: 4630
		private Control m_LabelBigAchiName;

		// Token: 0x04001217 RID: 4631
		private Control m_LabelDesc;

		// Token: 0x04001218 RID: 4632
		private Control m_LabelFinishCount;

		// Token: 0x04001219 RID: 4633
		private Control m_LabelMaxCount;

		// Token: 0x0400121A RID: 4634
		private Control m_LabelMaxPage;

		// Token: 0x0400121B RID: 4635
		private Control m_LabelPage;

		// Token: 0x0400121C RID: 4636
		private Control m_LabelTitle;

		// Token: 0x0400121D RID: 4637
		private Control m_ImageTexture;

		// Token: 0x0400121E RID: 4638
		private AchievementKindNode m_AchievementKindNode;

		// Token: 0x0400121F RID: 4639
		private int m_iNowPage;

		// Token: 0x04001220 RID: 4640
		private int m_iMaxPage;

		// Token: 0x04001221 RID: 4641
		private int m_iCountOnePage;

		// Token: 0x04001222 RID: 4642
		private int m_iType;

		// Token: 0x04001223 RID: 4643
		private int m_iIndex;
	}
}
