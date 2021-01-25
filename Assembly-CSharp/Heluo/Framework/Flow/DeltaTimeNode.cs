using System;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000178 RID: 376
	[Description("時間/Time.deltaTime")]
	public class DeltaTimeNode : OutputNode<float>
	{
		// Token: 0x06000774 RID: 1908 RVA: 0x0000679C File Offset: 0x0000499C
		public override float GetValue()
		{
			return Time.deltaTime;
		}
	}
}
