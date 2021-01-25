using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000331 RID: 817
	public class UIAbility : UILayer
	{
		// Token: 0x06001232 RID: 4658 RVA: 0x0000BCFE File Offset: 0x00009EFE
		protected override void Awake()
		{
			this.m_CtrlAbility = base.GetComponent<CtrlAbility>();
			base.Awake();
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0000BD12 File Offset: 0x00009F12
		[ContextMenu("StartAlchemyTest")]
		private void StartAlchemyTest()
		{
			this.m_bTest = true;
			this.StartAlchemy();
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0000BD21 File Offset: 0x00009F21
		[ContextMenu("StartForgingTest")]
		private void StartForgingTest()
		{
			this.m_bTest = true;
			this.StartForging();
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x0009E72C File Offset: 0x0009C92C
		public void StartAlchemy()
		{
			if (this.m_bTest)
			{
				for (int i = 10401; i < 10411; i++)
				{
					Game.Variable["Ability_" + i] = 1;
				}
			}
			this.iType = 5;
			this.GameOpen();
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x0009E788 File Offset: 0x0009C988
		public void StartForging()
		{
			if (this.m_bTest)
			{
				for (int i = 100001; i < 100005; i++)
				{
					Game.Variable["Ability_" + i] = 1;
				}
			}
			this.iType = 6;
			this.GameOpen();
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0009E7E4 File Offset: 0x0009C9E4
		public void GameOpen()
		{
			base.EnterState(1);
			this.m_AlchemyBowl.GameObject.SetActive(false);
			this.m_EndBackground.GameObject.SetActive(false);
			this.m_StartGroup.GameObject.SetActive(true);
			this.m_Group.GameObject.SetActive(true);
			this.m_Colloder = this.m_Group.GameObject.collider;
			this.m_Colloder.enabled = true;
			this.m_iPage = 0;
			this.m_fPromptTime = 0f;
			this.Show();
			this.GameStart();
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0009E87C File Offset: 0x0009CA7C
		private void GameStart()
		{
			this.m_BookNodeList = Game.AbilityBookData.GetBookNodeList(this.iType);
			string key = "Ability_" + ((UIAbility.eAbilityType)this.iType).ToString();
			this.iNowSkill = Game.Variable[key];
			this.iNowSkill = Mathf.Clamp(this.iNowSkill, 0, 500);
			this.SetAbilityView((UIAbility.eAbilityType)this.iType);
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

		// Token: 0x06001239 RID: 4665 RVA: 0x0009E964 File Offset: 0x0009CB64
		public void SetAbilityView(UIAbility.eAbilityType type)
		{
			if (type != UIAbility.eAbilityType.Alchemy)
			{
				if (type == UIAbility.eAbilityType.Forging)
				{
					this.m_BackGroundList[0].GameObject.SetActive(false);
					this.m_BackGroundList[1].GameObject.SetActive(true);
					this.TargetBaseTile2.transform.localPosition = new Vector3(27f, -60f, 0f);
					this.m_TargetBG.SpriteName = "Blacksmith_004";
					this.m_LabelSelectProduct.Text = Game.StringTable.GetString(300601);
					this.setTable(2, 300621, 300610);
					this.m_TargetSelectLabel.Text = Game.StringTable.GetString(300622);
					this.m_TargetLabel.Text = Game.StringTable.GetString(300623);
				}
			}
			else
			{
				this.m_BackGroundList[0].GameObject.SetActive(true);
				this.m_BackGroundList[1].GameObject.SetActive(false);
				this.TargetBaseTile2.transform.localPosition = new Vector3(27f, -130f, 0f);
				this.m_TargetBG.SpriteName = "UI_medicine_008";
				this.m_LabelSelectProduct.Text = Game.StringTable.GetString(303001);
				this.setTable(0, 300621, 303013);
				this.m_TargetSelectLabel.Text = Game.StringTable.GetString(303004);
				this.m_TargetLabel.Text = Game.StringTable.GetString(303005);
			}
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0009EB14 File Offset: 0x0009CD14
		private void setTable(int idx, int title, int sid)
		{
			this.m_LevelTitle.Text = Game.StringTable.GetString(title);
			for (int i = 0; i < this.m_LevelIcon.Count; i++)
			{
				this.m_LevelIcon[i].SpriteName = string.Format("Tile-{0}{1:00}", idx, i + 1);
				this.m_LevelLabel[i].Text = Game.StringTable.GetString(sid + i);
			}
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0009EB9C File Offset: 0x0009CD9C
		public void SetCastingData()
		{
			int num = this.m_iPage * 6;
			int num2 = num + 6;
			for (int i = num; i < num2; i++)
			{
				int num3 = i - num;
				this.m_WgAbilityBtnCastingList[num3].Hide();
				if (i < this.m_BookNodeList.Count)
				{
					AlchemyProduceNode alchemyProduceNode = Game.AlchemyData.GetAlchemyProduceNode(this.m_BookNodeList[i].m_iID);
					if (alchemyProduceNode != null)
					{
						Texture2D texture2D = Game.g_Item.Load("2dtexture/gameui/item/" + alchemyProduceNode.m_strIcon) as Texture2D;
						if (texture2D == null)
						{
							texture2D = (Game.g_Item.Load("2dtexture/gameui/item/i00409") as Texture2D);
						}
						bool flag = true;
						if (this.iNowSkill < alchemyProduceNode.m_iRequestSkill)
						{
							flag = false;
						}
						for (int j = 0; j < alchemyProduceNode.m_MaterialNodeList.Count; j++)
						{
							if (!flag)
							{
								break;
							}
							int num4 = BackpackStatus.m_Instance.CheclItemAmount(alchemyProduceNode.m_MaterialNodeList[j].m_iMaterialID);
							if (alchemyProduceNode.m_MaterialNodeList[j].m_iAmount > num4)
							{
								flag = false;
							}
						}
						if (this.m_bTest)
						{
							flag = true;
						}
						this.m_WgAbilityBtnCastingList[num3].SetWgAbilityBtnCasting(texture2D, flag, this.m_BookNodeList[i].m_strAbilityID, alchemyProduceNode);
					}
				}
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x0009ED1C File Offset: 0x0009CF1C
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

		// Token: 0x0600123D RID: 4669 RVA: 0x0009EDC8 File Offset: 0x0009CFC8
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIAbility.<>f__switch$map24 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(27);
					dictionary.Add("Group", 0);
					dictionary.Add("BackGround", 1);
					dictionary.Add("TargetBG", 2);
					dictionary.Add("LevelTitle", 3);
					dictionary.Add("LevelIcon", 4);
					dictionary.Add("LevelLabel", 5);
					dictionary.Add("AlchemyBowl", 6);
					dictionary.Add("StartGroup", 7);
					dictionary.Add("EndBackground", 8);
					dictionary.Add("BtnClose", 9);
					dictionary.Add("GetItemIcon", 10);
					dictionary.Add("GetItemIconBG", 11);
					dictionary.Add("LabelSkill", 12);
					dictionary.Add("LabelNewNumerical", 13);
					dictionary.Add("LabelOldNumerical", 14);
					dictionary.Add("LabelItemAmount", 15);
					dictionary.Add("UpIcon1", 16);
					dictionary.Add("UpIcon2", 17);
					dictionary.Add("LabelPrompt", 18);
					dictionary.Add("LabelSelectProduct", 19);
					dictionary.Add("BtnNext", 20);
					dictionary.Add("BtnUp", 21);
					dictionary.Add("BtnReturn", 22);
					dictionary.Add("ItemNameBase", 23);
					dictionary.Add("LabelItemName", 24);
					dictionary.Add("TargetSelectLabel", 25);
					dictionary.Add("TargetLabel", 26);
					UIAbility.<>f__switch$map24 = dictionary;
				}
				int num;
				if (UIAbility.<>f__switch$map24.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
					{
						Control control = sender;
						this.m_BackGroundList.Add(control);
						break;
					}
					case 2:
						this.m_TargetBG = sender;
						break;
					case 3:
						this.m_LevelTitle = sender;
						break;
					case 4:
					{
						Control control = sender;
						this.m_LevelIcon.Add(control);
						break;
					}
					case 5:
					{
						Control control = sender;
						this.m_LevelLabel.Add(control);
						break;
					}
					case 6:
						this.m_AlchemyBowl = sender;
						break;
					case 7:
						this.m_StartGroup = sender;
						break;
					case 8:
						this.m_EndBackground = sender;
						break;
					case 9:
						this.m_BtnClose = sender;
						this.m_BtnClose.OnClick += this.BtnCloseOnClick;
						this.m_BtnClose.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						base.SetInputButton(3, this.m_BtnClose.Listener);
						break;
					case 10:
						if (sender.gameObject.GetComponent<ImageData>().m_iIndex == 0)
						{
							this.m_GetItemIcon = sender;
						}
						break;
					case 11:
						if (sender.gameObject.GetComponent<ImageData>().m_iIndex == 0)
						{
							this.m_GetItemIconBG = sender;
							this.m_GetItemIconBG.OnHover += this.GetItemIconBGOnHover;
						}
						break;
					case 12:
					{
						Control control = sender;
						control.GameObject.SetActive(false);
						this.m_LabelSkillList.Add(control);
						break;
					}
					case 13:
					{
						Control control = sender;
						control.GameObject.SetActive(false);
						this.m_LabelNewNumericalList.Add(control);
						break;
					}
					case 14:
					{
						Control control = sender;
						control.GameObject.SetActive(false);
						this.m_LabelOldNumericalList.Add(control);
						break;
					}
					case 15:
						if (sender.gameObject.GetComponent<LabelData>().m_iIndex == 0)
						{
							this.m_LabelItemAmount = sender;
						}
						break;
					case 16:
						this.m_UpIcon1 = sender;
						break;
					case 17:
						this.m_UpIcon2 = sender;
						break;
					case 18:
						this.m_LabelPrompt = sender;
						break;
					case 19:
						this.m_LabelSelectProduct = sender;
						break;
					case 20:
						this.m_BtnNext = sender;
						this.m_BtnNext.OnClick += this.BtnNextOnClick;
						break;
					case 21:
						this.m_BtnUp = sender;
						this.m_BtnUp.OnClick += this.BtnUpOnClick;
						break;
					case 22:
						this.m_BtnReturn = sender;
						this.m_BtnReturn.OnClick += this.BtnCloseOnClick;
						this.m_BtnReturn.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					case 23:
						this.m_ItemNameBase = sender;
						break;
					case 24:
						this.m_LabelItemName = sender;
						break;
					case 25:
						this.m_TargetSelectLabel = sender;
						break;
					case 26:
						this.m_TargetLabel = sender;
						break;
					}
				}
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0009F2D0 File Offset: 0x0009D4D0
		protected override void AssignWidget(Widget sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIAbility.<>f__switch$map25 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
					dictionary.Add("BtnCasting", 0);
					dictionary.Add("ItemTip", 1);
					UIAbility.<>f__switch$map25 = dictionary;
				}
				int num;
				if (UIAbility.<>f__switch$map25.TryGetValue(name, ref num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							this.m_WgAbilityItemTip = (sender as WgAbilityItemTip);
						}
					}
					else
					{
						WgAbilityBtnCasting wgAbilityBtnCasting = sender as WgAbilityBtnCasting;
						this.m_WgAbilityBtnCastingList.Add(wgAbilityBtnCasting);
					}
				}
			}
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x0009F368 File Offset: 0x0009D568
		public void SetItemTip(AlchemyProduceNode node, bool bOpen)
		{
			this.m_WgAbilityItemTip.SetActive(bOpen);
			if (node == null)
			{
				return;
			}
			AlchemyScene alchemyScene = Game.AlchemyData.GetAlchemyScene(node.m_iAbilityBookID, this.iNowSkill);
			if (alchemyScene == null)
			{
				return;
			}
			ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(alchemyScene.m_iSuccessItemID);
			if (itemDataNode == null)
			{
				return;
			}
			string strItemName = itemDataNode.m_strItemName;
			string text = string.Empty;
			string addEffectTip = string.Empty;
			string text2 = string.Empty;
			for (int i = 0; i < itemDataNode.m_ItmeEffectNodeList.Count; i++)
			{
				int iItemType = itemDataNode.m_ItmeEffectNodeList[i].m_iItemType;
				int iRecoverType = itemDataNode.m_ItmeEffectNodeList[i].m_iRecoverType;
				int iValue = itemDataNode.m_ItmeEffectNodeList[i].m_iValue;
				if (iItemType == 1)
				{
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
						string text3 = text;
						text = string.Concat(new object[]
						{
							text3,
							@string,
							" ",
							iValue,
							"  "
						});
					}
				}
				else if (iItemType == 7)
				{
					string @string = Game.StringTable.GetString(160004);
					ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(iRecoverType);
					addEffectTip = @string + ":\n" + conditionNode.m_strName;
				}
			}
			for (int j = 0; j < node.m_MaterialNodeList.Count; j++)
			{
				if (node.m_MaterialNodeList[j] != null)
				{
					int iAmount = node.m_MaterialNodeList[j].m_iAmount;
					ItemDataNode itemDataNode2 = Game.ItemData.GetItemDataNode(node.m_MaterialNodeList[j].m_iMaterialID);
					if (itemDataNode2 != null)
					{
						text2 += string.Format("{0} : {1} ", itemDataNode2.m_strItemName, iAmount);
					}
				}
			}
			this.m_WgAbilityItemTip.SetTip(strItemName, text, addEffectTip, text2);
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0009F5B4 File Offset: 0x0009D7B4
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

		// Token: 0x06001241 RID: 4673 RVA: 0x0009F68C File Offset: 0x0009D88C
		public void BtnCastingOnClick(AlchemyProduceNode node)
		{
			this.m_AlchemyNode = node;
			if (this.m_AlchemyNode == null)
			{
				return;
			}
			bool flag = true;
			if (!this.m_bTest)
			{
				if (this.iNowSkill < this.m_AlchemyNode.m_iRequestSkill)
				{
					flag = false;
				}
				if (!flag)
				{
					this.m_LabelPrompt.GameObject.SetActive(true);
					this.m_LabelPrompt.Text = Game.StringTable.GetString(210011);
					this.m_fPromptTime = Time.time;
					return;
				}
				for (int i = 0; i < this.m_AlchemyNode.m_MaterialNodeList.Count; i++)
				{
					if (!flag)
					{
						break;
					}
					int num = BackpackStatus.m_Instance.CheclItemAmount(this.m_AlchemyNode.m_MaterialNodeList[i].m_iMaterialID);
					if (num < this.m_AlchemyNode.m_MaterialNodeList[i].m_iAmount)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					this.m_LabelPrompt.GameObject.SetActive(true);
					this.m_LabelPrompt.Text = Game.StringTable.GetString(210012);
					this.m_fPromptTime = Time.time;
					return;
				}
				for (int j = 0; j < this.m_AlchemyNode.m_MaterialNodeList.Count; j++)
				{
					BackpackStatus.m_Instance.LessPackItem(this.m_AlchemyNode.m_MaterialNodeList[j].m_iMaterialID, this.m_AlchemyNode.m_MaterialNodeList[j].m_iAmount, null);
				}
			}
			this.m_AlchemyScene = Game.AlchemyData.GetAlchemyScene(this.m_AlchemyNode.m_iAbilityBookID, this.iNowSkill);
			this.m_AlchemyBowl.GameObject.SetActive(true);
			this.m_StartGroup.GameObject.SetActive(false);
			this.m_CtrlAbility.SetData(this.m_AlchemyScene);
			base.EnterState(2);
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x0000BD30 File Offset: 0x00009F30
		private void BtnCloseOnClick(GameObject go)
		{
			this.m_Colloder.enabled = false;
			this.m_Group.GameObject.SetActive(false);
			Game.PrefabSmallGameEnd();
			this.Hide();
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0000BD5A File Offset: 0x00009F5A
		private void BtnNextOnClick(GameObject go)
		{
			this.m_iPage++;
			this.CheckBtn();
			this.SetCastingData();
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x0000BD76 File Offset: 0x00009F76
		private void BtnUpOnClick(GameObject go)
		{
			this.m_iPage--;
			this.CheckBtn();
			this.SetCastingData();
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x0009F874 File Offset: 0x0009DA74
		public void SetEnd(bool bWin)
		{
			this.m_AlchemyBowl.GameObject.SetActive(false);
			this.m_EndBackground.GameObject.SetActive(true);
			if (this.iType == 5)
			{
				this.m_LabelSkillList[0].Text = Game.StringTable.GetString(303024);
			}
			if (this.iType == 6)
			{
				this.m_LabelSkillList[0].Text = Game.StringTable.GetString(300606);
			}
			this.m_LabelSkillList[0].GameObject.SetActive(true);
			this.m_LabelOldNumericalList[0].GameObject.SetActive(true);
			this.m_LabelOldNumericalList[0].Text = this.iNowSkill.ToString();
			int value;
			if (bWin)
			{
				int amount = Random.Range(1, this.m_AlchemyScene.m_iMaxItemCount + 1);
				BackpackStatus.m_Instance.AddPackItem(this.m_AlchemyScene.m_iSuccessItemID, amount, true);
				value = this.iNowSkill + 5;
				this.m_LabelItemAmount.Text = amount.ToString();
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
				int num = 6;
				float num2 = 1f;
				num = Mathf.RoundToInt(num2 * (float)num);
			}
			else
			{
				this.m_GetItemIcon.GameObject.SetActive(false);
				this.m_GetItemIconBG.GameObject.SetActive(false);
				value = this.iNowSkill + 2;
			}
			this.m_LabelNewNumericalList[0].GameObject.SetActive(true);
			this.m_UpIcon1.GameObject.SetActive(true);
			string key = "Ability_" + ((UIAbility.eAbilityType)this.iType).ToString();
			Game.Variable[key] = value;
			this.m_LabelNewNumericalList[0].Text = value.ToString();
			base.EnterState(3);
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x0000BD92 File Offset: 0x00009F92
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			GameGlobal.m_bCFormOpen = true;
			Game.g_InputManager.Push(this);
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0000BDB8 File Offset: 0x00009FB8
		public override void Hide()
		{
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			Game.g_InputManager.Pop();
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x0009FB00 File Offset: 0x0009DD00
		protected override void OnStateEnter(int state)
		{
			base.OnStateEnter(state);
			if (state == 0)
			{
				this.m_Colloder.enabled = false;
				this.m_Group.GameObject.SetActive(false);
				Game.PrefabSmallGameEnd();
				this.Hide();
			}
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x0009FB50 File Offset: 0x0009DD50
		public override void OnKeyUp(KeyControl.Key key)
		{
			if (this.NowState == 2)
			{
				switch (key)
				{
				case KeyControl.Key.ArrowUp:
					this.m_CtrlAbility.OnSwipe("UP");
					break;
				case KeyControl.Key.ArrowDown:
					this.m_CtrlAbility.OnSwipe("DOWN");
					break;
				case KeyControl.Key.ArrowLeft:
					this.m_CtrlAbility.OnSwipe("LEFT");
					break;
				case KeyControl.Key.ArrowRight:
					this.m_CtrlAbility.OnSwipe("RIGHT");
					break;
				}
			}
			else
			{
				base.OnKeyUp(key);
			}
		}

		// Token: 0x040015FF RID: 5631
		private CtrlAbility m_CtrlAbility;

		// Token: 0x04001600 RID: 5632
		public GameObject TargetBaseTile2;

		// Token: 0x04001601 RID: 5633
		private List<Control> m_BackGroundList = new List<Control>();

		// Token: 0x04001602 RID: 5634
		private Control m_TargetBG;

		// Token: 0x04001603 RID: 5635
		private Control m_LevelTitle;

		// Token: 0x04001604 RID: 5636
		private List<Control> m_LevelIcon = new List<Control>();

		// Token: 0x04001605 RID: 5637
		private List<Control> m_LevelLabel = new List<Control>();

		// Token: 0x04001606 RID: 5638
		private Control m_TargetSelectLabel;

		// Token: 0x04001607 RID: 5639
		private Control m_TargetLabel;

		// Token: 0x04001608 RID: 5640
		private Control m_AlchemyBowl;

		// Token: 0x04001609 RID: 5641
		private Control m_StartGroup;

		// Token: 0x0400160A RID: 5642
		private Control m_EndBackground;

		// Token: 0x0400160B RID: 5643
		private Control m_BtnClose;

		// Token: 0x0400160C RID: 5644
		private Control m_GetItemIcon;

		// Token: 0x0400160D RID: 5645
		private Control m_GetItemIconBG;

		// Token: 0x0400160E RID: 5646
		private List<Control> m_LabelSkillList = new List<Control>();

		// Token: 0x0400160F RID: 5647
		private List<Control> m_LabelNewNumericalList = new List<Control>();

		// Token: 0x04001610 RID: 5648
		private List<Control> m_LabelOldNumericalList = new List<Control>();

		// Token: 0x04001611 RID: 5649
		private Control m_UpIcon1;

		// Token: 0x04001612 RID: 5650
		private Control m_UpIcon2;

		// Token: 0x04001613 RID: 5651
		private Control m_LabelPrompt;

		// Token: 0x04001614 RID: 5652
		private Control m_LabelSelectProduct;

		// Token: 0x04001615 RID: 5653
		private Control m_BtnNext;

		// Token: 0x04001616 RID: 5654
		private Control m_BtnUp;

		// Token: 0x04001617 RID: 5655
		private Control m_BtnReturn;

		// Token: 0x04001618 RID: 5656
		private Control m_LabelItemAmount;

		// Token: 0x04001619 RID: 5657
		private Control m_ItemNameBase;

		// Token: 0x0400161A RID: 5658
		private Control m_LabelItemName;

		// Token: 0x0400161B RID: 5659
		private Control m_Group;

		// Token: 0x0400161C RID: 5660
		private int m_iPage;

		// Token: 0x0400161D RID: 5661
		private float m_fPromptTime;

		// Token: 0x0400161E RID: 5662
		private List<BookNode> m_BookNodeList;

		// Token: 0x0400161F RID: 5663
		private int m_iIndex;

		// Token: 0x04001620 RID: 5664
		private List<WgAbilityBtnCasting> m_WgAbilityBtnCastingList = new List<WgAbilityBtnCasting>();

		// Token: 0x04001621 RID: 5665
		private WgAbilityItemTip m_WgAbilityItemTip;

		// Token: 0x04001622 RID: 5666
		private Collider m_Colloder;

		// Token: 0x04001623 RID: 5667
		private int m_iMaxPage;

		// Token: 0x04001624 RID: 5668
		private AlchemyProduceNode m_AlchemyNode;

		// Token: 0x04001625 RID: 5669
		private AlchemyScene m_AlchemyScene;

		// Token: 0x04001626 RID: 5670
		public int iType;

		// Token: 0x04001627 RID: 5671
		public int iNowSkill;

		// Token: 0x04001628 RID: 5672
		private bool m_bTest;

		// Token: 0x02000332 RID: 818
		public enum eState
		{
			// Token: 0x0400162C RID: 5676
			none,
			// Token: 0x0400162D RID: 5677
			Select,
			// Token: 0x0400162E RID: 5678
			GameStart,
			// Token: 0x0400162F RID: 5679
			GameEnd
		}

		// Token: 0x02000333 RID: 819
		public enum eAbilityType
		{
			// Token: 0x04001631 RID: 5681
			Alchemy = 5,
			// Token: 0x04001632 RID: 5682
			Forging
		}
	}
}
