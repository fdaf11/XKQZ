using System;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class SearchRange : MonoBehaviour
{
	// Token: 0x060009A0 RID: 2464 RVA: 0x00051C6C File Offset: 0x0004FE6C
	public static bool IsGameObjectPointInRange(GameObject objectA, float fSquaredR, float fCosTheta, GameObject objectB)
	{
		return SearchRange.IsPointInCircularSector(objectA.transform.position.x, objectA.transform.position.z, objectA.transform.forward.x, objectA.transform.forward.z, fSquaredR, fCosTheta, objectB.transform.position.x, objectB.transform.position.z);
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00051CF4 File Offset: 0x0004FEF4
	public static bool IsPointInCircularSector(float cx, float cy, float ux, float uy, float squaredR, float cosTheta, float px, float py)
	{
		float num = px - cx;
		float num2 = py - cy;
		float num3 = num * num + num2 * num2;
		if (num3 > squaredR)
		{
			return false;
		}
		float num4 = Mathf.Sqrt(num3);
		return num * ux + num2 * uy >= num4 * cosTheta;
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00051D34 File Offset: 0x0004FF34
	public static bool IsPointInSelfRange(float cx, float cy, float squaredR, float px, float py)
	{
		float num = px - cx;
		float num2 = py - cy;
		float num3 = num * num + num2 * num2;
		return num3 < squaredR;
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x00007D47 File Offset: 0x00005F47
	public static bool CheckEndPos(Vector3 vPos1, Vector3 vPos2, float fStopDis)
	{
		return Vector3.Distance(vPos1, vPos2) > fStopDis;
	}
}
