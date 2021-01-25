using System;
using UnityEngine;

namespace Exploder.Core.Math
{
	// Token: 0x020000B7 RID: 183
	public class Plane
	{
		// Token: 0x060003CF RID: 975 RVA: 0x000315CC File Offset: 0x0002F7CC
		public Plane(Vector3 a, Vector3 b, Vector3 c)
		{
			this.Normal = Vector3.Cross(b - a, c - a).normalized;
			this.Distance = Vector3.Dot(this.Normal, a);
			this.Pnt = a;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000049D1 File Offset: 0x00002BD1
		public Plane(Vector3 normal, Vector3 p)
		{
			this.Normal = normal.normalized;
			this.Distance = Vector3.Dot(this.Normal, p);
			this.Pnt = p;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000049FF File Offset: 0x00002BFF
		public Plane(Plane instance)
		{
			this.Normal = instance.Normal;
			this.Distance = instance.Distance;
			this.Pnt = instance.Pnt;
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00004A2B File Offset: 0x00002C2B
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x00004A33 File Offset: 0x00002C33
		public Vector3 Pnt { get; private set; }

		// Token: 0x060003D4 RID: 980 RVA: 0x0003161C File Offset: 0x0002F81C
		public Plane.PointClass ClassifyPoint(Vector3 p)
		{
			float num = Vector3.Dot(p, this.Normal) - this.Distance;
			return (num >= -0.0001f) ? ((num <= 0.0001f) ? Plane.PointClass.Coplanar : Plane.PointClass.Front) : Plane.PointClass.Back;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00004A3C File Offset: 0x00002C3C
		public bool GetSide(Vector3 n)
		{
			return Vector3.Dot(n, this.Normal) - this.Distance > 0.0001f;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00004A58 File Offset: 0x00002C58
		public void Flip()
		{
			this.Normal = -this.Normal;
			this.Distance = -this.Distance;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00031660 File Offset: 0x0002F860
		public bool GetSideFix(ref Vector3 n)
		{
			float num = n.x * this.Normal.x + n.y * this.Normal.y + n.z * this.Normal.z - this.Distance;
			float num2 = 1f;
			float num3 = num;
			if (num < 0f)
			{
				num2 = -1f;
				num3 = -num;
			}
			if (num3 < 0.0011f)
			{
				n.x += this.Normal.x * 0.001f * num2;
				n.y += this.Normal.y * 0.001f * num2;
				n.z += this.Normal.z * 0.001f * num2;
				num = n.x * this.Normal.x + n.y * this.Normal.y + n.z * this.Normal.z - this.Distance;
			}
			return num > 0.0001f;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00004A78 File Offset: 0x00002C78
		public bool SameSide(Vector3 a, Vector3 b)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0003177C File Offset: 0x0002F97C
		public bool IntersectSegment(Vector3 a, Vector3 b, out float t, ref Vector3 q)
		{
			float num = b.x - a.x;
			float num2 = b.y - a.y;
			float num3 = b.z - a.z;
			float num4 = this.Normal.x * a.x + this.Normal.y * a.y + this.Normal.z * a.z;
			float num5 = this.Normal.x * num + this.Normal.y * num2 + this.Normal.z * num3;
			t = (this.Distance - num4) / num5;
			if (t >= -0.0001f && t <= 1.0001f)
			{
				q.x = a.x + t * num;
				q.y = a.y + t * num2;
				q.z = a.z + t * num3;
				return true;
			}
			q = Vector3.zero;
			return false;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00031890 File Offset: 0x0002FA90
		public void InverseTransform(Transform transform)
		{
			Vector3 vector = transform.InverseTransformDirection(this.Normal);
			Vector3 vector2 = transform.InverseTransformPoint(this.Pnt);
			this.Normal = vector;
			this.Distance = Vector3.Dot(vector, vector2);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000318CC File Offset: 0x0002FACC
		public Matrix4x4 GetPlaneMatrix()
		{
			Matrix4x4 result = default(Matrix4x4);
			Quaternion quaternion = Quaternion.LookRotation(this.Normal);
			result.SetTRS(this.Pnt, quaternion, Vector3.one);
			return result;
		}

		// Token: 0x04000306 RID: 774
		private const float epsylon = 0.0001f;

		// Token: 0x04000307 RID: 775
		public Vector3 Normal;

		// Token: 0x04000308 RID: 776
		public float Distance;

		// Token: 0x020000B8 RID: 184
		[Flags]
		public enum PointClass
		{
			// Token: 0x0400030B RID: 779
			Coplanar = 0,
			// Token: 0x0400030C RID: 780
			Front = 1,
			// Token: 0x0400030D RID: 781
			Back = 2,
			// Token: 0x0400030E RID: 782
			Intersection = 3
		}
	}
}
