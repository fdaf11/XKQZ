using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000475 RID: 1141
[AddComponentMenu("NGUI/Interaction/NGUI Progress Bar")]
[ExecuteInEditMode]
public class UIProgressBar : UIWidgetContainer
{
	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06001B88 RID: 7048 RVA: 0x00012615 File Offset: 0x00010815
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x06001B89 RID: 7049 RVA: 0x0001263A File Offset: 0x0001083A
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = NGUITools.FindCameraForLayer(base.gameObject.layer);
			}
			return this.mCam;
		}
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06001B8A RID: 7050 RVA: 0x00012669 File Offset: 0x00010869
	// (set) Token: 0x06001B8B RID: 7051 RVA: 0x00012671 File Offset: 0x00010871
	public UIWidget foregroundWidget
	{
		get
		{
			return this.mFG;
		}
		set
		{
			if (this.mFG != value)
			{
				this.mFG = value;
				this.mIsDirty = true;
			}
		}
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06001B8C RID: 7052 RVA: 0x00012692 File Offset: 0x00010892
	// (set) Token: 0x06001B8D RID: 7053 RVA: 0x0001269A File Offset: 0x0001089A
	public UIWidget backgroundWidget
	{
		get
		{
			return this.mBG;
		}
		set
		{
			if (this.mBG != value)
			{
				this.mBG = value;
				this.mIsDirty = true;
			}
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06001B8E RID: 7054 RVA: 0x000126BB File Offset: 0x000108BB
	// (set) Token: 0x06001B8F RID: 7055 RVA: 0x000126C3 File Offset: 0x000108C3
	public UIProgressBar.FillDirection fillDirection
	{
		get
		{
			return this.mFill;
		}
		set
		{
			if (this.mFill != value)
			{
				this.mFill = value;
				this.ForceUpdate();
			}
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06001B90 RID: 7056 RVA: 0x000126DE File Offset: 0x000108DE
	// (set) Token: 0x06001B91 RID: 7057 RVA: 0x000D77D4 File Offset: 0x000D59D4
	public float value
	{
		get
		{
			if (this.numberOfSteps > 1)
			{
				return Mathf.Round(this.mValue * (float)(this.numberOfSteps - 1)) / (float)(this.numberOfSteps - 1);
			}
			return this.mValue;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mValue != num)
			{
				float value2 = this.value;
				this.mValue = num;
				if (value2 != this.value)
				{
					this.ForceUpdate();
					if (UIProgressBar.current == null && NGUITools.GetActive(this) && EventDelegate.IsValid(this.onChange))
					{
						UIProgressBar.current = this;
						EventDelegate.Execute(this.onChange);
						UIProgressBar.current = null;
					}
				}
			}
		}
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06001B92 RID: 7058 RVA: 0x000D7858 File Offset: 0x000D5A58
	// (set) Token: 0x06001B93 RID: 7059 RVA: 0x000D78A4 File Offset: 0x000D5AA4
	public float alpha
	{
		get
		{
			if (this.mFG != null)
			{
				return this.mFG.alpha;
			}
			if (this.mBG != null)
			{
				return this.mBG.alpha;
			}
			return 1f;
		}
		set
		{
			if (this.mFG != null)
			{
				this.mFG.alpha = value;
				if (this.mFG.collider != null)
				{
					this.mFG.collider.enabled = (this.mFG.alpha > 0.001f);
				}
				else if (this.mFG.GetComponent<Collider2D>() != null)
				{
					this.mFG.GetComponent<Collider2D>().enabled = (this.mFG.alpha > 0.001f);
				}
			}
			if (this.mBG != null)
			{
				this.mBG.alpha = value;
				if (this.mBG.collider != null)
				{
					this.mBG.collider.enabled = (this.mBG.alpha > 0.001f);
				}
				else if (this.mBG.GetComponent<Collider2D>() != null)
				{
					this.mBG.GetComponent<Collider2D>().enabled = (this.mBG.alpha > 0.001f);
				}
			}
			if (this.thumb != null)
			{
				UIWidget component = this.thumb.GetComponent<UIWidget>();
				if (component != null)
				{
					component.alpha = value;
					if (component.collider != null)
					{
						component.collider.enabled = (component.alpha > 0.001f);
					}
					else if (component.GetComponent<Collider2D>() != null)
					{
						component.GetComponent<Collider2D>().enabled = (component.alpha > 0.001f);
					}
				}
			}
		}
	}

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06001B94 RID: 7060 RVA: 0x00012712 File Offset: 0x00010912
	protected bool isHorizontal
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.LeftToRight || this.mFill == UIProgressBar.FillDirection.RightToLeft;
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06001B95 RID: 7061 RVA: 0x0001272B File Offset: 0x0001092B
	protected bool isInverted
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.RightToLeft || this.mFill == UIProgressBar.FillDirection.TopToBottom;
		}
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x000D7A5C File Offset: 0x000D5C5C
	protected void Start()
	{
		this.Upgrade();
		if (Application.isPlaying)
		{
			if (this.mBG != null)
			{
				this.mBG.autoResizeBoxCollider = true;
			}
			this.OnStart();
			if (UIProgressBar.current == null && this.onChange != null)
			{
				UIProgressBar.current = this;
				EventDelegate.Execute(this.onChange);
				UIProgressBar.current = null;
			}
		}
		this.ForceUpdate();
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x0000264F File Offset: 0x0000084F
	protected virtual void Upgrade()
	{
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x0000264F File Offset: 0x0000084F
	protected virtual void OnStart()
	{
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x00012745 File Offset: 0x00010945
	protected void Update()
	{
		if (this.mIsDirty)
		{
			this.ForceUpdate();
		}
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x000D7AD4 File Offset: 0x000D5CD4
	protected void OnValidate()
	{
		if (NGUITools.GetActive(this))
		{
			this.Upgrade();
			this.mIsDirty = true;
			float num = Mathf.Clamp01(this.mValue);
			if (this.mValue != num)
			{
				this.mValue = num;
			}
			if (this.numberOfSteps < 0)
			{
				this.numberOfSteps = 0;
			}
			else if (this.numberOfSteps > 20)
			{
				this.numberOfSteps = 20;
			}
			this.ForceUpdate();
		}
		else
		{
			float num2 = Mathf.Clamp01(this.mValue);
			if (this.mValue != num2)
			{
				this.mValue = num2;
			}
			if (this.numberOfSteps < 0)
			{
				this.numberOfSteps = 0;
			}
			else if (this.numberOfSteps > 20)
			{
				this.numberOfSteps = 20;
			}
		}
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x000D7B9C File Offset: 0x000D5D9C
	protected float ScreenToValue(Vector2 screenPos)
	{
		Transform cachedTransform = this.cachedTransform;
		Plane plane;
		plane..ctor(cachedTransform.rotation * Vector3.back, cachedTransform.position);
		Ray ray = this.cachedCamera.ScreenPointToRay(screenPos);
		float num;
		if (!plane.Raycast(ray, ref num))
		{
			return this.value;
		}
		return this.LocalToValue(cachedTransform.InverseTransformPoint(ray.GetPoint(num)));
	}

	// Token: 0x06001B9C RID: 7068 RVA: 0x000D7C10 File Offset: 0x000D5E10
	protected virtual float LocalToValue(Vector2 localPos)
	{
		if (!(this.mFG != null))
		{
			return this.value;
		}
		Vector3[] localCorners = this.mFG.localCorners;
		Vector3 vector = localCorners[2] - localCorners[0];
		if (this.isHorizontal)
		{
			float num = (localPos.x - localCorners[0].x) / vector.x;
			return (!this.isInverted) ? num : (1f - num);
		}
		float num2 = (localPos.y - localCorners[0].y) / vector.y;
		return (!this.isInverted) ? num2 : (1f - num2);
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x000D7CD8 File Offset: 0x000D5ED8
	public virtual void ForceUpdate()
	{
		this.mIsDirty = false;
		if (this.mFG != null)
		{
			UIBasicSprite uibasicSprite = this.mFG as UIBasicSprite;
			if (this.isHorizontal)
			{
				if (uibasicSprite != null && uibasicSprite.type == UIBasicSprite.Type.Filled)
				{
					if (uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
					{
						uibasicSprite.fillDirection = UIBasicSprite.FillDirection.Horizontal;
						uibasicSprite.invert = this.isInverted;
					}
					uibasicSprite.fillAmount = this.value;
				}
				else
				{
					this.mFG.drawRegion = ((!this.isInverted) ? new Vector4(0f, 0f, this.value, 1f) : new Vector4(1f - this.value, 0f, 1f, 1f));
					this.mFG.enabled = (this.value > 0.001f);
				}
			}
			else if (uibasicSprite != null && uibasicSprite.type == UIBasicSprite.Type.Filled)
			{
				if (uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uibasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
				{
					uibasicSprite.fillDirection = UIBasicSprite.FillDirection.Vertical;
					uibasicSprite.invert = this.isInverted;
				}
				uibasicSprite.fillAmount = this.value;
			}
			else
			{
				this.mFG.drawRegion = ((!this.isInverted) ? new Vector4(0f, 0f, 1f, this.value) : new Vector4(0f, 1f - this.value, 1f, 1f));
				this.mFG.enabled = (this.value > 0.001f);
			}
		}
		if (this.thumb != null && (this.mFG != null || this.mBG != null))
		{
			Vector3[] array = (!(this.mFG != null)) ? this.mBG.localCorners : this.mFG.localCorners;
			Vector4 vector = (!(this.mFG != null)) ? this.mBG.border : this.mFG.border;
			Vector3[] array2 = array;
			int num = 0;
			array2[num].x = array2[num].x + vector.x;
			Vector3[] array3 = array;
			int num2 = 1;
			array3[num2].x = array3[num2].x + vector.x;
			Vector3[] array4 = array;
			int num3 = 2;
			array4[num3].x = array4[num3].x - vector.z;
			Vector3[] array5 = array;
			int num4 = 3;
			array5[num4].x = array5[num4].x - vector.z;
			Vector3[] array6 = array;
			int num5 = 0;
			array6[num5].y = array6[num5].y + vector.y;
			Vector3[] array7 = array;
			int num6 = 1;
			array7[num6].y = array7[num6].y - vector.w;
			Vector3[] array8 = array;
			int num7 = 2;
			array8[num7].y = array8[num7].y - vector.w;
			Vector3[] array9 = array;
			int num8 = 3;
			array9[num8].y = array9[num8].y + vector.y;
			Transform transform = (!(this.mFG != null)) ? this.mBG.cachedTransform : this.mFG.cachedTransform;
			for (int i = 0; i < 4; i++)
			{
				array[i] = transform.TransformPoint(array[i]);
			}
			if (this.isHorizontal)
			{
				Vector3 vector2 = Vector3.Lerp(array[0], array[1], 0.5f);
				Vector3 vector3 = Vector3.Lerp(array[2], array[3], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(vector2, vector3, (!this.isInverted) ? this.value : (1f - this.value)));
			}
			else
			{
				Vector3 vector4 = Vector3.Lerp(array[0], array[3], 0.5f);
				Vector3 vector5 = Vector3.Lerp(array[1], array[2], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(vector4, vector5, (!this.isInverted) ? this.value : (1f - this.value)));
			}
		}
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x000D815C File Offset: 0x000D635C
	protected void SetThumbPosition(Vector3 worldPos)
	{
		Transform parent = this.thumb.parent;
		if (parent != null)
		{
			worldPos = parent.InverseTransformPoint(worldPos);
			worldPos.x = Mathf.Round(worldPos.x);
			worldPos.y = Mathf.Round(worldPos.y);
			worldPos.z = 0f;
			if (Vector3.Distance(this.thumb.localPosition, worldPos) > 0.001f)
			{
				this.thumb.localPosition = worldPos;
			}
		}
		else if (Vector3.Distance(this.thumb.position, worldPos) > 1E-05f)
		{
			this.thumb.position = worldPos;
		}
	}

	// Token: 0x04002067 RID: 8295
	public static UIProgressBar current;

	// Token: 0x04002068 RID: 8296
	public UIProgressBar.OnDragFinished onDragFinished;

	// Token: 0x04002069 RID: 8297
	public Transform thumb;

	// Token: 0x0400206A RID: 8298
	[HideInInspector]
	[SerializeField]
	protected UIWidget mBG;

	// Token: 0x0400206B RID: 8299
	[SerializeField]
	[HideInInspector]
	protected UIWidget mFG;

	// Token: 0x0400206C RID: 8300
	[SerializeField]
	[HideInInspector]
	protected float mValue = 1f;

	// Token: 0x0400206D RID: 8301
	[HideInInspector]
	[SerializeField]
	protected UIProgressBar.FillDirection mFill;

	// Token: 0x0400206E RID: 8302
	protected Transform mTrans;

	// Token: 0x0400206F RID: 8303
	protected bool mIsDirty;

	// Token: 0x04002070 RID: 8304
	protected Camera mCam;

	// Token: 0x04002071 RID: 8305
	protected float mOffset;

	// Token: 0x04002072 RID: 8306
	public int numberOfSteps;

	// Token: 0x04002073 RID: 8307
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x02000476 RID: 1142
	public enum FillDirection
	{
		// Token: 0x04002075 RID: 8309
		LeftToRight,
		// Token: 0x04002076 RID: 8310
		RightToLeft,
		// Token: 0x04002077 RID: 8311
		BottomToTop,
		// Token: 0x04002078 RID: 8312
		TopToBottom
	}

	// Token: 0x02000477 RID: 1143
	// (Invoke) Token: 0x06001BA0 RID: 7072
	public delegate void OnDragFinished();
}
