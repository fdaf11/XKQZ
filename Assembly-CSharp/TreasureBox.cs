using System;
using System.Collections;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x02000399 RID: 921
public class TreasureBox : PlayerTarget
{
	// Token: 0x06001551 RID: 5457 RVA: 0x0000D82E File Offset: 0x0000BA2E
	private void Awake()
	{
		this.m_TargetType = PlayerTarget.eTargetType.TreasureBox;
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x000B6330 File Offset: 0x000B4530
	private void Start()
	{
		if (this.m_TreasureBoxNode == null)
		{
			Debug.LogError("沒寶箱資料 " + this.m_strBoxID);
			return;
		}
		this.m_TryOpenOK = false;
		this.m_BoxAnimation = base.GetComponent<Animation>();
		if (this.m_BoxAnimation != null)
		{
			this.m_BoxAnimation.wrapMode = 1;
		}
		if (TeamStatus.m_Instance.m_TreasureBoxList.Contains(this.m_TreasureBoxNode.m_strBoxID))
		{
			this.SetAniOpen(false);
		}
		else
		{
			base.AddPlayerTargetList(this);
		}
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x0000D837 File Offset: 0x0000BA37
	public void SetTreasureBoxNode(TreasureBoxNode _TreasureBoxNode)
	{
		this.m_TreasureBoxNode = _TreasureBoxNode;
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x000B63C0 File Offset: 0x000B45C0
	public void OpenBox()
	{
		if (this.m_TreasureBoxNode == null)
		{
			return;
		}
		if (this.m_bOpen)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
		if (gameObject != null)
		{
			GameGlobal.m_TransferPos = gameObject.transform.localPosition;
			GameGlobal.m_fDir = gameObject.transform.localEulerAngles.y;
		}
		if (!this.m_TreasureBoxNode.m_strQuestID.Equals("0"))
		{
			if (!MissionStatus.m_instance.CheckCollectionQuest(this.m_TreasureBoxNode.m_strQuestID))
			{
				string @string = Game.StringTable.GetString(350001);
				Game.UI.Get<UIMapMessage>().SetMsg(@string);
				return;
			}
		}
		else
		{
			this.m_TryOpenOK = true;
		}
		if (this.m_TreasureBoxNode.m_iBoxType == 0)
		{
			this.m_TryOpenOK = true;
		}
		else if (this.m_TreasureBoxNode.m_iItemID == 0)
		{
			this.m_TryOpenOK = true;
		}
		else
		{
			int num = BackpackStatus.m_Instance.CheclItemAmount(this.m_TreasureBoxNode.m_iItemID);
			if (num > 0)
			{
				BackpackStatus.m_Instance.LessPackItem(this.m_TreasureBoxNode.m_iItemID, 1, null);
				this.m_TryOpenOK = true;
			}
			else
			{
				ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(this.m_TreasureBoxNode.m_iItemID);
				string text = Game.StringTable.GetString(200050);
				text = text + "(" + itemDataNode.m_strItemName + ")";
				Game.UI.Get<UIMapMessage>().SetMsg(text);
				this.m_TryOpenOK = false;
			}
		}
		if (this.m_TryOpenOK)
		{
			if (this.m_TreasureBoxNode.m_iBoxType != 2)
			{
				base.StartCoroutine(this.OpenBoxAndGetReward());
			}
			else if (!Game.IsLoading())
			{
				UITreasureBox orCreat = Game.UI.GetOrCreat<UITreasureBox>();
				if (orCreat != null)
				{
					orCreat.Show();
				}
				else
				{
					Debug.Log("cFormTreasureBox 沒有打包");
				}
			}
		}
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x0000264F File Offset: 0x0000084F
	private void CheckAchievement()
	{
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x000B65C4 File Offset: 0x000B47C4
	private IEnumerator OpenBoxAndGetReward()
	{
		this.m_bOpen = true;
		TeamStatus.m_Instance.m_TreasureBoxList.Add(this.m_strBoxID);
		if (this.m_BoxAnimation != null && this.m_BoxAnimation["Open"] != null)
		{
			float delay = this.m_BoxAnimation["Open"].length;
			this.m_BoxAnimation.Play("Open");
		}
		yield return null;
		base.gameObject.tag = "Untagged";
		Game.RewardData.DoRewardID(this.m_TreasureBoxNode.m_iRewardID, null);
		yield break;
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x0000D840 File Offset: 0x0000BA40
	public void SetAniOpen(bool bClose = true)
	{
		if (this.m_BoxAnimation != null)
		{
			this.m_BoxAnimation.Play("Open");
		}
		if (bClose)
		{
			base.RemovePlayerTargetList(this);
		}
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x0000D871 File Offset: 0x0000BA71
	public void GameEnd(bool bWin)
	{
		if (bWin)
		{
			base.StartCoroutine(this.OpenBoxAndGetReward());
		}
		else
		{
			this.m_TryOpenOK = false;
		}
	}

	// Token: 0x040019FB RID: 6651
	public string m_strBoxID;

	// Token: 0x040019FC RID: 6652
	public bool m_bOpen;

	// Token: 0x040019FD RID: 6653
	private bool m_TryOpenOK;

	// Token: 0x040019FE RID: 6654
	private Animation m_BoxAnimation;

	// Token: 0x040019FF RID: 6655
	public TreasureBoxNode m_TreasureBoxNode;
}
