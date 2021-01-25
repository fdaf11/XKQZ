using System;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000166 RID: 358
	[GenericTypeConstraint(false, new Type[]
	{
		typeof(GameObject)
	})]
	[Description("計算物件距離")]
	public class GameObjectDistanceNode : OutputNode<float>
	{
		// Token: 0x06000753 RID: 1875 RVA: 0x000488F0 File Offset: 0x00046AF0
		public override float GetValue()
		{
			if (this.gameObjectNode1 == null || this.gameObjectNode2 == null)
			{
				return 0f;
			}
			GameObject value = this.gameObjectNode1.GetValue();
			GameObject value2 = this.gameObjectNode2.GetValue();
			if (value == null || value2 == null)
			{
				return 0f;
			}
			return Vector3.Distance(value.transform.position, value2.transform.position);
		}

		// Token: 0x040007A5 RID: 1957
		[Argument("物件 1")]
		public OutputNode<GameObject> gameObjectNode1;

		// Token: 0x040007A6 RID: 1958
		[Argument("物件 2")]
		public OutputNode<GameObject> gameObjectNode2;
	}
}
