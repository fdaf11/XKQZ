using System;
using UnityEngine;

// Token: 0x020004FE RID: 1278
[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/UI/Localize")]
public class UILocalize : MonoBehaviour
{
	// Token: 0x17000323 RID: 803
	// (set) Token: 0x06002099 RID: 8345 RVA: 0x000F65FC File Offset: 0x000F47FC
	public string value
	{
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				UIWidget component = base.GetComponent<UIWidget>();
				UILabel uilabel = component as UILabel;
				UISprite uisprite = component as UISprite;
				if (uilabel != null)
				{
					UIInput uiinput = NGUITools.FindInParents<UIInput>(uilabel.gameObject);
					if (uiinput != null && uiinput.label == uilabel)
					{
						uiinput.defaultText = value;
					}
					else
					{
						uilabel.text = value;
					}
				}
				else if (uisprite != null)
				{
					UIButton uibutton = NGUITools.FindInParents<UIButton>(uisprite.gameObject);
					if (uibutton != null && uibutton.tweenTarget == uisprite.gameObject)
					{
						uibutton.normalSprite = value;
					}
					uisprite.spriteName = value;
					uisprite.MakePixelPerfect();
				}
			}
		}
	}

	// Token: 0x0600209A RID: 8346 RVA: 0x00015DF0 File Offset: 0x00013FF0
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnLocalize();
		}
	}

	// Token: 0x0600209B RID: 8347 RVA: 0x00015E03 File Offset: 0x00014003
	private void Start()
	{
		this.mStarted = true;
		this.OnLocalize();
	}

	// Token: 0x0600209C RID: 8348 RVA: 0x000F66C8 File Offset: 0x000F48C8
	private void OnLocalize()
	{
		if (string.IsNullOrEmpty(this.key))
		{
			UILabel component = base.GetComponent<UILabel>();
			if (component != null)
			{
				this.key = component.text;
			}
		}
		if (!string.IsNullOrEmpty(this.key))
		{
			this.value = Localization.Get(this.key);
		}
	}

	// Token: 0x040023F3 RID: 9203
	public string key;

	// Token: 0x040023F4 RID: 9204
	private bool mStarted;
}
