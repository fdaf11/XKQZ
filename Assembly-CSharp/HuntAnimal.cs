using System;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x02000384 RID: 900
public class HuntAnimal : MonoBehaviour
{
	// Token: 0x060014F9 RID: 5369 RVA: 0x0000D624 File Offset: 0x0000B824
	private void Start()
	{
		this.m_fDelayedTime = Time.time;
		this.m_iTouchAmount = 0;
		this.m_iHitAddSpeed = 100;
		this.m_bStart = true;
		this.m_bLeave = false;
		this.m_bAttack = false;
		this.m_iItemID = 0;
		this.SetRewardItem();
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x000B43E0 File Offset: 0x000B25E0
	private void SetStartPoint()
	{
		this.m_iScopeMaxX = (int)this.m_goActivitiesScope.transform.localPosition.x + this.m_goActivitiesScope.GetComponent<UITexture>().width / 2;
		this.m_iScopeMinX = (int)this.m_goActivitiesScope.transform.localPosition.x - this.m_goActivitiesScope.GetComponent<UITexture>().width / 2;
		this.m_iScopeMaxY = (int)this.m_goActivitiesScope.transform.localPosition.y + this.m_goActivitiesScope.GetComponent<UITexture>().height / 2;
		this.m_iScopeMinY = (int)this.m_goActivitiesScope.transform.localPosition.y - this.m_goActivitiesScope.GetComponent<UITexture>().height / 2;
		int num = 0;
		int num2 = 0;
		if (this.m_AnimalDataNode.m_iAnimalType != 2)
		{
			if (Random.Range(0, 2) == 0)
			{
				if (this.m_iPoint == 0 || this.m_iPoint == 1)
				{
					num = (int)base.transform.localPosition.y - this.m_goActivitiesScope.GetComponent<UITexture>().height - this.m_iHeight / 2;
				}
				else if (this.m_iPoint == 2 || this.m_iPoint == 3)
				{
					num = (int)base.transform.localPosition.y + this.m_goActivitiesScope.GetComponent<UITexture>().height + this.m_iHeight / 2;
				}
				num2 = Random.Range(0, this.m_goActivitiesScope.GetComponent<UITexture>().width + 1);
				if (this.m_iPoint == 0 || this.m_iPoint == 2)
				{
					num2 = (int)base.transform.localPosition.x + num2 + this.m_iWidth;
				}
				else if (this.m_iPoint == 1 || this.m_iPoint == 3)
				{
					num2 = (int)base.transform.localPosition.x - num2 - this.m_iWidth;
				}
			}
			else
			{
				if (this.m_iPoint == 0 || this.m_iPoint == 2)
				{
					num2 = (int)base.transform.localPosition.x + this.m_goActivitiesScope.GetComponent<UITexture>().width + this.m_iWidth / 2;
				}
				else if (this.m_iPoint == 1 || this.m_iPoint == 3)
				{
					num2 = (int)base.transform.localPosition.x - this.m_goActivitiesScope.GetComponent<UITexture>().width - this.m_iWidth / 2;
				}
				num = Random.Range(0, this.m_goActivitiesScope.GetComponent<UITexture>().height + 1);
				if (this.m_iPoint == 0 || this.m_iPoint == 1)
				{
					num = (int)base.transform.localPosition.y - num - this.m_iHeight;
				}
				else if (this.m_iPoint == 2 || this.m_iPoint == 3)
				{
					num = (int)base.transform.localPosition.y + num + this.m_iHeight;
				}
			}
		}
		else
		{
			num = (int)base.transform.localPosition.y;
			if (this.m_iPoint == 5)
			{
				num2 = (int)base.transform.localPosition.x + this.m_goActivitiesScope.GetComponent<UITexture>().width + this.m_iWidth / 2;
			}
			else if (this.m_iPoint == 6)
			{
				num2 = (int)base.transform.localPosition.x - this.m_goActivitiesScope.GetComponent<UITexture>().width - this.m_iWidth / 2;
			}
		}
		this.m_MoveToward = new Vector3((float)num2, (float)num, 0f);
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x000B47CC File Offset: 0x000B29CC
	private void SetRewardItem()
	{
		int num = Random.Range(0, 101);
		if (num <= this.m_AnimalDataNode.m_iProbability1)
		{
			this.m_iItemID = this.m_AnimalDataNode.m_iGift1Item;
		}
		else if (num <= this.m_AnimalDataNode.m_iProbability2)
		{
			this.m_iItemID = this.m_AnimalDataNode.m_iGift2Item;
		}
		else if (num <= this.m_AnimalDataNode.m_iProbability3)
		{
			this.m_iItemID = this.m_AnimalDataNode.m_iGift3Item;
		}
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x000B4854 File Offset: 0x000B2A54
	private void EnforceBounds()
	{
		Vector3 localPosition = base.transform.localPosition;
		if (localPosition.x < (float)this.m_iScopeMinX || localPosition.x > (float)this.m_iScopeMaxX)
		{
			if (this.m_iTouchAmount == this.m_AnimalDataNode.m_iRound)
			{
				if (this.m_AnimalDataNode.m_iAnimalType == 0)
				{
					base.gameObject.GetComponent<TweenAlpha>().ResetToBeginning();
					base.gameObject.GetComponent<TweenAlpha>().Play();
					this.m_bLeave = true;
				}
				else
				{
					if (this.m_go_HurtEffect.activeSelf)
					{
						this.m_go_HurtEffect.SetActive(false);
					}
					this.m_go_AtkEffect.SetActive(true);
					this.m_bAttack = true;
				}
			}
			else
			{
				localPosition.x = Mathf.Clamp(localPosition.x, (float)this.m_iScopeMinX, (float)this.m_iScopeMaxX);
				this.m_MoveDirection.x = -this.m_MoveDirection.x;
				this.m_iTouchAmount++;
			}
		}
		if (this.m_AnimalDataNode.m_iAnimalType != 2 && (localPosition.y < (float)this.m_iScopeMinY || localPosition.y > (float)this.m_iScopeMaxY))
		{
			if (this.m_iTouchAmount == this.m_AnimalDataNode.m_iRound)
			{
				if (this.m_AnimalDataNode.m_iAnimalType == 0)
				{
					base.gameObject.GetComponent<TweenAlpha>().ResetToBeginning();
					base.gameObject.GetComponent<TweenAlpha>().Play();
					this.m_bLeave = true;
				}
				else
				{
					if (this.m_go_HurtEffect.activeSelf)
					{
						this.m_go_HurtEffect.SetActive(false);
					}
					this.m_go_AtkEffect.SetActive(true);
					this.m_bAttack = true;
				}
			}
			else
			{
				localPosition.y = Mathf.Clamp(localPosition.y, (float)this.m_iScopeMinY, (float)this.m_iScopeMaxY);
				this.m_MoveDirection.y = -this.m_MoveDirection.y;
				this.m_iTouchAmount++;
			}
		}
		base.transform.localPosition = localPosition;
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x000B4A70 File Offset: 0x000B2C70
	public void AnimalLeave()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Equals("cFormHunt"))
			{
				array[i].GetComponent<UIHunt>().AnimalDisappear();
				break;
			}
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x000B4AD0 File Offset: 0x000B2CD0
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Arrow")
		{
			if (this.m_bDead)
			{
				return;
			}
			if (other.gameObject.GetComponent<HuntArrow>().m_bFly && Game.g_AudioBundle.Contains("audio/UI/" + this.m_AnimalDataNode.m_strSoundID))
			{
				AudioClip clip = Game.g_AudioBundle.Load("audio/UI/" + this.m_AnimalDataNode.m_strSoundID) as AudioClip;
				AudioSource component = base.gameObject.GetComponent<AudioSource>();
				component.pitch = 1f;
				component.clip = clip;
				component.loop = false;
				component.volume = GameGlobal.m_fSoundValue;
				component.Play();
				this.m_iHp -= other.gameObject.GetComponent<HuntArrow>().m_iAtk;
				if (this.m_iHp <= 0)
				{
					this.m_bDead = true;
					if (this.m_go_HurtEffect.activeSelf)
					{
						this.m_go_HurtEffect.SetActive(false);
					}
					if (this.m_go_AtkEffect.activeSelf)
					{
						this.m_go_AtkEffect.SetActive(false);
					}
					if (this.m_iItemID != 0)
					{
						this.m_go_AnimalItem.SetActive(true);
						this.m_go_AnimalItem.GetComponent<Animation>().Play("HuntAnimalItem");
					}
				}
				else if (!this.m_go_HurtEffect.activeSelf)
				{
					this.m_go_HurtEffect.SetActive(true);
				}
				Object.Destroy(other.gameObject);
				this.playHit();
			}
		}
		else if (other.tag != "Animal" && this.m_bAttack)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name.Equals("cFormHunt"))
				{
					array[i].GetComponent<UIHunt>().PlayerHit(this.m_AnimalDataNode.m_fAtkDedSec);
					break;
				}
			}
			if (this.m_go_AtkEffect.activeSelf)
			{
				this.m_go_AtkEffect.SetActive(false);
			}
			this.m_BarTimeBase.GetComponent<AudioSource>().volume = GameGlobal.m_fSoundValue;
			this.m_BarTimeBase.GetComponent<AudioSource>().Play();
			base.gameObject.GetComponent<TweenAlpha>().ResetToBeginning();
			base.gameObject.GetComponent<TweenAlpha>().Play();
			this.AnimalLeave();
		}
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x000B4D38 File Offset: 0x000B2F38
	private void playHit()
	{
		this.m_go_HitEffect.SetActive(true);
		this.m_go_HitImage.SetActive(true);
		this.m_go_HitLight.SetActive(true);
		this.m_HitEffect_TweenAlpha.ResetToBeginning();
		this.m_HitEffect_TweenScale.ResetToBeginning();
		this.m_HitEffect_TweenScale.Play();
		this.m_HitLight_TweenScale.ResetToBeginning();
		this.m_HitLight_TweenAlpha.ResetToBeginning();
		this.m_HitLight_TweenScale.Play();
		this.m_HitImage_TweenAlpha.ResetToBeginning();
		this.m_HitImage_TweenAlpha.Play();
	}

