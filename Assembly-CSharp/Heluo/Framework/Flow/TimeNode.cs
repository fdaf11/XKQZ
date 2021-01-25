using System;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000177 RID: 375
	[Description("時間/Time.time")]
	public class TimeNode : OutputNode<float>
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x00006795 File Offset: 0x00004995
		public override float GetValue()
		{
			return Time.time;
		}
	}
}
