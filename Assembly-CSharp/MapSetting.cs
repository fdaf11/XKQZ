using System;
using System.Collections;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class MapSetting : MonoBehaviour
{
	// Token: 0x0600030B RID: 779 RVA: 0x00004350 File Offset: 0x00002550
	private void Start()
	{
		Object.DontDestroyOnLoad(this);
		base.StartCoroutine(this.SetAllNpcData());
		if (this.goNpcData != null)
		{
			this.goNpcData.SetActive(false);
		}
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00004382 File Offset: 0x00002582
	private void Update()
	{
		this.CheckInput();
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0000264F File Offset: 0x0000084F
	public void LoadMap()
	{
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0002AF94 File Offset: 0x00029194
	public void SelectNpc(GameObject go)
	{
		if (this.goSelectNpc != go && this.goSelectNpc != null)
		{
			this.goSelectNpc.GetComponentInChildren<UILabel>().color = new Color(1f, 1f, 1f);
		}
		this.goSelectNpc = go;
		if (this.goSelectNpc != null)
		{
			this.goSelectNpc.GetComponentInChildren<UILabel>().color = new Color(0f, 1f, 1f);
			Debug.Log(go.GetComponentInChildren<UILabel>().text);
		}
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0002B034 File Offset: 0x00029234
	private IEnumerator DeleteSelectNpc()
	{
		UIScrollView sv = null;
		if (this.goSelectNpc != null)
		{
			sv = this.goSelectNpc.GetComponent<UIDragScrollView>().scrollView;
			Object.Destroy(this.goSelectNpc);
		}
		sv.restrictWithinPanel = false;
		this.gridNpcName.repositionNow = true;
		yield return new WaitForSeconds(0.1f);
		sv.restrictWithinPanel = true;
		sv.RestrictWithinBounds(true);
		yield break;
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0002B050 File Offset: 0x00029250
	private IEnumerator SetAllNpcData()
	{
		int Count = Game.NpcData.GetNpcCount();
		for (int i = 0; i < Count; i++)
		{
			NpcDataNode ndn = Game.NpcData.GetNpcNodePos(i);
			string caption = ndn.m_iNpcID.ToString() + "\t" + ndn.m_strNpcName;
			GameObject goTemp;
			if (i == 0)
			{
				goTemp = this.goNpcNameNode;
			}
			else
			{
				goTemp = (Object.Instantiate(this.goNpcNameNode) as GameObject);
			}
			goTemp.GetComponentInChildren<UILabel>().text = caption;
			goTemp.transform.parent = this.gridNpcName.gameObject.transform;
			goTemp.transform.localPosition = new Vector3(0f, (float)(-(float)i), 0f);
			goTemp.transform.localScale = new Vector3(1f, 1f, 1f);
			goTemp.name = this.goNpcNameNode.name;
			UIEventListener uiel = goTemp.AddComponent<UIEventListener>();
			UIEventListener uieventListener = uiel;
			uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.SelectNpc));
		}
		this.gridNpcName.repositionNow = true;
		yield return new WaitForSeconds(0.1f);
		this.goNpcListAll.SetActive(false);
		yield break;
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0000438A File Offset: 0x0000258A
	public void TriggerShowNpc()
	{
		this.bShowNpcAll = !this.bShowNpcAll;
		if (this.goNpcListAll != null)
		{
			this.goNpcListAll.SetActive(this.bShowNpcAll);
		}
	}

	// Token: 0x06000312 RID: 786 RVA: 0x0002B06C File Offset: 0x0002926C
	private void CheckInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (this.bShowNpcAll)
			{
				this.SetSelectNpcOnGround();
			}
			else
			{
				this.CheckSelectNpc();
			}
		}
		if (Input.GetKeyDown(127))
		{
			base.StartCoroutine(this.DeleteSelectNpc());
		}
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0002B0BC File Offset: 0x000292BC
	private void SetSelectNpcOnGround()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		LayerMask layerMask = 2048;
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity, layerMask))
		{
			this.SetSelectNpc(raycastHit.point);
			Debug.Log("SetSelectNpcOnGround");
		}
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0002B114 File Offset: 0x00029314
	private void SetSelectNpc(Vector3 vPos)
	{
		if (this.goSelectNpc == null)
		{
			return;
		}
		string text = this.goSelectNpc.GetComponentInChildren<UILabel>().text;
		int num = text.IndexOf("\t");
		text = text.Substring(0, num);
		num = 0;
		if (!int.TryParse(text, ref num))
		{
			num = 100012;
		}
		NpcDataNode npcData = Game.NpcData.GetNpcData(num);
		GameObject gameObject = Game.g_ModelBundle.Load(npcData.m_str3DModel + "_ModelPrefab") as GameObject;
		if (gameObject != null)
		{
			GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
			gameObject2.AddComponent<NpcOnChangePosition>();
			gameObject2.transform.position = vPos;
			gameObject2.tag = "Npc";
		}
		this.SelectNpc(null);
	}

	// Token: 0x06000315 RID: 789 RVA: 0x0002B1DC File Offset: 0x000293DC
	private void CheckSelectNpc()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, ref raycastHit, float.PositiveInfinity) && raycastHit.transform.tag == "Npc")
		{
			this.ShowNpcData(raycastHit.transform);
		}
	}

	// Token: 0x06000316 RID: 790 RVA: 0x0002B234 File Offset: 0x00029434
	private void ShowNpcData(Transform t)
	{
		if (this.goNpcData != null)
		{
			this.goNpcData.SetActive(true);
		}
		if (this.goPlayer == null)
		{
			this.goPlayer = GameObject.FindGameObjectWithTag("Player");
		}
		if (this.tSelectNpc != t)
		{
			if (this.tSelectNpc != null)
			{
				this.Set3rdPerson(this.tSelectNpc, false);
				this.tSelectNpc.GetComponent<NpcOnChangePosition>().onPositionDirectChange = null;
			}
			else
			{
				this.Set3rdPerson(this.goPlayer.transform, false);
			}
			this.tSelectNpc = t;
		}
		if (this.tSelectNpc != null && this.npd != null)
		{
			this.tSelectNpc.GetComponent<NpcOnChangePosition>().onPositionDirectChange = new NpcOnChangePosition.OnPositionDirectChange(this.npd.SetNpcPositionDirect);
			this.npd.SetNpcPositionDirect(this.tSelectNpc);
			this.Set3rdPerson(this.tSelectNpc, true);
		}
		else
		{
			this.Set3rdPerson(this.goPlayer.transform, true);
		}
	}

	// Token: 0x06000317 RID: 791 RVA: 0x000043BD File Offset: 0x000025BD
	private void Set3rdPerson(Transform t, bool bControl)
	{
		if (bControl)
		{
			t.SendMessage("Set3rdPersonEnable", 1);
		}
		else
		{
			t.SendMessage("Set3rdPersonDisable", 1);
		}
	}

	// Token: 0x04000238 RID: 568
	private string sNowMapID;

	// Token: 0x04000239 RID: 569
	public UILabel labelMapName;

	// Token: 0x0400023A RID: 570
	public GameObject goNpcNameNode;

	// Token: 0x0400023B RID: 571
	public UIGrid gridNpcName;

	// Token: 0x0400023C RID: 572
	public GameObject goNpcListAll;

	// Token: 0x0400023D RID: 573
	public GameObject goNpcData;

	// Token: 0x0400023E RID: 574
	public NpcPositionDirect npd;

	// Token: 0x0400023F RID: 575
	private GameObject goSelectNpc;

	// Token: 0x04000240 RID: 576
	private bool bShowNpcAll;

	// Token: 0x04000241 RID: 577
	private Transform tSelectNpc;

	// Token: 0x04000242 RID: 578
	private GameObject goPlayer;
}
