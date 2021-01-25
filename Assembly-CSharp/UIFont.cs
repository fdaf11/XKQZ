using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F2 RID: 1266
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Font")]
public class UIFont : MonoBehaviour
{
	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06001FCE RID: 8142 RVA: 0x000151E0 File Offset: 0x000133E0
	// (set) Token: 0x06001FCF RID: 8143 RVA: 0x00015209 File Offset: 0x00013409
	public BMFont bmFont
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mFont : this.mReplacement.bmFont;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.bmFont = value;
			}
			else
			{
				this.mFont = value;
			}
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x00015234 File Offset: 0x00013434
	// (set) Token: 0x06001FD1 RID: 8145 RVA: 0x00015273 File Offset: 0x00013473
	public int texWidth
	{
		get
		{
			return (!(this.mReplacement != null)) ? ((this.mFont == null) ? 1 : this.mFont.texWidth) : this.mReplacement.texWidth;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.texWidth = value;
			}
			else if (this.mFont != null)
			{
				this.mFont.texWidth = value;
			}
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x000152AE File Offset: 0x000134AE
	// (set) Token: 0x06001FD3 RID: 8147 RVA: 0x000152ED File Offset: 0x000134ED
	public int texHeight
	{
		get
		{
			return (!(this.mReplacement != null)) ? ((this.mFont == null) ? 1 : this.mFont.texHeight) : this.mReplacement.texHeight;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.texHeight = value;
			}
			else if (this.mFont != null)
			{
				this.mFont.texHeight = value;
			}
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x000F2228 File Offset: 0x000F0428
	public bool hasSymbols
	{
		get
		{
			return (!(this.mReplacement != null)) ? (this.mSymbols != null && this.mSymbols.Count != 0) : this.mReplacement.hasSymbols;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x00015328 File Offset: 0x00013528
	public List<BMSymbol> symbols
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mSymbols : this.mReplacement.symbols;
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x00015351 File Offset: 0x00013551
	// (set) Token: 0x06001FD7 RID: 8151 RVA: 0x000F2278 File Offset: 0x000F0478
	public UIAtlas atlas
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mAtlas : this.mReplacement.atlas;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.atlas = value;
			}
			else if (this.mAtlas != value)
			{
				if (value == null)
				{
					if (this.mAtlas != null)
					{
						this.mMat = this.mAtlas.spriteMaterial;
					}
					if (this.sprite != null)
					{
						this.mUVRect = this.uvRect;
					}
				}
				this.mPMA = -1;
				this.mAtlas = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x000F2314 File Offset: 0x000F0514
	// (set) Token: 0x06001FD9 RID: 8153 RVA: 0x000F23D8 File Offset: 0x000F05D8
	public Material material
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.material;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.spriteMaterial;
			}
			if (this.mMat != null)
			{
				if (this.mDynamicFont != null && this.mMat != this.mDynamicFont.material)
				{
					this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
				}
				return this.mMat;
			}
			if (this.mDynamicFont != null)
			{
				return this.mDynamicFont.material;
			}
			return null;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.material = value;
			}
			else if (this.mMat != value)
			{
				this.mPMA = -1;
				this.mMat = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06001FDA RID: 8154 RVA: 0x0001537A File Offset: 0x0001357A
	[Obsolete("Use UIFont.premultipliedAlphaShader instead")]
	public bool premultipliedAlpha
	{
		get
		{
			return this.premultipliedAlphaShader;
		}
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06001FDB RID: 8155 RVA: 0x000F242C File Offset: 0x000F062C
	public bool premultipliedAlphaShader
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlphaShader;
			}
			if (this.mAtlas != null)
			{
				return this.mAtlas.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = ((!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06001FDC RID: 8156 RVA: 0x000F24D4 File Offset: 0x000F06D4
	public bool packedFontShader
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.packedFontShader;
			}
			if (this.mAtlas != null)
			{
				return false;
			}
			if (this.mPacked == -1)
			{
				Material material = this.material;
				this.mPacked = ((!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Packed")) ? 0 : 1);
			}
			return this.mPacked == 1;
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06001FDD RID: 8157 RVA: 0x000F2574 File Offset: 0x000F0774
	public Texture2D texture
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.texture;
			}
			Material material = this.material;
			return (!(material != null)) ? null : (material.mainTexture as Texture2D);
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06001FDE RID: 8158 RVA: 0x000F25C4 File Offset: 0x000F07C4
	// (set) Token: 0x06001FDF RID: 8159 RVA: 0x000F2630 File Offset: 0x000F0830
	public Rect uvRect
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.uvRect;
			}
			return (!(this.mAtlas != null) || this.sprite == null) ? new Rect(0f, 0f, 1f, 1f) : this.mUVRect;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.uvRect = value;
			}
			else if (this.sprite == null && this.mUVRect != value)
			{
				this.mUVRect = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06001FE0 RID: 8160 RVA: 0x00015382 File Offset: 0x00013582
	// (set) Token: 0x06001FE1 RID: 8161 RVA: 0x000F2688 File Offset: 0x000F0888
	public string spriteName
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mFont.spriteName : this.mReplacement.spriteName;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteName = value;
			}
			else if (this.mFont.spriteName != value)
			{
				this.mFont.spriteName = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x06001FE2 RID: 8162 RVA: 0x000153B0 File Offset: 0x000135B0
	public bool isValid
	{
		get
		{
			return this.mDynamicFont != null || this.mFont.isValid;
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06001FE3 RID: 8163 RVA: 0x000153D1 File Offset: 0x000135D1
	// (set) Token: 0x06001FE4 RID: 8164 RVA: 0x000153D9 File Offset: 0x000135D9
	[Obsolete("Use UIFont.defaultSize instead")]
	public int size
	{
		get
		{
			return this.defaultSize;
		}
		set
		{
			this.defaultSize = value;
		}
	}

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x000F26E0 File Offset: 0x000F08E0
	// (set) Token: 0x06001FE6 RID: 8166 RVA: 0x000153E2 File Offset: 0x000135E2
	public int defaultSize
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.defaultSize;
			}
			if (this.isDynamic || this.mFont == null)
			{
				return this.mDynamicFontSize;
			}
			return this.mFont.charSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.defaultSize = value;
			}
			else
			{
				this.mDynamicFontSize = value;
			}
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06001FE7 RID: 8167 RVA: 0x000F2734 File Offset: 0x000F0934
	public UISpriteData sprite
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.sprite;
			}
			if (this.mSprite == null && this.mAtlas != null && !string.IsNullOrEmpty(this.mFont.spriteName))
			{
				this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
				if (this.mSprite == null)
				{
					this.mSprite = this.mAtlas.GetSprite(base.name);
				}
				if (this.mSprite == null)
				{
					this.mFont.spriteName = null;
				}
				else
				{
					this.UpdateUVRect();
				}
				int i = 0;
				int count = this.mSymbols.Count;
				while (i < count)
				{
					this.symbols[i].MarkAsChanged();
					i++;
				}
			}
			return this.mSprite;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06001FE8 RID: 8168 RVA: 0x0001540D File Offset: 0x0001360D
	// (set) Token: 0x06001FE9 RID: 8169 RVA: 0x000F2824 File Offset: 0x000F0A24
	public UIFont replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIFont uifont = value;
			if (uifont == this)
			{
				uifont = null;
			}
			if (this.mReplacement != uifont)
			{
				if (uifont != null && uifont.replacement == this)
				{
					uifont.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uifont;
				if (uifont != null)
				{
					this.mPMA = -1;
					this.mMat = null;
					this.mFont = null;
					this.mDynamicFont = null;
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06001FEA RID: 8170 RVA: 0x00015415 File Offset: 0x00013615
	public bool isDynamic
	{
		get
		{
			return (!(this.mReplacement != null)) ? (this.mDynamicFont != null) : this.mReplacement.isDynamic;
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06001FEB RID: 8171 RVA: 0x00015444 File Offset: 0x00013644
	// (set) Token: 0x06001FEC RID: 8172 RVA: 0x000F28C4 File Offset: 0x000F0AC4
	public Font dynamicFont
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mDynamicFont : this.mReplacement.dynamicFont;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFont = value;
			}
			else if (this.mDynamicFont != value)
			{
				if (this.mDynamicFont != null)
				{
					this.material = null;
				}
				this.mDynamicFont = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06001FED RID: 8173 RVA: 0x0001546D File Offset: 0x0001366D
	// (set) Token: 0x06001FEE RID: 8174 RVA: 0x00015496 File Offset: 0x00013696
	public FontStyle dynamicFontStyle
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mDynamicFontStyle : this.mReplacement.dynamicFontStyle;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.dynamicFontStyle = value;
			}
			else if (this.mDynamicFontStyle != value)
			{
				this.mDynamicFontStyle = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x06001FEF RID: 8175 RVA: 0x000F292C File Offset: 0x000F0B2C
	private void Trim()
	{
		Texture texture = this.mAtlas.texture;
		if (texture != null && this.mSprite != null)
		{
			Rect rect = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, true);
			Rect rect2;
			rect2..ctor((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
			int xMin = Mathf.RoundToInt(rect2.xMin - rect.xMin);
			int yMin = Mathf.RoundToInt(rect2.yMin - rect.yMin);
			int xMax = Mathf.RoundToInt(rect2.xMax - rect.xMin);
			int yMax = Mathf.RoundToInt(rect2.yMax - rect.yMin);
			this.mFont.Trim(xMin, yMin, xMax, yMax);
		}
	}

	// Token: 0x06001FF0 RID: 8176 RVA: 0x000F2A20 File Offset: 0x000F0C20
	private bool References(UIFont font)
	{
		return !(font == null) && (font == this || (this.mReplacement != null && this.mReplacement.References(font)));
	}

	// Token: 0x06001FF1 RID: 8177 RVA: 0x000F2A6C File Offset: 0x000F0C6C
	public static bool CheckIfRelated(UIFont a, UIFont b)
	{
		return !(a == null) && !(b == null) && ((a.isDynamic && b.isDynamic && a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0]) || a == b || a.References(b) || b.References(a));
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06001FF2 RID: 8178 RVA: 0x000154D3 File Offset: 0x000136D3
	private Texture dynamicTexture
	{
		get
		{
			if (this.mReplacement)
			{
				return this.mReplacement.dynamicTexture;
			}
			if (this.isDynamic)
			{
				return this.mDynamicFont.material.mainTexture;
			}
			return null;
		}
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x000F2AF4 File Offset: 0x000F0CF4
	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		this.mSprite = null;
		UILabel[] array = NGUITools.FindActive<UILabel>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			UILabel uilabel = array[i];
			if (uilabel.enabled && NGUITools.GetActive(uilabel.gameObject) && UIFont.CheckIfRelated(this, uilabel.bitmapFont))
			{
				UIFont bitmapFont = uilabel.bitmapFont;
				uilabel.bitmapFont = null;
				uilabel.bitmapFont = bitmapFont;
			}
			i++;
		}
		int j = 0;
		int count = this.symbols.Count;
		while (j < count)
		{
			this.symbols[j].MarkAsChanged();
			j++;
		}
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x000F2BC0 File Offset: 0x000F0DC0
	public void UpdateUVRect()
	{
		if (this.mAtlas == null)
		{
			return;
		}
		Texture texture = this.mAtlas.texture;
		if (texture != null)
		{
			this.mUVRect = new Rect((float)(this.mSprite.x - this.mSprite.paddingLeft), (float)(this.mSprite.y - this.mSprite.paddingTop), (float)(this.mSprite.width + this.mSprite.paddingLeft + this.mSprite.paddingRight), (float)(this.mSprite.height + this.mSprite.paddingTop + this.mSprite.paddingBottom));
			this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
			if (this.mSprite.hasPadding)
			{
				this.Trim();
			}
		}
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x000F2CB0 File Offset: 0x000F0EB0
	private BMSymbol GetSymbol(string sequence, bool createIfMissing)
	{
		int i = 0;
		int count = this.mSymbols.Count;
		while (i < count)
		{
			BMSymbol bmsymbol = this.mSymbols[i];
			if (bmsymbol.sequence == sequence)
			{
				return bmsymbol;
			}
			i++;
		}
		if (createIfMissing)
		{
			BMSymbol bmsymbol2 = new BMSymbol();
			bmsymbol2.sequence = sequence;
			this.mSymbols.Add(bmsymbol2);
			return bmsymbol2;
		}
		return null;
	}

	// Token: 0x06001FF6 RID: 8182 RVA: 0x000F2D20 File Offset: 0x000F0F20
	public BMSymbol MatchSymbol(string text, int offset, int textLength)
	{
		int count = this.mSymbols.Count;
		if (count == 0)
		{
			return null;
		}
		textLength -= offset;
		for (int i = 0; i < count; i++)
		{
			BMSymbol bmsymbol = this.mSymbols[i];
			int length = bmsymbol.length;
			if (length != 0 && textLength >= length)
			{
				bool flag = true;
				for (int j = 0; j < length; j++)
				{
					if (text.get_Chars(offset + j) != bmsymbol.sequence.get_Chars(j))
					{
						flag = false;
						break;
					}
				}
				if (flag && bmsymbol.Validate(this.atlas))
				{
					return bmsymbol;
				}
			}
		}
		return null;
	}

	// Token: 0x06001FF7 RID: 8183 RVA: 0x000F2DD8 File Offset: 0x000F0FD8
	public void AddSymbol(string sequence, string spriteName)
	{
		BMSymbol symbol = this.GetSymbol(sequence, true);
		symbol.spriteName = spriteName;
		this.MarkAsChanged();
	}

	// Token: 0x06001FF8 RID: 8184 RVA: 0x000F2DFC File Offset: 0x000F0FFC
	public void RemoveSymbol(string sequence)
	{
		BMSymbol symbol = this.GetSymbol(sequence, false);
		if (symbol != null)
		{
			this.symbols.Remove(symbol);
		}
		this.MarkAsChanged();
	}

	// Token: 0x06001FF9 RID: 8185 RVA: 0x000F2E2C File Offset: 0x000F102C
	public void RenameSymbol(string before, string after)
	{
		BMSymbol symbol = this.GetSymbol(before, false);
		if (symbol != null)
		{
			symbol.sequence = after;
		}
		this.MarkAsChanged();
	}

	// Token: 0x06001FFA RID: 8186 RVA: 0x000F2E58 File Offset: 0x000F1058
	public bool UsesSprite(string s)
	{
		if (!string.IsNullOrEmpty(s))
		{
			if (s.Equals(this.spriteName))
			{
				return true;
			}
			int i = 0;
			int count = this.symbols.Count;
			while (i < count)
			{
				BMSymbol bmsymbol = this.symbols[i];
				if (s.Equals(bmsymbol.spriteName))
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x04002375 RID: 9077
	[SerializeField]
	[HideInInspector]
	private Material mMat;

	// Token: 0x04002376 RID: 9078
	[HideInInspector]
	[SerializeField]
	private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x04002377 RID: 9079
	[SerializeField]
	[HideInInspector]
	private BMFont mFont = new BMFont();

	// Token: 0x04002378 RID: 9080
	[SerializeField]
	[HideInInspector]
	private UIAtlas mAtlas;

	// Token: 0x04002379 RID: 9081
	[SerializeField]
	[HideInInspector]
	private UIFont mReplacement;

	// Token: 0x0400237A RID: 9082
	[SerializeField]
	[HideInInspector]
	private List<BMSymbol> mSymbols = new List<BMSymbol>();

	// Token: 0x0400237B RID: 9083
	[SerializeField]
	[HideInInspector]
	private Font mDynamicFont;

	// Token: 0x0400237C RID: 9084
	[HideInInspector]
	[SerializeField]
	private int mDynamicFontSize = 16;

	// Token: 0x0400237D RID: 9085
	[SerializeField]
	[HideInInspector]
	private FontStyle mDynamicFontStyle;

	// Token: 0x0400237E RID: 9086
	[NonSerialized]
	private UISpriteData mSprite;

	// Token: 0x0400237F RID: 9087
	private int mPMA = -1;

	// Token: 0x04002380 RID: 9088
	private int mPacked = -1;
}
