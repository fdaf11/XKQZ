using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000444 RID: 1092
[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06001A32 RID: 6706 RVA: 0x000D1880 File Offset: 0x000CFA80
	// (set) Token: 0x06001A33 RID: 6707 RVA: 0x000D18D4 File Offset: 0x000CFAD4
	public override bool isEnabled
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			Collider collider = base.collider;
			if (collider && collider.enabled)
			{
				return true;
			}
			Collider2D component = base.GetComponent<Collider2D>();
			return component && component.enabled;
		}
		set
		{
			if (this.isEnabled != value)
			{
				Collider collider = base.collider;
				if (collider != null)
				{
					collider.enabled = value;
					this.SetState((!value) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, false);
				}
				else
				{
					Collider2D component = base.GetComponent<Collider2D>();
					if (component != null)
					{
						component.enabled = value;
						this.SetState((!value) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, false);
					}
					else
					{
						base.enabled = value;
					}
				}
			}
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06001A34 RID: 6708 RVA: 0x000111A1 File Offset: 0x0000F3A1
	// (set) Token: 0x06001A35 RID: 6709 RVA: 0x000D195C File Offset: 0x000CFB5C
	public string normalSprite
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite;
		}
		set
		{
			if (this.mSprite != null && !string.IsNullOrEmpty(this.mNormalSprite) && this.mNormalSprite == this.mSprite.spriteName)
			{
				this.mNormalSprite = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
			}
			else
			{
				this.mNormalSprite = value;
				if (this.mState == UIButtonColor.State.Normal)
				{
					this.SetSprite(value);
				}
			}
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06001A36 RID: 6710 RVA: 0x000111BA File Offset: 0x0000F3BA
	// (set) Token: 0x06001A37 RID: 6711 RVA: 0x000D19DC File Offset: 0x000CFBDC
	public Sprite normalSprite2D
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite2D;
		}
		set
		{
			if (this.mSprite2D != null && this.mNormalSprite2D == this.mSprite2D.sprite2D)
			{
				this.mNormalSprite2D = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
			}
			else
			{
				this.mNormalSprite2D = value;
				if (this.mState == UIButtonColor.State.Normal)
				{
					this.SetSprite(value);
				}
			}
		}
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x000D1A4C File Offset: 0x000CFC4C
	protected override void OnInit()
	{
		base.OnInit();
		this.mSprite = (this.mWidget as UISprite);
		this.mSprite2D = (this.mWidget as UI2DSprite);
		if (this.mSprite != null)
		{
			this.mNormalSprite = this.mSprite.spriteName;
		}
		if (this.mSprite2D != null)
		{
			this.mNormalSprite2D = this.mSprite2D.sprite2D;
		}
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x000D1AC8 File Offset: 0x000CFCC8
	protected override void OnEnable()
	{
		if (this.isEnabled)
		{
			if (this.mInitDone)
			{
				if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
				{
					this.OnHover(UICamera.selectedObject == base.gameObject);
				}
				else if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
				{
					this.OnHover(UICamera.hoveredObject == base.gameObject);
				}
				else
				{
					this.SetState(UIButtonColor.State.Normal, false);
				}
			}
		}
		else
		{
			this.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	// Token: 0x06001A3A RID: 6714 RVA: 0x000111D3 File Offset: 0x0000F3D3
	protected override void OnDragOver()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOver();
		}
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x0001120B File Offset: 0x0000F40B
	protected override void OnDragOut()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.gameObject))
		{
			base.OnDragOut();
		}
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x00011243 File Offset: 0x0000F443
	public virtual void OnClick()
	{
		if (UIButton.current == null && this.isEnabled)
		{
			UIButton.current = this;
			EventDelegate.Execute(this.onClick);
			UIButton.current = null;
		}
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x000D1B4C File Offset: 0x000CFD4C
	public override void SetState(UIButtonColor.State state, bool immediate)
	{
		base.SetState(state, immediate);
		if (this.mSprite != null)
		{
			switch (state)
			{
			case UIButtonColor.State.Normal:
				this.SetSprite(this.mNormalSprite);
				break;
			case UIButtonColor.State.Hover:
				this.SetSprite((!string.IsNullOrEmpty(this.hoverSprite)) ? this.hoverSprite : this.mNormalSprite);
				break;
			case UIButtonColor.State.Pressed:
				this.SetSprite(this.pressedSprite);
				break;
			case UIButtonColor.State.Disabled:
				this.SetSprite(this.disabledSprite);
				break;
			}
		}
		else if (this.mSprite2D != null)
		{
			switch (state)
			{
			case UIButtonColor.State.Normal:
				this.SetSprite(this.mNormalSprite2D);
				break;
			case UIButtonColor.State.Hover:
				this.SetSprite((!(this.hoverSprite2D == null)) ? this.hoverSprite2D : this.mNormalSprite2D);
				break;
			case UIButtonColor.State.Pressed:
				this.SetSprite(this.pressedSprite2D);
				break;
			case UIButtonColor.State.Disabled:
				this.SetSprite(this.disabledSprite2D);
				break;
			}
		}
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x000D1C84 File Offset: 0x000CFE84
	protected void SetSprite(string sp)
	{
		if (this.mSprite != null && !string.IsNullOrEmpty(sp) && this.mSprite.spriteName != sp)
		{
			this.mSprite.spriteName = sp;
			if (this.pixelSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x000D1CE8 File Offset: 0x000CFEE8
	protected void SetSprite(Sprite sp)
	{
		if (sp != null && this.mSprite2D != null && this.mSprite2D.sprite2D != sp)
		{
			this.mSprite2D.sprite2D = sp;
			if (this.pixelSnap)
			{
				this.mSprite2D.MakePixelPerfect();
			}
		}
	}

	// Token: 0x04001F01 RID: 7937
	public static UIButton current;

	// Token: 0x04001F02 RID: 7938
	public bool dragHighlight;

	// Token: 0x04001F03 RID: 7939
	public string hoverSprite;

	// Token: 0x04001F04 RID: 7940
	public string pressedSprite;

	// Token: 0x04001F05 RID: 7941
	public string disabledSprite;

	// Token: 0x04001F06 RID: 7942
	public Sprite hoverSprite2D;

	// Token: 0x04001F07 RID: 7943
	public Sprite pressedSprite2D;

	// Token: 0x04001F08 RID: 7944
	public Sprite disabledSprite2D;

	// Token: 0x04001F09 RID: 7945
	public bool pixelSnap;

	// Token: 0x04001F0A RID: 7946
	public List<EventDelegate> onClick = new List<EventDelegate>();

	// Token: 0x04001F0B RID: 7947
	[NonSerialized]
	private UISprite mSprite;

	// Token: 0x04001F0C RID: 7948
	[NonSerialized]
	private UI2DSprite mSprite2D;

	// Token: 0x04001F0D RID: 7949
	[NonSerialized]
	private string mNormalSprite;

	// Token: 0x04001F0E RID: 7950
	[NonSerialized]
	public Sprite mNormalSprite2D;
}
