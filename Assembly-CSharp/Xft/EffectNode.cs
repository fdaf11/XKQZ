using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200057B RID: 1403
	public class EffectNode : IComparable<EffectNode>
	{
		// Token: 0x06002317 RID: 8983 RVA: 0x001133D8 File Offset: 0x001115D8
		public EffectNode(int index, Transform clienttrans, bool sync, EffectLayer owner)
		{
			this.Index = index;
			this.ClientTrans = clienttrans;
			this.SyncClient = sync;
			this.Owner = owner;
			this.LowerLeftUV = Vector2.zero;
			this.UVDimensions = Vector2.one;
			this.Scale = Vector2.one;
			this.RotateAngle = 0f;
			this.Color = Color.white;
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x0001740F File Offset: 0x0001560F
		public Camera MyCamera
		{
			get
			{
				if (this.Owner == null)
				{
					Debug.LogError("something wrong with camera init!");
					return null;
				}
				return this.Owner.MyCamera;
			}
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x00017439 File Offset: 0x00015639
		public int CompareTo(EffectNode other)
		{
			return this.TotalIndex.CompareTo(other.TotalIndex);
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x0001744C File Offset: 0x0001564C
		public void SetAffectorList(List<Affector> afts)
		{
			this.AffectorList = afts;
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x00017455 File Offset: 0x00015655
		public List<Affector> GetAffectorList()
		{
			return this.AffectorList;
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x00113458 File Offset: 0x00111658
		public void Init(Vector3 oriDir, float speed, float life, int oriRot, float oriScaleX, float oriScaleY, Color oriColor, Vector2 oriLowerUv, Vector2 oriUVDimension)
		{
			this.OriDirection = oriDir;
			this.LifeTime = life;
			this.OriRotateAngle = oriRot;
			this.OriScaleX = oriScaleX;
			this.OriScaleY = oriScaleY;
			this.StartColor = oriColor;
			this.Color = oriColor;
			this.ElapsedTime = 0f;
			if (this.Owner.DirType != DIRECTION_TYPE.Sphere)
			{
				this.Velocity = this.Owner.ClientTransform.rotation * this.OriDirection * speed;
			}
			else
			{
				this.Velocity = this.OriDirection * speed;
			}
			this.LowerLeftUV = oriLowerUv;
			this.UVDimensions = oriUVDimension;
			this.IsCollisionEventSended = false;
			this.RenderObj.Initialize(this);
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x0001745D File Offset: 0x0001565D
		public float GetElapsedTime()
		{
			return this.ElapsedTime;
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x00017465 File Offset: 0x00015665
		public float GetLifeTime()
		{
			return this.LifeTime;
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x00113518 File Offset: 0x00111718
		public void SetLocalPosition(Vector3 pos)
		{
			if (this.Type == 1)
			{
				RibbonTrail ribbonTrail = this.RenderObj as RibbonTrail;
				if (!this.SyncClient)
				{
					ribbonTrail.OriHeadPos = pos;
				}
				else
				{
					ribbonTrail.OriHeadPos = this.GetRealClientPos() + pos;
				}
			}
			this.Position = pos;
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x0001746D File Offset: 0x0001566D
		public Vector3 GetLocalPosition()
		{
			return this.Position;
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x00113570 File Offset: 0x00111770
		public Vector3 GetRealClientPos()
		{
			Vector3 vector = Vector3.one * this.Owner.Owner.Scale;
			Vector3 zero = Vector3.zero;
			zero.x = this.ClientTrans.position.x / vector.x;
			zero.y = this.ClientTrans.position.y / vector.y;
			zero.z = this.ClientTrans.position.z / vector.z;
			return zero;
		}

		// Token: 0x06002322 RID: 8994 RVA: 0x00113608 File Offset: 0x00111808
		public Vector3 GetOriginalPos()
		{
			Vector3 realClientPos = this.GetRealClientPos();
			Vector3 result;
			if (!this.SyncClient)
			{
				result = this.Position - realClientPos + this.ClientTrans.position;
			}
			else
			{
				result = this.Position + this.ClientTrans.position;
			}
			return result;
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x00017475 File Offset: 0x00015675
		public Vector3 GetWorldPos()
		{
			return this.CurWorldPos;
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x00113664 File Offset: 0x00111864
		protected bool IsSimpleSprite()
		{
			bool result = false;
			if (this.Owner.SpriteType == 2 && this.Owner.OriVelocityAxis == Vector3.zero && !this.Owner.ScaleAffectorEnable && !this.Owner.RotAffectorEnable && (double)this.Owner.OriSpeed < 0.0001 && !this.Owner.GravityAffectorEnable && !this.Owner.AirAffectorEnable && !this.Owner.TurbulenceAffectorEnable && !this.Owner.BombAffectorEnable && !this.Owner.UVRotAffectorEnable && !this.Owner.UVScaleAffectorEnable && (double)Mathf.Abs(this.Owner.OriRotationMax - this.Owner.OriRotationMin) < 0.0001 && (double)Mathf.Abs(this.Owner.OriScaleXMin - this.Owner.OriScaleXMax) < 0.0001 && (double)Mathf.Abs(this.Owner.OriScaleYMin - this.Owner.OriScaleYMax) < 0.0001 && (double)this.Owner.SpeedMin < 0.0001)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x001137D8 File Offset: 0x001119D8
		public void SetRenderType(int type)
		{
			this.Type = type;
			if (type == 0)
			{
				this.RenderObj = this.Owner.GetVertexPool().AddSprite(this.Owner.SpriteWidth, this.Owner.SpriteHeight, (STYPE)this.Owner.SpriteType, (ORIPOINT)this.Owner.OriPoint, 60f, this.IsSimpleSprite());
			}
			else if (type == 1)
			{
				float width = this.Owner.RibbonWidth;
				float len = this.Owner.RibbonLen;
				if (this.Owner.UseRandomRibbon)
				{
					width = Random.Range(this.Owner.RibbonWidthMin, this.Owner.RibbonWidthMax);
					len = Random.Range(this.Owner.RibbonLenMin, this.Owner.RibbonLenMax);
				}
				this.RenderObj = this.Owner.GetVertexPool().AddRibbonTrail(this.Owner.FaceToObject, this.Owner.FaceObject, width, this.Owner.MaxRibbonElements, len, this.Owner.ClientTransform.position + this.Owner.EmitPoint, 60f);
			}
			else if (type == 2)
			{
				this.RenderObj = this.Owner.GetVertexPool().AddCone(this.Owner.ConeSize, this.Owner.ConeSegment, this.Owner.ConeAngle, this.Owner.OriVelocityAxis, 0, 60f, this.Owner.UseConeAngleChange, this.Owner.ConeDeltaAngle);
			}
			else if (type == 3)
			{
				if (this.Owner.CMesh == null)
				{
					Debug.LogError("custom mesh layer has no mesh to display!", this.Owner.gameObject);
				}
				Vector3 dir = Vector3.zero;
				if (this.Owner.OriVelocityAxis == Vector3.zero)
				{
					this.Owner.OriVelocityAxis = Vector3.up;
				}
				dir = this.Owner.OriVelocityAxis;
				this.RenderObj = this.Owner.GetVertexPool().AddCustomMesh(this.Owner.CMesh, dir, 60f);
			}
			else if (type == 4)
			{
				this.RenderObj = this.Owner.GetVertexPool().AddRope();
			}
			else if (type == 5)
			{
				this.RenderObj = this.Owner.GetVertexPool().AddSphericalBillboard();
			}
			this.RenderObj.Node = this;
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x00113A60 File Offset: 0x00111C60
		public void Reset()
		{
			if (this.Owner.UseSubEmitters && !string.IsNullOrEmpty(this.Owner.DeathSubEmitter))
			{
				XffectComponent effect = this.Owner.SpawnCache.GetEffect(this.Owner.DeathSubEmitter);
				if (effect == null)
				{
					return;
				}
				effect.transform.position = this.CurWorldPos;
				effect.Active();
			}
			this.Position = this.Owner.ClientTransform.position;
			this.Velocity = Vector3.zero;
			this.ElapsedTime = 0f;
			this.CurWorldPos = this.Owner.transform.position;
			this.LastWorldPos = this.CurWorldPos;
			this.IsCollisionEventSended = false;
			if (this.Owner.IsRandomStartColor)
			{
				this.StartColor = this.Owner.RandomColorGradient.Evaluate(Random.Range(0f, 1f));
			}
			for (int i = 0; i < this.AffectorList.Count; i++)
			{
				Affector affector = this.AffectorList[i];
				affector.Reset();
			}
			this.Scale = Vector3.one;
			this.mIsFade = false;
			this.RenderObj.Reset();
			if (this.Owner.UseSubEmitters && this.SubEmitter != null && XffectComponent.IsActive(this.SubEmitter.gameObject) && this.Owner.SubEmitterAutoStop)
			{
				this.SubEmitter.StopEmit();
			}
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x0001747D File Offset: 0x0001567D
		public void Remove()
		{
			this.Owner.RemoveActiveNode(this);
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0001748B File Offset: 0x0001568B
		public void Stop()
		{
			this.Reset();
			this.Remove();
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x00113C00 File Offset: 0x00111E00
		public void Fade(float time)
		{
			ColorAffector colorAffector = null;
			for (int i = 0; i < this.AffectorList.Count; i++)
			{
				if (this.AffectorList[i].Type == AFFECTORTYPE.ColorAffector)
				{
					colorAffector = (ColorAffector)this.AffectorList[i];
					break;
				}
			}
			if (colorAffector == null)
			{
				colorAffector = new ColorAffector(this.Owner, this);
				this.AffectorList.Add(colorAffector);
			}
			this.mIsFade = true;
			colorAffector.Fade(time);
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x00113C88 File Offset: 0x00111E88
		public void CollisionDetection()
		{
			if (!this.Owner.UseCollisionDetection || this.IsCollisionEventSended)
			{
				return;
			}
			bool flag = false;
			GameObject obj = null;
			if (this.Owner.CollisionType == COLLITION_TYPE.Sphere && this.Owner.CollisionGoal != null)
			{
				Vector3 lastCollisionDetectDir = this.CurWorldPos - this.Owner.CollisionGoal.position;
				float num = this.Owner.ColliisionPosRange + this.Owner.ParticleRadius;
				if (lastCollisionDetectDir.sqrMagnitude <= num * num)
				{
					flag = true;
					obj = this.Owner.CollisionGoal.gameObject;
				}
				this.LastCollisionDetectDir = lastCollisionDetectDir;
			}
			else if (this.Owner.CollisionType == COLLITION_TYPE.CollisionLayer)
			{
				int num2 = 1 << this.Owner.CollisionLayer;
				Vector3 originalPos = this.GetOriginalPos();
				RaycastHit raycastHit;
				if (Physics.SphereCast(originalPos, this.Owner.ParticleRadius, this.Velocity.normalized, ref raycastHit, this.Owner.ParticleRadius, num2))
				{
					flag = true;
					obj = raycastHit.collider.gameObject;
				}
			}
			else if (this.Owner.CollisionType == COLLITION_TYPE.Plane)
			{
				if (!this.Owner.CollisionPlane.GetSide(this.CurWorldPos - this.Owner.PlaneDir.normalized * this.Owner.ParticleRadius))
				{
					flag = true;
					obj = this.Owner.gameObject;
				}
			}
			if (flag)
			{
				if (this.Owner.EventHandleFunctionName != string.Empty && this.Owner.EventReceiver != null)
				{
					this.Owner.EventReceiver.SendMessage(this.Owner.EventHandleFunctionName, new CollisionParam(obj, this.GetOriginalPos(), this.Velocity.normalized));
				}
				this.IsCollisionEventSended = true;
				if (this.Owner.CollisionAutoDestroy)
				{
					this.ElapsedTime = float.PositiveInfinity;
				}
				if (this.Owner.UseSubEmitters && !string.IsNullOrEmpty(this.Owner.CollisionSubEmitter))
				{
					XffectComponent effect = this.Owner.SpawnCache.GetEffect(this.Owner.CollisionSubEmitter);
					if (effect == null)
					{
						return;
					}
					effect.transform.position = this.CurWorldPos;
					effect.Active();
				}
			}
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x00113F10 File Offset: 0x00112110
		public void Update(float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			for (int i = 0; i < this.AffectorList.Count; i++)
			{
				Affector affector = this.AffectorList[i];
				affector.Update(deltaTime);
			}
			this.Position += this.Velocity * deltaTime;
			if (this.SyncClient)
			{
				this.CurWorldPos = this.Position + this.GetRealClientPos();
			}
			else
			{
				this.CurWorldPos = this.Position;
			}
			this.CollisionDetection();
			if (this.Owner.UseSubEmitters && this.SubEmitter != null && XffectComponent.IsActive(this.SubEmitter.gameObject))
			{
				this.SubEmitter.transform.position = this.CurWorldPos;
			}
			this.RenderObj.Update(deltaTime);
			if (this.Owner.UseShaderCurve2 || this.Owner.UseShaderCurve1)
			{
				float x = (!this.Owner.UseShaderCurve1) ? 1f : this.Owner.ShaderCurveX1.Evaluate(this.GetElapsedTime(), this);
				float y = (!this.Owner.UseShaderCurve2) ? 0f : this.Owner.ShaderCurveX2.Evaluate(this.GetElapsedTime(), this);
				this.RenderObj.ApplyShaderParam(x, y);
			}
			this.LastWorldPos = this.CurWorldPos;
			if (this.ElapsedTime > this.LifeTime && this.LifeTime > 0f)
			{
				this.Reset();
				this.Remove();
			}
		}

		// Token: 0x04002A8A RID: 10890
		public RenderObject RenderObj;

		// Token: 0x04002A8B RID: 10891
		protected int Type;

		// Token: 0x04002A8C RID: 10892
		public int Index;

		// Token: 0x04002A8D RID: 10893
		public ulong TotalIndex;

		// Token: 0x04002A8E RID: 10894
		public Transform ClientTrans;

		// Token: 0x04002A8F RID: 10895
		public bool SyncClient;

		// Token: 0x04002A90 RID: 10896
		public EffectLayer Owner;

		// Token: 0x04002A91 RID: 10897
		protected Vector3 CurDirection;

		// Token: 0x04002A92 RID: 10898
		protected Vector3 LastWorldPos = Vector3.zero;

		// Token: 0x04002A93 RID: 10899
		public Vector3 CurWorldPos;

		// Token: 0x04002A94 RID: 10900
		protected float ElapsedTime;

		// Token: 0x04002A95 RID: 10901
		public Vector3 Position;

		// Token: 0x04002A96 RID: 10902
		public Vector2 LowerLeftUV;

		// Token: 0x04002A97 RID: 10903
		public Vector2 UVDimensions;

		// Token: 0x04002A98 RID: 10904
		public Vector3 Velocity;

		// Token: 0x04002A99 RID: 10905
		public Vector2 Scale;

		// Token: 0x04002A9A RID: 10906
		public float RotateAngle;

		// Token: 0x04002A9B RID: 10907
		public Color Color;

		// Token: 0x04002A9C RID: 10908
		public XffectComponent SubEmitter;

		// Token: 0x04002A9D RID: 10909
		public List<Affector> AffectorList;

		// Token: 0x04002A9E RID: 10910
		public Vector3 OriDirection;

		// Token: 0x04002A9F RID: 10911
		public float LifeTime;

		// Token: 0x04002AA0 RID: 10912
		public int OriRotateAngle;

		// Token: 0x04002AA1 RID: 10913
		public float OriScaleX;

		// Token: 0x04002AA2 RID: 10914
		public float OriScaleY;

		// Token: 0x04002AA3 RID: 10915
		public bool SimpleSprite;

		// Token: 0x04002AA4 RID: 10916
		public Color StartColor;

		// Token: 0x04002AA5 RID: 10917
		protected bool IsCollisionEventSended;

		// Token: 0x04002AA6 RID: 10918
		protected Vector3 LastCollisionDetectDir = Vector3.zero;

		// Token: 0x04002AA7 RID: 10919
		public bool mIsFade;
	}
}
