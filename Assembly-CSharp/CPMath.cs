using System;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class CPMath
{
	// Token: 0x060002E5 RID: 741 RVA: 0x00029FE4 File Offset: 0x000281E4
	public static Vector3 CalculateBezier(float t, Vector3 p, Vector3 a, Vector3 b, Vector3 q)
	{
		float num = t * t;
		float num2 = num * t;
		float num3 = 1f - t;
		float num4 = num3 * num3;
		float num5 = num4 * num3;
		return num5 * p + 3f * num4 * t * a + 3f * num3 * num * b + num2 * q;
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x0002A04C File Offset: 0x0002824C
	public static Vector3 CalculateHermite(Vector3 p, Vector3 a, Vector3 b, Vector3 q, float t, float tension, float bias)
	{
		float num = t * t;
		float num2 = num * t;
		Vector3 vector = (a - p) * (1f + bias) * (1f - tension) / 2f;
		vector += (b - a) * (1f - bias) * (1f - tension) / 2f;
		Vector3 vector2 = (b - a) * (1f + bias) * (1f - tension) / 2f;
		vector2 += (q - b) * (1f - bias) * (1f - tension) / 2f;
		float num3 = 2f * num2 - 3f * num + 1f;
		float num4 = num2 - 2f * num + t;
		float num5 = num2 - num;
		float num6 = -2f * num2 + 3f * num;
		return num3 * a + num4 * vector + num5 * vector2 + num6 * b;
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x0002A18C File Offset: 0x0002838C
	public static Vector3 CalculateCatmullRom(Vector3 p, Vector3 a, Vector3 b, Vector3 q, float t)
	{
		float num = t * t;
		Vector3 vector = -0.5f * p + 1.5f * a - 1.5f * b + 0.5f * q;
		Vector3 vector2 = p - 2.5f * a + 2f * b - 0.5f * q;
		Vector3 vector3 = -0.5f * p + 0.5f * b;
		return vector * t * num + vector2 * num + vector3 * t + a;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x0002A25C File Offset: 0x0002845C
	public static Vector2 CalculateBezier(float t, Vector2 p, Vector2 a, Vector2 b, Vector2 q)
	{
		float num = t * t;
		float num2 = num * t;
		float num3 = 1f - t;
		float num4 = num3 * num3;
		float num5 = num4 * num3;
		return num5 * p + 3f * num4 * t * a + 3f * num3 * num * b + num2 * q;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x0002A2C4 File Offset: 0x000284C4
	public static Vector2 CalculateHermite(Vector2 p, Vector2 a, Vector2 b, Vector2 q, float t, float tension, float bias)
	{
		float num = t * t;
		float num2 = num * t;
		Vector2 vector = (a - p) * (1f + bias) * (1f - tension) / 2f;
		vector += (b - a) * (1f - bias) * (1f - tension) / 2f;
		Vector2 vector2 = (b - a) * (1f + bias) * (1f - tension) / 2f;
		vector2 += (q - b) * (1f - bias) * (1f - tension) / 2f;
		float num3 = 2f * num2 - 3f * num + 1f;
		float num4 = num2 - 2f * num + t;
		float num5 = num2 - num;
		float num6 = -2f * num2 + 3f * num;
		return num3 * a + num4 * vector + num5 * vector2 + num6 * b;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x0002A404 File Offset: 0x00028604
	public static Vector2 CalculateCatmullRom(Vector2 p, Vector2 a, Vector2 b, Vector2 q, float t)
	{
		float num = t * t;
		Vector2 vector = -0.5f * p + 1.5f * a - 1.5f * b + 0.5f * q;
		Vector2 vector2 = p - 2.5f * a + 2f * b - 0.5f * q;
		Vector2 vector3 = -0.5f * p + 0.5f * b;
		return vector * t * num + vector2 * num + vector3 * t + a;
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0002A4D4 File Offset: 0x000286D4
	public static Quaternion CalculateCubic(Quaternion p, Quaternion a, Quaternion b, Quaternion q, float t)
	{
		if (Quaternion.Dot(p, q) < 0f)
		{
			q..ctor(-q.x, -q.y, -q.z, -q.w);
		}
		if (Quaternion.Dot(p, a) < 0f)
		{
			a..ctor(-a.x, -a.y, -a.z, -a.w);
		}
		if (Quaternion.Dot(p, b) < 0f)
		{
			b..ctor(-b.x, -b.y, -b.z, -b.w);
		}
		Quaternion p2 = CPMath.SquadTangent(a, p, q);
		Quaternion q2 = CPMath.SquadTangent(p, q, b);
		float t2 = 2f * t * (1f - t);
		return CPMath.Slerp(CPMath.Slerp(p, q, t), CPMath.Slerp(p2, q2, t), t2);
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0002A5C8 File Offset: 0x000287C8
	public static float CalculateCubic(float p, float a, float b, float q, float t)
	{
		float num = t * t;
		float num2 = num * t;
		float num3 = 1f - t;
		float num4 = num3 * num3;
		float num5 = num4 * num3;
		return num5 * p + 3f * num4 * t * q + 3f * num3 * num * a + num2 * b;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0002A614 File Offset: 0x00028814
	public static float CalculateHermite(float p, float a, float b, float q, float t, float tension, float bias)
	{
		float num = t * t;
		float num2 = num * t;
		float num3 = (a - p) * (1f + bias) * (1f - tension) / 2f;
		num3 += (b - a) * (1f - bias) * (1f - tension) / 2f;
		float num4 = (b - a) * (1f + bias) * (1f - tension) / 2f;
		num4 += (q - b) * (1f - bias) * (1f - tension) / 2f;
		float num5 = 2f * num2 - 3f * num + 1f;
		float num6 = num2 - 2f * num + t;
		float num7 = num2 - num;
		float num8 = -2f * num2 + 3f * num;
		return num5 * a + num6 * num3 + num7 * num4 + num8 * b;
	}

	// Token: 0x060002EE RID: 750 RVA: 0x0002A6F0 File Offset: 0x000288F0
	public static float CalculateCatmullRom(float p, float a, float b, float q, float t)
	{
		float num = t * t;
		float num2 = -0.5f * p + 1.5f * a - 1.5f * b + 0.5f * q;
		float num3 = p - 2.5f * a + 2f * b - 0.5f * q;
		float num4 = -0.5f * p + 0.5f * b;
		return num2 * t * num + num3 * num + num4 * t + a;
	}

	// Token: 0x060002EF RID: 751 RVA: 0x0000427F File Offset: 0x0000247F
	public static float SmoothStep(float val)
	{
		return val * val * (3f - 2f * val);
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0002A764 File Offset: 0x00028964
	public static Quaternion SquadTangent(Quaternion before, Quaternion center, Quaternion after)
	{
		Quaternion quaternion = CPMath.LnDif(center, before);
		Quaternion quaternion2 = CPMath.LnDif(center, after);
		Quaternion identity = Quaternion.identity;
		for (int i = 0; i < 4; i++)
		{
			identity[i] = -0.25f * (quaternion[i] + quaternion2[i]);
		}
		return center * CPMath.Exp(identity);
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0002A7C4 File Offset: 0x000289C4
	public static Quaternion LnDif(Quaternion a, Quaternion b)
	{
		Quaternion q = Quaternion.Inverse(a) * b;
		CPMath.Normalize(q);
		return CPMath.Log(q);
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0002A7EC File Offset: 0x000289EC
	public static Quaternion Normalize(Quaternion q)
	{
		float num = Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
		if (num > 0f)
		{
			q.x /= num;
			q.y /= num;
			q.z /= num;
			q.w /= num;
		}
		else
		{
			q.x = 0f;
			q.y = 0f;
			q.z = 0f;
			q.w = 1f;
		}
		return q;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0002A8BC File Offset: 0x00028ABC
	public static Quaternion Exp(Quaternion q)
	{
		float num = Mathf.Sqrt(q[0] * q[0] + q[1] * q[1] + q[2] * q[2]);
		if ((double)num < 1E-06)
		{
			return new Quaternion(q[0], q[1], q[2], Mathf.Cos(num));
		}
		float num2 = Mathf.Sin(num) / num;
		return new Quaternion(q[0] * num2, q[1] * num2, q[2] * num2, Mathf.Cos(num));
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x0002A96C File Offset: 0x00028B6C
	public static Quaternion Log(Quaternion q)
	{
		float num = Mathf.Sqrt(q[0] * q[0] + q[1] * q[1] + q[2] * q[2]);
		if ((double)num < 1E-06)
		{
			return new Quaternion(q[0], q[1], q[2], 0f);
		}
		float num2 = Mathf.Acos(q[3]) / num;
		return new Quaternion(q[0] * num2, q[1] * num2, q[2] * num2, 0f);
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x0002AA20 File Offset: 0x00028C20
	public static Quaternion Slerp(Quaternion p, Quaternion q, float t)
	{
		float num = Quaternion.Dot(p, q);
		Quaternion result;
		if (1f + num > 1E-05f)
		{
			float num5;
			float num6;
			if (1f - num > 1E-05f)
			{
				float num2 = Mathf.Acos(num);
				float num3 = Mathf.Sin(num2);
				float num4 = Mathf.Sign(num3) * 1f / num3;
				num5 = Mathf.Sin((1f - t) * num2) * num4;
				num6 = Mathf.Sin(t * num2) * num4;
			}
			else
			{
				num5 = 1f - t;
				num6 = t;
			}
			result.x = num5 * p.x + num6 * q.x;
			result.y = num5 * p.y + num6 * q.y;
			result.z = num5 * p.z + num6 * q.z;
			result.w = num5 * p.w + num6 * q.w;
		}
		else
		{
			float num5 = Mathf.Sin((1f - t) * 3.1415927f * 0.5f);
			float num6 = Mathf.Sin(t * 3.1415927f * 0.5f);
			result.x = num5 * p.x - num6 * p.y;
			result.y = num5 * p.y + num6 * p.x;
			result.z = num5 * p.z - num6 * p.w;
			result.w = p.z;
		}
		return result;
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x0002ABA0 File Offset: 0x00028DA0
	public static Quaternion Nlerp(Quaternion p, Quaternion q, float t)
	{
		float num = 1f - t;
		Quaternion quaternion;
		quaternion.x = num * p.x + t * q.x;
		quaternion.y = num * p.y + t * q.y;
		quaternion.z = num * p.z + t * q.z;
		quaternion.w = num * p.w + t * q.w;
		CPMath.Normalize(quaternion);
		return quaternion;
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00004292 File Offset: 0x00002492
	public static Quaternion GetQuatConjugate(Quaternion q)
	{
		return new Quaternion(-q.x, -q.y, -q.z, q.w);
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x0002AC28 File Offset: 0x00028E28
	public static float SignedAngle(Vector3 from, Vector3 to, Vector3 up)
	{
		Vector3 normalized = (to - from).normalized;
		Vector3 vector = Vector3.Cross(up, normalized);
		float num = Vector3.Dot(from, vector);
		return Vector3.Angle(from, to) * Mathf.Sign(num);
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x000042B8 File Offset: 0x000024B8
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, -max, -min);
	}
}
