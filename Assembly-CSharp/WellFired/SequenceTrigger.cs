using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000851 RID: 2129
	[RequireComponent(typeof(Collider))]
	public class SequenceTrigger : MonoBehaviour
	{
		// Token: 0x06003392 RID: 13202 RVA: 0x0018DB64 File Offset: 0x0018BD64
		private void OnTriggerEnter(Collider other)
		{
			if (!this.sequenceToPlay)
			{
				Debug.LogWarning("You have triggered a sequence in your scene, however, you didn't assign it a Sequence To Play", base.gameObject);
				return;
			}
			if (this.sequenceToPlay.IsPlaying)
			{
				return;
			}
			if (other.CompareTag("MainCamera") && this.isMainCameraTrigger)
			{
				this.sequenceToPlay.Play();
				return;
			}
			if (other.gameObject == this.triggerObject)
			{
				this.sequenceToPlay.Play();
				return;
			}
		}

		// Token: 0x04003FC4 RID: 16324
		public bool isPlayerTrigger;

		// Token: 0x04003FC5 RID: 16325
		public bool isMainCameraTrigger;

		// Token: 0x04003FC6 RID: 16326
		public int m_iEventID;

		// Token: 0x04003FC7 RID: 16327
		public GameObject triggerObject;

		// Token: 0x04003FC8 RID: 16328
		public USSequencer sequenceToPlay;
	}
}
