using System;
using UnityEngine;

// Token: 0x02000839 RID: 2105
public class TOD_Time : MonoBehaviour
{
	// Token: 0x0600334B RID: 13131 RVA: 0x0018CA40 File Offset: 0x0018AC40
	private void CalculateLinearTangents(Keyframe[] keys)
	{
		for (int i = 0; i < keys.Length; i++)
		{
			Keyframe keyframe = keys[i];
			if (i > 0)
			{
				Keyframe keyframe2 = keys[i - 1];
				keyframe.inTangent = (keyframe.value - keyframe2.value) / (keyframe.time - keyframe2.time);
			}
			if (i < keys.Length - 1)
			{
				Keyframe keyframe3 = keys[i + 1];
				keyframe.outTangent = (keyframe3.value - keyframe.value) / (keyframe3.time - keyframe.time);
			}
			keys[i] = keyframe;
		}
	}

	// Token: 0x0600334C RID: 13132 RVA: 0x0018CAF8 File Offset: 0x0018ACF8
	private void ApproximateCurve(AnimationCurve source, out Keyframe[] resKeys, out Keyframe[] resKeysInverse)
	{
		resKeys = new Keyframe[25];
		resKeysInverse = new Keyframe[25];
		float num = -0.01f;
		for (int i = 0; i < 25; i++)
		{
			num = Mathf.Max(num + 0.01f, source.Evaluate((float)i));
			resKeys[i] = new Keyframe((float)i, num);
			resKeysInverse[i] = new Keyframe(num, (float)i);
		}
	}

	// Token: 0x0600334D RID: 13133 RVA: 0x0018CB70 File Offset: 0x0018AD70
	public void ApplyTimeCurve()
	{
		this.TimeCurve.preWrapMode = 1;
		this.TimeCurve.postWrapMode = 1;
		Keyframe[] array;
		Keyframe[] array2;
		this.ApproximateCurve(this.TimeCurve, out array, out array2);
		this.CalculateLinearTangents(array);
		this.CalculateLinearTangents(array2);
		this.timeCurve = new AnimationCurve(array);
		this.timeCurve.preWrapMode = 2;
		this.timeCurve.postWrapMode = 2;
		this.timeCurveInverse = new AnimationCurve(array2);
		this.timeCurveInverse.preWrapMode = 2;
		this.timeCurveInverse.postWrapMode = 2;
	}

	// Token: 0x0600334E RID: 13134 RVA: 0x0018CBFC File Offset: 0x0018ADFC
	public void AddTime(float delta, bool adjust = false)
	{
		if (this.UseTimeCurve && adjust)
		{
			float num = delta + this.timeCurveInverse.Evaluate(this.skyTime);
			delta = this.timeCurve.Evaluate(num) - this.skyTime;
			if (this.ProgressDate)
			{
				if (num >= 24f)
				{
					delta += (float)((int)num / 24 * 24);
				}
				else if (num < 0f)
				{
					delta += (float)(((int)num / 24 - 1) * 24);
				}
			}
		}
		if (this.skyTime != this.sky.Cycle.Hour)
		{
			delta += this.sky.Cycle.Hour - this.skyTime;
			this.sky.Cycle.Hour = this.skyTime;
		}
		if (this.ProgressDate)
		{
			this.sky.Cycle.Hour += delta;
			if (this.sky.Cycle.Hour >= 24f)
			{
				int num2 = (int)this.sky.Cycle.Hour / 24;
				this.sky.Cycle.Hour -= (float)(num2 * 24);
				this.sky.Cycle.DateTime = this.sky.Cycle.DateTime.AddDays((double)num2);
			}
		}
		else
		{
			this.sky.Cycle.Hour += delta;
			this.CheckTimeRange();
		}
		this.skyTime = this.sky.Cycle.Hour;
	}

