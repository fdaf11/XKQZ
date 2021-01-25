using System;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000162 RID: 354
	[Description("變數設定")]
	public class ValueSetterAction : ActionNode
	{
		// Token: 0x06000746 RID: 1862 RVA: 0x00006589 File Offset: 0x00004789
		public ValueSetterAction()
		{
			this.getMethod = typeof(IOutput).GetMethod("GetValue");
			this.setMethod = typeof(IInput).GetMethod("SetValue");
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00048730 File Offset: 0x00046930
		public override bool GetValue()
		{
			if (this.dataNode == null || this.valueNode == null)
			{
				return false;
			}
			if (this.getMethod == null || this.setMethod == null)
			{
				return false;
			}
			bool result;
			try
			{
				object obj = this.getMethod.Invoke(this.dataNode, null);
				this.setMethod.Invoke(this.valueNode, new object[]
				{
					obj
				});
				result = true;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
				result = false;
			}
			return result;
		}

		// Token: 0x04000793 RID: 1939
		[Argument("資料", "要設定的資料")]
		public OutputNode dataNode;

		// Token: 0x04000794 RID: 1940
		[Argument("<變數")]
		public OutputNode valueNode;

		// Token: 0x04000795 RID: 1941
		[JsonIgnore]
		private MethodInfo getMethod;

		// Token: 0x04000796 RID: 1942
		[JsonIgnore]
		private MethodInfo setMethod;
	}
}
