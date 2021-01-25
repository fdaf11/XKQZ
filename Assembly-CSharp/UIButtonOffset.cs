using System;
using UnityEngine;

// Token: 0x0200044B RID: 1099
[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	// Token: 0x06001A63 RID: 6755 RVA: 0x000D2404 File Offset: 0x000D0604
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mPos = this.tweenTarget.localPosition;
		}
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x000114F8 File Offset: 0x0000F6F8
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x000D2454 File Offset: 0x000D0654
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenPosition component = this.tweenTarget.GetComponent<TweenPosition>();
			if (component != null)
			{
				component.value = this.mPos;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x000D24A8 File Offset: 0x000D06A8
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.gameObject)) ? this.mPos : (this.mPos + this.hover)) : (this.mPos + this.pressed)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x000D2538 File Offset: 0x000D0738
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.gameObject, this.duration, (!isOver) ? this.mPos : (this.mPos + this.hover)).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x00011516 File Offset: 0x0000F716
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x04001F31 RID: 7985
	public Transform tweenTarget;

	// Token: 0x04001F32 RID: 7986
	public Vector3 hover = Vector3.zero;

	// Token: 0x04001F33 RID: 7987
	public Vector3 pressed = new Vector3(2f, -2f);

	// Token: 0x04001F34 RID: 7988
	public float duration = 0.2f;

	// Token: 0x04001F35 RID: 7989
	private Vector3 mPos;

	// Token: 0x04001F36 RID: 7990
	private bool mStarted;
}
