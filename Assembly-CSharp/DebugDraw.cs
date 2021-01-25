using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200073B RID: 1851
public class DebugDraw : MonoBehaviour
{
	// Token: 0x06002BD8 RID: 11224 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x0001C4F0 File Offset: 0x0001A6F0
	public static void Square(Vector3 point, float duration)
	{
		DebugDraw.Square(point, 0.5f, Color.white, duration);
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x0001C503 File Offset: 0x0001A703
	public static void Square(Vector3 point, Color color, float duration)
	{
		DebugDraw.Square(point, 0.5f, color, duration);
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x001553BC File Offset: 0x001535BC
	public static void Square(Vector3 point, float width, Color color, float duration)
	{
		width *= 0.5f;
		Debug.DrawLine(point + new Vector3(-width, 0f, width), point + new Vector3(width, 0f, width), color, duration);
		Debug.DrawLine(point + new Vector3(width, 0f, -width), point + new Vector3(-width, 0f, -width), color, duration);
		Debug.DrawLine(point + new Vector3(-width, 0f, width), point + new Vector3(-width, 0f, -width), color, duration);
		Debug.DrawLine(point + new Vector3(width, 0f, -width), point + new Vector3(width, 0f, width), color, duration);
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x0001C512 File Offset: 0x0001A712
	public static void Cross(Vector3 point, float duration)
	{
		DebugDraw.Cross(point, 0.5f, Color.white, duration);
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x0001C525 File Offset: 0x0001A725
	public static void Cross(Vector3 point, Color color, float duration)
	{
		DebugDraw.Cross(point, 0.5f, color, duration);
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x00155488 File Offset: 0x00153688
	public static void Cross(Vector3 point, float width, Color color, float duration)
	{
		width *= 0.5f;
		Debug.DrawLine(point + new Vector3(width, 0f, width), point + new Vector3(-width, 0f, -width), color, duration);
		Debug.DrawLine(point + new Vector3(-width, 0f, width), point + new Vector3(width, 0f, -width), color, duration);
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x001554F8 File Offset: 0x001536F8
	public static void Rect(Vector3 center, float width, float height, Color color, float duration)
	{
		width /= 2f;
		height /= 2f;
		Debug.DrawLine(center + new Vector3(width, 0f, -height), center + new Vector3(width, 0f, height), color, duration);
		Debug.DrawLine(center + new Vector3(width, 0f, -height), center + new Vector3(-width, 0f, -height), color, duration);
		Debug.DrawLine(center + new Vector3(-width, 0f, height), center + new Vector3(-width, 0f, -height), color, duration);
		Debug.DrawLine(center + new Vector3(-width, 0f, height), center + new Vector3(width, 0f, height), color, duration);
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x0001C534 File Offset: 0x0001A734
	public static void Rect(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Color color, float duration)
	{
		Debug.DrawLine(p1, p2, color, duration);
		Debug.DrawLine(p2, p3, color, duration);
		Debug.DrawLine(p3, p4, color, duration);
		Debug.DrawLine(p4, p1, color, duration);
	}

	// Token: 0x06002BE2 RID: 11234 RVA: 0x001555D0 File Offset: 0x001537D0
	public static void PathPointSquare(List<Vector3> p, Color col, float duration)
	{
		for (int i = 0; i < p.Count; i++)
		{
			DebugDraw.Square(p[i], col, duration);
		}
	}

	// Token: 0x06002BE3 RID: 11235 RVA: 0x00155604 File Offset: 0x00153804
	public static void PathPointSquare(List<Vector3> p, float duration)
	{
		Debug.Log("draw");
		float num = 0f;
		float num2 = 1f;
		for (int i = 0; i < p.Count; i++)
		{
			num += 1f / (float)p.Count;
			num2 -= 1f / (float)p.Count;
			Color color;
			color..ctor(num, num2, 0f, 1f);
			DebugDraw.Square(p[i], color, duration);
		}
	}

	// Token: 0x06002BE4 RID: 11236 RVA: 0x00155680 File Offset: 0x00153880
	public static void PathPointSquare(List<Vector3> p, float width, Color col, float duration)
	{
		for (int i = 0; i < p.Count; i++)
		{
			DebugDraw.Square(p[i], width, col, duration);
		}
	}

	// Token: 0x06002BE5 RID: 11237 RVA: 0x001556B4 File Offset: 0x001538B4
	public static void PathPointSquare(List<Vector3> p, float width, float duration)
	{
		float num = 0f;
		float num2 = 1f;
		for (int i = 0; i < p.Count; i++)
		{
			num += 1f / (float)p.Count;
			num2 -= 1f / (float)p.Count;
			Color color;
			color..ctor(num, num2, 0f, 1f);
			DebugDraw.Square(p[i], width, color, duration);
		}
	}

	// Token: 0x06002BE6 RID: 11238 RVA: 0x00155728 File Offset: 0x00153928
	public static void PathPointCross(List<Vector3> p, Color col, float duration)
	{
		for (int i = 0; i < p.Count; i++)
		{
			DebugDraw.Cross(p[i], col, duration);
		}
	}

	// Token: 0x06002BE7 RID: 11239 RVA: 0x0015575C File Offset: 0x0015395C
	public static void PathPointCross(List<Vector3> p, float duration)
	{
		float num = 0f;
		float num2 = 1f;
		for (int i = 0; i < p.Count; i++)
		{
			num += 1f / (float)p.Count;
			num2 -= 1f / (float)p.Count;
			Color color;
			color..ctor(num, num2, 0f, 1f);
			DebugDraw.Cross(p[i], color, duration);
		}
	}

	// Token: 0x06002BE8 RID: 11240 RVA: 0x001557D0 File Offset: 0x001539D0
	public static void PathPointCross(List<Vector3> p, float width, Color col, float duration)
	{
		for (int i = 0; i < p.Count; i++)
		{
			DebugDraw.Cross(p[i], width, col, duration);
		}
	}

	// Token: 0x06002BE9 RID: 11241 RVA: 0x00155804 File Offset: 0x00153A04
	public static void PathPointCross(List<Vector3> p, float width, float duration)
	{
		float num = 0f;
		float num2 = 1f;
		for (int i = 0; i < p.Count; i++)
		{
			num += 1f / (float)p.Count;
			num2 -= 1f / (float)p.Count;
			Color color;
			color..ctor(num, num2, 0f, 1f);
			DebugDraw.Cross(p[i], width, color, duration);
		}
	}
}
