using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000574 RID: 1396
	public class EffectLayer : MonoBehaviour
	{
		// Token: 0x17000363 RID: 867
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x00017173 File Offset: 0x00015373
		public Plane CollisionPlane
		{
			get
			{
				return this.mCollisionPlane;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x0600229D RID: 8861 RVA: 0x0001717B File Offset: 0x0001537B
		public Camera MyCamera
		{
			get
			{
				return this.Owner.MyCamera;
			}
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x0010F838 File Offset: 0x0010DA38
		protected void InitCollision()
		{
			if (!this.UseCollisionDetection)
			{
				return;
			}
			this.mCollisionPlane = new Plane(this.PlaneDir.normalized, base.transform.position + this.PlaneOffset);
			if (this.CollisionType == COLLITION_TYPE.CollisionLayer || this.CollisionType == COLLITION_TYPE.Plane)
			{
				return;
			}
			if (this.CollisionGoal == null)
			{
			}
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x0010F8A8 File Offset: 0x0010DAA8
		public List<Affector> InitAffectors(EffectNode node)
		{
			List<Affector> list = new List<Affector>();
			if (this.UVAffectorEnable)
			{
				UVAnimation uvanimation = new UVAnimation();
				if (this.UVType == 1)
				{
					float num = this.OriUVDimensions.x / (float)this.Cols;
					float num2 = Mathf.Abs(this.OriUVDimensions.y / (float)this.Rows);
					Vector2 cellSize;
					cellSize..ctor(num, num2);
					uvanimation.BuildUVAnim(this.OriTopLeftUV, cellSize, this.Cols, this.Rows, this.Cols * this.Rows);
				}
				this.UVDimension = uvanimation.UVDimensions[0];
				this.UVTopLeft = uvanimation.frames[0];
				if (uvanimation.frames.Length != 1)
				{
					uvanimation.loopCycles = this.LoopCircles;
					Affector affector = new UVAffector(uvanimation, this.UVTime, node, this.RandomStartFrame);
					list.Add(affector);
				}
			}
			else
			{
				this.UVDimension = this.OriUVDimensions;
				this.UVTopLeft = this.OriTopLeftUV;
			}
			if (this.RotAffectorEnable && this.RotateType != RSTYPE.NONE)
			{
				Affector affector2;
				if (this.RotateType == RSTYPE.NONE)
				{
					affector2 = new RotateAffector(this.DeltaRot, node);
				}
				else
				{
					affector2 = new RotateAffector(this.RotateType, node);
				}
				list.Add(affector2);
			}
			if (this.ScaleAffectorEnable && this.ScaleType != RSTYPE.NONE)
			{
				Affector affector3;
				if (this.ScaleType == RSTYPE.NONE)
				{
					affector3 = new ScaleAffector(this.DeltaScaleX, this.DeltaScaleY, node);
				}
				else
				{
					affector3 = new ScaleAffector(this.ScaleType, node);
				}
				list.Add(affector3);
			}
			if (this.ColorAffectorEnable)
			{
				ColorAffector colorAffector = new ColorAffector(this, node);
				list.Add(colorAffector);
			}
			if (this.JetAffectorEnable)
			{
				Affector affector4 = new JetAffector(this.JetMag, this.JetMagType, this.JetCurve, node);
				list.Add(affector4);
			}
			if (this.VortexAffectorEnable)
			{
				Affector affector5 = new VortexAffector(this.VortexObj, this.VortexDirection, this.VortexInheritRotation, node);
				list.Add(affector5);
			}
			if (this.UVRotAffectorEnable)
			{
				float rotXSpeed = this.UVRotXSpeed;
				float rotYSpeed = this.UVRotYSpeed;
				if (this.RandomUVRotateSpeed)
				{
					rotXSpeed = Random.Range(this.UVRotXSpeed, this.UVRotXSpeedMax);
					rotYSpeed = Random.Range(this.UVRotYSpeed, this.UVRotYSpeedMax);
				}
				Affector affector6 = new UVRotAffector(rotXSpeed, rotYSpeed, node);
				list.Add(affector6);
			}
			if (this.UVScaleAffectorEnable)
			{
				Affector affector7 = new UVScaleAffector(node);
				list.Add(affector7);
			}
			if (this.GravityAffectorEnable)
			{
				Affector affector8 = new GravityAffector(this.GravityObject, this.GravityAftType, this.IsGravityAccelerate, this.GravityDirection, node);
				list.Add(affector8);
				if (this.GravityAftType == GAFTTYPE.Spherical && this.GravityObject == null)
				{
					Debug.LogWarning("Gravity Object is missing, automatically set to effect layer self:" + base.gameObject.name);
					this.GravityObject = base.transform;
				}
			}
			if (this.AirAffectorEnable)
			{
				Affector affector9 = new AirFieldAffector(this.AirObject, this.AirDirection, this.AirAttenuation, this.AirUseMaxDistance, this.AirMaxDistance, this.AirEnableSpread, this.AirSpread, this.AirInheritVelocity, this.AirInheritRotation, node);
				list.Add(affector9);
			}
			if (this.BombAffectorEnable)
			{
				Affector affector10 = new BombAffector(this.BombObject, this.BombType, this.BombDecayType, this.BombMagnitude, this.BombDecay, this.BombAxis, node);
				list.Add(affector10);
			}
			if (this.TurbulenceAffectorEnable)
			{
				Affector affector11 = new TurbulenceFieldAffector(this.TurbulenceObject, this.TurbulenceAttenuation, this.TurbulenceUseMaxDistance, this.TurbulenceMaxDistance, node);
				list.Add(affector11);
			}
			if (this.DragAffectorEnable)
			{
				Affector affector12 = new DragAffector(this.DragObj, this.DragUseDir, this.DragDir, this.DragMag, this.DragUseMaxDist, this.DragMaxDist, this.DragAtten, node);
				list.Add(affector12);
			}
			if (this.SineAffectorEnable)
			{
				Affector affector13 = new SineAffector(node);
				list.Add(affector13);
			}
			return list;
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x0010FCE0 File Offset: 0x0010DEE0
		public void SetClient(Transform client)
		{
			this.ClientTransform = client;
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode == null)
				{
					effectNode = this.AvailableENodes[i];
				}
				effectNode.ClientTrans = client;
			}
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x0010FD2C File Offset: 0x0010DF2C
		protected void Init()
		{
			this.InitCollision();
			this.Owner = base.transform.parent.gameObject.GetComponent<XffectComponent>();
			if (this.Owner == null)
			{
				Debug.LogError("you must set EffectLayer to be XffectComponent's child.");
			}
			if (this.ClientTransform == null)
			{
				Debug.LogWarning("effect layer: " + base.gameObject.name + " haven't assign a client transform, automaticly set to itself.");
				this.ClientTransform = base.transform;
			}
			this.AvailableENodes = new EffectNode[this.MaxENodes];
			this.ActiveENodes = new EffectNode[this.MaxENodes];
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = new EffectNode(i, this.ClientTransform, this.SyncClient, this);
				List<Affector> affectorList = this.InitAffectors(effectNode);
				effectNode.SetAffectorList(affectorList);
				effectNode.SetRenderType(this.RenderType);
				this.AvailableENodes[i] = effectNode;
			}
			if (this.RenderType == 4)
			{
				this.RopeDatas.Init(this);
			}
			this.AvailableNodeCount = this.MaxENodes;
			this.emitter = new Emitter(this);
			this.mStopped = false;
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x00017188 File Offset: 0x00015388
		public VertexPool GetVertexPool()
		{
			return this.Vertexpool;
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x00017190 File Offset: 0x00015390
		public int GetActiveNodeCount()
		{
			return this.ActiveENodes.Length;
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x0010FE5C File Offset: 0x0010E05C
		public void RemoveActiveNode(EffectNode node)
		{
			if (this.AvailableNodeCount == this.MaxENodes)
			{
				Debug.LogError("something wrong with removing node!");
				return;
			}
			if (this.ActiveENodes[node.Index] == null)
			{
				return;
			}
			this.ActiveENodes[node.Index] = null;
			this.AvailableENodes[node.Index] = node;
			this.AvailableNodeCount++;
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x0010FEC4 File Offset: 0x0010E0C4
		public void AddActiveNode(EffectNode node)
		{
			if (this.AvailableNodeCount == 0)
			{
				Debug.LogError("out index!");
			}
			if (this.AvailableENodes[node.Index] == null)
			{
				return;
			}
			this.ActiveENodes[node.Index] = node;
			this.AvailableENodes[node.Index] = null;
			this.AvailableNodeCount--;
			ulong totalAddedCount;
			this.TotalAddedCount = (totalAddedCount = this.TotalAddedCount) + 1UL;
			node.TotalIndex = totalAddedCount;
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x0010FF3C File Offset: 0x0010E13C
		protected void AddNodes(int num)
		{
			int num2 = 0;
			for (int i = 0; i < this.MaxENodes; i++)
			{
				if (num2 == num)
				{
					break;
				}
				EffectNode effectNode = this.AvailableENodes[i];
				if (effectNode != null)
				{
					this.AddActiveNode(effectNode);
					num2++;
					if (this.UseSubEmitters && !string.IsNullOrEmpty(this.BirthSubEmitter))
					{
						XffectComponent effect = this.SpawnCache.GetEffect(this.BirthSubEmitter);
						if (effect == null)
						{
							return;
						}
						effectNode.SubEmitter = effect;
						effect.Active();
					}
					this.emitter.SetEmitPosition(effectNode);
					float life;
					if (this.IsNodeLifeLoop)
					{
						life = -1f;
					}
					else
					{
						life = Random.Range(this.NodeLifeMin, this.NodeLifeMax);
					}
					Vector3 emitRotation = this.emitter.GetEmitRotation(effectNode);
					float speed = this.OriSpeed;
					if (this.IsRandomSpeed)
					{
						speed = Random.Range(this.SpeedMin, this.SpeedMax);
					}
					Color oriColor = this.Color1;
					if (this.IsRandomStartColor)
					{
						oriColor = this.RandomColorGradient.Evaluate(Random.Range(0f, 1f));
					}
					float num3 = Random.Range(this.OriScaleXMin, this.OriScaleXMax);
					float oriScaleY = Random.Range(this.OriScaleYMin, this.OriScaleYMax);
					if (this.UniformRandomScale)
					{
						oriScaleY = num3;
					}
					effectNode.Init(emitRotation.normalized, speed, life, Random.Range(this.OriRotationMin, this.OriRotationMax), num3, oriScaleY, oriColor, this.UVTopLeft, this.UVDimension);
				}
			}
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x001100E4 File Offset: 0x0010E2E4
		public void Reset()
		{
			if (this.ActiveENodes == null)
			{
				return;
			}
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode != null)
				{
					effectNode.Reset();
					this.RemoveActiveNode(effectNode);
				}
			}
			this.emitter.Reset();
			this.mStopped = false;
			this.TotalAddedCount = 0UL;
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x0011014C File Offset: 0x0010E34C
		public void FixedUpdateCustom(float deltaTime)
		{
			int nodes = this.emitter.GetNodes(deltaTime);
			this.AddNodes(nodes);
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode != null)
				{
					effectNode.Update(deltaTime);
				}
			}
			if (this.RenderType == 4)
			{
				this.RopeDatas.Update(deltaTime);
			}
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x0001719A File Offset: 0x0001539A
		public void StartCustom()
		{
			this.Init();
			this.LastClientPos = this.ClientTransform.position;
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x001101B8 File Offset: 0x0010E3B8
		private void OnDrawGizmos()
		{
			if (this.ClientTransform == null)
			{
				return;
			}
			Gizmos.color = this.DebugColor;
			float num;
			if (this.RenderType == 0)
			{
				num = (this.SpriteWidth + this.SpriteHeight) / 6f;
			}
			else if (this.RenderType == 1)
			{
				num = this.RibbonWidth / 3f;
			}
			else
			{
				num = (this.ConeSize.x + this.ConeSize.y) / 6f;
			}
			num = Mathf.Clamp(num, 0f, 1f);
			if (this.EmitType == 0 || this.EmitType == 3)
			{
				Gizmos.DrawWireSphere(this.ClientTransform.position + this.EmitPoint, num);
			}
			if (this.EmitType == 1)
			{
				Vector3 vector = this.BoxSize;
				if (this.Owner != null)
				{
					vector = this.BoxSize * this.Owner.Scale;
				}
				Gizmos.DrawWireCube(this.ClientTransform.position + this.EmitPoint, vector);
			}
			else if (this.EmitType == 2)
			{
				Gizmos.DrawWireSphere(this.ClientTransform.position + this.EmitPoint, this.Radius);
			}
			else if (this.EmitType == 4)
			{
				if (this.LineStartObj != null && this.LineEndObj != null)
				{
					Vector3 position = this.LineStartObj.position;
					Vector3 position2 = this.LineEndObj.position;
					Gizmos.DrawLine(position, position2);
				}
			}
			else if (this.EmitType == 5)
			{
			}
			if (this.OriVelocityAxis != Vector3.zero)
			{
				Gizmos.DrawLine(this.ClientTransform.position + this.EmitPoint, this.ClientTransform.position + this.EmitPoint + this.ClientTransform.rotation * this.OriVelocityAxis * num * 15f);
			}
			if (this.UseCollisionDetection && this.CollisionType == COLLITION_TYPE.Plane)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireCube(this.ClientTransform.position + this.PlaneOffset, new Vector3(num * 300f, 0f, num * 300f));
				Gizmos.color = Color.white;
			}
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x00110448 File Offset: 0x0010E648
		public bool EmitOver(float deltaTime)
		{
			if (this.ActiveENodes == null)
			{
				return false;
			}
			if (this.AvailableNodeCount == this.MaxENodes)
			{
				if (this.EmitWay == EEmitWay.ByRate)
				{
					if (this.emitter.EmitLoop == 0f)
					{
						return true;
					}
				}
				else if (this.EmitWay == EEmitWay.ByCurve)
				{
					if (this.emitter.CurveEmitDone)
					{
						return true;
					}
				}
				else if (this.EmitWay == EEmitWay.ByDistance && this.mStopped)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x001104D8 File Offset: 0x0010E6D8
		public void StopSmoothly(float fadeTime)
		{
			this.mStopped = true;
			this.emitter.StopEmit();
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode != null)
				{
					if (!this.IsNodeLifeLoop && effectNode.GetLifeTime() < fadeTime)
					{
						fadeTime = effectNode.GetLifeTime() - effectNode.GetElapsedTime();
					}
					effectNode.Fade(fadeTime);
				}
			}
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x00110550 File Offset: 0x0010E750
		public void StopEmit()
		{
			this.mStopped = true;
			if (this.IsNodeLifeLoop && this.EmitWay != EEmitWay.ByDistance)
			{
				for (int i = 0; i < this.MaxENodes; i++)
				{
					EffectNode effectNode = this.ActiveENodes[i];
					if (effectNode != null)
					{
						effectNode.Stop();
					}
				}
			}
			this.emitter.StopEmit();
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x000171B3 File Offset: 0x000153B3
		public void SetCollisionGoalPos(Transform pos)
		{
			if (!this.UseCollisionDetection)
			{
				Debug.LogWarning(base.gameObject.name + "is not set to collision detect mode, please check it");
				return;
			}
			this.CollisionGoal = pos;
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x001105B8 File Offset: 0x0010E7B8
		public void SetArractionAffectorGoal(Transform goal)
		{
			if (!this.GravityAffectorEnable || this.GravityAftType == GAFTTYPE.Planar)
			{
				Debug.LogWarning(base.gameObject.name + "has no attraction affector, please check it");
				return;
			}
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.AvailableENodes[i];
				if (effectNode == null)
				{
					effectNode = this.ActiveENodes[i];
				}
				List<Affector> affectorList = effectNode.GetAffectorList();
				foreach (Affector affector in affectorList)
				{
					if (affector.Type == AFFECTORTYPE.GravityAffector)
					{
						GravityAffector gravityAffector = (GravityAffector)affector;
						gravityAffector.SetAttraction(goal);
					}
				}
			}
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x0011068C File Offset: 0x0010E88C
		public void SetScale(Vector2 scale)
		{
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode == null)
				{
					effectNode = this.AvailableENodes[i];
				}
				effectNode.Scale = scale;
			}
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x001106D0 File Offset: 0x0010E8D0
		public void SetColor(Color c)
		{
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode == null)
				{
					effectNode = this.AvailableENodes[i];
				}
				effectNode.Color = c;
			}
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x00110714 File Offset: 0x0010E914
		public void SetRotation(float angle)
		{
			for (int i = 0; i < this.MaxENodes; i++)
			{
				EffectNode effectNode = this.ActiveENodes[i];
				if (effectNode == null)
				{
					effectNode = this.AvailableENodes[i];
				}
				effectNode.RotateAngle = angle;
			}
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x00110758 File Offset: 0x0010E958
		public EffectNode EmitByPos(Vector3 pos)
		{
			int num = 0;
			EffectNode result = null;
			for (int i = 0; i < this.MaxENodes; i++)
			{
				if (num == 1)
				{
					break;
				}
				EffectNode effectNode = this.AvailableENodes[i];
				if (effectNode != null)
				{
					this.AddActiveNode(effectNode);
					num++;
					effectNode.SetLocalPosition(pos);
					float life;
					if (this.IsNodeLifeLoop)
					{
						life = -1f;
					}
					else
					{
						life = Random.Range(this.NodeLifeMin, this.NodeLifeMax);
					}
					Vector3 emitRotation = this.emitter.GetEmitRotation(effectNode);
					Color oriColor = this.Color1;
					if (this.IsRandomStartColor)
					{
						oriColor = this.RandomColorGradient.Evaluate(Random.Range(0f, 1f));
					}
					effectNode.Init(emitRotation.normalized, this.OriSpeed, life, Random.Range(this.OriRotationMin, this.OriRotationMax), Random.Range(this.OriScaleXMin, this.OriScaleXMax), Random.Range(this.OriScaleYMin, this.OriScaleYMax), oriColor, this.UVTopLeft, this.UVDimension);
					result = effectNode;
				}
			}
			return result;
		}

		// Token: 0x04002945 RID: 10565
		public VertexPool Vertexpool;

		// Token: 0x04002946 RID: 10566
		public Transform ClientTransform;

		// Token: 0x04002947 RID: 10567
		public bool SyncClient;

		// Token: 0x04002948 RID: 10568
		public Material Material;

		// Token: 0x04002949 RID: 10569
		public int RenderType;

		// Token: 0x0400294A RID: 10570
		public float StartTime;

		// Token: 0x0400294B RID: 10571
		public float MaxFps = 60f;

		// Token: 0x0400294C RID: 10572
		public Color DebugColor = Color.white;

		// Token: 0x0400294D RID: 10573
		public int Depth;

		// Token: 0x0400294E RID: 10574
		public int SpriteType;

		// Token: 0x0400294F RID: 10575
		public int OriPoint;

		// Token: 0x04002950 RID: 10576
		public float SpriteWidth = 1f;

		// Token: 0x04002951 RID: 10577
		public float SpriteHeight = 1f;

		// Token: 0x04002952 RID: 10578
		public UV_STRETCH SpriteUVStretch;

		// Token: 0x04002953 RID: 10579
		public bool RandomOriScale;

		// Token: 0x04002954 RID: 10580
		public bool RandomOriRot;

		// Token: 0x04002955 RID: 10581
		public int OriRotationMin;

		// Token: 0x04002956 RID: 10582
		public int OriRotationMax;

		// Token: 0x04002957 RID: 10583
		public bool RotAffectorEnable;

		// Token: 0x04002958 RID: 10584
		public RSTYPE RotateType;

		// Token: 0x04002959 RID: 10585
		public float DeltaRot;

		// Token: 0x0400295A RID: 10586
		public AnimationCurve RotateCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 360f)
		});

		// Token: 0x0400295B RID: 10587
		public WRAP_TYPE RotateCurveWrap;

		// Token: 0x0400295C RID: 10588
		public float RotateCurveTime = 1f;

		// Token: 0x0400295D RID: 10589
		public float RotateCurveMaxValue = 1f;

		// Token: 0x0400295E RID: 10590
		public AnimationCurve RotateCurve01 = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x0400295F RID: 10591
		public float RotateSpeedMin;

		// Token: 0x04002960 RID: 10592
		public float RotateSpeedMax;

		// Token: 0x04002961 RID: 10593
		public bool UniformRandomScale;

		// Token: 0x04002962 RID: 10594
		public float OriScaleXMin = 1f;

		// Token: 0x04002963 RID: 10595
		public float OriScaleXMax = 1f;

		// Token: 0x04002964 RID: 10596
		public float OriScaleYMin = 1f;

		// Token: 0x04002965 RID: 10597
		public float OriScaleYMax = 1f;

		// Token: 0x04002966 RID: 10598
		public bool ScaleAffectorEnable;

		// Token: 0x04002967 RID: 10599
		public RSTYPE ScaleType;

		// Token: 0x04002968 RID: 10600
		public float DeltaScaleX;

		// Token: 0x04002969 RID: 10601
		public float DeltaScaleY;

		// Token: 0x0400296A RID: 10602
		public AnimationCurve ScaleXCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 5f)
		});

		// Token: 0x0400296B RID: 10603
		public AnimationCurve ScaleYCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 5f)
		});

		// Token: 0x0400296C RID: 10604
		public WRAP_TYPE ScaleWrapMode;

		// Token: 0x0400296D RID: 10605
		public float ScaleCurveTime = 1f;

		// Token: 0x0400296E RID: 10606
		public float MaxScaleCalue = 1f;

		// Token: 0x0400296F RID: 10607
		public float MaxScaleValueY = 1f;

		// Token: 0x04002970 RID: 10608
		public AnimationCurve ScaleXCurveNew = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002971 RID: 10609
		public AnimationCurve ScaleYCurveNew = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002972 RID: 10610
		public bool UseSameScaleCurve;

		// Token: 0x04002973 RID: 10611
		public float DeltaScaleXMax;

		// Token: 0x04002974 RID: 10612
		public float DeltaScaleYMax;

		// Token: 0x04002975 RID: 10613
		public bool ColorAffectorEnable;

		// Token: 0x04002976 RID: 10614
		public int ColorAffectType;

		// Token: 0x04002977 RID: 10615
		public float ColorGradualTimeLength = 1f;

		// Token: 0x04002978 RID: 10616
		public COLOR_GRADUAL_TYPE ColorGradualType;

		// Token: 0x04002979 RID: 10617
		public bool IsRandomStartColor;

		// Token: 0x0400297A RID: 10618
		public ColorParameter RandomColorParam;

		// Token: 0x0400297B RID: 10619
		public Gradient RandomColorGradient;

		// Token: 0x0400297C RID: 10620
		public Color Color1 = Color.white;

		// Token: 0x0400297D RID: 10621
		public Color Color2;

		// Token: 0x0400297E RID: 10622
		public Color Color3;

		// Token: 0x0400297F RID: 10623
		public Color Color4;

		// Token: 0x04002980 RID: 10624
		public Color Color5;

		// Token: 0x04002981 RID: 10625
		public COLOR_CHANGE_TYPE ColorChangeType;

		// Token: 0x04002982 RID: 10626
		public ColorParameter ColorParam;

		// Token: 0x04002983 RID: 10627
		public Gradient ColorGradient;

		// Token: 0x04002984 RID: 10628
		public AnimationCurve ColorGradualCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002985 RID: 10629
		public float RibbonWidth = 1f;

		// Token: 0x04002986 RID: 10630
		public int MaxRibbonElements = 8;

		// Token: 0x04002987 RID: 10631
		public float RibbonLen = 15f;

		// Token: 0x04002988 RID: 10632
		public float TailDistance;

		// Token: 0x04002989 RID: 10633
		public bool SyncTrailWithClient;

		// Token: 0x0400298A RID: 10634
		public UV_STRETCH RibbonUVStretch;

		// Token: 0x0400298B RID: 10635
		public bool FaceToObject;

		// Token: 0x0400298C RID: 10636
		public Transform FaceObject;

		// Token: 0x0400298D RID: 10637
		public bool UseRandomRibbon;

		// Token: 0x0400298E RID: 10638
		public float RibbonWidthMin = 1f;

		// Token: 0x0400298F RID: 10639
		public float RibbonWidthMax = 1f;

		// Token: 0x04002990 RID: 10640
		public float RibbonLenMin = 15f;

		// Token: 0x04002991 RID: 10641
		public float RibbonLenMax = 15f;

		// Token: 0x04002992 RID: 10642
		public Vector2 ConeSize = new Vector2(1f, 3f);

		// Token: 0x04002993 RID: 10643
		public float ConeAngle;

		// Token: 0x04002994 RID: 10644
		public int ConeSegment = 12;

		// Token: 0x04002995 RID: 10645
		public bool UseConeAngleChange;

		// Token: 0x04002996 RID: 10646
		public AnimationCurve ConeDeltaAngle = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 60f)
		});

		// Token: 0x04002997 RID: 10647
		public Mesh CMesh;

		// Token: 0x04002998 RID: 10648
		public Vector3 MeshRotateAxis = Vector3.up;

		// Token: 0x04002999 RID: 10649
		public int EmitType;

		// Token: 0x0400299A RID: 10650
		public Vector3 BoxSize;

		// Token: 0x0400299B RID: 10651
		public Vector3 EmitPoint;

		// Token: 0x0400299C RID: 10652
		public float Radius = 1f;

		// Token: 0x0400299D RID: 10653
		public bool UseRandomCircle;

		// Token: 0x0400299E RID: 10654
		public float CircleRadiusMin = 1f;

		// Token: 0x0400299F RID: 10655
		public float CircleRadiusMax = 10f;

		// Token: 0x040029A0 RID: 10656
		public Vector3 CircleDir = Vector3.up;

		// Token: 0x040029A1 RID: 10657
		public bool EmitUniform;

		// Token: 0x040029A2 RID: 10658
		public Transform LineStartObj;

		// Token: 0x040029A3 RID: 10659
		public Transform LineEndObj;

		// Token: 0x040029A4 RID: 10660
		public int MaxENodes = 1;

		// Token: 0x040029A5 RID: 10661
		public bool IsNodeLifeLoop = true;

		// Token: 0x040029A6 RID: 10662
		public float NodeLifeMin = 1f;

		// Token: 0x040029A7 RID: 10663
		public float NodeLifeMax = 1f;

		// Token: 0x040029A8 RID: 10664
		public EEmitWay EmitWay;

		// Token: 0x040029A9 RID: 10665
		public XCurveParam EmitterCurveX;

		// Token: 0x040029AA RID: 10666
		public float DiffDistance = 0.1f;

		// Token: 0x040029AB RID: 10667
		public Mesh EmitMesh;

		// Token: 0x040029AC RID: 10668
		public int EmitMeshType;

		// Token: 0x040029AD RID: 10669
		public bool IsBurstEmit;

		// Token: 0x040029AE RID: 10670
		public float ChanceToEmit = 100f;

		// Token: 0x040029AF RID: 10671
		public float EmitDuration = 100f;

		// Token: 0x040029B0 RID: 10672
		public float EmitRate = 20f;

		// Token: 0x040029B1 RID: 10673
		public int EmitLoop = -1;

		// Token: 0x040029B2 RID: 10674
		public DIRECTION_TYPE DirType;

		// Token: 0x040029B3 RID: 10675
		public Vector3 OriVelocityAxis = Vector3.up;

		// Token: 0x040029B4 RID: 10676
		public int AngleAroundAxis;

		// Token: 0x040029B5 RID: 10677
		public bool UseRandomDirAngle;

		// Token: 0x040029B6 RID: 10678
		public int AngleAroundAxisMax;

		// Token: 0x040029B7 RID: 10679
		public float OriSpeed;

		// Token: 0x040029B8 RID: 10680
		public bool AlwaysSyncRotation;

		// Token: 0x040029B9 RID: 10681
		public bool IsRandomSpeed;

		// Token: 0x040029BA RID: 10682
		public float SpeedMin;

		// Token: 0x040029BB RID: 10683
		public float SpeedMax;

		// Token: 0x040029BC RID: 10684
		public bool JetAffectorEnable;

		// Token: 0x040029BD RID: 10685
		public MAGTYPE JetMagType;

		// Token: 0x040029BE RID: 10686
		public float JetMag;

		// Token: 0x040029BF RID: 10687
		public AnimationCurve JetCurve;

		// Token: 0x040029C0 RID: 10688
		public XCurveParam JetCurveX;

		// Token: 0x040029C1 RID: 10689
		public bool VortexAffectorEnable;

		// Token: 0x040029C2 RID: 10690
		public MAGTYPE VortexMagType;

		// Token: 0x040029C3 RID: 10691
		public float VortexMag = 1f;

		// Token: 0x040029C4 RID: 10692
		public XCurveParam VortexCurveX;

		// Token: 0x040029C5 RID: 10693
		public AnimationCurve VortexCurve;

		// Token: 0x040029C6 RID: 10694
		public Vector3 VortexDirection = Vector3.up;

		// Token: 0x040029C7 RID: 10695
		public bool VortexInheritRotation = true;

		// Token: 0x040029C8 RID: 10696
		public Transform VortexObj;

		// Token: 0x040029C9 RID: 10697
		public bool IsFixedCircle;

		// Token: 0x040029CA RID: 10698
		public bool IsRandomVortexDir;

		// Token: 0x040029CB RID: 10699
		public bool IsVortexAccelerate;

		// Token: 0x040029CC RID: 10700
		public float VortexAttenuation;

		// Token: 0x040029CD RID: 10701
		public bool UseVortexMaxDistance;

		// Token: 0x040029CE RID: 10702
		public float VortexMaxDistance;

		// Token: 0x040029CF RID: 10703
		public bool UVRotAffectorEnable;

		// Token: 0x040029D0 RID: 10704
		public bool RandomUVRotateSpeed;

		// Token: 0x040029D1 RID: 10705
		public float UVRotXSpeed;

		// Token: 0x040029D2 RID: 10706
		public float UVRotYSpeed;

		// Token: 0x040029D3 RID: 10707
		public float UVRotXSpeedMax;

		// Token: 0x040029D4 RID: 10708
		public float UVRotYSpeedMax;

		// Token: 0x040029D5 RID: 10709
		public Vector2 UVRotStartOffset = Vector2.zero;

		// Token: 0x040029D6 RID: 10710
		public bool UVScaleAffectorEnable;

		// Token: 0x040029D7 RID: 10711
		public float UVScaleXSpeed;

		// Token: 0x040029D8 RID: 10712
		public float UVScaleYSpeed;

		// Token: 0x040029D9 RID: 10713
		public bool GravityAffectorEnable;

		// Token: 0x040029DA RID: 10714
		public GAFTTYPE GravityAftType;

		// Token: 0x040029DB RID: 10715
		public MAGTYPE GravityMagType;

		// Token: 0x040029DC RID: 10716
		public float GravityMag;

		// Token: 0x040029DD RID: 10717
		public AnimationCurve GravityCurve;

		// Token: 0x040029DE RID: 10718
		public XCurveParam GravityCurveX;

		// Token: 0x040029DF RID: 10719
		public Vector3 GravityDirection = Vector3.up;

		// Token: 0x040029E0 RID: 10720
		public Transform GravityObject;

		// Token: 0x040029E1 RID: 10721
		public bool IsGravityAccelerate = true;

		// Token: 0x040029E2 RID: 10722
		public bool AirAffectorEnable;

		// Token: 0x040029E3 RID: 10723
		public Transform AirObject;

		// Token: 0x040029E4 RID: 10724
		public MAGTYPE AirMagType;

		// Token: 0x040029E5 RID: 10725
		public float AirMagnitude;

		// Token: 0x040029E6 RID: 10726
		public AnimationCurve AirMagCurve;

		// Token: 0x040029E7 RID: 10727
		public XCurveParam AirMagCurveX;

		// Token: 0x040029E8 RID: 10728
		public Vector3 AirDirection = Vector3.up;

		// Token: 0x040029E9 RID: 10729
		public float AirAttenuation;

		// Token: 0x040029EA RID: 10730
		public bool AirUseMaxDistance;

		// Token: 0x040029EB RID: 10731
		public float AirMaxDistance;

		// Token: 0x040029EC RID: 10732
		public bool AirEnableSpread;

		// Token: 0x040029ED RID: 10733
		public float AirSpread;

		// Token: 0x040029EE RID: 10734
		public float AirInheritVelocity;

		// Token: 0x040029EF RID: 10735
		public bool AirInheritRotation;

		// Token: 0x040029F0 RID: 10736
		public bool BombAffectorEnable;

		// Token: 0x040029F1 RID: 10737
		public Transform BombObject;

		// Token: 0x040029F2 RID: 10738
		public BOMBTYPE BombType = BOMBTYPE.Spherical;

		// Token: 0x040029F3 RID: 10739
		public BOMBDECAYTYPE BombDecayType;

		// Token: 0x040029F4 RID: 10740
		public float BombMagnitude;

		// Token: 0x040029F5 RID: 10741
		public Vector3 BombAxis;

		// Token: 0x040029F6 RID: 10742
		public float BombDecay;

		// Token: 0x040029F7 RID: 10743
		public bool TurbulenceAffectorEnable;

		// Token: 0x040029F8 RID: 10744
		public Transform TurbulenceObject;

		// Token: 0x040029F9 RID: 10745
		public MAGTYPE TurbulenceMagType;

		// Token: 0x040029FA RID: 10746
		public float TurbulenceMagnitude = 1f;

		// Token: 0x040029FB RID: 10747
		public XCurveParam TurbulenceMagCurveX;

		// Token: 0x040029FC RID: 10748
		public AnimationCurve TurbulenceMagCurve;

		// Token: 0x040029FD RID: 10749
		public float TurbulenceAttenuation;

		// Token: 0x040029FE RID: 10750
		public bool TurbulenceUseMaxDistance;

		// Token: 0x040029FF RID: 10751
		public float TurbulenceMaxDistance;

		// Token: 0x04002A00 RID: 10752
		public Vector3 TurbulenceForce = Vector3.one;

		// Token: 0x04002A01 RID: 10753
		public bool DragAffectorEnable;

		// Token: 0x04002A02 RID: 10754
		public Transform DragObj;

		// Token: 0x04002A03 RID: 10755
		public bool DragUseDir;

		// Token: 0x04002A04 RID: 10756
		public Vector3 DragDir = Vector3.up;

		// Token: 0x04002A05 RID: 10757
		public float DragMag = 10f;

		// Token: 0x04002A06 RID: 10758
		public bool DragUseMaxDist;

		// Token: 0x04002A07 RID: 10759
		public float DragMaxDist = 50f;

		// Token: 0x04002A08 RID: 10760
		public float DragAtten;

		// Token: 0x04002A09 RID: 10761
		public bool UVAffectorEnable;

		// Token: 0x04002A0A RID: 10762
		public int UVType;

		// Token: 0x04002A0B RID: 10763
		public Vector2 OriTopLeftUV = Vector2.zero;

		// Token: 0x04002A0C RID: 10764
		public Vector2 OriUVDimensions = Vector2.one;

		// Token: 0x04002A0D RID: 10765
		protected Vector2 UVTopLeft;

		// Token: 0x04002A0E RID: 10766
		protected Vector2 UVDimension;

		// Token: 0x04002A0F RID: 10767
		public int Cols = 1;

		// Token: 0x04002A10 RID: 10768
		public int Rows = 1;

		// Token: 0x04002A11 RID: 10769
		public int LoopCircles = -1;

		// Token: 0x04002A12 RID: 10770
		public float UVTime = 1f;

		// Token: 0x04002A13 RID: 10771
		public string EanPath = "none";

		// Token: 0x04002A14 RID: 10772
		public int EanIndex;

		// Token: 0x04002A15 RID: 10773
		public bool RandomStartFrame;

		// Token: 0x04002A16 RID: 10774
		public bool UseCollisionDetection;

		// Token: 0x04002A17 RID: 10775
		public float ParticleRadius = 1f;

		// Token: 0x04002A18 RID: 10776
		public COLLITION_TYPE CollisionType;

		// Token: 0x04002A19 RID: 10777
		public bool CollisionAutoDestroy = true;

		// Token: 0x04002A1A RID: 10778
		public Transform EventReceiver;

		// Token: 0x04002A1B RID: 10779
		public string EventHandleFunctionName = " ";

		// Token: 0x04002A1C RID: 10780
		public Transform CollisionGoal;

		// Token: 0x04002A1D RID: 10781
		public float ColliisionPosRange;

		// Token: 0x04002A1E RID: 10782
		public LayerMask CollisionLayer;

		// Token: 0x04002A1F RID: 10783
		public Vector3 PlaneDir = Vector3.up;

		// Token: 0x04002A20 RID: 10784
		public Vector3 PlaneOffset = new Vector3(0f, -10f, 0f);

		// Token: 0x04002A21 RID: 10785
		protected Plane mCollisionPlane;

		// Token: 0x04002A22 RID: 10786
		public float RopeWidth = 1f;

		// Token: 0x04002A23 RID: 10787
		public float RopeUVLen = 5f;

		// Token: 0x04002A24 RID: 10788
		public bool RopeFixUVLen = true;

		// Token: 0x04002A25 RID: 10789
		public bool SineAffectorEnable;

		// Token: 0x04002A26 RID: 10790
		public MAGTYPE SineMagType;

		// Token: 0x04002A27 RID: 10791
		public float SineMagnitude = 1f;

		// Token: 0x04002A28 RID: 10792
		public float SineTime = 1f;

		// Token: 0x04002A29 RID: 10793
		public XCurveParam SineMagCurveX;

		// Token: 0x04002A2A RID: 10794
		public Vector3 SineForce = Vector3.up;

		// Token: 0x04002A2B RID: 10795
		public bool SineIsAccelarate;

		// Token: 0x04002A2C RID: 10796
		public bool UseShaderCurve1;

		// Token: 0x04002A2D RID: 10797
		public bool UseShaderCurve2;

		// Token: 0x04002A2E RID: 10798
		public XCurveParam ShaderCurveX1;

		// Token: 0x04002A2F RID: 10799
		public XCurveParam ShaderCurveX2;

		// Token: 0x04002A30 RID: 10800
		protected ulong TotalAddedCount;

		// Token: 0x04002A31 RID: 10801
		public bool UseSubEmitters;

		// Token: 0x04002A32 RID: 10802
		public XffectCache SpawnCache;

		// Token: 0x04002A33 RID: 10803
		public string BirthSubEmitter;

		// Token: 0x04002A34 RID: 10804
		public string CollisionSubEmitter;

		// Token: 0x04002A35 RID: 10805
		public string DeathSubEmitter;

		// Token: 0x04002A36 RID: 10806
		public bool SubEmitterAutoStop = true;

		// Token: 0x04002A37 RID: 10807
		public Emitter emitter;

		// Token: 0x04002A38 RID: 10808
		public EffectNode[] AvailableENodes;

		// Token: 0x04002A39 RID: 10809
		public EffectNode[] ActiveENodes;

		// Token: 0x04002A3A RID: 10810
		public int AvailableNodeCount;

		// Token: 0x04002A3B RID: 10811
		public Vector3 LastClientPos;

		// Token: 0x04002A3C RID: 10812
		public XffectComponent Owner;

		// Token: 0x04002A3D RID: 10813
		public bool mStopped;

		// Token: 0x04002A3E RID: 10814
		public RopeData RopeDatas = new RopeData();
	}
}
