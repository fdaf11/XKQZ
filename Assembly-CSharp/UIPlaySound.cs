using System;
using UnityEngine;

// Token: 0x0200046D RID: 1133
[AddComponentMenu("NGUI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour
{
	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06001B3A RID: 6970 RVA: 0x000D5E78 File Offset: 0x000D4078
	private bool canPlay
	{
		get
		{
			if (!base.enabled)
			{
				return false;
			}
			UIButton component = base.GetComponent<UIButton>();
			return component == null || component.isEnabled;
		}
	}

	// Token: 0x06001B3B RID: 6971 RVA: 0x00012256 File Offset: 0x00010456
	private void Start()
	{
		this.volume = GameGlobal.m_fSoundValue;
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x00012263 File Offset: 0x00010463
	private void OnEnable()
	{
		if (this.trigger == UIPlaySound.Trigger.OnEnable)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x00012289 File Offset: 0x00010489
	private void OnDisable()
	{
		if (this.trigger == UIPlaySound.Trigger.OnDisable)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x000D5EB0 File Offset: 0x000D40B0
	private void OnHover(bool isOver)
	{
		if (this.trigger == UIPlaySound.Trigger.OnMouseOver)
		{
			if (this.mIsOver == isOver)
			{
				return;
			}
			this.mIsOver = isOver;
		}
		if (this.canPlay && ((isOver && this.trigger == UIPlaySound.Trigger.OnMouseOver) || (!isOver && this.trigger == UIPlaySound.Trigger.OnMouseOut)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x000D5F24 File Offset: 0x000D4124
	private void OnPress(bool isPressed)
	{
		if (this.trigger == UIPlaySound.Trigger.OnPress)
		{
			if (this.mIsOver == isPressed)
			{
				return;
			}
			this.mIsOver = isPressed;
		}
		if (this.canPlay && ((isPressed && this.trigger == UIPlaySound.Trigger.OnPress) || (!isPressed && this.trigger == UIPlaySound.Trigger.OnRelease)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x000122AF File Offset: 0x000104AF
	private void OnClick()
	{
		if (this.canPlay && this.trigger == UIPlaySound.Trigger.OnClick)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x000122DF File Offset: 0x000104DF
	private void OnSelect(bool isSelected)
	{
		if (this.canPlay && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x06001B42 RID: 6978 RVA: 0x00012304 File Offset: 0x00010504
	public void Play()
	{
		NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
	}

	// Token: 0x04002015 RID: 8213
	public AudioClip audioClip;

	// Token: 0x04002016 RID: 8214
	public UIPlaySound.Trigger trigger;

	// Token: 0x04002017 RID: 8215
	[Range(0f, 1f)]
	public float volume = 1f;

	// Token: 0x04002018 RID: 8216
	[Range(0f, 2f)]
	public float pitch = 1f;

	// Token: 0x04002019 RID: 8217
	private bool mIsOver;

	// Token: 0x0200046E RID: 1134
	public enum Trigger
	{
		// Token: 0x0400201B RID: 8219
		OnClick,
		// Token: 0x0400201C RID: 8220
		OnMouseOver,
		// Token: 0x0400201D RID: 8221
		OnMouseOut,
		// Token: 0x0400201E RID: 8222
		OnPress,
		// Token: 0x0400201F RID: 8223
		OnRelease,
		// Token: 0x04002020 RID: 8224
		Custom,
		// Token: 0x04002021 RID: 8225
		OnEnable,
		// Token: 0x04002022 RID: 8226
		OnDisable,
		// Token: 0x04002023 RID: 8227
		OnKeySelect
	}
}
