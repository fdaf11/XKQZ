using System;
using UnityEngine;

namespace PigeonCoopToolkit.Effects.Trails
{
	// Token: 0x020005B3 RID: 1459
	[AddComponentMenu("Pigeon Coop Toolkit/Effects/Trail")]
	public class Trail : TrailRenderer_Base
	{
		// Token: 0x06002460 RID: 9312 RVA: 0x0001834F File Offset: 0x0001654F
		protected override void Start()
		{
			base.Start();
			this._lastPosition = this._t.position;
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0011CA98 File Offset: 0x0011AC98
		protected override void Update()
		{
			if (this._emit)
			{
				this._distanceMoved += Vector3.Distance(this._t.position, this._lastPosition);
				if (this._distanceMoved != 0f && this._distanceMoved >= this.MinVertexDistance)
				{
					base.AddPoint(new PCTrailPoint(), this._lastPosition);
					this._distanceMoved = 0f;
				}
				this._lastPosition = this._t.position;
			}
			base.Update();
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x00018368 File Offset: 0x00016568
		protected override void OnStartEmit()
		{
			this._lastPosition = this._t.position;
			this._distanceMoved = 0f;
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x00018386 File Offset: 0x00016586
		protected override void Reset()
		{
			base.Reset();
			this.MinVertexDistance = 0.1f;
		}

		// Token: 0x04002C31 RID: 11313
		public float MinVertexDistance = 0.1f;

		// Token: 0x04002C32 RID: 11314
		private Vector3 _lastPosition;

		// Token: 0x04002C33 RID: 11315
		private float _distanceMoved;
	}
}
