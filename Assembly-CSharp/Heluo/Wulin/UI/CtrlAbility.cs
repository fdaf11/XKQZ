using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002E9 RID: 745
	public class CtrlAbility : MonoBehaviour
	{
		// Token: 0x06000F55 RID: 3925 RVA: 0x000807B0 File Offset: 0x0007E9B0
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

		// Token: 0x06000F56 RID: 3926 RVA: 0x00080AD4 File Offset: 0x0007ECD4
		private void InitializeGUILabel()
		{
			Transform transform = base.gameObject.transform.Find("Group/AlchemyBowl/Move/MoveCounter");
			if (null != transform)
			{
				this.movesLabel = transform.gameObject.GetComponent<UILabel>();
				this.movesLabel.text = this.maxMoveCount.ToString();
				this.totalMoves = this.maxMoveCount;
			}
			transform = base.gameObject.transform.Find("Group/AlchemyBowl/RequestBackGround/TargetTitle/TargetTitleLabel");
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

		// Token: 0x06000F57 RID: 3927 RVA: 0x00080CBC File Offset: 0x0007EEBC
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

		// Token: 0x06000F58 RID: 3928 RVA: 0x00080E9C File Offset: 0x0007F09C
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

		// Token: 0x06000F59 RID: 3929 RVA: 0x00080F24 File Offset: 0x0007F124
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

		// Token: 0x06000F5A RID: 3930 RVA: 0x00081144 File Offset: 0x0007F344
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

		// Token: 0x06000F5B RID: 3931 RVA: 0x00081380 File Offset: 0x0007F580
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

		// Token: 0x06000F5C RID: 3932 RVA: 0x000813E0 File Offset: 0x0007F5E0
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

		// Token: 0x06000F5D RID: 3933 RVA: 0x00081448 File Offset: 0x0007F648
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

		// Token: 0x06000F5E RID: 3934 RVA: 0x000814D0 File Offset: 0x0007F6D0
		private int CalculateScore(int power)
		{
			int num = (int)Mathf.Pow(2f, (float)power);
			int num2 = Convert.ToInt32(this.totalMultiplier * 10f);
			return num * num2;
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00081500 File Offset: 0x0007F700
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

		// Token: 0x06000F60 RID: 3936 RVA: 0x000815AC File Offset: 0x0007F7AC
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

		// Token: 0x06000F61 RID: 3937 RVA: 0x00081634 File Offset: 0x0007F834
		private IEnumerator _SetEnd(bool bEndType)
		{
			yield return new WaitForSeconds(1f);
			base.SendMessage("SetEnd", bEndType);
			yield break;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x0000A575 File Offset: 0x00008775
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

		// Token: 0x06000F63 RID: 3939 RVA: 0x0000A5B1 File Offset: 0x000087B1
		private void UpdateTarget()
		{
			if (this.goalStatus.CheckGoalFinish())
			{
				this.OpenEndGameScreen(0);
			}
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0000A5B1 File Offset: 0x000087B1
		public void UpdateObs()
		{
			if (this.goalStatus.CheckGoalFinish())
			{
				this.OpenEndGameScreen(0);
			}
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00081660 File Offset: 0x0007F860
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

		// Token: 0x06000F66 RID: 3942 RVA: 0x000816C4 File Offset: 0x0007F8C4
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

		// Token: 0x06000F67 RID: 3943 RVA: 0x00081704 File Offset: 0x0007F904
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

		// Token: 0x06000F68 RID: 3944 RVA: 0x00081838 File Offset: 0x0007FA38
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

		// Token: 0x06000F69 RID: 3945 RVA: 0x0000A5CA File Offset: 0x000087CA
		private void UpdateDataArrayHistory()
		{
			this.UpdateDataArrayHistory(false);
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0008194C File Offset: 0x0007FB4C
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

		// Token: 0x06000F6B RID: 3947 RVA: 0x000819E4 File Offset: 0x0007FBE4
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

		// Token: 0x06000F6C RID: 3948 RVA: 0x00081C78 File Offset: 0x0007FE78
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

		// Token: 0x06000F6D RID: 3949 RVA: 0x00081D3C File Offset: 0x0007FF3C
		private bool CheckTileCanMove(int ival)
		{
			return this.CheckChick(ival) || this.CheckTireChick(ival) || this.CheckBombChick(ival) || ival == 500 || ival == 501 || ival == 502;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00081D9C File Offset: 0x0007FF9C
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

		// Token: 0x06000F6F RID: 3951 RVA: 0x00009B68 File Offset: 0x00007D68
		private bool CheckChick(int iVal)
		{
			return iVal >= 1 && iVal <= 11;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00009B7C File Offset: 0x00007D7C
		private bool CheckTireChick(int iVal)
		{
			return iVal >= 101 && iVal <= 111;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00009B91 File Offset: 0x00007D91
		private bool CheckBombChick(int iVal)
		{
			return iVal >= 611 && iVal <= 697;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00081E44 File Offset: 0x00080044
		private bool CheckHole(int iVal1, int iVal2)
		{
			return (iVal1 == 505 && this.CheckChick(iVal2)) || (iVal2 == 505 && this.CheckChick(iVal1)) || (iVal1 == 505 && iVal2 == 500) || (iVal2 == 505 && iVal1 == 500) || (iVal1 == 505 && iVal2 == 501) || (iVal2 == 505 && iVal1 == 501) || (iVal1 == 505 && iVal2 == 502) || (iVal2 == 505 && iVal1 == 502) || (iVal1 == 505 && this.CheckTireChick(iVal2)) || (iVal2 == 505 && this.CheckTireChick(iVal1)) || (iVal1 == 505 && this.CheckBombChick(iVal2)) || (iVal2 == 505 && this.CheckBombChick(iVal1));
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0000A5D3 File Offset: 0x000087D3
		private bool CheckFox(int iVal1, int iVal2)
		{
			return (iVal1 == 502 && this.CheckChick(iVal2)) || (iVal2 == 502 && this.CheckChick(iVal1));
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00077B14 File Offset: 0x00075D14
		private bool CheckRooster(int iVal1, int iVal2)
		{
			return (iVal1 == 500 && iVal2 == 500) || (iVal2 == 500 && iVal1 == 500) || (iVal1 == 500 && iVal2 == 501) || (iVal1 == 500 && iVal2 == 501) || (iVal2 == 500 && iVal1 == 501) || (iVal1 == 500 && iVal2 == 502) || (iVal2 == 500 && iVal1 == 502);
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0000A606 File Offset: 0x00008806
		private bool CheckRaccoon(int iVal1, int iVal2)
		{
			return (iVal1 == 501 && this.CheckChick(iVal2)) || (iVal2 == 501 && this.CheckChick(iVal1));
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00009C12 File Offset: 0x00007E12
		private bool CheckCage(int iVal1, int iVal2)
		{
			return iVal1 >= 201 && iVal1 <= 211 && iVal1 - iVal2 == 200;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00009C3A File Offset: 0x00007E3A
		private bool CheckDblCage(int iVal1, int iVal2)
		{
			return iVal1 >= 401 && iVal1 <= 411 && iVal1 - iVal2 == 400;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00009C62 File Offset: 0x00007E62
		private bool CheckTire(int iVal1, int iVal2)
		{
			return (iVal1 >= 101 && iVal1 <= 111 && iVal1 - iVal2 == 100) || (iVal2 >= 101 && iVal2 <= 111 && iVal2 - iVal1 == 100);
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x00077BC0 File Offset: 0x00075DC0
		private bool CheckBomb(int iVal1, int iVal2)
		{
			return (iVal1 >= 611 && iVal1 <= 697 && iVal1 % 10 - iVal2 == 0) || (iVal2 >= 611 && iVal2 <= 697 && iVal2 % 10 - iVal1 == 0);
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00081F64 File Offset: 0x00080164
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

		// Token: 0x06000F7B RID: 3963 RVA: 0x00081FA0 File Offset: 0x000801A0
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

		// Token: 0x06000F7C RID: 3964 RVA: 0x000820C8 File Offset: 0x000802C8
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

		// Token: 0x06000F7D RID: 3965 RVA: 0x000821E8 File Offset: 0x000803E8
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

		// Token: 0x06000F7E RID: 3966 RVA: 0x00082644 File Offset: 0x00080844
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

		// Token: 0x06000F7F RID: 3967 RVA: 0x0008278C File Offset: 0x0008098C
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

		// Token: 0x06000F80 RID: 3968 RVA: 0x000828F0 File Offset: 0x00080AF0
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

		// Token: 0x06000F81 RID: 3969 RVA: 0x00082A38 File Offset: 0x00080C38
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

		// Token: 0x06000F82 RID: 3970 RVA: 0x00082B9C File Offset: 0x00080D9C
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
				if (CtrlAbility.<>f__switch$map7 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("UP", 0);
					dictionary.Add("DOWN", 1);
					dictionary.Add("LEFT", 2);
					dictionary.Add("RIGHT", 3);
					CtrlAbility.<>f__switch$map7 = dictionary;
				}
				int num2;
				if (CtrlAbility.<>f__switch$map7.TryGetValue(direction, ref num2))
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

		// Token: 0x06000F83 RID: 3971 RVA: 0x00082CD8 File Offset: 0x00080ED8
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

		// Token: 0x06000F84 RID: 3972 RVA: 0x00082E28 File Offset: 0x00081028
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

		// Token: 0x06000F85 RID: 3973 RVA: 0x00082F94 File Offset: 0x00081194
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

		// Token: 0x06000F86 RID: 3974 RVA: 0x0008321C File Offset: 0x0008141C
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

		// Token: 0x06000F87 RID: 3975 RVA: 0x000832E8 File Offset: 0x000814E8
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

		// Token: 0x06000F88 RID: 3976 RVA: 0x00083350 File Offset: 0x00081550
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

		// Token: 0x06000F89 RID: 3977 RVA: 0x00083440 File Offset: 0x00081640
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

		// Token: 0x06000F8A RID: 3978 RVA: 0x000834C0 File Offset: 0x000816C0
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

		// Token: 0x06000F8B RID: 3979 RVA: 0x0008353C File Offset: 0x0008173C
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

		// Token: 0x06000F8C RID: 3980 RVA: 0x000835C4 File Offset: 0x000817C4
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

		// Token: 0x06000F8D RID: 3981 RVA: 0x00083690 File Offset: 0x00081890
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

		// Token: 0x06000F8E RID: 3982 RVA: 0x00083768 File Offset: 0x00081968
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

		// Token: 0x06000F8F RID: 3983 RVA: 0x000837C0 File Offset: 0x000819C0
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

		// Token: 0x06000F90 RID: 3984 RVA: 0x00083844 File Offset: 0x00081A44
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

		// Token: 0x06000F91 RID: 3985 RVA: 0x00083A48 File Offset: 0x00081C48
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

		// Token: 0x06000F92 RID: 3986 RVA: 0x00083A98 File Offset: 0x00081C98
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

		// Token: 0x06000F93 RID: 3987 RVA: 0x00083AF4 File Offset: 0x00081CF4
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

		// Token: 0x06000F94 RID: 3988 RVA: 0x00083FE4 File Offset: 0x000821E4
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

		// Token: 0x06000F95 RID: 3989 RVA: 0x0008406C File Offset: 0x0008226C
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

		// Token: 0x06000F96 RID: 3990 RVA: 0x000840C8 File Offset: 0x000822C8
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

		// Token: 0x06000F97 RID: 3991 RVA: 0x00084108 File Offset: 0x00082308
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

		// Token: 0x06000F98 RID: 3992 RVA: 0x000841B0 File Offset: 0x000823B0
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

		// Token: 0x06000F99 RID: 3993 RVA: 0x00084330 File Offset: 0x00082530
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
			foreach (int num in this.DanSpace)
			{
				if (num > this.MaxDan)
				{
					this.MaxDan = num;
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

		// Token: 0x06000F9A RID: 3994 RVA: 0x000845C8 File Offset: 0x000827C8
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

		// Token: 0x06000F9B RID: 3995 RVA: 0x00084C90 File Offset: 0x00082E90
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

		// Token: 0x06000F9C RID: 3996 RVA: 0x0008510C File Offset: 0x0008330C
		private void AddPlusTile(int x, int y, string sprName)
		{
			UISprite uisprite = Object.Instantiate(this.sprGrassTile_dark) as UISprite;
			uisprite.transform.parent = this.sprGrassTile_light.transform.parent;
			uisprite.GetComponent<TileData>().InitTile(this.iCol, this.iRow, x, y);
			uisprite.spriteName = sprName;
			this.TilePlusList.Add(uisprite);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00085174 File Offset: 0x00083374
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

		// Token: 0x06000F9E RID: 3998 RVA: 0x000851C4 File Offset: 0x000833C4
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

		// Token: 0x0400124E RID: 4686
		public const string directionLEFT = "LEFT";

		// Token: 0x0400124F RID: 4687
		public const string directionRIGHT = "RIGHT";

		// Token: 0x04001250 RID: 4688
		public const string directionUP = "UP";

		// Token: 0x04001251 RID: 4689
		public const string directionDOWN = "DOWN";

		// Token: 0x04001252 RID: 4690
		private const int MAX_COL = 7;

		// Token: 0x04001253 RID: 4691
		private const int MAX_ROW = 7;

		// Token: 0x04001254 RID: 4692
		private const int DELAY_SWPAN_EGG = 881;

		// Token: 0x04001255 RID: 4693
		private const int BREAK_CAGE_DONT_MOVE = 5000;

		// Token: 0x04001256 RID: 4694
		public UIAbility m_UIAbility;

		// Token: 0x04001257 RID: 4695
		public GameObject QuitButton;

		// Token: 0x04001258 RID: 4696
		public UISprite sprBaseTile;

		// Token: 0x04001259 RID: 4697
		public UISprite sprGrassTile_light;

		// Token: 0x0400125A RID: 4698
		public UISprite sprGrassTile_dark;

		// Token: 0x0400125B RID: 4699
		private int iType;

		// Token: 0x0400125C RID: 4700
		public int iCol = 7;

		// Token: 0x0400125D RID: 4701
		public int iRow = 7;

		// Token: 0x0400125E RID: 4702
		public int iGameStartTile = 1;

		// Token: 0x0400125F RID: 4703
		public int iPerTurn = 1;

		// Token: 0x04001260 RID: 4704
		public int iTargetObject1 = 4;

		// Token: 0x04001261 RID: 4705
		public int iTargetAmount1 = 10;

		// Token: 0x04001262 RID: 4706
		public int iTargetObject2;

		// Token: 0x04001263 RID: 4707
		public int iTargetAmount2;

		// Token: 0x04001264 RID: 4708
		public GoalStatus goalStatus;

		// Token: 0x04001265 RID: 4709
		public float fAutoMoveDealyTime = 0.5f;

		// Token: 0x04001266 RID: 4710
		public bool bAutoMove;

		// Token: 0x04001267 RID: 4711
		public int iLoadGameId = 1000;

		// Token: 0x04001268 RID: 4712
		private int iLoadlevel;

		// Token: 0x04001269 RID: 4713
		private float fNowTime;

		// Token: 0x0400126A RID: 4714
		public int iTotalTile = 49;

		// Token: 0x0400126B RID: 4715
		private int iLastGeneratePos;

		// Token: 0x0400126C RID: 4716
		public UISprite[] TileList;

		// Token: 0x0400126D RID: 4717
		public UISprite[] BaseTileList;

		// Token: 0x0400126E RID: 4718
		public int[] DataArray;

		// Token: 0x0400126F RID: 4719
		private List<int[]> HistoryDataArray = new List<int[]>();

		// Token: 0x04001270 RID: 4720
		private List<int> HistoryScore = new List<int>();

		// Token: 0x04001271 RID: 4721
		private List<int> HistoryCombo = new List<int>();

		// Token: 0x04001272 RID: 4722
		private List<int> HistoryTargetCount = new List<int>();

		// Token: 0x04001273 RID: 4723
		private List<bool> HistoryBooster = new List<bool>();

		// Token: 0x04001274 RID: 4724
		private List<float> HistoryMultiplier = new List<float>();

		// Token: 0x04001275 RID: 4725
		private int[] DanSpace;

		// Token: 0x04001276 RID: 4726
		private List<int> RandomDan;

		// Token: 0x04001277 RID: 4727
		private int MaxDan;

		// Token: 0x04001278 RID: 4728
		private int SpawnDan;

		// Token: 0x04001279 RID: 4729
		public List<int> CantDan;

		// Token: 0x0400127A RID: 4730
		public int RandomDanCount;

		// Token: 0x0400127B RID: 4731
		public int CantDanCount;

		// Token: 0x0400127C RID: 4732
		private bool bCanSpawnDan;

		// Token: 0x0400127D RID: 4733
		public List<int> DanParticle;

		// Token: 0x0400127E RID: 4734
		public List<int> BombStep01;

		// Token: 0x0400127F RID: 4735
		public int BombMoves;

		// Token: 0x04001280 RID: 4736
		private bool bCanMoveUp;

		// Token: 0x04001281 RID: 4737
		private bool bCanMoveDown;

		// Token: 0x04001282 RID: 4738
		private bool bCanMoveLeft;

		// Token: 0x04001283 RID: 4739
		private bool bCanMoveRight;

		// Token: 0x04001284 RID: 4740
		private bool bGameOver;

		// Token: 0x04001285 RID: 4741
		private int iMoveUpScore;

		// Token: 0x04001286 RID: 4742
		private int iMoveDownScore;

		// Token: 0x04001287 RID: 4743
		private int iMoveLeftScore;

		// Token: 0x04001288 RID: 4744
		private int iMoveRightScore;

		// Token: 0x04001289 RID: 4745
		private int totalMoves = 999;

		// Token: 0x0400128A RID: 4746
		public int maxMoveCount = 999;

		// Token: 0x0400128B RID: 4747
		private int totalScore;

		// Token: 0x0400128C RID: 4748
		private int maxScoreCount = 5000;

		// Token: 0x0400128D RID: 4749
		private int totalCombo;

		// Token: 0x0400128E RID: 4750
		private float totalMultiplier = 1f;

		// Token: 0x0400128F RID: 4751
		private float defaultMultiplier = 1f;

		// Token: 0x04001290 RID: 4752
		private float multiplierIncrement = 0.1f;

		// Token: 0x04001291 RID: 4753
		private bool previousWasCombo;

		// Token: 0x04001292 RID: 4754
		private UILabel movesLabel;

		// Token: 0x04001293 RID: 4755
		private UISprite movesBar;

		// Token: 0x04001294 RID: 4756
		private UILabel scoreLabel;

		// Token: 0x04001295 RID: 4757
		private UISprite scoreBar;

		// Token: 0x04001296 RID: 4758
		private UILabel comboLabel;

		// Token: 0x04001297 RID: 4759
		private int iTargetCurrentCount;

		// Token: 0x04001298 RID: 4760
		private UILabel targetLabel;

		// Token: 0x04001299 RID: 4761
		private int iObstacleCurrentCount;

		// Token: 0x0400129A RID: 4762
		private UILabel obsLabel;

		// Token: 0x0400129B RID: 4763
		private UILabel targetNameLabel;

		// Token: 0x0400129C RID: 4764
		private string targetName = string.Empty;

		// Token: 0x0400129D RID: 4765
		private float fGenerateDelay = 0.1f;

		// Token: 0x0400129E RID: 4766
		private float fGeneratePos;

		// Token: 0x0400129F RID: 4767
		private bool bGenerate;

		// Token: 0x040012A0 RID: 4768
		private bool bCanMove = true;

		// Token: 0x040012A1 RID: 4769
		private bool bStartGame;

		// Token: 0x040012A2 RID: 4770
		private bool bBooster;

		// Token: 0x040012A3 RID: 4771
		private bool bClaw;

		// Token: 0x040012A4 RID: 4772
		private bool bPromote;

		// Token: 0x040012A5 RID: 4773
		private GameObject boosterObj;

		// Token: 0x040012A6 RID: 4774
		private List<TileData> NeedReleaseTileList = new List<TileData>();

		// Token: 0x040012A7 RID: 4775
		private bool canDestroyRock;

		// Token: 0x040012A8 RID: 4776
		private GameObject itemObj;

		// Token: 0x040012A9 RID: 4777
		private List<int> SpawnList = new List<int>();

		// Token: 0x040012AA RID: 4778
		private List<UISprite> TilePlusList = new List<UISprite>();
	}
}
