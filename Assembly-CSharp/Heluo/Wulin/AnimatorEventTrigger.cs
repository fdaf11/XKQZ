using System;
using System.Collections.Generic;
using JsonFx.Json;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020000EA RID: 234
	[RequireComponent(typeof(Animator))]
	public class AnimatorEventTrigger : MonoBehaviour
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x0003A7A0 File Offset: 0x000389A0
		private void Start()
		{
			this.animator = base.GetComponent<Animator>();
			this.isTrigged = new bool[this.animator.layerCount];
			this.index = new Dictionary<int, int>[this.animator.layerCount];
			this.sortedEvent = new Dictionary<int, List<AnimatorEventData>>[this.animator.layerCount];
			for (int i = 0; i < this.sortedEvent.Length; i++)
			{
				this.index[i] = new Dictionary<int, int>();
				this.sortedEvent[i] = new Dictionary<int, List<AnimatorEventData>>();
			}
			foreach (AnimatorEventData animatorEventData in this.animatorEvent)
			{
				if (animatorEventData.Layer < this.sortedEvent.Length)
				{
					int num = Animator.StringToHash(animatorEventData.Animation.name);
					if (!this.index[animatorEventData.Layer].ContainsKey(num))
					{
						this.index[animatorEventData.Layer].Add(num, 0);
						this.sortedEvent[animatorEventData.Layer].Add(num, new List<AnimatorEventData>());
					}
					animatorEventData.Reset();
					this.sortedEvent[animatorEventData.Layer][num].Add(animatorEventData);
				}
			}
			for (int k = 0; k < this.sortedEvent.Length; k++)
			{
				foreach (KeyValuePair<int, List<AnimatorEventData>> keyValuePair in this.sortedEvent[k])
				{
					keyValuePair.Value.Sort((AnimatorEventData x, AnimatorEventData y) => (x.Time != y.Time) ? ((x.Time <= y.Time) ? -1 : 1) : 0);
				}
			}
			this.eventGroup = new Dictionary<int, AnimatorEventGroup>();
			this.LoadEffect();
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0003A984 File Offset: 0x00038B84
		private void Update()
		{
			for (int i = 0; i < this.animator.layerCount; i++)
			{
				if (this.animator.IsInTransition(i) && !this.isTrigged[i])
				{
					string text = string.Empty;
					string text2 = string.Empty;
					foreach (AnimationInfo animationInfo in this.animator.GetCurrentAnimationClipState(i))
					{
						text = animationInfo.clip.name;
					}
					foreach (AnimationInfo animationInfo2 in this.animator.GetNextAnimationClipState(0))
					{
						text2 = animationInfo2.clip.name;
					}
					AnimatorStateChangeEventArg animatorStateChangeEventArg = new AnimatorStateChangeEventArg
					{
						FromState = text,
						ToState = text2
					};
					base.SendMessage("OnAnimatorStateChange", animatorStateChangeEventArg);
					this.OnTransitionEnter(i, text, text2);
					this.isTrigged[i] = true;
				}
				else if (!this.animator.IsInTransition(i))
				{
					this.isTrigged[i] = false;
				}
				AnimatorStateInfo[] array = new AnimatorStateInfo[]
				{
					this.animator.GetCurrentAnimatorStateInfo(i),
					this.animator.GetNextAnimatorStateInfo(i)
				};
				float num = array[0].normalizedTime % 1f;
				if (this.lastAnimatorTime > num)
				{
					this.startPosition = base.transform.position;
					this.startRotation = base.transform.rotation;
				}
				this.lastAnimatorTime = num;
				for (int l = 0; l < array.Length; l++)
				{
					int tagHash = array[l].tagHash;
					if (this.eventGroup.ContainsKey(tagHash))
					{
						foreach (AnimatorEvent animatorEvent in this.eventGroup[tagHash].events)
						{
							if (array[l].normalizedTime > animatorEvent.fireTime)
							{
								animatorEvent.Play(base.transform, this.startPosition, this.startRotation);
							}
						}
					}
					if (this.sortedEvent[i].ContainsKey(tagHash))
					{
						int n = this.index[i][tagHash];
						while (n < this.sortedEvent[i][tagHash].Count)
						{
							if (array[l].normalizedTime > this.sortedEvent[i][tagHash][n].FireTime)
							{
								this.sortedEvent[i][tagHash][n].OnEvent(base.gameObject);
							}
							n++;
							this.index[i][tagHash] = n;
						}
						if (n >= this.sortedEvent[i][tagHash].Count)
						{
							this.index[i][tagHash] = 0;
						}
					}
				}
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0003ACB8 File Offset: 0x00038EB8
		private void LoadEffect()
		{
			if (this.effect == null || this.effect.Length <= 0)
			{
				return;
			}
			foreach (TextAsset textAsset in this.effect)
			{
				if (!(textAsset == null))
				{
					AnimatorEventGroup animatorEventGroup = JsonReader.Deserialize<AnimatorEventGroup>(textAsset.text);
					int num = Animator.StringToHash(animatorEventGroup.animationName);
					animatorEventGroup.filename = textAsset.name;
					if (animatorEventGroup.animationName.Length > 0)
					{
						if (this.eventGroup.ContainsKey(num))
						{
							this.eventGroup[num] = animatorEventGroup;
						}
						else
						{
							this.eventGroup.Add(num, animatorEventGroup);
						}
						animatorEventGroup.Load(base.transform);
					}
				}
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0003AD84 File Offset: 0x00038F84
		private void OnTransitionEnter(int layer, string start, string end)
		{
			int num = Animator.StringToHash(end);
			if (this.sortedEvent[layer].ContainsKey(num))
			{
				this.index[layer][num] = 0;
				foreach (AnimatorEventData animatorEventData in this.sortedEvent[layer][num])
				{
					animatorEventData.Reset();
				}
			}
			if (this.eventGroup.ContainsKey(num))
			{
				foreach (AnimatorEvent animatorEvent in this.eventGroup[num].events)
				{
					animatorEvent.Reset();
				}
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnTransitionExit(string start, string end)
		{
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnStateEnter(string state)
		{
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnStateExit(string state)
		{
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnState()
		{
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x000050EB File Offset: 0x000032EB
		private void OnDrawGizmos()
		{
			Gizmos.DrawLine(this.startPosition, this.startPosition + this.startRotation * Vector3.forward);
		}

		// Token: 0x040004B5 RID: 1205
		public AnimatorEventTrigger.OnStateChange onStateChange;

		// Token: 0x040004B6 RID: 1206
		public AnimatorEventData[] animatorEvent;

		// Token: 0x040004B7 RID: 1207
		private Dictionary<int, List<AnimatorEventData>>[] sortedEvent;

		// Token: 0x040004B8 RID: 1208
		private Dictionary<int, int>[] index;

		// Token: 0x040004B9 RID: 1209
		private Animator animator;

		// Token: 0x040004BA RID: 1210
		private bool[] isTrigged;

		// Token: 0x040004BB RID: 1211
		private float lastAnimatorTime = 1f;

		// Token: 0x040004BC RID: 1212
		public TextAsset[] effect;

		// Token: 0x040004BD RID: 1213
		private Dictionary<int, AnimatorEventGroup> eventGroup;

		// Token: 0x040004BE RID: 1214
		private Vector3 startPosition;

		// Token: 0x040004BF RID: 1215
		private Quaternion startRotation;

		// Token: 0x040004C0 RID: 1216
		private float lastTime;

		// Token: 0x020000EB RID: 235
		// (Invoke) Token: 0x060004FC RID: 1276
		public delegate void OnStateChange(string start, string end);
	}
}
