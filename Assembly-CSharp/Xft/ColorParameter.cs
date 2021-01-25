using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200055C RID: 1372
	[Serializable]
	public class ColorParameter
	{
		// Token: 0x0600227C RID: 8828 RVA: 0x0001705D File Offset: 0x0001525D
		public ColorParameter()
		{
			this.Colors = new List<ColorKey>();
			this.AddColorKey(0f, Color.white);
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x0010E038 File Offset: 0x0010C238
		public Color GetGradientColor(float t)
		{
			if (this.Colors.Count == 1)
			{
				return this.Colors[0].Color;
			}
			if (this.Colors.Count == 0)
			{
				return Color.black;
			}
			for (int i = 1; i < this.Colors.Count; i++)
			{
				if (t <= this.Colors[i].t)
				{
					int num = i - 1;
					return Color.Lerp(this.Colors[num].Color, this.Colors[i].Color, (t - this.Colors[num].t) / (this.Colors[i].t - this.Colors[num].t));
				}
			}
			return this.Colors[this.Colors.Count - 1].Color;
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x0010E134 File Offset: 0x0010C334
		public ColorKey AddColorKey(float t, Color color)
		{
			ColorKey colorKey = new ColorKey(t, color);
			this.Colors.Add(colorKey);
			this.Colors.Sort();
			return colorKey;
		}

		// Token: 0x040028DA RID: 10458
		public List<ColorKey> Colors;
	}
}
