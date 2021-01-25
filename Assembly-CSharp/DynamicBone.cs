using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000667 RID: 1639
[AddComponentMenu("Dynamic Bone/Dynamic Bone")]
public class DynamicBone : MonoBehaviour
{
	// Token: 0x06002828 RID: 10280 RVA: 0x0001A768 File Offset: 0x00018968
	private void Start()
	{
		this.SetupParticles();
	}

	// Token: 0x06002829 RID: 10281 RVA: 0x0001A770 File Offset: 0x00018970
	private void Update()
	{
		if (this.m_Weight > 0f && (!this.m_DistantDisable || !this.m_DistantDisabled))
		{
			this.InitTransforms();
		}
	}

	// Token: 0x0600282A RID: 10282 RVA: 0x0013D690 File Offset: 0x0013B890
	private void LateUpdate()
	{
		if (this.m_DistantDisable)
		{
			this.CheckDistance();
		}
		if (this.m_Weight > 0f && (!this.m_DistantDisable || !this.m_DistantDisabled))
		{
			this.UpdateDynamicBones(Time.deltaTime);
		}
	}

	// Token: 0x0600282B RID: 10283 RVA: 0x0013D6E0 File Offset: 0x0013B8E0
	private void CheckDistance()
	{
		Transform transform = this.m_ReferenceObject;
		if (transform == null && Camera.main != null)
		{
			transform = Camera.main.transform;
		}
		if (transform != null)
		{
			float sqrMagnitude = (transform.position - base.transform.position).sqrMagnitude;
			bool flag = sqrMagnitude > this.m_DistanceToObject * this.m_DistanceToObject;
			if (flag != this.m_DistantDisabled)
			{
				if (!flag)
				{
					this.ResetParticlesPosition();
				}
				this.m_DistantDisabled = flag;
			}
		}
	}

	// Token: 0x0600282C RID: 10284 RVA: 0x0001A79E File Offset: 0x0001899E
	private void OnEnable()
	{
		this.ResetParticlesPosition();
	}

	// Token: 0x0600282D RID: 10285 RVA: 0x0001A7A6 File Offset: 0x000189A6
	private void OnDisable()
	{
		this.InitTransforms();
	}

	// Token: 0x0600282E RID: 10286 RVA: 0x0013D778 File Offset: 0x0013B978
	private void OnValidate()
	{
		this.m_UpdateRate = Mathf.Max(this.m_UpdateRate, 0f);
		this.m_Damping = Mathf.Clamp01(this.m_Damping);
		this.m_Elasticity = Mathf.Clamp01(this.m_Elasticity);
		this.m_Stiffness = Mathf.Clamp01(this.m_Stiffness);
		this.m_Inert = Mathf.Clamp01(this.m_Inert);
		this.m_Radius = Mathf.Max(this.m_Radius, 0f);
		if (Application.isEditor && Application.isPlaying)
		{
			this.InitTransforms();
			this.SetupParticles();
		}
	}

