using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020007AC RID: 1964
public class UnitTBAnimation : MonoBehaviour
{
	// Token: 0x06002FF3 RID: 12275 RVA: 0x0001E52C File Offset: 0x0001C72C
	public void Start()
	{
		this.InitAnimation();
	}

	// Token: 0x06002FF4 RID: 12276 RVA: 0x001733F8 File Offset: 0x001715F8
	public void ClearModel()
	{
		if (this.meleeAttackAniBody != null)
		{
			this.meleeAttackAniBody.transform.parent = null;
			Object.Destroy(this.meleeAttackAniBody);
			this.meleeAttackAniBody = null;
		}
		this.idleAniBody = null;
		this.moveAniBody = null;
		this.hitAniBody = null;
		this.criticalHitAniBody = null;
		this.dodgeAniBody = null;
		this.destroyAniBody = null;
		this.standAniBody = null;
		this.destroyAniClip = null;
		this.dodgeAniClip = null;
		this.idleAniClip = null;
		this.moveAniClip = null;
		this.hitAniClip = null;
		this.criticalHitAniClip = null;
		this.meleeAttackAniClip = null;
		this.meleeAttackAniDelay = null;
		this.standAniClip = null;
		this.standAniDelay = null;
	}

	// Token: 0x06002FF5 RID: 12277 RVA: 0x001734B0 File Offset: 0x001716B0
	private AnimationClip[] AddAnimationClip(AnimationClip[] list, AnimationClip clip)
	{
		int num = 0;
		if (list != null)
		{
			num = list.Length;
		}
		AnimationClip[] array = new AnimationClip[num + 1];
		int i;
		for (i = 0; i < num; i++)
		{
			if (i < list.Length)
			{
				array[i] = list[i];
			}
		}
		array[i] = clip;
		return array;
	}

	// Token: 0x06002FF6 RID: 12278 RVA: 0x001734FC File Offset: 0x001716FC
	public AnimationState GetAnimationState(string str)
	{
		foreach (object obj in this.standAniBody.animation)
		{
			AnimationState animationState = (AnimationState)obj;
			if (animationState.name == str)
			{
				return animationState;
			}
		}
		return null;
	}

