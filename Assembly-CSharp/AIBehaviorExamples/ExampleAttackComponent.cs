using System;
using AIBehavior;
using UnityEngine;

namespace AIBehaviorExamples
{
	// Token: 0x02000007 RID: 7
	public class ExampleAttackComponent : MonoBehaviour
	{
		// Token: 0x0600000E RID: 14 RVA: 0x0000258C File Offset: 0x0000078C
		public void MeleeAttack(AttackData attackData)
		{
			Debug.Log("Melee attack");
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00021D98 File Offset: 0x0001FF98
		public void RangedAttack(AttackData attackData)
		{
			if (attackData.target != null)
			{
				this.launchPointWeapon.LookAt(attackData.target);
				GameObject gameObject = Object.Instantiate(this.projectilePrefab, this.launchPointWeapon.position, this.launchPointWeapon.rotation) as GameObject;
				ExampleProjectile component = gameObject.GetComponent<ExampleProjectile>();
				component.damage = attackData.damage;
				Debug.Log(string.Concat(new object[]
				{
					"Attacked target '",
					attackData.target.name,
					"' with attack state named '",
					attackData.attackState.name,
					"' with damage ",
					attackData.damage
				}));
			}
			else
			{
				Debug.LogWarning("attackData.target is null, you may want to have a NoPlayerInSight trigger on the AI '" + attackData.attackState.transform.parent.name + "'");
			}
		}

		// Token: 0x0400000A RID: 10
		public GameObject projectilePrefab;

		// Token: 0x0400000B RID: 11
		public Transform launchPointWeapon;
	}
}
