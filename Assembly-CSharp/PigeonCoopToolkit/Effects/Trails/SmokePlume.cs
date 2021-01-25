using System;
using UnityEngine;

namespace PigeonCoopToolkit.Effects.Trails
{
	// Token: 0x020005B0 RID: 1456
	[AddComponentMenu("Pigeon Coop Toolkit/Effects/Smoke Plume")]
	public class SmokePlume : TrailRenderer_Base
	{
		// Token: 0x0600244E RID: 9294 RVA: 0x000181E2 File Offset: 0x000163E2
		protected override void Start()
		{
			base.Start();
			this._timeSincePoint = 0f;
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x000181F5 File Offset: 0x000163F5
		protected override void OnStartEmit()
		{
			this._timeSincePoint = 0f;
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x0011C8EC File Offset: 0x0011AAEC
		protected override void Reset()
		{
			base.Reset();
			this.TrailData.SizeOverLife = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0f),
				new Keyframe(0.5f, 0.2f),
				new Keyframe(1f, 0.2f)
			});
			this.TrailData.Lifetime = 6f;
			this.ConstantForce = Vector3.up * 0.5f;
			this.TimeBetweenPoints = 0.1f;
			this.RandomForceScale = 0.05f;
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x0011C9A4 File Offset: 0x0011ABA4
		protected override void Update()
		{
			if (this._emit)
			{
				this._timeSincePoint += Time.deltaTime;
				if (this._timeSincePoint >= this.TimeBetweenPoints)
				{
					base.AddPoint(new SmokeTrailPoint(), this._t.position);
					this._timeSincePoint = 0f;
				}
			}
			base.Update();
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x00018202 File Offset: 0x00016402
		protected override void InitialiseNewPoint(PCTrailPoint newPoint)
		{
			((SmokeTrailPoint)newPoint).RandomVec = Random.onUnitSphere * this.RandomForceScale;
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x0001821F File Offset: 0x0001641F
		protected override void UpdatePoint(PCTrailPoint point, float deltaTime)
		{
			point.Position += this.ConstantForce * deltaTime;
		}

		// Token: 0x04002C27 RID: 11303
		public float TimeBetweenPoints = 0.1f;

		// Token: 0x04002C28 RID: 11304
		public Vector3 ConstantForce = Vector3.up * 0.5f;

		// Token: 0x04002C29 RID: 11305
		public float RandomForceScale = 0.05f;

		// Token: 0x04002C2A RID: 11306
		private float _timeSincePoint;
	}
}
