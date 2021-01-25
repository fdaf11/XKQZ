using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000300 RID: 768
	public class UIEnd : UILayer
	{
		// Token: 0x06001056 RID: 4182 RVA: 0x0000AB4F File Offset: 0x00008D4F
		protected override void Awake()
		{
			base.Awake();
			if (!Game.layerDeleteList.Contains(this))
			{
				Game.layerDeleteList.Add(this);
			}
			GameGlobal.m_bUIEnd = true;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0000AB78 File Offset: 0x00008D78
		private void OnDestroy()
		{
			GameGlobal.m_bUIEnd = false;
			if (Game.layerDeleteList.Contains(this))
			{
				Game.layerDeleteList.Remove(this);
			}
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x0008D8FC File Offset: 0x0008BAFC
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIEnd.<>f__switch$mapA == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("MovieImage", 0);
					dictionary.Add("DescLabel", 1);
					dictionary.Add("BackgroundImage", 2);
					dictionary.Add("Mask", 3);
					dictionary.Add("FullscreenButton", 4);
					dictionary.Add("StaffList", 5);
					UIEnd.<>f__switch$mapA = dictionary;
				}
				int num;
				if (UIEnd.<>f__switch$mapA.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.movieImage = sender;
						break;
					case 1:
						this.descLabel = sender;
						break;
					case 2:
						this.backgroundImage = sender;
						break;
					case 3:
						this.maskPanel = sender.GetComponent<UIPanel>();
						break;
					case 4:
						this.fullscreenButton = sender;
						this.fullscreenButton.OnClick += this.OnFullscreenButtonClick;
						break;
					case 5:
						this.staffList = sender.GetComponent<UIWidget>();
						break;
					}
				}
			}
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x0000AB9C File Offset: 0x00008D9C
		[ContextMenu("Play 1")]
		private void PlayTest1()
		{
			base.StartCoroutine(this._Play("谷月軒與荊棘，甫出谷便遭逢強敵，受到極大挫敗。幸得一浪人相助，方能保全性命。浪人帶著一只金翅巨鷹，神武非常，竟與傳聞中的天龍教護法迦樓羅神似。大難不死的師兄弟二人，從此隱居逍遙谷，潛心修練。\n\n兩年後，谷月軒、荊棘武藝大成，重出江湖，闖下一番名號。谷月軒在追捕巨盜陝北十三雁的過程中，結識胸懷英雄夢的少年東方未明，因緣際會成為同門，逍遙三俠之名不脛而走。有詩云：\n\n\u3000\u3000\u3000\u3000東有新星出北斗，一方谷月照軒桐\n\u3000\u3000\u3000\u3000荊棘未艾道多舛，曦馭乍明驚雁鴻\n\n另一方面，酆都與天龍教的鬥爭越演越烈，雙雄終在天都峰上一決死戰。酆都幫主閻羅敗給了龍王，天龍教大獲全勝，但也因此元氣大傷，延緩了併吞武林的野望。而酆都則全盤覆滅，埋沒在江湖的洪流之中……\n自此，武林似乎暫得太平。在法外三旬中，點蒼派、弦劍山莊、南少林慘遭屠戮，峨眉派避世不出，歸隱山林，正道受創斐淺。", this.movie, false));
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x0000ABB7 File Offset: 0x00008DB7
		[ContextMenu("Play 2")]
		private void PlayTest2()
		{
			base.StartCoroutine(this._Play("峨嵋雖安然無恙地度過這場武林大劫，但掌門浪花師太幾經思量，決定率領門徒遠渡域外。\n曾經顯赫一時的峨嵋派，從此消失在紛擾的中原武林。\n待峨嵋在西域落地生根後，水盼盼告別師門，漫遊廣袤的西方世界。", this.movie2, false));
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0008DA28 File Offset: 0x0008BC28
		[ContextMenu("Play 3")]
		private void PlayTest3()
		{
			List<EndMovieData> list = new List<EndMovieData>();
			list.Add(new EndMovieData
			{
				movieId = 1,
				endDesc = "谷月軒泯然於眾，再無一人知道其下落。\n有人說，荊棘為報師仇，加入天龍教。他的身影也被淹沒在天龍教與辟邪宮連年大戰之中。\n不知多少年後，有一比丘尼在逍遙谷舊址上搭建了一座簡陋的草廬，在此過著青燈禮佛的隱世生活。\n時不時，會看到她在屋外的三座無名墳塚前落淚低語……"
			});
			list.Add(new EndMovieData
			{
				movieId = 2,
				endDesc = "谷月軒泯然於眾，再無一人知道其下落。\n有人說，荊棘為報師仇，加入天龍教。他的身影也被淹沒在天龍教與辟邪宮連年大戰之中。\n不知多少年後，有一比丘尼在逍遙谷舊址上搭建了一座簡陋的草廬，在此過著青燈禮佛的隱世生活。\n時不時，會看到她在屋外的三座無名墳塚前落淚低語……"
			});
			list.Add(new EndMovieData
			{
				movieId = 3,
				endDesc = "谷月軒泯然於眾，再無一人知道其下落。\n有人說，荊棘為報師仇，加入天龍教。他的身影也被淹沒在天龍教與辟邪宮連年大戰之中。\n不知多少年後，有一比丘尼在逍遙谷舊址上搭建了一座簡陋的草廬，在此過著青燈禮佛的隱世生活。\n時不時，會看到她在屋外的三座無名墳塚前落淚低語……"
			});
			list.Add(new EndMovieData
			{
				movieId = 4,
				endDesc = "谷月軒泯然於眾，再無一人知道其下落。\n有人說，荊棘為報師仇，加入天龍教。他的身影也被淹沒在天龍教與辟邪宮連年大戰之中。\n不知多少年後，有一比丘尼在逍遙谷舊址上搭建了一座簡陋的草廬，在此過著青燈禮佛的隱世生活。\n時不時，會看到她在屋外的三座無名墳塚前落淚低語……"
			});
			list.Add(new EndMovieData
			{
				movieId = 7,
				endDesc = "谷月軒泯然於眾，再無一人知道其下落。\n有人說，荊棘為報師仇，加入天龍教。他的身影也被淹沒在天龍教與辟邪宮連年大戰之中。\n不知多少年後，有一比丘尼在逍遙谷舊址上搭建了一座簡陋的草廬，在此過著青燈禮佛的隱世生活。\n時不時，會看到她在屋外的三座無名墳塚前落淚低語……"
			});
			list.Add(new EndMovieData
			{
				movieId = 12,
				endDesc = "谷月軒泯然於眾，再無一人知道其下落。\n有人說，荊棘為報師仇，加入天龍教。他的身影也被淹沒在天龍教與辟邪宮連年大戰之中。\n不知多少年後，有一比丘尼在逍遙谷舊址上搭建了一座簡陋的草廬，在此過著青燈禮佛的隱世生活。\n時不時，會看到她在屋外的三座無名墳塚前落淚低語……"
			});
			List<EndMovieData> endingList = list;
			base.StartCoroutine(this.Play(endingList, true));
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x0000ABD2 File Offset: 0x00008DD2
		[ContextMenu("Play 4")]
		private void PlayTest4()
		{
			this.PlayEnd(2, UIEnd.PlayType.Normal);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0000ABDC File Offset: 0x00008DDC
		[ContextMenu("Play Staff")]
		private void PlayStaff()
		{
			base.StartCoroutine(this.Play("Ending.ogv", this.movieImage, true));
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0000ABF7 File Offset: 0x00008DF7
		public void PlayEnd(int endingId, int type)
		{
			this.PlayEnd(endingId, (UIEnd.PlayType)type);
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0008DB24 File Offset: 0x0008BD24
		public void PlayEnd(int endingId, UIEnd.PlayType type)
		{
			MainEndDataNode mainEndDataNode = Game.MainEndData.GetMainEndDataNode(endingId);
			if (mainEndDataNode == null)
			{
				Debug.LogError("找不到編號為 " + endingId + " 的結局 !!");
				return;
			}
			Game.StopMainCameraBGMusic();
			Game.StopUIRootBGMusic();
			if (Game.g_AudioBundle.Contains("audio/Map/" + mainEndDataNode.m_strMusic))
			{
				AudioClip audioClip = Game.g_AudioBundle.Load("audio/Map/" + mainEndDataNode.m_strMusic) as AudioClip;
				if (audioClip)
				{
					Game.PlayBGMusicClip(audioClip);
				}
				else
				{
					Debug.Log("Can't find music " + mainEndDataNode.m_strMusic + " !!");
				}
			}
			else
			{
				Debug.Log("Can't find music bundle " + mainEndDataNode.m_strMusic + " !!");
			}
			List<EndMovieData> list = new List<EndMovieData>();
			list.Add(new EndMovieData
			{
				movieId = mainEndDataNode.m_iEndID,
				endDesc = mainEndDataNode.m_strDesc
			});
			List<EndMovieData> list2 = list;
			if (mainEndDataNode.m_iChildEndPlay == 1)
			{
				List<EndMovieData> list3 = Game.ChildEndData.CheckEnd();
				if (list3 != null)
				{
					list2.AddRange(list3);
				}
			}
			bool isPlayStaff = endingId == 1 || endingId == 2;
			base.StartCoroutine(this.Play(list2, isPlayStaff));
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0008DC70 File Offset: 0x0008BE70
		private IEnumerator Play(List<EndMovieData> endingList, bool isPlayStaff)
		{
			GameGlobal.m_bCFormOpen = true;
			Game.g_InputManager.Push(this);
			for (int i = 0; i < endingList.Count; i++)
			{
				EndMovieData item = endingList[i];
				string name = "end" + item.movieId.ToString("00");
				string path = Path.GetDirectoryName(Application.dataPath) + "/EndingMovie/" + name + ".pk";
				Debug.Log(path);
				AssetBundle asb = AssetBundle.CreateFromFile(path);
				AssetBundleRequest task = asb.LoadAsync(name, typeof(MovieTexture));
				if (task == null)
				{
					Debug.LogError("Can't load movie " + name);
				}
				else
				{
					while (!task.isDone)
					{
						yield return null;
					}
					MovieTexture movie = task.asset as MovieTexture;
					while (!movie.isReadyToPlay)
					{
						yield return null;
					}
					bool badEnd = item.movieId == 4 || item.movieId == 5 || item.movieId == 6 || item.movieId == 7 || item.movieId == 22 || item.movieId == 21;
					string str = item.endDesc ?? string.Empty;
					IEnumerator iter = this._Play(str, movie, badEnd);
					while (iter.MoveNext())
					{
						yield return null;
					}
					Debug.Log("Wait for 5s !!");
					float s = 0f;
					while (s <= 5f && !this.skip)
					{
						s += Time.deltaTime;
						yield return null;
					}
					this.skip = false;
					asb.Unload(true);
				}
			}
			if (isPlayStaff)
			{
				this.maskPanel.gameObject.SetActive(false);
				Game.StopMainCameraBGMusic();
				Game.StopUIRootBGMusic();
				IEnumerator staff = this.Play("Ending.ogv", this.movieImage, false);
				while (staff.MoveNext())
				{
					yield return null;
				}
			}
			if (GameGlobal.m_bDLCMode)
			{
				int dlcend = Game.Variable["DLC_continue"];
				if (dlcend > -1)
				{
					this.ReturnToTrueDLC();
				}
				else
				{
					this.ReturnToTitle();
				}
			}
			else
			{
				int trueend = Game.Variable["trueend"];
				if (trueend > -1 && isPlayStaff)
				{
					this.ReturnToTrueEnd();
				}
				else
				{
					this.ReturnToTitle();
				}
			}
			GameGlobal.m_bCFormOpen = false;
			Game.g_InputManager.Pop();
			yield break;
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0008DCA8 File Offset: 0x0008BEA8
		private IEnumerator _Play(string desc, MovieTexture movie, bool badEnd = false)
		{
			desc = (desc ?? "Error");
			this.descLabel.Text = desc;
			this.movieImage.Texture = movie;
			UITexture background = this.movieImage.GetComponent<UITexture>();
			UILabel label = this.descLabel.GetComponent<UILabel>();
			TweenPosition tween = this.descLabel.GetComponent<TweenPosition>();
			int halfHeight = (label.height >> 1) + ((int)this.maskPanel.height >> 1);
			tween.from = new Vector3(this.descLabel.X, (float)(-(float)halfHeight), 0f);
			tween.to = new Vector3(this.descLabel.X, (float)halfHeight, 0f);
			tween.ResetToBeginning();
			tween.duration = Mathf.Max((float)label.height, this.maskPanel.height) / this.speed;
			tween.PlayForward();
			movie.Play();
			while (movie.isPlaying && (!this.skip || !badEnd))
			{
				yield return null;
			}
			if (GameGlobal.m_bDLCMode)
			{
				while (tween.enabled)
				{
					yield return null;
				}
			}
			movie.Stop();
			yield break;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0008DCF0 File Offset: 0x0008BEF0
		private IEnumerator Play(string movieName, Control control, bool canSkip = false)
		{
			if (this.staffList)
			{
				Vector3 pos = this.staffList.transform.position;
				UITexture texture = this.movieImage.GetComponent<UITexture>();
				int halfHeight = texture.height >> 1;
				TweenPosition t = this.staffList.GetComponent<TweenPosition>();
				t.from = (t.to = this.staffList.transform.localPosition);
				t.from.y = (float)(-(float)halfHeight);
				t.to.y = (float)(this.staffList.height + halfHeight);
				t.ResetToBeginning();
				t.duration = this.staffListDuration;
				t.PlayForward();
			}
			string path = "file:///" + Application.streamingAssetsPath + "/" + movieName;
			WWW www = new WWW(path);
			yield return www;
			if (www.error != null)
			{
				Debug.LogError("Can't find movie at " + path + " !!");
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
			TweenAlpha tween = control.GameObject.GetComponent<TweenAlpha>();
			tween.ResetToBeginning();
			tween.PlayForward();
			yield break;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0000AC01 File Offset: 0x00008E01
		public void OnFullscreenButtonClick(GameObject go)
		{
			this.skip = true;
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0000AC01 File Offset: 0x00008E01
		public override void OnKeyDown(KeyControl.Key key)
		{
			this.skip = true;
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0008DD38 File Offset: 0x0008BF38
		private void ReturnToTitle()
		{
			Game.StopMainCameraBGMusic();
			Game.StopUIRootBGMusic();
			GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
			GameObject gameObject = Array.Find<GameObject>(array, (GameObject item) => item.name == "cFormLoad");
			UILoad component = gameObject.GetComponent<UILoad>();
			component.LoadStage("GameStart");
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0008DD90 File Offset: 0x0008BF90
		private void ReturnToTrueEnd()
		{
			Game.StopMainCameraBGMusic();
			Game.StopUIRootBGMusic();
			GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
			GameObject gameObject = Array.Find<GameObject>(array, (GameObject item) => item.name == "cFormLoad");
			UILoad component = gameObject.GetComponent<UILoad>();
			component.LoadStage("M0008_01");
			GameGlobal.m_TransferPos = Vector3.zero;
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0000AC0A File Offset: 0x00008E0A
		private void ReturnToTrueDLC()
		{
			Game.LoadScene("ReadyCombat");
		}

		// Token: 0x04001391 RID: 5009
		private const string DefaultText = "谷月軒泯然於眾，再無一人知道其下落。\n有人說，荊棘為報師仇，加入天龍教。他的身影也被淹沒在天龍教與辟邪宮連年大戰之中。\n不知多少年後，有一比丘尼在逍遙谷舊址上搭建了一座簡陋的草廬，在此過著青燈禮佛的隱世生活。\n時不時，會看到她在屋外的三座無名墳塚前落淚低語……";

		// Token: 0x04001392 RID: 5010
		private const string DefaultText2 = "谷月軒與荊棘，甫出谷便遭逢強敵，受到極大挫敗。幸得一浪人相助，方能保全性命。浪人帶著一只金翅巨鷹，神武非常，竟與傳聞中的天龍教護法迦樓羅神似。大難不死的師兄弟二人，從此隱居逍遙谷，潛心修練。\n\n兩年後，谷月軒、荊棘武藝大成，重出江湖，闖下一番名號。谷月軒在追捕巨盜陝北十三雁的過程中，結識胸懷英雄夢的少年東方未明，因緣際會成為同門，逍遙三俠之名不脛而走。有詩云：\n\n\u3000\u3000\u3000\u3000東有新星出北斗，一方谷月照軒桐\n\u3000\u3000\u3000\u3000荊棘未艾道多舛，曦馭乍明驚雁鴻\n\n另一方面，酆都與天龍教的鬥爭越演越烈，雙雄終在天都峰上一決死戰。酆都幫主閻羅敗給了龍王，天龍教大獲全勝，但也因此元氣大傷，延緩了併吞武林的野望。而酆都則全盤覆滅，埋沒在江湖的洪流之中……\n自此，武林似乎暫得太平。在法外三旬中，點蒼派、弦劍山莊、南少林慘遭屠戮，峨眉派避世不出，歸隱山林，正道受創斐淺。";

		// Token: 0x04001393 RID: 5011
		private const string DefaultText3 = "峨嵋雖安然無恙地度過這場武林大劫，但掌門浪花師太幾經思量，決定率領門徒遠渡域外。\n曾經顯赫一時的峨嵋派，從此消失在紛擾的中原武林。\n待峨嵋在西域落地生根後，水盼盼告別師門，漫遊廣袤的西方世界。";

		// Token: 0x04001394 RID: 5012
		public float speed = 10f;

		// Token: 0x04001395 RID: 5013
		public float staffListDuration = 143f;

		// Token: 0x04001396 RID: 5014
		public MovieTexture movie;

		// Token: 0x04001397 RID: 5015
		public MovieTexture movie2;

		// Token: 0x04001398 RID: 5016
		public int id;

		// Token: 0x04001399 RID: 5017
		private Control backgroundImage;

		// Token: 0x0400139A RID: 5018
		private Control movieImage;

		// Token: 0x0400139B RID: 5019
		private Control descLabel;

		// Token: 0x0400139C RID: 5020
		private Control fullscreenButton;

		// Token: 0x0400139D RID: 5021
		private UIPanel maskPanel;

		// Token: 0x0400139E RID: 5022
		private UIWidget staffList;

		// Token: 0x0400139F RID: 5023
		private bool skip;

		// Token: 0x02000301 RID: 769
		public enum PlayType
		{
			// Token: 0x040013A4 RID: 5028
			Normal,
			// Token: 0x040013A5 RID: 5029
			Achievement
		}
	}
}
