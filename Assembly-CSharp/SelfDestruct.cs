using System;
using UnityEngine;

// Token: 0x02000769 RID: 1897
public class SelfDestruct : MonoBehaviour
{
	// Token: 0x06002D19 RID: 11545 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x0015C3A0 File Offset: 0x0015A5A0
	private void OnEnable()
	{
		if (this.mode == _DelayMode.RealTime)
		{
			this.DelayDestruct();
		}
		else if (this.mode == _DelayMode.Round)
		{
			GameControlTB.onNewRoundE += this.OnNewRound;
		}
		else if (this.mode == _DelayMode.Turn)
		{
			GameControlTB.onNextTurnE += this.OnNextTurn;
			UnitTB.onUnitDestroyedE += this.OnUnitDestroyed;
			this.countTillNextTurn = UnitControl.activeFactionCount;
		}
	}

	// Token: 0x06002D1C RID: 11548 RVA: 0x0001D106 File Offset: 0x0001B306
	private void OnNewRound(int roundCounter)
	{
		this.round--;
		if (this.round == 0)
		{
			GameControlTB.onNewRoundE -= this.OnNewRound;
			this.delay = 0f;
			this.DelayDestruct();
		}
	}

	// Token: 0x06002D1D RID: 11549 RVA: 0x0015C420 File Offset: 0x0015A620
	private void OnNextTurn()
	{
		this.countTillNextTurn--;
		if (this.countTillNextTurn == 0)
		{
			this.turn--;
			if (this.turn == 0)
			{
				GameControlTB.onNextTurnE -= this.OnNextTurn;
				UnitTB.onUnitDestroyedE -= this.OnUnitDestroyed;
				this.delay = 0f;
				this.DelayDestruct();
			}
			else
			{
				this.countTillNextTurn = UnitControl.activeFactionCount;
			}
		}
	}

	// Token: 0x06002D1E RID: 11550 RVA: 0x0001D143 File Offset: 0x0001B343
	private void OnUnitDestroyed(UnitTB unit)
	{
		this.countTillNextTurn--;
	}

	// Token: 0x06002D1F RID: 11551 RVA: 0x0001D153 File Offset: 0x0001B353
	public void DelayDestruct()
	{
		Object.Destroy(base.gameObject, this.delay);
	}

	// Token: 0x04003980 RID: 14720
	public _DelayMode mode;

	// Token: 0x04003981 RID: 14721
	public float delay = 5f;

	// Token: 0x04003982 RID: 14722
	public int round = 3;

	// Token: 0x04003983 RID: 14723
	public int turn = 2;

	// Token: 0x04003984 RID: 14724
	private int countTillNextTurn;
}
