using System;
using UnityEngine;

// Token: 0x0200045A RID: 1114
[AddComponentMenu("NGUI/Interaction/Drag Scroll View")]
public class UIDragScrollView : MonoBehaviour
{
	// Token: 0x06001AB8 RID: 6840 RVA: 0x000D3EBC File Offset: 0x000D20BC
	private void OnEnable()
	{
		this.mTrans = base.transform;
		if (this.scrollView == null && this.draggablePanel != null)
		{
			this.scrollView = this.draggablePanel;
			this.draggablePanel = null;
		}
		if (this.mStarted && (this.mAutoFind || this.mScroll == null))
		{
			this.FindScrollView();
		}
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x0001199E File Offset: 0x0000FB9E
	private void Start()
	{
		this.mStarted = true;
		this.FindScrollView();
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x000D3F38 File Offset: 0x000D2138
	private void FindScrollView()
	{
		UIScrollView uiscrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
		if (this.scrollView == null)
		{
			this.scrollView = uiscrollView;
			this.mAutoFind = true;
		}
		else if (this.scrollView == uiscrollView)
		{
			this.mAutoFind = true;
		}
		this.mScroll = this.scrollView;
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x000D3F9C File Offset: 0x000D219C
	private void OnPress(bool pressed)
	{
		if (this.mAutoFind && this.mScroll != this.scrollView)
		{
			this.mScroll = this.scrollView;
			this.mAutoFind = false;
		}
		if (this.scrollView && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.scrollView.Press(pressed);
			if (!pressed && this.mAutoFind)
			{
				this.scrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
				this.mScroll = this.scrollView;
			}
		}
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x000119AD File Offset: 0x0000FBAD
	private void OnDrag(Vector2 delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Drag();
		}
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x000119D5 File Offset: 0x0000FBD5
	private void OnScroll(float delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Scroll(delta);
		}
	}

	// Token: 0x04001F86 RID: 8070
	public UIScrollView scrollView;

	// Token: 0x04001F87 RID: 8071
	[SerializeField]
	[HideInInspector]
	private UIScrollView draggablePanel;

	// Token: 0x04001F88 RID: 8072
	private Transform mTrans;

	// Token: 0x04001F89 RID: 8073
	private UIScrollView mScroll;

	// Token: 0x04001F8A RID: 8074
	private bool mAutoFind;

	// Token: 0x04001F8B RID: 8075
	private bool mStarted;
}
