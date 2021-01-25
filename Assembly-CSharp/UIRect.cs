using System;
using UnityEngine;

// Token: 0x020004BC RID: 1212
public abstract class UIRect : MonoBehaviour
{
	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06001E08 RID: 7688 RVA: 0x00013DEF File Offset: 0x00011FEF
	public GameObject cachedGameObject
	{
		get
		{
			if (this.mGo == null)
			{
				this.mGo = base.gameObject;
			}
			return this.mGo;
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06001E09 RID: 7689 RVA: 0x00013E14 File Offset: 0x00012014
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

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06001E0A RID: 7690 RVA: 0x00013E39 File Offset: 0x00012039
	public Camera anchorCamera
	{
		get
		{
			if (!this.mAnchorsCached)
			{
				this.ResetAnchors();
			}
			return this.mCam;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06001E0B RID: 7691 RVA: 0x000E9A74 File Offset: 0x000E7C74
	public bool isFullyAnchored
	{
		get
		{
			return this.leftAnchor.target && this.rightAnchor.target && this.topAnchor.target && this.bottomAnchor.target;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06001E0C RID: 7692 RVA: 0x00013E52 File Offset: 0x00012052
	public virtual bool isAnchoredHorizontally
	{
		get
		{
			return this.leftAnchor.target || this.rightAnchor.target;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06001E0D RID: 7693 RVA: 0x00013E7C File Offset: 0x0001207C
	public virtual bool isAnchoredVertically
	{
		get
		{
			return this.bottomAnchor.target || this.topAnchor.target;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06001E0E RID: 7694 RVA: 0x00002B59 File Offset: 0x00000D59
	public virtual bool canBeAnchored
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06001E0F RID: 7695 RVA: 0x00013EA6 File Offset: 0x000120A6
	public UIRect parent
	{
		get
		{
			if (!this.mParentFound)
			{
				this.mParentFound = true;
				this.mParent = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
			}
			return this.mParent;
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06001E10 RID: 7696 RVA: 0x000E9AD4 File Offset: 0x000E7CD4
	public UIRoot root
	{
		get
		{
			if (this.parent != null)
			{
				return this.mParent.root;
			}
			if (!this.mRootSet)
			{
				this.mRootSet = true;
				this.mRoot = NGUITools.FindInParents<UIRoot>(this.cachedTransform);
			}
			return this.mRoot;
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06001E11 RID: 7697 RVA: 0x000E9B28 File Offset: 0x000E7D28
	public bool isAnchored
	{
		get
		{
			return (this.leftAnchor.target || this.rightAnchor.target || this.topAnchor.target || this.bottomAnchor.target) && this.canBeAnchored;
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06001E12 RID: 7698
	// (set) Token: 0x06001E13 RID: 7699
	public abstract float alpha { get; set; }

	// Token: 0x06001E14 RID: 7700
	public abstract float CalculateFinalAlpha(int frameID);

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06001E15 RID: 7701
	public abstract Vector3[] localCorners { get; }

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06001E16 RID: 7702
	public abstract Vector3[] worldCorners { get; }

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06001E17 RID: 7703 RVA: 0x000E9B94 File Offset: 0x000E7D94
	protected float cameraRayDistance
	{
		get
		{
			if (this.anchorCamera == null)
			{
				return 0f;
			}
			if (!this.mCam.isOrthoGraphic)
			{
				Transform cachedTransform = this.cachedTransform;
				Transform transform = this.mCam.transform;
				Plane plane;
				plane..ctor(cachedTransform.rotation * Vector3.back, cachedTransform.position);
				Ray ray;
				ray..ctor(transform.position, transform.rotation * Vector3.forward);
				float result;
				if (plane.Raycast(ray, ref result))
				{
					return result;
				}
			}
			return Mathf.Lerp(this.mCam.nearClipPlane, this.mCam.farClipPlane, 0.5f);
		}
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x000E9C48 File Offset: 0x000E7E48
	public virtual void Invalidate(bool includeChildren)
	{
		this.mChanged = true;
		if (includeChildren)
		{
			for (int i = 0; i < this.mChildren.size; i++)
			{
				this.mChildren.buffer[i].Invalidate(true);
			}
		}
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x000E9C94 File Offset: 0x000E7E94
	public virtual Vector3[] GetSides(Transform relativeTo)
	{
		if (this.anchorCamera != null)
		{
			return this.mCam.GetSides(this.cameraRayDistance, relativeTo);
		}
		Vector3 position = this.cachedTransform.position;
		for (int i = 0; i < 4; i++)
		{
			UIRect.mSides[i] = position;
		}
		if (relativeTo != null)
		{
			for (int j = 0; j < 4; j++)
			{
				UIRect.mSides[j] = relativeTo.InverseTransformPoint(UIRect.mSides[j]);
			}
		}
		return UIRect.mSides;
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x000E9D40 File Offset: 0x000E7F40
	protected Vector3 GetLocalPos(UIRect.AnchorPoint ac, Transform trans)
	{
		if (this.anchorCamera == null || ac.targetCam == null)
		{
			return this.cachedTransform.localPosition;
		}
		Vector3 vector = this.mCam.ViewportToWorldPoint(ac.targetCam.WorldToViewportPoint(ac.target.position));
		if (trans != null)
		{
			vector = trans.InverseTransformPoint(vector);
		}
		vector.x = Mathf.Floor(vector.x + 0.5f);
		vector.y = Mathf.Floor(vector.y + 0.5f);
		return vector;
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x00013ED6 File Offset: 0x000120D6
	protected virtual void OnEnable()
	{
		this.mUpdateFrame = -1;
		if (this.updateAnchors == UIRect.AnchorUpdate.OnEnable)
		{
			this.mAnchorsCached = false;
			this.mUpdateAnchors = true;
		}
		if (this.mStarted)
		{
			this.OnInit();
		}
		this.mUpdateFrame = -1;
	}

	// Token: 0x06001E1C RID: 7708 RVA: 0x00013F10 File Offset: 0x00012110
	protected virtual void OnInit()
	{
		this.mChanged = true;
		this.mRootSet = false;
		this.mParentFound = false;
		if (this.parent != null)
		{
			this.mParent.mChildren.Add(this);
		}
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x00013F49 File Offset: 0x00012149
	protected virtual void OnDisable()
	{
		if (this.mParent)
		{
			this.mParent.mChildren.Remove(this);
		}
		this.mParent = null;
		this.mRoot = null;
		this.mRootSet = false;
		this.mParentFound = false;
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x00013F89 File Offset: 0x00012189
	protected void Start()
	{
		this.mStarted = true;
		this.OnInit();
		this.OnStart();
	}

	// Token: 0x06001E1F RID: 7711 RVA: 0x000E9DE4 File Offset: 0x000E7FE4
	public void Update()
	{
		if (!this.mAnchorsCached)
		{
			this.ResetAnchors();
		}
		int frameCount = Time.frameCount;
		if (this.mUpdateFrame != frameCount)
		{
			if (this.updateAnchors == UIRect.AnchorUpdate.OnUpdate || this.mUpdateAnchors)
			{
				this.mUpdateFrame = frameCount;
				this.mUpdateAnchors = false;
				bool flag = false;
				if (this.leftAnchor.target)
				{
					flag = true;
					if (this.leftAnchor.rect != null && this.leftAnchor.rect.mUpdateFrame != frameCount)
					{
						this.leftAnchor.rect.Update();
					}
				}
				if (this.bottomAnchor.target)
				{
					flag = true;
					if (this.bottomAnchor.rect != null && this.bottomAnchor.rect.mUpdateFrame != frameCount)
					{
						this.bottomAnchor.rect.Update();
					}
				}
				if (this.rightAnchor.target)
				{
					flag = true;
					if (this.rightAnchor.rect != null && this.rightAnchor.rect.mUpdateFrame != frameCount)
					{
						this.rightAnchor.rect.Update();
					}
				}
				if (this.topAnchor.target)
				{
					flag = true;
					if (this.topAnchor.rect != null && this.topAnchor.rect.mUpdateFrame != frameCount)
					{
						this.topAnchor.rect.Update();
					}
				}
				if (flag)
				{
					this.OnAnchor();
				}
			}
			this.OnUpdate();
		}
	}

	// Token: 0x06001E20 RID: 7712 RVA: 0x00013F9E File Offset: 0x0001219E
	public void UpdateAnchors()
	{
		if (this.isAnchored && this.updateAnchors != UIRect.AnchorUpdate.OnStart)
		{
			this.OnAnchor();
		}
	}

	// Token: 0x06001E21 RID: 7713
	protected abstract void OnAnchor();

	// Token: 0x06001E22 RID: 7714 RVA: 0x00013FBD File Offset: 0x000121BD
	public void SetAnchor(Transform t)
	{
		this.leftAnchor.target = t;
		this.rightAnchor.target = t;
		this.topAnchor.target = t;
		this.bottomAnchor.target = t;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x06001E23 RID: 7715 RVA: 0x000E9F9C File Offset: 0x000E819C
	public void SetAnchor(GameObject go)
	{
		Transform target = (!(go != null)) ? null : go.transform;
		this.leftAnchor.target = target;
		this.rightAnchor.target = target;
		this.topAnchor.target = target;
		this.bottomAnchor.target = target;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x000EA000 File Offset: 0x000E8200
	public void SetAnchor(GameObject go, int left, int bottom, int right, int top)
	{
		Transform target = (!(go != null)) ? null : go.transform;
		this.leftAnchor.target = target;
		this.rightAnchor.target = target;
		this.topAnchor.target = target;
		this.bottomAnchor.target = target;
		this.leftAnchor.relative = 0f;
		this.rightAnchor.relative = 1f;
		this.bottomAnchor.relative = 0f;
		this.topAnchor.relative = 1f;
		this.leftAnchor.absolute = left;
		this.rightAnchor.absolute = right;
		this.bottomAnchor.absolute = bottom;
		this.topAnchor.absolute = top;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x000EA0D4 File Offset: 0x000E82D4
	public void ResetAnchors()
	{
		this.mAnchorsCached = true;
		this.leftAnchor.rect = ((!this.leftAnchor.target) ? null : this.leftAnchor.target.GetComponent<UIRect>());
		this.bottomAnchor.rect = ((!this.bottomAnchor.target) ? null : this.bottomAnchor.target.GetComponent<UIRect>());
		this.rightAnchor.rect = ((!this.rightAnchor.target) ? null : this.rightAnchor.target.GetComponent<UIRect>());
		this.topAnchor.rect = ((!this.topAnchor.target) ? null : this.topAnchor.target.GetComponent<UIRect>());
		this.mCam = NGUITools.FindCameraForLayer(this.cachedGameObject.layer);
		this.FindCameraFor(this.leftAnchor);
		this.FindCameraFor(this.bottomAnchor);
		this.FindCameraFor(this.rightAnchor);
		this.FindCameraFor(this.topAnchor);
		this.mUpdateAnchors = true;
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x00013FFB File Offset: 0x000121FB
	public void ResetAndUpdateAnchors()
	{
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x06001E27 RID: 7719
	public abstract void SetRect(float x, float y, float width, float height);

	// Token: 0x06001E28 RID: 7720 RVA: 0x000EA210 File Offset: 0x000E8410
	private void FindCameraFor(UIRect.AnchorPoint ap)
	{
		if (ap.target == null || ap.rect != null)
		{
			ap.targetCam = null;
		}
		else
		{
			ap.targetCam = NGUITools.FindCameraForLayer(ap.target.gameObject.layer);
		}
	}

	// Token: 0x06001E29 RID: 7721 RVA: 0x000EA268 File Offset: 0x000E8468
	public virtual void ParentHasChanged()
	{
		this.mParentFound = false;
		UIRect uirect = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
		if (this.mParent != uirect)
		{
			if (this.mParent)
			{
				this.mParent.mChildren.Remove(this);
			}
			this.mParent = uirect;
			if (this.mParent)
			{
				this.mParent.mChildren.Add(this);
			}
			this.mRootSet = false;
		}
	}

	// Token: 0x06001E2A RID: 7722
	protected abstract void OnStart();

	// Token: 0x06001E2B RID: 7723 RVA: 0x0000264F File Offset: 0x0000084F
	protected virtual void OnUpdate()
	{
	}

	// Token: 0x04002214 RID: 8724
	public UIRect.AnchorPoint leftAnchor = new UIRect.AnchorPoint();

	// Token: 0x04002215 RID: 8725
	public UIRect.AnchorPoint rightAnchor = new UIRect.AnchorPoint(1f);

	// Token: 0x04002216 RID: 8726
	public UIRect.AnchorPoint bottomAnchor = new UIRect.AnchorPoint();

	// Token: 0x04002217 RID: 8727
	public UIRect.AnchorPoint topAnchor = new UIRect.AnchorPoint(1f);

	// Token: 0x04002218 RID: 8728
	public UIRect.AnchorUpdate updateAnchors = UIRect.AnchorUpdate.OnUpdate;

	// Token: 0x04002219 RID: 8729
	protected GameObject mGo;

	// Token: 0x0400221A RID: 8730
	protected Transform mTrans;

	// Token: 0x0400221B RID: 8731
	protected BetterList<UIRect> mChildren = new BetterList<UIRect>();

	// Token: 0x0400221C RID: 8732
	protected bool mChanged = true;

	// Token: 0x0400221D RID: 8733
	protected bool mStarted;

	// Token: 0x0400221E RID: 8734
	protected bool mParentFound;

	// Token: 0x0400221F RID: 8735
	[NonSerialized]
	private bool mUpdateAnchors = true;

	// Token: 0x04002220 RID: 8736
	[NonSerialized]
	private int mUpdateFrame = -1;

	// Token: 0x04002221 RID: 8737
	[NonSerialized]
	private bool mAnchorsCached;

	// Token: 0x04002222 RID: 8738
	[NonSerialized]
	public float finalAlpha = 1f;

	// Token: 0x04002223 RID: 8739
	private UIRoot mRoot;

	// Token: 0x04002224 RID: 8740
	private UIRect mParent;

	// Token: 0x04002225 RID: 8741
	private bool mRootSet;

	// Token: 0x04002226 RID: 8742
	protected Camera mCam;

	// Token: 0x04002227 RID: 8743
	protected static Vector3[] mSides = new Vector3[4];

	// Token: 0x020004BD RID: 1213
	[Serializable]
	public class AnchorPoint
	{
		// Token: 0x06001E2C RID: 7724 RVA: 0x00002672 File Offset: 0x00000872
		public AnchorPoint()
		{
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x00014009 File Offset: 0x00012209
		public AnchorPoint(float relative)
		{
			this.relative = relative;
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x00014018 File Offset: 0x00012218
		public void Set(float relative, float absolute)
		{
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x00014033 File Offset: 0x00012233
		public void Set(Transform target, float relative, float absolute)
		{
			this.target = target;
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x00014055 File Offset: 0x00012255
		public void SetToNearest(float abs0, float abs1, float abs2)
		{
			this.SetToNearest(0f, 0.5f, 1f, abs0, abs1, abs2);
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x000EA2F0 File Offset: 0x000E84F0
		public void SetToNearest(float rel0, float rel1, float rel2, float abs0, float abs1, float abs2)
		{
			float num = Mathf.Abs(abs0);
			float num2 = Mathf.Abs(abs1);
			float num3 = Mathf.Abs(abs2);
			if (num < num2 && num < num3)
			{
				this.Set(rel0, abs0);
			}
			else if (num2 < num && num2 < num3)
			{
				this.Set(rel1, abs1);
			}
			else
			{
				this.Set(rel2, abs2);
			}
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x000EA358 File Offset: 0x000E8558
		public void SetHorizontal(Transform parent, float localPos)
		{
			if (this.rect)
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float num = Mathf.Lerp(sides[0].x, sides[2].x, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - num + 0.5f);
			}
			else
			{
				Vector3 vector = this.target.position;
				if (parent != null)
				{
					vector = parent.InverseTransformPoint(vector);
				}
				this.absolute = Mathf.FloorToInt(localPos - vector.x + 0.5f);
			}
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000EA3FC File Offset: 0x000E85FC
		public void SetVertical(Transform parent, float localPos)
		{
			if (this.rect)
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float num = Mathf.Lerp(sides[3].y, sides[1].y, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - num + 0.5f);
			}
			else
			{
				Vector3 vector = this.target.position;
				if (parent != null)
				{
					vector = parent.InverseTransformPoint(vector);
				}
				this.absolute = Mathf.FloorToInt(localPos - vector.y + 0.5f);
			}
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000EA4A0 File Offset: 0x000E86A0
		public Vector3[] GetSides(Transform relativeTo)
		{
			if (this.target != null)
			{
				if (this.rect != null)
				{
					return this.rect.GetSides(relativeTo);
				}
				if (this.target.camera != null)
				{
					return this.target.camera.GetSides(relativeTo);
				}
			}
			return null;
		}

		// Token: 0x04002228 RID: 8744
		public Transform target;

		// Token: 0x04002229 RID: 8745
		public float relative;

		// Token: 0x0400222A RID: 8746
		public int absolute;

		// Token: 0x0400222B RID: 8747
		[NonSerialized]
		public UIRect rect;

		// Token: 0x0400222C RID: 8748
		[NonSerialized]
		public Camera targetCam;
	}

	// Token: 0x020004BE RID: 1214
	public enum AnchorUpdate
	{
		// Token: 0x0400222E RID: 8750
		OnEnable,
		// Token: 0x0400222F RID: 8751
		OnUpdate,
		// Token: 0x04002230 RID: 8752
		OnStart
	}
}
