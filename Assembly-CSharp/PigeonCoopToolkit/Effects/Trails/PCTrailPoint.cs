using System;
using UnityEngine;

namespace PigeonCoopToolkit.Effects.Trails
{
	// Token: 0x020005B6 RID: 1462
	public class PCTrailPoint
	{
		// Token: 0x0600247E RID: 9342 RVA: 0x000183EA File Offset: 0x000165EA
		public virtual void Update(float deltaTime)
		{
			this._timeActive += deltaTime;
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000183FA File Offset: 0x000165FA
		public float TimeActive()
		{
			return this._timeActive;
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x00018402 File Offset: 0x00016602
		public void SetDistanceFromStart(float distance)
		{
			this._distance = distance;
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x0001840B File Offset: 0x0001660B
		public float GetDistanceFromStart()
		{
			return this._distance;
		}

		// Token: 0x04002C47 RID: 11335
		public Vector3 Forward;

		// Token: 0x04002C48 RID: 11336
		public Vector3 Position;

		// Token: 0x04002C49 RID: 11337
		public int PointNumber;

		// Token: 0x04002C4A RID: 11338
		private float _timeActive;

		// Token: 0x04002C4B RID: 11339
		private float _distance;
	}
}
