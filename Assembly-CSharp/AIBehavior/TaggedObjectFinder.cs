using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000026 RID: 38
	[Serializable]
	public class TaggedObjectFinder
	{
		// Token: 0x06000096 RID: 150 RVA: 0x00023234 File Offset: 0x00021434
		public Transform[] GetTransforms()
		{
			for (int i = 0; i < this.taggedTransforms.Length; i++)
			{
				if (this.taggedTransforms[i] == null)
				{
					this.CleanupNullTransforms();
					break;
				}
			}
			return this.taggedTransforms;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002AE9 File Offset: 0x00000CE9
		public void CacheTransforms()
		{
			this.CacheTransforms(this.cachePoint);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00023280 File Offset: 0x00021480
		public virtual void CacheTransforms(CachePoint potentialCachePoint)
		{
			if (potentialCachePoint == this.cachePoint)
			{
				List<Transform> list = new List<Transform>();
				for (int i = 0; i < this.tags.Length; i++)
				{
					GameObject[] gameObjectsWithTag = this.GetGameObjectsWithTag(this.tags[i]);
					for (int j = 0; j < gameObjectsWithTag.Length; j++)
					{
						list.Add(gameObjectsWithTag[j].transform);
					}
				}
				this.taggedTransforms = list.ToArray();
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002AF7 File Offset: 0x00000CF7
		protected virtual GameObject[] GetGameObjectsWithTag(string tag)
		{
			return GameObject.FindGameObjectsWithTag(tag);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000232F8 File Offset: 0x000214F8
		private void CleanupNullTransforms()
		{
			List<Transform> list = new List<Transform>(this.taggedTransforms);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == null)
				{
					list.RemoveAt(i);
					i--;
				}
			}
			this.taggedTransforms = list.ToArray();
		}

		// Token: 0x04000078 RID: 120
		public bool useCustomTags;

		// Token: 0x04000079 RID: 121
		public string[] tags = new string[]
		{
			"Player"
		};

		// Token: 0x0400007A RID: 122
		private Transform[] taggedTransforms = new Transform[0];

		// Token: 0x0400007B RID: 123
		public CachePoint cachePoint;
	}
}
