using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x02000663 RID: 1635
	public class FX_ElectroLine : MonoBehaviour
	{
		// Token: 0x06002818 RID: 10264 RVA: 0x0013CBE0 File Offset: 0x0013ADE0
		private void Start()
		{
			if (this.StartObject)
			{
				this.StartPosition = this.StartObject.transform.position;
			}
			if (this.EndObject)
			{
				this.EndPosition = this.EndObject.transform.position;
			}
			if (this.RayCast)
			{
				this.StartPosition = base.transform.position;
				Ray ray;
				ray..ctor(base.transform.position, base.transform.forward);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, ref raycastHit, this.Length))
				{
					this.EndPosition = raycastHit.point;
					this.vertexCount = (int)(raycastHit.distance / this.DistancePerSegment);
				}
			}
			else
			{
				this.vertexCount = (int)(Vector3.Distance(this.StartPosition, this.EndPosition) / this.DistancePerSegment);
			}
			if (this.LineRender == null)
			{
				this.LineRender = base.GetComponent<LineRenderer>();
				this.LineRender.SetVertexCount(this.vertexCount);
				this.vertexTemps = new Vector3[this.vertexCount];
				this.vertexTempsTarget = new Vector3[this.vertexCount];
				this.vertexTempsCurrent = new Vector3[this.vertexCount];
				for (int i = 0; i < this.vertexCount; i++)
				{
					this.vertexTemps[i] = this.StartPosition + base.transform.forward * this.DistancePerSegment * (float)i;
					if (i == 0 && this.StartObject)
					{
						this.vertexTemps[i] = this.StartPosition;
					}
					if (i == this.vertexCount - 1 && this.EndObject)
					{
						this.vertexTemps[i] = this.EndPosition;
					}
					this.vertexTempsTarget[i] = this.vertexTemps[i];
					this.vertexTempsCurrent[i] = this.vertexTemps[i];
					this.LineRender.SetPosition(i, this.vertexTemps[i]);
					if (!this.EndObject && i == this.vertexCount - 1)
					{
						this.EndPosition = this.vertexTemps[i];
					}
				}
			}
			if (this.FXStart != null)
			{
				Quaternion rotation = base.transform.rotation;
				if (!this.FixRotation)
				{
					rotation = this.FXStart.transform.rotation;
				}
				this.fxStart = (GameObject)Object.Instantiate(this.FXStart, this.StartPosition, rotation);
				if (this.Normal)
				{
					this.fxStart.transform.forward = base.transform.forward;
				}
				if (this.ParentFXstart)
				{
					this.fxStart.transform.parent = base.transform;
				}
			}
			if (this.FXEnd != null)
			{
				Quaternion rotation2 = base.transform.rotation;
				if (!this.FixRotation)
				{
					rotation2 = this.FXEnd.transform.rotation;
				}
				this.fxEnd = (GameObject)Object.Instantiate(this.FXEnd, this.EndPosition, rotation2);
				if (this.Normal)
				{
					this.fxEnd.transform.forward = base.transform.forward;
				}
				if (this.ParentFXend)
				{
					this.fxEnd.transform.parent = base.transform;
				}
			}
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x0013CFB4 File Offset: 0x0013B1B4
		private void UpdatePosition()
		{
			base.transform.forward = (this.EndPosition - this.StartPosition).normalized;
			for (int i = 0; i < this.vertexCount; i++)
			{
				this.vertexTemps[i] = this.StartPosition + base.transform.forward * this.DistancePerSegment * (float)i;
			}
			if (this.fxStart)
			{
				this.fxStart.transform.position = this.StartPosition;
			}
			if (this.fxEnd)
			{
				this.fxEnd.transform.position = this.EndPosition;
			}
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x0013D084 File Offset: 0x0013B284
		private void Update()
		{
			if (this.StartObject)
			{
				this.StartPosition = this.StartObject.transform.position;
			}
			if (this.EndObject)
			{
				this.EndPosition = this.EndObject.transform.position;
			}
			if (this.KeepConnect)
			{
				this.UpdatePosition();
			}
			if (this.LineRender == null)
			{
				return;
			}
			if (Time.time > this.noiseIntervalTemp + this.NoiseInterval)
			{
				this.noiseIntervalTemp = Time.time;
				if (this.Noise > 0f)
				{
					for (int i = 0; i < this.vertexCount; i++)
					{
						Vector3 vector;
						vector..ctor((float)Random.Range(-100, 100) * this.Noise * base.transform.up.x, (float)Random.Range(-100, 100) * this.Noise * base.transform.up.y, (float)Random.Range(-100, 100) * this.Noise * base.transform.up.z);
						Vector3 vector2;
						vector2..ctor((float)Random.Range(-100, 100) * this.Noise * base.transform.right.x, (float)Random.Range(-100, 100) * this.Noise * base.transform.right.y, (float)Random.Range(-100, 100) * this.Noise * base.transform.right.z);
						this.vertexTempsTarget[i] = this.vertexTemps[i] + vector2 + vector;
						if (!this.Blending)
						{
							this.LineRender.SetPosition(i, this.vertexTemps[i] + vector2 + vector);
						}
					}
				}
			}
			if (this.Blending)
			{
				for (int j = 0; j < this.vertexCount; j++)
				{
					if (j != 0 && j != this.vertexCount - 1)
					{
						this.vertexTempsCurrent[j] = Vector3.Lerp(this.vertexTempsCurrent[j], this.vertexTempsTarget[j], 0.5f);
						this.LineRender.SetPosition(j, this.vertexTempsCurrent[j]);
					}
				}
			}
		}

		// Token: 0x04003220 RID: 12832
		public LineRenderer LineRender;

		// Token: 0x04003221 RID: 12833
		public bool RayCast;

		// Token: 0x04003222 RID: 12834
		public float Length = 300f;

		// Token: 0x04003223 RID: 12835
		public Transform StartObject;

		// Token: 0x04003224 RID: 12836
		public Transform EndObject;

		// Token: 0x04003225 RID: 12837
		public Vector3 EndPosition;

		// Token: 0x04003226 RID: 12838
		public Vector3 StartPosition;

		// Token: 0x04003227 RID: 12839
		public float DistancePerSegment = 0.5f;

		// Token: 0x04003228 RID: 12840
		public float Noise = 0.5f;

		// Token: 0x04003229 RID: 12841
		public float NoiseInterval = 0.05f;

		// Token: 0x0400322A RID: 12842
		public bool Blending = true;

		// Token: 0x0400322B RID: 12843
		private Vector3[] vertexTemps;

		// Token: 0x0400322C RID: 12844
		private Vector3[] vertexTempsTarget;

		// Token: 0x0400322D RID: 12845
		private Vector3[] vertexTempsCurrent;

		// Token: 0x0400322E RID: 12846
		private int vertexCount;

		// Token: 0x0400322F RID: 12847
		private float noiseIntervalTemp;

		// Token: 0x04003230 RID: 12848
		public bool FixRotation;

		// Token: 0x04003231 RID: 12849
		public bool Normal;

		// Token: 0x04003232 RID: 12850
		public bool ParentFXstart = true;

		// Token: 0x04003233 RID: 12851
		public bool ParentFXend = true;

		// Token: 0x04003234 RID: 12852
		public GameObject FXStart;

		// Token: 0x04003235 RID: 12853
		public GameObject FXEnd;

		// Token: 0x04003236 RID: 12854
		private GameObject fxStart;

		// Token: 0x04003237 RID: 12855
		private GameObject fxEnd;

		// Token: 0x04003238 RID: 12856
		public bool KeepConnect;
	}
}
