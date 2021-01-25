using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x0200004A RID: 74
	public class SpeedExceededTrigger : BaseTrigger
	{
		// Token: 0x06000147 RID: 327 RVA: 0x00003184 File Offset: 0x00001384
		protected override void Init()
		{
			this.gameObjectPreviousPositions = new Dictionary<GameObject, Vector3>();
			this.previousCheckTime = Time.time;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000248E0 File Offset: 0x00022AE0
		protected override bool Evaluate(AIBehaviors fsm)
		{
			float time = Time.time;
			Transform speedItemTransform;
			if (this.SpeedExceededForTarget(out speedItemTransform, time))
			{
				base.StartCoroutine(this.HandleSeekObject(speedItemTransform));
				return true;
			}
			this.timeTotal += time - this.previousCheckTime;
			this.previousCheckTime = time;
			return false;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00024930 File Offset: 0x00022B30
		public bool SpeedExceededForTarget(out Transform target, float time)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(this.speedObjectsTag);
			float num = this.speedThreshold * this.speedThreshold;
			float num2 = this.maxDistanceFromAI * this.maxDistanceFromAI;
			float num3 = time - this.previousCheckTime;
			float num4 = num3 * num3;
			foreach (GameObject gameObject in array)
			{
				Vector3 position = gameObject.transform.position;
				if (this.gameObjectPreviousPositions.ContainsKey(gameObject) && (position - base.transform.position).sqrMagnitude < num2 && (position - this.gameObjectPreviousPositions[gameObject]).sqrMagnitude / num4 > num)
				{
					target = gameObject.transform;
					return true;
				}
				this.gameObjectPreviousPositions[gameObject] = position;
			}
			target = null;
			return false;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00024A1C File Offset: 0x00022C1C
		private IEnumerator HandleSeekObject(Transform speedItemTransform)
		{
			float destroyTime = Time.time + this.keepSeekObjectAliveDuration;
			GameObject speedSeekObject = new GameObject("Speed Seek Object");
			Transform speedSeekTransform = speedSeekObject.transform;
			speedSeekObject.tag = this.seekObjectTag;
			Object.Destroy(speedSeekObject, this.keepSeekObjectAliveDuration);
			while (destroyTime > Time.time)
			{
				if (speedItemTransform != null)
				{
					speedSeekTransform.position = speedItemTransform.position;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x040000FE RID: 254
		public float speedThreshold = 1f;

		// Token: 0x040000FF RID: 255
		public string speedObjectsTag = "Untagged";

		// Token: 0x04000100 RID: 256
		public string seekObjectTag = "Untagged";

		// Token: 0x04000101 RID: 257
		public float maxDistanceFromAI = 10f;

		// Token: 0x04000102 RID: 258
		public float keepSeekObjectAliveDuration = 10f;

		// Token: 0x04000103 RID: 259
		private Dictionary<GameObject, Vector3> gameObjectPreviousPositions = new Dictionary<GameObject, Vector3>();

		// Token: 0x04000104 RID: 260
		private float previousCheckTime;

		// Token: 0x04000105 RID: 261
		private float timeTotal;
	}
}
