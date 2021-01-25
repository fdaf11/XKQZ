using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200087B RID: 2171
	[USequencerFriendlyName("Time Scale")]
	[USequencerEvent("Time/Time Scale")]
	public class USTimeScaleEvent : USEventBase
	{
		// Token: 0x06003455 RID: 13397 RVA: 0x00020CFC File Offset: 0x0001EEFC
		public override void FireEvent()
		{
			this.prevTimeScale = Time.timeScale;
		}

		// Token: 0x06003456 RID: 13398 RVA: 0x00020D09 File Offset: 0x0001EF09
		public override void ProcessEvent(float deltaTime)
		{
			this.currentCurveSampleTime = deltaTime;
			Time.timeScale = Mathf.Max(0f, this.scaleCurve.Evaluate(this.currentCurveSampleTime));
		}

		// Token: 0x06003457 RID: 13399 RVA: 0x00190008 File Offset: 0x0018E208
		public override void EndEvent()
		{
			float time = this.scaleCurve.keys[this.scaleCurve.length - 1].time;
			Time.timeScale = Mathf.Max(0f, this.scaleCurve.Evaluate(time));
		}

		// Token: 0x06003458 RID: 13400 RVA: 0x00020D32 File Offset: 0x0001EF32
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x06003459 RID: 13401 RVA: 0x00020D3A File Offset: 0x0001EF3A
		public override void UndoEvent()
		{
			this.currentCurveSampleTime = 0f;
			Time.timeScale = this.prevTimeScale;
		}

		// Token: 0x04004038 RID: 16440
		public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f),
			new Keyframe(3f, 1f),
			new Keyframe(4f, 0f)
		});

		// Token: 0x04004039 RID: 16441
		private float currentCurveSampleTime;

		// Token: 0x0400403A RID: 16442
		private float prevTimeScale = 1f;
	}
}
