using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000258 RID: 600
	public class SceneAudioManager : TextDataManager
	{
		// Token: 0x06000B08 RID: 2824 RVA: 0x00008BA8 File Offset: 0x00006DA8
		public SceneAudioManager()
		{
			this.m_SceneAudioNodeList = new List<SceneAudioNode>();
			this.m_LoadFileName = "ScenesAudio";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00008BE4 File Offset: 0x00006DE4
		public static SceneAudioManager Singleton
		{
			get
			{
				return SceneAudioManager.instance;
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0005C054 File Offset: 0x0005A254
		protected override void LoadFile(string filePath)
		{
			this.m_SceneAudioNodeList.Clear();
			string[] array = base.ExtractTextFile(filePath);
			if (array == null)
			{
				return;
			}
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (text.get_Chars(0) != '#')
					{
						try
						{
							string[] array3 = text.Split(new char[]
							{
								"\t".get_Chars(0)
							});
							SceneAudioNode sceneAudioNode = new SceneAudioNode();
							sceneAudioNode.m_strMainID = array3[0];
							sceneAudioNode.m_iMinorID = int.Parse(array3[1]);
							string text2 = array3[2];
							if (text2.get_Chars(text2.Length - 1) == '\r')
							{
								text2 = text2.Substring(0, text2.Length - 1);
							}
							sceneAudioNode.m_strSencesAudio = text2;
							this.m_SceneAudioNodeList.Add(sceneAudioNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0005C170 File Offset: 0x0005A370
		public string GetSencesAudioName(string strAreaName, int iAreaMinor)
		{
			foreach (SceneAudioNode sceneAudioNode in this.m_SceneAudioNodeList)
			{
				if (sceneAudioNode.m_strMainID == strAreaName && sceneAudioNode.m_iMinorID == iAreaMinor)
				{
					return sceneAudioNode.m_strSencesAudio;
				}
			}
			return null;
		}

		// Token: 0x04000C89 RID: 3209
		private static readonly SceneAudioManager instance = new SceneAudioManager();

		// Token: 0x04000C8A RID: 3210
		private List<SceneAudioNode> m_SceneAudioNodeList;
	}
}
