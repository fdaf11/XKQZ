using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200061E RID: 1566
public class Interpolate
{
	// Token: 0x060026AA RID: 9898 RVA: 0x0000E400 File Offset: 0x0000C600
	private static Vector3 Identity(Vector3 v)
	{
		return v;
	}

	// Token: 0x060026AB RID: 9899 RVA: 0x0000E403 File Offset: 0x0000C603
	private static Vector3 TransformDotPosition(Transform t)
	{
		return t.position;
	}

	// Token: 0x060026AC RID: 9900 RVA: 0x0012C630 File Offset: 0x0012A830
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

	// Token: 0x060026AD RID: 9901 RVA: 0x0012C65C File Offset: 0x0012A85C
	private static IEnumerable<float> NewCounter(int start, int end, int step)
	{
		for (int i = start; i <= end; i += step)
		{
			yield return (float)i;
		}
		yield break;
	}

	// Token: 0x060026AE RID: 9902 RVA: 0x0012C6A4 File Offset: 0x0012A8A4
	public static IEnumerator NewEase(Interpolate.Function ease, Vector3 start, Vector3 end, float duration)
	{
		IEnumerable<float> driver = Interpolate.NewTimer(duration);
		return Interpolate.NewEase(ease, start, end, duration, driver);
	}

	// Token: 0x060026AF RID: 9903 RVA: 0x0012C6C4 File Offset: 0x0012A8C4
	public static IEnumerator NewEase(Interpolate.Function ease, Vector3 start, Vector3 end, int slices)
	{
		IEnumerable<float> driver = Interpolate.NewCounter(0, slices + 1, 1);
		return Interpolate.NewEase(ease, start, end, (float)(slices + 1), driver);
	}

	// Token: 0x060026B0 RID: 9904 RVA: 0x0012C6EC File Offset: 0x0012A8EC
	private static IEnumerator NewEase(Interpolate.Function ease, Vector3 start, Vector3 end, float total, IEnumerable<float> driver)
	{
		Vector3 distance = end - start;
		foreach (float num in driver)
		{
			float i = num;
			yield return Interpolate.Ease(ease, start, distance, i, total);
		}
		yield break;
	}

	// Token: 0x060026B1 RID: 9905 RVA: 0x0012C748 File Offset: 0x0012A948
	private static Vector3 Ease(Interpolate.Function ease, Vector3 start, Vector3 distance, float elapsedTime, float duration)
	{
		start.x = ease(start.x, distance.x, elapsedTime, duration);
		start.y = ease(start.y, distance.y, elapsedTime, duration);
		start.z = ease(start.z, distance.z, elapsedTime, duration);
		return start;
	}

