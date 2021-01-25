using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200038B RID: 907
	public class MouseEventCube : PlayerTarget
	{
		// Token: 0x06001518 RID: 5400 RVA: 0x0000D6FF File Offset: 0x0000B8FF
		public void GetPath()
		{
			this.FindAttachObj();
			if (this.AttachObj == null)
			{
				Debug.Log("此滑鼠事件，的Attach物件，須檢查是否有同名物件!!");
				this.AttachObj = GameObject.Find(base.gameObject.name);
				this.GetPath(null);
			}
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x000B5678 File Offset: 0x000B3878
		public void GetPath(GameObject Obj)
		{
			if (Obj == null)
			{
				Obj = this.AttachObj;
			}
			GameObject gameObject = Obj;
			if (gameObject == null)
			{
				Debug.Log("此滑鼠事件，無Attach物件");
				return;
			}
			List<int> list = new List<int>();
			while (gameObject.transform.parent != null)
			{
				int siblingIndex = gameObject.transform.GetSiblingIndex();
				if (siblingIndex >= 0)
				{
					list.Add(siblingIndex);
				}
				gameObject = gameObject.transform.parent.gameObject;
			}
			list.Reverse();
			this.m_ChildIndexList = list.ToArray();
			this.m_ParentName = gameObject.name;
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x000B5720 File Offset: 0x000B3920
		public void FindAttachObj()
		{
			if (this.m_ParentName == null)
			{
				this.m_ParentName = base.gameObject.name;
			}
			if (!base.gameObject.name.Contains("MEC"))
			{
				base.gameObject.name = base.gameObject.name + "_MEC";
			}
			GameObject gameObject = GameObject.Find(this.m_ParentName);
			if (gameObject != null)
			{
				if (this.m_ChildIndexList != null)
				{
					for (int i = 0; i < this.m_ChildIndexList.Length; i++)
					{
						if (gameObject.transform.GetChild(this.m_ChildIndexList[i]) != null)
						{
							gameObject = gameObject.transform.GetChild(this.m_ChildIndexList[i]).gameObject;
						}
					}
				}
				this.AttachObj = gameObject;
			}
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x000B5800 File Offset: 0x000B3A00
		private void Update()
		{
			if (this.AttachObj != null)
			{
				base.gameObject.transform.position = this.AttachObj.transform.position;
				base.gameObject.transform.eulerAngles = this.AttachObj.transform.eulerAngles;
			}
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x000B5860 File Offset: 0x000B3A60
		private void Awake()
		{
			this.m_TargetType = PlayerTarget.eTargetType.MouseEventCube;
			if (!MouseEventCube.m_MouseEventCubeList.Contains(this))
			{
				MouseEventCube.m_MouseEventCubeList.Add(this);
			}
			base.gameObject.tag = "Chest";
			base.gameObject.layer = LayerMask.NameToLayer("Item");
			GameObject gameObject = Resources.Load("Artist/Effect/Particle_bloomInside") as GameObject;
			this.EffectObj = (Object.Instantiate(gameObject) as GameObject);
			this.EffectObj.transform.parent = base.transform;
			this.EffectObj.transform.localPosition = Vector3.zero;
			this.EffectObj.transform.localScale = Vector3.one;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x000B5918 File Offset: 0x000B3B18
		private void Start()
		{
			if (this.m_GroundItemID != null && this.m_GroundItemID.Length != 0)
			{
				this.m_GroundItemData = Game.GroundItemData.GetGroundItemData(this.m_GroundItemID);
				if (this.m_GroundItemData != null)
				{
					this.m_ItemDataNode = Game.ItemData.GetItemDataNode(this.m_GroundItemData.ItemID);
				}
			}
			if (this.m_QuestID != null && this.m_QuestID.Length != 0)
			{
				this.m_QuestNode = Game.QuestData.GetQuestNode(this.m_QuestID);
			}
			this.ReCheck();
			base.AddPlayerTargetList(this);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x000B59BC File Offset: 0x000B3BBC
		public void ReCheck()
		{
			base.gameObject.SetActive(true);
			base.gameObject.collider.enabled = true;
			if (!(this.AttachObj == null))
			{
				this.AttachObj.SetActive(true);
			}
			if (this.m_GroundItemData != null)
			{
				int num = Game.Variable["GID_" + this.m_GroundItemData.GruondItemID];
				if (num >= 0)
				{
					this.Close();
				}
			}
			if (this.m_QuestNode != null)
			{
				if (MissionStatus.m_instance.CheckCollectionQuest(this.m_QuestID))
				{
					this.Close();
				}
				if (!ConditionManager.CheckCondition(this.m_QuestNode.m_QuestOpenNodeList, true, 0, string.Empty))
				{
					this.Close();
				}
			}
			if (this.EffectObj != null)
			{
				if (this.m_bSearch)
				{
					bool active = this.CheckItemShowEffect();
					this.EffectObj.SetActive(active);
				}
				else
				{
					this.EffectObj.SetActive(false);
				}
			}
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x000B5AC8 File Offset: 0x000B3CC8
		public void SetEvent()
		{
			bool flag = false;
			if (this.m_GroundItemData != null)
			{
				RewardDataNode rewardDataNode = new RewardDataNode();
				rewardDataNode.m_iRewardID = 999999;
				string[] args = new string[]
				{
					"5",
					this.m_GroundItemData.ItemID,
					this.m_GroundItemData.Amount,
					"210043",
					"0"
				};
				MapRewardNode mapRewardNode = new MapRewardNode(args);
				rewardDataNode.m_MapRewardNodeList.Add(mapRewardNode);
				if (rewardDataNode.m_MapRewardNodeList.Count > 0)
				{
					Game.RewardData.DoRewardID(999999, rewardDataNode);
				}
				Game.Variable["GID_" + this.m_GroundItemData.GruondItemID] = 1;
				flag = true;
			}
			if (this.m_QuestNode != null)
			{
				if (MissionStatus.m_instance.CheckCollectionQuest(this.m_QuestID))
				{
					this.Close();
					return;
				}
				string questTalkID = MissionStatus.m_instance.GetQuestTalkID(this.m_QuestID);
				if (questTalkID != string.Empty && questTalkID != "0")
				{
					Game.UI.Get<UITalk>().SetTalkData(questTalkID);
				}
				else if (this.m_QuestNode.m_iGiftID != 0)
				{
					Game.RewardData.DoRewardID(this.m_QuestNode.m_iGiftID, null);
				}
				if (this.m_strBGMusicName != string.Empty)
				{
					Game.PlayBGMusicMapPath(this.m_strBGMusicName);
				}
				flag = true;
			}
			if (!flag)
			{
				Debug.Log("this event : " + base.gameObject.name + " is not have ground item data or quest node please check again");
			}
			this.ReCheck();
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x000B5C6C File Offset: 0x000B3E6C
		public void Close()
		{
			base.gameObject.collider.enabled = false;
			if (!(this.AttachObj == null))
			{
				if (!this.m_bHide)
				{
					return;
				}
				this.AttachObj.SetActive(false);
			}
			if (!this.m_bHide)
			{
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x000B5CD0 File Offset: 0x000B3ED0
		public static void ResetAllMouseEvent()
		{
			for (int i = 0; i < MouseEventCube.m_MouseEventCubeList.Count; i++)
			{
				MouseEventCube.m_MouseEventCubeList[i].ReCheck();
			}
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x000B5D08 File Offset: 0x000B3F08
		private bool CheckItemShowEffect()
		{
			bool result = false;
			if (this.m_ItemDataNode != null)
			{
				ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(this.m_GroundItemData.ItemID);
				switch (itemDataNode.m_iItemKind)
				{
				case 0:
				case 4:
					result = TeamStatus.m_Instance.CheckTeamTalentEffect(TalentEffect.Observant);
					break;
				case 1:
					result = TeamStatus.m_Instance.CheckTeamTalentEffect(TalentEffect.ExploreHerbs);
					break;
				case 2:
					result = TeamStatus.m_Instance.CheckTeamTalentEffect(TalentEffect.ExploreMine);
					break;
				case 3:
					result = TeamStatus.m_Instance.CheckTeamTalentEffect(TalentEffect.ExplorePoison);
					break;
				}
			}
			else
			{
				result = TeamStatus.m_Instance.CheckTeamTalentEffect(TalentEffect.Observant);
			}
			return result;
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x0000D73F File Offset: 0x0000B93F
		private void OnDestroy()
		{
			base.RemovePlayerTargetList(this);
			if (MouseEventCube.m_MouseEventCubeList.Contains(this))
			{
				MouseEventCube.m_MouseEventCubeList.Remove(this);
			}
		}

		// Token: 0x040019C1 RID: 6593
		public static List<MouseEventCube> m_MouseEventCubeList = new List<MouseEventCube>();

		// Token: 0x040019C2 RID: 6594
		public string m_QuestID = string.Empty;

		// Token: 0x040019C3 RID: 6595
		public string m_GroundItemID = string.Empty;

		// Token: 0x040019C4 RID: 6596
		public string m_GroundItemName = string.Empty;

		// Token: 0x040019C5 RID: 6597
		private GroundItemData m_GroundItemData;

		// Token: 0x040019C6 RID: 6598
		private ItemDataNode m_ItemDataNode;

		// Token: 0x040019C7 RID: 6599
		private QuestNode m_QuestNode;

		// Token: 0x040019C8 RID: 6600
		public bool m_bSearch;

		// Token: 0x040019C9 RID: 6601
		public bool m_bHide = true;

		// Token: 0x040019CA RID: 6602
		public GameObject AttachObj;

		// Token: 0x040019CB RID: 6603
		private GameObject EffectObj;

		// Token: 0x040019CC RID: 6604
		public string m_ParentName;

		// Token: 0x040019CD RID: 6605
		public int[] m_ChildIndexList;

		// Token: 0x040019CE RID: 6606
		public string m_strBGMusicName;
	}
}
