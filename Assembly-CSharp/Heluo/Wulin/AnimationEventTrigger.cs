using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JsonFx.Json;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020000D1 RID: 209
	[RequireComponent(typeof(Animation))]
	public class AnimationEventTrigger : MonoBehaviour
	{
		// Token: 0x06000456 RID: 1110 RVA: 0x00004F01 File Offset: 0x00003101
		private void Awake()
		{
			this.fPos = 0f;
			this.fFrame = 0f;
			this.myAMO = null;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0003595C File Offset: 0x00033B5C
		private void Start()
		{
			if (this.PlaySkillID != 0)
			{
				int playSkillID = this.PlaySkillID;
				this.PlaySkillID = 0;
				this.PlaySkill(playSkillID, this.myAckInstList);
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00035990 File Offset: 0x00033B90
		private IEnumerator TestAnimation(float delay, string name)
		{
			yield return new WaitForSeconds(delay);
			base.animation.Play(name);
			yield break;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000359C8 File Offset: 0x00033BC8
		private void Update()
		{
			if (this.myAMO != null)
			{
				this.fPos += Time.deltaTime;
				float num = this.myAMO.fMoveStartFrame / this.fFrame;
				float num2 = this.myAMO.fMoveEndFrame / this.fFrame;
				float num3 = this.myAMO.fBackStartFrame / this.fFrame;
				float num4 = this.myAMO.fBackEndFrame / this.fFrame;
				if (this.fPos > num4)
				{
					if (this.myAMO.bTarget && this.goTarget != null)
					{
						this.goTarget.transform.position = this.vStartPos;
					}
					else
					{
						base.gameObject.transform.parent.position = this.vStartPos;
					}
					this.myAMO = null;
				}
				else if (this.fPos <= num4 && this.fPos > num3)
				{
					float num5 = (this.myAMO.fBackEndFrame - this.myAMO.fBackStartFrame) / this.fFrame;
					float num6 = this.fPos - num3;
					float num7 = num6 / num5;
					if (this.myAMO.bTarget && this.goTarget != null)
					{
						this.goTarget.transform.position = Vector3.Lerp(this.vEndPos, this.vStartPos, num7);
					}
					else
					{
						base.gameObject.transform.parent.position = Vector3.Lerp(this.vEndPos, this.vStartPos, num7);
					}
				}
				else if (this.fPos <= num3 && this.fPos > num2)
				{
					if (this.myAMO.bTarget && this.goTarget != null)
					{
						this.goTarget.transform.position = this.vEndPos;
					}
					else
					{
						base.gameObject.transform.parent.position = this.vEndPos;
					}
				}
				else if (this.fPos <= num2 && this.fPos > num)
				{
					float num8 = (this.myAMO.fMoveEndFrame - this.myAMO.fMoveStartFrame) / this.fFrame;
					float num9 = this.fPos - num;
					float num10 = num9 / num8;
					if (this.myAMO.bTarget && this.goTarget != null)
					{
						this.goTarget.transform.position = Vector3.Lerp(this.vStartPos, this.vEndPos, num10);
					}
					else
					{
						base.gameObject.transform.parent.position = Vector3.Lerp(this.vStartPos, this.vEndPos, num10);
					}
				}
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00035C9C File Offset: 0x00033E9C
		private bool EffectTextContains(string SkillName)
		{
			string text = string.Concat(new string[]
			{
				Game.g_strDataPathToApplicationPath,
				"Mods/",
				GameGlobal.m_strVersion,
				"/Config/EffectText/",
				SkillName,
				".txt"
			});
			return File.Exists(text) || Game.g_EffectText.Contains(SkillName);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00035CFC File Offset: 0x00033EFC
		private string EffectTextLoad(string SkillName)
		{
			string text = string.Concat(new string[]
			{
				Game.g_strDataPathToApplicationPath,
				"Mods/",
				GameGlobal.m_strVersion,
				"/Config/EffectText/",
				SkillName,
				".txt"
			});
			if (File.Exists(text))
			{
				try
				{
					Stream stream = File.OpenRead(text);
					StreamReader streamReader;
					try
					{
						streamReader = new StreamReader(stream, Encoding.Unicode);
					}
					catch (Exception ex)
					{
						Debug.LogError(SkillName + "Exception : " + ex.Message);
						return null;
					}
					string result = string.Empty;
					result = streamReader.ReadToEnd();
					if (GameGlobal.m_iModFixFileCount <= 0)
					{
						Debug.Log("Mod active");
						GameGlobal.m_iModFixFileCount++;
					}
					return result;
				}
				catch
				{
					Debug.LogError("MovieEventCube 散檔讀取失敗 !! ( " + text + " )");
					return null;
				}
			}
			TextAsset textAsset = Game.g_EffectText.Load(SkillName) as TextAsset;
			if (textAsset)
			{
				return textAsset.text;
			}
			Debug.LogError("包檔讀取失敗 !! ( " + SkillName + " )");
			return null;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00035E44 File Offset: 0x00034044
		private bool LoadHurtSkillNo(int SkillID)
		{
			if (this.hurt_eventGroup.ContainsKey(SkillID))
			{
				return true;
			}
			JsonReader jsonReader;
			if (Game.instance != null)
			{
				if (!this.EffectTextContains(SkillID.ToString() + "_Hurt"))
				{
					return false;
				}
				string text = this.EffectTextLoad(SkillID.ToString() + "_Hurt");
				jsonReader = new JsonReader(text);
			}
			else
			{
				string text2 = Application.dataPath + "/Config/effect/" + SkillID.ToString() + "_Hurt.sk";
				if (!File.Exists(text2))
				{
					return false;
				}
				StreamReader streamReader = new StreamReader(text2);
				jsonReader = new JsonReader(streamReader);
				streamReader.Close();
			}
			AnimatorEventGroup animatorEventGroup = jsonReader.Deserialize<AnimatorEventGroup>();
			animatorEventGroup.Load(base.transform, null);
			this.hurt_eventGroup.Add(SkillID, animatorEventGroup);
			return true;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00035F18 File Offset: 0x00034118
		private bool LoadSkillNo(int SkillID)
		{
			JsonReader jsonReader;
			if (Game.instance != null)
			{
				if (!this.EffectTextContains(SkillID.ToString() + "_Attack"))
				{
					return false;
				}
				string text = this.EffectTextLoad(SkillID.ToString() + "_Attack");
				jsonReader = new JsonReader(text);
			}
			else
			{
				string text2 = Application.dataPath + "/Config/effect/" + SkillID.ToString() + "_Attack.sk";
				if (!File.Exists(text2))
				{
					Debug.Log("檔案不存在 " + text2);
					return false;
				}
				StreamReader streamReader = new StreamReader(text2);
				jsonReader = new JsonReader(streamReader);
				streamReader.Close();
			}
			AnimatorEventGroup animatorEventGroup = jsonReader.Deserialize<AnimatorEventGroup>();
			string text3 = animatorEventGroup.GetSkillName();
			AnimationClip animationClip = base.animation.GetClip(text3);
			if (animationClip == null)
			{
				animationClip = (Game.g_ModelBundle.Load(text3) as AnimationClip);
				if (animationClip == null)
				{
					text3 = base.name + "@" + animatorEventGroup.animationName;
					animationClip = (Game.g_ModelBundle.Load(text3) as AnimationClip);
					if (animationClip == null)
					{
						Debug.LogError("沒有 " + text3 + " 模型與動作 如果看到這就是出大事了 請找小高 ");
						return false;
					}
					animatorEventGroup.modelname = base.name;
					base.animation.AddClip(animationClip, text3);
				}
				else
				{
					base.animation.AddClip(animationClip, text3);
				}
			}
			animatorEventGroup.Load(base.transform, animationClip);
			this.eventGroup.Add(SkillID, animatorEventGroup);
			if (this.hurt_eventGroup.ContainsKey(SkillID))
			{
				return true;
			}
			if (Game.instance != null)
			{
				if (!this.EffectTextContains(SkillID.ToString() + "_Hurt"))
				{
					return false;
				}
				string text4 = this.EffectTextLoad(SkillID.ToString() + "_Hurt");
				jsonReader = new JsonReader(text4);
			}
			else
			{
				string text2 = Application.dataPath + "/Config/effect/" + SkillID.ToString() + "_Hurt.sk";
				if (!File.Exists(text2))
				{
					Debug.Log("檔案不存在 " + text2);
					return false;
				}
				StreamReader streamReader = new StreamReader(text2);
				jsonReader = new JsonReader(streamReader);
				streamReader.Close();
			}
			animatorEventGroup = jsonReader.Deserialize<AnimatorEventGroup>();
			animatorEventGroup.Load(base.transform, animationClip);
			this.hurt_eventGroup.Add(SkillID, animatorEventGroup);
			return true;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0003618C File Offset: 0x0003438C
		public float PlayNoEffect(List<AttackInstance> atkInstList)
		{
			float num = 0f;
			if (atkInstList.Count > 0)
			{
				num = atkInstList[0].srcUnit.animationTB.PlayMeleeAttack();
			}
			base.StartCoroutine(this.PlayHurtsNoEffect(num, atkInstList));
			foreach (AttackInstance attackInstance in atkInstList)
			{
				attackInstance.animationLength = num;
				attackInstance.lastEmitHitTime = num;
				attackInstance.totalTarget = atkInstList.Count;
				base.StartCoroutine(this.HitTarget(attackInstance, num));
			}
			return num;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00004F20 File Offset: 0x00003120
		public float PlayMovieSkill(int SkillID, List<AttackInstance> atkInstList)
		{
			this.bMovieAnimation = true;
			return this.PlaySkill(SkillID, atkInstList);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0003623C File Offset: 0x0003443C
		public void PlayUseTactic(UnitTB unit)
		{
			int num = 100097002;
			if (!this.hurt_eventGroup.ContainsKey(num) && !this.LoadHurtSkillNo(num))
			{
				if (!this.hurt_eventGroup.ContainsKey(100097002))
				{
					bool flag = this.LoadHurtSkillNo(100097002);
					if (flag)
					{
						num = 100097002;
					}
				}
				else
				{
					num = 100097002;
				}
			}
			base.StartCoroutine(this.PlayTactic(0.2f, num, unit));
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000362BC File Offset: 0x000344BC
		public void PlayUseSchedule(UnitTB unit, int skillid)
		{
			int num = skillid;
			if (!this.hurt_eventGroup.ContainsKey(num) && !this.LoadHurtSkillNo(num))
			{
				if (!this.hurt_eventGroup.ContainsKey(100097002))
				{
					bool flag = this.LoadHurtSkillNo(100097002);
					if (flag)
					{
						num = 100097002;
					}
				}
				else
				{
					num = 100097002;
				}
			}
			base.StartCoroutine(this.PlaySchedule(0.2f, num, unit));
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00036338 File Offset: 0x00034538
		public float PlayUseItemSelf(List<AttackInstance> atkInstList)
		{
			foreach (AttackInstance attackInstance in atkInstList)
			{
				int num = attackInstance.unitAbility.skillID;
				if (!this.hurt_eventGroup.ContainsKey(num) && !this.LoadHurtSkillNo(num))
				{
					if (!this.hurt_eventGroup.ContainsKey(100097002))
					{
						bool flag = this.LoadHurtSkillNo(100097002);
						if (!flag)
						{
							continue;
						}
						num = 100097002;
					}
					else
					{
						num = 100097002;
					}
				}
				base.StartCoroutine(this.PlayHurts(0.2f, num, atkInstList));
				attackInstance.animationLength = 0.5f;
				attackInstance.lastEmitHitTime = 0.5f;
				attackInstance.totalTarget = atkInstList.Count;
				base.StartCoroutine(this.HitTarget(attackInstance, 0.5f));
			}
			return 0.5f;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00036444 File Offset: 0x00034644
		public float PlaySkill(int SkillID, List<AttackInstance> atkInstList)
		{
			if (!this.eventGroup.ContainsKey(SkillID) && !this.LoadSkillNo(SkillID))
			{
				return this.PlayNoEffect(atkInstList);
			}
			string text = this.eventGroup[SkillID].modelname + "@" + this.eventGroup[SkillID].animationName;
			base.animation.animation[text].layer = 1;
			base.animation.animation[text].wrapMode = 1;
			base.animation.animation[text].speed = 1f;
			if (BattleControl.instance != null && atkInstList.Count > 0)
			{
				this.myAMO = BattleControl.instance.m_aniMoveOffsetManager.GetMoveOffest(this.eventGroup[SkillID].modelname, this.eventGroup[SkillID].animationName);
			}
			else
			{
				this.myAMO = null;
			}
			if (this.myAMO != null)
			{
				if (this.myAMO.bTarget)
				{
					if (atkInstList[0].missed)
					{
						this.myAMO = null;
					}
					else
					{
						this.fPos = 0f;
						AnimationClip clip = base.animation.GetClip(text);
						if (clip != null)
						{
							this.fFrame = clip.frameRate;
							this.vStartPos = atkInstList[0].targetUnit.gameObject.transform.position;
							this.vEndPos = atkInstList[0].targetUnit.gameObject.transform.position + Vector3.up * this.myAMO.fBaseLength;
							if (atkInstList[0].protect)
							{
								this.goTarget = atkInstList[0].protectUnit.gameObject;
							}
							else
							{
								this.goTarget = atkInstList[0].targetUnit.gameObject;
							}
						}
					}
				}
				else
				{
					this.fPos = 0f;
					this.fFrame = base.animation.GetClip(text).frameRate;
					this.vStartPos = atkInstList[0].srcUnit.gameObject.transform.position;
					this.vEndPos = atkInstList[0].targetUnit.gameObject.transform.position;
					Vector3 vector = this.vEndPos - this.vStartPos;
					float num = Vector3.Distance(this.vStartPos, this.vEndPos);
					float num2 = num - this.myAMO.fBaseLength;
					this.vStartPos = atkInstList[0].srcUnit.gameObject.transform.position;
					this.vEndPos = this.vStartPos + vector.normalized * num2;
				}
			}
			base.animation.cullingType = 0;
			base.animation.CrossFade(text);
			this.CheckProtectFirstStep(atkInstList);
			float num3 = 0f;
			foreach (AnimatorEvent animatorEvent in this.eventGroup[SkillID].events)
			{
				if (animatorEvent.type == 4)
				{
					if (num3 < animatorEvent.fireTime)
					{
						num3 = animatorEvent.fireTime;
					}
					base.StartCoroutine(this.PlayHurts(animatorEvent.fireTime, SkillID, atkInstList));
				}
				else if (animatorEvent.type == 3 || animatorEvent.type == 5)
				{
					this.calcLastEmitTime(animatorEvent, atkInstList);
					base.StartCoroutine(this.PlayEmits(animatorEvent, SkillID, atkInstList));
				}
				else if (animatorEvent.type == 6)
				{
					this.calcLastEmitTime(animatorEvent, atkInstList);
					base.StartCoroutine(this.PlayEmitsLine(animatorEvent, SkillID, atkInstList));
				}
				else if (animatorEvent.type == 7)
				{
					base.StartCoroutine(this.PlayEventMulti(animatorEvent, atkInstList));
				}
				else
				{
					base.StartCoroutine(this.PlayEvent(animatorEvent, atkInstList));
				}
			}
			float num4 = 0f;
			num4 = base.animation.animation[text].length;
			foreach (AttackInstance attackInstance in atkInstList)
			{
				attackInstance.animationLength = num4;
				if (num3 == 0f && attackInstance.lastEmitHitTime == 0f)
				{
					attackInstance.lastEmitHitTime = num4;
				}
				else if (attackInstance.lastEmitHitTime < num3)
				{
					attackInstance.lastEmitHitTime = num3;
				}
				attackInstance.totalTarget = atkInstList.Count;
				base.StartCoroutine(this.HitTarget(attackInstance, attackInstance.lastEmitHitTime));
			}
			return num4;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00036938 File Offset: 0x00034B38
		private void calcLastEmitTime(AnimatorEvent e, List<AttackInstance> atkInstList)
		{
			foreach (AttackInstance attackInstance in atkInstList)
			{
				float num = 0f;
				if (e.type == 6)
				{
					num = e.lifetime;
				}
				float num2 = e.CalcEmit(base.transform, attackInstance.targetUnit);
				if (attackInstance.lastEmitHitTime < num2 + e.fireTime + num)
				{
					attackInstance.lastEmitHitTime = num2 + e.fireTime + num;
				}
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000369D8 File Offset: 0x00034BD8
		private void CheckProtectFirstStep(List<AttackInstance> atkInstList)
		{
			foreach (AttackInstance attackInstance in atkInstList)
			{
				if (attackInstance.protect)
				{
					base.StartCoroutine(this.PlayProtectFirstStep(attackInstance));
				}
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00036A44 File Offset: 0x00034C44
		private IEnumerator PlayProtectFirstStep(AttackInstance aInst)
		{
			UnitTB unitSource = aInst.srcUnit;
			UnitTB unitTarget = aInst.targetUnit;
			UnitTB unitProtect = aInst.protectUnit;
			unitTarget.animationTB.PlayProtectDodge();
			Quaternion wantedRot = Quaternion.LookRotation(unitTarget.occupiedTile.pos - unitProtect.occupiedTile.pos);
			float fovertime = 0f;
			for (;;)
			{
				float frot = Time.deltaTime * this.rotateSpeed;
				frot = Mathf.Clamp(frot, 0.05f, 1f);
				unitProtect.thisT.rotation = Quaternion.Slerp(unitProtect.thisT.rotation, wantedRot, frot);
				if (Quaternion.Angle(unitProtect.thisT.rotation, wantedRot) < 2f || fovertime > 0.5f)
				{
					break;
				}
				yield return null;
			}
			unitProtect.thisT.rotation = wantedRot;
			fovertime = 0f;
			for (;;)
			{
				fovertime += Time.deltaTime;
				float dist = Vector3.Distance(unitProtect.thisT.position, unitTarget.thisT.position);
				if (dist < 0.1f || dist < Time.deltaTime * 30f || fovertime > 0.5f)
				{
					break;
				}
				Vector3 vPosDiff = unitTarget.thisT.position - unitProtect.thisT.position;
				vPosDiff.Normalize();
				vPosDiff = vPosDiff * Time.deltaTime * 30f;
				unitProtect.thisT.position = unitProtect.thisT.position + vPosDiff;
				yield return null;
			}
			unitProtect.thisT.position = unitTarget.thisT.position;
			wantedRot = Quaternion.LookRotation(unitSource.occupiedTile.pos - unitTarget.occupiedTile.pos);
			fovertime = 0f;
			for (;;)
			{
				fovertime += Time.deltaTime;
				float frot2 = Time.deltaTime * this.rotateSpeed;
				frot2 = Mathf.Clamp(frot2, 0.05f, 1f);
				unitProtect.thisT.rotation = Quaternion.Slerp(unitProtect.thisT.rotation, wantedRot, frot2);
				if (Quaternion.Angle(unitProtect.thisT.rotation, wantedRot) < 2f || fovertime > 0.5f)
				{
					break;
				}
				yield return null;
			}
			unitProtect.thisT.rotation = wantedRot;
			yield break;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00036A70 File Offset: 0x00034C70
		private IEnumerator PlayProtectSecondStep(AttackInstance aInst, float DelayTime)
		{
			yield return new WaitForSeconds(DelayTime);
			UnitTB unitSource = aInst.srcUnit;
			UnitTB unitTarget = aInst.targetUnit;
			UnitTB unitProtect = aInst.protectUnit;
			unitTarget.animationTB.PlayIdle();
			if (unitProtect == null)
			{
				yield break;
			}
			if (unitProtect.IsDestroyed())
			{
				yield break;
			}
			Quaternion wantedRot = Quaternion.LookRotation(unitProtect.occupiedTile.pos - unitTarget.occupiedTile.pos);
			float fovertime = 0f;
			for (;;)
			{
				fovertime += Time.deltaTime;
				float frot = Time.deltaTime * this.rotateSpeed;
				frot = Mathf.Clamp(frot, 0.05f, 1f);
				unitProtect.thisT.rotation = Quaternion.Slerp(unitProtect.thisT.rotation, wantedRot, frot);
				if (Quaternion.Angle(unitProtect.thisT.rotation, wantedRot) < 2f || fovertime > 0.5f)
				{
					break;
				}
				yield return null;
			}
			unitProtect.thisT.rotation = wantedRot;
			fovertime = 0f;
			for (;;)
			{
				fovertime += Time.deltaTime;
				float dist = Vector3.Distance(unitProtect.thisT.position, unitProtect.occupiedTile.pos);
				if (dist < 0.1f || dist < Time.deltaTime * 30f || fovertime > 0.5f)
				{
					break;
				}
				Vector3 vPosDiff = unitProtect.occupiedTile.pos - unitProtect.thisT.position;
				vPosDiff.Normalize();
				vPosDiff = vPosDiff * Time.deltaTime * 30f;
				unitProtect.thisT.position = unitProtect.thisT.position + vPosDiff;
				yield return null;
			}
			unitProtect.thisT.position = unitProtect.occupiedTile.pos;
			wantedRot = Quaternion.LookRotation(unitSource.occupiedTile.pos - unitProtect.occupiedTile.pos);
			fovertime = 0f;
			for (;;)
			{
				fovertime += Time.deltaTime;
				float frot2 = Time.deltaTime * this.rotateSpeed;
				frot2 = Mathf.Clamp(frot2, 0.05f, 1f);
				unitProtect.thisT.rotation = Quaternion.Slerp(unitProtect.thisT.rotation, wantedRot, frot2);
				if (Quaternion.Angle(unitProtect.thisT.rotation, wantedRot) < 2f || fovertime > 0.5f)
				{
					break;
				}
				yield return null;
			}
			unitProtect.thisT.rotation = wantedRot;
			unitProtect.bSpeicalAction = false;
			yield break;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00036AA8 File Offset: 0x00034CA8
		private IEnumerator PlayMovePostion(AttackInstance aInst, float speed)
		{
			float ftime = aInst.targetUnit.animationTB.fLastHitLength;
			Tile tile = aInst.moveToTile;
			float fovertime = 0f;
			for (;;)
			{
				fovertime += Time.deltaTime;
				float dist = Vector3.Distance(aInst.targetUnit.thisT.position, tile.pos);
				if (dist < 0.1f || fovertime > 2f)
				{
					break;
				}
				ftime -= Time.deltaTime;
				aInst.targetUnit.thisT.position = Vector3.Lerp(aInst.targetUnit.thisT.position, tile.pos, Time.deltaTime * speed);
				yield return null;
			}
			aInst.targetUnit.ResetTilePostion(tile);
			if (ftime > 0f)
			{
				yield return new WaitForSeconds(ftime);
			}
			tile.bUnitOrder = false;
			aInst.targetUnit.bSpeicalAction = false;
			yield break;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00036AD8 File Offset: 0x00034CD8
		private void _HitTargetApplyCondition(AttackInstance aInst, string strName, List<int> conditionIDList)
		{
			for (int i = 0; i < conditionIDList.Count; i++)
			{
				int id = conditionIDList[i];
				ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(id);
				if (conditionNode != null)
				{
					if (conditionNode.m_iCondType == _ConditionType.Buff || conditionNode.m_iCondType == _ConditionType.Debuff)
					{
						ConditionNode conditionNode2 = conditionNode.Clone();
						foreach (EffectPart effectPart in conditionNode2.m_effectPartList)
						{
							effectPart.m_iValueBase = aInst.damageDone;
						}
						if (conditionNode2.GetEffectPartAbsoluteDebuff(_EffectPartType.Taunt) || conditionNode2.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee))
						{
							conditionNode2.m_iTargetUnitID = aInst.srcUnit.iUnitID;
						}
						if (conditionNode2.m_iCondTarget == 0)
						{
							if (aInst.srcUnit.HP > 0)
							{
								if (aInst.unitAbility.bItemSkill)
								{
									aInst.srcUnit.ApplyCondition(conditionNode2, false);
								}
								else if (conditionNode.m_iCondType == _ConditionType.Buff)
								{
									aInst.srcUnit.ApplyCondition(conditionNode2, false);
								}
							}
						}
						else if (conditionNode2.m_iCondTarget == 1)
						{
							if (aInst.protect)
							{
								if (aInst.protectUnit.HP > 0)
								{
									if (aInst.unitAbility.bItemSkill)
									{
										aInst.protectUnit.ApplyCondition(conditionNode2, false);
									}
									else if (conditionNode.m_iCondType != _ConditionType.Debuff || !aInst.srcUnit.CheckFriendFaction(aInst.protectUnit.factionID))
									{
										aInst.protectUnit.ApplyCondition(conditionNode2, false);
									}
								}
							}
							else if (aInst.targetUnit.HP > 0)
							{
								if (aInst.unitAbility.bItemSkill)
								{
									aInst.targetUnit.ApplyCondition(conditionNode2, false);
								}
								else if (conditionNode.m_iCondType != _ConditionType.Debuff || !aInst.srcUnit.CheckFriendFaction(aInst.targetUnit.factionID))
								{
									aInst.targetUnit.ApplyCondition(conditionNode2, false);
								}
							}
						}
						else
						{
							if (aInst.srcUnit.HP > 0)
							{
								if (aInst.unitAbility.bItemSkill)
								{
									aInst.srcUnit.ApplyCondition(conditionNode2, false);
								}
								else if (conditionNode.m_iCondType == _ConditionType.Buff)
								{
									aInst.srcUnit.ApplyCondition(conditionNode2, false);
								}
							}
							if (aInst.protect)
							{
								if (aInst.protectUnit.HP > 0)
								{
									if (aInst.unitAbility.bItemSkill)
									{
										aInst.protectUnit.ApplyCondition(conditionNode2, false);
									}
									else if (conditionNode.m_iCondType != _ConditionType.Debuff || !aInst.srcUnit.CheckFriendFaction(aInst.protectUnit.factionID))
									{
										aInst.protectUnit.ApplyCondition(conditionNode2, false);
									}
								}
							}
							else if (aInst.targetUnit.HP > 0)
							{
								if (aInst.unitAbility.bItemSkill)
								{
									aInst.targetUnit.ApplyCondition(conditionNode2, false);
								}
								else if (conditionNode.m_iCondType != _ConditionType.Debuff || !aInst.srcUnit.CheckFriendFaction(aInst.targetUnit.factionID))
								{
									aInst.targetUnit.ApplyCondition(conditionNode2, false);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00036E4C File Offset: 0x0003504C
		private IEnumerator HitTarget(AttackInstance aInst, float DelayTime)
		{
			yield return new WaitForSeconds(DelayTime);
			if ((aInst.critical || aInst.knockback || aInst.pullclose) && !aInst.heal)
			{
				if (aInst.critical)
				{
					if (aInst.protect)
					{
						aInst.protectUnit.animationTB.PlayCritical();
					}
					else
					{
						aInst.targetUnit.animationTB.PlayCritical();
					}
				}
				else if (aInst.knockback)
				{
					aInst.targetUnit.animationTB.PlayCritical();
				}
				else
				{
					aInst.targetUnit.animationTB.PlayHit();
				}
			}
			if (!this.bMovieAnimation)
			{
				if (aInst.protect)
				{
					yield return new WaitForSeconds(0.1f);
					aInst.srcUnit.HitTarget(aInst.protectUnit, aInst);
				}
				else
				{
					yield return new WaitForSeconds(0.1f);
					aInst.srcUnit.HitTarget(aInst.targetUnit, aInst);
				}
				if (!aInst.missed)
				{
					if (aInst.unitAbility.effectType == _EffectType.Buff || aInst.unitAbility.effectType == _EffectType.Debuff)
					{
						int index = Random.Range(0, aInst.unitAbility.chainedAbilityIDList.Count);
						ConditionNode conditionNodeOrig = Game.g_BattleControl.m_battleAbility.GetConditionNode(aInst.unitAbility.chainedAbilityIDList[index]);
						if (conditionNodeOrig != null)
						{
							ConditionNode conditionNode = conditionNodeOrig.Clone();
							foreach (EffectPart part in conditionNode.m_effectPartList)
							{
								part.m_iValueBase = aInst.damageDone;
							}
							if (conditionNode.GetEffectPartAbsoluteDebuff(_EffectPartType.Taunt) || conditionNode.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee))
							{
								conditionNode.m_iTargetUnitID = aInst.srcUnit.iUnitID;
							}
							aInst.targetUnit.ApplyCondition(conditionNode, false);
						}
					}
					else
					{
						List<int> conditionIDList = new List<int>();
						if (aInst.shock && aInst.unitAbility.effectType == _EffectType.Damage)
						{
							conditionIDList.Add(400007);
						}
						conditionIDList.AddRange(aInst.unitAbility.chainedAbilityIDList.ToArray());
						this._HitTargetApplyCondition(aInst, aInst.unitAbility.name, conditionIDList);
					}
				}
			}
			else if (aInst.destroyed)
			{
				yield return new WaitForSeconds(0.1f);
				aInst.targetUnit.animationTB.PlayDestroyed();
			}
			if (aInst.protect)
			{
				base.StartCoroutine(this.PlayProtectSecondStep(aInst, aInst.protectUnit.animationTB.fLastHitLength));
			}
			if (aInst.knockback)
			{
				base.StartCoroutine(this.PlayMovePostion(aInst, 10f));
			}
			if (aInst.pullclose)
			{
				base.StartCoroutine(this.PlayMovePostion(aInst, 8f));
			}
			if (CameraControl.instance != null && CameraControl.instance.trackTile && CameraControl.instance.trackNowTile == aInst.targetUnit.occupiedTile)
			{
				yield return new WaitForSeconds(1f);
				CameraControl.instance.trackNowTile = null;
				CameraControl.instance.trackTile = false;
			}
			yield break;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00036E84 File Offset: 0x00035084
		private IEnumerator PlayEventMulti(AnimatorEvent e, List<AttackInstance> atkInstList)
		{
			yield return new WaitForSeconds(e.fireTime);
			e.PlayEventAudio(base.transform);
			foreach (AttackInstance aInst in atkInstList)
			{
				e.PlayEventTarget(base.transform, aInst.targetUnit);
			}
			yield break;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00036EBC File Offset: 0x000350BC
		private IEnumerator PlayEvent(AnimatorEvent e, List<AttackInstance> atkInstList)
		{
			yield return new WaitForSeconds(e.fireTime);
			UnitTB unit = null;
			if (atkInstList.Count > 0)
			{
				unit = atkInstList[0].targetUnit;
			}
			e.PlayEvent(base.transform, unit);
			yield break;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00036EF4 File Offset: 0x000350F4
		private IEnumerator PlayEmits(AnimatorEvent e, int skillID, List<AttackInstance> atkInstList)
		{
			yield return new WaitForSeconds(e.fireTime);
			foreach (AttackInstance aInst in atkInstList)
			{
				float delayTime = e.PlayEmit(base.transform, aInst.targetUnit);
				base.StartCoroutine(this.PlayHurt(delayTime, skillID, aInst));
			}
			yield break;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00036F3C File Offset: 0x0003513C
		private IEnumerator PlayEmitsLine(AnimatorEvent e, int skillID, List<AttackInstance> atkInstList)
		{
			yield return new WaitForSeconds(e.fireTime);
			bool bfire = false;
			float delayTime = 0f;
			Vector3 vPos = new Vector3(0f, 0f, 0f);
			foreach (AttackInstance aInst in atkInstList)
			{
				delayTime = 0f;
				if (!bfire)
				{
					if (!(aInst.targetUnit != null))
					{
						continue;
					}
					vPos = e.PlayEmitLine(base.transform, aInst.targetUnit);
					bfire = true;
				}
				if (bfire)
				{
					float dist = Vector3.Distance(vPos, aInst.targetUnit.transform.position);
					if (e.flySpeed > 0f)
					{
						delayTime = dist / e.flySpeed;
					}
					else
					{
						delayTime = 0.5f;
					}
					base.StartCoroutine(this.PlayHurt(delayTime, skillID, aInst));
				}
			}
			yield break;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00036F84 File Offset: 0x00035184
		private IEnumerator PlayHurts(float delayTime, int skillID, List<AttackInstance> atkInstList)
		{
			yield return new WaitForSeconds(delayTime);
			foreach (AttackInstance aInst in atkInstList)
			{
				if (aInst.srcUnit != null && aInst.srcUnit.occupiedTile != null && aInst.targetUnit != null && aInst.targetUnit.occupiedTile != null && GridManager.Distance(aInst.srcUnit.occupiedTile, aInst.targetUnit.occupiedTile) > 4 && CameraControl.instance != null && !CameraControl.instance.trackTile)
				{
					CameraControl.instance.trackNowTile = aInst.targetUnit.occupiedTile;
					CameraControl.instance.trackTile = true;
				}
				if (aInst.missed)
				{
					if (aInst.targetUnit != null && !aInst.targetUnit.IsDestroyed())
					{
						aInst.targetUnit.animationTB.PlayDodge();
					}
				}
				else
				{
					if (aInst.heal)
					{
						if (aInst.targetUnit != null && !aInst.targetUnit.IsDestroyed())
						{
							aInst.targetUnit.animationTB.PlayStand();
							aInst.targetUnit.audioTB.PlayHit();
						}
					}
					else if (aInst.protect)
					{
						if (aInst.protectUnit != null && !aInst.protectUnit.IsDestroyed())
						{
							aInst.protectUnit.animationTB.PlayHit();
							aInst.protectUnit.audioTB.PlayHit();
						}
					}
					else if (aInst.targetUnit != null && !aInst.targetUnit.IsDestroyed())
					{
						aInst.targetUnit.animationTB.PlayHit();
						aInst.targetUnit.audioTB.PlayHit();
					}
					if (this.hurt_eventGroup.ContainsKey(skillID))
					{
						foreach (AnimatorEvent e in this.hurt_eventGroup[skillID].events)
						{
							if (aInst.targetUnit != null)
							{
								e.PlayEvent(aInst.targetUnit.transform, null);
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00036FCC File Offset: 0x000351CC
		private IEnumerator PlayHurtsNoEffect(float delayTime, List<AttackInstance> atkInstList)
		{
			yield return new WaitForSeconds(delayTime);
			foreach (AttackInstance aInst in atkInstList)
			{
				if (aInst.missed)
				{
					if (aInst.targetUnit != null && !aInst.targetUnit.IsDestroyed())
					{
						aInst.targetUnit.animationTB.PlayDodge();
					}
				}
				else if (aInst.heal)
				{
					if (aInst.targetUnit != null && !aInst.targetUnit.IsDestroyed())
					{
						aInst.targetUnit.animationTB.PlayStand();
						aInst.targetUnit.audioTB.PlayHit();
					}
				}
				else if (aInst.protect)
				{
					if (aInst.protectUnit != null && !aInst.protectUnit.IsDestroyed())
					{
						aInst.protectUnit.animationTB.PlayHit();
						aInst.protectUnit.audioTB.PlayHit();
					}
				}
				else if (aInst.targetUnit != null && !aInst.targetUnit.IsDestroyed())
				{
					aInst.targetUnit.animationTB.PlayHit();
					aInst.targetUnit.audioTB.PlayHit();
				}
			}
			yield break;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00036FFC File Offset: 0x000351FC
		private IEnumerator PlayHurt(float delayTime, int skillID, AttackInstance aInst)
		{
			yield return new WaitForSeconds(delayTime);
			if (aInst.missed)
			{
				if (aInst.targetUnit != null && !aInst.targetUnit.IsDestroyed())
				{
					aInst.targetUnit.animationTB.PlayDodge();
				}
			}
			else
			{
				if (aInst.heal)
				{
					if (aInst.targetUnit != null && !aInst.targetUnit.IsDestroyed())
					{
						aInst.targetUnit.animationTB.PlayStand();
						aInst.targetUnit.audioTB.PlayHit();
					}
				}
				else if (aInst.protect)
				{
					if (aInst.protectUnit != null && !aInst.protectUnit.IsDestroyed())
					{
						aInst.protectUnit.animationTB.PlayHit();
						aInst.protectUnit.audioTB.PlayHit();
					}
				}
				else if (aInst.targetUnit != null && !aInst.targetUnit.IsDestroyed())
				{
					aInst.targetUnit.animationTB.PlayHit();
					aInst.targetUnit.audioTB.PlayHit();
				}
				if (this.hurt_eventGroup.ContainsKey(skillID))
				{
					foreach (AnimatorEvent e in this.hurt_eventGroup[skillID].events)
					{
						if (aInst.targetUnit != null)
						{
							e.PlayEvent(aInst.targetUnit.transform, null);
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00037044 File Offset: 0x00035244
		private IEnumerator PlayTactic(float delayTime, int skillID, UnitTB unit)
		{
			yield return new WaitForSeconds(delayTime);
			unit.animationTB.PlayStand();
			if (this.hurt_eventGroup.ContainsKey(skillID))
			{
				foreach (AnimatorEvent e in this.hurt_eventGroup[skillID].events)
				{
					if (unit != null)
					{
						e.PlayEvent(unit.transform, null);
					}
				}
			}
			yield break;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0003708C File Offset: 0x0003528C
		private IEnumerator PlaySchedule(float delayTime, int skillID, UnitTB unit)
		{
			yield return new WaitForSeconds(delayTime);
			if (this.hurt_eventGroup.ContainsKey(skillID))
			{
				foreach (AnimatorEvent e in this.hurt_eventGroup[skillID].events)
				{
					if (unit != null)
					{
						e.PlayEvent(unit.transform, null);
					}
				}
			}
			yield break;
		}

		// Token: 0x040003C6 RID: 966
		public int PlaySkillID;

		// Token: 0x040003C7 RID: 967
		private Dictionary<int, AnimatorEventGroup> eventGroup = new Dictionary<int, AnimatorEventGroup>();

		// Token: 0x040003C8 RID: 968
		private Dictionary<int, AnimatorEventGroup> hurt_eventGroup = new Dictionary<int, AnimatorEventGroup>();

		// Token: 0x040003C9 RID: 969
		private float fPos;

		// Token: 0x040003CA RID: 970
		private float fFrame;

		// Token: 0x040003CB RID: 971
		private Vector3 vStartPos;

		// Token: 0x040003CC RID: 972
		private Vector3 vEndPos;

		// Token: 0x040003CD RID: 973
		private AnimationMoveOffest myAMO;

		// Token: 0x040003CE RID: 974
		private List<AttackInstance> myAckInstList = new List<AttackInstance>();

		// Token: 0x040003CF RID: 975
		private GameObject goTarget;

		// Token: 0x040003D0 RID: 976
		private bool bMovieAnimation;

		// Token: 0x040003D1 RID: 977
		private float rotateSpeed = 20f;
	}
}
