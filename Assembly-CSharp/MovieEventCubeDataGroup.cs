using System;
using System.Collections.Generic;
using JsonFx.Json;
using UnityEngine;

// Token: 0x02000284 RID: 644
public class MovieEventCubeDataGroup
{
	// Token: 0x06000C02 RID: 3074 RVA: 0x000632C4 File Offset: 0x000614C4
	public void ToJson(List<MovieEventCube> lst)
	{
		this.m_MovieEventCubeList.Clear();
		for (int i = 0; i < lst.Count; i++)
		{
			this.Add(lst[i]);
		}
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x00063300 File Offset: 0x00061500
	public static void ToPlayMode(string text)
	{
		JsonReader jsonReader = new JsonReader(text);
		MovieEventCubeDataGroup movieEventCubeDataGroup = new MovieEventCubeDataGroup();
		movieEventCubeDataGroup = jsonReader.Deserialize<MovieEventCubeDataGroup>();
		GameObject gameObject = new GameObject("EventCube");
		gameObject.tag = "EventCube";
		Battle.AddMainGameList(gameObject);
		Transform transform = gameObject.transform;
		movieEventCubeDataGroup.ToPlayMode(transform);
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x0006334C File Offset: 0x0006154C
	public List<MovieEventCube> ToPlayMode(Transform mecParent)
	{
		List<MovieEventCube> list = new List<MovieEventCube>();
		for (int i = 0; i < this.m_MovieEventCubeList.Count; i++)
		{
			MovieEventCubeData movieEventCubeData = this.m_MovieEventCubeList[i];
			GameObject gameObject = new GameObject(movieEventCubeData.mecObjName);
			gameObject.transform.parent = mecParent;
			gameObject.transform.localPosition = movieEventCubeData.mecObjPos;
			gameObject.transform.localEulerAngles = movieEventCubeData.mecObjEulerAngles;
			gameObject.transform.localScale = movieEventCubeData.mecObjScale;
			gameObject.SetActive(movieEventCubeData.mecObjActive);
			if (movieEventCubeData.mecCliMode == _EventCubeColliderMode.Box)
			{
				BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
				boxCollider.isTrigger = movieEventCubeData.mecCliIsTrigger;
				boxCollider.size = movieEventCubeData.mecCliSize;
			}
			if (movieEventCubeData.mecCliMode == _EventCubeColliderMode.Sphere)
			{
				SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
				sphereCollider.isTrigger = movieEventCubeData.mecCliIsTrigger;
				sphereCollider.radius = movieEventCubeData.mecCliRedius;
			}
			MovieEventCube movieEventCube = gameObject.AddComponent<MovieEventCube>();
			movieEventCube.CopyFromData(this.m_MovieEventCubeList[i]);
			list.Add(movieEventCube);
		}
		return list;
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x00063474 File Offset: 0x00061674
	public void Add(MovieEventCube cube)
	{
		MovieEventCubeData movieEventCubeData = new MovieEventCubeData();
		if (movieEventCubeData.CopyFromComponent(cube))
		{
			this.Add(movieEventCubeData);
		}
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x0000928E File Offset: 0x0000748E
	public void Add(MovieEventCubeData cube)
	{
		if (cube == null)
		{
			return;
		}
		this.m_MovieEventCubeList.Add(cube);
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x000092A3 File Offset: 0x000074A3
	public void Remove(MovieEventCubeData cube)
	{
		if (cube == null)
		{
			return;
		}
		this.m_MovieEventCubeList.Remove(cube);
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x000092B9 File Offset: 0x000074B9
	public void Clear()
	{
		this.m_MovieEventCubeList.Clear();
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000C09 RID: 3081 RVA: 0x000092C6 File Offset: 0x000074C6
	public int Count
	{
		get
		{
			return this.m_MovieEventCubeList.Count;
		}
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x000092D3 File Offset: 0x000074D3
	public MovieEventCubeData GetData(int idx)
	{
		if (idx < 0 || idx >= this.m_MovieEventCubeList.Count)
		{
			return null;
		}
		return this.m_MovieEventCubeList[idx];
	}

	// Token: 0x04000DDF RID: 3551
	public string sceneName = string.Empty;

	// Token: 0x04000DE0 RID: 3552
	public List<MovieEventCubeData> m_MovieEventCubeList = new List<MovieEventCubeData>();
}
