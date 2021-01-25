using System;
using System.IO;
using Heluo.Wulin;
using JsonFx.Json;

// Token: 0x0200019A RID: 410
public class ActionListManager
{
	// Token: 0x0600087F RID: 2175 RVA: 0x0004D8BC File Offset: 0x0004BABC
	public ActionNode GetActionNode(int iID, int iType)
	{
		foreach (ActionNode actionNode in this.m_Group.m_ActionList)
		{
			if (actionNode.m_iID == iID && actionNode.m_iType == iType)
			{
				return actionNode;
			}
		}
		ActionNode actionNode2 = new ActionNode();
		actionNode2.m_iID = iID;
		actionNode2.m_iType = iType;
		actionNode2.m_iCount = 0;
		this.m_Group.m_ActionList.Add(actionNode2);
		return actionNode2;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x0004D964 File Offset: 0x0004BB64
	public void AddActionCount(int iID, int iType)
	{
		ActionNode actionNode = this.GetActionNode(iID, iType);
		actionNode.m_iCount++;
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x0004D988 File Offset: 0x0004BB88
	public void LoadFromFile(string filename)
	{
		string text = Game.g_strAssetDataPath + "Config/SaveData/" + filename;
		if (!File.Exists(text))
		{
			this.m_Group.m_ActionList.Clear();
			this.m_Group.m_strFileName = filename;
		}
		else
		{
			this.m_Group.m_ActionList.Clear();
			StreamReader streamReader = new StreamReader(text);
			JsonReader jsonReader = new JsonReader(streamReader);
			this.m_Group = jsonReader.Deserialize<ActionGroup>();
			streamReader.Close();
		}
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x0004DA04 File Offset: 0x0004BC04
	public void SaveToFile()
	{
		if (this.m_Group.m_strFileName == string.Empty)
		{
			return;
		}
		string text = Game.g_strAssetDataPath + "Config/SaveData/" + this.m_Group.m_strFileName;
		JsonWriter jsonWriter = new JsonWriter(text);
		jsonWriter.Write(this.m_Group);
		jsonWriter.TextWriter.Close();
	}

	// Token: 0x0400084C RID: 2124
	public ActionGroup m_Group = new ActionGroup();
}
