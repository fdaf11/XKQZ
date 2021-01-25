using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000339 RID: 825
	public class UIBiographies : UILayer
	{
		// Token: 0x06001288 RID: 4744 RVA: 0x000A199C File Offset: 0x0009FB9C
		protected override void Awake()
		{
			base.Awake();
			CtrlBiographies biographiesController = this.m_BiographiesController;
			biographiesController.OnSetBiographiesView = (Action<BiographiesNode>)Delegate.Combine(biographiesController.OnSetBiographiesView, new Action<BiographiesNode>(this.setBiographiesView));
			CtrlBiographies biographiesController2 = this.m_BiographiesController;
			biographiesController2.OnSetBiographiesReward = (Action<int>)Delegate.Combine(biographiesController2.OnSetBiographiesReward, new Action<int>(this.setBiographiesReward));
			CtrlBiographies biographiesController3 = this.m_BiographiesController;
			biographiesController3.OnOverRound = (Func<bool>)Delegate.Combine(biographiesController3.OnOverRound, new Func<bool>(this.onOverRound));
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x000A1A24 File Offset: 0x0009FC24
		private void Start()
		{
			if (Game.UI.Root.GetComponentInChildren<Camera>() != null && Game.UI.Root.GetComponentInChildren<Camera>().audio != null)
			{
				Game.UI.Root.GetComponentInChildren<Camera>().audio.volume = GameGlobal.m_fMusicValue;
			}
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x000A1A94 File Offset: 0x0009FC94
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIBiographies.<>f__switch$map2D == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(19);
					dictionary.Add("BackgroundGroup", 0);
					dictionary.Add("ButtomBG", 1);
					dictionary.Add("TalkGroups", 2);
					dictionary.Add("TalkLabelLeft", 3);
					dictionary.Add("TalkLabelRight", 4);
					dictionary.Add("TalkLabelBottom", 5);
					dictionary.Add("TalkLeftBack", 6);
					dictionary.Add("TalkRightBack", 7);
					dictionary.Add("TalkBottom", 8);
					dictionary.Add("FromIconBtm", 9);
					dictionary.Add("NpcIcon", 10);
					dictionary.Add("ScenesBackgroundTexture", 11);
					dictionary.Add("EventBox", 12);
					dictionary.Add("EventBackgroundTexture", 13);
					dictionary.Add("TalkEvenGroup", 14);
					dictionary.Add("RoundEndBackground", 15);
					dictionary.Add("OverScene", 16);
					dictionary.Add("RoundOver", 17);
					dictionary.Add("RoundInterlude", 18);
					UIBiographies.<>f__switch$map2D = dictionary;
				}
				int num;
				if (UIBiographies.<>f__switch$map2D.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
					{
						Control control = sender;
						BackgroundGroup component = control.GetComponent<BackgroundGroup>();
						this.m_BackgroundGroup.Add(component);
						break;
					}
					case 1:
						this.m_ButtomBG = sender;
						break;
					case 2:
						this.m_TalkGroups = sender;
						this.m_TalkGroups.OnClick += this.TalkGroupsOnClick;
						base.SetInputButton(0, UIEventListener.Get(sender.gameObject));
						break;
					case 3:
						this.m_TalkLabelLeft = sender;
						break;
					case 4:
						this.m_TalkLabelRight = sender;
						break;
					case 5:
						this.m_TalkLabelBottom = sender;
						break;
					case 6:
						this.m_TalkLeftBack = sender;
						break;
					case 7:
						this.m_TalkRightBack = sender;
						break;
					case 8:
						this.m_TalkBottom = sender;
						break;
					case 9:
					{
						Control control = sender;
						this.m_FromList.Add(control);
						break;
					}
					case 10:
					{
						Control control = sender;
						this.m_QHeadList.Add(control);
						break;
					}
					case 11:
						this.m_ScenesBackground = sender;
						break;
					case 12:
						this.m_EventBox = sender;
						break;
					case 13:
						this.m_EventBackgroundTexture = sender;
						break;
					case 14:
						this.m_TalkEvenGroup = sender;
						break;
					case 15:
						this.m_RoundEndBackground = sender;
						break;
					case 16:
						this.m_OverScene = sender;
						this.OverScene_Tween_Position = sender.GetComponent<TweenPosition>();
						break;
					case 17:
						this.m_RoundOver = sender;
						this.RoundOver_Tween_Alpha = sender.GetComponent<TweenAlpha>();
						break;
					case 18:
					{
						Control control = sender;
						this.m_RoundInterludeList.Add(control);
						break;
					}
					}
				}
			}
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x000A1DC8 File Offset: 0x0009FFC8
		public override void OnKeyUp(KeyControl.Key key)
		{
			if (key != KeyControl.Key.OK)
			{
				if (key != KeyControl.Key.Cancel)
				{
				}
			}
			else
			{
				base.OnCurrentClick();
			}
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x000A1DFC File Offset: 0x0009FFFC
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			base.gameObject.SetActive(true);
			this.current = UIEventListener.Get(this.m_TalkGroups.GameObject);
			Game.g_InputManager.Push(this);
		}

		// Token: 0x0600128D RID: 4749 RVA: 0x0000C033 File Offset: 0x0000A233
		public override void Hide()
		{
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			base.gameObject.SetActive(false);
			Game.g_InputManager.Pop();
		}

		// Token: 0x0600128E RID: 4750 RVA: 0x0000C05F File Offset: 0x0000A25F
		private void setBiographiesReward(int id)
		{
			this.Hide();
			Game.RewardData.DoRewardID(id, null);
			Debug.Log("獎勵 : " + id);
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x000A1E4C File Offset: 0x000A004C
		private void setBiographiesView(BiographiesNode node)
		{
			if (node == null)
			{
				this.Hide();
				return;
			}
			this.Show();
			this.setMassage(node.m_eMsgPlace, node.m_Message);
			this.setAudio(node.m_Voice);
			this.setQHead(node.m_iOrder, node.m_BiographiesNpcQHeadNodeList);
			this.setBackground(node.m_BackgroundImage);
			this.setEventImage(node.m_Image);
			this.setOrverRound(node.m_EndMovie);
		}

		// Token: 0x06001290 RID: 4752 RVA: 0x0000C088 File Offset: 0x0000A288
		public void StartBiographies(string id)
		{
			this.m_BiographiesController.StartBiographies(id);
		}

		// Token: 0x06001291 RID: 4753 RVA: 0x0000C096 File Offset: 0x0000A296
		private void resetAll()
		{
			this.m_ButtomBG.GameObject.SetActive(false);
			this.resetQNpcHead();
			this.resetNpcTalkSetting();
			this.resetEventImage();
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0000C0BB File Offset: 0x0000A2BB
		private void resetNpcTalkSetting()
		{
			this.m_TalkGroups.GameObject.SetActive(false);
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x000A1EC0 File Offset: 0x000A00C0
		private void resetQNpcHead()
		{
			for (int i = 0; i < this.m_QHeadList.Count; i++)
			{
				this.m_QHeadList[i].GameObject.SetActive(false);
			}
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x000A1F00 File Offset: 0x000A0100
		private void setEventImage(string image)
		{
			if (image.Length > 1)
			{
				this.m_TalkEvenGroup.GameObject.SetActive(true);
				this.m_EventBox.GameObject.SetActive(true);
				Texture2D mainTexture = Game.g_DevelopTalkEvent.Load("2dtexture/gameui/develop/developtalkevent/" + image) as Texture2D;
				this.m_EventBackgroundTexture.GetComponent<UITexture>().mainTexture = mainTexture;
				this.m_EventBackgroundTexture.GameObject.SetActive(true);
			}
			else
			{
				this.m_TalkEvenGroup.GameObject.SetActive(false);
			}
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0000C0CE File Offset: 0x0000A2CE
		private void resetEventImage()
		{
			this.m_TalkEvenGroup.GameObject.SetActive(false);
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0000C0E1 File Offset: 0x0000A2E1
		private void setOrverRound(BiographiesNode.eEndMovie endmove)
		{
			this.m_EndMove = endmove;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x000A1F90 File Offset: 0x000A0190
		private void resetOrverRound()
		{
			this.m_RoundEndBackground.GameObject.SetActive(false);
			this.m_OverScene.GameObject.SetActive(false);
			this.m_RoundOver.GameObject.SetActive(false);
			for (int i = 0; i < this.m_RoundInterludeList.Count; i++)
			{
				this.m_RoundInterludeList[i].GameObject.SetActive(false);
			}
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x000A2004 File Offset: 0x000A0204
		private bool onOverRound()
		{
			if (this.m_EndMove == BiographiesNode.eEndMovie.None)
			{
				this.m_RoundEndBackground.GameObject.SetActive(false);
				return false;
			}
			this.resetAll();
			this.m_BiographiesController.m_bOverRound = true;
			Texture2D mainTexture = Game.g_DevelopBackground.Load("2dtexture/gameui/develop/developbackground/DM99999") as Texture2D;
			this.m_RoundEndBackground.GetComponent<UITexture>().mainTexture = mainTexture;
			this.m_RoundEndBackground.GameObject.SetActive(true);
			for (int i = 0; i < this.m_RoundInterludeList.Count; i++)
			{
				this.m_RoundInterludeList[i].GameObject.SetActive(true);
			}
			this.m_OverScene.GameObject.SetActive(true);
			this.OverScene_Tween_Position.ResetToBeginning();
			this.OverScene_Tween_Position.Play();
			if (this.m_EndMove == BiographiesNode.eEndMovie.OverAndNight)
			{
				this.m_RoundOver.GameObject.SetActive(true);
				this.RoundOver_Tween_Alpha.ResetToBeginning();
				this.RoundOver_Tween_Alpha.Play();
			}
			return true;
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x0000C0EA File Offset: 0x0000A2EA
		public void OnOverRoundFinished()
		{
			this.resetOrverRound();
			this.m_BiographiesController.m_bOverRound = false;
			this.m_EndMove = BiographiesNode.eEndMovie.None;
			this.m_BiographiesController.Next();
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x000A2108 File Offset: 0x000A0308
		private void setMassage(BiographiesNode.eMsgPos pos, string msg)
		{
			this.m_TalkGroups.GameObject.SetActive(true);
			this.m_TalkLabelBottom.Text = string.Empty;
			this.m_TalkLabelLeft.Text = string.Empty;
			this.m_TalkLabelRight.Text = string.Empty;
			this.m_TalkLeftBack.GameObject.SetActive(false);
			this.m_TalkRightBack.GameObject.SetActive(false);
			this.m_TalkBottom.GameObject.SetActive(false);
			for (int i = 0; i < this.m_FromList.Count; i++)
			{
				this.m_FromList[i].GameObject.SetActive(false);
			}
			switch (pos)
			{
			case BiographiesNode.eMsgPos.Left:
			case BiographiesNode.eMsgPos.MediumLeft:
			case BiographiesNode.eMsgPos.MediumRight:
			case BiographiesNode.eMsgPos.Right:
			{
				int num = pos - BiographiesNode.eMsgPos.Left;
				this.m_FromList[num].GameObject.SetActive(true);
				this.m_TalkBottom.GameObject.SetActive(true);
				this.m_TalkLabelBottom.Text = msg;
				break;
			}
			case BiographiesNode.eMsgPos.BackLeft:
				this.m_TalkLeftBack.GameObject.SetActive(true);
				this.m_TalkLabelLeft.Text = msg;
				break;
			case BiographiesNode.eMsgPos.BackRight:
				this.m_TalkRightBack.GameObject.SetActive(true);
				this.m_TalkLabelRight.Text = msg;
				break;
			}
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x000A2264 File Offset: 0x000A0464
		private void setAudio(string voice)
		{
			AudioSource component = this.m_TalkGroups.GetComponent<AudioSource>();
			component.clip = null;
			component.Stop();
			if (Game.g_AudioBundle.Contains(voice))
			{
				AudioClip clip = Game.g_AudioBundle.Load("audio/UI/" + voice) as AudioClip;
				component.pitch = 1f;
				component.volume = GameGlobal.m_fSoundValue;
				component.clip = clip;
				component.loop = false;
				component.Play();
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x000A22E0 File Offset: 0x000A04E0
		private void setQHead(int order, List<BiographiesNpcQHeadNode> nodeList)
		{
			this.resetQNpcHead();
			for (int i = 0; i < nodeList.Count; i++)
			{
				BiographiesNpcQHeadNode biographiesNpcQHeadNode = nodeList[i];
				Control control = this.m_QHeadList[(int)biographiesNpcQHeadNode.m_ePos];
				if (control != null)
				{
					Texture2D mainTexture = Game.g_DevelopQHead.Load("2dtexture/gameui/develop/developqhead/" + biographiesNpcQHeadNode.m_QHeadName) as Texture2D;
					UITexture component = control.GetComponent<UITexture>();
					component.alpha = 1f;
					if (!(component == null))
					{
						component.mainTexture = mainTexture;
						component.flip = biographiesNpcQHeadNode.m_Dir;
						control.GameObject.SetActive(true);
						if (biographiesNpcQHeadNode.m_ActionList.Count > 0)
						{
							BiographiesActionCtrl biographiesActionCtrl = new BiographiesActionCtrl(control, biographiesNpcQHeadNode.m_ActionList);
							this.m_BiographiesController.AddBiographiesActionCtrl(biographiesActionCtrl);
							BiographiesActionCtrl biographiesActionCtrl2 = biographiesActionCtrl;
							biographiesActionCtrl2.OnSetAction = (Action<Control, BiographiesActionCtrl>)Delegate.Combine(biographiesActionCtrl2.OnSetAction, new Action<Control, BiographiesActionCtrl>(this.OnSetAction));
							BiographiesActionCtrl biographiesActionCtrl3 = biographiesActionCtrl;
							biographiesActionCtrl3.OnActionFinish = (Action<BiographiesActionCtrl>)Delegate.Combine(biographiesActionCtrl3.OnActionFinish, new Action<BiographiesActionCtrl>(this.m_BiographiesController.RemoveBiographiesActionCtrl));
							biographiesActionCtrl.StartAction();
						}
					}
				}
			}
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x000A2414 File Offset: 0x000A0614
		private void setBackground(string backgroundImage)
		{
			this.m_ButtomBG.GameObject.SetActive(true);
			Texture2D texture2D = Game.g_DevelopBackground.Load("2dtexture/gameui/develop/developbackground/" + backgroundImage) as Texture2D;
			if (texture2D == null)
			{
				Debug.LogError("2dtexture/gameui/develop/developbackground/" + backgroundImage + " 讀不到");
			}
			this.m_ScenesBackground.GetComponent<UITexture>().mainTexture = texture2D;
			this.m_ScenesBackground.GameObject.SetActive(true);
			for (int i = 0; i < this.m_BackgroundGroup.Count; i++)
			{
				this.m_BackgroundGroup[i].gameObject.SetActive(false);
				if (this.m_BackgroundGroup[i].m_strBackgroundGroupID == backgroundImage)
				{
					this.m_BackgroundGroup[i].gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x0000C110 File Offset: 0x0000A310
		public void OnSetAction(Control ctrl, BiographiesActionCtrl bac)
		{
			this.SetFade(ctrl, bac);
			this.SetDisplacement(ctrl, bac);
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x000A24F8 File Offset: 0x000A06F8
		private void SetFade(Control ctrl, BiographiesActionCtrl bac)
		{
			BiographiesActionNode currentNode = bac.GetCurrentNode();
			if (currentNode.m_eFadeType == BiographiesActionNode.eFade.None)
			{
				return;
			}
			TweenAlpha tweenAlpha = ctrl.GameObject.GetComponent<TweenAlpha>();
			if (tweenAlpha == null)
			{
				tweenAlpha = ctrl.GameObject.AddComponent<TweenAlpha>();
			}
			tweenAlpha.onFinished.Clear();
			EventDelegate eventDelegate = new EventDelegate(this, "OnFadeFinished");
			eventDelegate.parameters[0] = new EventDelegate.Parameter(bac);
			tweenAlpha.onFinished.Add(eventDelegate);
			tweenAlpha.duration = currentNode.m_fFadeTime;
			if (currentNode.m_eFadeType == BiographiesActionNode.eFade.FadeIn)
			{
				tweenAlpha.value = 0f;
				tweenAlpha.from = 0f;
				tweenAlpha.to = 1f;
			}
			else if (currentNode.m_eFadeType == BiographiesActionNode.eFade.FadeOut)
			{
				tweenAlpha.value = 1f;
				tweenAlpha.from = 1f;
				tweenAlpha.to = 0f;
			}
			tweenAlpha.ResetToBeginning();
			tweenAlpha.ignoreTimeScale = true;
			tweenAlpha.enabled = true;
			bac.m_bFade = true;
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x000A25F4 File Offset: 0x000A07F4
		private void OnFadeFinished(object arg)
		{
			BiographiesActionCtrl biographiesActionCtrl = arg as BiographiesActionCtrl;
			if (biographiesActionCtrl != null)
			{
				biographiesActionCtrl.m_bFade = false;
				biographiesActionCtrl.NextAction();
			}
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x000A261C File Offset: 0x000A081C
		private void SetDisplacement(Control ctrl, BiographiesActionCtrl bac)
		{
			BiographiesActionNode currentNode = bac.GetCurrentNode();
			if (currentNode.m_eDisplacement == BiographiesActionNode.eDisplacement.None)
			{
				return;
			}
			TweenPosition tweenPosition = ctrl.GameObject.GetComponent<TweenPosition>();
			if (tweenPosition == null)
			{
				tweenPosition = ctrl.GameObject.AddComponent<TweenPosition>();
			}
			tweenPosition.onFinished.Clear();
			EventDelegate eventDelegate = new EventDelegate(this, "OnDisplacementFinished");
			eventDelegate.parameters[0] = new EventDelegate.Parameter(bac);
			tweenPosition.onFinished.Add(eventDelegate);
			tweenPosition.duration = currentNode.m_fDisTime;
			if (currentNode.m_eDisplacement == BiographiesActionNode.eDisplacement.LeftIn)
			{
				tweenPosition.from = new Vector3(-1280f, ctrl.GameObject.transform.localPosition.y, 0f);
			}
			else if (currentNode.m_eDisplacement == BiographiesActionNode.eDisplacement.BottomIn)
			{
				tweenPosition.from = ctrl.GameObject.transform.localPosition + new Vector3(0f, -860f, 0f);
			}
			else if (currentNode.m_eDisplacement == BiographiesActionNode.eDisplacement.RightIn)
			{
				tweenPosition.from = ctrl.GameObject.transform.localPosition + new Vector3(1280f, ctrl.GameObject.transform.localPosition.y, 0f);
			}
			else if (currentNode.m_eDisplacement == BiographiesActionNode.eDisplacement.TopIn)
			{
				tweenPosition.from = ctrl.GameObject.transform.localPosition + new Vector3(0f, 860f, 0f);
			}
			tweenPosition.to = ctrl.GameObject.transform.localPosition;
			tweenPosition.ResetToBeginning();
			tweenPosition.ignoreTimeScale = true;
			tweenPosition.enabled = true;
			bac.m_bDisplacement = true;
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x000A27DC File Offset: 0x000A09DC
		private void OnDisplacementFinished(object arg)
		{
			BiographiesActionCtrl biographiesActionCtrl = arg as BiographiesActionCtrl;
			if (biographiesActionCtrl != null)
			{
				biographiesActionCtrl.m_bDisplacement = false;
				biographiesActionCtrl.NextAction();
			}
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0000C122 File Offset: 0x0000A322
		private void TalkGroupsOnClick(GameObject go)
		{
			this.m_BiographiesController.Next();
		}

		// Token: 0x04001676 RID: 5750
		private CtrlBiographies m_BiographiesController = new CtrlBiographies();

		// Token: 0x04001677 RID: 5751
		private List<BackgroundGroup> m_BackgroundGroup = new List<BackgroundGroup>();

		// Token: 0x04001678 RID: 5752
		private Control m_ButtomBG;

		// Token: 0x04001679 RID: 5753
		private Control m_TalkGroups;

		// Token: 0x0400167A RID: 5754
		private Control m_TalkLabelLeft;

		// Token: 0x0400167B RID: 5755
		private Control m_TalkLabelRight;

		// Token: 0x0400167C RID: 5756
		private Control m_TalkLabelBottom;

		// Token: 0x0400167D RID: 5757
		private Control m_TalkLeftBack;

		// Token: 0x0400167E RID: 5758
		private Control m_TalkRightBack;

		// Token: 0x0400167F RID: 5759
		private Control m_TalkBottom;

		// Token: 0x04001680 RID: 5760
		private List<Control> m_FromList = new List<Control>();

		// Token: 0x04001681 RID: 5761
		private List<Control> m_QHeadList = new List<Control>();

		// Token: 0x04001682 RID: 5762
		private Control m_EventBox;

		// Token: 0x04001683 RID: 5763
		private Control m_EventBackgroundTexture;

		// Token: 0x04001684 RID: 5764
		private Control m_TalkEvenGroup;

		// Token: 0x04001685 RID: 5765
		private Control m_ScenesBackground;

		// Token: 0x04001686 RID: 5766
		private Control m_RoundEndBackground;

		// Token: 0x04001687 RID: 5767
		private Control m_RoundOver;

		// Token: 0x04001688 RID: 5768
		private Control m_OverScene;

		// Token: 0x04001689 RID: 5769
		private List<Control> m_RoundInterludeList = new List<Control>();

		// Token: 0x0400168A RID: 5770
		private TweenPosition OverScene_Tween_Position;

		// Token: 0x0400168B RID: 5771
		private TweenAlpha RoundOver_Tween_Alpha;

		// Token: 0x0400168C RID: 5772
		private BiographiesNode.eEndMovie m_EndMove;
	}
}
