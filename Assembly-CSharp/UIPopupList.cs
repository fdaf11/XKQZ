using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000470 RID: 1136
[AddComponentMenu("NGUI/Interaction/Popup List")]
[ExecuteInEditMode]
public class UIPopupList : UIWidgetContainer
{
	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06001B54 RID: 6996 RVA: 0x0001240A File Offset: 0x0001060A
	// (set) Token: 0x06001B55 RID: 6997 RVA: 0x000D65C4 File Offset: 0x000D47C4
	public Object ambigiousFont
	{
		get
		{
			if (this.trueTypeFont != null)
			{
				return this.trueTypeFont;
			}
			if (this.bitmapFont != null)
			{
				return this.bitmapFont;
			}
			return this.font;
		}
		set
		{
			if (value is Font)
			{
				this.trueTypeFont = (value as Font);
				this.bitmapFont = null;
				this.font = null;
			}
			else if (value is UIFont)
			{
				this.bitmapFont = (value as UIFont);
				this.trueTypeFont = null;
				this.font = null;
			}
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06001B56 RID: 6998 RVA: 0x00012442 File Offset: 0x00010642
	// (set) Token: 0x06001B57 RID: 6999 RVA: 0x0001244A File Offset: 0x0001064A
	[Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
	public UIPopupList.LegacyEvent onSelectionChange
	{
		get
		{
			return this.mLegacyEvent;
		}
		set
		{
			this.mLegacyEvent = value;
		}
	}

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06001B58 RID: 7000 RVA: 0x00012453 File Offset: 0x00010653
	public bool isOpen
	{
		get
		{
			return this.mChild != null;
		}
	}

	// Token: 0x1700020D RID: 525
	// (get) Token: 0x06001B59 RID: 7001 RVA: 0x00012461 File Offset: 0x00010661
	// (set) Token: 0x06001B5A RID: 7002 RVA: 0x00012469 File Offset: 0x00010669
	public string value
	{
		get
		{
			return this.mSelectedItem;
		}
		set
		{
			this.mSelectedItem = value;
			if (this.mSelectedItem == null)
			{
				return;
			}
			if (this.mSelectedItem != null)
			{
				this.TriggerCallbacks();
			}
		}
	}

	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06001B5B RID: 7003 RVA: 0x000D6620 File Offset: 0x000D4820
	public object data
	{
		get
		{
			int num = this.items.IndexOf(this.mSelectedItem);
			return (num >= this.itemData.Count) ? null : this.itemData[num];
		}
	}

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06001B5C RID: 7004 RVA: 0x0001248F File Offset: 0x0001068F
	// (set) Token: 0x06001B5D RID: 7005 RVA: 0x00012497 File Offset: 0x00010697
	[Obsolete("Use 'value' instead")]
	public string selection
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06001B5E RID: 7006 RVA: 0x000D6664 File Offset: 0x000D4864
	// (set) Token: 0x06001B5F RID: 7007 RVA: 0x000D6690 File Offset: 0x000D4890
	private bool handleEvents
	{
		get
		{
			UIKeyNavigation component = base.GetComponent<UIKeyNavigation>();
			return component == null || !component.enabled;
		}
		set
		{
			UIKeyNavigation component = base.GetComponent<UIKeyNavigation>();
			if (component != null)
			{
				component.enabled = !value;
			}
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06001B60 RID: 7008 RVA: 0x000124A0 File Offset: 0x000106A0
	private bool isValid
	{
		get
		{
			return this.bitmapFont != null || this.trueTypeFont != null;
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06001B61 RID: 7009 RVA: 0x000124C2 File Offset: 0x000106C2
	private int activeFontSize
	{
		get
		{
			return (!(this.trueTypeFont != null) && !(this.bitmapFont == null)) ? this.bitmapFont.defaultSize : this.fontSize;
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06001B62 RID: 7010 RVA: 0x000D66BC File Offset: 0x000D48BC
	private float activeFontScale
	{
		get
		{
			return (!(this.trueTypeFont != null) && !(this.bitmapFont == null)) ? ((float)this.fontSize / (float)this.bitmapFont.defaultSize) : 1f;
		}
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x000124FC File Offset: 0x000106FC
	public void Clear()
	{
		this.items.Clear();
		this.itemData.Clear();
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x00012514 File Offset: 0x00010714
	public void AddItem(string text)
	{
		this.items.Add(text);
		this.itemData.Add(null);
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x0001252E File Offset: 0x0001072E
	public void AddItem(string text, object data)
	{
		this.items.Add(text);
		this.itemData.Add(data);
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x000D670C File Offset: 0x000D490C
	protected void TriggerCallbacks()
	{
		if (UIPopupList.current != this)
		{
			UIPopupList uipopupList = UIPopupList.current;
			UIPopupList.current = this;
			if (this.mLegacyEvent != null)
			{
				this.mLegacyEvent(this.mSelectedItem);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				EventDelegate.Execute(this.onChange);
			}
			else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
			{
				this.eventReceiver.SendMessage(this.functionName, this.mSelectedItem, 1);
			}
			UIPopupList.current = uipopupList;
		}
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x000D67B0 File Offset: 0x000D49B0
	private void OnEnable()
	{
		if (EventDelegate.IsValid(this.onChange))
		{
			this.eventReceiver = null;
			this.functionName = null;
		}
		if (this.font != null)
		{
			if (this.font.isDynamic)
			{
				this.trueTypeFont = this.font.dynamicFont;
				this.fontStyle = this.font.dynamicFontStyle;
				this.mUseDynamicFont = true;
			}
			else if (this.bitmapFont == null)
			{
				this.bitmapFont = this.font;
				this.mUseDynamicFont = false;
			}
			this.font = null;
		}
		if (this.textScale != 0f)
		{
			this.fontSize = ((!(this.bitmapFont != null)) ? 16 : Mathf.RoundToInt((float)this.bitmapFont.defaultSize * this.textScale));
			this.textScale = 0f;
		}
		if (this.trueTypeFont == null && this.bitmapFont != null && this.bitmapFont.isDynamic)
		{
			this.trueTypeFont = this.bitmapFont.dynamicFont;
			this.bitmapFont = null;
		}
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x000D68F0 File Offset: 0x000D4AF0
	private void OnValidate()
	{
		Font font = this.trueTypeFont;
		UIFont uifont = this.bitmapFont;
		this.bitmapFont = null;
		this.trueTypeFont = null;
		if (font != null && (uifont == null || !this.mUseDynamicFont))
		{
			this.bitmapFont = null;
			this.trueTypeFont = font;
			this.mUseDynamicFont = true;
		}
		else if (uifont != null)
		{
			if (uifont.isDynamic)
			{
				this.trueTypeFont = uifont.dynamicFont;
				this.fontStyle = uifont.dynamicFontStyle;
				this.fontSize = uifont.defaultSize;
				this.mUseDynamicFont = true;
			}
			else
			{
				this.bitmapFont = uifont;
				this.mUseDynamicFont = false;
			}
		}
		else
		{
			this.trueTypeFont = font;
			this.mUseDynamicFont = true;
		}
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x000D69C0 File Offset: 0x000D4BC0
	private void Start()
	{
		if (this.textLabel != null)
		{
			EventDelegate.Add(this.onChange, new EventDelegate.Callback(this.textLabel.SetCurrentSelection));
			this.textLabel = null;
		}
		if (Application.isPlaying)
		{
			if (string.IsNullOrEmpty(this.mSelectedItem))
			{
				if (this.items.Count > 0)
				{
					this.value = this.items[0];
				}
			}
			else
			{
				string value = this.mSelectedItem;
				this.mSelectedItem = null;
				this.value = value;
			}
		}
	}

	// Token: 0x06001B6A RID: 7018 RVA: 0x00012548 File Offset: 0x00010748
	private void OnLocalize()
	{
		if (this.isLocalized)
		{
			this.TriggerCallbacks();
		}
	}

	// Token: 0x06001B6B RID: 7019 RVA: 0x000D6A5C File Offset: 0x000D4C5C
	private void Highlight(UILabel lbl, bool instant)
	{
		if (this.mHighlight != null)
		{
			this.mHighlightedLabel = lbl;
			if (this.mHighlight.GetAtlasSprite() == null)
			{
				return;
			}
			Vector3 highlightPosition = this.GetHighlightPosition();
			if (instant || !this.isAnimated)
			{
				this.mHighlight.cachedTransform.localPosition = highlightPosition;
			}
			else
			{
				TweenPosition.Begin(this.mHighlight.gameObject, 0.1f, highlightPosition).method = UITweener.Method.EaseOut;
				if (!this.mTweening)
				{
					this.mTweening = true;
					base.StartCoroutine(this.UpdateTweenPosition());
				}
			}
		}
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x000D6B00 File Offset: 0x000D4D00
	private Vector3 GetHighlightPosition()
	{
		if (this.mHighlightedLabel == null || this.mHighlight == null)
		{
			return Vector3.zero;
		}
		UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return Vector3.zero;
		}
		float pixelSize = this.atlas.pixelSize;
		float num = (float)atlasSprite.borderLeft * pixelSize;
		float num2 = (float)atlasSprite.borderTop * pixelSize;
		return this.mHighlightedLabel.cachedTransform.localPosition + new Vector3(-num, num2, 1f);
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x000D6B90 File Offset: 0x000D4D90
	private IEnumerator UpdateTweenPosition()
	{
		if (this.mHighlight != null && this.mHighlightedLabel != null)
		{
			TweenPosition tp = this.mHighlight.GetComponent<TweenPosition>();
			while (tp != null && tp.enabled)
			{
				tp.to = this.GetHighlightPosition();
				yield return null;
			}
		}
		this.mTweening = false;
		yield break;
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x000D6BAC File Offset: 0x000D4DAC
	private void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			UILabel component = go.GetComponent<UILabel>();
			this.Highlight(component, false);
		}
	}

	// Token: 0x06001B6F RID: 7023 RVA: 0x000D6BD0 File Offset: 0x000D4DD0
	private void Select(UILabel lbl, bool instant)
	{
		this.Highlight(lbl, instant);
		UIEventListener component = lbl.gameObject.GetComponent<UIEventListener>();
		this.value = (component.parameter as string);
		UIPlaySound[] components = base.GetComponents<UIPlaySound>();
		int i = 0;
		int num = components.Length;
		while (i < num)
		{
			UIPlaySound uiplaySound = components[i];
			if (uiplaySound.trigger == UIPlaySound.Trigger.OnClick)
			{
				NGUITools.PlaySound(uiplaySound.audioClip, uiplaySound.volume, 1f);
			}
			i++;
		}
	}

	// Token: 0x06001B70 RID: 7024 RVA: 0x0001255B File Offset: 0x0001075B
	private void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.Select(go.GetComponent<UILabel>(), true);
		}
	}

	// Token: 0x06001B71 RID: 7025 RVA: 0x00012570 File Offset: 0x00010770
	private void OnItemClick(GameObject go)
	{
		this.Close();
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x000D6C4C File Offset: 0x000D4E4C
	private void OnKey(KeyCode key)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.handleEvents)
		{
			int num = this.mLabelList.IndexOf(this.mHighlightedLabel);
			if (num == -1)
			{
				num = 0;
			}
			if (key == 273)
			{
				if (num > 0)
				{
					this.Select(this.mLabelList[num - 1], false);
				}
			}
			else if (key == 274)
			{
				if (num + 1 < this.mLabelList.Count)
				{
					this.Select(this.mLabelList[num + 1], false);
				}
			}
			else if (key == 27)
			{
				this.OnSelect(false);
			}
		}
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x00012570 File Offset: 0x00010770
	private void OnDisable()
	{
		this.Close();
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x00012578 File Offset: 0x00010778
	private void OnSelect(bool isSelected)
	{
		if (!isSelected)
		{
			this.Close();
		}
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x000D6D14 File Offset: 0x000D4F14
	public void Close()
	{
		if (this.mChild != null)
		{
			this.mLabelList.Clear();
			this.handleEvents = false;
			if (this.isAnimated)
			{
				UIWidget[] componentsInChildren = this.mChild.GetComponentsInChildren<UIWidget>();
				int i = 0;
				int num = componentsInChildren.Length;
				while (i < num)
				{
					UIWidget uiwidget = componentsInChildren[i];
					Color color = uiwidget.color;
					color.a = 0f;
					TweenColor.Begin(uiwidget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
					i++;
				}
				Collider[] componentsInChildren2 = this.mChild.GetComponentsInChildren<Collider>();
				int j = 0;
				int num2 = componentsInChildren2.Length;
				while (j < num2)
				{
					componentsInChildren2[j].enabled = false;
					j++;
				}
				Object.Destroy(this.mChild, 0.15f);
			}
			else
			{
				Object.Destroy(this.mChild);
			}
			this.mBackground = null;
			this.mHighlight = null;
			this.mChild = null;
		}
	}

	// Token: 0x06001B76 RID: 7030 RVA: 0x000D6E0C File Offset: 0x000D500C
	private void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x000D6E5C File Offset: 0x000D505C
	private void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 localPosition = widget.cachedTransform.localPosition;
		Vector3 localPosition2 = (!placeAbove) ? new Vector3(localPosition.x, 0f, localPosition.z) : new Vector3(localPosition.x, bottom, localPosition.z);
		widget.cachedTransform.localPosition = localPosition2;
		GameObject gameObject = widget.gameObject;
		TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x000D6ED4 File Offset: 0x000D50D4
	private void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject gameObject = widget.gameObject;
		Transform cachedTransform = widget.cachedTransform;
		float num = (float)this.activeFontSize * this.activeFontScale + this.mBgBorder * 2f;
		cachedTransform.localScale = new Vector3(1f, num / (float)widget.height, 1f);
		TweenScale.Begin(gameObject, 0.15f, Vector3.one).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 localPosition = cachedTransform.localPosition;
			cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - (float)widget.height + num, localPosition.z);
			TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
		}
	}

	// Token: 0x06001B79 RID: 7033 RVA: 0x00012586 File Offset: 0x00010786
	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		this.AnimateColor(widget);
		this.AnimatePosition(widget, placeAbove, bottom);
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x00012598 File Offset: 0x00010798
	private void OnClick()
	{
		if (this.openOn == UIPopupList.OpenOn.DoubleClick || this.openOn == UIPopupList.OpenOn.Manual)
		{
			return;
		}
		if (this.openOn == UIPopupList.OpenOn.RightClick && UICamera.currentTouchID != -2)
		{
			return;
		}
		this.Show();
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x000125D2 File Offset: 0x000107D2
	private void OnDoubleClick()
	{
		if (this.openOn == UIPopupList.OpenOn.DoubleClick)
		{
			this.Show();
		}
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x000D6F88 File Offset: 0x000D5188
	public void Show()
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.mChild == null && this.atlas != null && this.isValid && this.items.Count > 0)
		{
			this.mLabelList.Clear();
			if (this.mPanel == null)
			{
				this.mPanel = UIPanel.Find(base.transform);
				if (this.mPanel == null)
				{
					return;
				}
			}
			this.handleEvents = true;
			Transform transform = base.transform;
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform.parent, transform);
			this.mChild = new GameObject("Drop-down List");
			this.mChild.layer = base.gameObject.layer;
			Transform transform2 = this.mChild.transform;
			transform2.parent = transform.parent;
			transform2.localPosition = bounds.min;
			transform2.localRotation = Quaternion.identity;
			transform2.localScale = Vector3.one;
			this.mBackground = NGUITools.AddSprite(this.mChild, this.atlas, this.backgroundSprite);
			this.mBackground.pivot = UIWidget.Pivot.TopLeft;
			this.mBackground.depth = NGUITools.CalculateNextDepth(this.mPanel.gameObject);
			this.mBackground.color = this.backgroundColor;
			Vector4 border = this.mBackground.border;
			this.mBgBorder = border.y;
			this.mBackground.cachedTransform.localPosition = new Vector3(0f, border.y, 0f);
			this.mHighlight = NGUITools.AddSprite(this.mChild, this.atlas, this.highlightSprite);
			this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
			this.mHighlight.color = this.highlightColor;
			UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return;
			}
			float num = (float)atlasSprite.borderTop;
			float num2 = (float)this.activeFontSize;
			float activeFontScale = this.activeFontScale;
			float num3 = num2 * activeFontScale;
			float num4 = 0f;
			float num5 = -this.padding.y;
			List<UILabel> list = new List<UILabel>();
			if (!this.items.Contains(this.mSelectedItem))
			{
				this.mSelectedItem = null;
			}
			int i = 0;
			int count = this.items.Count;
			while (i < count)
			{
				string text = this.items[i];
				UILabel uilabel = NGUITools.AddWidget<UILabel>(this.mChild);
				uilabel.name = i.ToString();
				uilabel.pivot = UIWidget.Pivot.TopLeft;
				uilabel.bitmapFont = this.bitmapFont;
				uilabel.trueTypeFont = this.trueTypeFont;
				uilabel.fontSize = this.fontSize;
				uilabel.fontStyle = this.fontStyle;
				uilabel.text = ((!this.isLocalized) ? text : Localization.Get(text));
				uilabel.color = this.textColor;
				uilabel.cachedTransform.localPosition = new Vector3(border.x + this.padding.x - uilabel.pivotOffset.x, num5, -1f);
				uilabel.overflowMethod = UILabel.Overflow.ResizeFreely;
				uilabel.alignment = this.alignment;
				list.Add(uilabel);
				num5 -= num3;
				num5 -= this.padding.y;
				num4 = Mathf.Max(num4, uilabel.printedSize.x);
				UIEventListener uieventListener = UIEventListener.Get(uilabel.gameObject);
				uieventListener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
				uieventListener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
				uieventListener.onClick = new UIEventListener.VoidDelegate(this.OnItemClick);
				uieventListener.parameter = text;
				if (this.mSelectedItem == text || (i == 0 && string.IsNullOrEmpty(this.mSelectedItem)))
				{
					this.Highlight(uilabel, true);
				}
				this.mLabelList.Add(uilabel);
				i++;
			}
			num4 = Mathf.Max(num4, bounds.size.x * activeFontScale - (border.x + this.padding.x) * 2f);
			float num6 = num4;
			Vector3 vector;
			vector..ctor(num6 * 0.5f, -num2 * 0.5f, 0f);
			Vector3 vector2;
			vector2..ctor(num6, num3 + this.padding.y, 1f);
			int j = 0;
			int count2 = list.Count;
			while (j < count2)
			{
				UILabel uilabel2 = list[j];
				NGUITools.AddWidgetCollider(uilabel2.gameObject);
				uilabel2.autoResizeBoxCollider = false;
				BoxCollider component = uilabel2.GetComponent<BoxCollider>();
				if (component != null)
				{
					vector.z = component.center.z;
					component.center = vector;
					component.size = vector2;
				}
				else
				{
					BoxCollider2D component2 = uilabel2.GetComponent<BoxCollider2D>();
					component2.center = vector;
					component2.size = vector2;
				}
				j++;
			}
			int width = Mathf.RoundToInt(num4);
			num4 += (border.x + this.padding.x) * 2f;
			num5 -= border.y;
			this.mBackground.width = Mathf.RoundToInt(num4);
			this.mBackground.height = Mathf.RoundToInt(-num5 + border.y);
			int k = 0;
			int count3 = list.Count;
			while (k < count3)
			{
				UILabel uilabel3 = list[k];
				uilabel3.overflowMethod = UILabel.Overflow.ShrinkContent;
				uilabel3.width = width;
				k++;
			}
			float num7 = 2f * this.atlas.pixelSize;
			float num8 = num4 - (border.x + this.padding.x) * 2f + (float)atlasSprite.borderLeft * num7;
			float num9 = num3 + num * num7;
			this.mHighlight.width = Mathf.RoundToInt(num8);
			this.mHighlight.height = Mathf.RoundToInt(num9);
			bool flag = this.position == UIPopupList.Position.Above;
			if (this.position == UIPopupList.Position.Auto)
			{
				UICamera uicamera = UICamera.FindCameraForLayer(base.gameObject.layer);
				if (uicamera != null)
				{
					flag = (uicamera.cachedCamera.WorldToViewportPoint(transform.position).y < 0.5f);
				}
			}
			if (this.isAnimated)
			{
				float bottom = num5 + num3;
				this.Animate(this.mHighlight, flag, bottom);
				int l = 0;
				int count4 = list.Count;
				while (l < count4)
				{
					this.Animate(list[l], flag, bottom);
					l++;
				}
				this.AnimateColor(this.mBackground);
				this.AnimateScale(this.mBackground, flag, bottom);
			}
			if (flag)
			{
				transform2.localPosition = new Vector3(bounds.min.x, bounds.max.y - num5 - border.y, bounds.min.z);
			}
		}
		else
		{
			this.OnSelect(false);
		}
	}

	// Token: 0x04002035 RID: 8245
	private const float animSpeed = 0.15f;

	// Token: 0x04002036 RID: 8246
	public static UIPopupList current;

	// Token: 0x04002037 RID: 8247
	public UIAtlas atlas;

	// Token: 0x04002038 RID: 8248
	public UIFont bitmapFont;

	// Token: 0x04002039 RID: 8249
	public Font trueTypeFont;

	// Token: 0x0400203A RID: 8250
	public int fontSize = 16;

	// Token: 0x0400203B RID: 8251
	public FontStyle fontStyle;

	// Token: 0x0400203C RID: 8252
	public string backgroundSprite;

	// Token: 0x0400203D RID: 8253
	public string highlightSprite;

	// Token: 0x0400203E RID: 8254
	public UIPopupList.Position position;

	// Token: 0x0400203F RID: 8255
	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	// Token: 0x04002040 RID: 8256
	public List<string> items = new List<string>();

	// Token: 0x04002041 RID: 8257
	public List<object> itemData = new List<object>();

	// Token: 0x04002042 RID: 8258
	public Vector2 padding = new Vector3(4f, 4f);

	// Token: 0x04002043 RID: 8259
	public Color textColor = Color.white;

	// Token: 0x04002044 RID: 8260
	public Color backgroundColor = Color.white;

	// Token: 0x04002045 RID: 8261
	public Color highlightColor = new Color(0.88235295f, 0.78431374f, 0.5882353f, 1f);

	// Token: 0x04002046 RID: 8262
	public bool isAnimated = true;

	// Token: 0x04002047 RID: 8263
	public bool isLocalized;

	// Token: 0x04002048 RID: 8264
	public UIPopupList.OpenOn openOn;

	// Token: 0x04002049 RID: 8265
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x0400204A RID: 8266
	[SerializeField]
	[HideInInspector]
	private string mSelectedItem;

	// Token: 0x0400204B RID: 8267
	private UIPanel mPanel;

	// Token: 0x0400204C RID: 8268
	private GameObject mChild;

	// Token: 0x0400204D RID: 8269
	private UISprite mBackground;

	// Token: 0x0400204E RID: 8270
	private UISprite mHighlight;

	// Token: 0x0400204F RID: 8271
	private UILabel mHighlightedLabel;

	// Token: 0x04002050 RID: 8272
	private List<UILabel> mLabelList = new List<UILabel>();

	// Token: 0x04002051 RID: 8273
	private float mBgBorder;

	// Token: 0x04002052 RID: 8274
	[SerializeField]
	[HideInInspector]
	private GameObject eventReceiver;

	// Token: 0x04002053 RID: 8275
	[HideInInspector]
	[SerializeField]
	private string functionName = "OnSelectionChange";

	// Token: 0x04002054 RID: 8276
	[HideInInspector]
	[SerializeField]
	private float textScale;

	// Token: 0x04002055 RID: 8277
	[SerializeField]
	[HideInInspector]
	private UIFont font;

	// Token: 0x04002056 RID: 8278
	[SerializeField]
	[HideInInspector]
	private UILabel textLabel;

	// Token: 0x04002057 RID: 8279
	private UIPopupList.LegacyEvent mLegacyEvent;

	// Token: 0x04002058 RID: 8280
	private bool mUseDynamicFont;

	// Token: 0x04002059 RID: 8281
	private bool mTweening;

	// Token: 0x02000471 RID: 1137
	public enum Position
	{
		// Token: 0x0400205B RID: 8283
		Auto,
		// Token: 0x0400205C RID: 8284
		Above,
		// Token: 0x0400205D RID: 8285
		Below
	}

	// Token: 0x02000472 RID: 1138
	public enum OpenOn
	{
		// Token: 0x0400205F RID: 8287
		ClickOrTap,
		// Token: 0x04002060 RID: 8288
		RightClick,
		// Token: 0x04002061 RID: 8289
		DoubleClick,
		// Token: 0x04002062 RID: 8290
		Manual
	}

	// Token: 0x02000473 RID: 1139
	// (Invoke) Token: 0x06001B7E RID: 7038
	public delegate void LegacyEvent(string val);
}
