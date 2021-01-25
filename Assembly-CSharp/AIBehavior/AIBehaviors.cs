using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000012 RID: 18
	[RequireComponent(typeof(AIAnimationStates))]
	public class AIBehaviors : SavableComponent
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00022314 File Offset: 0x00020514
		public AIBehaviors()
		{
			this.objectFinder = this.CreateObjectFinder();
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0000281A File Offset: 0x00000A1A
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002822 File Offset: 0x00000A22
		public bool isActive { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600003D RID: 61 RVA: 0x0000282B File Offset: 0x00000A2B
		public int stateCount
		{
			get
			{
				return this.states.Length;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002835 File Offset: 0x00000A35
		// (set) Token: 0x0600003F RID: 63 RVA: 0x0000283D File Offset: 0x00000A3D
		public BaseState currentState { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002846 File Offset: 0x00000A46
		// (set) Token: 0x06000041 RID: 65 RVA: 0x0000284E File Offset: 0x00000A4E
		public BaseState previousState { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002857 File Offset: 0x00000A57
		// (set) Token: 0x06000043 RID: 67 RVA: 0x0000285F File Offset: 0x00000A5F
		public Vector3 currentDestination { get; private set; }

		// Token: 0x06000044 RID: 68 RVA: 0x00002868 File Offset: 0x00000A68
		protected virtual TaggedObjectFinder CreateObjectFinder()
		{
			return new TaggedObjectFinder();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000223BC File Offset: 0x000205BC
		private void Awake()
		{
			this.thisTFM = base.transform;
			this.animationStates = base.GetComponent<AIAnimationStates>();
			this.navMeshAgent = base.GetComponent<NavMeshAgent>();
			this.objectFinder.CacheTransforms(CachePoint.Awake);
			this.InitGlobalTriggers();
			if (!this.useSightTransform || this.sightTransform == null)
			{
				this.useSightTransform = false;
				this.sightTransform = this.thisTFM;
			}
			this.currentDestination = base.transform.position;
			this.SetActive(true);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000286F File Offset: 0x00000A6F
		private void Start()
		{
			this.ChangeActiveState(this.initialState);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00022448 File Offset: 0x00020648
		private void Update()
		{
			if (this.isActive && Time.timeScale > 0f)
			{
				this.objectFinder.CacheTransforms(CachePoint.EveryFrame);
				if (this.currentState.RotatesTowardTarget())
				{
					this.RotateAgent();
				}
				this.thisPos = this.thisTFM.position;
				for (int i = 0; i < this.triggers.Length; i++)
				{
					if (this.triggers[i].HandleEvaluate(this) && this.triggers[i].transitionState != null)
					{
						this.currentState = this.triggers[i].transitionState;
					}
				}
				if (this.currentState.HandleReason(this))
				{
					this.currentState.HandleAction(this);
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00022518 File Offset: 0x00020718
		private void InitGlobalTriggers()
		{
			for (int i = 0; i < this.triggers.Length; i++)
			{
				this.triggers[i].HandleInit(this.objectFinder);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000287D File Offset: 0x00000A7D
		public void SetActive(bool isActive)
		{
			this.isActive = isActive;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00022554 File Offset: 0x00020754
		public void GotHit(float damage)
		{
			GotHitState state = this.GetState<GotHitState>();
			if (state != null && state.CanGetHit(this))
			{
				this.SubtractHealthValue(damage * this.damageMultiplier);
				if (state.CoolDownFinished())
				{
					this.ChangeActiveState(state);
				}
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002886 File Offset: 0x00000A86
		public BaseState[] GetAllStates()
		{
			return this.states;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000288E File Offset: 0x00000A8E
		public BaseState GetStateByIndex(int stateIndex)
		{
			return this.states[stateIndex];
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000225A0 File Offset: 0x000207A0
		public BaseState GetStateByName(string stateName)
		{
			foreach (BaseState baseState in this.states)
			{
				if (baseState.name.Equals(stateName))
				{
					return baseState;
				}
			}
			return null;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000225E0 File Offset: 0x000207E0
		public T GetState<T>() where T : BaseState
		{
			foreach (BaseState baseState in this.states)
			{
				if (baseState is T)
				{
					return baseState as T;
				}
			}
			return (T)((object)null);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0002262C File Offset: 0x0002082C
		public BaseState GetState(Type type)
		{
			foreach (BaseState baseState in this.states)
			{
				if (baseState.GetType() == type)
				{
					return baseState;
				}
			}
			return null;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00022668 File Offset: 0x00020868
		public T[] GetStates<T>() where T : BaseState
		{
			List<T> list = new List<T>();
			foreach (BaseState baseState in this.states)
			{
				if (baseState is T)
				{
					list.Add(baseState as T);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000226BC File Offset: 0x000208BC
		public BaseState[] GetStates(Type type)
		{
			List<BaseState> list = new List<BaseState>();
			foreach (BaseState baseState in this.states)
			{
				if (baseState.GetType() == type)
				{
					list.Add(baseState);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002898 File Offset: 0x00000A98
		public void ReplaceAllStates(BaseState[] newStates)
		{
			this.states = newStates;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000028A1 File Offset: 0x00000AA1
		public void ReplaceStateAtIndex(BaseState newState, int index)
		{
			this.states[index] = newState;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00022708 File Offset: 0x00020908
		public void ChangeActiveStateByName(string stateName)
		{
			foreach (BaseState baseState in this.states)
			{
				if (baseState.name.Equals(stateName))
				{
					this.ChangeActiveState(baseState);
					return;
				}
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000028AC File Offset: 0x00000AAC
		public void ChangeActiveStateByIndex(int index)
		{
			this.ChangeActiveState(this.states[index]);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00022750 File Offset: 0x00020950
		public void ChangeActiveState(BaseState newState)
		{
			this.objectFinder.CacheTransforms(CachePoint.StateChanged);
			this.InitGlobalTriggers();
			this.previousState = this.currentState;
			if (this.previousState != null)
			{
				this.previousState.EndState(this);
			}
			if (newState != null)
			{
				this.currentState = newState;
				newState.InitState(this);
			}
			if (this.onStateChanged != null)
			{
				this.onStateChanged(newState, this.previousState);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000028BC File Offset: 0x00000ABC
		public void MoveAgent(Transform target, float targetSpeed, float rotationSpeed)
		{
			if (target != null)
			{
				this.RotateAgent(target, rotationSpeed);
			}
			this.MoveAgent(target.position, targetSpeed, rotationSpeed);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000227D0 File Offset: 0x000209D0
		public void MoveAgent(Vector3 targetPoint, float targetSpeed, float rotationSpeed)
		{
			bool flag = this.navMeshAgent != null;
			this.currentDestination = targetPoint;
			this.targetRotationPoint = targetPoint;
			this.rotationSpeed = rotationSpeed;
			if (flag && this.navMeshAgent.enabled)
			{
				this.navMeshAgent.speed = targetSpeed;
				this.navMeshAgent.angularSpeed = rotationSpeed;
				this.navMeshAgent.destination = targetPoint;
			}
			if (this.externalMove != null)
			{
				this.externalMove(targetPoint, targetSpeed, rotationSpeed);
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000028E0 File Offset: 0x00000AE0
		public void RotateAgent(Transform target, float rotationSpeed)
		{
			this.targetRotationPoint = target.position;
			this.lastKnownTarget = target;
			this.rotationSpeed = rotationSpeed;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00022854 File Offset: 0x00020A54
		private void RotateAgent()
		{
			bool flag = this.navMeshAgent != null;
			bool updateRotation = false;
			Vector3 position = this.thisTFM.position;
			Quaternion rotation = this.thisTFM.rotation;
			Vector3 sightPosition = this.GetSightPosition();
			if (this.lastKnownTarget != null)
			{
				Vector3 position2 = this.lastKnownTarget.position;
				if (Physics.Linecast(sightPosition, position2, this.raycastLayers))
				{
					this.lastKnownTarget = null;
				}
				else
				{
					this.targetRotationPoint = position2;
				}
			}
			this.targetRotationPoint.y = position.y;
			this.thisTFM.LookAt(this.targetRotationPoint);
			if (flag)
			{
				updateRotation = this.navMeshAgent.updateRotation;
				this.navMeshAgent.updateRotation = false;
			}
			if (this.rotationSpeed > 0f && Time.deltaTime > 0f)
			{
				this.thisTFM.rotation = Quaternion.RotateTowards(rotation, this.thisTFM.rotation, this.rotationSpeed * Time.deltaTime);
			}
			if (flag)
			{
				this.navMeshAgent.updateRotation = updateRotation;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00022978 File Offset: 0x00020B78
		public void PlayAnimation(AIAnimationState animState)
		{
			if (this.onPlayAnimation != null)
			{
				this.onPlayAnimation(animState);
			}
			else if (this.animationCallbackComponent != null && animState != null && !string.IsNullOrEmpty(this.animationCallbackMethodName))
			{
				this.animationCallbackComponent.SendMessage(this.animationCallbackMethodName, animState);
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000028FC File Offset: 0x00000AFC
		public float GetHealthValue()
		{
			return this.health;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002904 File Offset: 0x00000B04
		public void SetHealthValue(float healthAmount)
		{
			this.health = healthAmount;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000290D File Offset: 0x00000B0D
		public void AddHealthValue(float healthAmount)
		{
			this.health += healthAmount;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000291D File Offset: 0x00000B1D
		public void SubtractHealthValue(float healthAmount)
		{
			this.health -= healthAmount;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000229E0 File Offset: 0x00020BE0
		public Transform GetClosestPlayer(Transform[] playerTransforms)
		{
			float num;
			return this.GetClosestPlayer(playerTransforms, out num);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000229F8 File Offset: 0x00020BF8
		public Transform GetClosestPlayer(Transform[] playerTransforms, out float squareDistance)
		{
			int num = -1;
			squareDistance = float.PositiveInfinity;
			for (int i = 0; i < playerTransforms.Length; i++)
			{
				Vector3 position = playerTransforms[i].position;
				Vector3 vector = position - this.thisPos;
				if (vector.sqrMagnitude < squareDistance)
				{
					squareDistance = vector.sqrMagnitude;
					num = i;
				}
			}
			if (num == -1)
			{
				return null;
			}
			return playerTransforms[num];
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000292D File Offset: 0x00000B2D
		public Transform GetClosestPlayerWithinSight(Transform[] playerTransforms)
		{
			return this.GetClosestPlayerWithinSight(playerTransforms, false);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00022A60 File Offset: 0x00020C60
		public Transform GetClosestPlayerWithinSight(Transform[] playerTransforms, bool includeSightFalloff)
		{
			float num = 0f;
			return this.GetClosestPlayerWithinSight(playerTransforms, out num, includeSightFalloff);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00022A80 File Offset: 0x00020C80
		public Transform GetClosestPlayerWithinSight(Transform[] playerTransforms, out float squareDistance, bool includeSightFalloff)
		{
			float num = this.sightDistance * this.sightDistance;
			int num2 = -1;
			squareDistance = float.PositiveInfinity;
			for (int i = 0; i < playerTransforms.Length; i++)
			{
				Vector3 position = playerTransforms[i].position;
				Vector3 vector = position - this.thisPos;
				float sqrMagnitude = vector.sqrMagnitude;
				float num3 = Vector3.Angle(vector, this.sightTransform.forward);
				Vector3 sightPosition = this.GetSightPosition();
				if (num2 == -1 || sqrMagnitude <= squareDistance)
				{
					if (sqrMagnitude < num)
					{
						float num4 = this.sightFOV / 2f;
						if (num3 < num4)
						{
							if (this.useSightFalloff && includeSightFalloff)
							{
								float num5 = Mathf.InverseLerp(0f, num4, num3);
								if (Random.value < num5)
								{
									goto IL_10B;
								}
							}
							RaycastHit raycastHit;
							if (!Physics.Linecast(sightPosition, position, ref raycastHit, this.raycastLayers) || raycastHit.transform == playerTransforms[i] || raycastHit.transform == this.thisTFM)
							{
								squareDistance = sqrMagnitude;
								num2 = i;
							}
						}
					}
				}
				IL_10B:;
			}
			if (num2 == -1)
			{
				return null;
			}
			return playerTransforms[num2];
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002937 File Offset: 0x00000B37
		private Vector3 GetSightPosition()
		{
			if (this.useSightTransform)
			{
				return this.sightTransform.position;
			}
			return this.sightTransform.TransformPoint(this.eyePosition);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00022BB4 File Offset: 0x00020DB4
		public void PlayAudio()
		{
			if (this.currentState.audioClip == null)
			{
				return;
			}
			if (base.audio == null)
			{
				base.gameObject.AddComponent<AudioSource>();
			}
			base.audio.loop = this.currentState.loopAudio;
			base.audio.volume = this.currentState.audioVolume;
			base.audio.pitch = this.currentState.audioPitch + (Random.value * this.currentState.audioPitchRandomness - this.currentState.audioPitchRandomness / 2f) * 2f;
			base.audio.clip = this.currentState.audioClip;
			base.audio.Play();
		}

		// Token: 0x04000021 RID: 33
		public bool isDefending;

		// Token: 0x04000022 RID: 34
		public float damageMultiplier = 1f;

		// Token: 0x04000023 RID: 35
		private Transform thisTFM;

		// Token: 0x04000024 RID: 36
		private Vector3 thisPos;

		// Token: 0x04000025 RID: 37
		private NavMeshAgent navMeshAgent;

		// Token: 0x04000026 RID: 38
		public bool useSightTransform;

		// Token: 0x04000027 RID: 39
		public Transform sightTransform;

		// Token: 0x04000028 RID: 40
		public BaseState initialState;

		// Token: 0x04000029 RID: 41
		public BaseState[] states = new BaseState[0];

		// Token: 0x0400002A RID: 42
		public BaseTrigger[] triggers = new BaseTrigger[0];

		// Token: 0x0400002B RID: 43
		public TaggedObjectFinder objectFinder;

		// Token: 0x0400002C RID: 44
		public LayerMask raycastLayers = -1;

		// Token: 0x0400002D RID: 45
		public bool useSightFalloff = true;

		// Token: 0x0400002E RID: 46
		public float sightFOV = 180f;

		// Token: 0x0400002F RID: 47
		public float sightDistance = 50f;

		// Token: 0x04000030 RID: 48
		public Vector3 eyePosition = new Vector3(0f, 1.5f, 0f);

		// Token: 0x04000031 RID: 49
		public float health = 100f;

		// Token: 0x04000032 RID: 50
		public float maxHealth = 100f;

		// Token: 0x04000033 RID: 51
		public GameObject statesGameObject;

		// Token: 0x04000034 RID: 52
		public Component animationCallbackComponent;

		// Token: 0x04000035 RID: 53
		public string animationCallbackMethodName = string.Empty;

		// Token: 0x04000036 RID: 54
		public AIAnimationStates animationStates;

		// Token: 0x04000037 RID: 55
		public AIBehaviors.StateChangedDelegate onStateChanged;

		// Token: 0x04000038 RID: 56
		public AIBehaviors.AnimationCallbackDelegate onPlayAnimation;

		// Token: 0x04000039 RID: 57
		public AIBehaviors.ExternalMoveDelegate externalMove;

		// Token: 0x0400003A RID: 58
		private Transform lastKnownTarget;

		// Token: 0x0400003B RID: 59
		private Vector3 targetRotationPoint;

		// Token: 0x0400003C RID: 60
		private float rotationSpeed;

		// Token: 0x02000013 RID: 19
		// (Invoke) Token: 0x06000068 RID: 104
		public delegate void StateChangedDelegate(BaseState newState, BaseState previousState);

		// Token: 0x02000014 RID: 20
		// (Invoke) Token: 0x0600006C RID: 108
		public delegate void AnimationCallbackDelegate(AIAnimationState animationState);

		// Token: 0x02000015 RID: 21
		// (Invoke) Token: 0x06000070 RID: 112
		public delegate void ExternalMoveDelegate(Vector3 targetPoint, float targetSpeed, float rotationSpeed);
	}
}
