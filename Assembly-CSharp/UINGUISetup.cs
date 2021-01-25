using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007B6 RID: 1974
public class UINGUISetup : MonoBehaviour
{
	// Token: 0x06003044 RID: 12356 RVA: 0x00176EE0 File Offset: 0x001750E0
	public void OnTabUnitButton()
	{
		this.buttonTabUnit.uiButton.enabled = false;
		this.buttonTabPerk.uiButton.enabled = true;
		this.buttonTabUnit.spriteBG.spriteName = "Dark";
		this.buttonTabPerk.spriteBG.spriteName = "Button";
		this.uiUnitSelection.Show();
	}

	// Token: 0x06003045 RID: 12357 RVA: 0x00176F44 File Offset: 0x00175144
	public void OnTabPerkButton()
	{
		this.buttonTabUnit.uiButton.enabled = true;
		this.buttonTabPerk.uiButton.enabled = false;
		this.buttonTabUnit.spriteBG.spriteName = "Button";
		this.buttonTabPerk.spriteBG.spriteName = "Dark";
		this.uiUnitSelection.Hide();
	}

	// Token: 0x06003046 RID: 12358 RVA: 0x0001E79A File Offset: 0x0001C99A
	public static bool IsSetupScene()
	{
		return !(UINGUISetup.instance == null);
	}

	// Token: 0x06003047 RID: 12359 RVA: 0x0001E7B3 File Offset: 0x0001C9B3
	private void Awake()
	{
		UINGUISetup.instance = this;
	}

	// Token: 0x06003048 RID: 12360 RVA: 0x00176FA8 File Offset: 0x001751A8
	private void Start()
	{
		this.buttonTabUnit.Init();
		this.buttonTabPerk.Init();
		if (this.loadMode == _LoadMode.UsePersistantData)
		{
			if (!GlobalStatsTB.loaded)
			{
				GlobalStatsTB.Init();
			}
			UINGUISetup.playerPoint = GlobalStatsTB.GetPlayerPoint();
			this.uiUnitSelection.selectedUnitList = GlobalStatsTB.GetPlayerUnitList();
		}
		else
		{
			UINGUISetup.playerPoint = this.startingPlayerPoint;
		}
		UINGUISetup.UpdatePoints(0);
		this.uiUnitSelection.Init();
	}

	// Token: 0x06003049 RID: 12361 RVA: 0x00177020 File Offset: 0x00175220
	public static void UpdatePoints(int val)
	{
		if (UINGUISetup.instance == null)
		{
			return;
		}
		UINGUISetup.playerPoint += val;
		UINGUISetup.instance.lbPoint.text = "Point: " + UINGUISetup.playerPoint;
	}

	// Token: 0x0600304A RID: 12362 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x0600304B RID: 12363 RVA: 0x0001E7BB File Offset: 0x0001C9BB
	public void OnMenuButton()
	{
		if (this.mainMenu != string.Empty)
		{
			Application.LoadLevel(this.mainMenu);
		}
		else
		{
			Debug.Log("Menu scene name not specified");
		}
	}

	// Token: 0x0600304C RID: 12364 RVA: 0x0001E7EC File Offset: 0x0001C9EC
	public void OnNextSceneButton()
	{
		this.OnLoadNextScene();
	}

	// Token: 0x0600304D RID: 12365 RVA: 0x00177070 File Offset: 0x00175270
	private void OnLoadNextScene()
	{
		string text = string.Empty;
		if (!this.randomNextScene)
		{
			text = this.nextScene;
		}
		else
		{
			for (int i = 0; i < this.nextScenes.Count; i++)
			{
				if (this.nextScenes[i] == string.Empty)
				{
					this.nextScenes.RemoveAt(i);
					i--;
				}
			}
			if (this.nextScenes.Count != 0)
			{
				int num = Random.Range(0, this.nextScenes.Count);
				text = this.nextScenes[num];
			}
		}
		if (text != string.Empty)
		{
			this.OnStartBattle(text);
		}
		else
		{
			Debug.Log("Next scene is not specified");
		}
	}

