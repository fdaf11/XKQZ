using System;
using System.Collections;
using AIBehavior;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x0200000B RID: 11
	[RequireComponent(typeof(LineRenderer))]
	public class ExampleLaserAttackComponent : MonoBehaviour
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002664 File Offset: 0x00000864
		private void Awake()
		{
			this.lineRenderer = base.GetComponent<LineRenderer>();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00021F38 File Offset: 0x00020138
		public void LaserAttack(AttackData attackData)
		{
			if (attackData.target != null)
			{
				AIBehaviors component = attackData.target.GetComponent<AIBehaviors>();
				this.lineRenderer.SetVertexCount(2);
				this.lineRenderer.SetPosition(0, this.shootPoint.position);
				this.lineRenderer.SetPosition(1, attackData.target.position);
				this.hideTime = Time.time + this.laserVisibleDuration;
				base.StartCoroutine(this.HideRenderer());
				if (component != null)
				{
					component.GotHit(attackData.damage);
				}
			}
			else
			{
				Debug.LogWarning("attackData.target is null, you may want to have a NoPlayerInSight trigger on the AI '" + attackData.attackState.transform.parent.name + "'");
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00022008 File Offset: 0x00020208
		private IEnumerator HideRenderer()
		{
			if (!this.laserShowing)
			{
				this.laserShowing = true;
				this.lineRenderer.SetColors(Color.red, Color.red);
				while (Time.time < this.hideTime)
				{
					yield return null;
				}
				this.lineRenderer.SetColors(Color.clear, Color.clear);
				this.lineRenderer.SetVertexCount(0);
				this.laserShowing = false;
			}
			yield break;
		}

		// Token: 0x0400000E RID: 14
		public Transform shootPoint;

		// Token: 0x0400000F RID: 15
		private LineRenderer lineRenderer;

		// Token: 0x04000010 RID: 16
		private float hideTime;

		// Token: 0x04000011 RID: 17
		private float laserVisibleDuration = 0.1f;

		// Token: 0x04000012 RID: 18
		private bool laserShowing;
	}
}
