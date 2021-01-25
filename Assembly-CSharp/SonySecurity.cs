using System;
using UnityEngine;

// Token: 0x02000710 RID: 1808
public class SonySecurity : MonoBehaviour
{
	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x06002AE4 RID: 10980 RVA: 0x0001BCBF File Offset: 0x00019EBF
	public float FirstDataDirectoryCheckStartTime
	{
		get
		{
			return this._firstDataDirectoryCheckStartTime;
		}
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x06002AE5 RID: 10981 RVA: 0x0001BCC7 File Offset: 0x00019EC7
	public float FirstCompareImageSizeCheckStartTime
	{
		get
		{
			return this._firstCompareImageSizeCheckStartTime;
		}
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x06002AE6 RID: 10982 RVA: 0x0001BCCF File Offset: 0x00019ECF
	public float FirstHiddenSectionCheckTime
	{
		get
		{
			return this._firstHiddenSectionCheckStartTime;
		}
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06002AE7 RID: 10983 RVA: 0x0001BCD7 File Offset: 0x00019ED7
	public float FirstLicenceCheckStartTime
	{
		get
		{
			return this._firstLicenceCheckStartTime;
		}
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06002AE8 RID: 10984 RVA: 0x0001BCDF File Offset: 0x00019EDF
	public float RepeatDataDirectoryCheckEveryXseconds
	{
		get
		{
			return this._repeatDataDirectoryCheckEveryXseconds;
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06002AE9 RID: 10985 RVA: 0x0001BCC7 File Offset: 0x00019EC7
	public float RepeatCompareImageSizeCheckEveryXseconds
	{
		get
		{
			return this._firstCompareImageSizeCheckStartTime;
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06002AEA RID: 10986 RVA: 0x0001BCE7 File Offset: 0x00019EE7
	public float RepeatHiddenSectionCheckEveryXseconds
	{
		get
		{
			return this._repeatHiddenSectionCheckEveryXseconds;
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06002AEB RID: 10987 RVA: 0x0001BCEF File Offset: 0x00019EEF
	public float RepeatLicenceCheckEveryXseconds
	{
		get
		{
			return this._repeatLicenceCheckEveryXseconds;
		}
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x06002AEC RID: 10988 RVA: 0x0001BCF7 File Offset: 0x00019EF7
	public static SonySecurity Instance
	{
		get
		{
			return SonySecurity._instance;
		}
	}

	// Token: 0x06002AED RID: 10989 RVA: 0x0001BCFE File Offset: 0x00019EFE
	private void Awake()
	{
		if (SonySecurity._instance == null)
		{
			Object.DontDestroyOnLoad(base.transform.gameObject);
			SonySecurity._instance = this;
		}
		else
		{
			Object.Destroy(base.transform.gameObject);
		}
	}

	// Token: 0x06002AEE RID: 10990 RVA: 0x0001BD3B File Offset: 0x00019F3B
	private void Start()
	{
		dss_api.Init();
	}

	// Token: 0x06002AEF RID: 10991 RVA: 0x0001BD42 File Offset: 0x00019F42
	private void OnApplicationQuit()
	{
		SonySecurity.bQuit = true;
	}

	// Token: 0x06002AF0 RID: 10992 RVA: 0x0014DFC4 File Offset: 0x0014C1C4
	private void Update()
	{
		if (SonySecurity.bQuit)
		{
			return;
		}
		if (Time.time > this._firstDataDirectoryCheckStartTime && !this._updatePastedDataDirectoryCheckTime)
		{
			this.DataDirectoryCheck();
			this._updatePastedDataDirectoryCheckTime = true;
		}
		if (Time.time > this._firstCompareImageSizeCheckStartTime && !this._updatePastedCompareImageSizeCheckTime)
		{
			this.CompareImageSizeCheck();
			this._updatePastedCompareImageSizeCheckTime = true;
		}
		if (Time.time > this._firstHiddenSectionCheckStartTime && !this._updatePastedHiddenSectionCheckTime)
		{
			this.HiddenSectionCheck();
			this._updatePastedHiddenSectionCheckTime = true;
		}
		if (Time.time > this._firstLicenceCheckStartTime && !this._updatePastedLicenceCheckTime)
		{
			this.LicenceCheck();
			this._updatePastedLicenceCheckTime = true;
		}
		if (this._pastedDataDirectoryCheckTime > this._repeatDataDirectoryCheckEveryXseconds)
		{
			this.DataDirectoryCheck();
			this._pastedDataDirectoryCheckTime = 0f;
		}
		if (this._pastedCompareImageSizeCheckTime > this._repeatCompareImageSizeCheckEveryXseconds)
		{
			this.CompareImageSizeCheck();
			this._pastedCompareImageSizeCheckTime = 0f;
		}
		if (this._pastedHiddenSectionCheckTime > this._repeatHiddenSectionCheckEveryXseconds)
		{
			this.HiddenSectionCheck();
			this._pastedHiddenSectionCheckTime = 0f;
		}
		if (this._pastedLicenceCheckTime > this._repeatLicenceCheckEveryXseconds)
		{
			this.LicenceCheck();
			this._pastedLicenceCheckTime = 0f;
		}
		if (this._updatePastedDataDirectoryCheckTime)
		{
			this._pastedDataDirectoryCheckTime += Time.deltaTime;
		}
		if (this._updatePastedCompareImageSizeCheckTime)
		{
			this._pastedCompareImageSizeCheckTime += Time.deltaTime;
		}
		if (this._updatePastedHiddenSectionCheckTime)
		{
			this._pastedHiddenSectionCheckTime += Time.deltaTime;
		}
		if (this._updatePastedLicenceCheckTime)
		{
			this._pastedLicenceCheckTime += Time.deltaTime;
		}
	}

	// Token: 0x06002AF1 RID: 10993 RVA: 0x0001BD4A File Offset: 0x00019F4A
	public void DataDirectoryCheck()
	{
		dss_api.DataDirectoryCheck();
	}

	// Token: 0x06002AF2 RID: 10994 RVA: 0x0001BD51 File Offset: 0x00019F51
	public void CompareImageSizeCheck()
	{
		dss_api.CompareImageSizeCheck();
	}

	// Token: 0x06002AF3 RID: 10995 RVA: 0x0001BD58 File Offset: 0x00019F58
	public void HiddenSectionCheck()
	{
		dss_api.HiddenSectionCheck();
	}

	// Token: 0x06002AF4 RID: 10996 RVA: 0x0001BD5F File Offset: 0x00019F5F
	public void LicenceCheck()
	{
		dss_api.LicenceCheck();
	}

	// Token: 0x0400371A RID: 14106
	private float _firstDataDirectoryCheckStartTime = 3f;

	// Token: 0x0400371B RID: 14107
	private float _firstCompareImageSizeCheckStartTime = 4f;

	// Token: 0x0400371C RID: 14108
	private float _firstHiddenSectionCheckStartTime = 5f;

	// Token: 0x0400371D RID: 14109
	private float _firstLicenceCheckStartTime = 6f;

	// Token: 0x0400371E RID: 14110
	private float _repeatDataDirectoryCheckEveryXseconds = 12f;

	// Token: 0x0400371F RID: 14111
	private float _repeatCompareImageSizeCheckEveryXseconds = 13f;

	// Token: 0x04003720 RID: 14112
	private float _repeatHiddenSectionCheckEveryXseconds = 14f;

	// Token: 0x04003721 RID: 14113
	private float _repeatLicenceCheckEveryXseconds = 15f;

	// Token: 0x04003722 RID: 14114
	private static SonySecurity _instance;

	// Token: 0x04003723 RID: 14115
	private static bool bQuit;

	// Token: 0x04003724 RID: 14116
	private float _pastedDataDirectoryCheckTime;

	// Token: 0x04003725 RID: 14117
	private float _pastedCompareImageSizeCheckTime;

	// Token: 0x04003726 RID: 14118
	private float _pastedHiddenSectionCheckTime;

	// Token: 0x04003727 RID: 14119
	private float _pastedLicenceCheckTime;

	// Token: 0x04003728 RID: 14120
	private bool _updatePastedDataDirectoryCheckTime;

	// Token: 0x04003729 RID: 14121
	private bool _updatePastedCompareImageSizeCheckTime;

	// Token: 0x0400372A RID: 14122
	private bool _updatePastedHiddenSectionCheckTime;

	// Token: 0x0400372B RID: 14123
	private bool _updatePastedLicenceCheckTime;
}
