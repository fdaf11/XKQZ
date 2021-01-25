using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200030E RID: 782
	public class UILoad : UILayer
	{
		// Token: 0x060010DB RID: 4315 RVA: 0x00091A38 File Offset: 0x0008FC38
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UILoad.<>f__switch$mapF == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("Loading", 0);
					dictionary.Add("ProgressBar", 1);
					dictionary.Add("Label", 2);
					dictionary.Add("LabelPrompt", 3);
					UILoad.<>f__switch$mapF = dictionary;
				}
				int num;
				if (UILoad.<>f__switch$mapF.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Loading = sender;
						break;
					case 1:
						this.m_ProgressBar = sender;
						break;
					case 2:
						this.m_Label = sender;
						break;
					case 3:
						this.m_LabelPrompt = sender;
						break;
					}
				}
			}
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0000AFD4 File Offset: 0x000091D4
		public void SetLoadingBarTitle(string strText)
		{
			if (this.m_Label != null && this.m_Label.GetComponent<UILabel>() != null)
			{
				this.m_Label.GetComponent<UILabel>().text = strText;
			}
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0000B008 File Offset: 0x00009208
		public void LoadStage(string strName)
		{
			Game.LoadScene(strName);
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0000B010 File Offset: 0x00009210
		public override void Show()
		{
			this.ShowLoading();
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00091B0C File Offset: 0x0008FD0C
		public void ShowLoading()
		{
			string text;
			int num;
			if (!TeamStatus.m_Instance.CheckMovieEvent(205001004))
			{
				text = "UI_loading_01";
			}
			else if (!MissionStatus.m_instance.CheckCollectionQuest("Q000001"))
			{
				text = "UI_loading_02";
			}
			else if (!TeamStatus.m_Instance.CheckMovieEvent(200801001) && Game.CurSceneName == "M0004_02")
			{
				text = "UI_loading_03";
			}
			else if (!TeamStatus.m_Instance.CheckMovieEvent(200801001) && Game.CurSceneName == "M0000_01")
			{
				text = "UI_loading_04";
			}
			else if (!MissionStatus.m_instance.CheckCollectionQuest("Q000003"))
			{
				text = "UI_loading_05";
			}
			else
			{
				num = Random.Range(0, this.m_ImageID.Length);
				text = this.m_ImageID[num];
			}
			Texture2D texture2D = Game.g_LoadBackground.Load("2dtexture/gameui/loadbackground/" + text) as Texture2D;
			if (texture2D != null)
			{
				this.m_Loading.GetComponent<UITexture>().mainTexture = texture2D;
			}
			this.m_Loading.GameObject.SetActive(true);
			this.m_fShowTime = Time.time;
			num = Random.Range(0, this.m_iMax);
			if (GameGlobal.m_bDLCMode)
			{
				this.m_LabelPrompt.Text = Game.StringTable.GetString(this.m_iDLCBaseIndex + num);
			}
			else
			{
				this.m_LabelPrompt.Text = Game.StringTable.GetString(this.m_iBaseIndex + num);
			}
			this.m_bShow = true;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00091C80 File Offset: 0x0008FE80
		public void SetLoadingPercentage(int iValue)
		{
			float fillAmount = (float)iValue / 100f;
			if (this.m_ProgressBar != null && this.m_ProgressBar.GetComponent<UISprite>() != null)
			{
				this.m_ProgressBar.GetComponent<UISprite>().fillAmount = fillAmount;
			}
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0000B018 File Offset: 0x00009218
		public void HideLoading()
		{
			this.m_Loading.GameObject.SetActive(false);
			this.m_bShow = false;
			this.m_fShowTime = 0f;
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x0000B03D File Offset: 0x0000923D
		private void Start()
		{
			if (GameGlobal.m_bLoading)
			{
				this.ShowLoading();
				this.SetLoadingPercentage(GameGlobal.m_iLoadPos);
				this.SetLoadingBarTitle(Game.StringTable.GetString(GameGlobal.m_iLoadStringID));
			}
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00091CC8 File Offset: 0x0008FEC8
		private void Update()
		{
			if (this.m_bShow && Time.time - this.m_fShowTime > 5f)
			{
				this.m_fShowTime = Time.time;
				int num = Random.Range(0, this.m_iMax);
				if (GameGlobal.m_bDLCMode)
				{
					this.m_LabelPrompt.Text = Game.StringTable.GetString(this.m_iDLCBaseIndex + num);
				}
				else
				{
					this.m_LabelPrompt.Text = Game.StringTable.GetString(this.m_iBaseIndex + num);
				}
			}
			if (GameGlobal.m_bLoading && this.m_bShow)
			{
				this.SetLoadingPercentage(GameGlobal.m_iLoadPos);
				this.SetLoadingBarTitle(Game.StringTable.GetString(GameGlobal.m_iLoadStringID));
			}
		}

		// Token: 0x04001457 RID: 5207
		private Control m_Loading;

		// Token: 0x04001458 RID: 5208
		private Control m_ProgressBar;

		// Token: 0x04001459 RID: 5209
		private Control m_Label;

		// Token: 0x0400145A RID: 5210
		private Control m_LabelPrompt;

		// Token: 0x0400145B RID: 5211
		private float m_fShowTime;

		// Token: 0x0400145C RID: 5212
		private int m_iMax = 50;

		// Token: 0x0400145D RID: 5213
		private int m_iBaseIndex = 980001;

		// Token: 0x0400145E RID: 5214
		private int m_iDLCBaseIndex = 990201;

		// Token: 0x0400145F RID: 5215
		private string[] m_ImageID = new string[]
		{
			"UI_loding_03",
			"UI_loding_04",
			"UI_loding_05",
			"UI_loding_06",
			"UI_loding_07",
			"UI_loding_08",
			"UI_loding_09",
			"UI_loding_10",
			"UI_loding_11",
			"UI_loding_12"
		};
	}
}
