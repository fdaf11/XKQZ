using System;
using UnityEngine;

// Token: 0x020007AE RID: 1966
public class Utility : MonoBehaviour
{
	// Token: 0x06003017 RID: 12311 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x06003018 RID: 12312 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06003019 RID: 12313 RVA: 0x0001E680 File Offset: 0x0001C880
	public static bool IsActive(Transform obj)
	{
		return Utility.IsActive(obj.gameObject);
	}

	// Token: 0x0600301A RID: 12314 RVA: 0x0001E68D File Offset: 0x0001C88D
	public static bool IsActive(GameObject obj)
	{
		return obj.activeInHierarchy;
	}

	// Token: 0x0600301B RID: 12315 RVA: 0x0001E69D File Offset: 0x0001C89D
	public static void SetActive(Transform obj, bool flag)
	{
		Utility.SetActive(obj.gameObject, flag);
	}

	// Token: 0x0600301C RID: 12316 RVA: 0x0001E6AB File Offset: 0x0001C8AB
	public static void SetActive(GameObject obj, bool flag)
	{
		if (Utility.IsActive(obj) != flag)
		{
			obj.SetActive(flag);
		}
	}

	// Token: 0x0600301D RID: 12317 RVA: 0x001757B8 File Offset: 0x001739B8
	public static void SetLayerRecursively(Transform root, int layer)
	{
		foreach (object obj in root)
		{
			Transform transform = (Transform)obj;
			transform.gameObject.layer = layer;
			Utility.SetLayerRecursively(transform, layer);
		}
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x0001E6C0 File Offset: 0x0001C8C0
	public static float VectorToAngle(Vector3 dir)
	{
		return Utility.VectorToAngle(new Vector3(dir.x, dir.z));
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x00175824 File Offset: 0x00173A24
	public static float VectorToAngle(Vector2 dir)
	{
		if (dir.x == 0f)
		{
			if (dir.y > 0f)
			{
				return 90f;
			}
			if (dir.y < 0f)
			{
				return 270f;
			}
			return 0f;
		}
		else
		{
			if (dir.y != 0f)
			{
				float num = Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);
				float num2 = Mathf.Asin(dir.y / num) * 57.29578f;
				if (dir.y > 0f)
				{
					if (dir.x < 0f)
					{
						num2 = 180f - num2;
					}
				}
				else
				{
					if (dir.x > 0f)
					{
						num2 = 360f + num2;
					}
					if (dir.x < 0f)
					{
						num2 = 180f - num2;
					}
				}
				if (num2 == 360f)
				{
					num2 = 0f;
				}
				return num2;
			}
			if (dir.x > 0f)
			{
				return 0f;
			}
			if (dir.x < 0f)
			{
				return 180f;
			}
			return 0f;
		}
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x00175968 File Offset: 0x00173B68
	public static Vector3 GetWorldScale(Transform transform)
	{
		Vector3 vector = transform.localScale;
		Transform parent = transform.parent;
		while (parent != null)
		{
			vector = Vector3.Scale(vector, parent.localScale);
			parent = parent.parent;
		}
		return vector;
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x001759AC File Offset: 0x00173BAC
	public static void DestroyColliderRecursively(Transform root)
	{
		foreach (object obj in root)
		{
			Transform transform = (Transform)obj;
			if (transform.collider != null)
			{
				Object.Destroy(transform.collider);
			}
			Utility.DestroyColliderRecursively(transform);
		}
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x00175A28 File Offset: 0x00173C28
	public static void DisableColliderRecursively(Transform root)
	{
		foreach (object obj in root)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.collider != null)
			{
				transform.gameObject.collider.enabled = false;
			}
			Utility.DisableColliderRecursively(transform);
		}
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x00175AAC File Offset: 0x00173CAC
	public static void SetMat2DiffuseRecursively(Transform root)
	{
		foreach (object obj in root)
		{
			Transform transform = (Transform)obj;
			if (transform.renderer != null)
			{
				foreach (Material material in transform.renderer.materials)
				{
					material.shader = Shader.Find("Diffuse");
				}
			}
			Utility.SetMat2DiffuseRecursively(transform);
		}
	}

	// Token: 0x06003024 RID: 12324 RVA: 0x00175B58 File Offset: 0x00173D58
	public static void SetMat2AdditiveRecursively2(Transform root)
	{
		foreach (object obj in root)
		{
			Transform transform = (Transform)obj;
			if (transform.renderer != null)
			{
				foreach (Material material in transform.renderer.materials)
				{
					material.shader = Shader.Find("Particles/Additive");
				}
			}
			Utility.SetMat2AdditiveRecursively(transform);
		}
	}

	// Token: 0x06003025 RID: 12325 RVA: 0x00175C04 File Offset: 0x00173E04
	public static void DebugLog(MatShaderList list)
	{
		for (int i = 0; i < list.shaders.Count; i++)
		{
			Debug.Log(list.mats[i] + "   " + list.shaders[i]);
		}
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x0001E6DA File Offset: 0x0001C8DA
	public static MatShaderList SetMat2AdditiveRecursively(Transform root)
	{
		return Utility.SetMat2AdditiveRecursively(root, null);
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x00175C54 File Offset: 0x00173E54
	public static MatShaderList SetMat2AdditiveRecursively(Transform root, MatShaderList list)
	{
		if (list == null)
		{
			list = new MatShaderList();
		}
		foreach (object obj in root)
		{
			Transform transform = (Transform)obj;
			if (transform.renderer != null)
			{
				foreach (Material material in transform.renderer.materials)
				{
					list.Add(material, material.shader);
					material.shader = Shader.Find("Particles/Additive");
				}
			}
			list = Utility.SetMat2AdditiveRecursively(transform, list);
		}
		return list;
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x00175D1C File Offset: 0x00173F1C
	public static void ResetMatRecursively(Transform root, MatShaderList list, int num = 0)
	{
		foreach (object obj in root)
		{
			Transform transform = (Transform)obj;
			if (transform.renderer != null)
			{
				foreach (Material material in transform.renderer.materials)
				{
					for (int j = 0; j < list.mats.Count; j++)
					{
						if (material == list.mats[j])
						{
							material.shader = list.shaders[j];
							list.RemoveAt(j);
							break;
						}
					}
					num++;
				}
			}
			Utility.ResetMatRecursively(transform, list, num);
		}
	}

	// Token: 0x06003029 RID: 12329 RVA: 0x00175E18 File Offset: 0x00174018
	public static void SetAdditiveMatColorRecursively(Transform root, Color color)
	{
		foreach (object obj in root)
		{
			Transform transform = (Transform)obj;
			if (transform.renderer != null)
			{
				foreach (Material material in transform.renderer.materials)
				{
					material.SetColor("_TintColor", color);
				}
			}
			Utility.SetAdditiveMatColorRecursively(transform, color);
		}
	}
}
