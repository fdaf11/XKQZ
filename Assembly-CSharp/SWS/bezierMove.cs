using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006BB RID: 1723
	[AddComponentMenu("Simple Waypoint System/bezierMove")]
	public class bezierMove : MonoBehaviour
	{
		// Token: 0x06002987 RID: 10631 RVA: 0x0001B3E1 File Offset: 0x000195E1
		private void Start()
		{
			if (this.onStart)
			{
				this.StartMove();
			}
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x0014891C File Offset: 0x00146B1C
		private void InitWaypoints()
		{
			this.wpPos = new Vector3[this.waypoints.Length];
			for (int i = 0; i < this.wpPos.Length; i++)
			{
				this.wpPos[i] = this.waypoints[i] + new Vector3(0f, this.sizeToAdd, 0f);
			}
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x00148994 File Offset: 0x00146B94
		public void StartMove()
		{
			if (this.pathContainer == null)
			{
				Debug.LogWarning(base.gameObject.name + " has no path! Please set Path Container.");
				return;
			}
			this.waypoints = this.pathContainer.pathPoints;
			this.originSpeed = this.speed;
			this.Stop();
			base.StartCoroutine(this.Move());
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x00148A00 File Offset: 0x00146C00
		private IEnumerator Move()
		{
			if (this.moveToPath)
			{
				yield return base.StartCoroutine(this.MoveToPath());
			}
			else
			{
				this.InitWaypoints();
				base.transform.position = this.waypoints[0] + new Vector3(0f, this.sizeToAdd, 0f);
			}
			this.CreateTween();
			yield break;
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x00148A1C File Offset: 0x00146C1C
		private IEnumerator MoveToPath()
		{
			this.wpPos = new Vector3[7];
			this.wpPos[0] = base.transform.position;
			this.wpPos[1] = 2f * this.waypoints[0] - this.waypoints[1] + new Vector3(0f, this.sizeToAdd, 0f);
			this.wpPos[2] = 2f * this.waypoints[0] - this.waypoints[2] + new Vector3(0f, this.sizeToAdd, 0f);
			this.wpPos[3] = this.waypoints[0] + new Vector3(0f, this.sizeToAdd, 0f);
			List<Vector3> unsmoothedList = new List<Vector3>();
			for (int i = 0; i < 4; i++)
			{
				unsmoothedList.Add(this.wpPos[i]);
			}
			Vector3[] smoothed = WaypointManager.SmoothCurve(unsmoothedList, 1).ToArray();
			for (int j = 0; j < 4; j++)
			{
				this.wpPos[j] = smoothed[j];
			}
			this.wpPos[4] = this.waypoints[1] + new Vector3(0f, this.sizeToAdd, 0f);
			this.wpPos[5] = this.pathContainer.bPoints[1].wp.position + new Vector3(0f, this.sizeToAdd, 0f);
			if (this.pathContainer.bPoints.Count > 2)
			{
				this.wpPos[6] = this.pathContainer.bPoints[2].wp.position + new Vector3(0f, this.sizeToAdd, 0f);
			}
			else
			{
				this.wpPos[6] = this.wpPos[5];
			}
			this.CreateTween();
			yield return base.StartCoroutine(this.tween.UsePartialPath(-1, 3).WaitForCompletion());
			this.moveToPath = false;
			this.tween.Kill();
			this.tween = null;
			this.InitWaypoints();
			yield break;
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x00148A38 File Offset: 0x00146C38
		private void CreateTween()
		{
			this.plugPath = null;
			this.plugPath = new PlugVector3Path(this.wpPos, true, this.pathType);
			if (this.orientToPath)
			{
				this.plugPath.OrientToPath(this.lookAhead, this.lockAxis);
			}
			if (this.lockPosition != null)
			{
				this.plugPath.LockPosition(this.lockPosition);
			}
			this.tParms = new TweenParms();
			this.tParms.Prop("position", this.plugPath);
			this.tParms.AutoKill(false);
			this.tParms.Loops(1);
			if (this.timeValue == bezierMove.TimeValue.speed)
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
			if (!this.moveToPath)
			{
				this.tParms.OnUpdate(new TweenDelegate.TweenCallback(this.CheckPoint));
				this.tParms.OnComplete(new TweenDelegate.TweenCallback(this.ReachedEnd));
			}
			this.tween = HOTween.To(base.transform, this.originSpeed, this.tParms);
			if (this.originSpeed != this.speed)
			{
				this.ChangeSpeed(this.speed);
			}
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x00148BB8 File Offset: 0x00146DB8
		private void CheckPoint()
		{
			float num = this.positionOnPath;
			this.positionOnPath = this.tween.fullElapsed / this.tween.fullDuration;
			for (int i = 0; i < this.messages.list.Count; i++)
			{
				if (num < this.messages.list[i].pos && this.positionOnPath >= this.messages.list[i].pos && num != this.positionOnPath)
				{
					this.messages.Execute(this, i);
				}
			}
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x00148C60 File Offset: 0x00146E60
		public IEnumerator Wait(float value)
		{
			this.tween.Pause();
			float timer = Time.time + value;
			while (Time.time < timer)
			{
				yield return null;
			}
			if (this.positionOnPath < 1f && this.positionOnPath != -1f)
			{
				this.Resume();
			}
			yield break;
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x00148C8C File Offset: 0x00146E8C
		private void ReachedEnd()
		{
			this.positionOnPath = -1f;
			switch (this.loopType)
			{
			case bezierMove.LoopType.none:
				if (this.tween != null)
				{
					this.tween.Kill();
				}
				this.tween = null;
				break;
			case bezierMove.LoopType.loop:
				this.Stop();
				this.StartMove();
				break;
			case bezierMove.LoopType.pingPong:
			{
				if (this.tween != null)
				{
					this.tween.Kill();
				}
				this.tween = null;
				Vector3[] array = new Vector3[this.wpPos.Length];
				Array.Copy(this.wpPos, array, this.wpPos.Length);
				for (int i = 0; i < this.wpPos.Length; i++)
				{
					this.wpPos[i] = array[this.wpPos.Length - 1 - i];
				}
				MessageOptions[] array2 = new MessageOptions[this.messages.list.Count];
				this.messages.list.CopyTo(array2);
				for (int j = 0; j < this.messages.list.Count; j++)
				{
					this.messages.list[j].pos = 1f - this.messages.list[j].pos;
					this.messages.list[j] = array2[array2.Length - 1 - j];
				}
				this.CreateTween();
				break;
			}
			}
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x0001B3F4 File Offset: 0x000195F4
		public void SetPath(BezierPathManager newPath)
		{
			this.Stop();
			this.pathContainer = newPath;
			this.StartMove();
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x0001B409 File Offset: 0x00019609
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

		// Token: 0x06002992 RID: 10642 RVA: 0x00148E18 File Offset: 0x00147018
		public void ResetMove()
		{
			this.Stop();
			if (this.pathContainer)
			{
				base.transform.position = this.waypoints[0] + new Vector3(0f, this.sizeToAdd, 0f);
			}
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x0001B435 File Offset: 0x00019635
		public void Pause()
		{
			if (this.tween != null)
			{
				this.tween.Pause();
			}
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x0001B44D File Offset: 0x0001964D
		public void Resume()
		{
			if (this.tween != null)
			{
				this.tween.Play();
			}
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x00148E74 File Offset: 0x00147074
		public void ChangeSpeed(float value)
		{
			float timeScale;
			if (this.timeValue == bezierMove.TimeValue.speed)
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

		// Token: 0x06002996 RID: 10646 RVA: 0x00148EC4 File Offset: 0x001470C4
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.magenta;
			if (this.tween == null || this.moveToPath)
			{
				return;
			}
			for (int i = 0; i < this.messages.list.Count; i++)
			{
				Gizmos.DrawSphere(this.tween.GetPointOnPath(this.messages.list[i].pos), 0.2f);
			}
		}

		// Token: 0x04003488 RID: 13448
		public BezierPathManager pathContainer;

		// Token: 0x04003489 RID: 13449
		public PathType pathType = 1;

		// Token: 0x0400348A RID: 13450
		public bool onStart;

		// Token: 0x0400348B RID: 13451
		public bool moveToPath;

		// Token: 0x0400348C RID: 13452
		public bool orientToPath;

		// Token: 0x0400348D RID: 13453
		public float lookAhead;

		// Token: 0x0400348E RID: 13454
		public float sizeToAdd;

		// Token: 0x0400348F RID: 13455
		[HideInInspector]
		public Messages messages = new Messages();

		// Token: 0x04003490 RID: 13456
		public bezierMove.TimeValue timeValue = bezierMove.TimeValue.speed;

		// Token: 0x04003491 RID: 13457
		public float speed = 5f;

		// Token: 0x04003492 RID: 13458
		public EaseType easeType;

		// Token: 0x04003493 RID: 13459
		public AnimationCurve animEaseType;

		// Token: 0x04003494 RID: 13460
		public bezierMove.LoopType loopType;

		// Token: 0x04003495 RID: 13461
		private Vector3[] waypoints;

		// Token: 0x04003496 RID: 13462
		public Axis lockAxis = 2;

		// Token: 0x04003497 RID: 13463
		public Axis lockPosition;

		// Token: 0x04003498 RID: 13464
		public Tweener tween;

		// Token: 0x04003499 RID: 13465
		private Vector3[] wpPos;

		// Token: 0x0400349A RID: 13466
		private TweenParms tParms;

		// Token: 0x0400349B RID: 13467
		private PlugVector3Path plugPath;

		// Token: 0x0400349C RID: 13468
		private float positionOnPath = -1f;

		// Token: 0x0400349D RID: 13469
		private float originSpeed;

		// Token: 0x020006BC RID: 1724
		public enum TimeValue
		{
			// Token: 0x0400349F RID: 13471
			time,
			// Token: 0x040034A0 RID: 13472
			speed
		}

		// Token: 0x020006BD RID: 1725
		public enum LoopType
		{
			// Token: 0x040034A2 RID: 13474
			none,
			// Token: 0x040034A3 RID: 13475
			loop,
			// Token: 0x040034A4 RID: 13476
			pingPong
		}
	}
}
