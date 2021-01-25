using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200058C RID: 1420
	public class Spring
	{
		// Token: 0x0600239D RID: 9117 RVA: 0x00116F5C File Offset: 0x0011515C
		public Spring(Transform transform, Spring.TransformType modifier)
		{
			this.m_transform = transform;
			this.Modifier = modifier;
			this.RefreshTransformType();
		}

		// Token: 0x1700037C RID: 892
		// (set) Token: 0x0600239E RID: 9118 RVA: 0x00017ADD File Offset: 0x00015CDD
		public Transform Transform
		{
			set
			{
				this.m_transform = value;
				this.RefreshTransformType();
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x0600239F RID: 9119 RVA: 0x00017AEC File Offset: 0x00015CEC
		// (set) Token: 0x060023A0 RID: 9120 RVA: 0x00017AF4 File Offset: 0x00015CF4
		public bool Done
		{
			get
			{
				return this.m_done;
			}
			set
			{
				this.m_done = value;
			}
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x00117030 File Offset: 0x00115230
		public void FixedUpdate()
		{
			if (this.m_velocityFadeInEndTime > Time.time)
			{
				this.m_velocityFadeInCap = Mathf.Clamp01(1f - (this.m_velocityFadeInEndTime - Time.time) / this.m_velocityFadeInLength);
			}
			else
			{
				this.m_velocityFadeInCap = 1f;
			}
			if (this.Modifier != this.m_currentTransformType)
			{
				this.RefreshTransformType();
			}
			this.m_transformFunction();
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x00017AFD File Offset: 0x00015CFD
		private void Position()
		{
			this.Calculate();
			this.m_transform.localPosition = this.State;
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x00017B16 File Offset: 0x00015D16
		private void PositionAdditive()
		{
			this.Calculate();
			this.m_transform.localPosition += this.State;
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x00017B3A File Offset: 0x00015D3A
		private void Rotation()
		{
			this.Calculate();
			this.m_transform.localEulerAngles = this.State;
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x00017B53 File Offset: 0x00015D53
		private void RotationAdditive()
		{
			this.Calculate();
			this.m_transform.localEulerAngles += this.State;
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x00017B77 File Offset: 0x00015D77
		private void Scale()
		{
			this.Calculate();
			this.m_transform.localScale = this.State;
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x00017B90 File Offset: 0x00015D90
		private void ScaleAdditive()
		{
			this.Calculate();
			this.m_transform.localScale += this.State;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x001170A4 File Offset: 0x001152A4
		public void RefreshTransformType()
		{
			switch (this.Modifier)
			{
			case Spring.TransformType.Position:
				this.State = this.m_transform.localPosition;
				this.m_transformFunction = new Spring.TransformDelegate(this.Position);
				break;
			case Spring.TransformType.PositionAdditive:
				this.State = this.m_transform.localPosition;
				this.m_transformFunction = new Spring.TransformDelegate(this.PositionAdditive);
				break;
			case Spring.TransformType.Rotation:
				this.State = this.m_transform.localEulerAngles;
				this.m_transformFunction = new Spring.TransformDelegate(this.Rotation);
				break;
			case Spring.TransformType.RotationAdditive:
				this.State = this.m_transform.localEulerAngles;
				this.m_transformFunction = new Spring.TransformDelegate(this.RotationAdditive);
				break;
			case Spring.TransformType.Scale:
				this.State = this.m_transform.localScale;
				this.m_transformFunction = new Spring.TransformDelegate(this.Scale);
				break;
			case Spring.TransformType.ScaleAdditive:
				this.State = this.m_transform.localScale;
				this.m_transformFunction = new Spring.TransformDelegate(this.ScaleAdditive);
				break;
			}
			this.m_currentTransformType = this.Modifier;
			this.RestState = this.State;
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x001171E4 File Offset: 0x001153E4
		protected void Calculate()
		{
			if (this.State == this.RestState)
			{
				this.m_done = true;
				return;
			}
			Vector3 vector = this.RestState - this.State;
			this.m_velocity += Vector3.Scale(vector, this.Stiffness);
			this.m_velocity = Vector3.Scale(this.m_velocity, this.Damping);
			this.m_velocity = Vector3.ClampMagnitude(this.m_velocity, this.MaxVelocity);
			if (Mathf.Abs(this.m_velocity.sqrMagnitude) > this.MinVelocity * this.MinVelocity)
			{
				this.Move();
			}
			else
			{
				this.Reset();
			}
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x001172A0 File Offset: 0x001154A0
		public void AddForce(Vector3 force)
		{
			force *= this.m_velocityFadeInCap;
			this.m_velocity += force;
			this.m_velocity = Vector3.ClampMagnitude(this.m_velocity, this.MaxVelocity);
			this.Move();
			this.m_done = false;
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x00017BB4 File Offset: 0x00015DB4
		public void AddForce(float x, float y, float z)
		{
			this.AddForce(new Vector3(x, y, z));
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x001172F4 File Offset: 0x001154F4
		protected void Move()
		{
			this.State += this.m_velocity;
			this.State = new Vector3(Mathf.Clamp(this.State.x, this.MinState.x, this.MaxState.x), Mathf.Clamp(this.State.y, this.MinState.y, this.MaxState.y), Mathf.Clamp(this.State.z, this.MinState.z, this.MaxState.z));
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x00017BC4 File Offset: 0x00015DC4
		public void Reset()
		{
			this.m_velocity = Vector3.zero;
			this.State = this.RestState;
			this.m_done = true;
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x00017BE4 File Offset: 0x00015DE4
		public void Stop()
		{
			this.m_velocity = Vector3.zero;
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x00017BF1 File Offset: 0x00015DF1
		public void ForceVelocityFadeIn(float seconds)
		{
			this.m_velocityFadeInLength = seconds;
			this.m_velocityFadeInEndTime = Time.time + seconds;
			this.m_velocityFadeInCap = 0f;
		}

		// Token: 0x04002AFF RID: 11007
		public Spring.TransformType Modifier;

		// Token: 0x04002B00 RID: 11008
		protected Spring.TransformDelegate m_transformFunction;

		// Token: 0x04002B01 RID: 11009
		public Vector3 State = Vector3.zero;

		// Token: 0x04002B02 RID: 11010
		protected Spring.TransformType m_currentTransformType;

		// Token: 0x04002B03 RID: 11011
		protected Vector3 m_velocity = Vector3.zero;

		// Token: 0x04002B04 RID: 11012
		public Vector3 RestState = Vector3.zero;

		// Token: 0x04002B05 RID: 11013
		public Vector3 Stiffness = new Vector3(0.5f, 0.5f, 0.5f);

		// Token: 0x04002B06 RID: 11014
		public Vector3 Damping = new Vector3(0.75f, 0.75f, 0.75f);

		// Token: 0x04002B07 RID: 11015
		protected float m_velocityFadeInCap = 1f;

		// Token: 0x04002B08 RID: 11016
		protected float m_velocityFadeInEndTime;

		// Token: 0x04002B09 RID: 11017
		protected float m_velocityFadeInLength;

		// Token: 0x04002B0A RID: 11018
		public float MaxVelocity = 10000f;

		// Token: 0x04002B0B RID: 11019
		public float MinVelocity = 1E-07f;

		// Token: 0x04002B0C RID: 11020
		public Vector3 MaxState = new Vector3(10000f, 10000f, 10000f);

		// Token: 0x04002B0D RID: 11021
		public Vector3 MinState = new Vector3(-10000f, -10000f, -10000f);

		// Token: 0x04002B0E RID: 11022
		protected Transform m_transform;

		// Token: 0x04002B0F RID: 11023
		protected bool m_done;

		// Token: 0x0200058D RID: 1421
		public enum TransformType
		{
			// Token: 0x04002B11 RID: 11025
			Position,
			// Token: 0x04002B12 RID: 11026
			PositionAdditive,
			// Token: 0x04002B13 RID: 11027
			Rotation,
			// Token: 0x04002B14 RID: 11028
			RotationAdditive,
			// Token: 0x04002B15 RID: 11029
			Scale,
			// Token: 0x04002B16 RID: 11030
			ScaleAdditive
		}

		// Token: 0x0200058E RID: 1422
		// (Invoke) Token: 0x060023B1 RID: 9137
		protected delegate void TransformDelegate();
	}
}
