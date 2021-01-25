using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000392 RID: 914
	[RequireComponent(typeof(BoxCollider))]
	public class SceneObejctEvent : PlayerTarget
	{
		// Token: 0x06001533 RID: 5427 RVA: 0x0000D7C0 File Offset: 0x0000B9C0
		private void Awake()
		{
			this.m_TargetType = PlayerTarget.eTargetType.MouseEventCube;
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x000B5F98 File Offset: 0x000B4198
		private void Start()
		{
			base.gameObject.tag = "SceneObject";
			base.gameObject.layer = LayerMask.NameToLayer("Item");
			if (this.m_MoodTalkID != null && this.m_MoodTalkID.Length != 0)
			{
				this.m_MoodTalkGroup = Game.MoodTalk.GetMoodTalkGroup(this.m_MoodTalkID);
				if (this.m_iMoodTalkCount == 0)
				{
					this.m_iMoodTalkCount = 1;
				}
			}
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x000B6010 File Offset: 0x000B4210
		[ContextMenu("SetMoodTalk")]
		public void SetMoodTalk()
		{
			base.gameObject.tag = "SceneObject";
			base.gameObject.layer = LayerMask.NameToLayer("Item");
			this.m_MoodTalkGroup = Game.MoodTalk.GetMoodTalkGroup(this.m_MoodTalkID);
			if (this.m_MoodTalkGroup != null)
			{
				Debug.Log("Mood OK " + this.m_MoodTalkGroup.m_MoodTalkNodeList.Count);
			}
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x000B6088 File Offset: 0x000B4288
		public void DoEnent()
		{
			if (this.m_MoodTalkGroup != null)
			{
				List<MoodTalkNode> moodTalkNodeList = this.m_MoodTalkGroup.m_MoodTalkNodeList;
				string strString = moodTalkNodeList[this.m_iMoodTalkIndex].m_strString;
				float fDestroyTime = Random.Range(moodTalkNodeList[this.m_iMoodTalkIndex].m_fDestroyTime * 0.8f, moodTalkNodeList[this.m_iMoodTalkIndex].m_fDestroyTime);
				if (this.m_StringOverlay == null)
				{
					this.m_StringOverlay = Game.UI.Root.GetComponentInChildren<UIStringOverlay>();
				}
				if (this.m_bPlayer)
				{
					if (this.m_goPlayer == null)
					{
						this.m_goPlayer = GameObject.FindWithTag("Player");
					}
					if (this.m_goPlayer == null)
					{
						return;
					}
					this.m_StringOverlay.AddOneLineString(this.m_goPlayer, strString, fDestroyTime);
				}
				else
				{
					this.m_StringOverlay.AddOneLineString(base.gameObject, strString, fDestroyTime);
				}
				if (this.m_bOrder)
				{
					this.m_iMoodTalkIndex++;
					this.m_iClickCount = 0;
					if (this.m_iMoodTalkIndex >= moodTalkNodeList.Count)
					{
						this.m_bOrder = false;
						this.m_iMoodTalkIndex = 0;
					}
				}
				else if (this.m_iClickCount >= this.m_iMoodTalkCount)
				{
					this.m_iClickCount = 0;
					this.m_iMoodTalkIndex = Random.Range(0, moodTalkNodeList.Count);
				}
				this.m_iClickCount++;
			}
		}

		// Token: 0x040019E9 RID: 6633
		private int m_iClickCount;

		// Token: 0x040019EA RID: 6634
		private int m_iMoodTalkIndex;

		// Token: 0x040019EB RID: 6635
		private GameObject m_goPlayer;

		// Token: 0x040019EC RID: 6636
		private bool m_bOrder = true;

		// Token: 0x040019ED RID: 6637
		private UIStringOverlay m_StringOverlay;

		// Token: 0x040019EE RID: 6638
		private MoodTalkGroup m_MoodTalkGroup;

		// Token: 0x040019EF RID: 6639
		[Header("MoodTalk表")]
		public string m_MoodTalkID = string.Empty;

		// Token: 0x040019F0 RID: 6640
		[Tooltip("講幾次換下一句")]
		public int m_iMoodTalkCount;

		// Token: 0x040019F1 RID: 6641
		public bool m_bPlayer;
	}
}