	// Token: 0x0600282F RID: 10287 RVA: 0x0013D818 File Offset: 0x0013BA18
	private void OnDrawGizmosSelected()
	{
		if (!base.enabled || this.m_Root == null)
		{
			return;
		}
		if (Application.isEditor && !Application.isPlaying && base.transform.hasChanged)
		{
			this.InitTransforms();
			this.SetupParticles();
		}
		Gizmos.color = Color.white;
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_ParentIndex >= 0)
			{
				DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
				Gizmos.DrawLine(particle.m_Position, particle2.m_Position);
			}
			if (particle.m_Radius > 0f)
			{
				Gizmos.DrawWireSphere(particle.m_Position, particle.m_Radius * this.m_ObjectScale);
			}
		}
	}

	// Token: 0x06002830 RID: 10288 RVA: 0x0013D8FC File Offset: 0x0013BAFC
	public void SetWeight(float w)
	{
		if (this.m_Weight != w)
		{
			if (w == 0f)
			{
				this.InitTransforms();
			}
			else if (this.m_Weight == 0f)
			{
				this.ResetParticlesPosition();
			}
			this.m_Weight = w;
		}
	}

	// Token: 0x06002831 RID: 10289 RVA: 0x0001A7AE File Offset: 0x000189AE
	public float GetWeight()
	{
		return this.m_Weight;
	}

	// Token: 0x06002832 RID: 10290 RVA: 0x0013D948 File Offset: 0x0013BB48
	private void UpdateDynamicBones(float t)
	{
		if (this.m_Root == null)
		{
			return;
		}
		this.m_ObjectScale = Mathf.Abs(base.transform.lossyScale.x);
		this.m_ObjectMove = base.transform.position - this.m_ObjectPrevPosition;
		this.m_ObjectPrevPosition = base.transform.position;
		int num = 1;
		if (this.m_UpdateRate > 0f)
		{
			float num2 = 1f / this.m_UpdateRate;
			this.m_Time += t;
			num = 0;
			while (this.m_Time >= num2)
			{
				this.m_Time -= num2;
				if (++num >= 3)
				{
					this.m_Time = 0f;
					break;
				}
			}
		}
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				this.UpdateParticles1();
				this.UpdateParticles2();
				this.m_ObjectMove = Vector3.zero;
			}
		}
		else
		{
			this.SkipUpdateParticles();
		}
		this.ApplyParticlesToTransforms();
	}

	// Token: 0x06002833 RID: 10291 RVA: 0x0013DA5C File Offset: 0x0013BC5C
	private void SetupParticles()
	{
		this.m_Particles.Clear();
		if (this.m_Root == null)
		{
			return;
		}
		this.m_LocalGravity = this.m_Root.InverseTransformDirection(this.m_Gravity);
		this.m_ObjectScale = base.transform.lossyScale.x;
		this.m_ObjectPrevPosition = base.transform.position;
		this.m_ObjectMove = Vector3.zero;
		this.m_BoneTotalLength = 0f;
		this.AppendParticles(this.m_Root, -1, 0f);
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			particle.m_Damping = this.m_Damping;
			particle.m_Elasticity = this.m_Elasticity;
			particle.m_Stiffness = this.m_Stiffness;
			particle.m_Inert = this.m_Inert;
			particle.m_Radius = this.m_Radius;
			if (this.m_BoneTotalLength > 0f)
			{
				float num = particle.m_BoneLength / this.m_BoneTotalLength;
				if (this.m_DampingDistrib != null && this.m_DampingDistrib.keys.Length > 0)
				{
					particle.m_Damping *= this.m_DampingDistrib.Evaluate(num);
				}
				if (this.m_ElasticityDistrib != null && this.m_ElasticityDistrib.keys.Length > 0)
				{
					particle.m_Elasticity *= this.m_ElasticityDistrib.Evaluate(num);
				}
				if (this.m_StiffnessDistrib != null && this.m_StiffnessDistrib.keys.Length > 0)
				{
					particle.m_Stiffness *= this.m_StiffnessDistrib.Evaluate(num);
				}
				if (this.m_InertDistrib != null && this.m_InertDistrib.keys.Length > 0)
				{
					particle.m_Inert *= this.m_InertDistrib.Evaluate(num);
				}
				if (this.m_RadiusDistrib != null && this.m_RadiusDistrib.keys.Length > 0)
				{
					particle.m_Radius *= this.m_RadiusDistrib.Evaluate(num);
				}
			}
			particle.m_Damping = Mathf.Clamp01(particle.m_Damping);
			particle.m_Elasticity = Mathf.Clamp01(particle.m_Elasticity);
			particle.m_Stiffness = Mathf.Clamp01(particle.m_Stiffness);
			particle.m_Inert = Mathf.Clamp01(particle.m_Inert);
			particle.m_Radius = Mathf.Max(particle.m_Radius, 0f);
		}
	}

	// Token: 0x06002834 RID: 10292 RVA: 0x0013DCE0 File Offset: 0x0013BEE0
	private void AppendParticles(Transform b, int parentIndex, float boneLength)
	{
		DynamicBone.Particle particle = new DynamicBone.Particle();
		particle.m_Transform = b;
		particle.m_ParentIndex = parentIndex;
		if (b != null)
		{
			particle.m_Position = (particle.m_PrevPosition = b.position);
			particle.m_InitLocalPosition = b.localPosition;
			particle.m_InitLocalRotation = b.localRotation;
		}
		else
		{
			Transform transform = this.m_Particles[parentIndex].m_Transform;
			if (this.m_EndLength > 0f)
			{
				Transform parent = transform.parent;
				if (parent != null)
				{
					particle.m_EndOffset = transform.InverseTransformPoint(transform.position * 2f - parent.position) * this.m_EndLength;
				}
				else
				{
					particle.m_EndOffset = new Vector3(this.m_EndLength, 0f, 0f);
				}
			}
			else
			{
				particle.m_EndOffset = transform.InverseTransformPoint(base.transform.TransformDirection(this.m_EndOffset) + transform.position);
			}
			particle.m_Position = (particle.m_PrevPosition = transform.TransformPoint(particle.m_EndOffset));
		}
		if (parentIndex >= 0)
		{
			boneLength += (this.m_Particles[parentIndex].m_Transform.position - particle.m_Position).magnitude;
			particle.m_BoneLength = boneLength;
			this.m_BoneTotalLength = Mathf.Max(this.m_BoneTotalLength, boneLength);
		}
		int count = this.m_Particles.Count;
		this.m_Particles.Add(particle);
		if (b != null)
		{
			for (int i = 0; i < b.childCount; i++)
			{
				bool flag = false;
				if (this.m_Exclusions != null)
				{
					for (int j = 0; j < this.m_Exclusions.Count; j++)
					{
						Transform transform2 = this.m_Exclusions[j];
						if (transform2 == b.GetChild(i))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					this.AppendParticles(b.GetChild(i), count, boneLength);
				}
			}
			if (b.childCount == 0 && (this.m_EndLength > 0f || this.m_EndOffset != Vector3.zero))
			{
				this.AppendParticles(null, count, boneLength);
			}
		}
	}

	// Token: 0x06002835 RID: 10293 RVA: 0x0013DF48 File Offset: 0x0013C148
	private void InitTransforms()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_Transform != null)
			{
				particle.m_Transform.localPosition = particle.m_InitLocalPosition;
				particle.m_Transform.localRotation = particle.m_InitLocalRotation;
			}
		}
	}

	// Token: 0x06002836 RID: 10294 RVA: 0x0013DFB4 File Offset: 0x0013C1B4
	private void ResetParticlesPosition()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_Transform != null)
			{
				particle.m_Position = (particle.m_PrevPosition = particle.m_Transform.position);
			}
			else
			{
				Transform transform = this.m_Particles[particle.m_ParentIndex].m_Transform;
				particle.m_Position = (particle.m_PrevPosition = transform.TransformPoint(particle.m_EndOffset));
			}
		}
		this.m_ObjectPrevPosition = base.transform.position;
	}

	// Token: 0x06002837 RID: 10295 RVA: 0x0013E060 File Offset: 0x0013C260
	private void UpdateParticles1()
	{
		Vector3 vector = this.m_Gravity;
		Vector3 normalized = this.m_Gravity.normalized;
		Vector3 vector2 = this.m_Root.TransformDirection(this.m_LocalGravity);
		Vector3 vector3 = normalized * Mathf.Max(Vector3.Dot(vector2, normalized), 0f);
		vector -= vector3;
		vector = (vector + this.m_Force) * this.m_ObjectScale;
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_ParentIndex >= 0)
			{
				Vector3 vector4 = particle.m_Position - particle.m_PrevPosition;
				Vector3 vector5 = this.m_ObjectMove * particle.m_Inert;
				particle.m_PrevPosition = particle.m_Position + vector5;
				particle.m_Position += vector4 * (1f - particle.m_Damping) + vector + vector5;
			}
			else
			{
				particle.m_PrevPosition = particle.m_Position;
				particle.m_Position = particle.m_Transform.position;
			}
		}
	}

	// Token: 0x06002838 RID: 10296 RVA: 0x0013E1A0 File Offset: 0x0013C3A0
	private void UpdateParticles2()
	{
		Plane plane = default(Plane);
		for (int i = 1; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
			float magnitude;
			if (particle.m_Transform != null)
			{
				magnitude = (particle2.m_Transform.position - particle.m_Transform.position).magnitude;
			}
			else
			{
				magnitude = particle2.m_Transform.localToWorldMatrix.MultiplyVector(particle.m_EndOffset).magnitude;
			}
			float num = Mathf.Lerp(1f, particle.m_Stiffness, this.m_Weight);
			if (num > 0f || particle.m_Elasticity > 0f)
			{
				Matrix4x4 localToWorldMatrix = particle2.m_Transform.localToWorldMatrix;
				localToWorldMatrix.SetColumn(3, particle2.m_Position);
				Vector3 vector;
				if (particle.m_Transform != null)
				{
					vector = localToWorldMatrix.MultiplyPoint3x4(particle.m_Transform.localPosition);
				}
				else
				{
					vector = localToWorldMatrix.MultiplyPoint3x4(particle.m_EndOffset);
				}
				Vector3 vector2 = vector - particle.m_Position;
				particle.m_Position += vector2 * particle.m_Elasticity;
				if (num > 0f)
				{
					vector2 = vector - particle.m_Position;
					float magnitude2 = vector2.magnitude;
					float num2 = magnitude * (1f - num) * 2f;
					if (magnitude2 > num2)
					{
						particle.m_Position += vector2 * ((magnitude2 - num2) / magnitude2);
					}
				}
			}
			if (this.m_Colliders != null)
			{
				float particleRadius = particle.m_Radius * this.m_ObjectScale;
				for (int j = 0; j < this.m_Colliders.Count; j++)
				{
					DynamicBoneCollider dynamicBoneCollider = this.m_Colliders[j];
					if (dynamicBoneCollider != null && dynamicBoneCollider.enabled)
					{
						dynamicBoneCollider.Collide(ref particle.m_Position, particleRadius);
					}
				}
			}
			if (this.m_FreezeAxis != DynamicBone.FreezeAxis.None)
			{
				switch (this.m_FreezeAxis)
				{
				case DynamicBone.FreezeAxis.X:
					plane.SetNormalAndPosition(particle2.m_Transform.right, particle2.m_Position);
					break;
				case DynamicBone.FreezeAxis.Y:
					plane.SetNormalAndPosition(particle2.m_Transform.up, particle2.m_Position);
					break;
				case DynamicBone.FreezeAxis.Z:
					plane.SetNormalAndPosition(particle2.m_Transform.forward, particle2.m_Position);
					break;
				}
				particle.m_Position -= plane.normal * plane.GetDistanceToPoint(particle.m_Position);
			}
			Vector3 vector3 = particle2.m_Position - particle.m_Position;
			float magnitude3 = vector3.magnitude;
			if (magnitude3 > 0f)
			{
				particle.m_Position += vector3 * ((magnitude3 - magnitude) / magnitude3);
			}
		}
	}

	// Token: 0x06002839 RID: 10297 RVA: 0x0013E4D4 File Offset: 0x0013C6D4
	private void SkipUpdateParticles()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_ParentIndex >= 0)
			{
				particle.m_PrevPosition += this.m_ObjectMove;
				particle.m_Position += this.m_ObjectMove;
				DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
				float magnitude;
				if (particle.m_Transform != null)
				{
					magnitude = (particle2.m_Transform.position - particle.m_Transform.position).magnitude;
				}
				else
				{
					magnitude = particle2.m_Transform.localToWorldMatrix.MultiplyVector(particle.m_EndOffset).magnitude;
				}
				float num = Mathf.Lerp(1f, particle.m_Stiffness, this.m_Weight);
				if (num > 0f)
				{
					Matrix4x4 localToWorldMatrix = particle2.m_Transform.localToWorldMatrix;
					localToWorldMatrix.SetColumn(3, particle2.m_Position);
					Vector3 vector;
					if (particle.m_Transform != null)
					{
						vector = localToWorldMatrix.MultiplyPoint3x4(particle.m_Transform.localPosition);
					}
					else
					{
						vector = localToWorldMatrix.MultiplyPoint3x4(particle.m_EndOffset);
					}
					Vector3 vector2 = vector - particle.m_Position;
					float magnitude2 = vector2.magnitude;
					float num2 = magnitude * (1f - num) * 2f;
					if (magnitude2 > num2)
					{
						particle.m_Position += vector2 * ((magnitude2 - num2) / magnitude2);
					}
				}
				Vector3 vector3 = particle2.m_Position - particle.m_Position;
				float magnitude3 = vector3.magnitude;
				if (magnitude3 > 0f)
				{
					particle.m_Position += vector3 * ((magnitude3 - magnitude) / magnitude3);
				}
			}
			else
			{
				particle.m_PrevPosition = particle.m_Position;
				particle.m_Position = particle.m_Transform.position;
			}
		}
	}

	// Token: 0x0600283A RID: 10298 RVA: 0x0001A7B6 File Offset: 0x000189B6
	private Vector3 MirrorVector(Vector3 v, Vector3 axis)
	{
		return v - axis * (Vector3.Dot(v, axis) * 2f);
	}

	// Token: 0x0600283B RID: 10299 RVA: 0x0013E6F0 File Offset: 0x0013C8F0
	private void ApplyParticlesToTransforms()
	{
		Vector3 right = Vector3.right;
		Vector3 up = Vector3.up;
		Vector3 forward = Vector3.forward;
		Vector3 localScale = base.transform.localScale;
		bool flag = localScale.x < 0f;
		if (flag)
		{
			right = base.transform.right;
		}
		bool flag2 = localScale.y < 0f;
		if (flag2)
		{
			up = base.transform.up;
		}
		bool flag3 = localScale.z < 0f;
		if (flag3)
		{
			forward = base.transform.forward;
		}
		Transform parent = base.transform.parent;
		if (parent != null)
		{
			localScale = parent.localScale;
			if (!flag && localScale.x < 0f)
			{
				flag = true;
				right = parent.right;
			}
			if (!flag2 && localScale.y < 0f)
			{
				flag2 = true;
				up = parent.up;
			}
			if (!flag3 && localScale.z < 0f)
			{
				flag3 = true;
				forward = parent.forward;
			}
		}
		for (int i = 1; i < this.m_Particles.Count; i++)
		{
			DynamicBone.Particle particle = this.m_Particles[i];
			DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
			if (particle2.m_Transform.childCount <= 1)
			{
				Vector3 vector;
				if (particle.m_Transform != null)
				{
					vector = particle.m_Transform.localPosition;
				}
				else
				{
					vector = particle.m_EndOffset;
				}
				Vector3 vector2 = particle.m_Position - particle2.m_Position;
				if (flag)
				{
					vector2 = this.MirrorVector(vector2, right);
				}
				if (flag2)
				{
					vector2 = this.MirrorVector(vector2, up);
				}
				if (flag3)
				{
					vector2 = this.MirrorVector(vector2, forward);
				}
				Quaternion quaternion = Quaternion.FromToRotation(particle2.m_Transform.TransformDirection(vector), vector2);
				particle2.m_Transform.rotation = quaternion * particle2.m_Transform.rotation;
			}
			if (particle.m_Transform != null)
			{
				particle.m_Transform.position = particle.m_Position;
			}
		}
	}

	// Token: 0x0400324C RID: 12876
	public Transform m_Root;

	// Token: 0x0400324D RID: 12877
	public float m_UpdateRate = 60f;

	// Token: 0x0400324E RID: 12878
	[Range(0f, 1f)]
	public float m_Damping = 0.1f;

	// Token: 0x0400324F RID: 12879
	public AnimationCurve m_DampingDistrib;

	// Token: 0x04003250 RID: 12880
	[Range(0f, 1f)]
	public float m_Elasticity = 0.1f;

	// Token: 0x04003251 RID: 12881
	public AnimationCurve m_ElasticityDistrib;

	// Token: 0x04003252 RID: 12882
	[Range(0f, 1f)]
	public float m_Stiffness = 0.1f;

	// Token: 0x04003253 RID: 12883
	public AnimationCurve m_StiffnessDistrib;

	// Token: 0x04003254 RID: 12884
	[Range(0f, 1f)]
	public float m_Inert;

	// Token: 0x04003255 RID: 12885
	public AnimationCurve m_InertDistrib;

	// Token: 0x04003256 RID: 12886
	public float m_Radius;

	// Token: 0x04003257 RID: 12887
	public AnimationCurve m_RadiusDistrib;

	// Token: 0x04003258 RID: 12888
	public float m_EndLength;

	// Token: 0x04003259 RID: 12889
	public Vector3 m_EndOffset = Vector3.zero;

	// Token: 0x0400325A RID: 12890
	public Vector3 m_Gravity = Vector3.zero;

	// Token: 0x0400325B RID: 12891
	public Vector3 m_Force = Vector3.zero;

	// Token: 0x0400325C RID: 12892
	public List<DynamicBoneCollider> m_Colliders;

	// Token: 0x0400325D RID: 12893
	public List<Transform> m_Exclusions;

	// Token: 0x0400325E RID: 12894
	public DynamicBone.FreezeAxis m_FreezeAxis;

	// Token: 0x0400325F RID: 12895
	public bool m_DistantDisable;

	// Token: 0x04003260 RID: 12896
	public Transform m_ReferenceObject;

	// Token: 0x04003261 RID: 12897
	public float m_DistanceToObject = 20f;

	// Token: 0x04003262 RID: 12898
	private Vector3 m_LocalGravity = Vector3.zero;

	// Token: 0x04003263 RID: 12899
	private Vector3 m_ObjectMove = Vector3.zero;

	// Token: 0x04003264 RID: 12900
	private Vector3 m_ObjectPrevPosition = Vector3.zero;

	// Token: 0x04003265 RID: 12901
	private float m_BoneTotalLength;

	// Token: 0x04003266 RID: 12902
	private float m_ObjectScale = 1f;

	// Token: 0x04003267 RID: 12903
	private float m_Time;

	// Token: 0x04003268 RID: 12904
	private float m_Weight = 1f;

	// Token: 0x04003269 RID: 12905
	private bool m_DistantDisabled;

	// Token: 0x0400326A RID: 12906
	private List<DynamicBone.Particle> m_Particles = new List<DynamicBone.Particle>();

	// Token: 0x02000668 RID: 1640
	public enum FreezeAxis
	{
		// Token: 0x0400326C RID: 12908
		None,
		// Token: 0x0400326D RID: 12909
		X,
		// Token: 0x0400326E RID: 12910
		Y,
		// Token: 0x0400326F RID: 12911
		Z
	}

	// Token: 0x02000669 RID: 1641
	private class Particle
	{
		// Token: 0x04003270 RID: 12912
		public Transform m_Transform;

		// Token: 0x04003271 RID: 12913
		public int m_ParentIndex = -1;

		// Token: 0x04003272 RID: 12914
		public float m_Damping;

		// Token: 0x04003273 RID: 12915
		public float m_Elasticity;

		// Token: 0x04003274 RID: 12916
		public float m_Stiffness;

		// Token: 0x04003275 RID: 12917
		public float m_Inert;

		// Token: 0x04003276 RID: 12918
		public float m_Radius;

		// Token: 0x04003277 RID: 12919
		public float m_BoneLength;

		// Token: 0x04003278 RID: 12920
		public Vector3 m_Position = Vector3.zero;

		// Token: 0x04003279 RID: 12921
		public Vector3 m_PrevPosition = Vector3.zero;

		// Token: 0x0400327A RID: 12922
		public Vector3 m_EndOffset = Vector3.zero;

		// Token: 0x0400327B RID: 12923
		public Vector3 m_InitLocalPosition = Vector3.zero;

		// Token: 0x0400327C RID: 12924
		public Quaternion m_InitLocalRotation = Quaternion.identity;
	}
}
