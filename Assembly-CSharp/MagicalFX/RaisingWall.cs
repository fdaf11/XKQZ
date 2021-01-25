using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000661 RID: 1633
	public class RaisingWall : MonoBehaviour
	{
		// Token: 0x0600280B RID: 10251 RVA: 0x0013C56C File Offset: 0x0013A76C
		private void Start()
		{
			if (this.Skill != null)
			{
				FX_SpawnDirection component = this.Skill.GetComponent<FX_SpawnDirection>();
				if (component)
				{
					this.Offset = (float)(-(float)((int)((float)component.Number / 2f)));
				}
			}
			this.Raising();
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x0013C5C0 File Offset: 0x0013A7C0
		private void Raising()
		{
			if (this.Skill != null)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(this.Skill, base.transform.position + base.transform.forward * this.Distance + base.transform.right * this.Offset, this.Skill.transform.rotation);
				gameObject.transform.forward = base.transform.right;
			}
		}

		// Token: 0x04003215 RID: 12821
		public GameObject Skill;

		// Token: 0x04003216 RID: 12822
		public float Offset = -7f;

		// Token: 0x04003217 RID: 12823
		public float Distance = 2f;
	}
}
