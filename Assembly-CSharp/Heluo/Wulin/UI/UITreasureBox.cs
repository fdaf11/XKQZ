using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200032F RID: 815
	public class UITreasureBox : UILayer
	{
		// Token: 0x06001217 RID: 4631 RVA: 0x0009C638 File Offset: 0x0009A838
		protected override void Awake()
		{
			this.m_GetItemIconList = new List<Control>();
			this.m_LabelItemAmountList = new List<Control>();
			this.m_GetItemIconBGList = new List<Control>();
			this.m_BtnMoveList = new List<Control>();
			this.m_MiddleIconList = new List<Control>();
			this.m_PeripheryIconList = new List<Control>();
			this.m_NumList = new List<int>();
			base.Awake();
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x0009C698 File Offset: 0x0009A898
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			Game.g_InputManager.Push(this);
			this.m_iCount = 0;
			this.m_iMaxGrid = 45;
			this.m_bMoving = false;
			this.m_bSetEnd = false;
			GameGlobal.m_bCFormOpen = true;
			this.m_Group.GameObject.SetActive(true);
			this.GameStart();
			this.SetIconData();
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0009C704 File Offset: 0x0009A904
		public override void Hide()
		{
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			this.m_TreasureBox.GameEnd(this.bWin2Object);
			this.ReSetData();
			GameGlobal.m_bCFormOpen = false;
			this.m_Group.GameObject.SetActive(false);
			this.m_TreasureBoxNode = null;
			Game.g_InputManager.Pop();
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0000BC39 File Offset: 0x00009E39
		private void BtnCloseOnClick(GameObject go, bool t = false)
		{
			if (GameCursor.IsShow)
			{
				this.Hide();
			}
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x0000BC4B File Offset: 0x00009E4B
		public void GameOpen()
		{
			this.Show();
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0009C764 File Offset: 0x0009A964
		public override void OnKeyUp(KeyControl.Key key)
		{
			if (this.NowState == 0)
			{
				switch (key)
				{
				case KeyControl.Key.Up:
				case KeyControl.Key.Down:
				case KeyControl.Key.Left:
				case KeyControl.Key.Right:
				case KeyControl.Key.X:
				case KeyControl.Key.Y:
					this.SelectNextButton(key);
					break;
				case KeyControl.Key.OK:
					base.OnCurrentClick();
					break;
				case KeyControl.Key.Cancel:
					this.BtnLeaveOnClick(this.m_BtnLeave.GameObject, true);
					break;
				}
			}
			else if (this.NowState == 1)
			{
				switch (key)
				{
				case KeyControl.Key.OK:
					this.Hide();
					break;
				}
			}
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0009C81C File Offset: 0x0009AA1C
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UITreasureBox.<>f__switch$map23 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(17);
					dictionary.Add("Group", 0);
					dictionary.Add("BtnLeave", 1);
					dictionary.Add("ExplanationBackground", 2);
					dictionary.Add("GameBackground", 3);
					dictionary.Add("LabelMaxCount", 4);
					dictionary.Add("LabelMoveCount", 5);
					dictionary.Add("BtnMove", 6);
					dictionary.Add("MiddleIcon", 7);
					dictionary.Add("PeripheryIcon", 8);
					dictionary.Add("EndBackground", 9);
					dictionary.Add("BtnClose", 10);
					dictionary.Add("LabelEnd", 11);
					dictionary.Add("GetItemIcon", 12);
					dictionary.Add("LabelItemAmount", 13);
					dictionary.Add("GetItemIconBG", 14);
					dictionary.Add("ItemNameBase", 15);
					dictionary.Add("LabelItemName", 16);
					UITreasureBox.<>f__switch$map23 = dictionary;
				}
				int num;
				if (UITreasureBox.<>f__switch$map23.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_BtnLeave = sender;
						this.m_BtnLeave.OnClick += new UIEventListener.VoidDelegate(this.BtnLeaveOnClick);
						break;
					case 2:
						this.m_ExplanationBackground = sender;
						break;
					case 3:
						this.m_GameBackground = sender;
						break;
					case 4:
						this.m_LabelMaxCount = sender;
						break;
					case 5:
						this.m_LabelMoveCount = sender;
						break;
					case 6:
					{
						Control control = sender;
						control.OnClick += this.BtnMoveOnClick;
						control.OnHover += this.BtnOnKeySelect;
						control.OnKeySelect += this.BtnOnKeySelect;
						UIEventListener listener = UIEventListener.Get(sender.gameObject);
						base.SetInputButton(0, listener);
						this.m_BtnMoveList.Add(control);
						break;
					}
					case 7:
					{
						Control control = sender;
						this.m_MiddleIconList.Add(control);
						break;
					}
					case 8:
					{
						Control control = sender;
						this.m_PeripheryIconList.Add(control);
						break;
					}
					case 9:
						this.m_EndBackground = sender;
						break;
					case 10:
					{
						Control control = sender;
						control.OnClick += new UIEventListener.VoidDelegate(this.BtnCloseOnClick);
						UIEventListener listener2 = UIEventListener.Get(sender.gameObject);
						base.SetInputButton(1, listener2);
						break;
					}
					case 11:
						this.m_LabelEnd = sender;
						break;
					case 12:
					{
						Control control = sender;
						this.m_GetItemIconList.Add(control);
						break;
					}
					case 13:
					{
						Control control = sender;
						this.m_LabelItemAmountList.Add(control);
						break;
					}
					case 14:
					{
						Control control = sender;
						control.OnHover += this.GetItemIconBGOnHover;
						this.m_GetItemIconBGList.Add(control);
						break;
					}
					case 15:
						this.m_ItemNameBase = sender;
						break;
					case 16:
						this.m_LabelItemName = sender;
						break;
					}
				}
			}
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0009CB78 File Offset: 0x0009AD78
		private void GameStart()
		{
			if (this.m_TreasureBoxNode == null)
			{
				this.m_TreasureBox = PlayerController.m_Instance.Target.GetComponent<TreasureBox>();
				this.m_TreasureBoxNode = this.m_TreasureBox.m_TreasureBoxNode;
			}
			base.EnterState(0);
			this.m_LabelMaxCount.Text = this.m_TreasureBoxNode.m_iMoveCount.ToString();
			this.m_LabelMoveCount.Text = this.m_TreasureBoxNode.m_iMoveCount.ToString();
			this.m_iMaxMove = this.m_TreasureBoxNode.m_iMoveCount;
			this.m_iColorAmount = this.m_TreasureBoxNode.m_iLevel;
			this.m_NumList.Clear();
			List<int> list = new List<int>();
			for (int i = 0; i < 45; i++)
			{
				if (this.m_iColorAmount == 2)
				{
					if (i < 22)
					{
						list.Add(1);
					}
					else
					{
						list.Add(2);
					}
				}
				else if (this.m_iColorAmount == 3)
				{
					if (i < 15)
					{
						list.Add(1);
					}
					else if (i >= 15 && i < 30)
					{
						list.Add(2);
					}
					else if (i >= 30)
					{
						list.Add(3);
					}
				}
				else if (this.m_iColorAmount == 4)
				{
					if (i < 11)
					{
						list.Add(1);
					}
					else if (i >= 11 && i < 22)
					{
						list.Add(2);
					}
					else if (i >= 22 && i < 33)
					{
						list.Add(3);
					}
					else if (i >= 33)
					{
						list.Add(4);
					}
				}
				else if (this.m_iColorAmount == 5)
				{
					if (i < 9)
					{
						list.Add(1);
					}
					else if (i >= 9 && i < 18)
					{
						list.Add(2);
					}
					else if (i >= 18 && i < 27)
					{
						list.Add(3);
					}
					else if (i >= 27 && i < 36)
					{
						list.Add(4);
					}
					else if (i >= 36)
					{
						list.Add(5);
					}
				}
			}
			for (int i = 0; i < this.m_iMaxGrid; i++)
			{
				int num = Random.Range(0, list.Count - 1);
				this.m_NumList.Add(list[num]);
				list.RemoveAt(num);
			}
			this.m_GameBackground.GameObject.SetActive(true);
			this.m_ExplanationBackground.GameObject.SetActive(true);
			base.EnterState(0);
			if (!GameCursor.IsShow)
			{
				base.SetCurrent(this.m_BtnMoveList[0].Listener, true);
			}
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0000BC53 File Offset: 0x00009E53
		private void BtnLeaveOnClick(GameObject go, bool t = true)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.SetEnd(false);
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0000BC68 File Offset: 0x00009E68
		private void selectThisEnter(GameObject go)
		{
			go.GetComponent<UISprite>().spriteName = string.Empty;
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0009CE30 File Offset: 0x0009B030
		private void SetIconData()
		{
			for (int i = 0; i < this.m_MiddleIconList.Count; i++)
			{
				for (int j = 0; j < this.m_NumList.Count; j++)
				{
					if (this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex == j)
					{
						this.m_MiddleIconList[i].GetComponent<UISprite>().spriteName = "forge_i060" + this.m_NumList[j].ToString();
					}
				}
			}
			for (int k = 9; k < this.m_NumList.Count; k++)
			{
				for (int l = 0; l < this.m_PeripheryIconList.Count; l++)
				{
					if (this.m_PeripheryIconList[l].GetComponent<ImageData>().m_iIndex == k)
					{
						this.m_PeripheryIconList[l].GetComponent<UISprite>().spriteName = "forge_i0" + this.m_NumList[k].ToString();
					}
				}
			}
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0009CF50 File Offset: 0x0009B150
		private void BtnMoveOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (this.m_bMoving)
			{
				return;
			}
			this.m_bMoving = true;
			int num = go.GetComponent<BtnData>().m_iBtnID % 12;
			int[] array = null;
			if (GameCursor.IsShow)
			{
				this.current = UIEventListener.Get(go.gameObject);
			}
			if (num == 0 || num == 1)
			{
				array = this.m_iGroup1;
			}
			else if (num == 2 || num == 3)
			{
				array = this.m_iGroup2;
			}
			else if (num == 4 || num == 5)
			{
				array = this.m_iGroup3;
			}
			else if (num == 6 || num == 7)
			{
				array = this.m_iGroup4;
			}
			else if (num == 8 || num == 9)
			{
				array = this.m_iGroup5;
			}
			else if (num == 10 || num == 11)
			{
				array = this.m_iGroup6;
			}
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < this.m_MiddleIconList.Count; j++)
				{
					if (this.m_MiddleIconList[j].GetComponent<ImageData>().m_iIndex == array[i])
					{
						int num3 = 0;
						int num4 = 0;
						if (num == 0 || num == 2 || num == 4)
						{
							num4 = 272;
							if (num == 0)
							{
								num2 = 36;
								num3 = -134;
							}
							else if (num == 2)
							{
								num2 = 37;
								num3 = 1;
							}
							else if (num == 4)
							{
								num2 = 38;
								num3 = 136;
							}
							if (array[i] == num2)
							{
								this.m_MiddleIconList[j].GameObject.transform.localPosition = new Vector3((float)num3, (float)num4, 0f);
							}
							else
							{
								Vector3 localPosition = this.m_MiddleIconList[j].GameObject.transform.localPosition;
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().from = localPosition;
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().to = new Vector3(localPosition.x, localPosition.y - 136f, localPosition.z);
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().ResetToBeginning();
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().Play();
							}
						}
						else if (num == 1 || num == 3 || num == 5)
						{
							num4 = -272;
							if (num == 1)
							{
								num2 = 15;
								num3 = -134;
							}
							else if (num == 3)
							{
								num2 = 16;
								num3 = 1;
							}
							else if (num == 5)
							{
								num2 = 17;
								num3 = 136;
							}
							if (array[i] == num2)
							{
								this.m_MiddleIconList[j].GameObject.transform.localPosition = new Vector3((float)num3, (float)num4, 0f);
							}
							else
							{
								Vector3 localPosition2 = this.m_MiddleIconList[j].GameObject.transform.localPosition;
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().from = localPosition2;
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().to = new Vector3(localPosition2.x, localPosition2.y + 136f, localPosition2.z);
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().ResetToBeginning();
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().Play();
							}
						}
						else if (num == 6 || num == 8 || num == 10)
						{
							num3 = 274;
							if (num == 6)
							{
								num2 = 20;
								num4 = 136;
							}
							else if (num == 8)
							{
								num2 = 23;
								num4 = 0;
							}
							else if (num == 10)
							{
								num2 = 26;
								num4 = -136;
							}
							if (array[i] == num2)
							{
								this.m_MiddleIconList[j].GameObject.transform.localPosition = new Vector3((float)num3, (float)num4, 0f);
							}
							else
							{
								Vector3 localPosition3 = this.m_MiddleIconList[j].GameObject.transform.localPosition;
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().from = localPosition3;
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().to = new Vector3(localPosition3.x - 136f, localPosition3.y, localPosition3.z);
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().ResetToBeginning();
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().Play();
							}
						}
						else if (num == 7 || num == 9 || num == 11)
						{
							num3 = -270;
							if (num == 7)
							{
								num2 = 27;
								num4 = 136;
							}
							else if (num == 9)
							{
								num2 = 30;
								num4 = 0;
							}
							else if (num == 11)
							{
								num2 = 33;
								num4 = -136;
							}
							if (array[i] == num2)
							{
								this.m_MiddleIconList[j].GameObject.transform.localPosition = new Vector3((float)num3, (float)num4, 0f);
							}
							else
							{
								Vector3 localPosition4 = this.m_MiddleIconList[j].GameObject.transform.localPosition;
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().from = localPosition4;
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().to = new Vector3(localPosition4.x + 136f, localPosition4.y, localPosition4.z);
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().ResetToBeginning();
								this.m_MiddleIconList[j].GetComponent<TweenPosition>().Play();
							}
						}
					}
				}
			}
			if (num == 0 || num == 2 || num == 4 || num == 7 || num == 9 || num == 11)
			{
				int num5 = this.m_NumList[array[array.Length - 1]];
				for (int i = array.Length - 1; i >= 0; i--)
				{
					if (i == 0)
					{
						this.m_NumList[array[i]] = num5;
					}
					else
					{
						this.m_NumList[array[i]] = this.m_NumList[array[i - 1]];
					}
				}
			}
			else if (num == 1 || num == 3 || num == 5 || num == 6 || num == 8 || num == 10)
			{
				int num5 = this.m_NumList[array[0]];
				for (int i = 0; i < array.Length; i++)
				{
					if (i == array.Length - 1)
					{
						this.m_NumList[array[i]] = num5;
					}
					else
					{
						this.m_NumList[array[i]] = this.m_NumList[array[i + 1]];
					}
				}
			}
			this.ChangeMiddleGridIndex(num);
			this.m_iMoveCount++;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x0000BC7A File Offset: 0x00009E7A
		private void BtnOnKeySelect(GameObject go, bool bSelect)
		{
			if (bSelect)
			{
				go.GetComponent<UISprite>().spriteName = "forge_plusOn";
			}
			else
			{
				go.GetComponent<UISprite>().spriteName = "forge_plus";
			}
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0009D690 File Offset: 0x0009B890
		protected override void SelectNextButton(KeyControl.Key direction)
		{
			List<UIEventListener> list = this.controls[this.NowState];
			if (this.current == null && list != null && list.Count > 0)
			{
				this.current = list[0];
				base.SetCurrent(this.current, true);
				return;
			}
			float num = float.MaxValue;
			int num2 = -1;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].enabled && list[i].gameObject.activeSelf)
				{
					if (list[i].GetComponent<BtnData>() == null)
					{
						list[i].gameObject.AddComponent<BtnData>();
					}
					if (list[i].GetComponent<BtnData>().m_iBtnType != 1)
					{
						Vector3 vector = list[i].transform.position - this.current.transform.position;
						if ((double)vector.sqrMagnitude <= 0.05)
						{
							if (this.IsVectorInDirection(vector, direction))
							{
								if (vector.sqrMagnitude < num)
								{
									num = vector.sqrMagnitude;
									num2 = i;
								}
							}
						}
					}
				}
			}
			if (num2 > -1)
			{
				base.SetCurrent(this.current, false);
				this.current = list[num2];
				base.SetCurrent(this.current, true);
			}
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0009D820 File Offset: 0x0009BA20
		private bool IsVectorInDirection(Vector2 v, KeyControl.Key dir)
		{
			float num = v.y / v.x;
			switch (dir)
			{
			case KeyControl.Key.Up:
				return v.y > 0f && (num >= 0.5f || (double)num <= -0.5);
			case KeyControl.Key.Down:
				return v.y <= 0f && (num >= 0.5f || num <= -0.5f);
			case KeyControl.Key.Left:
				return v.x <= 0f && 0.5f >= num && num >= -0.5f;
			case KeyControl.Key.Right:
				return v.x >= 0f && 0.5f >= num && num >= -0.5f;
			default:
				return false;
			}
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0009D918 File Offset: 0x0009BB18
		public override void OnMouseControl(bool bCtrl)
		{
			base.OnMouseControl(bCtrl);
			for (int i = 0; i < this.m_BtnMoveList.Count; i++)
			{
				this.m_BtnMoveList[i].SpriteName = "forge_plus";
			}
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x0009D960 File Offset: 0x0009BB60
		private void ChangeMiddleGridIndex(int iIndex)
		{
			if (iIndex == 0)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex2 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex2 == 36)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 15;
					}
					else if (iIndex2 == 15)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 0;
					}
					else if (iIndex2 == 0)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 3;
					}
					else if (iIndex2 == 3)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 6;
					}
					else if (iIndex2 == 6)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 36;
					}
				}
			}
			else if (iIndex == 2)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex3 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex3 == 37)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 16;
					}
					else if (iIndex3 == 16)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 1;
					}
					else if (iIndex3 == 1)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 4;
					}
					else if (iIndex3 == 4)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 7;
					}
					else if (iIndex3 == 7)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 37;
					}
				}
			}
			else if (iIndex == 4)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex4 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex4 == 38)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 17;
					}
					else if (iIndex4 == 17)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 2;
					}
					else if (iIndex4 == 2)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 5;
					}
					else if (iIndex4 == 5)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 8;
					}
					else if (iIndex4 == 8)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 38;
					}
				}
			}
			else if (iIndex == 1)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex5 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex5 == 0)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 15;
					}
					else if (iIndex5 == 3)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 0;
					}
					else if (iIndex5 == 6)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 3;
					}
					else if (iIndex5 == 36)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 6;
					}
					else if (iIndex5 == 15)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 36;
					}
				}
			}
			else if (iIndex == 3)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex6 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex6 == 1)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 16;
					}
					else if (iIndex6 == 4)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 1;
					}
					else if (iIndex6 == 7)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 4;
					}
					else if (iIndex6 == 37)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 7;
					}
					else if (iIndex6 == 16)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 37;
					}
				}
			}
			else if (iIndex == 5)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex7 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex7 == 2)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 17;
					}
					else if (iIndex7 == 5)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 2;
					}
					else if (iIndex7 == 8)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 5;
					}
					else if (iIndex7 == 38)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 8;
					}
					else if (iIndex7 == 17)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 38;
					}
				}
			}
			else if (iIndex == 6)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex8 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex8 == 0)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 20;
					}
					else if (iIndex8 == 1)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 0;
					}
					else if (iIndex8 == 2)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 1;
					}
					else if (iIndex8 == 27)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 2;
					}
					else if (iIndex8 == 20)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 27;
					}
				}
			}
			else if (iIndex == 8)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex9 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex9 == 3)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 23;
					}
					else if (iIndex9 == 4)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 3;
					}
					else if (iIndex9 == 5)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 4;
					}
					else if (iIndex9 == 30)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 5;
					}
					else if (iIndex9 == 23)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 30;
					}
				}
			}
			else if (iIndex == 10)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex10 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex10 == 6)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 26;
					}
					else if (iIndex10 == 7)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 6;
					}
					else if (iIndex10 == 8)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 7;
					}
					else if (iIndex10 == 33)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 8;
					}
					else if (iIndex10 == 26)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 33;
					}
				}
			}
			else if (iIndex == 7)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex11 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex11 == 2)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 27;
					}
					else if (iIndex11 == 1)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 2;
					}
					else if (iIndex11 == 0)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 1;
					}
					else if (iIndex11 == 20)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 0;
					}
					else if (iIndex11 == 27)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 20;
					}
				}
			}
			else if (iIndex == 9)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex12 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex12 == 5)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 30;
					}
					else if (iIndex12 == 4)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 5;
					}
					else if (iIndex12 == 3)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 4;
					}
					else if (iIndex12 == 23)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 3;
					}
					else if (iIndex12 == 30)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 23;
					}
				}
			}
			else if (iIndex == 11)
			{
				for (int i = 0; i < this.m_MiddleIconList.Count; i++)
				{
					int iIndex13 = this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex;
					if (iIndex13 == 8)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 33;
					}
					else if (iIndex13 == 7)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 8;
					}
					else if (iIndex13 == 6)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 7;
					}
					else if (iIndex13 == 26)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 6;
					}
					else if (iIndex13 == 33)
					{
						this.m_MiddleIconList[i].GetComponent<ImageData>().m_iIndex = 26;
					}
				}
			}
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0000BCA7 File Offset: 0x00009EA7
		public void PlayAniFinish()
		{
			if (this.m_iCount == 3)
			{
				this.SetIconData();
				this.CheckEnd();
				this.m_iCount = 0;
			}
			this.m_iCount++;
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0009E4BC File Offset: 0x0009C6BC
		private void CheckEnd()
		{
			bool flag = true;
			int num = this.m_NumList[0];
			for (int i = 0; i < 9; i++)
			{
				if (!flag)
				{
					break;
				}
				if (num != this.m_NumList[i])
				{
					flag = false;
				}
			}
			int num2 = this.m_iMaxMove - this.m_iMoveCount;
			this.m_LabelMoveCount.Text = num2.ToString();
			this.m_bMoving = false;
			if (flag)
			{
				this.SetEnd(flag);
			}
			else if (num2 == 0)
			{
				this.SetEnd(false);
			}
			else
			{
				this.m_bMoving = false;
			}
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0000BCD6 File Offset: 0x00009ED6
		public void CheckSetEnd()
		{
			if (this.m_Group.GameObject.activeSelf)
			{
				this.SetEnd(true);
			}
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x0009E55C File Offset: 0x0009C75C
		private void SetEnd(bool bWin)
		{
			if (this.m_bSetEnd)
			{
				return;
			}
			base.EnterState(1);
			this.bWin2Object = bWin;
			this.m_bSetEnd = true;
			this.m_EndBackground.GameObject.SetActive(true);
			if (bWin)
			{
				this.m_LabelEnd.Text = Game.StringTable.GetString(210018);
				GameGlobal.m_iGameResult = 1;
			}
			else
			{
				this.m_LabelEnd.Text = Game.StringTable.GetString(210017);
				GameGlobal.m_iGameResult = 0;
			}
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0000264F File Offset: 0x0000084F
		private void GetItemIconBGOnHover(GameObject go, bool bHover)
		{
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x0000BCF4 File Offset: 0x00009EF4
		private void BtnCloseKeyonSelect(GameObject go, bool t = false)
		{
			this.BtnCloseOnClick(go, false);
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x0009E5E8 File Offset: 0x0009C7E8
		private void ReSetData()
		{
			this.m_EndBackground.GameObject.SetActive(false);
			this.m_bSetEnd = false;
			this.m_iCount = 0;
			this.m_iMaxGrid = 45;
			this.m_bMoving = false;
			this.m_iMoveCount = 0;
			this.bWin2Object = false;
			for (int i = 0; i < this.m_GetItemIconBGList.Count; i++)
			{
				this.m_GetItemIconBGList[i].GameObject.SetActive(false);
			}
			for (int i = 0; i < this.m_GetItemIconList.Count; i++)
			{
				this.m_GetItemIconList[i].GameObject.SetActive(false);
			}
			for (int i = 0; i < this.m_LabelItemAmountList.Count; i++)
			{
				this.m_LabelItemAmountList[i].Text = string.Empty;
			}
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x040015D9 RID: 5593
		private Control m_Group;

		// Token: 0x040015DA RID: 5594
		private Control m_BtnLeave;

		// Token: 0x040015DB RID: 5595
		private Control m_ExplanationBackground;

		// Token: 0x040015DC RID: 5596
		private Control m_GameBackground;

		// Token: 0x040015DD RID: 5597
		private Control m_LabelMaxCount;

		// Token: 0x040015DE RID: 5598
		private Control m_LabelMoveCount;

		// Token: 0x040015DF RID: 5599
		private Control m_EndBackground;

		// Token: 0x040015E0 RID: 5600
		private Control m_BtnClose;

		// Token: 0x040015E1 RID: 5601
		private Control m_LabelEnd;

		// Token: 0x040015E2 RID: 5602
		private Control m_ItemNameBase;

		// Token: 0x040015E3 RID: 5603
		private Control m_LabelItemName;

		// Token: 0x040015E4 RID: 5604
		private List<Control> m_GetItemIconList;

		// Token: 0x040015E5 RID: 5605
		private List<Control> m_LabelItemAmountList;

		// Token: 0x040015E6 RID: 5606
		private List<Control> m_GetItemIconBGList;

		// Token: 0x040015E7 RID: 5607
		private List<Control> m_BtnMoveList;

		// Token: 0x040015E8 RID: 5608
		private List<Control> m_MiddleIconList;

		// Token: 0x040015E9 RID: 5609
		private List<Control> m_PeripheryIconList;

		// Token: 0x040015EA RID: 5610
		private TreasureBoxNode m_TreasureBoxNode;

		// Token: 0x040015EB RID: 5611
		private TreasureBox m_TreasureBox;

		// Token: 0x040015EC RID: 5612
		private List<int> m_NumList;

		// Token: 0x040015ED RID: 5613
		private int m_iMaxGrid;

		// Token: 0x040015EE RID: 5614
		private int m_iColorAmount;

		// Token: 0x040015EF RID: 5615
		private int m_iMaxMove;

		// Token: 0x040015F0 RID: 5616
		private int m_iMoveCount;

		// Token: 0x040015F1 RID: 5617
		private int m_iCount;

		// Token: 0x040015F2 RID: 5618
		private bool m_bMoving;

		// Token: 0x040015F3 RID: 5619
		private bool m_bSetEnd;

		// Token: 0x040015F4 RID: 5620
		private bool bWin2Object;

		// Token: 0x040015F5 RID: 5621
		private int[] m_iGroup1 = new int[]
		{
			9,
			12,
			15,
			0,
			3,
			6,
			36,
			39,
			42
		};

		// Token: 0x040015F6 RID: 5622
		private int[] m_iGroup2 = new int[]
		{
			10,
			13,
			16,
			1,
			4,
			7,
			37,
			40,
			43
		};

		// Token: 0x040015F7 RID: 5623
		private int[] m_iGroup3 = new int[]
		{
			11,
			14,
			17,
			2,
			5,
			8,
			38,
			41,
			44
		};

		// Token: 0x040015F8 RID: 5624
		private int[] m_iGroup4 = new int[]
		{
			18,
			19,
			20,
			0,
			1,
			2,
			27,
			28,
			29
		};

		// Token: 0x040015F9 RID: 5625
		private int[] m_iGroup5 = new int[]
		{
			21,
			22,
			23,
			3,
			4,
			5,
			30,
			31,
			32
		};

		// Token: 0x040015FA RID: 5626
		private int[] m_iGroup6 = new int[]
		{
			24,
			25,
			26,
			6,
			7,
			8,
			33,
			34,
			35
		};

		// Token: 0x02000330 RID: 816
		private enum eState
		{
			// Token: 0x040015FD RID: 5629
			SelectButton,
			// Token: 0x040015FE RID: 5630
			FinsihClose
		}
	}
}
