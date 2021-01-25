using System;
using UnityEngine;

// Token: 0x0200041C RID: 1052
[AddComponentMenu("NGUI/Examples/UI Cursor")]
[RequireComponent(typeof(UISprite))]
public class UICursor : MonoBehaviour
{
	// Token: 0x0600199F RID: 6559 RVA: 0x00010A4D File Offset: 0x0000EC4D
	private void Awake()
	{
		UICursor.instance = this;
	}

	// Token: 0x060019A0 RID: 6560 RVA: 0x00010A55 File Offset: 0x0000EC55
	private void OnDestroy()
	{
		UICursor.instance = null;
	}

	// Token: 0x060019A1 RID: 6561 RVA: 0x000CEC44 File Offset: 0x000CCE44
	private void Start()
	{
		this.mTrans = base.transform;
		this.mSprite = base.GetComponentInChildren<UISprite>();
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		if (this.mSprite != null)
		{
			this.mAtlas = this.mSprite.atlas;
			this.mSpriteName = this.mSprite.spriteName;
			if (this.mSprite.depth < 100)
			{
				this.mSprite.depth = 100;
			}
		}
	}

	// Token: 0x060019A2 RID: 6562 RVA: 0x000CECE4 File Offset: 0x000CCEE4
	private void Update()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (this.uiCamera != null)
		{
			mousePosition.x = Mathf.Clamp01(mousePosition.x / (float)Screen.width);
			mousePosition.y = Mathf.Clamp01(mousePosition.y / (float)Screen.height);
			this.mTrans.position = this.uiCamera.ViewportToWorldPoint(mousePosition);
			if (this.uiCamera.isOrthoGraphic)
			{
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.x = Mathf.Round(localPosition.x);
				localPosition.y = Mathf.Round(localPosition.y);
				this.mTrans.localPosition = localPosition;
			}
		}
		else
		{
			mousePosition.x -= (float)Screen.width * 0.5f;
			mousePosition.y -= (float)Screen.height * 0.5f;
			mousePosition.x = Mathf.Round(mousePosition.x);
			mousePosition.y = Mathf.Round(mousePosition.y);
			this.mTrans.localPosition = mousePosition;
		}
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x00010A5D File Offset: 0x0000EC5D
	public static void Clear()
	{
		if (UICursor.instance != null && UICursor.instance.mSprite != null)
		{
			UICursor.Set(UICursor.instance.mAtlas, UICursor.instance.mSpriteName);
		}
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x000CEE0C File Offset: 0x000CD00C
	public static void Set2(UIAtlas atlas, string sprite)
	{
		if (UICursor.instance != null && UICursor.instance.mSprite)
		{
			UICursor.instance.mSprite.atlas = atlas;
			UICursor.instance.mSprite.spriteName = sprite;
			UICursor.instance.Update();
		}
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x000CEE68 File Offset: 0x000CD068
	public static void Set(UIAtlas atlas, string sprite)
	{
		if (UICursor.instance != null && UICursor.instance.mSprite)
		{
			UICursor.instance.mSprite.atlas = atlas;
			UICursor.instance.mSprite.spriteName = sprite;
			UICursor.instance.mSprite.MakePixelPerfect();
			UICursor.instance.Update();
		}
	}

	// Token: 0x04001E47 RID: 7751
	public static UICursor instance;

	// Token: 0x04001E48 RID: 7752
	public Camera uiCamera;

	// Token: 0x04001E49 RID: 7753
	private Transform mTrans;

	// Token: 0x04001E4A RID: 7754
	private UISprite mSprite;

	// Token: 0x04001E4B RID: 7755
	private UIAtlas mAtlas;

	// Token: 0x04001E4C RID: 7756
	private string mSpriteName;
}
