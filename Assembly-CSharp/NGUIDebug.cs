using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200049E RID: 1182
[AddComponentMenu("NGUI/Internal/Debug")]
public class NGUIDebug : MonoBehaviour
{
	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06001CAB RID: 7339 RVA: 0x0001310B File Offset: 0x0001130B
	// (set) Token: 0x06001CAC RID: 7340 RVA: 0x00013112 File Offset: 0x00011312
	public static bool debugRaycast
	{
		get
		{
			return NGUIDebug.mRayDebug;
		}
		set
		{
			if (Application.isPlaying)
			{
				NGUIDebug.mRayDebug = value;
				if (value)
				{
					NGUIDebug.CreateInstance();
				}
			}
		}
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x000DE200 File Offset: 0x000DC400
	public static void CreateInstance()
	{
		if (NGUIDebug.mInstance == null)
		{
			GameObject gameObject = new GameObject("_NGUI Debug");
			NGUIDebug.mInstance = gameObject.AddComponent<NGUIDebug>();
			Object.DontDestroyOnLoad(gameObject);
		}
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x000DE23C File Offset: 0x000DC43C
	private static void LogString(string text)
	{
		if (Application.isPlaying)
		{
			if (NGUIDebug.mLines.Count > 20)
			{
				NGUIDebug.mLines.RemoveAt(0);
			}
			NGUIDebug.mLines.Add(text);
			NGUIDebug.CreateInstance();
		}
		else
		{
			Debug.Log(text);
		}
	}

	// Token: 0x06001CAF RID: 7343 RVA: 0x000DE28C File Offset: 0x000DC48C
	public static void Log(params object[] objs)
	{
		string text = string.Empty;
		for (int i = 0; i < objs.Length; i++)
		{
			if (i == 0)
			{
				text += objs[i].ToString();
			}
			else
			{
				text = text + ", " + objs[i].ToString();
			}
		}
		NGUIDebug.LogString(text);
	}

	// Token: 0x06001CB0 RID: 7344 RVA: 0x0001312F File Offset: 0x0001132F
	public static void Clear()
	{
		NGUIDebug.mLines.Clear();
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x000DE2E8 File Offset: 0x000DC4E8
	public static void DrawBounds(Bounds b)
	{
		Vector3 center = b.center;
		Vector3 vector = b.center - b.extents;
		Vector3 vector2 = b.center + b.extents;
		Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector2.x, vector.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector.x, vector2.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector2.x, vector.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector.x, vector2.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.red);
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x000DE420 File Offset: 0x000DC620
	private void OnGUI()
	{
		if (NGUIDebug.mLines.Count == 0)
		{
			if (NGUIDebug.mRayDebug && UICamera.hoveredObject != null && Application.isPlaying)
			{
				GUILayout.Label("Last Hit: " + NGUITools.GetHierarchy(UICamera.hoveredObject).Replace("\"", string.Empty), new GUILayoutOption[0]);
			}
		}
		else
		{
			int i = 0;
			int count = NGUIDebug.mLines.Count;
			while (i < count)
			{
				GUILayout.Label(NGUIDebug.mLines[i], new GUILayoutOption[0]);
				i++;
			}
		}
	}

	// Token: 0x04002157 RID: 8535
	private static bool mRayDebug = false;

	// Token: 0x04002158 RID: 8536
	private static List<string> mLines = new List<string>();

	// Token: 0x04002159 RID: 8537
	private static NGUIDebug mInstance = null;
}
