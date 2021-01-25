using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003F7 RID: 1015
public class NgObject
{
	// Token: 0x0600185C RID: 6236 RVA: 0x0000FF26 File Offset: 0x0000E126
	public static void SetActive(GameObject target, bool bActive)
	{
		target.SetActive(bActive);
	}

	// Token: 0x0600185D RID: 6237 RVA: 0x000C7724 File Offset: 0x000C5924
	public static void SetActiveRecursively(GameObject target, bool bActive)
	{
		int num = target.transform.childCount - 1;
		while (0 <= num)
		{
			if (num < target.transform.childCount)
			{
				NgObject.SetActiveRecursively(target.transform.GetChild(num).gameObject, bActive);
			}
			num--;
		}
		target.SetActive(bActive);
	}

	// Token: 0x0600185E RID: 6238 RVA: 0x0000E8EA File Offset: 0x0000CAEA
	public static bool IsActive(GameObject target)
	{
		return target.activeInHierarchy && target.activeSelf;
	}

	// Token: 0x0600185F RID: 6239 RVA: 0x000C7780 File Offset: 0x000C5980
	public static GameObject CreateGameObject(GameObject prefabObj)
	{
		return (GameObject)NcSafeTool.SafeInstantiate(prefabObj);
	}

	// Token: 0x06001860 RID: 6240 RVA: 0x0000FF2F File Offset: 0x0000E12F
	public static GameObject CreateGameObject(GameObject parent, string name)
	{
		return NgObject.CreateGameObject(parent.transform, name);
	}

	// Token: 0x06001861 RID: 6241 RVA: 0x0000FF3D File Offset: 0x0000E13D
	public static GameObject CreateGameObject(MonoBehaviour parent, string name)
	{
		return NgObject.CreateGameObject(parent.transform, name);
	}

	// Token: 0x06001862 RID: 6242 RVA: 0x000C779C File Offset: 0x000C599C
	public static GameObject CreateGameObject(Transform parent, string name)
	{
		GameObject gameObject = new GameObject(name);
		if (parent != null)
		{
			NcTransformTool ncTransformTool = new NcTransformTool(gameObject.transform);
			gameObject.transform.parent = parent;
			ncTransformTool.CopyToLocalTransform(gameObject.transform);
		}
		return gameObject;
	}

	// Token: 0x06001863 RID: 6243 RVA: 0x0000FF4B File Offset: 0x0000E14B
	public static GameObject CreateGameObject(GameObject parent, GameObject prefabObj)
	{
		return NgObject.CreateGameObject(parent.transform, prefabObj);
	}

	// Token: 0x06001864 RID: 6244 RVA: 0x0000FF59 File Offset: 0x0000E159
	public static GameObject CreateGameObject(MonoBehaviour parent, GameObject prefabObj)
	{
		return NgObject.CreateGameObject(parent.transform, prefabObj);
	}

