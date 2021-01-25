using System;
using UnityEngine;

// Token: 0x02000457 RID: 1111
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Drag Object")]
public class UIDragObject : MonoBehaviour
{
	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x00011906 File Offset: 0x0000FB06
	// (set) Token: 0x06001AA7 RID: 6823 RVA: 0x0001190E File Offset: 0x0000FB0E
	public Vector3 dragMovement
	{
		get
		{
			return this.scale;
		}
		set
		{
			this.scale = value;
		}
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x000D3538 File Offset: 0x000D1738
	private void OnEnable()
	{
		if (this.scrollWheelFactor != 0f)
		{
			this.scrollMomentum = this.scale * this.scrollWheelFactor;
			this.scrollWheelFactor = 0f;
		}
		if (this.contentRect == null && this.target != null && Application.isPlaying)
		{
			UIWidget component = this.target.GetComponent<UIWidget>();
			if (component != null)
			{
				this.contentRect = component;
			}
		}
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x00011917 File Offset: 0x0000FB17
	private void OnDisable()
	{
		this.mStarted = false;
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x000D35C4 File Offset: 0x000D17C4
	private void FindPanel()
	{
		this.panelRegion = ((!(this.target != null)) ? null : UIPanel.Find(this.target.transform.parent));
		if (this.panelRegion == null)
		{
			this.restrictWithinPanel = false;
		}
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x000D361C File Offset: 0x000D181C
	private void UpdateBounds()
	{
		if (this.contentRect)
		{
			Transform cachedTransform = this.panelRegion.cachedTransform;
			Matrix4x4 worldToLocalMatrix = cachedTransform.worldToLocalMatrix;
			Vector3[] worldCorners = this.contentRect.worldCorners;
			for (int i = 0; i < 4; i++)
			{
				worldCorners[i] = worldToLocalMatrix.MultiplyPoint3x4(worldCorners[i]);
			}
			this.mBounds = new Bounds(worldCorners[0], Vector3.zero);
			for (int j = 1; j < 4; j++)
			{
				this.mBounds.Encapsulate(worldCorners[j]);
			}
		}
		else
		{
			this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.panelRegion.cachedTransform, this.target);
		}
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x000D36F4 File Offset: 0x000D18F4
	private void OnPress(bool pressed)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			if (pressed)
			{
				if (!this.mPressed)
				{
					this.mTouchID = UICamera.currentTouchID;
					this.mPressed = true;
					this.mStarted = false;
					this.CancelMovement();
					if (this.restrictWithinPanel && this.panelRegion == null)
					{
						this.FindPanel();
					}
					if (this.restrictWithinPanel)
					{
						this.UpdateBounds();
					}
					this.CancelSpring();
					Transform transform = UICamera.currentCamera.transform;
					this.mPlane = new Plane(((!(this.panelRegion != null)) ? transform.rotation : this.panelRegion.cachedTransform.rotation) * Vector3.back, UICamera.lastWorldPosition);
				}
			}
			else if (this.mPressed && this.mTouchID == UICamera.currentTouchID)
			{
				this.mPressed = false;
				if (this.restrictWithinPanel && this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, false))
				{
					this.CancelMovement();
				}
			}
		}
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x000D3848 File Offset: 0x000D1A48
	private void OnDrag(Vector2 delta)
	{
		if (this.mPressed && this.mTouchID == UICamera.currentTouchID && base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float num = 0f;
			if (this.mPlane.Raycast(ray, ref num))
			{
				Vector3 point = ray.GetPoint(num);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (!this.mStarted)
				{
					this.mStarted = true;
					vector = Vector3.zero;
				}
				if (vector.x != 0f || vector.y != 0f)
				{
					vector = this.target.InverseTransformDirection(vector);
					vector.Scale(this.scale);
					vector = this.target.TransformDirection(vector);
				}
				if (this.dragEffect != UIDragObject.DragEffect.None)
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				}
				Vector3 localPosition = this.target.localPosition;
				this.Move(vector);
				if (this.restrictWithinPanel)
				{
					this.mBounds.center = this.mBounds.center + (this.target.localPosition - localPosition);
					if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, true))
					{
						this.CancelMovement();
					}
				}
			}
		}
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x000D3A14 File Offset: 0x000D1C14
	private void Move(Vector3 worldDelta)
	{
		if (this.panelRegion != null)
		{
			this.mTargetPos += worldDelta;
			this.target.position = this.mTargetPos;
			Vector3 localPosition = this.target.localPosition;
			localPosition.x = Mathf.Round(localPosition.x);
			localPosition.y = Mathf.Round(localPosition.y);
			this.target.localPosition = localPosition;
			UIScrollView component = this.panelRegion.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
		else
		{
			this.target.position += worldDelta;
		}
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x000D3AD0 File Offset: 0x000D1CD0
	private void LateUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		this.mMomentum -= this.mScroll;
		this.mScroll = NGUIMath.SpringLerp(this.mScroll, Vector3.zero, 20f, deltaTime);
		if (this.mMomentum.magnitude < 0.0001f)
		{
			return;
		}
		if (!this.mPressed)
		{
			if (this.panelRegion == null)
			{
				this.FindPanel();
			}
			this.Move(NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime));
			if (this.restrictWithinPanel && this.panelRegion != null)
			{
				this.UpdateBounds();
				if (this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, this.dragEffect == UIDragObject.DragEffect.None))
				{
					this.CancelMovement();
				}
				else
				{
					this.CancelSpring();
				}
			}
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
			if (this.mMomentum.magnitude < 0.0001f)
			{
				this.CancelMovement();
			}
		}
		else
		{
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
		}
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x000D3C14 File Offset: 0x000D1E14
	public void CancelMovement()
	{
		if (this.target != null)
		{
			Vector3 localPosition = this.target.localPosition;
			localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
			localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
			localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
			this.target.localPosition = localPosition;
		}
		this.mTargetPos = ((!(this.target != null)) ? Vector3.zero : this.target.position);
		this.mMomentum = Vector3.zero;
		this.mScroll = Vector3.zero;
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x000D3CC8 File Offset: 0x000D1EC8
	public void CancelSpring()
	{
		SpringPosition component = this.target.GetComponent<SpringPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x00011920 File Offset: 0x0000FB20
	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.mScroll -= this.scrollMomentum * (delta * 0.05f);
		}
	}

