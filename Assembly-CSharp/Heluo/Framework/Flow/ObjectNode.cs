using System;
using Newtonsoft.Json;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200017A RID: 378
	[GenericTypeConstraint(TypeConstraint.UnityObject, false, new Type[]
	{

	})]
	[Description("Unity 物件輸出")]
	public class ObjectNode<T> : OutputNode<T> where T : Object
	{
		// Token: 0x0600077A RID: 1914 RVA: 0x000067D5 File Offset: 0x000049D5
		public override T GetValue()
		{
			return this.value;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00048DBC File Offset: 0x00046FBC
		internal override void OnAfterDeserialize(NodeGraph cond)
		{
			base.OnAfterDeserialize(cond);
			if (this.guid == Guid.Empty)
			{
				return;
			}
			if (cond.objectReference.ContainsKey(this.guid.ToString()))
			{
				this.value = (cond.objectReference[this.guid.ToString()] as T);
			}
			else
			{
				Debug.LogError("Lost object reference " + this.guid);
				this.guid = Guid.Empty;
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00048E54 File Offset: 0x00047054
		public override string ToString()
		{
			if (this.value)
			{
				return this.value.name + " = " + this.guid;
			}
			return "NULL !!";
		}

		// Token: 0x040007C8 RID: 1992
		[JsonIgnore]
		private T value;

		// Token: 0x040007C9 RID: 1993
		public Guid guid = Guid.Empty;
	}
}
