using System;
using UnityEngine;

// Token: 0x0200044C RID: 1100
[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
	// Token: 0x06001A6A RID: 6762 RVA: 0x000D25A0 File Offset: 0x000D07A0
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.transform;
			}
			this.mRot = this.tweenTarget.localRotation;
		}
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x00011564 File Offset: 0x0000F764
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x000D25F0 File Offset: 0x000D07F0
	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenRotation component = this.tweenTarget.GetComponent<TweenRotation>();
			if (component != null)
			{
				component.value = this.mRot;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x000D2644 File Offset: 0x000D0844
	private void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.gameObject)) ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))) : (this.mRot * Quaternion.Euler(this.pressed))).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x000D26DC File Offset: 0x000D08DC
	private void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenRotation.Begin(this.tweenTarget.gameObject, this.duration, (!isOver) ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))).method = UITweener.Method.EaseInOut;
		}
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x00011582 File Offset: 0x0000F782
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x04001F37 RID: 7991
	public Transform tweenTarget;

	// Token: 0x04001F38 RID: 7992
	public Vector3 hover = Vector3.zero;

	// Token: 0x04001F39 RID: 7993
	public Vector3 pressed = Vector3.zero;

	// Token: 0x04001F3A RID: 7994
	public float duration = 0.2f;

	// Token: 0x04001F3B RID: 7995
	private Quaternion mRot;

	// Token: 0x04001F3C RID: 7996
	private bool mStarted;
}
