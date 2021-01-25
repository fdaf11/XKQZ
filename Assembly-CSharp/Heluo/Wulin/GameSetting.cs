using System;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000135 RID: 309
	public class GameSetting : MonoBehaviour
	{
		// Token: 0x06000639 RID: 1593 RVA: 0x000059D2 File Offset: 0x00003BD2
		private void Awake()
		{
			if (GameSetting.m_Instance == null)
			{
				GameSetting.m_Instance = this;
			}
			else
			{
				Debug.LogError(Application.loadedLevelName + " 地圖的 GameSetting 沒關");
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00044F74 File Offset: 0x00043174
		public void SetFont(Font nameFont, Font textFont)
		{
			string text = IniFile.IniReadValue("setting", "textfont");
			if (text != string.Empty)
			{
				string[] array = text.Split(new char[]
				{
					','
				});
				if (array.Length < 1)
				{
					if (this.m_TextFont != null)
					{
						this.m_TextFont.dynamicFont = textFont;
					}
				}
				else if (this.m_TextFont != null)
				{
					this.m_TextFont.dynamicFont.fontNames = array;
					foreach (string text2 in this.m_TextFont.dynamicFont.fontNames)
					{
						Debug.Log("TextFont = " + text2);
					}
				}
			}
			else if (this.m_TextFont != null)
			{
				this.m_TextFont.dynamicFont = textFont;
			}
			text = IniFile.IniReadValue("setting", "namefont");
			if (text != string.Empty)
			{
				string[] array = text.Split(new char[]
				{
					','
				});
				if (array.Length < 1)
				{
					if (this.m_NameFont != null)
					{
						this.m_NameFont.dynamicFont = nameFont;
					}
				}
				else if (this.m_NameFont != null)
				{
					this.m_NameFont.dynamicFont.fontNames = array;
					foreach (string text3 in this.m_NameFont.dynamicFont.fontNames)
					{
						Debug.Log("NameFont = " + text3);
					}
				}
			}
			else if (this.m_NameFont != null)
			{
				this.m_NameFont.dynamicFont = nameFont;
			}
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00045144 File Offset: 0x00043344
		public void SetChtFont()
		{
			string text = "wuxiaTextFont,Arial Unicode MS";
			string[] fontNames = text.Split(new char[]
			{
				','
			});
			if (this.m_TextFont != null)
			{
				this.m_TextFont.dynamicFont.fontNames = fontNames;
				foreach (string text2 in this.m_TextFont.dynamicFont.fontNames)
				{
					Debug.Log("TextFont = " + text2);
				}
			}
			text = "wuxiaTextFont,Arial Unicode MS";
			fontNames = text.Split(new char[]
			{
				','
			});
			if (this.m_NameFont != null)
			{
				this.m_NameFont.dynamicFont.fontNames = fontNames;
				foreach (string text3 in this.m_NameFont.dynamicFont.fontNames)
				{
					Debug.Log("NameFont = " + text3);
				}
			}
		}

		// Token: 0x040006EA RID: 1770
		public static GameSetting m_Instance;

		// Token: 0x040006EB RID: 1771
		private CursorMode m_CursorMode;

		// Token: 0x040006EC RID: 1772
		private Vector2 m_HotSpot = Vector2.zero;

		// Token: 0x040006ED RID: 1773
		public UIFont m_NameFont;

		// Token: 0x040006EE RID: 1774
		public UIFont m_TextFont;
	}
}