	// Token: 0x0600334F RID: 13135 RVA: 0x000202B0 File Offset: 0x0001E4B0
	public void AddMoon(float delta)
	{
		this.sky.Moon.Phase += delta;
		this.CheckMoonRange();
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x0018CDA4 File Offset: 0x0018AFA4
	public void CheckTimeRange()
	{
		this.sky.Cycle.Year = Mathf.Clamp(this.sky.Cycle.Year, 1, 9999);
		this.sky.Cycle.Month = Mathf.Clamp(this.sky.Cycle.Month, 1, 12);
		this.sky.Cycle.Day = Mathf.Clamp(this.sky.Cycle.Day, 1, DateTime.DaysInMonth(this.sky.Cycle.Year, this.sky.Cycle.Month));
		this.sky.Cycle.Hour = Mathf.Repeat(this.sky.Cycle.Hour, 24f);
	}

	// Token: 0x06003351 RID: 13137 RVA: 0x0018CE7C File Offset: 0x0018B07C
	public void CheckMoonRange()
	{
		if (this.sky.Moon.Phase > 1f)
		{
			this.sky.Moon.Phase -= (float)((int)this.sky.Moon.Phase + 1);
		}
		else if (this.sky.Moon.Phase < -1f)
		{
			this.sky.Moon.Phase -= (float)((int)this.sky.Moon.Phase - 1);
		}
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x000202D0 File Offset: 0x0001E4D0
	protected void Awake()
	{
		this.ProgressTime = false;
		this.ProgressDate = false;
		this.sky = base.GetComponent<TOD_Sky>();
		this.ApplyTimeCurve();
	}

	// Token: 0x06003353 RID: 13139 RVA: 0x000202F2 File Offset: 0x0001E4F2
	protected void OnEnable()
	{
		this.skyTime = this.sky.Cycle.Hour;
	}

	// Token: 0x06003354 RID: 13140 RVA: 0x0018CF18 File Offset: 0x0018B118
	protected void Update()
	{
		if (this.m_bInside || this.m_bInDevelop)
		{
			return;
		}
		if (GameGlobal.m_bMovie)
		{
			return;
		}
		if (GameGlobal.m_bPlayerTalk)
		{
			return;
		}
		if (GameGlobal.m_bCFormOpen)
		{
			return;
		}
		if (GameGlobal.m_bPlayingSmallGame)
		{
			return;
		}
		if (GameGlobal.m_bBattle)
		{
			return;
		}
		float num = this.DayLengthInMinutes * 60f;
		float num2 = num / 24f;
		if (this.ProgressTime)
		{
			this.AddTime(Time.deltaTime / num2, true);
		}
		else
		{
			this.CheckTimeRange();
		}
		if (this.ProgressMoonPhase)
		{
			this.AddMoon(Time.deltaTime / (30f * num) * 2f);
		}
		else
		{
			this.CheckMoonRange();
		}
	}

	// Token: 0x04003F57 RID: 16215
	public bool m_bInside;

	// Token: 0x04003F58 RID: 16216
	public bool m_bInDevelop;

	// Token: 0x04003F59 RID: 16217
	public float DayLengthInMinutes = 30f;

	// Token: 0x04003F5A RID: 16218
	public bool ProgressTime = true;

	// Token: 0x04003F5B RID: 16219
	public bool ProgressDate = true;

	// Token: 0x04003F5C RID: 16220
	public bool ProgressMoonPhase = true;

	// Token: 0x04003F5D RID: 16221
	public bool UseTimeCurve;

	// Token: 0x04003F5E RID: 16222
	public AnimationCurve TimeCurve = AnimationCurve.Linear(0f, 0f, 24f, 24f);

	// Token: 0x04003F5F RID: 16223
	private TOD_Sky sky;

	// Token: 0x04003F60 RID: 16224
	private float hourIter;

	// Token: 0x04003F61 RID: 16225
	private float moonIter;

	// Token: 0x04003F62 RID: 16226
	private float skyTime;

	// Token: 0x04003F63 RID: 16227
	private AnimationCurve timeCurve;

	// Token: 0x04003F64 RID: 16228
	private AnimationCurve timeCurveInverse;
}
