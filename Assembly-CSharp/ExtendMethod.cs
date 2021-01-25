using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014A RID: 330
public static class ExtendMethod
{
	// Token: 0x060006F1 RID: 1777 RVA: 0x0000626A File Offset: 0x0000446A
	public static float Distance(this Vector3 vector, Vector3 other)
	{
		return Vector3.Distance(vector, other);
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00047F94 File Offset: 0x00046194
	public static float Angle(this Vector3 vector, Vector3 other)
	{
		float num = Vector3.Angle(vector, other);
		float y = Vector3.Cross(vector, other).y;
		return (y >= 0f) ? num : (-num);
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x00006273 File Offset: 0x00004473
	public static void AttachTo(this GameObject go, GameObject parent)
	{
		go.transform.parent = parent.transform;
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = Vector3.zero;
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00047FCC File Offset: 0x000461CC
	public static void SiblingTo(this GameObject go, GameObject sibling)
	{
		go.transform.parent = sibling.transform.parent;
		go.transform.localScale = sibling.transform.localScale;
		go.transform.localPosition = sibling.transform.localPosition;
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0004801C File Offset: 0x0004621C
	public static void RemoveComponents<T>(this GameObject go) where T : Object
	{
		T[] enumeration = go.GetComponents(typeof(T)) as T[];
		enumeration.ForEach(delegate(T x)
		{
			Object.Destroy(x);
		});
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00048058 File Offset: 0x00046258
	public static void RemoveIf<T>(this List<T> list, Func<T, bool> func)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (func.Invoke(list[i]))
			{
				list.RemoveAt(i--);
			}
		}
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0004809C File Offset: 0x0004629C
	public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
	{
		int num = 0;
		foreach (T t in enumeration)
		{
			action.Invoke(t, num);
			num++;
		}
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x000480F8 File Offset: 0x000462F8
	public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
	{
		foreach (T t in enumeration)
		{
			action.Invoke(t);
		}
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0004814C File Offset: 0x0004634C
	public static int CountIf<T>(this IEnumerable<T> enumeration, Func<T, bool> func)
	{
		int num = 0;
		foreach (T t in enumeration)
		{
			if (func.Invoke(t))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x000481AC File Offset: 0x000463AC
	public static void ForEach(this Transform parent, Action<Transform> action)
	{
		foreach (object obj in parent)
		{
			Transform transform = (Transform)obj;
			action.Invoke(transform);
		}
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x0004820C File Offset: 0x0004640C
	public static void Traversal(this Transform parent, Action<Transform> action)
	{
		action.Invoke(parent);
		parent.ForEach(delegate(Transform x)
		{
			x.Traversal(action);
		});
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x000062A6 File Offset: 0x000044A6
	public static void Invoke(this MonoBehaviour comp, Action action, float delay = 0f)
	{
		comp.StartCoroutine(ExtendMethod.InvokeDelegate(action, delay));
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00048244 File Offset: 0x00046444
	private static IEnumerator InvokeDelegate(Action action, float delay)
	{
		yield return new WaitForSeconds(delay);
		action.Invoke();
		yield break;
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x000062B6 File Offset: 0x000044B6
	public static void SendMessage(this MonoBehaviour comp, ExtendMethod.CharacterMethod method, object value, SendMessageOptions option = 0)
	{
		comp.SendMessage(method.ToString(), value, option);
	}

	// Token: 0x0200014B RID: 331
	public enum CharacterMethod
	{
		// Token: 0x0400075F RID: 1887
		OnDeath,
		// Token: 0x04000760 RID: 1888
		OnEnemyDeath,
		// Token: 0x04000761 RID: 1889
		OnDamage
	}
}
