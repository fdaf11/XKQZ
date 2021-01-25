using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000028 RID: 40
	public abstract class BaseState : SavableComponent
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x000235DC File Offset: 0x000217DC
		public BaseState()
		{
			this.objectFinder = this.CreateObjectFinder();
		}

		// Token: 0x060000A4 RID: 164
		protected abstract void Init(AIBehaviors fsm);

		// Token: 0x060000A5 RID: 165
		protected abstract bool Reason(AIBehaviors fsm);

		// Token: 0x060000A6 RID: 166
		protected abstract void Action(AIBehaviors fsm);

		// Token: 0x060000A7 RID: 167
		protected abstract void StateEnded(AIBehaviors fsm);

		// Token: 0x060000A8 RID: 168 RVA: 0x00002868 File Offset: 0x00000A68
		protected virtual TaggedObjectFinder CreateObjectFinder()
		{
			return new TaggedObjectFinder();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00002B5C File Offset: 0x00000D5C
		protected virtual void Awake()
		{
			this.objectFinder.CacheTransforms(CachePoint.Awake);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00002B6A File Offset: 0x00000D6A
		public virtual void InitState(AIBehaviors fsm)
		{
			this.lastActionTime = Time.time;
			this.deltaTime = 0f;
			this.InitObjectFinder(fsm);
			this.InitTriggers();
			this.Init(fsm);
			this.PlayRandomAnimation(fsm);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00002B9D File Offset: 0x00000D9D
		private void InitObjectFinder(AIBehaviors fsm)
		{
			if (!this.objectFinder.useCustomTags)
			{
				this.objectFinder = fsm.objectFinder;
			}
			else
			{
				this.ownsObjectFinder = true;
				this.objectFinder.CacheTransforms(CachePoint.StateChanged);
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00002BD3 File Offset: 0x00000DD3
		public void EndState(AIBehaviors fsm)
		{
			this.StateEnded(fsm);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00023654 File Offset: 0x00021854
		private void InitTriggers()
		{
			foreach (BaseTrigger baseTrigger in this.triggers)
			{
				baseTrigger.HandleInit(this.objectFinder);
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00002BDC File Offset: 0x00000DDC
		public bool HandleReason(AIBehaviors fsm)
		{
			this.objectFinder.CacheTransforms(CachePoint.EveryFrame);
			return !this.CheckTriggers(fsm) && this.Reason(fsm);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0002368C File Offset: 0x0002188C
		protected bool CheckTriggers(AIBehaviors fsm)
		{
			foreach (BaseTrigger baseTrigger in this.triggers)
			{
				if (baseTrigger.HandleEvaluate(fsm))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00002BFF File Offset: 0x00000DFF
		public virtual void HandleAction(AIBehaviors fsm)
		{
			this.CalculateDeltaTime();
			this.Action(fsm);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00002C0E File Offset: 0x00000E0E
		private void CalculateDeltaTime()
		{
			this.deltaTime = Time.time - this.lastActionTime;
			this.lastActionTime = Time.time;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000236C8 File Offset: 0x000218C8
		public void PlayRandomAnimation(AIBehaviors fsm)
		{
			if (this.animationStates.Length > 0)
			{
				int num = (int)(Random.value * (float)this.animationStates.Length);
				fsm.PlayAnimation(this.animationStates[num]);
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00002C2D File Offset: 0x00000E2D
		public virtual bool RotatesTowardTarget()
		{
			return false;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00002C30 File Offset: 0x00000E30
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x0400008A RID: 138
		public bool isEnabled = true;

		// Token: 0x0400008B RID: 139
		public string name = string.Empty;

		// Token: 0x0400008C RID: 140
		public BaseTrigger[] triggers = new BaseTrigger[0];

		// Token: 0x0400008D RID: 141
		public bool ownsObjectFinder;

		// Token: 0x0400008E RID: 142
		public TaggedObjectFinder objectFinder;

		// Token: 0x0400008F RID: 143
		public float movementSpeed = 1f;

		// Token: 0x04000090 RID: 144
		public float rotationSpeed = 90f;

		// Token: 0x04000091 RID: 145
		private float lastActionTime;

		// Token: 0x04000092 RID: 146
		protected float deltaTime;

		// Token: 0x04000093 RID: 147
		public AIAnimationStates animationStatesComponent;

		// Token: 0x04000094 RID: 148
		public AIAnimationState[] animationStates = new AIAnimationState[1];

		// Token: 0x04000095 RID: 149
		public AudioClip audioClip;

		// Token: 0x04000096 RID: 150
		public float audioVolume = 1f;

		// Token: 0x04000097 RID: 151
		public float audioPitch = 1f;

		// Token: 0x04000098 RID: 152
		public float audioPitchRandomness;

		// Token: 0x04000099 RID: 153
		public bool loopAudio;
	}
}
