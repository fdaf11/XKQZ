using System;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006CD RID: 1741
	[AddComponentMenu("Simple Waypoint System/splineMove")]
	public class splineMove : MonoBehaviour
	{
		// Token: 0x060029F2 RID: 10738 RVA: 0x0001B751 File Offset: 0x00019951
		private void Start()
		{
			if (this.onStart)
			{
				this.StartMove();
			}
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x0014A61C File Offset: 0x0014881C
		private void InitWaypoints()
		{
			this.wpPos = new Vector3[this.waypoints.Length];
			for (int i = 0; i < this.wpPos.Length; i++)
			{
				this.wpPos[i] = this.waypoints[i].position + new Vector3(0f, this.sizeToAdd, 0f);
			}
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x0014A690 File Offset: 0x00148890
		public void StartMove()
		{
			if (this.pathContainer == null)
			{
				Debug.LogWarning(base.gameObject.name + " has no path! Please set Path Container.");
				return;
			}
			this.waypoints = this.pathContainer.waypoints;
			this.originSpeed = this.speed;
			if (this.delays == null)
			{
				this.delays = new float[this.waypoints.Length];
			}
			else if (this.delays.Length < this.waypoints.Length)
			{
				float[] array = new float[this.delays.Length];
				Array.Copy(this.delays, array, this.delays.Length);
				this.delays = new float[this.waypoints.Length];
				Array.Copy(array, this.delays, array.Length);
			}
			if (this.messages.list.Count > 0)
			{
				this.messages.Initialize(this.waypoints.Length);
			}
			this.Stop();
			if (this.currentPoint > 0)
			{
				this.Teleport(this.currentPoint);
			}
			else
			{
				base.StartCoroutine(this.Move());
			}
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x0014A7BC File Offset: 0x001489BC
		private IEnumerator Move()
		{
			if (this.moveToPath)
			{
				yield return base.StartCoroutine(this.MoveToPath());
			}
			else
			{
				this.InitWaypoints();
				if (this.currentPoint == this.waypoints.Length - 1)
				{
					base.transform.position = this.wpPos[this.currentPoint];
				}
				else
				{
					base.transform.position = this.wpPos[0];
				}
			}
			if (this.loopType == splineMove.LoopType.random)
			{
				base.StartCoroutine(this.ReachedEnd());
			}
			else
			{
				this.CreateTween();
				base.StartCoroutine(this.NextWaypoint());
			}
			yield break;
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x0014A7D8 File Offset: 0x001489D8
		private IEnumerator MoveToPath()
		{
			int max = (this.waypoints.Length <= 4) ? this.waypoints.Length : 4;
			this.wpPos = new Vector3[max];
			for (int i = 1; i < max; i++)
			{
				this.wpPos[i] = this.waypoints[i - 1].position + new Vector3(0f, this.sizeToAdd, 0f);
			}
			this.wpPos[0] = base.transform.position;
			this.CreateTween();
			if (this.tween.isPaused)
			{
				this.tween.Play();
			}
			yield return base.StartCoroutine(this.tween.UsePartialPath(-1, 1).WaitForCompletion());
			this.moveToPath = false;
			if (this.tween != null)
			{
				this.tween.Kill();
			}
			this.tween = null;
			this.InitWaypoints();
			yield break;
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x0014A7F4 File Offset: 0x001489F4
		private void CreateTween()
		{
			this.plugPath = new PlugVector3Path(this.wpPos, true, this.pathType);
			if (this.orientToPath)
			{
				this.plugPath.OrientToPath(this.lookAhead, this.lockAxis);
			}
			if (this.lockPosition != null)
			{
				this.plugPath.LockPosition(this.lockPosition);
			}
			if (this.loopType == splineMove.LoopType.loop && this.closeLoop)
			{
				this.plugPath.ClosePath(true);
			}
			this.tParms = new TweenParms();
			if (this.local)
			{
				this.tParms.Prop("localPosition", this.plugPath);
			}
			else
			{
				this.tParms.Prop("position", this.plugPath);
			}
			this.tParms.AutoKill(false);
			this.tParms.Pause(true);
			this.tParms.Loops(1);
			if (this.timeValue == splineMove.TimeValue.speed)
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

		// Token: 0x060029F8 RID: 10744 RVA: 0x0014A988 File Offset: 0x00148B88
		private IEnumerator NextWaypoint()
		{
			for (int point = this.startingPoint; point < this.wpPos.Length - 1; point++)
			{
				this.messages.Execute(this, this.currentPoint);
				if (this.delays[this.currentPoint] > 0f)
				{
					yield return base.StartCoroutine(this.WaitDelay());
				}
				if (this.tween == null)
				{
					yield break;
				}
				while (this.waiting)
				{
					yield return null;
				}
				this.tween.Play();
				yield return base.StartCoroutine(this.tween.UsePartialPath(point, point + 1).WaitForCompletion());
				if (this.loopType == splineMove.LoopType.pingPong && this.repeat)
				{
					this.currentPoint--;
				}
				else if (this.loopType == splineMove.LoopType.random)
				{
					this.rndIndex++;
					this.currentPoint = this.rndArray[this.rndIndex];
				}
				else
				{
					this.currentPoint++;
				}
			}
			if (this.loopType != splineMove.LoopType.pingPong && this.loopType != splineMove.LoopType.random)
			{
				this.messages.Execute(this, this.currentPoint);
				if (this.delays[this.currentPoint] > 0f)
				{
					yield return base.StartCoroutine(this.WaitDelay());
				}
			}
			this.startingPoint = 0;
			base.StartCoroutine(this.ReachedEnd());
			yield break;
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x0014A9A4 File Offset: 0x00148BA4
		private IEnumerator WaitDelay()
		{
			this.tween.Pause();
			float timer = Time.time + this.delays[this.currentPoint];
			while (!this.waiting && Time.time < timer)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x0014A9C0 File Offset: 0x00148BC0
		private IEnumerator ReachedEnd()
		{
			switch (this.loopType)
			{
			case splineMove.LoopType.none:
				if (this.tween != null)
				{
					this.tween.Kill();
				}
				this.tween = null;
				yield break;
			case splineMove.LoopType.loop:
				if (this.closeLoop)
				{
					this.tween.Play();
					yield return base.StartCoroutine(this.tween.UsePartialPath(this.currentPoint, -1).WaitForCompletion());
				}
				this.currentPoint = 0;
				break;
			case splineMove.LoopType.pingPong:
				if (this.tween != null)
				{
					this.tween.Kill();
				}
				this.tween = null;
				if (!this.repeat)
				{
					this.repeat = true;
					for (int i = 0; i < this.wpPos.Length; i++)
					{
						this.wpPos[i] = this.waypoints[this.waypoints.Length - 1 - i].position + new Vector3(0f, this.sizeToAdd, 0f);
					}
				}
				else
				{
					this.InitWaypoints();
					this.repeat = false;
				}
				this.CreateTween();
				break;
			case splineMove.LoopType.random:
			{
				this.rndIndex = 0;
				this.InitWaypoints();
				if (this.tween != null)
				{
					this.tween.Kill();
				}
				this.tween = null;
				this.rndArray = new int[this.wpPos.Length];
				for (int j = 0; j < this.rndArray.Length; j++)
				{
					this.rndArray[j] = j;
				}
				int k = this.wpPos.Length;
				while (k > 1)
				{
					Random random = this.rand;
					int num;
					k = (num = k) - 1;
					int l = random.Next(num);
					Vector3 temp = this.wpPos[k];
					this.wpPos[k] = this.wpPos[l];
					this.wpPos[l] = temp;
					int tmpI = this.rndArray[k];
					this.rndArray[k] = this.rndArray[l];
					this.rndArray[l] = tmpI;
				}
				Vector3 first = this.wpPos[0];
				int rndFirst = this.rndArray[0];
				for (int m = 0; m < this.wpPos.Length; m++)
				{
					if (this.rndArray[m] == this.currentPoint)
					{
						this.rndArray[m] = rndFirst;
						this.wpPos[0] = this.wpPos[m];
						this.wpPos[m] = first;
					}
				}
				this.rndArray[0] = this.currentPoint;
				this.CreateTween();
				break;
			}
			}
			base.StartCoroutine(this.NextWaypoint());
			yield break;
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x0001B764 File Offset: 0x00019964
		public void SetPath(PathManager newPath)
		{
			this.Stop();
			this.pathContainer = newPath;
			this.StartMove();
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x0014A9DC File Offset: 0x00148BDC
		public void Teleport(int index)
		{
			if (this.loopType == splineMove.LoopType.random)
			{
				Debug.LogWarning("Teleporting doesn't work with looptype set to 'random'. Resetting.");
				index = 0;
			}
			index = Mathf.Clamp(index, 0, this.waypoints.Length - 1);
			this.Resume();
			this.Stop();
			this.moveToPath = false;
			if (this.loopType == splineMove.LoopType.loop && index == this.waypoints.Length - 1)
			{
				index = 0;
			}
			this.currentPoint = (this.startingPoint = index);
			base.StartCoroutine(this.Move());
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x0001B779 File Offset: 0x00019979
		public void Stop()
		{
			base.StopAllCoroutines();
			this.currentPoint = 0;
			if (this.tween != null)
			{
				this.tween.Kill();
			}
			this.plugPath = null;
			this.tween = null;
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x0014AA64 File Offset: 0x00148C64
		public void ResetMove()
		{
			this.Stop();
			if (this.pathContainer)
			{
				base.transform.position = this.pathContainer.waypoints[this.currentPoint].position + new Vector3(0f, this.sizeToAdd, 0f);
			}
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x0001B7AC File Offset: 0x000199AC
		public void Pause()
		{
			this.waiting = true;
			if (this.tween != null)
			{
				this.tween.Pause();
			}
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x0001B7CB File Offset: 0x000199CB
		public void Resume()
		{
			this.waiting = false;
			if (this.tween != null)
			{
				this.tween.Play();
			}
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x0014AAC4 File Offset: 0x00148CC4
		public void ChangeSpeed(float value)
		{
			float timeScale;
			if (this.timeValue == splineMove.TimeValue.speed)
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

		// Token: 0x06002A02 RID: 10754 RVA: 0x0001B7EA File Offset: 0x000199EA
		public void SetDelay(int index, float value)
		{
			if (this.delays == null)
			{
				this.delays = new float[this.waypoints.Length];
			}
			this.delays[index] = value;
		}

		// Token: 0x04003505 RID: 13573
		public PathManager pathContainer;

		// Token: 0x04003506 RID: 13574
		public PathType pathType = 1;

		// Token: 0x04003507 RID: 13575
		public int currentPoint;

		// Token: 0x04003508 RID: 13576
		public bool onStart;

		// Token: 0x04003509 RID: 13577
		public bool moveToPath;

		// Token: 0x0400350A RID: 13578
		public bool closeLoop;

		// Token: 0x0400350B RID: 13579
		public bool orientToPath;

		// Token: 0x0400350C RID: 13580
		public bool local;

		// Token: 0x0400350D RID: 13581
		public float lookAhead;

		// Token: 0x0400350E RID: 13582
		public float sizeToAdd;

		// Token: 0x0400350F RID: 13583
		[HideInInspector]
		public float[] delays;

		// Token: 0x04003510 RID: 13584
		[HideInInspector]
		public Messages messages = new Messages();

		// Token: 0x04003511 RID: 13585
		public splineMove.TimeValue timeValue = splineMove.TimeValue.speed;

		// Token: 0x04003512 RID: 13586
		public float speed = 5f;

		// Token: 0x04003513 RID: 13587
		public EaseType easeType;

		// Token: 0x04003514 RID: 13588
		public AnimationCurve animEaseType;

		// Token: 0x04003515 RID: 13589
		public splineMove.LoopType loopType;

		// Token: 0x04003516 RID: 13590
		[HideInInspector]
		public Transform[] waypoints;

		// Token: 0x04003517 RID: 13591
		[HideInInspector]
		public bool repeat;

		// Token: 0x04003518 RID: 13592
		public Axis lockAxis = 2;

		// Token: 0x04003519 RID: 13593
		public Axis lockPosition;

		// Token: 0x0400351A RID: 13594
		public Tweener tween;

		// Token: 0x0400351B RID: 13595
		private Vector3[] wpPos;

		// Token: 0x0400351C RID: 13596
		private TweenParms tParms;

		// Token: 0x0400351D RID: 13597
		private PlugVector3Path plugPath;

		// Token: 0x0400351E RID: 13598
		private Random rand = new Random();

		// Token: 0x0400351F RID: 13599
		private int[] rndArray;

		// Token: 0x04003520 RID: 13600
		private int rndIndex;

		// Token: 0x04003521 RID: 13601
		private bool waiting;

		// Token: 0x04003522 RID: 13602
		private float originSpeed;

		// Token: 0x04003523 RID: 13603
		private int startingPoint;

		// Token: 0x020006CE RID: 1742
		public enum TimeValue
		{
			// Token: 0x04003525 RID: 13605
			time,
			// Token: 0x04003526 RID: 13606
			speed
		}

		// Token: 0x020006CF RID: 1743
		public enum LoopType
		{
			// Token: 0x04003528 RID: 13608
			none,
			// Token: 0x04003529 RID: 13609
			loop,
			// Token: 0x0400352A RID: 13610
			pingPong,
			// Token: 0x0400352B RID: 13611
			random
		}
	}
}
