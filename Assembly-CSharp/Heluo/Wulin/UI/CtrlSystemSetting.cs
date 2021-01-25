using System;
using SSAA;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002FA RID: 762
	public class CtrlSystemSetting
	{
		// Token: 0x06001023 RID: 4131 RVA: 0x00089248 File Offset: 0x00087448
		public void GameSettingOnClose()
		{
			if (Game.IsLoading())
			{
				return;
			}
			if (Application.loadedLevelName == "GameMain")
			{
				this.bNeedToLoading = false;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				GameGlobal.m_TransferPos = gameObject.transform.localPosition;
				GameGlobal.m_fDir = gameObject.transform.localEulerAngles.y;
			}
			if (this.bNeedToLoading)
			{
				if (Camera.main != null && Camera.main.GetComponent<OrbitCam>() != null)
				{
					MapData.m_instance._CameraSaveDateNode.m_fDistance = Camera.main.GetComponent<OrbitCam>().Distance;
					MapData.m_instance._CameraSaveDateNode.m_fRotation = Camera.main.GetComponent<OrbitCam>().Rotation;
					MapData.m_instance._CameraSaveDateNode.m_fTilt = Camera.main.GetComponent<OrbitCam>().Tilt;
					GameGlobal.m_bSetCaramaDRT = true;
				}
				Game.UI.Get<UILoad>().LoadStage(Application.loadedLevelName);
			}
			this.bNeedToLoading = false;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0000AA5F File Offset: 0x00008C5F
		public SettingData UpdateSystemData()
		{
			this.GetSystemData();
			this.setView.Invoke(this.preset);
			return this.preset.Clone();
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0000AA83 File Offset: 0x00008C83
		public void GetSystemData()
		{
			this.GetResolution();
			this.GetEffect();
			this.GetVolume();
			this.GetKeyCode();
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00089368 File Offset: 0x00087568
		public void SetSystem()
		{
			bool flag = false;
			GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
			if (this.preset.iScreenWidth != Screen.width || this.preset.bFullScreen != Screen.fullScreen)
			{
				Screen.SetResolution(this.preset.iScreenWidth, this.preset.iScreenHeight, this.preset.bFullScreen);
				Debug.Log(string.Format("SetResolution {0}x{1} {2}", this.preset.iScreenWidth.ToString(), this.preset.iScreenHeight.ToString(), this.preset.bFullScreen.ToString()));
			}
			if (this.preset.settingDataNode.m_iQualityLevel != QualitySettings.GetQualityLevel())
			{
				flag = true;
				this.bNeedToLoading = true;
				QualitySettings.SetQualityLevel(this.preset.settingDataNode.m_iQualityLevel);
			}
			if (this.preset.bOwnSetting)
			{
				if (this.bShadow)
				{
					flag = true;
					this.bNeedToLoading = true;
					if (this.preset.settingDataNode.m_bShadow)
					{
						QualitySettings.shadowDistance = this.preset.settingDataNode.m_fShadowDistance;
					}
				}
				else if (this.preset.settingDataNode.m_fShadowDistance != QualitySettings.shadowDistance && this.preset.settingDataNode.m_bShadow)
				{
					flag = true;
					this.bNeedToLoading = true;
					QualitySettings.shadowDistance = this.preset.settingDataNode.m_fShadowDistance;
				}
				if (this.preset.settingDataNode.m_iVSync != QualitySettings.vSyncCount || flag)
				{
					this.bNeedToLoading = true;
					QualitySettings.vSyncCount = this.preset.settingDataNode.m_iVSync;
				}
				if (this.preset.settingDataNode.m_iAntiAliasing != QualitySettings.antiAliasing || flag)
				{
					this.bNeedToLoading = true;
					QualitySettings.antiAliasing = this.preset.settingDataNode.m_iAntiAliasing;
				}
				if (this.preset.settingDataNode.m_iImageQuality != QualitySettings.masterTextureLimit || flag)
				{
					this.bNeedToLoading = true;
					QualitySettings.masterTextureLimit = this.preset.settingDataNode.m_iImageQuality;
				}
			}
			if (this.preset.settingDataNode.m_bSSAO != GameGlobal.m_bSSAO)
			{
				GameGlobal.m_bSSAO = this.preset.settingDataNode.m_bSSAO;
				if (gameObject != null && gameObject.GetComponent<SSAOEffectDepthCutoff>() != null)
				{
					if (!GameGlobal.m_bSSAO)
					{
						gameObject.GetComponent<SSAOEffectDepthCutoff>().enabled = false;
					}
					else
					{
						gameObject.GetComponent<SSAOEffectDepthCutoff>().enabled = true;
					}
				}
			}
			if (this.preset.settingDataNode.m_bLensEffects != GameGlobal.m_bLensEffects)
			{
				GameGlobal.m_bLensEffects = this.preset.settingDataNode.m_bLensEffects;
				if (gameObject != null)
				{
					if (gameObject.GetComponent<ColorCorrectionCurves>() != null)
					{
						if (!GameGlobal.m_bLensEffects)
						{
							gameObject.GetComponent<ColorCorrectionCurves>().enabled = false;
						}
						else
						{
							gameObject.GetComponent<ColorCorrectionCurves>().enabled = true;
						}
					}
					if (gameObject.GetComponent<DeluxeTonemapper>() != null)
					{
						if (!GameGlobal.m_bLensEffects)
						{
							gameObject.GetComponent<DeluxeTonemapper>().enabled = false;
						}
						else
						{
							gameObject.GetComponent<DeluxeTonemapper>().enabled = true;
						}
					}
				}
			}
			if (this.preset.settingDataNode.m_bBloom != GameGlobal.m_bBloom)
			{
				GameGlobal.m_bBloom = this.preset.settingDataNode.m_bBloom;
				if (gameObject != null && gameObject.GetComponent<Bloom>() != null)
				{
					if (!GameGlobal.m_bBloom)
					{
						gameObject.GetComponent<Bloom>().enabled = false;
					}
					else
					{
						gameObject.GetComponent<Bloom>().enabled = true;
					}
				}
			}
			if (Terrain.activeTerrain != null)
			{
				float[] array = this.GetActiveTerrain();
				Terrain.activeTerrain.heightmapPixelError = array[0];
				Terrain.activeTerrain.detailObjectDistance = array[1];
				Terrain.activeTerrain.detailObjectDensity = array[2];
			}
			if (this.preset.bSSAA != GameGlobal.m_bSSAA)
			{
				GameGlobal.m_bSSAA = this.preset.bSSAA;
				if (gameObject != null && gameObject.GetComponent<SuperSampling_SSAA>() != null)
				{
					if (!GameGlobal.m_bSSAA)
					{
						gameObject.GetComponent<SuperSampling_SSAA>().enabled = false;
					}
					else
					{
						gameObject.GetComponent<SuperSampling_SSAA>().enabled = true;
					}
				}
			}
			if (GameGlobal.m_bSSAA)
			{
				float num = Mathf.Clamp(this.preset.fSSAA, 1.1f, 2f);
				num = (float)Math.Round((double)num, 1);
				GameGlobal.m_fSSAA = num;
				internal_SSAA.ChangeScale(num);
			}
			GameGlobal.m_fMusicValue = this.preset.fMusicValue;
			GameGlobal.m_fSoundValue = this.preset.fSoundValue;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00089830 File Offset: 0x00087A30
		public void GetResolution()
		{
			string text = IniFile.IniReadValue("setting", "resolution");
			int num = text.IndexOf("x");
			int num2 = 1024;
			int num3 = 576;
			int num4 = Screen.resolutions.Length - 1;
			if (num >= 0)
			{
				string text2 = text.Substring(0, num);
				string text3 = text.Substring(num + 1, text.Length - num - 1);
				if (!int.TryParse(text2, ref num2))
				{
					num2 = Screen.resolutions[num4].width;
					num3 = Screen.resolutions[num4].height;
				}
				if (!int.TryParse(text3, ref num3))
				{
					num2 = Screen.resolutions[num4].width;
					num3 = Screen.resolutions[num4].height;
				}
			}
			else
			{
				num2 = Screen.resolutions[num4].width;
				num3 = Screen.resolutions[num4].height;
			}
			if (num2 > 1920 || num3 > 1080)
			{
				for (int i = 0; i < Screen.resolutions.Length; i++)
				{
					if (Screen.resolutions[i].width > 1920)
					{
						break;
					}
					num2 = Screen.resolutions[i].width;
					num3 = Screen.resolutions[i].height;
				}
			}
			if (num2 / 16 != 0 || num3 / 9 != 0)
			{
				num3 = (int)((float)num2 * 0.5625f);
				Debug.Log(string.Concat(new object[]
				{
					"CCCCCCCCCCCCCC : ",
					num2,
					"x",
					num3
				}));
			}
			this.preset.iScreenWidth = num2;
			this.preset.iScreenHeight = num3;
			string text4 = IniFile.IniReadValue("setting", "fullscreen");
			bool bFullScreen = !(text4.ToLower() == "false");
			this.preset.bFullScreen = bFullScreen;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00089A48 File Offset: 0x00087C48
		public void GetEffect()
		{
			string text = IniFile.IniReadValue("setting", "qualityLevel");
			if (text.ToLower() == "fastest")
			{
				this.preset.settingDataNode.m_iQualityLevel = 0;
			}
			else if (text.ToLower() == "simple")
			{
				this.preset.settingDataNode.m_iQualityLevel = 2;
			}
			else if (text.ToLower() == "beautiful")
			{
				this.preset.settingDataNode.m_iQualityLevel = 4;
			}
			else if (text.ToLower() == "good")
			{
				this.preset.settingDataNode.m_iQualityLevel = 3;
			}
			else
			{
				this.preset.settingDataNode.m_iQualityLevel = 5;
			}
			string text2 = IniFile.IniReadValue("setting", "ownSetting");
			if (text2.ToLower() == "true" || Game.instance == null)
			{
				this.preset.bOwnSetting = true;
				string text3 = IniFile.IniReadValue("setting", "shadow");
				if (text3.ToLower() == "false")
				{
					this.preset.settingDataNode.m_bShadow = false;
					this.preset.settingDataNode.m_iQualityLevel = 0;
				}
				else
				{
					this.preset.settingDataNode.m_bShadow = true;
				}
				string text4 = IniFile.IniReadValue("setting", "shadowDistance");
				int num = 210;
				if (!int.TryParse(text4, ref num))
				{
					num = 100;
				}
				this.preset.settingDataNode.m_fShadowDistance = (float)num;
				string text5 = IniFile.IniReadValue("setting", "vSync");
				if (text5.ToLower() == "true")
				{
					this.preset.settingDataNode.m_iVSync = 1;
				}
				else
				{
					this.preset.settingDataNode.m_iVSync = 0;
				}
				string text6 = IniFile.IniReadValue("setting", "antiAliasing");
				if (text6.ToLower() == "false")
				{
					this.preset.settingDataNode.m_iAntiAliasing = 0;
				}
				else
				{
					this.preset.settingDataNode.m_iAntiAliasing = 2;
				}
				string text7 = IniFile.IniReadValue("setting", "imageQuality");
				int iImageQuality = 2;
				if (!int.TryParse(text7, ref iImageQuality))
				{
					iImageQuality = 0;
				}
				this.preset.settingDataNode.m_iImageQuality = iImageQuality;
				string text8 = IniFile.IniReadValue("setting", "screenSpaceAmbientOcclusion");
				if (text8.ToLower() == "false")
				{
					this.preset.settingDataNode.m_bSSAO = false;
				}
				else
				{
					this.preset.settingDataNode.m_bSSAO = true;
				}
				string text9 = IniFile.IniReadValue("setting", "lensEffects");
				if (text9.ToLower() == "false")
				{
					this.preset.settingDataNode.m_bLensEffects = false;
				}
				else
				{
					this.preset.settingDataNode.m_bLensEffects = true;
				}
				string text10 = IniFile.IniReadValue("setting", "bloom");
				if (text10.ToLower() == "false")
				{
					this.preset.settingDataNode.m_bBloom = false;
				}
				else
				{
					this.preset.settingDataNode.m_bBloom = true;
				}
				string text11 = IniFile.IniReadValue("setting", "resourcesCount");
				if (text11.ToLower() == "none")
				{
					this.preset.settingDataNode.m_iResourcesCount = 0;
				}
				else if (text11.ToLower() == "low")
				{
					this.preset.settingDataNode.m_iResourcesCount = 2;
				}
				else if (text11 == "medium")
				{
					this.preset.settingDataNode.m_iResourcesCount = 1;
				}
				else
				{
					this.preset.settingDataNode.m_iResourcesCount = 3;
				}
			}
			else
			{
				this.preset.bOwnSetting = false;
				SettingDataNode settingDataNode = Game.SettingData.GetSettingDataNode(this.preset.settingDataNode.m_iQualityLevel);
				this.preset.settingDataNode = settingDataNode;
			}
			string text12 = IniFile.IniReadValue("setting", "sSAA");
			if (text12.ToLower() == "true")
			{
				this.preset.bSSAA = true;
			}
			else
			{
				this.preset.bSSAA = false;
			}
			string text13 = IniFile.IniReadValue("setting", "sSAA_Multiplier");
			float fSSAA;
			if (!float.TryParse(text13, ref fSSAA))
			{
				fSSAA = 1.1f;
			}
			this.preset.fSSAA = fSSAA;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00089F0C File Offset: 0x0008810C
		private void GetVolume()
		{
			string text = IniFile.IniReadValue("setting", "musicValue");
			float fMusicValue;
			if (!float.TryParse(text, ref fMusicValue))
			{
				fMusicValue = 0.5f;
			}
			this.preset.fMusicValue = fMusicValue;
			string text2 = IniFile.IniReadValue("setting", "soundValue");
			float fSoundValue;
			if (!float.TryParse(text2, ref fSoundValue))
			{
				fSoundValue = 0.5f;
			}
			this.preset.fSoundValue = fSoundValue;
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00089F78 File Offset: 0x00088178
		private void GetKeyCode()
		{
			KeyCode[] keyCodeArray = Game.g_InputManager.GetKeyCodeArray<KeyboardControl>();
			for (int i = 0; i < keyCodeArray.Length; i++)
			{
				KeyCode keyCode;
				if (keyCodeArray[i] == null)
				{
					if (i == 25)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.RotateLeft, 113);
						keyCode = 113;
					}
					else if (i == 26)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.RotateRight, 101);
						keyCode = 101;
					}
					else if (i == 28)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.BattleNextUnit, 9);
						keyCode = 9;
					}
					else if (i == 32)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Character, 282);
						keyCode = 282;
					}
					else if (i == 33)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Team, 283);
						keyCode = 283;
					}
					else if (i == 34)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Backpack, 284);
						keyCode = 284;
					}
					else if (i == 35)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Mission, 285);
						keyCode = 285;
					}
					else if (i == 36)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Rumor, 286);
						keyCode = 286;
					}
					else if (i == 37)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Save, 287);
						keyCode = 287;
					}
					else if (i == 38)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Load, 288);
						keyCode = 288;
					}
					else if (i == 39)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Skill1, 49);
						keyCode = 49;
					}
					else if (i == 40)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Skill2, 50);
						keyCode = 50;
					}
					else if (i == 41)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Skill3, 51);
						keyCode = 51;
					}
					else if (i == 42)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Skill4, 52);
						keyCode = 52;
					}
					else if (i == 43)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Skill5, 53);
						keyCode = 53;
					}
					else if (i == 44)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.Skill6, 54);
						keyCode = 54;
					}
					else if (i == 45)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.SelectItem, 55);
						keyCode = 57;
					}
					else if (i == 46)
					{
						Game.g_InputManager.SetKeyCode<KeyboardControl>(KeyControl.Key.RestTurn, 56);
						keyCode = 48;
					}
					else
					{
						keyCode = keyCodeArray[i];
					}
				}
				else
				{
					keyCode = keyCodeArray[i];
				}
				this.preset.keyboard[i] = keyCode;
			}
			KeyCode[] keyCodeArray2 = Game.g_InputManager.GetKeyCodeArray<JoystickControl>();
			for (int j = 0; j < keyCodeArray2.Length; j++)
			{
				KeyCode keyCode2;
				if (keyCodeArray[j] == null)
				{
					if (j == 25)
					{
						Game.g_InputManager.SetKeyCode<JoystickControl>(KeyControl.Key.RotateLeft, 358);
						keyCode2 = 358;
					}
					else if (j == 26)
					{
						Game.g_InputManager.SetKeyCode<JoystickControl>(KeyControl.Key.RotateLeft, 359);
						keyCode2 = 359;
					}
					else if (j == 28)
					{
						Game.g_InputManager.SetKeyCode<JoystickControl>(KeyControl.Key.RotateLeft, 355);
						keyCode2 = 355;
					}
					else
					{
						keyCode2 = keyCodeArray2[j];
					}
				}
				else
				{
					keyCode2 = keyCodeArray2[j];
				}
				this.preset.joystick[j] = keyCode2;
			}
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0008A2CC File Offset: 0x000884CC
		public void IniSettingData(SettingData interim)
		{
			IniFile.IniWriteValue("setting", "resolution", interim.iScreenWidth.ToString() + "x" + interim.iScreenHeight.ToString());
			IniFile.IniWriteValue("setting", "fullscreen", interim.bFullScreen.ToString().ToLower());
			IniFile.IniWriteValue("setting", "ownSetting", interim.bOwnSetting.ToString().ToLower());
			if (interim.settingDataNode.m_iQualityLevel == 0)
			{
				IniFile.IniWriteValue("setting", "qualityLevel", "fastest");
			}
			else if (interim.settingDataNode.m_iQualityLevel == 2)
			{
				IniFile.IniWriteValue("setting", "qualityLevel", "simple");
			}
			else if (interim.settingDataNode.m_iQualityLevel == 3)
			{
				IniFile.IniWriteValue("setting", "qualityLevel", "good");
			}
			else if (interim.settingDataNode.m_iQualityLevel == 4)
			{
				IniFile.IniWriteValue("setting", "qualityLevel", "beautiful");
			}
			else
			{
				IniFile.IniWriteValue("setting", "qualityLevel", "fantastic");
			}
			if (this.preset.settingDataNode.m_bShadow != interim.settingDataNode.m_bShadow)
			{
				this.bShadow = true;
			}
			else
			{
				this.bShadow = false;
			}
			IniFile.IniWriteValue("setting", "shadow", interim.settingDataNode.m_bShadow.ToString().ToLower());
			IniFile.IniWriteValue("setting", "shadowDistance", interim.settingDataNode.m_fShadowDistance.ToString());
			if (interim.settingDataNode.m_iVSync == 1)
			{
				IniFile.IniWriteValue("setting", "vSync", "true");
			}
			else
			{
				IniFile.IniWriteValue("setting", "vSync", "false");
			}
			if (interim.settingDataNode.m_iAntiAliasing == 2)
			{
				IniFile.IniWriteValue("setting", "antiAliasing", "true");
			}
			else
			{
				IniFile.IniWriteValue("setting", "antiAliasing", "false");
			}
			IniFile.IniWriteValue("setting", "imageQuality", interim.settingDataNode.m_iImageQuality.ToString());
			IniFile.IniWriteValue("setting", "screenSpaceAmbientOcclusion", interim.settingDataNode.m_bSSAO.ToString().ToLower());
			IniFile.IniWriteValue("setting", "lensEffects", interim.settingDataNode.m_bLensEffects.ToString().ToLower());
			IniFile.IniWriteValue("setting", "bloom", interim.settingDataNode.m_bBloom.ToString().ToLower());
			if (interim.settingDataNode.m_iResourcesCount == 3)
			{
				IniFile.IniWriteValue("setting", "resourcesCount", "high");
			}
			else if (interim.settingDataNode.m_iResourcesCount == 2)
			{
				IniFile.IniWriteValue("setting", "resourcesCount", "medium");
			}
			else if (interim.settingDataNode.m_iResourcesCount == 1)
			{
				IniFile.IniWriteValue("setting", "resourcesCount", "low");
			}
			else
			{
				IniFile.IniWriteValue("setting", "resourcesCount", "none");
			}
			IniFile.IniWriteValue("setting", "sSAA", interim.bSSAA.ToString().ToLower());
			string value = interim.fSSAA.ToString("0.00");
			IniFile.IniWriteValue("setting", "sSAA_Multiplier", value);
			for (int i = 0; i < interim.keyboard.Length; i++)
			{
				Game.g_InputManager.SetKeyCode<KeyboardControl>((KeyControl.Key)i, interim.keyboard[i]);
			}
			Game.g_InputManager.SaveData<KeyboardControl>("hotkey.ini");
			for (int j = 0; j < interim.joystick.Length; j++)
			{
				Game.g_InputManager.SetKeyCode<JoystickControl>((KeyControl.Key)j, interim.joystick[j]);
			}
			Game.g_InputManager.SaveData<JoystickControl>("hotkey_joystick.ini");
			this.GetSystemData();
			this.SetSystem();
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0008A6D4 File Offset: 0x000888D4
		public float[] GetActiveTerrain()
		{
			switch (this.preset.settingDataNode.m_iResourcesCount)
			{
			case 1:
				this.activeTerrain[0] = 50f;
				this.activeTerrain[1] = 50f;
				this.activeTerrain[2] = 0.5f;
				break;
			case 2:
				this.activeTerrain[0] = 25f;
				this.activeTerrain[1] = 70f;
				this.activeTerrain[2] = 0.7f;
				break;
			case 3:
				this.activeTerrain[0] = 10f;
				this.activeTerrain[1] = 100f;
				this.activeTerrain[2] = 1f;
				break;
			}
			return this.activeTerrain;
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0000AA9D File Offset: 0x00008C9D
		public float GetShadowDistance()
		{
			return this.preset.settingDataNode.m_fShadowDistance;
		}

		// Token: 0x04001333 RID: 4915
		private SettingData preset = new SettingData();

		// Token: 0x04001334 RID: 4916
		private bool bNeedToLoading;

		// Token: 0x04001335 RID: 4917
		private float[] activeTerrain = new float[3];

		// Token: 0x04001336 RID: 4918
		private bool bShadow;

		// Token: 0x04001337 RID: 4919
		public Action<SettingData> setView;
	}
}
