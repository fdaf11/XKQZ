using System;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class GameTime : MonoBehaviour
{
	// Token: 0x17000102 RID: 258
	// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0004DF48 File Offset: 0x0004C148
	public static GameTime Instance
	{
		get
		{
			if (GameTime.instance == null)
			{
				GameObject gameObject = new GameObject("GameTime");
				GameTime.instance = gameObject.AddComponent<GameTime>();
			}
			return GameTime.instance;
		}
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0000501F File Offset: 0x0000321F
	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x060008EB RID: 2283 RVA: 0x00007594 File Offset: 0x00005794
	public float BigMapDeltaTime
	{
		get
		{
			return Time.unscaledDeltaTime * this.BigMapTimeScale;
		}
	}

	// Token: 0x0400086B RID: 2155
	private static GameTime instance;

	// Token: 0x0400086C RID: 2156
	public float BigMapTimeScale = 1f;
}
