using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
public abstract class UITweener : MonoBehaviour
{
	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06001F18 RID: 7960 RVA: 0x000ED1FC File Offset: 0x000EB3FC
	public float amountPerDelta
	{
		get
		{
			if (this.mDuration != this.duration)
			{
				this.mDuration = this.duration;
				this.mAmountPerDelta = Mathf.Abs((this.duration <= 0f) ? 1000f : (1f / this.duration)) * Mathf.Sign(this.mAmountPerDelta);
			}
			return this.mAmountPerDelta;
		}
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06001F19 RID: 7961 RVA: 0x00014AFD File Offset: 0x00012CFD
	// (set) Token: 0x06001F1A RID: 7962 RVA: 0x00014B05 File Offset: 0x00012D05
	public float tweenFactor
	{
		get
		{
			return this.mFactor;
		}
		set
		{
			this.mFactor = Mathf.Clamp01(value);
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06001F1B RID: 7963 RVA: 0x00014B13 File Offset: 0x00012D13
	public Direction direction
	{
		get
		{
			return (this.amountPerDelta >= 0f) ? Direction.Forward : Direction.Reverse;
		}
	}

	// Token: 0x06001F1C RID: 7964 RVA: 0x00014B2C File Offset: 0x00012D2C
	private void Reset()
	{
		if (!this.mStarted)
		{
			this.SetStartToCurrentValue();
			this.SetEndToCurrentValue();
		}
	}

	// Token: 0x06001F1D RID: 7965 RVA: 0x00014B45 File Offset: 0x00012D45
	protected virtual void Start()
	{
		this.Update();
	}

	// Token: 0x06001F1E RID: 7966 RVA: 0x000ED26C File Offset: 0x000EB46C
	private void Update()
	{
		float num = (!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime;
		float num2 = (!this.ignoreTimeScale) ? Time.time : RealTime.time;
		if (!this.mStarted)
		{
			this.mStarted = true;
			this.mStartTime = num2 + this.delay;
		}
		if (num2 < this.mStartTime)
		{
			return;
		}
		this.mFactor += this.amountPerDelta * num;
		if (this.style == UITweener.Style.Loop)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor -= Mathf.Floor(this.mFactor);
			}
		}
		else if (this.style == UITweener.Style.PingPong)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
			else if (this.mFactor < 0f)
			{
				this.mFactor = -this.mFactor;
				this.mFactor -= Mathf.Floor(this.mFactor);
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
		}
		if (this.style == UITweener.Style.Once && (this.duration == 0f || this.mFactor > 1f || this.mFactor < 0f))
		{
			this.mFactor = Mathf.Clamp01(this.mFactor);
			this.Sample(this.mFactor, true);
			if (this.duration == 0f || (this.mFactor == 1f && this.mAmountPerDelta > 0f) || (this.mFactor == 0f && this.mAmountPerDelta < 0f))
			{
				base.enabled = false;
			}
			if (UITweener.current == null)
			{
				UITweener.current = this;
				if (this.onFinished != null)
				{
					this.mTemp = this.onFinished;
					this.onFinished = new List<EventDelegate>();
					EventDelegate.Execute(this.mTemp);
					for (int i = 0; i < this.mTemp.Count; i++)
					{
						EventDelegate eventDelegate = this.mTemp[i];
						if (eventDelegate != null && !eventDelegate.oneShot)
						{
							EventDelegate.Add(this.onFinished, eventDelegate, eventDelegate.oneShot);
						}
					}
					this.mTemp = null;
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, 1);
				}
				UITweener.current = null;
			}
		}
		else
		{
			this.Sample(this.mFactor, false);
		}
	}

	// Token: 0x06001F1F RID: 7967 RVA: 0x00014B4D File Offset: 0x00012D4D
	public void SetOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	// Token: 0x06001F20 RID: 7968 RVA: 0x00014B5C File Offset: 0x00012D5C
	public void SetOnFinished(EventDelegate del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	// Token: 0x06001F21 RID: 7969 RVA: 0x00014B6A File Offset: 0x00012D6A
	public void AddOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	// Token: 0x06001F22 RID: 7970 RVA: 0x00014B79 File Offset: 0x00012D79
	public void AddOnFinished(EventDelegate del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	// Token: 0x06001F23 RID: 7971 RVA: 0x00014B87 File Offset: 0x00012D87
	public void RemoveOnFinished(EventDelegate del)
	{
		if (this.onFinished != null)
		{
			this.onFinished.Remove(del);
		}
		if (this.mTemp != null)
		{
			this.mTemp.Remove(del);
		}
	}

	// Token: 0x06001F24 RID: 7972 RVA: 0x00014BB9 File Offset: 0x00012DB9
	private void OnDisable()
	{
		this.mStarted = false;
	}

	// Token: 0x06001F25 RID: 7973 RVA: 0x000ED54C File Offset: 0x000EB74C
	public void Sample(float factor, bool isFinished)
	{
		float num = Mathf.Clamp01(factor);
		if (this.method == UITweener.Method.EaseIn)
		{
			num = 1f - Mathf.Sin(1.5707964f * (1f - num));
			if (this.steeperCurves)
			{
				num *= num;
			}
		}
		else if (this.method == UITweener.Method.EaseOut)
		{
			num = Mathf.Sin(1.5707964f * num);
			if (this.steeperCurves)
			{
				num = 1f - num;
				num = 1f - num * num;
			}
		}
		else if (this.method == UITweener.Method.EaseInOut)
		{
			num -= Mathf.Sin(num * 6.2831855f) / 6.2831855f;
			if (this.steeperCurves)
			{
				num = num * 2f - 1f;
				float num2 = Mathf.Sign(num);
				num = 1f - Mathf.Abs(num);
				num = 1f - num * num;
				num = num2 * num * 0.5f + 0.5f;
			}
		}
		else if (this.method == UITweener.Method.BounceIn)
		{
			num = this.BounceLogic(num);
		}
		else if (this.method == UITweener.Method.BounceOut)
		{
			num = 1f - this.BounceLogic(1f - num);
		}
		this.OnUpdate((this.animationCurve == null) ? num : this.animationCurve.Evaluate(num), isFinished);
	}

	// Token: 0x06001F26 RID: 7974 RVA: 0x000ED6A0 File Offset: 0x000EB8A0
	private float BounceLogic(float val)
	{
		if (val < 0.363636f)
		{
			val = 7.5685f * val * val;
		}
		else if (val < 0.727272f)
		{
			val = 7.5625f * (val -= 0.545454f) * val + 0.75f;
		}
		else if (val < 0.90909f)
		{
			val = 7.5625f * (val -= 0.818181f) * val + 0.9375f;
		}
		else
		{
			val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f;
		}
		return val;
	}

	// Token: 0x06001F27 RID: 7975 RVA: 0x00014BC2 File Offset: 0x00012DC2
	[Obsolete("Use PlayForward() instead")]
	public void Play()
	{
		this.Play(true);
	}

	// Token: 0x06001F28 RID: 7976 RVA: 0x00014BC2 File Offset: 0x00012DC2
	public void PlayForward()
	{
		this.Play(true);
	}

	// Token: 0x06001F29 RID: 7977 RVA: 0x00014BCB File Offset: 0x00012DCB
	public void PlayReverse()
	{
		this.Play(false);
	}

	// Token: 0x06001F2A RID: 7978 RVA: 0x00014BD4 File Offset: 0x00012DD4
	public void Play(bool forward)
	{
		this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		if (!forward)
		{
			this.mAmountPerDelta = -this.mAmountPerDelta;
		}
		base.enabled = true;
		this.Update();
	}

	// Token: 0x06001F2B RID: 7979 RVA: 0x00014C07 File Offset: 0x00012E07
	public void ResetToBeginning()
	{
		this.mStarted = false;
		this.mFactor = ((this.amountPerDelta >= 0f) ? 0f : 1f);
		this.Sample(this.mFactor, false);
	}

	// Token: 0x06001F2C RID: 7980 RVA: 0x00014C42 File Offset: 0x00012E42
	public void Toggle()
	{
		if (this.mFactor > 0f)
		{
			this.mAmountPerDelta = -this.amountPerDelta;
		}
		else
		{
			this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		}
		base.enabled = true;
	}

	// Token: 0x06001F2D RID: 7981
	protected abstract void OnUpdate(float factor, bool isFinished);

	// Token: 0x06001F2E RID: 7982 RVA: 0x000ED738 File Offset: 0x000EB938
	public static T Begin<T>(GameObject go, float duration) where T : UITweener
	{
		T t = go.GetComponent<T>();
		if (t != null && t.tweenGroup != 0)
		{
			t = (T)((object)null);
			T[] components = go.GetComponents<T>();
			int i = 0;
			int num = components.Length;
			while (i < num)
			{
				t = components[i];
				if (t != null && t.tweenGroup == 0)
				{
					break;
				}
				t = (T)((object)null);
				i++;
			}
		}
		if (t == null)
		{
			t = go.AddComponent<T>();
			if (t == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Unable to add ",
					typeof(T),
					" to ",
					NGUITools.GetHierarchy(go)
				}), go);
				return (T)((object)null);
			}
		}
		t.mStarted = false;
		t.duration = duration;
		t.mFactor = 0f;
		t.mAmountPerDelta = Mathf.Abs(t.amountPerDelta);
		t.style = UITweener.Style.Once;
		t.animationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 1f),
			new Keyframe(1f, 1f, 1f, 0f)
		});
		t.eventReceiver = null;
		t.callWhenFinished = null;
		t.enabled = true;
		return t;
	}

	// Token: 0x06001F2F RID: 7983 RVA: 0x0000264F File Offset: 0x0000084F
	public virtual void SetStartToCurrentValue()
	{
	}

	// Token: 0x06001F30 RID: 7984 RVA: 0x0000264F File Offset: 0x0000084F
	public virtual void SetEndToCurrentValue()
	{
	}

	// Token: 0x040022AB RID: 8875
	public static UITweener current;

	// Token: 0x040022AC RID: 8876
	[HideInInspector]
	public UITweener.Method method;

	// Token: 0x040022AD RID: 8877
	[HideInInspector]
	public UITweener.Style style;

	// Token: 0x040022AE RID: 8878
	[HideInInspector]
	public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x040022AF RID: 8879
	[HideInInspector]
	public bool ignoreTimeScale = true;

	// Token: 0x040022B0 RID: 8880
	[HideInInspector]
	public float delay;

	// Token: 0x040022B1 RID: 8881
	[HideInInspector]
	public float duration = 1f;

	// Token: 0x040022B2 RID: 8882
	[HideInInspector]
	public bool steeperCurves;

	// Token: 0x040022B3 RID: 8883
	[HideInInspector]
	public int tweenGroup;

	// Token: 0x040022B4 RID: 8884
	[HideInInspector]
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x040022B5 RID: 8885
	[HideInInspector]
	public GameObject eventReceiver;

	// Token: 0x040022B6 RID: 8886
	[HideInInspector]
	public string callWhenFinished;

	// Token: 0x040022B7 RID: 8887
	private bool mStarted;

	// Token: 0x040022B8 RID: 8888
	private float mStartTime;

	// Token: 0x040022B9 RID: 8889
	private float mDuration;

	// Token: 0x040022BA RID: 8890
	private float mAmountPerDelta = 1000f;

	// Token: 0x040022BB RID: 8891
	private float mFactor;

	// Token: 0x040022BC RID: 8892
	private List<EventDelegate> mTemp;

	// Token: 0x020004D7 RID: 1239
	public enum Method
	{
		// Token: 0x040022BE RID: 8894
		Linear,
		// Token: 0x040022BF RID: 8895
		EaseIn,
		// Token: 0x040022C0 RID: 8896
		EaseOut,
		// Token: 0x040022C1 RID: 8897
		EaseInOut,
		// Token: 0x040022C2 RID: 8898
		BounceIn,
		// Token: 0x040022C3 RID: 8899
		BounceOut
	}

	// Token: 0x020004D8 RID: 1240
	public enum Style
	{
		// Token: 0x040022C5 RID: 8901
		Once,
		// Token: 0x040022C6 RID: 8902
		Loop,
		// Token: 0x040022C7 RID: 8903
		PingPong
	}
}