	// Token: 0x060026B2 RID: 9906 RVA: 0x0012C7B0 File Offset: 0x0012A9B0
	public static Interpolate.Function Ease(Interpolate.EaseType type)
	{
		Interpolate.Function result = null;
		switch (type)
		{
		case Interpolate.EaseType.Linear:
			result = new Interpolate.Function(Interpolate.Linear);
			break;
		case Interpolate.EaseType.EaseInQuad:
			result = new Interpolate.Function(Interpolate.EaseInQuad);
			break;
		case Interpolate.EaseType.EaseOutQuad:
			result = new Interpolate.Function(Interpolate.EaseOutQuad);
			break;
		case Interpolate.EaseType.EaseInOutQuad:
			result = new Interpolate.Function(Interpolate.EaseInOutQuad);
			break;
		case Interpolate.EaseType.EaseInCubic:
			result = new Interpolate.Function(Interpolate.EaseInCubic);
			break;
		case Interpolate.EaseType.EaseOutCubic:
			result = new Interpolate.Function(Interpolate.EaseOutCubic);
			break;
		case Interpolate.EaseType.EaseInOutCubic:
			result = new Interpolate.Function(Interpolate.EaseInOutCubic);
			break;
		case Interpolate.EaseType.EaseInQuart:
			result = new Interpolate.Function(Interpolate.EaseInQuart);
			break;
		case Interpolate.EaseType.EaseOutQuart:
			result = new Interpolate.Function(Interpolate.EaseOutQuart);
			break;
		case Interpolate.EaseType.EaseInOutQuart:
			result = new Interpolate.Function(Interpolate.EaseInOutQuart);
			break;
		case Interpolate.EaseType.EaseInQuint:
			result = new Interpolate.Function(Interpolate.EaseInQuint);
			break;
		case Interpolate.EaseType.EaseOutQuint:
			result = new Interpolate.Function(Interpolate.EaseOutQuint);
			break;
		case Interpolate.EaseType.EaseInOutQuint:
			result = new Interpolate.Function(Interpolate.EaseInOutQuint);
			break;
		case Interpolate.EaseType.EaseInSine:
			result = new Interpolate.Function(Interpolate.EaseInSine);
			break;
		case Interpolate.EaseType.EaseOutSine:
			result = new Interpolate.Function(Interpolate.EaseOutSine);
			break;
		case Interpolate.EaseType.EaseInOutSine:
			result = new Interpolate.Function(Interpolate.EaseInOutSine);
			break;
		case Interpolate.EaseType.EaseInExpo:
			result = new Interpolate.Function(Interpolate.EaseInExpo);
			break;
		case Interpolate.EaseType.EaseOutExpo:
			result = new Interpolate.Function(Interpolate.EaseOutExpo);
			break;
		case Interpolate.EaseType.EaseInOutExpo:
			result = new Interpolate.Function(Interpolate.EaseInOutExpo);
			break;
		case Interpolate.EaseType.EaseInCirc:
			result = new Interpolate.Function(Interpolate.EaseInCirc);
			break;
		case Interpolate.EaseType.EaseOutCirc:
			result = new Interpolate.Function(Interpolate.EaseOutCirc);
			break;
		case Interpolate.EaseType.EaseInOutCirc:
			result = new Interpolate.Function(Interpolate.EaseInOutCirc);
			break;
		}
		return result;
	}

	// Token: 0x060026B3 RID: 9907 RVA: 0x0012C9B4 File Offset: 0x0012ABB4
	public static IEnumerable<Vector3> NewBezier(Interpolate.Function ease, Transform[] nodes, float duration)
	{
		IEnumerable<float> steps = Interpolate.NewTimer(duration);
		return Interpolate.NewBezier<Transform>(ease, nodes, new Interpolate.ToVector3<Transform>(Interpolate.TransformDotPosition), duration, steps);
	}

	// Token: 0x060026B4 RID: 9908 RVA: 0x0012C9E0 File Offset: 0x0012ABE0
	public static IEnumerable<Vector3> NewBezier(Interpolate.Function ease, Transform[] nodes, int slices)
	{
		IEnumerable<float> steps = Interpolate.NewCounter(0, slices + 1, 1);
		return Interpolate.NewBezier<Transform>(ease, nodes, new Interpolate.ToVector3<Transform>(Interpolate.TransformDotPosition), (float)(slices + 1), steps);
	}

	// Token: 0x060026B5 RID: 9909 RVA: 0x0012CA10 File Offset: 0x0012AC10
	public static IEnumerable<Vector3> NewBezier(Interpolate.Function ease, Vector3[] points, float duration)
	{
		IEnumerable<float> steps = Interpolate.NewTimer(duration);
		return Interpolate.NewBezier<Vector3>(ease, points, new Interpolate.ToVector3<Vector3>(Interpolate.Identity), duration, steps);
	}

	// Token: 0x060026B6 RID: 9910 RVA: 0x0012CA3C File Offset: 0x0012AC3C
	public static IEnumerable<Vector3> NewBezier(Interpolate.Function ease, Vector3[] points, int slices)
	{
		IEnumerable<float> steps = Interpolate.NewCounter(0, slices + 1, 1);
		return Interpolate.NewBezier<Vector3>(ease, points, new Interpolate.ToVector3<Vector3>(Interpolate.Identity), (float)(slices + 1), steps);
	}

