using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200087C RID: 2172
	[USequencerFriendlyName("Look At Object")]
	[USequencerEvent("Transform/Look At Object")]
	public class USLookAtObjectEvent : USEventBase
	{
		// Token: 0x0600345B RID: 13403 RVA: 0x00190118 File Offset: 0x0018E318
		public override void FireEvent()
		{
			if (!this.objectToLookAt)
			{
				Debug.LogWarning("The USLookAtObject event does not provice a object to look at", this);
				return;
			}
			this.previousRotation = base.AffectedObject.transform.rotation;
			this.sourceOrientation = base.AffectedObject.transform.rotation;
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x00190170 File Offset: 0x0018E370
		public override void ProcessEvent(float deltaTime)
		{
			if (!this.objectToLookAt)
			{
				Debug.LogWarning("The USLookAtObject event does not provice a object to look at", this);
				return;
			}
			float time = this.inCurve[this.inCurve.length - 1].time;
			float num = this.lookAtTime + time;
			float num2 = 1f;
			if (deltaTime <= time)
			{
				num2 = Mathf.Clamp(this.inCurve.Evaluate(deltaTime), 0f, 1f);
			}
			else if (deltaTime >= num)
			{
				num2 = Mathf.Clamp(this.outCurve.Evaluate(deltaTime - num), 0f, 1f);
			}
			Vector3 position = base.AffectedObject.transform.position;
			Vector3 position2 = this.objectToLookAt.transform.position;
			Vector3 vector = position2 - position;
			Quaternion quaternion = Quaternion.LookRotation(vector);
			base.AffectedObject.transform.rotation = Quaternion.Slerp(this.sourceOrientation, quaternion, num2);
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x00020D52 File Offset: 0x0001EF52
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x00020D5A File Offset: 0x0001EF5A
		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			base.AffectedObject.transform.rotation = this.previousRotation;
		}

		// Token: 0x0400403B RID: 16443
		public GameObject objectToLookAt;

		// Token: 0x0400403C RID: 16444
		public AnimationCurve inCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x0400403D RID: 16445
		public AnimationCurve outCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400403E RID: 16446
		public float lookAtTime = 2f;

		// Token: 0x0400403F RID: 16447
		private Quaternion sourceOrientation = Quaternion.identity;

		// Token: 0x04004040 RID: 16448
		private Quaternion previousRotation = Quaternion.identity;
	}
}
