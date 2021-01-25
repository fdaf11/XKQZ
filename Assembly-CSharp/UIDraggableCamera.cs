using System;
using UnityEngine;

// Token: 0x0200045B RID: 1115
[AddComponentMenu("NGUI/Interaction/Draggable Camera")]
[RequireComponent(typeof(Camera))]
public class UIDraggableCamera : MonoBehaviour
{
	// Token: 0x17000204 RID: 516
	// (get) Token: 0x06001ABF RID: 6847 RVA: 0x00011A35 File Offset: 0x0000FC35
	// (set) Token: 0x06001AC0 RID: 6848 RVA: 0x00011A3D File Offset: 0x0000FC3D
	public Vector2 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
		}
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x000D4044 File Offset: 0x000D2244
	private void Start()
	{
		this.mCam = base.camera;
		this.mTrans = base.transform;
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		if (this.rootForBounds == null)
		{
			Debug.LogError(NGUITools.GetHierarchy(base.gameObject) + " needs the 'Root For Bounds' parameter to be set", this);
			base.enabled = false;
		}
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x000D40B0 File Offset: 0x000D22B0
	private Vector3 CalculateConstrainOffset()
	{
		if (this.rootForBounds == null || this.rootForBounds.childCount == 0)
		{
			return Vector3.zero;
		}
		Vector3 vector;
		vector..ctor(this.mCam.rect.xMin * (float)Screen.width, this.mCam.rect.yMin * (float)Screen.height, 0f);
		Vector3 vector2;
		vector2..ctor(this.mCam.rect.xMax * (float)Screen.width, this.mCam.rect.yMax * (float)Screen.height, 0f);
		vector = this.mCam.ScreenToWorldPoint(vector);
		vector2 = this.mCam.ScreenToWorldPoint(vector2);
		Vector2 minRect;
		minRect..ctor(this.mBounds.min.x, this.mBounds.min.y);
		Vector2 maxRect;
		maxRect..ctor(this.mBounds.max.x, this.mBounds.max.y);
		return NGUIMath.ConstrainRect(minRect, maxRect, vector, vector2);
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x000D41F8 File Offset: 0x000D23F8
	public bool ConstrainToBounds(bool immediate)
	{
		if (this.mTrans != null && this.rootForBounds != null)
		{
			Vector3 vector = this.CalculateConstrainOffset();
			if (vector.sqrMagnitude > 0f)
			{
				if (immediate)
				{
					this.mTrans.position -= vector;
				}
				else
				{
					SpringPosition springPosition = SpringPosition.Begin(base.gameObject, this.mTrans.position - vector, 13f);
					springPosition.ignoreTimeScale = true;
					springPosition.worldSpace = true;
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x000D4294 File Offset: 0x000D2494
	public void Press(bool isPressed)
	{
		if (isPressed)
		{
			this.mDragStarted = false;
		}
		if (this.rootForBounds != null)
		{
			this.mPressed = isPressed;
			if (isPressed)
			{
				this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
				this.mMomentum = Vector2.zero;
				this.mScroll = 0f;
				SpringPosition component = base.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else if (this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring)
			{
				this.ConstrainToBounds(false);
			}
		}
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x000D4328 File Offset: 0x000D2528
	public void Drag(Vector2 delta)
	{
		if (this.smoothDragStart && !this.mDragStarted)
		{
			this.mDragStarted = true;
			return;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		if (this.mRoot != null)
		{
			delta *= this.mRoot.pixelSizeAdjustment;
		}
		Vector2 vector = Vector2.Scale(delta, -this.scale);
		this.mTrans.localPosition += vector;
		this.mMomentum = Vector2.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
		if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.ConstrainToBounds(true))
		{
			this.mMomentum = Vector2.zero;
			this.mScroll = 0f;
		}
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x000D4414 File Offset: 0x000D2614
	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			this.mScroll += delta * this.scrollWheelFactor;
		}
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x000D4474 File Offset: 0x000D2674
	private void Update()
	{
		float deltaTime = RealTime.deltaTime;
		if (this.mPressed)
		{
			SpringPosition component = base.GetComponent<SpringPosition>();
			if (component != null)
			{
				component.enabled = false;
			}
			this.mScroll = 0f;
		}
		else
		{
			this.mMomentum += this.scale * (this.mScroll * 20f);
			this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
			if (this.mMomentum.magnitude > 0.01f)
			{
				this.mTrans.localPosition += NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
				this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
				if (!this.ConstrainToBounds(this.dragEffect == UIDragObject.DragEffect.None))
				{
					SpringPosition component2 = base.GetComponent<SpringPosition>();
					if (component2 != null)
					{
						component2.enabled = false;
					}
				}
				return;
			}
			this.mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
	}

	// Token: 0x04001F8C RID: 8076
	public Transform rootForBounds;

	// Token: 0x04001F8D RID: 8077
	public Vector2 scale = Vector2.one;

	// Token: 0x04001F8E RID: 8078
	public float scrollWheelFactor;

	// Token: 0x04001F8F RID: 8079
	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	// Token: 0x04001F90 RID: 8080
	public bool smoothDragStart = true;

	// Token: 0x04001F91 RID: 8081
	public float momentumAmount = 35f;

	// Token: 0x04001F92 RID: 8082
	private Camera mCam;

	// Token: 0x04001F93 RID: 8083
	private Transform mTrans;

	// Token: 0x04001F94 RID: 8084
	private bool mPressed;

	// Token: 0x04001F95 RID: 8085
	private Vector2 mMomentum = Vector2.zero;

	// Token: 0x04001F96 RID: 8086
	private Bounds mBounds;

	// Token: 0x04001F97 RID: 8087
	private float mScroll;

	// Token: 0x04001F98 RID: 8088
	private UIRoot mRoot;

	// Token: 0x04001F99 RID: 8089
	private bool mDragStarted;
}
