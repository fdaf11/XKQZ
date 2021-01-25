using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001EB RID: 491
	public class BiographiesManager : TextDataManager
	{
		// Token: 0x060009DE RID: 2526 RVA: 0x00007F44 File Offset: 0x00006144
		public BiographiesManager()
		{
			this.m_LoadFileName = "BiographiesManager";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x00007F80 File Offset: 0x00006180
		public static BiographiesManager Singleton
		{
			get
			{
				return BiographiesManager.instance;
			}
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x000536A0 File Offset: 0x000518A0
		protected override void LoadFile(string filePath)
		{
			this.BiographiesTypeNodeList.Clear();
			string[] array = base.ExtractTextFile(filePath);
			if (array == null)
			{
				return;
			}
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (text.get_Chars(0) != '#')
					{
						string[] array3 = text.Split(new char[]
						{
							"\t".get_Chars(0)
						});
						BiographiesTypeNode biographiesTypeNode = this.ClickBiographiesTypeNode(array3[0]);
						BiographiesNode biographiesNode = new BiographiesNode();
						for (int j = 0; j < array3.Length; j++)
						{
							this.generateData(j, array3[j], biographiesNode);
						}
						biographiesTypeNode.m_BiographiesNodeList.Add(biographiesNode);
					}
				}
			}
			foreach (KeyValuePair<string, BiographiesTypeNode> keyValuePair in this.BiographiesTypeNodeList)
			{
				keyValuePair.Value.m_BiographiesNodeList.Sort((BiographiesNode x, BiographiesNode y) => x.m_iOrder.CompareTo(y.m_iOrder));
			}
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x000537E4 File Offset: 0x000519E4
		private void generateData(int idx, string data, BiographiesNode node)
		{
			switch (idx)
			{
			case 1:
				int.TryParse(data, ref node.m_iOrder);
				break;
			case 2:
				int.TryParse(data, ref node.m_iNpcID);
				break;
			case 3:
				node.m_Message = data;
				break;
			case 4:
				node.m_Voice = data;
				break;
			case 5:
				node.m_Image = data;
				break;
			case 6:
				node.m_BackgroundImage = data;
				break;
			case 7:
			{
				int endMovie = 0;
				int.TryParse(data, ref endMovie);
				node.m_EndMovie = (BiographiesNode.eEndMovie)endMovie;
				break;
			}
			case 8:
			{
				int num = 0;
				int.TryParse(data, ref num);
				num = Mathf.Clamp(num, 0, 6);
				node.m_eMsgPlace = (BiographiesNode.eMsgPos)num;
				break;
			}
			case 9:
			{
				string text = data.Replace(")*(", "*");
				text = text.Replace("\r", string.Empty);
				if (text.Length > 2)
				{
					text = text.Substring(1, text.Length - 2);
					string[] array = text.Split(new char[]
					{
						"*".get_Chars(0)
					});
					for (int i = 0; i < array.Length; i++)
					{
						string[] array2 = array[i].Split(new char[]
						{
							",".get_Chars(0)
						});
						if (array2.Length > 2)
						{
							BiographiesNpcQHeadNode biographiesNpcQHeadNode = new BiographiesNpcQHeadNode(array2);
							node.m_BiographiesNpcQHeadNodeList.Add(biographiesNpcQHeadNode);
						}
					}
				}
				break;
			}
			case 10:
				int.TryParse(data, ref node.m_BiographiesReward);
				break;
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0005397C File Offset: 0x00051B7C
		private BiographiesTypeNode ClickBiographiesTypeNode(string id)
		{
			if (!this.BiographiesTypeNodeList.ContainsKey(id))
			{
				BiographiesTypeNode biographiesTypeNode = new BiographiesTypeNode();
				biographiesTypeNode.m_BiographiesGroupID = id;
				this.BiographiesTypeNodeList.Add(id, biographiesTypeNode);
				return biographiesTypeNode;
			}
			return this.BiographiesTypeNodeList[id];
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00007F87 File Offset: 0x00006187
		public BiographiesTypeNode GetBiographiesTypeNode(string id)
		{
			if (!this.BiographiesTypeNodeList.ContainsKey(id))
			{
				return null;
			}
			return this.BiographiesTypeNodeList[id];
		}

		// Token: 0x04000A05 RID: 2565
		private static readonly BiographiesManager instance = new BiographiesManager();

		// Token: 0x04000A06 RID: 2566
		private Dictionary<string, BiographiesTypeNode> BiographiesTypeNodeList = new Dictionary<string, BiographiesTypeNode>();
	}
}
