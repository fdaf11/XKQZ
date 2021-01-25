using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020001F9 RID: 505
public class CharacterDataManager : TextDataManager
{
	// Token: 0x06000A0A RID: 2570 RVA: 0x0000809F File Offset: 0x0000629F
	private CharacterDataManager()
	{
		this.NPCList = new List<CharacterDataBase>();
		this.m_LoadFileName = "CharacterData";
		this.LoadFile(this.m_LoadFileName);
		TextDataManager.AddTextDataToList(this);
		TextDataManager.AddDLCTextDataToList(this);
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000A0C RID: 2572 RVA: 0x000080E1 File Offset: 0x000062E1
	public static CharacterDataManager Singleton
	{
		get
		{
			return CharacterDataManager.instance;
		}
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x00055220 File Offset: 0x00053420
	protected override void LoadFile(string filePath)
	{
		this.NPCList.Clear();
		string[] array = base.ExtractTextFile(filePath);
		if (array == null)
		{
			return;
		}
		if (GameGlobal.m_bDLCMode)
		{
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (text.get_Chars(0) != '#')
					{
						try
						{
							CharacterDataBase characterDataBase = new CharacterDataBase();
							string[] array3 = text.Trim().Split(new char[]
							{
								"\t".get_Chars(0)
							});
							characterDataBase.iNpcID = int.Parse(array3[0]);
							if (array3[1].Length > 0)
							{
								string[] array4 = array3[1].Split(new char[]
								{
									",".get_Chars(0)
								});
								foreach (string text2 in array4)
								{
									int num = 0;
									if (!int.TryParse(text2, ref num))
									{
										num = 0;
									}
									WeaponType weaponType = (WeaponType)num;
									characterDataBase.WeaponTypeList.Add(weaponType);
								}
							}
							characterDataBase.iMaxHp = int.Parse(array3[2]);
							characterDataBase.iMaxSp = int.Parse(array3[3]);
							characterDataBase.iCri = int.Parse(array3[4]);
							characterDataBase.iCounter = int.Parse(array3[5]);
							characterDataBase.iDodge = int.Parse(array3[6]);
							characterDataBase.iMoveStep = int.Parse(array3[7]);
							if (array3[8].Length > 1)
							{
								string[] array6 = array3[8].Split(new char[]
								{
									",".get_Chars(0)
								});
								for (int k = 0; k < array6.Length; k++)
								{
									int ivalue;
									if (!int.TryParse(array6[k], ref ivalue))
									{
										ivalue = 0;
									}
									characterDataBase._MartialDef.getdata(k, ivalue);
								}
							}
							characterDataBase.iEquipWeaponID = int.Parse(array3[9]);
							characterDataBase.iEquipArrorID = int.Parse(array3[10]);
							characterDataBase.iEquipNecklaceID = int.Parse(array3[11]);
							if (array3[12].Length > 1)
							{
								string text3 = array3[12].Replace(")*(", "*");
								if (text3.Length > 2)
								{
									text3 = text3.Substring(1, text3.Length - 2);
									string[] array7 = text3.Split(new char[]
									{
										"*".get_Chars(0)
									});
									for (int l = 0; l < array7.Length; l++)
									{
										string[] array8 = array7[l].Split(new char[]
										{
											",".get_Chars(0)
										});
										NpcItem npcItem = new NpcItem();
										npcItem.m_iItemID = int.Parse(array8[0]);
										npcItem.m_iCount = int.Parse(array8[1]);
										characterDataBase.Itemlist.Add(npcItem);
									}
								}
							}
							if (array3[13].Length > 1)
							{
								string text4 = array3[13].Replace(")*(", "*");
								if (text4.Length > 2)
								{
									text4 = text4.Substring(1, text4.Length - 2);
									string[] array9 = text4.Split(new char[]
									{
										"*".get_Chars(0)
									});
									for (int m = 0; m < array9.Length; m++)
									{
										string[] array10 = array9[m].Split(new char[]
										{
											",".get_Chars(0)
										});
										if (!(array10[0] == "0"))
										{
											for (int n = 0; n < characterDataBase.RoutineList.Count; n++)
											{
												if (characterDataBase.RoutineList[n].iSkillID.ToString() == array10[0])
												{
													Debug.LogError(characterDataBase.iNpcID + " 已擁有這個套路 " + array10[0]);
												}
											}
											NpcRoutine npcRoutine = new NpcRoutine();
											npcRoutine.iSkillID = int.Parse(array10[0]);
											npcRoutine.iLevel = int.Parse(array10[1]);
											npcRoutine.bUse = false;
											characterDataBase.RoutineList.Add(npcRoutine);
										}
									}
								}
							}
							if (array3[14].Length > 1)
							{
								string text5 = array3[14].Replace(")*(", "*");
								if (text5.Length > 2)
								{
									text5 = text5.Substring(1, text5.Length - 2);
									string[] array11 = text5.Split(new char[]
									{
										"*".get_Chars(0)
									});
									for (int num2 = 0; num2 < array11.Length; num2++)
									{
										string[] array12 = array11[num2].Split(new char[]
										{
											",".get_Chars(0)
										});
										if (!(array12[0] == "0"))
										{
											for (int num3 = 0; num3 < characterDataBase.NeigongList.Count; num3++)
											{
												if (characterDataBase.NeigongList[num3].iSkillID.ToString() == array12[0])
												{
													Debug.LogError(characterDataBase.iNpcID + " 已擁有這個內功 " + array12[0]);
												}
											}
											NpcNeigong npcNeigong = new NpcNeigong();
											npcNeigong.iSkillID = int.Parse(array12[0]);
											npcNeigong.iLevel = int.Parse(array12[1]);
											npcNeigong.bUse = false;
											characterDataBase.NeigongList.Add(npcNeigong);
										}
									}
								}
							}
							if (array3[15].Length > 1)
							{
								string[] array13 = array3[15].Split(new char[]
								{
									",".get_Chars(0)
								});
								for (int num4 = 0; num4 < array13.Length; num4++)
								{
									int num5 = int.Parse(array13[num4]);
									characterDataBase.TalentList.Add(num5);
								}
							}
							if (array3[16].Length > 1)
							{
								string[] array14 = array3[16].Split(new char[]
								{
									",".get_Chars(0)
								});
								for (int num6 = 0; num6 < array14.Length; num6++)
								{
									characterDataBase.sVoicList.Add(array14[num6]);
								}
							}
							characterDataBase.iPrice = int.Parse(array3[17]);
							if (int.Parse(array3[18]) > 0)
							{
								characterDataBase.bCaptive = true;
							}
							else
							{
								characterDataBase.bCaptive = false;
							}
							characterDataBase.iJoin = int.Parse(array3[19]);
							characterDataBase.iSmuggleSuccess = int.Parse(array3[20]);
							characterDataBase.iSmuggleSurvive = int.Parse(array3[21]);
							characterDataBase.iHonor = int.Parse(array3[22]);
							this.NPCList.Add(characterDataBase);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}
		else
		{
			foreach (string text6 in array)
			{
				if (!string.IsNullOrEmpty(text6))
				{
					if (text6.get_Chars(0) != '#')
					{
						try
						{
							CharacterDataBase characterDataBase2 = new CharacterDataBase();
							string[] array3 = text6.Split(new char[]
							{
								"\t".get_Chars(0)
							});
							characterDataBase2.iNpcID = int.Parse(array3[0]);
							if (array3[2].Length > 0)
							{
								string[] array16 = array3[2].Split(new char[]
								{
									",".get_Chars(0)
								});
								foreach (string text7 in array16)
								{
									int num9 = 0;
									if (!int.TryParse(text7, ref num9))
									{
										num9 = 0;
									}
									WeaponType weaponType2 = (WeaponType)num9;
									characterDataBase2.WeaponTypeList.Add(weaponType2);
								}
							}
							characterDataBase2.iMoney = int.Parse(array3[3]);
							characterDataBase2.iMoveStep = int.Parse(array3[5]);
							characterDataBase2.iMaxHp = int.Parse(array3[6]);
							characterDataBase2.iMaxSp = int.Parse(array3[7]);
							characterDataBase2.iStr = int.Parse(array3[8]);
							characterDataBase2.iCon = int.Parse(array3[9]);
							characterDataBase2.iInt = int.Parse(array3[10]);
							characterDataBase2.iDex = int.Parse(array3[11]);
							characterDataBase2.iMaxStr = int.Parse(array3[12]);
							characterDataBase2.iMaxCon = int.Parse(array3[13]);
							characterDataBase2.iMaxInt = int.Parse(array3[14]);
							characterDataBase2.iMaxDex = int.Parse(array3[15]);
							characterDataBase2.iCri = int.Parse(array3[16]);
							characterDataBase2.iCounter = int.Parse(array3[17]);
							characterDataBase2.iDodge = int.Parse(array3[18]);
							characterDataBase2.iCombo = int.Parse(array3[19]);
							if (array3[20].Length > 1)
							{
								string[] array18 = array3[20].Split(new char[]
								{
									",".get_Chars(0)
								});
								for (int num10 = 0; num10 < array18.Length; num10++)
								{
									int ivalue2;
									if (!int.TryParse(array18[num10], ref ivalue2))
									{
										ivalue2 = 0;
									}
									characterDataBase2._MartialArts.getdata(num10, ivalue2);
								}
							}
							if (array3[21].Length > 1)
							{
								string[] array19 = array3[21].Split(new char[]
								{
									",".get_Chars(0)
								});
								for (int num11 = 0; num11 < array19.Length; num11++)
								{
									int ivalue3;
									if (!int.TryParse(array19[num11], ref ivalue3))
									{
										ivalue3 = 0;
									}
									characterDataBase2._MartialDef.getdata(num11, ivalue3);
								}
							}
							characterDataBase2.iEquipWeaponID = int.Parse(array3[22]);
							characterDataBase2.iEquipArrorID = int.Parse(array3[23]);
							characterDataBase2.iEquipNecklaceID = int.Parse(array3[24]);
							if (array3[25].Length > 1)
							{
								string text8 = array3[25].Replace(")*(", "*");
								if (text8.Length > 2)
								{
									text8 = text8.Substring(1, text8.Length - 2);
									string[] array20 = text8.Split(new char[]
									{
										"*".get_Chars(0)
									});
									for (int num12 = 0; num12 < array20.Length; num12++)
									{
										string[] array21 = array20[num12].Split(new char[]
										{
											",".get_Chars(0)
										});
										NpcItem npcItem2 = new NpcItem();
										npcItem2.m_iItemID = int.Parse(array21[0]);
										npcItem2.m_iCount = int.Parse(array21[1]);
										characterDataBase2.Itemlist.Add(npcItem2);
									}
								}
							}
							if (array3[26].Length > 1)
							{
								string text9 = array3[26].Replace(")*(", "*");
								if (text9.Length > 2)
								{
									text9 = text9.Substring(1, text9.Length - 2);
									string[] array22 = text9.Split(new char[]
									{
										"*".get_Chars(0)
									});
									for (int num13 = 0; num13 < array22.Length; num13++)
									{
										string[] array23 = array22[num13].Split(new char[]
										{
											",".get_Chars(0)
										});
										if (!(array23[0] == "0"))
										{
											for (int num14 = 0; num14 < characterDataBase2.RoutineList.Count; num14++)
											{
												if (characterDataBase2.RoutineList[num14].iSkillID.ToString() == array23[0])
												{
													Debug.LogError(characterDataBase2.iNpcID + " 已擁有這個套路 " + array23[0]);
												}
											}
											NpcRoutine npcRoutine2 = new NpcRoutine();
											npcRoutine2.iSkillID = int.Parse(array23[0]);
											npcRoutine2.iLevel = int.Parse(array23[1]);
											npcRoutine2.bUse = false;
											characterDataBase2.RoutineList.Add(npcRoutine2);
										}
									}
								}
							}
							if (array3[27].Length > 1)
							{
								string text10 = array3[27].Replace(")*(", "*");
								if (text10.Length > 2)
								{
									text10 = text10.Substring(1, text10.Length - 2);
									string[] array24 = text10.Split(new char[]
									{
										"*".get_Chars(0)
									});
									for (int num15 = 0; num15 < array24.Length; num15++)
									{
										string[] array25 = array24[num15].Split(new char[]
										{
											",".get_Chars(0)
										});
										if (!(array25[0] == "0"))
										{
											for (int num16 = 0; num16 < characterDataBase2.NeigongList.Count; num16++)
											{
												if (characterDataBase2.NeigongList[num16].iSkillID.ToString() == array25[0])
												{
													Debug.LogError(characterDataBase2.iNpcID + " 已擁有這個內功 " + array25[0]);
												}
											}
											NpcNeigong npcNeigong2 = new NpcNeigong();
											npcNeigong2.iSkillID = int.Parse(array25[0]);
											npcNeigong2.iLevel = int.Parse(array25[1]);
											npcNeigong2.bUse = false;
											characterDataBase2.NeigongList.Add(npcNeigong2);
										}
									}
								}
							}
							if (array3[28].Length > 1)
							{
								string[] array26 = array3[28].Split(new char[]
								{
									",".get_Chars(0)
								});
								for (int num17 = 0; num17 < array26.Length; num17++)
								{
									int num18 = int.Parse(array26[num17]);
									characterDataBase2.TalentList.Add(num18);
								}
							}
							string text11 = array3[29].Replace("\r", string.Empty);
							if (text11.Length > 1)
							{
								string[] array27 = text11.Split(new char[]
								{
									",".get_Chars(0)
								});
								for (int num19 = 0; num19 < array27.Length; num19++)
								{
									characterDataBase2.sVoicList.Add(array27[num19]);
								}
							}
							this.NPCList.Add(characterDataBase2);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text6);
						}
					}
				}
			}
		}
		this.NPCList.Sort((CharacterDataBase A, CharacterDataBase B) => A.iNpcID.CompareTo(B.iNpcID));
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x000560E0 File Offset: 0x000542E0
	public CharacterData GetCharacterData(int id)
	{
		CharacterDataBase characterDataBase = this.NPCList.Find((CharacterDataBase x) => x.iNpcID == id);
		CharacterData characterData = characterDataBase.Clone();
		List<string> sVoicList = new List<string>(characterDataBase.sVoicList.ToArray());
		characterData.sVoicList = sVoicList;
		NpcDataNode npcData = Game.NpcData.GetNpcData(characterData.iNpcID);
		if (npcData == null)
		{
			Debug.LogError("找不到 Npc " + characterData.iNpcID.ToString() + " 請查 NpcData.txt ");
		}
		characterData._NpcDataNode = npcData;
		List<NpcNeigong> list = new List<NpcNeigong>();
		foreach (NpcNeigong npcNeigong in characterData.NeigongList)
		{
			if (list.Count < 12)
			{
				if (npcNeigong.SetNeigongData(characterData.iNpcID))
				{
					npcNeigong.SetExp(0);
				}
				else
				{
					Debug.LogError(string.Concat(new object[]
					{
						"請確認 CharacterData ",
						characterData.iNpcID.ToString(),
						" 的內功欄位 ",
						npcNeigong.iSkillID
					}));
				}
				list.Add(npcNeigong);
			}
		}
		characterData.NeigongList.Clear();
		characterData.NeigongList.AddRange(list);
		if (characterData.NeigongList.Count > 0)
		{
			characterData.NeigongList[0].bUse = true;
		}
		int num = 6;
		int num2 = 0;
		foreach (NpcRoutine npcRoutine in characterData.RoutineList)
		{
			if (npcRoutine.SetRoutine())
			{
				npcRoutine.SetExp(0);
				if (num2 < num)
				{
					npcRoutine.bUse = true;
				}
				else
				{
					npcRoutine.bUse = false;
				}
				num2++;
			}
			else
			{
				Debug.LogError(string.Concat(new object[]
				{
					"請確認 CharacterData ",
					characterData.iNpcID.ToString(),
					" 的套路欄位 ",
					npcRoutine.iSkillID
				}));
			}
		}
		if (characterData._EquipArror == null)
		{
			characterData._EquipArror = new EquipProperty();
		}
		if (characterData._EquipWeapon == null)
		{
			characterData._EquipWeapon = new EquipProperty();
		}
		if (characterData._EquipNecklace == null)
		{
			characterData._EquipNecklace = new EquipProperty();
		}
		characterData.SetEquip(ItemDataNode.ItemType.Weapon, characterData.iEquipWeaponID);
		characterData.SetEquip(ItemDataNode.ItemType.Arror, characterData.iEquipArrorID);
		characterData.SetEquip(ItemDataNode.ItemType.Necklace, characterData.iEquipNecklaceID);
		characterData.DLC_ResetNeigongConditionPassive();
		characterData.setTotalProperty();
		characterData.binit = false;
		return characterData;
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x00056380 File Offset: 0x00054580
	public List<CharacterData> GetNpcList()
	{
		List<CharacterData> list = new List<CharacterData>();
		for (int i = 0; i < this.NPCList.Count; i++)
		{
			CharacterData characterData = this.NPCList[i].Clone();
			list.Add(characterData);
		}
		return list;
	}

	// Token: 0x04000A76 RID: 2678
	private static readonly CharacterDataManager instance = new CharacterDataManager();

	// Token: 0x04000A77 RID: 2679
	private List<CharacterDataBase> NPCList;
}
