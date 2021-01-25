using System;
using System.Collections.Generic;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x020002D9 RID: 729
public class TileData : MonoBehaviour
{
	// Token: 0x06000EAE RID: 3758 RVA: 0x0000264F File Offset: 0x0000084F
	private void Awake()
	{
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x00009D56 File Offset: 0x00007F56
	private void Start()
	{
		this.go = GameObject.Find("cFormAbility").GetComponent<CtrlAbility>();
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x0007B7E8 File Offset: 0x000799E8
	public void InitTile(int iCol, int iRow, int StartX, int StartY)
	{
		switch (iCol)
		{
		case 3:
			this.fStartOffsetX = -150f;
			break;
		case 4:
			this.fStartOffsetX = -225f;
			break;
		case 5:
			this.fStartOffsetX = -300f;
			break;
		case 6:
			this.fStartOffsetX = -375f;
			break;
		case 7:
			this.fStartOffsetX = -450f;
			break;
		}
		switch (iRow)
		{
		case 3:
			this.fStartOffsetY = 150f;
			break;
		case 4:
			this.fStartOffsetY = 225f;
			break;
		case 5:
			this.fStartOffsetY = 300f;
			break;
		case 6:
			this.fStartOffsetY = 375f;
			break;
		case 7:
			this.fStartOffsetY = 450f;
			break;
		}
		Vector3 localPosition;
		localPosition..ctor(0f, 0f, 0f);
		localPosition.x = this.fStartOffsetX + this.fPlusOffsetX * (float)StartX;
		localPosition.y = this.fStartOffsetY + this.fPlusOffsetY * (float)StartY;
		base.transform.localPosition = localPosition;
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		if (this.ObjTweenScale != null)
		{
			this.temp_from = this.ObjTweenScale.from;
			this.temp_to = this.ObjTweenScale.to;
			this.ObjTweenScale.ResetToBeginning();
			this.ObjTweenScale.from = new Vector3(0.5f, 0.5f, 0.5f);
			this.ObjTweenScale.to = Vector3.one;
			this.ObjTweenScale.PlayForward();
		}
		this.iNowX = StartX;
		this.iNowY = StartY;
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x0007B9DC File Offset: 0x00079BDC
	public void SetTileValue(int iVal)
	{
		int num = iVal + this.iPlus;
		base.gameObject.GetComponent<UISprite>().spriteName = "Tile-" + num.ToString("000");
		if (iVal == -2)
		{
			base.gameObject.GetComponent<UISprite>().spriteName = "Stop002";
		}
		if (iVal == 503)
		{
			GameObject gameObject = Object.Instantiate(this.spawnPoint) as GameObject;
			RenderQueueModifier renderQueueModifier = gameObject.gameObject.AddComponent<RenderQueueModifier>();
			renderQueueModifier.m_type = RenderQueueModifier.RenderType.BACK;
			renderQueueModifier.m_target = base.gameObject.GetComponent<UIWidget>();
			gameObject.transform.parent = base.transform.parent;
			gameObject.transform.localPosition = base.transform.localPosition;
			float num2 = (float)Screen.width / 640f;
			ParticleEmitter[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleEmitter>();
			foreach (ParticleEmitter particleEmitter in componentsInChildren)
			{
				particleEmitter.minEnergy *= num2;
				particleEmitter.maxEnergy *= num2;
				particleEmitter.minSize *= num2;
				particleEmitter.maxSize *= num2;
			}
		}
		if (iVal >= 101 && iVal <= 111)
		{
			if (this.sprPlus != null)
			{
				this.sprPlus.spriteName = "splash02";
			}
			base.gameObject.GetComponent<UISprite>().spriteName = "Tile-" + (iVal - 100).ToString("000");
		}
		else if (iVal >= 200 && iVal <= 211)
		{
			if (this.sprPlus != null)
			{
				this.sprPlus.spriteName = "Trap-02";
			}
			base.gameObject.GetComponent<UISprite>().spriteName = "Tile-" + (iVal - 200).ToString("000");
		}
		else if (iVal >= 400 && iVal <= 411)
		{
			if (this.sprPlus != null)
			{
				this.sprPlus.spriteName = "Trap-01";
			}
			base.gameObject.GetComponent<UISprite>().spriteName = "Tile-" + (iVal - 400).ToString("000");
		}
		else if (iVal >= 611 && iVal <= 697)
		{
			if (this.sprPlus != null && this.BombPlus != null)
			{
				this.sprPlus.spriteName = "Boon003";
				this.BombPlus.text = ((iVal - 600) / 10 * 10 - this.go.BombMoves).ToString();
			}
			base.gameObject.GetComponent<UISprite>().spriteName = "Tile-" + ((iVal - 600) % 10).ToString("000");
		}
		else if (this.sprPlus != null)
		{
			this.sprPlus.spriteName = "null";
		}
		this.iNewValue = iVal;
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x0007BD24 File Offset: 0x00079F24
	public void MoveTo(int x, int y)
	{
		if (this.wantToMargeTile != null)
		{
			this.wantToMargeTile.MoveTo(x, y);
		}
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x = this.fStartOffsetX + this.fPlusOffsetX * (float)x;
		localPosition.y = this.fStartOffsetY + this.fPlusOffsetY * (float)y;
		TweenPosition.Begin(base.gameObject, 0.075f, localPosition);
		this.iMoveToX = x;
		this.iMoveToY = y;
		this.bMoveing = true;
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x0007BDB0 File Offset: 0x00079FB0
	public void PlayEffectAfterMarge()
	{
		switch (this.iNewValue)
		{
		case 500:
			this.PlayEffect(this.psFeatherWhiteParticle, true);
			break;
		case 502:
			if (this.wantToMargeTile != null)
			{
				int num = this.wantToMargeTile.iNewValue;
				if (num == 1 || num == 2)
				{
					this.PlayEffect(this.psEggShellParticle, true);
				}
				else if (num >= 3 && num <= 6)
				{
					this.PlayEffect(this.psFeatherYellowParticle, true);
				}
				else
				{
					this.PlayEffect(this.psFeatherWhiteParticle, true);
				}
			}
			break;
		case 505:
			this.PlayEffect(this.psSinkholeParticle, true);
			break;
		}
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x0007BE80 File Offset: 0x0007A080
	public void TweenPositionFinish()
	{
		if (this.bBounceEffecting)
		{
			TweenPosition.Begin(base.gameObject, 0.075f, this.bouncePos);
			this.bBounceEffecting = false;
			return;
		}
		this.iNowX = this.iMoveToX;
		this.iNowY = this.iMoveToY;
		this.SetTileValue(this.iNewValue);
		this.bMoveing = false;
		if (this.wantToMargeTile != null)
		{
			this.PlayEffectAfterMarge();
			this.wantToMargeTile = null;
			if (this.ObjTweenColor != null && this.ObjTweenScale != null)
			{
				this.ObjTweenScale.from = this.temp_from;
				this.ObjTweenScale.to = this.temp_to;
				this.bScaleForward = true;
				this.ObjTweenScale.PlayForward();
				this.ObjTweenColor.PlayForward();
			}
		}
		if (this.bRemove && this.iTargetPos != 0)
		{
			this.FlyToTarget();
		}
		else if (this.bRemove && this.iTargetPos == 0)
		{
			Object.Destroy(base.gameObject);
		}
		if (this.bObstacle && this.iTargetPos != 0)
		{
			this.FlyToTarget();
		}
		else if (this.bObstacle && this.iTargetPos == 0)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x0007BFE8 File Offset: 0x0007A1E8
	private void FlyToTarget()
	{
		UISprite component = base.gameObject.GetComponent<UISprite>();
		if (component != null)
		{
			component.depth += 5;
		}
		GameObject gameObject = null;
		if (this.iTargetPos == 1)
		{
			gameObject = this.targetObj1;
		}
		if (this.iTargetPos == 2)
		{
			gameObject = this.targetObj2;
		}
		if (gameObject == null)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			Vector3 vector = gameObject.transform.localPosition + gameObject.transform.parent.localPosition + gameObject.transform.parent.parent.localPosition + gameObject.transform.parent.parent.parent.localPosition;
			this.PlayEffect(this.peMergeParticle1, false);
			this.PlayEffect(this.peMergeParticle2, false);
			this.bFlyToTarget = true;
			Vector2 vector2 = vector - base.gameObject.transform.localPosition;
			float num = Mathf.Sqrt(Mathf.Pow(vector2.x, 2f) + Mathf.Pow(vector2.y, 2f));
			float num2 = Mathf.Atan(vector2.y / vector2.x) * 57.295776f;
			if (num2 < 0f)
			{
				num2 *= -1f;
			}
			float num3;
			if (vector2.x < 0f)
			{
				num3 = (90f - num2) / 100f;
			}
			else
			{
				num3 = -((90f - num2) / 100f);
			}
			if (this.peFlyParticle != null)
			{
				float x = base.gameObject.transform.parent.localScale.x;
				ParticleEmitter particleEmitter = Object.Instantiate(this.peFlyParticle) as ParticleEmitter;
				RenderQueueModifier renderQueueModifier = particleEmitter.gameObject.AddComponent<RenderQueueModifier>();
				renderQueueModifier.m_type = RenderQueueModifier.RenderType.BACK;
				renderQueueModifier.m_target = base.gameObject.GetComponent<UIWidget>();
				renderQueueModifier.renderQueue = base.gameObject.GetComponent<UIWidget>().drawCall.renderQueue;
				particleEmitter.transform.parent = base.transform;
				particleEmitter.transform.localPosition = new Vector3(0f, 0f, 0f);
				particleEmitter.minEnergy *= x;
				particleEmitter.maxEnergy *= x;
				particleEmitter.minSize *= x;
				particleEmitter.maxSize *= x;
				particleEmitter.localVelocity = new Vector3(num3, 0f, 0f);
			}
			TweenPosition.Begin(base.gameObject, num / 900f, vector);
			this.iTargetPos = 0;
		}
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x00009D6D File Offset: 0x00007F6D
	public void ObstacleTile(int newValue)
	{
		this.bObstacle = true;
		this.iNewValue = newValue;
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x00009D7D File Offset: 0x00007F7D
	public void MergeTile(TileData beMargeTile, int newValue, int pos)
	{
		this.wantToMargeTile = beMargeTile;
		this.iNewValue = newValue;
		this.iTargetPos = pos;
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x00009D94 File Offset: 0x00007F94
	public void SwitchTile(TileData beSwitchTile, int newValue)
	{
		this.wantToSwitchTile = beSwitchTile;
		this.iNewValue = newValue;
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x0007C2C0 File Offset: 0x0007A4C0
	public void BounceTile(string direction)
	{
		if (this.bBounceEffecting)
		{
			return;
		}
		this.bouncePos = base.transform.localPosition;
		Vector3 localPosition = base.transform.localPosition;
		if (direction != null)
		{
			if (TileData.<>f__switch$map3 == null)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
				dictionary.Add("UP", 0);
				dictionary.Add("DOWN", 1);
				dictionary.Add("LEFT", 2);
				dictionary.Add("RIGHT", 3);
				TileData.<>f__switch$map3 = dictionary;
			}
			int num;
			if (TileData.<>f__switch$map3.TryGetValue(direction, ref num))
			{
				switch (num)
				{
				case 0:
					localPosition.y += (float)this.iBounceOffset;
					break;
				case 1:
					localPosition.y -= (float)this.iBounceOffset;
					break;
				case 2:
					localPosition.x -= (float)this.iBounceOffset;
					break;
				case 3:
					localPosition.x += (float)this.iBounceOffset;
					break;
				}
			}
		}
		TweenPosition.Begin(base.gameObject, 0.075f, localPosition);
		this.bMoveing = true;
		this.bBounceEffecting = true;
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x0007C3FC File Offset: 0x0007A5FC
	public void TweenScaleFinish()
	{
		if (this.bScaleForward)
		{
			this.ObjTweenScale.PlayReverse();
			this.ObjTweenColor.PlayReverse();
			this.bScaleForward = false;
		}
		else if (this.bRemove && !this.bFlyToTarget)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x00009DA4 File Offset: 0x00007FA4
	public void MergeAndRemoveTile(TileData beMargeTile, int newValue, int pos)
	{
		this.wantToMargeTile = beMargeTile;
		this.iNewValue = newValue;
		this.iTargetPos = pos;
		this.bRemove = true;
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x0007C458 File Offset: 0x0007A658
	public void PlayEffect(ParticleSystem ps, bool inFront)
	{
		if (ps == null)
		{
			return;
		}
		float x = base.gameObject.transform.parent.localScale.x;
		Debug.Log("Playing Effect PS");
		ParticleSystem particleSystem = Object.Instantiate(ps) as ParticleSystem;
		particleSystem.startSpeed *= x;
		particleSystem.startSize *= x;
		this.EffectRenderLayer(particleSystem.gameObject, inFront);
		particleSystem.Play();
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x0007C4D8 File Offset: 0x0007A6D8
	public void PlayEffect(ParticleEmitter pe, bool inFront)
	{
		if (pe == null)
		{
			return;
		}
		float x = base.gameObject.transform.parent.localScale.x;
		ParticleEmitter particleEmitter = Object.Instantiate(pe) as ParticleEmitter;
		particleEmitter.minEnergy *= x;
		particleEmitter.maxEnergy *= x;
		particleEmitter.minSize *= x;
		particleEmitter.maxSize *= x;
		this.EffectRenderLayer(particleEmitter.gameObject, inFront);
		particleEmitter.Emit();
	}

	// Token: 0x06000EBE RID: 3774 RVA: 0x0007C568 File Offset: 0x0007A768
	public void PlayEffectDan(ParticleEmitter pe, bool inFront, int x, int y)
	{
		if (pe == null)
		{
			return;
		}
		float x2 = base.gameObject.transform.parent.localScale.x;
		ParticleEmitter particleEmitter = Object.Instantiate(pe) as ParticleEmitter;
		particleEmitter.transform.parent = base.transform;
		particleEmitter.localVelocity = new Vector3((float)x, (float)y, 0f);
		particleEmitter.minEnergy *= x2;
		particleEmitter.maxEnergy *= x2;
		particleEmitter.minSize *= x2;
		particleEmitter.maxSize *= x2;
		this.EffectRenderLayer(particleEmitter.gameObject, inFront);
		particleEmitter.Emit();
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x0007C620 File Offset: 0x0007A820
	private void EffectRenderLayer(GameObject effectObj, bool inFront)
	{
		RenderQueueModifier renderQueueModifier = effectObj.AddComponent<RenderQueueModifier>();
		if (inFront)
		{
			renderQueueModifier.m_type = RenderQueueModifier.RenderType.FRONT;
		}
		else
		{
			renderQueueModifier.m_type = RenderQueueModifier.RenderType.BACK;
		}
		renderQueueModifier.m_target = base.gameObject.GetComponent<UIWidget>();
		renderQueueModifier.renderQueue = base.gameObject.GetComponent<UIWidget>().drawCall.renderQueue;
		effectObj.transform.parent = base.transform.parent;
		effectObj.transform.localPosition = base.transform.localPosition;
		effectObj.transform.localScale = new Vector3(base.gameObject.transform.parent.localScale.x * effectObj.transform.localScale.x, base.gameObject.transform.parent.localScale.y * effectObj.transform.localScale.y, 1f);
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x00009DC2 File Offset: 0x00007FC2
	public int GetTileValue()
	{
		return this.iNewValue;
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x0007C720 File Offset: 0x0007A920
	public void PromoteChicken()
	{
		int num = this.iNowX + this.iNowY * this.go.iCol;
		if (this.iNewValue >= 0 && this.iNewValue < 11)
		{
			this.go.DataArray[num] = this.go.DataArray[num] + 1;
			this.go.TileList[num].GetComponent<TileData>().SetTileValue(this.go.DataArray[num]);
		}
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x0007C7A0 File Offset: 0x0007A9A0
	public void RemoveWithClaw()
	{
		int num = this.iNowX + this.iNowY * this.go.iCol;
		if (this.iNewValue >= 101 && this.iNewValue <= 111)
		{
			this.go.DataArray[num] = this.go.DataArray[num] - 100;
			this.go.TileList[num].GetComponent<TileData>().SetTileValue(this.go.DataArray[num]);
			if (this.go.goalStatus.UpdateGoal(100) != 0)
			{
				this.go.UpdateObs();
			}
			if (this.sprPlus != null)
			{
				this.sprPlus.spriteName = "null";
			}
		}
		else if (this.iNewValue >= 201 && this.iNewValue <= 211)
		{
			this.go.DataArray[num] = this.go.DataArray[num] - 200;
			this.go.TileList[num].GetComponent<TileData>().SetTileValue(this.go.DataArray[num]);
			if (this.go.goalStatus.UpdateGoal(200) != 0)
			{
				this.go.UpdateObs();
			}
			if (this.sprPlus != null)
			{
				this.sprPlus.spriteName = "null";
			}
		}
		else if (this.iNewValue >= 301 && this.iNewValue <= 305)
		{
			if (this.go.DataArray[num] == 301)
			{
				this.bRemove = true;
				TweenScale.Begin(base.gameObject, 0.3f, Vector3.zero);
				if (this.go.goalStatus.UpdateGoal(300) != 0)
				{
					this.go.UpdateObs();
				}
				this.go.DataArray[num] = 0;
				this.go.TileList[num] = null;
			}
			else
			{
				this.go.DataArray[num] = this.go.DataArray[num] - 1;
				this.go.TileList[num].GetComponent<TileData>().SetTileValue(this.go.DataArray[num]);
			}
		}
		else if (this.iNewValue >= 401 && this.iNewValue <= 411)
		{
			this.go.DataArray[num] = this.go.DataArray[num] - 200;
			this.go.TileList[num].GetComponent<TileData>().SetTileValue(this.go.DataArray[num]);
			if (this.sprPlus != null)
			{
				this.sprPlus.spriteName = "Trap-02";
			}
		}
		else
		{
			this.bRemove = true;
			TweenScale.Begin(base.gameObject, 0.3f, Vector3.zero);
			this.go.DataArray[num] = 0;
			this.go.TileList[num] = null;
		}
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x0007CABC File Offset: 0x0007ACBC
	public void ClearHay()
	{
		if (this.iNowX + this.iNowY * this.go.iCol + 1 < this.go.iCol * this.go.iRow && (this.iNowX + this.iNowY * this.go.iCol + 1) % this.go.iCol != 0 && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + 1] >= 301 && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + 1] <= 305)
		{
			this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + 1] = this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + 1] - 1;
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + 1].GetComponent<TileData>().PlayEffect(this.psHayBaleParticle, true);
			if (this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + 1] == 300)
			{
				this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + 1] = 0;
				Object.DestroyImmediate(this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + 1].gameObject);
				this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + 1] = null;
				if (this.go.goalStatus.UpdateGoal(300) != 0)
				{
					this.go.UpdateObs();
				}
			}
			else
			{
				this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + 1].GetComponent<TileData>().SetTileValue(this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + 1]);
			}
		}
		if (this.iNowX + this.iNowY * this.go.iCol - 1 >= 0 && (this.iNowX + this.iNowY * this.go.iCol - 1) % this.go.iCol != this.go.iCol - 1 && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - 1] >= 301 && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - 1] <= 305)
		{
			this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - 1] = this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - 1] - 1;
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - 1].GetComponent<TileData>().PlayEffect(this.psHayBaleParticle, true);
			if (this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - 1] == 300)
			{
				this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - 1] = 0;
				Object.DestroyImmediate(this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - 1].gameObject);
				this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - 1] = null;
				if (this.go.goalStatus.UpdateGoal(300) != 0)
				{
					this.go.UpdateObs();
				}
			}
			else
			{
				this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - 1].GetComponent<TileData>().SetTileValue(this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - 1]);
			}
		}
		if (this.iNowX + this.iNowY * this.go.iCol + this.go.iCol < this.go.iCol * this.go.iRow && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol] >= 301 && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol] <= 305)
		{
			this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol] = this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol] - 1;
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol].GetComponent<TileData>().PlayEffect(this.psHayBaleParticle, true);
			if (this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol] == 300)
			{
				this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol] = 0;
				Object.DestroyImmediate(this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol].gameObject);
				this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol] = null;
				if (this.go.goalStatus.UpdateGoal(300) != 0)
				{
					this.go.UpdateObs();
				}
			}
			else
			{
				this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol].GetComponent<TileData>().SetTileValue(this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol]);
			}
		}
		if (this.iNowX + this.iNowY * this.go.iCol - this.go.iCol >= 0 && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol] >= 301 && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol] <= 305)
		{
			this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol] = this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol] - 1;
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol].GetComponent<TileData>().PlayEffect(this.psHayBaleParticle, true);
			if (this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol] == 300)
			{
				this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol] = 0;
				Object.DestroyImmediate(this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol].gameObject);
				this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol] = null;
				if (this.go.goalStatus.UpdateGoal(300) != 0)
				{
					this.go.UpdateObs();
				}
			}
			else
			{
				this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol].GetComponent<TileData>().SetTileValue(this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol]);
			}
		}
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x0007D548 File Offset: 0x0007B748
	public void ClearDan()
	{
		if (this.iNowX + this.iNowY * this.go.iCol + 1 < this.go.iCol * this.go.iRow && (this.iNowX + this.iNowY * this.go.iCol + 1) % this.go.iCol != 0 && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + 1] == 504)
		{
			this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + 1] = 0;
			this.go.CantDan.Add(this.iNowX + this.iNowY * this.go.iCol);
			this.go.CantDanCount++;
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + 1].GetComponent<TileData>().PlayEffect(this.psDanClearParticle, true);
			Object.DestroyImmediate(this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + 1].gameObject);
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + 1] = null;
		}
		if (this.iNowX + this.iNowY * this.go.iCol - 1 >= 0 && (this.iNowX + this.iNowY * this.go.iCol - 1) % this.go.iCol != this.go.iCol - 1 && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - 1] == 504)
		{
			this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - 1] = 0;
			this.go.CantDan.Add(this.iNowX + this.iNowY * this.go.iCol - 1);
			this.go.CantDanCount++;
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - 1].GetComponent<TileData>().PlayEffect(this.psDanClearParticle, true);
			Object.DestroyImmediate(this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - 1].gameObject);
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - 1] = null;
		}
		if (this.iNowX + this.iNowY * this.go.iCol + this.go.iCol < this.go.iCol * this.go.iRow && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol] == 504)
		{
			this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol] = 0;
			this.go.CantDan.Add(this.iNowX + this.iNowY * this.go.iCol + this.go.iCol);
			this.go.CantDanCount++;
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol].GetComponent<TileData>().PlayEffect(this.psDanClearParticle, true);
			Object.DestroyImmediate(this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol].gameObject);
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol + this.go.iCol] = null;
		}
		if (this.iNowX + this.iNowY * this.go.iCol - this.go.iCol >= 0 && this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol] == 504)
		{
			this.go.DataArray[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol] = 0;
			this.go.CantDan.Add(this.iNowX + this.iNowY * this.go.iCol - this.go.iCol);
			this.go.CantDanCount++;
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol].GetComponent<TileData>().PlayEffect(this.psDanClearParticle, true);
			Object.DestroyImmediate(this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol].gameObject);
			this.go.TileList[this.iNowX + this.iNowY * this.go.iCol - this.go.iCol] = null;
		}
	}

	// Token: 0x0400118D RID: 4493
	private float fStartOffsetX;

	// Token: 0x0400118E RID: 4494
	private float fStartOffsetY;

	// Token: 0x0400118F RID: 4495
	private float fPlusOffsetX = 150f;

	// Token: 0x04001190 RID: 4496
	private float fPlusOffsetY = -150f;

	// Token: 0x04001191 RID: 4497
	public GameObject targetObj1;

	// Token: 0x04001192 RID: 4498
	public GameObject targetObj2;

	// Token: 0x04001193 RID: 4499
	private int iTargetPos;

	// Token: 0x04001194 RID: 4500
	public TweenPosition ObjTweenPosition;

	// Token: 0x04001195 RID: 4501
	public TweenScale ObjTweenScale;

	// Token: 0x04001196 RID: 4502
	public TweenColor ObjTweenColor;

	// Token: 0x04001197 RID: 4503
	public bool bRemove;

	// Token: 0x04001198 RID: 4504
	public bool bMoveing;

	// Token: 0x04001199 RID: 4505
	public bool bFlyToTarget;

	// Token: 0x0400119A RID: 4506
	public bool bObstacle;

	// Token: 0x0400119B RID: 4507
	public UISprite sprPlus;

	// Token: 0x0400119C RID: 4508
	public UILabel BombPlus;

	// Token: 0x0400119D RID: 4509
	public GameObject spawnPoint;

	// Token: 0x0400119E RID: 4510
	public ParticleEmitter peMergeParticle1;

	// Token: 0x0400119F RID: 4511
	public ParticleEmitter peMergeParticle2;

	// Token: 0x040011A0 RID: 4512
	public ParticleEmitter peFlyParticle;

	// Token: 0x040011A1 RID: 4513
	public ParticleEmitter peDanSpreadParticle;

	// Token: 0x040011A2 RID: 4514
	public ParticleSystem psDanClearParticle;

	// Token: 0x040011A3 RID: 4515
	public ParticleSystem psEggShellParticle;

	// Token: 0x040011A4 RID: 4516
	public ParticleSystem psFeatherWhiteParticle;

	// Token: 0x040011A5 RID: 4517
	public ParticleSystem psFeatherYellowParticle;

	// Token: 0x040011A6 RID: 4518
	public ParticleSystem psSinkholeParticle;

	// Token: 0x040011A7 RID: 4519
	public ParticleSystem psHayBaleParticle;

	// Token: 0x040011A8 RID: 4520
	public int iNowX;

	// Token: 0x040011A9 RID: 4521
	public int iNowY;

	// Token: 0x040011AA RID: 4522
	private int iMoveToX;

	// Token: 0x040011AB RID: 4523
	private int iMoveToY;

	// Token: 0x040011AC RID: 4524
	private TileData wantToMargeTile;

	// Token: 0x040011AD RID: 4525
	private TileData wantToSwitchTile;

	// Token: 0x040011AE RID: 4526
	private int iNewValue;

	// Token: 0x040011AF RID: 4527
	private bool bScaleForward;

	// Token: 0x040011B0 RID: 4528
	private bool bBounceEffecting;

	// Token: 0x040011B1 RID: 4529
	private int iBounceOffset = 15;

	// Token: 0x040011B2 RID: 4530
	private Vector3 bouncePos = Vector3.zero;

	// Token: 0x040011B3 RID: 4531
	public CtrlAbility go;

	// Token: 0x040011B4 RID: 4532
	private Vector3 temp_from;

	// Token: 0x040011B5 RID: 4533
	private Vector3 temp_to;

	// Token: 0x040011B6 RID: 4534
	public int iPlus;
}
