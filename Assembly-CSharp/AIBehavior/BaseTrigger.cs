using System;

namespace AIBehavior
{
	// Token: 0x0200003C RID: 60
	public abstract class BaseTrigger : SavableComponent
	{
		// Token: 0x0600010D RID: 269 RVA: 0x00002FE7 File Offset: 0x000011E7
		public BaseTrigger()
		{
			this.objectFinder = this.CreateObjectFinder();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00002868 File Offset: 0x00000A68
		protected virtual TaggedObjectFinder CreateObjectFinder()
		{
			return new TaggedObjectFinder();
		}

		// Token: 0x0600010F RID: 271
		protected abstract void Init();

		// Token: 0x06000110 RID: 272
		protected abstract bool Evaluate(AIBehaviors fsm);

		// Token: 0x06000111 RID: 273 RVA: 0x00024474 File Offset: 0x00022674
		protected virtual void Awake()
		{
			this.objectFinder.CacheTransforms(CachePoint.Awake);
			for (int i = 0; i < this.subTriggers.Length; i++)
			{
				this.subTriggers[i].parentTrigger = this;
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000244B4 File Offset: 0x000226B4
		public void HandleInit(TaggedObjectFinder parentObjectFinder)
		{
			if (!this.objectFinder.useCustomTags)
			{
				this.objectFinder = parentObjectFinder;
			}
			else
			{
				this.ownsObjectFinder = true;
			}
			this.objectFinder.CacheTransforms(CachePoint.StateChanged);
			this.Init();
			foreach (BaseTrigger baseTrigger in this.subTriggers)
			{
				baseTrigger.HandleInit(this.objectFinder);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00024524 File Offset: 0x00022724
		public bool HandleEvaluate(AIBehaviors fsm)
		{
			bool flag = base.enabled && this.Evaluate(fsm) && this.CheckSubTriggers(fsm);
			this.objectFinder.CacheTransforms(CachePoint.EveryFrame);
			if (flag)
			{
				this.ChangeToTransitionState(fsm);
			}
			return flag;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00003007 File Offset: 0x00001207
		protected virtual void ChangeToTransitionState(AIBehaviors fsm)
		{
			fsm.ChangeActiveState(this.transitionState);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00024570 File Offset: 0x00022770
		private bool CheckSubTriggers(AIBehaviors fsm)
		{
			if (this.subTriggers.Length == 0)
			{
				return true;
			}
			foreach (BaseTrigger baseTrigger in this.subTriggers)
			{
				if (baseTrigger.HandleEvaluate(fsm))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000245BC File Offset: 0x000227BC
		public void AddSubTrigger(BaseTrigger subTrigger)
		{
			BaseTrigger[] array = new BaseTrigger[this.subTriggers.Length + 1];
			for (int i = 0; i < this.subTriggers.Length; i++)
			{
				array[i] = this.subTriggers[i];
			}
			subTrigger.parentTrigger = this;
			array[this.subTriggers.Length] = subTrigger;
			this.subTriggers = array;
		}

		// Token: 0x040000EE RID: 238
		public BaseState transitionState;

		// Token: 0x040000EF RID: 239
		public BaseTrigger parentTrigger;

		// Token: 0x040000F0 RID: 240
		public BaseTrigger[] subTriggers = new BaseTrigger[0];

		// Token: 0x040000F1 RID: 241
		public bool ownsObjectFinder;

		// Token: 0x040000F2 RID: 242
		public TaggedObjectFinder objectFinder;
	}
}
