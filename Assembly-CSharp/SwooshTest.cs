using System;
using UnityEngine;

// Token: 0x0200061D RID: 1565
public class SwooshTest : MonoBehaviour
{
	// Token: 0x060026A7 RID: 9895 RVA: 0x0012C4D8 File Offset: 0x0012A6D8
	private void Start()
	{
		float num = this._animation.frameRate * this._animation.length;
		this._startN = (float)this._start / num;
		this._endN = (float)this._end / num;
		this._animationState = base.animation[this._animation.name];
		this._trail.Emit = false;
	}

	// Token: 0x060026A8 RID: 9896 RVA: 0x0012C544 File Offset: 0x0012A744
	private void Update()
	{
		this._time += this._animationState.normalizedTime - this._prevAnimTime;
		if (this._time > 1f || this._firstFrame)
		{
			if (!this._firstFrame)
			{
				this._time -= 1f;
			}
			this._firstFrame = false;
		}
		if (this._prevTime < this._startN && this._time >= this._startN)
		{
			this._trail.Emit = true;
		}
		else if (this._prevTime < this._endN && this._time >= this._endN)
		{
			this._trail.Emit = false;
		}
		this._prevTime = this._time;
		this._prevAnimTime = this._animationState.normalizedTime;
	}

	// Token: 0x04002FBC RID: 12220
	[SerializeField]
	private AnimationClip _animation;

	// Token: 0x04002FBD RID: 12221
	private AnimationState _animationState;

	// Token: 0x04002FBE RID: 12222
	[SerializeField]
	private int _start;

	// Token: 0x04002FBF RID: 12223
	[SerializeField]
	private int _end;

	// Token: 0x04002FC0 RID: 12224
	private float _startN;

	// Token: 0x04002FC1 RID: 12225
	private float _endN;

	// Token: 0x04002FC2 RID: 12226
	private float _time;

	// Token: 0x04002FC3 RID: 12227
	private float _prevTime;

	// Token: 0x04002FC4 RID: 12228
	private float _prevAnimTime;

	// Token: 0x04002FC5 RID: 12229
	[SerializeField]
	private MeleeWeaponTrail _trail;

	// Token: 0x04002FC6 RID: 12230
	private bool _firstFrame = true;
}
