using System;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020002DA RID: 730
	public class Control
	{
		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06000EC7 RID: 3783 RVA: 0x00009DEE File Offset: 0x00007FEE
		// (remove) Token: 0x06000EC8 RID: 3784 RVA: 0x00009E14 File Offset: 0x00008014
		public event UIEventListener.VoidDelegate OnClick
		{
			add
			{
				this.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.ClickOK);
				UIEventListener uieventListener = this.listener;
				uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onClick, value);
			}
		}

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06000EC9 RID: 3785 RVA: 0x00009E32 File Offset: 0x00008032
		// (remove) Token: 0x06000ECA RID: 3786 RVA: 0x00009E50 File Offset: 0x00008050
		public event UIEventListener.VoidDelegate OnDoubleClick
		{
			add
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onDoubleClick, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onDoubleClick, value);
			}
		}

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06000ECB RID: 3787 RVA: 0x00009E6E File Offset: 0x0000806E
		// (remove) Token: 0x06000ECC RID: 3788 RVA: 0x00009E8C File Offset: 0x0000808C
		public event UIEventListener.VectorDelegate OnDrag
		{
			add
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener.onDrag, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uieventListener.onDrag, value);
			}
		}

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x06000ECD RID: 3789 RVA: 0x00009EAA File Offset: 0x000080AA
		// (remove) Token: 0x06000ECE RID: 3790 RVA: 0x00009EC8 File Offset: 0x000080C8
		public event UIEventListener.ObjectDelegate OnDrop
		{
			add
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onDrop = (UIEventListener.ObjectDelegate)Delegate.Combine(uieventListener.onDrop, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onDrop = (UIEventListener.ObjectDelegate)Delegate.Remove(uieventListener.onDrop, value);
			}
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06000ECF RID: 3791 RVA: 0x00009EE6 File Offset: 0x000080E6
		// (remove) Token: 0x06000ED0 RID: 3792 RVA: 0x00009F0C File Offset: 0x0000810C
		public event UIEventListener.BoolDelegate OnHover
		{
			add
			{
				this.SetPlaySound(UIPlaySound.Trigger.OnMouseOver, UIAudioData.eUIAudio.Hover);
				UIEventListener uieventListener = this.listener;
				uieventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onHover, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener.onHover, value);
			}
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06000ED1 RID: 3793 RVA: 0x00009F2A File Offset: 0x0000812A
		// (remove) Token: 0x06000ED2 RID: 3794 RVA: 0x00009F48 File Offset: 0x00008148
		public event UIEventListener.KeyCodeDelegate OnKey
		{
			add
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onKey = (UIEventListener.KeyCodeDelegate)Delegate.Combine(uieventListener.onKey, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onKey = (UIEventListener.KeyCodeDelegate)Delegate.Remove(uieventListener.onKey, value);
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06000ED3 RID: 3795 RVA: 0x00009F66 File Offset: 0x00008166
		// (remove) Token: 0x06000ED4 RID: 3796 RVA: 0x00009F84 File Offset: 0x00008184
		public event UIEventListener.BoolDelegate OnPress
		{
			add
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onPress, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener.onPress, value);
			}
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06000ED5 RID: 3797 RVA: 0x00009FA2 File Offset: 0x000081A2
		// (remove) Token: 0x06000ED6 RID: 3798 RVA: 0x00009FC0 File Offset: 0x000081C0
		public event UIEventListener.BoolDelegate OnTooltip
		{
			add
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onTooltip = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onTooltip, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onTooltip = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener.onTooltip, value);
			}
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06000ED7 RID: 3799 RVA: 0x00009FDE File Offset: 0x000081DE
		// (remove) Token: 0x06000ED8 RID: 3800 RVA: 0x00009FFC File Offset: 0x000081FC
		public event UIEventListener.FloatDelegate OnScroll
		{
			add
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onScroll = (UIEventListener.FloatDelegate)Delegate.Combine(uieventListener.onScroll, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onScroll = (UIEventListener.FloatDelegate)Delegate.Remove(uieventListener.onScroll, value);
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06000ED9 RID: 3801 RVA: 0x0000A01A File Offset: 0x0000821A
		// (remove) Token: 0x06000EDA RID: 3802 RVA: 0x0000A038 File Offset: 0x00008238
		public event UIEventListener.BoolDelegate OnSelect
		{
			add
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onSelect, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onSelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener.onSelect, value);
			}
		}

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06000EDB RID: 3803 RVA: 0x0000A056 File Offset: 0x00008256
		// (remove) Token: 0x06000EDC RID: 3804 RVA: 0x0000A074 File Offset: 0x00008274
		public event UIEventListener.VoidDelegate OnSubmit
		{
			add
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onSubmit = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onSubmit, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onSubmit = (UIEventListener.VoidDelegate)Delegate.Remove(uieventListener.onSubmit, value);
			}
		}

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06000EDD RID: 3805 RVA: 0x0000A092 File Offset: 0x00008292
		// (remove) Token: 0x06000EDE RID: 3806 RVA: 0x0000A0B8 File Offset: 0x000082B8
		public event UIEventListener.BoolDelegate OnKeySelect
		{
			add
			{
				this.SetPlaySound(UIPlaySound.Trigger.OnKeySelect, UIAudioData.eUIAudio.Hover);
				UIEventListener uieventListener = this.listener;
				uieventListener.onKeySelect = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener.onKeySelect, value);
			}
			remove
			{
				UIEventListener uieventListener = this.listener;
				uieventListener.onKeySelect = (UIEventListener.BoolDelegate)Delegate.Remove(uieventListener.onKeySelect, value);
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x0000A0D6 File Offset: 0x000082D6
		// (set) Token: 0x06000EE0 RID: 3808 RVA: 0x0000A0FA File Offset: 0x000082FA
		public string Text
		{
			get
			{
				if (this.label == null)
				{
					return string.Empty;
				}
				return this.label.text;
			}
			set
			{
				if (this.label != null)
				{
					this.label.text = value;
				}
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x0000A119 File Offset: 0x00008319
		public int LineCount
		{
			get
			{
				if (this.label == null)
				{
					return 0;
				}
				return this.label.processedText.Split(new char[]
				{
					'\n'
				}).Length;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x0007DB94 File Offset: 0x0007BD94
		// (set) Token: 0x06000EE3 RID: 3811 RVA: 0x0007DBBC File Offset: 0x0007BDBC
		public float X
		{
			get
			{
				return this.listener.transform.localPosition.x;
			}
			set
			{
				Vector3 localPosition = this.listener.transform.localPosition;
				localPosition.x = value * Control.WidthFactor;
				this.listener.transform.localPosition = localPosition;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x0007DBFC File Offset: 0x0007BDFC
		// (set) Token: 0x06000EE5 RID: 3813 RVA: 0x0007DC24 File Offset: 0x0007BE24
		public float Y
		{
			get
			{
				return this.listener.transform.localPosition.y;
			}
			set
			{
				Vector3 localPosition = this.listener.transform.localPosition;
				localPosition.y = value * Control.WidthFactor;
				this.listener.transform.localPosition = localPosition;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x0007DC64 File Offset: 0x0007BE64
		// (set) Token: 0x06000EE7 RID: 3815 RVA: 0x0007DC8C File Offset: 0x0007BE8C
		public float Width
		{
			get
			{
				return this.listener.transform.localScale.x;
			}
			set
			{
				Vector3 localScale = this.listener.transform.localScale;
				localScale.x = value * Control.WidthFactor;
				this.listener.transform.localScale = localScale;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x0007DCCC File Offset: 0x0007BECC
		// (set) Token: 0x06000EE9 RID: 3817 RVA: 0x0007DCF4 File Offset: 0x0007BEF4
		public float Height
		{
			get
			{
				return this.listener.transform.localScale.y;
			}
			set
			{
				Vector3 localScale = this.listener.transform.localScale;
				localScale.y = value * Control.WidthFactor;
				this.listener.transform.localScale = localScale;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000EEA RID: 3818 RVA: 0x0000A14B File Offset: 0x0000834B
		// (set) Token: 0x06000EEB RID: 3819 RVA: 0x0000A158 File Offset: 0x00008358
		public Texture Texture
		{
			get
			{
				return this.texture.mainTexture;
			}
			set
			{
				this.texture.mainTexture = value;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000EEC RID: 3820 RVA: 0x0000A166 File Offset: 0x00008366
		// (set) Token: 0x06000EED RID: 3821 RVA: 0x0000A173 File Offset: 0x00008373
		public Color TextureColor
		{
			get
			{
				return this.texture.color;
			}
			set
			{
				this.texture.color = value;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000EEE RID: 3822 RVA: 0x0000A181 File Offset: 0x00008381
		public UIEventListener Listener
		{
			get
			{
				return this.listener;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000EEF RID: 3823 RVA: 0x0000A189 File Offset: 0x00008389
		// (set) Token: 0x06000EF0 RID: 3824 RVA: 0x0000A196 File Offset: 0x00008396
		public string SpriteName
		{
			get
			{
				return this.sprite.spriteName;
			}
			set
			{
				this.sprite.spriteName = value;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0000A1A4 File Offset: 0x000083A4
		// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x0000A1B1 File Offset: 0x000083B1
		public float sliderValue
		{
			get
			{
				return this.Slider.sliderValue;
			}
			set
			{
				this.Slider.sliderValue = value;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0000A1BF File Offset: 0x000083BF
		// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x0000A1CC File Offset: 0x000083CC
		public float Alpha
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

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0000A1DA File Offset: 0x000083DA
		// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x0000A1E2 File Offset: 0x000083E2
		public GameObject GameObject
		{
			get
			{
				return this.gameObject;
			}
			private set
			{
				this.gameObject = value;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0000A1EB File Offset: 0x000083EB
		public Collider Collider
		{
			get
			{
				return this.gameObject.collider;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x0000A1F8 File Offset: 0x000083F8
		public UISprite UISprite
		{
			get
			{
				return this.sprite;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x0000A200 File Offset: 0x00008400
		public UILabel UILabel
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0000A208 File Offset: 0x00008408
		public T GetComponent<T>() where T : Component
		{
			return this.gameObject.GetComponent<T>();
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0007DD34 File Offset: 0x0007BF34
		public void SetPlaySound(UIPlaySound.Trigger trigger, UIAudioData.eUIAudio audioType)
		{
			UIPlaySound[] components = this.gameObject.GetComponents<UIPlaySound>();
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].trigger == trigger)
				{
					return;
				}
			}
			this.listener.SetAudio(trigger, audioType);
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0007DD80 File Offset: 0x0007BF80
		public static implicit operator Control(Transform t)
		{
			UIEventListener uieventListener = UIEventListener.Get(t.gameObject);
			if (uieventListener == null)
			{
				return null;
			}
			UILabel uilabel = t.GetComponent<UILabel>();
			if (uilabel == null)
			{
				uilabel = t.GetComponentInChildren<UILabel>();
			}
			UISprite uisprite = t.GetComponent<UISprite>();
			if (uisprite == null)
			{
				uisprite = t.GetComponentInChildren<UISprite>();
			}
			return new Control
			{
				label = uilabel,
				texture = uieventListener.GetComponent<UITexture>(),
				sprite = uisprite,
				panel = t.GetComponent<UIPanel>(),
				Slider = t.GetComponent<UISlider>(),
				gameObject = t.gameObject,
				listener = uieventListener
			};
		}

		// Token: 0x040011B8 RID: 4536
		protected GameObject gameObject;

		// Token: 0x040011B9 RID: 4537
		protected UIEventListener listener;

		// Token: 0x040011BA RID: 4538
		protected UILabel label;

		// Token: 0x040011BB RID: 4539
		protected UITexture texture;

		// Token: 0x040011BC RID: 4540
		protected UISprite sprite;

		// Token: 0x040011BD RID: 4541
		public UIPanel panel;

		// Token: 0x040011BE RID: 4542
		protected UISlider Slider;

		// Token: 0x040011BF RID: 4543
		public object obj;

		// Token: 0x040011C0 RID: 4544
		public static float WidthFactor = 640f / (float)Screen.width;

		// Token: 0x040011C1 RID: 4545
		public static float HeightFactor = 1136f / (float)Screen.height;

		// Token: 0x020002DB RID: 731
		public class ControlEx
		{
			// Token: 0x06000EFD RID: 3837 RVA: 0x0000A215 File Offset: 0x00008415
			public ControlEx(Transform t)
			{
				this.transform = t;
			}

			// Token: 0x170001B0 RID: 432
			// (get) Token: 0x06000EFE RID: 3838 RVA: 0x0000A224 File Offset: 0x00008424
			public Transform Transform
			{
				get
				{
					return this.transform;
				}
			}

			// Token: 0x170001B1 RID: 433
			// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0000A22C File Offset: 0x0000842C
			public GameObject GameObject
			{
				get
				{
					return this.transform.gameObject;
				}
			}

			// Token: 0x170001B2 RID: 434
			// (get) Token: 0x06000F00 RID: 3840 RVA: 0x0000A239 File Offset: 0x00008439
			// (set) Token: 0x06000F01 RID: 3841 RVA: 0x0000A246 File Offset: 0x00008446
			public float Alpha
			{
				get
				{
					return this.Get<UIPanel>().alpha;
				}
				set
				{
					this.Get<UIPanel>().alpha = value;
				}
			}

			// Token: 0x06000F02 RID: 3842 RVA: 0x0000A254 File Offset: 0x00008454
			protected T Get<T>() where T : Component
			{
				return this.transform.GetComponent<T>();
			}

			// Token: 0x040011C2 RID: 4546
			protected Transform transform;
		}

		// Token: 0x020002DC RID: 732
		public class Label : Control.ControlEx
		{
			// Token: 0x06000F03 RID: 3843 RVA: 0x0000A261 File Offset: 0x00008461
			public Label(Transform t) : base(t)
			{
			}

			// Token: 0x170001B3 RID: 435
			// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0000A26A File Offset: 0x0000846A
			// (set) Token: 0x06000F05 RID: 3845 RVA: 0x0000A277 File Offset: 0x00008477
			public string Text
			{
				get
				{
					return base.Get<UILabel>().text;
				}
				set
				{
					base.Get<UILabel>().text = value;
				}
			}
		}

		// Token: 0x020002DD RID: 733
		public class Sprite : Control.ControlEx
		{
			// Token: 0x06000F06 RID: 3846 RVA: 0x0000A261 File Offset: 0x00008461
			public Sprite(Transform t) : base(t)
			{
			}

			// Token: 0x170001B4 RID: 436
			// (get) Token: 0x06000F07 RID: 3847 RVA: 0x0000A285 File Offset: 0x00008485
			// (set) Token: 0x06000F08 RID: 3848 RVA: 0x0000A292 File Offset: 0x00008492
			public string SpriteName
			{
				get
				{
					return base.Get<UISprite>().spriteName;
				}
				set
				{
					base.Get<UISprite>().spriteName = value;
				}
			}
		}
	}
}
