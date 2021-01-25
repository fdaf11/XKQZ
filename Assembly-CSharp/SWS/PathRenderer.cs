using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006B5 RID: 1717
	[RequireComponent(typeof(LineRenderer))]
	public class PathRenderer : MonoBehaviour
	{
		// Token: 0x06002967 RID: 10599 RVA: 0x0001B320 File Offset: 0x00019520
		private void Start()
		{
			this.line = base.GetComponent<LineRenderer>();
			this.path = base.GetComponent<PathManager>();
			if (this.path)
			{
				base.StartCoroutine("StartRenderer");
			}
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x00147DF0 File Offset: 0x00145FF0
		private IEnumerator StartRenderer()
		{
			this.Render();
			if (!this.onUpdate)
			{
				yield break;
			}
			for (;;)
			{
				yield return null;
				this.Render();
			}
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x00147E0C File Offset: 0x0014600C
		private void Render()
		{
			this.spacing = Mathf.Clamp01(this.spacing);
			if (this.spacing == 0f)
			{
				this.spacing = 0.05f;
			}
			List<Vector3> list = new List<Vector3>();
			list.AddRange(this.path.GetPathPoints());
			if (this.path.drawCurved)
			{
				list.Insert(0, list[0]);
				list.Add(list[list.Count - 1]);
				this.points = list.ToArray();
				this.DrawCurved();
			}
			else
			{
				this.points = list.ToArray();
				this.DrawLinear();
			}
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x00147EB8 File Offset: 0x001460B8
		private void DrawCurved()
		{
			int num = Mathf.RoundToInt(1f / this.spacing) + 1;
			this.line.SetVertexCount(num);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				this.line.SetPosition(i, WaypointManager.GetPoint(this.points, num2));
				num2 += this.spacing;
			}
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x00147F20 File Offset: 0x00146120
		private void DrawLinear()
		{
			this.line.SetVertexCount(this.points.Length);
			float num = 0f;
			for (int i = 0; i < this.points.Length; i++)
			{
				this.line.SetPosition(i, this.points[i]);
				num += this.spacing;
			}
		}

		// Token: 0x0400346F RID: 13423
		public bool onUpdate;

		// Token: 0x04003470 RID: 13424
		public float spacing = 0.05f;

		// Token: 0x04003471 RID: 13425
		private PathManager path;

		// Token: 0x04003472 RID: 13426
		private LineRenderer line;

		// Token: 0x04003473 RID: 13427
		private Vector3[] points;
	}
}
