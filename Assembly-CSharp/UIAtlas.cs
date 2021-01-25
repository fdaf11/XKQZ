using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004DD RID: 1245
[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x06001F52 RID: 8018 RVA: 0x00014ED8 File Offset: 0x000130D8
	// (set) Token: 0x06001F53 RID: 8019 RVA: 0x000EEAA8 File Offset: 0x000ECCA8
	public Material spriteMaterial
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.material : this.mReplacement.spriteMaterial;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteMaterial = value;
			}
			else if (this.material == null)
			{
				this.mPMA = 0;
				this.material = value;
			}
			else
			{
				this.MarkAsChanged();
				this.mPMA = -1;
				this.material = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06001F54 RID: 8020 RVA: 0x000EEB18 File Offset: 0x000ECD18
	public bool premultipliedAlpha
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material spriteMaterial = this.spriteMaterial;
				this.mPMA = ((!(spriteMaterial != null) || !(spriteMaterial.shader != null) || !spriteMaterial.shader.name.Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x170002CA RID: 714
	// (get) Token: 0x06001F55 RID: 8021 RVA: 0x00014F01 File Offset: 0x00013101
	// (set) Token: 0x06001F56 RID: 8022 RVA: 0x00014F3D File Offset: 0x0001313D
	public List<UISpriteData> spriteList
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.spriteList;
			}
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			return this.mSprites;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteList = value;
			}
			else
			{
				this.mSprites = value;
			}
		}
	}

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06001F57 RID: 8023 RVA: 0x000EEBA4 File Offset: 0x000ECDA4
	public Texture texture
	{
		get
		{
			return (!(this.mReplacement != null)) ? ((!(this.material != null)) ? null : this.material.mainTexture) : this.mReplacement.texture;
		}
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06001F58 RID: 8024 RVA: 0x00014F68 File Offset: 0x00013168
	// (set) Token: 0x06001F59 RID: 8025 RVA: 0x000EEBF4 File Offset: 0x000ECDF4
	public float pixelSize
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mPixelSize : this.mReplacement.pixelSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.pixelSize = value;
			}
			else
			{
				float num = Mathf.Clamp(value, 0.25f, 4f);
				if (this.mPixelSize != num)
				{
					this.mPixelSize = num;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06001F5A RID: 8026 RVA: 0x00014F91 File Offset: 0x00013191
	// (set) Token: 0x06001F5B RID: 8027 RVA: 0x000EEC50 File Offset: 0x000ECE50
	public UIAtlas replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIAtlas uiatlas = value;
			if (uiatlas == this)
			{
				uiatlas = null;
			}
			if (this.mReplacement != uiatlas)
			{
				if (uiatlas != null && uiatlas.replacement == this)
				{
					uiatlas.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uiatlas;
				if (uiatlas != null)
				{
					this.material = null;
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x06001F5C RID: 8028 RVA: 0x000EECDC File Offset: 0x000ECEDC
	public UISpriteData GetSprite(string name)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetSprite(name);
		}
		if (!string.IsNullOrEmpty(name))
		{
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			if (this.mSprites.Count == 0)
			{
				return null;
			}
			if (this.mSpriteIndices.Count != this.mSprites.Count)
			{
				this.MarkSpriteListAsChanged();
			}
			int num;
			if (this.mSpriteIndices.TryGetValue(name, ref num))
			{
				if (num > -1 && num < this.mSprites.Count)
				{
					return this.mSprites[num];
				}
				this.MarkSpriteListAsChanged();
				return (!this.mSpriteIndices.TryGetValue(name, ref num)) ? null : this.mSprites[num];
			}
			else
			{
				int i = 0;
				int count = this.mSprites.Count;
				while (i < count)
				{
					UISpriteData uispriteData = this.mSprites[i];
					if (!string.IsNullOrEmpty(uispriteData.name) && name == uispriteData.name)
					{
						this.MarkSpriteListAsChanged();
						return uispriteData;
					}
					i++;
				}
			}
		}
		return null;
	}

	// Token: 0x06001F5D RID: 8029 RVA: 0x000EEE18 File Offset: 0x000ED018
	public void MarkSpriteListAsChanged()
	{
		this.mSpriteIndices.Clear();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			this.mSpriteIndices[this.mSprites[i].name] = i;
			i++;
		}
	}

	// Token: 0x06001F5E RID: 8030 RVA: 0x00014F99 File Offset: 0x00013199
	public void SortAlphabetically()
	{
		this.mSprites.Sort((UISpriteData s1, UISpriteData s2) => s1.name.CompareTo(s2.name));
	}

	// Token: 0x06001F5F RID: 8031 RVA: 0x000EEE6C File Offset: 0x000ED06C
	public BetterList<string> GetListOfSprites()
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			UISpriteData uispriteData = this.mSprites[i];
			if (uispriteData != null && !string.IsNullOrEmpty(uispriteData.name))
			{
				betterList.Add(uispriteData.name);
			}
			i++;
		}
		return betterList;
	}

	// Token: 0x06001F60 RID: 8032 RVA: 0x000EEF04 File Offset: 0x000ED104
	public BetterList<string> GetListOfSprites(string match)
	{
		if (this.mReplacement)
		{
			return this.mReplacement.GetListOfSprites(match);
		}
		if (string.IsNullOrEmpty(match))
		{
			return this.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			UISpriteData uispriteData = this.mSprites[i];
			if (uispriteData != null && !string.IsNullOrEmpty(uispriteData.name) && string.Equals(match, uispriteData.name, 5))
			{
				betterList.Add(uispriteData.name);
				return betterList;
			}
			i++;
		}
		string[] array = match.Split(new char[]
		{
			' '
		}, 1);
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = array[j].ToLower();
		}
		int k = 0;
		int count2 = this.mSprites.Count;
		while (k < count2)
		{
			UISpriteData uispriteData2 = this.mSprites[k];
			if (uispriteData2 != null && !string.IsNullOrEmpty(uispriteData2.name))
			{
				string text = uispriteData2.name.ToLower();
				int num = 0;
				for (int l = 0; l < array.Length; l++)
				{
					if (text.Contains(array[l]))
					{
						num++;
					}
				}
				if (num == array.Length)
				{
					betterList.Add(uispriteData2.name);
				}
			}
			k++;
		}
		return betterList;
	}

	// Token: 0x06001F61 RID: 8033 RVA: 0x000EF0A0 File Offset: 0x000ED2A0
	private bool References(UIAtlas atlas)
	{
		return !(atlas == null) && (atlas == this || (this.mReplacement != null && this.mReplacement.References(atlas)));
	}

	// Token: 0x06001F62 RID: 8034 RVA: 0x00014FC3 File Offset: 0x000131C3
	public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
	{
		return !(a == null) && !(b == null) && (a == b || a.References(b) || b.References(a));
	}

	// Token: 0x06001F63 RID: 8035 RVA: 0x000EF0EC File Offset: 0x000ED2EC
	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		UISprite[] array = NGUITools.FindActive<UISprite>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			UISprite uisprite = array[i];
			if (UIAtlas.CheckIfRelated(this, uisprite.atlas))
			{
				UIAtlas atlas = uisprite.atlas;
				uisprite.atlas = null;
				uisprite.atlas = atlas;
			}
			i++;
		}
		UIFont[] array2 = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
		int j = 0;
		int num2 = array2.Length;
		while (j < num2)
		{
			UIFont uifont = array2[j];
			if (UIAtlas.CheckIfRelated(this, uifont.atlas))
			{
				UIAtlas atlas2 = uifont.atlas;
				uifont.atlas = null;
				uifont.atlas = atlas2;
			}
			j++;
		}
		UILabel[] array3 = NGUITools.FindActive<UILabel>();
		int k = 0;
		int num3 = array3.Length;
		while (k < num3)
		{
			UILabel uilabel = array3[k];
			if (uilabel.bitmapFont != null && UIAtlas.CheckIfRelated(this, uilabel.bitmapFont.atlas))
			{
				UIFont bitmapFont = uilabel.bitmapFont;
				uilabel.bitmapFont = null;
				uilabel.bitmapFont = bitmapFont;
			}
			k++;
		}
	}

	// Token: 0x06001F64 RID: 8036 RVA: 0x000EF234 File Offset: 0x000ED434
	private bool Upgrade()
	{
		if (this.mReplacement)
		{
			return this.mReplacement.Upgrade();
		}
		if (this.mSprites.Count == 0 && this.sprites.Count > 0 && this.material)
		{
			Texture mainTexture = this.material.mainTexture;
			int width = (!(mainTexture != null)) ? 512 : mainTexture.width;
			int height = (!(mainTexture != null)) ? 512 : mainTexture.height;
			for (int i = 0; i < this.sprites.Count; i++)
			{
				UIAtlas.Sprite sprite = this.sprites[i];
				Rect outer = sprite.outer;
				Rect inner = sprite.inner;
				if (this.mCoordinates == UIAtlas.Coordinates.TexCoords)
				{
					NGUIMath.ConvertToPixels(outer, width, height, true);
					NGUIMath.ConvertToPixels(inner, width, height, true);
				}
				UISpriteData uispriteData = new UISpriteData();
				uispriteData.name = sprite.name;
				uispriteData.x = Mathf.RoundToInt(outer.xMin);
				uispriteData.y = Mathf.RoundToInt(outer.yMin);
				uispriteData.width = Mathf.RoundToInt(outer.width);
				uispriteData.height = Mathf.RoundToInt(outer.height);
				uispriteData.paddingLeft = Mathf.RoundToInt(sprite.paddingLeft * outer.width);
				uispriteData.paddingRight = Mathf.RoundToInt(sprite.paddingRight * outer.width);
				uispriteData.paddingBottom = Mathf.RoundToInt(sprite.paddingBottom * outer.height);
				uispriteData.paddingTop = Mathf.RoundToInt(sprite.paddingTop * outer.height);
				uispriteData.borderLeft = Mathf.RoundToInt(inner.xMin - outer.xMin);
				uispriteData.borderRight = Mathf.RoundToInt(outer.xMax - inner.xMax);
				uispriteData.borderBottom = Mathf.RoundToInt(outer.yMax - inner.yMax);
				uispriteData.borderTop = Mathf.RoundToInt(inner.yMin - outer.yMin);
				this.mSprites.Add(uispriteData);
			}
			this.sprites.Clear();
			return true;
		}
		return false;
	}

	// Token: 0x040022EE RID: 8942
	[SerializeField]
	[HideInInspector]
	private Material material;

	// Token: 0x040022EF RID: 8943
	[HideInInspector]
	[SerializeField]
	private List<UISpriteData> mSprites = new List<UISpriteData>();

	// Token: 0x040022F0 RID: 8944
	[HideInInspector]
	[SerializeField]
	private float mPixelSize = 1f;

	// Token: 0x040022F1 RID: 8945
	[HideInInspector]
	[SerializeField]
	private UIAtlas mReplacement;

	// Token: 0x040022F2 RID: 8946
	[SerializeField]
	[HideInInspector]
	private UIAtlas.Coordinates mCoordinates;

	// Token: 0x040022F3 RID: 8947
	[HideInInspector]
	[SerializeField]
	private List<UIAtlas.Sprite> sprites = new List<UIAtlas.Sprite>();

	// Token: 0x040022F4 RID: 8948
	private int mPMA = -1;

	// Token: 0x040022F5 RID: 8949
	private Dictionary<string, int> mSpriteIndices = new Dictionary<string, int>();

	// Token: 0x020004DE RID: 1246
	[Serializable]
	private class Sprite
	{
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06001F67 RID: 8039 RVA: 0x000EF4E4 File Offset: 0x000ED6E4
		public bool hasPadding
		{
			get
			{
				return this.paddingLeft != 0f || this.paddingRight != 0f || this.paddingTop != 0f || this.paddingBottom != 0f;
			}
		}

		// Token: 0x040022F7 RID: 8951
		public string name = "Unity Bug";

		// Token: 0x040022F8 RID: 8952
		public Rect outer = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x040022F9 RID: 8953
		public Rect inner = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x040022FA RID: 8954
		public bool rotated;

		// Token: 0x040022FB RID: 8955
		public float paddingLeft;

		// Token: 0x040022FC RID: 8956
		public float paddingRight;

		// Token: 0x040022FD RID: 8957
		public float paddingTop;

		// Token: 0x040022FE RID: 8958
		public float paddingBottom;
	}

	// Token: 0x020004DF RID: 1247
	private enum Coordinates
	{
		// Token: 0x04002300 RID: 8960
		Pixels,
		// Token: 0x04002301 RID: 8961
		TexCoords
	}
}
