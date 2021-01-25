using System;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006B9 RID: 1721
	public class PathManager : MonoBehaviour
	{
		// Token: 0x0600297A RID: 10618 RVA: 0x0014836C File Offset: 0x0014656C
		private void OnDrawGizmos()
		{
			if (this.waypoints.Length <= 0)
			{
				return;
			}
			Vector3[] pathPoints = this.GetPathPoints();
			Gizmos.color = this.color1;
			Gizmos.DrawWireCube(pathPoints[0], this.size);
			Gizmos.DrawWireCube(pathPoints[pathPoints.Length - 1], this.size);
			Gizmos.color = this.color2;
			for (int i = 1; i < pathPoints.Length - 1; i++)
			{
				Gizmos.DrawWireSphere(pathPoints[i], this.radius);
			}
			if (this.drawCurved && pathPoints.Length >= 2)
			{
				WaypointManager.DrawCurved(pathPoints);
			}
			else
			{
				WaypointManager.DrawStraight(pathPoints);
			}
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x0014842C File Offset: 0x0014662C
		public virtual Vector3[] GetPathPoints()
		{
			Vector3[] array = new Vector3[this.waypoints.Length];
			for (int i = 0; i < this.waypoints.Length; i++)
			{
				array[i] = this.waypoints[i].position;
			}
			return array;
		}

		// Token: 0x04003480 RID: 13440
		public Transform[] waypoints;

		// Token: 0x04003481 RID: 13441
		public bool drawCurved = true;

		// Token: 0x04003482 RID: 13442
		public Color color1 = new Color(1f, 0f, 1f, 0.5f);

		// Token: 0x04003483 RID: 13443
		public Color color2 = new Color(1f, 0.92156863f, 0.015686275f, 0.5f);

		// Token: 0x04003484 RID: 13444
		public Vector3 size = new Vector3(0.7f, 0.7f, 0.7f);

		// Token: 0x04003485 RID: 13445
		public float radius = 0.4f;

		// Token: 0x04003486 RID: 13446
		public GameObject replaceObject;
	}
}
