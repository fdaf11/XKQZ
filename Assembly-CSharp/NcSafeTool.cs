using System;
using UnityEngine;

// Token: 0x020003FF RID: 1023
public class NcSafeTool : MonoBehaviour
{
	// Token: 0x060018B6 RID: 6326 RVA: 0x000C922C File Offset: 0x000C742C
	private static void Instance()
	{
		if (NcSafeTool.s_Instance == null)
		{
			GameObject gameObject = GameObject.Find("_GlobalManager");
			if (gameObject == null)
			{
				gameObject = new GameObject("_GlobalManager");
			}
			else
			{
				NcSafeTool.s_Instance = (NcSafeTool)gameObject.GetComponent(typeof(NcSafeTool));
			}
			if (NcSafeTool.s_Instance == null)
			{
				NcSafeTool.s_Instance = (NcSafeTool)gameObject.AddComponent(typeof(NcSafeTool));
			}
		}
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x00010020 File Offset: 0x0000E220
	public static bool IsSafe()
	{
		return !NcSafeTool.m_bShuttingDown && !NcSafeTool.m_bLoadLevel;
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x00010037 File Offset: 0x0000E237
	public static Object SafeInstantiate(Object original)
	{
		if (NcSafeTool.m_bShuttingDown)
		{
			return null;
		}
		if (NcSafeTool.s_Instance == null)
		{
			NcSafeTool.Instance();
		}
		return Object.Instantiate(original);
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x00010060 File Offset: 0x0000E260
	public static Object SafeInstantiate(Object original, Vector3 position, Quaternion rotation)
	{
		if (NcSafeTool.m_bShuttingDown)
		{
			return null;
		}
		if (NcSafeTool.s_Instance == null)
		{
			NcSafeTool.Instance();
		}
		return Object.Instantiate(original, position, rotation);
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x000C92B4 File Offset: 0x000C74B4
	public static void LoadLevel(int nLoadLevel)
	{
		if (NcSafeTool.m_bShuttingDown)
		{
			return;
		}
		if (NcSafeTool.s_Instance == null)
		{
			NcSafeTool.Instance();
		}
		NcSafeTool.m_bLoadLevel = true;
		Debug.Log("Safe LoadLevel start " + nLoadLevel);
		Application.LoadLevel(nLoadLevel);
		Debug.Log("Safe LoadLevel end");
		NcSafeTool.m_bLoadLevel = false;
	}

	// Token: 0x060018BB RID: 6331 RVA: 0x0001008B File Offset: 0x0000E28B
	public void OnApplicationQuit()
	{
		NcSafeTool.m_bShuttingDown = true;
	}

	// Token: 0x04001D1F RID: 7455
	public static bool m_bShuttingDown;

	// Token: 0x04001D20 RID: 7456
	public static bool m_bLoadLevel;

	// Token: 0x04001D21 RID: 7457
	private static NcSafeTool s_Instance;
}