	// Token: 0x06002FF7 RID: 12279 RVA: 0x00173578 File Offset: 0x00171778
	public void SetMovieAnimationSkill(MovieEventNode men)
	{
		GameObject goActor = men.goActor;
		this.standAniBody = goActor;
		this.meleeAttackAniBody = goActor;
		this.idleAniBody = goActor;
		this.moveAniBody = goActor;
		this.hitAniBody = goActor;
		this.criticalHitAniBody = goActor;
		this.dodgeAniBody = goActor;
		this.destroyAniBody = goActor;
		if (goActor.animation == null)
		{
			Debug.LogError("Error SetAnimation " + goActor.name);
			return;
		}
		if (men.mEventnType == _MovieEventNodeType.PlaySkill)
		{
			Game.g_ModelBundle.LoadAnimationGroup(goActor, "idle");
			Game.g_ModelBundle.LoadAnimationGroup(goActor, "skill");
		}
		else if (men.mEventnType == _MovieEventNodeType.PlayHurt)
		{
			Game.g_ModelBundle.LoadAnimationGroup(goActor, "idle");
			Game.g_ModelBundle.LoadAnimationGroup(goActor, "hurt01");
			if (men.bCrossFade)
			{
				Game.g_ModelBundle.LoadAnimationGroup(goActor, "dodge");
			}
			if (men.bLookAtMoving)
			{
				Game.g_ModelBundle.LoadAnimationGroup(goActor, "hurt02");
			}
			if (men.iTextOrder != 0)
			{
				Game.g_ModelBundle.LoadAnimationGroup(goActor, "die");
			}
		}
		this.destroyAniClip = null;
		this.destroyAniDelay = null;
		this.dodgeAniClip = null;
		this.idleAniClip = null;
		this.moveAniClip = null;
		this.hitAniClip = null;
		this.criticalHitAniClip = null;
		this.meleeAttackAniClip = null;
		this.meleeAttackAniDelay = null;
		this.standAniClip = null;
		this.standAniDelay = null;
		foreach (object obj in goActor.animation)
		{
			AnimationState animationState = (AnimationState)obj;
			if (animationState.name.IndexOf("die") >= 0)
			{
				this.destroyAniClip = this.AddAnimationClip(this.destroyAniClip, animationState.clip);
				this.destroyAniDelay = new float[this.destroyAniClip.Length];
				for (int i = 0; i < this.destroyAniClip.Length; i++)
				{
					this.destroyAniDelay[i] = this.destroyAniClip[i].length;
				}
			}
			if (animationState.name.IndexOf("dodge") >= 0)
			{
				this.dodgeAniClip = this.AddAnimationClip(this.dodgeAniClip, animationState.clip);
			}
			if (animationState.name.IndexOf("idle") >= 0)
			{
				this.idleAniClip = this.AddAnimationClip(this.idleAniClip, animationState.clip);
			}
			if (animationState.name.IndexOf("run") >= 0)
			{
				this.moveAniClip = this.AddAnimationClip(this.moveAniClip, animationState.clip);
			}
			if (animationState.name.IndexOf("hurt01") >= 0)
			{
				this.hitAniClip = this.AddAnimationClip(this.hitAniClip, animationState.clip);
			}
			if (animationState.name.IndexOf("hurt02") >= 0)
			{
				this.criticalHitAniClip = this.AddAnimationClip(this.criticalHitAniClip, animationState.clip);
			}
			if (animationState.name.IndexOf("skill") >= 0)
			{
				this.meleeAttackAniClip = this.AddAnimationClip(this.meleeAttackAniClip, animationState.clip);
				this.meleeAttackAniDelay = new float[this.meleeAttackAniClip.Length];
				for (int j = 0; j < this.meleeAttackAniClip.Length; j++)
				{
					this.meleeAttackAniDelay[j] = this.meleeAttackAniClip[j].length;
				}
			}
			if (animationState.name.IndexOf("stand") >= 0)
			{
				this.standAniClip = this.AddAnimationClip(this.standAniClip, animationState.clip);
				this.standAniDelay = new float[this.standAniClip.Length];
				for (int k = 0; k < this.standAniClip.Length; k++)
				{
					this.standAniDelay[k] = this.standAniClip[k].length;
				}
			}
		}
		if (this.meleeAttackAniBody != null)
		{
			this.meleeAttackAnimation = this.meleeAttackAniBody.GetComponent<Animation>();
			if (this.meleeAttackAnimation == null)
			{
				this.meleeAttackAnimation = this.meleeAttackAniBody.AddComponent<Animation>();
			}
			if (this.meleeAttackAniClip != null && this.meleeAttackAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip in this.meleeAttackAniClip)
				{
					if (animationClip != null)
					{
						this.meleeAttackAnimation.AddClip(animationClip, animationClip.name);
						this.meleeAttackAnimation.animation[animationClip.name].layer = 1;
						this.meleeAttackAnimation.animation[animationClip.name].wrapMode = 1;
						this.meleeAttackAnimation.animation[animationClip.name].speed = 1f;
					}
				}
				if (this.meleeAttackAniDelay.Length != this.meleeAttackAniClip.Length)
				{
					float[] array2 = new float[this.meleeAttackAniClip.Length];
					for (int m = 0; m < this.meleeAttackAniClip.Length; m++)
					{
						if (m < this.meleeAttackAniDelay.Length)
						{
							array2[m] = this.meleeAttackAniDelay[m];
						}
						else
						{
							array2[m] = this.meleeAttackAniClip[m].length;
						}
					}
					this.meleeAttackAniDelay = array2;
				}
			}
		}
		if (this.standAniBody != null)
		{
			this.standAnimation = this.standAniBody.GetComponent<Animation>();
			if (this.standAnimation == null)
			{
				this.standAnimation = this.standAniBody.AddComponent<Animation>();
			}
			if (this.standAniClip != null && this.standAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip2 in this.standAniClip)
				{
					if (animationClip2 != null)
					{
						this.standAnimation.AddClip(animationClip2, animationClip2.name);
						this.standAnimation.animation[animationClip2.name].layer = 1;
						this.standAnimation.animation[animationClip2.name].wrapMode = 1;
						this.standAnimation.animation[animationClip2.name].speed = 1f;
					}
				}
				if (this.standAniDelay.Length != this.standAniClip.Length)
				{
					float[] array4 = new float[this.standAniClip.Length];
					for (int num = 0; num < this.standAniClip.Length; num++)
					{
						if (num < this.standAniDelay.Length)
						{
							array4[num] = this.standAniDelay[num];
						}
						else
						{
							array4[num] = this.standAniClip[num].length;
						}
					}
					this.standAniDelay = array4;
				}
			}
		}
		if (this.idleAniBody != null)
		{
			this.idleAnimation = this.idleAniBody.GetComponent<Animation>();
			if (this.idleAnimation == null)
			{
				this.idleAnimation = this.idleAniBody.AddComponent<Animation>();
			}
			if (this.idleAniClip != null && this.idleAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip3 in this.idleAniClip)
				{
					if (animationClip3 != null)
					{
						this.idleAnimation.AddClip(animationClip3, animationClip3.name);
						this.idleAnimation.animation[animationClip3.name].layer = 0;
						this.idleAnimation.animation[animationClip3.name].wrapMode = 2;
						this.idleAnimation.animation[animationClip3.name].speed = 1f;
					}
				}
			}
		}
		if (this.moveAniBody != null)
		{
			this.moveAnimation = this.moveAniBody.GetComponent<Animation>();
			if (this.moveAnimation == null)
			{
				this.moveAnimation = this.moveAniBody.AddComponent<Animation>();
			}
			if (this.moveAniClip != null && this.moveAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip4 in this.moveAniClip)
				{
					if (animationClip4 != null)
					{
						this.moveAnimation.AddClip(animationClip4, animationClip4.name);
						this.moveAnimation.animation[animationClip4.name].layer = 1;
						this.moveAnimation.animation[animationClip4.name].wrapMode = 2;
						this.moveAnimation.animation[animationClip4.name].speed = 1f;
					}
				}
			}
		}
		if (this.hitAniBody != null)
		{
			this.hitAnimation = this.hitAniBody.GetComponent<Animation>();
			if (this.hitAnimation == null)
			{
				this.hitAnimation = this.hitAniBody.AddComponent<Animation>();
			}
			if (this.hitAniClip != null && this.hitAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip5 in this.hitAniClip)
				{
					if (animationClip5 != null)
					{
						this.hitAnimation.AddClip(animationClip5, animationClip5.name);
						this.hitAnimation.animation[animationClip5.name].layer = 1;
						this.hitAnimation.animation[animationClip5.name].wrapMode = 1;
						this.hitAnimation.animation[animationClip5.name].speed = 1f;
					}
				}
			}
		}
		if (this.criticalHitAniBody != null)
		{
			this.criticalHitAnimation = this.criticalHitAniBody.GetComponent<Animation>();
			if (this.criticalHitAnimation == null)
			{
				this.criticalHitAnimation = this.criticalHitAniBody.AddComponent<Animation>();
			}
			if (this.criticalHitAniClip != null && this.criticalHitAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip6 in this.criticalHitAniClip)
				{
					if (animationClip6 != null)
					{
						this.criticalHitAnimation.AddClip(animationClip6, animationClip6.name);
						this.criticalHitAnimation.animation[animationClip6.name].layer = 1;
						this.criticalHitAnimation.animation[animationClip6.name].wrapMode = 1;
						this.criticalHitAnimation.animation[animationClip6.name].speed = 1f;
					}
				}
			}
		}
		if (this.dodgeAniBody != null)
		{
			this.dodgeAnimation = this.dodgeAniBody.GetComponent<Animation>();
			if (this.dodgeAnimation == null)
			{
				this.dodgeAnimation = this.dodgeAniBody.AddComponent<Animation>();
			}
			if (this.dodgeAniClip != null && this.dodgeAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip7 in this.dodgeAniClip)
				{
					if (animationClip7 != null)
					{
						this.dodgeAnimation.AddClip(animationClip7, animationClip7.name);
						this.dodgeAnimation.animation[animationClip7.name].layer = 1;
						this.dodgeAnimation.animation[animationClip7.name].wrapMode = 1;
						this.dodgeAnimation.animation[animationClip7.name].speed = 1f;
					}
				}
			}
		}
		this.dodgeIndex = 0;
		if (this.destroyAniBody != null)
		{
			this.destroyAnimation = this.destroyAniBody.GetComponent<Animation>();
			if (this.destroyAnimation == null)
			{
				this.destroyAnimation = this.destroyAniBody.AddComponent<Animation>();
			}
			if (this.destroyAniClip != null && this.destroyAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip8 in this.destroyAniClip)
				{
					if (animationClip8 != null)
					{
						this.destroyAnimation.AddClip(animationClip8, animationClip8.name);
						this.destroyAnimation.animation[animationClip8.name].layer = 1;
						this.destroyAnimation.animation[animationClip8.name].wrapMode = 1;
						this.destroyAnimation.animation[animationClip8.name].speed = 1f;
					}
				}
				if (this.destroyAniDelay.Length != this.destroyAniClip.Length)
				{
					float[] array11 = new float[this.destroyAniClip.Length];
					for (int num8 = 0; num8 < this.destroyAniClip.Length; num8++)
					{
						if (num8 < this.destroyAniDelay.Length)
						{
							array11[num8] = this.destroyAniDelay[num8];
						}
						else
						{
							array11[num8] = this.destroyAniClip[num8].length;
						}
					}
					this.destroyAniDelay = array11;
				}
			}
		}
		if (this.idleAnimation != null)
		{
			this.idleAnimation.Stop();
		}
		this.PlayIdle();
	}

	// Token: 0x06002FF8 RID: 12280 RVA: 0x00174330 File Offset: 0x00172530
	public void SetAnimation(GameObject goModel)
	{
		this.standAniBody = goModel;
		this.meleeAttackAniBody = goModel;
		this.idleAniBody = goModel;
		this.moveAniBody = goModel;
		this.hitAniBody = goModel;
		this.criticalHitAniBody = goModel;
		this.dodgeAniBody = goModel;
		this.destroyAniBody = goModel;
		if (goModel.animation == null)
		{
			Debug.LogError("Error SetAnimation " + goModel.name);
			return;
		}
		Game.g_ModelBundle.LoadAllBattleAction(goModel);
		foreach (object obj in goModel.animation)
		{
			AnimationState animationState = (AnimationState)obj;
			if (animationState.name.IndexOf("die") >= 0)
			{
				this.destroyAniClip = this.AddAnimationClip(this.destroyAniClip, animationState.clip);
				this.destroyAniDelay = new float[this.destroyAniClip.Length];
				for (int i = 0; i < this.destroyAniClip.Length; i++)
				{
					this.destroyAniDelay[i] = this.destroyAniClip[i].length;
				}
			}
			if (animationState.name.IndexOf("dodge") >= 0)
			{
				this.dodgeAniClip = this.AddAnimationClip(this.dodgeAniClip, animationState.clip);
			}
			if (animationState.name.IndexOf("idle") >= 0)
			{
				this.idleAniClip = this.AddAnimationClip(this.idleAniClip, animationState.clip);
			}
			if (animationState.name.IndexOf("run") >= 0)
			{
				this.moveAniClip = this.AddAnimationClip(this.moveAniClip, animationState.clip);
			}
			if (animationState.name.IndexOf("hurt01") >= 0)
			{
				this.hitAniClip = this.AddAnimationClip(this.hitAniClip, animationState.clip);
			}
			if (animationState.name.IndexOf("hurt02") >= 0)
			{
				this.criticalHitAniClip = this.AddAnimationClip(this.criticalHitAniClip, animationState.clip);
			}
			if (animationState.name.IndexOf("skill") >= 0)
			{
				this.meleeAttackAniClip = this.AddAnimationClip(this.meleeAttackAniClip, animationState.clip);
				this.meleeAttackAniDelay = new float[this.meleeAttackAniClip.Length];
				for (int j = 0; j < this.meleeAttackAniClip.Length; j++)
				{
					this.meleeAttackAniDelay[j] = this.meleeAttackAniClip[j].length;
				}
			}
			if (animationState.name.IndexOf("stand") >= 0)
			{
				this.standAniClip = this.AddAnimationClip(this.standAniClip, animationState.clip);
				this.standAniDelay = new float[this.standAniClip.Length];
				for (int k = 0; k < this.standAniClip.Length; k++)
				{
					this.standAniDelay[k] = this.standAniClip[k].length;
				}
			}
		}
	}

	// Token: 0x06002FF9 RID: 12281 RVA: 0x00174640 File Offset: 0x00172840
	public void InitAnimation()
	{
		if (this.meleeAttackAniBody != null)
		{
			this.meleeAttackAnimation = this.meleeAttackAniBody.GetComponent<Animation>();
			if (this.meleeAttackAnimation == null)
			{
				this.meleeAttackAnimation = this.meleeAttackAniBody.AddComponent<Animation>();
			}
			if (this.meleeAttackAniClip != null && this.meleeAttackAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip in this.meleeAttackAniClip)
				{
					if (animationClip != null)
					{
						this.meleeAttackAnimation.AddClip(animationClip, animationClip.name);
						this.meleeAttackAnimation.animation[animationClip.name].layer = 1;
						this.meleeAttackAnimation.animation[animationClip.name].wrapMode = 1;
						this.meleeAttackAnimation.animation[animationClip.name].speed = 1f;
					}
				}
				if (this.meleeAttackAniDelay.Length != this.meleeAttackAniClip.Length)
				{
					float[] array2 = new float[this.meleeAttackAniClip.Length];
					for (int j = 0; j < this.meleeAttackAniClip.Length; j++)
					{
						if (j < this.meleeAttackAniDelay.Length)
						{
							array2[j] = this.meleeAttackAniDelay[j];
						}
						else
						{
							array2[j] = this.meleeAttackAniClip[j].length;
						}
					}
					this.meleeAttackAniDelay = array2;
				}
			}
		}
		if (this.standAniBody != null)
		{
			this.standAnimation = this.standAniBody.GetComponent<Animation>();
			if (this.standAnimation == null)
			{
				this.standAnimation = this.standAniBody.AddComponent<Animation>();
			}
			if (this.standAniClip != null && this.standAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip2 in this.standAniClip)
				{
					if (animationClip2 != null)
					{
						this.standAnimation.AddClip(animationClip2, animationClip2.name);
						this.standAnimation.animation[animationClip2.name].layer = 1;
						this.standAnimation.animation[animationClip2.name].wrapMode = 1;
						this.standAnimation.animation[animationClip2.name].speed = 1f;
					}
				}
				if (this.standAniDelay.Length != this.standAniClip.Length)
				{
					float[] array4 = new float[this.standAniClip.Length];
					for (int l = 0; l < this.standAniClip.Length; l++)
					{
						if (l < this.standAniDelay.Length)
						{
							array4[l] = this.standAniDelay[l];
						}
						else
						{
							array4[l] = this.standAniClip[l].length;
						}
					}
					this.standAniDelay = array4;
				}
			}
		}
		if (this.idleAniBody != null)
		{
			this.idleAnimation = this.idleAniBody.GetComponent<Animation>();
			if (this.idleAnimation == null)
			{
				this.idleAnimation = this.idleAniBody.AddComponent<Animation>();
			}
			if (this.idleAniClip != null && this.idleAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip3 in this.idleAniClip)
				{
					if (animationClip3 != null)
					{
						this.idleAnimation.AddClip(animationClip3, animationClip3.name);
						this.idleAnimation.animation[animationClip3.name].layer = 0;
						this.idleAnimation.animation[animationClip3.name].wrapMode = 2;
						this.idleAnimation.animation[animationClip3.name].speed = 1f;
					}
				}
			}
		}
		if (this.moveAniBody != null)
		{
			this.moveAnimation = this.moveAniBody.GetComponent<Animation>();
			if (this.moveAnimation == null)
			{
				this.moveAnimation = this.moveAniBody.AddComponent<Animation>();
			}
			if (this.moveAniClip != null && this.moveAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip4 in this.moveAniClip)
				{
					if (animationClip4 != null)
					{
						this.moveAnimation.AddClip(animationClip4, animationClip4.name);
						this.moveAnimation.animation[animationClip4.name].layer = 1;
						this.moveAnimation.animation[animationClip4.name].wrapMode = 2;
						this.moveAnimation.animation[animationClip4.name].speed = 1f;
					}
				}
			}
		}
		if (this.hitAniBody != null)
		{
			this.hitAnimation = this.hitAniBody.GetComponent<Animation>();
			if (this.hitAnimation == null)
			{
				this.hitAnimation = this.hitAniBody.AddComponent<Animation>();
			}
			if (this.hitAniClip != null && this.hitAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip5 in this.hitAniClip)
				{
					if (animationClip5 != null)
					{
						this.hitAnimation.AddClip(animationClip5, animationClip5.name);
						this.hitAnimation.animation[animationClip5.name].layer = 1;
						this.hitAnimation.animation[animationClip5.name].wrapMode = 1;
						this.hitAnimation.animation[animationClip5.name].speed = 1f;
					}
				}
			}
		}
		if (this.criticalHitAniBody != null)
		{
			this.criticalHitAnimation = this.criticalHitAniBody.GetComponent<Animation>();
			if (this.criticalHitAnimation == null)
			{
				this.criticalHitAnimation = this.criticalHitAniBody.AddComponent<Animation>();
			}
			if (this.criticalHitAniClip != null && this.criticalHitAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip6 in this.criticalHitAniClip)
				{
					if (animationClip6 != null)
					{
						this.criticalHitAnimation.AddClip(animationClip6, animationClip6.name);
						this.criticalHitAnimation.animation[animationClip6.name].layer = 1;
						this.criticalHitAnimation.animation[animationClip6.name].wrapMode = 1;
						this.criticalHitAnimation.animation[animationClip6.name].speed = 1f;
					}
				}
			}
		}
		if (this.dodgeAniBody != null)
		{
			this.dodgeAnimation = this.dodgeAniBody.GetComponent<Animation>();
			if (this.dodgeAnimation == null)
			{
				this.dodgeAnimation = this.dodgeAniBody.AddComponent<Animation>();
			}
			if (this.dodgeAniClip != null && this.dodgeAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip7 in this.dodgeAniClip)
				{
					if (animationClip7 != null)
					{
						this.dodgeAnimation.AddClip(animationClip7, animationClip7.name);
						this.dodgeAnimation.animation[animationClip7.name].layer = 1;
						this.dodgeAnimation.animation[animationClip7.name].wrapMode = 1;
						this.dodgeAnimation.animation[animationClip7.name].speed = 1f;
					}
				}
			}
		}
		this.dodgeIndex = 0;
		if (this.destroyAniBody != null)
		{
			this.destroyAnimation = this.destroyAniBody.GetComponent<Animation>();
			if (this.destroyAnimation == null)
			{
				this.destroyAnimation = this.destroyAniBody.AddComponent<Animation>();
			}
			if (this.destroyAniClip != null && this.destroyAniClip.Length > 0)
			{
				foreach (AnimationClip animationClip8 in this.destroyAniClip)
				{
					if (animationClip8 != null)
					{
						this.destroyAnimation.AddClip(animationClip8, animationClip8.name);
						this.destroyAnimation.animation[animationClip8.name].layer = 1;
						this.destroyAnimation.animation[animationClip8.name].wrapMode = 1;
						this.destroyAnimation.animation[animationClip8.name].speed = 1f;
					}
				}
				if (this.destroyAniDelay.Length != this.destroyAniClip.Length)
				{
					float[] array11 = new float[this.destroyAniClip.Length];
					for (int num5 = 0; num5 < this.destroyAniClip.Length; num5++)
					{
						if (num5 < this.destroyAniDelay.Length)
						{
							array11[num5] = this.destroyAniDelay[num5];
						}
						else
						{
							array11[num5] = this.destroyAniClip[num5].length;
						}
					}
					this.destroyAniDelay = array11;
				}
			}
		}
		this.PlayIdle();
	}

	// Token: 0x06002FFA RID: 12282 RVA: 0x00174FCC File Offset: 0x001731CC
	public void PlaySkill(int iSkillID, List<AttackInstance> TileList)
	{
		AnimationEventTrigger animationEventTrigger = this.meleeAttackAniBody.GetComponent<AnimationEventTrigger>();
		if (animationEventTrigger == null)
		{
			animationEventTrigger = this.meleeAttackAniBody.AddComponent<AnimationEventTrigger>();
			if (animationEventTrigger == null)
			{
				return;
			}
		}
		animationEventTrigger.PlaySkill(iSkillID, TileList);
	}

	// Token: 0x06002FFB RID: 12283 RVA: 0x00175014 File Offset: 0x00173214
	public void PlayMovieSkill(int iSkillID, List<AttackInstance> TileList)
	{
		if (this.meleeAttackAniBody == null)
		{
			return;
		}
		AnimationEventTrigger animationEventTrigger = this.meleeAttackAniBody.GetComponent<AnimationEventTrigger>();
		if (animationEventTrigger == null)
		{
			animationEventTrigger = this.meleeAttackAniBody.AddComponent<AnimationEventTrigger>();
			if (animationEventTrigger == null)
			{
				return;
			}
		}
		animationEventTrigger.PlayMovieSkill(iSkillID, TileList);
	}

	// Token: 0x06002FFC RID: 12284 RVA: 0x00175070 File Offset: 0x00173270
	public float PlayMeleeAttack()
	{
		if (this.meleeAttackAniBody != null && this.meleeAttackAniClip != null && this.meleeAttackAniClip.Length > 0)
		{
			int num = Random.Range(0, this.meleeAttackAniClip.Length - 1);
			this.meleeAttackAnimation.CrossFade(this.meleeAttackAniClip[num].name);
			return this.meleeAttackAniDelay[num];
		}
		return 0f;
	}

	// Token: 0x06002FFD RID: 12285 RVA: 0x001750E0 File Offset: 0x001732E0
	public float PlayStand()
	{
		if (this.standAniBody != null && this.standAniClip != null && this.standAniClip.Length > 0)
		{
			int num = Random.Range(0, this.standAniClip.Length - 1);
			this.standAnimation.CrossFade(this.standAniClip[num].name);
			return this.standAniDelay[num];
		}
		return 0f;
	}

	// Token: 0x06002FFE RID: 12286 RVA: 0x00175150 File Offset: 0x00173350
	public void PlayUseItemSelf(List<AttackInstance> aInstList)
	{
		AnimationEventTrigger animationEventTrigger = this.standAniBody.GetComponent<AnimationEventTrigger>();
		if (animationEventTrigger == null)
		{
			animationEventTrigger = this.standAniBody.AddComponent<AnimationEventTrigger>();
			if (animationEventTrigger == null)
			{
				return;
			}
		}
		animationEventTrigger.PlayUseItemSelf(aInstList);
	}

	// Token: 0x06002FFF RID: 12287 RVA: 0x00175198 File Offset: 0x00173398
	public void PlayUseTactic(UnitTB unit)
	{
		AnimationEventTrigger animationEventTrigger = this.standAniBody.GetComponent<AnimationEventTrigger>();
		if (animationEventTrigger == null)
		{
			animationEventTrigger = this.standAniBody.AddComponent<AnimationEventTrigger>();
			if (animationEventTrigger == null)
			{
				return;
			}
		}
		animationEventTrigger.PlayUseTactic(unit);
	}

	// Token: 0x06003000 RID: 12288 RVA: 0x001751E0 File Offset: 0x001733E0
	public void PlayUseSchedule(UnitTB unit, int skillno)
	{
		AnimationEventTrigger animationEventTrigger = this.standAniBody.GetComponent<AnimationEventTrigger>();
		if (animationEventTrigger == null)
		{
			animationEventTrigger = this.standAniBody.AddComponent<AnimationEventTrigger>();
			if (animationEventTrigger == null)
			{
				return;
			}
		}
		animationEventTrigger.PlayUseSchedule(unit, skillno);
	}

	// Token: 0x06003001 RID: 12289 RVA: 0x00175228 File Offset: 0x00173428
	public bool PlayMove()
	{
		if (this.moveAniBody != null && this.moveAniClip != null && this.moveAniClip.Length > 0)
		{
			this.moveAnimation.CrossFade(this.moveAniClip[Random.Range(0, this.moveAniClip.Length - 1)].name);
			return true;
		}
		return false;
	}

	// Token: 0x06003002 RID: 12290 RVA: 0x0017528C File Offset: 0x0017348C
	public bool PlayHit()
	{
		if (this.hitAniBody != null && this.hitAniClip != null && this.hitAniClip.Length > 0)
		{
			if (this.hitAnimation.IsPlaying(this.LastHitAniName))
			{
				this.hitAnimation.Stop(this.LastHitAniName);
			}
			this.LastHitAniName = this.hitAniClip[Random.Range(0, this.hitAniClip.Length - 1)].name;
			this.fLastHitLength = this.hitAniClip[Random.Range(0, this.hitAniClip.Length - 1)].length;
			this.hitAnimation.Play(this.LastHitAniName);
			return true;
		}
		return false;
	}

	// Token: 0x06003003 RID: 12291 RVA: 0x00175344 File Offset: 0x00173544
	public bool PlayCritical()
	{
		if (this.criticalHitAniBody != null && this.criticalHitAniClip != null && this.criticalHitAniClip.Length > 0)
		{
			if (this.hitAnimation.IsPlaying(this.LastHitAniName))
			{
				this.hitAnimation.Stop(this.LastHitAniName);
			}
			this.LastHitAniName = this.criticalHitAniClip[Random.Range(0, this.criticalHitAniClip.Length - 1)].name;
			this.fLastHitLength = this.criticalHitAniClip[Random.Range(0, this.criticalHitAniClip.Length - 1)].length;
			this.criticalHitAnimation.Play(this.LastHitAniName);
			return true;
		}
		return false;
	}

	// Token: 0x06003004 RID: 12292 RVA: 0x001753FC File Offset: 0x001735FC
	public bool PlayDodge()
	{
		if (this.dodgeAniBody != null && this.dodgeAniClip != null && this.dodgeAniClip.Length > 0)
		{
			this.dodgeAnimation.animation[this.dodgeAniClip[this.dodgeIndex].name].wrapMode = 1;
			this.dodgeAnimation.Play(this.dodgeAniClip[this.dodgeIndex].name);
			this.dodgeIndex++;
			if (this.dodgeIndex >= this.dodgeAniClip.Length)
			{
				this.dodgeIndex = 0;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06003005 RID: 12293 RVA: 0x001754A4 File Offset: 0x001736A4
	public bool PlayProtectDodge()
	{
		if (this.idleAnimation != null)
		{
			this.idleAnimation.Stop();
		}
		if (this.dodgeAniBody != null && this.dodgeAniClip != null && this.dodgeAniClip.Length > 0)
		{
			this.dodgeAnimation.animation[this.dodgeAniClip[this.dodgeIndex].name].wrapMode = 1;
			this.dodgeAnimation.Play(this.dodgeAniClip[this.dodgeIndex].name);
			this.dodgeIndex++;
			if (this.dodgeIndex >= this.dodgeAniClip.Length)
			{
				this.dodgeIndex = 0;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06003006 RID: 12294 RVA: 0x00175568 File Offset: 0x00173768
	public float PlayDestroyed()
	{
		if (this.idleAnimation != null)
		{
			this.idleAnimation.Stop();
		}
		if (this.destroyAniBody != null && this.destroyAniClip != null && this.destroyAniClip.Length > 0)
		{
			int num = Random.Range(0, this.destroyAniClip.Length - 1);
			this.destroyAnimation.Play(this.destroyAniClip[num].name);
			this.destroyAnimation.playAutomatically = false;
			return this.destroyAniDelay[num];
		}
		return 0f;
	}

	// Token: 0x06003007 RID: 12295 RVA: 0x00175600 File Offset: 0x00173800
	public void PlayIdle()
	{
		if (this.idleAniBody != null && this.idleAniClip != null && this.idleAniClip.Length > 0)
		{
			int num = Random.Range(0, this.idleAniClip.Length - 1);
			this.idleAnimation.Play(this.idleAniClip[num].name);
		}
	}

	// Token: 0x06003008 RID: 12296 RVA: 0x0001E534 File Offset: 0x0001C734
	public void StopMove()
	{
		if (this.moveAnimation != null)
		{
			this.moveAnimation.Stop();
		}
		this.PlayIdle();
	}

	// Token: 0x04003BB3 RID: 15283
	public GameObject meleeAttackAniBody;

	// Token: 0x04003BB4 RID: 15284
	public Animation meleeAttackAnimation;

	// Token: 0x04003BB5 RID: 15285
	public AnimationClip[] meleeAttackAniClip;

	// Token: 0x04003BB6 RID: 15286
	public float[] meleeAttackAniDelay;

	// Token: 0x04003BB7 RID: 15287
	public GameObject standAniBody;

	// Token: 0x04003BB8 RID: 15288
	public Animation standAnimation;

	// Token: 0x04003BB9 RID: 15289
	public AnimationClip[] standAniClip;

	// Token: 0x04003BBA RID: 15290
	public float[] standAniDelay;

	// Token: 0x04003BBB RID: 15291
	public GameObject idleAniBody;

	// Token: 0x04003BBC RID: 15292
	public Animation idleAnimation;

	// Token: 0x04003BBD RID: 15293
	public AnimationClip[] idleAniClip;

	// Token: 0x04003BBE RID: 15294
	public GameObject moveAniBody;

	// Token: 0x04003BBF RID: 15295
	public Animation moveAnimation;

	// Token: 0x04003BC0 RID: 15296
	public AnimationClip[] moveAniClip;

	// Token: 0x04003BC1 RID: 15297
	public GameObject hitAniBody;

	// Token: 0x04003BC2 RID: 15298
	public Animation hitAnimation;

	// Token: 0x04003BC3 RID: 15299
	public AnimationClip[] hitAniClip;

	// Token: 0x04003BC4 RID: 15300
	public GameObject dodgeAniBody;

	// Token: 0x04003BC5 RID: 15301
	public Animation dodgeAnimation;

	// Token: 0x04003BC6 RID: 15302
	public AnimationClip[] dodgeAniClip;

	// Token: 0x04003BC7 RID: 15303
	private int dodgeIndex;

	// Token: 0x04003BC8 RID: 15304
	public GameObject criticalHitAniBody;

	// Token: 0x04003BC9 RID: 15305
	public Animation criticalHitAnimation;

	// Token: 0x04003BCA RID: 15306
	public AnimationClip[] criticalHitAniClip;

	// Token: 0x04003BCB RID: 15307
	public GameObject destroyAniBody;

	// Token: 0x04003BCC RID: 15308
	public Animation destroyAnimation;

	// Token: 0x04003BCD RID: 15309
	public AnimationClip[] destroyAniClip;

	// Token: 0x04003BCE RID: 15310
	public float[] destroyAniDelay;

	// Token: 0x04003BCF RID: 15311
	private string LastHitAniName = "empty";

	// Token: 0x04003BD0 RID: 15312
	public float fLastHitLength;
}
