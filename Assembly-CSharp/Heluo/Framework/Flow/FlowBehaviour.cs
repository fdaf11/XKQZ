using System;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000187 RID: 391
	public class FlowBehaviour : MonoBehaviour
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060007AA RID: 1962 RVA: 0x00006914 File Offset: 0x00004B14
		// (remove) Token: 0x060007AB RID: 1963 RVA: 0x0000692D File Offset: 0x00004B2D
		public event Action AwakeEvent;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060007AC RID: 1964 RVA: 0x00006946 File Offset: 0x00004B46
		// (remove) Token: 0x060007AD RID: 1965 RVA: 0x0000695F File Offset: 0x00004B5F
		public event Action EnableEvent;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060007AE RID: 1966 RVA: 0x00006978 File Offset: 0x00004B78
		// (remove) Token: 0x060007AF RID: 1967 RVA: 0x00006991 File Offset: 0x00004B91
		public event Action StartEvent;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060007B0 RID: 1968 RVA: 0x000069AA File Offset: 0x00004BAA
		// (remove) Token: 0x060007B1 RID: 1969 RVA: 0x000069C3 File Offset: 0x00004BC3
		public event Action UpdateEvent;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060007B2 RID: 1970 RVA: 0x000069DC File Offset: 0x00004BDC
		// (remove) Token: 0x060007B3 RID: 1971 RVA: 0x000069F5 File Offset: 0x00004BF5
		public event Action FixedUpdateEvent;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060007B4 RID: 1972 RVA: 0x00006A0E File Offset: 0x00004C0E
		// (remove) Token: 0x060007B5 RID: 1973 RVA: 0x00006A27 File Offset: 0x00004C27
		public event Action<Collider> TriggerEnterEvent;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060007B6 RID: 1974 RVA: 0x00006A40 File Offset: 0x00004C40
		// (remove) Token: 0x060007B7 RID: 1975 RVA: 0x00006A59 File Offset: 0x00004C59
		public event Action<Collider> TriggerStayEvent;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060007B8 RID: 1976 RVA: 0x00006A72 File Offset: 0x00004C72
		// (remove) Token: 0x060007B9 RID: 1977 RVA: 0x00006A8B File Offset: 0x00004C8B
		public event Action<Collider> TriggerExitEvent;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060007BA RID: 1978 RVA: 0x00006AA4 File Offset: 0x00004CA4
		// (remove) Token: 0x060007BB RID: 1979 RVA: 0x00006ABD File Offset: 0x00004CBD
		public event Action<Collision> CollisionEnterEvent;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060007BC RID: 1980 RVA: 0x00006AD6 File Offset: 0x00004CD6
		// (remove) Token: 0x060007BD RID: 1981 RVA: 0x00006AEF File Offset: 0x00004CEF
		public event Action<Collision> CollisionStayEvent;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060007BE RID: 1982 RVA: 0x00006B08 File Offset: 0x00004D08
		// (remove) Token: 0x060007BF RID: 1983 RVA: 0x00006B21 File Offset: 0x00004D21
		public event Action<Collision> CollisionExitEvent;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x060007C0 RID: 1984 RVA: 0x00006B3A File Offset: 0x00004D3A
		// (remove) Token: 0x060007C1 RID: 1985 RVA: 0x00006B53 File Offset: 0x00004D53
		public event Action LateUpdateEvent;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x060007C2 RID: 1986 RVA: 0x00006B6C File Offset: 0x00004D6C
		// (remove) Token: 0x060007C3 RID: 1987 RVA: 0x00006B85 File Offset: 0x00004D85
		public event Action DisableEvent;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x060007C4 RID: 1988 RVA: 0x00006B9E File Offset: 0x00004D9E
		// (remove) Token: 0x060007C5 RID: 1989 RVA: 0x00006BB7 File Offset: 0x00004DB7
		public event Action DestroyEvent;

		// Token: 0x060007C6 RID: 1990 RVA: 0x000494D0 File Offset: 0x000476D0
		protected virtual void Awake()
		{
			if (this.graph == null)
			{
				Object.Destroy(this);
			}
			else
			{
				this.graph = (this.graph.Clone() as FlowGraph);
				this.graph.SetVariable("root", this);
				this.graph.RegisterEvent();
				if (this.AwakeEvent != null)
				{
					this.AwakeEvent.Invoke();
				}
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00006BD0 File Offset: 0x00004DD0
		protected virtual void OnEnable()
		{
			if (this.EnableEvent != null)
			{
				this.EnableEvent.Invoke();
			}
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00006BE8 File Offset: 0x00004DE8
		protected virtual void Start()
		{
			if (this.StartEvent != null)
			{
				this.StartEvent.Invoke();
			}
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00006C00 File Offset: 0x00004E00
		protected virtual void FixedUpdate()
		{
			if (this.FixedUpdateEvent != null)
			{
				this.FixedUpdateEvent.Invoke();
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00006C18 File Offset: 0x00004E18
		protected virtual void OnTriggerEnter(Collider other)
		{
			if (this.TriggerEnterEvent != null)
			{
				this.TriggerEnterEvent.Invoke(other);
			}
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00006C31 File Offset: 0x00004E31
		protected virtual void OnTriggerStay(Collider other)
		{
			if (this.TriggerStayEvent != null)
			{
				this.TriggerStayEvent.Invoke(other);
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00006C4A File Offset: 0x00004E4A
		protected virtual void OnTriggerExit(Collider other)
		{
			if (this.TriggerExitEvent != null)
			{
				this.TriggerExitEvent.Invoke(other);
			}
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00006C63 File Offset: 0x00004E63
		protected virtual void OnCollisionEnter(Collision collision)
		{
			if (this.CollisionEnterEvent != null)
			{
				this.CollisionEnterEvent.Invoke(collision);
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00006C7C File Offset: 0x00004E7C
		protected virtual void OnCollisionStay(Collision collision)
		{
			if (this.CollisionStayEvent != null)
			{
				this.CollisionStayEvent.Invoke(collision);
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00006C95 File Offset: 0x00004E95
		protected virtual void OnCollisionExit(Collision collision)
		{
			if (this.CollisionExitEvent != null)
			{
				this.CollisionExitEvent.Invoke(collision);
			}
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00006CAE File Offset: 0x00004EAE
		protected virtual void Update()
		{
			if (this.UpdateEvent != null)
			{
				this.UpdateEvent.Invoke();
			}
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00006CC6 File Offset: 0x00004EC6
		protected virtual void LateUpdate()
		{
			if (this.LateUpdateEvent != null)
			{
				this.LateUpdateEvent.Invoke();
			}
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00006CDE File Offset: 0x00004EDE
		protected virtual void OnDisable()
		{
			if (this.DisableEvent != null)
			{
				this.DisableEvent.Invoke();
			}
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00006CF6 File Offset: 0x00004EF6
		protected virtual void OnDestroy()
		{
			if (this.DestroyEvent != null)
			{
				this.DestroyEvent.Invoke();
			}
			Object.Destroy(this.graph);
		}

		// Token: 0x040007DB RID: 2011
		[SerializeField]
		private FlowGraph graph;
	}
}
