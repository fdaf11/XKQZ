using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200031F RID: 799
	public class UISaveRumor : UILayer
	{
		// Token: 0x06001163 RID: 4451 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0000B57D File Offset: 0x0000977D
		public void ClearRumorRecord()
		{
			this.rumorStrList.Clear();
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x000957D8 File Offset: 0x000939D8
		private void Start()
		{
			for (int i = 0; i < this.strMaxCount; i++)
			{
				this.CreatContent();
			}
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00095804 File Offset: 0x00093A04
		private void ContentOnKey(KeyControl.Key key, bool reverse = true)
		{
			this.SelectNextButton(key);
			Debug.Log(this.current.name);
			int iSelectIdx = int.Parse(this.current.name);
			this.SetScrollBar(iSelectIdx, this.showMaxCount, this.rumorStrList.Count, reverse, this.m_RumorSlider);
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00095858 File Offset: 0x00093A58
		public override void OnKeyUp(KeyControl.Key key)
		{
			int nowState = this.NowState;
			if (nowState == 1)
			{
				switch (key)
				{
				case KeyControl.Key.Up:
					this.ContentOnKey(key, true);
					goto IL_66;
				case KeyControl.Key.Down:
					this.ContentOnKey(key, false);
					goto IL_66;
				default:
					if (key != KeyControl.Key.Rumor)
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

		// Token: 0x06001168 RID: 4456 RVA: 0x000958D0 File Offset: 0x00093AD0
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UISaveRumor.<>f__switch$map17 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
					dictionary.Add("Group", 0);
					dictionary.Add("RumorGrid", 1);
					dictionary.Add("RumorExit", 2);
					dictionary.Add("RumorSlider", 3);
					dictionary.Add("RumorView", 4);
					UISaveRumor.<>f__switch$map17 = dictionary;
				}
				int num;
				if (UISaveRumor.<>f__switch$map17.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_RumorGrid = sender;
						break;
					case 2:
					{
						Control control = sender;
						control.OnHover += this.ExitSpriteOnHover;
						control.OnClick += this.ExitSpriteOnClick;
						this.m_RumorExit = sender;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
					case 3:
						this.m_RumorSlider = sender;
						break;
					case 4:
						this.m_RumorView = sender;
						break;
					}
				}
			}
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x000959F8 File Offset: 0x00093BF8
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			UIAudioDataManager.Singleton.PlayUIAudio(UIAudioData.eUIAudio.UpAndDown);
			this.m_bShow = true;
			GameGlobal.m_bCFormOpen = true;
			this.m_Group.GameObject.SetActive(true);
			Game.g_InputManager.Push(this);
			base.EnterState(1);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00095A50 File Offset: 0x00093C50
		public override void Hide()
		{
			if (!this.m_bShow)
			{
				return;
			}
			UIAudioDataManager.Singleton.PlayUIAudio(UIAudioData.eUIAudio.UpAndDown);
			this.m_bShow = false;
			GameGlobal.m_bCFormOpen = false;
			this.m_Group.GameObject.SetActive(false);
			Game.g_InputManager.Pop();
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00095AA0 File Offset: 0x00093CA0
		public void AddRumorList(Rumor rumor)
		{
			if (this.rumorStrList.Count == this.strMaxCount)
			{
				int num = this.rumorStrList.Count - 1;
				this.rumorStrList.RemoveAt(num);
			}
			this.rumorStrList.Insert(0, rumor);
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0000B58A File Offset: 0x0000978A
		public void LoadRumor(List<Rumor> LosdList)
		{
			this.rumorStrList.AddRange(LosdList);
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00095AEC File Offset: 0x00093CEC
		private void InitRumor()
		{
			for (int i = 0; i < this.m_ContentList.Count; i++)
			{
				this.m_ContentList[i].GameObject.SetActive(false);
			}
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00095B2C File Offset: 0x00093D2C
		private void SetRumorView()
		{
			this.InitRumor();
			for (int i = 0; i < this.rumorStrList.Count; i++)
			{
				this.m_ContentList[i].GameObject.SetActive(true);
				Texture texture = Game.g_BigHeadBundle.Load("2dtexture/gameui/bighead/" + this.rumorStrList[i].m_strImageId) as Texture;
				if (texture == null)
				{
					texture = (Game.g_BigHeadBundle.Load("2dtexture/gameui/bighead/B000001") as Texture);
				}
				this.m_ContentList[i].Texture = texture;
				this.m_ContentStrList[i].Text = this.rumorStrList[i].m_strTip;
			}
			this.CheckMouseWheel(this.rumorStrList.Count, this.showMaxCount, this.m_RumorSlider, this.m_RumorView, true);
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x0000B598 File Offset: 0x00009798
		private void ExitSpriteOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_RumorExit.SpriteName = "cdata_027";
			base.EnterState(0);
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x00095C18 File Offset: 0x00093E18
		private void ExitSpriteOnHover(GameObject go, bool bHover)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
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

		// Token: 0x06001171 RID: 4465 RVA: 0x0000B5BD File Offset: 0x000097BD
		private void ContentOnKeySelect(GameObject go, bool bSelect)
		{
			if (go.gameObject.activeSelf)
			{
				this.ShowKeySelect(go, new Vector3(-40f, -46.5f, 0f), KeySelect.eSelectDir.Left, 20, 20);
			}
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00095C58 File Offset: 0x00093E58
		public void CreatContent()
		{
			GameObject gameObject = Object.Instantiate(this.content) as GameObject;
			gameObject.transform.parent = this.m_RumorGrid.GameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "Content";
			gameObject.transform.Traversal(delegate(Transform x)
			{
				string name = x.name;
				if (name != null)
				{
					if (UISaveRumor.<>f__switch$map19 == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
						dictionary.Add("Content", 0);
						dictionary.Add("ContentStr", 1);
						UISaveRumor.<>f__switch$map19 = dictionary;
					}
					int num;
					if (UISaveRumor.<>f__switch$map19.TryGetValue(name, ref num))
					{
						if (num != 0)
						{
							if (num == 1)
							{
								this.m_ContentStrList.Add(x);
							}
						}
						else
						{
							x.OnKeySelect += this.ContentOnKeySelect;
							x.GameObject.name = this.cIndex.ToString();
							base.SetInputButton(1, x.Listener);
							this.m_ContentList.Add(x);
						}
					}
				}
			});
			this.cIndex++;
			this.m_RumorGrid.GetComponent<UIGrid>().Reposition();
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00095D00 File Offset: 0x00093F00
		protected override void OnStateEnter(int state)
		{
			if (state != 0)
			{
				if (state == 1)
				{
					this.SetRumorView();
					this.HideKeySelect();
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

		// Token: 0x06001174 RID: 4468 RVA: 0x0000B1AD File Offset: 0x000093AD
		public override void OnMouseControl(bool bCtrl)
		{
			base.OnMouseControl(bCtrl);
			this.HideKeySelect();
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x0000B5EF File Offset: 0x000097EF
		private void Update()
		{
			if (this.m_Group.GameObject.activeSelf && this.m_RumorSlider.GameObject.activeSelf)
			{
				this.SetMouseWheel(this.m_RumorSlider);
			}
		}

		// Token: 0x04001511 RID: 5393
		private Control m_Group;

		// Token: 0x04001512 RID: 5394
		public GameObject content;

		// Token: 0x04001513 RID: 5395
		private Control m_RumorGrid;

		// Token: 0x04001514 RID: 5396
		private List<Control> m_ContentList = new List<Control>();

		// Token: 0x04001515 RID: 5397
		private List<Control> m_ContentStrList = new List<Control>();

		// Token: 0x04001516 RID: 5398
		private Control m_RumorExit;

		// Token: 0x04001517 RID: 5399
		private Control m_RumorSlider;

		// Token: 0x04001518 RID: 5400
		private Control m_RumorView;

		// Token: 0x04001519 RID: 5401
		private List<Rumor> rumorStrList = new List<Rumor>();

		// Token: 0x0400151A RID: 5402
		private int cIndex;

		// Token: 0x0400151B RID: 5403
		private int money;

		// Token: 0x0400151C RID: 5404
		private int strMaxCount = 20;

		// Token: 0x0400151D RID: 5405
		private int showMaxCount = 7;

		// Token: 0x02000320 RID: 800
		private enum eState
		{
			// Token: 0x04001522 RID: 5410
			None,
			// Token: 0x04001523 RID: 5411
			SaveRumor
		}
	}
}
