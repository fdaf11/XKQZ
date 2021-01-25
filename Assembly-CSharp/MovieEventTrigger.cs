using System;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public class MovieEventTrigger : MonoBehaviour
{
	// Token: 0x06000D6E RID: 3438 RVA: 0x000096C9 File Offset: 0x000078C9
	public void Start()
	{
		this.PreLoadMovie();
		if (this.iTestMovieID != 0)
		{
			this.PlayMovie(this.iTestMovieID);
		}
	}

	// Token: 0x06000D6F RID: 3439 RVA: 0x0006CE94 File Offset: 0x0006B094
	public void AddNpcToMovieList(GameObject go)
	{
		if (this.goMovieEvent != null)
		{
			MovieEventMap component = this.goMovieEvent.GetComponent<MovieEventMap>();
			if (component.IsMoviePlaying())
			{
				component.AddNpcToList(go);
			}
		}
		if (this.goContinueMovieEvent != null)
		{
			MovieEventMap component2 = this.goContinueMovieEvent.GetComponent<MovieEventMap>();
			if (component2.IsMoviePlaying())
			{
				component2.AddNpcToList(go);
			}
		}
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x0006CF00 File Offset: 0x0006B100
	public void PreLoadMovie()
	{
		string loadedLevelName = Application.loadedLevelName;
		string text = loadedLevelName + "MovieEvent";
		if (this.goMovieEvent != null && this.goMovieEvent.name != text)
		{
			this.goMovieEvent = null;
		}
		if (this.goMovieEvent == null)
		{
			this.goMovieEvent = new GameObject();
			MovieEventMap movieEventMap = this.goMovieEvent.AddComponent<MovieEventMap>();
			movieEventMap.LoadSceneAllGroup(loadedLevelName);
			this.goMovieEvent.name = text;
			this.goMovieEvent.tag = "EventCube";
		}
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x0006CF98 File Offset: 0x0006B198
	public void PlayMovie(int iMovieID)
	{
		GameCursor.Instance.SetMouseCursor(GameCursor.eCursorState.Normal);
		if (this.goMovieEvent == null)
		{
			this.PreLoadMovie();
		}
		MovieEventMap component = this.goMovieEvent.GetComponent<MovieEventMap>();
		if (component != null)
		{
			component.PlayMovie(iMovieID);
		}
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x0006CFE8 File Offset: 0x0006B1E8
	public void ChangeScenes(MovieEventNode men)
	{
		if (Game.IsLoading())
		{
			Debug.LogError("Movie ChangeScene but Already Loading  : " + men.strScenesName);
			return;
		}
		if (men.strScenesName == Application.loadedLevelName)
		{
			Debug.LogError("Movie Event ChangeScenes Same Scene");
			return;
		}
		if (men.NextNodeID >= 0)
		{
			this.strNewScene = men.strScenesName;
			this.strMovieScene = Application.loadedLevelName;
			this.iMovieGroupID = men.GroupID;
			this.iNextNodeID = men.NextNodeID;
			Game.LoadLevelOnFinish = (Game.LoadSenceOnFinish)Delegate.Combine(Game.LoadLevelOnFinish, new Game.LoadSenceOnFinish(this.MovieLoadLevelOnFinish));
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Equals("cFormLoad"))
			{
				array[i].GetComponent<UILoad>().LoadStage(men.strScenesName);
			}
		}
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x0006D0DC File Offset: 0x0006B2DC
	private void ContinueMovieAfterChangeScene()
	{
		this.goContinueMovieEvent = new GameObject();
		MovieEventMap movieEventMap = this.goContinueMovieEvent.AddComponent<MovieEventMap>();
		movieEventMap.LoadSceneAllGroup(this.strMovieScene);
		this.goContinueMovieEvent.name = this.strMovieScene + "MovieEvent";
		this.goContinueMovieEvent.tag = "EventCube";
		if (movieEventMap != null)
		{
			movieEventMap.ContinueMovie(this.iMovieGroupID, this.iNextNodeID);
		}
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x000096E8 File Offset: 0x000078E8
	public void MovieLoadLevelOnFinish()
	{
		Game.LoadLevelOnFinish = (Game.LoadSenceOnFinish)Delegate.Remove(Game.LoadLevelOnFinish, new Game.LoadSenceOnFinish(this.MovieLoadLevelOnFinish));
		this.ContinueMovieAfterChangeScene();
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x0006D158 File Offset: 0x0006B358
	public void BeforeChangeSceneResetMovieStyle()
	{
		if (this.goMovieEvent != null && this.goMovieEvent.GetComponent<MovieEventMap>() != null)
		{
			this.goMovieEvent.GetComponent<MovieEventMap>().ChangeSceneResetMovieStyle();
		}
		if (this.goContinueMovieEvent != null && this.goContinueMovieEvent.GetComponent<MovieEventMap>() != null)
		{
			this.goContinueMovieEvent.GetComponent<MovieEventMap>().ChangeSceneResetMovieStyle();
		}
	}

	// Token: 0x04000FAA RID: 4010
	public int iTestMovieID;

	// Token: 0x04000FAB RID: 4011
	private GameObject goMovieEvent;

	// Token: 0x04000FAC RID: 4012
	private GameObject goContinueMovieEvent;

	// Token: 0x04000FAD RID: 4013
	private int iMovieGroupID;

	// Token: 0x04000FAE RID: 4014
	private int iNextNodeID;

	// Token: 0x04000FAF RID: 4015
	private string strMovieScene;

	// Token: 0x04000FB0 RID: 4016
	private string strNewScene;
}
