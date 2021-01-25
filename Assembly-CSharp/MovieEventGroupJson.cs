using System;
using System.Collections.Generic;

// Token: 0x02000286 RID: 646
public class MovieEventGroupJson
{
	// Token: 0x06000C0F RID: 3087 RVA: 0x0006359C File Offset: 0x0006179C
	public MovieEventGroup ToPlayMode()
	{
		MovieEventGroup movieEventGroup = new MovieEventGroup();
		movieEventGroup.ID = this.m_ID;
		movieEventGroup.strDesc = this.m_strDesc;
		for (int i = 0; i < this.m_movieEventNodeJsonList.Count; i++)
		{
			movieEventGroup.movieEventNodeList.Add(this.m_movieEventNodeJsonList[i].ToPlayMode());
		}
		return movieEventGroup;
	}

	// Token: 0x04000DE4 RID: 3556
	public int m_ID;

	// Token: 0x04000DE5 RID: 3557
	public string m_strDesc;

	// Token: 0x04000DE6 RID: 3558
	public List<MovieEventNodeJson> m_movieEventNodeJsonList = new List<MovieEventNodeJson>();
}
