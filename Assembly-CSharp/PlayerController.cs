using System;
using Heluo.Wulin;
using Heluo.Wulin.HeadLook;
using UnityEngine;

// Token: 0x02000142 RID: 322
public class PlayerController : MonoBehaviour
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060006CE RID: 1742 RVA: 0x000060DE File Offset: 0x000042DE
	public HeadLookController HeadLookController
	{
		get
		{
			if (this.m_HeadLookController == null)
			{
				this.m_HeadLookController = base.gameObject.GetComponentInChildren<HeadLookController>();
			}
			return this.m_HeadLookController;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060006CF RID: 1743 RVA: 0x00006108 File Offset: 0x00004308
	// (set) Token: 0x060006D0 RID: 1744 RVA: 0x00006115 File Offset: 0x00004315
	public GameObject Target
	{
		get
		{
			return this.m_PlayerFSM.GetTarget();
		}
		set
		{
			this.m_PlayerFSM.SetTarget(value);
		}
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x00047A18 File Offset: 0x00045C18
	private void Awake()
	{
		GameObject gameObject = GameObject.Find("GameSetting");
		if (gameObject == null)
		{
			gameObject = new GameObject("GameSetting");
			gameObject.tag = "GameSetting";
			gameObject.AddComponent<Save>();
			gameObject.AddComponent<Game>();
			gameObject.AddComponent<GameSetting>();
			gameObject.AddComponent<MapData>();
			gameObject.AddComponent<MovieEventTrigger>();
			gameObject.AddComponent<NPC>();
			gameObject.AddComponent<MissionStatus>();
			gameObject.AddComponent<YoungHeroTime>();
			gameObject.AddComponent<TeamStatus>();
			gameObject.AddComponent<BackpackStatus>();
			Game.SetBGMusicValue();
		}
		this.m_Player = base.gameObject.GetComponent<NavMeshAgent>();
		this.m_Contrller = base.gameObject.GetComponent<CharacterController>();
		if (PlayerController.m_Instance == null)
		{
			PlayerController.m_Instance = this;
		}
		else
		{
			Debug.LogError("Already Have Player why ??????");
		}
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x00006123 File Offset: 0x00004323
	private void Start()
	{
		this.Init();
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x00047AE4 File Offset: 0x00045CE4
	public void Init()
	{
		if (this.m_bInit)
		{
			return;
		}
		GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
		GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Equals("cFormTime"))
			{
				this.m_cFormTime = array[i];
				break;
			}
		}
		if (this.m_PlayerFSM != null)
		{
			return;
		}
		this.m_PlayerFSM = new PlayerFSM();
		this.m_PlayerFSM.init(this, this.m_Player, base.animation, this.m_Contrller, base.transform);
		this.m_PlayerFSM.SetSpeed(this.BaseSpeed, this.AddSpeed);
		this.m_bInit = true;
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x0000612B File Offset: 0x0000432B
	private void Update()
	{
		this.m_PlayerFSM.Update();
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x00006138 File Offset: 0x00004338
	public PlayerFSM GetPlayerFSM()
	{
		return this.m_PlayerFSM;
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x00006140 File Offset: 0x00004340
	public void Destory()
	{
		Game.g_InputManager.Pop();
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x0000614D File Offset: 0x0000434D
	public void SetTalkAni(string strAniID, WrapMode mod = 2)
	{
		this.m_PlayerFSM.SetTalkAni(strAniID, mod);
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x0000615C File Offset: 0x0000435C
	public void TalkStop()
	{
		this.m_PlayerFSM.TalkStop();
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x00006169 File Offset: 0x00004369
	public void ResetNav()
	{
		this.m_PlayerFSM.ResetNav();
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x00006176 File Offset: 0x00004376
	public void ReSetMoveDate()
	{
		this.m_PlayerFSM.ReSetMoveDate();
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x00006183 File Offset: 0x00004383
	public Vector3 GetPlayerPos()
	{
		return base.transform.position;
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x00006190 File Offset: 0x00004390
	public Vector3 GetPlayerAgl()
	{
		return base.transform.eulerAngles;
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x0000619D File Offset: 0x0000439D
	public bool GetIsMoveState()
	{
		return this.m_PlayerFSM.nowState == PlayerFSM.ePlayerState.ClickMove || this.m_PlayerFSM.nowState == PlayerFSM.ePlayerState.DirMove;
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x000061C4 File Offset: 0x000043C4
	public void OnDestroy()
	{
		if (!base.enabled)
		{
			return;
		}
		this.m_PlayerFSM.Destroy();
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00047BA8 File Offset: 0x00045DA8
	public void DestoryModel()
	{
		if (base.transform.childCount <= 0)
		{
			return;
		}
		Transform child = base.transform.GetChild(0);
		if (child != null)
		{
			Object.Destroy(child.gameObject);
		}
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x000061DD File Offset: 0x000043DD
	public void LookNpc(GameObject go)
	{
		this.m_PlayerFSM.LookNpc(go);
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x000061EB File Offset: 0x000043EB
	public bool CheckIdleState()
	{
		return base.enabled && this.m_PlayerFSM.CurrentState.GetType().ToString() == "PlayerIdleState";
	}

	// Token: 0x04000740 RID: 1856
	public static PlayerController m_Instance;

	// Token: 0x04000741 RID: 1857
	private NavMeshAgent m_Player;

	// Token: 0x04000742 RID: 1858
	private HeadLookController m_HeadLookController;

	// Token: 0x04000743 RID: 1859
	private GameObject m_Target;

	// Token: 0x04000744 RID: 1860
	private CharacterController m_Contrller;

	// Token: 0x04000745 RID: 1861
	public float BaseSpeed = 5f;

	// Token: 0x04000746 RID: 1862
	public float AddSpeed = 0.5f;

	// Token: 0x04000747 RID: 1863
	public bool Set;

	// Token: 0x04000748 RID: 1864
	public bool IsOnNaveMesh = true;

	// Token: 0x04000749 RID: 1865
	private string m_strTalkAinID;

	// Token: 0x0400074A RID: 1866
	private GameObject m_cFormTime;

	// Token: 0x0400074B RID: 1867
	public string m_strModelName;

	// Token: 0x0400074C RID: 1868
	private PlayerFSM m_PlayerFSM;

	// Token: 0x0400074D RID: 1869
	private bool m_bInit;
}
