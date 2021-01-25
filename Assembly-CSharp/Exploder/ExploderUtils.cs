using System;
using System.Diagnostics;
using UnityEngine;

namespace Exploder
{
	// Token: 0x020000BD RID: 189
	public static class ExploderUtils
	{
		// Token: 0x060003ED RID: 1005 RVA: 0x00004B32 File Offset: 0x00002D32
		[Conditional("UNITY_EDITOR_DEBUG")]
		public static void Assert(bool condition, string message)
		{
			if (!condition)
			{
				Debug.LogError("Assert! " + message);
				Debug.Break();
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00004B4F File Offset: 0x00002D4F
		[Conditional("UNITY_EDITOR_DEBUG")]
		public static void Warning(bool condition, string message)
		{
			if (!condition)
			{
				Debug.LogWarning("Warning! " + message);
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00004B67 File Offset: 0x00002D67
		[Conditional("UNITY_EDITOR_DEBUG")]
		public static void Log(string message)
		{
			Debug.Log(message);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x000324D8 File Offset: 0x000306D8
		public static Vector3 GetCentroid(GameObject obj)
		{
			MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>();
			Vector3 vector = Vector3.zero;
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				foreach (MeshRenderer meshRenderer in componentsInChildren)
				{
					vector += meshRenderer.bounds.center;
				}
				return vector / (float)componentsInChildren.Length;
			}
			SkinnedMeshRenderer componentInChildren = obj.GetComponentInChildren<SkinnedMeshRenderer>();
			if (componentInChildren)
			{
				return componentInChildren.bounds.center;
			}
			return obj.transform.position;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00032570 File Offset: 0x00030770
		public static void SetVisible(GameObject obj, bool status)
		{
			if (obj)
			{
				MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer meshRenderer in componentsInChildren)
				{
					meshRenderer.enabled = status;
				}
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000264F File Offset: 0x0000084F
		public static void ClearLog()
		{
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00004B6F File Offset: 0x00002D6F
		public static bool IsActive(GameObject obj)
		{
			return obj && obj.activeSelf;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00004B85 File Offset: 0x00002D85
		public static void SetActive(GameObject obj, bool status)
		{
			if (obj)
			{
				obj.SetActive(status);
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000325B0 File Offset: 0x000307B0
		public static void SetActiveRecursively(GameObject obj, bool status)
		{
			if (obj)
			{
				int childCount = obj.transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					ExploderUtils.SetActiveRecursively(obj.transform.GetChild(i).gameObject, status);
				}
				obj.SetActive(status);
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00032604 File Offset: 0x00030804
		public static void EnableCollider(GameObject obj, bool status)
		{
			if (obj)
			{
				Collider[] componentsInChildren = obj.GetComponentsInChildren<Collider>();
				foreach (Collider collider in componentsInChildren)
				{
					collider.enabled = status;
				}
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00032644 File Offset: 0x00030844
		public static bool IsExplodable(GameObject obj)
		{
			bool flag = obj.GetComponent<Explodable>() != null;
			if (!flag)
			{
				flag = obj.CompareTag(ExploderObject.Tag);
			}
			return flag;
		}
	}
}