	// Token: 0x06001865 RID: 6245 RVA: 0x000C77E4 File Offset: 0x000C59E4
	public static GameObject CreateGameObject(Transform parent, GameObject prefabObj)
	{
		GameObject gameObject = (GameObject)NcSafeTool.SafeInstantiate(prefabObj);
		if (parent != null)
		{
			NcTransformTool ncTransformTool = new NcTransformTool(gameObject.transform);
			gameObject.transform.parent = parent;
			ncTransformTool.CopyToLocalTransform(gameObject.transform);
		}
		return gameObject;
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x0000FF67 File Offset: 0x0000E167
	public static GameObject CreateGameObject(GameObject parent, GameObject prefabObj, Vector3 pos, Quaternion rot)
	{
		return NgObject.CreateGameObject(parent.transform, prefabObj, pos, rot);
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x0000FF77 File Offset: 0x0000E177
	public static GameObject CreateGameObject(MonoBehaviour parent, GameObject prefabObj, Vector3 pos, Quaternion rot)
	{
		return NgObject.CreateGameObject(parent.transform, prefabObj, pos, rot);
	}

	// Token: 0x06001868 RID: 6248 RVA: 0x000C7830 File Offset: 0x000C5A30
	public static GameObject CreateGameObject(Transform parent, GameObject prefabObj, Vector3 pos, Quaternion rot)
	{
		if (!NcSafeTool.IsSafe())
		{
			return null;
		}
		GameObject gameObject = (GameObject)NcSafeTool.SafeInstantiate(prefabObj, pos, rot);
		if (parent != null)
		{
			NcTransformTool ncTransformTool = new NcTransformTool(gameObject.transform);
			gameObject.transform.parent = parent;
			ncTransformTool.CopyToLocalTransform(gameObject.transform);
		}
		return gameObject;
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x000C7888 File Offset: 0x000C5A88
	public static void HideAllChildObject(GameObject parent)
	{
		int num = parent.transform.childCount - 1;
		while (0 <= num)
		{
			if (num < parent.transform.childCount)
			{
				NgObject.IsActive(parent.transform.GetChild(num).gameObject);
			}
			num--;
		}
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x000B9134 File Offset: 0x000B7334
	public static void RemoveAllChildObject(GameObject parent, bool bImmediate)
	{
		int num = parent.transform.childCount - 1;
		while (0 <= num)
		{
			if (num < parent.transform.childCount)
			{
				Transform child = parent.transform.GetChild(num);
				if (bImmediate)
				{
					Object.DestroyImmediate(child.gameObject);
				}
				else
				{
					Object.Destroy(child.gameObject);
				}
			}
			num--;
		}
	}

	// Token: 0x0600186B RID: 6251 RVA: 0x0000FF87 File Offset: 0x0000E187
	public static Component CreateComponent(Transform parent, Type type)
	{
		return NgObject.CreateComponent(parent.gameObject, type);
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x0000FF87 File Offset: 0x0000E187
	public static Component CreateComponent(MonoBehaviour parent, Type type)
	{
		return NgObject.CreateComponent(parent.gameObject, type);
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x000C78DC File Offset: 0x000C5ADC
	public static Component CreateComponent(GameObject parent, Type type)
	{
		Component component = parent.GetComponent(type);
		if (component != null)
		{
			return component;
		}
		return parent.AddComponent(type);
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x000C7908 File Offset: 0x000C5B08
	public static Transform FindTransform(Transform rootTrans, string name)
	{
		Transform transform = rootTrans.Find(name);
		if (transform)
		{
			return transform;
		}
		foreach (object obj in rootTrans)
		{
			Transform rootTrans2 = (Transform)obj;
			transform = NgObject.FindTransform(rootTrans2, name);
			if (transform)
			{
				return transform;
			}
		}
		return null;
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x000C7994 File Offset: 0x000C5B94
	public static bool FindTransform(Transform rootTrans, Transform findTrans)
	{
		if (rootTrans == findTrans)
		{
			return true;
		}
		foreach (object obj in rootTrans)
		{
			Transform rootTrans2 = (Transform)obj;
			if (NgObject.FindTransform(rootTrans2, findTrans))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x000C7A10 File Offset: 0x000C5C10
	public static Material ChangeMeshMaterial(Transform t, Material newMat)
	{
		MeshRenderer[] componentsInChildren = t.GetComponentsInChildren<MeshRenderer>(true);
		Material result = null;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			result = componentsInChildren[i].material;
			componentsInChildren[i].material = newMat;
		}
		return result;
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x000C7A50 File Offset: 0x000C5C50
	public static void ChangeSkinnedMeshColor(Transform t, Color color)
	{
		SkinnedMeshRenderer[] componentsInChildren = t.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.color = color;
		}
	}

	// Token: 0x06001872 RID: 6258 RVA: 0x000C7A88 File Offset: 0x000C5C88
	public static void ChangeMeshColor(Transform t, Color color)
	{
		MeshRenderer[] componentsInChildren = t.GetComponentsInChildren<MeshRenderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.color = color;
		}
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x000C7AC0 File Offset: 0x000C5CC0
	public static void ChangeSkinnedMeshAlpha(Transform t, float alpha)
	{
		SkinnedMeshRenderer[] componentsInChildren = t.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Color color = componentsInChildren[i].material.color;
			color.a = alpha;
			componentsInChildren[i].material.color = color;
		}
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x000C7B10 File Offset: 0x000C5D10
	public static void ChangeMeshAlpha(Transform t, float alpha)
	{
		MeshRenderer[] componentsInChildren = t.GetComponentsInChildren<MeshRenderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Color color = componentsInChildren[i].material.color;
			color.a = alpha;
			componentsInChildren[i].material.color = color;
		}
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x000C7B60 File Offset: 0x000C5D60
	public static Transform[] GetChilds(Transform parentObj)
	{
		Transform[] componentsInChildren = parentObj.GetComponentsInChildren<Transform>(true);
		Transform[] array = new Transform[componentsInChildren.Length - 1];
		for (int i = 1; i < componentsInChildren.Length; i++)
		{
			array[i - 1] = componentsInChildren[i];
		}
		return array;
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x000C7BA0 File Offset: 0x000C5DA0
	public static SortedList GetChildsSortList(Transform parentObj, bool bSub, bool bOnlyActive)
	{
		SortedList sortedList = new SortedList();
		if (bSub)
		{
			Transform[] componentsInChildren = parentObj.GetComponentsInChildren<Transform>(bOnlyActive);
			for (int i = 1; i < componentsInChildren.Length; i++)
			{
				sortedList.Add(componentsInChildren[i].name, componentsInChildren[i]);
			}
		}
		else
		{
			for (int j = 0; j < parentObj.childCount; j++)
			{
				Transform child = parentObj.GetChild(j);
				sortedList.Add(child.name, child);
			}
		}
		return sortedList;
	}

	// Token: 0x06001877 RID: 6263 RVA: 0x000C7C1C File Offset: 0x000C5E1C
	public static GameObject FindObjectWithTag(GameObject rootObj, string findTag)
	{
		if (rootObj == null)
		{
			return null;
		}
		if (rootObj.tag == findTag)
		{
			return rootObj;
		}
		for (int i = 0; i < rootObj.transform.childCount; i++)
		{
			GameObject gameObject = NgObject.FindObjectWithTag(rootObj.transform.GetChild(i).gameObject, findTag);
			if (gameObject != null)
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x000C7C90 File Offset: 0x000C5E90
	public static GameObject FindObjectWithLayer(GameObject rootObj, int nFindLayer)
	{
		if (rootObj == null)
		{
			return null;
		}
		if (rootObj.layer == nFindLayer)
		{
			return rootObj;
		}
		for (int i = 0; i < rootObj.transform.childCount; i++)
		{
			GameObject gameObject = NgObject.FindObjectWithLayer(rootObj.transform.GetChild(i).gameObject, nFindLayer);
			if (gameObject != null)
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x000C7CFC File Offset: 0x000C5EFC
	public static void ChangeLayerWithChild(GameObject rootObj, int nLayer)
	{
		if (rootObj == null)
		{
			return;
		}
		rootObj.layer = nLayer;
		for (int i = 0; i < rootObj.transform.childCount; i++)
		{
			NgObject.ChangeLayerWithChild(rootObj.transform.GetChild(i).gameObject, nLayer);
		}
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x000C7D50 File Offset: 0x000C5F50
	public static void GetMeshInfo(GameObject selObj, bool bInChildren, out int nVertices, out int nTriangles, out int nMeshCount)
	{
		nVertices = 0;
		nTriangles = 0;
		nMeshCount = 0;
		if (selObj == null)
		{
			return;
		}
		Component[] array;
		Component[] array2;
		if (bInChildren)
		{
			array = selObj.GetComponentsInChildren(typeof(SkinnedMeshRenderer));
			array2 = selObj.GetComponentsInChildren(typeof(MeshFilter));
		}
		else
		{
			array = selObj.GetComponents(typeof(SkinnedMeshRenderer));
			array2 = selObj.GetComponents(typeof(MeshFilter));
		}
		ArrayList arrayList = new ArrayList(array2.Length + array.Length);
		foreach (MeshFilter meshFilter in array2)
		{
			arrayList.Add(meshFilter.sharedMesh);
		}
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array)
		{
			arrayList.Add(skinnedMeshRenderer.sharedMesh);
		}
		for (int k = 0; k < arrayList.Count; k++)
		{
			Mesh mesh = (Mesh)arrayList[k];
			if (mesh != null)
			{
				nVertices += mesh.vertexCount;
				nTriangles += mesh.triangles.Length / 3;
				nMeshCount++;
			}
		}
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x000C7E88 File Offset: 0x000C6088
	public static void GetMeshInfo(Mesh mesh, out int nVertices, out int nTriangles, out int nMeshCount)
	{
		nVertices = 0;
		nTriangles = 0;
		nMeshCount = 0;
		if (mesh == null)
		{
			return;
		}
		if (mesh != null)
		{
			nVertices += mesh.vertexCount;
			nTriangles += mesh.triangles.Length / 3;
			nMeshCount++;
		}
	}
}
