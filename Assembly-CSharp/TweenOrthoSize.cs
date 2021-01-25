using System;
using UnityEngine;

// Token: 0x020004CF RID: 1231
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/Tween/Tween Orthographic Size")]
public class TweenOrthoSize : UITweener
{
	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06001ECE RID: 7886 RVA: 0x00014702 File Offset: 0x00012902
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.camera;
			}
			return this.mCam;
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06001ECF RID: 7887 RVA: 0x00014727 File Offset: 0x00012927
	// (set) Token: 0x06001ED0 RID: 7888 RVA: 0x0001472F File Offset: 0x0001292F
	[Obsolete("Use 'value' instead")]
	public float orthoSize
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x00014738 File Offset: 0x00012938
	// (set) Token: 0x06001ED2 RID: 7890 RVA: 0x00014745 File Offset: 0x00012945
	public float value
	{
		get
		{
			return this.cachedCamera.orthographicSize;
		}
		set
		{
			this.cachedCamera.orthographicSize = value;
		}
	}

	// Token: 0x06001ED3 RID: 7891 RVA: 0x00014753 File Offset: 0x00012953
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06001ED4 RID: 7892 RVA: 0x000ECA94 File Offset: 0x000EAC94
	public static TweenOrthoSize Begin(GameObject go, float duration, float to)
	{
		TweenOrthoSize tweenOrthoSize = UITweener.Begin<TweenOrthoSize>(go, duration);
		tweenOrthoSize.from = tweenOrthoSize.value;
		tweenOrthoSize.to = to;
		if (duration <= 0f)
		{
			tweenOrthoSize.Sample(1f, true);
			tweenOrthoSize.enabled = false;
		}
		return tweenOrthoSize;
	}

	// Token: 0x06001ED5 RID: 7893 RVA: 0x00014772 File Offset: 0x00012972
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001ED6 RID: 7894 RVA: 0x00014780 File Offset: 0x00012980
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x0400228C RID: 8844
	public float from = 1f;

	// Token: 0x0400228D RID: 8845
	public float to = 1f;

	// Token: 0x0400228E RID: 8846
	private Camera mCam;
}
