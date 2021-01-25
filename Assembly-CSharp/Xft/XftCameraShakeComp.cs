using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200057F RID: 1407
	[ExecuteInEditMode]
	public class XftCameraShakeComp : MonoBehaviour
	{
		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600233E RID: 9022 RVA: 0x00017553 File Offset: 0x00015753
		// (set) Token: 0x0600233D RID: 9021 RVA: 0x0001754A File Offset: 0x0001574A
		public Spring PositionSpring
		{
			get
			{
				return this.mPositionSpring;
			}
			set
			{
				this.mPositionSpring = value;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06002340 RID: 9024 RVA: 0x00017564 File Offset: 0x00015764
		// (set) Token: 0x0600233F RID: 9023 RVA: 0x0001755B File Offset: 0x0001575B
		public Spring RotationSpring
		{
			get
			{
				return this.mRotationSpring;
			}
			set
			{
				this.mRotationSpring = value;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x0001756C File Offset: 0x0001576C
		public XftEventComponent Client
		{
			get
			{
				return this.m_client;
			}
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x00014DE8 File Offset: 0x00012FE8
		private void Awake()
		{
			base.enabled = false;
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x00114F34 File Offset: 0x00113134
		public void Reset(XftEventComponent client)
		{
			this.m_client = client;
			if (this.m_client.CameraShakeType == XCameraShakeType.Spring)
			{
				if (this.PositionSpring != null && !this.CheckDone())
				{
					base.transform.localPosition = this.mOriPosition;
					base.transform.localRotation = Quaternion.Euler(this.mOriRotation);
				}
				this.PositionSpring = new Spring(client.transform, Spring.TransformType.Position);
				this.PositionSpring.MinVelocity = 1E-05f;
				this.RotationSpring = new Spring(client.transform, Spring.TransformType.Rotation);
				this.RotationSpring.MinVelocity = 1E-05f;
				this.PositionSpring.Stiffness = new Vector3(this.m_client.PositionStifness, this.m_client.PositionStifness, this.m_client.PositionStifness);
				this.PositionSpring.Damping = Vector3.one - new Vector3(this.m_client.PositionDamping, this.m_client.PositionDamping, this.m_client.PositionDamping);
				this.RotationSpring.Stiffness = new Vector3(this.m_client.RotationStiffness, this.m_client.RotationStiffness, this.m_client.RotationStiffness);
				this.RotationSpring.Damping = Vector3.one - new Vector3(this.m_client.RotationDamping, this.m_client.RotationDamping, this.m_client.RotationDamping);
				this.m_client.transform.localPosition = base.transform.localPosition;
				this.m_client.transform.localRotation = base.transform.localRotation;
				this.PositionSpring.RefreshTransformType();
				this.RotationSpring.RefreshTransformType();
				this.m_earthQuakeTimeTemp = this.m_client.EarthQuakeTime;
				this.mLastPosition = base.transform.localPosition;
				this.mLastRotation = base.transform.localRotation.eulerAngles;
				this.mOriPosition = base.transform.localPosition;
				this.mOriRotation = base.transform.localRotation.eulerAngles;
			}
			this.Update();
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x00115168 File Offset: 0x00113368
		private void UpdateEarthQuake()
		{
			if (this.m_client == null || !this.m_client.UseEarthQuake || this.m_earthQuakeTimeTemp <= 0f || !this.EarthQuakeToggled || this.m_client.ElapsedTime > this.m_client.EarthQuakeTime)
			{
				return;
			}
			this.m_earthQuakeTimeTemp -= 0.0166f * (60f * Time.deltaTime);
			float num;
			if (this.m_client.EarthQuakeMagTye == MAGTYPE.Fixed)
			{
				num = this.m_client.EarthQuakeMagnitude;
			}
			else if (this.m_client.EarthQuakeMagTye == MAGTYPE.Curve_OBSOLETE)
			{
				num = this.m_client.EarthQuakeMagCurve.Evaluate(this.m_client.ElapsedTime);
			}
			else
			{
				num = this.m_client.EarthQuakeMagCurveX.Evaluate(this.m_client.ElapsedTime);
			}
			Vector3 force = Vector3.Scale(XftSmoothRandom.GetVector3Centered(1f), new Vector3(num, 0f, num)) * Mathf.Min(this.m_earthQuakeTimeTemp, 1f);
			float num2 = 0f;
			if (Random.value < 0.3f)
			{
				num2 = Random.Range(0f, num * 0.35f) * Mathf.Min(this.m_earthQuakeTimeTemp, 1f);
				if (this.PositionSpring.State.y >= this.PositionSpring.RestState.y)
				{
					num2 = -num2;
				}
			}
			this.PositionSpring.AddForce(force);
			this.RotationSpring.AddForce(new Vector3(0f, 0f, -force.x * 2f) * this.m_client.EarthQuakeCameraRollFactor);
			this.PositionSpring.AddForce(new Vector3(0f, num2, 0f));
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x00115350 File Offset: 0x00113550
		public bool CheckDone()
		{
			if (this.m_client.CameraShakeType == XCameraShakeType.Spring)
			{
				if (this.PositionSpring == null || this.RotationSpring == null)
				{
					return true;
				}
				if (this.PositionSpring.Done && this.RotationSpring.Done)
				{
					return true;
				}
			}
			else if (this.m_client.ElapsedTime > this.m_client.ShakeCurveTime)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x001153CC File Offset: 0x001135CC
		private void UpdateCurve()
		{
			float num = this.m_client.ElapsedTime / this.m_client.ShakeCurveTime;
			Vector3 vector = this.mOriPosition + this.m_client.PositionForce * (this.m_client.PositionCurve.Evaluate(num) * 2f - 1f);
			Vector3 vector2 = this.mOriRotation + this.m_client.RotationForce * (this.m_client.RotationCurve.Evaluate(num) * 2f - 1f);
			Vector3 vector3 = vector - this.mLastPosition;
			Vector3 vector4 = vector2 - this.mLastRotation;
			base.transform.localPosition += vector3;
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			base.transform.localRotation = Quaternion.Euler(eulerAngles + vector4);
			this.mLastPosition = vector;
			this.mLastRotation = vector2;
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x001154D4 File Offset: 0x001136D4
		private void UpdateSpring()
		{
			if (this.PositionSpring == null || this.RotationSpring == null)
			{
				return;
			}
			this.UpdateEarthQuake();
			this.PositionSpring.FixedUpdate();
			this.RotationSpring.FixedUpdate();
			Vector3 vector = this.m_client.transform.localPosition - this.mLastPosition;
			Vector3 vector2 = this.m_client.transform.localEulerAngles - this.mLastRotation;
			base.transform.localPosition += vector;
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			base.transform.localRotation = Quaternion.Euler(eulerAngles + vector2);
			this.mLastPosition = this.m_client.transform.localPosition;
			this.mLastRotation = this.m_client.transform.localEulerAngles;
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x001155BC File Offset: 0x001137BC
		private void Update()
		{
			if (this.m_client == null)
			{
				return;
			}
			if (this.m_client.CameraShakeType == XCameraShakeType.Curve)
			{
				this.UpdateCurve();
			}
			else
			{
				this.UpdateSpring();
			}
			if (this.CheckDone())
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x04002AB7 RID: 10935
		protected Spring mPositionSpring;

		// Token: 0x04002AB8 RID: 10936
		protected Spring mRotationSpring;

		// Token: 0x04002AB9 RID: 10937
		protected XftEventComponent m_client;

		// Token: 0x04002ABA RID: 10938
		public bool EarthQuakeToggled;

		// Token: 0x04002ABB RID: 10939
		protected float m_earthQuakeTimeTemp;

		// Token: 0x04002ABC RID: 10940
		protected Vector3 mLastPosition;

		// Token: 0x04002ABD RID: 10941
		protected Vector3 mLastRotation;

		// Token: 0x04002ABE RID: 10942
		protected Vector3 mOriPosition;

		// Token: 0x04002ABF RID: 10943
		protected Vector3 mOriRotation;
	}
}
