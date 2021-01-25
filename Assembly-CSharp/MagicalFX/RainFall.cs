using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000660 RID: 1632
	public class RainFall : MonoBehaviour
	{
		// Token: 0x06002806 RID: 10246 RVA: 0x0001A66C File Offset: 0x0001886C
		private void Start()
		{
			this.StartRain();
			this.timeTemp = Time.time;
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x0001A67F File Offset: 0x0001887F
		private void Spawn(Vector3 position)
		{
			if (this.Skill == null)
			{
				return;
			}
			Object.Instantiate(this.Skill, position, this.Skill.transform.rotation);
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x0001A6B0 File Offset: 0x000188B0
		public void StartRain()
		{
			this.isRaining = true;
			this.timeTempDuration = Time.time;
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x0013C4AC File Offset: 0x0013A6AC
		private void Update()
		{
			if (this.isRaining)
			{
				if (this.count < this.MaxSpawn && Time.time < this.timeTempDuration + this.Duration)
				{
					if (Time.time > this.timeTemp + this.DropRate)
					{
						this.timeTemp = Time.time;
						this.count++;
						this.Spawn(base.transform.position + new Vector3(Random.Range(-this.AreaSize, this.AreaSize), 0f, Random.Range(-this.AreaSize, this.AreaSize)));
					}
				}
				else
				{
					this.isRaining = false;
				}
			}
		}

		// Token: 0x0400320C RID: 12812
		public GameObject Skill;

		// Token: 0x0400320D RID: 12813
		public float AreaSize = 20f;

		// Token: 0x0400320E RID: 12814
		public int MaxSpawn = 1000;

		// Token: 0x0400320F RID: 12815
		public float Duration = 3f;

		// Token: 0x04003210 RID: 12816
		public float DropRate;

		// Token: 0x04003211 RID: 12817
		private float timeTemp;

		// Token: 0x04003212 RID: 12818
		private float timeTempDuration;

		// Token: 0x04003213 RID: 12819
		private int count;

		// Token: 0x04003214 RID: 12820
		public bool isRaining;
	}
}
