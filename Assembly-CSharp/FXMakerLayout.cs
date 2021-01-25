using System;
using UnityEngine;

// Token: 0x0200040C RID: 1036
public class FXMakerLayout : NgLayout
{
	// Token: 0x06001935 RID: 6453 RVA: 0x00010649 File Offset: 0x0000E849
	public static float GetFixedWindowWidth()
	{
		return 115f;
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x00010650 File Offset: 0x0000E850
	public static float GetTopMenuHeight()
	{
		return (!FXMakerLayout.m_bMinimizeAll && !FXMakerLayout.m_bMinimizeTopMenu) ? 92f : FXMakerLayout.m_MinimizeHeight;
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x0000E400 File Offset: 0x0000C600
	public static int GetWindowId(FXMakerLayout.WINDOWID id)
	{
		return (int)id;
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x00010675 File Offset: 0x0000E875
	public static Rect GetChildTopRect(Rect rectParent, int topMargin, int nHeight)
	{
		return new Rect(FXMakerLayout.m_rectInnerMargin.x, (float)topMargin + FXMakerLayout.m_rectInnerMargin.y, rectParent.width - FXMakerLayout.m_rectInnerMargin.x - FXMakerLayout.m_rectInnerMargin.width, (float)nHeight);
	}

	// Token: 0x06001939 RID: 6457 RVA: 0x000CC680 File Offset: 0x000CA880
	public static Rect GetChildBottomRect(Rect rectParent, int nHeight)
	{
		return new Rect(FXMakerLayout.m_rectInnerMargin.x, rectParent.height - (float)nHeight - FXMakerLayout.m_rectInnerMargin.height, rectParent.width - FXMakerLayout.m_rectInnerMargin.x - FXMakerLayout.m_rectInnerMargin.width, (float)nHeight);
	}

	// Token: 0x0600193A RID: 6458 RVA: 0x000CC6D0 File Offset: 0x000CA8D0
	public static Rect GetChildVerticalRect(Rect rectParent, int topMargin, int count, int pos, int sumCount)
	{
		return new Rect(FXMakerLayout.m_rectInnerMargin.x, (float)topMargin + FXMakerLayout.m_rectInnerMargin.y + (rectParent.height - (float)topMargin - FXMakerLayout.m_rectInnerMargin.y - FXMakerLayout.m_rectInnerMargin.height) / (float)count * (float)pos, rectParent.width - FXMakerLayout.m_rectInnerMargin.x - FXMakerLayout.m_rectInnerMargin.width, (rectParent.height - (float)topMargin - FXMakerLayout.m_rectInnerMargin.y - FXMakerLayout.m_rectInnerMargin.height) / (float)count * (float)sumCount - FXMakerLayout.m_fButtonMargin);
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x000CC76C File Offset: 0x000CA96C
	public static Rect GetInnerVerticalRect(Rect rectBase, int count, int pos, int sumCount)
	{
		return new Rect(rectBase.x, rectBase.y + (rectBase.height + FXMakerLayout.m_fButtonMargin) / (float)count * (float)pos, rectBase.width, (rectBase.height + FXMakerLayout.m_fButtonMargin) / (float)count * (float)sumCount - FXMakerLayout.m_fButtonMargin);
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x000CC7C0 File Offset: 0x000CA9C0
	public static Rect GetChildHorizontalRect(Rect rectParent, int topMargin, int count, int pos, int sumCount)
	{
		return new Rect(FXMakerLayout.m_rectInnerMargin.x + (rectParent.width - FXMakerLayout.m_rectInnerMargin.x - FXMakerLayout.m_rectInnerMargin.width) / (float)count * (float)pos, (float)topMargin + FXMakerLayout.m_rectInnerMargin.y, (rectParent.width - FXMakerLayout.m_rectInnerMargin.x - FXMakerLayout.m_rectInnerMargin.width) / (float)count * (float)sumCount - FXMakerLayout.m_fButtonMargin, rectParent.height - FXMakerLayout.m_rectInnerMargin.y - FXMakerLayout.m_rectInnerMargin.height);
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x000CC854 File Offset: 0x000CAA54
	public static Rect GetInnerHorizontalRect(Rect rectBase, int count, int pos, int sumCount)
	{
		return new Rect(rectBase.x + (rectBase.width + FXMakerLayout.m_fButtonMargin) / (float)count * (float)pos, rectBase.y, (rectBase.width + FXMakerLayout.m_fButtonMargin) / (float)count * (float)sumCount - FXMakerLayout.m_fButtonMargin, rectBase.height);
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x000106B2 File Offset: 0x0000E8B2
	public static Rect GetMenuChangeRect()
	{
		return new Rect(FXMakerLayout.m_rectOuterMargin.x, FXMakerLayout.m_rectOuterMargin.y, FXMakerLayout.GetFixedWindowWidth(), FXMakerLayout.GetTopMenuHeight());
	}

	// Token: 0x0600193F RID: 6463 RVA: 0x000CC8A8 File Offset: 0x000CAAA8
	public static Rect GetMenuToolbarRect()
	{
		return new Rect(FXMakerLayout.GetMenuChangeRect().xMax + FXMakerLayout.m_rectOuterMargin.x, FXMakerLayout.m_rectOuterMargin.y, (float)Screen.width - FXMakerLayout.GetMenuChangeRect().width - FXMakerLayout.GetMenuTopRightRect().width - FXMakerLayout.m_rectOuterMargin.x * 4f, FXMakerLayout.GetTopMenuHeight());
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x000106D7 File Offset: 0x0000E8D7
	public static Rect GetMenuTopRightRect()
	{
		return new Rect((float)Screen.width - FXMakerLayout.GetFixedWindowWidth() - FXMakerLayout.m_rectOuterMargin.x, FXMakerLayout.m_rectOuterMargin.y, FXMakerLayout.GetFixedWindowWidth(), FXMakerLayout.GetTopMenuHeight());
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x000CC914 File Offset: 0x000CAB14
	public static Rect GetResListRect(int nIndex)
	{
		return new Rect(FXMakerLayout.m_rectOuterMargin.x + (FXMakerLayout.GetFixedWindowWidth() + FXMakerLayout.m_rectOuterMargin.x) * (float)nIndex, FXMakerLayout.GetMenuChangeRect().yMax + FXMakerLayout.m_rectOuterMargin.y, FXMakerLayout.GetFixedWindowWidth(), (float)Screen.height - FXMakerLayout.GetMenuChangeRect().yMax - FXMakerLayout.m_rectOuterMargin.y * 2f);
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x000CC988 File Offset: 0x000CAB88
	public static Rect GetEffectListRect()
	{
		return new Rect(FXMakerLayout.m_rectOuterMargin.x, FXMakerLayout.GetMenuChangeRect().yMax + FXMakerLayout.m_rectOuterMargin.y, FXMakerLayout.GetFixedWindowWidth() * (float)FXMakerLayout.m_nSidewindowWidthCount + FXMakerLayout.m_rectOuterMargin.x, (float)Screen.height - FXMakerLayout.GetMenuChangeRect().yMax - FXMakerLayout.m_rectOuterMargin.y * 2f);
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x000CC9F8 File Offset: 0x000CABF8
	public static Rect GetEffectHierarchyRect()
	{
		return new Rect((float)Screen.width - (FXMakerLayout.GetFixedWindowWidth() + FXMakerLayout.m_rectOuterMargin.x) * (float)FXMakerLayout.m_nSidewindowWidthCount, FXMakerLayout.GetMenuChangeRect().yMax + FXMakerLayout.m_rectOuterMargin.y, FXMakerLayout.GetFixedWindowWidth() * (float)FXMakerLayout.m_nSidewindowWidthCount + FXMakerLayout.m_rectOuterMargin.x, (float)Screen.height - FXMakerLayout.GetMenuChangeRect().yMax - FXMakerLayout.m_rectOuterMargin.y * 2f);
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x000CCA7C File Offset: 0x000CAC7C
	public static Rect GetActionToolbarRect()
	{
		return new Rect(FXMakerLayout.m_rectOuterMargin.x * 3f + FXMakerLayout.GetFixedWindowWidth() * (float)FXMakerLayout.m_nSidewindowWidthCount, (float)Screen.height - FXMakerLayout.m_fActionToolbarHeight - FXMakerLayout.m_rectOuterMargin.y, (float)Screen.width - FXMakerLayout.GetMenuChangeRect().width * 4f - FXMakerLayout.m_rectOuterMargin.x * 6f, FXMakerLayout.m_fActionToolbarHeight);
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x000CCAF4 File Offset: 0x000CACF4
	public static Rect GetToolMessageRect()
	{
		return new Rect(FXMakerLayout.GetFixedWindowWidth() * 2.1f, (float)Screen.height - FXMakerLayout.m_fActionToolbarHeight - FXMakerLayout.m_rectOuterMargin.y - FXMakerLayout.m_fToolMessageHeight - FXMakerLayout.m_fTooltipHeight, (float)Screen.width - FXMakerLayout.GetFixedWindowWidth() * (float)FXMakerLayout.m_nSidewindowWidthCount * 2f - FXMakerLayout.m_rectOuterMargin.x * 2f - FXMakerLayout.m_fTestPanelWidth, FXMakerLayout.m_fToolMessageHeight);
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x000CCB6C File Offset: 0x000CAD6C
	public static Rect GetTooltipRect()
	{
		return new Rect(FXMakerLayout.m_rectOuterMargin.x * 3f + FXMakerLayout.GetFixedWindowWidth() * (float)FXMakerLayout.m_nSidewindowWidthCount, (float)Screen.height - FXMakerLayout.m_fActionToolbarHeight - FXMakerLayout.m_rectOuterMargin.y - FXMakerLayout.m_fTooltipHeight, (float)Screen.width - FXMakerLayout.GetMenuChangeRect().width * 4f - FXMakerLayout.m_rectOuterMargin.x * 6f - FXMakerLayout.m_fTestPanelWidth, FXMakerLayout.m_fTooltipHeight);
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x000CCBF0 File Offset: 0x000CADF0
	public static Rect GetCursorTooltipRect(Vector2 size)
	{
		return NgLayout.ClampWindow(new Rect(Input.mousePosition.x + 15f, (float)Screen.height - Input.mousePosition.y + 80f, size.x, size.y));
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x000CCC44 File Offset: 0x000CAE44
	public static Rect GetModalMessageRect()
	{
		return new Rect(((float)Screen.width - FXMakerLayout.m_fModalMessageWidth) / 2f, ((float)Screen.height - FXMakerLayout.m_fModalMessageHeight - FXMakerLayout.m_fModalMessageHeight / 8f) / 2f, FXMakerLayout.m_fModalMessageWidth, FXMakerLayout.m_fModalMessageHeight);
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x00010709 File Offset: 0x0000E909
	public static Rect GetMenuGizmoRect()
	{
		return new Rect(FXMakerLayout.m_rectOuterMargin.x * 3f + FXMakerLayout.GetFixedWindowWidth() * (float)FXMakerLayout.m_nSidewindowWidthCount, FXMakerLayout.GetTopMenuHeight() + FXMakerLayout.m_rectOuterMargin.y, 490f, 26f);
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x000CCC90 File Offset: 0x000CAE90
	public static Rect GetMenuTestPanelRect()
	{
		return new Rect((float)Screen.width - FXMakerLayout.GetFixedWindowWidth() * 2f - FXMakerLayout.m_fTestPanelWidth - FXMakerLayout.m_rectOuterMargin.x * 2f, (float)Screen.height - FXMakerLayout.m_fActionToolbarHeight - FXMakerLayout.m_rectOuterMargin.y - FXMakerLayout.m_fTestPanelHeight, FXMakerLayout.m_fTestPanelWidth, FXMakerLayout.m_fTestPanelHeight);
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x000CCCF4 File Offset: 0x000CAEF4
	public static Rect GetClientRect()
	{
		return new Rect(FXMakerLayout.m_rectOuterMargin.x * 3f + FXMakerLayout.GetFixedWindowWidth() * (float)FXMakerLayout.m_nSidewindowWidthCount, FXMakerLayout.GetTopMenuHeight() + FXMakerLayout.m_rectOuterMargin.y, (float)Screen.width - (FXMakerLayout.m_rectOuterMargin.x * 3f + FXMakerLayout.GetFixedWindowWidth() * (float)FXMakerLayout.m_nSidewindowWidthCount) * 2f, (float)Screen.height - FXMakerLayout.m_fActionToolbarHeight - FXMakerLayout.m_rectOuterMargin.y * 3f - FXMakerLayout.GetTopMenuHeight());
	}

	// Token: 0x0600194C RID: 6476 RVA: 0x00010747 File Offset: 0x0000E947
	public static Rect GetScrollViewRect(int nWidth, int nButtonCount, int nColumn)
	{
		return new Rect(0f, 0f, (float)(nWidth - 2), FXMakerLayout.m_fScrollButtonHeight * (float)(nButtonCount / nColumn + ((0 >= nButtonCount % nColumn) ? 0 : 1)) + 25f);
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x0001077D File Offset: 0x0000E97D
	public static Rect GetScrollGridRect(int nWidth, int nButtonCount, int nColumn)
	{
		return new Rect(0f, 0f, (float)(nWidth - 2), FXMakerLayout.m_fScrollButtonHeight * (float)(nButtonCount / nColumn + ((0 >= nButtonCount % nColumn) ? 0 : 1)));
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x000CCD80 File Offset: 0x000CAF80
	public static Rect GetAspectScrollViewRect(int nWidth, float fAspect, int nButtonCount, int nColumn, bool bImageOnly)
	{
		return new Rect(0f, 0f, (float)(nWidth - 4), ((float)((nWidth - 4) / nColumn) * fAspect + (float)((!bImageOnly) ? 10 : 0)) * (float)(nButtonCount / nColumn + ((0 >= nButtonCount % nColumn) ? 0 : 1)) + 25f);
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x000CCDD8 File Offset: 0x000CAFD8
	public static Rect GetAspectScrollGridRect(int nWidth, float fAspect, int nButtonCount, int nColumn, bool bImageOnly)
	{
		return new Rect(0f, 0f, (float)(nWidth - 4), ((float)((nWidth - 4) / nColumn) * fAspect + (float)((!bImageOnly) ? 10 : 0)) * (float)(nButtonCount / nColumn + ((0 >= nButtonCount % nColumn) ? 0 : 1)));
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x000CCE28 File Offset: 0x000CB028
	public static KeyCode GetVaildInputKey(KeyCode key, bool bPress)
	{
		if (bPress || FXMakerLayout.m_fKeyLastTime + FXMakerLayout.m_fArrowIntervalRepeatTime * Time.timeScale < Time.time)
		{
			FXMakerLayout.m_fKeyLastTime = ((!bPress) ? Time.time : (Time.time + FXMakerLayout.m_fArrowIntervalStartTime));
			return key;
		}
		return 0;
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x000CCE7C File Offset: 0x000CB07C
	public static int GetGridHoverIndex(Rect windowRect, Rect listRect, Rect gridRect, int nCount, int nColumn, GUIStyle style)
	{
		int num = (style != null) ? style.margin.left : 0;
		int num2 = nCount / nColumn + ((0 >= nCount % nColumn) ? 0 : 1);
		float num3 = gridRect.width / (float)nColumn;
		float num4 = gridRect.height / (float)num2;
		Vector2 vector = NgLayout.GetGUIMousePosition() - new Vector2(windowRect.x, windowRect.y);
		if (!listRect.Contains(vector))
		{
			return -1;
		}
		for (int i = 0; i < nCount; i++)
		{
			Rect rect;
			rect..ctor(listRect.x + num3 * (float)(i % nColumn) + (float)num, listRect.y + num4 * (float)(i / nColumn) + (float)num, num3 - (float)(num * 2), num4 - (float)(num * 2));
			if (rect.Contains(vector))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x000107AD File Offset: 0x0000E9AD
	public static int TooltipToolbar(Rect windowRect, Rect gridRect, int nGridIndex, GUIContent[] cons)
	{
		return FXMakerLayout.TooltipToolbar(windowRect, gridRect, nGridIndex, cons, null);
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x000CCF64 File Offset: 0x000CB164
	public static int TooltipToolbar(Rect windowRect, Rect gridRect, int nGridIndex, GUIContent[] cons, GUIStyle style)
	{
		int result = GUI.Toolbar(gridRect, nGridIndex, cons, style);
		int gridHoverIndex = FXMakerLayout.GetGridHoverIndex(windowRect, gridRect, gridRect, cons.Length, cons.Length, null);
		if (0 <= gridHoverIndex)
		{
			GUI.tooltip = cons[gridHoverIndex].tooltip;
		}
		return result;
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x000107B9 File Offset: 0x0000E9B9
	public static int TooltipSelectionGrid(Rect windowRect, Rect listRect, int nGridIndex, GUIContent[] cons, int nColumCount)
	{
		return FXMakerLayout.TooltipSelectionGrid(windowRect, listRect, listRect, nGridIndex, cons, nColumCount, null);
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x000107B9 File Offset: 0x0000E9B9
	public static int TooltipSelectionGrid(Rect windowRect, Rect listRect, int nGridIndex, GUIContent[] cons, int nColumCount, GUIStyle style)
	{
		return FXMakerLayout.TooltipSelectionGrid(windowRect, listRect, listRect, nGridIndex, cons, nColumCount, null);
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x000107C8 File Offset: 0x0000E9C8
	public static int TooltipSelectionGrid(Rect windowRect, Rect listRect, Rect gridRect, int nGridIndex, GUIContent[] cons, int nColumCount)
	{
		return FXMakerLayout.TooltipSelectionGrid(windowRect, listRect, gridRect, nGridIndex, cons, nColumCount, null);
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x000CCFA4 File Offset: 0x000CB1A4
	public static int TooltipSelectionGrid(Rect windowRect, Rect listRect, Rect gridRect, int nGridIndex, GUIContent[] cons, int nColumCount, GUIStyle style)
	{
		int result = GUI.SelectionGrid(gridRect, nGridIndex, cons, nColumCount, style);
		int gridHoverIndex = FXMakerLayout.GetGridHoverIndex(windowRect, listRect, gridRect, cons.Length, nColumCount, null);
		if (0 <= gridHoverIndex)
		{
			GUI.tooltip = cons[gridHoverIndex].tooltip;
		}
		return result;
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x000CCFE8 File Offset: 0x000CB1E8
	public static void ModalWindow(Rect rect, GUI.WindowFunction func, string title)
	{
		GUI.Window(GUIUtility.GetControlID(2), rect, delegate(int id)
		{
			GUI.depth = 0;
			int controlID = GUIUtility.GetControlID(0);
			if (GUIUtility.hotControl < controlID)
			{
				FXMakerLayout.setHotControl(0);
			}
			func.Invoke(id);
			int controlID2 = GUIUtility.GetControlID(0);
			if (GUIUtility.hotControl < controlID || (GUIUtility.hotControl > controlID2 && controlID2 != -1))
			{
				FXMakerLayout.setHotControl(-1);
			}
			GUI.FocusWindow(id);
			GUI.BringWindowToFront(id);
		}, title);
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x000CD01C File Offset: 0x000CB21C
	private static void setHotControl(int id)
	{
		Rect rect;
		rect..ctor(0f, 0f, (float)Screen.width, (float)Screen.height);
		if (rect.Contains(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)))
		{
			GUIUtility.hotControl = id;
		}
	}

	// Token: 0x04001DA5 RID: 7589
	public const string m_CurrentVersion = "v1.5.0";

	// Token: 0x04001DA6 RID: 7590
	public const int m_nMaxResourceListCount = 100;

	// Token: 0x04001DA7 RID: 7591
	public const int m_nMaxPrefabListCount = 500;

	// Token: 0x04001DA8 RID: 7592
	public const int m_nMaxTextureListCount = 500;

	// Token: 0x04001DA9 RID: 7593
	public const int m_nMaxMaterialListCount = 1000;

	// Token: 0x04001DAA RID: 7594
	public const float m_fScreenShotEffectZoomRate = 1f;

	// Token: 0x04001DAB RID: 7595
	public const float m_fScreenShotBackZoomRate = 0.6f;

	// Token: 0x04001DAC RID: 7596
	public const float m_fScrollButtonAspect = 0.55f;

	// Token: 0x04001DAD RID: 7597
	public const float m_fReloadPreviewTime = 0.5f;

	// Token: 0x04001DAE RID: 7598
	public const int m_nThumbCaptureSize = 512;

	// Token: 0x04001DAF RID: 7599
	public const int m_nThumbImageSize = 128;

	// Token: 0x04001DB0 RID: 7600
	protected static float m_fFixedWindowWidth = -1f;

	// Token: 0x04001DB1 RID: 7601
	protected static float m_fTopMenuHeight = -1f;

	// Token: 0x04001DB2 RID: 7602
	protected static bool m_bLastStateTopMenuMini = false;

	// Token: 0x04001DB3 RID: 7603
	public static bool m_bDevelopState = false;

	// Token: 0x04001DB4 RID: 7604
	public static bool m_bDevelopPrefs = false;

	// Token: 0x04001DB5 RID: 7605
	public static Rect m_rectOuterMargin = new Rect(2f, 2f, 0f, 0f);

	// Token: 0x04001DB6 RID: 7606
	public static Rect m_rectInnerMargin = new Rect(7f, 19f, 7f, 4f);

	// Token: 0x04001DB7 RID: 7607
	public static int m_nSidewindowWidthCount = 2;

	// Token: 0x04001DB8 RID: 7608
	public static float m_fButtonMargin = 3f;

	// Token: 0x04001DB9 RID: 7609
	public static float m_fScrollButtonHeight = 70f;

	// Token: 0x04001DBA RID: 7610
	public static bool m_bMinimizeTopMenu = false;

	// Token: 0x04001DBB RID: 7611
	public static bool m_bMinimizeAll = false;

	// Token: 0x04001DBC RID: 7612
	public static float m_fMinimizeClickWidth = 60f;

	// Token: 0x04001DBD RID: 7613
	public static float m_fMinimizeClickHeight = 20f;

	// Token: 0x04001DBE RID: 7614
	public static float m_fOriActionToolbarHeight = 126f;

	// Token: 0x04001DBF RID: 7615
	public static float m_fActionToolbarHeight = FXMakerLayout.m_fOriActionToolbarHeight;

	// Token: 0x04001DC0 RID: 7616
	public static float m_MinimizeHeight = 43f;

	// Token: 0x04001DC1 RID: 7617
	public static float m_fToolMessageHeight = 50f;

	// Token: 0x04001DC2 RID: 7618
	public static float m_fTooltipHeight = 60f;

	// Token: 0x04001DC3 RID: 7619
	public static float m_fModalMessageWidth = 500f;

	// Token: 0x04001DC4 RID: 7620
	public static float m_fModalMessageHeight = 200f;

	// Token: 0x04001DC5 RID: 7621
	public static float m_fTestPanelWidth = 150f;

	// Token: 0x04001DC6 RID: 7622
	public static float m_fTestPanelHeight = 120f;

	// Token: 0x04001DC7 RID: 7623
	public static float m_fOriTestPanelHeight = FXMakerLayout.m_fTestPanelHeight;

	// Token: 0x04001DC8 RID: 7624
	public static Color m_ColorButtonHover = new Color(0.7f, 1f, 0.9f, 1f);

	// Token: 0x04001DC9 RID: 7625
	public static Color m_ColorButtonActive = new Color(1f, 1f, 0.6f, 1f);

	// Token: 0x04001DCA RID: 7626
	public static Color m_ColorButtonMatNormal = new Color(0.5f, 0.7f, 0.7f, 1f);

	// Token: 0x04001DCB RID: 7627
	public static Color m_ColorButtonUnityEngine = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x04001DCC RID: 7628
	public static Color m_ColorDropFocused = new Color(0.2f, 1f, 0.6f, 0.8f);

	// Token: 0x04001DCD RID: 7629
	public static Color m_ColorHelpBox = new Color(1f, 0.1f, 0.1f, 1f);

	// Token: 0x04001DCE RID: 7630
	protected static float m_fArrowIntervalStartTime = 0.2f;

	// Token: 0x04001DCF RID: 7631
	protected static float m_fArrowIntervalRepeatTime = 0.1f;

	// Token: 0x04001DD0 RID: 7632
	protected static float m_fKeyLastTime;

	// Token: 0x0200040D RID: 1037
	public enum WINDOWID
	{
		// Token: 0x04001DD2 RID: 7634
		NONE,
		// Token: 0x04001DD3 RID: 7635
		TOP_LEFT = 10,
		// Token: 0x04001DD4 RID: 7636
		TOP_CENTER,
		// Token: 0x04001DD5 RID: 7637
		TOP_RIGHT,
		// Token: 0x04001DD6 RID: 7638
		EFFECT_LIST,
		// Token: 0x04001DD7 RID: 7639
		EFFECT_HIERARCHY,
		// Token: 0x04001DD8 RID: 7640
		EFFECT_CONTROLS,
		// Token: 0x04001DD9 RID: 7641
		PANEL_TEST,
		// Token: 0x04001DDA RID: 7642
		TOOLIP_CURSOR,
		// Token: 0x04001DDB RID: 7643
		MODAL_MSG,
		// Token: 0x04001DDC RID: 7644
		MENUPOPUP,
		// Token: 0x04001DDD RID: 7645
		SPRITEPOPUP,
		// Token: 0x04001DDE RID: 7646
		POPUP = 100,
		// Token: 0x04001DDF RID: 7647
		RESOURCE_START = 200,
		// Token: 0x04001DE0 RID: 7648
		HINTRECT = 300
	}

	// Token: 0x0200040E RID: 1038
	public enum MODAL_TYPE
	{
		// Token: 0x04001DE2 RID: 7650
		MODAL_NONE,
		// Token: 0x04001DE3 RID: 7651
		MODAL_MSG,
		// Token: 0x04001DE4 RID: 7652
		MODAL_OK,
		// Token: 0x04001DE5 RID: 7653
		MODAL_YESNO,
		// Token: 0x04001DE6 RID: 7654
		MODAL_OKCANCEL
	}

	// Token: 0x0200040F RID: 1039
	public enum MODALRETURN_TYPE
	{
		// Token: 0x04001DE8 RID: 7656
		MODALRETURN_SHOW,
		// Token: 0x04001DE9 RID: 7657
		MODALRETURN_OK,
		// Token: 0x04001DEA RID: 7658
		MODALRETURN_CANCEL
	}
}
