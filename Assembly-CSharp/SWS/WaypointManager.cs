using System;
using System.Collections.Generic;
using Holoville.HOTween;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006BA RID: 1722
	public class WaypointManager : MonoBehaviour
	{
		// Token: 0x0600297E RID: 10622 RVA: 0x0014847C File Offset: 0x0014667C
		private void Awake()
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				WaypointManager.AddPath(transform.gameObject);
			}
			HOTween.Init(true, true, true);
			HOTween.EnableOverwriteManager(true);
			HOTween.showPathGizmos = true;
			HOTween.warningLevel = 1;
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x00148500 File Offset: 0x00146700
		public static void AddPath(GameObject path)
		{
			if (path.name.Contains("(Clone)"))
			{
				path.name = path.name.Replace("(Clone)", string.Empty);
			}
			if (WaypointManager.Paths.ContainsKey(path.name))
			{
				Debug.LogWarning("Called AddPath() but Scene already contains Path " + path.name + ".");
				return;
			}
			PathManager componentInChildren = path.GetComponentInChildren<PathManager>();
			if (componentInChildren)
			{
				WaypointManager.Paths.Add(path.name, componentInChildren);
				return;
			}
			Debug.LogWarning("Called AddPath() but Transform " + path.name + " has no Path Component attached.");
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x0001B397 File Offset: 0x00019597
		private void OnDestroy()
		{
			WaypointManager.Paths.Clear();
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x001485B0 File Offset: 0x001467B0
		public static void DrawStraight(Vector3[] waypoints)
		{
			for (int i = 0; i < waypoints.Length - 1; i++)
			{
				Gizmos.DrawLine(waypoints[i], waypoints[i + 1]);
			}
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x001485F4 File Offset: 0x001467F4
		public static void DrawCurved(Vector3[] waypoints)
		{
			Vector3[] array = new Vector3[waypoints.Length + 2];
			waypoints.CopyTo(array, 1);
			array[0] = waypoints[1];
			array[array.Length - 1] = array[array.Length - 2];
			int num = array.Length * 10;
			Vector3[] array2 = new Vector3[num + 1];
			for (int i = 0; i <= num; i++)
			{
				float t = (float)i / (float)num;
				Vector3 vector = WaypointManager.GetPoint(array, t);
				array2[i] = vector;
			}
			Vector3 vector2 = array2[0];
			for (int j = 1; j < array2.Length; j++)
			{
				Vector3 vector = array2[j];
				Gizmos.DrawLine(vector, vector2);
				vector2 = vector;
			}
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x001486D0 File Offset: 0x001468D0
		public static Vector3 GetPoint(Vector3[] gizmoPoints, float t)
		{
			int num = gizmoPoints.Length - 3;
			int num2 = (int)Mathf.Floor(t * (float)num);
			int num3 = num - 1;
			if (num3 > num2)
			{
				num3 = num2;
			}
			float num4 = t * (float)num - (float)num3;
			Vector3 vector = gizmoPoints[num3];
			Vector3 vector2 = gizmoPoints[num3 + 1];
			Vector3 vector3 = gizmoPoints[num3 + 2];
			Vector3 vector4 = gizmoPoints[num3 + 3];
			return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num4 * num4 * num4) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num4 * num4) + (-vector + vector3) * num4 + 2f * vector2);
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x001487F4 File Offset: 0x001469F4
		public static float GetPathLength(Vector3[] waypoints)
		{
			float num = 0f;
			for (int i = 0; i < waypoints.Length - 1; i++)
			{
				num += Vector3.Distance(waypoints[i], waypoints[i + 1]);
			}
			return num;
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x00148840 File Offset: 0x00146A40
		public static List<Vector3> SmoothCurve(List<Vector3> pathToCurve, int interpolations)
		{
			if (interpolations < 1)
			{
				interpolations = 1;
			}
			int count = pathToCurve.Count;
			int num = count * Mathf.RoundToInt((float)interpolations) - 1;
			List<Vector3> list = new List<Vector3>(num);
			for (int i = 0; i < num + 1; i++)
			{
				float num2 = Mathf.InverseLerp(0f, (float)num, (float)i);
				List<Vector3> list2 = new List<Vector3>(pathToCurve);
				for (int j = count - 1; j > 0; j--)
				{
					for (int k = 0; k < j; k++)
					{
						list2[k] = (1f - num2) * list2[k] + num2 * list2[k + 1];
					}
				}
				list.Add(list2[0]);
			}
			return list;
		}

		// Token: 0x04003487 RID: 13447
		public static readonly Dictionary<string, PathManager> Paths = new Dictionary<string, PathManager>();
	}
}
