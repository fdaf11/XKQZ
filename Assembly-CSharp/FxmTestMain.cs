using System;
using UnityEngine;

// Token: 0x02000405 RID: 1029
public class FxmTestMain : MonoBehaviour
{
	// Token: 0x060018E8 RID: 6376 RVA: 0x0001030A File Offset: 0x0000E50A
	private FxmTestMain()
	{
		FxmTestMain.inst = this;
	}

	// Token: 0x060018E9 RID: 6377 RVA: 0x00010326 File Offset: 0x0000E526
	public FxmTestMouse GetFXMakerMouse()
	{
		if (this.m_FXMakerMouse == null)
		{
			this.m_FXMakerMouse = base.GetComponentInChildren<FxmTestMouse>();
		}
		return this.m_FXMakerMouse;
	}

	// Token: 0x060018EA RID: 6378 RVA: 0x0001034B File Offset: 0x0000E54B
	public FxmTestControls GetFXMakerControls()
	{
		if (this.m_FXMakerControls == null)
		{
			this.m_FXMakerControls = base.GetComponent<FxmTestControls>();
		}
		return this.m_FXMakerControls;
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x00010370 File Offset: 0x0000E570
	private void Awake()
	{
		NgUtil.LogDevelop("Awake - FXMakerMain");
		this.GetFXMakerControls().enabled = true;
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x00010388 File Offset: 0x0000E588
	private void OnEnable()
	{
		NgUtil.LogDevelop("OnEnable - FXMakerMain");
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x000CA878 File Offset: 0x000C8A78
	private void Start()
	{
		if (0 < this.m_GroupList.transform.childCount)
		{
			this.m_PrefabList = this.m_GroupList.transform.GetChild(0).gameObject;
		}
		if (this.m_PrefabList != null && 0 < this.m_PrefabList.transform.childCount)
		{
			this.m_OriginalEffectObject = this.m_PrefabList.transform.GetChild(0).gameObject;
			this.CreateCurrentInstanceEffect(true);
		}
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x060018EF RID: 6383 RVA: 0x000CA904 File Offset: 0x000C8B04
	public void OnGUI()
	{
		GUI.skin = this.m_GuiMainSkin;
		float num = (float)(Screen.width / 7);
		float num2 = (float)(Screen.height / 10);
		this.m_FXMakerControls.OnGUIControl();
		if (GUI.Button(new Rect(0f, 0f, num, num2), "GPrev"))
		{
			this.ChangeGroup(false);
		}
		if (GUI.Button(new Rect(num + 10f, 0f, num, num2), "GNext"))
		{
			this.ChangeGroup(true);
		}
		GUI.Box(new Rect(0f, num2 + 10f, num * 2f + 10f, 20f), this.m_GroupList.transform.GetChild(this.m_CurrentGroupIndex).name, GUI.skin.FindStyle("Hierarchy_Button"));
		if (GUI.Button(new Rect((float)Screen.width - num * 2f - 10f, 0f, num, num2), "EPrev"))
		{
			this.ChangeEffect(false);
		}
		if (GUI.Button(new Rect((float)Screen.width - num, 0f, num, num2), "ENext"))
		{
			this.ChangeEffect(true);
		}
		this.m_bAutoChange = GUI.Toggle(new Rect((float)Screen.width - num, num2 + 10f, num, 20f), this.m_bAutoChange, "AutoChange");
		bool flag = GUI.Toggle(new Rect((float)Screen.width - num * 2f - 10f, num2 + 10f, num, 20f), this.m_bAutoSetting, "AutoSetting");
		if (flag != this.m_bAutoSetting)
		{
			this.m_bAutoSetting = flag;
			if (!flag)
			{
				this.m_FXMakerControls.SetDefaultSetting();
			}
		}
		float num3 = GUI.VerticalSlider(new Rect(10f, num2 + 10f + 30f, 25f, (float)Screen.height - (num2 + 10f + 50f) - this.GetFXMakerControls().GetActionToolbarRect().height), this.GetFXMakerMouse().m_fDistance, this.GetFXMakerMouse().m_fDistanceMin, this.GetFXMakerMouse().m_fDistanceMax);
		if (num3 != this.GetFXMakerMouse().m_fDistance)
		{
			this.GetFXMakerMouse().SetDistance(num3);
		}
	}

	// Token: 0x060018F0 RID: 6384 RVA: 0x000CAB54 File Offset: 0x000C8D54
	public void ChangeEffect(bool bNext)
	{
		if (this.m_PrefabList == null)
		{
			return;
		}
		if (bNext)
		{
			if (this.m_CurrentPrefabIndex >= this.m_PrefabList.transform.childCount - 1)
			{
				this.ChangeGroup(true);
				return;
			}
			this.m_CurrentPrefabIndex++;
		}
		else
		{
			if (this.m_CurrentPrefabIndex == 0)
			{
				this.ChangeGroup(false);
				return;
			}
			this.m_CurrentPrefabIndex--;
		}
		this.m_OriginalEffectObject = this.m_PrefabList.transform.GetChild(this.m_CurrentPrefabIndex).gameObject;
		this.CreateCurrentInstanceEffect(true);
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x000CAC04 File Offset: 0x000C8E04
	public bool ChangeGroup(bool bNext)
	{
		if (bNext)
		{
			if (this.m_CurrentGroupIndex < this.m_GroupList.transform.childCount - 1)
			{
				this.m_CurrentGroupIndex++;
			}
			else
			{
				this.m_CurrentGroupIndex = 0;
			}
		}
		else if (this.m_CurrentGroupIndex == 0)
		{
			this.m_CurrentGroupIndex = this.m_GroupList.transform.childCount - 1;
		}
		else
		{
			this.m_CurrentGroupIndex--;
		}
		this.m_PrefabList = this.m_GroupList.transform.GetChild(this.m_CurrentGroupIndex).gameObject;
		if (this.m_PrefabList != null && 0 < this.m_PrefabList.transform.childCount)
		{
			this.m_CurrentPrefabIndex = 0;
			this.m_OriginalEffectObject = this.m_PrefabList.transform.GetChild(this.m_CurrentPrefabIndex).gameObject;
			this.CreateCurrentInstanceEffect(true);
			return true;
		}
		return true;
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x00010394 File Offset: 0x0000E594
	public bool IsCurrentEffectObject()
	{
		return this.m_OriginalEffectObject != null;
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x000103A2 File Offset: 0x0000E5A2
	public GameObject GetOriginalEffectObject()
	{
		return this.m_OriginalEffectObject;
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x000103AA File Offset: 0x0000E5AA
	public void ChangeRoot_OriginalEffectObject(GameObject newRoot)
	{
		this.m_OriginalEffectObject = newRoot;
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x000103B3 File Offset: 0x0000E5B3
	public void ChangeRoot_InstanceEffectObject(GameObject newRoot)
	{
		this.m_InstanceEffectObject = newRoot;
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x000103BC File Offset: 0x0000E5BC
	public GameObject GetInstanceEffectObject()
	{
		return this.m_InstanceEffectObject;
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x000CAD04 File Offset: 0x000C8F04
	public void ClearCurrentEffectObject(GameObject effectRoot, bool bClearEventObject)
	{
		if (bClearEventObject)
		{
			GameObject instanceRoot = this.GetInstanceRoot();
			if (instanceRoot != null)
			{
				NgObject.RemoveAllChildObject(instanceRoot, true);
			}
		}
		NgObject.RemoveAllChildObject(effectRoot, true);
		this.m_OriginalEffectObject = null;
		this.CreateCurrentInstanceEffect(null);
	}

	// Token: 0x060018F8 RID: 6392 RVA: 0x000CAD48 File Offset: 0x000C8F48
	public void CreateCurrentInstanceEffect(bool bRunAction)
	{
		FxmTestSetting component = this.m_PrefabList.GetComponent<FxmTestSetting>();
		if (this.m_bAutoSetting && component != null)
		{
			this.m_FXMakerControls.AutoSetting(component.m_nPlayIndex, component.m_nTransIndex, component.m_nTransAxis, component.m_fDistPerTime, component.m_nRotateIndex, component.m_nMultiShotCount, component.m_fTransRate, component.m_fStartPosition);
		}
		NgUtil.LogDevelop("CreateCurrentInstanceEffect() - bRunAction - " + bRunAction);
		bool flag = this.CreateCurrentInstanceEffect(this.m_OriginalEffectObject);
		if (flag && bRunAction)
		{
			this.m_FXMakerControls.RunActionControl();
		}
	}

	// Token: 0x060018F9 RID: 6393 RVA: 0x000103C4 File Offset: 0x0000E5C4
	public GameObject GetInstanceRoot()
	{
		return NcEffectBehaviour.GetRootInstanceEffect();
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x000CADEC File Offset: 0x000C8FEC
	private bool CreateCurrentInstanceEffect(GameObject gameObj)
	{
		NgUtil.LogDevelop("CreateCurrentInstanceEffect() - gameObj - " + gameObj);
		GameObject instanceRoot = this.GetInstanceRoot();
		NgObject.RemoveAllChildObject(instanceRoot, true);
		if (gameObj != null)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(gameObj);
			NsEffectManager.PreloadResource(gameObject);
			gameObject.transform.parent = instanceRoot.transform;
			this.m_InstanceEffectObject = gameObject;
			NgObject.SetActiveRecursively(gameObject, true);
			this.m_FXMakerControls.SetStartTime();
			return true;
		}
		this.m_InstanceEffectObject = null;
		return false;
	}

	// Token: 0x04001D42 RID: 7490
	public static FxmTestMain inst;

	// Token: 0x04001D43 RID: 7491
	public GUISkin m_GuiMainSkin;

	// Token: 0x04001D44 RID: 7492
	public FxmTestMouse m_FXMakerMouse;

	// Token: 0x04001D45 RID: 7493
	public FxmTestControls m_FXMakerControls;

	// Token: 0x04001D46 RID: 7494
	public AnimationCurve m_SimulateArcCurve;

	// Token: 0x04001D47 RID: 7495
	public GameObject m_GroupList;

	// Token: 0x04001D48 RID: 7496
	public int m_CurrentGroupIndex;

	// Token: 0x04001D49 RID: 7497
	public GameObject m_PrefabList;

	// Token: 0x04001D4A RID: 7498
	public int m_CurrentPrefabIndex;

	// Token: 0x04001D4B RID: 7499
	public bool m_bAutoChange = true;

	// Token: 0x04001D4C RID: 7500
	public bool m_bAutoSetting = true;

	// Token: 0x04001D4D RID: 7501
	protected GameObject m_OriginalEffectObject;

	// Token: 0x04001D4E RID: 7502
	protected GameObject m_InstanceEffectObject;
}