	// Token: 0x060026B7 RID: 9911 RVA: 0x0012CA6C File Offset: 0x0012AC6C
	private static IEnumerable<Vector3> NewBezier<T>(Interpolate.Function ease, IList nodes, Interpolate.ToVector3<T> toVector3, float maxStep, IEnumerable<float> steps)
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
				yield return Interpolate.Bezier(ease, points, step, maxStep);
			}
		}
		yield break;
	}

	// Token: 0x060026B8 RID: 9912 RVA: 0x0012CAD0 File Offset: 0x0012ACD0
	private static Vector3 Bezier(Interpolate.Function ease, Vector3[] points, float elapsedTime, float duration)
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

	// Token: 0x060026B9 RID: 9913 RVA: 0x00019B9B File Offset: 0x00017D9B
	public static IEnumerable<Vector3> NewCatmullRom(Transform[] nodes, int slices, bool loop)
	{
		return Interpolate.NewCatmullRom<Transform>(nodes, new Interpolate.ToVector3<Transform>(Interpolate.TransformDotPosition), slices, loop);
	}

	// Token: 0x060026BA RID: 9914 RVA: 0x00019BB1 File Offset: 0x00017DB1
	public static IEnumerable<Vector3> NewCatmullRom(Vector3[] points, int slices, bool loop)
	{
		return Interpolate.NewCatmullRom<Vector3>(points, new Interpolate.ToVector3<Vector3>(Interpolate.Identity), slices, loop);
	}

	// Token: 0x060026BB RID: 9915 RVA: 0x0012CBC4 File Offset: 0x0012ADC4
	private static IEnumerable<Vector3> NewCatmullRom<T>(IList nodes, Interpolate.ToVector3<T> toVector3, int slices, bool loop)
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
					yield return Interpolate.CatmullRom(toVector3((T)((object)nodes[previous])), toVector3((T)((object)nodes[start])), toVector3((T)((object)nodes[end])), toVector3((T)((object)nodes[next])), (float)step, (float)stepCount);
				}
				current++;
			}
		}
		yield break;
	}

	// Token: 0x060026BC RID: 9916 RVA: 0x000BA974 File Offset: 0x000B8B74
	private static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime, float duration)
	{
		float num = elapsedTime / duration;
		float num2 = num * num;
		float num3 = num2 * num;
		return previous * (-0.5f * num3 + num2 - 0.5f * num) + start * (1.5f * num3 + -2.5f * num2 + 1f) + end * (-1.5f * num3 + 2f * num2 + 0.5f * num) + next * (0.5f * num3 - 0.5f * num2);
	}

	// Token: 0x060026BD RID: 9917 RVA: 0x0000E437 File Offset: 0x0000C637
	private static float Linear(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * (elapsedTime / duration) + start;
	}

	// Token: 0x060026BE RID: 9918 RVA: 0x0000E44A File Offset: 0x0000C64A
	private static float EaseInQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime + start;
	}

	// Token: 0x060026BF RID: 9919 RVA: 0x0000E469 File Offset: 0x0000C669
	private static float EaseOutQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return -distance * elapsedTime * (elapsedTime - 2f) + start;
	}

	// Token: 0x060026C0 RID: 9920 RVA: 0x000BAA04 File Offset: 0x000B8C04
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

	// Token: 0x060026C1 RID: 9921 RVA: 0x0000E48F File Offset: 0x0000C68F
	private static float EaseInCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime + start;
	}

	// Token: 0x060026C2 RID: 9922 RVA: 0x0000E4B0 File Offset: 0x0000C6B0
	private static float EaseOutCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * (elapsedTime * elapsedTime * elapsedTime + 1f) + start;
	}

	// Token: 0x060026C3 RID: 9923 RVA: 0x000BAA6C File Offset: 0x000B8C6C
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

	// Token: 0x060026C4 RID: 9924 RVA: 0x0000E4E0 File Offset: 0x0000C6E0
	private static float EaseInQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x0000E503 File Offset: 0x0000C703
	private static float EaseOutQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return -distance * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 1f) + start;
	}

	// Token: 0x060026C6 RID: 9926 RVA: 0x000BAAD0 File Offset: 0x000B8CD0
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

	// Token: 0x060026C7 RID: 9927 RVA: 0x0000E536 File Offset: 0x0000C736
	private static float EaseInQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
	}

	// Token: 0x060026C8 RID: 9928 RVA: 0x0000E55B File Offset: 0x0000C75B
	private static float EaseOutQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * (elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 1f) + start;
	}

	// Token: 0x060026C9 RID: 9929 RVA: 0x000BAB38 File Offset: 0x000B8D38
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

	// Token: 0x060026CA RID: 9930 RVA: 0x0000E58F File Offset: 0x0000C78F
	private static float EaseInSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return -distance * Mathf.Cos(elapsedTime / duration * 1.5707964f) + distance + start;
	}

	// Token: 0x060026CB RID: 9931 RVA: 0x0000E5B0 File Offset: 0x0000C7B0
	private static float EaseOutSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * Mathf.Sin(elapsedTime / duration * 1.5707964f) + start;
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x0000E5CE File Offset: 0x0000C7CE
	private static float EaseInOutSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return -distance / 2f * (Mathf.Cos(3.1415927f * elapsedTime / duration) - 1f) + start;
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x0000E5F9 File Offset: 0x0000C7F9
	private static float EaseInExpo(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * Mathf.Pow(2f, 10f * (elapsedTime / duration - 1f)) + start;
	}

	// Token: 0x060026CE RID: 9934 RVA: 0x0000E622 File Offset: 0x0000C822
	private static float EaseOutExpo(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * (-Mathf.Pow(2f, -10f * elapsedTime / duration) + 1f) + start;
	}

	// Token: 0x060026CF RID: 9935 RVA: 0x000BABA4 File Offset: 0x000B8DA4
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

	// Token: 0x060026D0 RID: 9936 RVA: 0x0000E64C File Offset: 0x0000C84C
	private static float EaseInCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return -distance * (Mathf.Sqrt(1f - elapsedTime * elapsedTime) - 1f) + start;
	}

	// Token: 0x060026D1 RID: 9937 RVA: 0x0000E67D File Offset: 0x0000C87D
	private static float EaseOutCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * Mathf.Sqrt(1f - elapsedTime * elapsedTime) + start;
	}

	// Token: 0x060026D2 RID: 9938 RVA: 0x000BAC28 File Offset: 0x000B8E28
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

	// Token: 0x0200061F RID: 1567
	public enum EaseType
	{
		// Token: 0x04002FC8 RID: 12232
		Linear,
		// Token: 0x04002FC9 RID: 12233
		EaseInQuad,
		// Token: 0x04002FCA RID: 12234
		EaseOutQuad,
		// Token: 0x04002FCB RID: 12235
		EaseInOutQuad,
		// Token: 0x04002FCC RID: 12236
		EaseInCubic,
		// Token: 0x04002FCD RID: 12237
		EaseOutCubic,
		// Token: 0x04002FCE RID: 12238
		EaseInOutCubic,
		// Token: 0x04002FCF RID: 12239
		EaseInQuart,
		// Token: 0x04002FD0 RID: 12240
		EaseOutQuart,
		// Token: 0x04002FD1 RID: 12241
		EaseInOutQuart,
		// Token: 0x04002FD2 RID: 12242
		EaseInQuint,
		// Token: 0x04002FD3 RID: 12243
		EaseOutQuint,
		// Token: 0x04002FD4 RID: 12244
		EaseInOutQuint,
		// Token: 0x04002FD5 RID: 12245
		EaseInSine,
		// Token: 0x04002FD6 RID: 12246
		EaseOutSine,
		// Token: 0x04002FD7 RID: 12247
		EaseInOutSine,
		// Token: 0x04002FD8 RID: 12248
		EaseInExpo,
		// Token: 0x04002FD9 RID: 12249
		EaseOutExpo,
		// Token: 0x04002FDA RID: 12250
		EaseInOutExpo,
		// Token: 0x04002FDB RID: 12251
		EaseInCirc,
		// Token: 0x04002FDC RID: 12252
		EaseOutCirc,
		// Token: 0x04002FDD RID: 12253
		EaseInOutCirc
	}

	// Token: 0x02000620 RID: 1568
	// (Invoke) Token: 0x060026D4 RID: 9940
	public delegate Vector3 ToVector3<T>(T v);

	// Token: 0x02000621 RID: 1569
	// (Invoke) Token: 0x060026D8 RID: 9944
	public delegate float Function(float a, float b, float c, float d);
}
