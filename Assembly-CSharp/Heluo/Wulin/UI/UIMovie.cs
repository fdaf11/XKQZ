using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000317 RID: 791
	public class UIMovie : UILayer
	{
		// Token: 0x06001121 RID: 4385 RVA: 0x00094284 File Offset: 0x00092484
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIMovie.<>f__switch$map13 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("MovieImage", 0);
					dictionary.Add("StartImage", 1);
					dictionary.Add("LogoImage", 2);
					dictionary.Add("Background", 3);
					UIMovie.<>f__switch$map13 = dictionary;
				}
				int num;
				if (UIMovie.<>f__switch$map13.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.movieImage = sender;
						this.movieImage.OnClick += this.OnMovieClick;
						break;
					case 1:
						this.startImage = sender;
						if (GameGlobal.m_iLangType == GameGlobal.GameLan.Cht)
						{
							UITexture component = this.startImage.GameObject.GetComponent<UITexture>();
							component.mainTexture = null;
						}
						break;
					case 2:
						this.logoImage = sender;
						break;
					case 3:
						this.backgroundImage = sender;
						break;
					}
				}
			}
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x00094390 File Offset: 0x00092590
		private void OnMovieClick(GameObject go)
		{
			if (PlayerPrefs.GetInt("Movie", 0) == 0)
			{
				return;
			}
			this.skip = true;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0000B2CD File Offset: 0x000094CD
		public override void OnKeyUp(KeyControl.Key key)
		{
			if (key == KeyControl.Key.OK)
			{
				this.OnMovieClick(null);
			}
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x0000B2DD File Offset: 0x000094DD
		protected override void Awake()
		{
			base.Awake();
			if (!Game.layerDeleteList.Contains(this))
			{
				Game.layerDeleteList.Add(this);
			}
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x000943B8 File Offset: 0x000925B8
		private IEnumerator Start()
		{
			Game.StopUIRootBGMusic();
			yield return new WaitForSeconds(2f);
			TweenAlpha tween = this.startImage.GetComponent<TweenAlpha>();
			tween.ResetToBeginning();
			tween.PlayForward();
			this.Show();
			yield break;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0000B300 File Offset: 0x00009500
		public void PlayLogo()
		{
			base.StartCoroutine(this.Play(this.logoName, this.logoImage, false));
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x0000B31C File Offset: 0x0000951C
		public void PlayMovie()
		{
			base.StartCoroutine(this.Play(this.movieName, this.movieImage, true));
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x000943D4 File Offset: 0x000925D4
		public void EndPlayMovie()
		{
			TweenAlpha component = this.backgroundImage.GetComponent<TweenAlpha>();
			component.ResetToBeginning();
			component.PlayForward();
			Game.PlayBGMusicMapPath("Y000077", true);
			this.Hide();
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0009440C File Offset: 0x0009260C
		private IEnumerator Play(string movieName, Control control, bool canSkip = false)
		{
			string path = Application.streamingAssetsPath + "/" + movieName;
			string wwwPath = "file:///" + path;
			string temp = string.Empty;
			if (File.Exists(path))
			{
				temp = Path.GetTempPath() + movieName;
				File.Copy(path, temp, true);
				if (File.Exists(temp))
				{
					wwwPath = "file:///" + temp;
				}
			}
			WWW www = new WWW(wwwPath);
			yield return www;
			if (www.error != null)
			{
				Debug.LogError("Can't find movie at " + wwwPath + " !!");
			}
			else
			{
				MovieTexture movie = www.movie;
				while (!movie.isReadyToPlay)
				{
					yield return null;
				}
				control.Texture = movie;
				AudioSource audio = control.GameObject.GetComponent<AudioSource>();
				audio.clip = movie.audioClip;
				this.skip = false;
				movie.Play();
				audio.volume = GameGlobal.m_fMusicValue;
				audio.Play();
				while (movie.isPlaying)
				{
					if (this.skip && canSkip)
					{
						movie.Stop();
						audio.Stop();
					}
					yield return null;
				}
			}
			if (!this.skip)
			{
				yield return new WaitForSeconds(2f);
			}
			if (!string.IsNullOrEmpty(temp) && File.Exists(temp))
			{
				File.Delete(temp);
			}
			TweenAlpha tween = control.GameObject.GetComponent<TweenAlpha>();
			tween.ResetToBeginning();
			tween.PlayForward();
			yield break;
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x0000B338 File Offset: 0x00009538
		public override void Show()
		{
			base.Show();
			Game.g_InputManager.Push(this);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0000B34B File Offset: 0x0000954B
		public override void Hide()
		{
			base.Hide();
			PlayerPrefs.SetInt("Movie", 1);
			Game.g_InputManager.Pop();
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x0000B375 File Offset: 0x00009575
		[ContextMenu("Clear PlayerPrefs")]
		private void ClearPlayerPrefs()
		{
			PlayerPrefs.DeleteKey("Movie");
		}

		// Token: 0x040014CB RID: 5323
		private const string MovieWatchedKey = "Movie";

		// Token: 0x040014CC RID: 5324
		[SerializeField]
		private string logoName;

		// Token: 0x040014CD RID: 5325
		[SerializeField]
		private string movieName;

		// Token: 0x040014CE RID: 5326
		private Control startImage;

		// Token: 0x040014CF RID: 5327
		private Control logoImage;

		// Token: 0x040014D0 RID: 5328
		private Control movieImage;

		// Token: 0x040014D1 RID: 5329
		private Control backgroundImage;

		// Token: 0x040014D2 RID: 5330
		private bool skip;
	}
}
