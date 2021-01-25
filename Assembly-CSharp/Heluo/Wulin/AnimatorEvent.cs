using System;
using System.Collections.Generic;
using JsonFx.Json;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020000E3 RID: 227
	public class AnimatorEvent
	{
		// Token: 0x060004D0 RID: 1232 RVA: 0x0000502C File Offset: 0x0000322C
		public void Reset()
		{
			this.fireTime = this.originalTime;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00039088 File Offset: 0x00037288
		public void Load(Transform t, AnimationClip clip)
		{
			t.ForEach(delegate(Transform x)
			{
				this.TraversalFind(x, this.linkPoint);
			});
			if (clip != null)
			{
				this.originalTime = (this.fireTime = (float)this.keyframe / clip.frameRate);
			}
			else
			{
				this.originalTime = (this.fireTime = (float)this.keyframe / 30f);
			}
			if (this.audioPath.Length > "Assets/Resources/Artist/".Length)
			{
				string text = this.audioPath.Substring("Assets/Resources/Artist/".Length);
				text = text.Substring(0, text.Length - 4);
				if (Game.g_AudioBundle.Contains(text))
				{
					this.audio = (Game.g_AudioBundle.Load(text) as AudioClip);
				}
				else
				{
					Debug.LogWarning("File No Found audio = " + text);
				}
			}
			if (this.effectPath.Length > "Assets/Resources/Artist/CantUpLoad/".Length)
			{
				string text = this.effectPath.Substring("Assets/Resources/Artist/CantUpLoad/".Length);
				text = text.Substring(0, text.Length - 7);
				if (Game.g_EffectsBundle != null)
				{
					if (Game.g_EffectsBundle.Contains(text))
					{
						this.effect = (Game.g_EffectsBundle.Load(text) as GameObject);
					}
					else
					{
						Debug.LogWarning("File No Found effect = " + text);
					}
				}
				else
				{
					Debug.LogWarning("File No Found effect = " + text);
				}
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00039208 File Offset: 0x00037408
		private void TraversalFind(Transform t, string name)
		{
			if (t.name == name)
			{
				this.linkTransform = t;
			}
			else
			{
				t.ForEach(delegate(Transform x)
				{
					this.TraversalFind(x, name);
				});
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00039260 File Offset: 0x00037460
		public void Play(Transform t, Vector3 refPoint, Quaternion refRot)
		{
			if (this.audio != null && t.audio != null)
			{
				t.audio.PlayOneShot(this.audio);
			}
			if (this.effect != null)
			{
				GameObject gameObject = Object.Instantiate(this.effect, Vector3.zero, Quaternion.identity) as GameObject;
				gameObject.name = this.effect.name;
				if (this.linkTransform == null)
				{
					gameObject.transform.localPosition = refPoint + refRot * this.linkPosition;
					gameObject.transform.localRotation = refRot * Quaternion.Euler(this.linkRotation);
				}
				else
				{
					gameObject.transform.parent = this.linkTransform;
					gameObject.transform.localPosition = this.linkPosition;
					gameObject.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				switch (this.type)
				{
				case 0:
					goto IL_14F;
				case 2:
					Object.Destroy(gameObject, this.lifetime);
					goto IL_14F;
				}
				Object.Destroy(gameObject, 5f);
			}
			IL_14F:
			this.fireTime += 1f;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000393D0 File Offset: 0x000375D0
		public void PlayEventAudio(Transform t)
		{
			if (this.audio != null)
			{
				float length = this.audio.length;
				GameObject gameObject = new GameObject("PlayAudio");
				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				gameObject.transform.position = t.position;
				audioSource.rolloffMode = 1;
				audioSource.minDistance = 20f;
				audioSource.playOnAwake = false;
				audioSource.volume = GameGlobal.m_fSoundValue;
				audioSource.ignoreListenerVolume = true;
				audioSource.clip = this.audio;
				audioSource.loop = false;
				audioSource.Play();
				Object.Destroy(gameObject, length);
			}
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00039468 File Offset: 0x00037668
		public void PlayEventTarget(Transform t, UnitTB unit)
		{
			if (this.effect != null)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "TempObject";
				if (this.linkTransform == null)
				{
					gameObject.transform.parent = t.transform;
					gameObject.transform.localPosition = this.linkPosition;
					gameObject.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				else
				{
					gameObject.transform.parent = this.linkTransform;
					gameObject.transform.localPosition = this.linkPosition;
					gameObject.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				GameObject gameObject2 = Object.Instantiate(this.effect, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
				Object.Destroy(gameObject);
				if (gameObject2.GetComponent<EffectSettings>() != null)
				{
					gameObject2.GetComponent<EffectSettings>().SetTarget(unit.gameObject);
				}
				gameObject2.name = this.effect.name;
				gameObject2.tag = "DynamicEffect";
				if (this.linkTransform == null)
				{
					gameObject2.transform.parent = t.transform;
					gameObject2.transform.localPosition = this.linkPosition;
					gameObject2.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				else
				{
					gameObject2.transform.parent = this.linkTransform;
					gameObject2.transform.localPosition = this.linkPosition;
					gameObject2.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				List<ParticleSystem> list = new List<ParticleSystem>();
				List<ParticleAnimator> list2 = new List<ParticleAnimator>();
				List<ParticleEmitter> list3 = new List<ParticleEmitter>();
				List<TrailRenderer> list4 = new List<TrailRenderer>();
				ParticleSystem[] array = gameObject2.GetComponents<ParticleSystem>();
				ParticleAnimator[] array2 = gameObject2.GetComponents<ParticleAnimator>();
				ParticleEmitter[] array3 = gameObject2.GetComponents<ParticleEmitter>();
				TrailRenderer[] array4 = gameObject2.GetComponents<TrailRenderer>();
				list.AddRange(array);
				list2.AddRange(array2);
				list3.AddRange(array3);
				list4.AddRange(array4);
				array = gameObject2.GetComponentsInChildren<ParticleSystem>();
				array2 = gameObject2.GetComponentsInChildren<ParticleAnimator>();
				array3 = gameObject2.GetComponentsInChildren<ParticleEmitter>();
				array4 = gameObject2.GetComponentsInChildren<TrailRenderer>();
				list.AddRange(array);
				list2.AddRange(array2);
				list3.AddRange(array3);
				list4.AddRange(array4);
				this.PlayOne(gameObject2, list2.ToArray(), list3.ToArray(), list4.ToArray());
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x000396F0 File Offset: 0x000378F0
		public void PlayEvent(Transform t, UnitTB unit)
		{
			if (this.audio != null)
			{
				float length = this.audio.length;
				GameObject gameObject = new GameObject("PlayAudio");
				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				gameObject.transform.position = t.position;
				audioSource.rolloffMode = 1;
				audioSource.minDistance = 20f;
				audioSource.playOnAwake = false;
				audioSource.volume = GameGlobal.m_fSoundValue;
				audioSource.ignoreListenerVolume = true;
				audioSource.clip = this.audio;
				audioSource.loop = false;
				audioSource.Play();
				Object.Destroy(gameObject, length);
			}
			if (this.effect != null)
			{
				GameObject gameObject2 = new GameObject();
				gameObject2.name = "TempObject";
				if (this.linkTransform == null)
				{
					gameObject2.transform.parent = t.transform;
					gameObject2.transform.localPosition = this.linkPosition;
					gameObject2.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				else
				{
					gameObject2.transform.parent = this.linkTransform;
					gameObject2.transform.localPosition = this.linkPosition;
					gameObject2.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				GameObject gameObject3 = Object.Instantiate(this.effect, gameObject2.transform.position, gameObject2.transform.rotation) as GameObject;
				Object.Destroy(gameObject2);
				gameObject3.name = this.effect.name;
				gameObject3.tag = "DynamicEffect";
				if (this.linkTransform == null)
				{
					gameObject3.transform.parent = t.transform;
					gameObject3.transform.localPosition = this.linkPosition;
					gameObject3.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				else
				{
					gameObject3.transform.parent = this.linkTransform;
					gameObject3.transform.localPosition = this.linkPosition;
					gameObject3.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				List<ParticleSystem> list = new List<ParticleSystem>();
				List<ParticleAnimator> list2 = new List<ParticleAnimator>();
				List<ParticleEmitter> list3 = new List<ParticleEmitter>();
				List<TrailRenderer> list4 = new List<TrailRenderer>();
				ParticleSystem[] array = gameObject3.GetComponents<ParticleSystem>();
				ParticleAnimator[] array2 = gameObject3.GetComponents<ParticleAnimator>();
				ParticleEmitter[] array3 = gameObject3.GetComponents<ParticleEmitter>();
				TrailRenderer[] array4 = gameObject3.GetComponents<TrailRenderer>();
				list.AddRange(array);
				list2.AddRange(array2);
				list3.AddRange(array3);
				list4.AddRange(array4);
				array = gameObject3.GetComponentsInChildren<ParticleSystem>();
				array2 = gameObject3.GetComponentsInChildren<ParticleAnimator>();
				array3 = gameObject3.GetComponentsInChildren<ParticleEmitter>();
				array4 = gameObject3.GetComponentsInChildren<TrailRenderer>();
				list.AddRange(array);
				list2.AddRange(array2);
				list3.AddRange(array3);
				list4.AddRange(array4);
				switch (this.type)
				{
				case 0:
					break;
				case 1:
					this.PlayOne(gameObject3, list2.ToArray(), list3.ToArray(), list4.ToArray());
					break;
				case 2:
					if (gameObject3.GetComponent("HideTargetModel") != null)
					{
						gameObject3.SendMessage("SetTarget", unit.gameObject, 1);
					}
					this.PlayLifeTime(gameObject3, list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray());
					break;
				default:
					Object.Destroy(gameObject3, 5f);
					break;
				}
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00039A88 File Offset: 0x00037C88
		private void PlayOne(GameObject go, ParticleAnimator[] pa, ParticleEmitter[] pe, TrailRenderer[] trArray)
		{
			foreach (ParticleAnimator particleAnimator in pa)
			{
				particleAnimator.autodestruct = true;
			}
			foreach (ParticleEmitter particleEmitter in pe)
			{
				particleEmitter.emit = false;
				particleEmitter.Emit();
			}
			foreach (TrailRenderer trailRenderer in trArray)
			{
				trailRenderer.autodestruct = true;
			}
			Object.Destroy(go, 2f);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00039B1C File Offset: 0x00037D1C
		private void PlayLifeTime(GameObject go, ParticleSystem[] ps, ParticleAnimator[] pa, ParticleEmitter[] pe, TrailRenderer[] trArray)
		{
			foreach (ParticleAnimator particleAnimator in pa)
			{
				particleAnimator.autodestruct = false;
			}
			foreach (TrailRenderer trailRenderer in trArray)
			{
				trailRenderer.autodestruct = true;
			}
			bool bNeedDelete = false;
			if (go.GetComponent("CSWeaponTrail") != null)
			{
				bNeedDelete = true;
			}
			if (go.GetComponent("CharacterShadowEffect") != null)
			{
				bNeedDelete = true;
			}
			if (go.GetComponent("HideTargetModel") != null)
			{
				bNeedDelete = true;
			}
			PlayEffectAutoDestroy playEffectAutoDestroy = go.AddComponent<PlayEffectAutoDestroy>();
			playEffectAutoDestroy.psArray = ps;
			playEffectAutoDestroy.paArray = pa;
			playEffectAutoDestroy.peArray = pe;
			playEffectAutoDestroy.trArray = trArray;
			playEffectAutoDestroy.bNeedDelete = bNeedDelete;
			Object.Destroy(playEffectAutoDestroy, this.lifetime);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00039C04 File Offset: 0x00037E04
		public float CalcEmit(Transform t, UnitTB unitTarget)
		{
			if (unitTarget == null)
			{
				return 0f;
			}
			GameObject gameObject = new GameObject();
			gameObject.name = "TempObject";
			if (this.linkTransform == null)
			{
				gameObject.transform.parent = t.transform;
				gameObject.transform.localPosition = this.linkPosition;
				gameObject.transform.localRotation = Quaternion.Euler(this.linkRotation);
			}
			else
			{
				gameObject.transform.parent = this.linkTransform;
				gameObject.transform.localPosition = this.linkPosition;
				gameObject.transform.localRotation = Quaternion.Euler(this.linkRotation);
			}
			float num = Vector3.Distance(gameObject.transform.position, unitTarget.transform.position);
			if (this.flySpeed > 0f)
			{
				return num / this.flySpeed;
			}
			return 0f;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00039D10 File Offset: 0x00037F10
		public float PlayEmit(Transform t, UnitTB unit)
		{
			if (this.audio != null && t.audio != null)
			{
				float length = this.audio.length;
				GameObject gameObject = new GameObject("PlayAudio");
				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				gameObject.transform.position = t.position;
				audioSource.clip = this.audio;
				audioSource.volume = GameGlobal.m_fSoundValue;
				audioSource.loop = false;
				audioSource.Play();
				Object.Destroy(gameObject, length);
			}
			if (this.effect != null)
			{
				GameObject gameObject2 = new GameObject();
				gameObject2.name = "TempObject";
				if (this.linkTransform == null)
				{
					gameObject2.transform.parent = t.transform;
					gameObject2.transform.localPosition = this.linkPosition;
					gameObject2.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				else
				{
					gameObject2.transform.parent = this.linkTransform;
					gameObject2.transform.localPosition = this.linkPosition;
					gameObject2.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				GameObject gameObject3 = Object.Instantiate(this.effect, gameObject2.transform.position, gameObject2.transform.rotation) as GameObject;
				Object.Destroy(gameObject2);
				gameObject3.name = this.effect.name;
				gameObject3.tag = "DynamicEffect";
				if (this.linkTransform == null)
				{
					gameObject3.transform.parent = t.transform;
					gameObject3.transform.localPosition = this.linkPosition;
					gameObject3.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				else
				{
					gameObject3.transform.parent = this.linkTransform;
					gameObject3.transform.localPosition = this.linkPosition;
					gameObject3.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				AnimatorEventFly animatorEventFly = new GameObject("FlyObject")
				{
					tag = "DynamicEffect",
					transform = 
					{
						parent = gameObject3.transform,
						localPosition = new Vector3(0f, 0f, 0f)
					}
				}.AddComponent<AnimatorEventFly>();
				animatorEventFly.Speed = this.flySpeed;
				return animatorEventFly.SetTarget(unit);
			}
			return 1f;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00039FB0 File Offset: 0x000381B0
		public Vector3 PlayEmitLine(Transform t, UnitTB unit)
		{
			if (this.audio != null && t.audio != null)
			{
				float length = this.audio.length;
				GameObject gameObject = new GameObject("PlayAudio");
				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				gameObject.transform.position = t.position;
				audioSource.clip = this.audio;
				audioSource.volume = GameGlobal.m_fSoundValue;
				audioSource.loop = false;
				audioSource.Play();
				Object.Destroy(gameObject, length);
			}
			if (this.effect != null)
			{
				GameObject gameObject2 = new GameObject();
				gameObject2.name = "TempObject";
				if (this.linkTransform == null)
				{
					gameObject2.transform.parent = t.transform;
					gameObject2.transform.localPosition = this.linkPosition;
					gameObject2.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				else
				{
					gameObject2.transform.parent = this.linkTransform;
					gameObject2.transform.localPosition = this.linkPosition;
					gameObject2.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				GameObject gameObject3 = Object.Instantiate(this.effect, gameObject2.transform.position, gameObject2.transform.rotation) as GameObject;
				Object.Destroy(gameObject2);
				gameObject3.name = this.effect.name;
				if (this.linkTransform == null)
				{
					gameObject3.transform.parent = t.transform;
					gameObject3.transform.localPosition = this.linkPosition;
					gameObject3.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				else
				{
					gameObject3.transform.parent = this.linkTransform;
					gameObject3.transform.localPosition = this.linkPosition;
					gameObject3.transform.localRotation = Quaternion.Euler(this.linkRotation);
				}
				AnimatorEventFly animatorEventFly = new GameObject("FlyObject")
				{
					tag = "DynamicEffect",
					transform = 
					{
						parent = gameObject3.transform,
						localPosition = new Vector3(0f, 0f, 0f)
					}
				}.AddComponent<AnimatorEventFly>();
				animatorEventFly.Speed = this.flySpeed;
				animatorEventFly.DelayTime = this.lifetime;
				animatorEventFly.SetLineDir(unit);
				return gameObject3.transform.position;
			}
			return unit.transform.position;
		}

		// Token: 0x0400047E RID: 1150
		private const string ResourcePath = "Assets/Resources/";

		// Token: 0x0400047F RID: 1151
		private const string EffectsPath = "Assets/Resources/Artist/CantUpLoad/";

		// Token: 0x04000480 RID: 1152
		private const string AudioPath = "Assets/Resources/Artist/";

		// Token: 0x04000481 RID: 1153
		[JsonName("m_AssetPathChanged")]
		public bool assetPathChanged;

		// Token: 0x04000482 RID: 1154
		[JsonName("m_AudioPath")]
		public string audioPath;

		// Token: 0x04000483 RID: 1155
		[JsonName("m_Effect_Asset")]
		public string effectPath;

		// Token: 0x04000484 RID: 1156
		[JsonName("m_FlySpeed")]
		public float flySpeed;

		// Token: 0x04000485 RID: 1157
		[JsonName("m_Keyframe")]
		public int keyframe;

		// Token: 0x04000486 RID: 1158
		[JsonName("m_LifeTime")]
		public float lifetime;

		// Token: 0x04000487 RID: 1159
		[JsonName("m_Link_Pos")]
		public AnimatorEvent.Vector linkPosition;

		// Token: 0x04000488 RID: 1160
		[JsonName("m_Link_Rot")]
		public AnimatorEvent.Vector linkRotation;

		// Token: 0x04000489 RID: 1161
		[JsonName("m_Link_tf_name")]
		public string linkPoint;

		// Token: 0x0400048A RID: 1162
		[JsonName("m_PlayType")]
		public int type;

		// Token: 0x0400048B RID: 1163
		public float fireTime;

		// Token: 0x0400048C RID: 1164
		private float originalTime;

		// Token: 0x0400048D RID: 1165
		private AudioClip audio;

		// Token: 0x0400048E RID: 1166
		private GameObject effect;

		// Token: 0x0400048F RID: 1167
		private Transform linkTransform;

		// Token: 0x020000E4 RID: 228
		public class Vector
		{
			// Token: 0x060004DE RID: 1246 RVA: 0x00005049 File Offset: 0x00003249
			public static implicit operator Vector3(AnimatorEvent.Vector v)
			{
				return new Vector3(v.x, v.y, v.z);
			}

			// Token: 0x060004DF RID: 1247 RVA: 0x0003A264 File Offset: 0x00038464
			public static implicit operator AnimatorEvent.Vector(Vector3 v)
			{
				return new AnimatorEvent.Vector
				{
					x = v.x,
					y = v.y,
					z = v.z
				};
			}

			// Token: 0x04000490 RID: 1168
			public float x;

			// Token: 0x04000491 RID: 1169
			public float y;

			// Token: 0x04000492 RID: 1170
			public float z;
		}
	}
}
