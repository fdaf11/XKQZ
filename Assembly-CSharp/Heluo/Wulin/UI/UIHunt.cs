using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200030A RID: 778
	public class UIHunt : UILayer
	{
		// Token: 0x06001097 RID: 4247 RVA: 0x0008F940 File Offset: 0x0008DB40
		protected override void Awake()
		{
			this.m_BtnBowList = new List<Control>();
			this.m_LabelBowNameList = new List<Control>();
			this.m_SelectBowBoxList = new List<Control>();
			this.m_LabelAmountList = new List<Control>();
			this.m_GetItemIconList = new List<Control>();
			this.m_GetItemIconBGList = new List<Control>();
			this.m_LabelItemAmountList = new List<Control>();
			this.m_HuntRewardNodeList = new List<HuntRewardNode>();
			this.m_PointList = new List<int>();
			this.m_PointFlyList = new List<int>();
			base.Awake();
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0008F9C4 File Offset: 0x0008DBC4
		private void Start()
		{
			this.SetBowData();
			this.GetPlayerLv();
			this.m_bArrowReady = true;
			this.m_bProduce = false;
			this.m_iUseArrowAmount = 0;
			this.m_fLessTime = 0f;
			this.m_iPoint = 0;
			this.m_BowDataNode = null;
			this.m_fReloadingTime = 0f;
			this.m_iMinAmount = 2;
			this.m_iMaxAmount = 4;
			this.m_LabelPoint.Text = this.m_iPoint.ToString();
			for (int i = 0; i < this.m_LabelAmountList.Count; i++)
			{
				this.m_LabelAmountList[i].Text = "0";
			}
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0008FA6C File Offset: 0x0008DC6C
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIHunt.<>f__switch$mapC == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(33);
					dictionary.Add("Group", 0);
					dictionary.Add("BarTime", 1);
					dictionary.Add("BarTimeBase", 2);
					dictionary.Add("BtnLeave", 3);
					dictionary.Add("SelectBowBackground", 4);
					dictionary.Add("AnimalTitleGroup", 5);
					dictionary.Add("BtnBow", 6);
					dictionary.Add("LabelBowName", 7);
					dictionary.Add("SelectBowBox", 8);
					dictionary.Add("LabelAmount", 9);
					dictionary.Add("LabelPoint", 10);
					dictionary.Add("LabelArrow", 11);
					dictionary.Add("BowImage", 12);
					dictionary.Add("SceneBackground", 13);
					dictionary.Add("BornPoint1", 14);
					dictionary.Add("BornPoint2", 15);
					dictionary.Add("BornPoint3", 16);
					dictionary.Add("BornPoint4", 17);
					dictionary.Add("BornPoint5", 18);
					dictionary.Add("BornPoint6", 19);
					dictionary.Add("AnimalNodeList", 20);
					dictionary.Add("ActivitiesScope", 21);
					dictionary.Add("EndBackground", 22);
					dictionary.Add("BtnClose", 23);
					dictionary.Add("LabelNewNumerical", 24);
					dictionary.Add("LabelOldNumerical", 25);
					dictionary.Add("UpIcon1", 26);
					dictionary.Add("GetItemIcon", 27);
					dictionary.Add("GetItemIconBG", 28);
					dictionary.Add("LabelItemAmount", 29);
					dictionary.Add("ItemNameBase", 30);
					dictionary.Add("LabelItemName", 31);
					dictionary.Add("LabelDodgeAdd", 32);
					UIHunt.<>f__switch$mapC = dictionary;
				}
				int num;
				if (UIHunt.<>f__switch$mapC.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_BarTime = sender;
						break;
					case 2:
						this.m_BarTimeBase = sender;
						break;
					case 3:
						this.m_BtnLeave = sender;
						this.m_BtnLeave.OnClick += this.BtnLeaveOnClick;
						break;
					case 4:
						this.m_SelectBowBackground = sender;
						break;
					case 5:
						this.m_AnimalTitleGroup = sender;
						break;
					case 6:
					{
						Control control = sender;
						control.OnHover += this.BtnBowOnHover;
						control.OnClick += this.BtnBowOnClick;
						this.m_BtnBowList.Add(control);
						break;
					}
					case 7:
					{
						Control control = sender;
						this.m_LabelBowNameList.Add(control);
						break;
					}
					case 8:
					{
						Control control = sender;
						this.m_SelectBowBoxList.Add(control);
						break;
					}
					case 9:
					{
						Control control = sender;
						this.m_LabelAmountList.Add(control);
						break;
					}
					case 10:
						this.m_LabelPoint = sender;
						break;
					case 11:
						this.m_LabelArrow = sender;
						break;
					case 12:
						this.m_BowImage = sender;
						break;
					case 13:
						this.m_SceneBackground = sender;
						break;
					case 14:
						this.m_BornPoint1 = sender;
						break;
					case 15:
						this.m_BornPoint2 = sender;
						break;
					case 16:
						this.m_BornPoint3 = sender;
						break;
					case 17:
						this.m_BornPoint4 = sender;
						break;
					case 18:
						this.m_BornPoint5 = sender;
						break;
					case 19:
						this.m_BornPoint6 = sender;
						break;
					case 20:
						this.m_AnimalNodeList = sender;
						break;
					case 21:
						this.m_ActivitiesScope = sender;
						break;
					case 22:
						this.m_EndBackground = sender;
						break;
					case 23:
						this.m_BtnClose = sender;
						this.m_BtnClose.OnClick += this.BtnCloseOnClick;
						break;
					case 24:
						this.m_LabelNewNumerical = sender;
						break;
					case 25:
						this.m_LabelOldNumerical = sender;
						break;
					case 26:
						this.m_UpIcon1 = sender;
						break;
					case 27:
					{
						Control control = sender;
						this.m_GetItemIconList.Add(control);
						break;
					}
					case 28:
					{
						Control control = sender;
						control.OnHover += this.GetItemIconBGOnHover;
						this.m_GetItemIconBGList.Add(control);
						break;
					}
					case 29:
					{
						Control control = sender;
						this.m_LabelItemAmountList.Add(control);
						break;
					}
					case 30:
						this.m_ItemNameBase = sender;
						break;
					case 31:
						this.m_LabelItemName = sender;
						break;
					case 32:
						this.m_LabelDodgeAdd = sender;
						break;
					}
				}
			}
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0000264F File Offset: 0x0000084F
		private void GetPlayerLv()
		{
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0008FFAC File Offset: 0x0008E1AC
		private void SetBowData()
		{
			this.m_LabelArrow.Text = this.m_iArrowAmount.ToString();
			for (int i = 0; i < this.m_SelectBowBoxList.Count; i++)
			{
				this.m_SelectBowBoxList[i].GameObject.SetActive(false);
			}
			for (int j = 0; j < this.m_BtnBowList.Count; j++)
			{
				this.m_BtnBowList[j].GameObject.SetActive(false);
			}
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00090038 File Offset: 0x0008E238
		private void BtnBowOnHover(GameObject go, bool bHover)
		{
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			for (int i = 0; i < this.m_SelectBowBoxList.Count; i++)
			{
				this.m_SelectBowBoxList[i].GameObject.SetActive(false);
				if (bHover && this.m_SelectBowBoxList[i].GetComponent<ImageData>().m_iIndex == iBtnID)
				{
					this.m_SelectBowBoxList[i].GameObject.SetActive(true);
				}
			}
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x000900C0 File Offset: 0x0008E2C0
		private void BtnBowOnClick(GameObject go)
		{
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.m_SelectBowBackground.GameObject.SetActive(false);
			this.m_AnimalTitleGroup.GetComponent<TweenPosition>().ResetToBeginning();
			this.m_AnimalTitleGroup.GetComponent<TweenPosition>().Play();
			this.m_AnimalTitleGroup.GameObject.SetActive(true);
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0000AD48 File Offset: 0x00008F48
		public void GameOpen()
		{
			this.Start();
			this.m_Group.GameObject.SetActive(true);
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0000AD61 File Offset: 0x00008F61
		public void GameStart()
		{
			this.ReloadArrow();
			this.m_BowImage.GameObject.SetActive(true);
			this.m_fOverTime = Time.time + 90f;
			this.m_bStart = true;
			this.m_bProduce = true;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0009011C File Offset: 0x0008E31C
		private void ReloadArrow()
		{
			this.m_BowImage.GetComponent<UISprite>().spriteName = this.m_strBowReadyName;
			GameObject gameObject = Object.Instantiate(this.ArrowPrefab) as GameObject;
			gameObject.GetComponent<HuntArrow>().m_iIndex = this.m_iUseArrowAmount;
			gameObject.GetComponent<HuntArrow>().m_fTurnSpeed = this.m_fTurnSpeed;
			gameObject.GetComponent<HuntArrow>().m_fMoveSpeed = (float)this.m_BowDataNode.m_iSpeed;
			int num = 0;
			gameObject.GetComponent<HuntArrow>().m_iAtk = this.m_BowDataNode.m_iAtk + num;
			gameObject.GetComponent<HuntArrow>().m_goBackground = this.m_SceneBackground.GameObject;
			gameObject.transform.parent = this.m_Group.GameObject.transform;
			gameObject.transform.localPosition = this.m_BowImage.GameObject.transform.localPosition;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localRotation = this.m_BowImage.GameObject.transform.localRotation;
			gameObject.name = this.ArrowPrefab.name;
			this.m_bArrowReady = true;
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0009023C File Offset: 0x0008E43C
		public void Fire()
		{
			this.m_bArrowReady = false;
			this.m_iUseArrowAmount++;
			int num = this.m_iArrowAmount - this.m_iUseArrowAmount;
			this.m_LabelArrow.Text = num.ToString();
			if (num == 0)
			{
				this.EndGame();
			}
			else
			{
				this.m_fReloadingTime = Time.time;
			}
			this.m_BowImage.GetComponent<UISprite>().spriteName = this.m_strBowStandbyName;
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x000902B0 File Offset: 0x0008E4B0
		private void ProduceAnimal()
		{
			this.m_PointList.Clear();
			this.m_PointFlyList.Clear();
			for (int i = 0; i < 4; i++)
			{
				this.m_PointList.Add(i);
			}
			for (int i = 0; i < 2; i++)
			{
				this.m_PointFlyList.Add(i);
			}
			this.m_iAnimalAmount = Random.Range(this.m_iMinAmount, this.m_iMaxAmount + 1);
			for (int i = 0; i < this.m_iAnimalAmount; i++)
			{
				int iIndex = Random.Range(0, this.m_iProbability);
				AnimalDataNode _AnimalDataNode = Game.HuntData.GetAnimalDataNode(this.m_iLevel, iIndex);
				if (this.m_PointFlyList.Count == 0 && _AnimalDataNode.m_iAnimalType == 2)
				{
					_AnimalDataNode = this.ReRandomAnimal();
				}
				GameObject go_animal = Object.Instantiate(this.AnimalPrefab) as GameObject;
				go_animal.transform.parent = this.m_AnimalNodeList.GameObject.transform;
				go_animal.GetComponent<HuntAnimal>().m_AnimalDataNode = _AnimalDataNode;
				go_animal.GetComponent<HuntAnimal>().m_goActivitiesScope = this.m_ActivitiesScope.GameObject;
				go_animal.GetComponent<HuntAnimal>().m_iHp = _AnimalDataNode.m_iAnimalHp;
				int iWidth = 0;
				int iHeight = 0;
				if (_AnimalDataNode.m_iBodyType == 0)
				{
					iWidth = 120;
					iHeight = 120;
				}
				else if (_AnimalDataNode.m_iBodyType == 1)
				{
					iWidth = 140;
					iHeight = 140;
				}
				else if (_AnimalDataNode.m_iBodyType == 2)
				{
					iWidth = 160;
					iHeight = 160;
				}
				else
				{
					iWidth = _AnimalDataNode.m_iBodyType;
					iHeight = _AnimalDataNode.m_iBodyType;
				}
				go_animal.GetComponent<UISprite>().spriteName = _AnimalDataNode.m_strIcon;
				go_animal.GetComponent<UISprite>().width = iWidth;
				go_animal.GetComponent<UISprite>().height = iHeight;
				go_animal.GetComponent<HuntAnimal>().m_iWidth = iWidth;
				go_animal.GetComponent<HuntAnimal>().m_iHeight = iHeight;
				Vector3 localPosition = Vector3.zero;
				if (_AnimalDataNode.m_iAnimalType == 0 || _AnimalDataNode.m_iAnimalType == 1)
				{
					int num = Random.Range(0, this.m_PointList.Count);
					if (num > this.m_PointList.Count)
					{
						Debug.LogError(num);
					}
					else
					{
						if (this.m_PointList[num] == 0)
						{
							localPosition = this.m_BornPoint1.GameObject.transform.localPosition;
						}
						else if (this.m_PointList[num] == 1)
						{
							localPosition = this.m_BornPoint2.GameObject.transform.localPosition;
						}
						else if (this.m_PointList[num] == 2)
						{
							localPosition = this.m_BornPoint3.GameObject.transform.localPosition;
						}
						else if (this.m_PointList[num] == 3)
						{
							localPosition = this.m_BornPoint4.GameObject.transform.localPosition;
						}
						go_animal.GetComponent<HuntAnimal>().m_iPoint = this.m_PointList[num];
						this.m_PointList.RemoveAt(num);
					}
				}
				else
				{
					int num = Random.Range(0, this.m_PointFlyList.Count);
					if (num > this.m_PointFlyList.Count)
					{
						Debug.LogError(num);
					}
					else
					{
						if (this.m_PointFlyList[num] == 0)
						{
							localPosition = this.m_BornPoint5.GameObject.transform.localPosition;
						}
						else if (this.m_PointFlyList[num] == 1)
						{
							localPosition = this.m_BornPoint6.GameObject.transform.localPosition;
						}
						go_animal.GetComponent<HuntAnimal>().m_iPoint = this.m_PointFlyList[num];
						go_animal.GetComponent<UISprite>().depth = 4;
						this.m_PointFlyList.RemoveAt(num);
					}
				}
				go_animal.GetComponent<HuntAnimal>().m_BarTimeBase = this.m_BarTimeBase.GameObject;
				go_animal.transform.localPosition = localPosition;
				go_animal.transform.localScale = Vector3.one;
				go_animal.name = this.AnimalPrefab.name;
				go_animal.transform.Traversal(delegate(Transform x)
				{
					string name = x.name;
					if (name != null)
					{
						if (UIHunt.<>f__switch$mapE == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
							dictionary.Add("HitEffect", 0);
							dictionary.Add("HitImage", 1);
							dictionary.Add("HitLight", 2);
							dictionary.Add("AnimalItem", 3);
							dictionary.Add("AtkEffect", 4);
							dictionary.Add("HurtEffect", 5);
							UIHunt.<>f__switch$mapE = dictionary;
						}
						int num2;
						if (UIHunt.<>f__switch$mapE.TryGetValue(name, ref num2))
						{
							switch (num2)
							{
							case 0:
								go_animal.GetComponent<HuntAnimal>().m_go_HitEffect = x.gameObject;
								go_animal.GetComponent<HuntAnimal>().m_HitEffect_TweenScale = x.gameObject.GetComponent<TweenScale>();
								go_animal.GetComponent<HuntAnimal>().m_HitEffect_TweenAlpha = x.gameObject.GetComponent<TweenAlpha>();
								x.gameObject.GetComponent<UITexture>().width = iWidth;
								x.gameObject.GetComponent<UITexture>().height = iHeight;
								if (_AnimalDataNode.m_iAnimalType == 2)
								{
									x.gameObject.GetComponent<UITexture>().depth = 5;
								}
								break;
							case 1:
								go_animal.GetComponent<HuntAnimal>().m_go_HitImage = x.gameObject;
								go_animal.GetComponent<HuntAnimal>().m_HitImage_TweenAlpha = x.gameObject.GetComponent<TweenAlpha>();
								x.gameObject.GetComponent<UITexture>().width = iWidth;
								x.gameObject.GetComponent<UITexture>().height = iHeight;
								if (_AnimalDataNode.m_iAnimalType == 2)
								{
									x.gameObject.GetComponent<UITexture>().depth = 5;
								}
								break;
							case 2:
								go_animal.GetComponent<HuntAnimal>().m_go_HitLight = x.gameObject;
								go_animal.GetComponent<HuntAnimal>().m_HitLight_TweenAlpha = x.gameObject.GetComponent<TweenAlpha>();
								go_animal.GetComponent<HuntAnimal>().m_HitLight_TweenScale = x.gameObject.GetComponent<TweenScale>();
								x.gameObject.GetComponent<UITexture>().width = iWidth;
								x.gameObject.GetComponent<UITexture>().height = iHeight;
								if (_AnimalDataNode.m_iAnimalType == 2)
								{
									x.gameObject.GetComponent<UITexture>().depth = 5;
								}
								break;
							case 3:
								go_animal.GetComponent<HuntAnimal>().m_go_AnimalItem = x.gameObject;
								if (_AnimalDataNode.m_iAnimalType == 2)
								{
									x.gameObject.GetComponent<UITexture>().depth = 5;
								}
								break;
							case 4:
								go_animal.GetComponent<HuntAnimal>().m_go_AtkEffect = x.gameObject;
								x.gameObject.GetComponent<UITexture>().width = iWidth;
								x.gameObject.GetComponent<UITexture>().height = iHeight;
								if (_AnimalDataNode.m_iAnimalType == 2)
								{
									x.gameObject.GetComponent<UITexture>().depth = 5;
								}
								break;
							case 5:
								go_animal.GetComponent<HuntAnimal>().m_go_HurtEffect = x.gameObject;
								if (_AnimalDataNode.m_iAnimalType == 2)
								{
									x.gameObject.GetComponent<UITexture>().depth = 5;
								}
								break;
							}
						}
					}
				});
			}
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x000907D0 File Offset: 0x0008E9D0
		private AnimalDataNode ReRandomAnimal()
		{
			int iIndex = Random.Range(0, this.m_iProbability);
			AnimalDataNode animalDataNode = Game.HuntData.GetAnimalDataNode(this.m_iLevel, iIndex);
			if (animalDataNode.m_iAnimalType == 2)
			{
				return this.ReRandomAnimal();
			}
			return animalDataNode;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0000AD99 File Offset: 0x00008F99
		private void BtnLeaveOnClick(GameObject go)
		{
			this.EndGame();
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x00090814 File Offset: 0x0008EA14
		public void SetReward(int iItemID, int iAnimalID, int iPoint)
		{
			bool flag = false;
			if (iItemID != 0)
			{
				for (int i = 0; i < this.m_HuntRewardNodeList.Count; i++)
				{
					if (this.m_HuntRewardNodeList[i].m_iItemID == iItemID)
					{
						this.m_HuntRewardNodeList[i].m_iAmount++;
						flag = true;
					}
				}
				if (!flag)
				{
					HuntRewardNode huntRewardNode = new HuntRewardNode();
					huntRewardNode.m_iItemID = iItemID;
					huntRewardNode.m_iAmount = 1;
					huntRewardNode.m_strItemName = Game.ItemData.GetItemName(iItemID);
					this.m_HuntRewardNodeList.Add(huntRewardNode);
				}
			}
			for (int j = 0; j < this.m_LabelAmountList.Count; j++)
			{
				if (this.m_LabelAmountList[j].GetComponent<LabelData>().m_iIndex == iAnimalID)
				{
					this.m_LabelAmountList[j].Text = (int.Parse(this.m_LabelAmountList[j].Text) + 1).ToString();
				}
			}
			this.m_iPoint += iPoint;
			this.m_LabelPoint.Text = this.m_iPoint.ToString();
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0000ADA1 File Offset: 0x00008FA1
		public void AnimalDisappear()
		{
			this.m_iAnimalAmount--;
			if (this.m_iAnimalAmount == 0)
			{
				this.m_bProduce = true;
			}
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0000ADC3 File Offset: 0x00008FC3
		public void PlayerHit(float fTime)
		{
			this.m_fLessTime += fTime;
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0009093C File Offset: 0x0008EB3C
		private void EndGame()
		{
			if (this.m_bEnd)
			{
				return;
			}
			this.m_bStart = false;
			this.m_bArrowReady = false;
			this.m_bEnd = true;
			ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(100051);
			GameObject[] array = GameObject.FindGameObjectsWithTag("Arrow");
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponent<HuntArrow>().m_bEnd = true;
			}
			array = GameObject.FindGameObjectsWithTag("Animal");
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponent<HuntAnimal>().m_bEnd = true;
			}
			this.m_EndBackground.GameObject.SetActive(true);
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x000909E8 File Offset: 0x0008EBE8
		private void GetItemIconBGOnHover(GameObject go, bool bHover)
		{
			if (bHover)
			{
				this.m_ItemNameBase.GameObject.SetActive(true);
				this.m_ItemNameBase.GameObject.transform.localPosition = new Vector3(go.transform.localPosition.x - 28f, go.transform.localPosition.y + 34f, this.m_ItemNameBase.GameObject.transform.localPosition.z);
				int iIndex = go.GetComponent<ImageData>().m_iIndex;
				this.m_LabelItemName.Text = this.m_HuntRewardNodeList[iIndex].m_strItemName;
				int num = (int)this.m_LabelItemName.GetComponent<UILabel>().printedSize.x;
				this.m_ItemNameBase.GameObject.GetComponent<UITexture>().width = num + 60;
			}
			else
			{
				this.m_ItemNameBase.GameObject.SetActive(false);
			}
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0000ADD3 File Offset: 0x00008FD3
		private void BtnCloseOnClick(GameObject go)
		{
			this.ReSetData();
			this.m_Group.GameObject.SetActive(false);
			Game.PrefabSmallGameEnd();
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x00090AE8 File Offset: 0x0008ECE8
		private void ReSetData()
		{
			this.m_HuntRewardNodeList.Clear();
			this.m_EndBackground.GameObject.SetActive(false);
			this.m_BowImage.GameObject.SetActive(false);
			this.m_SelectBowBackground.GameObject.SetActive(true);
			this.m_bEnd = false;
			GameObject[] array = GameObject.FindGameObjectsWithTag("Arrow");
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i]);
			}
			array = GameObject.FindGameObjectsWithTag("Animal");
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i]);
			}
			for (int j = 0; j < this.m_GetItemIconBGList.Count; j++)
			{
				this.m_GetItemIconBGList[j].GameObject.SetActive(false);
			}
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x00090BBC File Offset: 0x0008EDBC
		private void Update()
		{
			if (!this.m_bStart)
			{
				return;
			}
			if (this.m_fOverTime - Time.time - this.m_fLessTime > 0f)
			{
				float fillAmount = (this.m_fOverTime - Time.time - this.m_fLessTime) / 90f;
				this.m_BarTime.GetComponent<UISprite>().fillAmount = fillAmount;
				if (!this.m_bArrowReady && Time.time - this.m_fReloadingTime > this.m_BowDataNode.m_fReload)
				{
					this.m_bArrowReady = true;
					this.ReloadArrow();
				}
				if (this.m_bProduce)
				{
					this.m_bProduce = false;
					this.ProduceAnimal();
				}
				Vector3 vector = GameGlobal.m_camSmallGame.ScreenToWorldPoint(Input.mousePosition);
				Vector3 vector2 = vector - this.m_BowImage.GameObject.transform.position;
				vector2.z = 0f;
				vector2.Normalize();
				float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
				num -= 90f;
				if (num < -85f)
				{
					if (num > -180f)
					{
						num = -85f;
					}
					else
					{
						num = 85f;
					}
				}
				else if (num > 85f)
				{
					num = 85f;
				}
				this.m_BowImage.GameObject.transform.rotation = Quaternion.Slerp(this.m_BowImage.GameObject.transform.rotation, Quaternion.Euler(0f, 0f, num), this.m_fTurnSpeed * Time.deltaTime);
			}
			else if (this.m_fOverTime - Time.time - this.m_fLessTime <= 0f)
			{
				this.EndGame();
			}
		}

		// Token: 0x0400140A RID: 5130
		private Control m_Group;

		// Token: 0x0400140B RID: 5131
		private Control m_BarTime;

		// Token: 0x0400140C RID: 5132
		private Control m_BarTimeBase;

		// Token: 0x0400140D RID: 5133
		private Control m_BtnLeave;

		// Token: 0x0400140E RID: 5134
		private Control m_AnimalTitleGroup;

		// Token: 0x0400140F RID: 5135
		private Control m_SelectBowBackground;

		// Token: 0x04001410 RID: 5136
		private Control m_LabelPoint;

		// Token: 0x04001411 RID: 5137
		private Control m_LabelArrow;

		// Token: 0x04001412 RID: 5138
		private Control m_BowImage;

		// Token: 0x04001413 RID: 5139
		private Control m_SceneBackground;

		// Token: 0x04001414 RID: 5140
		private Control m_BornPoint1;

		// Token: 0x04001415 RID: 5141
		private Control m_BornPoint2;

		// Token: 0x04001416 RID: 5142
		private Control m_BornPoint3;

		// Token: 0x04001417 RID: 5143
		private Control m_BornPoint4;

		// Token: 0x04001418 RID: 5144
		private Control m_BornPoint5;

		// Token: 0x04001419 RID: 5145
		private Control m_BornPoint6;

		// Token: 0x0400141A RID: 5146
		private Control m_AnimalNodeList;

		// Token: 0x0400141B RID: 5147
		private Control m_ActivitiesScope;

		// Token: 0x0400141C RID: 5148
		private Control m_LabelDodgeAdd;

		// Token: 0x0400141D RID: 5149
		private List<Control> m_BtnBowList;

		// Token: 0x0400141E RID: 5150
		private List<Control> m_LabelBowNameList;

		// Token: 0x0400141F RID: 5151
		private List<Control> m_SelectBowBoxList;

		// Token: 0x04001420 RID: 5152
		private List<Control> m_LabelAmountList;

		// Token: 0x04001421 RID: 5153
		private Control m_EndBackground;

		// Token: 0x04001422 RID: 5154
		private Control m_BtnClose;

		// Token: 0x04001423 RID: 5155
		private Control m_LabelNewNumerical;

		// Token: 0x04001424 RID: 5156
		private Control m_LabelOldNumerical;

		// Token: 0x04001425 RID: 5157
		private Control m_UpIcon1;

		// Token: 0x04001426 RID: 5158
		private Control m_ItemNameBase;

		// Token: 0x04001427 RID: 5159
		private Control m_LabelItemName;

		// Token: 0x04001428 RID: 5160
		private List<Control> m_GetItemIconList;

		// Token: 0x04001429 RID: 5161
		private List<Control> m_GetItemIconBGList;

		// Token: 0x0400142A RID: 5162
		private List<Control> m_LabelItemAmountList;

		// Token: 0x0400142B RID: 5163
		private List<HuntRewardNode> m_HuntRewardNodeList;

		// Token: 0x0400142C RID: 5164
		private int m_iPoint;

		// Token: 0x0400142D RID: 5165
		private int m_iArrowAmount;

		// Token: 0x0400142E RID: 5166
		private float m_fOverTime;

		// Token: 0x0400142F RID: 5167
		private float m_fLessTime;

		// Token: 0x04001430 RID: 5168
		private bool m_bTimeOver;

		// Token: 0x04001431 RID: 5169
		public bool m_bStart;

		// Token: 0x04001432 RID: 5170
		private int m_iMinAmount;

		// Token: 0x04001433 RID: 5171
		private int m_iMaxAmount;

		// Token: 0x04001434 RID: 5172
		private List<int> m_PointList;

		// Token: 0x04001435 RID: 5173
		private List<int> m_PointFlyList;

		// Token: 0x04001436 RID: 5174
		private int m_iLevel;

		// Token: 0x04001437 RID: 5175
		private int m_iProbability;

		// Token: 0x04001438 RID: 5176
		private int m_iAnimalAmount;

		// Token: 0x04001439 RID: 5177
		private bool m_bEnd;

		// Token: 0x0400143A RID: 5178
		public float m_fTurnSpeed;

		// Token: 0x0400143B RID: 5179
		private BowDataNode m_BowDataNode;

		// Token: 0x0400143C RID: 5180
		private float m_fReloadingTime;

		// Token: 0x0400143D RID: 5181
		private string m_strBowStandbyName;

		// Token: 0x0400143E RID: 5182
		private string m_strBowReadyName;

		// Token: 0x0400143F RID: 5183
		public GameObject ArrowPrefab;

		// Token: 0x04001440 RID: 5184
		private bool m_bArrowReady;

		// Token: 0x04001441 RID: 5185
		private int m_iUseArrowAmount;

		// Token: 0x04001442 RID: 5186
		public GameObject AnimalPrefab;

		// Token: 0x04001443 RID: 5187
		private bool m_bProduce;
	}
}
