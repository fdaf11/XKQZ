using System;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000280 RID: 640
[Serializable]
public class EventCubeTriggleNode
{
	// Token: 0x06000BED RID: 3053 RVA: 0x00002672 File Offset: 0x00000872
	public EventCubeTriggleNode()
	{
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00062368 File Offset: 0x00060568
	public EventCubeTriggleNode(EventCubeTriggleNode src)
	{
		this.m_nodeType = src.m_nodeType;
		this.m_iValue1 = src.m_iValue1;
		this.m_iValue2 = src.m_iValue2;
		this.m_iValue3 = src.m_iValue3;
		this.m_fValue1 = src.m_fValue1;
		this.m_fValue2 = src.m_fValue2;
		this.strValue = src.strValue;
		this.m_bInverse = src.m_bInverse;
		this.m_iNPCID = src.m_iNPCID;
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x000623E8 File Offset: 0x000605E8
	public void CheckString()
	{
		if (this.strValue != null && this.strValue != string.Empty)
		{
			string[] array = this.strValue.Split(new char[]
			{
				"\n".get_Chars(0)
			});
			if (array.Length > 1)
			{
				Debug.LogError(Application.loadedLevelName + " EventCube Error 字串有兩行以上 strValue = " + this.strValue);
				this.strValue = array[0];
			}
		}
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00062464 File Offset: 0x00060664
	public void QuestToCollection()
	{
		if (this.m_nodeType != _EventNodeType.Quest)
		{
			return;
		}
		if (!MissionStatus.m_instance.CheckQuest(this.strValue) && !this.m_bInverse)
		{
			return;
		}
		if (MissionStatus.m_instance.CheckQuest(this.strValue) && this.m_bInverse)
		{
			return;
		}
		if (MissionStatus.m_instance.CheckQuest(this.strValue))
		{
			MissionStatus.m_instance.AddCollectionQuest(this.strValue);
		}
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x000624E4 File Offset: 0x000606E4
	public bool Pass()
	{
		switch (this.m_nodeType)
		{
		case _EventNodeType.Quest:
			if (!MissionStatus.m_instance.CheckQuest(this.strValue) && !this.m_bInverse)
			{
				return false;
			}
			if (MissionStatus.m_instance.CheckQuest(this.strValue) && this.m_bInverse)
			{
				return false;
			}
			break;
		case _EventNodeType.Collection:
			if (!MissionStatus.m_instance.CheckCollectionQuest(this.strValue) && !this.m_bInverse)
			{
				return false;
			}
			if (MissionStatus.m_instance.CheckCollectionQuest(this.strValue) && this.m_bInverse)
			{
				return false;
			}
			break;
		case _EventNodeType.DevelopQuest:
			return false;
		case _EventNodeType.TriggleTime:
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("WorldTime");
			if (gameObject == null)
			{
				return false;
			}
			float hour = gameObject.GetComponent<TOD_Sky>().Cycle.Hour;
			bool flag = true;
			if (this.m_fValue1 <= this.m_fValue2)
			{
				if (hour < this.m_fValue1 || hour > this.m_fValue2)
				{
					flag = false;
				}
			}
			else if (hour > this.m_fValue1 && hour < this.m_fValue2)
			{
				flag = false;
			}
			if (!flag && !this.m_bInverse)
			{
				return false;
			}
			if (flag && this.m_bInverse)
			{
				return false;
			}
			break;
		}
		case _EventNodeType.Item:
		{
			int num = BackpackStatus.m_Instance.CheclItemAmount(this.m_iValue1);
			if (num < this.m_iValue2 && !this.m_bInverse)
			{
				return false;
			}
			if (num >= this.m_iValue2 && this.m_bInverse)
			{
				return false;
			}
			break;
		}
		case _EventNodeType.MovieEvent:
			if (!TeamStatus.m_Instance.m_EventList.Contains(this.m_iValue1) && !this.m_bInverse)
			{
				return false;
			}
			if (TeamStatus.m_Instance.m_EventList.Contains(this.m_iValue1) && this.m_bInverse)
			{
				return false;
			}
			break;
		case _EventNodeType.GetPropert:
			return false;
		case _EventNodeType.NpcFriendly:
		{
			int npcFriendly = Game.NpcData.GetNpcFriendly(this.m_iValue1);
			if (npcFriendly < this.m_iValue2 && !this.m_bInverse)
			{
				return false;
			}
			if (npcFriendly >= this.m_iValue2 && this.m_bInverse)
			{
				return false;
			}
			break;
		}
		case _EventNodeType.TeamMember:
			return TeamStatus.m_Instance.CheckTeamMember(this.m_iNPCID);
		case _EventNodeType.NoTeamMember:
			return !TeamStatus.m_Instance.CheckTeamMember(this.m_iNPCID);
		}
		return true;
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x00062778 File Offset: 0x00060978
	public EventCubeTriggleNode Copy()
	{
		return new EventCubeTriggleNode(this);
	}

	// Token: 0x04000D9C RID: 3484
	public _EventNodeType m_nodeType;

	// Token: 0x04000D9D RID: 3485
	public int m_iValue1;

	// Token: 0x04000D9E RID: 3486
	public int m_iValue2;

	// Token: 0x04000D9F RID: 3487
	public int m_iValue3;

	// Token: 0x04000DA0 RID: 3488
	public float m_fValue1;

	// Token: 0x04000DA1 RID: 3489
	public float m_fValue2;

	// Token: 0x04000DA2 RID: 3490
	public string strValue;

	// Token: 0x04000DA3 RID: 3491
	public bool m_bInverse;

	// Token: 0x04000DA4 RID: 3492
	public int m_iNPCID;
}
