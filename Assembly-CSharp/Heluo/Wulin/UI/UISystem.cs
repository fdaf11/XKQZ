using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000328 RID: 808
	public class UISystem : UILayer
	{
		// Token: 0x060011D0 RID: 4560 RVA: 0x0000B952 File Offset: 0x00009B52
		protected override void Awake()
		{
			this.m_BtnSystemBaseList = new List<UISystem.SystemBtnNode>();
			this.m_SelectLogoList = new List<Control>();
			base.Awake();
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0000B970 File Offset: 0x00009B70
		private void Start()
		{
			this.m_bOpenPlaying = false;
			this.m_bOnClick = false;
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x00098AAC File Offset: 0x00096CAC
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UISystem.<>f__switch$map21 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(25);
					dictionary.Add("Group", 0);
					dictionary.Add("BtnSystemBase", 1);
					dictionary.Add("SelectLogo", 2);
					dictionary.Add("InkBase", 3);
					dictionary.Add("GameSetting", 4);
					dictionary.Add("Close", 5);
					dictionary.Add("BackGround", 6);
					dictionary.Add("SSAOLabel", 7);
					dictionary.Add("FullscreenOption", 8);
					dictionary.Add("ResLabel", 8);
					dictionary.Add("ResOption", 8);
					dictionary.Add("ResTitle", 8);
					dictionary.Add("FastSelectLabel", 8);
					dictionary.Add("FastSelectOption", 8);
					dictionary.Add("FastSelectTitle", 8);
					dictionary.Add("ShadowOption", 9);
					dictionary.Add("ToneMappingOption", 9);
					dictionary.Add("vSyncOption", 9);
					dictionary.Add("AntiAliasOption", 9);
					dictionary.Add("SSAOOption", 9);
					dictionary.Add("TextureOption", 10);
					dictionary.Add("ObjectOption", 10);
					dictionary.Add("ShadowQualityOption", 10);
					dictionary.Add("MusicSlider", 11);
					dictionary.Add("SoundSlider", 12);
					UISystem.<>f__switch$map21 = dictionary;
				}
				int num;
				if (UISystem.<>f__switch$map21.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
					{
						UISystem.SystemBtnNode systemBtnNode = new UISystem.SystemBtnNode();
						systemBtnNode.m_BtnSystemBase = sender;
						systemBtnNode.m_BtnSystemBase.OnHover += this.BtnSystemBaseOnHover;
						systemBtnNode.m_BtnSystemBase.OnClick += this.BtnSystemBaseOnClick;
						systemBtnNode.BtnSystemBase_Tween_Position = sender.GetComponent<TweenPosition>();
						this.m_BtnSystemBaseList.Add(systemBtnNode);
						break;
					}
					case 2:
					{
						Control control = sender;
						this.m_SelectLogoList.Add(control);
						break;
					}
					case 3:
						this.m_InkBase = sender;
						this.Ink_Tween_Alpha = sender.GetComponent<TweenAlpha>();
						break;
					case 4:
						this.m_GameSetting = sender;
						break;
					case 5:
						this.m_Close = sender;
						this.m_Close.OnClick += this.GameSettingOnClose;
						break;
					case 6:
						if (sender.GetComponent<BoxCollider>() == null)
						{
							BoxCollider boxCollider = sender.gameObject.AddComponent<BoxCollider>();
							boxCollider.size = new Vector3(1920f, 1200f);
						}
						break;
					case 7:
						if (sender.GetComponent<LabelData>().m_iIndex == 110)
						{
							Control control = sender;
							control.GameObject.SetActive(false);
						}
						break;
					case 8:
					{
						Control control = sender;
						control.GameObject.SetActive(false);
						break;
					}
					case 9:
					{
						Control control = sender;
						this.m_OnOffList.Add(sender.gameObject);
						control.OnClick += this.OnOffOnClick;
						break;
					}
					case 10:
					{
						Control control = sender;
						this.m_GroupList.Add(sender.gameObject);
						control.OnClick += this.GroupOnClick;
						break;
					}
					case 11:
						this.m_sMusic = sender.GetComponent<UISlider>();
						EventDelegate.Add(this.m_sMusic.onChange, new EventDelegate.Callback(this.MusicOnChange));
						break;
					case 12:
						this.m_sSound = sender.GetComponent<UISlider>();
						EventDelegate.Add(this.m_sSound.onChange, new EventDelegate.Callback(this.SoundOnChange));
						break;
					}
				}
			}
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x00098E90 File Offset: 0x00097090
		private void BtnSystemBaseOnClick(GameObject go)
		{
			if (this.m_bOpenPlaying)
			{
				return;
			}
			this.m_bOnClick = true;
			int iIndex = go.GetComponent<SelectBtn>().m_iIndex;
			for (int i = 0; i < this.m_SelectLogoList.Count; i++)
			{
				Vector3 localPosition = this.m_SelectLogoList[i].GameObject.transform.localPosition;
				if (this.m_SelectLogoList[i].GetComponent<SelectBtn>().m_iIndex == iIndex)
				{
					localPosition..ctor(localPosition.x - 5f, localPosition.y, localPosition.z);
					this.m_SelectLogoList[i].GameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z);
				}
			}
			if (GameGlobal.m_bPrequel && (iIndex == 1 || iIndex == 2 || iIndex == 3 || iIndex == 4))
			{
				this.m_bOnClick = false;
				return;
			}
			GameGlobal.m_bCFormOpen = false;
			if (iIndex == 0)
			{
				this.FormClose();
			}
			else if (iIndex == 1)
			{
				this.m_bOnClick = false;
				this.m_Group.GameObject.SetActive(false);
				GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j].name.Equals("cFormStatus"))
					{
						break;
					}
				}
			}
			else if (iIndex == 2)
			{
				GameGlobal.m_bCFormOpen = true;
				this.UpdateGameSetting();
				this.m_GameSetting.GameObject.SetActive(true);
			}
			else if (iIndex == 3)
			{
				this.m_bOnClick = false;
				GameObject[] array2 = GameObject.FindGameObjectsWithTag("cForm");
				for (int k = 0; k < array2.Length; k++)
				{
					if (array2[k].name.Equals("cFormSaveLoad"))
					{
						this.m_Group.GameObject.SetActive(false);
						break;
					}
				}
			}
			else if (iIndex == 4)
			{
				this.m_bOnClick = false;
				GameObject[] array3 = GameObject.FindGameObjectsWithTag("cForm");
				for (int l = 0; l < array3.Length; l++)
				{
					if (array3[l].name.Equals("cFormSaveLoad"))
					{
						this.m_Group.GameObject.SetActive(false);
						break;
					}
				}
			}
			else if (iIndex == 5)
			{
				if (Game.IsLoading())
				{
					this.m_bOnClick = false;
					return;
				}
				GameGlobal.m_bPrequel = false;
				GameGlobal.m_bHouseInside = false;
				GameObject[] array4 = GameObject.FindGameObjectsWithTag("cForm");
				for (int m = 0; m < array4.Length; m++)
				{
					if (array4[m].name.Equals("cFormLoad"))
					{
						this.m_Group.GameObject.SetActive(false);
						array4[m].GetComponent<UILoad>().LoadStage("GameStart");
						break;
					}
				}
			}
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0000B980 File Offset: 0x00009B80
		public void FormClose()
		{
			this.m_Group.GameObject.SetActive(false);
			this.m_bOnClick = false;
			GameGlobal.m_bCFormOpen = false;
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x00099194 File Offset: 0x00097394
		public void SystemOpen()
		{
			GameGlobal.m_bCFormOpen = true;
			if (this.m_Group.GameObject.activeSelf)
			{
				return;
			}
			this.m_Group.GameObject.SetActive(true);
			this.m_Group.GetComponent<AudioSource>().volume = GameGlobal.m_fSoundValue;
			this.m_Group.GetComponent<AudioSource>().Play();
			this.m_bOpenPlaying = true;
			for (int i = 0; i < this.m_BtnSystemBaseList.Count; i++)
			{
				this.m_BtnSystemBaseList[i].m_BtnSystemBase.GameObject.SetActive(true);
				this.m_BtnSystemBaseList[i].BtnSystemBase_Tween_Position.ResetToBeginning();
				this.m_BtnSystemBaseList[i].BtnSystemBase_Tween_Position.Play(true);
				this.m_SelectLogoList[i].GameObject.SetActive(false);
				this.Ink_Tween_Alpha.ResetToBeginning();
				this.Ink_Tween_Alpha.Play(true);
			}
			if (!Game.layerHideList.Contains(this))
			{
				Game.layerHideList.Add(this);
			}
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x0000B9A0 File Offset: 0x00009BA0
		public override void Show()
		{
			this.SystemOpen();
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0000B9A8 File Offset: 0x00009BA8
		public override void Hide()
		{
			this.FormClose();
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x000992A8 File Offset: 0x000974A8
		private void BtnSystemBaseOnHover(GameObject go, bool isOver)
		{
			if (this.m_bOpenPlaying || this.m_bOnClick)
			{
				return;
			}
			int iIndex = go.GetComponent<SelectBtn>().m_iIndex;
			if (GameGlobal.m_bPrequel && (iIndex == 1 || iIndex == 2 || iIndex == 3 || iIndex == 4))
			{
				return;
			}
			for (int i = 0; i < this.m_SelectLogoList.Count; i++)
			{
				this.m_SelectLogoList[i].GameObject.SetActive(false);
				if (this.m_SelectLogoList[i].GetComponent<SelectBtn>().m_iIndex == iIndex)
				{
					Vector3 localPosition = this.m_SelectLogoList[i].GameObject.transform.localPosition;
					Vector3 localPosition2 = go.transform.localPosition;
					if (isOver)
					{
						this.m_SelectLogoList[i].GameObject.SetActive(true);
						localPosition..ctor(localPosition.x + 5f, localPosition.y, localPosition.z);
						this.m_SelectLogoList[i].GameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z);
						localPosition2..ctor(localPosition2.x + 5f, localPosition2.y, localPosition2.z);
						go.transform.localPosition = new Vector3(localPosition2.x, localPosition2.y, localPosition2.z);
					}
					else
					{
						localPosition..ctor(localPosition.x - 5f, localPosition.y, localPosition.z);
						this.m_SelectLogoList[i].GameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z);
						localPosition2..ctor(localPosition2.x - 5f, localPosition2.y, localPosition2.z);
						go.transform.localPosition = new Vector3(localPosition2.x, localPosition2.y, localPosition2.z);
					}
				}
			}
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0000B9B0 File Offset: 0x00009BB0
		public void SystemClose()
		{
			this.m_GameSetting.GameObject.SetActive(false);
			this.FormClose();
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0000B9C9 File Offset: 0x00009BC9
		public void OpenFinished()
		{
			this.m_bOpenPlaying = false;
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x000994D4 File Offset: 0x000976D4
		private void GameSettingOnClose(GameObject go)
		{
			if (Game.IsLoading())
			{
				return;
			}
			this.bNeedToLoading = false;
			this.SetGameSetting();
			this.m_bOnClick = false;
			GameObject gameObject = GameObject.FindGameObjectWithTag("WorldTime");
			if (gameObject != null)
			{
			}
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("Player");
			if (gameObject2 != null)
			{
				GameGlobal.m_TransferPos = gameObject2.transform.localPosition;
				GameGlobal.m_fDir = gameObject2.transform.localEulerAngles.y;
			}
			if (this.bNeedToLoading && !GameGlobal.m_bUIDevelop)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].name.Equals("cFormLoad"))
					{
						array[i].GetComponent<UILoad>().LoadStage(Application.loadedLevelName);
						break;
					}
				}
			}
			this.SystemClose();
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x000995C0 File Offset: 0x000977C0
		private void SetGameSetting()
		{
			foreach (GameObject gameObject in this.m_OnOffList)
			{
				BtnData component = gameObject.GetComponent<BtnData>();
				UISprite component2 = gameObject.GetComponent<UISprite>();
				if (component2.spriteName == "UI_install_02")
				{
					this.NewOption[component.m_iBtnType] = 0;
				}
				else
				{
					this.NewOption[component.m_iBtnType] = 1;
				}
			}
			foreach (GameObject gameObject2 in this.m_GroupList)
			{
				UISprite component3 = gameObject2.GetComponent<UISprite>();
				if (!(component3.spriteName == "UI_install_03"))
				{
					BtnData component4 = gameObject2.GetComponent<BtnData>();
					this.NewOption[component4.m_iBtnType] = component4.m_iBtnID;
				}
			}
			this.ChangeSettingValue();
			string value = GameGlobal.m_fMusicValue.ToString("0.00");
			IniFile.IniWriteValue("setting", "MusicValue", value);
			value = GameGlobal.m_fSoundValue.ToString("0.00");
			IniFile.IniWriteValue("setting", "SoundValue", value);
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00099728 File Offset: 0x00097928
		private void ChangeSettingValue()
		{
			if (this.NewOption[4] != this.SaveOption[4])
			{
				this.bNeedToLoading = true;
				if (this.NewOption[4] == 0)
				{
					IniFile.IniWriteValue("setting", "shadow", "true");
					if (this.NewOption[5] == 0)
					{
						IniFile.IniWriteValue("setting", "shadowQuality", "low");
						QualitySelector.ApplyQuality(1);
					}
					else if (this.NewOption[5] == 1)
					{
						IniFile.IniWriteValue("setting", "shadowQuality", "medium");
						QualitySelector.ApplyQuality(2);
					}
					else if (this.NewOption[5] == 2)
					{
						IniFile.IniWriteValue("setting", "shadowQuality", "high");
						QualitySelector.ApplyQuality(3);
					}
					if (this.NewOption[6] == 0)
					{
						QualitySettings.shadowDistance = 35f;
						IniFile.IniWriteValue("setting", "shadowDistance", "low");
					}
					else if (this.NewOption[6] == 1)
					{
						QualitySettings.shadowDistance = 45f;
						IniFile.IniWriteValue("setting", "shadowDistance", "medium");
					}
					else if (this.NewOption[6] == 2)
					{
						QualitySettings.shadowDistance = 60f;
						IniFile.IniWriteValue("setting", "shadowDistance", "high");
					}
				}
				else
				{
					IniFile.IniWriteValue("setting", "shadow", "false");
					QualitySelector.ApplyQuality(0);
					if (this.NewOption[5] == 0)
					{
						IniFile.IniWriteValue("setting", "shadowQuality", "low");
					}
					else if (this.NewOption[5] == 1)
					{
						IniFile.IniWriteValue("setting", "shadowQuality", "medium");
					}
					else if (this.NewOption[5] == 2)
					{
						IniFile.IniWriteValue("setting", "shadowQuality", "high");
					}
					if (this.NewOption[6] == 0)
					{
						IniFile.IniWriteValue("setting", "shadowDistance", "low");
					}
					else if (this.NewOption[6] == 1)
					{
						IniFile.IniWriteValue("setting", "shadowDistance", "medium");
					}
					else if (this.NewOption[6] == 2)
					{
						IniFile.IniWriteValue("setting", "shadowDistance", "high");
					}
				}
			}
			else if (this.NewOption[4] == 0)
			{
				if (this.NewOption[5] != this.SaveOption[5])
				{
					this.bNeedToLoading = true;
					if (this.NewOption[5] == 0)
					{
						IniFile.IniWriteValue("setting", "shadowQuality", "low");
						QualitySelector.ApplyQuality(1);
					}
					else if (this.NewOption[5] == 1)
					{
						IniFile.IniWriteValue("setting", "shadowQuality", "medium");
						QualitySelector.ApplyQuality(2);
					}
					else if (this.NewOption[5] == 2)
					{
						IniFile.IniWriteValue("setting", "shadowQuality", "high");
						QualitySelector.ApplyQuality(3);
					}
				}
				if (this.NewOption[6] != this.SaveOption[6])
				{
					this.bNeedToLoading = true;
					if (this.NewOption[6] == 0)
					{
						QualitySettings.shadowDistance = 35f;
						IniFile.IniWriteValue("setting", "shadowDistance", "low");
					}
					else if (this.NewOption[6] == 1)
					{
						QualitySettings.shadowDistance = 45f;
						IniFile.IniWriteValue("setting", "shadowDistance", "medium");
					}
					else if (this.NewOption[6] == 2)
					{
						QualitySettings.shadowDistance = 60f;
						IniFile.IniWriteValue("setting", "shadowDistance", "high");
					}
				}
			}
			else
			{
				if (this.NewOption[5] == 0)
				{
					IniFile.IniWriteValue("setting", "shadowQuality", "low");
				}
				else if (this.NewOption[5] == 1)
				{
					IniFile.IniWriteValue("setting", "shadowQuality", "medium");
				}
				else if (this.NewOption[5] == 2)
				{
					IniFile.IniWriteValue("setting", "shadowQuality", "high");
				}
				if (this.NewOption[6] == 0)
				{
					IniFile.IniWriteValue("setting", "shadowDistance", "low");
				}
				else if (this.NewOption[6] == 1)
				{
					IniFile.IniWriteValue("setting", "shadowDistance", "medium");
				}
				else if (this.NewOption[6] == 2)
				{
					IniFile.IniWriteValue("setting", "shadowDistance", "high");
				}
			}
			if (this.NewOption[2] != this.SaveOption[2])
			{
				this.bNeedToLoading = true;
				if (this.NewOption[2] == 0)
				{
					IniFile.IniWriteValue("setting", "imageQuality", "low");
					QualitySettings.masterTextureLimit = 2;
				}
				else if (this.NewOption[2] == 1)
				{
					IniFile.IniWriteValue("setting", "imageQuality", "medium");
					QualitySettings.masterTextureLimit = 1;
				}
				else
				{
					IniFile.IniWriteValue("setting", "imageQuality", "high");
					QualitySettings.masterTextureLimit = 0;
				}
			}
			if (this.NewOption[3] != this.SaveOption[3])
			{
				this.bNeedToLoading = true;
				if (this.NewOption[3] == 0)
				{
					IniFile.IniWriteValue("setting", "resourcesCount", "low");
					QualitySettings.particleRaycastBudget = 64;
					QualitySettings.lodBias = 0.5f;
					QualitySettings.blendWeights = 1;
				}
				else if (this.NewOption[3] == 1)
				{
					IniFile.IniWriteValue("setting", "resourcesCount", "medium");
					QualitySettings.particleRaycastBudget = 512;
					QualitySettings.lodBias = 1f;
					QualitySettings.blendWeights = 2;
				}
				else
				{
					IniFile.IniWriteValue("setting", "resourcesCount", "high");
					QualitySettings.particleRaycastBudget = 2048;
					QualitySettings.lodBias = 2f;
					QualitySettings.blendWeights = 4;
				}
			}
			if (this.NewOption[7] != this.SaveOption[7])
			{
				this.bNeedToLoading = true;
				if (this.NewOption[7] == 0)
				{
					IniFile.IniWriteValue("setting", "tonemapping", "true");
					GameGlobal.m_bToneMapping = true;
				}
				else
				{
					IniFile.IniWriteValue("setting", "tonemapping", "false");
					GameGlobal.m_bToneMapping = false;
				}
			}
			if (this.NewOption[8] != this.SaveOption[8])
			{
				this.bNeedToLoading = true;
				if (this.NewOption[8] == 0)
				{
					IniFile.IniWriteValue("setting", "vSync", "true");
					QualitySettings.vSyncCount = 1;
				}
				else
				{
					IniFile.IniWriteValue("setting", "vSync", "false");
					QualitySettings.vSyncCount = 0;
				}
			}
			if (this.NewOption[9] != this.SaveOption[9])
			{
				this.bNeedToLoading = true;
				if (this.NewOption[9] == 0)
				{
					IniFile.IniWriteValue("setting", "antiAliasing", "true");
					QualitySettings.antiAliasing = 2;
				}
				else
				{
					IniFile.IniWriteValue("setting", "antiAliasing", "false");
					QualitySettings.antiAliasing = 0;
				}
			}
			if (this.NewOption[10] != this.SaveOption[10])
			{
				this.bNeedToLoading = true;
				if (this.NewOption[10] == 0)
				{
					IniFile.IniWriteValue("setting", "screenSpaceAmbientOcclusion", "true");
					QualitySettings.antiAliasing = 2;
				}
				else
				{
					IniFile.IniWriteValue("setting", "screenSpaceAmbientOcclusion", "false");
					QualitySettings.antiAliasing = 0;
				}
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00099EA0 File Offset: 0x000980A0
		private void OnOffOnClick(GameObject go)
		{
			UISprite component = go.GetComponent<UISprite>();
			if (component.spriteName == "UI_install_02")
			{
				component.spriteName = "UI_install_03";
			}
			else
			{
				component.spriteName = "UI_install_02";
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00099EE4 File Offset: 0x000980E4
		private void GroupOnClick(GameObject go)
		{
			BtnData component = go.GetComponent<BtnData>();
			foreach (GameObject gameObject in this.m_GroupList)
			{
				BtnData component2 = gameObject.GetComponent<BtnData>();
				if (component2.m_iBtnType == component.m_iBtnType)
				{
					if (component2.m_iBtnID == component.m_iBtnID)
					{
						UISprite component3 = gameObject.GetComponent<UISprite>();
						component3.spriteName = "UI_install_02";
					}
					else
					{
						UISprite component4 = gameObject.GetComponent<UISprite>();
						component4.spriteName = "UI_install_03";
					}
				}
			}
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00099F9C File Offset: 0x0009819C
		private void SetGameOption(int GroupID, int iIndex)
		{
			this.SaveOption[GroupID] = iIndex;
			foreach (GameObject gameObject in this.m_GroupList)
			{
				BtnData component = gameObject.GetComponent<BtnData>();
				if (component.m_iBtnType == GroupID && component.m_iBtnID == iIndex)
				{
					this.GroupOnClick(gameObject);
					break;
				}
			}
			foreach (GameObject gameObject2 in this.m_OnOffList)
			{
				BtnData component2 = gameObject2.GetComponent<BtnData>();
				if (component2.m_iBtnType == GroupID)
				{
					UISprite component3 = gameObject2.GetComponent<UISprite>();
					if (iIndex == 0)
					{
						component3.spriteName = "UI_install_02";
					}
					else
					{
						component3.spriteName = "UI_install_03";
					}
					break;
				}
			}
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0009A0AC File Offset: 0x000982AC
		private void UpdateGameSetting()
		{
			string text = IniFile.IniReadValue("setting", "shadow");
			if (text.ToLower() == "true")
			{
				this.SetGameOption(4, 0);
			}
			else
			{
				this.SetGameOption(4, 1);
			}
			string text2 = IniFile.IniReadValue("setting", "shadowQuality");
			if (text2.ToLower() == "high")
			{
				this.SetGameOption(5, 2);
			}
			else if (text2.ToLower() == "medium")
			{
				this.SetGameOption(5, 1);
			}
			else
			{
				this.SetGameOption(5, 0);
			}
			string text3 = IniFile.IniReadValue("setting", "shadowDistance");
			if (text3.ToLower() == "high")
			{
				this.SetGameOption(6, 2);
			}
			else if (text3.ToLower() == "medium")
			{
				this.SetGameOption(6, 1);
			}
			else
			{
				this.SetGameOption(6, 0);
			}
			string text4 = IniFile.IniReadValue("setting", "vSync");
			if (text4.ToLower() == "true")
			{
				this.SetGameOption(8, 0);
			}
			else
			{
				this.SetGameOption(8, 1);
			}
			string text5 = IniFile.IniReadValue("setting", "antiAliasing");
			if (text5.ToLower() == "true")
			{
				this.SetGameOption(9, 0);
			}
			else
			{
				this.SetGameOption(9, 1);
			}
			string text6 = IniFile.IniReadValue("setting", "imageQuality");
			if (text6.ToLower() == "high")
			{
				this.SetGameOption(2, 2);
			}
			else if (text6.ToLower() == "medium")
			{
				this.SetGameOption(2, 1);
			}
			else if (text6 == string.Empty)
			{
				this.SetGameOption(2, 1);
			}
			else
			{
				this.SetGameOption(2, 0);
			}
			string text7 = IniFile.IniReadValue("setting", "screenSpaceAmbientOcclusion");
			if (text7.ToLower() == "true")
			{
				this.SetGameOption(10, 0);
			}
			else
			{
				this.SetGameOption(10, 1);
			}
			string text8 = IniFile.IniReadValue("setting", "tonemapping");
			if (text8.ToLower() == "true")
			{
				this.SetGameOption(7, 0);
			}
			else
			{
				this.SetGameOption(7, 1);
			}
			string text9 = IniFile.IniReadValue("setting", "resourcesCount");
			if (text9.ToLower() == "high")
			{
				this.SetGameOption(3, 2);
			}
			else if (text9.ToLower() == "medium")
			{
				this.SetGameOption(3, 1);
			}
			else if (text9 == string.Empty)
			{
				this.SetGameOption(3, 1);
			}
			else
			{
				this.SetGameOption(3, 0);
			}
			string text10 = IniFile.IniReadValue("setting", "MusicValue");
			float value;
			if (!float.TryParse(text10, ref value))
			{
				value = 0.5f;
			}
			this.m_sMusic.value = value;
			string text11 = IniFile.IniReadValue("setting", "SoundValue");
			if (!float.TryParse(text11, ref value))
			{
				value = 0.5f;
			}
			this.m_sSound.value = value;
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0000B9D2 File Offset: 0x00009BD2
		public void MusicOnChange()
		{
			GameGlobal.m_fMusicValue = this.m_sMusic.value;
			Game.SetBGMusicValue();
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0000B9E9 File Offset: 0x00009BE9
		public void SoundOnChange()
		{
			GameGlobal.m_fSoundValue = this.m_sSound.value;
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0009A400 File Offset: 0x00098600
		private void ReadSettingIni()
		{
			string text = IniFile.IniReadValue("setting", "shadow");
			if (!(text.ToLower() == "true"))
			{
				if (text == string.Empty)
				{
					QualitySelector.ApplyQuality(2);
					QualitySettings.shadowDistance = 45f;
				}
				else
				{
					QualitySelector.ApplyQuality(0);
				}
			}
			string text2 = IniFile.IniReadValue("setting", "vSync");
			if (text2.ToLower() == "true")
			{
				QualitySettings.vSyncCount = 1;
			}
			else
			{
				QualitySettings.vSyncCount = 0;
			}
			string text3 = IniFile.IniReadValue("setting", "antiAliasing");
			if (text3.ToLower() == "true")
			{
				QualitySettings.antiAliasing = 2;
			}
			else
			{
				QualitySettings.antiAliasing = 0;
			}
			string text4 = IniFile.IniReadValue("setting", "imageQuality");
			if (text4.ToLower() == "high")
			{
				QualitySettings.masterTextureLimit = 0;
			}
			else if (text4.ToLower() == "medium")
			{
				QualitySettings.masterTextureLimit = 1;
			}
			else if (text4 == string.Empty)
			{
				QualitySettings.masterTextureLimit = 0;
			}
			else
			{
				QualitySettings.masterTextureLimit = 2;
			}
			string text5 = IniFile.IniReadValue("setting", "screenSpaceAmbientOcclusion");
			if (text5.ToLower() == "true")
			{
				GameGlobal.m_bSSAO = true;
			}
			else
			{
				GameGlobal.m_bSSAO = false;
			}
			string text6 = IniFile.IniReadValue("setting", "tonemapping");
			if (text6.ToLower() == "true")
			{
				GameGlobal.m_bToneMapping = true;
			}
			else
			{
				GameGlobal.m_bToneMapping = false;
			}
			string text7 = IniFile.IniReadValue("setting", "resourcesCount");
			if (text7.ToLower() == "high")
			{
				QualitySettings.particleRaycastBudget = 2048;
				QualitySettings.lodBias = 2f;
				QualitySettings.blendWeights = 4;
			}
			else if (text7.ToLower() == "medium")
			{
				QualitySettings.particleRaycastBudget = 512;
				QualitySettings.lodBias = 1f;
				QualitySettings.blendWeights = 2;
			}
			else if (text4 == string.Empty)
			{
				QualitySettings.particleRaycastBudget = 512;
				QualitySettings.lodBias = 1f;
				QualitySettings.blendWeights = 2;
			}
			else
			{
				QualitySettings.particleRaycastBudget = 64;
				QualitySettings.lodBias = 0.5f;
				QualitySettings.blendWeights = 1;
			}
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x0400157E RID: 5502
		private List<UISystem.SystemBtnNode> m_BtnSystemBaseList;

		// Token: 0x0400157F RID: 5503
		private List<Control> m_SelectLogoList;

		// Token: 0x04001580 RID: 5504
		private TweenAlpha Ink_Tween_Alpha;

		// Token: 0x04001581 RID: 5505
		private Control m_TestBtn;

		// Token: 0x04001582 RID: 5506
		private Control m_Group;

		// Token: 0x04001583 RID: 5507
		private Control m_InkBase;

		// Token: 0x04001584 RID: 5508
		private Control m_GameSetting;

		// Token: 0x04001585 RID: 5509
		private bool m_bOpenPlaying;

		// Token: 0x04001586 RID: 5510
		private bool m_bOnClick;

		// Token: 0x04001587 RID: 5511
		private Control m_Close;

		// Token: 0x04001588 RID: 5512
		private List<GameObject> m_OnOffList = new List<GameObject>();

		// Token: 0x04001589 RID: 5513
		private List<GameObject> m_GroupList = new List<GameObject>();

		// Token: 0x0400158A RID: 5514
		private UISlider m_sMusic;

		// Token: 0x0400158B RID: 5515
		private UISlider m_sSound;

		// Token: 0x0400158C RID: 5516
		private int[] SaveOption = new int[20];

		// Token: 0x0400158D RID: 5517
		private int[] NewOption = new int[20];

		// Token: 0x0400158E RID: 5518
		private bool bNeedToLoading;

		// Token: 0x02000329 RID: 809
		public class SystemBtnNode
		{
			// Token: 0x04001590 RID: 5520
			public Control m_BtnSystemBase;

			// Token: 0x04001591 RID: 5521
			public TweenPosition BtnSystemBase_Tween_Position;
		}
	}
}
