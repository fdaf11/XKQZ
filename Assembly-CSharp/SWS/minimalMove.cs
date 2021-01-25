using System;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006C1 RID: 1729
	[AddComponentMenu("Simple Waypoint System/minimalMove")]
	public class minimalMove : MonoBehaviour
	{
		// Token: 0x060029AA RID: 10666 RVA: 0x0001B4C0 File Offset: 0x000196C0
		private void Start()
		{
			if (this.onStart)
			{
				this.StartMove();
			}
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x001494E4 File Offset: 0x001476E4
		public void StartMove()
		{
			if (this.pathContainer == null)
			{
				Debug.LogWarning(base.gameObject.name + " has no path! Please set Path Container.");
				return;
			}
			this.waypoints = this.pathContainer.GetPathPoints();
			this.originSpeed = this.speed;
			this.Stop();
			base.StartCoroutine(this.Move());
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x00149550 File Offset: 0x00147750
		private IEnumerator Move()
		{
			if (this.moveToPath)
			{
				yield return base.StartCoroutine(this.MoveToPath());
			}
			else
			{
				base.transform.position = this.waypoints[0];
			}
			this.CreateTween();
			yield break;
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x0014956C File Offset: 0x0014776C
		private IEnumerator MoveToPath()
		{
			int max = (this.waypoints.Length <= 4) ? this.waypoints.Length : 4;
			Vector3[] wpPos = this.pathContainer.GetPathPoints();
			this.waypoints = new Vector3[max];
			for (int i = 1; i < max; i++)
			{
				this.waypoints[i] = wpPos[i - 1];
			}
			this.waypoints[0] = base.transform.position;
			this.CreateTween();
			if (this.tween.isPaused)
			{
				this.tween.Play();
			}
			this.waypoints = this.pathContainer.GetPathPoints();
			yield return base.StartCoroutine(this.tween.UsePartialPath(-1, 1).WaitForCompletion());
			this.moveToPath = false;
			if (this.tween != null)
			{
				this.tween.Kill();
			}
			this.tween = null;
			yield break;
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x00149588 File Offset: 0x00147788
		private void CreateTween()
		{
			this.plugPath = new PlugVector3Path(this.waypoints, true, this.pathType);
			if (this.orientToPath)
			{
				this.plugPath.OrientToPath(this.lookAhead, this.lockAxis);
			}
			if (this.lockPosition != null)
			{
				this.plugPath.LockPosition(this.lockPosition);
			}
			if (this.loopType == minimalMove.LoopType.loop && this.closeLoop)
			{
				this.plugPath.ClosePath(true);
			}
			this.tParms = new TweenParms();
			this.tParms.Prop("position", this.plugPath);
			this.tParms.AutoKill(false);
			this.tParms.Loops(1);
			if (!this.moveToPath)
			{
				this.tParms.OnComplete(new TweenDelegate.TweenCallback(this.ReachedEnd));
			}
			if (this.timeValue == minimalMove.TimeValue.speed)
			{
				this.tParms.SpeedBased();
				this.tParms.Ease(0);
			}
			else if (this.easeType == 31)
			{
				this.tParms.Ease(this.animEaseType);
			}
			else
			{
				this.tParms.Ease(this.easeType);
			}
			this.tween = HOTween.To(base.transform, this.originSpeed, this.tParms);
			if (this.originSpeed != this.speed)
			{
				this.ChangeSpeed(this.speed);
			}
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x0014970C File Offset: 0x0014790C
		private void ReachedEnd()
		{
			switch (this.loopType)
			{
			case minimalMove.LoopType.none:
				if (this.tween != null)
				{
					this.tween.Kill();
				}
				this.tween = null;
				return;
			case minimalMove.LoopType.loop:
				this.tween.Restart();
				break;
			case minimalMove.LoopType.pingPong:
				if (this.tween != null)
				{
					this.tween.Kill();
				}
				this.tween = null;
				this.repeat = !this.repeat;
				Array.Reverse(this.waypoints);
				this.CreateTween();
				break;
			}
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x0001B4D3 File Offset: 0x000196D3
		public void SetPath(PathManager newPath)
		{
			this.Stop();
			this.pathContainer = newPath;
			this.StartMove();
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x0001B4E8 File Offset: 0x000196E8
		public void Stop()
		{
			base.StopAllCoroutines();
			if (this.tween != null)
			{
				this.tween.Kill();
			}
			this.plugPath = null;
			this.tween = null;
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x0001B514 File Offset: 0x00019714
		public void ResetMove()
		{
			this.Stop();
			if (this.pathContainer)
			{
				base.transform.position = this.pathContainer.waypoints[0].position;
			}
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x0001B549 File Offset: 0x00019749
		public void Pause()
		{
			if (this.tween != null)
			{
				this.tween.Pause();
			}
		}

		// Token: 0x060029B4 RID: 10676 RVA: 0x0001B561 File Offset: 0x00019761
		public void Resume()
		{
			if (this.tween != null)
			{
				this.tween.Play();
			}
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x001497A8 File Offset: 0x001479A8
		public void ChangeSpeed(float value)
		{
			float timeScale;
			if (this.timeValue == minimalMove.TimeValue.speed)
			{
				timeScale = value / this.originSpeed;
			}
			else
			{
				timeScale = this.originSpeed / value;
			}
			this.speed = value;
			if (this.tween != null)
			{
				this.tween.timeScale = timeScale;
			}
		}

		// Token: 0x040034B5 RID: 13493
		public PathManager pathContainer;

		// Token: 0x040034B6 RID: 13494
		public PathType pathType = 1;

		// Token: 0x040034B7 RID: 13495
		public bool onStart;

		// Token: 0x040034B8 RID: 13496
		public bool moveToPath;

		// Token: 0x040034B9 RID: 13497
		public bool closeLoop;

		// Token: 0x040034BA RID: 13498
		public bool orientToPath;

		// Token: 0x040034BB RID: 13499
		public float lookAhead;

		// Token: 0x040034BC RID: 13500
		public minimalMove.TimeValue timeValue = minimalMove.TimeValue.speed;

		// Token: 0x040034BD RID: 13501
		public float speed = 5f;

		// Token: 0x040034BE RID: 13502
		public EaseType easeType;

		// Token: 0x040034BF RID: 13503
		public AnimationCurve animEaseType;

		// Token: 0x040034C0 RID: 13504
		public minimalMove.LoopType loopType;

		// Token: 0x040034C1 RID: 13505
		[HideInInspector]
		public Vector3[] waypoints;

		// Token: 0x040034C2 RID: 13506
		[HideInInspector]
		public bool repeat;

		// Token: 0x040034C3 RID: 13507
		public Axis lockAxis = 2;

		// Token: 0x040034C4 RID: 13508
		public Axis lockPosition;

		// Token: 0x040034C5 RID: 13509
		public Tweener tween;

		// Token: 0x040034C6 RID: 13510
		private TweenParms tParms;

		// Token: 0x040034C7 RID: 13511
		private PlugVector3Path plugPath;

		// Token: 0x040034C8 RID: 13512
		private float originSpeed;

		// Token: 0x020006C2 RID: 1730
		public enum TimeValue
		{
			// Token: 0x040034CA RID: 13514
			time,
			// Token: 0x040034CB RID: 13515
			speed
		}

		// Token: 0x020006C3 RID: 1731
		public enum LoopType
		{
			// Token: 0x040034CD RID: 13517
			none,
			// Token: 0x040034CE RID: 13518
			loop,
			// Token: 0x040034CF RID: 13519
			pingPong
		}
	}
}
