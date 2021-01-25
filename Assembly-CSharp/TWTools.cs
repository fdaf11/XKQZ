using System;
using UnityEngine;

// Token: 0x0200060A RID: 1546
public static class TWTools
{
	// Token: 0x06002642 RID: 9794 RVA: 0x0000FF26 File Offset: 0x0000E126
	public static void SetActiveSelf(GameObject go, bool state)
	{
		go.SetActive(state);
	}

	// Token: 0x06002643 RID: 9795 RVA: 0x00128CAC File Offset: 0x00126EAC
	private static void Activate(Transform t)
	{
		TWTools.SetActiveSelf(t.gameObject, true);
		int i = 0;
		int childCount = t.GetChildCount();
		while (i < childCount)
		{
			Transform child = t.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				return;
			}
			i++;
		}
		int j = 0;
		int childCount2 = t.GetChildCount();
		while (j < childCount2)
		{
			Transform child2 = t.GetChild(j);
			TWTools.Activate(child2);
			j++;
		}
	}

	// Token: 0x06002644 RID: 9796 RVA: 0x00019770 File Offset: 0x00017970
	private static void Deactivate(Transform t)
	{
		TWTools.SetActiveSelf(t.gameObject, false);
	}

	// Token: 0x06002645 RID: 9797 RVA: 0x0001977E File Offset: 0x0001797E
	public static void SetActive(GameObject go, bool state)
	{
		if (state)
		{
			TWTools.Activate(go.transform);
		}
		else
		{
			TWTools.Deactivate(go.transform);
		}
	}

	// Token: 0x06002646 RID: 9798 RVA: 0x000E3FFC File Offset: 0x000E21FC
	public static GameObject AddChild(GameObject parent)
	{
		GameObject gameObject = new GameObject();
		if (parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	// Token: 0x06002647 RID: 9799 RVA: 0x000E405C File Offset: 0x000E225C
	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = Object.Instantiate(prefab) as GameObject;
		if (gameObject != null && parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	// Token: 0x06002648 RID: 9800 RVA: 0x000197A1 File Offset: 0x000179A1
	public static void Destroy(Object obj)
	{
		if (obj != null)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(obj);
			}
			else
			{
				Object.DestroyImmediate(obj);
			}
		}
	}

	// Token: 0x06002649 RID: 9801 RVA: 0x000134FE File Offset: 0x000116FE
	public static void DestroyImmediate(Object obj)
	{
		if (obj != null)
		{
			if (Application.isEditor)
			{
				Object.DestroyImmediate(obj);
			}
			else
			{
				Object.Destroy(obj);
			}
		}
	}
}
