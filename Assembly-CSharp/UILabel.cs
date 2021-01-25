using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020004FA RID: 1274
[AddComponentMenu("NGUI/UI/NGUI Label")]
[ExecuteInEditMode]
public class UILabel : UIWidget
{
	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06002035 RID: 8245 RVA: 0x0001581D File Offset: 0x00013A1D
	// (set) Token: 0x06002036 RID: 8246 RVA: 0x00015825 File Offset: 0x00013A25
	private bool shouldBeProcessed
	{
		get
		{
			return this.mShouldBeProcessed;
		}
		set
		{
			if (value)
			{
				this.mChanged = true;
				this.mShouldBeProcessed = true;
			}
			else
			{
				this.mShouldBeProcessed = false;
			}
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06002037 RID: 8247 RVA: 0x00015847 File Offset: 0x00013A47
	public override bool isAnchoredHorizontally
	{
		get
		{
			return base.isAnchoredHorizontally || this.mOverflow == UILabel.Overflow.ResizeFreely;
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x06002038 RID: 8248 RVA: 0x00015860 File Offset: 0x00013A60
	public override bool isAnchoredVertically
	{
		get
		{
			return base.isAnchoredVertically || this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight;
		}
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x06002039 RID: 8249 RVA: 0x000F49B4 File Offset: 0x000F2BB4
	// (set) Token: 0x0600203A RID: 8250 RVA: 0x00015885 File Offset: 0x00013A85
	public override Material material
	{
		get
		{
			if (this.mMaterial != null)
			{
				return this.mMaterial;
			}
			if (this.mFont != null)
			{
				return this.mFont.material;
			}
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont.material;
			}
			return null;
		}
		set
		{
			if (this.mMaterial != value)
			{
				base.RemoveFromPanel();
				this.mMaterial = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x0600203B RID: 8251 RVA: 0x000158AB File Offset: 0x00013AAB
	// (set) Token: 0x0600203C RID: 8252 RVA: 0x000158B3 File Offset: 0x00013AB3
	[Obsolete("Use UILabel.bitmapFont instead")]
	public UIFont font
	{
		get
		{
			return this.bitmapFont;
		}
		set
		{
			this.bitmapFont = value;
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x0600203D RID: 8253 RVA: 0x000158BC File Offset: 0x00013ABC
	// (set) Token: 0x0600203E RID: 8254 RVA: 0x000158C4 File Offset: 0x00013AC4
	public UIFont bitmapFont
	{
		get
		{
			return this.mFont;
		}
		set
		{
			if (this.mFont != value)
			{
				base.RemoveFromPanel();
				this.mFont = value;
				this.mTrueTypeFont = null;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x0600203F RID: 8255 RVA: 0x000158F1 File Offset: 0x00013AF1
	// (set) Token: 0x06002040 RID: 8256 RVA: 0x000F4A14 File Offset: 0x000F2C14
	public Font trueTypeFont
	{
		get
		{
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont;
			}
			if (this.mFont != null)
			{
				return this.mFont.dynamicFont;
			}
			return null;
		}
		set
		{
			if (this.mTrueTypeFont != value)
			{
				this.SetActiveFont(null);
				base.RemoveFromPanel();
				this.mTrueTypeFont = value;
				this.shouldBeProcessed = true;
				this.mFont = null;
				this.SetActiveFont(value);
				this.ProcessAndRequest();
				if (this.mActiveTTF != null)
				{
					base.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06002041 RID: 8257 RVA: 0x00015923 File Offset: 0x00013B23
	// (set) Token: 0x06002042 RID: 8258 RVA: 0x000F4A78 File Offset: 0x000F2C78
	public Object ambigiousFont
	{
		get
		{
			return (!(this.mFont != null)) ? this.mTrueTypeFont : this.mFont;
		}
		set
		{
			UIFont uifont = value as UIFont;
			if (uifont != null)
			{
				this.bitmapFont = uifont;
			}
			else
			{
				this.trueTypeFont = (value as Font);
			}
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06002043 RID: 8259 RVA: 0x00015947 File Offset: 0x00013B47
	// (set) Token: 0x06002044 RID: 8260 RVA: 0x000F4AB0 File Offset: 0x000F2CB0
	public string text
	{
		get
		{
			return this.mText;
		}
		set
		{
			if (this.mText == value)
			{
				return;
			}
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(this.mText))
				{
					this.mText = string.Empty;
					this.MarkAsChanged();
					this.ProcessAndRequest();
				}
			}
			else if (this.mText != value)
			{
				this.mText = value;
				this.MarkAsChanged();
				this.ProcessAndRequest();
			}
			if (this.autoResizeBoxCollider)
			{
				base.ResizeCollider();
			}
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06002045 RID: 8261 RVA: 0x000F4B3C File Offset: 0x000F2D3C
	public int defaultFontSize
	{
		get
		{
			return (!(this.trueTypeFont != null)) ? ((!(this.mFont != null)) ? 16 : this.mFont.defaultSize) : this.mFontSize;
		}
	}

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06002046 RID: 8262 RVA: 0x0001594F File Offset: 0x00013B4F
	// (set) Token: 0x06002047 RID: 8263 RVA: 0x00015957 File Offset: 0x00013B57
	public int fontSize
	{
		get
		{
			return this.mFontSize;
		}
		set
		{
			value = Mathf.Clamp(value, 0, 256);
			if (this.mFontSize != value)
			{
				this.mFontSize = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06002048 RID: 8264 RVA: 0x00015987 File Offset: 0x00013B87
	// (set) Token: 0x06002049 RID: 8265 RVA: 0x0001598F File Offset: 0x00013B8F
	public FontStyle fontStyle
	{
		get
		{
			return this.mFontStyle;
		}
		set
		{
			if (this.mFontStyle != value)
			{
				this.mFontStyle = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x0600204A RID: 8266 RVA: 0x000159B1 File Offset: 0x00013BB1
	// (set) Token: 0x0600204B RID: 8267 RVA: 0x000159B9 File Offset: 0x00013BB9
	public NGUIText.Alignment alignment
	{
		get
		{
			return this.mAlignment;
		}
		set
		{
			if (this.mAlignment != value)
			{
				this.mAlignment = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x0600204C RID: 8268 RVA: 0x000159DB File Offset: 0x00013BDB
	// (set) Token: 0x0600204D RID: 8269 RVA: 0x000159E3 File Offset: 0x00013BE3
	public bool applyGradient
	{
		get
		{
			return this.mApplyGradient;
		}
		set
		{
			if (this.mApplyGradient != value)
			{
				this.mApplyGradient = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x0600204E RID: 8270 RVA: 0x000159FE File Offset: 0x00013BFE
	// (set) Token: 0x0600204F RID: 8271 RVA: 0x00015A06 File Offset: 0x00013C06
	public Color gradientTop
	{
		get
		{
			return this.mGradientTop;
		}
		set
		{
			if (this.mGradientTop != value)
			{
				this.mGradientTop = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06002050 RID: 8272 RVA: 0x00015A31 File Offset: 0x00013C31
	// (set) Token: 0x06002051 RID: 8273 RVA: 0x00015A39 File Offset: 0x00013C39
	public Color gradientBottom
	{
		get
		{
			return this.mGradientBottom;
		}
		set
		{
			if (this.mGradientBottom != value)
			{
				this.mGradientBottom = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x06002052 RID: 8274 RVA: 0x00015A64 File Offset: 0x00013C64
	// (set) Token: 0x06002053 RID: 8275 RVA: 0x00015A6C File Offset: 0x00013C6C
	public int spacingX
	{
		get
		{
			return this.mSpacingX;
		}
		set
		{
			if (this.mSpacingX != value)
			{
				this.mSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06002054 RID: 8276 RVA: 0x00015A87 File Offset: 0x00013C87
	// (set) Token: 0x06002055 RID: 8277 RVA: 0x00015A8F File Offset: 0x00013C8F
	public int spacingY
	{
		get
		{
			return this.mSpacingY;
		}
		set
		{
			if (this.mSpacingY != value)
			{
				this.mSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06002056 RID: 8278 RVA: 0x00015AAA File Offset: 0x00013CAA
	private bool keepCrisp
	{
		get
		{
			return this.trueTypeFont != null && this.keepCrispWhenShrunk != UILabel.Crispness.Never;
		}
	}

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06002057 RID: 8279 RVA: 0x00015ACB File Offset: 0x00013CCB
	// (set) Token: 0x06002058 RID: 8280 RVA: 0x00015AD3 File Offset: 0x00013CD3
	public bool supportEncoding
	{
		get
		{
			return this.mEncoding;
		}
		set
		{
			if (this.mEncoding != value)
			{
				this.mEncoding = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06002059 RID: 8281 RVA: 0x00015AEF File Offset: 0x00013CEF
	// (set) Token: 0x0600205A RID: 8282 RVA: 0x00015AF7 File Offset: 0x00013CF7
	public NGUIText.SymbolStyle symbolStyle
	{
		get
		{
			return this.mSymbols;
		}
		set
		{
			if (this.mSymbols != value)
			{
				this.mSymbols = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x0600205B RID: 8283 RVA: 0x00015B13 File Offset: 0x00013D13
	// (set) Token: 0x0600205C RID: 8284 RVA: 0x00015B1B File Offset: 0x00013D1B
	public UILabel.Overflow overflowMethod
	{
		get
		{
			return this.mOverflow;
		}
		set
		{
			if (this.mOverflow != value)
			{
				this.mOverflow = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x0600205D RID: 8285 RVA: 0x00015B37 File Offset: 0x00013D37
	// (set) Token: 0x0600205E RID: 8286 RVA: 0x00015B3F File Offset: 0x00013D3F
	[Obsolete("Use 'width' instead")]
	public int lineWidth
	{
		get
		{
			return base.width;
		}
		set
		{
			base.width = value;
		}
	}

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x0600205F RID: 8287 RVA: 0x00015B48 File Offset: 0x00013D48
	// (set) Token: 0x06002060 RID: 8288 RVA: 0x00015B50 File Offset: 0x00013D50
	[Obsolete("Use 'height' instead")]
	public int lineHeight
	{
		get
		{
			return base.height;
		}
		set
		{
			base.height = value;
		}
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06002061 RID: 8289 RVA: 0x00015B59 File Offset: 0x00013D59
	// (set) Token: 0x06002062 RID: 8290 RVA: 0x00015B67 File Offset: 0x00013D67
	public bool multiLine
	{
		get
		{
			return this.mMaxLineCount != 1;
		}
		set
		{
			if (this.mMaxLineCount != 1 != value)
			{
				this.mMaxLineCount = ((!value) ? 1 : 0);
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06002063 RID: 8291 RVA: 0x00015B95 File Offset: 0x00013D95
	public override Vector3[] localCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.localCorners;
		}
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06002064 RID: 8292 RVA: 0x00015BAE File Offset: 0x00013DAE
	public override Vector3[] worldCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.worldCorners;
		}
	}

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x06002065 RID: 8293 RVA: 0x00015BC7 File Offset: 0x00013DC7
	public override Vector4 drawingDimensions
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.drawingDimensions;
		}
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06002066 RID: 8294 RVA: 0x00015BE0 File Offset: 0x00013DE0
	// (set) Token: 0x06002067 RID: 8295 RVA: 0x00015BE8 File Offset: 0x00013DE8
	public int maxLineCount
	{
		get
		{
			return this.mMaxLineCount;
		}
		set
		{
			if (this.mMaxLineCount != value)
			{
				this.mMaxLineCount = Mathf.Max(value, 0);
				this.shouldBeProcessed = true;
				if (this.overflowMethod == UILabel.Overflow.ShrinkContent)
				{
					this.MakePixelPerfect();
				}
			}
		}
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06002068 RID: 8296 RVA: 0x00015C1B File Offset: 0x00013E1B
	// (set) Token: 0x06002069 RID: 8297 RVA: 0x00015C23 File Offset: 0x00013E23
	public UILabel.Effect effectStyle
	{
		get
		{
			return this.mEffectStyle;
		}
		set
		{
			if (this.mEffectStyle != value)
			{
				this.mEffectStyle = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x0600206A RID: 8298 RVA: 0x00015C3F File Offset: 0x00013E3F
	// (set) Token: 0x0600206B RID: 8299 RVA: 0x00015C47 File Offset: 0x00013E47
	public Color effectColor
	{
		get
		{
			return this.mEffectColor;
		}
		set
		{
			if (this.mEffectColor != value)
			{
				this.mEffectColor = value;
				if (this.mEffectStyle != UILabel.Effect.None)
				{
					this.shouldBeProcessed = true;
				}
			}
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x0600206C RID: 8300 RVA: 0x00015C73 File Offset: 0x00013E73
	// (set) Token: 0x0600206D RID: 8301 RVA: 0x00015C7B File Offset: 0x00013E7B
	public Vector2 effectDistance
	{
		get
		{
			return this.mEffectDistance;
		}
		set
		{
			if (this.mEffectDistance != value)
			{
				this.mEffectDistance = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x0600206E RID: 8302 RVA: 0x00015C9C File Offset: 0x00013E9C
	// (set) Token: 0x0600206F RID: 8303 RVA: 0x00015CA7 File Offset: 0x00013EA7
	[Obsolete("Use 'overflowMethod == UILabel.Overflow.ShrinkContent' instead")]
	public bool shrinkToFit
	{
		get
		{
			return this.mOverflow == UILabel.Overflow.ShrinkContent;
		}
		set
		{
			if (value)
			{
				this.overflowMethod = UILabel.Overflow.ShrinkContent;
			}
		}
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x06002070 RID: 8304 RVA: 0x000F4B88 File Offset: 0x000F2D88
	public string processedText
	{
		get
		{
			if (this.mLastWidth != this.mWidth || this.mLastHeight != this.mHeight)
			{
				this.mLastWidth = this.mWidth;
				this.mLastHeight = this.mHeight;
				this.mShouldBeProcessed = true;
			}
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return this.mProcessedText;
		}
	}

	// Token: 0x17000320 RID: 800
	// (get) Token: 0x06002071 RID: 8305 RVA: 0x00015CB6 File Offset: 0x00013EB6
	public Vector2 printedSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return this.mCalculatedSize;
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x06002072 RID: 8306 RVA: 0x00015CCF File Offset: 0x00013ECF
	public override Vector2 localSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.localSize;
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x06002073 RID: 8307 RVA: 0x00015CE8 File Offset: 0x00013EE8
	private bool isValid
	{
		get
		{
			return this.mFont != null || this.mTrueTypeFont != null;
		}
	}

	// Token: 0x06002074 RID: 8308 RVA: 0x000F4BF0 File Offset: 0x000F2DF0
	protected override void OnInit()
	{
		if (this.bitmapFont != null && GameSetting.m_Instance != null)
		{
			if (this.bitmapFont.name == "ChineseDragon")
			{
				this.bitmapFont = GameSetting.m_Instance.m_NameFont;
			}
			else if (this.bitmapFont.name == "ChnieseFont")
			{
				this.bitmapFont = GameSetting.m_Instance.m_TextFont;
			}
		}
		base.OnInit();
		UILabel.mList.Add(this);
		this.SetActiveFont(this.trueTypeFont);
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x00015D0A File Offset: 0x00013F0A
	protected override void OnDisable()
	{
		this.SetActiveFont(null);
		UILabel.mList.Remove(this);
		base.OnDisable();
	}

	// Token: 0x06002076 RID: 8310 RVA: 0x000F4C94 File Offset: 0x000F2E94
	protected void SetActiveFont(Font fnt)
	{
		if (this.mActiveTTF != fnt)
		{
			if (this.mActiveTTF != null)
			{
				int num;
				if (UILabel.mFontUsage.TryGetValue(this.mActiveTTF, ref num))
				{
					num = Mathf.Max(0, --num);
					if (num == 0)
					{
						this.mActiveTTF.textureRebuildCallback = null;
						UILabel.mFontUsage.Remove(this.mActiveTTF);
					}
					else
					{
						UILabel.mFontUsage[this.mActiveTTF] = num;
					}
				}
				else
				{
					this.mActiveTTF.textureRebuildCallback = null;
				}
			}
			this.mActiveTTF = fnt;
			if (this.mActiveTTF != null)
			{
				int num2 = 0;
				if (!UILabel.mFontUsage.TryGetValue(this.mActiveTTF, ref num2))
				{
					this.mActiveTTF.textureRebuildCallback = new Font.FontTextureRebuildCallback(UILabel.OnFontTextureChanged);
				}
				num2 = (UILabel.mFontUsage[this.mActiveTTF] = num2 + 1);
			}
		}
	}

	// Token: 0x06002077 RID: 8311 RVA: 0x000F4D90 File Offset: 0x000F2F90
	private static void OnFontTextureChanged()
	{
		for (int i = 0; i < UILabel.mList.size; i++)
		{
			UILabel uilabel = UILabel.mList[i];
			if (uilabel != null)
			{
				Font trueTypeFont = uilabel.trueTypeFont;
				if (trueTypeFont != null)
				{
					trueTypeFont.RequestCharactersInTexture(uilabel.mText, uilabel.mPrintedSize, uilabel.mFontStyle);
				}
			}
		}
		for (int j = 0; j < UILabel.mList.size; j++)
		{
			UILabel uilabel2 = UILabel.mList[j];
			if (uilabel2 != null)
			{
				Font trueTypeFont2 = uilabel2.trueTypeFont;
				if (trueTypeFont2 != null)
				{
					uilabel2.RemoveFromPanel();
					uilabel2.CreatePanel();
				}
			}
		}
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x00015D25 File Offset: 0x00013F25
	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.shouldBeProcessed)
		{
			this.ProcessText();
		}
		return base.GetSides(relativeTo);
	}

	// Token: 0x06002079 RID: 8313 RVA: 0x000F4E58 File Offset: 0x000F3058
	protected override void UpgradeFrom265()
	{
		this.ProcessText(true, true);
		if (this.mShrinkToFit)
		{
			this.overflowMethod = UILabel.Overflow.ShrinkContent;
			this.mMaxLineCount = 0;
		}
		if (this.mMaxLineWidth != 0)
		{
			base.width = this.mMaxLineWidth;
			this.overflowMethod = ((this.mMaxLineCount <= 0) ? UILabel.Overflow.ShrinkContent : UILabel.Overflow.ResizeHeight);
		}
		else
		{
			this.overflowMethod = UILabel.Overflow.ResizeFreely;
		}
		if (this.mMaxLineHeight != 0)
		{
			base.height = this.mMaxLineHeight;
		}
		if (this.mFont != null)
		{
			int defaultSize = this.mFont.defaultSize;
			if (base.height < defaultSize)
			{
				base.height = defaultSize;
			}
			this.fontSize = defaultSize;
		}
		this.mMaxLineWidth = 0;
		this.mMaxLineHeight = 0;
		this.mShrinkToFit = false;
		NGUITools.UpdateWidgetCollider(base.gameObject, true);
	}

	// Token: 0x0600207A RID: 8314 RVA: 0x000F4F34 File Offset: 0x000F3134
	protected override void OnAnchor()
	{
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			if (base.isFullyAnchored)
			{
				this.mOverflow = UILabel.Overflow.ShrinkContent;
			}
		}
		else if (this.mOverflow == UILabel.Overflow.ResizeHeight && this.topAnchor.target != null && this.bottomAnchor.target != null)
		{
			this.mOverflow = UILabel.Overflow.ShrinkContent;
		}
		base.OnAnchor();
	}

	// Token: 0x0600207B RID: 8315 RVA: 0x00015D3F File Offset: 0x00013F3F
	private void ProcessAndRequest()
	{
		if (this.ambigiousFont != null)
		{
			this.ProcessText();
		}
	}

	// Token: 0x0600207C RID: 8316 RVA: 0x000F4FAC File Offset: 0x000F31AC
	protected override void OnStart()
	{
		base.OnStart();
		if (this.mLineWidth > 0f)
		{
			this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
			this.mLineWidth = 0f;
		}
		if (!this.mMultiline)
		{
			this.mMaxLineCount = 1;
			this.mMultiline = true;
		}
		this.mPremultiply = (this.material != null && this.material.shader != null && this.material.shader.name.Contains("Premultiplied"));
		this.ProcessAndRequest();
	}

	// Token: 0x0600207D RID: 8317 RVA: 0x00015D58 File Offset: 0x00013F58
	public override void MarkAsChanged()
	{
		this.shouldBeProcessed = true;
		base.MarkAsChanged();
	}

	// Token: 0x0600207E RID: 8318 RVA: 0x00015D67 File Offset: 0x00013F67
	public void ProcessText()
	{
		this.ProcessText(false, true);
	}

	// Token: 0x0600207F RID: 8319 RVA: 0x000F5054 File Offset: 0x000F3254
	private void ProcessText(bool legacyMode, bool full)
	{
		if (!this.isValid)
		{
			return;
		}
		this.mChanged = true;
		this.shouldBeProcessed = false;
		float num = this.mDrawRegion.z - this.mDrawRegion.x;
		float num2 = this.mDrawRegion.w - this.mDrawRegion.y;
		NGUIText.rectWidth = ((!legacyMode) ? base.width : ((this.mMaxLineWidth == 0) ? 1000000 : this.mMaxLineWidth));
		NGUIText.rectHeight = ((!legacyMode) ? base.height : ((this.mMaxLineHeight == 0) ? 1000000 : this.mMaxLineHeight));
		NGUIText.regionWidth = ((num == 1f) ? NGUIText.rectWidth : Mathf.RoundToInt((float)NGUIText.rectWidth * num));
		NGUIText.regionHeight = ((num2 == 1f) ? NGUIText.rectHeight : Mathf.RoundToInt((float)NGUIText.rectHeight * num2));
		this.mPrintedSize = Mathf.Abs((!legacyMode) ? this.defaultFontSize : Mathf.RoundToInt(base.cachedTransform.localScale.x));
		this.mScale = 1f;
		if (NGUIText.regionWidth < 1 || NGUIText.regionHeight < 0)
		{
			this.mProcessedText = string.Empty;
			return;
		}
		bool flag = this.trueTypeFont != null;
		if (flag && this.keepCrisp)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				this.mDensity = ((!(root != null)) ? 1f : root.pixelSizeAdjustment);
			}
		}
		else
		{
			this.mDensity = 1f;
		}
		if (full)
		{
			this.UpdateNGUIText();
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			NGUIText.rectWidth = 1000000;
			NGUIText.regionWidth = 1000000;
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight)
		{
			NGUIText.rectHeight = 1000000;
			NGUIText.regionHeight = 1000000;
		}
		if (this.mPrintedSize > 0)
		{
			bool keepCrisp = this.keepCrisp;
			for (int i = this.mPrintedSize; i > 0; i--)
			{
				if (keepCrisp)
				{
					this.mPrintedSize = i;
					NGUIText.fontSize = this.mPrintedSize;
				}
				else
				{
					this.mScale = (float)i / (float)this.mPrintedSize;
					NGUIText.fontScale = ((!flag) ? ((float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale) : this.mScale);
				}
				NGUIText.Update(false);
				bool flag2 = NGUIText.WrapText(this.mText, out this.mProcessedText, true);
				if (this.mOverflow != UILabel.Overflow.ShrinkContent || flag2)
				{
					if (this.mOverflow == UILabel.Overflow.ResizeFreely)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						this.mWidth = Mathf.Max(this.minWidth, Mathf.RoundToInt(this.mCalculatedSize.x));
						if (num != 1f)
						{
							this.mWidth = Mathf.RoundToInt((float)this.mWidth / num);
						}
						this.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (num2 != 1f)
						{
							this.mHeight = Mathf.RoundToInt((float)this.mHeight / num2);
						}
						if ((this.mWidth & 1) == 1)
						{
							this.mWidth++;
						}
						if ((this.mHeight & 1) == 1)
						{
							this.mHeight++;
						}
					}
					else if (this.mOverflow == UILabel.Overflow.ResizeHeight)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						this.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (num2 != 1f)
						{
							this.mHeight = Mathf.RoundToInt((float)this.mHeight / num2);
						}
						if ((this.mHeight & 1) == 1)
						{
							this.mHeight++;
						}
					}
					else
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
					}
					if (legacyMode)
					{
						base.width = Mathf.RoundToInt(this.mCalculatedSize.x);
						base.height = Mathf.RoundToInt(this.mCalculatedSize.y);
						base.cachedTransform.localScale = Vector3.one;
					}
					break;
				}
				if (--i <= 1)
				{
					break;
				}
			}
		}
		else
		{
			base.cachedTransform.localScale = Vector3.one;
			this.mProcessedText = string.Empty;
			this.mScale = 1f;
		}
		if (full)
		{
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
	}

	// Token: 0x06002080 RID: 8320 RVA: 0x000F5538 File Offset: 0x000F3738
	public override void MakePixelPerfect()
	{
		if (this.ambigiousFont != null)
		{
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
			localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
			localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
			base.cachedTransform.localPosition = localPosition;
			base.cachedTransform.localScale = Vector3.one;
			if (this.mOverflow == UILabel.Overflow.ResizeFreely)
			{
				this.AssumeNaturalSize();
			}
			else
			{
				int width = base.width;
				int height = base.height;
				UILabel.Overflow overflow = this.mOverflow;
				if (overflow != UILabel.Overflow.ResizeHeight)
				{
					this.mWidth = 100000;
				}
				this.mHeight = 100000;
				this.mOverflow = UILabel.Overflow.ShrinkContent;
				this.ProcessText(false, true);
				this.mOverflow = overflow;
				int num = Mathf.RoundToInt(this.mCalculatedSize.x);
				int num2 = Mathf.RoundToInt(this.mCalculatedSize.y);
				num = Mathf.Max(num, base.minWidth);
				num2 = Mathf.Max(num2, base.minHeight);
				this.mWidth = Mathf.Max(width, num);
				this.mHeight = Mathf.Max(height, num2);
				this.MarkAsChanged();
			}
		}
		else
		{
			base.MakePixelPerfect();
		}
	}

	// Token: 0x06002081 RID: 8321 RVA: 0x000F5688 File Offset: 0x000F3888
	public void AssumeNaturalSize()
	{
		if (this.ambigiousFont != null)
		{
			this.mWidth = 100000;
			this.mHeight = 100000;
			this.ProcessText(false, true);
			this.mWidth = Mathf.RoundToInt(this.mCalculatedSize.x);
			this.mHeight = Mathf.RoundToInt(this.mCalculatedSize.y);
			if ((this.mWidth & 1) == 1)
			{
				this.mWidth++;
			}
			if ((this.mHeight & 1) == 1)
			{
				this.mHeight++;
			}
			this.MarkAsChanged();
		}
	}

	// Token: 0x06002082 RID: 8322 RVA: 0x00015D71 File Offset: 0x00013F71
	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector3 worldPos)
	{
		return this.GetCharacterIndexAtPosition(worldPos, false);
	}

	// Token: 0x06002083 RID: 8323 RVA: 0x00015D7B File Offset: 0x00013F7B
	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector2 localPos)
	{
		return this.GetCharacterIndexAtPosition(localPos, false);
	}

	// Token: 0x06002084 RID: 8324 RVA: 0x000F5730 File Offset: 0x000F3930
	public int GetCharacterIndexAtPosition(Vector3 worldPos, bool precise)
	{
		Vector2 localPos = base.cachedTransform.InverseTransformPoint(worldPos);
		return this.GetCharacterIndexAtPosition(localPos, precise);
	}

	// Token: 0x06002085 RID: 8325 RVA: 0x000F5758 File Offset: 0x000F3958
	public int GetCharacterIndexAtPosition(Vector2 localPos, bool precise)
	{
		if (this.isValid)
		{
			string processedText = this.processedText;
			if (string.IsNullOrEmpty(processedText))
			{
				return 0;
			}
			this.UpdateNGUIText();
			if (precise)
			{
				NGUIText.PrintExactCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			else
			{
				NGUIText.PrintApproximateCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			if (UILabel.mTempVerts.size > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int result = (!precise) ? NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos) : NGUIText.GetExactCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos);
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
				NGUIText.bitmapFont = null;
				NGUIText.dynamicFont = null;
				return result;
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
		return 0;
	}

	// Token: 0x06002086 RID: 8326 RVA: 0x000F5830 File Offset: 0x000F3A30
	public string GetWordAtPosition(Vector3 worldPos)
	{
		int characterIndexAtPosition = this.GetCharacterIndexAtPosition(worldPos, true);
		return this.GetWordAtCharacterIndex(characterIndexAtPosition);
	}

	// Token: 0x06002087 RID: 8327 RVA: 0x000F5850 File Offset: 0x000F3A50
	public string GetWordAtPosition(Vector2 localPos)
	{
		int characterIndexAtPosition = this.GetCharacterIndexAtPosition(localPos, true);
		return this.GetWordAtCharacterIndex(characterIndexAtPosition);
	}

	// Token: 0x06002088 RID: 8328 RVA: 0x000F5870 File Offset: 0x000F3A70
	public string GetWordAtCharacterIndex(int characterIndex)
	{
		if (characterIndex != -1 && characterIndex < this.mText.Length)
		{
			int num = this.mText.LastIndexOfAny(new char[]
			{
				' ',
				'\n'
			}, characterIndex) + 1;
			int num2 = this.mText.IndexOfAny(new char[]
			{
				' ',
				'\n',
				',',
				'.'
			}, characterIndex);
			if (num2 == -1)
			{
				num2 = this.mText.Length;
			}
			if (num != num2)
			{
				int num3 = num2 - num;
				if (num3 > 0)
				{
					string text = this.mText.Substring(num, num3);
					return NGUIText.StripSymbols(text);
				}
			}
		}
		return null;
	}

	// Token: 0x06002089 RID: 8329 RVA: 0x00015D85 File Offset: 0x00013F85
	public string GetUrlAtPosition(Vector3 worldPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(worldPos, true));
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x00015D95 File Offset: 0x00013F95
	public string GetUrlAtPosition(Vector2 localPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(localPos, true));
	}

	// Token: 0x0600208B RID: 8331 RVA: 0x000F5910 File Offset: 0x000F3B10
	public string GetUrlAtCharacterIndex(int characterIndex)
	{
		if (characterIndex != -1 && characterIndex < this.mText.Length - 6)
		{
			int num;
			if (this.mText.get_Chars(characterIndex) == '[' && this.mText.get_Chars(characterIndex + 1) == 'u' && this.mText.get_Chars(characterIndex + 2) == 'r' && this.mText.get_Chars(characterIndex + 3) == 'l' && this.mText.get_Chars(characterIndex + 4) == '=')
			{
				num = characterIndex;
			}
			else
			{
				num = this.mText.LastIndexOf("[url=", characterIndex);
			}
			if (num == -1)
			{
				return null;
			}
			num += 5;
			int num2 = this.mText.IndexOf("]", num);
			if (num2 == -1)
			{
				return null;
			}
			int num3 = this.mText.IndexOf("[/url]", num2);
			if (num3 == -1 || characterIndex <= num3)
			{
				return this.mText.Substring(num, num2 - num);
			}
		}
		return null;
	}

	// Token: 0x0600208C RID: 8332 RVA: 0x000F5A10 File Offset: 0x000F3C10
	public int GetCharacterIndex(int currentIndex, KeyCode key)
	{
		if (this.isValid)
		{
			string processedText = this.processedText;
			if (string.IsNullOrEmpty(processedText))
			{
				return 0;
			}
			int defaultFontSize = this.defaultFontSize;
			this.UpdateNGUIText();
			NGUIText.PrintApproximateCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			if (UILabel.mTempVerts.size > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int i = 0;
				while (i < UILabel.mTempIndices.size)
				{
					if (UILabel.mTempIndices[i] == currentIndex)
					{
						Vector2 pos = UILabel.mTempVerts[i];
						if (key == 273)
						{
							pos.y += (float)(defaultFontSize + this.spacingY);
						}
						else if (key == 274)
						{
							pos.y -= (float)(defaultFontSize + this.spacingY);
						}
						else if (key == 278)
						{
							pos.x -= 1000f;
						}
						else if (key == 279)
						{
							pos.x += 1000f;
						}
						int approximateCharacterIndex = NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, pos);
						if (approximateCharacterIndex == currentIndex)
						{
							break;
						}
						UILabel.mTempVerts.Clear();
						UILabel.mTempIndices.Clear();
						return approximateCharacterIndex;
					}
					else
					{
						i++;
					}
				}
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
			if (key == 273 || key == 278)
			{
				return 0;
			}
			if (key == 274 || key == 279)
			{
				return processedText.Length;
			}
		}
		return currentIndex;
	}

	// Token: 0x0600208D RID: 8333 RVA: 0x000F5BD0 File Offset: 0x000F3DD0
	public void PrintOverlay(int start, int end, UIGeometry caret, UIGeometry highlight, Color caretColor, Color highlightColor)
	{
		if (caret != null)
		{
			caret.Clear();
		}
		if (highlight != null)
		{
			highlight.Clear();
		}
		if (!this.isValid)
		{
			return;
		}
		string processedText = this.processedText;
		this.UpdateNGUIText();
		int size = caret.verts.size;
		Vector2 item;
		item..ctor(0.5f, 0.5f);
		float finalAlpha = this.finalAlpha;
		if (highlight != null && start != end)
		{
			int size2 = highlight.verts.size;
			NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, highlight.verts);
			if (highlight.verts.size > size2)
			{
				this.ApplyOffset(highlight.verts, size2);
				Color32 item2 = new Color(highlightColor.r, highlightColor.g, highlightColor.b, highlightColor.a * finalAlpha);
				for (int i = size2; i < highlight.verts.size; i++)
				{
					highlight.uvs.Add(item);
					highlight.cols.Add(item2);
				}
			}
		}
		else
		{
			NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, null);
		}
		this.ApplyOffset(caret.verts, size);
		Color32 item3 = new Color(caretColor.r, caretColor.g, caretColor.b, caretColor.a * finalAlpha);
		for (int j = size; j < caret.verts.size; j++)
		{
			caret.uvs.Add(item);
			caret.cols.Add(item3);
		}
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x000F5D80 File Offset: 0x000F3F80
	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (!this.isValid)
		{
			return;
		}
		int num = verts.size;
		Color color = base.color;
		color.a = this.finalAlpha;
		if (this.mFont != null && this.mFont.premultipliedAlphaShader)
		{
			color = NGUITools.ApplyPMA(color);
		}
		if (QualitySettings.activeColorSpace == 1)
		{
			color.r = Mathf.Pow(color.r, 2.2f);
			color.g = Mathf.Pow(color.g, 2.2f);
			color.b = Mathf.Pow(color.b, 2.2f);
		}
		string processedText = this.processedText;
		int size = verts.size;
		this.UpdateNGUIText();
		NGUIText.tint = color;
		NGUIText.Print(processedText, verts, uvs, cols);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		Vector2 vector = this.ApplyOffset(verts, size);
		if (this.mFont != null && this.mFont.packedFontShader)
		{
			return;
		}
		if (this.effectStyle != UILabel.Effect.None)
		{
			int size2 = verts.size;
			vector.x = this.mEffectDistance.x;
			vector.y = this.mEffectDistance.y;
			this.ApplyShadow(verts, uvs, cols, num, size2, vector.x, -vector.y);
			if (this.effectStyle == UILabel.Effect.Outline)
			{
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, -vector.x, vector.y);
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, vector.x, vector.y);
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, -vector.x, -vector.y);
			}
		}
		if (this.onPostFill != null)
		{
			this.onPostFill(this, num, verts, uvs, cols);
		}
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x000F5F78 File Offset: 0x000F4178
	public Vector2 ApplyOffset(BetterList<Vector3> verts, int start)
	{
		Vector2 pivotOffset = base.pivotOffset;
		float num = Mathf.Lerp(0f, (float)(-(float)this.mWidth), pivotOffset.x);
		float num2 = Mathf.Lerp((float)this.mHeight, 0f, pivotOffset.y) + Mathf.Lerp(this.mCalculatedSize.y - (float)this.mHeight, 0f, pivotOffset.y);
		num = Mathf.Round(num);
		num2 = Mathf.Round(num2);
		for (int i = start; i < verts.size; i++)
		{
			Vector3[] buffer = verts.buffer;
			int num3 = i;
			buffer[num3].x = buffer[num3].x + num;
			Vector3[] buffer2 = verts.buffer;
			int num4 = i;
			buffer2[num4].y = buffer2[num4].y + num2;
		}
		return new Vector2(num, num2);
	}

	// Token: 0x06002090 RID: 8336 RVA: 0x000F6044 File Offset: 0x000F4244
	public void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
	{
		Color color = this.mEffectColor;
		color.a *= this.finalAlpha;
		Color32 color2 = (!(this.bitmapFont != null) || !this.bitmapFont.premultipliedAlphaShader) ? color : NGUITools.ApplyPMA(color);
		for (int i = start; i < end; i++)
		{
			verts.Add(verts.buffer[i]);
			uvs.Add(uvs.buffer[i]);
			cols.Add(cols.buffer[i]);
			Vector3 vector = verts.buffer[i];
			vector.x += x;
			vector.y += y;
			verts.buffer[i] = vector;
			Color32 color3 = cols.buffer[i];
			if (color3.a == 255)
			{
				cols.buffer[i] = color2;
			}
			else
			{
				Color color4 = color;
				color4.a = (float)color3.a / 255f * color.a;
				cols.buffer[i] = ((!(this.bitmapFont != null) || !this.bitmapFont.premultipliedAlphaShader) ? color4 : NGUITools.ApplyPMA(color4));
			}
		}
	}

	// Token: 0x06002091 RID: 8337 RVA: 0x000F61DC File Offset: 0x000F43DC
	public int CalculateOffsetToFit(string text)
	{
		this.UpdateNGUIText();
		NGUIText.encoding = false;
		NGUIText.symbolStyle = NGUIText.SymbolStyle.None;
		int result = NGUIText.CalculateOffsetToFit(text);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return result;
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x000F6210 File Offset: 0x000F4410
	public void SetCurrentProgress()
	{
		if (UIProgressBar.current != null)
		{
			this.text = UIProgressBar.current.value.ToString("F");
		}
	}

	// Token: 0x06002093 RID: 8339 RVA: 0x00015DA5 File Offset: 0x00013FA5
	public void SetCurrentPercent()
	{
		if (UIProgressBar.current != null)
		{
			this.text = Mathf.RoundToInt(UIProgressBar.current.value * 100f) + "%";
		}
	}

	// Token: 0x06002094 RID: 8340 RVA: 0x000F624C File Offset: 0x000F444C
	public void SetCurrentSelection()
	{
		if (UIPopupList.current != null)
		{
			this.text = ((!UIPopupList.current.isLocalized) ? UIPopupList.current.value : Localization.Get(UIPopupList.current.value));
		}
	}

	// Token: 0x06002095 RID: 8341 RVA: 0x00015DE1 File Offset: 0x00013FE1
	public bool Wrap(string text, out string final)
	{
		return this.Wrap(text, out final, 1000000);
	}

	// Token: 0x06002096 RID: 8342 RVA: 0x000F629C File Offset: 0x000F449C
	public bool Wrap(string text, out string final, int height)
	{
		this.UpdateNGUIText();
		NGUIText.rectHeight = height;
		NGUIText.regionHeight = height;
		bool result = NGUIText.WrapText(text, out final);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return result;
	}

	// Token: 0x06002097 RID: 8343 RVA: 0x000F62D0 File Offset: 0x000F44D0
	public void UpdateNGUIText()
	{
		Font trueTypeFont = this.trueTypeFont;
		bool flag = trueTypeFont != null;
		NGUIText.fontSize = this.mPrintedSize;
		NGUIText.fontStyle = this.mFontStyle;
		NGUIText.rectWidth = this.mWidth;
		NGUIText.rectHeight = this.mHeight;
		NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
		NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		NGUIText.gradient = (this.mApplyGradient && (this.mFont == null || !this.mFont.packedFontShader));
		NGUIText.gradientTop = this.mGradientTop;
		NGUIText.gradientBottom = this.mGradientBottom;
		NGUIText.encoding = this.mEncoding;
		NGUIText.premultiply = this.mPremultiply;
		NGUIText.symbolStyle = this.mSymbols;
		NGUIText.maxLines = this.mMaxLineCount;
		NGUIText.spacingX = (float)this.mSpacingX;
		NGUIText.spacingY = (float)this.mSpacingY;
		NGUIText.fontScale = ((!flag) ? ((float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale) : this.mScale);
		if (this.mFont != null)
		{
			NGUIText.bitmapFont = this.mFont;
			for (;;)
			{
				UIFont replacement = NGUIText.bitmapFont.replacement;
				if (replacement == null)
				{
					break;
				}
				NGUIText.bitmapFont = replacement;
			}
			if (NGUIText.bitmapFont.isDynamic)
			{
				NGUIText.dynamicFont = NGUIText.bitmapFont.dynamicFont;
				NGUIText.bitmapFont = null;
			}
			else
			{
				NGUIText.dynamicFont = null;
			}
		}
		else
		{
			NGUIText.dynamicFont = trueTypeFont;
			NGUIText.bitmapFont = null;
		}
		if (flag && this.keepCrisp)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				NGUIText.pixelDensity = ((!(root != null)) ? 1f : root.pixelSizeAdjustment);
			}
		}
		else
		{
			NGUIText.pixelDensity = 1f;
		}
		if (this.mDensity != NGUIText.pixelDensity)
		{
			this.ProcessText(false, false);
			NGUIText.rectWidth = this.mWidth;
			NGUIText.rectHeight = this.mHeight;
			NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
			NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		}
		if (this.alignment == NGUIText.Alignment.Automatic)
		{
			UIWidget.Pivot pivot = base.pivot;
			if (pivot == UIWidget.Pivot.Left || pivot == UIWidget.Pivot.TopLeft || pivot == UIWidget.Pivot.BottomLeft)
			{
				NGUIText.alignment = NGUIText.Alignment.Left;
			}
			else if (pivot == UIWidget.Pivot.Right || pivot == UIWidget.Pivot.TopRight || pivot == UIWidget.Pivot.BottomRight)
			{
				NGUIText.alignment = NGUIText.Alignment.Right;
			}
			else
			{
				NGUIText.alignment = NGUIText.Alignment.Center;
			}
		}
		else
		{
			NGUIText.alignment = this.alignment;
		}
		NGUIText.Update();
	}

	// Token: 0x040023BF RID: 9151
	public UILabel.Crispness keepCrispWhenShrunk = UILabel.Crispness.OnDesktop;

	// Token: 0x040023C0 RID: 9152
	[SerializeField]
	[HideInInspector]
	private Font mTrueTypeFont;

	// Token: 0x040023C1 RID: 9153
	[SerializeField]
	[HideInInspector]
	private UIFont mFont;

	// Token: 0x040023C2 RID: 9154
	[SerializeField]
	[HideInInspector]
	[Multiline(6)]
	private string mText = string.Empty;

	// Token: 0x040023C3 RID: 9155
	[HideInInspector]
	[SerializeField]
	private int mFontSize = 16;

	// Token: 0x040023C4 RID: 9156
	[SerializeField]
	[HideInInspector]
	private FontStyle mFontStyle;

	// Token: 0x040023C5 RID: 9157
	[HideInInspector]
	[SerializeField]
	private NGUIText.Alignment mAlignment;

	// Token: 0x040023C6 RID: 9158
	[HideInInspector]
	[SerializeField]
	private bool mEncoding = true;

	// Token: 0x040023C7 RID: 9159
	[SerializeField]
	[HideInInspector]
	private int mMaxLineCount;

	// Token: 0x040023C8 RID: 9160
	[SerializeField]
	[HideInInspector]
	private UILabel.Effect mEffectStyle;

	// Token: 0x040023C9 RID: 9161
	[SerializeField]
	[HideInInspector]
	private Color mEffectColor = Color.black;

	// Token: 0x040023CA RID: 9162
	[HideInInspector]
	[SerializeField]
	private NGUIText.SymbolStyle mSymbols = NGUIText.SymbolStyle.Normal;

	// Token: 0x040023CB RID: 9163
	[SerializeField]
	[HideInInspector]
	private Vector2 mEffectDistance = Vector2.one;

	// Token: 0x040023CC RID: 9164
	[HideInInspector]
	[SerializeField]
	private UILabel.Overflow mOverflow;

	// Token: 0x040023CD RID: 9165
	[SerializeField]
	[HideInInspector]
	private Material mMaterial;

	// Token: 0x040023CE RID: 9166
	[SerializeField]
	[HideInInspector]
	private bool mApplyGradient;

	// Token: 0x040023CF RID: 9167
	[HideInInspector]
	[SerializeField]
	private Color mGradientTop = Color.white;

	// Token: 0x040023D0 RID: 9168
	[HideInInspector]
	[SerializeField]
	private Color mGradientBottom = new Color(0.7f, 0.7f, 0.7f);

	// Token: 0x040023D1 RID: 9169
	[HideInInspector]
	[SerializeField]
	private int mSpacingX;

	// Token: 0x040023D2 RID: 9170
	[SerializeField]
	[HideInInspector]
	private int mSpacingY;

	// Token: 0x040023D3 RID: 9171
	[HideInInspector]
	[SerializeField]
	private bool mShrinkToFit;

	// Token: 0x040023D4 RID: 9172
	[SerializeField]
	[HideInInspector]
	private int mMaxLineWidth;

	// Token: 0x040023D5 RID: 9173
	[HideInInspector]
	[SerializeField]
	private int mMaxLineHeight;

	// Token: 0x040023D6 RID: 9174
	[HideInInspector]
	[SerializeField]
	private float mLineWidth;

	// Token: 0x040023D7 RID: 9175
	[HideInInspector]
	[SerializeField]
	private bool mMultiline = true;

	// Token: 0x040023D8 RID: 9176
	[NonSerialized]
	private Font mActiveTTF;

	// Token: 0x040023D9 RID: 9177
	private float mDensity = 1f;

	// Token: 0x040023DA RID: 9178
	private bool mShouldBeProcessed = true;

	// Token: 0x040023DB RID: 9179
	private string mProcessedText;

	// Token: 0x040023DC RID: 9180
	private bool mPremultiply;

	// Token: 0x040023DD RID: 9181
	private Vector2 mCalculatedSize = Vector2.zero;

	// Token: 0x040023DE RID: 9182
	private float mScale = 1f;

	// Token: 0x040023DF RID: 9183
	private int mPrintedSize;

	// Token: 0x040023E0 RID: 9184
	private int mLastWidth;

	// Token: 0x040023E1 RID: 9185
	private int mLastHeight;

	// Token: 0x040023E2 RID: 9186
	private static BetterList<UILabel> mList = new BetterList<UILabel>();

	// Token: 0x040023E3 RID: 9187
	public static Dictionary<Font, int> mFontUsage = new Dictionary<Font, int>();

	// Token: 0x040023E4 RID: 9188
	private static BetterList<Vector3> mTempVerts = new BetterList<Vector3>();

	// Token: 0x040023E5 RID: 9189
	private static BetterList<int> mTempIndices = new BetterList<int>();

	// Token: 0x020004FB RID: 1275
	public enum Effect
	{
		// Token: 0x040023E7 RID: 9191
		None,
		// Token: 0x040023E8 RID: 9192
		Shadow,
		// Token: 0x040023E9 RID: 9193
		Outline
	}

	// Token: 0x020004FC RID: 1276
	public enum Overflow
	{
		// Token: 0x040023EB RID: 9195
		ShrinkContent,
		// Token: 0x040023EC RID: 9196
		ClampContent,
		// Token: 0x040023ED RID: 9197
		ResizeFreely,
		// Token: 0x040023EE RID: 9198
		ResizeHeight
	}

	// Token: 0x020004FD RID: 1277
	public enum Crispness
	{
		// Token: 0x040023F0 RID: 9200
		Never,
		// Token: 0x040023F1 RID: 9201
		OnDesktop,
		// Token: 0x040023F2 RID: 9202
		Always
	}
}
