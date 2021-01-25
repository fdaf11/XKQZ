using System;
using Newtonsoft.Json;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000185 RID: 389
	public abstract class OutputNode : ICloneable
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x000068E5 File Offset: 0x00004AE5
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x000068ED File Offset: 0x00004AED
		[JsonIgnore]
		private protected NodeGraph Graph { protected get; private set; }

		// Token: 0x060007A0 RID: 1952 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void Update()
		{
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0000264F File Offset: 0x0000084F
		internal virtual void OnArgumentChange()
		{
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x000068F6 File Offset: 0x00004AF6
		internal virtual void OnAfterDeserialize(NodeGraph graph)
		{
			this.Graph = graph;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0000264F File Offset: 0x0000084F
		internal virtual void OnBeforeSerialize(NodeGraph graph)
		{
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00049480 File Offset: 0x00047680
		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x040007D7 RID: 2007
		[JsonIgnore]
		public Rect rect;

		// Token: 0x040007D8 RID: 2008
		public float x;

		// Token: 0x040007D9 RID: 2009
		public float y;
	}
}
