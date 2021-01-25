using System;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public class AnimationMoveOffestManager
{
	// Token: 0x060004CD RID: 1229 RVA: 0x00038F90 File Offset: 0x00037190
	public void Load()
	{
		string text = "BattleField/AnimationMoveOffest";
		GameObject gameObject = Resources.Load(text, typeof(GameObject)) as GameObject;
		if (gameObject == null)
		{
			GameObject gameObject2 = new GameObject("AnimationMoveOffest");
			this.animationMoveOffsetGroup = gameObject2.AddComponent<AnimationMoveOffestGroup>();
		}
		else
		{
			GameObject gameObject3 = Object.Instantiate(gameObject) as GameObject;
			gameObject3.name = "AnimationMoveOffest";
			this.animationMoveOffsetGroup = gameObject3.GetComponent<AnimationMoveOffestGroup>();
		}
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00039004 File Offset: 0x00037204
	public AnimationMoveOffest GetMoveOffest(string sName, string sClip)
	{
		foreach (AnimationMoveOffest animationMoveOffest in this.animationMoveOffsetGroup.animationMoveOffectList)
		{
			if (animationMoveOffest.strName == sName && animationMoveOffest.strClip == sClip)
			{
				return animationMoveOffest;
			}
		}
		return null;
	}

	// Token: 0x0400047D RID: 1149
	public AnimationMoveOffestGroup animationMoveOffsetGroup;
}
