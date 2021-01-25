using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200087D RID: 2173
	[USequencerFriendlyName("Match Objects Orientation")]
	[USequencerEvent("Transform/Match Objects Orientation")]
	public class USMatchObjectEvent : USEventBase
	{
		// Token: 0x06003460 RID: 13408 RVA: 0x001902DC File Offset: 0x0018E4DC
		public override void FireEvent()
		{
			if (!this.objectToMatch)
			{
				Debug.LogWarning("The USMatchObjectEvent event does not provice a object to match", this);
				return;
			}
			this.sourceRotation = base.AffectedObject.transform.rotation;
			this.sourcePosition = base.AffectedObject.transform.position;
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x00190334 File Offset: 0x0018E534
		public override void ProcessEvent(float deltaTime)
		{
			if (!this.objectToMatch)
			{
				Debug.LogWarning("The USMatchObjectEvent event does not provice a object to look at", this);
				return;
			}
			float num = Mathf.Clamp(this.inCurve.Evaluate(deltaTime), 0f, 1f);
			Vector3 position = this.objectToMatch.transform.position;
			Quaternion rotation = this.objectToMatch.transform.rotation;
			base.AffectedObject.transform.rotation = Quaternion.Slerp(this.sourceRotation, rotation, num);
			base.AffectedObject.transform.position = Vector3.Slerp(this.sourcePosition, position, num);
		}

		// Token: 0x06003462 RID: 13410 RVA: 0x00020D83 File Offset: 0x0001EF83
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x06003463 RID: 13411 RVA: 0x00020D8B File Offset: 0x0001EF8B
		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			base.AffectedObject.transform.rotation = this.sourceRotation;
			base.AffectedObject.transform.position = this.sourcePosition;
		}

		// Token: 0x04004041 RID: 16449
		public GameObject objectToMatch;

		// Token: 0x04004042 RID: 16450
		public AnimationCurve inCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04004043 RID: 16451
		private Quaternion sourceRotation = Quaternion.identity;

		// Token: 0x04004044 RID: 16452
		private Vector3 sourcePosition = Vector3.zero;
	}
}
