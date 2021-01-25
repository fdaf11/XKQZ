using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000492 RID: 1170
[Serializable]
public class BMFont
{
	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06001C23 RID: 7203 RVA: 0x00012C0F File Offset: 0x00010E0F
	public bool isValid
	{
		get
		{
			return this.mSaved.Count > 0;
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06001C24 RID: 7204 RVA: 0x00012C1F File Offset: 0x00010E1F
	// (set) Token: 0x06001C25 RID: 7205 RVA: 0x00012C27 File Offset: 0x00010E27
	public int charSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			this.mSize = value;
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06001C26 RID: 7206 RVA: 0x00012C30 File Offset: 0x00010E30
	// (set) Token: 0x06001C27 RID: 7207 RVA: 0x00012C38 File Offset: 0x00010E38
	public int baseOffset
	{
		get
		{
			return this.mBase;
		}
		set
		{
			this.mBase = value;
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06001C28 RID: 7208 RVA: 0x00012C41 File Offset: 0x00010E41
	// (set) Token: 0x06001C29 RID: 7209 RVA: 0x00012C49 File Offset: 0x00010E49
	public int texWidth
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			this.mWidth = value;
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06001C2A RID: 7210 RVA: 0x00012C52 File Offset: 0x00010E52
	// (set) Token: 0x06001C2B RID: 7211 RVA: 0x00012C5A File Offset: 0x00010E5A
	public int texHeight
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			this.mHeight = value;
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06001C2C RID: 7212 RVA: 0x00012C63 File Offset: 0x00010E63
	public int glyphCount
	{
		get
		{
			return (!this.isValid) ? 0 : this.mSaved.Count;
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06001C2D RID: 7213 RVA: 0x00012C81 File Offset: 0x00010E81
	// (set) Token: 0x06001C2E RID: 7214 RVA: 0x00012C89 File Offset: 0x00010E89
	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			this.mSpriteName = value;
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06001C2F RID: 7215 RVA: 0x00012C92 File Offset: 0x00010E92
	public List<BMGlyph> glyphs
	{
		get
		{
			return this.mSaved;
		}
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x000DC45C File Offset: 0x000DA65C
	public BMGlyph GetGlyph(int index, bool createIfMissing)
	{
		BMGlyph bmglyph = null;
		if (this.mDict.Count == 0)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bmglyph2 = this.mSaved[i];
				this.mDict.Add(bmglyph2.index, bmglyph2);
				i++;
			}
		}
		if (!this.mDict.TryGetValue(index, ref bmglyph) && createIfMissing)
		{
			bmglyph = new BMGlyph();
			bmglyph.index = index;
			this.mSaved.Add(bmglyph);
			this.mDict.Add(index, bmglyph);
		}
		return bmglyph;
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x00012C9A File Offset: 0x00010E9A
	public BMGlyph GetGlyph(int index)
	{
		return this.GetGlyph(index, false);
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x00012CA4 File Offset: 0x00010EA4
	public void Clear()
	{
		this.mDict.Clear();
		this.mSaved.Clear();
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x000DC4F8 File Offset: 0x000DA6F8
	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		if (this.isValid)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bmglyph = this.mSaved[i];
				if (bmglyph != null)
				{
					bmglyph.Trim(xMin, yMin, xMax, yMax);
				}
				i++;
			}
		}
	}

	// Token: 0x0400211A RID: 8474
	[SerializeField]
	[HideInInspector]
	private int mSize = 16;

	// Token: 0x0400211B RID: 8475
	[SerializeField]
	[HideInInspector]
	private int mBase;

	// Token: 0x0400211C RID: 8476
	[SerializeField]
	[HideInInspector]
	private int mWidth;

	// Token: 0x0400211D RID: 8477
	[SerializeField]
	[HideInInspector]
	private int mHeight;

	// Token: 0x0400211E RID: 8478
	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	// Token: 0x0400211F RID: 8479
	[SerializeField]
	[HideInInspector]
	private List<BMGlyph> mSaved = new List<BMGlyph>();

	// Token: 0x04002120 RID: 8480
	private Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();
}
