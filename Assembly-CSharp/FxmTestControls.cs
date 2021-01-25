using System;
using UnityEngine;

// Token: 0x02000403 RID: 1027
public class FxmTestControls : MonoBehaviour
{
	// Token: 0x060018C9 RID: 6345 RVA: 0x000101B3 File Offset: 0x0000E3B3
	public float GetTimeScale()
	{
		return this.m_fTimeScale;
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x000101BB File Offset: 0x0000E3BB
	public bool IsRepeat()
	{
		return 3 <= this.m_nPlayIndex;
	}

	// Token: 0x060018CB RID: 6347 RVA: 0x000101C9 File Offset: 0x0000E3C9
	public bool IsAutoRepeat()
	{
		return this.m_nPlayIndex == 0;
	}

	// Token: 0x060018CC RID: 6348 RVA: 0x000101D4 File Offset: 0x0000E3D4
	public float GetRepeatTime()
	{
		return this.m_fPlayToolbarTimes[this.m_nPlayIndex];
	}

	// Token: 0x060018CD RID: 6349 RVA: 0x000101E3 File Offset: 0x0000E3E3
	public void SetStartTime()
	{
		this.m_fPlayStartTime = Time.time;
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x000C93AC File Offset: 0x000C75AC
	private void LoadPrefs()
	{
		this.m_nPlayIndex = PlayerPrefs.GetInt("FxmTestControls.m_nPlayIndex", this.m_nPlayIndex);
		this.m_nTransIndex = PlayerPrefs.GetInt("FxmTestControls.m_nTransIndex", this.m_nTransIndex);
		this.m_fTimeScale = PlayerPrefs.GetFloat("FxmTestControls.m_fTimeScale", this.m_fTimeScale);
		this.m_fDistPerTime = PlayerPrefs.GetFloat("FxmTestControls.m_fDistPerTime", this.m_fDistPerTime);
		this.m_nRotateIndex = PlayerPrefs.GetInt("FxmTestControls.m_nRotateIndex", this.m_nRotateIndex);
		this.m_nTransAxis = (FxmTestControls.AXIS)PlayerPrefs.GetInt("FxmTestControls.m_nTransAxis", (int)this.m_nTransAxis);
		this.m_bMinimize = (PlayerPrefs.GetInt("FxmTestControls.m_bMinimize", (!this.m_bMinimize) ? 0 : 1) == 1);
		this.SetTimeScale(this.m_fTimeScale);
	}

	// Token: 0x060018CF RID: 6351 RVA: 0x000C9470 File Offset: 0x000C7670
	private void SavePrefs()
	{
		PlayerPrefs.SetInt("FxmTestControls.m_nPlayIndex", this.m_nPlayIndex);
		PlayerPrefs.SetInt("FxmTestControls.m_nTransIndex", this.m_nTransIndex);
		PlayerPrefs.SetFloat("FxmTestControls.m_fTimeScale", this.m_fTimeScale);
		PlayerPrefs.SetFloat("FxmTestControls.m_fDistPerTime", this.m_fDistPerTime);
		PlayerPrefs.SetInt("FxmTestControls.m_nRotateIndex", this.m_nRotateIndex);
		PlayerPrefs.SetInt("FxmTestControls.m_nTransAxis", (int)this.m_nTransAxis);
	}

	// Token: 0x060018D0 RID: 6352 RVA: 0x000C94E0 File Offset: 0x000C76E0
	public void SetDefaultSetting()
	{
		this.m_nPlayIndex = 0;
		this.m_nTransIndex = 0;
		this.m_nTransAxis = FxmTestControls.AXIS.Z;
		this.m_fDistPerTime = 10f;
		this.m_nRotateIndex = 0;
		this.m_nMultiShotCount = 1;
		this.m_fTransRate = 1f;
		this.m_fStartPosition = 0f;
		this.SavePrefs();
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x000101F0 File Offset: 0x0000E3F0
	public void AutoSetting(int nPlayIndex, int nTransIndex, FxmTestControls.AXIS nTransAxis, float fDistPerTime, int nRotateIndex, int nMultiShotCount, float fTransRate, float fStartAdjustRate)
	{
		this.m_nPlayIndex = nPlayIndex;
		this.m_nTransIndex = nTransIndex;
		this.m_nTransAxis = nTransAxis;
		this.m_fDistPerTime = fDistPerTime;
		this.m_nRotateIndex = nRotateIndex;
		this.m_nMultiShotCount = nMultiShotCount;
		this.m_fTransRate = fTransRate;
		this.m_fStartPosition = fStartAdjustRate;
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x0001022F File Offset: 0x0000E42F
	private void Awake()
	{
		NgUtil.LogDevelop("Awake - m_FXMakerControls");
		this.LoadPrefs();
	}

	// Token: 0x060018D3 RID: 6355 RVA: 0x00010241 File Offset: 0x0000E441
	private void OnEnable()
	{
		NgUtil.LogDevelop("OnEnable - m_FXMakerControls");
		this.LoadPrefs();
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x000C9538 File Offset: 0x000C7738
	private void Update()
	{
		this.m_fTimeScale = Time.timeScale;
		if (FxmTestMain.inst.GetInstanceEffectObject() == null && !this.IsAutoRepeat())
		{
			this.DelayCreateInstanceEffect(false);
		}
		else
		{
			NgObject.GetMeshInfo(NcEffectBehaviour.GetRootInstanceEffect(), true, out this.m_nVertices, out this.m_nTriangles, out this.m_nMeshCount);
			this.m_nParticleCount = 0;
			ParticleSystem[] componentsInChildren = NcEffectBehaviour.GetRootInstanceEffect().GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem particleSystem in componentsInChildren)
			{
				this.m_nParticleCount += particleSystem.particleCount;
			}
			ParticleEmitter[] componentsInChildren2 = NcEffectBehaviour.GetRootInstanceEffect().GetComponentsInChildren<ParticleEmitter>();
			foreach (ParticleEmitter particleEmitter in componentsInChildren2)
			{
				this.m_nParticleCount += particleEmitter.particleCount;
			}
			if (this.m_fDelayCreateTime < Time.time - this.m_fPlayStartTime)
			{
				if (this.IsRepeat() && this.m_fCreateTime + this.GetRepeatTime() < Time.time)
				{
					this.DelayCreateInstanceEffect(false);
				}
				if (this.m_nTransIndex == 0 && this.IsAutoRepeat() && !this.m_bCalledDelayCreate && !this.IsAliveAnimation())
				{
					this.DelayCreateInstanceEffect(false);
				}
			}
		}
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x000C9690 File Offset: 0x000C7890
	private bool IsAliveAnimation()
	{
		GameObject rootInstanceEffect = NcEffectBehaviour.GetRootInstanceEffect();
		Transform[] componentsInChildren = rootInstanceEffect.GetComponentsInChildren<Transform>(true);
		foreach (Transform transform in componentsInChildren)
		{
			int num = -1;
			int num2 = -1;
			bool flag = false;
			NcEffectBehaviour[] components = transform.GetComponents<NcEffectBehaviour>();
			foreach (NcEffectBehaviour ncEffectBehaviour in components)
			{
				int animationState = ncEffectBehaviour.GetAnimationState();
				if (animationState != 0)
				{
					if (animationState == 1)
					{
						num = 1;
					}
				}
				else
				{
					num = 0;
				}
			}
			if (transform.particleSystem != null)
			{
				num2 = 0;
				if (NgObject.IsActive(transform.gameObject) && ((transform.particleSystem.enableEmission && transform.particleSystem.IsAlive()) || 0 < transform.particleSystem.particleCount))
				{
					num2 = 1;
				}
			}
			if (num2 < 1 && transform.particleEmitter != null)
			{
				num2 = 0;
				if (NgObject.IsActive(transform.gameObject) && (transform.particleEmitter.emit || 0 < transform.particleEmitter.particleCount))
				{
					num2 = 1;
				}
			}
			if (transform.renderer != null && transform.renderer.enabled && NgObject.IsActive(transform.gameObject))
			{
				flag = true;
			}
			if (0 < num)
			{
				return true;
			}
			if (num2 == 1)
			{
				return true;
			}
			if (flag && (transform.GetComponent<MeshFilter>() != null || transform.GetComponent<TrailRenderer>() != null || transform.GetComponent<LineRenderer>() != null))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x000C985C File Offset: 0x000C7A5C
	public void OnGUIControl()
	{
		GUI.Window(10, this.GetActionToolbarRect(), new GUI.WindowFunction(this.winActionToolbar), "PrefabSimulate - " + ((!FxmTestMain.inst.IsCurrentEffectObject()) ? "Not Selected" : FxmTestMain.inst.GetOriginalEffectObject().name));
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x000C98B8 File Offset: 0x000C7AB8
	public Rect GetActionToolbarRect()
	{
		float num = (float)Screen.height * ((!this.m_bMinimize) ? 0.35f : 0.1f);
		return new Rect(0f, (float)Screen.height - num, (float)Screen.width, num);
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x00010253 File Offset: 0x0000E453
	private string NgTooltipTooltip(string str)
	{
		return str;
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x000C9900 File Offset: 0x000C7B00
	public static GUIContent[] GetHcEffectControls_Play(float fAutoRet, float timeScale, float timeOneShot1, float timeRepeat1, float timeRepeat2, float timeRepeat3, float timeRepeat4, float timeRepeat5)
	{
		return new GUIContent[]
		{
			new GUIContent("AutoRet", string.Empty),
			new GUIContent(timeScale.ToString("0.00") + "x S", string.Empty),
			new GUIContent(timeOneShot1.ToString("0.0") + "x S", string.Empty),
			new GUIContent(timeRepeat1.ToString("0.0") + "s R", string.Empty),
			new GUIContent(timeRepeat2.ToString("0.0") + "s R", string.Empty),
			new GUIContent(timeRepeat3.ToString("0.0") + "s R", string.Empty),
			new GUIContent(timeRepeat4.ToString("0.0") + "s R", string.Empty),
			new GUIContent(timeRepeat5.ToString("0.0") + "s R", string.Empty)
		};
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x000C9A1C File Offset: 0x000C7C1C
	public static GUIContent[] GetHcEffectControls_Trans(FxmTestControls.AXIS nTransAxis)
	{
		return new GUIContent[]
		{
			new GUIContent("Stop", string.Empty),
			new GUIContent(nTransAxis.ToString() + " Move", string.Empty),
			new GUIContent(nTransAxis.ToString() + " Scale", string.Empty),
			new GUIContent("Arc", string.Empty),
			new GUIContent("Fall", string.Empty),
			new GUIContent("Raise", string.Empty),
			new GUIContent("Circle", string.Empty),
			new GUIContent("Tornado", string.Empty)
		};
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x000C9AE4 File Offset: 0x000C7CE4
	public static GUIContent[] GetHcEffectControls_Rotate()
	{
		return new GUIContent[]
		{
			new GUIContent("Rot", string.Empty),
			new GUIContent("Fix", string.Empty)
		};
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x000C9B20 File Offset: 0x000C7D20
	private void winActionToolbar(int id)
	{
		Rect actionToolbarRect = this.GetActionToolbarRect();
		string text = string.Empty;
		string str = string.Empty;
		int num = 10;
		int count = 5;
		this.m_bMinimize = GUI.Toggle(new Rect(3f, 1f, FXMakerLayout.m_fMinimizeClickWidth, FXMakerLayout.m_fMinimizeClickHeight), this.m_bMinimize, "Mini");
		if (GUI.changed)
		{
			PlayerPrefs.SetInt("FxmTestControls.m_bMinimize", (!this.m_bMinimize) ? 0 : 1);
		}
		GUI.changed = false;
		Rect childVerticalRect;
		Rect innerHorizontalRect;
		if (FXMakerLayout.m_bMinimizeAll || this.m_bMinimize)
		{
			count = 1;
			childVerticalRect = FXMakerLayout.GetChildVerticalRect(actionToolbarRect, 0, count, 0, 1);
			if (FxmTestMain.inst.IsCurrentEffectObject())
			{
				text = string.Format("P={0} M={1} T={2}", this.m_nParticleCount, this.m_nMeshCount, this.m_nTriangles);
				str = string.Format("ParticleCount = {0} MeshCount = {1}\n Mesh: Triangles = {2} Vertices = {3}", new object[]
				{
					this.m_nParticleCount,
					this.m_nMeshCount,
					this.m_nTriangles,
					this.m_nVertices
				});
			}
			GUI.Box(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 0, 2), text);
			if (FxmTestMain.inst.IsCurrentEffectObject())
			{
				float num2 = (3 > this.m_nPlayIndex) ? 10f : this.m_fPlayToolbarTimes[this.m_nPlayIndex];
				childVerticalRect = FXMakerLayout.GetChildVerticalRect(actionToolbarRect, 0, count, 0, 1);
				GUI.Box(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 2, 2), "ElapsedTime " + (Time.time - this.m_fPlayStartTime).ToString("0.000"));
				innerHorizontalRect = FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 4, 4);
				innerHorizontalRect.y += 5f;
				GUI.HorizontalSlider(innerHorizontalRect, Time.time - this.m_fPlayStartTime, 0f, num2);
				childVerticalRect = FXMakerLayout.GetChildVerticalRect(actionToolbarRect, 0, count, 0, 1);
				if (GUI.Button(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 8, 2), "Restart"))
				{
					this.CreateInstanceEffect();
				}
			}
			return;
		}
		childVerticalRect = FXMakerLayout.GetChildVerticalRect(actionToolbarRect, 0, count, 0, 2);
		if (NcEffectBehaviour.GetRootInstanceEffect())
		{
			text = string.Format("P = {0}\nM = {1}\nT = {2}", this.m_nParticleCount, this.m_nMeshCount, this.m_nTriangles);
			str = string.Format("ParticleCount = {0} MeshCount = {1}\n Mesh: Triangles = {2} Vertices = {3}", new object[]
			{
				this.m_nParticleCount,
				this.m_nMeshCount,
				this.m_nTriangles,
				this.m_nVertices
			});
		}
		GUI.Box(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 0, 1), new GUIContent(text, this.NgTooltipTooltip(str)));
		if (FxmTestMain.inst.IsCurrentEffectObject())
		{
			bool flag = false;
			GUIContent[] hcEffectControls_Play = FxmTestControls.GetHcEffectControls_Play(0f, this.m_fTimeScale, this.m_fPlayToolbarTimes[1], this.m_fPlayToolbarTimes[3], this.m_fPlayToolbarTimes[4], this.m_fPlayToolbarTimes[5], this.m_fPlayToolbarTimes[6], this.m_fPlayToolbarTimes[7]);
			childVerticalRect = FXMakerLayout.GetChildVerticalRect(actionToolbarRect, 0, count, 0, 1);
			GUI.Box(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 1, 1), new GUIContent("Play", string.Empty));
			int nPlayIndex = FXMakerLayout.TooltipSelectionGrid(actionToolbarRect, FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 2, 8), this.m_nPlayIndex, hcEffectControls_Play, hcEffectControls_Play.Length);
			if (GUI.changed)
			{
				flag = true;
			}
			GUIContent[] hcEffectControls_Trans = FxmTestControls.GetHcEffectControls_Trans(this.m_nTransAxis);
			childVerticalRect = FXMakerLayout.GetChildVerticalRect(actionToolbarRect, 0, count, 1, 1);
			GUI.Box(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 1, 1), new GUIContent("Trans", string.Empty));
			int num3 = FXMakerLayout.TooltipSelectionGrid(actionToolbarRect, FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 2, 8), this.m_nTransIndex, hcEffectControls_Trans, hcEffectControls_Trans.Length);
			if (GUI.changed)
			{
				flag = true;
				if ((num3 == 1 || num3 == 2) && Input.GetMouseButtonUp(1))
				{
					if (this.m_nTransAxis == FxmTestControls.AXIS.Z)
					{
						this.m_nTransAxis = FxmTestControls.AXIS.X;
					}
					else
					{
						this.m_nTransAxis++;
					}
					PlayerPrefs.SetInt("FxmTestControls.m_nTransAxis", (int)this.m_nTransAxis);
				}
			}
			if (flag)
			{
				FxmTestMain.inst.CreateCurrentInstanceEffect(false);
				this.RunActionControl(nPlayIndex, num3);
				PlayerPrefs.SetInt("FxmTestControls.m_nPlayIndex", this.m_nPlayIndex);
				PlayerPrefs.SetInt("FxmTestControls.m_nTransIndex", this.m_nTransIndex);
			}
		}
		float num4 = this.m_fDistPerTime;
		childVerticalRect = FXMakerLayout.GetChildVerticalRect(actionToolbarRect, 0, count, 2, 1);
		GUIContent guicontent = new GUIContent("DistPerTime", string.Empty);
		GUIContent guicontent2 = guicontent;
		guicontent2.text = guicontent2.text + " " + this.m_fDistPerTime.ToString("00.00");
		GUI.Box(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 0, 2), guicontent);
		innerHorizontalRect = FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 2, 5);
		innerHorizontalRect.y += 5f;
		num4 = GUI.HorizontalSlider(innerHorizontalRect, num4, 0.1f, 40f);
		if (GUI.Button(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num * 2, 14, 1), new GUIContent("<", string.Empty)))
		{
			num4 = (float)((int)(num4 - 1f));
		}
		if (GUI.Button(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num * 2, 15, 1), new GUIContent(">", string.Empty)))
		{
			num4 = (float)((int)(num4 + 1f));
		}
		if (num4 != this.m_fDistPerTime)
		{
			this.m_fDistPerTime = ((num4 != 0f) ? num4 : 0.1f);
			PlayerPrefs.SetFloat("FxmTestControls.m_fDistPerTime", this.m_fDistPerTime);
			if (0 < this.m_nTransIndex)
			{
				this.CreateInstanceEffect();
			}
		}
		if (NgLayout.GUIButton(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 9, 1), new GUIContent("Multi", this.m_nMultiShotCount.ToString()), true))
		{
			if (Input.GetMouseButtonUp(0))
			{
				this.m_nMultiShotCount++;
				if (4 < this.m_nMultiShotCount)
				{
					this.m_nMultiShotCount = 1;
				}
			}
			else
			{
				this.m_nMultiShotCount = 1;
			}
			this.CreateInstanceEffect();
		}
		GUIContent[] hcEffectControls_Rotate = FxmTestControls.GetHcEffectControls_Rotate();
		childVerticalRect = FXMakerLayout.GetChildVerticalRect(actionToolbarRect, 0, count, 2, 1);
		int num5 = FXMakerLayout.TooltipSelectionGrid(actionToolbarRect, FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 8, 1), this.m_nRotateIndex, hcEffectControls_Rotate, hcEffectControls_Rotate.Length);
		if (num5 != this.m_nRotateIndex)
		{
			this.m_nRotateIndex = num5;
			PlayerPrefs.SetInt("FxmTestControls.m_nRotateIndex", this.m_nRotateIndex);
			if (0 < this.m_nTransIndex)
			{
				this.CreateInstanceEffect();
			}
		}
		float num6 = this.m_fTimeScale;
		childVerticalRect = FXMakerLayout.GetChildVerticalRect(actionToolbarRect, 0, count, 3, 1);
		guicontent = new GUIContent("TimeScale", string.Empty);
		GUIContent guicontent3 = guicontent;
		guicontent3.text = guicontent3.text + " " + this.m_fTimeScale.ToString("0.00");
		GUI.Box(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 0, 2), guicontent);
		innerHorizontalRect = FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 2, 5);
		innerHorizontalRect.y += 5f;
		num6 = GUI.HorizontalSlider(innerHorizontalRect, num6, 0f, 3f);
		if (num6 == 0f)
		{
			if (GUI.Button(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 7, 1), new GUIContent("Resume", string.Empty)))
			{
				num6 = this.m_fOldTimeScale;
			}
		}
		else if (GUI.Button(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 7, 1), new GUIContent("Pause", string.Empty)))
		{
			num6 = 0f;
		}
		if (GUI.Button(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 8, 1), new GUIContent("Reset", string.Empty)))
		{
			num6 = 1f;
		}
		this.SetTimeScale(num6);
		if (FxmTestMain.inst.IsCurrentEffectObject())
		{
			float num7 = (3 > this.m_nPlayIndex) ? 10f : this.m_fPlayToolbarTimes[this.m_nPlayIndex];
			childVerticalRect = FXMakerLayout.GetChildVerticalRect(actionToolbarRect, 0, count, 4, 1);
			guicontent = new GUIContent("ElapsedTime", string.Empty);
			GUIContent guicontent4 = guicontent;
			guicontent4.text = guicontent4.text + " " + (Time.time - this.m_fPlayStartTime).ToString("0.000");
			GUI.Box(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 0, 2), guicontent);
			innerHorizontalRect = FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 2, 5);
			innerHorizontalRect.y += 5f;
			GUI.HorizontalSlider(innerHorizontalRect, Time.time - this.m_fPlayStartTime, 0f, num7);
			if (GUI.Button(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num * 2, 14, 1), new GUIContent("+.5", string.Empty)))
			{
				this.SetTimeScale(1f);
				base.Invoke("invokeStopTimer", 0.5f);
			}
			if (GUI.Button(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num * 2, 15, 1), new GUIContent("+.1", string.Empty)))
			{
				this.SetTimeScale(0.4f);
				base.Invoke("invokeStopTimer", 0.1f);
			}
			if (GUI.Button(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num * 2, 16, 1), new GUIContent("+.05", string.Empty)))
			{
				this.SetTimeScale(0.2f);
				base.Invoke("invokeStopTimer", 0.05f);
			}
			if (GUI.Button(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num * 2, 17, 1), new GUIContent("+.01", string.Empty)))
			{
				this.SetTimeScale(0.04f);
				base.Invoke("invokeStopTimer", 0.01f);
			}
			childVerticalRect = FXMakerLayout.GetChildVerticalRect(actionToolbarRect, 0, count, 3, 2);
			if (GUI.Button(FXMakerLayout.GetInnerHorizontalRect(childVerticalRect, num, 9, 1), new GUIContent("Restart", string.Empty)))
			{
				this.CreateInstanceEffect();
			}
		}
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x00010256 File Offset: 0x0000E456
	private void invokeStopTimer()
	{
		this.SetTimeScale(0f);
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x00010263 File Offset: 0x0000E463
	public void RunActionControl()
	{
		this.RunActionControl(this.m_nPlayIndex, this.m_nTransIndex);
	}

	// Token: 0x060018E0 RID: 6368 RVA: 0x000CA4B8 File Offset: 0x000C86B8
	protected void RunActionControl(int nPlayIndex, int nTransIndex)
	{
		NgUtil.LogDevelop("RunActionControl() - nPlayIndex " + nPlayIndex);
		base.CancelInvoke();
		this.m_bCalledDelayCreate = false;
		this.ResumeTimeScale();
		this.m_bStartAliveAnimation = false;
		switch (nPlayIndex)
		{
		case 2:
			this.SetTimeScale(this.m_fPlayToolbarTimes[nPlayIndex]);
			break;
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
			if (nPlayIndex != this.m_nPlayIndex)
			{
				nTransIndex = 0;
			}
			break;
		}
		if (0 < nTransIndex)
		{
			float num = ((!(Camera.main != null)) ? 1f : (Vector3.Magnitude(Camera.main.transform.position) * 0.8f)) * this.m_fTransRate;
			GameObject instanceEffectObject = FxmTestMain.inst.GetInstanceEffectObject();
			GameObject gameObject = NgObject.CreateGameObject(instanceEffectObject.transform.parent.gameObject, "simulate");
			FxmTestSimulate fxmTestSimulate = gameObject.AddComponent<FxmTestSimulate>();
			instanceEffectObject.transform.parent = gameObject.transform;
			FxmTestMain.inst.ChangeRoot_InstanceEffectObject(gameObject);
			fxmTestSimulate.Init(this, this.m_nMultiShotCount);
			switch (nTransIndex)
			{
			case 1:
				fxmTestSimulate.SimulateMove(this.m_nTransAxis, num, this.m_fDistPerTime, this.m_nRotateIndex == 0);
				break;
			case 2:
				fxmTestSimulate.SimulateScale(this.m_nTransAxis, num * 0.3f, this.m_fStartPosition, this.m_fDistPerTime, this.m_nRotateIndex == 0);
				break;
			case 3:
				fxmTestSimulate.SimulateArc(num * 0.7f, this.m_fDistPerTime, this.m_nRotateIndex == 0);
				break;
			case 4:
				fxmTestSimulate.SimulateFall(num * 0.7f, this.m_fDistPerTime, this.m_nRotateIndex == 0);
				break;
			case 5:
				fxmTestSimulate.SimulateRaise(num * 0.7f, this.m_fDistPerTime, this.m_nRotateIndex == 0);
				break;
			case 6:
				fxmTestSimulate.SimulateCircle(num * 0.5f, this.m_fDistPerTime, this.m_nRotateIndex == 0);
				break;
			case 7:
				fxmTestSimulate.SimulateTornado(num * 0.3f, num * 0.7f, this.m_fDistPerTime, this.m_nRotateIndex == 0);
				break;
			}
		}
		if (0 < nTransIndex && 3 <= nPlayIndex)
		{
			nPlayIndex = 0;
		}
		this.m_nPlayIndex = nPlayIndex;
		this.m_nTransIndex = nTransIndex;
		if (this.IsRepeat())
		{
			this.m_fCreateTime = Time.time;
		}
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x00010277 File Offset: 0x0000E477
	public void OnActionTransEnd()
	{
		this.DelayCreateInstanceEffect(true);
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x000CA74C File Offset: 0x000C894C
	private void RotateFront(Transform target)
	{
		Quaternion localRotation = FxmTestMain.inst.GetOriginalEffectObject().transform.localRotation;
		Vector3 eulerAngles = localRotation.eulerAngles;
		switch (this.m_nRotateIndex)
		{
		case 1:
			eulerAngles.y += 90f;
			break;
		case 2:
			eulerAngles.y -= 90f;
			break;
		case 3:
			eulerAngles.z -= 90f;
			break;
		}
		localRotation.eulerAngles = eulerAngles;
		target.localRotation = localRotation;
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x00010280 File Offset: 0x0000E480
	private void DelayCreateInstanceEffect(bool bEndMove)
	{
		this.m_bCalledDelayCreate = true;
		base.Invoke("NextInstanceEffect", (float)((!bEndMove) ? 1 : 3) * this.m_fDelayCreateTime);
	}

	// Token: 0x060018E4 RID: 6372 RVA: 0x000102A9 File Offset: 0x0000E4A9
	private void NextInstanceEffect()
	{
		if (FxmTestMain.inst.m_bAutoChange)
		{
			FxmTestMain.inst.ChangeEffect(true);
		}
		else
		{
			this.CreateInstanceEffect();
		}
	}

	// Token: 0x060018E5 RID: 6373 RVA: 0x000102D0 File Offset: 0x0000E4D0
	private void CreateInstanceEffect()
	{
		if (FxmTestMain.inst.IsCurrentEffectObject())
		{
			FxmTestMain.inst.CreateCurrentInstanceEffect(true);
		}
	}

	// Token: 0x060018E6 RID: 6374 RVA: 0x000CA7F4 File Offset: 0x000C89F4
	private void SetTimeScale(float timeScale)
	{
		if (this.m_fTimeScale != timeScale || this.m_fTimeScale != Time.timeScale)
		{
			if (timeScale == 0f && this.m_fTimeScale != 0f)
			{
				this.m_fOldTimeScale = this.m_fTimeScale;
			}
			this.m_fTimeScale = timeScale;
			if (0.01f <= this.m_fTimeScale)
			{
				PlayerPrefs.SetFloat("FxmTestControls.m_fTimeScale", this.m_fTimeScale);
			}
			Time.timeScale = this.m_fTimeScale;
		}
	}

	// Token: 0x060018E7 RID: 6375 RVA: 0x000102EC File Offset: 0x0000E4EC
	public void ResumeTimeScale()
	{
		if (this.m_fTimeScale == 0f)
		{
			this.SetTimeScale(this.m_fOldTimeScale);
		}
	}

	// Token: 0x04001D28 RID: 7464
	protected const int m_nRepeatIndex = 3;

	// Token: 0x04001D29 RID: 7465
	public bool m_bMinimize;

	// Token: 0x04001D2A RID: 7466
	protected int m_nTriangles;

	// Token: 0x04001D2B RID: 7467
	protected int m_nVertices;

	// Token: 0x04001D2C RID: 7468
	protected int m_nMeshCount;

	// Token: 0x04001D2D RID: 7469
	protected int m_nParticleCount;

	// Token: 0x04001D2E RID: 7470
	protected int m_nPlayIndex;

	// Token: 0x04001D2F RID: 7471
	protected int m_nTransIndex;

	// Token: 0x04001D30 RID: 7472
	protected float[] m_fPlayToolbarTimes = new float[]
	{
		1f,
		1f,
		1f,
		0.3f,
		0.6f,
		1f,
		2f,
		3f
	};

	// Token: 0x04001D31 RID: 7473
	protected FxmTestControls.AXIS m_nTransAxis = FxmTestControls.AXIS.Z;

	// Token: 0x04001D32 RID: 7474
	protected float m_fDelayCreateTime = 0.2f;

	// Token: 0x04001D33 RID: 7475
	protected bool m_bCalledDelayCreate;

	// Token: 0x04001D34 RID: 7476
	protected bool m_bStartAliveAnimation;

	// Token: 0x04001D35 RID: 7477
	protected float m_fDistPerTime = 10f;

	// Token: 0x04001D36 RID: 7478
	protected int m_nRotateIndex;

	// Token: 0x04001D37 RID: 7479
	protected int m_nMultiShotCount = 1;

	// Token: 0x04001D38 RID: 7480
	protected float m_fTransRate = 1f;

	// Token: 0x04001D39 RID: 7481
	protected float m_fStartPosition;

	// Token: 0x04001D3A RID: 7482
	public float m_fTimeScale = 1f;

	// Token: 0x04001D3B RID: 7483
	protected float m_fPlayStartTime;

	// Token: 0x04001D3C RID: 7484
	protected float m_fOldTimeScale = 1f;

	// Token: 0x04001D3D RID: 7485
	protected float m_fCreateTime;

	// Token: 0x02000404 RID: 1028
	public enum AXIS
	{
		// Token: 0x04001D3F RID: 7487
		X,
		// Token: 0x04001D40 RID: 7488
		Y,
		// Token: 0x04001D41 RID: 7489
		Z
	}
}
