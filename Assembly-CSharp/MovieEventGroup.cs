using System;
using System.Collections.Generic;
using System.IO;
using Heluo.Wulin;
using JsonFx.Json;
using UnityEngine;

// Token: 0x02000285 RID: 645
[Serializable]
public class MovieEventGroup
{
	// Token: 0x06000C0C RID: 3084 RVA: 0x0006349C File Offset: 0x0006169C
	public MovieEventGroupJson ToJson()
	{
		MovieEventGroupJson movieEventGroupJson = new MovieEventGroupJson();
		movieEventGroupJson.m_ID = this.ID;
		movieEventGroupJson.m_strDesc = this.strDesc;
		for (int i = 0; i < this.movieEventNodeList.Count; i++)
		{
			movieEventGroupJson.m_movieEventNodeJsonList.Add(this.movieEventNodeList[i].ToJson());
		}
		return movieEventGroupJson;
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x00063500 File Offset: 0x00061700
	public void Save(string strSceneName)
	{
		string text = string.Empty;
		if (Game.instance != null)
		{
			text = Game.g_strAssetDataPath + "Config/movie/" + strSceneName + "/";
		}
		else
		{
			text = Application.dataPath + "/Config/movie/" + strSceneName + "/";
		}
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		text = text + this.ID.ToString() + ".movie";
		JsonWriter jsonWriter = new JsonWriter(text);
		jsonWriter.Write(this.ToJson());
		jsonWriter.TextWriter.Close();
	}

	// Token: 0x04000DE1 RID: 3553
	public int ID;

	// Token: 0x04000DE2 RID: 3554
	public string strDesc;

	// Token: 0x04000DE3 RID: 3555
	public List<MovieEventNode> movieEventNodeList = new List<MovieEventNode>();
}
