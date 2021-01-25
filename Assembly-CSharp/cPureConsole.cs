using System;
using System.Collections.Generic;
using System.Text;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x0200039F RID: 927
public class cPureConsole : MonoBehaviour
{
	// Token: 0x06001581 RID: 5505 RVA: 0x0000DB09 File Offset: 0x0000BD09
	private void OnEnable()
	{
		this.GT = cPureConsole.GridType.Select;
		Application.RegisterLogCallback(new Application.LogCallback(this.HandleLog));
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x0000DB23 File Offset: 0x0000BD23
	private void OnDisable()
	{
		Application.RegisterLogCallback(null);
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x000B7218 File Offset: 0x000B5418
	private void Start()
	{
		this.m_gsStyleLabel.wordWrap = true;
		if (this.isVisible)
		{
			this.show(false);
		}
		this.TeammateNameText[9] = "閱歷點";
		this.TeammateNameText[10] = "錢";
		this.TryCommand();
	}

	// Token: 0x06001584 RID: 5508 RVA: 0x000B7268 File Offset: 0x000B5468
	private void Update()
	{
		if (this.m_iScreenWCurn != Screen.width || this.m_iScreenHCurn != Screen.height)
		{
			float num = (float)Screen.height / 1080f;
			this.m_rcWindowRect = new Rect(this.m_rcWindowRect.x * num, this.m_rcWindowRect.y * num, this.m_fConsoleW * num, this.m_fConsoleH * num);
			this.m_gsStyleLabel.fontSize = (int)(24f * num + 0.5f);
			this.m_iScreenWCurn = Screen.width;
			this.m_iScreenHCurn = Screen.height;
		}
	}

	// Token: 0x06001585 RID: 5509 RVA: 0x000B7308 File Offset: 0x000B5508
	private void OnGUI()
	{
		this.m_rcWindowRect = GUILayout.Window(2329889, this.m_rcWindowRect, new GUI.WindowFunction(this.ConsoleWindow), "Console", new GUILayoutOption[0]);
		Event current = Event.current;
		if (current.type == 4 && current.keyCode == 13)
		{
			this.TryCommand();
		}
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x000B7368 File Offset: 0x000B5568
	private void TryCommand()
	{
		if (this.strCommand.IndexOf("/i") < 0)
		{
			this.HandleLog(this.strCommand, null, 5);
		}
		else
		{
			int length = this.strCommand.Length;
			int num = this.strCommand.IndexOf("/i");
			if (num + 3 < length)
			{
				string str = this.strCommand.Substring(num + 3, length - num - 3);
				this.DoCommand(str);
			}
		}
		this.strCommand = string.Empty;
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x000B73EC File Offset: 0x000B55EC
	private void DoCommand(string str)
	{
		string text = str.ToLower();
		if (text.IndexOf("/h") >= 0)
		{
			this.HandleLog("增加道具 item物品ID,數量 /i item100001,10 ", null, 5);
			this.HandleLog("已完成任務 list1", null, 5);
			this.HandleLog("進行中任務 list2", null, 5);
			this.HandleLog("已撥的MOVIE list3", null, 5);
		}
		if (text.IndexOf("map") >= 0)
		{
			this.iPopSelectIndex = 0;
			this.SetMapArray();
			this.GT = cPureConsole.GridType.Map;
			return;
		}
		if (text.IndexOf("autosave") >= 0)
		{
			GameGlobal.m_OpenAutoSave = !GameGlobal.m_OpenAutoSave;
			if (GameGlobal.m_OpenAutoSave)
			{
				this.HandleLog("自動存檔已開啟", null, 5);
			}
			else
			{
				this.HandleLog("自動存檔已關閉", null, 5);
			}
			return;
		}
		if (text.IndexOf("npcitem") >= 0)
		{
			text = text.Replace("npcitem", string.Empty);
			string[] array = text.Split(new char[]
			{
				",".get_Chars(0)
			});
			int npcId = int.Parse(array[0]);
			int itemID = int.Parse(array[1]);
			NPC.m_instance.AddNpcItem(npcId, itemID);
			return;
		}
		if (text.IndexOf("resetnpcdo") >= 0)
		{
			foreach (CharacterData characterData in NPC.m_instance.NpcList)
			{
				if (characterData.NpcType == eNPCType.NothingCanDo)
				{
					characterData.NpcType = eNPCType.Nothing;
				}
			}
			return;
		}
		if (text.IndexOf("item") >= 0)
		{
			this.HandleLog("取得資料中----------------------------------------", null, 5);
			text = text.Replace("item", string.Empty);
			string[] array2 = text.Split(new char[]
			{
				",".get_Chars(0)
			});
			int num = int.Parse(array2[0]);
			int num2;
			if (!int.TryParse(array2[1], ref num2))
			{
				num2 = 1;
			}
			BackpackStatus.m_Instance.AddPackItem(num, num2, true);
			string itemName = Game.ItemData.GetItemName(num);
			this.HandleLog(string.Concat(new object[]
			{
				"新增物品 ",
				num,
				"    ",
				itemName,
				"  ",
				num2,
				"   個"
			}), null, 5);
			return;
		}
		if (text.IndexOf("battle") >= 0)
		{
			text = text.Replace("battle", string.Empty);
			GameObject gameObject = GameObject.FindWithTag("Player");
			if (gameObject != null)
			{
				GameGlobal.m_TransferPos = gameObject.transform.position;
			}
			int iBattleID = int.Parse(text.Trim());
			Game.g_BattleControl.StartBattle(iBattleID);
			return;
		}
		if (text.IndexOf("rcq") >= 0)
		{
			text = text.Replace("rcq", string.Empty);
			text = text.Trim().ToUpper();
			MissionStatus.m_instance.RemoveCollectionQuest(text);
			return;
		}
		if (text.IndexOf("cq") >= 0)
		{
			text = text.Replace("cq", string.Empty);
			text = text.Trim().ToUpper();
			MissionStatus.m_instance.AddCollectionQuest(text);
			return;
		}
		if (text.IndexOf("rq") >= 0)
		{
			text = text.Replace("rq", string.Empty);
			text = text.Trim().ToUpper();
			MissionStatus.m_instance.RemoveQuest(text);
			return;
		}
		if (text.IndexOf("q") >= 0)
		{
			text = text.Replace("q", string.Empty);
			text = text.Trim().ToUpper();
			MissionStatus.m_instance.AddQuestList(text);
			return;
		}
		if (text.IndexOf("list2") >= 0)
		{
			this.HandleLog("進行中任務----------------------------------------", null, 5);
			if (MissionStatus.m_instance != null)
			{
				foreach (QuestStatus questStatus in MissionStatus.m_instance.QuestList)
				{
					if (questStatus.iType == 0)
					{
						string text2 = questStatus.m_strQuestID;
						QuestNode questNode = Game.QuestData.GetQuestNode(text2);
						text2 = text2 + "  " + questNode.m_strQuestName;
						this.HandleLog(text2, null, 5);
					}
				}
			}
			this.HandleLog("進行中任務----------------------------------------", null, 5);
			return;
		}
		if (text.IndexOf("list1") >= 0)
		{
			this.HandleLog("已完成任務----------------------------------------", null, 5);
			if (MissionStatus.m_instance != null)
			{
				foreach (QuestStatus questStatus2 in MissionStatus.m_instance.QuestList)
				{
					if (questStatus2.iType == 1)
					{
						string text3 = questStatus2.m_strQuestID;
						QuestNode questNode2 = Game.QuestData.GetQuestNode(text3);
						text3 = text3 + "  " + questNode2.m_strQuestName;
						this.HandleLog(text3, null, 5);
					}
				}
			}
			this.HandleLog("已完成任務----------------------------------------", null, 5);
			return;
		}
		if (text.IndexOf("list3") >= 0)
		{
			this.HandleLog("已撥的MovieEvent---------------------------------", null, 5);
			if (TeamStatus.m_Instance != null)
			{
				foreach (int num3 in TeamStatus.m_Instance.m_EventList)
				{
					this.HandleLog(num3.ToString(), null, 5);
				}
			}
			this.HandleLog("已撥的MovieEvent---------------------------------", null, 5);
			return;
		}
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x000B79CC File Offset: 0x000B5BCC
	private void ClearArray()
	{
		for (int i = 0; i < this.RoutineArray.Length; i++)
		{
			this.RoutineArray[i] = "沒東西";
		}
		for (int i = 0; i < this.NeigongArray.Length; i++)
		{
			this.NeigongArray[i] = "沒東西";
		}
		for (int i = 0; i < this.TalentArray.Length; i++)
		{
			this.TalentArray[i] = "0";
		}
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x000B7A50 File Offset: 0x000B5C50
	private void updataArray()
	{
		this.ClearArray();
		for (int i = 0; i < this.CD.RoutineList.Count; i++)
		{
			this.RoutineArray[i] = this.CD.RoutineList[i].m_Routine.m_strRoutineName;
		}
		for (int i = 0; i < this.CD.NeigongList.Count; i++)
		{
			this.NeigongArray[i] = this.CD.NeigongList[i].m_Neigong.m_strNeigongName;
		}
		for (int i = 0; i < this.CD.TalentList.Count; i++)
		{
			string talentName = Game.TalentNewData.GetTalentName(this.CD.TalentList[i]);
			this.TalentArray[i] = talentName;
		}
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x000B7B34 File Offset: 0x000B5D34
	private void SetMapArray()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		for (int i = 0; i < Game.MapID.m_MapIDNodeList.Count; i++)
		{
			if (!(Game.MapID.m_MapIDNodeList[i].m_strMapID != "M0000_01") || !(Game.MapID.m_MapIDNodeList[i].Pos == Vector3.zero))
			{
				list.Add(Game.MapID.m_MapIDNodeList[i].m_strMapName);
				list2.Add(Game.MapID.m_MapIDNodeList[i].m_strMapID);
			}
		}
		this.MapName = list.ToArray();
		this.MapID = list2.ToArray();
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x000B7C08 File Offset: 0x000B5E08
	private void SetTeamArray()
	{
		for (int i = 0; i < this.TeammateNameText.Length - 1; i++)
		{
			this.TeammateNameText[i] = string.Empty;
		}
		List<int> teamMemberIDList = TeamStatus.m_Instance.GetTeamMemberIDList();
		for (int j = 0; j < teamMemberIDList.Count; j++)
		{
			this.TeammateID[j] = teamMemberIDList[j];
			this.TeammateNameText[j] = Game.NpcData.GetNpcName(teamMemberIDList[j]);
		}
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x0000DB2B File Offset: 0x0000BD2B
	private bool GUIKeyDown(KeyCode key)
	{
		return Event.current.type == 4 && Event.current.keyCode == key;
	}

	// Token: 0x0600158D RID: 5517 RVA: 0x000B7C88 File Offset: 0x000B5E88
	private void ConsoleWindow(int windowID)
	{
		this.m_vecScrollPos = GUILayout.BeginScrollView(this.m_vecScrollPos, new GUILayoutOption[0]);
		int count = this.m_EntryList.Count;
		int i = 0;
		while (i < count)
		{
			cPureConsole.ConsoleMessage consoleMessage = this.m_EntryList[i];
			switch (consoleMessage.logType)
			{
			case 0:
			case 4:
				this.m_gsStyleLabel.normal.textColor = Color.red;
				GUILayout.Label(string.Format("{0}\n{1}", consoleMessage.message, consoleMessage.stackTrace), this.m_gsStyleLabel, new GUILayoutOption[0]);
				break;
			case 5:
				this.m_gsStyleLabel.normal.textColor = Color.white;
				GUILayout.Label(consoleMessage.message, this.m_gsStyleLabel, new GUILayoutOption[0]);
				break;
			}
			IL_E0:
			i++;
			continue;
			goto IL_E0;
		}
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Button(this.m_gcClearLabel, new GUILayoutOption[0]))
		{
			this.clearLog();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.strCommand = GUILayout.TextField(this.strCommand, new GUILayoutOption[0]);
		if (GUILayout.Button(this.m_gcCommandLabel, new GUILayoutOption[0]))
		{
			this.TryCommand();
		}
		GUILayout.EndHorizontal();
		int num = 0;
		if (this.GT == cPureConsole.GridType.Select)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			this.iPopSelectIndex = GUILayout.SelectionGrid(this.iPopSelectIndex, this.eTypeText, 5, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (this.iPopSelectIndex == 4)
			{
				string text = "現在數值" + GameGlobal.m_iBattleDifficulty.ToString();
				GUILayout.Label(text, new GUILayoutOption[0]);
				this.strNewValue = GUILayout.TextField(this.strNewValue, new GUILayoutOption[0]);
			}
			if (GUILayout.Button("設定", new GUILayoutOption[0]))
			{
				switch (this.iPopSelectIndex)
				{
				case 0:
					this.SetMapArray();
					this.GT = cPureConsole.GridType.Map;
					this.iPopSelectIndex = 0;
					break;
				case 1:
					this.GT = cPureConsole.GridType.Property;
					this.iPopSelectIndex = 0;
					break;
				case 2:
					this.SetTeamArray();
					this.GT = cPureConsole.GridType.Team;
					this.iPopSelectIndex = 0;
					break;
				case 3:
					this.HandleLog(" 還沒做   ", null, 5);
					this.iPopSelectIndex = 0;
					break;
				case 4:
				{
					int iBattleDifficulty;
					if (!int.TryParse(this.strNewValue, ref iBattleDifficulty))
					{
						iBattleDifficulty = 0;
					}
					GameGlobal.m_iBattleDifficulty = iBattleDifficulty;
					break;
				}
				}
			}
			GUILayout.EndHorizontal();
		}
		else if (this.GT == cPureConsole.GridType.Map)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			this.iPopSelectIndex = GUILayout.SelectionGrid(this.iPopSelectIndex, this.MapName, 5, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			if (GUILayout.Button("回上一層", new GUILayoutOption[]
			{
				GUILayout.Width(this.m_rcWindowRect.width * 0.25f)
			}))
			{
				this.GT = cPureConsole.GridType.Select;
				this.strNewValue = string.Empty;
			}
			if (GUILayout.Button("傳送", new GUILayoutOption[0]))
			{
				if (this.iPopSelectIndex < this.MapName.Length)
				{
					MapIDNode mapIDNode = Game.MapID.GetMapIDNode(this.MapID[this.iPopSelectIndex]);
					if (mapIDNode != null)
					{
						GameGlobal.m_TransferPos = mapIDNode.Pos;
						GameGlobal.m_fDir = mapIDNode.AngleY;
						Game.LoadScene(this.MapID[this.iPopSelectIndex]);
					}
				}
				this.strNewValue = string.Empty;
			}
		}
		else if (this.GT == cPureConsole.GridType.Team)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			this.iPopSelectIndex = GUILayout.SelectionGrid(this.iPopSelectIndex, this.TeammateNameText, 5, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			if (GUILayout.Button("回上一層", new GUILayoutOption[]
			{
				GUILayout.Width(this.m_rcWindowRect.width * 0.25f)
			}))
			{
				this.GT = cPureConsole.GridType.Select;
				this.strNewValue = string.Empty;
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (this.iPopSelectIndex < 9)
			{
				if (this.iPopSelectIndex < this.TeammateID.Length)
				{
					num = this.TeammateID[this.iPopSelectIndex];
				}
			}
			else if (this.iPopSelectIndex == 9)
			{
				num = TeamStatus.m_Instance.GetAttributePoints();
			}
			else if (this.iPopSelectIndex == 10)
			{
				num = BackpackStatus.m_Instance.GetMoney();
			}
			string text2 = "現在數值" + num.ToString();
			GUILayout.Label(text2, new GUILayoutOption[0]);
			this.strNewValue = GUILayout.TextField(this.strNewValue, new GUILayoutOption[0]);
			if (GUILayout.Button("Set", new GUILayoutOption[0]))
			{
				int num2;
				if (!int.TryParse(this.strNewValue, ref num2))
				{
					num2 = 0;
				}
				if (this.iPopSelectIndex < 9)
				{
					if (this.iPopSelectIndex < this.TeammateID.Length)
					{
						if (num > 0)
						{
							this.CD = NPC.m_instance.GetCharacterData(this.TeammateID[this.iPopSelectIndex]);
							this.HandleLog(this.CD._NpcDataNode.m_strNpcName + " 資料取得", null, 5);
							this.iPopSelectIndex = 0;
							this.GT = cPureConsole.GridType.Property;
						}
						else
						{
							TeamStatus.m_Instance.AddTeamMember(num2);
							this.SetTeamArray();
						}
					}
				}
				else if (this.iPopSelectIndex == 9)
				{
					TeamStatus.m_Instance.ChangeAttributePoints(num2);
					this.HandleLog("閱歷點以增加" + num2 + "點", null, 5);
				}
				else if (this.iPopSelectIndex == 10)
				{
					BackpackStatus.m_Instance.ChangeMoney(num2);
					this.HandleLog("金錢以調整 " + num2 + "   ", null, 5);
				}
				this.strNewValue = string.Empty;
			}
			GUILayout.EndHorizontal();
		}
		else if (this.GT == cPureConsole.GridType.Property)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			this.iPopSelectIndex = GUILayout.SelectionGrid(this.iPopSelectIndex, this.iPopText, 10, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (this.CD != null)
			{
				if (this.iPopSelectIndex < 45)
				{
					if (this.iPopSelectIndex < this.Property.Length)
					{
						num = this.CD.GetValue(this.Property[this.iPopSelectIndex]);
					}
				}
				else if (this.iPopSelectIndex == 45)
				{
					num = this.CD.RoutineList.Count;
				}
				else if (this.iPopSelectIndex == 46)
				{
					num = this.CD.NeigongList.Count;
				}
				else if (this.iPopSelectIndex == 47)
				{
					num = this.CD.TalentList.Count;
				}
				else if (this.iPopSelectIndex == 48)
				{
					num = 0;
				}
				else if (this.iPopSelectIndex == 49)
				{
					num = 0;
				}
			}
			string text3 = "現在數值" + num.ToString();
			GUILayout.Label(text3, new GUILayoutOption[0]);
			this.strNewValue = GUILayout.TextField(this.strNewValue, new GUILayoutOption[0]);
			if (GUILayout.Button("設定", new GUILayoutOption[0]))
			{
				if (this.CD != null)
				{
					int num3;
					if (!int.TryParse(this.strNewValue, ref num3))
					{
						num3 = 0;
					}
					if (this.iPopSelectIndex < 45)
					{
						NPC.m_instance.SetCharacterProperty(this.CD.iNpcID, (int)this.Property[this.iPopSelectIndex], num3);
					}
					else
					{
						switch (this.iPopSelectIndex)
						{
						case 45:
							this.iPopSelectIndex = 0;
							this.GT = cPureConsole.GridType.Routine;
							break;
						case 46:
							this.iPopSelectIndex = 0;
							this.GT = cPureConsole.GridType.Neigong;
							break;
						case 47:
							this.iPopSelectIndex = 0;
							this.GT = cPureConsole.GridType.Talent;
							break;
						case 48:
							this.iPopSelectIndex = 0;
							this.CD.setTotalProperty();
							break;
						case 49:
							this.iPopSelectIndex = 0;
							this.GT = cPureConsole.GridType.Select;
							break;
						}
					}
				}
				else if (this.iPopSelectIndex != 49)
				{
					int num3;
					if (!int.TryParse(this.strNewValue, ref num3))
					{
						num3 = 0;
					}
					this.CD = NPC.m_instance.GetCharacterData(num3);
					if (this.CD != null)
					{
						this.HandleLog(this.CD._NpcDataNode.m_strNpcName + " 資料取得", null, 5);
						this.updataArray();
					}
					else
					{
						this.HandleLog("無取得資料  " + this.strNewValue, null, 5);
					}
				}
				else
				{
					this.GT = cPureConsole.GridType.Select;
				}
				this.strNewValue = string.Empty;
			}
			GUILayout.EndHorizontal();
		}
		else
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			if (this.CD != null && this.GT != cPureConsole.GridType.Map && this.GT != cPureConsole.GridType.Item)
			{
				if (GUILayout.Button("人物資料", new GUILayoutOption[0]))
				{
					this.iPopSelectIndex = 0;
					this.GT = cPureConsole.GridType.Property;
				}
				if (GUILayout.Button("武學", new GUILayoutOption[0]))
				{
					this.iPopSelectIndex = 0;
					this.GT = cPureConsole.GridType.Routine;
					this.updataArray();
				}
				if (GUILayout.Button("內功", new GUILayoutOption[0]))
				{
					this.iPopSelectIndex = 0;
					this.GT = cPureConsole.GridType.Neigong;
					this.updataArray();
				}
				if (GUILayout.Button("天賦", new GUILayoutOption[0]))
				{
					this.iPopSelectIndex = 0;
					this.GT = cPureConsole.GridType.Talent;
					this.updataArray();
				}
			}
			GUILayout.EndHorizontal();
			if (this.GT == cPureConsole.GridType.Routine)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				this.iPopSelectIndex = GUILayout.SelectionGrid(this.iPopSelectIndex, this.RoutineArray, 6, new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (this.CD != null && this.iPopSelectIndex < 12)
				{
					if (this.iPopSelectIndex < this.CD.RoutineList.Count)
					{
						num = this.CD.RoutineList[this.iPopSelectIndex].iLevel;
					}
					else
					{
						num = -1;
					}
				}
				string text4 = "現在數值" + num.ToString();
				GUILayout.Label(text4, new GUILayoutOption[0]);
				this.strNewValue = GUILayout.TextField(this.strNewValue, new GUILayoutOption[0]);
				if (GUILayout.Button("設定", new GUILayoutOption[0]))
				{
					int num4;
					if (!int.TryParse(this.strNewValue, ref num4))
					{
						num4 = 0;
					}
					if (this.iPopSelectIndex < 12 && num4 != 0)
					{
						if (num > 0)
						{
							if (num4 > 0)
							{
								if (num4 >= 10)
								{
									num4 = 10;
								}
								this.CD.RoutineList[this.iPopSelectIndex].SetLv(num4);
								this.HandleLog(string.Concat(new object[]
								{
									this.CD._NpcDataNode.m_strNpcName,
									" 的",
									this.CD.RoutineList[this.iPopSelectIndex].m_Routine.m_strRoutineName,
									" 重數提高到 ",
									num4
								}), null, 5);
							}
							else
							{
								this.HandleLog(this.CD._NpcDataNode.m_strNpcName + " 遺忘了" + this.CD.RoutineList[this.iPopSelectIndex].m_Routine.m_strRoutineName, null, 5);
								this.CD.RoutineList.Remove(this.CD.RoutineList[this.iPopSelectIndex]);
								this.updataArray();
							}
						}
						else
						{
							bool flag = NPC.m_instance.AddRoutine(this.CD.iNpcID, num4, 1);
							if (flag)
							{
								this.updataArray();
								this.iPopSelectIndex = this.CD.RoutineList.Count - 1;
								this.HandleLog(this.CD._NpcDataNode.m_strNpcName + " 學會了" + this.CD.RoutineList[this.iPopSelectIndex].m_Routine.m_strRoutineName, null, 5);
							}
							else
							{
								this.HandleLog("失敗 ?????   " + num4, null, 5);
							}
						}
					}
					this.strNewValue = string.Empty;
				}
				if (GUILayout.Button("查詢套路", new GUILayoutOption[0]))
				{
					List<RoutineNewDataNode> routineList = Game.RoutineNewData.GetRoutineList();
					foreach (RoutineNewDataNode routineNewDataNode in routineList)
					{
						this.HandleLog(routineNewDataNode.m_iRoutineID + "       " + routineNewDataNode.m_strRoutineName, null, 5);
					}
				}
				GUILayout.EndHorizontal();
			}
			else if (this.GT == cPureConsole.GridType.Neigong)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				this.iPopSelectIndex = GUILayout.SelectionGrid(this.iPopSelectIndex, this.NeigongArray, 6, new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (this.iPopSelectIndex < this.CD.NeigongList.Count)
				{
					num = this.CD.NeigongList[this.iPopSelectIndex].iLevel;
				}
				else
				{
					num = -1;
				}
				string text5 = "現在數值" + num.ToString();
				GUILayout.Label(text5, new GUILayoutOption[0]);
				this.strNewValue = GUILayout.TextField(this.strNewValue, new GUILayoutOption[0]);
				if (GUILayout.Button("設定", new GUILayoutOption[0]))
				{
					int num5;
					if (!int.TryParse(this.strNewValue, ref num5))
					{
						num5 = 0;
					}
					if (this.iPopSelectIndex < 12 && num5 != 0)
					{
						if (num > 0)
						{
							if (num5 > 0)
							{
								if (num5 >= 10)
								{
									num5 = 10;
								}
								this.CD.NeigongList[this.iPopSelectIndex].SetLv(num5);
								this.HandleLog(string.Concat(new object[]
								{
									this.CD._NpcDataNode.m_strNpcName,
									" 的",
									this.CD.RoutineList[this.iPopSelectIndex].m_Routine.m_strRoutineName,
									" 重數提高到 ",
									num5
								}), null, 5);
							}
							else
							{
								this.HandleLog(this.CD._NpcDataNode.m_strNpcName + " 遺忘了" + this.CD.NeigongList[this.iPopSelectIndex].m_Neigong.m_strNeigongName, null, 5);
								this.CD.NeigongList.Remove(this.CD.NeigongList[this.iPopSelectIndex]);
								this.updataArray();
							}
						}
						else
						{
							bool flag2 = NPC.m_instance.AddNeigong(this.CD.iNpcID, num5, 1);
							if (flag2)
							{
								this.updataArray();
								this.iPopSelectIndex = this.CD.NeigongList.Count - 1;
								this.HandleLog(this.CD._NpcDataNode.m_strNpcName + " 學會了" + this.CD.NeigongList[this.iPopSelectIndex].m_Neigong.m_strNeigongName, null, 5);
							}
							else
							{
								this.HandleLog("失敗 ?????   " + num5, null, 5);
							}
						}
					}
					this.strNewValue = string.Empty;
				}
				if (GUILayout.Button("查詢內功", new GUILayoutOption[0]))
				{
					List<NeigongNewDataNode> neigongList = Game.NeigongData.GetNeigongList();
					foreach (NeigongNewDataNode neigongNewDataNode in neigongList)
					{
						this.HandleLog(neigongNewDataNode.m_iNeigongID + "       " + neigongNewDataNode.m_strNeigongName, null, 5);
					}
				}
				GUILayout.EndHorizontal();
			}
			else if (this.GT == cPureConsole.GridType.Talent)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				this.iPopSelectIndex = GUILayout.SelectionGrid(this.iPopSelectIndex, this.TalentArray, 3, new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (this.iPopSelectIndex < this.CD.TalentList.Count)
				{
					num = this.CD.TalentList[this.iPopSelectIndex];
				}
				else
				{
					num = -1;
				}
				string text6 = "現在數值" + num.ToString();
				GUILayout.Label(text6, new GUILayoutOption[0]);
				this.strNewValue = GUILayout.TextField(this.strNewValue, new GUILayoutOption[0]);
				if (GUILayout.Button("設定", new GUILayoutOption[0]))
				{
					int num6;
					if (!int.TryParse(this.strNewValue, ref num6))
					{
						num6 = 0;
					}
					if (this.iPopSelectIndex < 4)
					{
						if (num6 > 0)
						{
							bool flag3 = NPC.m_instance.AddTalent(this.CD.iNpcID, num6);
							if (flag3)
							{
								string talentName = Game.TalentNewData.GetTalentName(num6);
								this.updataArray();
								this.HandleLog(this.CD._NpcDataNode.m_strNpcName + " 領悟   " + talentName, null, 5);
							}
							else
							{
								this.HandleLog(this.CD._NpcDataNode.m_strNpcName + " 他不懂這串數字  " + num6, null, 5);
							}
						}
						else
						{
							this.CD.TalentList.Remove(num6);
							string talentName2 = Game.TalentNewData.GetTalentName(num6);
							this.updataArray();
							this.HandleLog(this.CD._NpcDataNode.m_strNpcName + " 被全身麻醉後忘了   " + talentName2, null, 5);
						}
					}
					this.strNewValue = string.Empty;
				}
				if (GUILayout.Button("查詢天賦", new GUILayoutOption[0]))
				{
					List<TalentNewDataNode> talentList = Game.TalentNewData.GetTalentList();
					foreach (TalentNewDataNode talentNewDataNode in talentList)
					{
						this.HandleLog(talentNewDataNode.m_iTalentID + "       " + talentNewDataNode.m_strTalentName, null, 5);
					}
				}
				GUILayout.EndHorizontal();
			}
		}
		GUI.DragWindow(new Rect(0f, 0f, this.m_rcWindowRect.width, this.m_rcWindowRect.height));
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x000B8FA8 File Offset: 0x000B71A8
	private void HandleLog(string strMessage, string strStackTrace, LogType logType)
	{
		this.m_STRBuilder.Length = 0;
		switch (logType)
		{
		case 0:
			this.m_STRBuilder.AppendFormat("\n<Error>:{0}\n{1}</Error>", strMessage, strStackTrace);
			goto IL_86;
		case 2:
			this.m_STRBuilder.AppendFormat("\n<Warning>:{0}</Warning>", strMessage);
			goto IL_86;
		case 4:
			this.m_STRBuilder.AppendFormat("\n<Exception>:{0}\n{1}</Exception>", strMessage, strStackTrace);
			goto IL_86;
		}
		this.m_STRBuilder.Append(strMessage);
		IL_86:
		cPureConsole.ConsoleMessage consoleMessage = new cPureConsole.ConsoleMessage(strMessage, strStackTrace, logType);
		this.m_EntryList.Add(consoleMessage);
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x0000DB4C File Offset: 0x0000BD4C
	public void toggleVisible()
	{
		this.show(!this.isVisible);
	}

	// Token: 0x06001590 RID: 5520 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
	public void show(bool bShow)
	{
		base.enabled = bShow;
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06001591 RID: 5521 RVA: 0x0000DB01 File Offset: 0x0000BD01
	public bool isVisible
	{
		get
		{
			return base.enabled;
		}
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x0000DB5D File Offset: 0x0000BD5D
	public void clearLog()
	{
		this.m_EntryList.Clear();
	}

	// Token: 0x04001A18 RID: 6680
	private const int k_iMargin = 20;

	// Token: 0x04001A19 RID: 6681
	private StringBuilder m_STRBuilder = new StringBuilder();

	// Token: 0x04001A1A RID: 6682
	private List<cPureConsole.ConsoleMessage> m_EntryList = new List<cPureConsole.ConsoleMessage>();

	// Token: 0x04001A1B RID: 6683
	private Vector2 m_vecScrollPos;

	// Token: 0x04001A1C RID: 6684
	private int m_iScreenWCurn = 800;

	// Token: 0x04001A1D RID: 6685
	private int m_iScreenHCurn = 600;

	// Token: 0x04001A1E RID: 6686
	private float m_fConsoleW = 1344f;

	// Token: 0x04001A1F RID: 6687
	private float m_fConsoleH = 918f;

	// Token: 0x04001A20 RID: 6688
	private Rect m_rcWindowRect = new Rect(20f, 20f, (float)(Screen.width - 40), (float)(Screen.height - 40));

	// Token: 0x04001A21 RID: 6689
	private GUIContent m_gcClearLabel = new GUIContent("Clear", "Clear the contents of the console.");

	// Token: 0x04001A22 RID: 6690
	private GUIContent m_gcCommandLabel = new GUIContent("Command", "Command");

	// Token: 0x04001A23 RID: 6691
	private GUIStyle m_gsStyleLabel = new GUIStyle();

	// Token: 0x04001A24 RID: 6692
	private CharacterData.PropertyType[] Property = new CharacterData.PropertyType[]
	{
		CharacterData.PropertyType.CurHP,
		CharacterData.PropertyType.MaxHP,
		CharacterData.PropertyType.CurSP,
		CharacterData.PropertyType.MaxSP,
		CharacterData.PropertyType.MoveStep,
		CharacterData.PropertyType.Strength,
		CharacterData.PropertyType.Constitution,
		CharacterData.PropertyType.Intelligence,
		CharacterData.PropertyType.Dexterity,
		CharacterData.PropertyType.StrengthMax,
		CharacterData.PropertyType.ConstitutionMax,
		CharacterData.PropertyType.IntelligenceMax,
		CharacterData.PropertyType.DexterityMax,
		CharacterData.PropertyType.UseSword,
		CharacterData.PropertyType.UseBlade,
		CharacterData.PropertyType.UseArrow,
		CharacterData.PropertyType.UseFist,
		CharacterData.PropertyType.UseGas,
		CharacterData.PropertyType.UseRope,
		CharacterData.PropertyType.UseWhip,
		CharacterData.PropertyType.UsePike,
		CharacterData.PropertyType.UseSwordMax,
		CharacterData.PropertyType.UseBladeMax,
		CharacterData.PropertyType.UseArrowMax,
		CharacterData.PropertyType.UseFistMax,
		CharacterData.PropertyType.UseGasMax,
		CharacterData.PropertyType.UseRopeMax,
		CharacterData.PropertyType.UseWhipMax,
		CharacterData.PropertyType.UsePikeMax,
		CharacterData.PropertyType.DefSword,
		CharacterData.PropertyType.DefBlade,
		CharacterData.PropertyType.DefArrow,
		CharacterData.PropertyType.DefFist,
		CharacterData.PropertyType.DefGas,
		CharacterData.PropertyType.DefRope,
		CharacterData.PropertyType.DefWhip,
		CharacterData.PropertyType.DefPike,
		CharacterData.PropertyType.Dodge,
		CharacterData.PropertyType.Counter,
		CharacterData.PropertyType.Critical,
		CharacterData.PropertyType.Combo,
		CharacterData.PropertyType.DefendDodge,
		CharacterData.PropertyType.DefendCounter,
		CharacterData.PropertyType.DefendCritical,
		CharacterData.PropertyType.DefendCombo
	};

	// Token: 0x04001A25 RID: 6693
	private string[] iPopText = new string[]
	{
		"當前氣血",
		"最大氣血",
		"當前內力",
		"最大內力",
		"移動格數",
		"臂力",
		"根骨",
		"悟性",
		"身法",
		"臂力上限",
		"根骨上限",
		"悟性上限",
		"身法上限",
		"劍術",
		"刀法",
		"箭法",
		"拳法",
		"氣功",
		"繩法",
		"鞭法",
		"槍法",
		"劍術上限",
		"刀法上限",
		"箭法上限",
		"拳法上限",
		"氣功上限",
		"繩法上限",
		"鞭法上限",
		"槍法上限",
		"破劍",
		"破刀",
		"破箭",
		"破拳",
		"破氣",
		"破繩",
		"破鞭",
		"破槍",
		"閃避",
		"反擊",
		"爆擊",
		"連擊",
		"命中",
		"抗反",
		"抗爆",
		"抗連",
		"武學",
		"內功",
		"天賦",
		"設定總值",
		"回到選擇"
	};

	// Token: 0x04001A26 RID: 6694
	private string[] eTypeText = new string[]
	{
		"地圖",
		"人物",
		"隊伍",
		"道具",
		"戰鬥難度"
	};

	// Token: 0x04001A27 RID: 6695
	private int[] TeammateID = new int[9];

	// Token: 0x04001A28 RID: 6696
	private string[] TeammateNameText = new string[11];

	// Token: 0x04001A29 RID: 6697
	private string[] RoutineArray = new string[12];

	// Token: 0x04001A2A RID: 6698
	private string[] NeigongArray = new string[12];

	// Token: 0x04001A2B RID: 6699
	private string[] TalentArray = new string[3];

	// Token: 0x04001A2C RID: 6700
	private string[] MapName;

	// Token: 0x04001A2D RID: 6701
	private string[] MapID;

	// Token: 0x04001A2E RID: 6702
	private int iPopSelectIndex;

	// Token: 0x04001A2F RID: 6703
	private string strNewValue = string.Empty;

	// Token: 0x04001A30 RID: 6704
	private string[] WeaponNameArray = new string[]
	{
		"逍遙刀劍",
		"太乙刀劍",
		"無極刀劍",
		"離魂金線纏",
		"玄鐵手套",
		"金絲手套",
		"萃毒指",
		"血飲",
		"圓月彎刀",
		"閻羅",
		"菊一文字．無銘",
		"三日月．無銘  ",
		"村正",
		"辟邪",
		"殤瑤",
		"紫薇軟劍",
		"瓊華劍",
		"傳家寶劍",
		"含光劍",
		"宵練劍",
		"瘋魔杖",
		"升龍棍",
		"定海神針",
		"玄鐵棍",
		"神劍鏢",
		"雷震天",
		"離火玄冰鏢",
		"金針",
		"十六串呂",
		"神農鋤",
		"舞天環",
		"烏煙",
		"孔雀翎",
		"花開四季",
		"涯角槍",
		"鶴嘴",
		"紫蝶繡鞋",
		"凌波踏霜",
		"驚鴻靴",
		"虎步龍行"
	};

	// Token: 0x04001A31 RID: 6705
	private string strCommand = "/i /h";

	// Token: 0x04001A32 RID: 6706
	private CharacterData CD;

	// Token: 0x04001A33 RID: 6707
	private cPureConsole.GridType GT;

	// Token: 0x020003A0 RID: 928
	private struct ConsoleMessage
	{
		// Token: 0x06001593 RID: 5523 RVA: 0x0000DB6A File Offset: 0x0000BD6A
		public ConsoleMessage(string strMessage, string strStackTrace, LogType logType)
		{
			this.m_strMessage = strMessage;
			this.m_strStackTrace = strStackTrace;
			this.m_LogType = logType;
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06001594 RID: 5524 RVA: 0x0000DB81 File Offset: 0x0000BD81
		public string message
		{
			get
			{
				return this.m_strMessage;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06001595 RID: 5525 RVA: 0x0000DB89 File Offset: 0x0000BD89
		public string stackTrace
		{
			get
			{
				return this.m_strStackTrace;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06001596 RID: 5526 RVA: 0x0000DB91 File Offset: 0x0000BD91
		public LogType logType
		{
			get
			{
				return this.m_LogType;
			}
		}

		// Token: 0x04001A34 RID: 6708
		private string m_strMessage;

		// Token: 0x04001A35 RID: 6709
		private string m_strStackTrace;

		// Token: 0x04001A36 RID: 6710
		private LogType m_LogType;
	}

	// Token: 0x020003A1 RID: 929
	private enum GridType
	{
		// Token: 0x04001A38 RID: 6712
		Select,
		// Token: 0x04001A39 RID: 6713
		Property,
		// Token: 0x04001A3A RID: 6714
		Routine,
		// Token: 0x04001A3B RID: 6715
		Neigong,
		// Token: 0x04001A3C RID: 6716
		Talent,
		// Token: 0x04001A3D RID: 6717
		Map,
		// Token: 0x04001A3E RID: 6718
		Item,
		// Token: 0x04001A3F RID: 6719
		Team
	}
}
