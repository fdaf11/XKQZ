using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200018E RID: 398
	internal class FieldResolver : Resolver
	{
		// Token: 0x060007F1 RID: 2033 RVA: 0x00006DDF File Offset: 0x00004FDF
		public FieldResolver(FieldInfo field, object parent, int reference) : base(reference)
		{
			this.parent = parent;
			this.field = field;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00049F00 File Offset: 0x00048100
		public override void Resolve(Dictionary<int, OutputNode> map)
		{
			if (!map.ContainsKey(this.reference))
			{
				Debug.LogError(this.reference.ToString("X2") + " : " + this.parent.GetType().Name);
			}
			this.field.SetValue(this.parent, map[this.reference]);
		}

		// Token: 0x040007F6 RID: 2038
		private object parent;

		// Token: 0x040007F7 RID: 2039
		private FieldInfo field;
	}
}
