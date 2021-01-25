using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
	// Token: 0x020000CC RID: 204
	public class ExploderQueue
	{
		// Token: 0x0600042D RID: 1069 RVA: 0x00004D55 File Offset: 0x00002F55
		public ExploderQueue(ExploderObject exploder)
		{
			this.exploder = exploder;
			this.queue = new Queue<ExploderSettings>();
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x000345F4 File Offset: 0x000327F4
		public void Explode(ExploderObject.OnExplosion callback)
		{
			ExploderSettings exploderSettings = new ExploderSettings
			{
				Position = ExploderUtils.GetCentroid(this.exploder.gameObject),
				DontUseTag = this.exploder.DontUseTag,
				Radius = this.exploder.Radius,
				ForceVector = this.exploder.ForceVector,
				UseForceVector = this.exploder.UseForceVector,
				Force = this.exploder.Force,
				FrameBudget = this.exploder.FrameBudget,
				TargetFragments = this.exploder.TargetFragments,
				DeactivateOptions = this.exploder.DeactivateOptions,
				DeactivateTimeout = this.exploder.DeactivateTimeout,
				MeshColliders = this.exploder.MeshColliders,
				ExplodeSelf = this.exploder.ExplodeSelf,
				HideSelf = this.exploder.HideSelf,
				DestroyOriginalObject = this.exploder.DestroyOriginalObject,
				ExplodeFragments = this.exploder.ExplodeFragments,
				SplitMeshIslands = this.exploder.SplitMeshIslands,
				FragmentOptions = this.exploder.FragmentOptions.Clone(),
				SfxOptions = this.exploder.SFXOptions.Clone(),
				Callback = callback
			};
			this.queue.Enqueue(exploderSettings);
			this.ProcessQueue();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00034764 File Offset: 0x00032964
		private void ProcessQueue()
		{
			if (this.queue.Count > 0)
			{
				ExploderSettings exploderSettings = this.queue.Peek();
				if (!exploderSettings.processing)
				{
					this.exploder.DontUseTag = exploderSettings.DontUseTag;
					this.exploder.Radius = exploderSettings.Radius;
					this.exploder.ForceVector = exploderSettings.ForceVector;
					this.exploder.UseForceVector = exploderSettings.UseForceVector;
					this.exploder.Force = exploderSettings.Force;
					this.exploder.FrameBudget = exploderSettings.FrameBudget;
					this.exploder.TargetFragments = exploderSettings.TargetFragments;
					this.exploder.DeactivateOptions = exploderSettings.DeactivateOptions;
					this.exploder.DeactivateTimeout = exploderSettings.DeactivateTimeout;
					this.exploder.MeshColliders = exploderSettings.MeshColliders;
					this.exploder.ExplodeSelf = exploderSettings.ExplodeSelf;
					this.exploder.HideSelf = exploderSettings.HideSelf;
					this.exploder.DestroyOriginalObject = exploderSettings.DestroyOriginalObject;
					this.exploder.ExplodeFragments = exploderSettings.ExplodeFragments;
					this.exploder.SplitMeshIslands = exploderSettings.SplitMeshIslands;
					this.exploder.FragmentOptions = exploderSettings.FragmentOptions;
					this.exploder.SFXOptions = exploderSettings.SfxOptions;
					exploderSettings.id = Random.Range(int.MinValue, int.MaxValue);
					exploderSettings.processing = true;
					this.exploder.StartExplosionFromQueue(exploderSettings.Position, exploderSettings.id, exploderSettings.Callback);
				}
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000348F4 File Offset: 0x00032AF4
		public void OnExplosionFinished(int id)
		{
			ExploderSettings exploderSettings = this.queue.Dequeue();
			this.ProcessQueue();
		}

		// Token: 0x0400039D RID: 925
		private readonly Queue<ExploderSettings> queue;

		// Token: 0x0400039E RID: 926
		private readonly ExploderObject exploder;
	}
}
