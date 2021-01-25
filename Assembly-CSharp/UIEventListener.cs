using System;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
	// Token: 0x06001DD4 RID: 7636 RVA: 0x000E965C File Offset: 0x000E785C
	private void CreateAudioData()
	{
		Array values = Enum.GetValues(typeof(UIPlaySound.Trigger));
		this.m_UIAudioData = new UIAudioData[values.Length];
		for (int i = 0; i < this.m_UIAudioData.Length; i++)
		{
			this.m_UIAudioData[i] = new UIAudioData();
		}
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x00013B59 File Offset: 0x00011D59
	private void OnSubmit()
	{
		if (this.onSubmit != null)
		{
			this.onSubmit(base.gameObject);
		}
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x00013B77 File Offset: 0x00011D77
	public void OnClick()
	{
		if (this.onClick != null)
		{
			this.PlayAudio(UIPlaySound.Trigger.OnClick);
			this.onClick(base.gameObject);
		}
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x00013B9C File Offset: 0x00011D9C
	private void OnDoubleClick()
	{
		if (this.onDoubleClick != null)
		{
			this.onDoubleClick(base.gameObject);
		}
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x000E96B0 File Offset: 0x000E78B0
	private void OnHover(bool isOver)
	{
		if (this.onHover != null)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
			if (isOver && !Input.GetMouseButtonUp(0))
			{
				this.PlayAudio(UIPlaySound.Trigger.OnMouseOver);
			}
			this.onHover(base.gameObject, isOver);
		}
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x00013BBA File Offset: 0x00011DBA
	private void OnPress(bool isPressed)
	{
		if (this.onPress != null)
		{
			this.onPress(base.gameObject, isPressed);
		}
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x00013BD9 File Offset: 0x00011DD9
	private void OnSelect(bool selected)
	{
		if (this.onSelect != null)
		{
			this.onSelect(base.gameObject, selected);
		}
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x00013BF8 File Offset: 0x00011DF8
	private void OnScroll(float delta)
	{
		if (this.onScroll != null)
		{
			this.onScroll(base.gameObject, delta);
		}
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x00013C17 File Offset: 0x00011E17
	private void OnDragStart()
	{
		if (this.onDragStart != null)
		{
			this.onDragStart(base.gameObject);
		}
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x00013C35 File Offset: 0x00011E35
	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag != null)
		{
			this.onDrag(base.gameObject, delta);
		}
	}

	// Token: 0x06001DDE RID: 7646 RVA: 0x00013C54 File Offset: 0x00011E54
	private void OnDragOver()
	{
		if (this.onDragOver != null)
		{
			this.onDragOver(base.gameObject);
		}
	}

	// Token: 0x06001DDF RID: 7647 RVA: 0x00013C72 File Offset: 0x00011E72
	private void OnDragOut()
	{
		if (this.onDragOut != null)
		{
			this.onDragOut(base.gameObject);
		}
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x00013C90 File Offset: 0x00011E90
	private void OnDragEnd()
	{
		if (this.onDragEnd != null)
		{
			this.onDragEnd(base.gameObject);
		}
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x00013CAE File Offset: 0x00011EAE
	private void OnDrop(GameObject go)
	{
		if (this.onDrop != null)
		{
			this.onDrop(base.gameObject, go);
		}
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x00013CCD File Offset: 0x00011ECD
	private void OnKey(KeyCode key)
	{
		if (this.onKey != null)
		{
			this.onKey(base.gameObject, key);
		}
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x00013CEC File Offset: 0x00011EEC
	private void OnTooltip(bool show)
	{
		if (this.onTooltip != null)
		{
			this.onTooltip(base.gameObject, show);
		}
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x00013D0B File Offset: 0x00011F0B
	public void OnKeySelect(bool show)
	{
		if (this.onKeySelect != null)
		{
			if (show)
			{
				this.PlayAudio(UIPlaySound.Trigger.OnKeySelect);
			}
			this.onKeySelect(base.gameObject, show);
		}
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x000E9700 File Offset: 0x000E7900
	public void SetAudio(UIPlaySound.Trigger trigger, UIAudioData.eUIAudio type)
	{
		if (this.m_UIAudioData == null)
		{
			this.CreateAudioData();
		}
		if (trigger < UIPlaySound.Trigger.OnClick || trigger >= (UIPlaySound.Trigger)this.m_UIAudioData.Length)
		{
			return;
		}
		UIAudioData uiaudioData = this.m_UIAudioData[(int)trigger];
		uiaudioData.audioType = type;
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x000E9748 File Offset: 0x000E7948
	protected void PlayAudio(UIPlaySound.Trigger trigger)
	{
		if (this.m_UIAudioData == null)
		{
			this.CreateAudioData();
		}
		if (trigger < UIPlaySound.Trigger.OnClick || trigger >= (UIPlaySound.Trigger)this.m_UIAudioData.Length)
		{
			return;
		}
		UIAudioData uiaudioData = this.m_UIAudioData[(int)trigger];
		if (uiaudioData.audioType == UIAudioData.eUIAudio.None)
		{
			return;
		}
		if (uiaudioData.audioClip == null)
		{
			uiaudioData.audioClip = UIAudioDataManager.Singleton.GetUIAudio(uiaudioData.audioType);
		}
		NGUITools.PlaySound(uiaudioData.audioClip, GameGlobal.m_fSoundValue, 1f);
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x000E97D4 File Offset: 0x000E79D4
	public static UIEventListener Get(GameObject go)
	{
		UIEventListener uieventListener = go.GetComponent<UIEventListener>();
		if (uieventListener == null)
		{
			uieventListener = go.AddComponent<UIEventListener>();
		}
		return uieventListener;
	}

	// Token: 0x040021FC RID: 8700
	public object parameter;

	// Token: 0x040021FD RID: 8701
	public UIEventListener.VoidDelegate onSubmit;

	// Token: 0x040021FE RID: 8702
	public UIEventListener.VoidDelegate onClick;

	// Token: 0x040021FF RID: 8703
	public UIEventListener.VoidDelegate onDoubleClick;

	// Token: 0x04002200 RID: 8704
	public UIEventListener.BoolDelegate onHover;

	// Token: 0x04002201 RID: 8705
	public UIEventListener.BoolDelegate onPress;

	// Token: 0x04002202 RID: 8706
	public UIEventListener.BoolDelegate onSelect;

	// Token: 0x04002203 RID: 8707
	public UIEventListener.FloatDelegate onScroll;

	// Token: 0x04002204 RID: 8708
	public UIEventListener.VoidDelegate onDragStart;

	// Token: 0x04002205 RID: 8709
	public UIEventListener.VectorDelegate onDrag;

	// Token: 0x04002206 RID: 8710
	public UIEventListener.VoidDelegate onDragOver;

	// Token: 0x04002207 RID: 8711
	public UIEventListener.VoidDelegate onDragOut;

	// Token: 0x04002208 RID: 8712
	public UIEventListener.VoidDelegate onDragEnd;

	// Token: 0x04002209 RID: 8713
	public UIEventListener.ObjectDelegate onDrop;

	// Token: 0x0400220A RID: 8714
	public UIEventListener.KeyCodeDelegate onKey;

	// Token: 0x0400220B RID: 8715
	public UIEventListener.BoolDelegate onTooltip;

	// Token: 0x0400220C RID: 8716
	public UIEventListener.BoolDelegate onKeySelect;

	// Token: 0x0400220D RID: 8717
	private UIAudioData[] m_UIAudioData;

	// Token: 0x020004B5 RID: 1205
	// (Invoke) Token: 0x06001DE9 RID: 7657
	public delegate void VoidDelegate(GameObject go);

	// Token: 0x020004B6 RID: 1206
	// (Invoke) Token: 0x06001DED RID: 7661
	public delegate void BoolDelegate(GameObject go, bool state);

	// Token: 0x020004B7 RID: 1207
	// (Invoke) Token: 0x06001DF1 RID: 7665
	public delegate void FloatDelegate(GameObject go, float delta);

	// Token: 0x020004B8 RID: 1208
	// (Invoke) Token: 0x06001DF5 RID: 7669
	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	// Token: 0x020004B9 RID: 1209
	// (Invoke) Token: 0x06001DF9 RID: 7673
	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	// Token: 0x020004BA RID: 1210
	// (Invoke) Token: 0x06001DFD RID: 7677
	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);
}
