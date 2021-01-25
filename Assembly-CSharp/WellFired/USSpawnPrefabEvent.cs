using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200087A RID: 2170
	[USequencerEvent("Spawn/Spawn Prefab")]
	[USequencerFriendlyName("Spawn Prefab")]
	public class USSpawnPrefabEvent : USEventBase
	{
		// Token: 0x06003452 RID: 13394 RVA: 0x0018FEF4 File Offset: 0x0018E0F4
		public override void FireEvent()
		{
			if (!this.spawnPrefab)
			{
				Debug.Log("Attempting to spawn a prefab, but you haven't given a prefab to the event from USSpawnPrefabEvent::FireEvent");
				return;
			}
			if (this.spawnTransform)
			{
				Object.Instantiate(this.spawnPrefab, this.spawnTransform.position, this.spawnTransform.rotation);
			}
			else
			{
				Object.Instantiate(this.spawnPrefab, Vector3.zero, Quaternion.identity);
			}
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x04004036 RID: 16438
		public GameObject spawnPrefab;

		// Token: 0x04004037 RID: 16439
		public Transform spawnTransform;
	}
}
