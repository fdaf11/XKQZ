using System;
using UnityEngine;

// Token: 0x020004DB RID: 1243
[AddComponentMenu("NGUI/UI/Anchor")]
[ExecuteInEditMode]
public class UIAnchor : MonoBehaviour
{
	// Token: 0x06001F4C RID: 8012 RVA: 0x00014E23 File Offset: 0x00013023
	private void Awake()
	{
		this.mTrans = base.transform;
		this.mAnim = base.animation;
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	// Token: 0x06001F4D RID: 8013 RVA: 0x00014E5D File Offset: 0x0001305D
	private void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	// Token: 0x06001F4E RID: 8014 RVA: 0x00014E7F File Offset: 0x0001307F
	private void ScreenSizeChanged()
	{
		if (this.mStarted && this.runOnlyOnce)
		{
			this.Update();
		}
	}

	// Token: 0x06001F4F RID: 8015 RVA: 0x000EE3D0 File Offset: 0x000EC5D0
	private void Start()
	{
		if (this.container == null && this.widgetContainer != null)
		{
			this.container = this.widgetContainer.gameObject;
			this.widgetContainer = null;
		}
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.Update();
		this.mStarted = true;
	}

	// Token: 0x06001F50 RID: 8016 RVA: 0x000EE45C File Offset: 0x000EC65C
	private void Update()
	{
		if (this.mAnim != null && this.mAnim.enabled && this.mAnim.isPlaying)
		{
			return;
		}
		bool flag = false;
		UIWidget uiwidget = (!(this.container == null)) ? this.container.GetComponent<UIWidget>() : null;
		UIPanel uipanel = (!(this.container == null) || !(uiwidget == null)) ? this.container.GetComponent<UIPanel>() : null;
		if (uiwidget != null)
		{
			Bounds bounds = uiwidget.CalculateBounds(this.container.transform.parent);
			this.mRect.x = bounds.min.x;
			this.mRect.y = bounds.min.y;
			this.mRect.width = bounds.size.x;
			this.mRect.height = bounds.size.y;
		}
		else if (uipanel != null)
		{
			if (uipanel.clipping == UIDrawCall.Clipping.None)
			{
				float num = (!(this.mRoot != null)) ? 0.5f : ((float)this.mRoot.activeHeight / (float)Screen.height * 0.5f);
				this.mRect.xMin = (float)(-(float)Screen.width) * num;
				this.mRect.yMin = (float)(-(float)Screen.height) * num;
				this.mRect.xMax = -this.mRect.xMin;
				this.mRect.yMax = -this.mRect.yMin;
			}
			else
			{
				Vector4 finalClipRegion = uipanel.finalClipRegion;
				this.mRect.x = finalClipRegion.x - finalClipRegion.z * 0.5f;
				this.mRect.y = finalClipRegion.y - finalClipRegion.w * 0.5f;
				this.mRect.width = finalClipRegion.z;
				this.mRect.height = finalClipRegion.w;
			}
		}
		else if (this.container != null)
		{
			Transform parent = this.container.transform.parent;
			Bounds bounds2 = (!(parent != null)) ? NGUIMath.CalculateRelativeWidgetBounds(this.container.transform) : NGUIMath.CalculateRelativeWidgetBounds(parent, this.container.transform);
			this.mRect.x = bounds2.min.x;
			this.mRect.y = bounds2.min.y;
			this.mRect.width = bounds2.size.x;
			this.mRect.height = bounds2.size.y;
		}
		else
		{
			if (!(this.uiCamera != null))
			{
				return;
			}
			flag = true;
			this.mRect = this.uiCamera.pixelRect;
		}
		float num2 = (this.mRect.xMin + this.mRect.xMax) * 0.5f;
		float num3 = (this.mRect.yMin + this.mRect.yMax) * 0.5f;
		Vector3 vector;
		vector..ctor(num2, num3, 0f);
		if (this.side != UIAnchor.Side.Center)
		{
			if (this.side == UIAnchor.Side.Right || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.BottomRight)
			{
				vector.x = this.mRect.xMax;
			}
			else if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Bottom)
			{
				vector.x = num2;
			}
			else
			{
				vector.x = this.mRect.xMin;
			}
			if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.TopLeft)
			{
				vector.y = this.mRect.yMax;
			}
			else if (this.side == UIAnchor.Side.Left || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Right)
			{
				vector.y = num3;
			}
			else
			{
				vector.y = this.mRect.yMin;
			}
		}
		float width = this.mRect.width;
		float height = this.mRect.height;
		vector.x += this.pixelOffset.x + this.relativeOffset.x * width;
		vector.y += this.pixelOffset.y + this.relativeOffset.y * height;
		if (flag)
		{
			if (this.uiCamera.orthographic)
			{
				vector.x = Mathf.Round(vector.x);
				vector.y = Mathf.Round(vector.y);
			}
			vector.z = this.uiCamera.WorldToScreenPoint(this.mTrans.position).z;
			vector = this.uiCamera.ScreenToWorldPoint(vector);
		}
		else
		{
			vector.x = Mathf.Round(vector.x);
			vector.y = Mathf.Round(vector.y);
			if (uipanel != null)
			{
				vector = uipanel.cachedTransform.TransformPoint(vector);
			}
			else if (this.container != null)
			{
				Transform parent2 = this.container.transform.parent;
				if (parent2 != null)
				{
					vector = parent2.TransformPoint(vector);
				}
			}
			vector.z = this.mTrans.position.z;
		}
		if (this.mTrans.position != vector)
		{
			this.mTrans.position = vector;
		}
		if (this.runOnlyOnce && Application.isPlaying)
		{
			base.enabled = false;
		}
	}

	// Token: 0x040022D8 RID: 8920
	public Camera uiCamera;

	// Token: 0x040022D9 RID: 8921
	public GameObject container;

	// Token: 0x040022DA RID: 8922
	public UIAnchor.Side side = UIAnchor.Side.Center;

	// Token: 0x040022DB RID: 8923
	public bool runOnlyOnce = true;

	// Token: 0x040022DC RID: 8924
	public Vector2 relativeOffset = Vector2.zero;

	// Token: 0x040022DD RID: 8925
	public Vector2 pixelOffset = Vector2.zero;

	// Token: 0x040022DE RID: 8926
	[HideInInspector]
	[SerializeField]
	private UIWidget widgetContainer;

	// Token: 0x040022DF RID: 8927
	private Transform mTrans;

	// Token: 0x040022E0 RID: 8928
	private Animation mAnim;

	// Token: 0x040022E1 RID: 8929
	private Rect mRect = default(Rect);

	// Token: 0x040022E2 RID: 8930
	private UIRoot mRoot;

	// Token: 0x040022E3 RID: 8931
	private bool mStarted;

	// Token: 0x020004DC RID: 1244
	public enum Side
	{
		// Token: 0x040022E5 RID: 8933
		BottomLeft,
		// Token: 0x040022E6 RID: 8934
		Left,
		// Token: 0x040022E7 RID: 8935
		TopLeft,
		// Token: 0x040022E8 RID: 8936
		Top,
		// Token: 0x040022E9 RID: 8937
		TopRight,
		// Token: 0x040022EA RID: 8938
		Right,
		// Token: 0x040022EB RID: 8939
		BottomRight,
		// Token: 0x040022EC RID: 8940
		Bottom,
		// Token: 0x040022ED RID: 8941
		Center
	}
}
