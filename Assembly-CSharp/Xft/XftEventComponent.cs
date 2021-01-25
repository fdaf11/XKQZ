using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000598 RID: 1432
	public class XftEventComponent : MonoBehaviour
	{
		// Token: 0x17000380 RID: 896
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x00017DA0 File Offset: 0x00015FA0
		public float ElapsedTime
		{
			get
			{
				return this.m_elapsedTime;
			}
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x00117CFC File Offset: 0x00115EFC
		public void Initialize(XffectComponent owner)
		{
			this.Owner = owner;
			switch (this.Type)
			{
			case XEventType.CameraShake:
				this.m_eventHandler = new CameraShakeEvent(this);
				break;
			case XEventType.Sound:
				this.m_eventHandler = new SoundEvent(this);
				break;
			case XEventType.Light:
				this.m_eventHandler = new LightEvent(this);
				break;
			case XEventType.CameraEffect:
				if (this.CameraEffectType == CameraEffectEvent.EType.ColorInverse)
				{
					this.m_eventHandler = new ColorInverseEvent(this);
				}
				else if (this.CameraEffectType == CameraEffectEvent.EType.Glow)
				{
					this.m_eventHandler = new GlowEvent(this);
				}
				else if (this.CameraEffectType == CameraEffectEvent.EType.GlowPerObj)
				{
					this.m_eventHandler = new GlowPerObjEvent(this);
				}
				else if (this.CameraEffectType == CameraEffectEvent.EType.RadialBlur)
				{
					this.m_eventHandler = new RadialBlurEvent(this);
				}
				else if (this.CameraEffectType == CameraEffectEvent.EType.RadialBlurMask)
				{
					this.m_eventHandler = new RadialBlurTexAddEvent(this);
				}
				else if (this.CameraEffectType == CameraEffectEvent.EType.Glitch)
				{
					this.m_eventHandler = new GlitchEvent(this);
				}
				break;
			case XEventType.TimeScale:
				this.m_eventHandler = new TimeScaleEvent(this);
				break;
			default:
				Debug.LogWarning("invalid event type!");
				break;
			}
			this.m_eventHandler.Initialize();
			this.m_elapsedTime = 0f;
			this.m_finished = false;
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x00017DA8 File Offset: 0x00015FA8
		public void ResetCustom()
		{
			this.m_elapsedTime = 0f;
			if (this.m_eventHandler != null)
			{
				this.m_eventHandler.Reset();
			}
			this.m_finished = false;
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x00117E54 File Offset: 0x00116054
		public void UpdateCustom(float deltaTime)
		{
			if (this.m_finished)
			{
				return;
			}
			if (this.m_eventHandler != null)
			{
				this.m_elapsedTime += deltaTime;
				if (!this.m_eventHandler.CanUpdate && this.m_elapsedTime >= this.StartTime && this.StartTime >= 0f)
				{
					this.m_eventHandler.OnBegin();
				}
				if (this.m_eventHandler.CanUpdate)
				{
					this.m_eventHandler.Update(deltaTime);
				}
				if (this.m_eventHandler.CanUpdate && this.m_elapsedTime > this.StartTime + this.EndTime && this.EndTime > 0f)
				{
					this.ResetCustom();
					this.m_finished = true;
				}
			}
		}

		// Token: 0x04002B33 RID: 11059
		public XftEventType EventType;

		// Token: 0x04002B34 RID: 11060
		public XEventType Type;

		// Token: 0x04002B35 RID: 11061
		public float StartTime;

		// Token: 0x04002B36 RID: 11062
		public float EndTime = -1f;

		// Token: 0x04002B37 RID: 11063
		public CameraEffectEvent.EType CameraEffectType = CameraEffectEvent.EType.Glow;

		// Token: 0x04002B38 RID: 11064
		public int CameraEffectPriority;

		// Token: 0x04002B39 RID: 11065
		public Shader RadialBlurShader;

		// Token: 0x04002B3A RID: 11066
		public Transform RadialBlurObj;

		// Token: 0x04002B3B RID: 11067
		public float RBSampleDist = 0.3f;

		// Token: 0x04002B3C RID: 11068
		public MAGTYPE RBStrengthType;

		// Token: 0x04002B3D RID: 11069
		public float RBSampleStrength = 1f;

		// Token: 0x04002B3E RID: 11070
		public AnimationCurve RBSampleStrengthCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002B3F RID: 11071
		public XCurveParam RBSampleStrengthCurveX;

		// Token: 0x04002B40 RID: 11072
		public Shader RadialBlurTexAddShader;

		// Token: 0x04002B41 RID: 11073
		public Texture2D RadialBlurMask;

		// Token: 0x04002B42 RID: 11074
		public float RBMaskSampleDist = 3f;

		// Token: 0x04002B43 RID: 11075
		public MAGTYPE RBMaskStrengthType;

		// Token: 0x04002B44 RID: 11076
		public float RBMaskSampleStrength = 5f;

		// Token: 0x04002B45 RID: 11077
		public AnimationCurve RBMaskSampleStrengthCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002B46 RID: 11078
		public XCurveParam RBMaskSampleStrengthCurveX;

		// Token: 0x04002B47 RID: 11079
		public Shader GlowCompositeShader;

		// Token: 0x04002B48 RID: 11080
		public Shader GlowBlurShader;

		// Token: 0x04002B49 RID: 11081
		public Shader GlowDownSampleShader;

		// Token: 0x04002B4A RID: 11082
		public float GlowIntensity = 1.5f;

		// Token: 0x04002B4B RID: 11083
		public int GlowBlurIterations = 3;

		// Token: 0x04002B4C RID: 11084
		public float GlowBlurSpread = 0.7f;

		// Token: 0x04002B4D RID: 11085
		public Color GlowColorStart = new Color(0f, 0.02745098f, 0.81960785f, 0.4392157f);

		// Token: 0x04002B4E RID: 11086
		public Color GlowColorEnd = new Color(0.29803923f, 0.5882353f, 1f, 1f);

		// Token: 0x04002B4F RID: 11087
		public COLOR_GRADUAL_TYPE GlowColorGradualType;

		// Token: 0x04002B50 RID: 11088
		public float GlowColorGradualTime = 2f;

		// Token: 0x04002B51 RID: 11089
		public AnimationCurve ColorCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002B52 RID: 11090
		public Shader GlowPerObjReplacementShader;

		// Token: 0x04002B53 RID: 11091
		public Shader GlowPerObjBlendShader;

		// Token: 0x04002B54 RID: 11092
		public Shader ColorInverseShader;

		// Token: 0x04002B55 RID: 11093
		public AnimationCurve CIStrengthCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002B56 RID: 11094
		public Shader GlitchShader;

		// Token: 0x04002B57 RID: 11095
		public Texture2D GlitchMask;

		// Token: 0x04002B58 RID: 11096
		public float MinAmp;

		// Token: 0x04002B59 RID: 11097
		public float MaxAmp = 0.05f;

		// Token: 0x04002B5A RID: 11098
		public float MinRand = 0.05f;

		// Token: 0x04002B5B RID: 11099
		public float MaxRand = 0.85f;

		// Token: 0x04002B5C RID: 11100
		public int WaveLen = 10;

		// Token: 0x04002B5D RID: 11101
		public AudioClip Clip;

		// Token: 0x04002B5E RID: 11102
		public float Volume = 1f;

		// Token: 0x04002B5F RID: 11103
		public float Pitch = 1f;

		// Token: 0x04002B60 RID: 11104
		public bool IsSoundLoop;

		// Token: 0x04002B61 RID: 11105
		public Vector3 PositionForce = new Vector3(0f, 4f, 0f);

		// Token: 0x04002B62 RID: 11106
		public Vector3 RotationForce = Vector3.zero;

		// Token: 0x04002B63 RID: 11107
		public float PositionStifness = 0.3f;

		// Token: 0x04002B64 RID: 11108
		public float PositionDamping = 0.1f;

		// Token: 0x04002B65 RID: 11109
		public float RotationStiffness = 0.1f;

		// Token: 0x04002B66 RID: 11110
		public float RotationDamping = 0.25f;

		// Token: 0x04002B67 RID: 11111
		public XCameraShakeType CameraShakeType;

		// Token: 0x04002B68 RID: 11112
		public float ShakeCurveTime = 1f;

		// Token: 0x04002B69 RID: 11113
		public AnimationCurve PositionCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.5f),
			new Keyframe(0.33f, 1f),
			new Keyframe(0.66f, 0f),
			new Keyframe(1f, 0.5f)
		});

		// Token: 0x04002B6A RID: 11114
		public AnimationCurve RotationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.5f),
			new Keyframe(0.33f, 1f),
			new Keyframe(0.66f, 0f),
			new Keyframe(1f, 0.5f)
		});

		// Token: 0x04002B6B RID: 11115
		public bool UseEarthQuake;

		// Token: 0x04002B6C RID: 11116
		public float EarthQuakeMagnitude = 2f;

		// Token: 0x04002B6D RID: 11117
		public MAGTYPE EarthQuakeMagTye;

		// Token: 0x04002B6E RID: 11118
		public AnimationCurve EarthQuakeMagCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002B6F RID: 11119
		public XCurveParam EarthQuakeMagCurveX;

		// Token: 0x04002B70 RID: 11120
		public float EarthQuakeTime = 2f;

		// Token: 0x04002B71 RID: 11121
		public float EarthQuakeCameraRollFactor = 0.1f;

		// Token: 0x04002B72 RID: 11122
		public Light LightComp;

		// Token: 0x04002B73 RID: 11123
		public float LightIntensity = 1f;

		// Token: 0x04002B74 RID: 11124
		public MAGTYPE LightIntensityType;

		// Token: 0x04002B75 RID: 11125
		public AnimationCurve LightIntensityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04002B76 RID: 11126
		public XCurveParam LightIntensityCurveX;

		// Token: 0x04002B77 RID: 11127
		public float LightRange = 10f;

		// Token: 0x04002B78 RID: 11128
		public MAGTYPE LightRangeType;

		// Token: 0x04002B79 RID: 11129
		public AnimationCurve LightRangeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 20f)
		});

		// Token: 0x04002B7A RID: 11130
		public XCurveParam LightRangeCurveX;

		// Token: 0x04002B7B RID: 11131
		public float TimeScale = 1f;

		// Token: 0x04002B7C RID: 11132
		public float TimeScaleDuration = 1f;

		// Token: 0x04002B7D RID: 11133
		public XffectComponent Owner;

		// Token: 0x04002B7E RID: 11134
		protected XftEvent m_eventHandler;

		// Token: 0x04002B7F RID: 11135
		protected float m_elapsedTime;

		// Token: 0x04002B80 RID: 11136
		protected bool m_finished;
	}
}
