using System;
using UnityEngine;

// Token: 0x02000510 RID: 1296
[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
	// Token: 0x1700035E RID: 862
	// (get) Token: 0x06002159 RID: 8537 RVA: 0x0001667C File Offset: 0x0001487C
	public static bool isVisible
	{
		get
		{
			return UITooltip.mInstance != null && UITooltip.mInstance.mTarget == 1f;
		}
	}

	// Token: 0x0600215A RID: 8538 RVA: 0x000166A2 File Offset: 0x000148A2
	private void Awake()
	{
		UITooltip.mInstance = this;
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x000166AA File Offset: 0x000148AA
	private void OnDestroy()
	{
		UITooltip.mInstance = null;
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x000FB4B0 File Offset: 0x000F96B0
	protected virtual void Start()
	{
		this.mTrans = base.transform;
		this.mWidgets = base.GetComponentsInChildren<UIWidget>();
		this.mPos = this.mTrans.localPosition;
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.SetAlpha(0f);
	}

	// Token: 0x0600215D RID: 8541 RVA: 0x000FB518 File Offset: 0x000F9718
	protected virtual void Update()
	{
		if (this.mHover != UICamera.hoveredObject)
		{
			this.mHover = null;
			this.mTarget = 0f;
		}
		if (this.mCurrent != this.mTarget)
		{
			this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, RealTime.deltaTime * this.appearSpeed);
			if (Mathf.Abs(this.mCurrent - this.mTarget) < 0.001f)
			{
				this.mCurrent = this.mTarget;
			}
			this.SetAlpha(this.mCurrent * this.mCurrent);
			if (this.scalingTransitions)
			{
				Vector3 vector = this.mSize * 0.25f;
				vector.y = -vector.y;
				Vector3 localScale = Vector3.one * (1.5f - this.mCurrent * 0.5f);
				Vector3 localPosition = Vector3.Lerp(this.mPos - vector, this.mPos, this.mCurrent);
				this.mTrans.localPosition = localPosition;
				this.mTrans.localScale = localScale;
			}
		}
	}

	// Token: 0x0600215E RID: 8542 RVA: 0x000FB63C File Offset: 0x000F983C
	protected virtual void SetAlpha(float val)
	{
		int i = 0;
		int num = this.mWidgets.Length;
		while (i < num)
		{
			UIWidget uiwidget = this.mWidgets[i];
			Color color = uiwidget.color;
			color.a = val;
			uiwidget.color = color;
			i++;
		}
	}

	// Token: 0x0600215F RID: 8543 RVA: 0x000FB684 File Offset: 0x000F9884
	protected virtual void SetText(string tooltipText)
	{
		if (this.text != null && !string.IsNullOrEmpty(tooltipText))
		{
			this.mTarget = 1f;
			this.mHover = UICamera.hoveredObject;
			this.text.text = tooltipText;
			this.mPos = Input.mousePosition;
			Transform transform = this.text.transform;
			Vector3 localPosition = transform.localPosition;
			Vector3 localScale = transform.localScale;
			this.mSize = this.text.printedSize;
			this.mSize.x = this.mSize.x * localScale.x;
			this.mSize.y = this.mSize.y * localScale.y;
			if (this.background != null)
			{
				Vector4 border = this.background.border;
				this.mSize.x = this.mSize.x + (border.x + border.z + (localPosition.x - border.x) * 2f);
				this.mSize.y = this.mSize.y + (border.y + border.w + (-localPosition.y - border.y) * 2f);
				this.background.width = Mathf.RoundToInt(this.mSize.x);
				this.background.height = Mathf.RoundToInt(this.mSize.y);
			}
			if (this.uiCamera != null)
			{
				this.mPos.x = Mathf.Clamp01(this.mPos.x / (float)Screen.width);
				this.mPos.y = Mathf.Clamp01(this.mPos.y / (float)Screen.height);
				float num = this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y;
				float num2 = (float)Screen.height * 0.5f / num;
				Vector2 vector;
				vector..ctor(num2 * this.mSize.x / (float)Screen.width, num2 * this.mSize.y / (float)Screen.height);
				this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector.x);
				this.mPos.y = Mathf.Max(this.mPos.y, vector.y);
				this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
				this.mPos = this.mTrans.localPosition;
				this.mPos.x = Mathf.Round(this.mPos.x);
				this.mPos.y = Mathf.Round(this.mPos.y);
				this.mTrans.localPosition = this.mPos;
			}
			else
			{
				if (this.mPos.x + this.mSize.x > (float)Screen.width)
				{
					this.mPos.x = (float)Screen.width - this.mSize.x;
				}
				if (this.mPos.y - this.mSize.y < 0f)
				{
					this.mPos.y = this.mSize.y;
				}
				this.mPos.x = this.mPos.x - (float)Screen.width * 0.5f;
				this.mPos.y = this.mPos.y - (float)Screen.height * 0.5f;
			}
		}
		else
		{
			this.mHover = null;
			this.mTarget = 0f;
		}
	}

	// Token: 0x06002160 RID: 8544 RVA: 0x000166B2 File Offset: 0x000148B2
	[Obsolete("Use UITooltip.Show instead")]
	public static void ShowText(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	// Token: 0x06002161 RID: 8545 RVA: 0x000166B2 File Offset: 0x000148B2
	public static void Show(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x000166CF File Offset: 0x000148CF
	public static void Hide()
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.mHover = null;
			UITooltip.mInstance.mTarget = 0f;
		}
	}

	// Token: 0x04002482 RID: 9346
	protected static UITooltip mInstance;

	// Token: 0x04002483 RID: 9347
	public Camera uiCamera;

	// Token: 0x04002484 RID: 9348
	public UILabel text;

	// Token: 0x04002485 RID: 9349
	public UISprite background;

	// Token: 0x04002486 RID: 9350
	public float appearSpeed = 10f;

	// Token: 0x04002487 RID: 9351
	public bool scalingTransitions = true;

	// Token: 0x04002488 RID: 9352
	protected GameObject mHover;

	// Token: 0x04002489 RID: 9353
	protected Transform mTrans;

	// Token: 0x0400248A RID: 9354
	protected float mTarget;

	// Token: 0x0400248B RID: 9355
	protected float mCurrent;

	// Token: 0x0400248C RID: 9356
	protected Vector3 mPos;

	// Token: 0x0400248D RID: 9357
	protected Vector3 mSize = Vector3.zero;

	// Token: 0x0400248E RID: 9358
	protected UIWidget[] mWidgets;
}
