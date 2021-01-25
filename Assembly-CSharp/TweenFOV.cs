using System;
using UnityEngine;

// Token: 0x020004CD RID: 1229
[AddComponentMenu("NGUI/Tween/Tween Field of View")]
[RequireComponent(typeof(Camera))]
public class TweenFOV : UITweener
{
	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x0001459B File Offset: 0x0001279B
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

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000145C0 File Offset: 0x000127C0
	// (set) Token: 0x06001EB8 RID: 7864 RVA: 0x000145C8 File Offset: 0x000127C8
	[Obsolete("Use 'value' instead")]
	public float fov
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

	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06001EB9 RID: 7865 RVA: 0x000145D1 File Offset: 0x000127D1
	// (set) Token: 0x06001EBA RID: 7866 RVA: 0x000145DE File Offset: 0x000127DE
	public float value
	{
		get
		{
			return this.cachedCamera.fieldOfView;
		}
		set
		{
			this.cachedCamera.fieldOfView = value;
		}
	}

	// Token: 0x06001EBB RID: 7867 RVA: 0x000145EC File Offset: 0x000127EC
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06001EBC RID: 7868 RVA: 0x000EC97C File Offset: 0x000EAB7C
	public static TweenFOV Begin(GameObject go, float duration, float to)
	{
		TweenFOV tweenFOV = UITweener.Begin<TweenFOV>(go, duration);
		tweenFOV.from = tweenFOV.value;
		tweenFOV.to = to;
		if (duration <= 0f)
		{
			tweenFOV.Sample(1f, true);
			tweenFOV.enabled = false;
		}
		return tweenFOV;
	}

	// Token: 0x06001EBD RID: 7869 RVA: 0x0001460B File Offset: 0x0001280B
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06001EBE RID: 7870 RVA: 0x00014619 File Offset: 0x00012819
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06001EBF RID: 7871 RVA: 0x00014627 File Offset: 0x00012827
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x00014635 File Offset: 0x00012835
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04002284 RID: 8836
	public float from = 45f;

	// Token: 0x04002285 RID: 8837
	public float to = 45f;

	// Token: 0x04002286 RID: 8838
	private Camera mCam;
}
