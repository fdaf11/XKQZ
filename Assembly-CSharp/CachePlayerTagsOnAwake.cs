using System;
using AIBehavior;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class CachePlayerTagsOnAwake : MonoBehaviour
{
	// Token: 0x06000005 RID: 5 RVA: 0x00021D04 File Offset: 0x0001FF04
	private void Awake()
	{
		AIBehaviors[] array = Object.FindObjectsOfType<AIBehaviors>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].objectFinder.CacheTransforms(CachePoint.Awake);
			array[i].objectFinder.CacheTransforms(CachePoint.StateChanged);
			array[i].objectFinder.CacheTransforms(CachePoint.EveryFrame);
		}
	}
}
