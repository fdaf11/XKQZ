using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x0200019B RID: 411
	public class Astar
	{
		// Token: 0x06000884 RID: 2180 RVA: 0x0004DA68 File Offset: 0x0004BC68
		public static List<T> GetMovNode<T>(T start, T end, Func<string, string, int> getBigMapNodeWeights) where T : MapPathNode
		{
			PriorityQueue<T> priorityQueue = new PriorityQueue<T>();
			priorityQueue.Enqueue(start, 0);
			Dictionary<string, T> dictionary = new Dictionary<string, T>();
			dictionary.Add(start.m_NodeId, (T)((object)null));
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			dictionary2.Add(start.m_NodeId, 0);
			while (priorityQueue.Count != 0)
			{
				T t = (T)((object)priorityQueue.Dequeue());
				if (end.m_NodeId == t.m_NodeId)
				{
					break;
				}
				for (int i = 0; i < t.m_NebrList.Count; i++)
				{
					string nodeId = t.m_NebrList[i].m_NodeId;
					if (!dictionary.ContainsKey(nodeId))
					{
						T t2 = t.m_NebrList[i] as T;
						int priority_ = 0;
						if (getBigMapNodeWeights != null)
						{
							priority_ = getBigMapNodeWeights.Invoke(t.m_NodeId, end.m_NodeId);
						}
						priorityQueue.Enqueue(t2, priority_);
						dictionary.Add(t2.m_NodeId, t);
					}
				}
			}
			T t3 = end;
			List<T> list = new List<T>();
			list.Add(t3);
			while (t3 != start)
			{
				if (!dictionary.ContainsKey(t3.m_NodeId))
				{
					break;
				}
				t3 = dictionary[t3.m_NodeId];
				list.Add(t3);
			}
			return list;
		}
	}
}
