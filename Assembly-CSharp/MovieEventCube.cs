using System;
using System.Collections.Generic;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x02000281 RID: 641
public class MovieEventCube : MonoBehaviour
{
	// Token: 0x06000BF4 RID: 3060 RVA: 0x00062790 File Offset: 0x00060990
	public void CopyFromData(MovieEventCubeData data)
	{
		this.bInverse = data.bInverse;
		this.strCollectionID = data.strCollectionID;
		this.strQuestID = data.strQuestID;
		this.iBattleID = data.iBattleID;
		this.iEventID = data.iEventID;
		this.strBGMusicName = data.strBGMusicName;
		this.bCheckTime = data.bCheckTime;
		this.fStartTime = data.fStartTime;
		this.fEndTime = data.fEndTime;
		this.bDontRemoveEventCube = data.bDontRemoveEventCube;
		this.iItemID = data.iItemID;
		this.iItemAmount = data.iItemAmount;
		this.iDevelopQuestID = data.iDevelopQuestID;
		this.CheckConvert = data.CheckConvert;
		this.strTransferSceneName = data.strTransferSceneName;
		this.vPos = data.vPos;
		this.vLocalEulerAngles = data.vLocalEulerAngles;
		this.fDir = data.fDir;
		this.goTarget = GameObject.Find(data.strTarget);
		this.eventMode = data.eventMode;
		for (int i = 0; i < data.mustHaveList.Count; i++)
		{
			EventCubeTriggleNode eventCubeTriggleNode = data.mustHaveList[i];
			this.mustHaveList.Add(eventCubeTriggleNode.Copy());
		}
		for (int i = 0; i < data.oneHaveList.Count; i++)
		{
			EventCubeTriggleNode eventCubeTriggleNode2 = data.oneHaveList[i];
			this.oneHaveList.Add(eventCubeTriggleNode2.Copy());
		}
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x0000264F File Offset: 0x0000084F
	private void Awake()
	{
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x0000922B File Offset: 0x0000742B
	private void Start()
	{
		this.CheckStartTransform();
		this.CheckActive();
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00062914 File Offset: 0x00060B14
	public bool CheckNeedConvert()
	{
		if (this.CheckConvert)
		{
			return false;
		}
		if (this.iBattleID != 0)
		{
			this.eventMode = _EventCubeTriggleMode.Battle;
		}
		else if (this.strBGMusicName != string.Empty && this.strBGMusicName != null)
		{
			this.eventMode = _EventCubeTriggleMode.BGMusic;
		}
		else if (this.iEventID != 0)
		{
			this.eventMode = _EventCubeTriggleMode.Movie;
		}
		else
		{
			this.eventMode = _EventCubeTriggleMode.Movie;
		}
		if (this.strQuestID != string.Empty && this.strQuestID != null)
		{
			EventCubeTriggleNode eventCubeTriggleNode = new EventCubeTriggleNode();
			eventCubeTriggleNode.m_nodeType = _EventNodeType.Quest;
			eventCubeTriggleNode.strValue = this.strQuestID;
			eventCubeTriggleNode.m_bInverse = this.bInverse;
			this.mustHaveList.Add(eventCubeTriggleNode);
		}
		if (this.iDevelopQuestID != 0)
		{
			EventCubeTriggleNode eventCubeTriggleNode2 = new EventCubeTriggleNode();
			eventCubeTriggleNode2.m_nodeType = _EventNodeType.DevelopQuest;
			eventCubeTriggleNode2.m_iValue1 = this.iDevelopQuestID;
			eventCubeTriggleNode2.m_bInverse = this.bInverse;
			this.mustHaveList.Add(eventCubeTriggleNode2);
		}
		if (this.strCollectionID != string.Empty && this.strCollectionID != null)
		{
			EventCubeTriggleNode eventCubeTriggleNode3 = new EventCubeTriggleNode();
			eventCubeTriggleNode3.m_nodeType = _EventNodeType.Collection;
			eventCubeTriggleNode3.strValue = this.strCollectionID;
			eventCubeTriggleNode3.m_bInverse = this.bInverse;
			this.mustHaveList.Add(eventCubeTriggleNode3);
		}
		if (this.bCheckTime)
		{
			EventCubeTriggleNode eventCubeTriggleNode4 = new EventCubeTriggleNode();
			eventCubeTriggleNode4.m_nodeType = _EventNodeType.TriggleTime;
			eventCubeTriggleNode4.m_fValue1 = this.fStartTime;
			eventCubeTriggleNode4.m_fValue2 = this.fEndTime;
			eventCubeTriggleNode4.m_bInverse = this.bInverse;
			this.mustHaveList.Add(eventCubeTriggleNode4);
		}
		if (this.iItemID != 0)
		{
			EventCubeTriggleNode eventCubeTriggleNode5 = new EventCubeTriggleNode();
			eventCubeTriggleNode5.m_nodeType = _EventNodeType.Item;
			eventCubeTriggleNode5.m_iValue1 = this.iItemID;
			eventCubeTriggleNode5.m_iValue1 = this.iItemAmount;
			eventCubeTriggleNode5.m_bInverse = this.bInverse;
			this.mustHaveList.Add(eventCubeTriggleNode5);
		}
		this.CheckConvert = true;
		return true;
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x00062B0C File Offset: 0x00060D0C
	public void QuestToCollection()
	{
		for (int i = 0; i < this.mustHaveList.Count; i++)
		{
			this.mustHaveList[i].QuestToCollection();
		}
		for (int j = 0; j < this.oneHaveList.Count; j++)
		{
			this.oneHaveList[j].QuestToCollection();
		}
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00062B74 File Offset: 0x00060D74
	public void CheckActive()
	{
		if (this.goTarget == null)
		{
			this.goTarget = GameObject.Find(this.strTarget);
		}
		if (!(this.goTarget != null))
		{
			return;
		}
		this.goTarget.SetActive(false);
		if (this.eventMode != _EventCubeTriggleMode.Active)
		{
			return;
		}
		for (int i = 0; i < this.mustHaveList.Count; i++)
		{
			if (!this.mustHaveList[i].Pass())
			{
				return;
			}
		}
		if (this.oneHaveList.Count > 0)
		{
			bool flag = false;
			for (int j = 0; j < this.oneHaveList.Count; j++)
			{
				if (this.oneHaveList[j].Pass())
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		if (this.goTarget != null)
		{
			this.goTarget.SetActive(true);
		}
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x00062C74 File Offset: 0x00060E74
	private void CheckStartTransform()
	{
		if (this.eventMode != _EventCubeTriggleMode.SetTransform)
		{
			return;
		}
		for (int i = 0; i < this.mustHaveList.Count; i++)
		{
			if (!this.mustHaveList[i].Pass())
			{
				return;
			}
		}
		if (this.oneHaveList.Count > 0)
		{
			bool flag = false;
			for (int j = 0; j < this.oneHaveList.Count; j++)
			{
				if (this.oneHaveList[j].Pass())
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		base.gameObject.transform.position = this.vPos;
		base.gameObject.transform.localEulerAngles = this.vLocalEulerAngles;
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x00062D3C File Offset: 0x00060F3C
	public void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
		{
			return;
		}
		if (!this.CheckConvert)
		{
			this.CheckNeedConvert();
		}
		for (int i = 0; i < this.mustHaveList.Count; i++)
		{
			if (!this.mustHaveList[i].Pass())
			{
				return;
			}
		}
		if (this.oneHaveList.Count > 0)
		{
			bool flag = false;
			for (int j = 0; j < this.oneHaveList.Count; j++)
			{
				if (this.oneHaveList[j].Pass())
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		if (this.eventMode == _EventCubeTriggleMode.Movie && this.iEventID != 0)
		{
			if (TeamStatus.m_Instance.m_EventList.Contains(this.iEventID) && !this.bDontRemoveEventCube)
			{
				Object.Destroy(base.gameObject, 0.1f);
				return;
			}
			if (GameSetting.m_Instance.GetComponent<MovieEventTrigger>() != null)
			{
				GameSetting.m_Instance.GetComponent<MovieEventTrigger>().PlayMovie(this.iEventID);
				if (!TeamStatus.m_Instance.m_EventList.Contains(this.iEventID))
				{
					TeamStatus.m_Instance.m_EventList.Add(this.iEventID);
				}
			}
		}
		if (this.eventMode == _EventCubeTriggleMode.Battle && this.iBattleID != 0)
		{
			Game.g_BattleControl.StartBattle(this.iBattleID);
			return;
		}
		if (this.eventMode == _EventCubeTriggleMode.BGMusic && this.strBGMusicName != string.Empty)
		{
			Game.PlayBGMusicMapPath(this.strBGMusicName);
		}
		if (this.eventMode == _EventCubeTriggleMode.Transfer)
		{
			if (Game.IsLoading())
			{
				return;
			}
			GameGlobal.m_fDir = this.fDir;
			GameGlobal.m_TransferPos = this.vPos;
			GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k].name.Equals("cFormLoad"))
				{
					array[k].GetComponent<UILoad>().LoadStage(this.strTransferSceneName);
				}
			}
		}
		this.QuestToCollection();
	}

	// Token: 0x04000DA5 RID: 3493
	public bool bInverse;

	// Token: 0x04000DA6 RID: 3494
	public string strCollectionID;

	// Token: 0x04000DA7 RID: 3495
	public string strQuestID;

	// Token: 0x04000DA8 RID: 3496
	public int iBattleID;

	// Token: 0x04000DA9 RID: 3497
	public int iEventID;

	// Token: 0x04000DAA RID: 3498
	public string strBGMusicName;

	// Token: 0x04000DAB RID: 3499
	public bool bCheckTime;

	// Token: 0x04000DAC RID: 3500
	public float fStartTime;

	// Token: 0x04000DAD RID: 3501
	public float fEndTime;

	// Token: 0x04000DAE RID: 3502
	public bool bDontRemoveEventCube;

	// Token: 0x04000DAF RID: 3503
	public int iItemID;

	// Token: 0x04000DB0 RID: 3504
	public int iItemAmount;

	// Token: 0x04000DB1 RID: 3505
	public int iDevelopQuestID;

	// Token: 0x04000DB2 RID: 3506
	public bool CheckConvert;

	// Token: 0x04000DB3 RID: 3507
	public string strTransferSceneName;

	// Token: 0x04000DB4 RID: 3508
	public Vector3 vPos;

	// Token: 0x04000DB5 RID: 3509
	public Vector3 vLocalEulerAngles;

	// Token: 0x04000DB6 RID: 3510
	public float fDir;

	// Token: 0x04000DB7 RID: 3511
	public GameObject goTarget;

	// Token: 0x04000DB8 RID: 3512
	public string strTarget;

	// Token: 0x04000DB9 RID: 3513
	public _EventCubeTriggleMode eventMode;

	// Token: 0x04000DBA RID: 3514
	public List<EventCubeTriggleNode> mustHaveList = new List<EventCubeTriggleNode>();

	// Token: 0x04000DBB RID: 3515
	public List<EventCubeTriggleNode> oneHaveList = new List<EventCubeTriggleNode>();

	// Token: 0x04000DBC RID: 3516
	private GameObject go_Sky;
}
