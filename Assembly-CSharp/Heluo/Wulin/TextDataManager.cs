using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000264 RID: 612
	public class TextDataManager
	{
		// Token: 0x06000B3D RID: 2877 RVA: 0x00008D96 File Offset: 0x00006F96
		public static void AddTextDataToList(TextDataManager tdm)
		{
			if (!TextDataManager.m_TextDataManagerList.Contains(tdm))
			{
				TextDataManager.m_TextDataManagerList.Add(tdm);
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00008DB3 File Offset: 0x00006FB3
		public static void AddDLCTextDataToList(TextDataManager tdm)
		{
			if (!TextDataManager.m_DLCTextDataManagerList.Contains(tdm))
			{
				TextDataManager.m_DLCTextDataManagerList.Add(tdm);
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0005D078 File Offset: 0x0005B278
		public static void ReLoadAllTextData()
		{
			for (int i = 0; i < TextDataManager.m_TextDataManagerList.Count; i++)
			{
				if (TextDataManager.m_TextDataManagerList[i] != null)
				{
					TextDataManager.m_TextDataManagerList[i].ReLoadTextData();
				}
			}
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0005D0C0 File Offset: 0x0005B2C0
		public static void ReLoadAllDLCTextData()
		{
			for (int i = 0; i < TextDataManager.m_DLCTextDataManagerList.Count; i++)
			{
				if (TextDataManager.m_DLCTextDataManagerList[i] != null)
				{
					TextDataManager.m_DLCTextDataManagerList[i].ReLoadDLCTextData();
				}
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0005D108 File Offset: 0x0005B308
		public void ReLoadDLCTextData()
		{
			string filePath = "DLC_" + this.m_LoadFileName;
			this.LoadFile(filePath);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00008DD0 File Offset: 0x00006FD0
		public void ReLoadTextData()
		{
			this.LoadFile(this.m_LoadFileName);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void LoadFile(string filePath)
		{
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0005D130 File Offset: 0x0005B330
		protected int PraseInt(string text)
		{
			int result = 0;
			if (!int.TryParse(text, ref result))
			{
				result = 1;
			}
			return result;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x0005D150 File Offset: 0x0005B350
		protected float PraseFloat(string text)
		{
			float result = 0f;
			if (!float.TryParse(text, ref result))
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00008DDE File Offset: 0x00006FDE
		protected bool PraseBool(string text)
		{
			return !(text == "0");
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0005D178 File Offset: 0x0005B378
		protected string[] ExtractTextFile(string fileName)
		{
			string text = string.Concat(new string[]
			{
				Game.g_strDataPathToApplicationPath,
				"Mods/",
				GameGlobal.m_strVersion,
				"/Config/TextFiles/",
				fileName,
				".txt"
			});
			if (File.Exists(text))
			{
				try
				{
					Stream stream = File.OpenRead(text);
					StreamReader streamReader;
					try
					{
						streamReader = new StreamReader(stream, Encoding.Unicode);
					}
					catch (Exception ex)
					{
						Debug.LogError(fileName + "Exception : " + ex.Message);
						return null;
					}
					string text2 = string.Empty;
					text2 = streamReader.ReadToEnd();
					string[] result = text2.Split(new char[]
					{
						"\n".get_Chars(0)
					});
					if (GameGlobal.m_iModFixFileCount <= 0)
					{
						Debug.Log("Mod active");
					}
					GameGlobal.m_iModFixFileCount++;
					return result;
				}
				catch
				{
					Debug.LogError("散檔讀取失敗 !! ( " + text + " )");
					return null;
				}
			}
			if (Game.g_TextFiles == null)
			{
				Debug.LogWarning("包檔讀取失敗 !! ( " + fileName + " ) 未包檔");
				return null;
			}
			if (!Game.g_TextFiles.Contains("textfiles/" + fileName))
			{
				Debug.LogWarning("包檔讀取失敗 !! ( " + fileName + " ) 不存在");
				return null;
			}
			TextAsset textAsset = Game.g_TextFiles.Load("textfiles/" + fileName) as TextAsset;
			if (textAsset)
			{
				return textAsset.text.Split(new char[]
				{
					"\n".get_Chars(0)
				});
			}
			Debug.LogError("包檔讀取失敗 !! ( " + fileName + " )");
			return null;
		}

		// Token: 0x04000D11 RID: 3345
		public static List<TextDataManager> m_TextDataManagerList = new List<TextDataManager>();

		// Token: 0x04000D12 RID: 3346
		public static List<TextDataManager> m_DLCTextDataManagerList = new List<TextDataManager>();

		// Token: 0x04000D13 RID: 3347
		protected string m_LoadFileName;
	}
}
