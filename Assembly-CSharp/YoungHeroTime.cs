using System;
using System.Collections.Generic;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x0200039D RID: 925
public class YoungHeroTime : MonoBehaviour
{
	// Token: 0x06001566 RID: 5478 RVA: 0x0000D91E File Offset: 0x0000BB1E
	private void Awake()
	{
		if (YoungHeroTime.m_instance == null)
		{
			YoungHeroTime.m_instance = this;
			this.bmeDayCount = 0;
			this.BigMapEventDay = Random.Range(5, 60);
			return;
		}
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06001569 RID: 5481 RVA: 0x0000D951 File Offset: 0x0000BB51
	public void StartNewGame()
	{
		this._YoungHeroTime.ReSetDate();
		this._PlayGameTime.m_first = 0;
		this._PlayGameTime.m_StartTime = this.GetUnixTimes();
		this._PlayGameTime.m_FinalTime = this._PlayGameTime.m_StartTime;
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x000B67A8 File Offset: 0x000B49A8
	private int GetUnixTimes()
	{
		return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
	}

	// Token: 0x0600156B RID: 5483 RVA: 0x0000D991 File Offset: 0x0000BB91
	public void LoadSaveGameTime(PlayGameTime LoadPlayGameTime)
	{
		this._PlayGameTime = LoadPlayGameTime;
		if (this._PlayGameTime == null)
		{
			this._PlayGameTime = new PlayGameTime();
		}
		this._PlayGameTime.m_StartTime = this.GetUnixTimes();
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x0000D9C1 File Offset: 0x0000BBC1
	public PlayGameTime GetPlayGameTime()
	{
		this._PlayGameTime = this._PlayGameTime.Clone();
		this._PlayGameTime.m_FinalTime = this.GetUnixTimes();
		this._PlayGameTime.SetTotal();
		return this._PlayGameTime;
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x000B67DC File Offset: 0x000B49DC
	public YoungHeroTimeNode Round2Time(YoungHeroTimeNode _YoungHeroTimeNode)
	{
		int i = _YoungHeroTimeNode.iRound + 5;
		int num = 1;
		int num2 = 0;
		while (i > 65)
		{
			i -= 60;
			num++;
		}
		while (5 < i && i <= 65)
		{
			i -= 5;
			num2++;
		}
		int iweek = i;
		_YoungHeroTimeNode.iYear = num;
		_YoungHeroTimeNode.iMonth = num2;
		_YoungHeroTimeNode.iweek = iweek;
		return _YoungHeroTimeNode;
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x0000D9F6 File Offset: 0x0000BBF6
	public YoungHeroTimeNode Time2Round(YoungHeroTimeNode _YoungHeroTimeNode)
	{
		_YoungHeroTimeNode.iRound = (_YoungHeroTimeNode.iYear - 1) * 60 + (_YoungHeroTimeNode.iMonth - 1) * 5 + _YoungHeroTimeNode.iweek;
		return _YoungHeroTimeNode;
	}

	// Token: 0x0600156F RID: 5487 RVA: 0x0000DA1C File Offset: 0x0000BC1C
	public void LoadTime(int LoadRound, int LoadDay)
	{
		this._YoungHeroTime.ReSetDate();
		this._YoungHeroTime.iRound = LoadRound;
		this._YoungHeroTime.iday = LoadDay;
		this._YoungHeroTime = this.Round2Time(this._YoungHeroTime);
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x000B6844 File Offset: 0x000B4A44
	public YoungHeroTimeNode GetNowTime()
	{
		return this._YoungHeroTime;
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x000B685C File Offset: 0x000B4A5C
	public int GetNowRound()
	{
		return this._YoungHeroTime.iRound;
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x000B6878 File Offset: 0x000B4A78
	public void AddDay(int iday)
	{
		for (int i = 0; i < iday; i++)
		{
			this._YoungHeroTime.iday++;
			if (this._YoungHeroTime.iday > this.RoundDays)
			{
				this._YoungHeroTime.iday = 1;
				this._YoungHeroTime.iRound++;
				this._YoungHeroTime = this.Round2Time(this._YoungHeroTime);
				if (this._YoungHeroTime.iRound % this.iRandeventRound == 0)
				{
					MissionStatus.m_instance.RandomRomurQuest();
				}
				MissionStatus.m_instance.Time2Check();
			}
			this.bmeDayCount++;
			if (this.bmeDayCount >= this.BigMapEventDay)
			{
				if (this.OnBigMapEventTrigger != null)
				{
					this.OnBigMapEventTrigger.Invoke();
				}
				this.bmeDayCount = 0;
				this.BigMapEventDay = Random.Range(5, 60);
			}
		}
		if (Game.UI.Get<UIDate>() != null)
		{
			Game.UI.Get<UIDate>().SetYearMonth();
		}
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x000B698C File Offset: 0x000B4B8C
	public void AddRound(int iRound)
	{
		int iday = iRound * this.RoundDays;
		this.AddDay(iday);
		Game.DLCShopInfo.m_UseInfo = false;
		if (GameGlobal.m_bDLCMode)
		{
			int nowRound = YoungHeroTime.m_instance.GetNowRound();
			int iRound2 = Game.DLCShopInfo.m_iRound;
			TeamStatus.m_Instance.DLCInfoRemain = TeamStatus.m_Instance.DLCInfoLimit;
			if (TeamStatus.m_Instance.DLCStoreRenewTurn <= 0)
			{
				TeamStatus.m_Instance.DLCStoreRenewTurn = 1;
			}
			if (nowRound != iRound2 && nowRound % TeamStatus.m_Instance.DLCStoreRenewTurn == 1)
			{
				this.UpdateDLCStore();
			}
		}
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x000B6A24 File Offset: 0x000B4C24
	public void UpdateDLCStore()
	{
		Game.DLCShopInfo.m_ItemList.Clear();
		StoreDataNode storeDataNode = Game.StoreData.GetStoreDataNode(1000087);
		if (storeDataNode == null)
		{
			return;
		}
		List<StoreItemNode> storeItemNodeList = storeDataNode.m_StoreItemNodeList;
		List<StoreItemNode> list = new List<StoreItemNode>();
		list.AddRange(storeItemNodeList.ToArray());
		int dlcstoreLimit = TeamStatus.m_Instance.DLCStoreLimit;
		while (list.Count > 0 && Game.DLCShopInfo.m_ItemList.Count < dlcstoreLimit)
		{
			int num = Random.Range(0, list.Count);
			StoreItemNode storeItemNode = list[num];
			list.RemoveAt(num);
			if (Game.ItemData.GetItemDataNode(storeItemNode.m_iItemID) != null)
			{
				if (ConditionManager.CheckCondition(storeItemNode.m_ConditionList, storeItemNode.bAnd, 0, string.Empty))
				{
					if (Random.Range(0, 100) < storeItemNode.m_iProbability)
					{
						Game.DLCShopInfo.m_ItemList.Add(storeItemNode.m_iItemID);
					}
				}
			}
		}
		int nowRound = YoungHeroTime.m_instance.GetNowRound();
		if (nowRound != 1)
		{
			Game.DLCShopInfo.m_Update = true;
		}
	}

	// Token: 0x06001575 RID: 5493 RVA: 0x000B6B50 File Offset: 0x000B4D50
	public int AddCheckRound(int iRound)
	{
		return this._YoungHeroTime.iRound + iRound;
	}

	// Token: 0x06001576 RID: 5494 RVA: 0x0000DA53 File Offset: 0x0000BC53
	public bool CheackRoundRange(int StartRound, int EndRound)
	{
		return StartRound <= this._YoungHeroTime.iRound && this._YoungHeroTime.iRound <= EndRound;
	}

	// Token: 0x06001577 RID: 5495 RVA: 0x000B6B6C File Offset: 0x000B4D6C
	public bool CheackRound(eCompareType iType, int Round)
	{
		if (iType == eCompareType.Equal)
		{
			return this._YoungHeroTime.iRound == Round;
		}
		if (iType == eCompareType.Greater)
		{
			return this._YoungHeroTime.iRound > Round;
		}
		if (iType == eCompareType.Lass)
		{
			return this._YoungHeroTime.iRound < Round;
		}
		return iType == eCompareType.GreaterOrEqual && this._YoungHeroTime.iRound >= Round;
	}

	// Token: 0x04001A0C RID: 6668
	public static YoungHeroTime m_instance;

	// Token: 0x04001A0D RID: 6669
	public YoungHeroTimeNode _YoungHeroTime = new YoungHeroTimeNode();

	// Token: 0x04001A0E RID: 6670
	private PlayGameTime _PlayGameTime = new PlayGameTime();

	// Token: 0x04001A0F RID: 6671
	public int BigMapEventDay;

	// Token: 0x04001A10 RID: 6672
	private int bmeDayCount;

	// Token: 0x04001A11 RID: 6673
	public Action OnBigMapEventTrigger;

	// Token: 0x04001A12 RID: 6674
	public int RoundDays = 30;

	// Token: 0x04001A13 RID: 6675
	public int iRandeventRound = 10;
}
