using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001BD RID: 445
	public class MapNpcDataNode
	{
		// Token: 0x06000951 RID: 2385 RVA: 0x00007A91 File Offset: 0x00005C91
		public MapNpcDataNode()
		{
			this.m_PointNodeList = new List<PointNode>();
			this.m_SpecialTalkNodeList = new List<SpecialTalkNode>();
			this.m_ShowSCondition = new List<Condition>();
			this.m_CloseSCondition = new List<Condition>();
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00050458 File Offset: 0x0004E658
		public string GetTalkID(Animation m_Ani, Vector3 PlayerV3, MapNpcDataNode.ToPointType _ToPoint)
		{
			string text = string.Empty;
			for (int i = 0; i < this.m_SpecialTalkNodeList.Count; i++)
			{
				if (text != string.Empty)
				{
					return text;
				}
				SpecialTalkNode specialTalkNode = this.m_SpecialTalkNodeList[i];
				switch (specialTalkNode.m_Type)
				{
				case SpecialTalkNode.SpecialType.Point:
				{
					string[] array = specialTalkNode.m_strValue.Split(new char[]
					{
						"_".get_Chars(0)
					});
					Vector3 vector;
					vector..ctor(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
					if (_ToPoint == MapNpcDataNode.ToPointType.Stand && Math.Round((double)vector.x) - 1.0 <= Math.Round((double)PlayerV3.x) && Math.Round((double)vector.x) + 1.0 >= Math.Round((double)PlayerV3.x) && Math.Round((double)vector.y) - 1.0 <= Math.Round((double)PlayerV3.y) && Math.Round((double)vector.y) + 1.0 >= Math.Round((double)PlayerV3.y) && Math.Round((double)vector.z) - 1.0 <= Math.Round((double)PlayerV3.z) && Math.Round((double)vector.z) + 1.0 >= Math.Round((double)PlayerV3.z))
					{
						text = MissionStatus.m_instance.GetQuestTalkID(specialTalkNode.m_strMsgID);
					}
					break;
				}
				case SpecialTalkNode.SpecialType.PlayingAnimation:
					if (m_Ani.IsPlaying(specialTalkNode.m_strValue))
					{
						text = MissionStatus.m_instance.GetQuestTalkID(specialTalkNode.m_strMsgID);
					}
					break;
				case SpecialTalkNode.SpecialType.Quest:
					text = MissionStatus.m_instance.GetQuestTalkID(specialTalkNode.m_strValue);
					break;
				case SpecialTalkNode.SpecialType.QuestIng:
				{
					bool flag = MissionStatus.m_instance.CheckQuest(specialTalkNode.m_strValue);
					text = ((!flag) ? string.Empty : specialTalkNode.m_strMsgID);
					break;
				}
				case SpecialTalkNode.SpecialType.OldQuest:
					text = MissionStatus.m_instance.OldGetQuestTalkID(specialTalkNode.m_strValue);
					break;
				case SpecialTalkNode.SpecialType.CollectionQuest:
				{
					bool flag2 = MissionStatus.m_instance.CheckCollectionQuest(specialTalkNode.m_strValue);
					text = ((!flag2) ? string.Empty : specialTalkNode.m_strMsgID);
					break;
				}
				case SpecialTalkNode.SpecialType.MovieEvent:
				{
					bool flag3 = TeamStatus.m_Instance.m_EventList.Contains(int.Parse(specialTalkNode.m_strValue));
					text = ((!flag3) ? string.Empty : specialTalkNode.m_strMsgID);
					break;
				}
				case SpecialTalkNode.SpecialType.FlagOnce:
				{
					int num = Game.Variable[specialTalkNode.m_strMsgID];
					if (num < 0)
					{
						GlobalVariableManager variable;
						GlobalVariableManager globalVariableManager = variable = Game.Variable;
						string strMsgID;
						string key = strMsgID = specialTalkNode.m_strMsgID;
						int num2 = variable[strMsgID];
						globalVariableManager[key] = num2 + 1;
						text = specialTalkNode.m_strValue;
					}
					break;
				}
				}
			}
			if (text.Length > 2)
			{
				return text;
			}
			if (_ToPoint == MapNpcDataNode.ToPointType.Stand)
			{
				if (this.m_strPointTalkID.Length > 1)
				{
					string text2 = this.m_strPointTalkID.Substring(0, 1);
					int num3 = int.Parse(this.m_strPointTalkID.Substring(1, this.m_strPointTalkID.Length - 1));
					int num4 = Random.Range(0, this.m_iMaxTalk);
					text2 += (num3 + num4).ToString();
					text = text2;
				}
				else
				{
					text = "N9000001";
				}
			}
			else if (_ToPoint == MapNpcDataNode.ToPointType.Walk)
			{
				if (this.m_strMoveTalkID.Length > 1)
				{
					text = this.m_strMoveTalkID;
				}
				else
				{
					text = "N9000001";
				}
			}
			else if (_ToPoint == MapNpcDataNode.ToPointType.Battle)
			{
				if (this.m_strCheakPlayerTalkID.Length > 1)
				{
					text = this.m_strCheakPlayerTalkID;
				}
				else
				{
					text = "N9000001";
				}
			}
			return text;
		}

		// Token: 0x0400091A RID: 2330
		public int m_iNpcID;

		// Token: 0x0400091B RID: 2331
		public List<PointNode> m_PointNodeList;

		// Token: 0x0400091C RID: 2332
		public int m_iNpcType;

		// Token: 0x0400091D RID: 2333
		public string m_strSCondition;

		// Token: 0x0400091E RID: 2334
		public List<Condition> m_ShowSCondition;

		// Token: 0x0400091F RID: 2335
		public List<Condition> m_CloseSCondition;

		// Token: 0x04000920 RID: 2336
		public int m_iMoveType;

		// Token: 0x04000921 RID: 2337
		public string m_strPointTalkID;

		// Token: 0x04000922 RID: 2338
		public string m_strMoveTalkID;

		// Token: 0x04000923 RID: 2339
		public List<SpecialTalkNode> m_SpecialTalkNodeList;

		// Token: 0x04000924 RID: 2340
		public int m_iTalkSteer;

		// Token: 0x04000925 RID: 2341
		public int m_iStoreID;

		// Token: 0x04000926 RID: 2342
		public int m_iMaxTalk;

		// Token: 0x04000927 RID: 2343
		public int m_iCheakPlayer;

		// Token: 0x04000928 RID: 2344
		public int m_iBattleArea;

		// Token: 0x04000929 RID: 2345
		public string m_strCheakPlayerTalkID;

		// Token: 0x0400092A RID: 2346
		public bool m_bIsBattle;

		// Token: 0x0400092B RID: 2347
		public int m_iStayAffterBattle;

		// Token: 0x020001BE RID: 446
		public enum NpcType
		{
			// Token: 0x0400092D RID: 2349
			Npc,
			// Token: 0x0400092E RID: 2350
			Shop,
			// Token: 0x0400092F RID: 2351
			Ornament
		}

		// Token: 0x020001BF RID: 447
		public enum MoveType
		{
			// Token: 0x04000931 RID: 2353
			None,
			// Token: 0x04000932 RID: 2354
			BackAndForth,
			// Token: 0x04000933 RID: 2355
			Standing,
			// Token: 0x04000934 RID: 2356
			WentDisappeared,
			// Token: 0x04000935 RID: 2357
			Patrol,
			// Token: 0x04000936 RID: 2358
			ToThePoint
		}

		// Token: 0x020001C0 RID: 448
		public enum ToPointType
		{
			// Token: 0x04000938 RID: 2360
			Walk,
			// Token: 0x04000939 RID: 2361
			Stand,
			// Token: 0x0400093A RID: 2362
			Battle
		}
	}
}
