﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000437 RID: 1079
[AddComponentMenu("NGUI/Examples/Play Idle Animations")]
public class PlayIdleAnimations : MonoBehaviour
{
	// Token: 0x06001A05 RID: 6661 RVA: 0x000D0594 File Offset: 0x000CE794
	private void Start()
	{
		this.mAnim = base.GetComponentInChildren<Animation>();
		if (this.mAnim == null)
		{
			Debug.LogWarning(NGUITools.GetHierarchy(base.gameObject) + " has no Animation component");
			Object.Destroy(this);
		}
		else
		{
			foreach (object obj in this.mAnim)
			{
				AnimationState animationState = (AnimationState)obj;
				if (animationState.clip.name == "idle")
				{
					animationState.layer = 0;
					this.mIdle = animationState.clip;
					this.mAnim.Play(this.mIdle.name);
				}
				else if (animationState.clip.name.StartsWith("idle"))
				{
					animationState.layer = 1;
					this.mBreaks.Add(animationState.clip);
				}
			}
			if (this.mBreaks.Count == 0)
			{
				Object.Destroy(this);
			}
		}
	}

	// Token: 0x06001A06 RID: 6662 RVA: 0x000D06C4 File Offset: 0x000CE8C4
	private void Update()
	{
		if (this.mNextBreak < Time.time)
		{
			if (this.mBreaks.Count == 1)
			{
				AnimationClip animationClip = this.mBreaks[0];
				this.mNextBreak = Time.time + animationClip.length + Random.Range(5f, 15f);
				this.mAnim.CrossFade(animationClip.name);
			}
			else
			{
				int num = Random.Range(0, this.mBreaks.Count - 1);
				if (this.mLastIndex == num)
				{
					num++;
					if (num >= this.mBreaks.Count)
					{
						num = 0;
					}
				}
				this.mLastIndex = num;
				AnimationClip animationClip2 = this.mBreaks[num];
				this.mNextBreak = Time.time + animationClip2.length + Random.Range(2f, 8f);
				this.mAnim.CrossFade(animationClip2.name);
			}
		}
	}

	// Token: 0x04001ECB RID: 7883
	private Animation mAnim;

	// Token: 0x04001ECC RID: 7884
	private AnimationClip mIdle;

	// Token: 0x04001ECD RID: 7885
	private List<AnimationClip> mBreaks = new List<AnimationClip>();

	// Token: 0x04001ECE RID: 7886
	private float mNextBreak;

	// Token: 0x04001ECF RID: 7887
	private int mLastIndex;
}