	// Token: 0x06001500 RID: 5376 RVA: 0x0000D662 File Offset: 0x0000B862
	public void PlayHitEffectEnd()
	{
		this.m_HitEffect_TweenAlpha.Play();
		this.m_HitLight_TweenAlpha.Play();
	}

	// Token: 0x06001501 RID: 5377 RVA: 0x000B4DC4 File Offset: 0x000B2FC4
	public void PlayHitEnd()
	{
		this.m_go_HitImage.SetActive(false);
		this.m_go_HitEffect.SetActive(false);
		this.m_go_HitLight.SetActive(false);
		if (this.m_iHp <= 0)
		{
			base.gameObject.GetComponent<TweenAlpha>().ResetToBeginning();
			base.gameObject.GetComponent<TweenAlpha>().Play();
			GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name.Equals("cFormHunt"))
				{
					array[i].GetComponent<UIHunt>().SetReward(this.m_iItemID, this.m_AnimalDataNode.m_iAnimalID, this.m_AnimalDataNode.m_ipoint);
					break;
				}
			}
		}
	}

	// Token: 0x06001502 RID: 5378 RVA: 0x000B4E88 File Offset: 0x000B3088
	private void Update()
	{
		if (this.m_bDead || this.m_bEnd)
		{
			return;
		}
		Vector3 localPosition = base.transform.localPosition;
		if (this.m_bStart)
		{
			this.m_bStart = false;
			this.SetStartPoint();
			this.m_MoveDirection = this.m_MoveToward - localPosition;
			this.m_MoveDirection.z = 0f;
			this.m_MoveDirection.Normalize();
		}
		if (this.m_bAttack)
		{
			this.m_MoveDirection = this.m_BarTimeBase.transform.localPosition - localPosition;
			this.m_MoveDirection.z = 0f;
			this.m_MoveDirection.Normalize();
		}
		Vector3 vector = this.m_MoveDirection * (500f * this.m_AnimalDataNode.m_fSpeed) + localPosition;
		base.transform.localPosition = Vector3.Lerp(localPosition, vector, Time.deltaTime);
		if (!this.m_bAttack && !this.m_bLeave && Time.time - this.m_fDelayedTime > 1.3f)
		{
			this.EnforceBounds();
		}
	}

	// Token: 0x04001977 RID: 6519
	public GameObject m_goActivitiesScope;

	// Token: 0x04001978 RID: 6520
	private Vector3 m_MoveDirection;

	// Token: 0x04001979 RID: 6521
	private Vector3 m_MoveToward;

	// Token: 0x0400197A RID: 6522
	public GameObject m_BarTimeBase;

	// Token: 0x0400197B RID: 6523
	public int m_iPoint;

	// Token: 0x0400197C RID: 6524
	public AnimalDataNode m_AnimalDataNode;

	// Token: 0x0400197D RID: 6525
	private int m_iScopeMaxX;

	// Token: 0x0400197E RID: 6526
	private int m_iScopeMinX;

	// Token: 0x0400197F RID: 6527
	private int m_iScopeMaxY;

	// Token: 0x04001980 RID: 6528
	private int m_iScopeMinY;

	// Token: 0x04001981 RID: 6529
	public int m_iWidth;

	// Token: 0x04001982 RID: 6530
	public int m_iHeight;

	// Token: 0x04001983 RID: 6531
	private bool m_bStart;

	// Token: 0x04001984 RID: 6532
	private float m_fDelayedTime;

	// Token: 0x04001985 RID: 6533
	private int m_iHitAddSpeed;

	// Token: 0x04001986 RID: 6534
	private int m_iTouchAmount;

	// Token: 0x04001987 RID: 6535
	private bool m_bAttack;

	// Token: 0x04001988 RID: 6536
	private bool m_bLeave;

	// Token: 0x04001989 RID: 6537
	private bool m_bDead;

	// Token: 0x0400198A RID: 6538
	public GameObject m_go_HitEffect;

	// Token: 0x0400198B RID: 6539
	public GameObject m_go_HitImage;

	// Token: 0x0400198C RID: 6540
	public GameObject m_go_HitLight;

	// Token: 0x0400198D RID: 6541
	public GameObject m_go_AnimalItem;

	// Token: 0x0400198E RID: 6542
	public GameObject m_go_AtkEffect;

	// Token: 0x0400198F RID: 6543
	public GameObject m_go_HurtEffect;

	// Token: 0x04001990 RID: 6544
	public TweenScale m_HitEffect_TweenScale;

	// Token: 0x04001991 RID: 6545
	public TweenAlpha m_HitImage_TweenAlpha;

	// Token: 0x04001992 RID: 6546
	public TweenScale m_HitLight_TweenScale;

	// Token: 0x04001993 RID: 6547
	public TweenAlpha m_HitLight_TweenAlpha;

	// Token: 0x04001994 RID: 6548
	public TweenAlpha m_HitEffect_TweenAlpha;

	// Token: 0x04001995 RID: 6549
	public int m_iHp;

	// Token: 0x04001996 RID: 6550
	private int m_iItemID;

	// Token: 0x04001997 RID: 6551
	public bool m_bEnd;
}
