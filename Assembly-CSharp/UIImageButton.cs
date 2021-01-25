using System;
using UnityEngine;

// Token: 0x02000466 RID: 1126
[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06001B0D RID: 6925 RVA: 0x000D51A8 File Offset: 0x000D33A8
	// (set) Token: 0x06001B0E RID: 6926 RVA: 0x000D51D0 File Offset: 0x000D33D0
	public bool isEnabled
	{
		get
		{
			Collider collider = base.collider;
			return collider && collider.enabled;
		}
		set
		{
			Collider collider = base.collider;
			if (!collider)
			{
				return;
			}
			if (collider.enabled != value)
			{
				collider.enabled = value;
				this.UpdateImage();
			}
		}
	}

	// Token: 0x06001B0F RID: 6927 RVA: 0x00011F4F File Offset: 0x0001014F
	private void OnEnable()
	{
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<UISprite>();
		}
		this.UpdateImage();
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x000D520C File Offset: 0x000D340C
	private void OnValidate()
	{
		if (this.target != null)
		{
			if (string.IsNullOrEmpty(this.normalSprite))
			{
				this.normalSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.hoverSprite))
			{
				this.hoverSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.pressedSprite))
			{
				this.pressedSprite = this.target.spriteName;
			}
			if (string.IsNullOrEmpty(this.disabledSprite))
			{
				this.disabledSprite = this.target.spriteName;
			}
		}
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x000D52B0 File Offset: 0x000D34B0
	private void UpdateImage()
	{
		if (this.target != null)
		{
			if (this.isEnabled)
			{
				this.SetSprite((!UICamera.IsHighlighted(base.gameObject)) ? this.normalSprite : this.hoverSprite);
			}
			else
			{
				this.SetSprite(this.disabledSprite);
			}
		}
	}

	// Token: 0x06001B12 RID: 6930 RVA: 0x00011F74 File Offset: 0x00010174
	private void OnHover(bool isOver)
	{
		if (this.isEnabled && this.target != null)
		{
			this.SetSprite((!isOver) ? this.normalSprite : this.hoverSprite);
		}
	}

	// Token: 0x06001B13 RID: 6931 RVA: 0x00011FAF File Offset: 0x000101AF
	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			this.SetSprite(this.pressedSprite);
		}
		else
		{
			this.UpdateImage();
		}
	}

	// Token: 0x06001B14 RID: 6932 RVA: 0x000D5314 File Offset: 0x000D3514
	private void SetSprite(string sprite)
	{
		if (this.target.atlas == null || this.target.atlas.GetSprite(sprite) == null)
		{
			return;
		}
		this.target.spriteName = sprite;
		if (this.pixelSnap)
		{
			this.target.MakePixelPerfect();
		}
	}

	// Token: 0x04001FE3 RID: 8163
	public UISprite target;

	// Token: 0x04001FE4 RID: 8164
	public string normalSprite;

	// Token: 0x04001FE5 RID: 8165
	public string hoverSprite;

	// Token: 0x04001FE6 RID: 8166
	public string pressedSprite;

	// Token: 0x04001FE7 RID: 8167
	public string disabledSprite;

	// Token: 0x04001FE8 RID: 8168
	public bool pixelSnap = true;
}
