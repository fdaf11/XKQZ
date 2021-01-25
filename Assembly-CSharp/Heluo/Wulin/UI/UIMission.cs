using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000367 RID: 871
	public class UIMission : UILayer
	{
		// Token: 0x060013D8 RID: 5080 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x000AB7D0 File Offset: 0x000A99D0
		private void Start()
		{
			CtrlMission missionController = this.m_MissionController;
			missionController.complementAmount = (Action<int>)Delegate.Combine(missionController.complementAmount, new Action<int>(this.ComplementAmount));
			CtrlMission missionController2 = this.m_MissionController;
			missionController2.setMissionView = (Action<int, string, bool>)Delegate.Combine(missionController2.setMissionView, new Action<int, string, bool>(this.SetMissionView));
			CtrlMission missionController3 = this.m_MissionController;
			missionController3.resetMission = (Action)Delegate.Combine(missionController3.resetMission, new Action(this.ResetMission));
			CtrlMission missionController4 = this.m_MissionController;
			missionController4.setTipView = (Action<string, string>)Delegate.Combine(missionController4.setTipView, new Action<string, string>(this.SetTipView));
			CtrlMission missionController5 = this.m_MissionController;
			missionController5.resetUIMission = (Action)Delegate.Combine(missionController5.resetUIMission, new Action(this.ResetUIMission));
			CtrlMission missionController6 = this.m_MissionController;
			missionController6.setRecordView = (Action<int>)Delegate.Combine(missionController6.setRecordView, new Action<int>(this.SetRecordView));
			CtrlMission missionController7 = this.m_MissionController;
			missionController7.setMouseActive = (Action<int, int, bool>)Delegate.Combine(missionController7.setMouseActive, new Action<int, int, bool>(this.SetMouseActive));
			this.onClickID = string.Empty;
			for (int i = 0; i < 30; i++)
			{
				this.CreatMission();
			}
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x000AB914 File Offset: 0x000A9B14
		public override void OnKeyUp(KeyControl.Key key)
		{
			int nowState = this.NowState;
			if (nowState == 1 || nowState == 2)
			{
				switch (key)
				{
				case KeyControl.Key.Up:
					this.MissionOnKey(key, true);
					break;
				case KeyControl.Key.Down:
					this.MissionOnKey(key, false);
					break;
				default:
					if (key == KeyControl.Key.Mission)
					{
						base.EnterState(0);
					}
					break;
				case KeyControl.Key.OK:
					base.OnCurrentClick();
					break;
				case KeyControl.Key.Cancel:
					this.Hide();
					break;
				case KeyControl.Key.L1:
					this.TitleOnKey(true);
					break;
				case KeyControl.Key.R1:
					this.TitleOnKey(false);
					break;
				}
			}
			Debug.Log(this.current);
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x000AB9DC File Offset: 0x000A9BDC
		protected override void OnStateEnter(int state)
		{
			if (state == 0)
			{
				this.Hide();
			}
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x000ABA04 File Offset: 0x000A9C04
		private void TitleOnKey(bool reverse = true)
		{
			if (reverse)
			{
				this.stateIndex--;
			}
			else
			{
				this.stateIndex++;
			}
			if (this.stateIndex < 1 || this.stateIndex > this.m_TitleList.Count)
			{
				this.stateIndex = Mathf.Clamp(this.stateIndex, 1, this.m_TitleList.Count);
				return;
			}
			this.TitleOnClick(this.m_TitleList[this.stateIndex - 1].GameObject);
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x000ABA98 File Offset: 0x000A9C98
		private void MissionOnKey(KeyControl.Key key, bool reverse = true)
		{
			this.SelectNextButton(key);
			int sortCount = this.m_MissionController.GetSortCount();
			int maxAmount = this.m_MissionController.GetMaxAmount();
			this.selectIndex = int.Parse(this.current.name);
			this.SetScrollBar(this.selectIndex, maxAmount, sortCount, reverse, this.m_MissionSlider);
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x000ABAF0 File Offset: 0x000A9CF0
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIMission.<>f__switch$map4D == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(10);
					dictionary.Add("Group", 0);
					dictionary.Add("MissionSlider", 1);
					dictionary.Add("Title", 2);
					dictionary.Add("TitleOnHover", 3);
					dictionary.Add("TitleOnClick", 4);
					dictionary.Add("GridMission", 5);
					dictionary.Add("MissionOn", 6);
					dictionary.Add("MissionOnClick", 7);
					dictionary.Add("MissionExit", 8);
					dictionary.Add("MissionScrollView", 9);
					UIMission.<>f__switch$map4D = dictionary;
				}
				int num;
				if (UIMission.<>f__switch$map4D.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_MissionSlider = sender;
						break;
					case 2:
					{
						Control control = sender;
						control.OnClick += this.TitleOnClick;
						control.OnHover += this.TitleOnHover;
						this.tIndex++;
						control.GameObject.name = this.tIndex.ToString();
						this.m_TitleList.Add(control);
						break;
					}
					case 3:
						this.m_TitleOnHover = sender;
						break;
					case 4:
						this.m_TitleOnClick = sender;
						break;
					case 5:
						this.m_GridMission = sender;
						break;
					case 6:
						this.m_MissionOn = sender;
						break;
					case 7:
						this.m_MissionOnClick = sender;
						break;
					case 8:
					{
						Control control = sender;
						control.OnHover += this.IconMissionExitOnHover;
						control.OnClick += this.IconMissionExitOnClick;
						this.m_MissionExit = sender;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
					case 9:
						this.m_MissionScrollView = sender;
						break;
					}
				}
			}
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x000ABD0C File Offset: 0x000A9F0C
		protected override void AssignWidget(Widget sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIMission.<>f__switch$map4E == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
					dictionary.Add("ExplainGroup", 0);
					UIMission.<>f__switch$map4E = dictionary;
				}
				int num;
				if (UIMission.<>f__switch$map4E.TryGetValue(name, ref num))
				{
					if (num == 0)
					{
						this.m_ExplainGroup = (sender as WgMissionTip);
					}
				}
			}
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x000ABD78 File Offset: 0x000A9F78
		public override void Show()
		{
			Game.UI.Hide<UIMainSelect>();
			if (this.m_bShow)
			{
				return;
			}
			GameGlobal.m_bCFormOpen = true;
			this.m_bShow = true;
			Game.g_InputManager.Push(this);
			this.m_Group.GameObject.SetActive(this.m_bShow);
			this.TitleOnClick(this.m_TitleList[this.stateIndex - 1].GameObject);
			this.m_MissionController.SetRecordData();
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x000ABDF4 File Offset: 0x000A9FF4
		public override void Hide()
		{
			this.m_MissionController.CloseView();
			this.m_MissionExit.SpriteName = "cdata_027";
			if (!this.m_bShow)
			{
				return;
			}
			GameGlobal.m_bCFormOpen = false;
			this.m_bShow = false;
			this.m_Group.GameObject.SetActive(this.m_bShow);
			Game.g_InputManager.Pop();
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0000CC6F File Offset: 0x0000AE6F
		private void IconMissionExitOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.Hide();
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x000ABE58 File Offset: 0x000AA058
		private void IconMissionExitOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				UISprite component = go.GetComponent<UISprite>();
				if (bHover)
				{
					component.spriteName = "cdata_028";
				}
				else
				{
					component.spriteName = "cdata_027";
				}
			}
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x000ABE98 File Offset: 0x000AA098
		private void TitleOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.SetControl(this.m_TitleOnClick, go, 83f, 5f, 0f);
			this.ResetTitleColor();
			go.GetComponent<UILabel>().color = Color.yellow;
			this.stateIndex = int.Parse(go.name);
			this.m_MissionController.SetMissionData(this.stateIndex - 1);
			this.m_MissionScrollView.GetComponent<UIScrollView>().ResetPosition();
			base.EnterState(this.stateIndex);
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x000ABF24 File Offset: 0x000AA124
		private void ResetTitleColor()
		{
			for (int i = 0; i < this.m_TitleList.Count; i++)
			{
				this.m_TitleList[i].GetComponent<UILabel>().color = new Color(0.8039216f, 0.52156866f, 0f, 1f);
			}
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0000CC83 File Offset: 0x0000AE83
		private void TitleOnHover(GameObject go, bool bHover)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
			this.OnGameObject(go, bHover, this.m_TitleOnHover, 83f, 5f, 0f, false);
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x000ABF7C File Offset: 0x000AA17C
		private void MissionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.SetControl(this.m_MissionOnClick, go, -60f, 1.3f, 0f);
			int tipData = int.Parse(go.name);
			this.m_MissionController.SetTipData(tipData);
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0000CCAE File Offset: 0x0000AEAE
		private void SetRecordView(int index)
		{
			this.MissionOnClick(this.m_NameList[index].GameObject);
			this.m_MissionOn.GameObject.SetActive(true);
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0000CCD8 File Offset: 0x0000AED8
		private void SetTipView(string name, string explain)
		{
			this.m_MissionOn.GameObject.SetActive(false);
			this.m_ExplainGroup.SetActive(true);
			this.m_ExplainGroup.SetMissionTip(name, explain);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0000CD04 File Offset: 0x0000AF04
		private void MissionOnHover(GameObject go, bool bHover)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
			this.OnGameObject(go, bHover, this.m_MissionOn, -60f, 1.3f, 0f, true);
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0000CD2F File Offset: 0x0000AF2F
		private void MissionOnKeySelect(GameObject go, bool bSelect)
		{
			this.ShowKeySelect(go, new Vector3(-20f, 0f, 0f), KeySelect.eSelectDir.Left, 20, 20);
			this.OnGameObject(go, bSelect, this.m_MissionOn, -60f, 1.3f, 0f, false);
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x000ABFCC File Offset: 0x000AA1CC
		private void OnGameObject(GameObject goes, bool bOn, Control control, float x, float y, float z, bool bMouse = false)
		{
			if (!goes.gameObject.activeSelf)
			{
				return;
			}
			if (bOn)
			{
				this.SetControl(control, goes, x, y, z);
				if (bMouse)
				{
					this.current = UIEventListener.Get(goes.gameObject);
				}
			}
			else
			{
				control.GameObject.SetActive(false);
			}
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x000AC028 File Offset: 0x000AA228
		private void ComplementAmount(int Count)
		{
			if (Count > this.m_NameList.Count)
			{
				int num = Count - this.m_NameList.Count;
				for (int i = 0; i < num; i++)
				{
					this.CreatMission();
				}
			}
			this.m_GridMission.GetComponent<UIGrid>().Reposition();
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x000AC07C File Offset: 0x000AA27C
		private void SetMissionView(int index, string name, bool bfinish)
		{
			this.m_NameList[index].GameObject.SetActive(true);
			this.m_NameList[index].Text = name;
			this.m_ConditionList[index].GameObject.SetActive(true);
			if (bfinish)
			{
				this.m_ConditionList[index].SpriteName = "ui_task_009";
			}
			else
			{
				this.m_ConditionList[index].SpriteName = null;
			}
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0000CD6F File Offset: 0x0000AF6F
		private void SetMouseActive(int count, int maxAmount, bool bReset)
		{
			this.CheckMouseWheel(count, maxAmount, this.m_MissionSlider, this.m_MissionScrollView, bReset);
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x000AC0FC File Offset: 0x000AA2FC
		private void ResetMission()
		{
			for (int i = 0; i < this.m_NameList.Count; i++)
			{
				this.m_NameList[i].GameObject.SetActive(false);
				this.m_ConditionList[i].GameObject.SetActive(false);
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x000AC154 File Offset: 0x000AA354
		private void ResetUIMission()
		{
			this.m_MissionOnClick.GameObject.SetActive(false);
			this.m_TitleOnHover.GameObject.SetActive(false);
			this.m_MissionOn.GameObject.SetActive(false);
			this.m_ExplainGroup.SetActive(false);
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0000CD86 File Offset: 0x0000AF86
		public override void OnMouseControl(bool bCtrl)
		{
			base.OnMouseControl(bCtrl);
			this.HideKeySelect();
			this.ResetUIMission();
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x000AC1A0 File Offset: 0x000AA3A0
		private void CreatMission()
		{
			GameObject gameObject = Object.Instantiate(this.m_Mission) as GameObject;
			gameObject.transform.parent = this.m_GridMission.GameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "Mission";
			gameObject.transform.Traversal(delegate(Transform x)
			{
				string name = x.name;
				if (name != null)
				{
					if (UIMission.<>f__switch$map50 == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
						dictionary.Add("Name", 0);
						dictionary.Add("Condition", 1);
						UIMission.<>f__switch$map50 = dictionary;
					}
					int num;
					if (UIMission.<>f__switch$map50.TryGetValue(name, ref num))
					{
						if (num != 0)
						{
							if (num == 1)
							{
								this.m_ConditionList.Add(x);
							}
						}
						else
						{
							x.OnClick += this.MissionOnClick;
							x.OnKeySelect += this.MissionOnKeySelect;
							x.OnHover += this.MissionOnHover;
							x.GameObject.name = this.index.ToString();
							UIEventListener listener = UIEventListener.Get(x.gameObject);
							base.SetInputButton(1, listener);
							base.SetInputButton(2, listener);
							this.m_NameList.Add(x);
						}
					}
				}
			});
			this.index++;
			this.m_GridMission.GetComponent<UIGrid>().Reposition();
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0000CD9B File Offset: 0x0000AF9B
		private void Update()
		{
			if (this.m_Group.GameObject.activeSelf && this.m_MissionSlider.GameObject.activeSelf)
			{
				this.SetMouseWheel(this.m_MissionSlider);
			}
		}

		// Token: 0x0400180B RID: 6155
		private CtrlMission m_MissionController = new CtrlMission();

		// Token: 0x0400180C RID: 6156
		public GameObject m_Mission;

		// Token: 0x0400180D RID: 6157
		private WgMissionTip m_ExplainGroup;

		// Token: 0x0400180E RID: 6158
		private Control m_Group;

		// Token: 0x0400180F RID: 6159
		private Control m_MissionSlider;

		// Token: 0x04001810 RID: 6160
		private List<Control> m_TitleList = new List<Control>();

		// Token: 0x04001811 RID: 6161
		private Control m_TitleOnHover;

		// Token: 0x04001812 RID: 6162
		private Control m_TitleOnClick;

		// Token: 0x04001813 RID: 6163
		private Control m_GridMission;

		// Token: 0x04001814 RID: 6164
		private List<Control> m_NameList = new List<Control>();

		// Token: 0x04001815 RID: 6165
		private List<Control> m_ConditionList = new List<Control>();

		// Token: 0x04001816 RID: 6166
		private Control m_MissionOn;

		// Token: 0x04001817 RID: 6167
		private Control m_MissionOnClick;

		// Token: 0x04001818 RID: 6168
		private Control m_MissionExit;

		// Token: 0x04001819 RID: 6169
		private int stateIndex = 1;

		// Token: 0x0400181A RID: 6170
		private int selectIndex;

		// Token: 0x0400181B RID: 6171
		private int index;

		// Token: 0x0400181C RID: 6172
		private int tIndex;

		// Token: 0x0400181D RID: 6173
		private string onClickID;

		// Token: 0x0400181E RID: 6174
		private Control m_MissionScrollView;

		// Token: 0x02000368 RID: 872
		private enum eState
		{
			// Token: 0x04001824 RID: 6180
			None,
			// Token: 0x04001825 RID: 6181
			enterUnderway,
			// Token: 0x04001826 RID: 6182
			enterFinish
		}
	}
}
