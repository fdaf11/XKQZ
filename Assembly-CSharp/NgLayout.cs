using System;
using UnityEngine;

// Token: 0x020003F5 RID: 1013
public class NgLayout
{
	// Token: 0x0600182E RID: 6190 RVA: 0x0000FC80 File Offset: 0x0000DE80
	public static Rect GetZeroRect()
	{
		return new Rect(0f, 0f, 0f, 0f);
	}

	// Token: 0x0600182F RID: 6191 RVA: 0x000C6EA0 File Offset: 0x000C50A0
	public static Rect GetSumRect(Rect rect1, Rect rect2)
	{
		return NgLayout.GetOffsetRect(rect1, Mathf.Min(0f, rect2.xMin - rect1.xMin), Mathf.Min(0f, rect2.yMin - rect1.yMin), Mathf.Max(0f, rect2.xMax - rect1.xMax), Mathf.Max(0f, rect2.yMax - rect1.yMax));
	}

	// Token: 0x06001830 RID: 6192 RVA: 0x0000FC9B File Offset: 0x0000DE9B
	public static Rect GetOffsetRect(Rect rect, float left, float top)
	{
		return new Rect(rect.x + left, rect.y + top, rect.width, rect.height);
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x0000FCC2 File Offset: 0x0000DEC2
	public static Rect GetOffsetRect(Rect rect, float left, float top, float right, float bottom)
	{
		return new Rect(rect.x + left, rect.y + top, rect.width - left + right, rect.height - top + bottom);
	}

	// Token: 0x06001832 RID: 6194 RVA: 0x0000FCF2 File Offset: 0x0000DEF2
	public static Rect GetOffsetRect(Rect rect, float fOffset)
	{
		return new Rect(rect.x - fOffset, rect.y - fOffset, rect.width + fOffset * 2f, rect.height + fOffset * 2f);
	}

	// Token: 0x06001833 RID: 6195 RVA: 0x0000FD29 File Offset: 0x0000DF29
	public static Rect GetVOffsetRect(Rect rect, float fOffset)
	{
		return new Rect(rect.x, rect.y - fOffset, rect.width, rect.height + fOffset * 2f);
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x0000FD56 File Offset: 0x0000DF56
	public static Rect GetHOffsetRect(Rect rect, float fOffset)
	{
		return new Rect(rect.x - fOffset, rect.y, rect.width + fOffset * 2f, rect.height);
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x000C6F18 File Offset: 0x000C5118
	public static Rect GetOffsetRateRect(Rect rect, float fOffsetRate)
	{
		return new Rect(rect.x - Mathf.Abs(rect.x) * fOffsetRate, rect.y - Mathf.Abs(rect.y) * fOffsetRate, rect.width + Mathf.Abs(rect.x) * fOffsetRate * 2f, rect.height + Mathf.Abs(rect.y) * fOffsetRate * 2f);
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x0000FD83 File Offset: 0x0000DF83
	public static Rect GetZeroStartRect(Rect rect)
	{
		return new Rect(0f, 0f, rect.width, rect.height);
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x0000FDA2 File Offset: 0x0000DFA2
	public static Rect GetLeftRect(Rect rect, float width)
	{
		return new Rect(rect.x, rect.y, width, rect.height);
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x0000FDBF File Offset: 0x0000DFBF
	public static Rect GetRightRect(Rect rect, float width)
	{
		return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x0000FDE6 File Offset: 0x0000DFE6
	public static Rect GetInnerTopRect(Rect rectBase, int topMargin, int nHeight)
	{
		return new Rect(rectBase.x, (float)topMargin + rectBase.y, rectBase.width, (float)nHeight);
	}

	// Token: 0x0600183A RID: 6202 RVA: 0x0000FE07 File Offset: 0x0000E007
	public static Rect GetInnerBottomRect(Rect rectBase, int nHeight)
	{
		return new Rect(rectBase.x, rectBase.y + rectBase.height - (float)nHeight, rectBase.width, (float)nHeight);
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x000C6F90 File Offset: 0x000C5190
	public static Vector2 ClampPoint(Rect rect, Vector2 point)
	{
		if (point.x < rect.xMin)
		{
			point.x = rect.xMin;
		}
		if (point.y < rect.yMin)
		{
			point.y = rect.yMin;
		}
		if (rect.xMax < point.x)
		{
			point.x = rect.xMax;
		}
		if (rect.yMax < point.y)
		{
			point.y = rect.yMax;
		}
		return point;
	}

	// Token: 0x0600183C RID: 6204 RVA: 0x000C7024 File Offset: 0x000C5224
	public static Vector3 ClampPoint(Rect rect, Vector3 point)
	{
		if (point.x < rect.xMin)
		{
			point.x = rect.xMin;
		}
		if (point.y < rect.yMin)
		{
			point.y = rect.yMin;
		}
		if (rect.xMax < point.x)
		{
			point.x = rect.xMax;
		}
		if (rect.yMax < point.y)
		{
			point.y = rect.yMax;
		}
		return point;
	}

	// Token: 0x0600183D RID: 6205 RVA: 0x000C70B8 File Offset: 0x000C52B8
	public static Rect ClampWindow(Rect popupRect)
	{
		if (popupRect.y < 0f)
		{
			popupRect.y = 0f;
		}
		if ((float)Screen.width < popupRect.xMax)
		{
			popupRect.x -= popupRect.xMax - (float)Screen.width;
		}
		if ((float)Screen.height < popupRect.yMax)
		{
			popupRect.y -= popupRect.yMax - (float)Screen.height;
		}
		return popupRect;
	}

	// Token: 0x0600183E RID: 6206 RVA: 0x000C7140 File Offset: 0x000C5340
	public static bool GUIToggle(Rect pos, bool bToggle, GUIContent content, bool bEnable)
	{
		bool enabled = GUI.enabled;
		if (!bEnable)
		{
			GUI.enabled = false;
		}
		bToggle = GUI.Toggle(pos, bToggle, content);
		GUI.enabled = enabled;
		return bToggle;
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x000C7170 File Offset: 0x000C5370
	public static bool GUIButton(Rect pos, string name, bool bEnable)
	{
		bool enabled = GUI.enabled;
		if (!bEnable)
		{
			GUI.enabled = false;
		}
		bool result = GUI.Button(pos, name);
		GUI.enabled = enabled;
		return result;
	}

	// Token: 0x06001840 RID: 6208 RVA: 0x000C71A0 File Offset: 0x000C53A0
	public static bool GUIButton(Rect pos, GUIContent content, bool bEnable)
	{
		bool enabled = GUI.enabled;
		if (!bEnable)
		{
			GUI.enabled = false;
		}
		bool result = GUI.Button(pos, content);
		GUI.enabled = enabled;
		return result;
	}

	// Token: 0x06001841 RID: 6209 RVA: 0x000C71D0 File Offset: 0x000C53D0
	public static bool GUIButton(Rect pos, GUIContent content, GUIStyle style, bool bEnable)
	{
		bool enabled = GUI.enabled;
		if (!bEnable)
		{
			GUI.enabled = false;
		}
		bool result = GUI.Button(pos, content, style);
		GUI.enabled = enabled;
		return result;
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x000C7200 File Offset: 0x000C5400
	public static string GUITextField(Rect pos, string name, bool bEnable)
	{
		bool enabled = GUI.enabled;
		if (!bEnable)
		{
			GUI.enabled = false;
		}
		string result = GUI.TextField(pos, name);
		GUI.enabled = enabled;
		return result;
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x0000FE30 File Offset: 0x0000E030
	public static bool GUIEnableBackup(bool newEnable)
	{
		NgLayout.m_GuiOldEnable = GUI.enabled;
		GUI.enabled = newEnable;
		return NgLayout.m_GuiOldEnable;
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x0000FE47 File Offset: 0x0000E047
	public static void GUIEnableRestore()
	{
		GUI.enabled = NgLayout.m_GuiOldEnable;
	}

	// Token: 0x06001845 RID: 6213 RVA: 0x0000FE53 File Offset: 0x0000E053
	public static Color GUIColorBackup(Color newColor)
	{
		NgLayout.m_GuiOldColor = GUI.color;
		GUI.color = newColor;
		return NgLayout.m_GuiOldColor;
	}

	// Token: 0x06001846 RID: 6214 RVA: 0x0000FE6A File Offset: 0x0000E06A
	public static void GUIColorRestore()
	{
		GUI.color = NgLayout.m_GuiOldColor;
	}

	// Token: 0x06001847 RID: 6215 RVA: 0x000C7230 File Offset: 0x000C5430
	public static Vector2 GetGUIMousePosition()
	{
		return new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
	}

	// Token: 0x06001848 RID: 6216 RVA: 0x000C7264 File Offset: 0x000C5464
	public static float GetWorldPerScreenPixel(Vector3 worldPoint)
	{
		Camera main = Camera.main;
		if (main == null)
		{
			return 0f;
		}
		Plane plane;
		plane..ctor(main.transform.forward, main.transform.position);
		float distanceToPoint = plane.GetDistanceToPoint(worldPoint);
		float num = 100f;
		return Vector3.Distance(main.ScreenToWorldPoint(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2) - num / 2f, distanceToPoint)), main.ScreenToWorldPoint(new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2) + num / 2f, distanceToPoint))) / num;
	}

	// Token: 0x06001849 RID: 6217 RVA: 0x000C7304 File Offset: 0x000C5504
	public static Vector3 GetScreenToWorld(Vector3 targetWorld, Vector2 screenPos)
	{
		Camera main = Camera.main;
		if (main == null)
		{
			return Vector3.zero;
		}
		Plane plane;
		plane..ctor(main.transform.forward, main.transform.position);
		float distanceToPoint = plane.GetDistanceToPoint(targetWorld);
		return main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, distanceToPoint));
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x000C736C File Offset: 0x000C556C
	public static Vector3 GetWorldToScreen(Vector3 targetWorld)
	{
		Camera main = Camera.main;
		if (main == null)
		{
			return Vector3.zero;
		}
		return main.WorldToScreenPoint(targetWorld);
	}

	// Token: 0x04001D0A RID: 7434
	protected static Color m_GuiOldColor;

	// Token: 0x04001D0B RID: 7435
	protected static bool m_GuiOldEnable;
}
