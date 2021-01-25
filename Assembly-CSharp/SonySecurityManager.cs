using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000711 RID: 1809
public class SonySecurityManager : MonoBehaviour
{
	// Token: 0x06002AF7 RID: 10999 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x06002AF8 RID: 11000 RVA: 0x0001BD8B File Offset: 0x00019F8B
	private void OnEnable()
	{
		dss_api.CrackDetected = (dss_api.CrackDetectedDelegate)Delegate.Combine(dss_api.CrackDetected, new dss_api.CrackDetectedDelegate(SonySecurityManager.CrackDetectedCallback));
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x0001BDAD File Offset: 0x00019FAD
	private void OnDisable()
	{
		dss_api.CrackDetected = (dss_api.CrackDetectedDelegate)Delegate.Remove(dss_api.CrackDetected, new dss_api.CrackDetectedDelegate(SonySecurityManager.CrackDetectedCallback));
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x0014E178 File Offset: 0x0014C378
	private void Update()
	{
		if (Time.time > this._firstAssetsCheckStartTime && !this._updatePastedAssetsCheckTime)
		{
			this.CheckSonySecurityExistance();
			this._updatePastedAssetsCheckTime = true;
			this._timeToGoThroughAllChecksOnce = this._firstAssetsCheckStartTime;
			if (this._SonySecurity.FirstDataDirectoryCheckStartTime > this._timeToGoThroughAllChecksOnce)
			{
				this._timeToGoThroughAllChecksOnce = this._SonySecurity.FirstDataDirectoryCheckStartTime;
			}
			if (this._SonySecurity.FirstCompareImageSizeCheckStartTime > this._timeToGoThroughAllChecksOnce)
			{
				this._timeToGoThroughAllChecksOnce = this._SonySecurity.FirstCompareImageSizeCheckStartTime;
			}
			if (this._SonySecurity.FirstHiddenSectionCheckTime > this._timeToGoThroughAllChecksOnce)
			{
				this._timeToGoThroughAllChecksOnce = this._SonySecurity.FirstHiddenSectionCheckTime;
			}
			if (this._SonySecurity.FirstLicenceCheckStartTime > this._timeToGoThroughAllChecksOnce)
			{
				this._timeToGoThroughAllChecksOnce = this._SonySecurity.FirstLicenceCheckStartTime;
			}
		}
		if (this._pastedAssetsCheckTime > this._repeatAssetsCheckEverySeconds)
		{
			this.CheckSonySecurityExistance();
			this._pastedAssetsCheckTime = 0f;
		}
		if (this._updatePastedAssetsCheckTime)
		{
			this._pastedAssetsCheckTime += Time.deltaTime;
		}
		if (!this._allChecksGoneThroughOnce && this._SonySecurity != null)
		{
			if (this._pastedTimeToGoThroughAllChecksOnce > this._timeToGoThroughAllChecksOnce - this._firstAssetsCheckStartTime + 0.2f)
			{
				this._allChecksGoneThroughOnce = true;
			}
			else
			{
				this._pastedTimeToGoThroughAllChecksOnce += Time.deltaTime;
			}
		}
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x0014E2F0 File Offset: 0x0014C4F0
	private SonySecurity CheckSonySecurityExistance()
	{
		this._SonySecurity = (SonySecurity)Object.FindObjectOfType(typeof(SonySecurity));
		if (this._SonySecurity == null)
		{
			if (this._SonySecurityPrefab == null)
			{
				dss_api.AssetsFailed();
			}
			else
			{
				Transform transform = Object.Instantiate(this._SonySecurityPrefab) as Transform;
				transform.name = this._SonySecurityPrefab.name;
				this._SonySecurity = transform.GetComponent<SonySecurity>();
				if (this._SonySecurity == null)
				{
					dss_api.AssetsFailed();
				}
			}
		}
		return this._SonySecurity;
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x0014E390 File Offset: 0x0014C590
	public static void CrackDetectedCallback(dss_api.FailReason failReason, int id)
	{
		Helper.Print(string.Concat(new object[]
		{
			"CrackDetectedCallback with reason: ",
			failReason,
			" ID: ",
			id
		}));
		SonySecurityManager._crackDetected = true;
		Process.GetCurrentProcess().Kill();
	}

	// Token: 0x0400372C RID: 14124
	public Transform _SonySecurityPrefab;

	// Token: 0x0400372D RID: 14125
	private SonySecurity _SonySecurity;

	// Token: 0x0400372E RID: 14126
	private float _firstAssetsCheckStartTime = 1f;

	// Token: 0x0400372F RID: 14127
	private float _repeatAssetsCheckEverySeconds = 10f;

	// Token: 0x04003730 RID: 14128
	private float _pastedAssetsCheckTime;

	// Token: 0x04003731 RID: 14129
	private bool _updatePastedAssetsCheckTime;

	// Token: 0x04003732 RID: 14130
	private float _timeToGoThroughAllChecksOnce;

	// Token: 0x04003733 RID: 14131
	private float _pastedTimeToGoThroughAllChecksOnce;

	// Token: 0x04003734 RID: 14132
	private bool _allChecksGoneThroughOnce;

	// Token: 0x04003735 RID: 14133
	private bool _showAllChecksGoneThroughMsgBox = true;

	// Token: 0x04003736 RID: 14134
	private static bool _crackDetected;
}
