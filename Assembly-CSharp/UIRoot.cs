using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000504 RID: 1284
[AddComponentMenu("NGUI/UI/Root")]
[ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
	// Token: 0x1700033C RID: 828
	// (get) Token: 0x060020F6 RID: 8438 RVA: 0x000160CB File Offset: 0x000142CB
	public UIRoot.Constraint constraint
	{
		get
		{
			if (this.fitWidth)
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.Fit;
				}
				return UIRoot.Constraint.FitWidth;
			}
			else
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.FitHeight;
				}
				return UIRoot.Constraint.Fill;
			}
		}
	}

	// Token: 0x1700033D RID: 829
	// (get) Token: 0x060020F7 RID: 8439 RVA: 0x000F8FEC File Offset: 0x000F71EC
	public UIRoot.Scaling activeScaling
	{
		get
		{
			UIRoot.Scaling scaling = this.scalingStyle;
			if (scaling == UIRoot.Scaling.ConstrainedOnMobiles)
			{
				return UIRoot.Scaling.Flexible;
			}
			return scaling;
		}
	}

	// Token: 0x1700033E RID: 830
	// (get) Token: 0x060020F8 RID: 8440 RVA: 0x000F900C File Offset: 0x000F720C
	public int activeHeight
	{
		get
		{
			if (this.activeScaling == UIRoot.Scaling.Flexible)
			{
				Vector2 screenSize = NGUITools.screenSize;
				float num = screenSize.x / screenSize.y;
				if (screenSize.y < (float)this.minimumHeight)
				{
					screenSize.y = (float)this.minimumHeight;
					screenSize.x = screenSize.y * num;
				}
				else if (screenSize.y > (float)this.maximumHeight)
				{
					screenSize.y = (float)this.maximumHeight;
					screenSize.x = screenSize.y * num;
				}
				int num2 = Mathf.RoundToInt((!this.shrinkPortraitUI || screenSize.y <= screenSize.x) ? screenSize.y : (screenSize.y / num));
				return (!this.adjustByDPI) ? num2 : NGUIMath.AdjustByDPI((float)num2);
			}
			UIRoot.Constraint constraint = this.constraint;
			if (constraint == UIRoot.Constraint.FitHeight)
			{
				return this.manualHeight;
			}
			Vector2 screenSize2 = NGUITools.screenSize;
			float num3 = screenSize2.x / screenSize2.y;
			float num4 = (float)this.manualWidth / (float)this.manualHeight;
			switch (constraint)
			{
			case UIRoot.Constraint.Fit:
				return (num4 <= num3) ? this.manualHeight : Mathf.RoundToInt((float)this.manualWidth / num3);
			case UIRoot.Constraint.Fill:
				return (num4 >= num3) ? this.manualHeight : Mathf.RoundToInt((float)this.manualWidth / num3);
			case UIRoot.Constraint.FitWidth:
				return Mathf.RoundToInt((float)this.manualWidth / num3);
			default:
				return this.manualHeight;
			}
		}
	}

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x060020F9 RID: 8441 RVA: 0x000F91B0 File Offset: 0x000F73B0
	public float pixelSizeAdjustment
	{
		get
		{
			int num = Mathf.RoundToInt(NGUITools.screenSize.y);
			return (num != -1) ? this.GetPixelSizeAdjustment(num) : 1f;
		}
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x000F91E8 File Offset: 0x000F73E8
	public static float GetPixelSizeAdjustment(GameObject go)
	{
		UIRoot uiroot = NGUITools.FindInParents<UIRoot>(go);
		return (!(uiroot != null)) ? 1f : uiroot.pixelSizeAdjustment;
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x000F9218 File Offset: 0x000F7418
	public float GetPixelSizeAdjustment(int height)
	{
		height = Mathf.Max(2, height);
		if (this.activeScaling == UIRoot.Scaling.Constrained)
		{
			return (float)this.activeHeight / (float)height;
		}
		if (height < this.minimumHeight)
		{
			return (float)this.minimumHeight / (float)height;
		}
		if (height > this.maximumHeight)
		{
			return (float)this.maximumHeight / (float)height;
		}
		return 1f;
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x000160F5 File Offset: 0x000142F5
	protected virtual void Awake()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x060020FD RID: 8445 RVA: 0x00016103 File Offset: 0x00014303
	protected virtual void OnEnable()
	{
		UIRoot.list.Add(this);
	}

	// Token: 0x060020FE RID: 8446 RVA: 0x00016110 File Offset: 0x00014310
	protected virtual void OnDisable()
	{
		UIRoot.list.Remove(this);
	}

	// Token: 0x060020FF RID: 8447 RVA: 0x000F9278 File Offset: 0x000F7478
	protected virtual void Start()
	{
		UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
		if (componentInChildren != null)
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
			Camera component = componentInChildren.gameObject.GetComponent<Camera>();
			componentInChildren.enabled = false;
			if (component != null)
			{
				component.orthographicSize = 1f;
			}
		}
		else
		{
			this.Update();
		}
	}

	// Token: 0x06002100 RID: 8448 RVA: 0x000F92D8 File Offset: 0x000F74D8
	private void Update()
	{
		if (this.mTrans != null)
		{
			float num = (float)this.activeHeight;
			if (num > 0f)
			{
				float num2 = 2f / num;
				Vector3 localScale = this.mTrans.localScale;
				if (Mathf.Abs(localScale.x - num2) > 1E-45f || Mathf.Abs(localScale.y - num2) > 1E-45f || Mathf.Abs(localScale.z - num2) > 1E-45f)
				{
					this.mTrans.localScale = new Vector3(num2, num2, num2);
				}
			}
		}
	}

	// Token: 0x06002101 RID: 8449 RVA: 0x000F9378 File Offset: 0x000F7578
	public static void Broadcast(string funcName)
	{
		int i = 0;
		int count = UIRoot.list.Count;
		while (i < count)
		{
			UIRoot uiroot = UIRoot.list[i];
			if (uiroot != null)
			{
				uiroot.BroadcastMessage(funcName, 1);
			}
			i++;
		}
	}

	// Token: 0x06002102 RID: 8450 RVA: 0x000F93C4 File Offset: 0x000F75C4
	public static void Broadcast(string funcName, object param)
	{
		if (param == null)
		{
			Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
		}
		else
		{
			int i = 0;
			int count = UIRoot.list.Count;
			while (i < count)
			{
				UIRoot uiroot = UIRoot.list[i];
				if (uiroot != null)
				{
					uiroot.BroadcastMessage(funcName, param, 1);
				}
				i++;
			}
		}
	}

	// Token: 0x04002425 RID: 9253
	public static List<UIRoot> list = new List<UIRoot>();

	// Token: 0x04002426 RID: 9254
	public UIRoot.Scaling scalingStyle;

	// Token: 0x04002427 RID: 9255
	public int manualWidth = 1280;

	// Token: 0x04002428 RID: 9256
	public int manualHeight = 720;

	// Token: 0x04002429 RID: 9257
	public int minimumHeight = 320;

	// Token: 0x0400242A RID: 9258
	public int maximumHeight = 1536;

	// Token: 0x0400242B RID: 9259
	public bool fitWidth;

	// Token: 0x0400242C RID: 9260
	public bool fitHeight = true;

	// Token: 0x0400242D RID: 9261
	public bool adjustByDPI;

	// Token: 0x0400242E RID: 9262
	public bool shrinkPortraitUI;

	// Token: 0x0400242F RID: 9263
	private Transform mTrans;

	// Token: 0x02000505 RID: 1285
	public enum Scaling
	{
		// Token: 0x04002431 RID: 9265
		Flexible,
		// Token: 0x04002432 RID: 9266
		Constrained,
		// Token: 0x04002433 RID: 9267
		ConstrainedOnMobiles
	}

	// Token: 0x02000506 RID: 1286
	public enum Constraint
	{
		// Token: 0x04002435 RID: 9269
		Fit,
		// Token: 0x04002436 RID: 9270
		Fill,
		// Token: 0x04002437 RID: 9271
		FitWidth,
		// Token: 0x04002438 RID: 9272
		FitHeight
	}
}