	// Token: 0x0600304E RID: 12366 RVA: 0x00177138 File Offset: 0x00175338
	private void OnStartBattle(string sceneToLoad)
	{
		if (this.uiUnitSelection.selectedUnitList.Count == 0)
		{
			UINGUISetup.DisplayMessage("No unit is selected!");
			return;
		}
		if (this.loadMode == _LoadMode.UsePersistantData)
		{
			GlobalStatsTB.SetPlayerPoint(UINGUISetup.playerPoint);
			GlobalStatsTB.SetPlayerUnitList(this.uiUnitSelection.selectedUnitList);
		}
		else if (this.loadMode == _LoadMode.UseTemporaryData)
		{
			GlobalStatsTB.SetTempPlayerUnitList(this.uiUnitSelection.selectedUnitList);
		}
		Application.LoadLevel(sceneToLoad);
	}

	// Token: 0x0600304F RID: 12367 RVA: 0x0001E7F4 File Offset: 0x0001C9F4
	public void OnResetAllStats()
	{
		GlobalStatsTB.ResetAll();
		Application.LoadLevel(Application.loadedLevelName);
	}

	// Token: 0x06003050 RID: 12368 RVA: 0x0001E805 File Offset: 0x0001CA05
	private void OnDisplayMessage(string msg)
	{
		this._DisplayMessage(msg);
	}

	// Token: 0x06003051 RID: 12369 RVA: 0x0001E80E File Offset: 0x0001CA0E
	public static void DisplayMessage(string msg)
	{
		if (UINGUISetup.instance != null)
		{
			UINGUISetup.instance._DisplayMessage(msg);
		}
	}

	// Token: 0x06003052 RID: 12370 RVA: 0x001771B4 File Offset: 0x001753B4
	private void _DisplayMessage(string msg)
	{
		if (this.lbGlobalMessage == null)
		{
			return;
		}
		int num = this.msgList.Count;
		foreach (GameObject go in this.msgList)
		{
			Vector3 pos = this.lbGlobalMessage.transform.localPosition + new Vector3(0f, (float)(num * 20), 0f);
			TweenPosition.Begin(go, 0.15f, pos);
			num--;
		}
		GameObject gameObject = (GameObject)Object.Instantiate(this.lbGlobalMessage);
		gameObject.transform.parent = this.lbGlobalMessage.transform.parent;
		gameObject.transform.localPosition = this.lbGlobalMessage.transform.localPosition;
		gameObject.transform.localScale = this.lbGlobalMessage.transform.localScale;
		gameObject.GetComponent<UILabel>().text = msg;
		this.msgList.Add(gameObject);
		base.StartCoroutine(this.DestroyMessage(gameObject));
	}

	// Token: 0x06003053 RID: 12371 RVA: 0x001772F0 File Offset: 0x001754F0
	private IEnumerator DestroyMessage(GameObject obj)
	{
		yield return new WaitForSeconds(1.25f);
		TweenScale.Begin(obj, 0.5f, new Vector3(0.01f, 0.01f, 0.01f));
		yield return new WaitForSeconds(0.75f);
		this.msgList.RemoveAt(0);
		Object.Destroy(obj);
		yield break;
	}

	// Token: 0x04003BEB RID: 15339
	public _LoadMode loadMode;

	// Token: 0x04003BEC RID: 15340
	public UILabel lbPoint;

	// Token: 0x04003BED RID: 15341
	public int startingPlayerPoint = 25;

	// Token: 0x04003BEE RID: 15342
	public static int playerPoint;

	// Token: 0x04003BEF RID: 15343
	public NGUIButton buttonTabUnit;

	// Token: 0x04003BF0 RID: 15344
	public NGUIButton buttonTabPerk;

	// Token: 0x04003BF1 RID: 15345
	public string mainMenu = string.Empty;

	// Token: 0x04003BF2 RID: 15346
	public string nextScene = string.Empty;

	// Token: 0x04003BF3 RID: 15347
	public bool randomNextScene;

	// Token: 0x04003BF4 RID: 15348
	public List<string> nextScenes = new List<string>();

	// Token: 0x04003BF5 RID: 15349
	public UINGUIUnitSelection uiUnitSelection;

	// Token: 0x04003BF6 RID: 15350
	public static UINGUISetup instance;

	// Token: 0x04003BF7 RID: 15351
	public GameObject lbGlobalMessage;

	// Token: 0x04003BF8 RID: 15352
	private List<GameObject> msgList = new List<GameObject>();
}
