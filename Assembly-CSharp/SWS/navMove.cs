using System;
using System.Collections;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006C6 RID: 1734
	[AddComponentMenu("Simple Waypoint System/navMove")]
	[RequireComponent(typeof(NavMeshAgent))]
	public class navMove : MonoBehaviour
	{
		// Token: 0x060029C3 RID: 10691 RVA: 0x0001B5C0 File Offset: 0x000197C0
		private void Start()
		{
			this.agent = base.GetComponent<NavMeshAgent>();
			if (this.onStart)
			{
				this.StartMove();
			}
		}

		// Token: 0x060029C4 RID: 10692 RVA: 0x00149A70 File Offset: 0x00147C70
		public void StartMove()
		{
			if (this.pathContainer == null)
			{
				Debug.LogWarning(base.gameObject.name + " has no path! Please set Path Container.");
				return;
			}
			this.waypoints = new Transform[this.pathContainer.waypoints.Length];
			Array.Copy(this.pathContainer.waypoints, this.waypoints, this.pathContainer.waypoints.Length);
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
			this.Stop(false);
			base.StartCoroutine(this.Move());
		}

		// Token: 0x060029C5 RID: 10693 RVA: 0x00149BA0 File Offset: 0x00147DA0
		private IEnumerator Move()
		{
			this.agent.Resume();
			this.agent.updateRotation = this.updateRotation;
			if (this.moveToPath)
			{
				if (this.loopType == navMove.LoopType.random)
				{
					this.agent.SetDestination(this.waypoints[this.rand.Next(0, this.pathContainer.waypoints.Length)].position);
				}
				else
				{
					this.agent.SetDestination(this.waypoints[this.currentPoint].position);
				}
				yield return base.StartCoroutine(this.WaitForDestination());
				this.moveToPath = false;
			}
			else
			{
				this.agent.Warp(this.waypoints[this.currentPoint].position);
			}
			if (this.loopType == navMove.LoopType.random)
			{
				base.StartCoroutine(this.ReachedEnd());
			}
			else
			{
				base.StartCoroutine(this.NextWaypoint());
			}
			yield break;
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x00149BBC File Offset: 0x00147DBC
		private IEnumerator NextWaypoint()
		{
			this.messages.Execute(this, this.currentPoint);
			yield return new WaitForEndOfFrame();
			if (this.delays[this.currentPoint] > 0f)
			{
				yield return base.StartCoroutine(this.WaitDelay());
			}
			while (this.waiting)
			{
				yield return null;
			}
			Transform next = null;
			if (this.loopType == navMove.LoopType.pingPong && this.repeat)
			{
				this.currentPoint--;
			}
			else if (this.loopType == navMove.LoopType.random)
			{
				this.rndIndex++;
				this.currentPoint = int.Parse(this.waypoints[this.rndIndex].name.Replace("Waypoint ", string.Empty));
				next = this.waypoints[this.rndIndex];
			}
			else
			{
				this.currentPoint++;
			}
			this.currentPoint = Mathf.Clamp(this.currentPoint, 0, this.waypoints.Length - 1);
			if (next == null)
			{
				next = this.waypoints[this.currentPoint];
			}
			this.agent.SetDestination(next.position);
			yield return base.StartCoroutine(this.WaitForDestination());
			if ((this.loopType != navMove.LoopType.random && this.currentPoint == this.waypoints.Length - 1) || this.rndIndex == this.waypoints.Length - 1 || (this.repeat && this.currentPoint == 0))
			{
				base.StartCoroutine(this.ReachedEnd());
			}
			else
			{
				base.StartCoroutine(this.NextWaypoint());
			}
			yield break;
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x00149BD8 File Offset: 0x00147DD8
		private IEnumerator WaitForDestination()
		{
			while (this.agent.pathPending)
			{
				yield return null;
			}
			float remain = this.agent.remainingDistance;
			while (remain == float.PositiveInfinity || remain - this.agent.stoppingDistance > 1E-45f || this.agent.pathStatus != null)
			{
				remain = this.agent.remainingDistance;
				yield return null;
			}
			yield break;
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x00149BF4 File Offset: 0x00147DF4
		private IEnumerator WaitDelay()
		{
			float timer = Time.time + this.delays[this.currentPoint];
			while (!this.waiting && Time.time < timer)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060029C9 RID: 10697 RVA: 0x00149C10 File Offset: 0x00147E10
		private IEnumerator ReachedEnd()
		{
			switch (this.loopType)
			{
			case navMove.LoopType.none:
				this.messages.Execute(this, this.currentPoint);
				if (this.delays[this.currentPoint] > 0f)
				{
					yield return base.StartCoroutine(this.WaitDelay());
				}
				yield break;
			case navMove.LoopType.loop:
				this.messages.Execute(this, this.currentPoint);
				if (this.delays[this.currentPoint] > 0f)
				{
					yield return base.StartCoroutine(this.WaitDelay());
				}
				if (this.closeLoop)
				{
					this.agent.SetDestination(this.waypoints[0].position);
					yield return base.StartCoroutine(this.WaitForDestination());
				}
				else
				{
					this.agent.Warp(this.waypoints[0].position);
				}
				this.currentPoint = 0;
				break;
			case navMove.LoopType.pingPong:
				this.repeat = !this.repeat;
				break;
			case navMove.LoopType.random:
			{
				Array.Copy(this.pathContainer.waypoints, this.waypoints, this.pathContainer.waypoints.Length);
				int i = this.waypoints.Length;
				while (i > 1)
				{
					Random random = this.rand;
					int num;
					i = (num = i) - 1;
					int j = random.Next(num);
					Transform temp = this.waypoints[i];
					this.waypoints[i] = this.waypoints[j];
					this.waypoints[j] = temp;
				}
				Transform first = this.pathContainer.waypoints[this.currentPoint];
				for (int k = 0; k < this.waypoints.Length; k++)
				{
					if (this.waypoints[k] == first)
					{
						Transform temp2 = this.waypoints[0];
						this.waypoints[0] = this.waypoints[k];
						this.waypoints[k] = temp2;
						break;
					}
				}
				this.rndIndex = 0;
				break;
			}
			}
			base.StartCoroutine(this.NextWaypoint());
			yield break;
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x0001B5DF File Offset: 0x000197DF
		public void SetPath(PathManager newPath)
		{
			this.Stop(true);
			this.pathContainer = newPath;
			this.StartMove();
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x0001B5F5 File Offset: 0x000197F5
		public void Stop()
		{
			this.Stop(false);
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x0001B5FE File Offset: 0x000197FE
		public void Stop(bool stopUpdates)
		{
			base.StopAllCoroutines();
			this.currentPoint = 0;
			this.agent.Stop(stopUpdates);
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x0001B619 File Offset: 0x00019819
		public void ResetMove()
		{
			this.Stop(true);
			if (this.pathContainer)
			{
				this.agent.Warp(this.pathContainer.waypoints[this.currentPoint].position);
			}
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x0001B655 File Offset: 0x00019855
		public void Pause()
		{
			this.Pause(false);
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x0001B65E File Offset: 0x0001985E
		public void Pause(bool stopUpdates)
		{
			this.waiting = true;
			this.agent.Stop(stopUpdates);
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x0001B673 File Offset: 0x00019873
		public void Resume()
		{
			this.waiting = false;
			this.agent.Resume();
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x0001B687 File Offset: 0x00019887
		public void ChangeSpeed(float value)
		{
			this.agent.speed = value;
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x0001B695 File Offset: 0x00019895
		public void SetDelay(int index, float value)
		{
			if (this.delays == null)
			{
				this.delays = new float[this.waypoints.Length];
			}
			this.delays[index] = value;
		}

		// Token: 0x040034D9 RID: 13529
		public PathManager pathContainer;

		// Token: 0x040034DA RID: 13530
		public int currentPoint;

		// Token: 0x040034DB RID: 13531
		public bool onStart;

		// Token: 0x040034DC RID: 13532
		public bool moveToPath;

		// Token: 0x040034DD RID: 13533
		public bool closeLoop;

		// Token: 0x040034DE RID: 13534
		public bool updateRotation = true;

		// Token: 0x040034DF RID: 13535
		[HideInInspector]
		public float[] delays;

		// Token: 0x040034E0 RID: 13536
		[HideInInspector]
		public Messages messages = new Messages();

		// Token: 0x040034E1 RID: 13537
		public navMove.LoopType loopType;

		// Token: 0x040034E2 RID: 13538
		[HideInInspector]
		public Transform[] waypoints;

		// Token: 0x040034E3 RID: 13539
		[HideInInspector]
		public bool repeat;

		// Token: 0x040034E4 RID: 13540
		private NavMeshAgent agent;

		// Token: 0x040034E5 RID: 13541
		private Random rand = new Random();

		// Token: 0x040034E6 RID: 13542
		private int rndIndex;

		// Token: 0x040034E7 RID: 13543
		private bool waiting;

		// Token: 0x020006C7 RID: 1735
		public enum LoopType
		{
			// Token: 0x040034E9 RID: 13545
			none,
			// Token: 0x040034EA RID: 13546
			loop,
			// Token: 0x040034EB RID: 13547
			pingPong,
			// Token: 0x040034EC RID: 13548
			random
		}
	}
}
