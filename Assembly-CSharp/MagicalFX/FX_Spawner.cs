using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x0200065F RID: 1631
	public class FX_Spawner : MonoBehaviour
	{
		// Token: 0x06002803 RID: 10243 RVA: 0x0013C314 File Offset: 0x0013A514
		private void Start()
		{
			this.timeTemp = Time.time;
			if (this.FXSpawn != null && this.TimeSpawn <= 0f)
			{
				Quaternion rotation = base.transform.rotation;
				if (!this.FixRotation)
				{
					rotation = this.FXSpawn.transform.rotation;
				}
				GameObject gameObject = (GameObject)Object.Instantiate(this.FXSpawn, base.transform.position, rotation);
				if (this.Normal)
				{
					gameObject.transform.forward = base.transform.forward;
				}
				if (this.LifeTime > 0f)
				{
					Object.Destroy(gameObject.gameObject, this.LifeTime);
				}
			}
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x0013C3D4 File Offset: 0x0013A5D4
		private void Update()
		{
			if (this.TimeSpawn > 0f && Time.time > this.timeTemp + this.TimeSpawn)
			{
				if (this.FXSpawn != null)
				{
					Quaternion rotation = base.transform.rotation;
					if (!this.FixRotation)
					{
						rotation = this.FXSpawn.transform.rotation;
					}
					GameObject gameObject = (GameObject)Object.Instantiate(this.FXSpawn, base.transform.position, rotation);
					if (this.Normal)
					{
						gameObject.transform.forward = base.transform.forward;
					}
					if (this.LifeTime > 0f)
					{
						Object.Destroy(gameObject.gameObject, this.LifeTime);
					}
				}
				this.timeTemp = Time.time;
			}
		}

		// Token: 0x04003206 RID: 12806
		public bool FixRotation;

		// Token: 0x04003207 RID: 12807
		public bool Normal;

		// Token: 0x04003208 RID: 12808
		public GameObject FXSpawn;

		// Token: 0x04003209 RID: 12809
		public float LifeTime;

		// Token: 0x0400320A RID: 12810
		public float TimeSpawn;

		// Token: 0x0400320B RID: 12811
		private float timeTemp;
	}
}
