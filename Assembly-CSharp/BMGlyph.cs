using System;
using System.Collections.Generic;

// Token: 0x02000493 RID: 1171
[Serializable]
public class BMGlyph
{
	// Token: 0x06001C35 RID: 7221 RVA: 0x000DC54C File Offset: 0x000DA74C
	public int GetKerning(int previousChar)
	{
		if (this.kerning != null && previousChar != 0)
		{
			int i = 0;
			int count = this.kerning.Count;
			while (i < count)
			{
				if (this.kerning[i] == previousChar)
				{
					return this.kerning[i + 1];
				}
				i += 2;
			}
		}
		return 0;
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x000DC5AC File Offset: 0x000DA7AC
	public void SetKerning(int previousChar, int amount)
	{
		if (this.kerning == null)
		{
			this.kerning = new List<int>();
		}
		for (int i = 0; i < this.kerning.Count; i += 2)
		{
			if (this.kerning[i] == previousChar)
			{
				this.kerning[i + 1] = amount;
				return;
			}
		}
		this.kerning.Add(previousChar);
		this.kerning.Add(amount);
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x000DC628 File Offset: 0x000DA828
	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		int num = this.x + this.width;
		int num2 = this.y + this.height;
		if (this.x < xMin)
		{
			int num3 = xMin - this.x;
			this.x += num3;
			this.width -= num3;
			this.offsetX += num3;
		}
		if (this.y < yMin)
		{
			int num4 = yMin - this.y;
			this.y += num4;
			this.height -= num4;
			this.offsetY += num4;
		}
		if (num > xMax)
		{
			this.width -= num - xMax;
		}
		if (num2 > yMax)
		{
			this.height -= num2 - yMax;
		}
	}

	// Token: 0x04002121 RID: 8481
	public int index;

	// Token: 0x04002122 RID: 8482
	public int x;

	// Token: 0x04002123 RID: 8483
	public int y;

	// Token: 0x04002124 RID: 8484
	public int width;

	// Token: 0x04002125 RID: 8485
	public int height;

	// Token: 0x04002126 RID: 8486
	public int offsetX;

	// Token: 0x04002127 RID: 8487
	public int offsetY;

	// Token: 0x04002128 RID: 8488
	public int advance;

	// Token: 0x04002129 RID: 8489
	public int channel;

	// Token: 0x0400212A RID: 8490
	public List<int> kerning;
}
