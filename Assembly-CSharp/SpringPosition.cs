using System;
using UnityEngine;

// Token: 0x020004C9 RID: 1225
[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : MonoBehaviour
{
	// Token: 0x06001E97 RID: 7831 RVA: 0x0001446D File Offset: 0x0001266D
	private void Start()
	{
		this.mTrans = base.transform;
		if (this.updateScrollView)
		{
			this.mSv = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		}
	}

	// Token: 0x06001E98 RID: 7832 RVA: 0x000EC2C0 File Offset: 0x000EA4C0
	private void Update()
	{
		float deltaTime = (!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime;
		if (this.worldSpace)
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.position).sqrMagnitude * 0.001f;
			}
			this.mTrans.position = NGUIMath.SpringLerp(this.mTrans.position, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.position).sqrMagnitude)
			{
				this.mTrans.position = this.target;
				this.NotifyListeners();
				base.enabled = false;
			}
		}
		else
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.localPosition).sqrMagnitude * 1E-05f;
			}
			this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.localPosition).sqrMagnitude)
			{
				this.mTrans.localPosition = this.target;
				this.NotifyListeners();
				base.enabled = false;
			}
		}
		if (this.mSv != null)
		{
			this.mSv.UpdateScrollbars(true);
		}
	}

	// Token: 0x06001E99 RID: 7833 RVA: 0x000EC468 File Offset: 0x000EA668
	private void NotifyListeners()
	{
		SpringPosition.current = this;
		if (this.onFinished != null)
		{
			this.onFinished();
		}
		if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
		{
			this.eventReceiver.SendMessage(this.callWhenFinished, this, 1);
		}
		SpringPosition.current = null;
	}

	// Token: 0x06001E9A RID: 7834 RVA: 0x000EC4CC File Offset: 0x000EA6CC
	public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPosition springPosition = go.GetComponent<SpringPosition>();
		if (springPosition == null)
		{
			springPosition = go.AddComponent<SpringPosition>();
		}
		springPosition.target = pos;
		springPosition.strength = strength;
		springPosition.onFinished = null;
		if (!springPosition.enabled)
		{
			springPosition.mThreshold = 0f;
			springPosition.enabled = true;
		}
		return springPosition;
	}

	// Token: 0x0400226B RID: 8811
	public static SpringPosition current;

	// Token: 0x0400226C RID: 8812
	public Vector3 target = Vector3.zero;

	// Token: 0x0400226D RID: 8813
	public float strength = 10f;

	// Token: 0x0400226E RID: 8814
	public bool worldSpace;

	// Token: 0x0400226F RID: 8815
	public bool ignoreTimeScale;

	// Token: 0x04002270 RID: 8816
	public bool updateScrollView;

	// Token: 0x04002271 RID: 8817
	public SpringPosition.OnFinished onFinished;

	// Token: 0x04002272 RID: 8818
	[SerializeField]
	[HideInInspector]
	private GameObject eventReceiver;

	// Token: 0x04002273 RID: 8819
	[HideInInspector]
	[SerializeField]
	public string callWhenFinished;

	// Token: 0x04002274 RID: 8820
	private Transform mTrans;

	// Token: 0x04002275 RID: 8821
	private float mThreshold;

	// Token: 0x04002276 RID: 8822
	private UIScrollView mSv;

	// Token: 0x020004CA RID: 1226
	// (Invoke) Token: 0x06001E9C RID: 7836
	public delegate void OnFinished();
}
