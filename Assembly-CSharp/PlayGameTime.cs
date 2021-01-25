using System;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x0200039C RID: 924
public class PlayGameTime
{
	// Token: 0x06001562 RID: 5474 RVA: 0x000B66FC File Offset: 0x000B48FC
	public string TimeFormt()
	{
		string empty = string.Empty;
		this.m_first = ((this.m_first >= 0) ? this.m_first : 0);
		int num = Mathf.FloorToInt((float)(this.m_first / 3600));
		string text = ((this.m_first - num * 3600) / 60).ToString();
		return num.ToString() + " : " + text;
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x0000D8C8 File Offset: 0x0000BAC8
	public void SetTotal()
	{
		this.m_first += this.m_FinalTime - this.m_StartTime;
		this.m_StartTime = this.m_FinalTime;
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x000B6770 File Offset: 0x000B4970
	public PlayGameTime Clone()
	{
		return new PlayGameTime
		{
			m_StartTime = this.m_StartTime,
			m_first = this.m_first,
			m_FinalTime = this.m_FinalTime
		};
	}

	// Token: 0x04001A09 RID: 6665
	public int m_first;

	// Token: 0x04001A0A RID: 6666
	[JsonIgnore]
	public int m_StartTime;

	// Token: 0x04001A0B RID: 6667
	[JsonIgnore]
	public int m_FinalTime;
}