	// Token: 0x04001F64 RID: 8036
	public Transform target;

	// Token: 0x04001F65 RID: 8037
	public UIPanel panelRegion;

	// Token: 0x04001F66 RID: 8038
	public Vector3 scrollMomentum = Vector3.zero;

	// Token: 0x04001F67 RID: 8039
	public bool restrictWithinPanel;

	// Token: 0x04001F68 RID: 8040
	public UIRect contentRect;

	// Token: 0x04001F69 RID: 8041
	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	// Token: 0x04001F6A RID: 8042
	public float momentumAmount = 35f;

	// Token: 0x04001F6B RID: 8043
	[SerializeField]
	protected Vector3 scale = new Vector3(1f, 1f, 0f);

	// Token: 0x04001F6C RID: 8044
	[SerializeField]
	[HideInInspector]
	private float scrollWheelFactor;

	// Token: 0x04001F6D RID: 8045
	private Plane mPlane;

	// Token: 0x04001F6E RID: 8046
	private Vector3 mTargetPos;

	// Token: 0x04001F6F RID: 8047
	private Vector3 mLastPos;

	// Token: 0x04001F70 RID: 8048
	private Vector3 mMomentum = Vector3.zero;

	// Token: 0x04001F71 RID: 8049
	private Vector3 mScroll = Vector3.zero;

	// Token: 0x04001F72 RID: 8050
	private Bounds mBounds;

	// Token: 0x04001F73 RID: 8051
	private int mTouchID;

	// Token: 0x04001F74 RID: 8052
	private bool mStarted;

	// Token: 0x04001F75 RID: 8053
	private bool mPressed;

	// Token: 0x02000458 RID: 1112
	public enum DragEffect
	{
		// Token: 0x04001F77 RID: 8055
		None,
		// Token: 0x04001F78 RID: 8056
		Momentum,
		// Token: 0x04001F79 RID: 8057
		MomentumAndSpring
	}
}
