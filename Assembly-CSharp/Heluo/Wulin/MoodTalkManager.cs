using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000227 RID: 551
	public class MoodTalkManager : TextDataManager
	{
		// Token: 0x06000A7C RID: 2684 RVA: 0x000085AD File Offset: 0x000067AD
		public MoodTalkManager()
		{
			this.m_MoodTalkGroupeList = new List<MoodTalkGroup>();
			this.m_LoadFileName = "MoodTalk";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x000085E9 File Offset: 0x000067E9
		public static MoodTalkManager Singleton
		{
			get
			{
				return MoodTalkManager.instance;
			}
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x000586FC File Offset: 0x000568FC
		protected override void LoadFile(string filePath)
		{
			this.m_MoodTalkGroupeList.Clear();
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
							MoodTalkGroup npcRandomGroup = this.GetNpcRandomGroup(array3[0]);
							MoodTalkNode moodTalkNode = new MoodTalkNode();
							moodTalkNode.m_strString = array3[1];
							float fDestroyTime = 0f;
							if (!float.TryParse(array3[2], ref fDestroyTime))
							{
								fDestroyTime = 0f;
							}
							moodTalkNode.m_fDestroyTime = fDestroyTime;
							npcRandomGroup.m_MoodTalkNodeList.Add(moodTalkNode);
							this.m_MoodTalkGroupeList.Add(npcRandomGroup);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00058810 File Offset: 0x00056A10
		private MoodTalkGroup GetNpcRandomGroup(string strID)
		{
			MoodTalkGroup moodTalkGroup;
			if (this.m_MoodTalkGroupeList.Count == 0)
			{
				moodTalkGroup = new MoodTalkGroup();
				moodTalkGroup.m_strMoodID = strID;
				this.m_MoodTalkGroupeList.Add(moodTalkGroup);
				return moodTalkGroup;
			}
			for (int i = 0; i < this.m_MoodTalkGroupeList.Count; i++)
			{
				if (this.m_MoodTalkGroupeList[i].m_strMoodID.Equals(strID))
				{
					return this.m_MoodTalkGroupeList[i];
				}
			}
			moodTalkGroup = new MoodTalkGroup();
			moodTalkGroup.m_strMoodID = strID;
			this.m_MoodTalkGroupeList.Add(moodTalkGroup);
			return moodTalkGroup;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x000588A8 File Offset: 0x00056AA8
		public MoodTalkGroup GetMoodTalkGroup(string strID)
		{
			for (int i = 0; i < this.m_MoodTalkGroupeList.Count; i++)
			{
				if (this.m_MoodTalkGroupeList[i].m_strMoodID == strID)
				{
					return this.m_MoodTalkGroupeList[i];
				}
			}
			return null;
		}

		// Token: 0x04000B74 RID: 2932
		private static readonly MoodTalkManager instance = new MoodTalkManager();

		// Token: 0x04000B75 RID: 2933
		private List<MoodTalkGroup> m_MoodTalkGroupeList;
	}
}
