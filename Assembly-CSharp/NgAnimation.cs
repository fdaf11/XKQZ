using System;
using UnityEngine;

// Token: 0x020003EC RID: 1004
public class NgAnimation
{
	// Token: 0x06001804 RID: 6148 RVA: 0x000C5FB0 File Offset: 0x000C41B0
	public static AnimationClip SetAnimation(Animation tarAnimation, int tarIndex, AnimationClip srcClip)
	{
		int num = 0;
		AnimationClip[] array = new AnimationClip[tarAnimation.GetClipCount() - tarIndex + 1];
		foreach (object obj in tarAnimation)
		{
			AnimationState animationState = (AnimationState)obj;
			if (tarIndex == num)
			{
				tarAnimation.RemoveClip(animationState.clip);
			}
			if (tarIndex < num)
			{
				array[num - tarIndex - 1] = animationState.clip;
				tarAnimation.RemoveClip(animationState.clip);
			}
		}
		tarAnimation.AddClip(srcClip, srcClip.name);
		for (int i = 0; i < array.Length; i++)
		{
			tarAnimation.AddClip(array[i], array[i].name);
		}
		return srcClip;
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x000C6088 File Offset: 0x000C4288
	public static AnimationState GetAnimationByIndex(Animation anim, int nIndex)
	{
		int num = 0;
		foreach (object obj in anim)
		{
			AnimationState result = (AnimationState)obj;
			if (num == nIndex)
			{
				return result;
			}
			num++;
		}
		return null;
	}
}
