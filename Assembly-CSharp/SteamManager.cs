using System;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x020002CF RID: 719
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06000E3D RID: 3645 RVA: 0x00009A92 File Offset: 0x00007C92
	private static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000E3E RID: 3646 RVA: 0x00009AB9 File Offset: 0x00007CB9
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x00009AC5 File Offset: 0x00007CC5
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x00075B9C File Offset: 0x00073D9C
	private void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInialized = true;
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x00075C8C File Offset: 0x00073E8C
	private void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x00009ACD File Offset: 0x00007CCD
	private void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x00009AF7 File Offset: 0x00007CF7
	private void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x040010B0 RID: 4272
	private static SteamManager s_instance;

	// Token: 0x040010B1 RID: 4273
	private static bool s_EverInialized;

	// Token: 0x040010B2 RID: 4274
	private bool m_bInitialized;

	// Token: 0x040010B3 RID: 4275
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
