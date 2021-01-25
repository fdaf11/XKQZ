using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class CameraPathAnimator : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060001F3 RID: 499 RVA: 0x000037F6 File Offset: 0x000019F6
	// (remove) Token: 0x060001F4 RID: 500 RVA: 0x0000380F File Offset: 0x00001A0F
	public event CameraPathAnimator.AnimationStartedEventHandler AnimationStartedEvent;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x060001F5 RID: 501 RVA: 0x00003828 File Offset: 0x00001A28
	// (remove) Token: 0x060001F6 RID: 502 RVA: 0x00003841 File Offset: 0x00001A41
	public event CameraPathAnimator.AnimationPausedEventHandler AnimationPausedEvent;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x060001F7 RID: 503 RVA: 0x0000385A File Offset: 0x00001A5A
	// (remove) Token: 0x060001F8 RID: 504 RVA: 0x00003873 File Offset: 0x00001A73
	public event CameraPathAnimator.AnimationStoppedEventHandler AnimationStoppedEvent;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x060001F9 RID: 505 RVA: 0x0000388C File Offset: 0x00001A8C
	// (remove) Token: 0x060001FA RID: 506 RVA: 0x000038A5 File Offset: 0x00001AA5
	public event CameraPathAnimator.AnimationFinishedEventHandler AnimationFinishedEvent;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x060001FB RID: 507 RVA: 0x000038BE File Offset: 0x00001ABE
	// (remove) Token: 0x060001FC RID: 508 RVA: 0x000038D7 File Offset: 0x00001AD7
	public event CameraPathAnimator.AnimationLoopedEventHandler AnimationLoopedEvent;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x060001FD RID: 509 RVA: 0x000038F0 File Offset: 0x00001AF0
	// (remove) Token: 0x060001FE RID: 510 RVA: 0x00003909 File Offset: 0x00001B09
	public event CameraPathAnimator.AnimationPingPongEventHandler AnimationPingPongEvent;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x060001FF RID: 511 RVA: 0x00003922 File Offset: 0x00001B22
	// (remove) Token: 0x06000200 RID: 512 RVA: 0x0000393B File Offset: 0x00001B3B
	public event CameraPathAnimator.AnimationPointReachedEventHandler AnimationPointReachedEvent;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06000201 RID: 513 RVA: 0x00003954 File Offset: 0x00001B54
	// (remove) Token: 0x06000202 RID: 514 RVA: 0x0000396D File Offset: 0x00001B6D
	public event CameraPathAnimator.AnimationPointReachedWithNumberEventHandler AnimationPointReachedWithNumberEvent;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06000203 RID: 515 RVA: 0x00003986 File Offset: 0x00001B86
	// (remove) Token: 0x06000204 RID: 516 RVA: 0x0000399F File Offset: 0x00001B9F
	public event CameraPathAnimator.AnimationCustomEventHandler AnimationCustomEvent;

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000205 RID: 517 RVA: 0x000039B8 File Offset: 0x00001BB8
	// (set) Token: 0x06000206 RID: 518 RVA: 0x000039C0 File Offset: 0x00001BC0
	public float pathSpeed
	{
		get
		{
			return this._pathSpeed;
		}
		set
		{
			if (this._cameraPath.speedList.listEnabled)
			{
				Debug.LogWarning("Path Speed in Animator component is ignored and overridden by Camera Path speed points.");
			}
			this._pathSpeed = Mathf.Max(value, this.minimumCameraSpeed);
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x06000207 RID: 519 RVA: 0x000039F3 File Offset: 0x00001BF3
	// (set) Token: 0x06000208 RID: 520 RVA: 0x000039FB File Offset: 0x00001BFB
	public float animationTime
	{
		get
		{
			return this._pathTime;
		}
		set
		{
			if (this.animationMode != CameraPathAnimator.animationModes.still)
			{
				Debug.LogWarning("Path time is ignored and overridden during animation when not in Animation Mode Still.");
			}
			this._pathTime = Mathf.Max(value, 0f);
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000209 RID: 521 RVA: 0x00003A24 File Offset: 0x00001C24
	public float currentTime
	{
		get
		{
			return this._pathTime * this._percentage;
		}
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00026E3C File Offset: 0x0002503C
	public void Play()
	{
		this._playing = true;
		if (!this.isReversed)
		{
			if (this._percentage == 0f)
			{
				if (this.AnimationStartedEvent != null)
				{
					this.AnimationStartedEvent();
				}
				this.cameraPath.eventList.OnAnimationStart(0f);
			}
		}
		else if (this._percentage == 1f)
		{
			if (this.AnimationStartedEvent != null)
			{
				this.AnimationStartedEvent();
			}
			this.cameraPath.eventList.OnAnimationStart(1f);
		}
		this._lastPercentage = this._percentage;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x00003A33 File Offset: 0x00001C33
	public void Stop()
	{
		this._playing = false;
		this._percentage = 0f;
		if (this.AnimationStoppedEvent != null)
		{
			this.AnimationStoppedEvent();
		}
	}

	// Token: 0x0600020C RID: 524 RVA: 0x00003A5D File Offset: 0x00001C5D
	public void Pause()
	{
		this._playing = false;
		if (this.AnimationPausedEvent != null)
		{
			this.AnimationPausedEvent();
		}
	}

	// Token: 0x0600020D RID: 525 RVA: 0x00026EE4 File Offset: 0x000250E4
	public void Seek(float value)
	{
		this._percentage = Mathf.Clamp01(value);
		this._lastPercentage = this._percentage;
		this.UpdateAnimationTime(false);
		this.UpdatePointReached();
		bool playing = this._playing;
		this._playing = true;
		this.UpdateAnimation();
		this._playing = playing;
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x0600020E RID: 526 RVA: 0x00003A7C File Offset: 0x00001C7C
	public bool isPlaying
	{
		get
		{
			return this._playing;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x0600020F RID: 527 RVA: 0x00003A84 File Offset: 0x00001C84
	public float percentage
	{
		get
		{
			return this._percentage;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000210 RID: 528 RVA: 0x00003A8C File Offset: 0x00001C8C
	public bool pingPongGoingForward
	{
		get
		{
			return this.pingPongDirection == 1f;
		}
	}

	// Token: 0x06000211 RID: 529 RVA: 0x00026F34 File Offset: 0x00025134
	public void Reverse()
	{
		switch (this.animationMode)
		{
		case CameraPathAnimator.animationModes.once:
			this.animationMode = CameraPathAnimator.animationModes.reverse;
			break;
		case CameraPathAnimator.animationModes.loop:
			this.animationMode = CameraPathAnimator.animationModes.reverseLoop;
			break;
		case CameraPathAnimator.animationModes.reverse:
			this.animationMode = CameraPathAnimator.animationModes.once;
			break;
		case CameraPathAnimator.animationModes.reverseLoop:
			this.animationMode = CameraPathAnimator.animationModes.loop;
			break;
		case CameraPathAnimator.animationModes.pingPong:
			this.pingPongDirection = (float)((this.pingPongDirection != -1f) ? -1 : 1);
			break;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000212 RID: 530 RVA: 0x00003A9B File Offset: 0x00001C9B
	public CameraPath cameraPath
	{
		get
		{
			if (!this._cameraPath)
			{
				this._cameraPath = base.GetComponent<CameraPath>();
			}
			return this._cameraPath;
		}
	}

	// Token: 0x06000213 RID: 531 RVA: 0x00026FBC File Offset: 0x000251BC
	public Quaternion GetAnimatedOrientation(float percent, bool ignoreNormalisation)
	{
		Quaternion quaternion = Quaternion.identity;
		switch (this.orientationMode)
		{
		case CameraPathAnimator.orientationModes.custom:
			quaternion = this.cameraPath.GetPathRotation(percent, ignoreNormalisation);
			break;
		case CameraPathAnimator.orientationModes.target:
		{
			Vector3 pathPosition = this.cameraPath.GetPathPosition(percent);
			Vector3 vector;
			if (this.orientationTarget != null)
			{
				vector = this.orientationTarget.transform.position - pathPosition;
			}
			else
			{
				vector = Vector3.forward;
			}
			quaternion = Quaternion.LookRotation(vector, this.targetModeUp);
			break;
		}
		case CameraPathAnimator.orientationModes.mouselook:
			if (!Application.isPlaying)
			{
				quaternion = Quaternion.LookRotation(this.cameraPath.GetPathDirection(percent));
				quaternion *= Quaternion.Euler(base.transform.forward * -this.cameraPath.GetPathTilt(percent));
			}
			else
			{
				quaternion = Quaternion.LookRotation(this.cameraPath.GetPathDirection(percent));
				quaternion *= this.GetMouseLook();
			}
			break;
		case CameraPathAnimator.orientationModes.followpath:
			quaternion = Quaternion.LookRotation(this.cameraPath.GetPathDirection(percent));
			quaternion *= Quaternion.Euler(base.transform.forward * -this.cameraPath.GetPathTilt(percent));
			break;
		case CameraPathAnimator.orientationModes.reverseFollowpath:
			quaternion = Quaternion.LookRotation(-this.cameraPath.GetPathDirection(percent));
			quaternion *= Quaternion.Euler(base.transform.forward * -this.cameraPath.GetPathTilt(percent));
			break;
		case CameraPathAnimator.orientationModes.followTransform:
		{
			if (this.orientationTarget == null)
			{
				return Quaternion.identity;
			}
			float num = this.cameraPath.GetNearestPoint(this.orientationTarget.position);
			num = Mathf.Clamp01(num + this.nearestOffset);
			Vector3 pathPosition = this.cameraPath.GetPathPosition(num);
			Vector3 vector = this.orientationTarget.transform.position - pathPosition;
			quaternion = Quaternion.LookRotation(vector);
			break;
		}
		case CameraPathAnimator.orientationModes.twoDimentions:
			quaternion = Quaternion.LookRotation(Vector3.forward);
			break;
		case CameraPathAnimator.orientationModes.fixedOrientation:
			quaternion = Quaternion.LookRotation(this.fixedOrientaion);
			break;
		case CameraPathAnimator.orientationModes.none:
			quaternion = this.animationObject.rotation;
			break;
		}
		return quaternion * base.transform.rotation;
	}

	// Token: 0x06000214 RID: 532 RVA: 0x00027210 File Offset: 0x00025410
	private void Awake()
	{
		if (this.animationObject == null)
		{
			this._isCamera = false;
		}
		else
		{
			this.animationObjectCamera = this.animationObject.GetComponentInChildren<Camera>();
			this._isCamera = (this.animationObjectCamera != null);
		}
		Camera[] allCameras = Camera.allCameras;
		if (allCameras.Length == 0)
		{
			Debug.LogWarning("Warning: There are no cameras in the scene");
			this._isCamera = false;
		}
		if (!this.isReversed)
		{
			this._percentage = 0f + this.startPercent;
		}
		else
		{
			this._percentage = 1f - this.startPercent;
		}
	}

	// Token: 0x06000215 RID: 533 RVA: 0x000272B0 File Offset: 0x000254B0
	private void OnEnable()
	{
		this.cameraPath.eventList.CameraPathEventPoint += this.OnCustomEvent;
		this.cameraPath.delayList.CameraPathDelayEvent += this.OnDelayEvent;
		if (this.animationObject != null)
		{
			this.animationObjectCamera = this.animationObject.GetComponentInChildren<Camera>();
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x00027318 File Offset: 0x00025518
	private void Start()
	{
		if (this.playOnStart)
		{
			this.Play();
		}
		if (Application.isPlaying && this.orientationTarget == null && (this.orientationMode == CameraPathAnimator.orientationModes.followTransform || this.orientationMode == CameraPathAnimator.orientationModes.target))
		{
			Debug.LogWarning("There has not been an orientation target specified in the Animation component of Camera Path.", base.transform);
		}
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0002737C File Offset: 0x0002557C
	private void Update()
	{
		if (!this.isCamera)
		{
			if (this._playing)
			{
				this.UpdateAnimation();
				this.UpdatePointReached();
				this.UpdateAnimationTime();
			}
			else if (this._cameraPath.nextPath != null && this._percentage >= 1f)
			{
				this.PlayNextAnimation();
			}
		}
	}

	// Token: 0x06000218 RID: 536 RVA: 0x000273E4 File Offset: 0x000255E4
	private void LateUpdate()
	{
		if (this.isCamera)
		{
			if (this._playing)
			{
				this.UpdateAnimation();
				this.UpdatePointReached();
				this.UpdateAnimationTime();
			}
			else if (this._cameraPath.nextPath != null && this._percentage >= 1f)
			{
				this.PlayNextAnimation();
			}
		}
	}

	// Token: 0x06000219 RID: 537 RVA: 0x00003ABF File Offset: 0x00001CBF
	private void OnDisable()
	{
		this.CleanUp();
	}

	// Token: 0x0600021A RID: 538 RVA: 0x00003ABF File Offset: 0x00001CBF
	private void OnDestroy()
	{
		this.CleanUp();
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0002744C File Offset: 0x0002564C
	private void PlayNextAnimation()
	{
		if (this._cameraPath.nextPath != null)
		{
			this._cameraPath.nextPath.GetComponent<CameraPathAnimator>().Play();
			this._percentage %= 1f;
			this.Stop();
		}
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0002749C File Offset: 0x0002569C
	private void UpdateAnimation()
	{
		if (this.animationObject == null)
		{
			Debug.LogError("There is no animation object specified in the Camera Path Animator component. Nothing to animate.\nYou can find this component in the main camera path game object called " + base.gameObject.name + ".");
			this.Stop();
			return;
		}
		if (!this._playing)
		{
			return;
		}
		if (this.animationMode != CameraPathAnimator.animationModes.still)
		{
			if (this.cameraPath.speedList.listEnabled)
			{
				this._pathTime = this._cameraPath.pathLength / Mathf.Max(this.cameraPath.GetPathSpeed(this._percentage), this.minimumCameraSpeed);
			}
			else
			{
				this._pathTime = this._cameraPath.pathLength / Mathf.Max(this._pathSpeed * this.cameraPath.GetPathEase(this._percentage), this.minimumCameraSpeed);
			}
			this.animationObject.position = this.cameraPath.GetPathPosition(this._percentage);
		}
		if (this.orientationMode != CameraPathAnimator.orientationModes.none)
		{
			this.animationObject.rotation = this.GetAnimatedOrientation(this._percentage, false);
		}
		if (this.isCamera && this._cameraPath.fovList.listEnabled)
		{
			if (!this.animationObjectCamera.orthographic)
			{
				this.animationObjectCamera.fieldOfView = this._cameraPath.GetPathFOV(this._percentage);
			}
			else
			{
				this.animationObjectCamera.orthographicSize = this._cameraPath.GetPathOrthographicSize(this._percentage);
			}
		}
		this.CheckEvents();
	}

	// Token: 0x0600021D RID: 541 RVA: 0x00027628 File Offset: 0x00025828
	private void UpdatePointReached()
	{
		if (this._percentage == this._lastPercentage)
		{
			return;
		}
		if (Mathf.Abs(this.percentage - this._lastPercentage) > 0.999f)
		{
			this._lastPercentage = this.percentage;
			return;
		}
		for (int i = 0; i < this.cameraPath.realNumberOfPoints; i++)
		{
			CameraPathControlPoint cameraPathControlPoint = this.cameraPath[i];
			bool flag = (cameraPathControlPoint.percentage >= this._lastPercentage && cameraPathControlPoint.percentage <= this.percentage) || (cameraPathControlPoint.percentage >= this.percentage && cameraPathControlPoint.percentage <= this._lastPercentage);
			if (flag)
			{
				if (this.AnimationPointReachedEvent != null)
				{
					this.AnimationPointReachedEvent();
				}
				if (this.AnimationPointReachedWithNumberEvent != null)
				{
					this.AnimationPointReachedWithNumberEvent(i);
				}
			}
		}
		this._lastPercentage = this.percentage;
	}

	// Token: 0x0600021E RID: 542 RVA: 0x00003AC7 File Offset: 0x00001CC7
	private void UpdateAnimationTime()
	{
		this.UpdateAnimationTime(true);
	}

	// Token: 0x0600021F RID: 543 RVA: 0x00027724 File Offset: 0x00025924
	private void UpdateAnimationTime(bool advance)
	{
		if (this.orientationMode == CameraPathAnimator.orientationModes.followTransform)
		{
			return;
		}
		if (this.delayTime > 0f)
		{
			this.delayTime += -Time.deltaTime;
			return;
		}
		if (advance)
		{
			switch (this.animationMode)
			{
			case CameraPathAnimator.animationModes.once:
				if (this._percentage >= 1f)
				{
					this._playing = false;
					if (this.AnimationFinishedEvent != null)
					{
						this.AnimationFinishedEvent();
					}
				}
				else
				{
					this._percentage += Time.deltaTime * (1f / this._pathTime);
				}
				break;
			case CameraPathAnimator.animationModes.loop:
				if (this._percentage >= 1f)
				{
					this._percentage = 0f;
					this._lastPercentage = 0f;
					if (this.AnimationLoopedEvent != null)
					{
						this.AnimationLoopedEvent();
					}
				}
				this._percentage += Time.deltaTime * (1f / this._pathTime);
				break;
			case CameraPathAnimator.animationModes.reverse:
				if (this._percentage <= 0f)
				{
					this._percentage = 0f;
					this._playing = false;
					if (this.AnimationFinishedEvent != null)
					{
						this.AnimationFinishedEvent();
					}
				}
				else
				{
					this._percentage += -Time.deltaTime * (1f / this._pathTime);
				}
				break;
			case CameraPathAnimator.animationModes.reverseLoop:
				if (this._percentage <= 0f)
				{
					this._percentage = 1f;
					this._lastPercentage = 1f;
					if (this.AnimationLoopedEvent != null)
					{
						this.AnimationLoopedEvent();
					}
				}
				this._percentage += -Time.deltaTime * (1f / this._pathTime);
				break;
			case CameraPathAnimator.animationModes.pingPong:
			{
				float num = Time.deltaTime * (1f / this._pathTime);
				this._percentage += num * this.pingPongDirection;
				if (this._percentage >= 1f)
				{
					this._percentage = 1f - num;
					this._lastPercentage = 1f;
					this.pingPongDirection = -1f;
					if (this.AnimationPingPongEvent != null)
					{
						this.AnimationPingPongEvent();
					}
				}
				if (this._percentage <= 0f)
				{
					this._percentage = num;
					this._lastPercentage = 0f;
					this.pingPongDirection = 1f;
					if (this.AnimationPingPongEvent != null)
					{
						this.AnimationPingPongEvent();
					}
				}
				break;
			}
			case CameraPathAnimator.animationModes.still:
				if (this._percentage >= 1f)
				{
					this._playing = false;
					if (this.AnimationFinishedEvent != null)
					{
						this.AnimationFinishedEvent();
					}
				}
				else
				{
					this._percentage += Time.deltaTime * (1f / this._pathTime);
				}
				break;
			}
		}
		this._percentage = Mathf.Clamp01(this._percentage);
	}

	// Token: 0x06000220 RID: 544 RVA: 0x00027A2C File Offset: 0x00025C2C
	private Quaternion GetMouseLook()
	{
		if (this.animationObject == null)
		{
			return Quaternion.identity;
		}
		float num = (float)Screen.width / 2f;
		float num2 = (float)Screen.height / 2f;
		float num3 = (Input.mousePosition.x - num) / (float)Screen.width * 180f;
		float num4 = ((float)Screen.height - Input.mousePosition.y - num2) / (float)Screen.height * 180f;
		num4 = Mathf.Clamp(num4, this.minX, this.maxX);
		return Quaternion.Euler(new Vector3(num4, num3, 0f));
	}

	// Token: 0x06000221 RID: 545 RVA: 0x00003AD0 File Offset: 0x00001CD0
	private void CheckEvents()
	{
		this.cameraPath.CheckEvents(this._percentage);
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000222 RID: 546 RVA: 0x00003AE3 File Offset: 0x00001CE3
	private bool isReversed
	{
		get
		{
			return this.animationMode == CameraPathAnimator.animationModes.reverse || this.animationMode == CameraPathAnimator.animationModes.reverseLoop || this.pingPongDirection < 0f;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000223 RID: 547 RVA: 0x00003B0D File Offset: 0x00001D0D
	public bool isCamera
	{
		get
		{
			if (this.animationObject == null)
			{
				this._isCamera = false;
			}
			else
			{
				this._isCamera = (this.animationObjectCamera != null);
			}
			return this._isCamera;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000224 RID: 548 RVA: 0x00003B44 File Offset: 0x00001D44
	// (set) Token: 0x06000225 RID: 549 RVA: 0x00027AD4 File Offset: 0x00025CD4
	public bool animateSceneObjectInEditor
	{
		get
		{
			return this._animateSceneObjectInEditor;
		}
		set
		{
			if (value != this._animateSceneObjectInEditor)
			{
				this._animateSceneObjectInEditor = value;
				if (this.animationObject != null && this.animationMode != CameraPathAnimator.animationModes.still)
				{
					if (this._animateSceneObjectInEditor)
					{
						this.animatedObjectStartPosition = this.animationObject.transform.position;
						this.animatedObjectStartRotation = this.animationObject.transform.rotation;
					}
					else
					{
						this.animationObject.transform.position = this.animatedObjectStartPosition;
						this.animationObject.transform.rotation = this.animatedObjectStartRotation;
					}
				}
			}
			this._animateSceneObjectInEditor = value;
		}
	}

	// Token: 0x06000226 RID: 550 RVA: 0x00003B4C File Offset: 0x00001D4C
	private void CleanUp()
	{
		this.cameraPath.eventList.CameraPathEventPoint += this.OnCustomEvent;
		this.cameraPath.delayList.CameraPathDelayEvent += this.OnDelayEvent;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x00003B86 File Offset: 0x00001D86
	private void OnDelayEvent(float time)
	{
		if (time > 0f)
		{
			this.delayTime = time;
		}
		else
		{
			this.Pause();
		}
	}

	// Token: 0x06000228 RID: 552 RVA: 0x00003BA5 File Offset: 0x00001DA5
	private void OnCustomEvent(string eventName)
	{
		if (this.AnimationCustomEvent != null)
		{
			this.AnimationCustomEvent(eventName);
		}
	}

	// Token: 0x0400018D RID: 397
	public float minimumCameraSpeed = 0.01f;

	// Token: 0x0400018E RID: 398
	public Transform orientationTarget;

	// Token: 0x0400018F RID: 399
	[SerializeField]
	private CameraPath _cameraPath;

	// Token: 0x04000190 RID: 400
	public bool playOnStart = true;

	// Token: 0x04000191 RID: 401
	public Transform animationObject;

	// Token: 0x04000192 RID: 402
	private Camera animationObjectCamera;

	// Token: 0x04000193 RID: 403
	private bool _isCamera = true;

	// Token: 0x04000194 RID: 404
	private bool _playing;

	// Token: 0x04000195 RID: 405
	public CameraPathAnimator.animationModes animationMode;

	// Token: 0x04000196 RID: 406
	public CameraPathAnimator.orientationModes orientationMode;

	// Token: 0x04000197 RID: 407
	private float pingPongDirection = 1f;

	// Token: 0x04000198 RID: 408
	public Vector3 fixedOrientaion = Vector3.forward;

	// Token: 0x04000199 RID: 409
	public Vector3 fixedPosition;

	// Token: 0x0400019A RID: 410
	public bool normalised = true;

	// Token: 0x0400019B RID: 411
	public float editorPercentage;

	// Token: 0x0400019C RID: 412
	[SerializeField]
	private float _pathTime = 10f;

	// Token: 0x0400019D RID: 413
	[SerializeField]
	private float _pathSpeed = 10f;

	// Token: 0x0400019E RID: 414
	private float _percentage;

	// Token: 0x0400019F RID: 415
	private float _lastPercentage;

	// Token: 0x040001A0 RID: 416
	public float nearestOffset;

	// Token: 0x040001A1 RID: 417
	private float delayTime;

	// Token: 0x040001A2 RID: 418
	public float startPercent;

	// Token: 0x040001A3 RID: 419
	public bool animateFOV = true;

	// Token: 0x040001A4 RID: 420
	public Vector3 targetModeUp = Vector3.up;

	// Token: 0x040001A5 RID: 421
	public float sensitivity = 5f;

	// Token: 0x040001A6 RID: 422
	public float minX = -90f;

	// Token: 0x040001A7 RID: 423
	public float maxX = 90f;

	// Token: 0x040001A8 RID: 424
	public bool showPreview = true;

	// Token: 0x040001A9 RID: 425
	public GameObject editorPreview;

	// Token: 0x040001AA RID: 426
	public bool showScenePreview = true;

	// Token: 0x040001AB RID: 427
	private bool _animateSceneObjectInEditor;

	// Token: 0x040001AC RID: 428
	public Vector3 animatedObjectStartPosition;

	// Token: 0x040001AD RID: 429
	public Quaternion animatedObjectStartRotation;

	// Token: 0x0200005E RID: 94
	public enum animationModes
	{
		// Token: 0x040001B8 RID: 440
		once,
		// Token: 0x040001B9 RID: 441
		loop,
		// Token: 0x040001BA RID: 442
		reverse,
		// Token: 0x040001BB RID: 443
		reverseLoop,
		// Token: 0x040001BC RID: 444
		pingPong,
		// Token: 0x040001BD RID: 445
		still
	}

	// Token: 0x0200005F RID: 95
	public enum orientationModes
	{
		// Token: 0x040001BF RID: 447
		custom,
		// Token: 0x040001C0 RID: 448
		target,
		// Token: 0x040001C1 RID: 449
		mouselook,
		// Token: 0x040001C2 RID: 450
		followpath,
		// Token: 0x040001C3 RID: 451
		reverseFollowpath,
		// Token: 0x040001C4 RID: 452
		followTransform,
		// Token: 0x040001C5 RID: 453
		twoDimentions,
		// Token: 0x040001C6 RID: 454
		fixedOrientation,
		// Token: 0x040001C7 RID: 455
		none
	}

	// Token: 0x02000060 RID: 96
	// (Invoke) Token: 0x0600022A RID: 554
	public delegate void AnimationStartedEventHandler();

	// Token: 0x02000061 RID: 97
	// (Invoke) Token: 0x0600022E RID: 558
	public delegate void AnimationPausedEventHandler();

	// Token: 0x02000062 RID: 98
	// (Invoke) Token: 0x06000232 RID: 562
	public delegate void AnimationStoppedEventHandler();

	// Token: 0x02000063 RID: 99
	// (Invoke) Token: 0x06000236 RID: 566
	public delegate void AnimationFinishedEventHandler();

	// Token: 0x02000064 RID: 100
	// (Invoke) Token: 0x0600023A RID: 570
	public delegate void AnimationLoopedEventHandler();

	// Token: 0x02000065 RID: 101
	// (Invoke) Token: 0x0600023E RID: 574
	public delegate void AnimationPingPongEventHandler();

	// Token: 0x02000066 RID: 102
	// (Invoke) Token: 0x06000242 RID: 578
	public delegate void AnimationPointReachedEventHandler();

	// Token: 0x02000067 RID: 103
	// (Invoke) Token: 0x06000246 RID: 582
	public delegate void AnimationCustomEventHandler(string eventName);

	// Token: 0x02000068 RID: 104
	// (Invoke) Token: 0x0600024A RID: 586
	public delegate void AnimationPointReachedWithNumberEventHandler(int pointNumber);
}
