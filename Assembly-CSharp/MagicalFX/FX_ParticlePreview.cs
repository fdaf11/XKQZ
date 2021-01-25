using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000657 RID: 1623
	public class FX_ParticlePreview : MonoBehaviour
	{
		// Token: 0x060027EC RID: 10220 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x0013BC38 File Offset: 0x00139E38
		public void AddParticle(Vector3 position)
		{
			if (Input.GetKeyDown(273))
			{
				this.Index++;
				if (this.Index >= this.Particles.Length || this.Index < 0)
				{
					this.Index = 0;
				}
			}
			if (Input.GetKeyDown(274))
			{
				this.Index--;
				if (this.Index < 0)
				{
					this.Index = this.Particles.Length - 1;
				}
			}
			if (this.Index >= this.Particles.Length || this.Index < 0)
			{
				this.Index = 0;
			}
			if (this.Index >= 0 && this.Index < this.Particles.Length && this.Particles.Length > 0)
			{
				Object.Instantiate(this.Particles[this.Index], position, this.Particles[this.Index].transform.rotation);
			}
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x0013BD40 File Offset: 0x00139F40
		private void Update()
		{
			base.transform.Rotate(Vector3.up * this.RotationSpeed * Time.deltaTime);
			RaycastHit raycastHit = default(RaycastHit);
			if (Input.GetButtonDown("Fire1"))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, ref raycastHit, 1000f))
				{
					this.AddParticle(raycastHit.point + Vector3.up);
				}
			}
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x0013BDC4 File Offset: 0x00139FC4
		private void OnGUI()
		{
			string text = string.Empty;
			if (this.Index >= 0 && this.Index < this.Particles.Length && this.Particles.Length > 0)
			{
				text = this.Particles[this.Index].name;
			}
			GUI.Label(new Rect(30f, 30f, (float)Screen.width, 100f), "Change FX : Key Up / Down \nCurrent FX " + text);
			if (GUI.Button(new Rect(30f, 90f, 200f, 30f), "Next"))
			{
				this.Index++;
				this.AddParticle(Vector3.up);
			}
			if (GUI.Button(new Rect(30f, 130f, 200f, 30f), "Prev"))
			{
				this.Index--;
				this.AddParticle(Vector3.up);
			}
			if (this.logo)
			{
				GUI.DrawTexture(new Rect((float)(Screen.width - this.logo.width - 30), 30f, (float)this.logo.width, (float)this.logo.height), this.logo);
			}
		}

		// Token: 0x040031E8 RID: 12776
		public GameObject[] Particles;

		// Token: 0x040031E9 RID: 12777
		public float RotationSpeed = 3f;

		// Token: 0x040031EA RID: 12778
		public int Index;

		// Token: 0x040031EB RID: 12779
		public Texture2D logo;
	}
}
