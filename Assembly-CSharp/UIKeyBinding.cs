using System;
using UnityEngine;

// Token: 0x02000467 RID: 1127
[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
	// Token: 0x06001B16 RID: 6934 RVA: 0x000D5370 File Offset: 0x000D3570
	private void Start()
	{
		UIInput component = base.GetComponent<UIInput>();
		this.mIsInput = (component != null);
		if (component != null)
		{
			EventDelegate.Add(component.onSubmit, new EventDelegate.Callback(this.OnSubmit));
		}
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x00011FCE File Offset: 0x000101CE
	private void OnSubmit()
	{
		if (UICamera.currentKey == this.keyCode && this.IsModifierActive())
		{
			this.mIgnoreUp = true;
		}
	}

	// Token: 0x06001B18 RID: 6936 RVA: 0x000D53B8 File Offset: 0x000D35B8
	private bool IsModifierActive()
	{
		if (this.modifier == UIKeyBinding.Modifier.None)
		{
			return true;
		}
		if (this.modifier == UIKeyBinding.Modifier.Alt)
		{
			if (Input.GetKey(308) || Input.GetKey(307))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Control)
		{
			if (Input.GetKey(306) || Input.GetKey(305))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Shift && (Input.GetKey(304) || Input.GetKey(303)))
		{
			return true;
		}
		return false;
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x000D5464 File Offset: 0x000D3664
	private void Update()
	{
		if (this.keyCode == null || !this.IsModifierActive())
		{
			return;
		}
		if (this.action == UIKeyBinding.Action.PressAndClick || this.action == UIKeyBinding.Action.All)
		{
			if (UICamera.inputHasFocus)
			{
				return;
			}
			UICamera.currentTouch = UICamera.controller;
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			UICamera.currentTouch.current = base.gameObject;
			if (Input.GetKeyDown(this.keyCode))
			{
				this.mPress = true;
				UICamera.Notify(base.gameObject, "OnPress", true);
			}
			if (Input.GetKeyUp(this.keyCode))
			{
				UICamera.Notify(base.gameObject, "OnPress", false);
				if (this.mPress)
				{
					UICamera.Notify(base.gameObject, "OnClick", null);
					this.mPress = false;
				}
			}
			UICamera.currentTouch.current = null;
		}
		if ((this.action == UIKeyBinding.Action.Select || this.action == UIKeyBinding.Action.All) && Input.GetKeyUp(this.keyCode))
		{
			if (this.mIsInput)
			{
				if (!this.mIgnoreUp && !UICamera.inputHasFocus)
				{
					UICamera.selectedObject = base.gameObject;
				}
				this.mIgnoreUp = false;
			}
			else
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	// Token: 0x04001FE9 RID: 8169
	public KeyCode keyCode;

	// Token: 0x04001FEA RID: 8170
	public UIKeyBinding.Modifier modifier;

	// Token: 0x04001FEB RID: 8171
	public UIKeyBinding.Action action;

	// Token: 0x04001FEC RID: 8172
	private bool mIgnoreUp;

	// Token: 0x04001FED RID: 8173
	private bool mIsInput;

	// Token: 0x04001FEE RID: 8174
	private bool mPress;

	// Token: 0x02000468 RID: 1128
	public enum Action
	{
		// Token: 0x04001FF0 RID: 8176
		PressAndClick,
		// Token: 0x04001FF1 RID: 8177
		Select,
		// Token: 0x04001FF2 RID: 8178
		All
	}

	// Token: 0x02000469 RID: 1129
	public enum Modifier
	{
		// Token: 0x04001FF4 RID: 8180
		None,
		// Token: 0x04001FF5 RID: 8181
		Shift,
		// Token: 0x04001FF6 RID: 8182
		Control,
		// Token: 0x04001FF7 RID: 8183
		Alt
	}
}
