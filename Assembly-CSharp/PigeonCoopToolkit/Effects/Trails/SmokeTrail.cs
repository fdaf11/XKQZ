using System;
using UnityEngine;

namespace PigeonCoopToolkit.Effects.Trails
{
	// Token: 0x020005B1 RID: 1457
	[AddComponentMenu("Pigeon Coop Toolkit/Effects/Smoke Trail")]
	public class SmokeTrail : TrailRenderer_Base
	{
		// Token: 0x06002455 RID: 9301 RVA: 0x0001825C File Offset: 0x0001645C
		public void ClampFirstPointTo(Transform clampTo)
		{
			this._clampPosition = clampTo;
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x00018265 File Offset: 0x00016465
		public void ClearClamp()
		{
			this._clampPosition = null;
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x0001826E File Offset: 0x0001646E
		protected override void Start()
		{
			base.Start();
			this._lastPosition = this._t.position;
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x0011CA08 File Offset: 0x0011AC08
		protected override void Update()
		{
			if (this._emit)
			{
				this._distanceMoved += Vector3.Distance(this._t.position, this._lastPosition);
				if (this._distanceMoved != 0f && this._distanceMoved >= this.MinVertexDistance)
				{
					base.AddPoint(new SmokeTrailPoint(), this._lastPosition);
					this._distanceMoved = 0f;
				}
				this._lastPosition = this._t.position;
			}
			base.Update();
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x00018287 File Offset: 0x00016487
		protected override void OnStartEmit()
		{
			this._lastPosition = this._t.position;
			this._distanceMoved = 0f;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000182A5 File Offset: 0x000164A5
		protected override void UpdatePoint(PCTrailPoint point, float deltaTime)
		{
			if (this._clampPosition && point.PointNumber == 0)
			{
				point.Position = this._clampPosition.position;
			}
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000182D3 File Offset: 0x000164D3
		protected override void Reset()
		{
			base.Reset();
			this.MinVertexDistance = 0.1f;
			this.RandomForceScale = 1f;
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x000182F1 File Offset: 0x000164F1
		protected override void InitialiseNewPoint(PCTrailPoint newPoint)
		{
			((SmokeTrailPoint)newPoint).RandomVec = Random.onUnitSphere * this.RandomForceScale;
		}

		// Token: 0x04002C2B RID: 11307
		public float MinVertexDistance = 0.1f;

		// Token: 0x04002C2C RID: 11308
		private Vector3 _lastPosition;

		// Token: 0x04002C2D RID: 11309
		private float _distanceMoved;

		// Token: 0x04002C2E RID: 11310
		public float RandomForceScale = 1f;

		// Token: 0x04002C2F RID: 11311
		private Transform _clampPosition;
	}
}
