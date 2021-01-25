using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020002D4 RID: 724
public class AlchemyGameControl : MonoBehaviour
{
	// Token: 0x06000E49 RID: 3657 RVA: 0x00076360 File Offset: 0x00074560
	public void SetData(AlchemyScene scene)
	{
		this.bGameOver = false;
		this.iCol = scene.m_iWidth;
		this.iRow = scene.m_iHeight;
		this.iGameStartTile = 2;
		this.iPerTurn = 1;
		this.maxMoveCount = scene.m_iMoveCount;
		this.iTargetObject1 = scene.m_iMarkTarget;
		this.iTargetAmount1 = scene.m_iMarkTargetCount;
		this.targetName = Game.ItemData.GetItemName(scene.m_iSuccessItemID);
		if (Game.AlchemyData.GetAlchemyProduceNode(scene.m_iAbilityBookID) != null)
		{
			this.iType = Game.AlchemyData.GetAlchemyProduceNode(scene.m_iAbilityBookID).m_iType;
		}
		this.ClearAll();
		this.Rebuild();
		this.InitializeGUILabel();
		this.SpawnList.Clear();
		for (int i = 0; i < this.iRow; i++)
		{
			int j = 0;
			while (j < this.iCol)
			{
				int num = i * this.iCol + j;
				int num2 = scene.m_iTileList[num];
				this.DataArray[num] = num2;
				if (num2 >= 0)
				{
					goto IL_18E;
				}
				if (num2 != -1)
				{
					if (num2 == -2 && this.sprBaseTile != null)
					{
						this.TileList[num] = (Object.Instantiate(this.sprBaseTile) as UISprite);
						this.TileList[num].transform.parent = this.sprBaseTile.transform.parent;
						this.TileList[num].name = "Rock";
						this.TileList[num].GetComponent<TileData>().InitTile(this.iCol, this.iRow, j, i);
						this.TileList[num].GetComponent<TileData>().SetTileValue(num2);
						goto IL_18E;
					}
					goto IL_18E;
				}
				IL_2D0:
				j++;
				continue;
				IL_18E:
				if (num2 == 503)
				{
					this.SpawnList.Add(num);
					this.DataArray[num] = 0;
				}
				if ((j + i) % 2 == 0)
				{
					this.BaseTileList[num] = (Object.Instantiate(this.sprGrassTile_light) as UISprite);
				}
				else
				{
					this.BaseTileList[num] = (Object.Instantiate(this.sprGrassTile_dark) as UISprite);
				}
				this.BaseTileList[num].transform.parent = this.sprGrassTile_light.transform.parent;
				this.BaseTileList[num].GetComponent<TileData>().InitTile(this.iCol, this.iRow, j, i);
				if (num2 > 0)
				{
					this.TileList[num] = (Object.Instantiate(this.sprBaseTile) as UISprite);
					this.TileList[num].transform.parent = this.sprBaseTile.transform.parent;
					this.TileList[num].name = "Tile";
					this.TileList[num].GetComponent<TileData>().iPlus = this.iType * 100;
					this.TileList[num].GetComponent<TileData>().InitTile(this.iCol, this.iRow, j, i);
					this.TileList[num].GetComponent<TileData>().SetTileValue(num2);
					goto IL_2D0;
				}
				goto IL_2D0;
			}
		}
		this.BuildPlusTile();
		this.SpawnEgg(this.iGameStartTile);
		this.UpdateDataArrayHistory();
		this.CheckMoveScore();
		this.bStartGame = true;
	}

	// Token: 0x06000E4A RID: 3658 RVA: 0x00076684 File Offset: 0x00074884
	private void InitializeGUILabel()
	{
		Transform transform = base.gameObject.transform.Find("Group/AlchemyBowl/Move/Counter");
		if (null != transform)
		{
			this.movesLabel = transform.gameObject.GetComponent<UILabel>();
			this.movesLabel.text = this.maxMoveCount.ToString();
			this.totalMoves = this.maxMoveCount;
		}
		transform = base.gameObject.transform.Find("Group/AlchemyBowl/RequestBackGround/TargetTitle/Label");
		if (null != transform)
		{
			this.targetNameLabel = transform.gameObject.GetComponent<UILabel>();
			this.targetNameLabel.text = this.targetName;
		}
		transform = base.gameObject.transform.Find("Bottom/moves/bar");
		if (null != transform)
		{
			this.movesBar = transform.gameObject.GetComponent<UISprite>();
			this.movesBar.fillAmount = 1f;
		}
		transform = base.gameObject.transform.Find("Bottom/score/value_score");
		if (null != transform)
		{
			this.scoreLabel = transform.gameObject.GetComponent<UILabel>();
			this.UpdateScore(0);
		}
		transform = base.gameObject.transform.Find("Bottom/score/bar");
		if (null != transform)
		{
			this.scoreBar = transform.gameObject.GetComponent<UISprite>();
			this.scoreBar.fillAmount = 0f;
		}
		transform = base.gameObject.transform.Find("Bottom/combo/value_combo");
		if (null != transform)
		{
			this.comboLabel = transform.gameObject.GetComponent<UILabel>();
			this.UpdateMultiplier(false);
		}
		this.goalStatus.Init(this.iTargetObject1, this.iTargetAmount1, this.iTargetObject2, this.iTargetAmount2, this.iType * 100);
		this.goalStatus.SetTargetTile(this.sprBaseTile.gameObject.GetComponent<TileData>());
	}

	// Token: 0x06000E4B RID: 3659 RVA: 0x0007686C File Offset: 0x00074A6C
	public void ClearAll()
	{
		if (this.TileList != null)
		{
			int num = this.TileList.Length;
			for (int i = 0; i < num; i++)
			{
				if (this.TileList[i] != null)
				{
					Object.DestroyImmediate(this.TileList[i].gameObject);
				}
			}
			this.TileList = null;
		}
		if (this.BaseTileList != null)
		{
			int num2 = this.BaseTileList.Length;
			for (int j = 0; j < num2; j++)
			{
				if (this.BaseTileList[j] != null)
				{
					Object.DestroyImmediate(this.BaseTileList[j].gameObject);
				}
			}
			this.BaseTileList = null;
		}
		if (this.TilePlusList != null)
		{
			int count = this.TilePlusList.Count;
			for (int k = 0; k < count; k++)
			{
				if (this.TilePlusList[k] != null)
				{
					Object.DestroyImmediate(this.TilePlusList[k].gameObject);
				}
			}
			this.TilePlusList.Clear();
		}
		this.DataArray = null;
		UISprite[] componentsInChildren = this.sprBaseTile.parent.gameObject.GetComponentsInChildren<UISprite>();
		int num3 = componentsInChildren.Length;
		for (int l = 0; l < num3; l++)
		{
			if (componentsInChildren[l].name.IndexOf("Editor") >= 0)
			{
				Object.DestroyImmediate(componentsInChildren[l].gameObject);
			}
		}
		componentsInChildren = this.sprGrassTile_dark.parent.gameObject.GetComponentsInChildren<UISprite>();
		num3 = componentsInChildren.Length;
		for (int m = 0; m < num3; m++)
		{
			if (componentsInChildren[m].name.IndexOf("Editor") >= 0)
			{
				Object.DestroyImmediate(componentsInChildren[m].gameObject);
			}
		}
	}

	// Token: 0x06000E4C RID: 3660 RVA: 0x00076A4C File Offset: 0x00074C4C
	private void Rebuild()
	{
		this.iTotalTile = this.iCol * this.iRow;
		if (this.sprBaseTile != null && this.sprGrassTile_light != null)
		{
			this.iTotalTile = this.iRow * this.iCol;
			this.TileList = new UISprite[this.iTotalTile];
			this.DataArray = new int[this.iTotalTile];
			this.BaseTileList = new UISprite[this.iTotalTile];
		}
	}

