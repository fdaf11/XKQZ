using System;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000140 RID: 320
[RequireComponent(typeof(CharacterController))]
public class PlayerAnimation
{
	// Token: 0x060006BB RID: 1723 RVA: 0x00006066 File Offset: 0x00004266
	public PlayerAnimation(GameObject Obj)
	{
		this.m_Anims = Obj.GetComponentsInChildren<Animation>();
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00047614 File Offset: 0x00045814
	private void setAnimation(Animation anim, string name, int layer, WrapMode mod)
	{
		if (anim == null)
		{
			return;
		}
		if (anim[name] != null)
		{
			return;
		}
		Game.g_ModelBundle.LoadAnimation(anim, name);
		if (anim[name] != null)
		{
			anim[name].layer = layer;
			anim[name].wrapMode = mod;
		}
		else
		{
			Debug.LogError(anim.name + " " + name + " animation clip  no found");
		}
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x0000607A File Offset: 0x0000427A
	private void setAnimationSpeed(Animation anim, string name, float speed)
	{
		if (anim == null)
		{
			return;
		}
		if (anim[name] == null)
		{
			return;
		}
		anim[name].speed = speed;
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x0004769C File Offset: 0x0004589C
	private void play(Animation anim, string name, float fade, float speed)
	{
		if (anim == null)
		{
			return;
		}
		this.setAnimation(anim, name, 0, 1);
		if (anim[name] == null)
		{
			return;
		}
		if (name == "stand01" || name == "run")
		{
			anim[name].wrapMode = 2;
		}
		anim[name].layer = 0;
		anim[name].speed = speed;
		anim.CrossFade(name, fade);
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x00047724 File Offset: 0x00045924
	private float play(Animation anim, string name, float fade, float speed, int layer, WrapMode mod)
	{
		if (anim == null)
		{
			return 0f;
		}
		this.setAnimation(anim, name, layer, mod);
		if (anim[name] == null)
		{
			return 0f;
		}
		anim[name].speed = speed;
		anim.CrossFade(name, fade);
		anim[name].layer = layer;
		anim[name].wrapMode = mod;
		return anim[name].length;
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x000477A8 File Offset: 0x000459A8
	private void setTalkAni(Animation anim, string name, float fade, float speed, int layer, WrapMode mod)
	{
		if (anim == null)
		{
			return;
		}
		this.setAnimation(anim, name, layer, mod);
		if (anim[name] == null)
		{
			return;
		}
		anim[name].speed = speed;
		anim.CrossFade(name, fade);
		anim[name].layer = layer;
		anim[name].wrapMode = mod;
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x00047814 File Offset: 0x00045A14
	public void SetAnimation()
	{
		for (int i = 0; i < this.m_Anims.Length; i++)
		{
			this.setAnimation(this.m_Anims[i], "stand01", 0, 2);
			this.setAnimation(this.m_Anims[i], "run", 0, 2);
		}
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x00047864 File Offset: 0x00045A64
	public void SetAnimationSpeed(string name, float speed)
	{
		for (int i = 0; i < this.m_Anims.Length; i++)
		{
			this.setAnimationSpeed(this.m_Anims[i], name, speed);
		}
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x0004789C File Offset: 0x00045A9C
	public void Play(string name, float fade, float speed)
	{
		for (int i = 0; i < this.m_Anims.Length; i++)
		{
			this.play(this.m_Anims[i], name, fade, speed);
		}
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x000060A9 File Offset: 0x000042A9
	public bool IsPlaying(string name)
	{
		return this.m_Anims[0].IsPlaying(name);
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x000478D4 File Offset: 0x00045AD4
	public float Play(string name, float fade, float speed, int layer, WrapMode mod)
	{
		float num = 0f;
		for (int i = 0; i < this.m_Anims.Length; i++)
		{
			float num2 = this.play(this.m_Anims[i], name, fade, speed, layer, mod);
			if (num2 > num)
			{
				num = num2;
			}
		}
		return num;
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x00047920 File Offset: 0x00045B20
	public void SetTalkAni(string name, float fade, float speed, int layer, WrapMode mod)
	{
		for (int i = 0; i < this.m_Anims.Length; i++)
		{
			this.setTalkAni(this.m_Anims[i], name, fade, speed, layer, mod);
		}
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x0004795C File Offset: 0x00045B5C
	public void ResetAnimationLayer()
	{
		for (int i = 0; i < this.m_Anims.Length; i++)
		{
			if (!(this.m_Anims[i] == null))
			{
				foreach (object obj in this.m_Anims[i])
				{
					AnimationState animationState = (AnimationState)obj;
					animationState.layer = 0;
				}
			}
		}
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x000479F4 File Offset: 0x00045BF4
	public int GetAnimationLayer(string name)
	{
		Animation animation = this.m_Anims[0];
		return animation[name].layer;
	}

	// Token: 0x0400073D RID: 1853
	private Animation[] m_Anims;

	// Token: 0x0400073E RID: 1854
	private string m_CrtAnimName;

	// Token: 0x0400073F RID: 1855
	private PlayerAnimation.AnimCallBack m_CallBack;

	// Token: 0x02000141 RID: 321
	// (Invoke) Token: 0x060006CA RID: 1738
	public delegate void AnimCallBack();
}
