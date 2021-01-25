using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000662 RID: 1634
	public class Wizard : MonoBehaviour
	{
		// Token: 0x0600280E RID: 10254 RVA: 0x0001A700 File Offset: 0x00018900
		private void Start()
		{
			this.timeTemp = Time.time;
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x0013C658 File Offset: 0x0013A858
		private void Update()
		{
			if (this.Showtime)
			{
				if (Time.time >= this.timeTemp + this.Delay)
				{
					Ray ray;
					ray..ctor(base.transform.position + new Vector3(Random.Range(-this.RandomSize, this.RandomSize), 0f, Random.Range(-this.RandomSize, this.RandomSize)), -Vector3.up);
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, ref raycastHit, 100f))
					{
						this.positionLook = raycastHit.point;
					}
					Quaternion rotation = Quaternion.LookRotation((this.positionLook - base.transform.position).normalized);
					rotation.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
					base.transform.rotation = rotation;
					if (this.RandomSkill)
					{
						this.Index = Random.Range(0, this.Skills.Length);
					}
					else
					{
						this.Index++;
					}
					this.Deploy();
					this.timeTemp = Time.time;
				}
			}
			else
			{
				this.Aim();
				if (Input.GetMouseButtonDown(0))
				{
					this.Deploy();
				}
			}
			this.KeyUpdate();
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x0013C7B0 File Offset: 0x0013A9B0
		private void KeyUpdate()
		{
			if (Input.GetKeyDown(97))
			{
				this.Index--;
			}
			if (Input.GetKeyDown(100))
			{
				this.Index++;
			}
			if (this.Index < 0)
			{
				this.Index = this.Skills.Length - 1;
			}
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x0013C810 File Offset: 0x0013AA10
		private void Deploy()
		{
			if (this.Index >= this.Skills.Length || this.Index < 0)
			{
				this.Index = 0;
			}
			FX_Position component = this.Skills[this.Index].GetComponent<FX_Position>();
			if (component)
			{
				if (component.Mode == SpawnMode.Static)
				{
					this.Place(this.Skills[this.Index]);
				}
				if (component.Mode == SpawnMode.OnDirection)
				{
					this.PlaceDirection(this.Skills[this.Index]);
				}
			}
			else
			{
				this.Shoot(this.Skills[this.Index]);
			}
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x0013C8B8 File Offset: 0x0013AAB8
		private void Aim()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, ref raycastHit, 100f))
			{
				this.positionLook = raycastHit.point;
			}
			Quaternion quaternion = Quaternion.LookRotation((this.positionLook - base.transform.position).normalized);
			quaternion.eulerAngles = new Vector3(0f, quaternion.eulerAngles.y, 0f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, quaternion, 0.5f);
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x0013C960 File Offset: 0x0013AB60
		private void Shoot(GameObject skill)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(skill, base.transform.position + Vector3.up * 0.5f + base.transform.forward, skill.transform.rotation);
			gameObject.transform.forward = (this.positionLook - base.transform.position).normalized;
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x0001A70D File Offset: 0x0001890D
		private void Place(GameObject skill)
		{
			Object.Instantiate(skill, this.positionLook, skill.transform.rotation);
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x0013C9DC File Offset: 0x0013ABDC
		private void PlaceDirection(GameObject skill)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(skill, base.transform.position + base.transform.forward, skill.transform.rotation);
			FX_Position component = gameObject.GetComponent<FX_Position>();
			if (component.Mode == SpawnMode.OnDirection)
			{
				component.transform.forward = base.transform.forward;
			}
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x0013CA44 File Offset: 0x0013AC44
		private void OnGUI()
		{
			string text = string.Empty;
			if (this.Index >= 0 && this.Index < this.Skills.Length && this.Skills.Length > 0)
			{
				text = this.Skills[this.Index].name;
			}
			GUI.Label(new Rect(30f, 30f, (float)Screen.width, 100f), string.Empty + text);
			if (GUI.Button(new Rect(30f, (float)(Screen.height - 40), 100f, 30f), "Prev"))
			{
				this.Index--;
			}
			if (GUI.Button(new Rect(140f, (float)(Screen.height - 40), 100f, 30f), "Next"))
			{
				this.Index++;
			}
			if (GUI.Button(new Rect(250f, (float)(Screen.height - 40), 100f, 30f), "Show time"))
			{
				this.Showtime = !this.Showtime;
			}
			if (this.Index < 0)
			{
				this.Index = this.Skills.Length - 1;
			}
		}

		// Token: 0x04003218 RID: 12824
		public GameObject[] Skills;

		// Token: 0x04003219 RID: 12825
		private Vector3 positionLook;

		// Token: 0x0400321A RID: 12826
		public int Index;

		// Token: 0x0400321B RID: 12827
		public bool Showtime;

		// Token: 0x0400321C RID: 12828
		public float Delay = 1f;

		// Token: 0x0400321D RID: 12829
		public float RandomSize = 10f;

		// Token: 0x0400321E RID: 12830
		public bool RandomSkill;

		// Token: 0x0400321F RID: 12831
		private float timeTemp;
	}
}
