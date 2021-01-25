using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200030C RID: 780
	public class UILayer : MonoBehaviour, IController
	{
		// Token: 0x060010B0 RID: 4272 RVA: 0x000910D0 File Offset: 0x0008F2D0
		protected void createKeySelect()
		{
			if (UILayer.m_KeySelect != null)
			{
				return;
			}
			GameObject gameObject = Resources.Load("MouseImage/MouseEffect/Efx_Key_Select") as GameObject;
			GameObject gameObject2 = Object.Instantiate(gameObject, new Vector3(0f, 500f, 0f), Quaternion.identity) as GameObject;
			gameObject2.transform.parent = Game.UI.Root.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.SetActive(true);
			UILayer.m_KeySelect = gameObject2.GetComponent<KeySelect>();
			this.HideKeySelect();
			Object.DontDestroyOnLoad(gameObject2);
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0000AE04 File Offset: 0x00009004
		protected void EnterState(int state)
		{
			if (this.NowState == state)
			{
				return;
			}
			this.OnStateExit(this.NowState);
			this.NowState = state;
			this.OnStateEnter(this.NowState);
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x0000AE32 File Offset: 0x00009032
		protected void BackState()
		{
			if (this.NowState == 0)
			{
				return;
			}
			this.EnterState(this.NowState - 1);
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x0009117C File Offset: 0x0008F37C
		protected virtual void OnStateEnter(int state)
		{
			this.current = null;
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
					this.SetCurrent(this.current, true);
				}
			}
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void OnStateExit(int state)
		{
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x0000AE4E File Offset: 0x0000904E
		// (set) Token: 0x060010B6 RID: 4278 RVA: 0x0000AE5B File Offset: 0x0000905B
		public virtual float Alpha
		{
			get
			{
				return this.panel.alpha;
			}
			set
			{
				this.panel.alpha = value;
			}
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0000AE69 File Offset: 0x00009069
		protected virtual void Awake()
		{
			Game.UI.Add(this);
			this.panel = base.GetComponent<UIPanel>();
			this.Traversal(base.transform);
			this.InitialData();
			this.createKeySelect();
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0000AE9A File Offset: 0x0000909A
		private void Traversal(Transform t)
		{
			this.AssignControl(t);
			t.ForEach(delegate(Transform x)
			{
				Widget component = x.GetComponent<Widget>();
				if (component == null)
				{
					this.Traversal(x);
				}
				else
				{
					component.InitWidget(this);
					this.AssignWidget(component);
				}
			});
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void AssignControl(Transform sender)
		{
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void AssignWidget(Widget sender)
		{
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void InitialData()
		{
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void Show()
		{
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x0000AEB5 File Offset: 0x000090B5
		public virtual void Hide()
		{
			UILayer.m_KeySelect.transform.parent = Game.UI.Root.transform;
			this.HideKeySelect();
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void ClearUIData()
		{
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x0000AEDB File Offset: 0x000090DB
		public virtual void HideKeySelect()
		{
			UILayer.m_KeySelect.Hide();
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x000911F0 File Offset: 0x0008F3F0
		public virtual void ShowKeySelect(GameObject goes, Vector3 adj, KeySelect.eSelectDir dir, int w = 64, int h = 64)
		{
			UILayer.m_KeySelect.SetArrowDir(dir);
			UILayer.m_KeySelect.SetSize(w, h);
			UILayer.m_KeySelect.gameObject.transform.parent = goes.transform;
			UILayer.m_KeySelect.gameObject.transform.localScale = Vector3.one;
			UILayer.m_KeySelect.gameObject.transform.localPosition = Vector3.zero + adj;
			UILayer.m_KeySelect.Show();
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0000AEE7 File Offset: 0x000090E7
		protected virtual void SetControl(GameObject src, GameObject dit, Vector3 adj)
		{
			src.SetActive(true);
			src.transform.parent = dit.transform;
			src.transform.localPosition = adj;
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0000AF0D File Offset: 0x0000910D
		protected virtual void SetControl(Control nowSet, GameObject goes, float x, float y, float z)
		{
			nowSet.GameObject.SetActive(true);
			nowSet.GameObject.transform.parent = goes.transform;
			nowSet.GameObject.transform.localPosition = new Vector3(x, y, z);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00091274 File Offset: 0x0008F474
		protected virtual void SetScrollBar(int iSelectIdx, int iShowAmount, int iMaxAmount, bool bReverse, Control ScrollBar)
		{
			float num = ScrollBar.sliderValue * (float)(iMaxAmount - iShowAmount) + 1f;
			int num2 = Mathf.Clamp((int)Mathf.Round(num), 1, iMaxAmount);
			int num3 = num2 + iShowAmount - 1;
			iSelectIdx++;
			GameDebugTool.Log("iSelectIdx : " + iSelectIdx);
			GameDebugTool.Log(string.Concat(new object[]
			{
				"sliderValue : ",
				ScrollBar.sliderValue,
				" f : ",
				Mathf.Round(num),
				" start : ",
				num2,
				" end : ",
				num3
			}));
			if (iSelectIdx < num2 || iSelectIdx > num3)
			{
				ScrollBar.GameObject.SetActive(true);
				float sliderValue;
				if (bReverse)
				{
					sliderValue = Mathf.Clamp((float)(num3 - iShowAmount - 1) / (float)(iMaxAmount - iShowAmount), 0f, 1f);
				}
				else
				{
					sliderValue = Mathf.Clamp((float)(num3 - iShowAmount + 1) / (float)(iMaxAmount - iShowAmount), 0f, 1f);
				}
				ScrollBar.sliderValue = sliderValue;
				GameDebugTool.Log("sliderValue : " + ScrollBar.sliderValue);
			}
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0000AF4B File Offset: 0x0000914B
		protected virtual void CheckMouseWheel(int nowAmount, int maxAmount, Control scrollSlider, Control scrollView, bool bReset = false)
		{
			if (nowAmount > maxAmount)
			{
				scrollSlider.GameObject.SetActive(true);
			}
			else
			{
				scrollSlider.GameObject.SetActive(false);
			}
			if (bReset)
			{
				scrollView.GetComponent<UIScrollView>().ResetPosition();
			}
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x000913A8 File Offset: 0x0008F5A8
		private IEnumerator ResetSliderValue(Control scrollSlider, float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			scrollSlider.sliderValue = 0f;
			yield break;
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x000913D8 File Offset: 0x0008F5D8
		protected virtual void SetMouseWheel(Control ScrollSlider)
		{
			if (Input.GetAxis("Mouse ScrollWheel") == 0f)
			{
				return;
			}
			float sliderValue = ScrollSlider.sliderValue;
			ScrollSlider.sliderValue = sliderValue - Input.GetAxis("Mouse ScrollWheel") / 4f;
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void OnMove(Vector2 diretion)
		{
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0009141C File Offset: 0x0008F61C
		public virtual void OnKeyUp(KeyControl.Key key)
		{
			switch (key)
			{
			case KeyControl.Key.Up:
			case KeyControl.Key.Down:
			case KeyControl.Key.Left:
			case KeyControl.Key.Right:
				this.SelectNextButton(key);
				break;
			case KeyControl.Key.OK:
				this.OnCurrentClick();
				break;
			case KeyControl.Key.Cancel:
				this.BackState();
				break;
			}
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void OnKeyDown(KeyControl.Key key)
		{
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void OnKeyHeld(KeyControl.Key key)
		{
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00091470 File Offset: 0x0008F670
		public virtual void OnMouseControl(bool bCtrl)
		{
			foreach (KeyValuePair<int, List<UIEventListener>> keyValuePair in this.controls)
			{
				for (int i = 0; i < keyValuePair.Value.Count; i++)
				{
					if (bCtrl)
					{
						if (keyValuePair.Value[i].onKeySelect != null)
						{
							keyValuePair.Value[i].onKeySelect(keyValuePair.Value[i].gameObject, false);
						}
					}
					else if (keyValuePair.Value[i].onHover != null)
					{
						keyValuePair.Value[i].onHover(keyValuePair.Value[i].gameObject, false);
					}
				}
			}
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00091570 File Offset: 0x0008F770
		protected virtual void SelectNextButton(KeyControl.Key direction)
		{
			if (!this.controls.ContainsKey(this.NowState))
			{
				return;
			}
			List<UIEventListener> list = this.controls[this.NowState];
			if (this.current == null && list != null && list.Count > 0)
			{
				this.current = list[0];
				this.SetCurrent(this.current, true);
				return;
			}
			float num = float.MaxValue;
			int num2 = -1;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].gameObject.collider.enabled && list[i].enabled && list[i].gameObject.activeSelf)
				{
					Vector3 vector = list[i].transform.position - this.current.transform.position;
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
			if (num2 > -1)
			{
				this.SetCurrent(this.current, false);
				this.current = list[num2];
				this.SetCurrent(this.current, true);
			}
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x000916D0 File Offset: 0x0008F8D0
		public void SetInputButton(int state, UIEventListener listener)
		{
			if (!this.controls.ContainsKey(state))
			{
				List<UIEventListener> list = new List<UIEventListener>();
				list.Add(listener);
				this.controls.Add(state, list);
			}
			else
			{
				List<UIEventListener> list2 = this.controls[state];
				list2.Add(listener);
			}
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0000AF84 File Offset: 0x00009184
		protected void SetCurrentOnly(UIEventListener val)
		{
			this.current = val;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00091724 File Offset: 0x0008F924
		protected void OnCurrentClick()
		{
			if (this.current)
			{
				if (!this.current.GetComponent<Collider>().enabled)
				{
					return;
				}
				if (!this.current.gameObject.activeSelf)
				{
					return;
				}
				this.current.OnClick();
			}
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0000AF8D File Offset: 0x0000918D
		protected void SetCurrent(UIEventListener val, bool show)
		{
			this.current = val;
			if (this.current != null && this.current.onKeySelect != null)
			{
				this.current.OnKeySelect(show);
			}
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00091778 File Offset: 0x0008F978
		private bool IsVectorInDirection(Vector2 v, KeyControl.Key dir)
		{
			float num = v.y / v.x;
			switch (dir)
			{
			case KeyControl.Key.Up:
				return v.y >= 0f && (num >= 1f || num <= -1f);
			case KeyControl.Key.Down:
				return v.y <= 0f && (num >= 1f || num <= -1f);
			case KeyControl.Key.Left:
				return v.x <= 0f && 1f >= num && num >= -1f;
			case KeyControl.Key.Right:
				return v.x >= 0f && 1f >= num && num >= -1f;
			default:
				return false;
			}
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0009186C File Offset: 0x0008FA6C
		public void CreateUIWidget<T>(string name, GameObject parent, GameObject prefabs, List<T> list, int cloneCount) where T : Widget
		{
			for (int i = 0; i < cloneCount; i++)
			{
				GameObject gameObject = Object.Instantiate(prefabs) as GameObject;
				gameObject.transform.parent = parent.transform;
				gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.name = name;
				T component = gameObject.GetComponent<T>();
				component.InitWidget(this);
				list.Add(component);
			}
		}

		// Token: 0x0400144B RID: 5195
		protected static KeySelect m_KeySelect;

		// Token: 0x0400144C RID: 5196
		protected bool m_bShow;

		// Token: 0x0400144D RID: 5197
		protected int NowState;

		// Token: 0x0400144E RID: 5198
		private UIPanel panel;

		// Token: 0x0400144F RID: 5199
		protected Dictionary<int, List<UIEventListener>> controls = new Dictionary<int, List<UIEventListener>>();

		// Token: 0x04001450 RID: 5200
		protected UIEventListener current;
	}
}
