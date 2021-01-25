using System;
using UnityEngine;

// Token: 0x020002D8 RID: 728
public class GoalStatus : MonoBehaviour
{
	// Token: 0x06000EA6 RID: 3750 RVA: 0x0007B384 File Offset: 0x00079584
	public void Init(int iTargetEgg1, int iTargetAmount1, int iTargetEgg2, int iTargetAmount2, int plus)
	{
		this.iTargetCount = 0;
		this.iTargetPos1 = 0;
		this.iTargetPos2 = 0;
		this.iTarget1 = iTargetEgg1;
		this.iTarget2 = iTargetEgg2;
		this.iPlus = plus;
		if (iTargetAmount1 != 0)
		{
			this.iTargetCount++;
		}
		if (iTargetAmount2 != 0)
		{
			this.iTargetCount++;
		}
		if (this.iTargetCount == 1)
		{
			if (this.goTargetIcon0 != null)
			{
				this.goTargetIcon0.SetActive(true);
			}
			if (this.goTargetIcon1 != null)
			{
				this.goTargetIcon1.SetActive(false);
			}
			if (this.goTargetIcon2 != null)
			{
				this.goTargetIcon2.SetActive(false);
			}
			if (this.labelText0 != null)
			{
				this.labelText0.gameObject.SetActive(true);
			}
			if (this.labelText1 != null)
			{
				this.labelText1.gameObject.SetActive(false);
			}
			if (this.labelText2 != null)
			{
				this.labelText2.gameObject.SetActive(false);
			}
			this.sprTarget1 = this.goTargetIcon0.GetComponent<UISprite>();
			this.sprTarget2 = null;
			this.Text1 = this.labelText0;
			this.Text2 = null;
			this.iAmount1 = iTargetAmount1;
			this.SetTile(this.sprTarget1, this.iTarget1);
			this.UpdateText(this.Text1, this.iTargetPos1, this.iAmount1);
		}
		else if (this.iTargetCount == 2)
		{
			if (this.goTargetIcon0 != null)
			{
				this.goTargetIcon0.SetActive(false);
			}
			if (this.goTargetIcon1 != null)
			{
				this.goTargetIcon1.SetActive(true);
			}
			if (this.goTargetIcon2 != null)
			{
				this.goTargetIcon2.SetActive(true);
			}
			if (this.labelText0 != null)
			{
				this.labelText0.gameObject.SetActive(false);
			}
			if (this.labelText1 != null)
			{
				this.labelText1.gameObject.SetActive(true);
			}
			if (this.labelText2 != null)
			{
				this.labelText2.gameObject.SetActive(true);
			}
			this.sprTarget1 = this.goTargetIcon1.GetComponent<UISprite>();
			this.sprTarget2 = this.goTargetIcon2.GetComponent<UISprite>();
			this.Text1 = this.labelText1;
			this.Text2 = this.labelText2;
			this.iAmount1 = iTargetAmount1;
			this.iAmount2 = iTargetAmount2;
			this.SetTile(this.sprTarget1, this.iTarget1);
			this.SetTile(this.sprTarget2, this.iTarget2);
			this.UpdateText(this.Text1, this.iTargetPos1, this.iAmount1);
			this.UpdateText(this.Text2, this.iTargetPos2, this.iAmount2);
		}
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x0007B67C File Offset: 0x0007987C
	private void SetTile(UISprite sprTile, int iValue)
	{
		if (iValue >= 1 && iValue <= 11)
		{
			sprTile.spriteName = "Tile-" + (iValue + this.iPlus).ToString("000");
		}
		else if (iValue == 100)
		{
			sprTile.spriteName = "splash02";
		}
		else if (iValue == 200)
		{
			sprTile.spriteName = "Trap-02";
		}
		else if (iValue == 300)
		{
			sprTile.spriteName = "Tile-301";
		}
		else
		{
			sprTile.spriteName = "null";
		}
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x00009CBF File Offset: 0x00007EBF
	private void UpdateText(UILabel Text, int iPos, int iAmount)
	{
		Text.text = iPos.ToString() + "/" + iAmount.ToString();
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x0007B71C File Offset: 0x0007991C
	public int UpdateGoal(int iTileID)
	{
		int result = 0;
		if (iTileID == this.iTarget1)
		{
			result = 1;
			this.iTargetPos1++;
			this.UpdateText(this.Text1, this.iTargetPos1, this.iAmount1);
		}
		if (iTileID == this.iTarget2)
		{
			result = 2;
			this.iTargetPos2++;
			this.UpdateText(this.Text2, this.iTargetPos2, this.iAmount2);
		}
		return result;
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x00009CDF File Offset: 0x00007EDF
	public bool CheckHaveGoalTarget(int iTileID)
	{
		return iTileID == this.iTarget1 || iTileID == this.iTarget2;
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x00009CFE File Offset: 0x00007EFE
	public bool CheckGoalFinish()
	{
		return this.iTargetPos1 >= this.iAmount1 && this.iTargetPos2 >= this.iAmount2;
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x0007B794 File Offset: 0x00079994
	public void SetTargetTile(TileData tdTemp)
	{
		if (this.sprTarget1 != null)
		{
			tdTemp.targetObj1 = this.sprTarget1.gameObject;
		}
		if (this.sprTarget2 != null)
		{
			tdTemp.targetObj2 = this.sprTarget2.gameObject;
		}
	}

	// Token: 0x0400117B RID: 4475
	public GameObject goTargetIcon0;

	// Token: 0x0400117C RID: 4476
	public GameObject goTargetIcon1;

	// Token: 0x0400117D RID: 4477
	public GameObject goTargetIcon2;

	// Token: 0x0400117E RID: 4478
	public UILabel labelText0;

	// Token: 0x0400117F RID: 4479
	public UILabel labelText1;

	// Token: 0x04001180 RID: 4480
	public UILabel labelText2;

	// Token: 0x04001181 RID: 4481
	private UISprite sprTarget1;

	// Token: 0x04001182 RID: 4482
	private UISprite sprTarget2;

	// Token: 0x04001183 RID: 4483
	private UILabel Text1;

	// Token: 0x04001184 RID: 4484
	private UILabel Text2;

	// Token: 0x04001185 RID: 4485
	private int iTargetCount;

	// Token: 0x04001186 RID: 4486
	private int iTarget1;

	// Token: 0x04001187 RID: 4487
	private int iTarget2;

	// Token: 0x04001188 RID: 4488
	private int iTargetPos1;

	// Token: 0x04001189 RID: 4489
	private int iTargetPos2;

	// Token: 0x0400118A RID: 4490
	private int iAmount1;

	// Token: 0x0400118B RID: 4491
	private int iAmount2;

	// Token: 0x0400118C RID: 4492
	private int iPlus;
}
