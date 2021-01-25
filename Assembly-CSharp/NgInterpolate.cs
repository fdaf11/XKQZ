using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003AF RID: 943
public class NgInterpolate
{
	// Token: 0x0600162A RID: 5674 RVA: 0x0000E400 File Offset: 0x0000C600
	private static Vector3 Identity(Vector3 v)
	{
		return v;
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x0000E403 File Offset: 0x0000C603
	private static Vector3 TransformDotPosition(Transform t)
	{
		return t.position;
	}

	// Token: 0x0600162C RID: 5676 RVA: 0x000BA38C File Offset: 0x000B858C
	private static IEnumerable<float> NewTimer(float duration)
	{
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			yield return elapsedTime;
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= duration)
			{
				yield return elapsedTime;
			}
		}
		yield break;
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x000BA3B8 File Offset: 0x000B85B8
	private static IEnumerable<float> NewCounter(int start, int end, int step)
	{
		for (int i = start; i <= end; i += step)
		{
			yield return (float)i;
		}
		yield break;
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x000BA400 File Offset: 0x000B8600
	public static IEnumerator NewEase(NgInterpolate.Function ease, Vector3 start, Vector3 end, float duration)
	{
		IEnumerable<float> driver = NgInterpolate.NewTimer(duration);
		return NgInterpolate.NewEase(ease, start, end, duration, driver);
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x000BA420 File Offset: 0x000B8620
	public static IEnumerator NewEase(NgInterpolate.Function ease, Vector3 start, Vector3 end, int slices)
	{
		IEnumerable<float> driver = NgInterpolate.NewCounter(0, slices + 1, 1);
		return NgInterpolate.NewEase(ease, start, end, (float)(slices + 1), driver);
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x000BA448 File Offset: 0x000B8648
	private static IEnumerator NewEase(NgInterpolate.Function ease, Vector3 start, Vector3 end, float total, IEnumerable<float> driver)
	{
		Vector3 distance = end - start;
		foreach (float num in driver)
		{
			float i = num;
			yield return NgInterpolate.Ease(ease, start, distance, i, total);
		}
		yield break;
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x000BA4A4 File Offset: 0x000B86A4
	private static Vector3 Ease(NgInterpolate.Function ease, Vector3 start, Vector3 distance, float elapsedTime, float duration)
	{
		start.x = ease(start.x, distance.x, elapsedTime, duration);
		start.y = ease(start.y, distance.y, elapsedTime, duration);
		start.z = ease(start.z, distance.z, elapsedTime, duration);
		return start;
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x000BA50C File Offset: 0x000B870C
	public static NgInterpolate.Function Ease(NgInterpolate.EaseType type)
	{
		NgInterpolate.Function result = null;
		switch (type)
		{
		case NgInterpolate.EaseType.Linear:
			result = new NgInterpolate.Function(NgInterpolate.Linear);
			break;
		case NgInterpolate.EaseType.EaseInQuad:
			result = new NgInterpolate.Function(NgInterpolate.EaseInQuad);
			break;
		case NgInterpolate.EaseType.EaseOutQuad:
			result = new NgInterpolate.Function(NgInterpolate.EaseOutQuad);
			break;
		case NgInterpolate.EaseType.EaseInOutQuad:
			result = new NgInterpolate.Function(NgInterpolate.EaseInOutQuad);
			break;
		case NgInterpolate.EaseType.EaseInCubic:
			result = new NgInterpolate.Function(NgInterpolate.EaseInCubic);
			break;
		case NgInterpolate.EaseType.EaseOutCubic:
			result = new NgInterpolate.Function(NgInterpolate.EaseOutCubic);
			break;
		case NgInterpolate.EaseType.EaseInOutCubic:
			result = new NgInterpolate.Function(NgInterpolate.EaseInOutCubic);
			break;
		case NgInterpolate.EaseType.EaseInQuart:
			result = new NgInterpolate.Function(NgInterpolate.EaseInQuart);
			break;
		case NgInterpolate.EaseType.EaseOutQuart:
			result = new NgInterpolate.Function(NgInterpolate.EaseOutQuart);
			break;
		case NgInterpolate.EaseType.EaseInOutQuart:
			result = new NgInterpolate.Function(NgInterpolate.EaseInOutQuart);
			break;
		case NgInterpolate.EaseType.EaseInQuint:
			result = new NgInterpolate.Function(NgInterpolate.EaseInQuint);
			break;
		case NgInterpolate.EaseType.EaseOutQuint:
			result = new NgInterpolate.Function(NgInterpolate.EaseOutQuint);
			break;
		case NgInterpolate.EaseType.EaseInOutQuint:
			result = new NgInterpolate.Function(NgInterpolate.EaseInOutQuint);
			break;
		case NgInterpolate.EaseType.EaseInSine:
			result = new NgInterpolate.Function(NgInterpolate.EaseInSine);
			break;
		case NgInterpolate.EaseType.EaseOutSine:
			result = new NgInterpolate.Function(NgInterpolate.EaseOutSine);
			break;
		case NgInterpolate.EaseType.EaseInOutSine:
			result = new NgInterpolate.Function(NgInterpolate.EaseInOutSine);
			break;
		case NgInterpolate.EaseType.EaseInExpo:
			result = new NgInterpolate.Function(NgInterpolate.EaseInExpo);
			break;
		case NgInterpolate.EaseType.EaseOutExpo:
			result = new NgInterpolate.Function(NgInterpolate.EaseOutExpo);
			break;
		case NgInterpolate.EaseType.EaseInOutExpo:
			result = new NgInterpolate.Function(NgInterpolate.EaseInOutExpo);
			break;
		case NgInterpolate.EaseType.EaseInCirc:
			result = new NgInterpolate.Function(NgInterpolate.EaseInCirc);
			break;
		case NgInterpolate.EaseType.EaseOutCirc:
			result = new NgInterpolate.Function(NgInterpolate.EaseOutCirc);
			break;
		case NgInterpolate.EaseType.EaseInOutCirc:
			result = new NgInterpolate.Function(NgInterpolate.EaseInOutCirc);
			break;
		}
		return result;
	}

	// Token: 0x06001633 RID: 5683 RVA: 0x000BA710 File Offset: 0x000B8910
	public static IEnumerable<Vector3> NewBezier(NgInterpolate.Function ease, Transform[] nodes, float duration)
	{
		IEnumerable<float> steps = NgInterpolate.NewTimer(duration);
		return NgInterpolate.NewBezier<Transform>(ease, nodes, new NgInterpolate.ToVector3<Transform>(NgInterpolate.TransformDotPosition), duration, steps);
	}

	// Token: 0x06001634 RID: 5684 RVA: 0x000BA73C File Offset: 0x000B893C
	public static IEnumerable<Vector3> NewBezier(NgInterpolate.Function ease, Transform[] nodes, int slices)
	{
		IEnumerable<float> steps = NgInterpolate.NewCounter(0, slices + 1, 1);
		return NgInterpolate.NewBezier<Transform>(ease, nodes, new NgInterpolate.ToVector3<Transform>(NgInterpolate.TransformDotPosition), (float)(slices + 1), steps);
	}

	// Token: 0x06001635 RID: 5685 RVA: 0x000BA76C File Offset: 0x000B896C
	public static IEnumerable<Vector3> NewBezier(NgInterpolate.Function ease, Vector3[] points, float duration)
	{
		IEnumerable<float> steps = NgInterpolate.NewTimer(duration);
		return NgInterpolate.NewBezier<Vector3>(ease, points, new NgInterpolate.ToVector3<Vector3>(NgInterpolate.Identity), duration, steps);
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x000BA798 File Offset: 0x000B8998
	public static IEnumerable<Vector3> NewBezier(NgInterpolate.Function ease, Vector3[] points, int slices)
	{
		IEnumerable<float> steps = NgInterpolate.NewCounter(0, slices + 1, 1);
		return NgInterpolate.NewBezier<Vector3>(ease, points, new NgInterpolate.ToVector3<Vector3>(NgInterpolate.Identity), (float)(slices + 1), steps);
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x000BA7C8 File Offset: 0x000B89C8
	private static IEnumerable<Vector3> NewBezier<T>(NgInterpolate.Function ease, IList nodes, NgInterpolate.ToVector3<T> toVector3, float maxStep, IEnumerable<float> steps)
	{
		if (nodes.Count >= 2)
		{
			Vector3[] points = new Vector3[nodes.Count];
			foreach (float num in steps)
			{
				float step = num;
				for (int i = 0; i < nodes.Count; i++)
				{
					points[i] = toVector3((T)((object)nodes[i]));
				}
				yield return NgInterpolate.Bezier(ease, points, step, maxStep);
			}
		}
		yield break;
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x000BA82C File Offset: 0x000B8A2C
	private static Vector3 Bezier(NgInterpolate.Function ease, Vector3[] points, float elapsedTime, float duration)
	{
		for (int i = points.Length - 1; i > 0; i--)
		{
			for (int j = 0; j < i; j++)
			{
				points[j].x = ease(points[j].x, points[j + 1].x - points[j].x, elapsedTime, duration);
				points[j].y = ease(points[j].y, points[j + 1].y - points[j].y, elapsedTime, duration);
				points[j].z = ease(points[j].z, points[j + 1].z - points[j].z, elapsedTime, duration);
			}
		}
		return points[0];
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x0000E40B File Offset: 0x0000C60B
	public static IEnumerable<Vector3> NewCatmullRom(Transform[] nodes, int slices, bool loop)
	{
		return NgInterpolate.NewCatmullRom<Transform>(nodes, new NgInterpolate.ToVector3<Transform>(NgInterpolate.TransformDotPosition), slices, loop);
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x0000E421 File Offset: 0x0000C621
	public static IEnumerable<Vector3> NewCatmullRom(Vector3[] points, int slices, bool loop)
	{
		return NgInterpolate.NewCatmullRom<Vector3>(points, new NgInterpolate.ToVector3<Vector3>(NgInterpolate.Identity), slices, loop);
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x000BA920 File Offset: 0x000B8B20
	private static IEnumerable<Vector3> NewCatmullRom<T>(IList nodes, NgInterpolate.ToVector3<T> toVector3, int slices, bool loop)
	{
		if (nodes.Count >= 2)
		{
			yield return toVector3((T)((object)nodes[0]));
			int last = nodes.Count - 1;
			int current = 0;
			while (loop || current < last)
			{
				if (loop && current > last)
				{
					current = 0;
				}
				int previous = (current != 0) ? (current - 1) : ((!loop) ? current : last);
				int start = current;
				int end = (current != last) ? (current + 1) : ((!loop) ? current : 0);
				int next = (end != last) ? (end + 1) : ((!loop) ? end : 0);
				int stepCount = slices + 1;
				for (int step = 1; step <= stepCount; step++)
				{
					yield return NgInterpolate.CatmullRom(toVector3((T)((object)nodes[previous])), toVector3((T)((object)nodes[start])), toVector3((T)((object)nodes[end])), toVector3((T)((object)nodes[next])), (float)step, (float)stepCount);
				}
				current++;
			}
		}
		yield break;
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x000BA974 File Offset: 0x000B8B74
	private static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime, float duration)
	{
		float num = elapsedTime / duration;
		float num2 = num * num;
		float num3 = num2 * num;
		return previous * (-0.5f * num3 + num2 - 0.5f * num) + start * (1.5f * num3 + -2.5f * num2 + 1f) + end * (-1.5f * num3 + 2f * num2 + 0.5f * num) + next * (0.5f * num3 - 0.5f * num2);
	}

	// Token: 0x0600163D RID: 5693 RVA: 0x0000E437 File Offset: 0x0000C637
	private static float Linear(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * (elapsedTime / duration) + start;
	}

	// Token: 0x0600163E RID: 5694 RVA: 0x0000E44A File Offset: 0x0000C64A
	private static float EaseInQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime + start;
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x0000E469 File Offset: 0x0000C669
	private static float EaseOutQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return -distance * elapsedTime * (elapsedTime - 2f) + start;
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x000BAA04 File Offset: 0x000B8C04
	private static float EaseInOutQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 1f;
		return -distance / 2f * (elapsedTime * (elapsedTime - 2f) - 1f) + start;
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x0000E48F File Offset: 0x0000C68F
	private static float EaseInCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime + start;
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x0000E4B0 File Offset: 0x0000C6B0
	private static float EaseOutCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * (elapsedTime * elapsedTime * elapsedTime + 1f) + start;
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x000BAA6C File Offset: 0x000B8C6C
	private static float EaseInOutCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (elapsedTime * elapsedTime * elapsedTime + 2f) + start;
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x0000E4E0 File Offset: 0x0000C6E0
	private static float EaseInQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x0000E503 File Offset: 0x0000C703
	private static float EaseOutQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return -distance * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 1f) + start;
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x000BAAD0 File Offset: 0x000B8CD0
	private static float EaseInOutQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return -distance / 2f * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 2f) + start;
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x0000E536 File Offset: 0x0000C736
	private static float EaseInQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
	}

	// Token: 0x06001648 RID: 5704 RVA: 0x0000E55B File Offset: 0x0000C75B
	private static float EaseOutQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * (elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 1f) + start;
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x000BAB38 File Offset: 0x000B8D38
	private static float EaseInOutQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 2f) + start;
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x0000E58F File Offset: 0x0000C78F
	private static float EaseInSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return -distance * Mathf.Cos(elapsedTime / duration * 1.5707964f) + distance + start;
	}

	// Token: 0x0600164B RID: 5707 RVA: 0x0000E5B0 File Offset: 0x0000C7B0
	private static float EaseOutSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * Mathf.Sin(elapsedTime / duration * 1.5707964f) + start;
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x0000E5CE File Offset: 0x0000C7CE
	private static float EaseInOutSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return -distance / 2f * (Mathf.Cos(3.1415927f * elapsedTime / duration) - 1f) + start;
	}

	// Token: 0x0600164D RID: 5709 RVA: 0x0000E5F9 File Offset: 0x0000C7F9
	private static float EaseInExpo(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * Mathf.Pow(2f, 10f * (elapsedTime / duration - 1f)) + start;
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x0000E622 File Offset: 0x0000C822
	private static float EaseOutExpo(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * (-Mathf.Pow(2f, -10f * elapsedTime / duration) + 1f) + start;
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x000BABA4 File Offset: 0x000B8DA4
	private static float EaseInOutExpo(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * Mathf.Pow(2f, 10f * (elapsedTime - 1f)) + start;
		}
		elapsedTime -= 1f;
		return distance / 2f * (-Mathf.Pow(2f, -10f * elapsedTime) + 2f) + start;
	}

	// Token: 0x06001650 RID: 5712 RVA: 0x0000E64C File Offset: 0x0000C84C
	private static float EaseInCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return -distance * (Mathf.Sqrt(1f - elapsedTime * elapsedTime) - 1f) + start;
	}

	// Token: 0x06001651 RID: 5713 RVA: 0x0000E67D File Offset: 0x0000C87D
	private static float EaseOutCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * Mathf.Sqrt(1f - elapsedTime * elapsedTime) + start;
	}

	// Token: 0x06001652 RID: 5714 RVA: 0x000BAC28 File Offset: 0x000B8E28
	private static float EaseInOutCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return -distance / 2f * (Mathf.Sqrt(1f - elapsedTime * elapsedTime) - 1f) + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (Mathf.Sqrt(1f - elapsedTime * elapsedTime) + 1f) + start;
	}

	// Token: 0x020003B0 RID: 944
	public enum EaseType
	{
		// Token: 0x04001A7C RID: 6780
		Linear,
		// Token: 0x04001A7D RID: 6781
		EaseInQuad,
		// Token: 0x04001A7E RID: 6782
		EaseOutQuad,
		// Token: 0x04001A7F RID: 6783
		EaseInOutQuad,
		// Token: 0x04001A80 RID: 6784
		EaseInCubic,
		// Token: 0x04001A81 RID: 6785
		EaseOutCubic,
		// Token: 0x04001A82 RID: 6786
		EaseInOutCubic,
		// Token: 0x04001A83 RID: 6787
		EaseInQuart,
		// Token: 0x04001A84 RID: 6788
		EaseOutQuart,
		// Token: 0x04001A85 RID: 6789
		EaseInOutQuart,
		// Token: 0x04001A86 RID: 6790
		EaseInQuint,
		// Token: 0x04001A87 RID: 6791
		EaseOutQuint,
		// Token: 0x04001A88 RID: 6792
		EaseInOutQuint,
		// Token: 0x04001A89 RID: 6793
		EaseInSine,
		// Token: 0x04001A8A RID: 6794
		EaseOutSine,
		// Token: 0x04001A8B RID: 6795
		EaseInOutSine,
		// Token: 0x04001A8C RID: 6796
		EaseInExpo,
		// Token: 0x04001A8D RID: 6797
		EaseOutExpo,
		// Token: 0x04001A8E RID: 6798
		EaseInOutExpo,
		// Token: 0x04001A8F RID: 6799
		EaseInCirc,
		// Token: 0x04001A90 RID: 6800
		EaseOutCirc,
		// Token: 0x04001A91 RID: 6801
		EaseInOutCirc
	}

	// Token: 0x020003B1 RID: 945
	// (Invoke) Token: 0x06001654 RID: 5716
	public delegate Vector3 ToVector3<T>(T v);

	// Token: 0x020003B2 RID: 946
	// (Invoke) Token: 0x06001658 RID: 5720
	public delegate float Function(float a, float b, float c, float d);
}
