using System;
using UnityEngine;

// Token: 0x020005FF RID: 1535
public static class VLightGeometryUtil
{
	// Token: 0x060025F7 RID: 9719 RVA: 0x001259C8 File Offset: 0x00123BC8
	public static void RecalculateFrustrumPoints(Camera camera, float aspectRatio, out Vector3[] _frustrumPoints)
	{
		float far = camera.far;
		float near = camera.near;
		if (!camera.isOrthoGraphic)
		{
			float num = 2f * Mathf.Tan(camera.fov * 0.5f * 0.017453292f) * near;
			float num2 = num * aspectRatio;
			float num3 = 2f * Mathf.Tan(camera.fov * 0.5f * 0.017453292f) * far;
			float num4 = num3 * aspectRatio;
			Vector3 vector = Vector3.forward * far;
			Vector3 vector2 = vector + Vector3.up * num3 / 2f - Vector3.right * num4 / 2f;
			Vector3 vector3 = vector + Vector3.up * num3 / 2f + Vector3.right * num4 / 2f;
			Vector3 vector4 = vector - Vector3.up * num3 / 2f - Vector3.right * num4 / 2f;
			Vector3 vector5 = vector - Vector3.up * num3 / 2f + Vector3.right * num4 / 2f;
			Vector3 vector6 = Vector3.forward * near;
			Vector3 vector7 = vector6 + Vector3.up * num / 2f - Vector3.right * num2 / 2f;
			Vector3 vector8 = vector6 + Vector3.up * num / 2f + Vector3.right * num2 / 2f;
			Vector3 vector9 = vector6 - Vector3.up * num / 2f - Vector3.right * num2 / 2f;
			Vector3 vector10 = vector6 - Vector3.up * num / 2f + Vector3.right * num2 / 2f;
			_frustrumPoints = new Vector3[8];
			_frustrumPoints[0] = vector7;
			_frustrumPoints[1] = vector2;
			_frustrumPoints[2] = vector8;
			_frustrumPoints[3] = vector3;
			_frustrumPoints[4] = vector9;
			_frustrumPoints[5] = vector4;
			_frustrumPoints[6] = vector10;
			_frustrumPoints[7] = vector5;
		}
		else
		{
			float num5 = camera.orthographicSize * 0.5f;
			_frustrumPoints = new Vector3[8];
			_frustrumPoints[0] = new Vector3(-num5, num5, near);
			_frustrumPoints[1] = new Vector3(-num5, num5, far);
			_frustrumPoints[2] = new Vector3(num5, num5, near);
			_frustrumPoints[3] = new Vector3(num5, num5, far);
			_frustrumPoints[4] = new Vector3(-num5, -num5, near);
			_frustrumPoints[5] = new Vector3(-num5, -num5, far);
			_frustrumPoints[6] = new Vector3(num5, -num5, near);
			_frustrumPoints[7] = new Vector3(num5, -num5, far);
		}
	}

	// Token: 0x060025F8 RID: 9720 RVA: 0x00125D78 File Offset: 0x00123F78
	public static Vector3[] ClipPolygonAgainstPlane(Vector3[] subjectPolygon, Plane[] planes)
	{
		Array.Copy(subjectPolygon, VLightGeometryUtil._outputList, subjectPolygon.Length);
		int num = subjectPolygon.Length;
		foreach (Plane plane in planes)
		{
			Array.Copy(VLightGeometryUtil._outputList, VLightGeometryUtil._inputList, num);
			int num2 = num;
			num = 0;
			if (num2 != 0)
			{
				Vector3 vector = VLightGeometryUtil._inputList[num2 - 1];
				for (int j = 0; j < num2; j++)
				{
					Vector3 vector2 = VLightGeometryUtil._inputList[j];
					bool side = plane.GetSide(vector2);
					bool side2 = plane.GetSide(vector);
					if (side)
					{
						Vector3 vector3;
						if (!side2 && VLightGeometryUtil.ComputeIntersection(vector, vector2, plane, 0f, out vector3))
						{
							VLightGeometryUtil._outputList[num++] = vector3;
						}
						VLightGeometryUtil._outputList[num++] = vector2;
					}
					else if (side2)
					{
						Vector3 vector4;
						if (VLightGeometryUtil.ComputeIntersection(vector, vector2, plane, 0f, out vector4))
						{
							VLightGeometryUtil._outputList[num++] = vector4;
						}
						else
						{
							VLightGeometryUtil._outputList[num++] = vector2;
						}
					}
					vector = vector2;
				}
				if (num == 0)
				{
				}
			}
		}
		Vector3[] array = new Vector3[num];
		Array.Copy(VLightGeometryUtil._outputList, array, num);
		return array;
	}

	// Token: 0x060025F9 RID: 9721 RVA: 0x00125F04 File Offset: 0x00124104
	public static bool ComputeIntersection(Vector3 start, Vector3 end, Plane plane, float e, out Vector3 result)
	{
		Vector3 vector = start - end;
		float num = Vector3.Dot(plane.normal, start) + plane.distance;
		float num2 = Vector3.Dot(plane.normal, vector);
		float num3 = num / num2;
		if (Mathf.Abs(num3) < e)
		{
			result = Vector3.zero;
		}
		else
		{
			if (num3 > 0f && num3 < 1f)
			{
				result = (end - start) * num3 + start;
				return true;
			}
			result = Vector3.zero;
		}
		return false;
	}

	// Token: 0x060025FA RID: 9722 RVA: 0x00125FA0 File Offset: 0x001241A0
	public static Vector4 Vector4Multiply(Vector4 right, Vector4 left)
	{
		return new Vector4(right.x * left.x, right.y * left.y, right.z * left.z, right.w * left.w);
	}

	// Token: 0x060025FB RID: 9723 RVA: 0x000194EF File Offset: 0x000176EF
	public static Vector4 Vector4Frac(Vector4 vector)
	{
		return new Vector4(VLightGeometryUtil.Frac(vector.x), VLightGeometryUtil.Frac(vector.y), VLightGeometryUtil.Frac(vector.z), VLightGeometryUtil.Frac(vector.w));
	}

	// Token: 0x060025FC RID: 9724 RVA: 0x000131CC File Offset: 0x000113CC
	public static float Frac(float value)
	{
		return value - (float)Mathf.FloorToInt(value);
	}

	// Token: 0x060025FD RID: 9725 RVA: 0x00125FF0 File Offset: 0x001241F0
	public static Color FloatToRGBA(float value)
	{
		Vector4 vector = new Vector4(1f, 255f, 65025f, 160581380f) * value;
		vector = VLightGeometryUtil.Vector4Frac(vector);
		vector -= VLightGeometryUtil.Vector4Multiply(new Vector4(vector.y, vector.z, vector.w, vector.w), new Vector4(0.003921569f, 0.003921569f, 0.003921569f, 0f));
		return vector;
	}

	// Token: 0x04002E8E RID: 11918
	private static Vector3[] _outputList = new Vector3[20];

	// Token: 0x04002E8F RID: 11919
	private static Vector3[] _inputList = new Vector3[20];
}