	// Token: 0x06000E4D RID: 3661 RVA: 0x00076AD4 File Offset: 0x00074CD4
	public void RebuildEditMode()
	{
		this.iTotalTile = this.iCol * this.iRow;
		if (this.sprBaseTile != null && this.sprGrassTile_light != null)
		{
			this.iTotalTile = this.iRow * this.iCol;
			this.TileList = new UISprite[this.iTotalTile];
			this.DataArray = new int[this.iTotalTile];
			this.BaseTileList = new UISprite[this.iTotalTile];
			for (int i = 0; i < this.iRow; i++)
			{
				for (int j = 0; j < this.iCol; j++)
				{
					int num = i * this.iCol + j;
					if ((j + i) % 2 == 0)
					{
						this.BaseTileList[num] = (Object.Instantiate(this.sprGrassTile_light) as UISprite);
					}
					else
					{
						this.BaseTileList[num] = (Object.Instantiate(this.sprGrassTile_dark) as UISprite);
					}
					this.BaseTileList[num].transform.parent = this.sprGrassTile_light.transform.parent;
					this.BaseTileList[num].GetComponent<TileData>().InitTile(this.iCol, this.iRow, j, i);
					this.BaseTileList[num].name = "EditorGrass-" + i.ToString() + "-" + j.ToString();
					this.TileList[num] = (Object.Instantiate(this.sprBaseTile) as UISprite);
					this.TileList[num].transform.parent = this.sprBaseTile.transform.parent;
					this.TileList[num].GetComponent<TileData>().iPlus = this.iType * 100;
					this.TileList[num].GetComponent<TileData>().InitTile(this.iCol, this.iRow, j, i);
					this.TileList[num].GetComponent<TileData>().SetTileValue(0);
					this.TileList[num].name = "EditorTile-" + i.ToString() + "-" + j.ToString();
				}
			}
		}
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x00076CF4 File Offset: 0x00074EF4
	private void Update()
	{
		if (UICamera.selectedObject == null || !NGUITools.GetActive(UICamera.selectedObject))
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.selectedObject = base.gameObject;
		}
		if (!this.bGameOver && this.bAutoMove)
		{
			this.fNowTime += Time.deltaTime;
			if (this.fNowTime > this.fAutoMoveDealyTime)
			{
				this.AutoMove();
				this.fNowTime -= this.fAutoMoveDealyTime;
			}
		}
		if (this.bGenerate && this.CheckAllTileMoveFinish())
		{
			this.bGenerate = false;
			this.SpawnEgg(this.iPerTurn);
			this.CheckMoveScore();
			if (!this.bCanMoveUp && !this.bCanMoveDown && !this.bCanMoveLeft && !this.bCanMoveRight)
			{
				this.OpenEndGameScreen(2);
			}
			for (int i = 0; i < this.iCol * this.iRow; i++)
			{
				if (this.DataArray[i] > 600)
				{
					this.BombStep01.Add((this.DataArray[i] - 600) / 10 * 10);
				}
			}
			for (int j = 0; j < this.BombStep01.Count; j++)
			{
				this.BombStep01[j] = this.BombStep01[j] - this.BombMoves;
				if (this.BombStep01[j] == 0)
				{
					this.OpenEndGameScreen(3);
					break;
				}
			}
			this.BombStep01.Clear();
			if (this.CantDanCount > 0)
			{
				this.bCanSpawnDan = false;
			}
			if (this.CantDanCount == 0)
			{
				this.bCanSpawnDan = true;
			}
			this.SetDan();
			this.UpdateDataArrayHistory();
		}
		this.CheckReleaseTile();
		if (!this.bStartGame)
		{
			return;
		}
		if (!this.bCanMoveUp && !this.bCanMoveDown && !this.bCanMoveLeft && !this.bCanMoveRight)
		{
			this.OpenEndGameScreen(2);
		}
		else if (this.totalMoves <= 0 && this.CheckAllTileMoveFinish())
		{
			this.OpenEndGameScreen(1);
		}
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x00076F30 File Offset: 0x00075130
	private void PlayBounceEffect(string direction)
	{
		for (int i = 0; i < this.TileList.Length; i++)
		{
			if (this.TileList[i] != null && this.CheckTileCanMove(this.DataArray[i]))
			{
				this.TileList[i].GetComponent<TileData>().BounceTile(direction);
			}
		}
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x00076F90 File Offset: 0x00075190
	private void CheckReleaseTile()
	{
		int i = 0;
		while (i < this.NeedReleaseTileList.Count)
		{
			if (!this.NeedReleaseTileList[i].bMoveing)
			{
				Object.Destroy(this.NeedReleaseTileList[i].gameObject);
				this.NeedReleaseTileList.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x00076FF8 File Offset: 0x000751F8
	private void UpdateMoves()
	{
		this.totalMoves--;
		this.BombMoves++;
		if (this.movesLabel != null)
		{
			this.movesLabel.text = this.totalMoves.ToString();
		}
		if (this.movesBar != null)
		{
			this.movesBar.fillAmount = (float)this.totalMoves * 1f / ((float)this.maxMoveCount * 1f);
		}
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x00077080 File Offset: 0x00075280
	private int CalculateScore(int power)
	{
		int num = (int)Mathf.Pow(2f, (float)power);
		int num2 = Convert.ToInt32(this.totalMultiplier * 10f);
		return num * num2;
	}

	// Token: 0x06000E53 RID: 3667 RVA: 0x000770B0 File Offset: 0x000752B0
	private void UpdateScore(int value)
	{
		this.totalScore += value;
		if (value > 0)
		{
			this.UpdateMultiplier(true);
			this.CantDanCount = 0;
		}
		if (value == 0)
		{
			this.UpdateMultiplier(false);
			this.CantDanCount = 0;
			this.bCanSpawnDan = true;
		}
		if (this.scoreLabel != null)
		{
			this.scoreLabel.text = this.totalScore.ToString();
		}
		if (this.scoreBar != null)
		{
			this.scoreBar.fillAmount = (float)this.totalScore * 1f / ((float)this.maxScoreCount * 1f);
		}
	}

	// Token: 0x06000E54 RID: 3668 RVA: 0x0007715C File Offset: 0x0007535C
	private void UpdateMultiplier(bool combo)
	{
		string text = string.Empty;
		if (combo)
		{
			this.totalMultiplier += this.multiplierIncrement;
			this.totalCombo++;
			text = this.totalCombo.ToString() + " COMBO";
		}
		else
		{
			this.totalMultiplier = this.defaultMultiplier;
			this.totalCombo = 0;
		}
		if (this.comboLabel != null)
		{
			this.comboLabel.text = text;
		}
	}

	// Token: 0x06000E55 RID: 3669 RVA: 0x000771E4 File Offset: 0x000753E4
	private IEnumerator _SetEnd(bool bEndType)
	{
		yield return new WaitForSeconds(1f);
		base.SendMessage("SetEnd", bEndType);
		yield break;
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x00009B0A File Offset: 0x00007D0A
	private void OpenEndGameScreen(int type)
	{
		if (this.bGameOver)
		{
			return;
		}
		this.bGameOver = true;
		if (type == 0)
		{
			base.StartCoroutine(this._SetEnd(true));
		}
		else
		{
			base.StartCoroutine(this._SetEnd(false));
		}
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x00009B46 File Offset: 0x00007D46
	private void UpdateTarget()
	{
		if (this.goalStatus.CheckGoalFinish())
		{
			this.OpenEndGameScreen(0);
		}
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x00009B46 File Offset: 0x00007D46
	public void UpdateObs()
	{
		if (this.goalStatus.CheckGoalFinish())
		{
			this.OpenEndGameScreen(0);
		}
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x00077210 File Offset: 0x00075410
	private void UpdateScreen()
	{
		for (int i = 0; i < this.iTotalTile; i++)
		{
			if (this.TileList[i] != null)
			{
				this.TileList[i].spriteName = "Tile-" + this.DataArray[i].ToString("000");
			}
		}
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x00077274 File Offset: 0x00075474
	private void SpawnEgg(int iCount)
	{
		if (this.bGameOver)
		{
			return;
		}
		int count = this.SpawnList.Count;
		if (count > 0)
		{
			this.SpawnOnArea(iCount);
		}
		else
		{
			this.RandomGenerate(iCount);
		}
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x000772B4 File Offset: 0x000754B4
	private void SpawnOnArea(int iCount)
	{
		int num = 0;
		int count = this.SpawnList.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.DataArray[this.SpawnList[i]] == 0)
			{
				num++;
			}
		}
		if (num == 0)
		{
			return;
		}
		if (iCount > num)
		{
			iCount = num;
		}
		for (int j = 0; j < iCount; j++)
		{
			int num2 = Random.Range(1, 3);
			int num3 = Random.Range(1, count + 1);
			int num4 = 0;
			int num5 = this.iLastGeneratePos;
			while (num4 != num3)
			{
				num5++;
				if (num5 >= count)
				{
					num5 = 0;
				}
				if (this.DataArray[this.SpawnList[num5]] == 0)
				{
					num4++;
				}
			}
			this.iLastGeneratePos = num5;
			this.NewTile(this.SpawnList[num5], num2);
			this.DataArray[this.SpawnList[num5]] = num2;
		}
		for (int k = 0; k < this.iTotalTile; k++)
		{
			if (this.DataArray[k] == 881)
			{
				this.DataArray[k] = 0;
			}
		}
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x000773E8 File Offset: 0x000755E8
	private void RandomGenerate(int iCount)
	{
		if (this.bGameOver)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < this.iTotalTile; i++)
		{
			if (this.DataArray[i] == 0)
			{
				num++;
			}
		}
		if (num == 0)
		{
			return;
		}
		if (iCount > num)
		{
			iCount = num;
		}
		for (int j = 0; j < iCount; j++)
		{
			int num2 = Random.Range(1, 3);
			int num3 = Random.Range(1, this.iTotalTile);
			int num4 = 0;
			int num5 = this.iLastGeneratePos;
			while (num4 != num3)
			{
				num5++;
				if (num5 >= this.iTotalTile)
				{
					num5 = 0;
				}
				if (this.DataArray[num5] == 0)
				{
					num4++;
				}
			}
			this.iLastGeneratePos = num5;
			this.NewTile(num5, num2);
			this.DataArray[num5] = num2;
		}
		for (int k = 0; k < this.iTotalTile; k++)
		{
			if (this.DataArray[k] == 881)
			{
				this.DataArray[k] = 0;
			}
		}
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x00009B5F File Offset: 0x00007D5F
	private void UpdateDataArrayHistory()
	{
		this.UpdateDataArrayHistory(false);
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x000774FC File Offset: 0x000756FC
	private void UpdateDataArrayHistory(bool isBooster)
	{
		int[] array = new int[this.DataArray.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.DataArray[i];
		}
		this.HistoryDataArray.Add(array);
		this.HistoryCombo.Add(this.totalCombo);
		this.HistoryScore.Add(this.totalScore);
		this.HistoryTargetCount.Add(this.iTargetCurrentCount);
		this.HistoryBooster.Add(isBooster);
		this.HistoryMultiplier.Add(this.totalMultiplier);
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x00077594 File Offset: 0x00075794
	private void CheckMoveScore()
	{
		this.iMoveUpScore = 0;
		this.bCanMoveUp = false;
		int[] array = new int[this.iRow];
		for (int i = 0; i < this.iCol; i++)
		{
			for (int j = 0; j < this.iRow; j++)
			{
				int num = j * this.iCol + i;
				array[j] = this.DataArray[num];
			}
			if (!this.bCanMoveUp)
			{
				bool flag = this.CheckMove(array);
				if (flag)
				{
					this.bCanMoveUp = true;
				}
			}
			int num2 = this.CheckArrayScore(array);
			this.iMoveUpScore += num2;
		}
		this.iMoveDownScore = 0;
		this.bCanMoveDown = false;
		for (int k = 0; k < this.iCol; k++)
		{
			for (int l = 0; l < this.iRow; l++)
			{
				int num3 = (this.iRow - 1 - l) * this.iCol + k;
				array[l] = this.DataArray[num3];
			}
			if (!this.bCanMoveDown)
			{
				bool flag2 = this.CheckMove(array);
				if (flag2)
				{
					this.bCanMoveDown = true;
				}
			}
			int num4 = this.CheckArrayScore(array);
			this.iMoveDownScore += num4;
		}
		this.iMoveLeftScore = 0;
		this.bCanMoveLeft = false;
		array = new int[this.iCol];
		for (int m = 0; m < this.iRow; m++)
		{
			for (int n = 0; n < this.iCol; n++)
			{
				int num5 = m * this.iCol + n;
				array[n] = this.DataArray[num5];
			}
			if (!this.bCanMoveLeft)
			{
				bool flag3 = this.CheckMove(array);
				if (flag3)
				{
					this.bCanMoveLeft = true;
				}
			}
			int num6 = this.CheckArrayScore(array);
			this.iMoveLeftScore += num6;
		}
		this.iMoveRightScore = 0;
		this.bCanMoveRight = false;
		for (int num7 = 0; num7 < this.iRow; num7++)
		{
			for (int num8 = 0; num8 < this.iCol; num8++)
			{
				int num9 = num7 * this.iCol + (this.iCol - 1 - num8);
				array[num8] = this.DataArray[num9];
			}
			if (!this.bCanMoveRight)
			{
				bool flag4 = this.CheckMove(array);
				if (flag4)
				{
					this.bCanMoveRight = true;
				}
			}
			int num10 = this.CheckArrayScore(array);
			this.iMoveRightScore += num10;
		}
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x00077828 File Offset: 0x00075A28
	private bool CheckMove(int[] VelueArray)
	{
		int num = VelueArray.Length;
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < num; i++)
		{
			if (VelueArray[i] == 0)
			{
				if (num2 < 0)
				{
					num2 = i;
				}
			}
			else if (num3 < 0)
			{
				if (num2 >= 0 && i > num2)
				{
					bool flag = this.CheckTileCanMove(VelueArray[i]);
					if (flag)
					{
						return true;
					}
					num2 = -1;
				}
				num3 = i;
			}
			else
			{
				bool flag;
				if (num2 >= 0 && i > num2)
				{
					flag = this.CheckTileCanMove(VelueArray[i]);
					if (flag)
					{
						return true;
					}
					num2 = -1;
				}
				eTileType eTileType = this.CheckTileMerge(VelueArray[num3], VelueArray[i]);
				flag = false;
				if (eTileType != eTileType.Unknown)
				{
					flag = true;
				}
				if (flag)
				{
					return true;
				}
				num3 = i;
			}
		}
		return false;
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x000778EC File Offset: 0x00075AEC
	private bool CheckTileCanMove(int ival)
	{
		return this.CheckChick(ival) || this.CheckTireChick(ival) || this.CheckBombChick(ival) || ival == 500 || ival == 501 || ival == 502;
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x0007794C File Offset: 0x00075B4C
	private eTileType CheckTileMerge(int iVal1, int iVal2)
	{
		if (this.CheckHole(iVal1, iVal2))
		{
			return eTileType.Hole;
		}
		if (this.CheckBomb(iVal1, iVal2))
		{
			return eTileType.Bomb;
		}
		if (this.CheckCage(iVal1, iVal2))
		{
			return eTileType.Cage;
		}
		if (this.CheckDblCage(iVal1, iVal2))
		{
			return eTileType.DblCage;
		}
		if (this.CheckTire(iVal1, iVal2))
		{
			return eTileType.Tire;
		}
		if (this.CheckRooster(iVal1, iVal2))
		{
			return eTileType.Rooster;
		}
		if (this.CheckRaccoon(iVal1, iVal2))
		{
			return eTileType.Raccoon;
		}
		if (this.CheckFox(iVal1, iVal2))
		{
			return eTileType.Fox;
		}
		if (this.CheckChick(iVal1) && this.CheckChick(iVal2) && iVal1 == iVal2)
		{
			return eTileType.Chick;
		}
		return eTileType.Unknown;
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x00009B68 File Offset: 0x00007D68
	private bool CheckChick(int iVal)
	{
		return iVal >= 1 && iVal <= 11;
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x00009B7C File Offset: 0x00007D7C
	private bool CheckTireChick(int iVal)
	{
		return iVal >= 101 && iVal <= 111;
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x00009B91 File Offset: 0x00007D91
	private bool CheckBombChick(int iVal)
	{
		return iVal >= 611 && iVal <= 697;
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x000779F4 File Offset: 0x00075BF4
	private bool CheckHole(int iVal1, int iVal2)
	{
		return (iVal1 == 505 && this.CheckChick(iVal2)) || (iVal2 == 505 && this.CheckChick(iVal1)) || (iVal1 == 505 && iVal2 == 500) || (iVal2 == 505 && iVal1 == 500) || (iVal1 == 505 && iVal2 == 501) || (iVal2 == 505 && iVal1 == 501) || (iVal1 == 505 && iVal2 == 502) || (iVal2 == 505 && iVal1 == 502) || (iVal1 == 505 && this.CheckTireChick(iVal2)) || (iVal2 == 505 && this.CheckTireChick(iVal1)) || (iVal1 == 505 && this.CheckBombChick(iVal2)) || (iVal2 == 505 && this.CheckBombChick(iVal1));
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x00009BAC File Offset: 0x00007DAC
	private bool CheckFox(int iVal1, int iVal2)
	{
		return (iVal1 == 502 && this.CheckChick(iVal2)) || (iVal2 == 502 && this.CheckChick(iVal1));
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x00077B14 File Offset: 0x00075D14
	private bool CheckRooster(int iVal1, int iVal2)
	{
		return (iVal1 == 500 && iVal2 == 500) || (iVal2 == 500 && iVal1 == 500) || (iVal1 == 500 && iVal2 == 501) || (iVal1 == 500 && iVal2 == 501) || (iVal2 == 500 && iVal1 == 501) || (iVal1 == 500 && iVal2 == 502) || (iVal2 == 500 && iVal1 == 502);
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x00009BDF File Offset: 0x00007DDF
	private bool CheckRaccoon(int iVal1, int iVal2)
	{
		return (iVal1 == 501 && this.CheckChick(iVal2)) || (iVal2 == 501 && this.CheckChick(iVal1));
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x00009C12 File Offset: 0x00007E12
	private bool CheckCage(int iVal1, int iVal2)
	{
		return iVal1 >= 201 && iVal1 <= 211 && iVal1 - iVal2 == 200;
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x00009C3A File Offset: 0x00007E3A
	private bool CheckDblCage(int iVal1, int iVal2)
	{
		return iVal1 >= 401 && iVal1 <= 411 && iVal1 - iVal2 == 400;
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x00009C62 File Offset: 0x00007E62
	private bool CheckTire(int iVal1, int iVal2)
	{
		return (iVal1 >= 101 && iVal1 <= 111 && iVal1 - iVal2 == 100) || (iVal2 >= 101 && iVal2 <= 111 && iVal2 - iVal1 == 100);
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x00077BC0 File Offset: 0x00075DC0
	private bool CheckBomb(int iVal1, int iVal2)
	{
		return (iVal1 >= 611 && iVal1 <= 697 && iVal1 % 10 - iVal2 == 0) || (iVal2 >= 611 && iVal2 <= 697 && iVal2 % 10 - iVal1 == 0);
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x00077C14 File Offset: 0x00075E14
	private int CheeckTargetObs(int iVal1, int iVal2)
	{
		eTileType eTileType = this.CheckTileMerge(iVal1, iVal2);
		if (eTileType == eTileType.Tire)
		{
			return 100;
		}
		if (eTileType == eTileType.Cage)
		{
			return 200;
		}
		if (eTileType == eTileType.DblCage)
		{
			return 400;
		}
		return 0;
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x00077C50 File Offset: 0x00075E50
	private int TileMerge(int iVal1, int iVal2)
	{
		switch (this.CheckTileMerge(iVal1, iVal2))
		{
		case eTileType.Unknown:
			return 0;
		case eTileType.Chick:
			iVal1++;
			return iVal1;
		case eTileType.Rooster:
			iVal1 = 500;
			return 500;
		case eTileType.Raccoon:
			iVal1 = 501;
			return 501;
		case eTileType.Fox:
			iVal1 = 502;
			return 502;
		case eTileType.Cage:
			if (iVal1 >= 201 && iVal1 <= 211)
			{
				return iVal2;
			}
			break;
		case eTileType.Tire:
			if (iVal1 >= 101 && iVal1 <= 111)
			{
				return iVal2;
			}
			if (iVal2 >= 101 && iVal2 <= 111)
			{
				return iVal1;
			}
			break;
		case eTileType.Hole:
			iVal1 = 505;
			return 505;
		case eTileType.DblCage:
			if (iVal1 >= 401 && iVal1 <= 411)
			{
				return iVal2 + 200;
			}
			break;
		case eTileType.Bomb:
			if (iVal1 >= 611 && iVal1 <= 697)
			{
				return iVal2;
			}
			if (iVal2 >= 611 && iVal2 <= 697)
			{
				return iVal1;
			}
			break;
		}
		return 0;
	}

	// Token: 0x06000E70 RID: 3696 RVA: 0x00077D78 File Offset: 0x00075F78
	private int CheckArrayScore(int[] OrigArray)
	{
		int num = OrigArray.Length;
		int[] array = new int[num];
		int num2 = 0;
		int num3 = -1;
		for (int i = 0; i < num; i++)
		{
			if (OrigArray[i] == 0)
			{
				array[i] = 0;
			}
			else if (num3 < 0)
			{
				array[i] = OrigArray[i];
				num3 = i;
			}
			else
			{
				int num4 = this.TileMerge(OrigArray[num3], OrigArray[i]);
				if (num4 != 0)
				{
					array[num3] = num4;
					num2 += this.CalculateScore(num4);
					num3 = -1;
				}
				else
				{
					num3 = i;
					array[i] = OrigArray[i];
				}
			}
		}
		int num5 = -1;
		for (int j = 0; j < num; j++)
		{
			OrigArray[j] = 0;
			if (array[j] == 0)
			{
				if (num5 < 0)
				{
					num5 = j;
				}
			}
			else if (num5 >= 0 && j > num5)
			{
				bool flag = this.CheckTileCanMove(array[j]);
				if (flag)
				{
					OrigArray[num5] = array[j];
					num5++;
				}
				else
				{
					OrigArray[j] = array[j];
					num5 = -1;
				}
			}
			else
			{
				OrigArray[j] = array[j];
			}
		}
		return num2;
	}

	// Token: 0x06000E71 RID: 3697 RVA: 0x00077E98 File Offset: 0x00076098
	private int MoveArrayScore(int[] OrigArray, UISprite[] OrigSprArray)
	{
		int num = OrigArray.Length;
		int[] array = new int[num];
		UISprite[] array2 = new UISprite[num];
		int num2 = 0;
		int num3 = -1;
		for (int i = 0; i < num; i++)
		{
			if (OrigArray[i] == 0)
			{
				array[i] = 0;
				array2[i] = null;
			}
			else if (num3 < 0)
			{
				array[i] = OrigArray[i];
				array2[i] = OrigSprArray[i];
				num3 = i;
			}
			else
			{
				int num4 = this.CheeckTargetObs(OrigArray[num3], OrigArray[i]);
				int num5 = this.TileMerge(OrigArray[num3], OrigArray[i]);
				if (num5 != 0 && num5 != 501 && num5 != 502 && num5 != 505)
				{
					int num6 = this.goalStatus.UpdateGoal(num5);
					if (num6 != 0)
					{
						this.NeedReleaseTileList.Add(OrigSprArray[i].GetComponent<TileData>());
						OrigSprArray[num3].GetComponent<TileData>().MergeAndRemoveTile(OrigSprArray[i].GetComponent<TileData>(), num5, num6);
					}
					else if (num4 != 0)
					{
						num6 = this.goalStatus.UpdateGoal(num4);
						OrigSprArray[i].GetComponent<TileData>().ObstacleTile(num4);
						OrigSprArray[num3].GetComponent<TileData>().MergeTile(OrigSprArray[i].GetComponent<TileData>(), num5, num6);
						if (num6 != 0)
						{
							this.UpdateObs();
						}
					}
					else
					{
						this.NeedReleaseTileList.Add(OrigSprArray[i].GetComponent<TileData>());
						OrigSprArray[num3].GetComponent<TileData>().MergeTile(OrigSprArray[i].GetComponent<TileData>(), num5, 0);
					}
					if (OrigArray[num3] >= 201 && OrigArray[num3] <= 211)
					{
						array[num3] = num5 + 5000;
					}
					else
					{
						array[num3] = num5;
					}
					array2[num3] = OrigSprArray[num3];
					num3 = -1;
					num2 += this.CalculateScore(num5);
				}
				else if (num5 == 501)
				{
					if (OrigArray[num3] == num5)
					{
						int num7 = OrigArray[i];
						UISprite uisprite = OrigSprArray[i];
						array[i] = OrigArray[num3];
						array2[i] = OrigSprArray[num3];
						array[num3] = num7;
						array2[num3] = uisprite;
					}
					else if (OrigArray[i] == num5)
					{
						int num7 = OrigArray[num3];
						UISprite uisprite = OrigSprArray[num3];
						array[num3] = OrigArray[i];
						array2[num3] = OrigSprArray[i];
						array[i] = num7;
						array2[i] = uisprite;
					}
					num3 = -1;
				}
				else if (num5 == 502)
				{
					if (OrigArray[i] == num5)
					{
						int tileValue = OrigArray[num3];
						OrigSprArray[i].GetComponent<TileData>().SetTileValue(tileValue);
						OrigSprArray[num3].GetComponent<TileData>().SetTileValue(num5);
					}
					this.NeedReleaseTileList.Add(OrigSprArray[i].GetComponent<TileData>());
					OrigSprArray[num3].GetComponent<TileData>().MergeTile(OrigSprArray[i].GetComponent<TileData>(), num5, 0);
					array[num3] = num5;
					array2[num3] = OrigSprArray[num3];
					num3 = -1;
				}
				else if (num5 == 505)
				{
					if (OrigArray[num3] == num5 && num3 < i)
					{
						this.NeedReleaseTileList.Add(OrigSprArray[i].GetComponent<TileData>());
						OrigSprArray[num3].GetComponent<TileData>().MergeTile(OrigSprArray[i].GetComponent<TileData>(), num5, 0);
						array[num3] = num5;
						array2[num3] = OrigSprArray[num3];
						num3 = -1;
					}
					else
					{
						num3 = i;
						array[i] = OrigArray[i];
						array2[i] = OrigSprArray[i];
					}
				}
				else
				{
					num3 = i;
					array[i] = OrigArray[i];
					array2[i] = OrigSprArray[i];
				}
			}
		}
		int num8 = -1;
		for (int j = 0; j < num; j++)
		{
			OrigArray[j] = 0;
			OrigSprArray[j] = null;
			if (array[j] == 0)
			{
				if (num8 < 0)
				{
					num8 = j;
				}
			}
			else if (num8 >= 0 && j > num8)
			{
				bool flag = this.CheckTileCanMove(array[j]);
				if (flag)
				{
					OrigArray[num8] = array[j];
					OrigSprArray[num8] = array2[j];
					num8++;
				}
				else
				{
					OrigSprArray[j] = array2[j];
					if (array[j] > 5000)
					{
						OrigArray[j] = array[j] - 5000;
					}
					else
					{
						OrigArray[j] = array[j];
					}
					num8 = -1;
				}
			}
			else
			{
				OrigSprArray[j] = array2[j];
				if (array[j] > 5000)
				{
					OrigArray[j] = array[j] - 5000;
				}
				else
				{
					OrigArray[j] = array[j];
				}
			}
		}
		return num2;
	}

	// Token: 0x06000E72 RID: 3698 RVA: 0x000782F4 File Offset: 0x000764F4
	public int MoveUp()
	{
		if (!this.bCanMoveUp)
		{
			this.PlayBounceEffect("UP");
			return -1;
		}
		int[] array = new int[this.iRow];
		UISprite[] array2 = new UISprite[this.iRow];
		this.iMoveUpScore = 0;
		for (int i = 0; i < this.iCol; i++)
		{
			for (int j = 0; j < this.iRow; j++)
			{
				int num = j * this.iCol + i;
				array[j] = this.DataArray[num];
				array2[j] = this.TileList[num];
			}
			int num2 = this.MoveArrayScore(array, array2);
			this.iMoveUpScore += num2;
			for (int k = 0; k < this.iRow; k++)
			{
				int num3 = k * this.iCol + i;
				this.DataArray[num3] = array[k];
				this.TileList[num3] = array2[k];
				if (array2[k] != null)
				{
					array2[k].GetComponent<TileData>().MoveTo(i, k);
					if (array2[k].GetComponent<TileData>().bRemove)
					{
						this.DataArray[num3] = 881;
						this.TileList[num3] = null;
						this.UpdateTarget();
					}
				}
			}
		}
		return this.iMoveUpScore;
	}

	// Token: 0x06000E73 RID: 3699 RVA: 0x0007843C File Offset: 0x0007663C
	public int MoveDown()
	{
		if (!this.bCanMoveDown)
		{
			this.PlayBounceEffect("DOWN");
			return -1;
		}
		int[] array = new int[this.iRow];
		UISprite[] array2 = new UISprite[this.iRow];
		this.iMoveDownScore = 0;
		for (int i = 0; i < this.iCol; i++)
		{
			for (int j = 0; j < this.iRow; j++)
			{
				int num = (this.iRow - 1 - j) * this.iCol + i;
				array[j] = this.DataArray[num];
				array2[j] = this.TileList[num];
			}
			int num2 = this.MoveArrayScore(array, array2);
			this.iMoveDownScore += num2;
			for (int k = 0; k < this.iRow; k++)
			{
				int num3 = (this.iRow - 1 - k) * this.iCol + i;
				this.DataArray[num3] = array[k];
				this.TileList[num3] = array2[k];
				if (array2[k] != null)
				{
					array2[k].GetComponent<TileData>().MoveTo(i, this.iRow - 1 - k);
					if (array2[k].GetComponent<TileData>().bRemove)
					{
						this.DataArray[num3] = 881;
						this.TileList[num3] = null;
						this.UpdateTarget();
					}
				}
			}
		}
		return this.iMoveDownScore;
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x000785A0 File Offset: 0x000767A0
	public int MoveLeft()
	{
		if (!this.bCanMoveLeft)
		{
			this.PlayBounceEffect("LEFT");
			return -1;
		}
		int[] array = new int[this.iCol];
		UISprite[] array2 = new UISprite[this.iCol];
		this.iMoveLeftScore = 0;
		for (int i = 0; i < this.iRow; i++)
		{
			for (int j = 0; j < this.iCol; j++)
			{
				int num = i * this.iCol + j;
				array[j] = this.DataArray[num];
				array2[j] = this.TileList[num];
			}
			int num2 = this.MoveArrayScore(array, array2);
			this.iMoveLeftScore += num2;
			for (int k = 0; k < this.iCol; k++)
			{
				int num3 = i * this.iCol + k;
				this.DataArray[num3] = array[k];
				this.TileList[num3] = array2[k];
				if (array2[k] != null)
				{
					array2[k].GetComponent<TileData>().MoveTo(k, i);
					if (array2[k].GetComponent<TileData>().bRemove)
					{
						this.DataArray[num3] = 881;
						this.TileList[num3] = null;
						this.UpdateTarget();
					}
				}
			}
		}
		return this.iMoveLeftScore;
	}

	// Token: 0x06000E75 RID: 3701 RVA: 0x000786E8 File Offset: 0x000768E8
	public int MoveRight()
	{
		if (!this.bCanMoveRight)
		{
			this.PlayBounceEffect("RIGHT");
			return -1;
		}
		int[] array = new int[this.iCol];
		UISprite[] array2 = new UISprite[this.iCol];
		this.iMoveRightScore = 0;
		for (int i = 0; i < this.iRow; i++)
		{
			for (int j = 0; j < this.iCol; j++)
			{
				int num = i * this.iCol + (this.iCol - 1 - j);
				array[j] = this.DataArray[num];
				array2[j] = this.TileList[num];
			}
			int num2 = this.MoveArrayScore(array, array2);
			this.iMoveRightScore += num2;
			for (int k = 0; k < this.iCol; k++)
			{
				int num3 = i * this.iCol + (this.iCol - 1 - k);
				this.DataArray[num3] = array[k];
				this.TileList[num3] = array2[k];
				if (array2[k] != null)
				{
					array2[k].GetComponent<TileData>().MoveTo(this.iCol - 1 - k, i);
					if (array2[k].GetComponent<TileData>().bRemove)
					{
						this.DataArray[num3] = 881;
						this.TileList[num3] = null;
						this.UpdateTarget();
					}
				}
			}
		}
		return this.iMoveRightScore;
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x0007884C File Offset: 0x00076A4C
	private void OnKey(KeyCode key)
	{
		switch (key)
		{
		case 273:
			this.OnSwipe("UP");
			break;
		case 274:
			this.OnSwipe("DOWN");
			break;
		case 275:
			this.OnSwipe("RIGHT");
			break;
		case 276:
			this.OnSwipe("LEFT");
			break;
		}
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x000788BC File Offset: 0x00076ABC
	public void OnSwipe(string direction)
	{
		if (this.bBooster)
		{
			return;
		}
		if (this.bGameOver)
		{
			return;
		}
		if (this.bGenerate)
		{
			return;
		}
		if (!this.CheckAllTileMoveFinish())
		{
			return;
		}
		int num = -1;
		if (direction != null)
		{
			if (AlchemyGameControl.<>f__switch$map2 == null)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
				dictionary.Add("UP", 0);
				dictionary.Add("DOWN", 1);
				dictionary.Add("LEFT", 2);
				dictionary.Add("RIGHT", 3);
				AlchemyGameControl.<>f__switch$map2 = dictionary;
			}
			int num2;
			if (AlchemyGameControl.<>f__switch$map2.TryGetValue(direction, ref num2))
			{
				switch (num2)
				{
				case 0:
					num = this.MoveUp();
					break;
				case 1:
					num = this.MoveDown();
					break;
				case 2:
					num = this.MoveLeft();
					break;
				case 3:
					num = this.MoveRight();
					break;
				}
			}
		}
		if (num >= 0)
		{
			this.UpdateMoves();
			this.UpdateScore(num);
			this.bGenerate = true;
			if (!this.bCanMoveUp && !this.bCanMoveDown && !this.bCanMoveLeft && !this.bCanMoveRight)
			{
				this.OpenEndGameScreen(2);
			}
		}
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x000789F8 File Offset: 0x00076BF8
	private void AutoMove()
	{
		int num = -1;
		int num2 = 0;
		int num3 = 0;
		if (num3 < this.iMoveDownScore)
		{
			num2 = 2;
			num3 = this.iMoveDownScore;
		}
		if (num3 < this.iMoveLeftScore)
		{
			num2 = 3;
			num3 = this.iMoveLeftScore;
		}
		if (num3 < this.iMoveUpScore)
		{
			num2 = 1;
			num3 = this.iMoveUpScore;
		}
		if (num3 < this.iMoveRightScore)
		{
			num2 = 4;
			num3 = this.iMoveRightScore;
		}
		if (num2 == 0)
		{
			if (this.bCanMoveDown)
			{
				num2 = 2;
			}
			else if (this.bCanMoveLeft)
			{
				num2 = 3;
			}
			else if (this.bCanMoveUp)
			{
				num2 = 1;
			}
			else
			{
				num2 = 4;
			}
		}
		switch (num2)
		{
		case 1:
			num = this.MoveUp();
			break;
		case 2:
			num = this.MoveDown();
			break;
		case 3:
			num = this.MoveLeft();
			break;
		case 4:
			num = this.MoveRight();
			break;
		}
		if (num >= 0)
		{
			this.UpdateMoves();
			this.UpdateScore(num);
			this.SpawnEgg(this.iPerTurn);
			this.CheckMoveScore();
			if (!this.bCanMoveUp && !this.bCanMoveDown && !this.bCanMoveLeft && !this.bCanMoveRight)
			{
				this.OpenEndGameScreen(2);
			}
		}
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x00078B48 File Offset: 0x00076D48
	public void SaveDataArrayToStringWrite(StringWriter strWriter)
	{
		for (int i = 0; i < 7; i++)
		{
			string text = string.Empty;
			for (int j = 0; j < 7; j++)
			{
				if (j >= this.iCol)
				{
					text += "-1\t";
				}
				else if (i >= this.iRow)
				{
					text += "-1\t";
				}
				else
				{
					int num = i * this.iCol + j;
					int num2 = 0;
					if (this.BaseTileList[num] == null)
					{
						text += "-1\t";
					}
					else if (this.BaseTileList[num].GetAtlasSprite() == null)
					{
						text += "-1\t";
					}
					else if (this.TileList[num] == null)
					{
						text += "0\t";
					}
					else if (this.TileList[num].spriteName.IndexOf("Tile-") >= 0)
					{
						string text2 = this.TileList[num].spriteName.Substring(5);
						if (int.TryParse(text2, ref num2))
						{
							text = text + num2.ToString() + "\t";
						}
						else
						{
							text += "0\t";
						}
					}
					else
					{
						text += "0\t";
					}
				}
			}
			strWriter.WriteLine(text);
		}
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x00078CB4 File Offset: 0x00076EB4
	private void GameLoadDataArrayFormStringRead(StringReader strReader)
	{
		this.SpawnList.Clear();
		for (int i = 0; i < this.iRow; i++)
		{
			string text = strReader.ReadLine();
			int j = 0;
			while (j < this.iCol)
			{
				int num = i * this.iCol + j;
				int num2 = 0;
				int num3 = text.IndexOf("\t");
				string text2;
				if (num3 < 0)
				{
					text2 = text;
				}
				else
				{
					text2 = text.Substring(0, num3);
					text = text.Substring(num3 + 1);
				}
				if (!int.TryParse(text2, ref num2))
				{
					num2 = 0;
				}
				this.DataArray[num] = num2;
				if (num2 >= 0)
				{
					goto IL_12E;
				}
				if (num2 != -1)
				{
					if (num2 == -2 && this.sprBaseTile != null)
					{
						this.TileList[num] = (Object.Instantiate(this.sprBaseTile) as UISprite);
						this.TileList[num].transform.parent = this.sprBaseTile.transform.parent;
						this.TileList[num].name = "Rock";
						this.TileList[num].GetComponent<TileData>().InitTile(this.iCol, this.iRow, j, i);
						this.TileList[num].GetComponent<TileData>().SetTileValue(num2);
						goto IL_12E;
					}
					goto IL_12E;
				}
				IL_258:
				j++;
				continue;
				IL_12E:
				if (num2 == 503)
				{
					this.SpawnList.Add(num);
					this.DataArray[num] = 0;
				}
				if ((j + i) % 2 == 0)
				{
					this.BaseTileList[num] = (Object.Instantiate(this.sprGrassTile_light) as UISprite);
				}
				else
				{
					this.BaseTileList[num] = (Object.Instantiate(this.sprGrassTile_dark) as UISprite);
				}
				this.BaseTileList[num].transform.parent = this.sprGrassTile_light.transform.parent;
				this.BaseTileList[num].GetComponent<TileData>().InitTile(this.iCol, this.iRow, j, i);
				if (num2 > 0)
				{
					this.TileList[num] = (Object.Instantiate(this.sprBaseTile) as UISprite);
					this.TileList[num].transform.parent = this.sprBaseTile.transform.parent;
					this.TileList[num].name = "Tile";
					this.TileList[num].GetComponent<TileData>().InitTile(this.iCol, this.iRow, j, i);
					this.TileList[num].GetComponent<TileData>().SetTileValue(num2);
					goto IL_258;
				}
				goto IL_258;
			}
		}
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x00078F3C File Offset: 0x0007713C
	public void LoadDataArrayFormStringRead(StringReader strReader)
	{
		for (int i = 0; i < this.iRow; i++)
		{
			string text = strReader.ReadLine();
			for (int j = 0; j < this.iCol; j++)
			{
				int num = i * this.iCol + j;
				int num2 = 0;
				int num3 = text.IndexOf("\t");
				string text2;
				if (num3 < 0)
				{
					text2 = text;
				}
				else
				{
					text2 = text.Substring(0, num3);
					text = text.Substring(num3 + 1);
				}
				if (!int.TryParse(text2, ref num2))
				{
					num2 = 0;
				}
				if (num2 < 0)
				{
					this.BaseTileList[num].spriteName = "null";
				}
				else
				{
					this.TileList[num].GetComponent<TileData>().SetTileValue(num2);
				}
			}
		}
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x00079008 File Offset: 0x00077208
	public string GetBackGroundString()
	{
		string text = string.Empty;
		for (int i = 0; i < this.iTotalTile; i++)
		{
			if (this.BaseTileList[i] != null)
			{
				text = text + this.BaseTileList[i].spriteName + "\t";
			}
			else
			{
				text += "null\t";
			}
		}
		return text;
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x00079070 File Offset: 0x00077270
	private void SetBackGroundString(string str)
	{
		int num = 0;
		while (str.Length > 0)
		{
			int num2 = str.IndexOf("\t");
			if (num2 < 0)
			{
				break;
			}
			string spriteName = str.Substring(0, num2);
			str = str.Substring(num2 + 1);
			int startX = num % this.iCol;
			int startY = num / this.iCol;
			this.BaseTileList[num] = (Object.Instantiate(this.sprGrassTile_light) as UISprite);
			this.BaseTileList[num].transform.parent = this.sprGrassTile_light.transform.parent;
			this.BaseTileList[num].GetComponent<TileData>().InitTile(this.iCol, this.iRow, startX, startY);
			this.BaseTileList[num].spriteName = spriteName;
			if (this.BaseTileList[num].GetAtlasSprite() == null)
			{
				this.DataArray[num] = -1;
			}
			num++;
		}
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x00079160 File Offset: 0x00077360
	public void SetBackGroundStringEditMode(string str)
	{
		int num = 0;
		while (str.Length > 0)
		{
			int num2 = str.IndexOf("\t");
			if (num2 < 0)
			{
				break;
			}
			string spriteName = str.Substring(0, num2);
			str = str.Substring(num2 + 1);
			this.BaseTileList[num].spriteName = spriteName;
			if (this.BaseTileList[num].GetAtlasSprite() == null)
			{
				this.DataArray[num] = -1;
			}
			num++;
		}
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x000791E0 File Offset: 0x000773E0
	private void SetTileString(string str)
	{
		int num = 0;
		int num2 = 0;
		while (str.Length > 0)
		{
			num2 = str.IndexOf("\t");
			if (num2 < 0)
			{
				break;
			}
			string text = str.Substring(0, num2);
			str = str.Substring(num2 + 1);
			num2 = 0;
			if (!int.TryParse(text, ref num2))
			{
				num2 = 0;
			}
			if (num2 > 0)
			{
				this.NewTile(num, num2);
				this.DataArray[num] = num2;
			}
			num++;
		}
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x0007925C File Offset: 0x0007745C
	public void SetTileStringEditMode(string str)
	{
		int num = 0;
		int num2 = 0;
		while (str.Length > 0)
		{
			num2 = str.IndexOf("\t");
			if (num2 < 0)
			{
				break;
			}
			string text = str.Substring(0, num2);
			str = str.Substring(num2 + 1);
			num2 = 0;
			if (!int.TryParse(text, ref num2))
			{
				num2 = 0;
			}
			if (num2 > 0)
			{
				this.TileList[num].GetComponent<TileData>().SetTileValue(num2);
				this.DataArray[num] = num2;
			}
			num++;
		}
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x000792E4 File Offset: 0x000774E4
	public string GetTileString()
	{
		string text = string.Empty;
		string text2 = string.Empty;
		int num = 0;
		for (int i = 0; i < this.iTotalTile; i++)
		{
			if (this.TileList[i] == null)
			{
				text += "null\t";
			}
			else if (this.TileList[i].spriteName.IndexOf("Tile-") >= 0)
			{
				text2 = this.TileList[i].spriteName.Substring(5);
				if (int.TryParse(text2, ref num))
				{
					text = text + num.ToString() + "\t";
				}
				else
				{
					text += "null\t";
				}
			}
			else
			{
				text += "null\t";
			}
		}
		return text;
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x000793B0 File Offset: 0x000775B0
	private void LoadGame(int iLoadingID)
	{
		string text = "Level/" + iLoadingID.ToString("0000");
		TextAsset textAsset = (TextAsset)Resources.Load(text, typeof(TextAsset));
		if (textAsset == null)
		{
			text = "Level/" + iLoadingID.ToString();
			textAsset = (TextAsset)Resources.Load(text, typeof(TextAsset));
			if (textAsset == null)
			{
				Debug.Log("Error Loading Game " + text);
			}
		}
		StringReader stringReader = new StringReader(textAsset.text);
		string text2 = string.Empty;
		text2 = stringReader.ReadLine();
		int num;
		if (!int.TryParse(text2, ref num))
		{
			num = 4;
		}
		int num2 = num;
		if (num2 != 100)
		{
			if (num2 == 101)
			{
				this.LoadGameVersion101(stringReader);
			}
		}
		else
		{
			this.LoadGameVersion100(stringReader);
		}
		this.ClearAll();
		this.Rebuild();
		this.InitializeGUILabel();
		this.GameLoadDataArrayFormStringRead(stringReader);
		this.BuildPlusTile();
		this.SpawnEgg(this.iGameStartTile);
		this.UpdateDataArrayHistory();
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x000794C8 File Offset: 0x000776C8
	private void LoadGameVersion100(StringReader myReader)
	{
		string text = string.Empty;
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iRow))
		{
			this.iRow = 4;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iCol))
		{
			this.iCol = 4;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iGameStartTile))
		{
			this.iGameStartTile = 1;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iPerTurn))
		{
			this.iPerTurn = 1;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.maxMoveCount))
		{
			this.maxMoveCount = 999;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iTargetObject1))
		{
			this.iTargetObject1 = 1;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iTargetAmount1))
		{
			this.iTargetAmount1 = 1;
		}
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x000795B8 File Offset: 0x000777B8
	private void LoadGameVersion101(StringReader myReader)
	{
		string text = string.Empty;
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iRow))
		{
			this.iRow = 4;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iCol))
		{
			this.iCol = 4;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iGameStartTile))
		{
			this.iGameStartTile = 1;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iPerTurn))
		{
			this.iPerTurn = 1;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.maxMoveCount))
		{
			this.maxMoveCount = 999;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iTargetObject1))
		{
			this.iTargetObject1 = 0;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iTargetAmount1))
		{
			this.iTargetAmount1 = 0;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iTargetObject2))
		{
			this.iTargetObject2 = 0;
		}
		text = myReader.ReadLine();
		if (!int.TryParse(text, ref this.iTargetAmount2))
		{
			this.iTargetAmount2 = 0;
		}
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x000796E8 File Offset: 0x000778E8
	private void NewTile(int index, int pos)
	{
		if (this.TileList == null)
		{
			return;
		}
		int startX = index % this.iCol;
		int startY = index / this.iCol;
		if (this.TileList[index] == null)
		{
			this.TileList[index] = (Object.Instantiate(this.sprBaseTile) as UISprite);
			this.TileList[index].transform.parent = this.sprBaseTile.transform.parent;
			this.TileList[index].GetComponent<TileData>().iPlus = this.iType * 100;
			this.TileList[index].GetComponent<TileData>().InitTile(this.iCol, this.iRow, startX, startY);
		}
		this.TileList[index].GetComponent<TileData>().SetTileValue(pos);
		this.TileList[index].name = "Tile";
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x000797C0 File Offset: 0x000779C0
	private IEnumerator ReloadGame()
	{
		yield return new WaitForSeconds(2.5f);
		Application.LoadLevel(0);
		yield break;
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x000797D4 File Offset: 0x000779D4
	private bool CheckAllTileMoveFinish()
	{
		for (int i = 0; i < this.TileList.Length; i++)
		{
			if (!(this.TileList[i] == null))
			{
				if (this.TileList[i].GetComponent<TileData>().bMoveing)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x0007982C File Offset: 0x00077A2C
	private void HaveTileToRemove()
	{
		for (int i = 0; i < this.TileList.Length; i++)
		{
			if (!(this.TileList[i] == null))
			{
				if (this.TileList[i].GetComponent<TileData>().bRemove)
				{
					Debug.Log("HaveTileToRemove Index = " + i.ToString());
					this.DataArray[i] = 0;
					this.TileList[i] = null;
					this.UpdateTarget();
				}
			}
		}
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x000798B0 File Offset: 0x00077AB0
	private void OnBooster(GameObject booster)
	{
		int num = Convert.ToInt32(booster.name);
		if (this.boosterObj == null)
		{
			this.boosterObj = booster;
			this.boosterObj.transform.Find("frame").gameObject.SetActive(true);
		}
		else if (this.boosterObj == booster)
		{
			this.boosterObj.transform.Find("frame").gameObject.SetActive(false);
			this.boosterObj = null;
		}
		else
		{
			booster.transform.Find("frame").gameObject.SetActive(true);
			this.boosterObj.transform.Find("frame").gameObject.SetActive(false);
			this.boosterObj = booster;
		}
		switch (num)
		{
		case 0:
			this.bClaw = !this.bClaw;
			this.bPromote = false;
			if (this.bClaw)
			{
				this.ActivateObstacleTween();
				this.UpdateDataArrayHistory(true);
			}
			else
			{
				this.DeactivateObstacleTween();
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.selectedObject = base.gameObject;
			}
			break;
		case 1:
			this.bPromote = !this.bPromote;
			this.bClaw = false;
			this.DeactivateObstacleTween();
			if (this.bPromote)
			{
				this.ActivateChickenTween();
				this.UpdateDataArrayHistory(true);
			}
			else
			{
				this.DeactivateChickenTween();
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.selectedObject = base.gameObject;
			}
			break;
		case 2:
			this.bClaw = false;
			this.bPromote = false;
			this.DeactivateObstacleTween();
			this.DeactivateChickenTween();
			this.ActivateUndoTween();
			booster.transform.Find("frame").gameObject.SetActive(false);
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.selectedObject = base.gameObject;
			break;
		default:
			this.bClaw = false;
			this.bPromote = false;
			this.DeactivateObstacleTween();
			this.DeactivateChickenTween();
			break;
		}
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x00079AB4 File Offset: 0x00077CB4
	private void ActivateObstacleTween()
	{
		for (int i = 0; i < this.iTotalTile; i++)
		{
			if (this.TileList[i] != null && this.DataArray[i] != 505)
			{
				this.ActivateTween(i);
			}
		}
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x00079B08 File Offset: 0x00077D08
	private void ActivateChickenTween()
	{
		for (int i = 0; i < this.iTotalTile; i++)
		{
			if (this.TileList[i] != null && this.DataArray[i] >= 1 && this.DataArray[i] <= 11)
			{
				this.ActivateTween(i);
			}
		}
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x00079B68 File Offset: 0x00077D68
	private void ActivateUndoTween()
	{
		if (this.HistoryDataArray.Count > 1)
		{
			this.DataArray = new int[this.HistoryDataArray[this.HistoryDataArray.Count - 2].Length];
			for (int i = 0; i < this.HistoryDataArray[this.HistoryDataArray.Count - 2].Length; i++)
			{
				this.DataArray[i] = this.HistoryDataArray[this.HistoryDataArray.Count - 2][i];
			}
			this.BombMoves--;
			this.BombStep01.Clear();
			for (int j = 0; j < this.iCol * this.iRow; j++)
			{
				if (this.DataArray[j] > 600)
				{
					this.BombStep01.Add((this.DataArray[j] - 600) / 10 * 10);
				}
			}
			for (int k = 0; k < this.BombStep01.Count; k++)
			{
				this.BombStep01[k] = this.BombStep01[k] - this.BombMoves;
			}
			if (this.TileList != null)
			{
				int num = this.TileList.Length;
				for (int l = 0; l < num; l++)
				{
					if (this.TileList[l] != null)
					{
						Object.DestroyImmediate(this.TileList[l].gameObject);
					}
				}
				this.TileList = null;
			}
			int num2 = this.iRow * this.iCol;
			this.TileList = new UISprite[num2];
			for (int m = 0; m < this.iRow; m++)
			{
				for (int n = 0; n < this.iCol; n++)
				{
					int num3 = m * this.iCol + n;
					this.TileList[num3] = (Object.Instantiate(this.sprBaseTile) as UISprite);
					this.TileList[num3].transform.parent = this.sprBaseTile.transform.parent;
					this.TileList[num3].GetComponent<TileData>().iPlus = this.iType * 100;
					this.TileList[num3].GetComponent<TileData>().InitTile(this.iCol, this.iRow, n, m);
					this.TileList[num3].GetComponent<TileData>().SetTileValue(this.DataArray[num3]);
					this.TileList[num3].name = "Tile";
				}
			}
			if (!this.HistoryBooster[this.HistoryBooster.Count - 1])
			{
				this.totalMoves++;
			}
			if (this.movesLabel != null)
			{
				this.movesLabel.text = this.totalMoves.ToString();
			}
			if (this.movesBar != null)
			{
				this.movesBar.fillAmount = (float)this.totalMoves * 1f / ((float)this.maxMoveCount * 1f);
			}
			if (this.HistoryCombo[this.HistoryCombo.Count - 2] > 0)
			{
				this.totalMultiplier = this.HistoryMultiplier[this.HistoryMultiplier.Count - 2];
				this.totalCombo = this.HistoryCombo[this.HistoryCombo.Count - 2];
				this.comboLabel.text = this.totalCombo.ToString() + " COMBO";
			}
			else
			{
				this.totalMultiplier = this.defaultMultiplier;
				this.totalCombo = 0;
				this.comboLabel.text = string.Empty;
			}
			this.totalScore = this.HistoryScore[this.HistoryScore.Count - 2];
			if (this.scoreLabel != null)
			{
				this.scoreLabel.text = this.totalScore.ToString();
			}
			if (this.scoreBar != null)
			{
				this.scoreBar.fillAmount = (float)this.totalScore * 1f / ((float)this.maxScoreCount * 1f);
			}
			this.iTargetCurrentCount = this.HistoryTargetCount[this.HistoryTargetCount.Count - 2];
			this.CheckMoveScore();
			this.HistoryDataArray.RemoveAt(this.HistoryDataArray.Count - 1);
			this.HistoryBooster.RemoveAt(this.HistoryBooster.Count - 1);
			this.HistoryCombo.RemoveAt(this.HistoryCombo.Count - 1);
			this.HistoryScore.RemoveAt(this.HistoryScore.Count - 1);
			this.HistoryTargetCount.RemoveAt(this.HistoryTargetCount.Count - 1);
			this.HistoryMultiplier.RemoveAt(this.HistoryMultiplier.Count - 1);
		}
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x0007A058 File Offset: 0x00078258
	private void ActivateTween(int index)
	{
		TweenScale component = this.TileList[index].GetComponent<TweenScale>();
		component.from = new Vector3(0.9f, 0.9f, 0.9f);
		component.to = new Vector3(1.05f, 1.05f, 1.05f);
		component.duration = 0.3f;
		component.style = UITweener.Style.PingPong;
		component.ResetToBeginning();
		component.enabled = true;
		this.TileList[index].gameObject.collider.enabled = true;
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x0007A0E0 File Offset: 0x000782E0
	private void DeactivateChickenTween()
	{
		for (int i = 0; i < this.iTotalTile; i++)
		{
			if (this.TileList[i] != null && this.DataArray[i] >= 1 && this.DataArray[i] <= 11)
			{
				this.DeactivateTween(i);
			}
		}
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x0007A140 File Offset: 0x00078340
	private void DeactivateObstacleTween()
	{
		for (int i = 0; i < this.iTotalTile; i++)
		{
			if (this.TileList[i] != null)
			{
				this.DeactivateTween(i);
			}
		}
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x0007A180 File Offset: 0x00078380
	private void DeactivateTween(int index)
	{
		TweenScale component = this.TileList[index].GetComponent<TweenScale>();
		component.PlayForward();
		component.from = new Vector3(1f, 1f, 1f);
		component.to = new Vector3(1.2f, 1.2f, 1.2f);
		component.duration = 0.1f;
		component.style = UITweener.Style.Once;
		component.enabled = false;
		component.ResetToBeginning();
		this.TileList[index].gameObject.collider.enabled = false;
		this.TileList[index].transform.localScale = Vector3.one;
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x0007A228 File Offset: 0x00078428
	private void OnTile(GameObject tile)
	{
		TileData component = tile.GetComponent<TileData>();
		if (this.bClaw)
		{
			this.bClaw = false;
			this.DeactivateObstacleTween();
			if (component.GetTileValue() >= 1 && component.GetTileValue() <= 11)
			{
				int num = component.iNowX + component.iNowY * this.iCol;
				int num2 = this.DataArray[num];
				UISprite uisprite = this.TileList[num];
				this.DataArray[num] = 0;
				this.TileList[num] = null;
				this.CheckMoveScore();
				if (!this.bCanMoveUp && !this.bCanMoveDown && !this.bCanMoveLeft && !this.bCanMoveRight)
				{
					Debug.Log("Removing that with the claw will result in failure");
					this.DataArray[num] = num2;
					this.TileList[num] = uisprite;
				}
				else
				{
					component.RemoveWithClaw();
				}
			}
			else
			{
				component.RemoveWithClaw();
			}
		}
		else if (this.bPromote)
		{
			component.PromoteChicken();
			if (this.goalStatus.UpdateGoal(component.GetTileValue()) != 0)
			{
				component.bRemove = true;
				component.TweenPositionFinish();
				this.HaveTileToRemove();
			}
			this.bPromote = false;
			this.DeactivateChickenTween();
		}
		if (this.boosterObj != null)
		{
			this.boosterObj.transform.Find("frame").gameObject.SetActive(false);
			this.boosterObj = null;
		}
		this.CheckMoveScore();
		UICamera.currentScheme = UICamera.ControlScheme.Controller;
		UICamera.selectedObject = base.gameObject;
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x0007A3A8 File Offset: 0x000785A8
	public void SetDan()
	{
		this.DanSpace = new int[this.iCol * this.iRow];
		for (int i = 0; i < this.iCol * this.iRow; i++)
		{
			if (this.DataArray[i] == 504)
			{
				this.DanSpace[i] = -4;
			}
			if (this.DataArray[i] == 0)
			{
				this.DanSpace[i] = 0;
			}
			if (this.DataArray[i] != 0 && this.DataArray[i] != 504)
			{
				this.DanSpace[i] = -5;
			}
		}
		for (int j = 0; j < this.DanSpace.Length; j++)
		{
			int num = j % this.iCol;
			int num2 = j / this.iCol;
			if (this.DanSpace[j] == -4)
			{
				if (j + 1 < this.DanSpace.Length && (j + 1) % this.iCol != 0 && this.DanSpace[j + 1] == 0)
				{
					this.DanSpace[j + 1] = this.DanSpace[j + 1] + 1;
				}
				if (j - 1 >= 0 && (j - 1) % this.iCol != this.iCol - 1 && this.DanSpace[j - 1] == 0)
				{
					this.DanSpace[j - 1] = this.DanSpace[j - 1] + 1;
				}
				if (j + this.iCol < this.DanSpace.Length && this.DanSpace[j + this.iCol] == 0)
				{
					this.DanSpace[j + this.iCol] = this.DanSpace[j + this.iCol] + 1;
				}
				if (j - this.iCol >= 0 && this.DanSpace[j - this.iCol] == 0)
				{
					this.DanSpace[j - this.iCol] = this.DanSpace[j - this.iCol] + 1;
				}
			}
		}
		foreach (int num3 in this.DanSpace)
		{
			if (num3 > this.MaxDan)
			{
				this.MaxDan = num3;
			}
		}
		this.RandomDanCount = 0;
		if (this.MaxDan > 0)
		{
			this.RandomDan = new List<int>();
			for (int l = 0; l < this.DanSpace.Length; l++)
			{
				if (this.DanSpace[l] == this.MaxDan)
				{
					this.RandomDan.Add(l);
					this.RandomDanCount++;
				}
			}
		}
		if (this.RandomDanCount == 0)
		{
		}
		if (this.RandomDanCount > 0)
		{
			this.Dan();
		}
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x0007A658 File Offset: 0x00078858
	public void Dan()
	{
		this.SpawnDan = this.RandomDan[Random.Range(0, this.RandomDanCount - 1)];
		if (this.bCanSpawnDan)
		{
			if (this.CantDanCount != 0)
			{
				for (int i = 0; i < this.CantDanCount; i++)
				{
					for (int j = 0; j < this.RandomDanCount; j++)
					{
						if (this.CantDan[i] == this.RandomDan[j])
						{
							this.RandomDan.Remove(this.RandomDan[j]);
							this.RandomDanCount--;
						}
					}
				}
				if (this.RandomDanCount > 0)
				{
					this.DanParticle.Clear();
					this.SpawnDan = this.RandomDan[Random.Range(0, this.RandomDanCount - 1)];
					if (this.SpawnDan + 1 < this.iCol * this.iRow && (this.SpawnDan + 1) % this.iCol != 0 && this.DataArray[this.SpawnDan + 1] == 504)
					{
						this.DanParticle.Add(this.SpawnDan + 1);
					}
					if (this.SpawnDan - 1 >= 0 && (this.SpawnDan - 1) % this.iCol != this.iCol - 1 && this.DataArray[this.SpawnDan - 1] == 504)
					{
						this.DanParticle.Add(this.SpawnDan - 1);
					}
					if (this.SpawnDan + this.iCol < this.iCol * this.iRow && this.DataArray[this.SpawnDan + this.iCol] == 504)
					{
						this.DanParticle.Add(this.SpawnDan + this.iCol);
					}
					if (this.SpawnDan - this.iCol >= 0 && this.DataArray[this.SpawnDan - this.iCol] == 504)
					{
						this.DanParticle.Add(this.SpawnDan - this.iCol);
					}
					int x = 0;
					int y = 0;
					int num = this.DanParticle[Random.Range(0, this.DanParticle.Count - 1)];
					if (num % this.iCol > this.SpawnDan % this.iCol)
					{
						x = -1;
					}
					if (num % this.iCol < this.SpawnDan % this.iCol)
					{
						x = 1;
					}
					if (num / this.iCol > this.SpawnDan / this.iCol)
					{
						y = 1;
					}
					if (num / this.iCol < this.SpawnDan / this.iCol)
					{
						y = -1;
					}
					ParticleEmitter peDanSpreadParticle = this.TileList[num].GetComponent<TileData>().peDanSpreadParticle;
					this.TileList[num].GetComponent<TileData>().PlayEffectDan(peDanSpreadParticle, true, x, y);
					this.DataArray[this.SpawnDan] = 504;
					this.TileList[this.SpawnDan] = (Object.Instantiate(this.sprBaseTile) as UISprite);
					this.TileList[this.SpawnDan].transform.parent = this.sprBaseTile.transform.parent;
					this.TileList[this.SpawnDan].GetComponent<TileData>().InitTile(this.iCol, this.iRow, this.SpawnDan % this.iCol, this.SpawnDan / this.iCol);
					this.TileList[this.SpawnDan].GetComponent<TileData>().SetTileValue(504);
					this.TileList[this.SpawnDan].name = "Tile";
				}
			}
			if (this.CantDanCount == 0)
			{
				this.DanParticle.Clear();
				if (this.SpawnDan + 1 < this.iCol * this.iRow && (this.SpawnDan + 1) % this.iCol != 0 && this.DataArray[this.SpawnDan + 1] == 504)
				{
					this.DanParticle.Add(this.SpawnDan + 1);
				}
				if (this.SpawnDan - 1 >= 0 && (this.SpawnDan - 1) % this.iCol != this.iCol - 1 && this.DataArray[this.SpawnDan - 1] == 504)
				{
					this.DanParticle.Add(this.SpawnDan - 1);
				}
				if (this.SpawnDan + this.iCol < this.iCol * this.iRow && this.DataArray[this.SpawnDan + this.iCol] == 504)
				{
					this.DanParticle.Add(this.SpawnDan + this.iCol);
				}
				if (this.SpawnDan - this.iCol >= 0 && this.DataArray[this.SpawnDan - this.iCol] == 504)
				{
					this.DanParticle.Add(this.SpawnDan - this.iCol);
				}
				int x2 = 0;
				int y2 = 0;
				int num2 = this.DanParticle[Random.Range(0, this.DanParticle.Count - 1)];
				if (num2 % this.iCol > this.SpawnDan % this.iCol)
				{
					x2 = -1;
				}
				if (num2 % this.iCol < this.SpawnDan % this.iCol)
				{
					x2 = 1;
				}
				if (num2 / this.iCol > this.SpawnDan / this.iCol)
				{
					y2 = 1;
				}
				if (num2 / this.iCol < this.SpawnDan / this.iCol)
				{
					y2 = -1;
				}
				ParticleEmitter peDanSpreadParticle2 = this.TileList[num2].GetComponent<TileData>().peDanSpreadParticle;
				this.TileList[num2].GetComponent<TileData>().PlayEffectDan(peDanSpreadParticle2, true, x2, y2);
				this.DataArray[this.SpawnDan] = 504;
				this.TileList[this.SpawnDan] = (Object.Instantiate(this.sprBaseTile) as UISprite);
				this.TileList[this.SpawnDan].transform.parent = this.sprBaseTile.transform.parent;
				this.TileList[this.SpawnDan].GetComponent<TileData>().InitTile(this.iCol, this.iRow, this.SpawnDan % this.iCol, this.SpawnDan / this.iCol);
				this.TileList[this.SpawnDan].GetComponent<TileData>().SetTileValue(504);
				this.TileList[this.SpawnDan].name = "Dan";
			}
		}
		if (!this.bCanSpawnDan)
		{
		}
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x0007AD20 File Offset: 0x00078F20
	private void BuildPlusTile()
	{
		for (int i = -1; i <= this.iRow; i++)
		{
			for (int j = -1; j <= this.iCol; j++)
			{
				int tileValue = this.GetTileValue(j, i);
				if (tileValue == -1)
				{
					int num = 0;
					int tileValue2 = this.GetTileValue(j, i - 1);
					int tileValue3 = this.GetTileValue(j - 1, i);
					int tileValue4 = this.GetTileValue(j + 1, i);
					int tileValue5 = this.GetTileValue(j, i + 1);
					int tileValue6 = this.GetTileValue(j - 1, i - 1);
					int tileValue7 = this.GetTileValue(j + 1, i - 1);
					int tileValue8 = this.GetTileValue(j - 1, i + 1);
					int tileValue9 = this.GetTileValue(j + 1, i + 1);
					if (tileValue2 != -1)
					{
						num++;
					}
					if (tileValue3 != -1)
					{
						num += 2;
					}
					if (tileValue4 != -1)
					{
						num += 4;
					}
					if (tileValue5 != -1)
					{
						num += 8;
					}
					if (tileValue6 != -1)
					{
						num += 16;
					}
					if (tileValue7 != -1)
					{
						num += 32;
					}
					if (tileValue8 != -1)
					{
						num += 64;
					}
					if (tileValue9 != -1)
					{
						num += 128;
					}
					if (num != 0)
					{
						switch (num & 15)
						{
						case 0:
							if ((num & 16) == 16)
							{
								this.AddPlusTile(j, i, "A-9");
							}
							if ((num & 32) == 32)
							{
								this.AddPlusTile(j, i, "A-7");
							}
							if ((num & 64) == 64)
							{
								this.AddPlusTile(j, i, "A-3");
							}
							if ((num & 128) == 128)
							{
								this.AddPlusTile(j, i, "A-1");
							}
							break;
						case 1:
							this.AddPlusTile(j, i, "B-2");
							if ((num & 64) == 64)
							{
								this.AddPlusTile(j, i, "A-3");
							}
							if ((num & 128) == 128)
							{
								this.AddPlusTile(j, i, "A-1");
							}
							break;
						case 2:
							this.AddPlusTile(j, i, "B-4");
							if ((num & 32) == 32)
							{
								this.AddPlusTile(j, i, "A-7");
							}
							if ((num & 128) == 128)
							{
								this.AddPlusTile(j, i, "A-1");
							}
							break;
						case 3:
							this.AddPlusTile(j, i, "B-1");
							if ((num & 128) == 128)
							{
								this.AddPlusTile(j, i, "A-1");
							}
							break;
						case 4:
							this.AddPlusTile(j, i, "B-6");
							if ((num & 64) == 64)
							{
								this.AddPlusTile(j, i, "A-3");
							}
							if ((num & 16) == 16)
							{
								this.AddPlusTile(j, i, "A-9");
							}
							break;
						case 5:
							this.AddPlusTile(j, i, "B-3");
							if ((num & 64) == 64)
							{
								this.AddPlusTile(j, i, "A-3");
							}
							break;
						case 6:
							this.AddPlusTile(j, i, "B-4");
							this.AddPlusTile(j, i, "B-6");
							break;
						case 7:
							this.AddPlusTile(j, i, "B-3");
							this.AddPlusTile(j, i, "B-4");
							break;
						case 8:
							this.AddPlusTile(j, i, "B-8");
							if ((num & 16) == 16)
							{
								this.AddPlusTile(j, i, "A-9");
							}
							if ((num & 32) == 32)
							{
								this.AddPlusTile(j, i, "A-7");
							}
							break;
						case 9:
							this.AddPlusTile(j, i, "B-2");
							this.AddPlusTile(j, i, "B-8");
							break;
						case 10:
							this.AddPlusTile(j, i, "B-7");
							if ((num & 32) == 32)
							{
								this.AddPlusTile(j, i, "A-7");
							}
							break;
						case 11:
							this.AddPlusTile(j, i, "B-2");
							this.AddPlusTile(j, i, "B-7");
							break;
						case 12:
							this.AddPlusTile(j, i, "B-9");
							if ((num & 16) == 16)
							{
								this.AddPlusTile(j, i, "A-9");
							}
							break;
						case 13:
							this.AddPlusTile(j, i, "B-2");
							this.AddPlusTile(j, i, "B-9");
							break;
						case 14:
							this.AddPlusTile(j, i, "B-6");
							this.AddPlusTile(j, i, "B-7");
							break;
						case 15:
							this.AddPlusTile(j, i, "B-5");
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x0007B19C File Offset: 0x0007939C
	private void AddPlusTile(int x, int y, string sprName)
	{
		UISprite uisprite = Object.Instantiate(this.sprGrassTile_dark) as UISprite;
		uisprite.transform.parent = this.sprGrassTile_light.transform.parent;
		uisprite.GetComponent<TileData>().InitTile(this.iCol, this.iRow, x, y);
		uisprite.spriteName = sprName;
		this.TilePlusList.Add(uisprite);
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x0007B204 File Offset: 0x00079404
	private int GetTileValue(int x, int y)
	{
		if (x < 0)
		{
			return -1;
		}
		if (y < 0)
		{
			return -1;
		}
		if (x >= this.iCol)
		{
			return -1;
		}
		if (y >= this.iRow)
		{
			return -1;
		}
		int num = y * this.iCol + x;
		return this.DataArray[num];
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x0007B254 File Offset: 0x00079454
	public void QuitGame()
	{
		this.OpenEndGameScreen(4);
		if (this.QuitButton != null)
		{
			Vector3 localPosition = this.QuitButton.transform.localPosition;
			localPosition.x = 888f;
			localPosition.y = 651f;
			TweenPosition.Begin(this.QuitButton, 0.75f, localPosition);
		}
	}

	// Token: 0x040010FD RID: 4349
	public const string directionLEFT = "LEFT";

	// Token: 0x040010FE RID: 4350
	public const string directionRIGHT = "RIGHT";

	// Token: 0x040010FF RID: 4351
	public const string directionUP = "UP";

	// Token: 0x04001100 RID: 4352
	public const string directionDOWN = "DOWN";

	// Token: 0x04001101 RID: 4353
	private const int MAX_COL = 7;

	// Token: 0x04001102 RID: 4354
	private const int MAX_ROW = 7;

	// Token: 0x04001103 RID: 4355
	private const int DELAY_SWPAN_EGG = 881;

	// Token: 0x04001104 RID: 4356
	private const int BREAK_CAGE_DONT_MOVE = 5000;

	// Token: 0x04001105 RID: 4357
	public GameObject QuitButton;

	// Token: 0x04001106 RID: 4358
	public UISprite sprBaseTile;

	// Token: 0x04001107 RID: 4359
	public UISprite sprGrassTile_light;

	// Token: 0x04001108 RID: 4360
	public UISprite sprGrassTile_dark;

	// Token: 0x04001109 RID: 4361
	private int iType;

	// Token: 0x0400110A RID: 4362
	public int iCol = 7;

	// Token: 0x0400110B RID: 4363
	public int iRow = 7;

	// Token: 0x0400110C RID: 4364
	public int iGameStartTile = 1;

	// Token: 0x0400110D RID: 4365
	public int iPerTurn = 1;

	// Token: 0x0400110E RID: 4366
	public int iTargetObject1 = 4;

	// Token: 0x0400110F RID: 4367
	public int iTargetAmount1 = 10;

	// Token: 0x04001110 RID: 4368
	public int iTargetObject2;

	// Token: 0x04001111 RID: 4369
	public int iTargetAmount2;

	// Token: 0x04001112 RID: 4370
	public GoalStatus goalStatus;

	// Token: 0x04001113 RID: 4371
	public float fAutoMoveDealyTime = 0.5f;

	// Token: 0x04001114 RID: 4372
	public bool bAutoMove;

	// Token: 0x04001115 RID: 4373
	public int iLoadGameId = 1000;

	// Token: 0x04001116 RID: 4374
	private int iLoadlevel;

	// Token: 0x04001117 RID: 4375
	private float fNowTime;

	// Token: 0x04001118 RID: 4376
	public int iTotalTile = 49;

	// Token: 0x04001119 RID: 4377
	private int iLastGeneratePos;

	// Token: 0x0400111A RID: 4378
	public UISprite[] TileList;

	// Token: 0x0400111B RID: 4379
	public UISprite[] BaseTileList;

	// Token: 0x0400111C RID: 4380
	public int[] DataArray;

	// Token: 0x0400111D RID: 4381
	private List<int[]> HistoryDataArray = new List<int[]>();

	// Token: 0x0400111E RID: 4382
	private List<int> HistoryScore = new List<int>();

	// Token: 0x0400111F RID: 4383
	private List<int> HistoryCombo = new List<int>();

	// Token: 0x04001120 RID: 4384
	private List<int> HistoryTargetCount = new List<int>();

	// Token: 0x04001121 RID: 4385
	private List<bool> HistoryBooster = new List<bool>();

	// Token: 0x04001122 RID: 4386
	private List<float> HistoryMultiplier = new List<float>();

	// Token: 0x04001123 RID: 4387
	private int[] DanSpace;

	// Token: 0x04001124 RID: 4388
	private List<int> RandomDan;

	// Token: 0x04001125 RID: 4389
	private int MaxDan;

	// Token: 0x04001126 RID: 4390
	private int SpawnDan;

	// Token: 0x04001127 RID: 4391
	public List<int> CantDan;

	// Token: 0x04001128 RID: 4392
	public int RandomDanCount;

	// Token: 0x04001129 RID: 4393
	public int CantDanCount;

	// Token: 0x0400112A RID: 4394
	private bool bCanSpawnDan;

	// Token: 0x0400112B RID: 4395
	public List<int> DanParticle;

	// Token: 0x0400112C RID: 4396
	public List<int> BombStep01;

	// Token: 0x0400112D RID: 4397
	public int BombMoves;

	// Token: 0x0400112E RID: 4398
	private bool bCanMoveUp;

	// Token: 0x0400112F RID: 4399
	private bool bCanMoveDown;

	// Token: 0x04001130 RID: 4400
	private bool bCanMoveLeft;

	// Token: 0x04001131 RID: 4401
	private bool bCanMoveRight;

	// Token: 0x04001132 RID: 4402
	private bool bGameOver;

	// Token: 0x04001133 RID: 4403
	private int iMoveUpScore;

	// Token: 0x04001134 RID: 4404
	private int iMoveDownScore;

	// Token: 0x04001135 RID: 4405
	private int iMoveLeftScore;

	// Token: 0x04001136 RID: 4406
	private int iMoveRightScore;

	// Token: 0x04001137 RID: 4407
	private int totalMoves = 999;

	// Token: 0x04001138 RID: 4408
	public int maxMoveCount = 999;

	// Token: 0x04001139 RID: 4409
	private int totalScore;

	// Token: 0x0400113A RID: 4410
	private int maxScoreCount = 5000;

	// Token: 0x0400113B RID: 4411
	private int totalCombo;

	// Token: 0x0400113C RID: 4412
	private float totalMultiplier = 1f;

	// Token: 0x0400113D RID: 4413
	private float defaultMultiplier = 1f;

	// Token: 0x0400113E RID: 4414
	private float multiplierIncrement = 0.1f;

	// Token: 0x0400113F RID: 4415
	private bool previousWasCombo;

	// Token: 0x04001140 RID: 4416
	private UILabel movesLabel;

	// Token: 0x04001141 RID: 4417
	private UISprite movesBar;

	// Token: 0x04001142 RID: 4418
	private UILabel scoreLabel;

	// Token: 0x04001143 RID: 4419
	private UISprite scoreBar;

	// Token: 0x04001144 RID: 4420
	private UILabel comboLabel;

	// Token: 0x04001145 RID: 4421
	private int iTargetCurrentCount;

	// Token: 0x04001146 RID: 4422
	private UILabel targetLabel;

	// Token: 0x04001147 RID: 4423
	private int iObstacleCurrentCount;

	// Token: 0x04001148 RID: 4424
	private UILabel obsLabel;

	// Token: 0x04001149 RID: 4425
	private UILabel targetNameLabel;

	// Token: 0x0400114A RID: 4426
	private string targetName = string.Empty;

	// Token: 0x0400114B RID: 4427
	private float fGenerateDelay = 0.1f;

	// Token: 0x0400114C RID: 4428
	private float fGeneratePos;

	// Token: 0x0400114D RID: 4429
	private bool bGenerate;

	// Token: 0x0400114E RID: 4430
	private bool bCanMove = true;

	// Token: 0x0400114F RID: 4431
	private bool bStartGame;

	// Token: 0x04001150 RID: 4432
	private bool bBooster;

	// Token: 0x04001151 RID: 4433
	private bool bClaw;

	// Token: 0x04001152 RID: 4434
	private bool bPromote;

	// Token: 0x04001153 RID: 4435
	private GameObject boosterObj;

	// Token: 0x04001154 RID: 4436
	private List<TileData> NeedReleaseTileList = new List<TileData>();

	// Token: 0x04001155 RID: 4437
	private bool canDestroyRock;

	// Token: 0x04001156 RID: 4438
	private GameObject itemObj;

	// Token: 0x04001157 RID: 4439
	private List<int> SpawnList = new List<int>();

	// Token: 0x04001158 RID: 4440
	private List<UISprite> TilePlusList = new List<UISprite>();
}
