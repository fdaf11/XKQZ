using System;
using UnityEngine;

// Token: 0x0200044D RID: 1101
[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour
{
	// Token: 0x06001A71 RID: 6769 RVA: 0x000D279C File Offset: 0x000D099C
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mScale = this.tweenTarget.localScale;
		}
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x000115A7 File Offset: 0x0000F7A7
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x000D27EC File Offset: 0x000D09EC
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenScale component = this.tweenTarget.GetComponent<TweenScale>();
			if (component != null)
			{
				component.value = this.mScale;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x000D2840 File Offset: 0x000D0A40
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenScale.Begin(this.tweenTarget.gameObject, this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.gameObject)) ? this.mScale : Vector3.Scale(this.mScale, this.hover)) : Vector3.Scale(this.mScale, this.pressed)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x000D28D0 File Offset: 0x000D0AD0
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenScale.Begin(this.tweenTarget.gameObject, this.duration, (!isOver) ? this.mScale : Vector3.Scale(this.mScale, this.hover)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x000115C5 File Offset: 0x0000F7C5
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x04001F3D RID: 7997
	public Transform tweenTarget;

	// Token: 0x04001F3E RID: 7998
	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);

	// Token: 0x04001F3F RID: 7999
	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);

	// Token: 0x04001F40 RID: 8000
	public float duration = 0.2f;

	// Token: 0x04001F41 RID: 8001
	private Vector3 mScale;

	// Token: 0x04001F42 RID: 8002
	private bool mStarted;
}
