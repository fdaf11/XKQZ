using System;
using UnityEngine;

// Token: 0x020004AA RID: 1194
[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Internal/Spring Panel")]
public class SpringPanel : MonoBehaviour
{
	// Token: 0x06001D85 RID: 7557 RVA: 0x000138A2 File Offset: 0x00011AA2
	private void Start()
	{
		this.mPanel = base.GetComponent<UIPanel>();
		this.mDrag = base.GetComponent<UIScrollView>();
		this.mTrans = base.transform;
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x000138C8 File Offset: 0x00011AC8
	private void Update()
	{
		this.AdvanceTowardsPosition();
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x000E5A98 File Offset: 0x000E3C98
	protected virtual void AdvanceTowardsPosition()
	{
		float deltaTime = RealTime.deltaTime;
		bool flag = false;
		Vector3 localPosition = this.mTrans.localPosition;
		Vector3 vector = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
		if ((vector - this.target).sqrMagnitude < 0.01f)
		{
			vector = this.target;
			base.enabled = false;
			flag = true;
		}
		this.mTrans.localPosition = vector;
		Vector3 vector2 = vector - localPosition;
		Vector2 clipOffset = this.mPanel.clipOffset;
		clipOffset.x -= vector2.x;
		clipOffset.y -= vector2.y;
		this.mPanel.clipOffset = clipOffset;
		if (this.mDrag != null)
		{
			this.mDrag.UpdateScrollbars(false);
		}
		if (flag && this.onFinished != null)
		{
			SpringPanel.current = this;
			this.onFinished();
			SpringPanel.current = null;
		}
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x000E5BA4 File Offset: 0x000E3DA4
	public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPanel springPanel = go.GetComponent<SpringPanel>();
		if (springPanel == null)
		{
			springPanel = go.AddComponent<SpringPanel>();
		}
		springPanel.target = pos;
		springPanel.strength = strength;
		springPanel.onFinished = null;
		springPanel.enabled = true;
		return springPanel;
	}

	// Token: 0x040021AA RID: 8618
	public static SpringPanel current;

	// Token: 0x040021AB RID: 8619
	public Vector3 target = Vector3.zero;

	// Token: 0x040021AC RID: 8620
	public float strength = 10f;

	// Token: 0x040021AD RID: 8621
	public SpringPanel.OnFinished onFinished;

	// Token: 0x040021AE RID: 8622
	private UIPanel mPanel;

	// Token: 0x040021AF RID: 8623
	private Transform mTrans;

	// Token: 0x040021B0 RID: 8624
	private UIScrollView mDrag;

	// Token: 0x020004AB RID: 1195
	// (Invoke) Token: 0x06001D8A RID: 7562
	public delegate void OnFinished();
}
