using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000671 RID: 1649
public class MecanimMeshAnimatorController : MonoBehaviour
{
	// Token: 0x06002852 RID: 10322 RVA: 0x0013F37C File Offset: 0x0013D57C
	protected virtual void Awake()
	{
		if (!this.meshAnimator)
		{
			Debug.LogError("MecanimMeshAnimatorController.meshAnimator is null", this);
			return;
		}
		for (int i = 0; i < this.meshAnimator.animations.Length; i++)
		{
			this.animHashes.Add(Animator.StringToHash("Base Layer." + this.meshAnimator.animations[i].name), this.meshAnimator.animations[i].name);
		}
	}

	// Token: 0x06002853 RID: 10323 RVA: 0x0013F404 File Offset: 0x0013D604
	protected virtual void LateUpdate()
	{
		int nameHash = this.animator.GetCurrentAnimatorStateInfo(0).nameHash;
		if (this.animHashes.ContainsKey(nameHash) && this.cAnim != this.animHashes[nameHash])
		{
			this.cAnim = this.animHashes[nameHash];
			if (this.crossFade)
			{
				this.meshAnimator.Crossfade(this.animHashes[nameHash]);
			}
			else
			{
				this.meshAnimator.Play(this.animHashes[nameHash]);
			}
		}
	}

	// Token: 0x0400329B RID: 12955
	public Animator animator;

	// Token: 0x0400329C RID: 12956
	public MeshAnimator meshAnimator;

	// Token: 0x0400329D RID: 12957
	public bool crossFade;

	// Token: 0x0400329E RID: 12958
	private Dictionary<int, string> animHashes = new Dictionary<int, string>();

	// Token: 0x0400329F RID: 12959
	private string cAnim = string.Empty;
}
