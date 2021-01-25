using System;
using System.Collections.Generic;
using System.IO;
using Heluo.Wulin.UI;
using JsonFx.Json;
using UnityEngine;

// Token: 0x0200038E RID: 910
public class ReSetMapNpc : MonoBehaviour
{
	// Token: 0x0600152B RID: 5419 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x000B5DCC File Offset: 0x000B3FCC
	private void Update()
	{
		if (this.CreateEventCube)
		{
			this.CreateEventCube = false;
		}
		if (this.CreateMouseEventCube)
		{
			MouseEventCubeData[] array = null;
			string text = Application.dataPath + "/Config/mouseeventcube/" + this.SceneName + ".msevc";
			if (File.Exists(text))
			{
				StreamReader streamReader = new StreamReader(text);
				JsonReader jsonReader = new JsonReader(streamReader);
				streamReader.Close();
				array = jsonReader.Deserialize<MouseEventCubeData[]>();
			}
			if (array != null)
			{
				MouseEventCube.m_MouseEventCubeList.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					MouseEventCube mouseEventCube = array[i].CreateToGameObject();
				}
			}
			this.CreateMouseEventCube = false;
		}
	}

	// Token: 0x040019D3 RID: 6611
	public bool ReLoadMapNpc;

	// Token: 0x040019D4 RID: 6612
	public bool ReLoadMapTxtData;

	// Token: 0x040019D5 RID: 6613
	public bool ReLoadTeam;

	// Token: 0x040019D6 RID: 6614
	public bool SetQuest;

	// Token: 0x040019D7 RID: 6615
	public bool GetItem;

	// Token: 0x040019D8 RID: 6616
	public bool CreateEventCube;

	// Token: 0x040019D9 RID: 6617
	public bool CreateMouseEventCube;

	// Token: 0x040019DA RID: 6618
	public string SceneName;

	// Token: 0x040019DB RID: 6619
	public bool CheckMapNpcOpen;

	// Token: 0x040019DC RID: 6620
	public int m_iMoney;

	// Token: 0x040019DD RID: 6621
	public List<int> Team = new List<int>();

	// Token: 0x040019DE RID: 6622
	public List<string> CollectQuest = new List<string>();

	// Token: 0x040019DF RID: 6623
	public List<string> Quest = new List<string>();

	// Token: 0x040019E0 RID: 6624
	public List<int> Item = new List<int>();
}
