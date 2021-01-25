using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000893 RID: 2195
public class iTweenPath : MonoBehaviour
{
	// Token: 0x060035A7 RID: 13735 RVA: 0x001A1A20 File Offset: 0x0019FC20
	public iTweenPath()
	{
		List<Vector3> list = new List<Vector3>();
		list.Add(Vector3.zero);
		list.Add(Vector3.zero);
		this.nodes = list;
		this.initialName = string.Empty;
		base..ctor();
	}

	// Token: 0x060035A9 RID: 13737 RVA: 0x00021C3F File Offset: 0x0001FE3F
	private void OnEnable()
	{
		if (iTweenPath.paths.ContainsKey(this.pathName))
		{
			return;
		}
		iTweenPath.paths.Add(this.pathName.ToLower(), this);
	}

	// Token: 0x060035AA RID: 13738 RVA: 0x00021C6D File Offset: 0x0001FE6D
	private void OnDrawGizmosSelected()
	{
		if (base.enabled && this.nodes.Count > 0)
		{
			iTween.DrawPath(this.nodes.ToArray(), this.pathColor);
		}
	}

	// Token: 0x060035AB RID: 13739 RVA: 0x00021CA1 File Offset: 0x0001FEA1
	public static Vector3[] GetPath(string requestedName)
	{
		requestedName = requestedName.ToLower();
		if (iTweenPath.paths.ContainsKey(requestedName))
		{
			return iTweenPath.paths[requestedName].nodes.ToArray();
		}
		Debug.Log("No path with that name exists! Are you sure you wrote it correctly?");
		return null;
	}

	// Token: 0x0400411D RID: 16669
	public string pathName = string.Empty;

	// Token: 0x0400411E RID: 16670
	public Color pathColor = Color.cyan;

	// Token: 0x0400411F RID: 16671
	public List<Vector3> nodes;

	// Token: 0x04004120 RID: 16672
	public int nodeCount;

	// Token: 0x04004121 RID: 16673
	public static Dictionary<string, iTweenPath> paths = new Dictionary<string, iTweenPath>();

	// Token: 0x04004122 RID: 16674
	public bool initialized;

	// Token: 0x04004123 RID: 16675
	public string initialName;
}
