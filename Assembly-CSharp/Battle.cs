using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006D5 RID: 1749
public class Battle : MonoBehaviour
{
	// Token: 0x06002A23 RID: 10787 RVA: 0x0001B868 File Offset: 0x00019A68
	private void Awake()
	{
		if (Battle.instance == null)
		{
			Battle.instance = this;
			this.listMainGame = new List<GameObject>();
			this.listBattle = new List<GameObject>();
		}
	}

	// Token: 0x06002A24 RID: 10788 RVA: 0x0014B68C File Offset: 0x0014988C
	private void Start()
	{
		if (Battle.instance != null)
		{
			if (Battle.instance.listMainGame != null && this.MainGameObject != null && this.MainGameObject.Length > 0)
			{
				Battle.instance.listMainGame.AddRange(this.MainGameObject);
			}
			if (Battle.instance.listBattle != null && this.BattleObject != null && this.BattleObject.Length > 0)
			{
				Battle.instance.listBattle.AddRange(this.BattleObject);
			}
			int i = 0;
			while (i < Battle.instance.listMainGame.Count)
			{
				if (Battle.instance.listMainGame[i] == null)
				{
					Battle.instance.listMainGame.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
			int j = 0;
			while (j < Battle.instance.listBattle.Count)
			{
				if (Battle.instance.listBattle[j] == null)
				{
					Battle.instance.listBattle.RemoveAt(j);
				}
				else
				{
					j++;
				}
			}
		}
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x0001B896 File Offset: 0x00019A96
	public static void BattleStart()
	{
		if (Battle.instance != null)
		{
			Battle.instance._BattleStart();
		}
	}

	// Token: 0x06002A26 RID: 10790 RVA: 0x0001B8B2 File Offset: 0x00019AB2
	public static void BattleEnd()
	{
		if (Battle.instance != null)
		{
			Battle.instance._BattleEnd();
		}
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x0014B7C4 File Offset: 0x001499C4
	public static void AddMainGameList(GameObject go)
	{
		if (Battle.instance == null)
		{
			return;
		}
		if (Battle.instance.listMainGame != null && go != null && Battle.instance.listMainGame.IndexOf(go) < 0)
		{
			Battle.instance.listMainGame.Add(go);
		}
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x0014B824 File Offset: 0x00149A24
	public static void AddBattleList(GameObject go)
	{
		if (Battle.instance == null)
		{
			return;
		}
		if (Battle.instance.listBattle != null && go != null && Battle.instance.listBattle.IndexOf(go) < 0)
		{
			Battle.instance.listBattle.Add(go);
		}
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x0014B884 File Offset: 0x00149A84
	private void _BattleStart()
	{
		int i = 0;
		while (i < this.listMainGame.Count)
		{
			if (this.listMainGame[i] != null)
			{
				this.listMainGame[i].SetActive(false);
				i++;
			}
			else
			{
				this.listMainGame.RemoveAt(i);
			}
		}
		int j = 0;
		while (j < this.listBattle.Count)
		{
			if (this.listBattle[j] != null)
			{
				this.listBattle[j].SetActive(true);
				j++;
			}
			else
			{
				this.listBattle.RemoveAt(j);
			}
		}
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x0014B940 File Offset: 0x00149B40
	private void _BattleEnd()
	{
		int i = 0;
		while (i < this.listBattle.Count)
		{
			if (this.listBattle[i] != null)
			{
				this.listBattle[i].SetActive(false);
				i++;
			}
			else
			{
				this.listBattle.RemoveAt(i);
			}
		}
		int j = 0;
		while (j < this.listMainGame.Count)
		{
			if (this.listMainGame[j] != null)
			{
				this.listMainGame[j].SetActive(true);
				j++;
			}
			else
			{
				this.listMainGame.RemoveAt(j);
			}
		}
	}

	// Token: 0x04003548 RID: 13640
	public GameObject[] MainGameObject;

	// Token: 0x04003549 RID: 13641
	public GameObject[] BattleObject;

	// Token: 0x0400354A RID: 13642
	private List<GameObject> listMainGame;

	// Token: 0x0400354B RID: 13643
	private List<GameObject> listBattle;

	// Token: 0x0400354C RID: 13644
	public static Battle instance;
}
