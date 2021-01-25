using System;
using System.Collections.Generic;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x020007D1 RID: 2001
public class UINGUIItem : MonoBehaviour
{
	// Token: 0x06003158 RID: 12632 RVA: 0x0017DBCC File Offset: 0x0017BDCC
	private void Awake()
	{
		this.isOn = false;
		this.m_iSetItemIndex = 0;
		if (this.battleItemObj != null)
		{
			for (int i = 0; i < this.m_ItemCount; i++)
			{
				this.CreateObj();
			}
			this.itemGrid.Reposition();
		}
		this.m_ExplainArea.InitWidget(null);
		this.m_ExplainArea.gameObject.SetActive(false);
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x0001F0CB File Offset: 0x0001D2CB
	private void OnEnable()
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown += new Action<KeyControl.Key>(this.OnKeyDown);
			UINGUI.instance.MouseControl += new Action<bool>(this.OnMouseControl);
		}
	}

	// Token: 0x0600315A RID: 12634 RVA: 0x0001F109 File Offset: 0x0001D309
	private void OnDisable()
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown -= new Action<KeyControl.Key>(this.OnKeyDown);
			UINGUI.instance.MouseControl -= new Action<bool>(this.OnMouseControl);
		}
	}

	// Token: 0x0600315B RID: 12635 RVA: 0x0001F147 File Offset: 0x0001D347
	private void OnMouseControl(bool bMouse)
	{
		if (UINGUI.instance.battleControlState != BattleControlState.SelectItem)
		{
			return;
		}
		if (bMouse)
		{
			this.ItmeOnHover(this.goFocusHover, false);
		}
		else
		{
			this.ItmeOnHover(this.goFocusHover, true);
		}
	}

	// Token: 0x0600315C RID: 12636 RVA: 0x0017DC40 File Offset: 0x0017BE40
	private void OnKeyDown(KeyControl.Key keyCode)
	{
		if (GameGlobal.m_bBattleTalk)
		{
			return;
		}
		if (UINGUI.instance.battleControlState != BattleControlState.SelectItem)
		{
			return;
		}
		switch (keyCode)
		{
		case KeyControl.Key.Up:
			this.MoveDirect(new Vector2(0f, 1f));
			break;
		case KeyControl.Key.Down:
			this.MoveDirect(new Vector2(0f, -1f));
			break;
		case KeyControl.Key.OK:
			this.ItemOnClick(this.goFocusHover);
			break;
		case KeyControl.Key.Cancel:
			this.Hide(true);
			break;
		}
	}

	// Token: 0x0600315D RID: 12637 RVA: 0x0001F17F File Offset: 0x0001D37F
	public void OnMove(Vector2 direction)
	{
		if (UINGUI.instance.battleControlState != BattleControlState.SelectItem)
		{
			return;
		}
		this.MoveDirect(direction);
	}

	// Token: 0x0600315E RID: 12638 RVA: 0x0017DCE0 File Offset: 0x0017BEE0
	private void MoveDirect(Vector2 direction)
	{
		if (direction.sqrMagnitude <= 0.1f)
		{
			return;
		}
		if (this.ftime + this.ftick > Time.time)
		{
			return;
		}
		this.ftime = Time.time;
		this.ItmeOnHover(this.goFocusHover, false);
		GameObject gameObject = this.MoveDirectGameObject(direction);
		if (gameObject != null)
		{
			this.goFocus = gameObject;
			this.goFocusHover = gameObject.GetComponentInChildren<LabelData>().gameObject;
		}
		this.ItmeOnHover(this.goFocusHover, true);
	}

	// Token: 0x0600315F RID: 12639 RVA: 0x0017DD68 File Offset: 0x0017BF68
	private GameObject MoveDirectGameObject(Vector2 direction)
	{
		if (this.goFocus == null)
		{
			return null;
		}
		GameObject result = null;
		float num = float.MaxValue;
		Vector3 vector;
		vector..ctor(direction.x, direction.y, 0f);
		Quaternion quaternion = Quaternion.LookRotation(vector);
		foreach (GameObject gameObject in this.m_ItemList)
		{
			if (!(gameObject == this.goFocus))
			{
				if (gameObject.activeSelf)
				{
					Vector3 vector2 = Vector3.zero;
					float num2 = Vector3.Distance(gameObject.transform.localPosition, this.goFocus.transform.localPosition);
					vector2 = gameObject.transform.localPosition - this.goFocus.transform.localPosition;
					vector2.z = 0f;
					Quaternion quaternion2 = Quaternion.LookRotation(vector2);
					float num3 = Quaternion.Angle(quaternion, quaternion2);
					if (num3 < 45.5f && num2 < num)
					{
						result = gameObject;
						num = num2;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06003160 RID: 12640 RVA: 0x0017DEAC File Offset: 0x0017C0AC
	private void CreateObj()
	{
		if (this.battleItemObj == null)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate(this.battleItemObj) as GameObject;
		gameObject.transform.parent = this.itemGrid.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.name = "BattleItem";
		gameObject.transform.Traversal(delegate(Transform x)
		{
			string name = x.name;
			if (name != null)
			{
				if (UINGUIItem.<>f__switch$map6A == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
					dictionary.Add("Name", 0);
					dictionary.Add("Amount", 1);
					UINGUIItem.<>f__switch$map6A = dictionary;
				}
				int num;
				if (UINGUIItem.<>f__switch$map6A.TryGetValue(name, ref num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							this.m_AmountList.Add(x);
						}
					}
					else
					{
						x.gameObject.GetComponent<LabelData>().m_iIndex = this.m_iSetItemIndex;
						x.OnClick += this.ItemOnClick;
						x.OnHover += this.ItmeOnHover;
						this.m_iSetItemIndex++;
						this.m_NameList.Add(x);
					}
				}
			}
		});
		this.m_ItemList.Add(gameObject);
	}

	// Token: 0x06003161 RID: 12641 RVA: 0x0017DF48 File Offset: 0x0017C148
	private void ResetItem()
	{
		for (int i = 0; i < this.m_NameList.Count; i++)
		{
			this.m_ItemList[i].SetActive(false);
		}
		this.m_ExplainArea.gameObject.SetActive(false);
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x0017DF94 File Offset: 0x0017C194
	private void ItemOnClick(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		BackpackNewDataNode backpackItem = BackpackStatus.m_Instance.GetBackpackItem(go.GetComponent<LabelData>().m_iHoverID);
		if (backpackItem == null)
		{
			return;
		}
		UINGUI.instance.UseItem(backpackItem._ItemDataNode);
		this.Hide(true);
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x0017DFE4 File Offset: 0x0017C1E4
	private void ItmeOnHover(GameObject go, bool bHover)
	{
		if (go == null)
		{
			return;
		}
		if (!GameCursor.IsShow)
		{
			this.AdjustSlider(go);
		}
		if (!go.gameObject.activeSelf)
		{
			return;
		}
		if (bHover)
		{
			this.highlight.gameObject.transform.position = go.transform.position;
		}
		else
		{
			this.highlight.gameObject.transform.position = new Vector3(0f, 500000f);
		}
		this.SetItemExplain(go, bHover);
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x0017E078 File Offset: 0x0017C278
	private void AdjustSlider(GameObject go)
	{
		int i;
		for (i = 0; i < this.m_NameList.Count; i++)
		{
			if (this.m_NameList[i].GameObject == go)
			{
				break;
			}
		}
		if (i < this.m_NameList.Count)
		{
			bool flag = false;
			if (i < this.m_iSliderIndex)
			{
				this.m_iSliderIndex = i;
				flag = true;
			}
			if (i >= this.m_iSliderIndex + this.m_OnePageCount)
			{
				this.m_iSliderIndex = i - this.m_OnePageCount + 1;
				flag = true;
			}
			if (flag)
			{
				this.slider.value = this.m_fStep * (float)this.m_iSliderIndex;
			}
		}
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x0017E130 File Offset: 0x0017C330
	private void SetItemExplain(GameObject go, bool on)
	{
		if (on)
		{
			BackpackNewDataNode backpackItem = BackpackStatus.m_Instance.GetBackpackItem(go.GetComponent<LabelData>().m_iHoverID);
			if (backpackItem == null)
			{
				return;
			}
			this.SetTipData(backpackItem._ItemDataNode);
		}
		else
		{
			this.m_ExplainArea.SetActive(false);
		}
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x0017E180 File Offset: 0x0017C380
	public void Show()
	{
		base.gameObject.SetActive(true);
		if (BackpackStatus.m_Instance == null)
		{
			return;
		}
		BackpackStatus.m_Instance.SortBattleItem();
		List<BackpackNewDataNode> sortTeamBackpack = BackpackStatus.m_Instance.GetSortTeamBackpack();
		if (sortTeamBackpack.Count > this.m_ItemCount)
		{
			while (sortTeamBackpack.Count > this.m_ItemCount)
			{
				this.CreateObj();
				this.m_ItemCount++;
			}
			this.itemGrid.Reposition();
		}
		this.ResetItem();
		this.SetBackpackItem(sortTeamBackpack);
		this.isOn = true;
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x0001F199 File Offset: 0x0001D399
	public void Hide(bool bCallBack = true)
	{
		this.isOn = false;
		base.gameObject.SetActive(false);
		if (bCallBack)
		{
			UINGUI.instance.CallBackSelectItem();
		}
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x0017E21C File Offset: 0x0017C41C
	private void SetBackpackItem(List<BackpackNewDataNode> sortList)
	{
		for (int i = 0; i < sortList.Count; i++)
		{
			BackpackNewDataNode backpackNewDataNode = sortList[i];
			this.m_ItemList[i].SetActive(true);
			this.m_NameList[i].Text = backpackNewDataNode._ItemDataNode.m_strItemName;
			this.m_AmountList[i].Text = backpackNewDataNode.m_iAmount.ToString();
			this.m_NameList[i].GameObject.GetComponent<LabelData>().m_iHoverID = backpackNewDataNode._ItemDataNode.m_iItemID;
		}
		if (sortList.Count > 0)
		{
			this.goFocus = this.m_ItemList[0];
			this.goFocusHover = this.m_NameList[0].GameObject;
			this.m_iSliderIndex = 0;
			if (sortList.Count > this.m_OnePageCount)
			{
				this.m_fStep = 1f / (float)(sortList.Count - this.m_OnePageCount);
			}
		}
		else
		{
			this.goFocus = null;
			this.goFocusHover = null;
		}
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x0017E334 File Offset: 0x0017C534
	private void SetTipData(ItemDataNode itemDataNode)
	{
		TipData tipData = new TipData();
		tipData.name = itemDataNode.m_strItemName;
		tipData.texture = (Game.g_Item.Load("2dtexture/gameui/item/" + itemDataNode.m_strIcon) as Texture);
		tipData.explain = itemDataNode.m_strTip;
		for (int i = 0; i < itemDataNode.m_ItmeEffectNodeList.Count; i++)
		{
			if (i < itemDataNode.m_ItmeEffectNodeList.Count)
			{
				ItmeEffectNode itmeEffectNode = itemDataNode.m_ItmeEffectNodeList[i];
				int iItemType = itmeEffectNode.m_iItemType;
				int iRecoverType = itmeEffectNode.m_iRecoverType;
				int iValue = itmeEffectNode.m_iValue;
				switch (iItemType)
				{
				case 1:
				{
					string text = Game.StringTable.GetString(110100 + iRecoverType);
					string text2;
					if (iValue > 0)
					{
						text2 = "  +";
					}
					else
					{
						text2 = "  ";
					}
					tipData.appendList.Add(text + text2 + iValue.ToString());
					break;
				}
				case 3:
				{
					RoutineNewDataNode routineNewData = Game.RoutineNewData.GetRoutineNewData(iRecoverType);
					tipData.appendList.Add(routineNewData.m_strUpgradeNotes.Replace("<br>", "\n"));
					break;
				}
				case 4:
				{
					NeigongNewDataNode neigongData = Game.NeigongData.GetNeigongData(iRecoverType);
					tipData.appendList.Add(neigongData.m_strUpgradeNotes.Replace("<br>", "\n"));
					break;
				}
				case 7:
				{
					string text = Game.StringTable.GetString(160004);
					ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(iRecoverType);
					tipData.appendList.Add(string.Concat(new string[]
					{
						"[c][12C0D3]",
						text,
						"  ",
						conditionNode.m_strName,
						"[-][/c]"
					}));
					break;
				}
				case 8:
				{
					string text = Game.StringTable.GetString(110300);
					tipData.appendList.Add(text + " " + iValue.ToString());
					break;
				}
				case 9:
				{
					string text = Game.StringTable.GetString(110301);
					tipData.appendList.Add(text + " " + iValue.ToString());
					break;
				}
				case 16:
				{
					string text = Game.StringTable.GetString(110100);
					tipData.appendList.Add(text + " " + iValue.ToString() + "%");
					break;
				}
				case 17:
				{
					string text = Game.StringTable.GetString(110102);
					tipData.appendList.Add(text + " " + iValue.ToString() + "%");
					break;
				}
				case 18:
				{
					string text = Game.StringTable.GetString(110302);
					text = string.Format(text, iValue.ToString());
					tipData.appendList.Add(text);
					break;
				}
				}
			}
		}
		for (int j = 0; j < itemDataNode.m_UseLimitNodeList.Count; j++)
		{
			string text3 = string.Empty;
			if (j == 0)
			{
				switch (itemDataNode.m_iItemType)
				{
				case 1:
				case 2:
				case 3:
					text3 = Game.StringTable.GetString(110200) + "：";
					break;
				case 6:
					text3 = Game.StringTable.GetString(110201) + "：";
					break;
				}
			}
			UseLimitNode useLimitNode = itemDataNode.m_UseLimitNodeList[j];
			UseLimitType type = useLimitNode.m_Type;
			int iInde = useLimitNode.m_iInde;
			int iValue2 = useLimitNode.m_iValue;
			if (type == UseLimitType.MoreNpcProperty)
			{
				text3 += Game.StringTable.GetString(110100 + iInde);
				text3 += iValue2.ToString();
				tipData.limitList.Add(text3);
			}
		}
		this.m_ExplainArea.SetActive(true);
		this.m_ExplainArea.SetItemTip(tipData);
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04003CDE RID: 15582
	public GameObject battleItemObj;

	// Token: 0x04003CDF RID: 15583
	public UIGrid itemGrid;

	// Token: 0x04003CE0 RID: 15584
	public UISprite highlight;

	// Token: 0x04003CE1 RID: 15585
	public bool isOn;

	// Token: 0x04003CE2 RID: 15586
	public WgBackPackTip m_ExplainArea;

	// Token: 0x04003CE3 RID: 15587
	public UISlider slider;

	// Token: 0x04003CE4 RID: 15588
	private int m_iSetItemIndex;

	// Token: 0x04003CE5 RID: 15589
	private int m_iSliderIndex;

	// Token: 0x04003CE6 RID: 15590
	private int m_ItemCount = 14;

	// Token: 0x04003CE7 RID: 15591
	private int m_OnePageCount = 14;

	// Token: 0x04003CE8 RID: 15592
	private float m_fStep;

	// Token: 0x04003CE9 RID: 15593
	private List<GameObject> m_ItemList = new List<GameObject>();

	// Token: 0x04003CEA RID: 15594
	private List<Control> m_NameList = new List<Control>();

	// Token: 0x04003CEB RID: 15595
	private List<Control> m_AmountList = new List<Control>();

	// Token: 0x04003CEC RID: 15596
	private GameObject goFocus;

	// Token: 0x04003CED RID: 15597
	private GameObject goFocusHover;

	// Token: 0x04003CEE RID: 15598
	private float ftime;

	// Token: 0x04003CEF RID: 15599
	private float ftick = 0.15f;
}
