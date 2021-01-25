using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006B7 RID: 1719
	public class BezierPathManager : PathManager
	{
		// Token: 0x06002973 RID: 10611 RVA: 0x0001B367 File Offset: 0x00019567
		private void Awake()
		{
			this.CalculatePath();
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x00148050 File Offset: 0x00146250
		private void OnDrawGizmos()
		{
			if (this.bPoints.Count <= 0)
			{
				return;
			}
			Gizmos.color = this.color1;
			Gizmos.DrawWireCube(this.bPoints[0].wp.position, this.size);
			Gizmos.DrawWireCube(this.bPoints[this.bPoints.Count - 1].wp.position, this.size);
			Gizmos.color = this.color2;
			for (int i = 1; i < this.bPoints.Count - 1; i++)
			{
				Gizmos.DrawWireSphere(this.bPoints[i].wp.position, this.radius);
			}
			if (this.drawCurved && this.bPoints.Count >= 2)
			{
				WaypointManager.DrawCurved(this.pathPoints);
			}
			else
			{
				WaypointManager.DrawStraight(this.pathPoints);
			}
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x0001B36F File Offset: 0x0001956F
		public override Vector3[] GetPathPoints()
		{
			return this.pathPoints;
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x0014814C File Offset: 0x0014634C
		public void CalculatePath()
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < this.bPoints.Count - 1; i++)
			{
				BezierPoint bezierPoint = this.bPoints[i];
				float detail = this.pathDetail;
				if (this.customDetail)
				{
					detail = this.segmentDetail[i];
				}
				list.AddRange(this.GetPoints(bezierPoint.wp.position, bezierPoint.cp[1].position, this.bPoints[i + 1].cp[0].position, this.bPoints[i + 1].wp.position, detail));
			}
			this.pathPoints = Enumerable.ToArray<Vector3>(Enumerable.Distinct<Vector3>(list));
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x00148214 File Offset: 0x00146414
		private List<Vector3> GetPoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float detail)
		{
			List<Vector3> list = new List<Vector3>();
			float num = detail * 10f;
			int num2 = 0;
			while ((float)num2 <= num)
			{
				float num3 = (float)num2 / num;
				float num4 = 1f - num3;
				Vector3 vector = Vector3.zero;
				vector += p0 * num4 * num4 * num4;
				vector += p1 * num3 * 3f * num4 * num4;
				vector += p2 * 3f * num3 * num3 * num4;
				vector += p3 * num3 * num3 * num3;
				list.Add(vector);
				num2++;
			}
			return list;
		}

		// Token: 0x04003477 RID: 13431
		public Vector3[] pathPoints;

		// Token: 0x04003478 RID: 13432
		public List<BezierPoint> bPoints;

		// Token: 0x04003479 RID: 13433
		public bool showHandles = true;

		// Token: 0x0400347A RID: 13434
		public Color color3 = new Color(0.42352942f, 0.5921569f, 1f, 1f);

		// Token: 0x0400347B RID: 13435
		public float pathDetail = 1f;

		// Token: 0x0400347C RID: 13436
		public bool customDetail;

		// Token: 0x0400347D RID: 13437
		public List<float> segmentDetail = new List<float>();
	}
}
