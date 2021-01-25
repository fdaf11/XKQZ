using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x0200065E RID: 1630
	public class FX_SpawnDirection : MonoBehaviour
	{
		// Token: 0x060027FF RID: 10239 RVA: 0x0013C078 File Offset: 0x0013A278
		private void Start()
		{
			this.counter = 0;
			this.timeTemp = Time.time;
			if (this.TimeSpawn <= 0f)
			{
				for (int i = 0; i < this.Number - 1; i++)
				{
					if (this.UseObjectForward)
					{
						this.Direction = base.transform.forward;
					}
					this.Spawn(base.transform.position + this.Direction * this.Frequency * (float)i);
				}
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x0013C118 File Offset: 0x0013A318
		private void Update()
		{
			if (this.counter >= this.Number - 1)
			{
				Object.Destroy(base.gameObject);
			}
			if (this.TimeSpawn > 0f && Time.time > this.timeTemp + this.TimeSpawn)
			{
				if (this.UseObjectForward)
				{
					this.Direction = base.transform.forward + new Vector3(base.transform.right.x * Random.Range(-this.Noise.x, this.Noise.x), base.transform.right.y * Random.Range(-this.Noise.y, this.Noise.y), base.transform.right.z * Random.Range(-this.Noise.z, this.Noise.z)) * 0.01f;
				}
				this.Spawn(base.transform.position + this.Direction * this.Frequency * (float)this.counter);
				this.counter++;
				this.timeTemp = Time.time;
			}
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x0013C278 File Offset: 0x0013A478
		private void Spawn(Vector3 position)
		{
			if (this.FXSpawn != null)
			{
				Quaternion rotation = base.transform.rotation;
				if (!this.FixRotation)
				{
					rotation = this.FXSpawn.transform.rotation;
				}
				GameObject gameObject = (GameObject)Object.Instantiate(this.FXSpawn, position, rotation);
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

		// Token: 0x040031FA RID: 12794
		public int Number = 10;

		// Token: 0x040031FB RID: 12795
		public float Frequency = 1f;

		// Token: 0x040031FC RID: 12796
		public bool FixRotation;

		// Token: 0x040031FD RID: 12797
		public bool Normal;

		// Token: 0x040031FE RID: 12798
		public GameObject FXSpawn;

		// Token: 0x040031FF RID: 12799
		public float LifeTime;

		// Token: 0x04003200 RID: 12800
		public float TimeSpawn;

		// Token: 0x04003201 RID: 12801
		private float timeTemp;

		// Token: 0x04003202 RID: 12802
		public bool UseObjectForward = true;

		// Token: 0x04003203 RID: 12803
		public Vector3 Direction = Vector3.forward;

		// Token: 0x04003204 RID: 12804
		public Vector3 Noise = Vector3.zero;

		// Token: 0x04003205 RID: 12805
		private int counter;
	}
}
