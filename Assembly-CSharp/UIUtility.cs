using System;
using UnityEngine;

// Token: 0x020007C5 RID: 1989
public class UIUtility
{
	// Token: 0x060030D4 RID: 12500 RVA: 0x0001EDB2 File Offset: 0x0001CFB2
	public static bool IsCursorOnUI(Vector3 point)
	{
		return UIUtility.IsCursorOnUI(Camera.main, point, 31);
	}

	// Token: 0x060030D5 RID: 12501 RVA: 0x0001EDC1 File Offset: 0x0001CFC1
	public static bool IsCursorOnUI(Camera cam, Vector3 point)
	{
		return UIUtility.IsCursorOnUI(cam, point, 31);
	}

	// Token: 0x060030D6 RID: 12502 RVA: 0x00179180 File Offset: 0x00177380
	public static bool IsCursorOnUI(Camera cam, Vector3 point, int layer)
	{
		Ray ray = cam.ScreenPointToRay(point);
		LayerMask layerMask = 1 << layer;
		RaycastHit raycastHit;
		return Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity, layerMask);
	}
}
