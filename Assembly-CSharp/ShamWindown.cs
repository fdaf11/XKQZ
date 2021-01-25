using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x0200012B RID: 299
public class ShamWindown : MonoBehaviour
{
	// Token: 0x0600061A RID: 1562
	[DllImport("user32.dll")]
	private static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

	// Token: 0x0600061B RID: 1563
	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

	// Token: 0x0600061C RID: 1564
	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	// Token: 0x0600061D RID: 1565 RVA: 0x00043DFC File Offset: 0x00041FFC
	private void Awake()
	{
		this.screenPosition.x = (float)((int)(((float)Screen.currentResolution.width - this.screenPosition.width) / 2f));
		this.screenPosition.y = (float)((int)(((float)Screen.currentResolution.height - this.screenPosition.height) / 2f));
		if (Screen.currentResolution.height <= 768)
		{
			this.screenPosition.y = 0f;
		}
		ShamWindown.SetWindowLong(ShamWindown.GetForegroundWindow(), -16, 1);
		bool flag = ShamWindown.SetWindowPos(ShamWindown.GetForegroundWindow(), 0, (int)this.screenPosition.x, (int)this.screenPosition.y, (int)this.screenPosition.width, (int)this.screenPosition.height, 64U);
	}

	// Token: 0x04000697 RID: 1687
	private const uint SWP_SHOWWINDOW = 64U;

	// Token: 0x04000698 RID: 1688
	private const int GWL_STYLE = -16;

	// Token: 0x04000699 RID: 1689
	private const int WS_BORDER = 1;

	// Token: 0x0400069A RID: 1690
	public Rect screenPosition;
}
