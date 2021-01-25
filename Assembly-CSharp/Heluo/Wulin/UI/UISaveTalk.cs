using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000372 RID: 882
	public class UISaveTalk : UILayer
	{
		// Token: 0x06001445 RID: 5189 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x000AEE74 File Offset: 0x000AD074
		private void Start()
		{
			for (int i = 0; i < this.strMaxAmount; i++)
			{
				this.CreatSaveTalk();
			}
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x000AEEA0 File Offset: 0x000AD0A0
		public override void OnKeyUp(KeyControl.Key key)
		{
			int nowState = this.NowState;
			if (nowState == 1)
			{
				switch (key)
				{
				case KeyControl.Key.Up:
					this.SaveTalkOnKey(key, true);
					goto IL_66;
				case KeyControl.Key.Down:
					this.SaveTalkOnKey(key, false);
					goto IL_66;
				default:
					if (key != KeyControl.Key.TalkLog)
					{
						goto IL_66;
					}
					break;
				case KeyControl.Key.Cancel:
					break;
				}
				base.EnterState(0);
				IL_66:;
			}
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x000AEF18 File Offset: 0x000AD118
		private void SaveTalkOnKey(KeyControl.Key key, bool reverse = true)
		{
			this.SelectNextButton(key);
			Debug.Log(this.current.name);
			int iSelectIdx = int.Parse(this.current.name);
			this.SetScrollBar(iSelectIdx, this.showMaxAmount, this.m_StrList.Count, reverse, this.m_LogSlider);
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x000AEF6C File Offset: 0x000AD16C
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UISaveTalk.<>f__switch$map59 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
					dictionary.Add("Group", 0);
					dictionary.Add("StrGrid", 1);
					dictionary.Add("LogSlider", 2);
					dictionary.Add("TalkLogExit", 3);
					dictionary.Add("LogView", 4);
					UISaveTalk.<>f__switch$map59 = dictionary;
				}
				int num;
				if (UISaveTalk.<>f__switch$map59.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_StrGrid = sender;
						break;
					case 2:
						this.m_LogSlider = sender;
						break;
					case 3:
					{
						Control control = sender;
						control.OnHover += this.ExitSpriteOnHover;
						control.OnClick += this.ExitSpriteOnClick;
						this.m_TalkLogExit = sender;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
					case 4:
						this.m_LogView = sender;
						break;
					}
				}
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x000AF094 File Offset: 0x000AD294
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			Game.g_InputManager.Push(this);
			this.m_Group.GameObject.SetActive(this.m_bShow);
			GameGlobal.m_bCFormOpen = this.m_bShow;
			this.UpdateTalkLogData();
			base.EnterState(1);
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x000AF0F0 File Offset: 0x000AD2F0
		public override void Hide()
		{
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			this.m_Group.GameObject.SetActive(this.m_bShow);
			GameGlobal.m_bCFormOpen = this.m_bShow;
			Game.g_InputManager.Pop();
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0000D0E4 File Offset: 0x0000B2E4
		private void ExitSpriteOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_TalkLogExit.SpriteName = "cdata_027";
			base.EnterState(0);
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x000ABE58 File Offset: 0x000AA058
		private void ExitSpriteOnHover(GameObject go, bool bHover)
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

		// Token: 0x0600144E RID: 5198 RVA: 0x0000D109 File Offset: 0x0000B309
		private void SaveTalkOnKeySelect(GameObject go, bool bHover)
		{
			this.ShowKeySelect(go, new Vector3(-385f, 437.5f, 0f), KeySelect.eSelectDir.Left, 20, 20);
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0000D12B File Offset: 0x0000B32B
		private void UpdateTalkLogData()
		{
			this.m_StrList = Game.UI.Get<UITalk>().m_TalkStrList;
			this.m_NameList = Game.UI.Get<UITalk>().m_TalkNameList;
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x000AF13C File Offset: 0x000AD33C
		private void InitSaveTalk()
		{
			for (int i = 0; i < this.m_SaveTalkList.Count; i++)
			{
				this.m_SaveTalkList[i].GameObject.SetActive(false);
			}
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x000AF17C File Offset: 0x000AD37C
		private void SetSaveTalkData()
		{
			this.InitSaveTalk();
			for (int i = 0; i < this.m_StrList.Count; i++)
			{
				this.m_StrList[i] = this.m_StrList[i].Replace("\n", string.Empty);
				this.m_SaveTalkList[i].GameObject.SetActive(true);
				this.m_SaveTalkStrList[i].Text = this.m_StrList[i];
				this.m_SaveTalkNameList[i].Text = this.m_NameList[i];
			}
			this.CheckMouseWheel(this.m_SaveTalkStrList.Count, this.showMaxAmount, this.m_LogSlider, this.m_LogView, false);
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x000AF248 File Offset: 0x000AD448
		public void CreatSaveTalk()
		{
			GameObject gameObject = Object.Instantiate(this.SaveTalk) as GameObject;
			gameObject.transform.parent = this.m_StrGrid.GameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "SaveTalk";
			gameObject.transform.Traversal(delegate(Transform x)
			{
				string name = x.name;
				if (name != null)
				{
					if (UISaveTalk.<>f__switch$map5B == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
						dictionary.Add("SaveTalk", 0);
						dictionary.Add("SaveTalkName", 1);
						dictionary.Add("SaveTalkStr", 2);
						UISaveTalk.<>f__switch$map5B = dictionary;
					}
					int num;
					if (UISaveTalk.<>f__switch$map5B.TryGetValue(name, ref num))
					{
						switch (num)
						{
						case 0:
							x.GameObject.name = this.index.ToString();
							x.OnKeySelect += this.SaveTalkOnKeySelect;
							base.SetInputButton(1, x.Listener);
							this.m_SaveTalkList.Add(x);
							break;
						case 1:
							this.m_SaveTalkNameList.Add(x);
							break;
						case 2:
							this.m_SaveTalkStrList.Add(x);
							break;
						}
					}
				}
			});
			this.index++;
			this.m_StrGrid.GetComponent<UIGrid>().Reposition();
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x000AF2F0 File Offset: 0x000AD4F0
		protected override void OnStateEnter(int state)
		{
			if (state != 0)
			{
				if (state == 1)
				{
					this.SetSaveTalkData();
				}
			}
			else
			{
				this.Hide();
			}
			if (!GameCursor.IsShow)
			{
				if (!this.controls.ContainsKey(this.NowState))
				{
					return;
				}
				List<UIEventListener> list = this.controls[this.NowState];
				if (list != null && list.Count > 0)
				{
					this.current = list[0];
					if (!this.current.gameObject.collider.enabled || !this.current.gameObject.activeSelf)
					{
						return;
					}
					base.SetCurrent(this.current, true);
				}
			}
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0000D157 File Offset: 0x0000B357
		public override void OnMouseControl(bool bCtrl)
		{
			base.OnMouseControl(bCtrl);
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0000D160 File Offset: 0x0000B360
		private void Update()
		{
			if (this.m_Group.GameObject.activeSelf && this.m_LogSlider.GameObject.activeSelf)
			{
				this.SetMouseWheel(this.m_LogSlider);
			}
		}

		// Token: 0x04001892 RID: 6290
		private List<Control> m_SaveTalkList = new List<Control>();

		// Token: 0x04001893 RID: 6291
		private List<Control> m_SaveTalkStrList = new List<Control>();

		// Token: 0x04001894 RID: 6292
		private List<Control> m_SaveTalkNameList = new List<Control>();

		// Token: 0x04001895 RID: 6293
		private List<string> m_StrList = new List<string>();

		// Token: 0x04001896 RID: 6294
		private List<string> m_NameList = new List<string>();

		// Token: 0x04001897 RID: 6295
		public GameObject SaveTalk;

		// Token: 0x04001898 RID: 6296
		private Control m_LogView;

		// Token: 0x04001899 RID: 6297
		private Control m_StrGrid;

		// Token: 0x0400189A RID: 6298
		private Control m_Group;

		// Token: 0x0400189B RID: 6299
		private Control m_LogSlider;

		// Token: 0x0400189C RID: 6300
		private Control m_TalkLogExit;

		// Token: 0x0400189D RID: 6301
		private bool m_bMouseWheel;

		// Token: 0x0400189E RID: 6302
		private int showMaxAmount = 4;

		// Token: 0x0400189F RID: 6303
		private int strMaxAmount = 20;

		// Token: 0x040018A0 RID: 6304
		private int index;

		// Token: 0x02000373 RID: 883
		private enum eState
		{
			// Token: 0x040018A5 RID: 6309
			None,
			// Token: 0x040018A6 RID: 6310
			SaveTalk
		}
	}
}
